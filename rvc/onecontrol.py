"""
OneControl Client - Control Lippert OneControl RV devices

CRITICAL: Each control command requires a FRESH connection with authentication!

Usage:
    from rvc.onecontrol import control_light, KITCHEN, LIVING_ROOM, BED_CEILING
    
    # Turn on Kitchen
    await control_light(KITCHEN, on=True)
    
    # Turn off Bed Ceiling
    await control_light(BED_CEILING, on=False)
"""

import asyncio
import struct
from dataclasses import dataclass
from typing import Optional

# Controller address
DEFAULT_HOST = "192.168.1.1"
DEFAULT_PORT = 6969

# TEA cipher constant for REMOTE_CONTROL
REMOTE_CONTROL_CYPHER = 0xB16B00B5


@dataclass
class LightConfig:
    """Configuration for a specific light."""
    name: str
    protocol: int  # 0x80, 0x82, or 0x83
    session: int   # Registration session ID
    device: int    # Device ID (always 0x04 in practice)
    conn: int      # Connection ID
    counter: int   # Counter (stays fixed)
    
    @property
    def ctrl_conn(self) -> int:
        """Control connection = conn + 2"""
        return self.conn + 2


# Known light configurations (discovered through packet captures)
KITCHEN = LightConfig(
    name="Kitchen",
    protocol=0x82,
    session=0x82,
    device=0x04,
    conn=0x44,
    counter=0x28,
)

LIVING_ROOM = LightConfig(
    name="Living Room Ceiling",
    protocol=0x83,
    session=0xE3,
    device=0x04,
    conn=0x8C,
    counter=0x77,
)

BED_CEILING = LightConfig(
    name="Bed Ceiling",
    protocol=0x80,
    session=0x02,
    device=0x04,
    conn=0x08,
    counter=0xFB,
)

PORCH = LightConfig(
    name="Porch",
    protocol=0x83,
    session=0xE2,
    device=0x04,
    conn=0x88,
    counter=0xCF,
)


def crc8_maxim(data: bytes, init: int = 0x55) -> int:
    """CRC-8/MAXIM (poly=0x8C reflected, init=0x55)"""
    table = [0] * 256
    for i in range(256):
        c = i
        for _ in range(8):
            c = (c >> 1) ^ 0x8C if c & 1 else c >> 1
        table[i] = c
    crc = init
    for b in data:
        crc = table[(crc ^ b) & 0xFF]
    return crc


def cobs_encode(payload: bytes) -> bytes:
    """COBS encode with CRC-8"""
    crc = crc8_maxim(payload)
    data = bytes(payload) + bytes([crc])
    out = bytearray()
    i = 0
    while i < len(data):
        code_pos = len(out)
        out.append(0)
        count = 0
        while i < len(data) and data[i] != 0 and count < 63:
            out.append(data[i])
            i += 1
            count += 1
        zeros = 0
        while i < len(data) and data[i] == 0 and zeros < 3:
            i += 1
            zeros += 1
        out[code_pos] = count + (zeros * 64)
    return bytes([0x00]) + bytes(out) + bytes([0x00])


def cobs_decode_single(encoded: bytes) -> bytes:
    """Decode a single COBS block"""
    out = bytearray()
    i = 0
    while i < len(encoded):
        code = encoded[i]
        i += 1
        count = code & 63
        zeros = code >> 6
        for _ in range(count):
            if i < len(encoded):
                out.append(encoded[i])
                i += 1
        for _ in range(zeros):
            out.append(0x00)
    return bytes(out)


def decode_frames(data: bytes) -> list[bytes]:
    """Decode multiple COBS frames from data stream"""
    frames = []
    buf = bytearray()
    for b in data:
        if b == 0:
            if buf:
                try:
                    decoded = cobs_decode_single(bytes(buf))
                    if len(decoded) >= 2:
                        p = decoded[:-1]
                        crc = decoded[-1]
                        if crc == crc8_maxim(p):
                            frames.append(p)
                except Exception:
                    pass
                buf = bytearray()
        else:
            buf.append(b)
    return frames


