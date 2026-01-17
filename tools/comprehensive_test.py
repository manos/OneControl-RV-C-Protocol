#!/usr/bin/env python3
"""
Comprehensive OneControl TCP protocol test suite.

Systematically tests different command formats and approaches to identify
why control commands are not working.
"""

import socket
import time
import random
import sys
from typing import Optional, List, Tuple

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

# Known device IDs
BED_CEILING = 0x28
KITCHEN = 0x21

def crc8(data: bytes, init: int = 0x55) -> int:
    crc = init
    for b in data:
        crc = CRC8_TABLE[(crc ^ b) & 0xFF]
    return crc

def cobs_encode(raw: bytes) -> bytes:
    """COBS encode with CRC-8."""
    crc = crc8(raw)
    source = bytearray(raw) + bytes([crc])
    result = bytearray([0x00])
    i = 0
    while i < len(source):
        data_count = 0
        data_start = i
        while i < len(source) and source[i] != 0 and data_count < 63:
            i += 1
            data_count += 1
        zero_count = 0
        while i < len(source) and source[i] == 0 and zero_count < 3:
            i += 1
            zero_count += 1
        code = data_count | (zero_count << 6)
        result.append(code)
        result.extend(source[data_start:data_start + data_count])
    result.append(0x00)
    return bytes(result)

def cobs_decode(encoded: bytes) -> bytes:
    """COBS decode."""
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

class OneControlClient:
    def __init__(self, host: str = "192.168.1.1", port: int = 6969):
        self.host = host
        self.port = port
        self.sock = None
        self.command_id = 1
        
    def connect(self):
        self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.sock.settimeout(5)
        self.sock.connect((self.host, self.port))
        print(f"Connected to {self.host}:{self.port}")
        
    def close(self):
        if self.sock:
            self.sock.close()
            self.sock = None
            
    def drain(self, timeout: float = 0.5):
        """Drain any pending data."""
        self.sock.setblocking(False)
        try:
            while self.sock.recv(4096):
                pass
        except:
            pass
        self.sock.setblocking(True)
        self.sock.settimeout(5)
        
    def send_raw(self, data: bytes):
        """Send raw COBS-encoded data."""
        print(f"  TX: {data.hex()}")
        self.sock.sendall(data)
        
    def send_command(self, cmd_type: int, payload: bytes) -> int:
        """Send a command with auto-incrementing ID."""
        cmd_id = self.command_id
        self.command_id = (self.command_id + 1) % 65534
        if self.command_id == 0:
            self.command_id = 1
            
        raw = bytes([
            cmd_id & 0xFF, (cmd_id >> 8) & 0xFF,
            cmd_type
        ]) + payload
        
        encoded = cobs_encode(raw)
        print(f"  TX (ID={cmd_id}): {raw.hex()} -> {encoded.hex()}")
        self.sock.sendall(encoded)
        return cmd_id
        
    def recv_frames(self, timeout: float = 1.0) -> List[bytes]:
        """Receive and decode frames."""
        self.sock.settimeout(timeout)
        data = b""
        try:
            while True:
                chunk = self.sock.recv(4096)
                if chunk:
                    data += chunk
                else:
                    break
        except socket.timeout:
            pass
        except:
            pass
            
        # Parse frames
        frames = []
        current = bytearray()
        in_frame = False
        for b in data:
            if b == 0x00:
                if in_frame and len(current) > 0:
                    decoded = cobs_decode(bytes(current))
                    if len(decoded) > 1:
                        payload = decoded[:-1]
                        crc_byte = decoded[-1]
                        if crc8(payload) == crc_byte:
                            frames.append(payload)
                    current = bytearray()
                in_frame = True
            else:
                if in_frame:
                    current.append(b)
                    
        return frames
    
    def find_response(self, frames: List[bytes], cmd_id: int) -> Optional[bytes]:
        """Find command response by ID."""
        for frame in frames:
            if len(frame) > 2 and frame[0] == 0x02:  # DeviceCommand response
                resp_id = frame[1] | (frame[2] << 8)
                if resp_id == cmd_id:
                    return frame
        return None
        
    def find_device_status(self, frames: List[bytes], device_id: int) -> Optional[bytes]:
        """Find device status update."""
        for frame in frames:
            if len(frame) >= 3 and frame[0] == 0x08:  # DimmableLightStatus
                if frame[2] == device_id:
                    return frame
        return None


def test_action_switch(client: OneControlClient, device_id: int, table_id: int = 0x00, state: int = 0x01):
    """Test ActionSwitch command."""
    print(f"\n[TEST] ActionSwitch device=0x{device_id:02X}, table=0x{table_id:02X}, state={state}")
    
    payload = bytes([table_id, state, device_id])
    cmd_id = client.send_command(0x40, payload)
    
    time.sleep(0.5)
    frames = client.recv_frames(timeout=2)
    
    # Look for response
    response = client.find_response(frames, cmd_id)
    if response:
        print(f"  ✓ Got response: {response.hex()}")
    else:
        print(f"  ✗ No response to command ID {cmd_id}")
        
    # Look for status update
    status = client.find_device_status(frames, device_id)
    if status:
        print(f"  Device status: {status.hex()}")
        
    return response is not None


