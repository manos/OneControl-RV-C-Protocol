"""
OneControl command building.

Based on decompiled LippertConnect app (OneControl.Direct.MyRvLink namespace).

Command Structure (MyRvLinkCommand):
  [ClientCommandId (2 bytes, LE)] [CommandType (1 byte)] [Payload...]

For ActionDimmable (CommandType=67):
  [ClientCommandId (2 bytes, LE)] [67] [DeviceTableId] [DeviceId] [LightCommand...]

LightCommand (LogicalDeviceLightDimmableCommand, 8 bytes):
  [Command] [MaxBrightness] [Duration] [CycleTime1 MSB] [CycleTime1 LSB] 
  [CycleTime2 MSB] [CycleTime2 LSB] [Undefined]

DimmableLightCommand enum:
  0 = Off
  1 = On
  2 = Blink
  3 = Swell
  4 = Settings
  127 = Restore (turn on to last brightness)

The entire command is then COBS encoded with CRC-8 before sending.
"""

from dataclasses import dataclass
from typing import Optional
from enum import IntEnum
import struct

from . import cobs


class MyRvLinkCommandType(IntEnum):
    """Command types from MyRvLinkCommandType enum."""
    UNKNOWN = 0
    GET_DEVICES = 1
    GET_DEVICES_METADATA = 2
    REMOVE_OFFLINE_DEVICES = 3
    RENAME_DEVICE = 4
    SET_REAL_TIME_CLOCK = 5
    GET_PRODUCT_DTC_VALUES = 16
    GET_DEVICE_PID_LIST = 17
    GET_DEVICE_PID = 18
    SET_DEVICE_PID = 19
    GET_DEVICE_PID_WITH_ADDRESS = 20
    SET_DEVICE_PID_WITH_ADDRESS = 21
    GET_DEVICE_BLOCK_LIST = 48
    GET_DEVICE_BLOCK_PROPERTIES = 49
    START_DEVICE_BLOCK_TRANSFER = 50
    DEVICE_BLOCK_WRITE_DATA = 51
    STOP_DEVICE_BLOCK_TRANSFER = 52
    ACTION_SWITCH = 64
    ACTION_MOVEMENT = 65
    ACTION_GENERATOR_GENIE = 66
    ACTION_DIMMABLE = 67
    ACTION_RGB = 68
    ACTION_HVAC = 69
    ACTION_ACCESSORY_GATEWAY = 70
    LEVELER4_BUTTON_COMMAND = 80
    LEVELER5_COMMAND = 81
    LEVELER1_BUTTON_COMMAND = 82
    LEVELER3_BUTTON_COMMAND = 83
    GET_FIRMWARE_INFORMATION = 96
    DIAGNOSTICS = 102
    INVALID = 255


class DimmableLightCommand(IntEnum):
    """Light command values from DimmableLightCommand enum."""
    OFF = 0
    ON = 1
    BLINK = 2
    SWELL = 3
    SETTINGS = 4
    RESTORE = 127  # Turn on to last brightness


# Default cycle time for blink/swell modes
DEFAULT_CYCLE_TIME = 220


# Global command ID counter (wraps at 65535)
_command_id_counter = 0


def _next_command_id() -> int:
    """Get next command ID (incrementing counter)."""
    global _command_id_counter
    cmd_id = _command_id_counter
    _command_id_counter = (_command_id_counter + 1) & 0xFFFF
    return cmd_id


@dataclass
class Command:
    """Base command class."""
    raw_data: bytes  # Unencoded command data
    
    def encode(self) -> bytes:
        """Return the COBS+CRC encoded command ready to send."""
        return cobs.encode(self.raw_data, use_crc=True, prepend_start=True)
    
    def __repr__(self):
        return f"{self.__class__.__name__}(raw={self.raw_data.hex()}, encoded={self.encode().hex()})"


def build_light_command(
    device_table_id: int,
    device_id: int,
    command: DimmableLightCommand,
    max_brightness: int = 0,
    duration: int = 0,
    cycle_time1: int = 0,
    cycle_time2: int = 0,
    command_id: Optional[int] = None
) -> Command:
    """
    Build a dimmable light command.
    
    Args:
        device_table_id: Device table ID (identifies the device type/group)
        device_id: Device ID within the table
        command: Light command (OFF, ON, RESTORE, etc.)
        max_brightness: Brightness level 0-255 (for ON/BLINK/SWELL)
        duration: Duration parameter
        cycle_time1: Cycle time 1 for blink/swell (default 220 if 0)
        cycle_time2: Cycle time 2 for blink/swell (default 220 if 0)
        command_id: Optional command ID (auto-generated if None)
        
    Returns:
        Command object ready to encode and send
    """
    if command_id is None:
        command_id = _next_command_id()
    
    # For blink/swell, set default cycle times if not specified
    if command in (DimmableLightCommand.BLINK, DimmableLightCommand.SWELL):
        if cycle_time1 == 0 or cycle_time2 == 0:
            cycle_time1 = cycle_time2 = DEFAULT_CYCLE_TIME
    
    # Build the light command data (8 bytes)
    light_cmd = bytes([
        command,                          # Byte 0: Command
        max_brightness,                   # Byte 1: Max brightness
        duration,                         # Byte 2: Duration
        (cycle_time1 >> 8) & 0xFF,       # Byte 3: CycleTime1 MSB
        cycle_time1 & 0xFF,              # Byte 4: CycleTime1 LSB
        (cycle_time2 >> 8) & 0xFF,       # Byte 5: CycleTime2 MSB
        cycle_time2 & 0xFF,              # Byte 6: CycleTime2 LSB
        0,                                # Byte 7: Undefined
    ])
    
    # Build full command: [CmdId LE][Type][TableId][DeviceId][LightCmd]
    raw_data = struct.pack('<H', command_id)  # Little-endian command ID
    raw_data += bytes([
        MyRvLinkCommandType.ACTION_DIMMABLE,
        device_table_id,
        device_id,
    ])
    raw_data += light_cmd
    
    return Command(raw_data)


