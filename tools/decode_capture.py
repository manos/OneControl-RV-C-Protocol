#!/usr/bin/env python3
"""
Decode captured TCP data from OneControl port 6969.

The protocol appears to use 0x00 as frame delimiters, but the payload
may or may not be COBS encoded. This tool tries both approaches.
"""

import sys
import os
sys.path.insert(0, os.path.dirname(os.path.dirname(os.path.abspath(__file__))))

from rvc.cobs import decode as cobs_decode, crc8, crc32_le


def hex_dump(data: bytes, prefix: str = "  ") -> str:
    """Create a hex dump of data."""
    lines = []
    for i in range(0, len(data), 16):
        chunk = data[i:i+16]
        hex_part = ' '.join(f'{b:02x}' for b in chunk)
        ascii_part = ''.join(chr(b) if 32 <= b < 127 else '.' for b in chunk)
        lines.append(f"{prefix}{i:04x}:  {hex_part:<48}  {ascii_part}")
    return '\n'.join(lines)


def split_by_null(data: bytes) -> list:
    """Split data by 0x00 bytes, keeping non-empty segments."""
    segments = []
    current = bytearray()
    
    for byte in data:
        if byte == 0:
            if current:
                segments.append(bytes(current))
                current = bytearray()
        else:
            current.append(byte)
    
    if current:
        segments.append(bytes(current))
    
    return segments


def analyze_segment(segment: bytes, index: int):
    """Analyze a single segment (data between 0x00 delimiters)."""
    print(f"\n{'='*60}")
    print(f"Segment {index}: {len(segment)} bytes")
    print(f"{'='*60}")
    
    if len(segment) < 1:
        print("  Empty segment")
        return
    
    # Show hex dump
    print(hex_dump(segment))
    
    # Try to identify message type (first byte)
    msg_type = segment[0]
    print(f"\n  First byte (msg type?): 0x{msg_type:02X} ({msg_type})")
    
    # Known message types from analysis
    type_names = {
        0x40: "DeviceState/StatusUpdate",
        0x41: "NodeInfo/Discovery",
        0x43: "MultiMessage/Compound",
        0x44: "Command44",
        0x45: "SetValue/DimmerCommand",
        0x49: "Init/Handshake?",
        0x85: "Toggle/Switch",
        0xC3: "StatusC3",
        0xC5: "StatusQuery/Poll",
    }
    
    if msg_type in type_names:
        print(f"  Known type: {type_names[msg_type]}")
    
    # If length byte follows
    if len(segment) >= 2:
        len_byte = segment[1]
        print(f"  Second byte (length?): 0x{len_byte:02X} ({len_byte})")
        
        # Check if it could be a length
        expected_len = len(segment) - 2  # minus type and length byte
        if len_byte == expected_len:
            print(f"    ✓ Matches remaining length ({expected_len})")
        elif len_byte == expected_len + 1:
            print(f"    ✓ Matches remaining length + 1 ({expected_len + 1})")
        elif len_byte == len(segment):
            print(f"    ✓ Matches total segment length")
    
    # Look for magic bytes in hex string
    hex_str = segment.hex()
    magic_bytes = ['83ac', '83ae', '8084', '8086', '83dc']
    for magic in magic_bytes:
        if magic in hex_str:
            pos = hex_str.find(magic) // 2
            print(f"  Contains magic 0x{magic.upper()} at offset {pos}")
    
    # Look for potential device instance bytes
    if len(segment) >= 4:
        # Instance usually at offset 3-4
        for i in range(2, min(6, len(segment))):
            inst = segment[i]
            if inst in [0x21, 0x28, 0xEB, 0x00]:  # Known instances
                print(f"  Potential instance 0x{inst:02X} at offset {i}")
    
    # Try COBS decode
    try:
        decoded = cobs_decode(segment)
        if decoded != segment:
            print(f"\n  COBS decoded ({len(decoded)} bytes):")
            print(hex_dump(decoded, "    "))
    except ValueError:
        pass  # Not valid COBS
    
    # Check for CRC at end
    if len(segment) >= 2:
        crc8_check = crc8(segment[:-1])
        if crc8_check == segment[-1]:
            print(f"\n  CRC-8 valid! Last byte = 0x{segment[-1]:02X}")
    
    if len(segment) >= 5:
        crc32_check = crc32_le(segment[:-4])
        crc32_val = int.from_bytes(segment[-4:], 'little')
        if crc32_check == crc32_val:
            print(f"\n  CRC-32 LE valid! Last 4 bytes = 0x{crc32_val:08X}")


