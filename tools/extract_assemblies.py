#!/usr/bin/env python3
"""
Extract .NET assemblies from Xamarin blob files.

The blob format (XABA - Xamarin Assembly Blob Archive):
- Found at offset 0x4000 in the ELF wrapper
- Header: "XABA" + version (4 bytes) + entry_count (4 bytes) + index_entry_count (4 bytes)
- Index: array of (hash, offset, size) entries
- Assemblies: XALZ compressed blocks

XALZ format (LZ4 compressed assembly):
- "XALZ" magic (4 bytes)
- Index (4 bytes)
- Uncompressed size (4 bytes)  
- LZ4 compressed data (starts with MZ after decompression)
"""

import struct
import sys
import os
from pathlib import Path
import re

# Try to import lz4
try:
    import lz4.block
    HAS_LZ4 = True
except ImportError:
    HAS_LZ4 = False
    print("Warning: lz4 not available. Install with: pip3 install lz4")

def find_xaba_header(data):
    """Find XABA header in data."""
    offset = data.find(b'XABA')
    return offset if offset != -1 else None

def find_all_xalz(data):
    """Find all XALZ blocks in data."""
    blocks = []
    offset = 0
    while True:
        pos = data.find(b'XALZ', offset)
        if pos == -1:
            break
        blocks.append(pos)
        offset = pos + 1
    return blocks

def decompress_xalz(data, offset):
    """Decompress a XALZ block."""
    if data[offset:offset+4] != b'XALZ':
        return None, None, None
    
    # Parse header
    idx = struct.unpack('<I', data[offset+4:offset+8])[0]
    uncompressed_size = struct.unpack('<I', data[offset+8:offset+12])[0]
    
    # Compressed data starts after 12-byte header
    # The data appears to start with what looks like MZ but it's part of LZ4 frame
    compressed_start = offset + 12
    
    # Find the next XALZ or end
    next_xalz = data.find(b'XALZ', compressed_start)
    if next_xalz == -1:
        next_xalz = len(data)
    
    compressed_data = data[compressed_start:next_xalz]
    
    if not HAS_LZ4:
        return idx, uncompressed_size, None
    
    try:
        # Try LZ4 block decompression
        decompressed = lz4.block.decompress(compressed_data, uncompressed_size=uncompressed_size)
        return idx, uncompressed_size, decompressed
    except Exception as e:
        # Maybe it's not actually compressed, check if it starts with MZ
        if compressed_data[:2] == b'MZ':
            return idx, uncompressed_size, compressed_data
        return idx, uncompressed_size, None

def extract_assembly_names(data):
    """Extract assembly names from the blob."""
    names = {}
    # Look for .dll names with null terminator
    pattern = rb'([A-Za-z][A-Za-z0-9_.]*\.dll)\x00'
    for match in re.finditer(pattern, data):
        name = match.group(1).decode('utf-8')
        names[match.start()] = name
    return names

def main():
    blob_path = Path(__file__).parent / "decompile/native/lib/armeabi-v7a/libassemblies.armeabi-v7a.blob.so"
    output_dir = Path(__file__).parent / "decompile/extracted_dlls"
    
    if len(sys.argv) > 1:
        blob_path = Path(sys.argv[1])
    if len(sys.argv) > 2:
        output_dir = Path(sys.argv[2])
    
    print(f"Reading: {blob_path}")
    with open(blob_path, 'rb') as f:
        data = f.read()
    print(f"Size: {len(data):,} bytes")
    
    # Find assembly names
    names = extract_assembly_names(data)
    print(f"\nFound {len(names)} DLL name references")
    
    # Filter to interesting ones
    interesting = [n for n in names.values() if 
                   'OneControl' in n or 'IDS' in n or 'DotNetty' in n or 'MyRv' in n]
    print("Interesting assemblies:")
    for name in sorted(set(interesting)):
        print(f"  - {name}")
    
    # Find XABA header
    xaba_offset = find_xaba_header(data)
    if xaba_offset:
        print(f"\nXABA header at offset 0x{xaba_offset:x}")
        version = struct.unpack('<HH', data[xaba_offset+4:xaba_offset+8])
        entry_count = struct.unpack('<I', data[xaba_offset+8:xaba_offset+12])[0]
        index_count = struct.unpack('<I', data[xaba_offset+12:xaba_offset+16])[0]
        print(f"Version: {version}, Entries: {entry_count}, Index entries: {index_count}")
    
    # Find all XALZ blocks
    xalz_blocks = find_all_xalz(data)
    print(f"\nFound {len(xalz_blocks)} XALZ compressed blocks")
    
    os.makedirs(output_dir, exist_ok=True)
    
    extracted = 0
    for i, offset in enumerate(xalz_blocks):
        idx, size, assembly = decompress_xalz(data, offset)
        
        # Try to find name before this offset
        name = f"assembly_{idx:04d}.dll"
        
        # Look backwards for a name
        for name_offset in sorted(names.keys(), reverse=True):
            if name_offset < offset:
                candidate = names[name_offset]
                if candidate.endswith('.dll') and 'resources' not in candidate.lower():
                    name = candidate
                    break
        
        if assembly:
            # Verify it's a valid PE
            if assembly[:2] == b'MZ':
                out_path = output_dir / name
                # Avoid overwriting
                if out_path.exists():
                    out_path = output_dir / f"assembly_{idx:04d}_{name}"
                with open(out_path, 'wb') as f:
                    f.write(assembly)
                extracted += 1
                if extracted <= 15 or any(x in name for x in ['OneControl', 'IDS', 'DotNetty']):
                    print(f"  [{idx:3d}] {name}: {len(assembly):,} bytes")
            else:
                print(f"  [{idx:3d}] {name}: NOT a valid PE (first bytes: {assembly[:4].hex()})")
        else:
            if size and size > 0:
                print(f"  [{idx:3d}] {name}: Could not decompress (size would be {size:,})")
    
    print(f"\nExtracted {extracted} assemblies to {output_dir}")
    
    # Also try raw binary search for uncompressed assemblies
    print("\n=== Searching for uncompressed MZ/PE headers ===")
    mz_positions = []
    offset = 0
    while True:
        pos = data.find(b'MZ', offset)
        if pos == -1:
            break
        # Check for PE signature
        if pos + 64 <= len(data):
            try:
                pe_off = struct.unpack('<I', data[pos+60:pos+64])[0]
                if pos + pe_off + 4 <= len(data) and data[pos+pe_off:pos+pe_off+4] == b'PE\x00\x00':
                    mz_positions.append(pos)
            except:
                pass
        offset = pos + 1
    
    print(f"Found {len(mz_positions)} raw PE headers")
    for pos in mz_positions[:5]:
        print(f"  PE at 0x{pos:x}")

if __name__ == '__main__':
    main()
