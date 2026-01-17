"""
Lippert OneControl proprietary TCP protocol decoder.

Based on reverse-engineering of TCP traffic on port 6969 between the
LippertConnect app and OneControl controller.

The protocol uses variable-length messages with different type markers.
"""

from dataclasses import dataclass, field
from typing import List, Dict, Any, Optional, Iterator, Tuple
from enum import IntEnum
import struct


class MessageType(IntEnum):
    """Observed message type markers in the Lippert protocol."""
    STATUS_SHORT = 0x40      # Short status (3-4 bytes after type)
    STATUS_8 = 0x41          # 8-byte status message
    COMMAND = 0x43           # Command/control message
    EXTENDED = 0x45          # Extended message format
    HEARTBEAT = 0x49         # Periodic heartbeat/sync
    SYSTEM = 0x85            # System-level message
    DEVICE_STATUS = 0xC3     # Device status report
    DEVICE_CONFIG = 0xC5     # Device configuration
    

@dataclass
class LippertMessage:
    """Decoded Lippert protocol message."""
    msg_type: int
    instance: int
    data: bytes
    offset: int  # Position in stream where this message was found
    decoded: Dict[str, Any] = field(default_factory=dict)
    
    @property
    def type_name(self) -> str:
        try:
            return MessageType(self.msg_type).name
        except ValueError:
            return f"UNKNOWN_0x{self.msg_type:02X}"
    
    def __repr__(self):
        return f"LippertMessage(type={self.type_name}, inst=0x{self.instance:02X}, data={self.data.hex()})"


