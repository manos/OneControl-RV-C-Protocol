# Lippert OneControl TCP Protocol

Detailed reverse-engineering notes for the Lippert OneControl TCP protocol on port 6969.

> **Status**: Monitoring works, control does NOT work. See [Investigation Results](#investigation-results).

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
- **Protocol**: Custom binary (NOT HTTP, NOT raw RV-C)

## Message Format

### General Structure

All messages follow this pattern:

```
[00] [Type] [Sub] [Magic...] [Instance] [Cmd] [Data...] [00]
 │     │      │       │          │        │       │       │
 │     │      │       │          │        │       │       └── End delimiter
 │     │      │       │          │        │       └────────── Variable payload
 │     │      │       │          │        └────────────────── Command byte
 │     │      │       │          └─────────────────────────── Device instance
 │     │      │       └────────────────────────────────────── 2-byte magic
 │     │      └────────────────────────────────────────────── Sub-type/length
 │     └───────────────────────────────────────────────────── Message type
 └─────────────────────────────────────────────────────────── Start delimiter
```

### Message Types

| Type | Hex | Direction | Description |
|------|-----|-----------|-------------|
| STATUS | `0x40` | Both | Short device state |
| NODE | `0x41` | Both | Device registration/node info |
| MULTI | `0x43` | Both | Multi-part query/response |
| SET | `0x45` | Both | Status poll or set value |
| BROADCAST | `0x85` | Ctrl→App | Status broadcast |
| CONFIG | `0xC3` | Ctrl→App | Configuration data |
| EXT_STATUS | `0xC5` | Ctrl→App | Extended status |

### Magic Byte Sequences

Different operations use different 2-byte magic sequences:

| Magic | Hex | Observed Usage |
|-------|-----|----------------|
| `80 84` | Legacy | Older command format |
| `80 86` | Alternate | Alternative control |
| `83 ac` | Primary | Polling, status requests |
| `83 ae` | Control | Type 40 control commands |
| `83 7d` | Config | Device configuration |
| `80 a1` | Extended | Extended status messages |
| `80 70` | System | System-level messages |

## Captured Message Examples

### Polling Messages (App → Controller)

**Status Poll** (sent every ~1 second):
```
00 45 02 83 ac 04 11 02 2b c2 00
│  │  │  │     │  │  │  │     │
│  │  │  │     │  │  │  │     └── End
│  │  │  │     │  │  │  └──────── Checksum/counter: 0x2bc2
│  │  │  │     │  │  └─────────── Data length: 2
│  │  │  │     │  └────────────── Command: 0x11 (status request)
│  │  │  │     └───────────────── Instance: 0x04
│  │  │  └─────────────────────── Magic: 83 ac
│  │  └────────────────────────── Sub-type: 02
│  └───────────────────────────── Type: 45
└──────────────────────────────── Start
```

**Device Query**:
```
00 43 01 06 eb 01 51 00
│  │  │  │  │  │  │  │
│  │  │  │  │  │  │  └── End
│  │  │  │  │  │  └───── Value: 0x51 (81 decimal)
│  │  │  │  │  └──────── Sub-command: 01
│  │  │  │  └─────────── Instance: 0xEB (235)
│  │  │  └────────────── Command: 06
│  │  └───────────────── Sub-type: 01
│  └──────────────────── Type: 43
└─────────────────────── Start
```

### Registration Messages

**App Registration** (sent on connection):
```
00 41 08 41 eb 08 1c 88 43 4f af 67 82 79 00
│  │  │  │  │  │              │           │
│  │  │  │  │  │              │           └── End
│  │  │  │  │  │              └────────────── Device ID: 434faf6782
│  │  │  │  │  └───────────────────────────── Unknown: 1c 88
│  │  │  │  └──────────────────────────────── Instance: 0xEB
│  │  │  └─────────────────────────────────── Sub-command: 41
│  │  └────────────────────────────────────── Length: 08
│  └───────────────────────────────────────── Type: 41
└──────────────────────────────────────────── Start
```

### Control Commands (Captured from App)

**SET Command** (type 45, cmd 42):
```
00 45 02 83 ac 28 42 02 04 5d 00
│  │  │  │     │  │  │  │     │
│  │  │  │     │  │  │  │     └── End
│  │  │  │     │  │  │  └──────── Value: 0x045d (1117) little-endian
│  │  │  │     │  │  └─────────── Data length: 02
│  │  │  │     │  └────────────── Command: 42 (SET)
│  │  │  │     └───────────────── Instance: 0x28
│  │  │  └─────────────────────── Magic: 83 ac
│  │  └────────────────────────── Sub-type: 02
│  └───────────────────────────── Type: 45
└──────────────────────────────── Start
```

**Type 40 Control** (different magic):
```
00 40 05 83 ae 28 01 fd 00
│  │  │  │     │  │  │  │
│  │  │  │     │  │  │  └── End
│  │  │  │     │  │  └───── Value: 0xfd (253)
│  │  │  │     │  └──────── Sub-command: 01
│  │  │  │     └─────────── Instance: 0x28
│  │  │  └────────────────── Magic: 83 ae
│  │  └───────────────────── Sub-type: 05
│  └──────────────────────── Type: 40
└─────────────────────────── Start
```

### Status Responses (Controller → App)

**Status for instance 0x28**:
```
00 45 12 83 ac 28 42 02 04 a6 00
                          │
                          └── Current value: 0x04a6 (1190)
```

**Extended Status**:
```
00 45 06 03 28 81 ff 81 95 01 47 00
              │           │     │
              │           │     └── Unknown
              │           └──────── State: 0x95 (149)
              └──────────────────── Instance: 0x28
```

## Device Instances

Observed device instances from captured traffic:

| Instance | Hex | Device Type | Notes |
|----------|-----|-------------|-------|
| 4 | `0x04` | System | Polled frequently |
| 5 | `0x05` | Unknown | Status broadcasts |
| 14 | `0x0E` | Unknown | |
| 40 | `0x28` | Light | Bed Ceiling (toggle) |
| 235 | `0xEB` | Light | Kitchen (toggle) |
| 236 | `0xEC` | System | Polled frequently |
| 242 | `0xF2` | Unknown | Configuration |

## Investigation Results

### What We Verified

1. **Phone uses ONLY TCP** - No Bluetooth, no cloud, no DNS queries
2. **Connection is accepted** - Controller responds to our messages
3. **Status is readable** - We can see device states in responses
4. **App commands captured** - We have exact bytes the app sends

### What We Tested (All Failed to Control)

| Attempt | Command | Result |
|---------|---------|--------|
| Exact replay | App's captured bytes | Response, no change |
| Type 45/42 SET | `00 45 02 83 ac 28 42 02 <val> 00` | Response, no change |
| Type 43 query | `00 43 01 06 eb 01 51 00` | Response, no change |
| Type 40 control | `00 40 05 83 ae 28 01 fd 00` | Response, no change |
| Different magic | `80 84`, `80 86`, `83 ae` | Response, no change |
| Different values | `00`, `ff`, `04c2`, `045d` | Response, no change |
| With registration | Full init sequence first | Response, no change |

### Conclusion

The TCP protocol on port 6969 appears to be **read-only for monitoring**. Control likely requires:

1. **Direct CAN bus access** - Using RV-C protocol over physical CAN
2. **App decompilation** - To find hidden authentication/signing
3. **Different interface** - Possibly undocumented port or protocol

## Next Steps

1. **Decompile LippertConnect app** - Find command signing/authentication
2. **Add CAN bus hardware** - MCP2515 module for direct RV-C
3. **Research RV-Bridge** - ESP32 project that does RV-C → HomeKit

## Raw Packet Captures

### Full App Startup Sequence

```
# Connection established
# App sends (19 bytes):
00 45 02 83 ac 04 11 02 2b c2 00 00 43 01 06 eb 01 51 00

# App sends registration (45 bytes):
00 41 08 41 eb 08 1c 88 43 4f af 67 82 02 00 00
43 08 02 eb 43 2f eb 17 81 02 01 0c 00 00
c3 04 01 eb 40 01 03 00 00 40 03 03 eb 66 00

# Controller responds with status broadcasts...
# App continues polling every ~1 second
```

### Light Toggle Sequence (Captured)

When user toggles Kitchen light in app:

```
# Poll + command
00 45 02 83 ac 04 11 02 2b c2 00
00 43 01 06 eb 01 51 00

# Registration refresh
00 41 08 41 eb 08 1c 88 43 4f af 67 82 79 00

# Extended messages
00 43 08 02 eb 43 2f eb 17 81 02 01 27 00 00
c3 04 01 eb 40 01 9c 00

# Control attempt (maybe?)
00 40 03 03 eb d2 00
```

## References

- [RV-C Specification](http://www.rv-c.com/) - Official standard
- [RV-Bridge](https://github.com/rubillos/RV-Bridge) - ESP32 RV-C to HomeKit
- [CoachProxyOS](https://github.com/linuxkidd/coachproxyos) - RV monitoring
