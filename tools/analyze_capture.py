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
    """Analyze potential RV-C framing in TCP payload."""
    print(f"\n{'='*60}")
    print(f"Payload: {len(payload)} bytes")
    print(f"Hex: {payload.hex()}")
    
    # Look for patterns
    offset = 0
    messages = []
    
    while offset < len(payload):
        remaining = payload[offset:]
        
        # Pattern 1: 0x00 followed by length byte
        if len(remaining) >= 2 and remaining[0] == 0x00:
            length = remaining[1]
            if length > 0 and len(remaining) >= length + 2:
                msg = remaining[2:length + 2]
                messages.append(('len_prefixed', offset, msg))
                print(f"\n  [Offset {offset}] Length-prefixed message ({length} bytes):")
                print(f"    Data: {msg.hex()}")
                
                # Try to interpret as CAN frame
                if len(msg) >= 12:
                    canid = int.from_bytes(msg[:4], 'big')
                    dgn = (canid >> 8) & 0x1FFFF
                    src = canid & 0xFF
                    data = msg[4:12]
                    print(f"    → CAN ID: 0x{canid:08X} (DGN=0x{dgn:05X}, src=0x{src:02X})")
                    print(f"    → Data: {data.hex()}")
                
                offset += length + 2
                continue
        
        # Pattern 2: Type marker (0x40, 0x41, 0x43) followed by data
        if remaining[0] in (0x40, 0x41, 0x43):
            type_byte = remaining[0]
            # Guess at message boundary - try common sizes
            for size in [8, 12, 16]:
                if len(remaining) >= size:
                    msg = remaining[1:size]
                    messages.append(('typed', offset, msg))
                    print(f"\n  [Offset {offset}] Type 0x{type_byte:02X} message (~{size-1} bytes):")
                    print(f"    Data: {msg.hex()}")
                    break
            offset += 1  # Move at least one byte
            continue
        
        # Unknown - try to find next recognizable pattern
        print(f"\n  [Offset {offset}] Unknown byte: 0x{remaining[0]:02X}")
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
