"""
COBS (Consistent Overhead Byte Stuffing) encoder/decoder.

COBS is a framing protocol that:
- Uses 0x00 as frame delimiters
- Encodes data to eliminate 0x00 bytes within the payload
- Adds minimal overhead

Used by Lippert OneControl for TCP communication on port 6969.

Lippert-specific parameters (from decompiled app):
- CobsEncoder(prependStartFrame=true, useCrc=true, frameByte=0, numDataBits=6)
- CRC-8 polynomial: 0x8C (reflected), initial value: 0x55

References:
- https://en.wikipedia.org/wiki/Consistent_Overhead_Byte_Stuffing
- Discovered in LippertConnect app decompilation (IDS.Portable.Common.COBS)
"""

from typing import Generator, List, Optional, Tuple


# CRC-8 lookup table for polynomial 0x8C (reflected)
# This is CRC-8/MAXIM (also known as CRC-8/1-Wire or DOW CRC)
CRC8_TABLE = [
    0x00, 0x5e, 0xbc, 0xe2, 0x61, 0x3f, 0xdd, 0x83, 0xc2, 0x9c, 0x7e, 0x20, 0xa3, 0xfd, 0x1f, 0x41,
    0x9d, 0xc3, 0x21, 0x7f, 0xfc, 0xa2, 0x40, 0x1e, 0x5f, 0x01, 0xe3, 0xbd, 0x3e, 0x60, 0x82, 0xdc,
    0x23, 0x7d, 0x9f, 0xc1, 0x42, 0x1c, 0xfe, 0xa0, 0xe1, 0xbf, 0x5d, 0x03, 0x80, 0xde, 0x3c, 0x62,
    0xbe, 0xe0, 0x02, 0x5c, 0xdf, 0x81, 0x63, 0x3d, 0x7c, 0x22, 0xc0, 0x9e, 0x1d, 0x43, 0xa1, 0xff,
    0x46, 0x18, 0xfa, 0xa4, 0x27, 0x79, 0x9b, 0xc5, 0x84, 0xda, 0x38, 0x66, 0xe5, 0xbb, 0x59, 0x07,
    0xdb, 0x85, 0x67, 0x39, 0xba, 0xe4, 0x06, 0x58, 0x19, 0x47, 0xa5, 0xfb, 0x78, 0x26, 0xc4, 0x9a,
    0x65, 0x3b, 0xd9, 0x87, 0x04, 0x5a, 0xb8, 0xe6, 0xa7, 0xf9, 0x1b, 0x45, 0xc6, 0x98, 0x7a, 0x24,
    0xf8, 0xa6, 0x44, 0x1a, 0x99, 0xc7, 0x25, 0x7b, 0x3a, 0x64, 0x86, 0xd8, 0x5b, 0x05, 0xe7, 0xb9,
    0x8c, 0xd2, 0x30, 0x6e, 0xed, 0xb3, 0x51, 0x0f, 0x4e, 0x10, 0xf2, 0xac, 0x2f, 0x71, 0x93, 0xcd,
    0x11, 0x4f, 0xad, 0xf3, 0x70, 0x2e, 0xcc, 0x92, 0xd3, 0x8d, 0x6f, 0x31, 0xb2, 0xec, 0x0e, 0x50,
    0xaf, 0xf1, 0x13, 0x4d, 0xce, 0x90, 0x72, 0x2c, 0x6d, 0x33, 0xd1, 0x8f, 0x0c, 0x52, 0xb0, 0xee,
    0x32, 0x6c, 0x8e, 0xd0, 0x53, 0x0d, 0xef, 0xb1, 0xf0, 0xae, 0x4c, 0x12, 0x91, 0xcf, 0x2d, 0x73,
    0xca, 0x94, 0x76, 0x28, 0xab, 0xf5, 0x17, 0x49, 0x08, 0x56, 0xb4, 0xea, 0x69, 0x37, 0xd5, 0x8b,
    0x57, 0x09, 0xeb, 0xb5, 0x36, 0x68, 0x8a, 0xd4, 0x95, 0xcb, 0x29, 0x77, 0xf4, 0xaa, 0x48, 0x16,
    0xe9, 0xb7, 0x55, 0x0b, 0x88, 0xd6, 0x34, 0x6a, 0x2b, 0x75, 0x97, 0xc9, 0x4a, 0x14, 0xf6, 0xa8,
    0x74, 0x2a, 0xc8, 0x96, 0x15, 0x4b, 0xa9, 0xf7, 0xb6, 0xe8, 0x0a, 0x54, 0xd7, 0x89, 0x6b, 0x35,
]

