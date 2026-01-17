# Lippert OneControl TCP Protocol

Reverse-engineered documentation of the Lippert OneControl proprietary TCP protocol.

## Network Overview

```
┌─────────────────┐     WiFi (192.168.1.x)    ┌──────────────────┐
│  LippertConnect │◄─────────────────────────►│  OneControl      │
│  App (iPhone)   │      TCP port 6969        │  Controller      │
│  10.2.0.162     │                           │  192.168.1.1     │
└─────────────────┘                           └────────┬─────────┘
                                                       │ CAN Bus
                                              ┌────────┴─────────┐
                                              │  RV-C Network    │
                                              │  (Lights, HVAC,  │
                                              │   Tanks, etc.)   │
                                              └──────────────────┘
```

## WiFi Access Point

- **SSID**: `MyRV_<serial>` (e.g., `MyRV_002630732020218F`)
- **Security**: WPA2-PSK
- **Password**: Device-specific (found on unit or documentation)
- **DHCP Range**: `10.2.0.x` for clients, `192.168.1.1` for controller

## TCP Protocol (Port 6969)

### Frame Format

All messages use a common framing format:

```
[00] [type] [len] [83 dc] [instance] [cmd] [subcmd] [data...] [00]
```

| Field | Size | Description |
|-------|------|-------------|
| Frame Start | 1 byte | Always `0x00` |
| Type | 1 byte | Message type (see below) |
| Length/Marker | 1 byte | Typically `0x02` for short commands |
| Magic | 2 bytes | Always `0x83 0xDC` |
| Instance | 1 byte | Device instance number (0x00-0xFF) |
| Command | 1 byte | Command type |
| Subcommand | 1 byte | Command subtype |
| Data | variable | Command-specific data |
| Terminator | 1 byte | Usually `0x00` |

### Message Types

| Type | Name | Description |
|------|------|-------------|
| `0x40` | STATUS_SHORT | Short status message (3-4 bytes data) |
| `0x41` | STATUS_8 | 8-byte status message |
| `0x43` | MULTI_CMD | Multi-byte command/status |
| `0x45` | SET_VALUE | Set value command (with 16-bit value) |
| `0x49` | HEARTBEAT | Periodic heartbeat/sync |
| `0x85` | TOGGLE | Toggle/on/off command |
| `0xC3` | DEVICE_STATUS | Device status report |
| `0xC5` | DEVICE_CONFIG | Device configuration status |

### Commands

#### Toggle Light (0x85)

Toggle a light on or off:

```
00 85 02 83 dc <instance> 30 01 aa 00
```

Example - Toggle instance 0xC4:
```
00 85 02 83 dc c4 30 01 aa 00
```

Response:
```
00 85 12 83 dc c4 30 01 <status> 00 00 ...
```

#### Set Dimmer Value (0x45)

Set a dimmer to a specific brightness:

```
00 45 02 83 dc <instance> 30 02 <val_lo> <val_hi> 00
```

| Field | Description |
|-------|-------------|
| instance | Device instance (0x00-0xFF) |
| val_lo | Low byte of value |
| val_hi | High byte of value |

Value range observed: 0-1000 (0x0000-0x03E8) maps to 0-100%

Example - Set instance 0xC4 to 50% (value 500 = 0x01F4):
```
00 45 02 83 dc c4 30 02 f4 01 00
```

#### Request Status (0x45 with 0x04)

```
00 45 02 83 dc <instance> 04 11 02 2b af 00
```

### Status Messages

#### Device Status (0xC5)

Periodic status updates from controller:

```
00 c5 06 03 <device_class> 80 ff 40 01 <value> 00
```

Example values observed:
- `c5 06 03 77 80 ff 40 01 13 00` - Device 0x77, value 0x13
- `c5 06 03 3f c0 ff 40 01 50 00` - Device 0x3F, value 0x50

#### Node Status (0x41)

8-byte status for network nodes:

```
00 41 08 c3 <node_hi> <node_lo> 02 14 04 08 91 5d <status>
```

### Heartbeat/Keepalive

The app sends periodic 53-byte keepalive messages:

```
00 43 01 06 f7 01 f0 00 00
   41 08 41 f7 08 1c 88 43 4f af 67 82 3c 00 00
   43 08 02 f7 43 2f f7 17 81 02 01 44 00 00
   c3 04 01 f7 40 01 c1 00 00
   40 03 03 f7 ec 00
```

## Instance Numbers

Instance numbers are RV-specific and identify individual devices. They need to be
discovered by monitoring traffic or from RV documentation.

Observed in testing:
- `0xC4` (196) - A dimmable light
- `0xFB` (251) - Another light/device
- `0x28` (40) - ?
- `0x15` (21) - ?

## Relationship to RV-C

The OneControl system uses RV-C (Recreational Vehicle CAN) internally on the 
CAN bus, but the TCP protocol is proprietary framing. The instance numbers
appear to map to RV-C device instances.

Standard RV-C DGNs (Data Group Numbers) may be referenced internally:
- `0x1FEDA` - DC_DIMMER_STATUS_3 
- `0x1FEDB` - DC_DIMMER_COMMAND_2
- `0x1FFB7` - TANK_STATUS
- etc.

See `rvc/dgn.py` for a complete list of known DGNs.

## Example Session

```python
import socket

HOST, PORT = "192.168.1.1", 6969

# Connect
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
sock.connect((HOST, PORT))

# Toggle light instance 0xC4
toggle = bytes([0x00, 0x85, 0x02, 0x83, 0xdc, 0xc4, 0x30, 0x01, 0xaa, 0x00])
sock.send(toggle)

# Read response
response = sock.recv(1024)
print(f"Response: {response.hex()}")

sock.close()
```

## Tools

### Capture Traffic

```bash
# On a device connected to OneControl WiFi:
sudo tcpdump -i wlan0 -n -X port 6969
```

### Analyze Captures

```bash
python tools/analyze_capture.py --raw "00850283dcc43001aa00"
```

## References

- [RV-C Specification](http://www.rv-c.com/) - RVIA
- [RV-Bridge](https://github.com/rubillos/RV-Bridge) - ESP32 HomeKit bridge with DGN definitions
- [eRVin](https://github.com/linuxkidd/ervin) - Raspberry Pi RV-C gateway