def tea_encrypt(seed: int, cypher: int) -> int:
    """
    Modified TEA cipher for seed/key authentication.
    Constants spell "Copyright IDSsnc" in ASCII.
    """
    K1 = 0x436F7079  # "Copy"
    K2 = 0x72696768  # "righ"
    K3 = 0x74204944  # "t ID"
    K4 = 0x53736E63  # "Ssnc"
    DELTA = 0x9E3779B9
    
    v0, v1 = seed, cypher
    sum_val = DELTA
    
    for _ in range(32):
        v0 = (v0 + (((v1 << 4) + K1) ^ (v1 + sum_val) ^ ((v1 >> 5) + K2))) & 0xFFFFFFFF
        v1 = (v1 + (((v0 << 4) + K3) ^ (v0 + sum_val) ^ ((v0 >> 5) + K4))) & 0xFFFFFFFF
        sum_val = (sum_val + DELTA) & 0xFFFFFFFF
    
    return v0


async def control_light(
    light: LightConfig,
    on: bool,
    host: str = DEFAULT_HOST,
    port: int = DEFAULT_PORT,
) -> bool:
    """
    Control a light (ON or OFF).
    
    CRITICAL: This function creates a fresh connection and authenticates
    for EVERY control command. This is required by the protocol.
    
    Args:
        light: Light configuration (use KITCHEN, LIVING_ROOM, or BED_CEILING)
        on: True to turn on, False to turn off
        host: Controller IP address
        port: Controller port
        
    Returns:
        True if successful, False otherwise
    """
    reader: Optional[asyncio.StreamReader] = None
    writer: Optional[asyncio.StreamWriter] = None
    
    try:
        # Fresh connection for each command
        reader, writer = await asyncio.open_connection(host, port)
        
        # Clear initial data
        await asyncio.sleep(0.3)
        try:
            await asyncio.wait_for(reader.read(8192), timeout=0.3)
        except asyncio.TimeoutError:
            pass
        
        # Helper to send COBS frame
        async def send(payload: bytes):
            writer.write(cobs_encode(payload))
            await writer.drain()
        
        # Helper to receive frames
        async def recv(timeout: float = 0.5) -> list[bytes]:
            try:
                data = await asyncio.wait_for(reader.read(8192), timeout=timeout)
                return decode_frames(data)
            except asyncio.TimeoutError:
                return []
        
        # UUID for identity
        uuid = bytes([0x1c, 0x88, 0x43, 0x4f, 0xaf, 0x67, 0x82])
        
        # 1. Register
        await send(bytes([0x01, 0x06, light.session, 0x00]))
        await asyncio.sleep(0.1)
        
        # 2. Identity
        await send(bytes([0x08, 0x00, light.session, 0x00]) + uuid)
        await asyncio.sleep(0.2)
        await recv()
        
        # 3. Seed Request (ALWAYS authenticate before control!)
        await send(bytes([
            0x02, light.protocol, light.conn, light.counter,
            0x42, 0x00, light.device
        ]))
        
        # 4. Wait for seed
        # NOTE: Seed response always comes on Protocol 0x80, regardless of request protocol!
        seed = None
        for _ in range(10):
            await asyncio.sleep(0.3)
            frames = await recv()
            for f in frames:
                # Look for ANY seed response: 06 8x ... 42 00 [device] [seed]
                if len(f) >= 11 and f[0] == 0x06 and (f[1] & 0x80) and f[4] == 0x42:
                    seed = int.from_bytes(f[7:11], 'big')
                    break
            if seed:
                break
        
        if seed is None:
            return False
        
        # 5. Compute key
        key = tea_encrypt(seed, REMOTE_CONTROL_CYPHER)
        key_bytes = struct.pack('>I', key)
        
        # 6. Key Transmit
        await send(bytes([
            0x06, light.protocol, light.conn, light.counter,
            0x43, 0x00, light.device
        ]) + key_bytes)
        await asyncio.sleep(0.2)
        await recv()
        
        # 7. Control command
        value = 0x01 if on else 0x00
        await send(bytes([
            0x00, light.protocol, light.ctrl_conn, light.counter, value
        ]))
        
        await asyncio.sleep(0.3)
        return True
        
    except Exception as e:
        print(f"Error: {e}")
        return False
        
    finally:
        if writer:
            writer.close()
            await writer.wait_closed()