class LippertProtocolDecoder:
    """
    Decoder for the Lippert OneControl proprietary TCP protocol.
    
    Usage:
        decoder = LippertProtocolDecoder()
        for message in decoder.decode_stream(raw_bytes):
            print(message)
    """
    
    # Known message type -> expected data length mappings
    # These are heuristic based on observed traffic
    TYPE_LENGTHS = {
        0x40: 3,   # Status short: type(1) + inst(1) + status(1) + pad(2)
        0x41: 8,   # Status 8-byte
        0x43: -1,  # Variable - need to parse
        0x45: -1,  # Variable
        0x49: 8,   # Heartbeat
        0x85: 6,   # System
        0xC3: 4,   # Device status
        0xC5: 6,   # Device config
    }
    
    # Message markers that indicate start of a message
    MSG_START_MARKERS = {0x40, 0x41, 0x43, 0x45, 0x49, 0x85, 0xC3, 0xC5, 0x05, 0x00}
    
    def __init__(self):
        self.buffer = bytearray()
        self.messages: List[LippertMessage] = []
    
    def decode_stream(self, data: bytes) -> Iterator[LippertMessage]:
        """
        Decode a stream of bytes into Lippert messages.
        
        This is a heuristic decoder - the exact protocol is still being
        reverse-engineered.
        """
        self.buffer.extend(data)
        
        offset = 0
        while offset < len(self.buffer):
            msg = self._try_decode_at(offset)
            if msg:
                yield msg
                offset += len(msg.data) + 2  # +2 for type and length/inst byte
            else:
                offset += 1
        
        # Keep unprocessed bytes in buffer
        if offset > 0:
            del self.buffer[:offset]
    
    def _try_decode_at(self, offset: int) -> Optional[LippertMessage]:
        """Try to decode a message starting at the given offset."""
        if offset >= len(self.buffer):
            return None
        
        data = self.buffer[offset:]
        
        # Check for length-prefixed frame: 0x00 <length> <data...>
        if data[0] == 0x00 and len(data) >= 2:
            length = data[1]
            if length > 0 and len(data) >= length + 2:
                payload = bytes(data[2:length + 2])
                # Parse the payload for sub-messages
                return self._parse_frame_payload(payload, offset)
        
        # Check for type-marker messages
        msg_type = data[0]
        if msg_type in self.MSG_START_MARKERS and len(data) >= 4:
            return self._decode_typed_message(data, offset)
        
        return None
    
    def _parse_frame_payload(self, payload: bytes, offset: int) -> Optional[LippertMessage]:
        """Parse the payload of a length-prefixed frame."""
        if len(payload) < 2:
            return None
        
        # The payload contains one or more sub-messages
        # For now, return the whole payload as one message
        return LippertMessage(
            msg_type=0x00,  # Frame wrapper
            instance=0,
            data=payload,
            offset=offset,
            decoded={"frame_type": "length_prefixed", "length": len(payload)}
        )
    
    def _decode_typed_message(self, data: bytes, offset: int) -> Optional[LippertMessage]:
        """Decode a message starting with a type marker."""
        msg_type = data[0]
        
        # Different message types have different formats
        if msg_type == 0x41 and len(data) >= 12:
            # 0x41 messages appear to be: 41 08 <node_hi> <node_lo> <6 bytes data>
            # Example: 4108c32802140408915db3
            instance = (data[2] << 8) | data[3]
            msg_data = bytes(data[1:12])
            
            decoded = self._decode_41_message(data[1:12])
            return LippertMessage(msg_type, instance, msg_data, offset, decoded)
        
        elif msg_type == 0x43 and len(data) >= 4:
            # 0x43 messages: 43 <len> <sub> <instance> <data...>
            # Examples: 430106, 430802
            sub_len = data[1]
            sub_type = data[2]
            
            if sub_len <= 8 and len(data) >= sub_len + 2:
                instance = data[3] if len(data) > 3 else 0
                msg_data = bytes(data[1:sub_len + 4])
                
                decoded = self._decode_43_message(data[1:sub_len + 4])
                return LippertMessage(msg_type, instance, msg_data, offset, decoded)
        
        elif msg_type == 0xC3 and len(data) >= 6:
            # 0xC3 messages: C3 04 01 <inst> 40 01 <status> 00 00
            instance = data[3]
            msg_data = bytes(data[1:8])
            
            decoded = {"format": "device_status", "instance": instance}
            return LippertMessage(msg_type, instance, msg_data, offset, decoded)
        
        elif msg_type == 0x40 and len(data) >= 5:
            # 0x40 short status
            instance = data[2]
            msg_data = bytes(data[1:5])
            
            return LippertMessage(msg_type, instance, msg_data, offset, {"format": "status_short"})
        
        return None
    
    def _decode_41_message(self, data: bytes) -> Dict[str, Any]:
        """Decode a 0x41 type message."""
        if len(data) < 10:
            return {"format": "status_8", "raw": data.hex()}
        
        # Format appears to be: 08 <node_hi> <node_lo> 02 14 04 08 <??> <??> <status?>
        node_id = (data[1] << 8) | data[2]
        
        return {
            "format": "status_8",
            "node_id": f"0x{node_id:04X}",
            "raw": data.hex()
        }
    
    def _decode_43_message(self, data: bytes) -> Dict[str, Any]:
        """Decode a 0x43 type message."""
        if len(data) < 3:
            return {"format": "command", "raw": data.hex()}
        
        sub_len = data[0]
        sub_type = data[1]
        
        decoded = {
            "format": "command",
            "sub_length": sub_len,
            "sub_type": f"0x{sub_type:02X}",
        }
        
        # Try to identify specific message subtypes
        if sub_type == 0x01:  # Appears to be simple on/off or status
            if len(data) >= 5:
                decoded["instance"] = data[2]
                decoded["value1"] = data[3]
                decoded["value2"] = data[4] if len(data) > 4 else 0
        elif sub_type == 0x02:  # More complex command
            if len(data) >= 6:
                decoded["instance"] = data[2]
                decoded["command"] = data[3]
                decoded["params"] = data[4:].hex()
        elif sub_type == 0x06:  # Observed in state messages
            if len(data) >= 5:
                decoded["instance"] = data[2]
                decoded["state"] = data[3]
        
        decoded["raw"] = data.hex()
        return decoded


def analyze_capture(raw_hex: str) -> List[Dict[str, Any]]:
    """
    Analyze a hex string capture and return structured findings.
    
    Args:
        raw_hex: Hex string from tcpdump or similar
        
    Returns:
        List of analysis results
    """
    data = bytes.fromhex(raw_hex.replace(' ', '').replace('\n', ''))
    
    results = []
    decoder = LippertProtocolDecoder()
    
    for msg in decoder.decode_stream(data):
        results.append({
            "offset": msg.offset,
            "type": msg.type_name,
            "type_byte": f"0x{msg.msg_type:02X}",
            "instance": f"0x{msg.instance:02X}",
            "data_hex": msg.data.hex(),
            "decoded": msg.decoded
        })
    
    return results


def find_device_instances(raw_hex: str) -> Dict[int, List[Dict]]:
    """
    Find all unique device instances in a capture.
    
    Returns dict mapping instance ID to list of messages for that instance.
    """
    data = bytes.fromhex(raw_hex.replace(' ', '').replace('\n', ''))
    
    instances: Dict[int, List[Dict]] = {}
    decoder = LippertProtocolDecoder()
    
    for msg in decoder.decode_stream(data):
        if msg.instance not in instances:
            instances[msg.instance] = []
        instances[msg.instance].append({
            "type": msg.type_name,
            "data": msg.data.hex(),
            "decoded": msg.decoded
        })
    
    return instances
