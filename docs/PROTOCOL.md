# Lippert OneControl TCP Protocol

Technical documentation for the Lippert OneControl TCP protocol on port 6969.

## Network Configuration

The OneControl controller creates a WiFi access point:

| Setting | Value |
|---------|-------|
| SSID | `MyRV_<serial>` |
| Security | WPA2-PSK |
| Gateway | 192.168.1.1 |
| DHCP Range | 192.168.1.100+ |
| Protocol Port | TCP 6969 |

## Protocol Stack

```
[TCP/IP (port 6969)]
    └── [COBS Framing (0x00 delimiters)]
        └── [CRC-8/MAXIM Validation]
            └── [IDS-CAN Messages]
                └── [Seed/Key Authentication]
```

## COBS Framing

### Frame Format

```
[0x00 Start][COBS-encoded payload + CRC][0x00 End]
```

### COBS Encoding

Uses a modified scheme where each code byte contains:
- Bits 0-5: Count of data bytes following (0-63)
- Bits 6-7: Count of trailing zero bytes (0-3)

```python
def cobs_encode(payload: bytes) -> bytes:
    """COBS encode with CRC-8"""
    crc = crc8_maxim(payload)
    data = bytes(payload) + bytes([crc])
    out = bytearray()
    i = 0
    while i < len(data):
        code_pos = len(out)
        out.append(0)  # Placeholder
        count = 0
        while i < len(data) and data[i] != 0 and count < 63:
            out.append(data[i])
            i += 1
            count += 1
        zeros = 0
        while i < len(data) and data[i] == 0 and zeros < 3:
            i += 1
            zeros += 1
        out[code_pos] = count + (zeros * 64)
    return bytes([0x00]) + bytes(out) + bytes([0x00])
```

### CRC-8/MAXIM

- **Polynomial**: 0x8C (reflected)
- **Initial Value**: 0x55

```python
def crc8_maxim(data: bytes, init: int = 0x55) -> int:
    table = [0] * 256
    for i in range(256):
        c = i
        for _ in range(8):
            c = (c >> 1) ^ 0x8C if c & 1 else c >> 1
        table[i] = c
    crc = init
    for b in data:
        crc = table[(crc ^ b) & 0xFF]
    return crc
```

## Message Types

| Request | Response | Purpose |
|---------|----------|---------|
| `01 XX` | `11 XX` | Session registration |
| `02 XX` | `12 XX` | Seed request |
| `04 XX` | `14 XX` | Device metadata request |
| `06 XX` | `16 XX` | Key transmit / Commands |
| `08 XX` | `18 XX` | Identity / Status |

## Seed/Key Authentication

Device control requires challenge-response authentication using a modified TEA cipher.

### Session Types

| ID | Cypher | Name | Purpose |
|----|--------|------|---------|
| 4 | `0xB16B00B5` | REMOTE_CONTROL | Device control |
| 1 | `0xB16B0015` | MANUFACTURING | Factory |
| 2 | `0xBABECAFE` | DIAGNOSTIC | Debug |
| 3 | `0xDEADBEEF` | REPROGRAMMING | Firmware |

### TEA Cipher

```python
def tea_encrypt(seed: int, cypher: int) -> int:
    """
    Modified TEA cipher.
    Constants spell "Copyright IDSsnc" in ASCII.
    """
    K1 = 0x436F7079  # "Copy"
    K2 = 0x72696768  # "righ"
    K3 = 0x74204944  # "t ID"
    K4 = 0x53736E63  # "Ssnc"
    DELTA = 0x9E3779B9
    
    v0, v1 = seed, cypher
    sum_val = DELTA
    
    for _ in range(32):
        v0 = (v0 + (((v1 << 4) + K1) ^ (v1 + sum_val) ^ ((v1 >> 5) + K2))) & 0xFFFFFFFF
        v1 = (v1 + (((v0 << 4) + K3) ^ (v0 + sum_val) ^ ((v0 >> 5) + K4))) & 0xFFFFFFFF
        sum_val = (sum_val + DELTA) & 0xFFFFFFFF
    
    return v0

# Usage
REMOTE_CONTROL_CYPHER = 0xB16B00B5
key = tea_encrypt(seed, REMOTE_CONTROL_CYPHER)
```

## Control Sequence

### Complete Light Toggle