# Lippert COBS parameters
LIPPERT_CRC8_INIT = 0x55  # Initial CRC value
LIPPERT_FRAME_BYTE = 0x00  # Frame delimiter
LIPPERT_NUM_DATA_BITS = 6  # Number of data bits in code byte
LIPPERT_MAX_DATA_BYTES = (1 << LIPPERT_NUM_DATA_BITS) - 1  # 63 bytes max per block
LIPPERT_FRAME_BYTE_COUNT_LSB = 1 << LIPPERT_NUM_DATA_BITS  # 64
LIPPERT_MAX_COMPRESSED_FRAME_BYTES = 255 - LIPPERT_MAX_DATA_BYTES  # 192


def crc8(data: bytes, init: int = LIPPERT_CRC8_INIT) -> int:
    """
    Calculate CRC-8 checksum using Lippert's algorithm.
    
    Uses polynomial 0x8C (reflected) with initial value 0x55.
    This is CRC-8/MAXIM variant.
    
    Args:
        data: Bytes to checksum
        init: Initial CRC value (default: 0x55)
        
    Returns:
        CRC-8 checksum (0-255)
    """
    crc = init
    for byte in data:
        crc = CRC8_TABLE[(crc ^ byte) & 0xFF]
    return crc


def encode(data: bytes, use_crc: bool = True, prepend_start: bool = True) -> bytes:
    """
    Encode data using Lippert's COBS variant.
    
    Exact port of decompiled CobsEncoder from IDS.Portable.Common.COBS.
    
    Args:
        data: Raw bytes to encode
        use_crc: Append CRC-8 checksum (default: True)
        prepend_start: Prepend 0x00 start delimiter (default: True)
        
    Returns:
        COBS encoded bytes with frame delimiters
    """
    output = bytearray(382)  # Max output buffer size
    out_idx = 0
    
    if prepend_start:
        output[out_idx] = LIPPERT_FRAME_BYTE
        out_idx += 1
    
    if not data:
        output[out_idx] = LIPPERT_FRAME_BYTE
        out_idx += 1
        return bytes(output[:out_idx])
    
    # CRC state - initialized to 0x55
    crc = LIPPERT_CRC8_INIT
    
    source_idx = 0  # num3 in C#
    source_count = len(data)  # count in C#
    total_count = source_count + 1 if use_crc else source_count  # num4 in C#
    
    while source_idx < total_count:
        code_idx = out_idx  # num5 in C#
        block_len = 0  # num6 in C#
        output[out_idx] = 0xFF  # Placeholder
        out_idx += 1
        
        # Inner loop: copy non-frame bytes
        while source_idx < total_count:
            if source_idx < source_count:
                b = data[source_idx]
                if b == LIPPERT_FRAME_BYTE:
                    break
                # Update CRC with this byte
                crc = CRC8_TABLE[(crc ^ b) & 0xFF]
            else:
                # This is the CRC byte position
                b = crc
                if b == LIPPERT_FRAME_BYTE:
                    break
            
            source_idx += 1
            output[out_idx] = b
            out_idx += 1
            block_len += 1
            
            if block_len >= LIPPERT_MAX_DATA_BYTES:
                break
        
        # Handle consecutive frame bytes (zeros)
        while source_idx < total_count:
            # Get current byte (source or CRC)
            if source_idx < source_count:
                b = data[source_idx]
            else:
                b = crc
            
            if b != LIPPERT_FRAME_BYTE:
                break
            
            # Update CRC with zero byte (only for source bytes, not CRC itself)
            if source_idx < source_count:
                crc = CRC8_TABLE[(crc ^ LIPPERT_FRAME_BYTE) & 0xFF]
            
            source_idx += 1
            block_len += LIPPERT_FRAME_BYTE_COUNT_LSB
            
            if block_len >= LIPPERT_MAX_COMPRESSED_FRAME_BYTES:
                break
        
        # Write the code byte
        output[code_idx] = block_len
    
    # Append end frame byte
    output[out_idx] = LIPPERT_FRAME_BYTE
    out_idx += 1
    
    return bytes(output[:out_idx])


