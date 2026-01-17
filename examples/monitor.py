#!/usr/bin/env python3
"""
OneControl Protocol Monitor

Connects to OneControl device and monitors protocol messages.
Correctly parses COBS-framed MyRvLink events.
"""

import socket
import time
import sys
from datetime import datetime

# CRC-8 lookup table (polynomial 0x8C reflected, CRC-8/MAXIM)
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

# Event types (from MyRvLinkEventType enum)
EVENT_TYPES = {
    0x00: "Unknown",
    0x01: "GatewayInformation",
    0x02: "DeviceCommand",
    0x03: "DeviceOnlineStatus",
    0x04: "DeviceLockStatus",
    0x05: "RelayBasicLatchingStatusType1",
    0x06: "RelayBasicLatchingStatusType2",
    0x07: "RvStatus",
    0x08: "DimmableLightStatus",
    0x09: "RgbLightStatus",
    0x0A: "GeneratorGenieStatus",
    0x0B: "HvacStatus",
    0x0C: "TankSensorStatus",
    0x0D: "RelayHBridgeMomentaryStatusType1",
    0x0E: "RelayHBridgeMomentaryStatusType2",
    0x0F: "HourMeterStatus",
    0x10: "Leveler4DeviceStatus",
    0x11: "LevelerConsoleText",
    0x12: "Leveler1DeviceStatus",
    0x13: "Leveler3DeviceStatus",
    0x14: "Leveler5DeviceStatus",
    0x15: "AutoOperationProgressStatus",
    0x1A: "DeviceSessionStatus",
    0x1B: "TankSensorStatusV2",
    0x20: "RealTimeClock",
    0x21: "CloudGatewayStatus",
    0x22: "TemperatureSensorStatus",
    0x23: "JaycoTbbStatus",
    0x2B: "MonitorPanelStatus",
    0x2C: "AccessoryGatewayStatus",
    0x2F: "AwningSensorStatus",
    0x30: "BrakingSystemStatus",
    0x31: "BatteryMonitorStatus",
    0x32: "ReFlashBootloader",
    0x33: "DoorLockStatus",
    0x35: "DimmableLightExtendedStatus",
    0x36: "LevelerType5ExtendedStatus",
    0x66: "HostDebug",
    0xFF: "Invalid",
}


def crc8(data: bytes, init: int = 0x55) -> int:
    """Calculate CRC-8/MAXIM"""
    crc = init
    for byte in data:
        crc = CRC8_TABLE[(crc ^ byte) & 0xFF]
    return crc


def cobs_decode(encoded: bytes) -> bytes:
    """
    Decode COBS frame (without start/end 0x00 delimiters)
    
    Based on IDS.Portable.Common.COBS.CobsDecoder with numDataBits=6:
    - Lower 6 bits of code byte = number of non-zero data bytes
    - Upper 2 bits of code byte = number of zero bytes to append
    - MaxDataBytes = 63 (2^6 - 1)
    - FrameByteCountLsb = 64 (2^6)
    """
    if not encoded:
        return bytes()
    
    result = bytearray()
    i = 0
    
    while i < len(encoded):
        code = encoded[i]
        i += 1
        
        if code == 0:
            break
        
        # Extract counts from code byte
        # Lower 6 bits = data bytes count
        # Upper 2 bits = zero bytes count
        data_count = code & 0x3F  # Lower 6 bits (max 63)
        zero_count = (code >> 6) & 0x03  # Upper 2 bits (0-3)
        
        # Copy data bytes
        for _ in range(data_count):
            if i < len(encoded):
                result.append(encoded[i])
                i += 1
        
        # Append zero bytes
        for _ in range(zero_count):
            result.append(0x00)
    
    return bytes(result)


def parse_gateway_info(data: bytes) -> dict:
    """Parse GatewayInformation event"""
    result = {"raw": data.hex(), "length": len(data)}
    
    if len(data) < 13:
        result["error"] = "too short"
        return result
    
    result.update({
        "protocol_version": data[1],
        "options": data[2],
        "config_mode": bool(data[2] & 0x01),
        "device_count": data[3],
        "device_table_id": data[4],
        "device_table_crc": int.from_bytes(data[5:9], 'little'),
        "metadata_crc": int.from_bytes(data[9:13], 'little'),
    })
    return result


def parse_dimmable_light_status(data: bytes) -> dict:
    """Parse DimmableLightStatus event"""
    if len(data) < 5:
        return {"error": "too short"}
    
    return {
        "device_table_id": data[1],
        "device_id": data[2],
        "brightness": data[3],
        "state": data[4],
        "raw": data[5:].hex() if len(data) > 5 else "",
    }