```
1. TCP connect to 192.168.1.1:6969
2. Register:      01 06 [session] 00
3. Identity:      08 00 [session] 00 [UUID...]
4. Seed Request:  02 [proto] [conn] [counter] 42 00 [device]
5. Wait for seed: 06 80 ... 42 00 [device] [4-byte SEED]
6. Compute key:   key = TEA_encrypt(seed, 0xB16B00B5)
7. Key Transmit:  06 [proto] [conn] [counter] 43 00 [device] [KEY]
8. Control:       00 [proto] [conn+2] [counter] [01=ON | 00=OFF]
9. TCP close
```

### Universal Control Values

Discovery found that these values work for all devices:

```python
PROTOCOL = 0x80
SESSION  = 0x80
CONN     = 0x40
DEVICE   = 0x04
# Only COUNTER varies per device
```

### Seed Request Format

```
02 [proto] [conn] [counter] 42 00 [device]
│    │       │       │       │  │    └─ Device type (0x04 for lights)
│    │       │       │       │  └───── Table ID (always 0x00)
│    │       │       │       └──────── Command: SESSION_REQUEST_SEED (0x42)
│    │       │       └─────────────── Counter (device-specific)
│    │       └─────────────────────── Connection ID
│    └─────────────────────────────── Protocol (0x80)
└──────────────────────────────────── Frame type: Seed Request
```

### Seed Response Format

```
06 80 [session] [info] 42 00 [device] [seed0] [seed1] [seed2] [seed3]
│  │      │       │     │  │    │       └───────────────────────────┘
│  │      │       │     │  │    │              4-byte seed (big-endian)
│  │      │       │     │  │    └───── Device type
│  │      │       │     │  └────────── Table ID
│  │      │       │     └───────────── Command echo
│  │      │       └─────────────────── Address info
│  │      └─────────────────────────── Session ID
│  └────────────────────────────────── Protocol 0x80 (always)
└───────────────────────────────────── Frame type: Response
```

**Note**: Seed response always comes on Protocol 0x80, regardless of request protocol.

### Key Transmit Format

```
06 [proto] [conn] [counter] 43 00 [device] [key0] [key1] [key2] [key3]
│    │       │       │       │  │    │       └───────────────────────┘
│    │       │       │       │  │    │              4-byte key (big-endian)
│    │       │       │       │  │    └───── Device type
│    │       │       │       │  └────────── Table ID
│    │       │       │       └───────────── Command: SESSION_TRANSMIT_KEY (0x43)
│    │       │       └───────────────────── Counter
│    │       └───────────────────────────── Connection ID
│    └───────────────────────────────────── Protocol
└────────────────────────────────────────── Frame type: Key Transmit
```

### Control Command Format

```
00 [proto] [ctrl_conn] [counter] [value]
│    │         │           │        └─── 0x01=ON, 0x00=OFF
│    │         │           └──────────── Counter (same as auth)
│    │         └──────────────────────── Control connection (conn + 2)
│    └────────────────────────────────── Protocol
└─────────────────────────────────────── Frame type: Control
```

## Sensor Data Frames

### Tank Levels (01 03)

```
01 03 [counter] [level%]
│  │      │        └─── Level percentage (0-100)
│  │      └──────────── Tank counter
│  └─────────────────── Subtype: Tank status
└────────────────────── Frame type: Status
```

Known counters:
- 0x04: Grey Tank
- 0x3E: Fresh Tank
- 0x86: Black Tank
- 0x10: LP Tank

### Generator Hours (05 03)

```
05 03 [counter] [seconds (4 bytes BE)] [status]
│  │      │           └───────────────────┘
│  │      │           Operating seconds (uint32 big-endian)
│  │      └────────── Counter (0x80 for hour meter)
│  └─────────────────  Subtype: Hour meter
└──────────────────── Frame type: Generator status
```

## Device Discovery

Devices broadcast metadata on `08 02` frames:

```
08 02 [counter] 00 7d 28 [??] 00 [func_id] ...
│  │      │                        └───── FunctionName ID
│  │      └─────────────────────────────── Device counter
│  └────────────────────────────────────── Subtype: Device metadata
└───────────────────────────────────────── Frame type: Status
```

FunctionName IDs map to device names (see tools/auto_discover.py for full list).

## Error Codes

Authentication errors return in response byte 7:

| Code | Meaning |
|------|---------|
| 0 | Success |
| 2 | Invalid packet length |
| 9 | Session busy |
| 11 | Session already open |
| 12 | Seed timeout / Wrong source |
| 13 | KEY_NOT_CORRECT |
| 14 | Session not open |

## References

- **Algorithm Source**: `IDS.Core.Types.SESSION_ID.Encrypt()` from Lippert IDS library
- [RV-C Specification](http://www.rv-c.com/)
- [RV-Bridge Project](https://github.com/rubillos/RV-Bridge)
