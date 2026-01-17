"""
Lippert OneControl command builders.

Based on reverse-engineering of LippertConnect iOS app traffic.

Command Format (observed):
    [00] [type] [len] [83 dc] [instance] [cmd] [subcmd] [value...] [00]

Message Types:
    0x45 - Dimmer/value set command (with 16-bit value)
    0x85 - Toggle/on/off command
    0x40 - Short status/ack
    0x43 - Multi-byte command/status
"""

import struct
from typing import Optional
from dataclasses import dataclass


@dataclass  
class CommandResult:
    """Result of building a command."""
    raw: bytes
    description: str
    
    def __repr__(self):
        return f"Command({self.description}): {self.raw.hex()}"


class OneControlCommands:
    """
    Build commands for Lippert OneControl system.
    
    All commands are prefixed with 0x00 (frame marker) in the TCP stream.
    """
    
    # Magic bytes that appear in all commands
    MAGIC = bytes([0x83, 0xdc])
    
    @staticmethod
    def toggle_light(instance: int) -> CommandResult:
        """
        Toggle a light on/off.
        
        Args:
            instance: Light instance number (0x00-0xFF)
            
        Returns:
            CommandResult with raw bytes to send
        """
        # Format: 00 85 02 83 dc <inst> 30 01 aa 00
        cmd = bytes([
            0x00,           # Frame start
            0x85,           # Message type (toggle)
            0x02,           # Length marker
            0x83, 0xdc,     # Magic
            instance,       # Instance
            0x30,           # Command type
            0x01,           # Subcommand (toggle)
            0xaa,           # Unknown constant
            0x00,           # Terminator
        ])
        return CommandResult(cmd, f"toggle light instance 0x{instance:02X}")
    
    @staticmethod
    def set_dimmer(instance: int, value: int) -> CommandResult:
        """
        Set a dimmer to a specific value.
        
        Args:
            instance: Dimmer instance number (0x00-0xFF)
            value: Brightness value (0-0xFFFF, typically 0-1000 or 0-0xC8)
            
        Returns:
            CommandResult with raw bytes to send
        """
        # Format: 00 45 02 83 dc <inst> 30 02 <val_lo> <val_hi> 00
        val_lo = value & 0xFF
        val_hi = (value >> 8) & 0xFF
        
        cmd = bytes([
            0x00,           # Frame start
            0x45,           # Message type (set value)
            0x02,           # Length marker
            0x83, 0xdc,     # Magic
            instance,       # Instance
            0x30,           # Command type
            0x02,           # Subcommand (set)
            val_lo,         # Value low byte
            val_hi,         # Value high byte
            0x00,           # Terminator
        ])
        return CommandResult(cmd, f"set dimmer instance 0x{instance:02X} to {value}")
    
    @staticmethod
    def set_dimmer_percent(instance: int, percent: float) -> CommandResult:
        """
        Set a dimmer to a percentage (0-100%).
        
        Args:
            instance: Dimmer instance number
            percent: Brightness 0.0-100.0%
            
        Returns:
            CommandResult with raw bytes
        """
        # Map 0-100% to 0-1000 (observed max value around 0x0AD4 = 2772)
        value = int(percent * 10)
        value = max(0, min(1000, value))
        return OneControlCommands.set_dimmer(instance, value)
    
    @staticmethod
    def request_status(instance: int) -> CommandResult:
        """
        Request status for a device instance.
        
        Based on observed app traffic, this appears to use 0x45 with 0x04 subtype.
        """
        # Format: 00 45 02 83 dc <inst> 04 11 02 2b af 00
        cmd = bytes([
            0x00,           # Frame start
            0x45,           # Message type
            0x02,           # Length marker
            0x83, 0xdc,     # Magic
            instance,       # Instance
            0x04,           # Command type (request?)
            0x11,           # Subcommand
            0x02,           # ?
            0x2b,           # ?
            0xaf,           # ?
            0x00,           # Terminator
        ])
        return CommandResult(cmd, f"request status for instance 0x{instance:02X}")
    
    @staticmethod
    def heartbeat() -> CommandResult:
        """
        Send a heartbeat/keepalive message.
        
        Based on observed periodic messages from the app.
        """
        # This is the 53-byte status message seen repeatedly
        cmd = bytes([
            0x00,
            0x43, 0x01, 0x06, 0xf7, 0x01, 0xf0, 0x00, 0x00,
            0x41, 0x08, 0x41, 0xf7, 0x08, 0x1c, 0x88, 0x43,
            0x4f, 0xaf, 0x67, 0x82, 0x3c, 0x00, 0x00,
            0x43, 0x08, 0x02, 0xf7, 0x43, 0x2f, 0xf7, 0x17,
            0x81, 0x02, 0x01, 0x44, 0x00, 0x00, 0xc3, 0x04,
            0x01, 0xf7, 0x40, 0x01, 0xc1, 0x00, 0x00, 0x40,
            0x03, 0x03, 0xf7, 0xec, 0x00,
        ])
        return CommandResult(cmd, "heartbeat/keepalive")


class InstanceMap:
    """
    Map human-readable names to instance numbers.
    
    These are RV-specific and would need to be discovered or configured
    per installation.
    """
    
    def __init__(self):
        self.instances = {}
        
    def add(self, name: str, instance: int, device_type: str = "light"):
        """Add a named instance."""
        self.instances[name.lower()] = {
            "instance": instance,
            "type": device_type,
        }
    
    def get(self, name: str) -> Optional[int]:
        """Get instance number by name."""
        entry = self.instances.get(name.lower())
        return entry["instance"] if entry else None
    
    def list_all(self):
        """List all known instances."""
        return list(self.instances.items())


# Example instance mappings (these would vary by RV)
DEFAULT_INSTANCES = InstanceMap()
# DEFAULT_INSTANCES.add("bedroom_main", 0xC4, "dimmer")
# DEFAULT_INSTANCES.add("kitchen_ceiling", 0xFB, "dimmer")


def build_command(action: str, instance: int, value: Optional[int] = None) -> bytes:
    """
    Build a command from action string.
    
    Args:
        action: "toggle", "on", "off", "dim", "set"
        instance: Device instance number
        value: Optional value for dim/set actions (0-100 for percent, 0-1000 for raw)
        
    Returns:
        Raw command bytes to send
    """
    if action in ("toggle", "on", "off"):
        return OneControlCommands.toggle_light(instance).raw
    elif action == "dim" and value is not None:
        return OneControlCommands.set_dimmer_percent(instance, value).raw
    elif action == "set" and value is not None:
        return OneControlCommands.set_dimmer(instance, value).raw
    else:
        raise ValueError(f"Unknown action: {action}")
