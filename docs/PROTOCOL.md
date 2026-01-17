# Lippert OneControl TCP Protocol

Detailed reverse-engineering notes for the Lippert OneControl TCP protocol on port 6969.

> **Status**: Monitoring works, control does NOT work yet. Full protocol structure understood from app decompilation.

## Network Setup

### OneControl WiFi Access Point

The OneControl system creates its own WiFi network:

```
SSID:     MyRV_<serial>        (e.g., MyRV_002630732020218F)
Security: WPA2-PSK
Gateway:  192.168.1.1
DHCP:     192.168.1.100+
```

### TCP Connection

- **Host**: `192.168.1.1`
- **Port**: `6969`
- **Protocol**: COBS-framed MyRvLink protocol
- **Timeouts**: Receive 8000ms, Send 4000ms, NoDelay=true

## Protocol Stack

```
[TCP/IP (port 6969)]
    └── [COBS Framing (0x00 delimiters)]
        └── [CRC-8 Validation]
            └── [MyRvLink Command/Event Messages]
```

## COBS Framing

All data is encoded using COBS (Consistent Overhead Byte Stuffing):

### Parameters (from decompiled app)

```csharp
CobsEncoder = new CobsEncoder(prependStartFrame: true, useCrc: true, frameByte: 0x00, numDataBits: 6);
CobsDecoder = new CobsDecoder(useCrc: true, frameByte: 0x00, numDataBits: 6);
```

- **Frame Delimiter**: `0x00`
- **Start Frame**: Prepended `0x00` byte before data
- **CRC**: Appended before COBS encoding
- **Max Data Bytes**: 63 per block (2^6 - 1)

### Frame Format

```
[0x00 Start][COBS-encoded payload + CRC][0x00 End]
```

### CRC-8 Algorithm

- **Polynomial**: `0x8C` (reflected) - CRC-8/MAXIM / DOW CRC
- **Initial Value**: `0x55`
- **Calculation**: Over raw payload before COBS encoding

```python
# Python implementation
CRC8_TABLE = [
    0x00, 0x5e, 0xbc, 0xe2, 0x61, 0x3f, 0xdd, 0x83,
    0xc2, 0x9c, 0x7e, 0x20, 0xa3, 0xfd, 0x1f, 0x41,
    0x9d, 0xc3, 0x21, 0x7f, 0xfc, 0xa2, 0x40, 0x1e,
    0x5f, 0x01, 0xe3, 0xbd, 0x3e, 0x60, 0x82, 0xdc,
    # ... (full 256-entry table)
]

def crc8(data, init=0x55):
    crc = init
    for byte in data:
        crc = CRC8_TABLE[(crc ^ byte) & 0xFF]
    return crc
```

## Message Types

### Events (Controller → App)

Events have the event type as the first byte:

| Type | Value | Description |
|------|-------|-------------|
| GatewayInformation | `0x01` | Connection info, protocol version |
| DeviceCommand | `0x02` | Command responses |
| DeviceOnlineStatus | `0x03` | Device online/offline |
| DeviceLockStatus | `0x04` | Lock states |
| RelayBasicLatchingStatusType1 | `0x05` | Relay status |
| RelayBasicLatchingStatusType2 | `0x06` | Relay status |
| RvStatus | `0x07` | RV overall status |
| DimmableLightStatus | `0x08` | Light brightness/state |
| RgbLightStatus | `0x09` | RGB light state |
| GeneratorGenieStatus | `0x0A` | Generator status |
| HvacStatus | `0x0B` | HVAC/thermostat |
| TankSensorStatus | `0x0C` | Tank levels |
| RealTimeClock | `0x20` | Time sync |
| CloudGatewayStatus | `0x21` | Cloud bridge state |
| MonitorPanelStatus | `0x2B` | Monitor panel |

### Commands (App → Controller)

Commands have the command type at byte index 2:

| Type | Value | Description |
|------|-------|-------------|
| GetDevices | `0x01` | Query device list |
| GetDevicesMetadata | `0x02` | Query device metadata |
| RemoveOfflineDevices | `0x03` | Remove offline devices |
| RenameDevice | `0x04` | Rename a device |
| SetRealTimeClock | `0x05` | Set time |
| ActionSwitch | `0x40` | Toggle on/off devices |
| ActionMovement | `0x41` | Movement commands |
| ActionDimmable | `0x43` | Dimmable light control |
| ActionRgb | `0x44` | RGB light control |
| ActionHvac | `0x45` | HVAC control |

## Command Format

### Base Command Structure

All commands follow this format (before COBS encoding):

```
[ClientCommandId (2 bytes, LE)][CommandType (1 byte)][Payload...]
```

- **ClientCommandId**: 16-bit counter (1-65534, skips 0 and 65535)
- **CommandType**: Enum value from MyRvLinkCommandType

### GetDevices Command (Type 0x01)

```
[ClientCommandId (2 LE)][0x01][DeviceTableId][StartDeviceId][MaxCount]
```

| Byte | Field | Description |
|------|-------|-------------|
| 0-1 | ClientCommandId | Request ID (LE) |
| 2 | CommandType | `0x01` |
| 3 | DeviceTableId | From GatewayInformation |
| 4 | StartDeviceId | Usually `0x00` |
| 5 | MaxCount | Usually `0xFF` (255) |

### ActionSwitch Command (Type 0x40)

```
[ClientCommandId (2 LE)][0x40][DeviceTableId][SwitchState][DeviceId...]
```

| Byte | Field | Description |
|------|-------|-------------|
| 0-1 | ClientCommandId | Request ID (LE) |
| 2 | CommandType | `0x40` |
| 3 | DeviceTableId | From GatewayInformation |
| 4 | SwitchState | `0x00`=Off, `0x01`=On |
| 5+ | DeviceIds | One or more device IDs |

### ActionDimmable Command (Type 0x43)

```
[ClientCommandId (2 LE)][0x43][DeviceTableId][DeviceId][LightCommand (8 bytes)]
```

| Byte | Field | Description |
|------|-------|-------------|
| 0-1 | ClientCommandId | Request ID (LE) |
| 2 | CommandType | `0x43` |
| 3 | DeviceTableId | From GatewayInformation |
| 4 | DeviceId | Target device |
| 5 | Command | `0`=Off, `1`=On, `127`=Restore |
| 6 | MaxBrightness | 0-255 |
| 7 | Duration | Fade duration |
| 8-9 | CycleTime1 | Blink cycle (MSB first) |
| 10-11 | CycleTime2 | Blink cycle (MSB first) |
| 12 | Reserved | Usually `0x00` |

### DimmableLightCommand Enum

| Value | Name | Description |
|-------|------|-------------|
| 0 | Off | Turn light off |
| 1 | On | Turn on to MaxBrightness |
| 2 | Blink | Blink mode |
| 3 | Swell | Pulse/swell mode |
| 4 | Settings | Configure settings |
| 127 | Restore | Restore last brightness |

## Event Format

### GatewayInformation (Type 0x01)

Sent by controller on connection:

```
[0x01][ProtocolVersion][Options][DeviceCount][DeviceTableId][DeviceTableCrc (4 LE)][MetadataCrc (4 LE)]
```

| Byte | Field | Description |
|------|-------|-------------|
| 0 | EventType | `0x01` |
| 1 | ProtocolVersion | Major version |
| 2 | Options | Flags (bit 0 = ConfigMode) |
| 3 | DeviceCount | Number of devices |
| 4 | DeviceTableId | ID for device queries |
| 5-8 | DeviceTableCrc | Device list checksum (LE) |
| 9-12 | MetadataCrc | Metadata checksum (LE) |

### DimmableLightStatus (Type 0x08)

```
[0x08][DeviceTableId][DeviceId][Brightness][State][Flags...]
```

## Connection Flow

