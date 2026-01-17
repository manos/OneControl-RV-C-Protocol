"""
TCP Client for Lippert OneControl systems.

Connects to the OneControl controller via TCP port 6969 and handles
the binary protocol that wraps RV-C messages.
"""

import asyncio
import logging
import struct
from dataclasses import dataclass
from typing import Optional, Callable, Awaitable, List
from .decoder import RVCDecoder, RVCMessage
from .dgn import DGN, DCDimmerCommand

logger = logging.getLogger(__name__)


@dataclass
class OneControlFrame:
    """Raw frame from OneControl TCP stream"""
    frame_type: int
    payload: bytes
    
    def __repr__(self):
        return f"OneControlFrame(type=0x{self.frame_type:02X}, len={len(self.payload)}, data={self.payload.hex()})"


class OneControlClient:
    """
    Async TCP client for Lippert OneControl RV systems.
    
    The protocol appears to wrap RV-C CAN frames in a custom TCP framing
    format. Based on packet captures, the format seems to be:
    
    [length][type][payload...]
    
    Where the payload contains one or more RV-C CAN frames.
    
    Usage:
        async with OneControlClient("192.168.1.1") as client:
            async for message in client.messages():
                print(message)
    """
    
    DEFAULT_PORT = 6969
    
    def __init__(
        self, 
        host: str, 
        port: int = DEFAULT_PORT,
        message_callback: Optional[Callable[[RVCMessage], Awaitable[None]]] = None
    ):
        self.host = host
        self.port = port
        self.message_callback = message_callback
        self.decoder = RVCDecoder()
        
        self._reader: Optional[asyncio.StreamReader] = None
        self._writer: Optional[asyncio.StreamWriter] = None
        self._running = False
        self._buffer = bytearray()
    
    async def connect(self):
        """Establish connection to OneControl."""
        logger.info(f"Connecting to OneControl at {self.host}:{self.port}")
        self._reader, self._writer = await asyncio.open_connection(
            self.host, self.port
        )
        self._running = True
        logger.info("Connected to OneControl")
    
    async def disconnect(self):
        """Close connection."""
        self._running = False
        if self._writer:
            self._writer.close()
            await self._writer.wait_closed()
        self._reader = None
        self._writer = None
        logger.info("Disconnected from OneControl")
    
    async def __aenter__(self):
        await self.connect()
        return self
    
    async def __aexit__(self, exc_type, exc_val, exc_tb):
        await self.disconnect()
    
    def _try_parse_frame(self) -> Optional[OneControlFrame]:
        """
        Try to parse a frame from the buffer.
        
        Based on tcpdump analysis, the protocol appears to use variable-length
        framing. This is a preliminary parser - needs refinement based on
        more traffic analysis.
        
        Observed patterns from captures:
        - Messages often start with 0x00 followed by length
        - Some messages have 0x40, 0x41, 0x43 type markers
        - CAN IDs appear to be embedded in the payload
        
        TODO: Refine this based on more protocol analysis
        """
        if len(self._buffer) < 2:
            return None
        
        # Look for frame boundaries
        # This is a heuristic based on observed traffic - needs refinement
        
        # Try to find message start marker
        # Common observed patterns: 0x00 XX (length), 0x40/0x41/0x43 type markers
        
        if self._buffer[0] == 0x00:
            # Appears to be length-prefixed
            if len(self._buffer) < 2:
                return None
            length = self._buffer[1]
            if len(self._buffer) < length + 2:
                return None
            frame_data = bytes(self._buffer[2:length + 2])
            del self._buffer[:length + 2]
            return OneControlFrame(frame_type=0x00, payload=frame_data)
        
        elif self._buffer[0] in (0x40, 0x41, 0x43):
            # Type marker followed by data
            frame_type = self._buffer[0]
            
            # Try to find next frame boundary or use fixed size
            # This is heuristic - actual protocol may differ
            if len(self._buffer) >= 8:
                frame_data = bytes(self._buffer[1:8])
                del self._buffer[:8]
                return OneControlFrame(frame_type=frame_type, payload=frame_data)
        
        else:
            # Unknown format - skip byte and try again
            logger.debug(f"Skipping unknown byte: 0x{self._buffer[0]:02X}")
            del self._buffer[:1]
        
        return None
    
    def _extract_rvc_messages(self, frame: OneControlFrame) -> List[RVCMessage]:
        """
        Extract RV-C messages from a OneControl frame.
        
        The exact mapping from TCP frames to CAN messages is still being
        reverse-engineered. This is a preliminary implementation.
        
        Observed in captures:
        - CAN IDs appear to be 4 bytes (29-bit extended ID)
        - Data is 8 bytes
        - Multiple messages may be packed together
        """
        messages = []
        
        # Try to interpret payload as CAN frames
        # Standard CAN frame: 4 byte ID + 8 byte data = 12 bytes
        payload = frame.payload
        
        if len(payload) >= 12:
            # Try parsing as [4-byte CAN ID][8-byte data]
            offset = 0
            while offset + 12 <= len(payload):
                try:
                    # CAN ID (little-endian or big-endian - need to determine)
                    canid_le = struct.unpack("<I", payload[offset:offset+4])[0]
                    canid_be = struct.unpack(">I", payload[offset:offset+4])[0]
                    data = payload[offset+4:offset+12]
                    
                    # Try both endianness and see which gives valid DGN
                    for canid in [canid_be, canid_le]:
                        dgn, src, pri = self.decoder.extract_dgn_from_canid(canid)
                        try:
                            DGN(dgn)  # Check if it's a known DGN
                            msg = self.decoder.decode(canid, data)
                            messages.append(msg)
                            break
                        except ValueError:
                            continue
                    
                    offset += 12
                except Exception as e:
                    logger.debug(f"Failed to parse CAN frame at offset {offset}: {e}")
                    break
        
        return messages
    
    async def read_raw(self) -> Optional[bytes]:
        """Read raw data from the connection."""
        if not self._reader:
            return None
        
        try:
            data = await self._reader.read(1024)
            if not data:
                logger.warning("Connection closed by remote")
                self._running = False
                return None
            return data
        except Exception as e:
            logger.error(f"Read error: {e}")
            self._running = False
            return None
    
    async def messages(self):
        """
        Async generator that yields decoded RV-C messages.
        
        Usage:
            async for message in client.messages():
                print(message)
        """
        while self._running:
            data = await self.read_raw()
            if data is None:
                break
            
            self._buffer.extend(data)
            logger.debug(f"Buffer: {len(self._buffer)} bytes, raw: {data.hex()}")
            
            while True:
                frame = self._try_parse_frame()
                if frame is None:
                    break
                
                logger.debug(f"Frame: {frame}")
                
                messages = self._extract_rvc_messages(frame)
                for msg in messages:
                    if self.message_callback:
                        await self.message_callback(msg)
                    yield msg
    
    async def send_raw(self, data: bytes):
        """Send raw bytes to OneControl."""
        if not self._writer:
            raise RuntimeError("Not connected")
        self._writer.write(data)
        await self._writer.drain()
    
    def _build_dimmer_command(
        self,
        instance: int,
        brightness: int,
        command: DCDimmerCommand,
        group: int = 0xFF,
        duration: int = 0xFF,
        interlock: int = 0
    ) -> bytes:
        """
        Build a DC_DIMMER_COMMAND_2 message.
        
        Returns raw CAN frame bytes (ID + data).
        
        TODO: This needs the proper TCP framing wrapper
        """
        canid = self.decoder.make_canid(DGN.DC_DIMMER_COMMAND_2)
        
        data = bytes([
            instance,
            group,
            brightness,
            command,
            duration,
            interlock,
            0xFF,  # Reserved
            0xFF,  # Reserved
        ])
        
        return struct.pack(">I", canid) + data
    
    async def set_light(self, instance: int, on: bool, brightness: int = 200):
        """
        Turn a light on or off.
        
        Args:
            instance: Light instance number
            on: True to turn on, False to turn off
            brightness: Brightness level 0-200 (only used when on=True)
        """
        cmd = DCDimmerCommand.ON_DURATION if on else DCDimmerCommand.OFF
        bright = brightness if on else 0
        
        # TODO: Wrap in proper TCP framing
        frame = self._build_dimmer_command(instance, bright, cmd)
        logger.info(f"Sending light command: instance={instance}, on={on}, brightness={brightness}")
        logger.debug(f"Raw frame: {frame.hex()}")
        
        # For now, just send raw - this may need framing
        await self.send_raw(frame)
    
    async def set_light_brightness(self, instance: int, brightness: int):
        """
        Set light brightness level.
        
        Args:
            instance: Light instance number
            brightness: Brightness level 0-200
        """
        if brightness == 0:
            await self.set_light(instance, False)
        else:
            frame = self._build_dimmer_command(
                instance, 
                brightness, 
                DCDimmerCommand.SET_BRIGHTNESS,
                duration=0  # Immediate
            )
            await self.send_raw(frame)


async def monitor_onecontrol(host: str, port: int = 6969, duration: float = 60):
    """
    Monitor OneControl traffic for a specified duration.
    
    Useful for protocol analysis and debugging.
    """
    logging.basicConfig(level=logging.DEBUG)
    
    print(f"Connecting to {host}:{port}...")
    
    async with OneControlClient(host, port) as client:
        print(f"Connected! Monitoring for {duration} seconds...")
        
        try:
            async with asyncio.timeout(duration):
                async for message in client.messages():
                    print(f"\n{message.dgn_name} (0x{message.dgn:05X}) from 0x{message.source_address:02X}")
                    for key, value in message.data.items():
                        print(f"  {key}: {value}")
        except asyncio.TimeoutError:
            print("\nMonitoring complete.")


if __name__ == "__main__":
    import sys
    
    host = sys.argv[1] if len(sys.argv) > 1 else "192.168.1.1"
    asyncio.run(monitor_onecontrol(host))