def parse_event(data: bytes) -> str:
    """Parse a decoded event and return description"""
    if not data:
        return "Empty event"
    
    event_type = data[0]
    event_name = EVENT_TYPES.get(event_type, f"Unknown(0x{event_type:02X})")
    
    # Parse specific events
    if event_type == 0x01:  # GatewayInformation
        info = parse_gateway_info(data)
        return f"{event_name}: version={info.get('protocol_version')}, devices={info.get('device_count')}, table=0x{info.get('device_table_id', 0):02X}"
    
    elif event_type == 0x08:  # DimmableLightStatus
        info = parse_dimmable_light_status(data)
        return f"{event_name}: device=0x{info.get('device_id', 0):02X}, brightness={info.get('brightness')}, state=0x{info.get('state', 0):02X}"
    
    elif event_type == 0x02:  # DeviceCommand (response)
        if len(data) > 3:
            cmd_id = int.from_bytes(data[1:3], 'little')
            return f"{event_name}: cmd_id=0x{cmd_id:04X}, data={data[3:].hex()}"
    
    return f"{event_name}: {data[1:].hex()}"


class CobsDecoder:
    """Streaming COBS decoder"""
    
    def __init__(self):
        self.buffer = bytearray()
        self.in_frame = False
    
    def process(self, data: bytes):
        """Process incoming data, yield decoded frames"""
        for byte in data:
            if byte == 0x00:
                if self.in_frame and len(self.buffer) > 0:
                    # End of frame - decode it
                    decoded = cobs_decode(bytes(self.buffer))
                    if len(decoded) > 1:
                        # Verify CRC (last byte should be CRC of previous bytes)
                        payload = decoded[:-1]
                        expected_crc = decoded[-1]
                        actual_crc = crc8(payload)
                        if actual_crc == expected_crc:
                            yield payload
                        else:
                            # CRC mismatch - still yield but note it
                            yield payload  # Yield anyway for analysis
                    self.buffer.clear()
                # Start of new frame
                self.in_frame = True
            else:
                if self.in_frame:
                    self.buffer.append(byte)


def monitor(host: str = "192.168.1.1", port: int = 6969, duration: int = 60):
    """Monitor OneControl protocol messages"""
    
    print(f"Connecting to {host}:{port}...")
    
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    sock.settimeout(10)
    sock.connect((host, port))
    sock.setblocking(False)
    
    print(f"Connected! Monitoring for {duration} seconds...")
    print("-" * 60)
    
    decoder = CobsDecoder()
    start_time = time.time()
    gateway_info = None
    
    while time.time() - start_time < duration:
        try:
            data = sock.recv(4096)
            if not data:
                break
            
            for frame in decoder.process(data):
                timestamp = datetime.now().strftime("%H:%M:%S.%f")[:-3]
                
                # Parse and display
                description = parse_event(frame)
                
                # Check for gateway info
                if frame[0] == 0x01 and gateway_info is None:
                    gateway_info = parse_gateway_info(frame)
                    print(f"\n{'='*60}")
                    print(f"GATEWAY INFO RECEIVED:")
                    if "error" in gateway_info:
                        print(f"  Error: {gateway_info['error']}")
                        print(f"  Raw: {gateway_info['raw']}")
                    else:
                        print(f"  Protocol Version: {gateway_info.get('protocol_version', 'N/A')}")
                        print(f"  Device Count: {gateway_info.get('device_count', 'N/A')}")
                        print(f"  Device Table ID: 0x{gateway_info.get('device_table_id', 0):02X}")
                        print(f"  Config Mode: {gateway_info.get('config_mode', 'N/A')}")
                    print(f"{'='*60}\n")
                
                # Show event
                print(f"[{timestamp}] {description}")
                
        except BlockingIOError:
            time.sleep(0.01)
        except socket.timeout:
            pass
    
    sock.close()
    print("\nMonitoring complete.")
    
    if gateway_info:
        print(f"\nGateway Summary:")
        print(f"  Devices: {gateway_info['device_count']}")
        print(f"  Table ID: 0x{gateway_info['device_table_id']:02X}")


if __name__ == "__main__":
    host = sys.argv[1] if len(sys.argv) > 1 else "192.168.1.1"
    port = int(sys.argv[2]) if len(sys.argv) > 2 else 6969
    duration = int(sys.argv[3]) if len(sys.argv) > 3 else 30
    
    monitor(host, port, duration)