# Convenience functions
async def kitchen_on(host: str = DEFAULT_HOST) -> bool:
    """Turn on Kitchen light."""
    return await control_light(KITCHEN, on=True, host=host)


async def kitchen_off(host: str = DEFAULT_HOST) -> bool:
    """Turn off Kitchen light."""
    return await control_light(KITCHEN, on=False, host=host)


async def living_room_on(host: str = DEFAULT_HOST) -> bool:
    """Turn on Living Room Ceiling light."""
    return await control_light(LIVING_ROOM, on=True, host=host)


async def living_room_off(host: str = DEFAULT_HOST) -> bool:
    """Turn off Living Room Ceiling light."""
    return await control_light(LIVING_ROOM, on=False, host=host)


async def bed_ceiling_on(host: str = DEFAULT_HOST) -> bool:
    """Turn on Bed Ceiling light."""
    return await control_light(BED_CEILING, on=True, host=host)


async def bed_ceiling_off(host: str = DEFAULT_HOST) -> bool:
    """Turn off Bed Ceiling light."""
    return await control_light(BED_CEILING, on=False, host=host)


# =============================================================================
# TANK SENSORS - Read-only
# =============================================================================

@dataclass
class TankConfig:
    """Configuration for a tank sensor."""
    name: str
    counter: int
    
# Known tank sensors
TANK_GREY = TankConfig(name="Grey Tank", counter=0x04)
TANK_FRESH = TankConfig(name="Fresh Tank", counter=0x3E)
TANK_BLACK = TankConfig(name="Black Tank", counter=0x86)
TANK_LP = TankConfig(name="LP Tank", counter=0x10)

ALL_TANKS = [TANK_GREY, TANK_FRESH, TANK_BLACK, TANK_LP]


async def read_tank_levels(
    host: str = DEFAULT_HOST,
    port: int = DEFAULT_PORT,
    duration: float = 3.0
) -> dict[str, int]:
    """
    Read tank levels from controller broadcasts.
    
    Returns dict mapping tank name to level percentage (0-100).
    
    Tank levels are broadcast in 01 03 frames:
        Frame format: 01 03 [counter] [level%]
    
    Example:
        levels = await read_tank_levels()
        print(f"Fresh: {levels.get('Fresh Tank', 'N/A')}%")
        print(f"Black: {levels.get('Black Tank', 'N/A')}%")
    """
    reader = None
    writer = None
    
    try:
        reader, writer = await asyncio.open_connection(host, port)
        
        async def send(data: bytes):
            writer.write(cobs_encode(data))
            await writer.drain()
        
        async def recv() -> bytes:
            return await asyncio.wait_for(reader.read(8192), timeout=1.0)
        
        # Register
        session = 0x80
        uuid = bytes([0x1c, 0x88, 0x43, 0x4f, 0xaf, 0x67, 0x82])
        await send(bytes([0x01, 0x06, session, 0x00]))
        await asyncio.sleep(0.1)
        await send(bytes([0x08, 0x00, session, 0x00]) + uuid)
        
        # Build counter -> name mapping
        counter_to_name = {tank.counter: tank.name for tank in ALL_TANKS}
        
        # Collect 01 03 frames
        levels = {}
        start = asyncio.get_event_loop().time()
        
        while asyncio.get_event_loop().time() - start < duration:
            try:
                data = await asyncio.wait_for(reader.read(8192), timeout=0.5)
                frames = decode_frames(data)
                for f in frames:
                    # 01 03 frames contain tank levels: 01 03 [counter] [level]
                    if len(f) >= 4 and f[0] == 0x01 and f[1] == 0x03:
                        counter = f[2]
                        level = f[3]
                        if counter in counter_to_name:
                            levels[counter_to_name[counter]] = level
                
                # Exit early if we have all tanks
                if len(levels) >= len(ALL_TANKS):
                    break
                    
            except asyncio.TimeoutError:
                continue
        
        return levels
        
    except Exception as e:
        print(f"Error reading tanks: {e}")
        return {}
        
    finally:
        if writer:
            writer.close()
            await writer.wait_closed()


# =============================================================================
# BATTERY VOLTAGE - Read from Generator Genie
# =============================================================================