def build_light_on(device_table_id: int, device_id: int, brightness: int = 200) -> Command:
    """
    Build command to turn light ON.
    
    Args:
        device_table_id: Device table ID
        device_id: Device ID
        brightness: Brightness level 0-255 (default 200 = ~78%)
    """
    return build_light_command(
        device_table_id, device_id,
        DimmableLightCommand.ON,
        max_brightness=brightness
    )


def build_light_off(device_table_id: int, device_id: int) -> Command:
    """Build command to turn light OFF."""
    return build_light_command(
        device_table_id, device_id,
        DimmableLightCommand.OFF
    )


def build_light_restore(device_table_id: int, device_id: int) -> Command:
    """Build command to restore light to last brightness."""
    return build_light_command(
        device_table_id, device_id,
        DimmableLightCommand.RESTORE
    )


def build_light_settings(device_table_id: int, device_id: int, 
                         max_brightness: int, duration: int) -> Command:
    """Build command to set light settings."""
    return build_light_command(
        device_table_id, device_id,
        DimmableLightCommand.SETTINGS,
        max_brightness=max_brightness,
        duration=duration
    )


def build_get_devices(command_id: Optional[int] = None) -> Command:
    """Build command to get list of devices."""
    if command_id is None:
        command_id = _next_command_id()
    
    raw_data = struct.pack('<H', command_id)
    raw_data += bytes([MyRvLinkCommandType.GET_DEVICES])
    
    return Command(raw_data)


def build_get_devices_metadata(command_id: Optional[int] = None) -> Command:
    """Build command to get device metadata."""
    if command_id is None:
        command_id = _next_command_id()
    
    raw_data = struct.pack('<H', command_id)
    raw_data += bytes([MyRvLinkCommandType.GET_DEVICES_METADATA])
    
    return Command(raw_data)


# Convenience class for working with discovered devices
@dataclass
class LightDevice:
    """Represents a discovered light device."""
    name: str
    table_id: int
    device_id: int
    
    def on(self, brightness: int = 200) -> Command:
        """Turn light ON."""
        return build_light_on(self.table_id, self.device_id, brightness)
    
    def off(self) -> Command:
        """Turn light OFF."""
        return build_light_off(self.table_id, self.device_id)
    
    def restore(self) -> Command:
        """Restore to last brightness."""
        return build_light_restore(self.table_id, self.device_id)
    
    def set_brightness(self, brightness: int) -> Command:
        """Set specific brightness (0-255)."""
        return build_light_on(self.table_id, self.device_id, brightness)


# Known devices (to be populated from device discovery)
# These are placeholders - actual IDs need to be discovered from the device
KITCHEN_LIGHT = LightDevice("Kitchen", table_id=0x00, device_id=0x21)
BED_CEILING_LIGHT = LightDevice("Bed Ceiling", table_id=0x00, device_id=0x28)


if __name__ == '__main__':
    print("OneControl Command Builder")
    print("=" * 50)
    
    # Test command building
    commands = [
        ("Light ON (table=0, id=0x21)", build_light_on(0, 0x21, 200)),
        ("Light OFF (table=0, id=0x21)", build_light_off(0, 0x21)),
        ("Light RESTORE (table=0, id=0x21)", build_light_restore(0, 0x21)),
        ("Get Devices", build_get_devices()),
        ("Get Metadata", build_get_devices_metadata()),
    ]
    
    for name, cmd in commands:
        print(f"\n{name}:")
        print(f"  Raw:     {cmd.raw_data.hex()}")
        print(f"  Encoded: {cmd.encode().hex()}")
    
    print("\n" + "=" * 50)
    print("\nUsing LightDevice convenience class:")
    
    print(f"\nKitchen ON:  {KITCHEN_LIGHT.on().encode().hex()}")
    print(f"Kitchen OFF: {KITCHEN_LIGHT.off().encode().hex()}")
    print(f"Kitchen 50%: {KITCHEN_LIGHT.set_brightness(128).encode().hex()}")
