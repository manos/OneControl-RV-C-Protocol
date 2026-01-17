# OneControl RV-C Protocol

Reverse-engineered protocol documentation and Python client for **Lippert OneControl** RV systems.

> ⚠️ **Work in Progress** - This project is actively being reverse-engineered. Protocol details may be incomplete or incorrect.

## Overview

Lippert OneControl is a control system found in many modern RVs that manages:
- **Lighting** (dimmable and on/off)
- **HVAC** (thermostats, air conditioners, furnaces)
- **Tank levels** (fresh, gray, black water, propane)
- **Power systems** (batteries, inverters, chargers, generators)
- **Water heaters**
- **Awnings and slides**
- **Leveling systems**

The system uses **RV-C (Recreational Vehicle CAN)**, an open standard built on CAN bus, as its underlying protocol. This project documents the protocol and provides a Python library for direct communication with OneControl systems.

## Protocol Details

### Network Architecture

```
┌─────────────────┐     WiFi AP        ┌──────────────────┐
│  Mobile App     │◄──────────────────►│  OneControl      │
│  (iOS/Android)  │   192.168.1.x      │  Controller      │
└─────────────────┘                    │  (TCP:6969)      │
                                       └────────┬─────────┘
                                                │ CAN Bus
                                       ┌────────┴─────────┐
                                       │  RV-C Network    │
                                       │  (All devices)   │
                                       └──────────────────┘
```

### OneControl WiFi AP

The OneControl system creates a WiFi access point:
- **SSID**: `MyRV_<serial>`  (e.g., `MyRV_002630732020218F`)
- **Password**: Found on the unit or in documentation
- **Controller IP**: Typically `192.168.1.1`
- **Protocol**: TCP on port `6969`

### RV-C Protocol Basics

RV-C is built on CAN 2.0B (29-bit extended identifiers):

```
CAN ID (29 bits):
├── Priority (3 bits)     - bits 28-26
├── DGN (17 bits)         - bits 25-8 (Data Group Number)
└── Source Address (8b)   - bits 7-0
```

**Data Group Numbers (DGN)** identify the message type:

| DGN | Name | Description |
|-----|------|-------------|
| 0x1FEDA | DC_DIMMER_STATUS_3 | Light status (brightness, on/off) |
| 0x1FEDB | DC_DIMMER_COMMAND_2 | Light control commands |
| 0x1FF9C | THERMOSTAT_AMBIENT_STATUS | Current temperature |
| 0x1FFE2 | THERMOSTAT_STATUS_1 | HVAC operating state |
| 0x1FEF9 | THERMOSTAT_COMMAND_1 | HVAC control commands |
| 0x1FFB7 | TANK_STATUS | Tank levels |
| 0x1FFFD | DC_SOURCE_STATUS_1 | Battery voltage/current |
| 0x1FFE4 | FURNACE_STATUS | Furnace state |
| 0x1FFF7 | WATER_HEATER_STATUS | Water heater state |

See [`rvc/dgn.py`](rvc/dgn.py) for the complete list.

### TCP Framing (Preliminary)

The TCP protocol wraps RV-C CAN frames in a custom format. Based on traffic analysis:

```
[Length?][Type?][CAN ID (4 bytes)][CAN Data (8 bytes)]...
```

> 🔬 **Protocol analysis in progress** - The exact framing format is still being determined.

## Installation

### Using Pixi (Recommended)

```bash
git clone https://github.com/manos/OneControl-RV-C-Protocol.git
cd OneControl-RV-C-Protocol
pixi install
pixi shell
```

### Using pip

```bash
git clone https://github.com/manos/OneControl-RV-C-Protocol.git
cd OneControl-RV-C-Protocol
python -m venv .venv
source .venv/bin/activate
pip install -e .
```

## Usage

### Monitor Traffic

```python
import asyncio
from rvc import OneControlClient

async def main():
    async with OneControlClient("192.168.1.1") as client:
        async for message in client.messages():
            print(f"{message.dgn_name}: {message.data}")

asyncio.run(main())
```

### Decode RV-C Messages

```python
from rvc import RVCDecoder

decoder = RVCDecoder()

# Decode a CAN frame (ID + 8 bytes data)
canid = 0x19FEDA40  # DC_DIMMER_STATUS_3 from source 0x40
data = bytes([0x01, 0xFF, 0xC8, 0x00, 0xFF, 0x01, 0x00, 0xFF])

message = decoder.decode(canid, data)
print(f"Light {message.data['instance']}: {message.data['brightness_percent']}%")
```

### Control Lights

```python
async with OneControlClient("192.168.1.1") as client:
    # Turn on light instance 1
    await client.set_light(1, on=True, brightness=200)
    
    # Dim light to 50%
    await client.set_light_brightness(1, 100)
    
    # Turn off
    await client.set_light(1, on=False)
```

## Command Line Tools

### Protocol Monitor

```bash
python -m rvc.client 192.168.1.1
```

### Packet Analyzer

```bash
python tools/analyze_capture.py capture.pcap
```

## Project Structure

```
OneControl-RV-C-Protocol/
├── rvc/
│   ├── __init__.py      # Package exports
│   ├── dgn.py           # DGN definitions and enums
│   ├── decoder.py       # RV-C message decoder
│   └── client.py        # TCP client for OneControl
├── docs/
│   └── protocol.md      # Detailed protocol documentation
├── tools/
│   └── analyze_capture.py  # Packet capture analyzer
├── examples/
│   └── monitor.py       # Example monitoring script
└── tests/
    └── test_decoder.py  # Unit tests
```

## Resources

### RV-C Documentation
- [RVIA RV-C Specification](http://www.rv-c.com/) - Official standard (requires purchase)
- [RV-C Lite Primer](http://www.rv-c.com/forum/viewtopic.php?f=4&t=33) - Free introduction

### Related Projects
- [RV-Bridge](https://github.com/rubillos/RV-Bridge) - ESP32 HomeKit to RV-C bridge
- [eRVin](https://github.com/linuxkidd/ervin) - Raspberry Pi RV-C gateway
- [CoachProxyOS](https://github.com/linuxkidd/coachproxyos) - RV monitoring system
- [HomeAssistant OneControl Discovery](https://github.com/example/HomeAssistant_OneControl_Discovery) - Node-RED integration (requires Cloud Bridge)

### Lippert Documentation
- [OneControl Support](https://support.lci1.com/onecontrol)
- [LippertConnect App](https://www.lippertcomponents.com/products/lippertconnect)

## Contributing

Contributions welcome! Areas that need work:
1. **TCP framing** - Document the exact byte format
2. **More DGNs** - Add missing message types
3. **Bidirectional control** - Sending commands
4. **Home Assistant integration** - Native component

## License

MIT License - See [LICENSE](LICENSE)

## Acknowledgments

- [RV-Bridge](https://github.com/rubillos/RV-Bridge) by Randy Ubillos - DGN definitions reference
- RV-C specification by RVIA
- The RV community for protocol insights
