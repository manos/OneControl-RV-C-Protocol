"""
OneControl RV-C Protocol Library

A Python library for communicating with Lippert OneControl RV systems
using a proprietary TCP protocol (port 6969) that wraps RV-C messages.
"""

from .dgn import DGN, DGN_NAMES
from .decoder import RVCDecoder
from .client import OneControlClient
from .commands import (
    Command, 
    build_light_command, 
    build_light_on, 
    build_light_off,
    build_light_restore,
    build_get_devices,
    LightDevice,
    MyRvLinkCommandType,
    DimmableLightCommand,
)
from .lippert_protocol import LippertProtocolDecoder, LippertMessage
from .cobs import crc8, encode as cobs_encode, decode as cobs_decode, StreamDecoder

__version__ = "0.1.0"
__all__ = [
    "DGN", 
    "DGN_NAMES", 
    "RVCDecoder", 
    "OneControlClient",
    "Command",
    "build_light_command",
    "build_light_on",
    "build_light_off",
    "build_light_restore",
    "build_get_devices",
    "LightDevice",
    "MyRvLinkCommandType",
    "DimmableLightCommand",
    "LippertProtocolDecoder",
    "LippertMessage",
    "crc8",
    "cobs_encode",
    "cobs_decode",
    "StreamDecoder",
]
