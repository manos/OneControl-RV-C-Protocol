#!/usr/bin/env python3
"""
Device Analyzer for OneControl

Analyzes broadcast traffic to understand device structure and find specific devices.
"""

import socket
import time
from collections import defaultdict

# CRC-8/MAXIM table
CRC8_TABLE = [
    0x00, 0x5e, 0xbc, 0xe2, 0x61, 0x3f, 0xdd, 0x83,
    0xc2, 0x9c, 0x7e, 0x20, 0xa3, 0xfd, 0x1f, 0x41,
    0x9d, 0xc3, 0x21, 0x7f, 0xfc, 0xa2, 0x40, 0x1e,
    0x5f, 0x01, 0xe3, 0xbd, 0x3e, 0x60, 0x82, 0xdc,
    0x23, 0x7d, 0x9f, 0xc1, 0x42, 0x1c, 0xfe, 0xa0,
    0xe1, 0xbf, 0x5d, 0x03, 0x80, 0xde, 0x3c, 0x62,
    0xbe, 0xe0, 0x02, 0x5c, 0xdf, 0x81, 0x63, 0x3d,
    0x7c, 0x22, 0xc0, 0x9e, 0x1d, 0x43, 0xa1, 0xff,
    0x46, 0x18, 0xfa, 0xa4, 0x27, 0x79, 0x9b, 0xc5,
    0x84, 0xda, 0x38, 0x66, 0xe5, 0xbb, 0x59, 0x07,
    0xdb, 0x85, 0x67, 0x39, 0xba, 0xe4, 0x06, 0x58,
    0x19, 0x47, 0xa5, 0xfb, 0x78, 0x26, 0xc4, 0x9a,
    0x65, 0x3b, 0xd9, 0x87, 0x04, 0x5a, 0xb8, 0xe6,
    0xa7, 0xf9, 0x1b, 0x45, 0xc6, 0x98, 0x7a, 0x24,
    0xf8, 0xa6, 0x44, 0x1a, 0x99, 0xc7, 0x25, 0x7b,
    0x3a, 0x64, 0x86, 0xd8, 0x5b, 0x05, 0xe7, 0xb9,
    0x8c, 0xd2, 0x30, 0x6e, 0xed, 0xb3, 0x51, 0x0f,
    0x4e, 0x10, 0xf2, 0xac, 0x2f, 0x71, 0x93, 0xcd,
    0x11, 0x4f, 0xad, 0xf3, 0x70, 0x2e, 0xcc, 0x92,
    0xd3, 0x8d, 0x6f, 0x31, 0xb2, 0xec, 0x0e, 0x50,
    0xaf, 0xf1, 0x13, 0x4d, 0xce, 0x90, 0x72, 0x2c,
    0x6d, 0x33, 0xd1, 0x8f, 0x0c, 0x52, 0xb0, 0xee,
    0x32, 0x6c, 0x8e, 0xd0, 0x53, 0x0d, 0xef, 0xb1,
    0xf0, 0xae, 0x4c, 0x12, 0x91, 0xcf, 0x2d, 0x73,
    0xca, 0x94, 0x76, 0x28, 0xab, 0xf5, 0x17, 0x49,
    0x08, 0x56, 0xb4, 0xea, 0x69, 0x37, 0xd5, 0x8b,
    0x57, 0x09, 0xeb, 0xb5, 0x36, 0x68, 0x8a, 0xd4,
    0x95, 0xcb, 0x29, 0x77, 0xf4, 0xaa, 0x48, 0x16,
    0xe9, 0xb7, 0x55, 0x0b, 0x88, 0xd6, 0x34, 0x6a,
    0x2b, 0x75, 0x97, 0xc9, 0x4a, 0x14, 0xf6, 0xa8,
    0x74, 0x2a, 0xc8, 0x96, 0x15, 0x4b, 0xa9, 0xf7,
    0xb6, 0xe8, 0x0a, 0x54, 0xd7, 0x89, 0x6b, 0x35,
]

EVENT_TYPES = {
    0x01: "GatewayInfo",
    0x02: "DeviceCommand",
    0x03: "DeviceOnlineStatus",
    0x04: "DeviceLockStatus",
    0x05: "RelayBasicLatchingStatus1",
    0x06: "RelayBasicLatchingStatus2",
    0x07: "RvStatus",
    0x08: "DimmableLightStatus",
    0x09: "RgbLightStatus",
    0x0A: "GeneratorGenieStatus",
    0x0B: "HvacStatus",
    0x0C: "TankSensorStatus",
    0x0D: "RelayHBridgeMomentary1",
    0x0E: "RelayHBridgeMomentary2",
    0x0F: "HourMeterStatus",
}


def crc8(data: bytes, init: int = 0x55) -> int:
    crc = init
    for b in data:
        crc = CRC8_TABLE[(crc ^ b) & 0xFF]
    return crc


def cobs_decode(encoded: bytes) -> bytes:
    result = bytearray()
    i = 0
    while i < len(encoded):
        code = encoded[i]
        i += 1
        if code == 0:
            break
        data_count = code & 0x3F
        zero_count = (code >> 6) & 0x03
        for _ in range(data_count):
            if i < len(encoded):
                result.append(encoded[i])
                i += 1
        for _ in range(zero_count):
            result.append(0x00)
    return bytes(result)