GENERATOR_GENIE_COUNTER = 0x87


async def read_battery_voltage(
    host: str = DEFAULT_HOST,
    port: int = DEFAULT_PORT,
    timeout: float = 3.0
) -> Optional[float]:
    """
    Read battery voltage from Generator Genie broadcasts.
    
    Returns voltage as float (e.g., 13.4), or None if not found.
    
    Battery voltage is broadcast in 05 03 frames from Generator Genie:
        Frame format: 05 03 87 [state] [voltage_hi] [voltage_lo] [temp_hi] [temp_lo]
        Voltage is 8.8 fixed point (high byte + low byte / 256)
    
    Example:
        voltage = await read_battery_voltage()
        if voltage:
            print(f"Battery: {voltage:.2f}V")
    """
    reader = None
    writer = None
    
    try:
        reader, writer = await asyncio.open_connection(host, port)
        
        async def send(data: bytes):
            writer.write(cobs_encode(data))
            await writer.drain()
        
        # Register
        session = 0x80
        uuid = bytes([0x1c, 0x88, 0x43, 0x4f, 0xaf, 0x67, 0x82])
        await send(bytes([0x01, 0x06, session, 0x00]))
        await asyncio.sleep(0.1)
        await send(bytes([0x08, 0x00, session, 0x00]) + uuid)
        
        # Look for 05 03 87 frame (Generator Genie status)
        start = asyncio.get_event_loop().time()
        
        while asyncio.get_event_loop().time() - start < timeout:
            try:
                data = await asyncio.wait_for(reader.read(8192), timeout=0.5)
                frames = decode_frames(data)
                for f in frames:
                    # Generator Genie status: 05 03 87 [state] [volt_hi] [volt_lo] ...
                    if (len(f) >= 6 and f[0] == 0x05 and f[1] == 0x03 
                        and f[2] == GENERATOR_GENIE_COUNTER):
                        # Parse voltage as 8.8 fixed point
                        voltage = f[4] + f[5] / 256.0
                        return voltage
                        
            except asyncio.TimeoutError:
                continue
        
        return None
        
    except Exception as e:
        print(f"Error reading battery voltage: {e}")
        return None
        
    finally:
        if writer:
            writer.close()
            await writer.wait_closed()


# =============================================================================
# GENERATOR HOUR METER - Read-only
# =============================================================================

GENERATOR_HOUR_METER_COUNTER = 0x80


async def read_generator_hours(
    host: str = DEFAULT_HOST,
    port: int = DEFAULT_PORT,
    timeout: float = 3.0
) -> Optional[float]:
    """
    Read generator operating hours from controller broadcasts.
    
    Returns hours as float, or None if not found.
    
    Generator hours are broadcast in 05 03 frames:
        Frame format: 05 03 [counter] [uint32 BE seconds] [status]
    
    Example:
        hours = await read_generator_hours()
        print(f"Generator: {hours:.1f} hours")
    """
    reader = None
    writer = None
    
    try:
        reader, writer = await asyncio.open_connection(host, port)
        
        async def send(data: bytes):
            writer.write(cobs_encode(data))
            await writer.drain()
        
        # Register
        session = 0x80
        uuid = bytes([0x1c, 0x88, 0x43, 0x4f, 0xaf, 0x67, 0x82])
        await send(bytes([0x01, 0x06, session, 0x00]))
        await asyncio.sleep(0.1)
        await send(bytes([0x08, 0x00, session, 0x00]) + uuid)
        
        # Look for 05 03 frame with generator counter
        start = asyncio.get_event_loop().time()
        
        while asyncio.get_event_loop().time() - start < timeout:
            try:
                data = await asyncio.wait_for(reader.read(8192), timeout=0.5)
                frames = decode_frames(data)
                for f in frames:
                    # 05 03 frames: 05 03 [counter] [uint32 BE seconds] [status]
                    if (len(f) >= 8 and f[0] == 0x05 and f[1] == 0x03 
                        and f[2] == GENERATOR_HOUR_METER_COUNTER):
                        # Parse operating seconds as big-endian uint32
                        operating_seconds = int.from_bytes(f[3:7], 'big')
                        hours = operating_seconds / 3600.0
                        return hours
                        
            except asyncio.TimeoutError:
                continue
        
        return None
        
    except Exception as e:
        print(f"Error reading generator hours: {e}")
        return None
        
    finally:
        if writer:
            writer.close()
            await writer.wait_closed()


