#!/usr/bin/env python3
"""
OneControl Protocol Monitor

Connects to OneControl controller and monitors broadcast messages.
Shows tank levels, generator hours, and device status updates.

Usage:
    python examples/monitor.py [host] [port] [duration]
    
Example:
    python examples/monitor.py 192.168.1.1 6969 30
"""

import asyncio
import sys
from datetime import datetime

# Add parent directory for imports
sys.path.insert(0, '.')
from rvc.onecontrol import (
    cobs_encode, decode_frames, DEFAULT_HOST, DEFAULT_PORT
)

# Event types (from MyRvLinkEventType enum)
EVENT_TYPES = {
    0x00: "Unknown",
    0x01: "GatewayInformation",
    0x02: "DeviceCommand",
    0x03: "DeviceOnlineStatus",
    0x05: "RelayLatchingStatus",
    0x08: "DimmableLightStatus",
    0x0A: "GeneratorGenieStatus",
    0x0C: "TankSensorStatus",
    0x0D: "RelayHBridgeStatus",
    0x0F: "HourMeterStatus",
    0x20: "RealTimeClock",
}


def parse_frame(frame: bytes) -> str:
    """Parse a frame and return human-readable description."""
    if not frame or len(frame) < 2:
        return f"Short frame: {frame.hex()}"
    
    frame_type = frame[0]
    subtype = frame[1]
    
    # Tank levels: 01 03 [counter] [level]
    if frame_type == 0x01 and subtype == 0x03 and len(frame) >= 4:
        counter = frame[2]
        level = frame[3]
        tank_names = {0x04: "Grey", 0x3E: "Fresh", 0x86: "Black", 0x10: "LP"}
        tank = tank_names.get(counter, f"Tank_0x{counter:02X}")
        return f"Tank: {tank} = {level}%"
    
    # Generator hours: 05 03 [counter] [uint32 seconds] [status]
    if frame_type == 0x05 and subtype == 0x03 and len(frame) >= 8:
        counter = frame[2]
        if counter == 0x80:  # Hour meter
            seconds = int.from_bytes(frame[3:7], 'big')
            hours = seconds / 3600.0
            status = frame[7] if len(frame) > 7 else 0
            return f"Generator: {hours:.1f} hours (status={status})"
        elif counter == 0x87:  # Generator Genie
            state = frame[3]
            states = {0: "OFF", 1: "STARTING", 2: "RUNNING", 3: "STOPPING"}
            return f"Generator Genie: {states.get(state, state)}"
    
    # Device metadata: 08 02 [counter] ...
    if frame_type == 0x08 and subtype == 0x02 and len(frame) >= 9:
        counter = frame[2]
        func_id = frame[8] if len(frame) > 8 else 0
        return f"Device: counter=0x{counter:02X}, func_id={func_id}"
    
    # Seed broadcast: 06 82 ...
    if frame_type == 0x06 and subtype == 0x82 and len(frame) >= 11:
        seed = int.from_bytes(frame[7:11], 'big')
        return f"Seed broadcast: 0x{seed:08X}"
    
    # Generic
    return f"Frame {frame_type:02X} {subtype:02X}: {frame[2:].hex()}"


async def monitor(host: str = DEFAULT_HOST, port: int = DEFAULT_PORT, duration: float = 30.0):
    """Monitor OneControl broadcasts."""
    print(f"Connecting to {host}:{port}...")
    
    reader, writer = await asyncio.open_connection(host, port)
    
    # Register
    session = 0x80
    uuid = bytes([0x1c, 0x88, 0x43, 0x4f, 0xaf, 0x67, 0x82])
    writer.write(cobs_encode(bytes([0x01, 0x06, session, 0x00])))
    await writer.drain()
    await asyncio.sleep(0.1)
    writer.write(cobs_encode(bytes([0x08, 0x00, session, 0x00]) + uuid))
    await writer.drain()
    
    print(f"Connected! Monitoring for {duration} seconds...")
    print("-" * 60)
    
    seen = set()
    start = asyncio.get_event_loop().time()
    
    try:
        while asyncio.get_event_loop().time() - start < duration:
            try:
                data = await asyncio.wait_for(reader.read(8192), timeout=0.5)
                frames = decode_frames(data)
                
                for frame in frames:
                    # Skip duplicates
                    fhex = frame.hex()
                    if fhex in seen:
                        continue
                    seen.add(fhex)
                    
                    # Parse and display
                    timestamp = datetime.now().strftime("%H:%M:%S")
                    description = parse_frame(frame)
                    print(f"[{timestamp}] {description}")
                    
            except asyncio.TimeoutError:
                continue
                
    finally:
        writer.close()
        await writer.wait_closed()
    
    print("-" * 60)
    print(f"Monitoring complete. Saw {len(seen)} unique frames.")


if __name__ == "__main__":
    host = sys.argv[1] if len(sys.argv) > 1 else DEFAULT_HOST
    port = int(sys.argv[2]) if len(sys.argv) > 2 else DEFAULT_PORT
    duration = float(sys.argv[3]) if len(sys.argv) > 3 else 30.0
    
    asyncio.run(monitor(host, port, duration))
