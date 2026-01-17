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

The TCP protocol uses a **proprietary Lippert framing**, not raw RV-C CAN frames. Based on extensive traffic analysis:

#### Message Structure

```
[00] [Type] [Len?] [Magic] [Instance] [Cmd] [Data...] [00]
```

| Byte | Description |
|------|-------------|
| `00` | Start delimiter |
| Type | Message type: `40`, `41`, `43`, `45`, `85`, `C3`, `C5` |
| Len? | Length marker or sub-type (often `02`) |
| Magic | Two magic bytes: `80 84`, `80 86`, or `83 dc` |
| Instance | Device instance ID (1 byte) |
| Cmd | Command byte (e.g., `11`=status, `42`=set) |
| Data | Variable length payload |
| `00` | End delimiter |

#### Message Types (Observed)

| Type | Direction | Description |
|------|-----------|-------------|
| `0x40` | Both | Device state / short status |
| `0x41` | Both | Device registration / node info |
| `0x43` | Both | Multi-part messages / queries |
| `0x45` | Both | Status request (`11`) or Set value (`42`) |
| `0x85` | Controller→App | Status broadcast |
| `0xC3` | Controller→App | Configuration data |
| `0xC5` | Controller→App | Extended status |

#### Example Messages

**Status Poll** (App → Controller):
```
00 45 02 80 84 04 11 02 2b 5d 00
   │     │     │  │  │  └────── Checksum?
   │     │     │  │  └───────── Status request
   │     │     │  └──────────── Instance 0x04
   │     │     └─────────────── Magic bytes
   │     └───────────────────── Length marker
   └─────────────────────────── Type 0x45
```

**Device Registration** (App → Controller):
```
00 41 08 41 21 08 1c 88 43 4f af 67 82 02 00 00
         │        └──────────────────── Device ID
         └─────────────────────────────── Instance 0x21
```

**Set Command** (Captured but not yet working):
```
00 45 02 80 84 28 42 02 04 c2 00
               │  │  │  └────── Value (little-endian)
               │  │  └───────── Data length
               │  └──────────── Set command
               └─────────────── Instance 0x28
```

#### Device Instances (Example RV)

| Instance | Device | Magic Bytes |
|----------|--------|-------------|
| `0x04` | System poll target | `83 ac` |
| `0x28` | Light (toggle type) | `83 ac`, `83 ae` |
| `0xEB` | Kitchen light | `83 ac` |
| `0xEC` | System poll target | `83 ac` |
| `0xF2` | Unknown device | `83 7d` |

#### Multiple Magic Byte Sequences

Different operations use different magic bytes:

| Magic | Usage |
|-------|-------|
| `80 84` | Legacy/alternate format |
| `80 86` | Alternate control format |
| `83 ac` | Primary polling/status |
| `83 ae` | Control commands (type 40) |
| `83 7d` | Device configuration |

#### Observed Command Patterns

**App Polling Sequence** (sent every ~1 second):
```
00 45 02 83 ac 04 11 02 2b c2 00   # Poll instance 0x04
00 43 01 06 eb 01 51 00            # Query instance 0xEB
```

**Device Registration** (sent on connect):
```
00 41 08 41 eb 08 1c 88 43 4f af 67 82 79 00
         │        └──────────────────────── Device ID (unique per app)
         └───────────────────────────────── Instance
```

**Control Command Candidates** (captured but not working):
```
00 45 02 83 ac 28 42 02 04 5d 00   # SET for instance 0x28, value 0x045d
00 40 05 83 ae 28 01 fd 00         # Type 40 control, value 0xfd
00 45 06 03 28 81 ff 81 95 01 47 00  # Extended format
```

> ⚠️ **TCP Control Not Working** - After extensive testing, the TCP interface appears to be **read-only** or requires unknown authentication.
> 
> **Evidence:**
> - Commands receive valid responses (status broadcasts continue)
> - Response shows current state, not changed state
> - App traffic captured but exact replay doesn't actuate
> - No visible difference between our commands and app commands
> 
> **Likely Cause:** The TCP interface is for **monitoring only**. Actual control probably happens via:
> - Direct CAN bus commands
> - Bluetooth (though app works with BT off, so unlikely)
> - Unknown authentication/session mechanism

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
- Identifying device instances from traffic
- Capturing and analyzing app traffic via tcpdump
- Distinguishing polling vs command messages
- Phone app works with Bluetooth OFF (confirms WiFi control)

### What Doesn't Work ❌
- **Any control command format we tried** - 50+ variations tested
- Exact byte-for-byte replay of captured app commands
- Different magic byte combinations (`80 84`, `83 ac`, `83 ae`)
- Type `40`, `43`, `45` command formats
- Various value encodings (little-endian, big-endian)

### Key Findings

1. **No cloud traffic** - Phone talks ONLY to `192.168.1.1:6969`, no DNS queries
2. **Bluetooth not required** - App works with phone BT disabled
3. **Commands acknowledged** - Controller sends back status, but state unchanged
4. **Multiple magic bytes** - Different operations use `83 ac`, `83 ae`, `80 84`, etc.
5. **Device ID in registration** - `43 4f af 67 82` appears to be app's unique ID
6. **App is Xamarin/.NET** - Decompiled LippertConnect, extracted 430 .NET assemblies
7. **COBS encoding** - Protocol uses COBS (Consistent Overhead Byte Stuffing) framing
8. **CRC validation** - CRC-8 or CRC-32 LE checksums used, algorithm still unknown
9. **Nonce/counter byte** - Commands include a varying byte that affects checksum

### Tested Command Formats

| Format | Example | Result |
|--------|---------|--------|
| `45/42 SET` | `00 45 02 83 ac 28 42 02 04 5d 00` | Response, no change |
| `43/06 QUERY` | `00 43 01 06 eb 01 51 00` | Response, no change |
| `40/05 CTRL` | `00 40 05 83 ae 28 01 fd 00` | Response, no change |
| `45/06 EXT` | `00 45 06 03 28 81 ff 81 95 01 47 00` | Response, no change |

### Theories (Updated)

1. ~~Missing session token~~ - Registration appears to work
2. **CAN-only control** - TCP may be read-only, control via CAN bus
3. **Checksum/nonce** - Values like `2b c2`, `51`, `fd` may be computed
4. **App signing** - Commands may be cryptographically signed

## License

MIT License - See [LICENSE](LICENSE)

## Acknowledgments

- [RV-Bridge](https://github.com/rubillos/RV-Bridge) by Randy Ubillos - DGN definitions reference
- RV-C specification by RVIA
- The RV community for protocol insights