class CobsDecoder:
    """
    Streaming COBS decoder matching Lippert's implementation.
    
    Based on decompiled CobsDecoder from IDS.Portable.Common.COBS.
    """
    
    def __init__(self, use_crc: bool = True):
        self.use_crc = use_crc
        self._code_byte = 0
        self._output = bytearray()
        self._min_payload_size = 1 if use_crc else 0
    
    def decode_byte(self, b: int) -> Optional[bytes]:
        """
        Process a single byte and return decoded message if frame complete.
        
        Args:
            b: Input byte
            
        Returns:
            Decoded message if frame complete, None otherwise
            
        Raises:
            ValueError: If CRC mismatch
        """
        if b == LIPPERT_FRAME_BYTE:
            try:
                if self._code_byte != 0:
                    return None
                if len(self._output) <= self._min_payload_size:
                    return None
                
                if self.use_crc:
                    received_crc = self._output[-1]
                    payload = bytes(self._output[:-1])
                    calculated_crc = crc8(payload)
                    
                    if calculated_crc != received_crc:
                        raise ValueError(f"CRC mismatch: expected 0x{calculated_crc:02X}, got 0x{received_crc:02X}")
                    
                    return payload
                else:
                    return bytes(self._output)
            finally:
                self._code_byte = 0
                self._output.clear()
        
        if self._code_byte <= 0:
            self._code_byte = b & 0xFF
        else:
            self._code_byte -= 1
            self._output.append(b)
        
        # Check if we need to add implicit zeros
        if (self._code_byte & LIPPERT_MAX_DATA_BYTES) == 0:
            while self._code_byte > 0:
                self._output.append(LIPPERT_FRAME_BYTE)
                self._code_byte -= LIPPERT_FRAME_BYTE_COUNT_LSB
        
        return None
    
    def reset(self):
        """Reset decoder state."""
        self._code_byte = 0
        self._output.clear()


def decode(data: bytes, use_crc: bool = True) -> bytes:
    """
    Decode COBS encoded data (without frame delimiters).
    
    Args:
        data: COBS encoded bytes (without frame delimiters)
        use_crc: Verify and strip CRC-8 checksum (default: True)
        
    Returns:
        Decoded raw bytes
        
    Raises:
        ValueError: If data is invalid or CRC mismatch
    """
    decoder = CobsDecoder(use_crc=use_crc)
    
    # Feed all bytes
    for b in data:
        result = decoder.decode_byte(b)
        if result is not None:
            return result
    
    # Feed final frame byte to complete
    result = decoder.decode_byte(LIPPERT_FRAME_BYTE)
    if result is not None:
        return result
    
    raise ValueError("Incomplete COBS frame")


def frame(data: bytes, use_crc: bool = True) -> bytes:
    """
    Encode data and add frame delimiters.
    
    Args:
        data: Raw bytes to frame
        use_crc: Include CRC-8 checksum
        
    Returns:
        Framed data: 0x00 + COBS(data + CRC) + 0x00
    """
    return encode(data, use_crc=use_crc, prepend_start=True)


class StreamDecoder:
    """
    Streaming COBS decoder for processing TCP data.
    
    Usage:
        decoder = StreamDecoder()
        for chunk in tcp_stream:
            for message in decoder.feed(chunk):
                process_message(message)
    """
    
    def __init__(self, use_crc: bool = True):
        self._decoder = CobsDecoder(use_crc=use_crc)
    
    def feed(self, data: bytes) -> Generator[bytes, None, None]:
        """
        Feed data to the decoder and yield complete decoded messages.
        
        Args:
            data: Raw bytes from TCP stream
            
        Yields:
            Decoded messages (one per complete frame)
        """
        for byte in data:
            try:
                result = self._decoder.decode_byte(byte)
                if result is not None:
                    yield result
            except ValueError:
                # CRC error, skip this frame
                self._decoder.reset()
    
    def reset(self):
        """Reset decoder state."""
        self._decoder.reset()


