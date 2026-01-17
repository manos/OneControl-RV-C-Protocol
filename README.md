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
- **Protocol**: TCP on port `6969` (heh)

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

### Lippert TCP Protocol (Port 6969)

The TCP protocol uses **COBS (Consistent Overhead Byte Stuffing)** framing with **CRC-8** checksum, wrapping **MyRvLink** commands.

#### Protocol Stack

```
[TCP/IP (port 6969)]
    └── [COBS Framing (0x00 delimiters, 6-bit blocks)]
        └── [CRC-8 Validation (poly=0x8C reflected, init=0x55)]
            └── [MyRvLink Command/Event Messages]
```

#### COBS Parameters (from decompiled app)

```python
# CobsEncoder(prependStartFrame=true, useCrc=true, frameByte=0, numDataBits=6)
FRAME_BYTE = 0x00      # Frame delimiter
NUM_DATA_BITS = 6      # Max 63 data bytes per block
CRC8_POLY = 0x8C       # Reflected polynomial (CRC-8/MAXIM)
CRC8_INIT = 0x55       # Initial CRC value
```

#### MyRvLink Command Structure

```
[ClientCommandId (2 bytes, LE)] [CommandType (1 byte)] [Payload...]
```

| Field | Size | Description |
|-------|------|-------------|
| ClientCommandId | 2 bytes | Incrementing command ID (little-endian) |
| CommandType | 1 byte | Command type (see MyRvLinkCommandType enum) |
| Payload | Variable | Command-specific data |

#### Command Types (MyRvLinkCommandType)

| Value | Name | Description |
|-------|------|-------------|
| 1 | GetDevices | Request device list |
| 2 | GetDevicesMetadata | Request device metadata |
| 64 | ActionSwitch | On/off switch control |
| 65 | ActionMovement | Movement control (slides, awnings) |
| 67 | ActionDimmable | Dimmable light control |
| 68 | ActionRgb | RGB light control |
| 69 | ActionHvac | HVAC control |

#### ActionDimmable Command (Type 67)

```
[CmdId (2B LE)] [67] [TableId] [DeviceId] [LightCommand (8 bytes)]
```

**LightCommand Structure:**
```
Byte 0: Command (0=Off, 1=On, 127=Restore)
Byte 1: MaxBrightness (0-255)
Byte 2: Duration
Bytes 3-4: CycleTime1 (big-endian)
Bytes 5-6: CycleTime2 (big-endian)
Byte 7: Reserved (0x00)
```

#### Example: Light ON Command

**Raw command (before COBS+CRC):**
```
00 00    # ClientCommandId = 0 (little-endian)
43       # CommandType = ActionDimmable (67)
00       # DeviceTableId
21       # DeviceId (e.g., Kitchen light)
01       # Command = On
C8       # MaxBrightness = 200 (~78%)
00       # Duration = 0
00 00    # CycleTime1 = 0
00 00    # CycleTime2 = 0
00       # Reserved
```

**After COBS+CRC encoding:**
```
00 80 41 43 C3 21 01 C8 C0 01 6B 00
│                              │
└── Frame start                └── Frame end
```

#### Example: Light OFF Command

**Raw command:**
```
01 00 43 00 21 00 00 00 00 00 00 00 00
```

**Encoded:**
```
00 41 01 43 C1 21 C0 80 01 2E 00
```

#### DimmableLightCommand Values

| Value | Name | Description |
|-------|------|-------------|
| 0 | Off | Turn light off |
| 1 | On | Turn on to MaxBrightness |
| 2 | Blink | Blink mode |
| 3 | Swell | Pulse/swell mode |
| 4 | Settings | Configure settings |
| 127 | Restore | Restore to last brightness |

> ⚠️ **Testing Required** - The protocol has been reverse-engineered from app decompilation but needs testing against real hardware.
> 
> **What we know:**
> - COBS framing with CRC-8 (poly=0x8C, init=0x55) ✓
> - MyRvLink command structure ✓
> - ActionDimmable command format ✓
> 
> **What we need to discover:**
> - Correct DeviceTableId and DeviceId values for your RV
> - Device discovery (GetDevices command response format)
> - Event/status message parsing

### Alternative: CAN Bus Direct Access

If TCP control remains elusive, direct CAN bus access is possible:

| Adapter | Price | Interface |
|---------|-------|-----------|
| CANable Pro | ~$40 | USB (slcan) |
| Waveshare CAN HAT | ~$20 | Raspberry Pi SPI |
| ESP32 + SN65HVD230 | ~$10 | Built-in CAN controller |
| MCP2515 module | ~$5 | SPI (any microcontroller) |

CAN bus uses **RV-C** protocol directly - see DGN definitions above.

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
│   ├── __init__.py         # Package exports
│   ├── dgn.py              # DGN definitions and enums
│   ├── decoder.py          # RV-C message decoder
│   ├── client.py           # TCP client for OneControl
│   ├── commands.py         # Command builder
│   ├── cobs.py             # COBS encoder/decoder
│   └── lippert_protocol.py # Lippert framing layer
├── docs/
│   └── PROTOCOL.md         # Detailed protocol documentation
├── tools/
│   ├── analyze_capture.py  # Packet capture analyzer
│   ├── decode_capture.py   # TCP message decoder
│   ├── extract_assemblies.py # .NET assembly extractor
│   └── decompile_notes.md  # App decompilation findings
├── examples/
│   └── monitor.py          # Example monitoring script
└── pixi.toml               # Pixi package manager config
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

### High Priority
1. **🔓 Crack TCP control** - Figure out why commands don't actuate devices
   - Try different init sequences
   - Find authentication/pairing mechanism
   - Test with Wireshark on actual phone
