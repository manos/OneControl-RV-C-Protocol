#!/usr/bin/env python3
"""
Auto Device Discovery - Discover and control OneControl devices automatically

NO PACKET CAPTURE NEEDED!

Discovery process:
1. Connect to controller
2. Listen for 08 02 broadcasts → get FunctionName + counter
3. Use universal values: proto=0x80, conn=0x40, session=0x80
4. Control with counter from broadcast!
"""

import asyncio
import sys
from dataclasses import dataclass

sys.path.insert(0, '.')
from rvc.onecontrol import (
    cobs_encode, decode_frames, control_light, LightConfig,
    DEFAULT_HOST, DEFAULT_PORT
)

# Universal values that work for ALL devices!
UNIVERSAL_PROTO = 0x80
UNIVERSAL_CONN = 0x40
UNIVERSAL_SESSION = 0x80

# Known function names from decompiled app (IDS.Core.Types.FUNCTION_NAME)
FUNCTION_NAMES = {
    # System devices
    2: "MyRV Tablet",
    # Water Heaters
    3: "Gas Water Heater",
    4: "Electric Water Heater",
    5: "Water Pump",
    # Generic Lights
    32: "Kitchen Ceiling Light",
    33: "Kitchen Sconce Light",
    # Living Room
    41: "Living Room Ceiling Light",
    # Outdoor Lights
    48: "Porch Light",
    49: "Awning Light",
    50: "Outdoor Light",
    # Bedroom
    57: "Bedroom Light",
    58: "Living Room Light",
    59: "Kitchen Light",
    63: "Bed Ceiling Light",
    # Tank Sensors
    67: "Fresh Tank",
    68: "Grey Tank",
    69: "Black Tank",
    71: "Generator Fuel Tank",
    # Water Heaters (alt IDs)
    76: "Electric Water Heater (Alt)",
    77: "Gas Water Heater (Alt)",
    # Leveler/Stabilizers
    88: "Landing Gear",  # LEVELER!
    89: "Front Stabilizer",
    90: "Rear Stabilizer",
    # Generator
    95: "Generator",
    96: "Vent Cover",
    97: "Main Slide",
    # Motors
    105: "Awning",
    107: "Under Cabinet Light",
    # Lights
    122: "Scare Light",
    # LP Tank
    176: "LP Tank",
    # Network
    184: "Ethernet Bridge",
}


@dataclass
class DiscoveredDevice:
    """A discovered device ready to control."""
    name: str
    func_id: int
    counter: int
    
    def to_light_config(self) -> LightConfig:
        """Convert to LightConfig for control."""
        return LightConfig(
            name=self.name,
            protocol=UNIVERSAL_PROTO,
            session=UNIVERSAL_SESSION,
            device=0x04,
            conn=UNIVERSAL_CONN,
            counter=self.counter,
        )


async def discover_devices(host: str = DEFAULT_HOST, port: int = DEFAULT_PORT, duration: float = 3.0) -> list[DiscoveredDevice]:
    """
    Discover all devices from controller broadcasts.
    Returns list of DiscoveredDevice ready to control.
    """
    print(f"Discovering devices from {host}:{port}...")
    
    reader, writer = await asyncio.open_connection(host, port)
    devices = {}
    
    try:
        # Register
        session = 0x80
        uuid = bytes([0x1c, 0x88, 0x43, 0x4f, 0xaf, 0x67, 0x82])
        writer.write(cobs_encode(bytes([0x01, 0x06, session, 0x00])))
        await writer.drain()
        await asyncio.sleep(0.1)
        writer.write(cobs_encode(bytes([0x08, 0x00, session, 0x00]) + uuid))
        await writer.drain()
        
        # Collect broadcasts
        start = asyncio.get_event_loop().time()
        while asyncio.get_event_loop().time() - start < duration:
            try:
                data = await asyncio.wait_for(reader.read(8192), timeout=0.5)
                frames = decode_frames(data)
                for f in frames:
                    # 08 02 frames: 08 02 [counter] 00 7d 28 [??] 00 [func_id] ...
                    if len(f) >= 9 and f[0] == 0x08 and f[1] == 0x02:
                        counter = f[2]
                        func_id = f[8]
                        if func_id > 0 and func_id not in [0x00, 0x7d, 0x28, 0x1e, 0x0a, 0x21, 0x27]:
                            if counter not in devices:
                                name = FUNCTION_NAMES.get(func_id, f"Unknown_{func_id}")
                                devices[counter] = DiscoveredDevice(
                                    name=name,
                                    func_id=func_id,
                                    counter=counter,
                                )
            except asyncio.TimeoutError:
                continue
    finally:
        writer.close()
        await writer.wait_closed()
    
    result = list(devices.values())
    print(f"Found {len(result)} devices")
    return result


async def main():
    """Discover and list all devices."""
    print("=" * 60)
    print("OneControl Auto Device Discovery")
    print("=" * 60)
    print()
    print("Universal control values:")
    print(f"  Protocol: 0x{UNIVERSAL_PROTO:02X}")
    print(f"  Session:  0x{UNIVERSAL_SESSION:02X}")
    print(f"  Conn:     0x{UNIVERSAL_CONN:02X}")
    print()
    
    devices = await discover_devices()
    
    if not devices:
        print("No devices found!")
        return
    
    print()
    print("Discovered devices:")
    print("-" * 60)
    for d in sorted(devices, key=lambda x: x.name):
        print(f"  {d.name:30} counter=0x{d.counter:02X} (func_id={d.func_id})")
    
    print()
    print("=" * 60)
    print("To control a device:")
    print("=" * 60)
    print("""
from rvc.onecontrol import control_light, LightConfig

# Use universal values + counter from discovery
device = LightConfig(
    name="Device Name",
    protocol=0x80,
    session=0x80,
    device=0x04,
    conn=0x40,
    counter=0xXX,  # From discovery
)

await control_light(device, on=True)
await control_light(device, on=False)
""")


if __name__ == "__main__":
    asyncio.run(main())
