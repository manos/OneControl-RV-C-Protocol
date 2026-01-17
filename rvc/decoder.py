"""
RV-C message decoder.

Decodes raw CAN frames into structured data based on the RV-C specification.

NOTE: The Lippert OneControl TCP protocol (port 6969) uses a PROPRIETARY
framing format, not raw RV-C CAN frames. The format appears to be:

    [msg_type][length?][instance/node][payload...]

Message types observed:
    0x40 - Status/state message (short)
    0x41 - Status/state message (8 bytes)  
    0x43 - Command/control message
    0x45 - Extended message
    0x49 - Periodic/heartbeat
    0x85 - System message
    0xC3 - Device status
    0xC5 - Device config

This module provides decoders for both raw RV-C and the Lippert proprietary format.
"""

from dataclasses import dataclass
from typing import Optional, Dict, Any, Tuple, List
from .dgn import DGN, DCDimmerCommand, ThermostatMode, FanMode, TankType


@dataclass
class RVCMessage:
    """Decoded RV-C message"""
    dgn: int
    dgn_name: str
    source_address: int
    priority: int
    instance: int
    data: Dict[str, Any]
    raw_data: bytes


class RVCDecoder:
    """Decoder for RV-C CAN messages"""
    
    # Temperature conversion constants (RV-C spec)
    TEMP_OFFSET = -273.0  # Kelvin to Celsius offset
    TEMP_SCALE = 0.03125  # Resolution per bit
    
    # Percentage/brightness max values
    RVC_PERCENT_MAX = 250
    RVC_BRIGHT_MAX = 200
    
    def __init__(self):
        self.decoders = {
            DGN.DC_DIMMER_STATUS_3: self._decode_dc_dimmer_status_3,
            DGN.DC_DIMMER_COMMAND_2: self._decode_dc_dimmer_command_2,
            DGN.THERMOSTAT_AMBIENT_STATUS: self._decode_thermostat_ambient_status,
            DGN.THERMOSTAT_STATUS_1: self._decode_thermostat_status_1,
            DGN.THERMOSTAT_COMMAND_1: self._decode_thermostat_command_1,
            DGN.TANK_STATUS: self._decode_tank_status,
            DGN.DC_SOURCE_STATUS_1: self._decode_dc_source_status_1,
            DGN.FURNACE_STATUS: self._decode_furnace_status,
            DGN.WATER_HEATER_STATUS: self._decode_water_heater_status,
            DGN.GENERATOR_STATUS_1: self._decode_generator_status_1,
        }
    
    @staticmethod
    def extract_dgn_from_canid(canid: int) -> Tuple[int, int, int]:
        """
        Extract DGN, source address, and priority from a 29-bit CAN ID.
        
        RV-C CAN ID format (29-bit extended):
        - Bits 28-26: Priority (3 bits)
        - Bits 25-8: DGN (17 bits + reserved)
        - Bits 7-0: Source Address (8 bits)
        
        Returns: (dgn, source_address, priority)
        """
        priority = (canid >> 26) & 0x07
        dgn = (canid >> 8) & 0x1FFFF
        source_address = canid & 0xFF
        return dgn, source_address, priority
    
    @staticmethod
    def make_canid(dgn: int, source_address: int = 0, priority: int = 6) -> int:
        """Construct a CAN ID from DGN, source address, and priority."""
        return (priority << 26) | (dgn << 8) | source_address
    
    def _temp_to_celsius(self, raw_value: int) -> float:
        """Convert RV-C temperature value to Celsius."""
        if raw_value == 0xFFFF:
            return None  # Not available
        return self.TEMP_OFFSET + raw_value * self.TEMP_SCALE
    
    def _celsius_to_raw(self, temp_c: float) -> int:
        """Convert Celsius to RV-C temperature value."""
        return int((temp_c - self.TEMP_OFFSET) / self.TEMP_SCALE)
    
    def _brightness_to_percent(self, brightness: int) -> float:
        """Convert RV-C brightness (0-200) to percentage (0-100)."""
        return min(brightness, self.RVC_BRIGHT_MAX) * 100.0 / self.RVC_BRIGHT_MAX
    
    def _percent_to_brightness(self, percent: float) -> int:
        """Convert percentage (0-100) to RV-C brightness (0-200)."""
        return int(percent * self.RVC_BRIGHT_MAX / 100.0)
    
    def decode(self, canid: int, data: bytes) -> RVCMessage:
        """
        Decode an RV-C message from CAN ID and data bytes.
        
        Args:
            canid: 29-bit CAN identifier
            data: 8 bytes of CAN data
            
        Returns:
            RVCMessage with decoded fields
        """
        dgn, source_address, priority = self.extract_dgn_from_canid(canid)
        
        # Get DGN name
        try:
            dgn_enum = DGN(dgn)
            dgn_name = dgn_enum.name
        except ValueError:
            dgn_name = f"UNKNOWN_0x{dgn:05X}"
        
        # Instance is typically byte 0
        instance = data[0] if len(data) > 0 else 0
        
        # Decode based on DGN
        decoder = self.decoders.get(dgn)
        if decoder:
            decoded_data = decoder(data)
        else:
            decoded_data = {"raw_bytes": data.hex()}
        
        return RVCMessage(
            dgn=dgn,
            dgn_name=dgn_name,
            source_address=source_address,
            priority=priority,
            instance=instance,
            data=decoded_data,
            raw_data=data
        )
    
    def _decode_dc_dimmer_status_3(self, data: bytes) -> Dict[str, Any]:
        """
        Decode DC_DIMMER_STATUS_3 (0x1FEDA)
        
        Byte 0: Instance
        Byte 1: Group
        Byte 2: Brightness (0-200)
        Byte 3: bits 6-7: Enable state
        Byte 4: Delay/Duration
        Byte 5: Last command
        Byte 6: bits 2-3: Operating status
        """
        instance = data[0]
        group = data[1]
        brightness = min(data[2], self.RVC_BRIGHT_MAX)
        enable = (data[3] >> 6) & 0x03
        delay_duration = data[4]
        last_cmd = data[5]
        status = (data[6] >> 2) & 0x03
        
        return {
            "instance": instance,
            "group": group,
            "brightness_raw": brightness,
            "brightness_percent": self._brightness_to_percent(brightness),
            "is_on": brightness > 0,
            "enable": enable,
            "delay_duration": delay_duration,
            "last_command": DCDimmerCommand(last_cmd).name if last_cmd < 16 else last_cmd,
            "status": status,
        }
    
    def _decode_dc_dimmer_command_2(self, data: bytes) -> Dict[str, Any]:
        """
        Decode DC_DIMMER_COMMAND_2 (0x1FEDB)
        
        Byte 0: Instance
        Byte 1: Group
        Byte 2: Brightness (0-200)
        Byte 3: Command
        Byte 4: Duration
        Byte 5: Interlock
        """
        instance = data[0]
        group = data[1]
        brightness = data[2]
        cmd = data[3]
        duration = data[4]
        interlock = data[5]
        
        return {
            "instance": instance,
            "group": group,
            "brightness_raw": brightness,
            "brightness_percent": self._brightness_to_percent(brightness),
            "command": DCDimmerCommand(cmd).name if cmd < 16 else cmd,
            "duration": duration,
            "interlock": interlock,
        }
    
    def _decode_thermostat_ambient_status(self, data: bytes) -> Dict[str, Any]:
        """
        Decode THERMOSTAT_AMBIENT_STATUS (0x1FF9C)
        
        Byte 0: Instance
        Bytes 1-2: Ambient temperature (little-endian)
        """
        instance = data[0]
        temp_raw = data[1] | (data[2] << 8)
        temp_c = self._temp_to_celsius(temp_raw)
        
        return {
            "instance": instance,
            "temperature_c": temp_c,
            "temperature_f": temp_c * 9/5 + 32 if temp_c else None,
        }
    
    def _decode_thermostat_status_1(self, data: bytes) -> Dict[str, Any]:
        """
        Decode THERMOSTAT_STATUS_1 (0x1FFE2)
        
        Byte 0: Instance
        Byte 1: bits 0-3: Operating mode, bits 4-5: Fan mode, bits 6-7: Schedule enabled
        Byte 2: Fan speed
        Bytes 3-4: Heat setpoint (little-endian)
        Bytes 5-6: Cool setpoint (little-endian)
        """
        instance = data[0]
        op_mode = data[1] & 0x0F
        fan_mode = (data[1] >> 4) & 0x03
        schedule_enabled = (data[1] >> 6) & 0x03
        fan_speed = data[2]
        heat_temp_raw = data[3] | (data[4] << 8)
        cool_temp_raw = data[5] | (data[6] << 8)
        
        heat_temp_c = self._temp_to_celsius(heat_temp_raw)
        cool_temp_c = self._temp_to_celsius(cool_temp_raw)
        
        return {
            "instance": instance,
            "operating_mode": ThermostatMode(op_mode).name if op_mode < 7 else op_mode,
            "fan_mode": FanMode(fan_mode).name if fan_mode < 2 else fan_mode,
            "schedule_enabled": schedule_enabled,
            "fan_speed": fan_speed,
            "fan_speed_percent": fan_speed * 100.0 / self.RVC_PERCENT_MAX,
            "heat_setpoint_c": heat_temp_c,
            "heat_setpoint_f": heat_temp_c * 9/5 + 32 if heat_temp_c else None,
            "cool_setpoint_c": cool_temp_c,
            "cool_setpoint_f": cool_temp_c * 9/5 + 32 if cool_temp_c else None,
        }
    
    def _decode_thermostat_command_1(self, data: bytes) -> Dict[str, Any]:
        """Decode THERMOSTAT_COMMAND_1 (0x1FEF9) - same format as STATUS_1"""
        return self._decode_thermostat_status_1(data)
    
    def _decode_tank_status(self, data: bytes) -> Dict[str, Any]:
        """
        Decode TANK_STATUS (0x1FFB7)
        
        Byte 0: Instance (bits 0-3: instance, bits 4-7: tank type)
        Byte 1: Level (0-250 = 0-100%)
        Bytes 2-3: Resolution (liters)
        Bytes 4-5: Capacity (liters)
        """
        instance = data[0] & 0x0F
        tank_type = (data[0] >> 4) & 0x0F
        level_raw = data[1]
        resolution = data[2] | (data[3] << 8)
        capacity = data[4] | (data[5] << 8)
        
        level_percent = level_raw * 100.0 / self.RVC_PERCENT_MAX if level_raw < 251 else None
        
        return {
            "instance": instance,
            "tank_type": TankType(tank_type).name if tank_type < 5 else tank_type,
            "level_raw": level_raw,
            "level_percent": level_percent,
            "resolution_liters": resolution if resolution != 0xFFFF else None,
            "capacity_liters": capacity if capacity != 0xFFFF else None,
        }
    
    def _decode_dc_source_status_1(self, data: bytes) -> Dict[str, Any]:
        """
        Decode DC_SOURCE_STATUS_1 (0x1FFFD)
        
        Byte 0: Instance
        Byte 1: Device priority
        Bytes 2-3: DC Voltage (0.05V per bit)
        Bytes 4-7: DC Current (0.001A per bit, signed)
        """
        instance = data[0]
        priority = data[1]
        voltage_raw = data[2] | (data[3] << 8)
        current_raw = data[4] | (data[5] << 8) | (data[6] << 16) | (data[7] << 24)
        
        # Voltage: 0.05V per bit
        voltage = voltage_raw * 0.05 if voltage_raw != 0xFFFF else None
        
        # Current: signed 32-bit, 0.001A per bit
        if current_raw != 0xFFFFFFFF:
            if current_raw > 0x7FFFFFFF:
                current_raw -= 0x100000000
            current = current_raw * 0.001
        else:
            current = None
        
        return {
            "instance": instance,
            "device_priority": priority,
            "voltage": voltage,
            "current": current,
        }
    
    def _decode_furnace_status(self, data: bytes) -> Dict[str, Any]:
        """
        Decode FURNACE_STATUS (0x1FFE4)
        
        Byte 0: Instance
        Byte 1: bits 0-1: Operating mode
        Byte 2: Heat output (0-250)
        Byte 3: Circulation fan speed (0-250)
        """
        instance = data[0]
        op_mode = data[1] & 0x03
        heat_output = data[2]
        fan_speed = data[3]
        
        return {
            "instance": instance,
            "operating_mode": op_mode,
            "heat_output_percent": heat_output * 100.0 / self.RVC_PERCENT_MAX if heat_output < 251 else None,
            "fan_speed_percent": fan_speed * 100.0 / self.RVC_PERCENT_MAX if fan_speed < 251 else None,
        }
    
    def _decode_water_heater_status(self, data: bytes) -> Dict[str, Any]:
        """
        Decode WATER_HEATER_STATUS (0x1FFF7)
        
        Byte 0: Instance
        Byte 1: bits 0-3: Operating mode
        Bytes 2-3: Set point temperature
        Bytes 4-5: Water temperature
        """
        instance = data[0]
        op_mode = data[1] & 0x0F
        set_temp_raw = data[2] | (data[3] << 8)
        water_temp_raw = data[4] | (data[5] << 8)
        
        set_temp_c = self._temp_to_celsius(set_temp_raw)
        water_temp_c = self._temp_to_celsius(water_temp_raw)
        
        return {
            "instance": instance,
            "operating_mode": op_mode,
            "setpoint_c": set_temp_c,
            "setpoint_f": set_temp_c * 9/5 + 32 if set_temp_c else None,
            "water_temp_c": water_temp_c,
            "water_temp_f": water_temp_c * 9/5 + 32 if water_temp_c else None,
        }
    
    def _decode_generator_status_1(self, data: bytes) -> Dict[str, Any]:
        """
        Decode GENERATOR_STATUS_1 (0x1FFDC)
        
        Byte 0: Instance
        Byte 1: bits 0-1: Operating status, bits 2-3: Generator status
        Byte 2: Engine speed (rpm / 32)
        Bytes 4-5: Engine load (% * 2.5)
        """
        instance = data[0]
        op_status = data[1] & 0x03
        gen_status = (data[1] >> 2) & 0x03
        engine_speed = data[2] * 32 if data[2] != 0xFF else None
        engine_load = (data[4] | (data[5] << 8)) / 2.5 if data[4] != 0xFF else None
        
        return {
            "instance": instance,
            "operating_status": op_status,
            "generator_status": gen_status,
            "engine_rpm": engine_speed,
            "engine_load_percent": engine_load,
        }
