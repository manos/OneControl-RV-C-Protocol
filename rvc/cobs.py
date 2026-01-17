"""
COBS (Consistent Overhead Byte Stuffing) encoder/decoder.

COBS is a framing protocol that:
- Uses 0x00 as frame delimiters
- Encodes data to eliminate 0x00 bytes within the payload
- Adds minimal overhead (1 byte per 254 bytes max)

Used by Lippert OneControl for TCP communication on port 6969.

References:
- https://en.wikipedia.org/wiki/Consistent_Overhead_Byte_Stuffing
- Discovered in LippertConnect app decompilation (IDS.Portable.Common.COBS)
"""

from typing import Generator, List, Optional, Tuple


def encode(data: bytes) -> bytes:
    """
    Encode data using COBS.
    
    Args:
        data: Raw bytes to encode
        
    Returns:
        COBS encoded bytes (without frame delimiters)
    """
    if not data:
        return b'\x01'
    
    output = bytearray()
    code_index = 0
    code = 1
    output.append(0)  # Placeholder for first code byte
    
    for byte in data:
        if byte == 0:
            output[code_index] = code
            code_index = len(output)
            output.append(0)  # Placeholder for next code byte
            code = 1
        else:
            output.append(byte)
            code += 1
            if code == 0xFF:  # Max block length reached
                output[code_index] = code
                code_index = len(output)
                output.append(0)  # Placeholder for next code byte
                code = 1
    
    output[code_index] = code
    return bytes(output)


def decode(data: bytes) -> bytes:
    """
    Decode COBS encoded data.
    
    Args:
        data: COBS encoded bytes (without frame delimiters)
        
    Returns:
        Decoded raw bytes
        
    Raises:
        ValueError: If data is invalid COBS encoding
    """
    if not data:
        return b''
    
    output = bytearray()
    index = 0
    
    while index < len(data):
        code = data[index]
        index += 1
        
        if code == 0:
            raise ValueError(f"Invalid COBS: unexpected zero at position {index-1}")
        
        # Copy the next (code - 1) bytes
        for i in range(code - 1):
            if index >= len(data):
                raise ValueError(f"Invalid COBS: unexpected end of data at position {index}")
            output.append(data[index])
            index += 1
        
        # Add implicit zero if not at max block length and not at end
        if code < 0xFF and index < len(data):
            output.append(0)
    
    return bytes(output)


def frame(data: bytes) -> bytes:
    """
    Encode data and add frame delimiters.
    
    Args:
        data: Raw bytes to frame
        
    Returns:
        Framed data: 0x00 + COBS(data) + 0x00
    """
    return b'\x00' + encode(data) + b'\x00'


class StreamDecoder:
    """
    Streaming COBS decoder for processing TCP data.
    
    Usage:
        decoder = StreamDecoder()
        for chunk in tcp_stream:
            for message in decoder.feed(chunk):
                process_message(message)
    """
    
    def __init__(self):
        self.buffer = bytearray()
        self.in_frame = False
    
    def feed(self, data: bytes) -> Generator[bytes, None, None]:
        """
        Feed data to the decoder and yield complete decoded messages.
        
        Args:
            data: Raw bytes from TCP stream
            
        Yields:
            Decoded messages (one per complete frame)
        """
        for byte in data:
            if byte == 0:
                if self.in_frame and self.buffer:
                    # End of frame - decode and yield
                    try:
                        decoded = decode(bytes(self.buffer))
                        if decoded:  # Don't yield empty frames
                            yield decoded
                    except ValueError as e:
                        # Invalid COBS, skip this frame
                        pass
                # Start new frame
                self.buffer.clear()
                self.in_frame = True
            elif self.in_frame:
                self.buffer.append(byte)
    
    def reset(self):
        """Reset decoder state."""
        self.buffer.clear()
        self.in_frame = False


def extract_frames(data: bytes) -> List[bytes]:
    """
    Extract all complete frames from raw TCP data.
    
    Args:
        data: Raw bytes possibly containing multiple frames
        
    Returns:
        List of decoded messages
    """
    decoder = StreamDecoder()
    return list(decoder.feed(data))


# CRC utilities (used with COBS frames)

CRC8_TABLE = None
CRC32_TABLE = None


def _init_crc8_table():
    """Initialize CRC-8 lookup table."""
    global CRC8_TABLE
    if CRC8_TABLE is not None:
        return
    
    CRC8_TABLE = []
    for i in range(256):
        crc = i
        for _ in range(8):
            if crc & 0x80:
                crc = (crc << 1) ^ 0x07  # Polynomial x^8 + x^2 + x + 1
            else:
                crc <<= 1
            crc &= 0xFF
        CRC8_TABLE.append(crc)


def crc8(data: bytes, init: int = 0x00) -> int:
    """
    Calculate CRC-8 checksum.
    
    Args:
        data: Bytes to checksum
        init: Initial CRC value
        
    Returns:
        CRC-8 checksum (0-255)
    """
    _init_crc8_table()
    crc = init
    for byte in data:
        crc = CRC8_TABLE[crc ^ byte]
    return crc


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
    Calculate CRC-32 checksum (little-endian, as used by Lippert).
    
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


def verify_crc(data: bytes, expected_crc: int, use_crc32: bool = True) -> bool:
    """
    Verify CRC of data.
    
    Args:
        data: Payload bytes
        expected_crc: Expected CRC value
        use_crc32: Use CRC-32 (True) or CRC-8 (False)
        
    Returns:
        True if CRC matches
    """
    if use_crc32:
        return crc32_le(data) == expected_crc
    else:
        return crc8(data) == expected_crc


if __name__ == '__main__':
    # Test COBS encoding/decoding
    test_cases = [
        b'',
        b'\x00',
        b'\x00\x00',
        b'Hello',
        b'Hello\x00World',
        b'\x01\x02\x03',
        bytes(range(256)),
    ]
    
    print("COBS Encoder/Decoder Tests")
    print("=" * 40)
    
    for i, original in enumerate(test_cases):
        encoded = encode(original)
        decoded = decode(encoded)
        
        status = "✓" if decoded == original else "✗"
        print(f"\nTest {i+1}: {status}")
        print(f"  Original: {original[:30]}{'...' if len(original) > 30 else ''} ({len(original)} bytes)")
        print(f"  Encoded:  {encoded[:30]}{'...' if len(encoded) > 30 else ''} ({len(encoded)} bytes)")
        print(f"  Decoded:  {decoded[:30]}{'...' if len(decoded) > 30 else ''} ({len(decoded)} bytes)")
        
        # Verify no zeros in encoded data
        if 0 in encoded:
            print(f"  ERROR: Zero byte found in encoded data!")
    
    print("\n" + "=" * 40)
    print("CRC Tests")
    print("=" * 40)
    
    test_data = b"123456789"
    print(f"\nData: {test_data}")
    print(f"CRC-8:  0x{crc8(test_data):02X}")
    print(f"CRC-32: 0x{crc32_le(test_data):08X}")
