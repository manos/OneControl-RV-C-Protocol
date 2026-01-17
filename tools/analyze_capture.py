#!/usr/bin/env python3
"""
Analyze captured traffic from OneControl TCP connection.

This tool helps reverse-engineer the protocol by analyzing raw packet data
captured via tcpdump or similar tools.

Usage:
    # Capture traffic on cubie:
    tcpdump -i wlan0 -w capture.pcap port 6969
    
    # Or capture raw hex:
    tcpdump -i wlan0 -X port 6969 > capture.txt
    
    # Analyze:
    python analyze_capture.py capture.pcap
    python analyze_capture.py --hex capture.txt
"""

import sys
import argparse
from typing import List, Tuple
from collections import Counter


def parse_hex_dump(text: str) -> List[bytes]:
    """Parse tcpdump -X output into raw bytes."""
    packets = []
    current_packet = bytearray()
    
    for line in text.split('\n'):
        line = line.strip()
        
        # Skip non-hex lines
        if not line or ':' not in line:
            if current_packet:
                packets.append(bytes(current_packet))
                current_packet = bytearray()
            continue
        
        # Parse hex dump format: "0x0000:  4500 003c 1c46 4000"
        parts = line.split()
        if len(parts) < 2:
            continue
            
        # Check if this looks like hex offset: starts with hex address
        if parts[0].startswith('0x') or parts[0].endswith(':'):
            # Extract hex bytes
            for part in parts[1:]:
                # Skip ASCII representation at end of line
                if len(part) != 4 or not all(c in '0123456789abcdefABCDEF' for c in part):
                    break
                try:
                    current_packet.append(int(part[:2], 16))
                    current_packet.append(int(part[2:], 16))
                except ValueError:
                    break
    
    if current_packet:
        packets.append(bytes(current_packet))
    
    return packets


def analyze_tcp_payload(payload: bytes) -> None:
    """Analyze Lippert OneControl TCP payload."""
    print(f"\n{'='*60}")
    print(f"Payload: {len(payload)} bytes")
    print(f"Hex: {payload.hex()}")
    
    # Lippert message types
    MSG_TYPES = {
        0x40: "STATE/SHORT",
        0x41: "NODE/REGISTER",
        0x43: "MULTI/QUERY",
        0x45: "STATUS/SET",
        0x85: "BROADCAST",
        0xC3: "CONFIG",
        0xC5: "EXT_STATUS",
    }
    
    # Magic byte sequences
    MAGIC_BYTES = {
        (0x80, 0x84): "Standard",
        (0x80, 0x86): "Alternate",
        (0x83, 0xdc): "Legacy?",
    }
    
    # Parse Lippert messages
    offset = 0
    msg_count = 0
    
    while offset < len(payload) - 2:
        remaining = payload[offset:]
        
        # Lippert messages start with 0x00
        if remaining[0] == 0x00 and len(remaining) >= 6:
            msg_type = remaining[1]
            
            if msg_type in MSG_TYPES:
                msg_count += 1
                type_name = MSG_TYPES[msg_type]
                
                # Try to find message end (next 0x00 start or known pattern)
                # Common message lengths: 8, 11, 12, 15, 19, 30, 53
                
                print(f"\n  [{msg_count}] Type 0x{msg_type:02X} ({type_name}) at offset {offset}")
                
                # Parse based on type
                if msg_type == 0x45:
                    # 0x45 format: 00 45 02 [magic] [inst] [cmd] [data] 00
                    if len(remaining) >= 11:
                        len_marker = remaining[2]
                        magic = (remaining[3], remaining[4])
                        instance = remaining[5]
                        cmd = remaining[6]
                        
                        magic_name = MAGIC_BYTES.get(magic, "Unknown")
                        print(f"       Magic: {magic[0]:02X} {magic[1]:02X} ({magic_name})")
                        print(f"       Instance: 0x{instance:02X} ({instance})")
                        print(f"       Command: 0x{cmd:02X} ({'STATUS' if cmd == 0x11 else 'SET' if cmd == 0x42 else 'OTHER'})")
                        
                        if cmd == 0x42 and len(remaining) >= 11:
                            # Set command with value
                            value = remaining[8] | (remaining[9] << 8)
                            print(f"       Value: {value} (0x{value:04X})")
                        
                        # Find end
                        for end_off in range(8, min(15, len(remaining))):
                            if remaining[end_off] == 0x00:
                                print(f"       Raw: {remaining[:end_off+1].hex()}")
                                offset += end_off + 1
                                break
                        else:
                            offset += 11
                        continue
                
                elif msg_type == 0x43:
                    # Multi-part: 00 43 01 06 [inst] 01 [val] 00
                    if len(remaining) >= 8:
                        sub = remaining[2]
                        cmd = remaining[3]
                        instance = remaining[4]
                        
                        print(f"       Sub: 0x{sub:02X}, Cmd: 0x{cmd:02X}")
                        print(f"       Instance: 0x{instance:02X} ({instance})")
                        
                        if len(remaining) >= 8 and remaining[7] == 0x00:
                            print(f"       Raw: {remaining[:8].hex()}")
                            offset += 8
                            continue
                
                elif msg_type == 0x41:
                    # Registration: 00 41 08 41 [inst] ...
                    if len(remaining) >= 16:
                        instance = remaining[4]
                        device_id = remaining[8:13]
                        print(f"       Instance: 0x{instance:02X}")
                        print(f"       Device ID: {device_id.hex()}")
                        print(f"       Raw: {remaining[:16].hex()}")
                        offset += 16
                        continue
                
                # Default: show first 12 bytes
                show_len = min(12, len(remaining))
                print(f"       Raw: {remaining[:show_len].hex()}")
                offset += show_len
                continue
        
        # Not a recognized message start
        offset += 1
    
    # Byte frequency analysis
    print(f"\n  Byte frequency (top 10):")
    freq = Counter(payload)
    for byte, count in freq.most_common(10):
        print(f"    0x{byte:02X}: {count} times ({count*100/len(payload):.1f}%)")


