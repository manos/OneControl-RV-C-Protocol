# OneControl RV-C Protocol

Reverse-engineered protocol documentation and Python client for **Lippert OneControl** RV systems.

> **Status**: Light control, tank sensors, and generator hour meter working. Auto-discovery available.

## Features

| Feature | Status |
|---------|--------|
| **Light Control** | ✅ ON/OFF for all discovered lights |
| **Tank Sensors** | ✅ Fresh, Grey, Black, LP levels |
| **Battery Voltage** | ✅ 12V system voltage |
| **Generator Hours** | ✅ Hour meter reading |
| **Auto-Discovery** | ✅ No packet capture needed |

## Quick Start

```python
import asyncio
from rvc.onecontrol import control_light, LightConfig

# Define a light using universal values + discovered counter
MY_LIGHT = LightConfig(
    name="Kitchen",
    protocol=0x80,
    session=0x80,
    device=0x04,
    conn=0x40,
    counter=0x28,  # From auto_discover.py
)

async def main():
    await control_light(MY_LIGHT, on=True)
    await asyncio.sleep(2)
    await control_light(MY_LIGHT, on=False)

asyncio.run(main())
```

## Installation

```bash
git clone https://github.com/your-repo/OneControl-RV-C-Protocol.git
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
  Awning Light                   counter=0x15
  Bed Ceiling Light              counter=0xFB
  Kitchen Ceiling Light          counter=0x28
  Living Room Ceiling Light      counter=0x77
  Porch Light                    counter=0xCF
  Scare Light                    counter=0x86
```

## Usage

### Control Lights

```python
from rvc.onecontrol import control_light, LightConfig

# Universal values work for all lights
light = LightConfig(
    name="My Light",
    protocol=0x80,
    session=0x80,
    device=0x04,
    conn=0x40,
    counter=0xXX,  # From discovery
)

await control_light(light, on=True)
await control_light(light, on=False)
```

### Read Tank Levels

```python
from rvc.onecontrol import read_tank_levels

levels = await read_tank_levels()
print(f"Fresh: {levels.get('Fresh Tank', 'N/A')}%")
print(f"Grey: {levels.get('Grey Tank', 'N/A')}%")
print(f"Black: {levels.get('Black Tank', 'N/A')}%")
print(f"LP: {levels.get('LP Tank', 'N/A')}%")
```

### Read Battery Voltage

```python
from rvc.onecontrol import read_battery_voltage

voltage = await read_battery_voltage()
if voltage:
    print(f"Battery: {voltage:.2f}V")
```

### Read Generator Hours

```python
from rvc.onecontrol import read_generator_hours

hours = await read_generator_hours()
if hours:
    print(f"Generator: {hours:.1f} hours")
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
                                       │  (Lights, HVAC)  │
                                       └──────────────────┘
```

## Protocol Overview

Each control command requires:
1. Fresh TCP connection
2. Session registration
3. Seed/key authentication (TEA cipher)
4. Control command
5. Connection close

See [docs/PROTOCOL.md](docs/PROTOCOL.md) for full technical details.

## Project Structure

```
OneControl-RV-C-Protocol/
├── rvc/
│   ├── onecontrol.py      # Main client - USE THIS
│   └── cobs.py            # COBS encoding
├── tools/
│   └── auto_discover.py   # Device discovery
├── docs/
│   ├── PROTOCOL.md        # Technical protocol docs
│   └── ANALYSIS.md        # Reverse-engineering notes
└── captures/              # Packet captures (reference)
```

## Safety Warning

⚠️ **Use caution with non-light devices!**

- **Slides/Awnings** - Can hit objects or people
- **Water Heater** - Fire hazard if tank is empty
- **Jacks/Levelers** - Can cause damage or injury
- **Generator** - Fuel and safety concerns

Always verify device names and confirm safe conditions before actuating.

## Future Work

- [ ] Home Assistant integration
- [ ] Dimming support
- [ ] Status polling
- [ ] Generator start/stop control

## Acknowledgments

- [RV-Bridge](https://github.com/rubillos/RV-Bridge) by Randy Ubillos
- RV-C specification by RVIA
- Lippert IDS library (decompiled for protocol details)

## License

MIT License - See [LICENSE](LICENSE)