2. **🔌 CAN bus implementation** - Direct RV-C control as backup

### Medium Priority
3. **📝 Protocol documentation** - Complete message type catalog
4. **🏠 Home Assistant integration** - Native component (once control works)
5. **📱 Reverse engineer iOS app** - Decompile LippertConnect for secrets

### Research Needed
- Does the Cloud Bridge use a different protocol?
- Is there an MQTT or REST API hidden somewhere?
- What role does the touch panel play in command routing?

## Investigation Log

### What Works ✅
- WiFi connection to OneControl AP (`MyRV_*` network)
- TCP connection to port 6969
- Receiving continuous status broadcasts
- Identifying device instances from traffic (Kitchen=`0x28`, etc.)
- Capturing and analyzing app traffic via tcpdump
- Distinguishing polling vs command messages
- Phone app works with Bluetooth OFF (confirms WiFi control)
- **Registration packet exchange** - Controller responds to our registration
- **CRC-8 calculation verified** - Polynomial 0x8C (reflected), init 0x55
- **COBS decoding/encoding** - Correctly parse captured messages

### What Doesn't Work ❌
- **Any control command format we tried** - 100+ variations tested
- Exact byte-for-byte replay of captured app commands (different session)
- Commands with correct CRC but different session ID
- Different device addresses (`fe64`, `0x28`, `7d28`, etc.)

### Key Findings

1. **No cloud traffic** - Phone talks ONLY to `192.168.1.1:6969`, no DNS queries
2. **Bluetooth not required** - App works with phone BT disabled
3. **Commands send, no response action** - Controller sends back status stream, but devices unchanged
4. **App is Xamarin/.NET** - Decompiled LippertConnect, extracted 430 .NET assemblies
5. **COBS encoding VERIFIED** - Matches decompiled `CobsEncoder(true, true, 0, 6)`
6. **CRC-8/MAXIM VERIFIED** - Calculated CRC matches captured packets exactly

### Protocol Deep Dive (New!)

#### Registration Sequence
```
App → Controller: 00 81 08 09 [SessionID] 1c 88 43 4f af 67 82 [CRC] 00
Controller → App: 00 81 18 09 [SessionID] 1c 88 43 4f af 67 82 [CRC] 00
```
- `81` = Registration message type
- `08`/`18` = Request/Response indicator
- `434faf67` = Appears to be a device/app identifier (consistent across sessions)
- Session ID (e.g., `9d`, `30`, `32`) changes per connection

#### Toggle Command Format (COBS-decoded)
```
Raw payload: 08 07 [SS] [SS] fe 64 86 00 01 00 00
            │  │    │    │   │    │  │   │
            │  │    │    │   │    │  │   └─ ON command (01=On, 02=Off?)
            │  │    │    │   │    │  └───── Zero byte
            │  │    │    │   │    └──────── Counter/sequence (86, 87, 88...)
            │  │    │    │   └───────────── Device address (fe64)
            │  │    │    └────────────────── Session ID (repeated)
            │  │    └─────────────────────── Session ID
            │  └──────────────────────────── Message type (07)
            └─────────────────────────────── Header byte (08)
```

COBS-encoded over wire:
```
00 47 08 07 [SS] [SS] fe 64 86 81 01 01 [CRC] 00
   │
   └── Code byte: 7 data bytes + 1 zero (7 + 64 = 71 = 0x47)
```

#### CRC Verification Success ✅
```python
# Captured: 47 08 07 30 30 fe 64 86 81 01 01 a5
# Decoded raw: 08 07 30 30 fe 64 86 00 01 00 00
# Calculated CRC: 0xA5
# Captured CRC: 0xA5  ← MATCH!
```

### Remaining Mysteries

1. **Why don't our commands work?**
   - We can register, we can calculate correct CRC
   - But toggle commands with our session ID don't actuate devices
   - Possible: Device address `fe64` is session/device specific

2. **Device address discovery**
   - Captured toggle uses `fe 64` - what does this map to?
   - Kitchen light is instance `0x28` but toggle doesn't use `28` directly
   - Need to understand the `fe XX` address format

3. **Second session phenomenon**
   - First connection (port 60477) gets session `32`
   - App opens second connection (port 60480) gets session `30`
   - Toggle commands go on second connection with `30`
   - Maybe we need multiple connections?

### Tested Command Formats (Expanded)

| Format | Raw Payload | Wire Bytes | Result |
|--------|-------------|------------|--------|
| Toggle (session 30) | `08 07 30 30 fe 64 86 00 01 00 00` | `00 47 08 07 30 30 fe 64 86 81 01 01 a5 00` | App works |
| Toggle (session 9d) | `08 07 9d 9d fe 64 86 00 01 00 00` | `00 47 08 07 9d 9d fe 64 86 81 01 01 78 00` | No effect |
| ActionSwitch | `01 00 40 00 01 28` | COBS encoded | No effect |
| ActionDimmable | `00 00 43 00 21 01 c8 00 00 00 00 00 00` | COBS encoded | No effect |
| Registration | `09 9d 1c 88 43 4f af 67 82` | `00 81 08 09 9d...71 00` | Response! |

### Next Steps

1. **Capture full multi-connection session** - App may use separate connections for control vs. status
2. **Understand `fe XX` addressing** - How does `fe 64` relate to device `0x28` (Kitchen)?
3. **Try CAN bus direct** - TCP may truly be read-only; CAN might be required for control
4. **iOS app decompilation** - Android app may have obfuscated secrets; try iOS

## License

MIT License - See [LICENSE](LICENSE)

## Acknowledgments

- [RV-Bridge](https://github.com/rubillos/RV-Bridge) by Randy Ubillos - DGN definitions reference
- RV-C specification by RVIA
- The RV community for protocol insights