# =============================================================================
# GENERATOR CONTROL - ON/OFF
# =============================================================================

GENERATOR_COUNTER = 0x87
GENERATOR_PROTOCOL = 0x81
GENERATOR_CONN = 0xe8


async def control_generator(
    on: bool,
    host: str = DEFAULT_HOST,
    port: int = DEFAULT_PORT
) -> bool:
    """
    Control the generator (ON/OFF).
    
    IMPORTANT: Generator uses a DIFFERENT protocol than lights!
    - Protocol: 0x81 (not 0x80)
    - Control frame type: 0x01 (not 0x00)
    - ON command: 0x02
    - OFF command: 0x01
    
    Args:
        on: True to start, False to stop
        host: Controller IP (default 192.168.1.1)
        port: Controller port (default 6969)
        
    Returns:
        True if command was sent successfully
        
    Example:
        # Start generator
        await control_generator(on=True)
        
        # Stop generator
        await control_generator(on=False)
    """
    reader = None
    writer = None
    
    try:
        reader, writer = await asyncio.open_connection(host, port)
        
        async def send(data: bytes):
            writer.write(cobs_encode(data))
            await writer.drain()
        
        async def recv(timeout: float = 0.5) -> list:
            try:
                data = await asyncio.wait_for(reader.read(8192), timeout=timeout)
                return decode_frames(data)
            except asyncio.TimeoutError:
                return []
        
        # 1. Register
        session = 0x7a
        uuid = bytes([0x1c, 0x88, 0x43, 0x4f, 0xaf, 0x67, 0x82])
        await send(bytes([0x01, 0x06, session, 0x00]))
        await asyncio.sleep(0.1)
        await send(bytes([0x08, 0x00, session, 0x00]) + uuid)
        await asyncio.sleep(0.3)
        await recv(0.3)
        
        # 2. Seed request - Protocol 0x81, device type 42 00 04
        await send(bytes([
            0x02, GENERATOR_PROTOCOL, GENERATOR_CONN, GENERATOR_COUNTER,
            0x42, 0x00, 0x04
        ]))
        
        # 3. Wait for seed (comes on protocol 0x82)
        seed = None
        for _ in range(10):
            await asyncio.sleep(0.3)
            frames = await recv()
            for f in frames:
                # Look for 06 82 ... 42 00 04 [seed]
                if len(f) >= 11 and f[0] == 0x06 and f[1] == 0x82 and f[4] == 0x42:
                    seed = int.from_bytes(f[7:11], 'big')
                    break
            if seed:
                break
        
        if seed is None:
            print("Generator: No seed received")
            return False
        
        # 4. Compute key
        key = tea_encrypt(seed, REMOTE_CONTROL_CYPHER)
        key_bytes = struct.pack('>I', key)
        
        # 5. Key transmit - device type 43 00 04
        await send(bytes([
            0x06, GENERATOR_PROTOCOL, GENERATOR_CONN, GENERATOR_COUNTER,
            0x43, 0x00, 0x04
        ]) + key_bytes)
        await asyncio.sleep(0.2)
        await recv()
        
        # 6. Control command - Frame type 0x01 (NOT 0x00!)
        # Commands: 0x02 = ON, 0x01 = OFF
        cmd = 0x02 if on else 0x01
        ctrl_conn = GENERATOR_CONN + 2
        await send(bytes([
            0x01, GENERATOR_PROTOCOL, ctrl_conn, GENERATOR_COUNTER,
            0x00, cmd
        ]))
        await asyncio.sleep(0.3)
        await recv()
        
        return True
        
    except Exception as e:
        print(f"Generator control error: {e}")
        return False
        
    finally:
        if writer:
            writer.close()
            await writer.wait_closed()


async def generator_on(host: str = DEFAULT_HOST) -> bool:
    """Start the generator."""
    return await control_generator(on=True, host=host)


async def generator_off(host: str = DEFAULT_HOST) -> bool:
    """Stop the generator."""
    return await control_generator(on=False, host=host)
