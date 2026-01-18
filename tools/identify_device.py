#!/usr/bin/env python3
"""
OneControl Device Identification Tool

Interactive tool to identify device IDs by monitoring traffic while the user
toggles devices using the LippertConnect app. Safe fallback when automatic
discovery via GetDevices/GetDevicesMetadata doesn't work.

Usage:
    python identify_device.py [--host 192.168.1.1] [--output devices.json]
    
Workflow:
    1. Start this script
    2. When prompted, toggle a device in the LippertConnect app
    3. Enter the device name (from the app)
    4. Repeat for each device
    5. Device mappings are saved to JSON file
"""

import argparse
import asyncio
import json
import os
import sys
from datetime import datetime
from pathlib import Path
from typing import Optional

# Add parent directory to path for imports
sys.path.insert(0, str(__file__).rsplit("/", 2)[0])

from rvc.device_types import FunctionName, is_light, get_function_name, LIGHT_FUNCTION_IDS

# Connection parameters
DEFAULT_HOST = "192.168.1.1"
DEFAULT_PORT = 6969


def crc8_maxim(data: bytes, init: int = 0x55) -> int:
    """CRC-8/MAXIM checksum."""
    table = [0] * 256
    for i in range(256):
        c = i
        for _ in range(8):
            c = (c >> 1) ^ 0x8C if c & 1 else c >> 1
        table[i] = c
    crc = init
    for b in data:
        crc = table[(crc ^ b) & 0xFF]
    return crc


def cobs_decode_single(encoded: bytes) -> bytes:
    """Decode a single COBS block."""
    out = bytearray()
    i = 0
    while i < len(encoded):
        code = encoded[i]
        i += 1
        count = code & 63
        zeros = code >> 6
        for _ in range(count):
            if i < len(encoded):
                out.append(encoded[i])
                i += 1
        for _ in range(zeros):
            out.append(0x00)
    return bytes(out)


def decode_frames(data: bytes) -> list[bytes]:
    """Decode multiple COBS frames from data stream."""
    frames = []
    buf = bytearray()
    for b in data:
        if b == 0:
            if buf:
                try:
                    decoded = cobs_decode_single(bytes(buf))
                    if len(decoded) >= 2:
                        p = decoded[:-1]
                        crc = decoded[-1]
                        if crc == crc8_maxim(p):
                            frames.append(p)
                except Exception:
                    pass
                buf = bytearray()
        else:
            buf.append(b)
    return frames