def extract_frames(data: bytes, use_crc: bool = True) -> List[bytes]:
    """
    Extract all complete frames from raw TCP data.
    
    Args:
        data: Raw bytes possibly containing multiple frames
        use_crc: Verify CRC checksums
        
    Returns:
        List of decoded messages
    """
    decoder = StreamDecoder(use_crc=use_crc)
    return list(decoder.feed(data))


# Legacy CRC functions for compatibility

CRC32_TABLE = None

def _init_crc32_table():
    """Initialize CRC-32 lookup table (little-endian polynomial)."""
    global CRC32_TABLE
    if CRC32_TABLE is not None:
        return
    
    CRC32_TABLE = []
    poly = 0xEDB88320  # Reversed polynomial for LE
    for i in range(256):
        crc = i
        for _ in range(8):
            if crc & 1:
                crc = (crc >> 1) ^ poly
            else:
                crc >>= 1
        CRC32_TABLE.append(crc)


def crc32_le(data: bytes, init: int = 0xFFFFFFFF) -> int:
    """
    Calculate CRC-32 checksum (little-endian).
    
    Args:
        data: Bytes to checksum
        init: Initial CRC value
        
    Returns:
        CRC-32 checksum
    """
    _init_crc32_table()
    crc = init
    for byte in data:
        crc = CRC32_TABLE[(crc ^ byte) & 0xFF] ^ (crc >> 8)
    return crc ^ 0xFFFFFFFF


if __name__ == '__main__':
    print("Lippert COBS/CRC Implementation Tests")
    print("=" * 50)
    
    # Test CRC-8
    print("\nCRC-8 Tests (poly=0x8C reflected, init=0x55):")
    test_data = bytes([0x01, 0x02, 0x03])
    crc = crc8(test_data)
    print(f"  CRC of [01 02 03]: 0x{crc:02X}")
    
    # Test with empty
    crc_empty = crc8(b'')
    print(f"  CRC of []: 0x{crc_empty:02X} (should be 0x55)")
    
    # Test COBS encoding/decoding
    print("\nCOBS Encode/Decode Tests:")
    
    test_cases = [
        b'\x01\x02\x03',
        b'\x00',
        b'\x00\x00\x00',
        b'Hello',
        b'Hello\x00World',
        bytes(range(10)),
        bytes(range(100)),  # Longer test
    ]
    
    all_passed = True
    for original in test_cases:
        try:
            encoded = encode(original, use_crc=True)
            # Strip leading frame byte for decoding
            encoded_inner = encoded[1:] if encoded[0] == 0 else encoded
            decoded = decode(encoded_inner, use_crc=True)
            
            status = "✓" if decoded == original else "✗"
            if decoded != original:
                all_passed = False
            print(f"  {status} Len={len(original):3d}: {original[:10].hex()}{'...' if len(original) > 10 else ''}")
        except Exception as e:
            all_passed = False
            print(f"  ✗ Len={len(original):3d}: {original[:10].hex()}{'...' if len(original) > 10 else ''} -> Error: {e}")
    
    # Test frame extraction
    print("\nFrame Extraction Test:")
    frame1 = encode(b'\x01\x02\x03', use_crc=True)
    frame2 = encode(b'\x04\x05\x06', use_crc=True)
    combined = frame1 + frame2
    print(f"  Combined frames: {combined.hex()}")
    
    extracted = extract_frames(combined, use_crc=True)
    print(f"  Extracted {len(extracted)} frames:")
    for i, msg in enumerate(extracted):
        print(f"    Frame {i+1}: {msg.hex()}")
    
    print("\n" + "=" * 50)
    print(f"All tests passed: {'✓' if all_passed else '✗'}")