def find_can_frames(data: bytes) -> List[Tuple[int, int, bytes]]:
    """
    Search for potential CAN frame patterns in raw data.
    
    Returns list of (offset, canid, data) tuples.
    """
    from rvc.dgn import DGN
    
    known_dgns = set(dgn.value for dgn in DGN)
    frames = []
    
    for offset in range(len(data) - 11):
        # Try both endianness
        for endian in ['big', 'little']:
            canid = int.from_bytes(data[offset:offset+4], endian)
            dgn = (canid >> 8) & 0x1FFFF
            
            if dgn in known_dgns:
                frame_data = data[offset+4:offset+12]
                frames.append((offset, canid, frame_data, endian))
                break
    
    return frames


def main():
    parser = argparse.ArgumentParser(description='Analyze OneControl TCP captures')
    parser.add_argument('file', help='Capture file (pcap or text)')
    parser.add_argument('--hex', action='store_true', help='File is tcpdump -X hex output')
    parser.add_argument('--raw', type=str, help='Raw hex string to analyze')
    args = parser.parse_args()
    
    if args.raw:
        data = bytes.fromhex(args.raw.replace(' ', ''))
        analyze_tcp_payload(data)
        
        print("\n\nSearching for CAN frames with known DGNs...")
        frames = find_can_frames(data)
        for offset, canid, fdata, endian in frames:
            dgn = (canid >> 8) & 0x1FFFF
            src = canid & 0xFF
            print(f"  Found at offset {offset} ({endian}): DGN=0x{dgn:05X}, src=0x{src:02X}, data={fdata.hex()}")
        
        return
    
    if args.hex:
        with open(args.file, 'r') as f:
            text = f.read()
        packets = parse_hex_dump(text)
        
        print(f"Found {len(packets)} packets")
        for i, packet in enumerate(packets):
            print(f"\n\n{'#'*60}")
            print(f"# Packet {i+1}")
            print(f"{'#'*60}")
            analyze_tcp_payload(packet)
    else:
        # Try to use scapy for pcap files
        try:
            from scapy.all import rdpcap, TCP
            
            packets = rdpcap(args.file)
            tcp_payloads = []
            
            for pkt in packets:
                if TCP in pkt and pkt[TCP].payload:
                    payload = bytes(pkt[TCP].payload)
                    if payload:
                        tcp_payloads.append(payload)
            
            print(f"Found {len(tcp_payloads)} TCP payloads")
            for i, payload in enumerate(tcp_payloads):
                print(f"\n\n{'#'*60}")
                print(f"# TCP Payload {i+1}")
                print(f"{'#'*60}")
                analyze_tcp_payload(payload)
                
        except ImportError:
            print("scapy not installed. Use --hex with tcpdump -X output, or install scapy:")
            print("  pip install scapy")
            sys.exit(1)


if __name__ == '__main__':
    main()