class DeviceIdentifier:
    """Interactive device identification through traffic monitoring."""
    
    def __init__(self, host: str, output_file: str):
        self.host = host
        self.output_file = output_file
        self.devices: dict[int, dict] = {}
        self.reader: Optional[asyncio.StreamReader] = None
        self.writer: Optional[asyncio.StreamWriter] = None
        
        # Load existing mappings
        self._load_existing()
    
    def _load_existing(self):
        """Load existing device mappings from file."""
        if os.path.exists(self.output_file):
            try:
                with open(self.output_file, "r") as f:
                    data = json.load(f)
                    self.devices = {int(k): v for k, v in data.items()}
                print(f"Loaded {len(self.devices)} existing device mappings")
            except Exception as e:
                print(f"Warning: Could not load existing mappings: {e}")
    
    def _save(self):
        """Save device mappings to file."""
        with open(self.output_file, "w") as f:
            json.dump(self.devices, f, indent=2)
        print(f"Saved {len(self.devices)} device mappings to {self.output_file}")
    
    async def connect(self):
        """Connect to controller."""
        self.reader, self.writer = await asyncio.open_connection(self.host, DEFAULT_PORT)
        print(f"Connected to {self.host}:{DEFAULT_PORT}")
    
    async def disconnect(self):
        """Disconnect from controller."""
        if self.writer:
            self.writer.close()
            await self.writer.wait_closed()
    
    async def _recv_frames(self, timeout: float = 0.5) -> list[bytes]:
        """Receive and decode frames."""
        if not self.reader:
            return []
        try:
            data = await asyncio.wait_for(self.reader.read(8192), timeout=timeout)
            return decode_frames(data)
        except asyncio.TimeoutError:
            return []
    
    def _extract_device_ids_from_frame(self, frame: bytes) -> list[int]:
        """
        Extract potential device IDs from a frame.
        
        Toggle commands use formats like:
        - 02 82 [conn] [cnt] 42 00 [device]  (seed request)
        - 06 82 [conn] [cnt] 43 00 [device] [key]  (key transmit)
        - 00 82 [conn] [cnt] [on/off]  (control - doesn't contain device!)
        - 08 07 [sess] [cnt] [device 2B] [data]  (legacy toggle)
        """
        device_ids = []
        
        if len(frame) < 5:
            return device_ids
        
        # Check for seed request: 02 82 [conn] [cnt] 42 00 [device]
        if (frame[0] == 0x02 and frame[1] == 0x82 and 
            len(frame) >= 7 and frame[4] == 0x42):
            device_ids.append(frame[6])
        
        # Check for key transmit: 06 82 [conn] [cnt] 43 00 [device] [key 4B]
        if (frame[0] == 0x06 and frame[1] == 0x82 and 
            len(frame) >= 11 and frame[4] == 0x43):
            device_ids.append(frame[6])
        
        # Check for legacy toggle: 08 07 [sess] [cnt] [device 2B] [data]
        if frame[0] == 0x08 and frame[1] == 0x07 and len(frame) >= 6:
            # 2-byte device ID (e.g., fe a8, ff 3c)
            device_2b = (frame[4] << 8) | frame[5]
            if device_2b > 0:
                device_ids.append(device_2b)
        
        return device_ids
    
    async def monitor_for_toggle(self, timeout_seconds: int = 30) -> Optional[int]:
        """
        Monitor traffic for toggle commands and return detected device ID.
        """
        print(f"\n  Monitoring for {timeout_seconds} seconds...")
        print("  Toggle the device in the LippertConnect app NOW!")
        
        detected_ids = []
        start_time = asyncio.get_event_loop().time()
        
        while asyncio.get_event_loop().time() - start_time < timeout_seconds:
            frames = await self._recv_frames()
            
            for frame in frames:
                device_ids = self._extract_device_ids_from_frame(frame)
                for dev_id in device_ids:
                    if dev_id not in detected_ids:
                        detected_ids.append(dev_id)
                        print(f"  >> Detected toggle for device: 0x{dev_id:02X}")
        
        if detected_ids:
            # Return the most recently detected ID
            return detected_ids[-1]
        return None
    
    async def identify_single(self) -> bool:
        """Identify a single device interactively."""
        print("\n" + "=" * 50)
        print("DEVICE IDENTIFICATION")
        print("=" * 50)
        
        # Ask user for device name first
        name = input("\nEnter device name (from LippertConnect app): ").strip()
        if not name:
            print("Cancelled.")
            return False
        
        # Suggest a category
        print("\nDevice category:")
        print("  1. Light (interior)")
        print("  2. Light (exterior)")
        print("  3. Other (slide, awning, etc.)")
        print("  4. Unknown")
        
        category_input = input("Select category [1-4, default=1]: ").strip()
        category_map = {
            "1": "light_interior",
            "2": "light_exterior", 
            "3": "other",
            "4": "unknown",
            "": "light_interior",
        }
        category = category_map.get(category_input, "unknown")
        
        # Clear any pending data
        await self._recv_frames()
        
        # Monitor for toggle
        device_id = await self.monitor_for_toggle()
        
        if device_id is None:
            print("\n  No toggle detected!")
            print("  Make sure you're toggling in the LippertConnect app.")
            
            # Ask user if they want to manually enter ID
            manual = input("\n  Enter device ID manually? (hex, e.g., 04): ").strip()
            if manual:
                try:
                    device_id = int(manual, 16)
                except ValueError:
                    print("  Invalid hex value.")
                    return False
            else:
                return False
        
        # Confirm with user
        print(f"\n  Detected device ID: 0x{device_id:02X}")
        confirm = input(f"  Save as '{name}'? [Y/n]: ").strip().lower()
        
        if confirm in ("", "y", "yes"):
            self.devices[device_id] = {
                "name": name,
                "category": category,
                "is_light": category.startswith("light"),
                "identified_at": datetime.now().isoformat(),
            }
            self._save()
            print(f"  Saved: 0x{device_id:02X} = {name}")
            return True
        else:
            print("  Not saved.")
            return False
    
    async def run_interactive(self):
        """Run interactive identification session."""
        print("\n" + "=" * 60)
        print("ONECONTROL DEVICE IDENTIFICATION")
        print("=" * 60)
        print("\nThis tool helps you identify device IDs by monitoring traffic")
        print("while you toggle devices in the LippertConnect app.")
        print("\nInstructions:")
        print("  1. Have the LippertConnect app open and connected")
        print("  2. When prompted, toggle a single device")
        print("  3. This tool will detect the device ID")
        print("  4. Enter the device name as shown in the app")
        print("  5. Repeat for each device you want to map")
        print("\nType 'quit' or 'q' to exit, 'list' to show mapped devices")
        
        if self.devices:
            print(f"\n{len(self.devices)} devices already mapped:")
            for dev_id, info in sorted(self.devices.items()):
                print(f"  0x{dev_id:02X} = {info['name']}")
        
        while True:
            print("\n" + "-" * 40)
            cmd = input("Press Enter to identify a device (or 'quit'/'list'): ").strip().lower()
            
            if cmd in ("quit", "q", "exit"):
                break
            elif cmd == "list":
                if self.devices:
                    print("\nMapped devices:")
                    for dev_id, info in sorted(self.devices.items()):
                        light_tag = " [LIGHT]" if info.get("is_light") else ""
                        print(f"  0x{dev_id:02X} = {info['name']}{light_tag}")
                else:
                    print("\nNo devices mapped yet.")
            elif cmd == "export":
                # Export as Python dict for easy copy/paste
                print("\n# Device mappings for rvc/devices.py:")
                print("DEVICES = {")
                for dev_id, info in sorted(self.devices.items()):
                    print(f'    0x{dev_id:02X}: "{info["name"]}",')
                print("}")
            else:
                await self.identify_single()
        
        print("\nDone! Device mappings saved to:", self.output_file)
        
        # Show export option
        if self.devices:
            print("\nTo use these devices in your code:")
            print("```python")
            print("from rvc.onecontrol import OneControlClient")
            print("")
            print("DEVICES = {")
            for dev_id, info in sorted(self.devices.items()):
                if info.get("is_light"):
                    print(f'    "{info["name"]}": 0x{dev_id:02X},')
            print("}")
            print("")
            print('async with OneControlClient("192.168.1.1") as client:')
            print('    await client.light_on(DEVICES["Kitchen"])')
            print("```")


async def main():
    parser = argparse.ArgumentParser(description="Identify OneControl devices")
    parser.add_argument("--host", default=DEFAULT_HOST, help="Controller IP")
    parser.add_argument("--output", default="devices.json", help="Output file")
    args = parser.parse_args()
    
    identifier = DeviceIdentifier(host=args.host, output_file=args.output)
    
    try:
        await identifier.connect()
        await identifier.run_interactive()
    except KeyboardInterrupt:
        print("\nInterrupted.")
    except Exception as e:
        print(f"Error: {e}")
        import traceback
        traceback.print_exc()
    finally:
        await identifier.disconnect()


if __name__ == "__main__":
    asyncio.run(main())