def test_action_dimmable(client: OneControlClient, device_id: int, table_id: int = 0x00, 
                         cmd: int = 1, brightness: int = 200):
    """Test ActionDimmable command."""
    print(f"\n[TEST] ActionDimmable device=0x{device_id:02X}, table=0x{table_id:02X}, cmd={cmd}, br={brightness}")
    
    # [TableId][DeviceId][Command][Brightness][Duration][CycleTime1(2)][CycleTime2(2)][Reserved]
    payload = bytes([
        table_id,
        device_id,
        cmd,           # 0=Off, 1=On, 127=Restore
        brightness,    # Max brightness
        0x00,          # Duration
        0x00, 0xDC,    # CycleTime1 = 220
        0x00, 0xDC,    # CycleTime2 = 220
        0x00           # Reserved
    ])
    
    cmd_id = client.send_command(0x43, payload)
    
    time.sleep(0.5)
    frames = client.recv_frames(timeout=2)
    
    response = client.find_response(frames, cmd_id)
    if response:
        print(f"  ✓ Got response: {response.hex()}")
    else:
        print(f"  ✗ No response to command ID {cmd_id}")
        
    return response is not None


def test_registration(client: OneControlClient, session_id: Optional[int] = None):
    """Test registration packet."""
    if session_id is None:
        session_id = random.randint(0x01, 0xFF)
        
    print(f"\n[TEST] Registration with session_id=0x{session_id:02X}")
    
    # Registration format from app capture
    app_id = bytes([0x12, 0x34, 0x56, 0x78, 0x9a, 0xbc, 0xde])
    raw = bytes([
        0x08, 0x00,  # ClientCommandId = 8
        0x00,        # CommandType = 0 (registration?)
        session_id,
    ]) + app_id
    
    encoded = cobs_encode(raw)
    print(f"  TX: {raw.hex()} -> {encoded.hex()}")
    client.sock.sendall(encoded)
    
    time.sleep(0.5)
    frames = client.recv_frames(timeout=2)
    
    # Look for any response that might indicate registration success
    for frame in frames[:10]:
        if len(frame) > 0 and frame[0] == 0x02:
            print(f"  Got DeviceCommand response: {frame.hex()}")
            return True
            
    print(f"  Received {len(frames)} frames, no specific registration response found")
    return False


def test_get_gateway_info(client: OneControlClient) -> Optional[dict]:
    """Wait for GatewayInformation event."""
    print("\n[TEST] Waiting for GatewayInformation...")
    
    frames = client.recv_frames(timeout=3)
    
    for frame in frames:
        if len(frame) > 0 and frame[0] == 0x01:  # GatewayInformation
            print(f"  Found GatewayInformation: {frame.hex()}")
            
            if len(frame) >= 5:
                info = {
                    'protocol_version': frame[1] if len(frame) > 1 else 0,
                    'options': frame[2] if len(frame) > 2 else 0,
                    'device_count': frame[3] if len(frame) > 3 else 0,
                    'device_table_id': frame[4] if len(frame) > 4 else 0,
                }
                print(f"  Protocol: {info['protocol_version']}, Devices: {info['device_count']}, Table: 0x{info['device_table_id']:02X}")
                return info
                
    print("  No GatewayInformation found")
    return None


def run_comprehensive_test():
    """Run all tests systematically."""
    print("=" * 60)
    print("OneControl Comprehensive Protocol Test")
    print("=" * 60)
    
    client = OneControlClient()
    
    try:
        # Connect
        client.connect()
        client.drain()
        
        # Get gateway info
        gateway_info = test_get_gateway_info(client)
        
        # Test registration
        session_id = random.randint(0x01, 0xFF)
        test_registration(client, session_id)
        
        # Try various commands
        print("\n" + "=" * 60)
        print("Testing Control Commands")
        print("=" * 60)
        
        # Test 1: ActionSwitch with different tables
        for table_id in [0x00, 0x01, 0x02]:
            test_action_switch(client, BED_CEILING, table_id, state=0x01)
            time.sleep(1)
            test_action_switch(client, BED_CEILING, table_id, state=0x00)
            time.sleep(1)
            
        # Test 2: ActionDimmable
        test_action_dimmable(client, BED_CEILING, table_id=0x00, cmd=1, brightness=200)
        time.sleep(1)
        test_action_dimmable(client, BED_CEILING, table_id=0x00, cmd=0, brightness=0)
        time.sleep(1)
        test_action_dimmable(client, BED_CEILING, table_id=0x00, cmd=127)  # Restore
        
        # Test 3: Try Kitchen light
        print("\n" + "=" * 60)
        print("Testing Kitchen Light (0x21)")
        print("=" * 60)
        
        test_action_switch(client, KITCHEN, table_id=0x00, state=0x01)
        time.sleep(1)
        test_action_dimmable(client, KITCHEN, table_id=0x00, cmd=1, brightness=200)
        
        # Summary
        print("\n" + "=" * 60)
        print("Test Complete")
        print("=" * 60)
        print("\nCheck if any lights changed state!")
        print("- Bed Ceiling should have toggled on/off")
        print("- Kitchen should have turned on")
        
    finally:
        client.close()


if __name__ == "__main__":
    run_comprehensive_test()
