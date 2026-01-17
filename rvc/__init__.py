"""
OneControl RV-C Protocol Library

A Python library for communicating with Lippert OneControl RV systems
using a proprietary TCP protocol (port 6969) that wraps RV-C messages.
"""

from .dgn import DGN, DGN_NAMES
from .decoder import RVCDecoder
from .client import OneControlClient
from .commands import OneControlCommands, build_command
from .lippert_protocol import LippertProtocolDecoder, LippertMessage

__version__ = "0.1.0"
__all__ = [
    "DGN", 
    "DGN_NAMES", 
    "RVCDecoder", 
    "OneControlClient",
    "OneControlCommands",
    "build_command",
    "LippertProtocolDecoder",
    "LippertMessage",
]
