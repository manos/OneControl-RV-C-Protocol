# OneControl RV-C Protocol

Reverse-engineered protocol documentation and Python client for **Lippert OneControl** RV systems.

> **Status**: Full device control and sensor reading. Home Assistant integration available via [HACS](https://github.com/manos/HomeAssistant-Lippert-OneControl-Integration).

## Features

| Feature | Status |
|---------|--------|
| **Light Control** | ✅ ON/OFF for all discovered lights |
| **Generator Control** | ✅ Start/Stop with TEA cipher auth |
| **Water Heater Control** | ✅ Gas & Electric ON/OFF |
| **Water Pump Control** | ✅ ON/OFF toggle |
| **Tank Sensors** | ✅ Fresh, Grey, Black, LP levels |
| **Battery Voltage** | ✅ 12V system voltage |
| **Generator Hours** | ✅ Hour meter reading |
| **Generator State** | ✅ Off/Priming/Starting/Running/Stopping |
| **Status Polling** | ✅ Live relay states from broadcasts |
| **Auto-Discovery** | ✅ func_id-based, no packet capture needed |
| **Leveler Control** | 🔧 In progress (protocol decoded, implementation WIP) |
| **Home Assistant** | ✅ HACS integration (v0.3.0) |

## Quick Start

```python
import asyncio
from rvc.onecontrol import OneControlClient

client = OneControlClient("192.168.1.1")

async def main():
    # Discover all devices (keyed by func_id)
    devices = await client.discover_devices()
    print(devices)

    # Control a light (counter resolved dynamically)
    await client.light_on(counter=0x28)
    await asyncio.sleep(2)
    await client.light_off(counter=0x28)

asyncio.run(main())
```

## Installation

```bash
git clone https://github.com/manos/OneControl-RV-C-Protocol.git
cd OneControl-RV-C-Protocol
pip install -e .
```

## Discovery

Run auto-discovery to find all devices:

```bash
python tools/auto_discover.py
```

Output example:
```
Discovered devices:
  Lights:
    func_id=57  Bedroom Light
    func_id=63  Bed Ceiling Light
    func_id=32  Kitchen Ceiling Light
    func_id=41  Living Room Ceiling Light
    func_id=48  Porch Light
    func_id=122 Scare Light
  Tanks:
    func_id=67  Fresh Tank
    func_id=68  Grey Tank
    func_id=69  Black Tank
    func_id=70  LP Tank
  Water Heaters:
    func_id=3   Gas Water Heater
    func_id=4   Electric Water Heater
  Water Pump:
    func_id=5   Water Pump
  Generator:
    func_id=95  Generator
```

## Key Concept: func_id vs Counter

Devices are identified by two values:

- **func_id** (stable): Permanent device type identifier from firmware. Used as the primary key for all device references. Examples: `57` = Bedroom Light, `95` = Generator.
- **counter** (volatile): Session address assigned by the controller. Changes on reboot. Resolved dynamically at runtime from `0x08 0x02` registration broadcasts.

The integration and discovery tools use `func_id` as the stable identifier and resolve the current `counter` at command time.

## Usage

### Control Lights

```python
from rvc.onecontrol import OneControlClient

client = OneControlClient("192.168.1.1")

# Counter is the current session address (from discovery or live device_map)
await client.light_on(counter=0x28)
await client.light_off(counter=0x28)
```

### Control Water Heater

```python
await client.water_heater_on(counter=0x15)
await client.water_heater_off(counter=0x15)
```

### Control Water Pump

```python
await client.water_pump_on(counter=0x42)
await client.water_pump_off(counter=0x42)
```

### Control Generator

```python
await client.generator_on(counter=0x24)
await client.generator_off(counter=0x24)
```

### Read All Sensors (Single Connection)

```python
data = await client.read_all_sensors(duration=3.0)

# Tank levels
for counter, level in data["tanks"].items():
    print(f"Tank {counter:#04x}: {level}%")

# Battery voltage
if data["battery_voltage"]:
    print(f"Battery: {data['battery_voltage']:.2f}V")

# Generator
if data["generator_hours"]:
    print(f"Generator: {data['generator_hours']:.1f} hours")
print(f"Generator state: {data['generator_state']}")

# Relay states (lights, water heaters, water pumps)
for counter, is_on in data["relay_states"].items():
    print(f"Relay {counter:#04x}: {'ON' if is_on else 'OFF'}")

# Live func_id -> counter map (refreshed every read)
print(f"Device map: {data['device_map']}")
```

## Network Setup

The OneControl system creates its own WiFi network:

```
SSID:     MyRV_<serial>
Security: WPA2-PSK
Gateway:  192.168.1.1 (TCP port 6969)
```

```
┌─────────────────┐     WiFi AP        ┌──────────────────┐
│  Your App       │◄──────────────────►│  OneControl      │
│  (This library) │   192.168.1.x      │  Controller      │
└─────────────────┘                    │  192.168.1.1:6969│
                                       └────────┬─────────┘
                                                │ CAN Bus
                                       ┌────────┴─────────┐
                                       │  RV-C Network    │
                                       │  (Lights, HVAC,  │
                                       │   Generator, etc)│
                                       └──────────────────┘
```

## Protocol Overview

### Lights, Water Heaters, Water Pumps (Latching Relay)

Each ON/OFF command requires:
1. Fresh TCP connection to port 6969
2. Session registration (`0x01` + `0x08`)
3. Seed/key authentication (TEA cipher)
4. Control command (`0x00` frame)
5. Connection close

### Generator (Different Protocol!)

Same auth flow but uses protocol `0x81` instead of `0x80`, frame type `0x01` instead of `0x00`, and different ON/OFF values (`0x02`/`0x01`).

### Leveler (No Auth)

Uses button-press simulation via `0x03` frames. No seed/key authentication required. Requires a persistent TCP session with keepalives.

See [docs/PROTOCOL.md](docs/PROTOCOL.md) for full technical details.

## Home Assistant Integration

A full Home Assistant custom component is available:

**[HomeAssistant-Lippert-OneControl-Integration](https://github.com/manos/HomeAssistant-Lippert-OneControl-Integration)**

- Install via HACS (custom repository)
- Auto-discovers all devices
- Uses `func_id`-based addressing (survives controller reboots)
- Supports: lights, generator, water heaters, water pump, tank sensors, battery voltage

## Project Structure

```
OneControl-RV-C-Protocol/
├── rvc/
│   ├── onecontrol.py      # Main client (OneControlClient class)
│   ├── protocol.py        # COBS encoding, CRC-8, TEA cipher
│   ├── device_names.py    # func_id → human-readable name mapping
│   └── __init__.py
├── tools/
│   ├── auto_discover.py   # Device discovery tool
│   ├── device_analyzer.py # Packet analysis
│   ├── identify_device.py # Device identification
│   └── devices.json       # Known device database
├── examples/
│   ├── basic_usage.py     # Simple control examples
│   └── monitor.py         # Live monitoring
├── docs/
│   ├── PROTOCOL.md        # Technical protocol specification
│   └── ANALYSIS.md        # Reverse-engineering notes
├── captures/              # Reference packet captures
└── LICENSE
```

## Safety Warning

**Use caution with non-light devices!**

- **Slides/Awnings** - Can hit objects or people
- **Water Heater** - Fire hazard if tank is empty
- **Water Pump** - Can burn out without water supply
- **Jacks/Levelers** - Can cause damage or injury (requires ACC power + parking brake)
- **Generator** - Fuel and safety concerns

Always verify device names and confirm safe conditions before actuating.

## Future Work

- [ ] Leveler HA entities (protocol decoded, session management WIP)
- [ ] Dimming support
- [ ] "All Lights" master control
- [ ] Awning extend/retract (protocol known, safety concerns)

## Acknowledgments

- [RV-Bridge](https://github.com/rubillos/RV-Bridge) by Randy Ubillos
- RV-C specification by RVIA
- Lippert IDS library (decompiled for protocol details)

## License

MIT License - See [LICENSE](LICENSE)