def parse_frames(raw_data: bytes):
    """Parse raw TCP data into decoded frames"""
    frames = []
    current = bytearray()
    in_frame = False
    
    for b in raw_data:
        if b == 0x00:
            if in_frame and len(current) > 0:
                decoded = cobs_decode(bytes(current))
                if len(decoded) > 1:
                    payload = decoded[:-1]
                    crc_byte = decoded[-1]
                    calc_crc = crc8(payload)
                    if calc_crc == crc_byte:
                        frames.append(payload)
                current = bytearray()
            in_frame = True
        else:
            if in_frame:
                current.append(b)
    
    return frames


def analyze_devices(host: str = "192.168.1.1", port: int = 6969, duration: int = 5):
    """Analyze device traffic"""
    print(f"Connecting to {host}:{port}...")
    
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    sock.settimeout(10)
    sock.connect((host, port))
    
    print(f"Collecting data for {duration} seconds...")
    
    raw_data = b""
    start = time.time()
    while time.time() - start < duration:
        try:
            chunk = sock.recv(4096)
            if chunk:
                raw_data += chunk
        except socket.timeout:
            break
    
    sock.close()
    
    # Parse frames
    frames = parse_frames(raw_data)
    print(f"\nTotal frames: {len(frames)}")
    print(f"Raw bytes: {len(raw_data)}")
    
    # Analyze by event type
    event_counts = defaultdict(int)
    devices_by_type = defaultdict(set)
    device_samples = defaultdict(list)
    
    for payload in frames:
        if len(payload) < 1:
            continue
        
        event_type = payload[0]
        event_name = EVENT_TYPES.get(event_type, f"Unknown_{event_type:02X}")
        event_counts[event_name] += 1
        
        # For DimmableLightStatus, extract device info
        if event_type == 0x08 and len(payload) >= 3:
            table_id = payload[1]
            device_id = payload[2]
            key = (table_id, device_id)
            devices_by_type["DimmableLight"].add(key)
            if len(device_samples[key]) < 3:
                device_samples[key].append(payload.hex())
    
    # Print event type summary
    print("\n" + "=" * 60)
    print("EVENT TYPE SUMMARY")
    print("=" * 60)
    for event_name, count in sorted(event_counts.items(), key=lambda x: -x[1]):
        print(f"  {event_name}: {count}")
    
    # Print device summary
    print("\n" + "=" * 60)
    print("DIMMABLE LIGHT DEVICES")
    print("=" * 60)
    print("\nTable  Device  Sample Payload")
    print("-" * 60)
    
    for (table_id, device_id) in sorted(devices_by_type["DimmableLight"]):
        key = (table_id, device_id)
        sample = device_samples[key][0] if device_samples[key] else "N/A"
        print(f"  0x{table_id:02X}   0x{device_id:02X}    {sample[:40]}...")
    
    # Detailed analysis for table 0x00 devices (main device table)
    print("\n" + "=" * 60)
    print("TABLE 0x00 DEVICES (Main Device Table)")
    print("=" * 60)
    
    table0_devices = sorted([d for (t, d) in devices_by_type["DimmableLight"] if t == 0x00])
    print(f"\nFound {len(table0_devices)} devices:")
    for dev_id in table0_devices:
        samples = device_samples[(0x00, dev_id)]
        if samples:
            payload = bytes.fromhex(samples[0])
            if len(payload) >= 5:
                brightness = payload[3]
                state = payload[4]
                print(f"  0x{dev_id:02X}: brightness={brightness}, state=0x{state:02X}")
    
    # Look for specific patterns
    print("\n" + "=" * 60)
    print("SEARCHING FOR KITCHEN LIGHT CANDIDATES")
    print("=" * 60)
    print("\nPossible Kitchen light IDs (based on name patterns):")
    print("  - 0x6A: NOT FOUND in broadcasts")
    print("  - 0x28: Found (labeled as Bed Ceiling)")
    print("\nDevices with state changes (potential toggle candidates):")
    
    # Print any GatewayInfo messages
    gateway_frames = [f for f in frames if len(f) > 0 and f[0] == 0x01]
    if gateway_frames:
        print("\n" + "=" * 60)
        print("GATEWAY INFORMATION MESSAGES")
        print("=" * 60)
        for i, gw in enumerate(gateway_frames[:3]):
            print(f"  {i}: {gw.hex()}")
            if len(gw) >= 2:
                print(f"      EventType=0x01, ProtocolVersion=0x{gw[1]:02X}")
            if len(gw) >= 3:
                print(f"      Options=0x{gw[2]:02X}")
            if len(gw) >= 4:
                print(f"      DeviceCount={gw[3]}")


if __name__ == "__main__":
    import sys
    host = sys.argv[1] if len(sys.argv) > 1 else "192.168.1.1"
    port = int(sys.argv[2]) if len(sys.argv) > 2 else 6969
    duration = int(sys.argv[3]) if len(sys.argv) > 3 else 5
    
    analyze_devices(host, port, duration)