def decode_hex_string(hex_str: str):
    """Decode a hex string (from tcpdump, etc.)."""
    # Remove spaces, newlines, colons
    hex_str = hex_str.replace(':', '').replace('\n', ' ')
    hex_str = ''.join(hex_str.split())
    
    try:
        raw_data = bytes.fromhex(hex_str)
    except ValueError as e:
        print(f"Error parsing hex: {e}")
        return
    
    print(f"Input: {len(raw_data)} bytes")
    print(hex_dump(raw_data))
    
    # Split by null bytes
    segments = split_by_null(raw_data)
    
    print(f"\n{'#'*60}")
    print(f"Found {len(segments)} segments (split by 0x00)")
    print(f"{'#'*60}")
    
    for i, segment in enumerate(segments):
        analyze_segment(segment, i)


def analyze_message_structure():
    """Show analysis of the discovered message structure."""
    print("""
OneControl TCP Message Structure (Port 6969)
============================================

Frame Format:
  [0x00] [Message] [0x00]

Message Format (preliminary):
  [Type] [Len?] [Magic?] [Instance] [Command] [Data...] [CRC?]

Known Message Types:
  0x40 - Device state / status update
  0x41 - Node info / device discovery
  0x43 - Multi-message / compound message
  0x45 - Set value / dimmer command
  0x85 - Toggle / switch command
  0xC5 - Status query / poll

Known Magic Bytes:
  0x83AC - Seen in status messages
  0x83AE - Seen in status messages
  0x8084 - ?
  0x8086 - ?
  0x83DC - ?

Known Device Instances (from tcpdump):
  0x21 - Kitchen light
  0x28 - Bed Ceiling light
  0xEB - Another device (possibly Kitchen?)

CRC:
  - Possibly CRC-8 or CRC-32 LE
  - May be optional (UseCrc flag found in DLLs)

COBS:
  - Found in DLL but may not be used for all messages
  - Or may be used at a different layer
""")


def interactive_mode():
    """Interactive mode for testing."""
    print("OneControl Decoder - Interactive Mode")
    print("Enter hex data (spaces allowed), 'h' for help, or 'q' to quit")
    print()
    
    while True:
        try:
            line = input("hex> ").strip()
            if line.lower() in ('q', 'quit', 'exit'):
                break
            if line.lower() == 'h':
                analyze_message_structure()
                continue
            if not line:
                continue
            decode_hex_string(line)
            print()
        except EOFError:
            break
        except KeyboardInterrupt:
            print()
            break


# Sample captures from tcpdump
SAMPLE_CAPTURES = {
    "controller_stream": """
        00 43 08 02 6d 43 2f 6d 17 81 02 01 21 
        00 40 03 03 6d 83
        00 c5 04 01 6d 40 01 cb
        00 40 03 03 6d 83
    """,
    
    "app_commands": """
        00 43 01 06 6d 01 d4
        00 41 08 41 6d 08 1c 88 43 4f af 67 82 27
    """,
    
    "kitchen_toggle": """
        00 45 02 83 ac 21 42 02 00 00
        00 45 02 83 ac 21 42 02 64 00
    """,
    
    "bed_ceiling_toggle": """
        00 45 02 83 ac 28 42 02 00 00
        00 45 02 83 ac 28 42 02 64 00
    """,
}


if __name__ == '__main__':
    if len(sys.argv) > 1:
        if sys.argv[1] == '-i':
            interactive_mode()
        elif sys.argv[1] == '-s':
            # Show sample captures
            for name, data in SAMPLE_CAPTURES.items():
                print(f"\n{'#'*60}")
                print(f"Sample: {name}")
                print(f"{'#'*60}")
                decode_hex_string(data)
        elif sys.argv[1] == '-h':
            analyze_message_structure()
        else:
            # Treat argument as hex string
            decode_hex_string(' '.join(sys.argv[1:]))
    else:
        print("Usage:")
        print("  python decode_capture.py <hex_data>   - Decode hex string")
        print("  python decode_capture.py -i          - Interactive mode")
        print("  python decode_capture.py -s          - Show sample captures")
        print("  python decode_capture.py -h          - Show message structure")
        print()
        print("Example:")
        print("  python decode_capture.py 00 43 08 02 6d 43 2f 6d 00")
