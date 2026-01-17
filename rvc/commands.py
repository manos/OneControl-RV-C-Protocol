"""
OneControl command building.

Message format (discovered from LippertConnect decompilation):

Frame: [0x00] [Message] [0x00]

SetValue/Dimmer (0x45):
  [45] [02] [83 ac] [instance] [42] [02] [brightness]
  
  Where:
    - 45 = SetValue message type
    - 02 = Sub-type for dimmer/light
    - 83 ac = Magic bytes (little-endian 0xAC83)
    - instance = Device instance ID (e.g., 0x21 for Kitchen)
    - 42 = "set" command
    - 02 = Command modifier?
    - brightness = 0-100 (0x00-0x64), or omit for OFF

Known device instances (from tcpdump analysis):
    - 0x21 = Kitchen light
    - 0x28 = Bed Ceiling light
"""

from dataclasses import dataclass
from typing import Optional
from enum import IntEnum


class MessageType(IntEnum):
    """Known message types."""
    DEVICE_STATE = 0x40
    NODE_INFO = 0x41
    MULTI_MESSAGE = 0x43
    SET_VALUE = 0x45
    TOGGLE = 0x85
    STATUS_QUERY = 0xC5


class DeviceInstance(IntEnum):
    """Known device instances."""
    KITCHEN = 0x21
    BED_CEILING = 0x28
    # More to be discovered...


@dataclass
class Command:
    """Base command class."""
    data: bytes
    
    def frame(self) -> bytes:
        """Return the framed command (with 0x00 delimiters)."""
        return b'\x00' + self.data + b'\x00'
    
    def __repr__(self):
        return f"{self.__class__.__name__}(data={self.data.hex()})"


def build_dimmer_command(instance: int, brightness: int) -> Command:
    """
    Build a dimmer/light command.
    
    Args:
        instance: Device instance ID (e.g., 0x21 for Kitchen)
        brightness: Brightness level 0-100 (0=off, 100=full on)
        
    Returns:
        Command object ready to send
    """
    # Clamp brightness
    brightness = max(0, min(100, brightness))
    
    # Message format: [45] [02] [83 ac] [instance] [42] [02] [brightness]
    data = bytes([
        0x45,           # Message type: SetValue
        0x02,           # Sub-type: dimmer
        0x83, 0xac,     # Magic bytes
        instance,       # Device instance
        0x42,           # Command: set
        0x02,           # Modifier
        brightness,     # Brightness value
    ])
    
    return Command(data)


def build_light_on(instance: int) -> Command:
    """Build command to turn light ON (100%)."""
    return build_dimmer_command(instance, 100)


def build_light_off(instance: int) -> Command:
    """
    Build command to turn light OFF.
    
    Note: OFF might use a shorter format without the brightness byte.
    """
    # Try with brightness=0 first
    return build_dimmer_command(instance, 0)


def build_toggle_command(instance: int) -> Command:
    """
    Build a toggle command (0x85 type).
    
    Note: Format not fully understood yet.
    """
    # Speculative format based on message type 0x85
    data = bytes([
        0x85,           # Message type: Toggle
        0x02,           # Sub-type
        0x83, 0xac,     # Magic bytes
        instance,       # Device instance
    ])
    
    return Command(data)


def build_status_query(instance: int = 0x00) -> Command:
    """
    Build a status query command.
    
    Args:
        instance: Device instance (0x00 for all?)
    """
    # Based on captured 0xC5 messages
    data = bytes([
        0xC5,           # Message type: StatusQuery
        0x04,           # Length?
        0x01,           # ?
        0x6d,           # ?
        0x40,           # ?
        0x01,           # ?
        0xcb,           # ?
    ])
    
    return Command(data)


# Convenience functions for known devices

def kitchen_on() -> Command:
    """Turn Kitchen light ON."""
    return build_light_on(DeviceInstance.KITCHEN)


def kitchen_off() -> Command:
    """Turn Kitchen light OFF."""
    return build_light_off(DeviceInstance.KITCHEN)


def kitchen_dim(brightness: int) -> Command:
    """Set Kitchen light to specific brightness."""
    return build_dimmer_command(DeviceInstance.KITCHEN, brightness)


def bed_ceiling_on() -> Command:
    """Turn Bed Ceiling light ON."""
    return build_light_on(DeviceInstance.BED_CEILING)


def bed_ceiling_off() -> Command:
    """Turn Bed Ceiling light OFF."""
    return build_light_off(DeviceInstance.BED_CEILING)


def bed_ceiling_dim(brightness: int) -> Command:
    """Set Bed Ceiling light to specific brightness."""
    return build_dimmer_command(DeviceInstance.BED_CEILING, brightness)


if __name__ == '__main__':
    # Test command building
    print("OneControl Command Builder")
    print("=" * 40)
    
    commands = [
        ("Kitchen ON", kitchen_on()),
        ("Kitchen OFF", kitchen_off()),
        ("Kitchen 50%", kitchen_dim(50)),
        ("Bed Ceiling ON", bed_ceiling_on()),
        ("Bed Ceiling OFF", bed_ceiling_off()),
        ("Toggle Kitchen", build_toggle_command(DeviceInstance.KITCHEN)),
        ("Status Query", build_status_query()),
    ]
    
    for name, cmd in commands:
        print(f"\n{name}:")
        print(f"  Raw:    {cmd.data.hex()}")
        print(f"  Framed: {cmd.frame().hex()}")
