"""
OneControl RV-C Protocol Library

A Python library for communicating with Lippert OneControl RV systems.

Quick Start:
    from rvc.onecontrol import control_light, LightConfig
    
    # Define a light with universal values + discovered counter
    my_light = LightConfig(
        name="Kitchen",
        protocol=0x80,
        session=0x80,
        device=0x04,
        conn=0x40,
        counter=0x28,  # From auto_discover.py
    )
    
    await control_light(my_light, on=True)
"""

# Main API - import from onecontrol module
from .onecontrol import (
    # Light control
    LightConfig,
    control_light,
    KITCHEN,
    LIVING_ROOM,
    BED_CEILING,
    PORCH,
    # Tank sensors
    TankConfig,
    read_tank_levels,
    TANK_GREY,
    TANK_FRESH,
    TANK_BLACK,
    TANK_LP,
    ALL_TANKS,
    # Battery voltage
    read_battery_voltage,
    GENERATOR_GENIE_COUNTER,
    # Generator hours
    read_generator_hours,
    GENERATOR_HOUR_METER_COUNTER,
    # Low-level utilities
    cobs_encode,
    decode_frames,
    tea_encrypt,
    REMOTE_CONTROL_CYPHER,
    DEFAULT_HOST,
    DEFAULT_PORT,
)

__version__ = "0.2.0"
__all__ = [
    # Light control
    "LightConfig",
    "control_light",
    "KITCHEN",
    "LIVING_ROOM", 
    "BED_CEILING",
    "PORCH",
    # Tank sensors
    "TankConfig",
    "read_tank_levels",
    "TANK_GREY",
    "TANK_FRESH",
    "TANK_BLACK",
    "TANK_LP",
    "ALL_TANKS",
    # Battery voltage
    "read_battery_voltage",
    "GENERATOR_GENIE_COUNTER",
    # Generator hours
    "read_generator_hours",
    "GENERATOR_HOUR_METER_COUNTER",
    # Low-level
    "cobs_encode",
    "decode_frames",
    "tea_encrypt",
    "REMOTE_CONTROL_CYPHER",
    "DEFAULT_HOST",
    "DEFAULT_PORT",
]