1. **Connect** to `192.168.1.1:6969`
2. **Receive** GatewayInformation event (type `0x01`)
   - Extract `DeviceTableId`, `DeviceCount`, `DeviceTableCrc`
3. **Send** GetDevices command (type `0x01`)
   - Use `DeviceTableId` from step 2
4. **Receive** Device list responses
5. **Send** GetDevicesMetadata (type `0x02`) if CRC changed
6. **Control** devices using ActionSwitch/ActionDimmable commands

## Device Tables

The controller organizes devices into multiple "tables":

| Table ID | Description | Data Format |
|----------|-------------|-------------|
| 0x00 | Dimmable lights | Simple status broadcast |
| 0x02 | Device configuration | Extended device info |
| 0x03 | Unknown (limited devices) | Sparse |
| 0x07 | CAN address mapping | Contains `FE XX` addresses |

### Table 0x00 - Dimmable Light Status

Example: `08 00 28 02 14 00 00 00 08 91 5d`

- `08` = EventType (DimmableLightStatus)
- `00` = TableId  
- `28` = DeviceId (Bed Ceiling)
- Remaining bytes = status data

### Table 0x07 - CAN Address Mapping

Contains the mapping between logical device IDs and physical CAN addresses:

Example: `08 07 C4 30 FE 72 47 08 17 00 01`

- Contains `FE XX` pattern which is RV-C source address format
- Device 0xC4 maps to CAN address `FE 72`

## Known Devices (User's RV)

| Device ID | Name | Table 0x00 | Table 0x02 |
|-----------|------|------------|------------|
| 0x28 | Bed Ceiling | Present | Present |
| 0x21 | Kitchen | NOT in broadcasts | Ref'd in other devices |
| 0x41 | Unknown (lit) | Has brightness | Present |

**Mystery**: Kitchen light (0x21) does not appear in Table 0x00 broadcasts but is referenced in Table 0x02 data of other devices.

## Registration Packet

The app sends a special packet on connection (decoded):

```
08 00 00 9D 1C 88 43 4F AF 67 82
```

- `08 00` = ClientCommandId (0x0008)
- `00` = CommandType (Unknown/Registration?)
- `9D` = Session ID (app-generated)
- `1C 88 43 4F AF 67 82` = App identifier (7 bytes)

This may be required before control commands are accepted.

## Known Issues

### Control Commands Not Working

Despite having correct protocol structure:

1. Commands are COBS-encoded correctly ✓
2. CRC-8 validates correctly ✓
3. Controller silently accepts messages (no error response)
4. **But lights do not change state**

### Theories Under Investigation

1. **Missing session/auth**: The registration packet may establish a session that must be used
2. **Wrong device addressing**: Table 0x00 device IDs may not be directly controllable
3. **CAN routing**: May need to target via Table 0x07 CAN addresses
4. **Touch panel mediation**: Commands may need to route through the physical touch panel

### What Works

- ✓ Monitoring device status (all events decoded correctly)
- ✓ Receiving GatewayInformation
- ✓ Parsing device tables
- ✓ CRC validation

### What Doesn't Work (Yet)

- ✗ ActionSwitch commands (no response, no effect)
- ✗ ActionDimmable commands (no response, no effect)
- ✗ Any control commands

## Debug Checklist

When testing control commands:

1. [ ] Verify COBS encoding matches app behavior
2. [ ] Check ClientCommandId is unique (1-65534)
3. [ ] Confirm DeviceTableId from GatewayInformation
4. [ ] Try registration packet first
5. [ ] Test both Table 0x00 device IDs and Table 0x07 CAN addresses
6. [ ] Compare exact bytes to captured app traffic

## References

- [RV-C Specification](http://www.rv-c.com/) - Official standard
- [RV-Bridge](https://github.com/rubillos/RV-Bridge) - ESP32 RV-C to HomeKit
- [CoachProxyOS](https://github.com/linuxkidd/coachproxyos) - RV monitoring
