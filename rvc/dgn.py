"""
RV-C Data Group Numbers (DGN) definitions.

Based on RV-C specification and reverse-engineered from:
- RV-Bridge project (https://github.com/rubillos/RV-Bridge)
- Lippert OneControl TCP traffic analysis

DGN is a 17-bit identifier embedded in the CAN message ID that identifies
the type of data being transmitted.
"""

from enum import IntEnum


class DGN(IntEnum):
    """RV-C Data Group Numbers (DGN)"""
    
    # Date/Time
    DATE_TIME_STATUS = 0x1FFFF
    
    # DC Dimmer / Lighting
    DC_DIMMER_COMMAND_2 = 0x1FEDB
    DC_DIMMER_STATUS_3 = 0x1FEDA
    
    # Thermostat / HVAC
    THERMOSTAT_AMBIENT_STATUS = 0x1FF9C
    THERMOSTAT_STATUS_1 = 0x1FFE2
    THERMOSTAT_COMMAND_1 = 0x1FEF9
    
    # Air Conditioner
    AIR_CONDITIONER_STATUS = 0x1FFE0
    AIR_CONDITIONER_STATUS_2 = 0x1FFE1
    
    # Generic Indicator
    GENERIC_INDICATOR_COMMAND = 0x1FED9
    GENERIC_CONFIGURATION_STATUS = 0x1FED8
    
    # DC Power Source
    DC_SOURCE_STATUS_1 = 0x1FFFD
    DC_SOURCE_STATUS_2 = 0x1FFFC
    DC_SOURCE_STATUS_3 = 0x1FFFB
    DC_SOURCE_STATUS_4 = 0x1FFFA
    DC_SOURCE_STATUS_6 = 0x1FEA4
    DC_SOURCE_COMMAND = 0x1FEA3
    
    # Tank Levels
    TANK_STATUS = 0x1FFB7
    
    # Generator
    GENERATOR_STATUS_1 = 0x1FFDC
    GENERATOR_STATUS_2 = 0x1FFDB
    GENERATOR_COMMAND = 0x1FFDA
    GENERATOR_START_CONFIG_STATUS = 0x1FFD8
    GENERATOR_START_CONFIG_COMMAND = 0x1FFD9
    
    # Furnace
    FURNACE_STATUS = 0x1FFE4
    FURNACE_COMMAND = 0x1FFE3
    
    # Inverter / Charger
    INVERTER_STATUS = 0x1FFD4
    INVERTER_COMMAND = 0x1FFD5
    INVERTER_AC_STATUS_1 = 0x1FFD7
    INVERTER_AC_STATUS_2 = 0x1FFD6
    INVERTER_AC_STATUS_3 = 0x1FFD3
    INVERTER_AC_STATUS_4 = 0x1FED1
    INVERTER_DC_STATUS = 0x1FFD2
    INVERTER_TEMPERATURE_STATUS = 0x1FEBD
    CHARGER_STATUS = 0x1FFC7
    CHARGER_STATUS_2 = 0x1FEA8
    CHARGER_CONFIGURATION_STATUS = 0x1FFC6
    CHARGER_CONFIGURATION_STATUS_2 = 0x1FFC5
    CHARGER_CONFIGURATION_COMMAND = 0x1FFC4
    CHARGER_CONFIGURATION_COMMAND_2 = 0x1FEA9
    CHARGER_EQUALIZATION_STATUS = 0x1FF99
    CHARGER_EQUALIZATION_CONFIGURATION_STATUS = 0x1FF98
    CHARGER_EQUALIZATION_CONFIGURATION_COMMAND = 0x1FF97
    CHARGER_AC_STATUS_1 = 0x1FFCA
    CHARGER_AC_STATUS_2 = 0x1FFCB
    CHARGER_AC_STATUS_3 = 0x1FFCC
    CHARGER_AC_STATUS_4 = 0x1FEA7
    
    # DC Load
    DC_LOAD_COMMAND = 0x1FFBC
    DC_LOAD_STATUS = 0x1FFBD
    DC_LOAD_STATUS_2 = 0x1FED7
    
    # AC Load
    AC_LOAD_STATUS = 0x1FFBF
    AC_LOAD_COMMAND = 0x1FFBE
    
    # Transfer Switch (ATS)
    ATS_STATUS = 0x1FFAA
    ATS_AC_STATUS_1 = 0x1FFAD
    ATS_AC_STATUS_2 = 0x1FFAC
    ATS_AC_STATUS_3 = 0x1FFAB
    ATS_AC_STATUS_4 = 0x1FF85
    
    # Awning / Slide
    AWNING_STATUS = 0x1FEF3
    AWNING_COMMAND = 0x1FEF2
    AWNING_STATUS_2 = 0x1FDCD
    SLIDE_STATUS = 0x1FEA1
    SLIDE_COMMAND = 0x1FEA0
    
    # Leveling
    LEVELING_JACK_STATUS = 0x1FF9F
    LEVELING_JACK_COMMAND = 0x1FF9E
    LEVELING_CONTROL_STATUS = 0x1FF9D
    LEVELING_CONTROL_COMMAND = 0x1FEE0
    
    # Water Heater
    WATER_HEATER_STATUS = 0x1FFF7
    WATER_HEATER_STATUS_2 = 0x1FEFD
    WATER_HEATER_COMMAND = 0x1FFF6
    
    # Water Pump
    WATER_PUMP_STATUS = 0x1FFF5
    WATER_PUMP_COMMAND = 0x1FFF4
    
    # Battery
    BATTERY_STATUS = 0x1AAFD  # Lippert-specific?
    
    # Product Identification
    PRODUCT_IDENTIFICATION = 0x1FEE9
    
    # Diagnostic Messages
    DM_RV = 0x1FECA
    
    # Floor Heat
    FLOOR_HEAT_STATUS = 0x1FEF5
    FLOOR_HEAT_COMMAND = 0x1FEF4


# Human-readable names for DGNs
DGN_NAMES = {dgn: dgn.name for dgn in DGN}


class DCDimmerCommand(IntEnum):
    """DC Dimmer command codes"""
    SET_BRIGHTNESS = 0
    ON_DURATION = 1
    ON_DELAY = 2
    OFF = 3
    STOP = 4
    TOGGLE = 5
    MEMORY_OFF = 6
    RAMP_BRIGHTNESS = 7
    RAMP_TOGGLE = 8
    RAMP_UP = 9
    RAMP_DOWN = 10
    RAMP_UP_DOWN = 11
    LOCK = 12
    UNLOCK = 13
    FLASH = 14
    FLASH_MOMENTARILY = 15


class ThermostatMode(IntEnum):
    """Thermostat operating mode"""
    OFF = 0
    COOL = 1
    HEAT = 2
    AUTO = 3
    FAN_ONLY = 4
    AUX_HEAT = 5
    DEHUMIDIFY = 6


class FanMode(IntEnum):
    """Fan operating mode"""
    AUTO = 0
    ON = 1


class TankType(IntEnum):
    """Tank type identifiers"""
    FRESH_WATER = 0
    BLACK_WATER = 1
    GRAY_WATER = 2
    LPG = 3
    FUEL = 4


class AwningMotion(IntEnum):
    """Awning motion states"""
    NO_MOTION = 0
    EXTENDING = 1
    RETRACTING = 2


class WaterHeaterMode(IntEnum):
    """Water heater operating modes"""
    OFF = 0
    COMBUSTION = 1
    ELECTRIC = 2
    GAS_ELECTRIC = 3
    AUTOMATIC = 4
