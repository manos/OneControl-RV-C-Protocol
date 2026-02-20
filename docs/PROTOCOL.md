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
| `03 XX` | `13 XX` | Button commands / Keepalive |
| `04 XX` | `14 XX` | Device metadata request |
| `05 XX` | — | Generator status broadcasts |
| `06 XX` | `16 XX` | Key transmit / Seed response |
| `08 XX` | `18 XX` | Identity / Device registration / Status |

## Seed/Key Authentication

Device control (lights, water heaters, water pump, generator) requires challenge-response authentication using a modified TEA cipher.

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

REMOTE_CONTROL_CYPHER = 0xB16B00B5
key = tea_encrypt(seed, REMOTE_CONTROL_CYPHER)
```

## Device Addressing: func_id vs Counter

**Critical concept** — devices have two identifiers:

| Identifier | Stability | Source | Example |
|-----------|-----------|--------|---------|
| **func_id** | Permanent | Device firmware | `57` = Bedroom Light |
| **counter** | Volatile | Controller session | `0xFB` (changes on reboot) |

The `func_id` is the stable device type ID from firmware. The `counter` is a session address assigned by the controller and **can change after any reboot**. Always resolve `counter` dynamically from live `0x08 0x02` registration broadcasts.

### Registration Broadcast (0x08 0x02)

```
08 02 [counter] 00 7d 28 [??] 00 [func_id] ...
│  │      │                        └───── Permanent device type ID (func_id)
│  │      └─────────────────────────────── Current session counter (volatile)
│  └────────────────────────────────────── Subtype: Device registration
└───────────────────────────────────────── Frame type: Status
```

These broadcasts are sent continuously. Collect them to build a live `func_id → counter` map that is refreshed every poll cycle.

### Known func_ids

| func_id | Device | Category |
|---------|--------|----------|
| 3 | Gas Water Heater | Latching Relay |
| 4 | Electric Water Heater | Latching Relay |
| 5 | Water Pump | Latching Relay |
| 32 | Kitchen Ceiling Light | Latching Relay |
| 33 | Kitchen Sconce Light | Latching Relay |
| 41 | Living Room Ceiling Light | Latching Relay |
| 48 | Porch Light | Latching Relay |
| 49 | Awning Light | Latching Relay |
| 50 | Outdoor Light | Latching Relay |
| 57 | Bedroom Light | Latching Relay |
| 58 | Living Room Light | Latching Relay |
| 59 | Kitchen Light | Latching Relay |
| 63 | Bed Ceiling Light | Latching Relay |
| 67 | Fresh Tank | Tank Sensor |
| 68 | Grey Tank | Tank Sensor |
| 69 | Black Tank | Tank Sensor |
| 70 | LP Tank | Tank Sensor |
| 71 | Generator Fuel Tank | Tank Sensor |
| 88 | Landing Gear | Leveler |
| 95 | Generator | Generator Genie |
| 105 | Awning Motor | H-Bridge Motor |
| 122 | Scare Light | Latching Relay |

## Light / Water Heater / Water Pump Control

All latching relay devices (lights, water heaters, water pumps) use the same protocol.

### Complete Toggle Sequence

```
1. TCP connect to 192.168.1.1:6969
2. Register:      01 06 [session] 00
3. Identity:      08 00 [session] 00 [UUID...]
4. Seed Request:  02 80 40 [counter] 42 00 04
5. Wait for seed: 06 80 ... 42 00 04 [4-byte SEED]
6. Compute key:   key = TEA_encrypt(seed, 0xB16B00B5)
7. Key Transmit:  06 80 40 [counter] 43 00 04 [KEY]
8. Control:       00 80 42 [counter] [01=ON | 00=OFF]
9. TCP close
```

### Universal Control Values

These values work for all latching relay devices:

```python
PROTOCOL = 0x80
SESSION  = 0x80
CONN     = 0x40
DEVICE   = 0x04
# Only COUNTER varies per device (resolved from func_id at runtime)
```

### Seed Request Format

```
02 [proto] [conn] [counter] 42 00 [device]
│    │       │       │       │  │    └─ Device type (0x04)
│    │       │       │       │  └───── Table ID (always 0x00)
│    │       │       │       └──────── Command: SESSION_REQUEST_SEED (0x42)
│    │       │       └─────────────── Counter (device-specific, volatile)
│    │       └─────────────────────── Connection ID (0x40)
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
│    │       │       └───────────────────── Counter (volatile)
│    │       └───────────────────────────── Connection ID
│    └───────────────────────────────────── Protocol
└────────────────────────────────────────── Frame type: Key Transmit
```

### Control Command Format

```
00 [proto] [ctrl_conn] [counter] [value]
│    │         │           │        └─── 0x01=ON, 0x00=OFF
│    │         │           └──────────── Counter (same as auth)
│    │         └──────────────────────── Control connection (conn + 2 = 0x42)
│    └────────────────────────────────── Protocol (0x80)
└─────────────────────────────────────── Frame type: Control (0x00)
```

## Generator Control

The generator uses a **different protocol** than latching relay devices.

### Key Differences from Lights

| Aspect | Lights | Generator |
|--------|--------|-----------|
| Protocol byte | `0x80` | `0x81` |
| Connection byte | `0x40` | `0xE8` |
| Control frame type | `0x00` | `0x01` |
| ON value | `0x01` | `0x02` |
| OFF value | `0x00` | `0x01` |
| Auth required | Yes | Yes |
| func_id | varies | `95` |

### Command Sequence

```
1. TCP connect to 192.168.1.1:6969
2. Register:      01 06 [session] 00
3. Identity:      08 00 [session] 00 [UUID...]
4. Seed Request:  02 81 E8 [counter] 42 00 04
5. Wait for seed: 06 [80|82] ... 42 00 04 [SEED]
6. Compute key:   key = TEA_encrypt(seed, 0xB16B00B5)
7. Key Transmit:  06 81 E8 [counter] 43 00 04 [KEY]
8. ON command:    01 81 EA [counter] 00 02
9. OFF command:   01 81 EA [counter] 00 01
10. TCP close
```

**Note**: The seed response may come on protocol `0x80` or `0x82` — check both.

### Generator State Values

| State | Value | Description |
|-------|-------|-------------|
| Off | 0 | Generator is off |
| Priming | 1 | Fuel priming |
| Starting | 2 | Cranking |
| Running | 3 | Running normally |
| Stopping | 4 | Shutting down |

## Leveler Control

The leveler uses a completely different approach: **button-press simulation** with no authentication.

### Key Differences

| Aspect | Lights/Generator | Leveler |
|--------|-----------------|---------|
| Auth required | Yes (seed/key) | **No** |
| Frame type | 0x00/0x01 | **0x03** |
| Control model | ON/OFF state | **Button press** |
| TCP session | Fresh per command | **Persistent with keepalives** |

### Prerequisites

Leveler commands only work when:
1. ACC/Engine power is ON
2. Parking brake is ENGAGED
3. Controller is physically powered (safety interlock)

The leveler does **not** appear in idle device discovery broadcasts.

### Registration Sequence

Before sending button commands, the app performs a 5-frame registration:

```
1. REGISTER:       01 [len] [session] 01
2. IDENTITY:       08 41 [session] 00 [UUID...]
3. DEVICE REG:     08 02 ...
4. TYPE 04:        04 01 ...
5. KEEPALIVE:      03 03 [session] ...
```

### Button Command Format

```
03 80 [conn_hi+2] [conn_lo] 41 [device] 02 [button]
│  │      │           │      │    │      │    └─── Button code
│  │      │           │      │    │      └──────── Screen ID (0x02)
│  │      │           │      │    └─────────────── Device ID (0x01 or 0x08)
│  │      │           │      └──────────────────── Table ID (0x41)
│  │      │           └─────────────────────────── Leveler counter (volatile)
│  │      └─────────────────────────────────────── conn + 2
│  └────────────────────────────────────────────── Protocol (0x80)
└───────────────────────────────────────────────── Frame type: Button command
```

**Important**: `conn_lo` is the leveler's current counter value (resolved from func_id 88 at runtime).

### Button Codes

| Button | Code | Purpose |
|--------|------|---------|
| AutoLevel | `0x10` | Start auto-leveling |
| Retract | `0x20` | Retract all jacks |
| Enter | `0x40` | Confirm operation |
| Cancel | `0x80` | Cancel operation |
| Front | `0x08` | Manual front jack |
| Rear | `0x04` | Manual rear jack |
| Left | `0x02` | Manual left jack |
| Right | `0x01` | Manual right jack |
| Back | `0x0200` | Back/exit (16-bit) |

### Auto-Level Sequence

```
1. Establish persistent TCP connection
2. Perform 5-frame registration
3. Send AutoLevel button (0x10, device=0x01)
4. Send Enter to confirm (0x40, device=0x08)
5. Wait for leveling to complete
6. Send keepalives (03 03 [session]) every few seconds
7. Close connection
```

### Keepalive

While the leveler session is active, send periodic keepalives to prevent timeout:

```
03 03 [session] ...
```

### Status Broadcast

While the leveler screen is active, the controller broadcasts status:

```
08 03 [counter] ... [button_state]
```

Contains the current button state and leveler position data.

## Sensor Data Frames

### Tank Levels (0x01 0x03)

```
01 03 [counter] [level%]
│  │      │        └─── Level percentage (0-100)
│  │      └──────────── Tank counter (volatile, resolve via func_id)
│  └─────────────────── Subtype: Tank status
└────────────────────── Frame type: Status
```

Tank func_ids: 67 (Fresh), 68 (Grey), 69 (Black), 70 (LP), 71 (Generator Fuel), 176 (LP alt).

### Battery Voltage & Generator State (0x05 0x03)

```
05 03 [counter] [state] [volt_hi] [volt_lo] [temp_hi] [temp_lo]
│  │      │        │        └─────────────────── Voltage: 8.8 fixed point
│  │      │        └─────────────────────────── Generator state (0-4)
│  │      └──────────────────────────────────── Generator counter (func_id 95)
│  └─────────────────────────────────────────── Subtype: Generator status
└────────────────────────────────────────────── Frame type: Status
```

Voltage is 8.8 fixed point: `volt_hi + volt_lo / 256.0`

### Generator Hours (0x05 0x03 with counter 0x80)

```
05 03 80 [seconds (4 bytes BE)] [status]
│  │  │        └──────────────────── Operating seconds (uint32 big-endian)
│  │  └──────────────────────────── Hour meter counter (always 0x80)
│  └─────────────────────────────── Subtype: Hour meter
└────────────────────────────────── Frame type: Generator status
```

Hours = operating_seconds / 3600.0

### Relay Status Broadcast (0x06 0x03)

Live ON/OFF state for all latching relay devices (lights, water heaters, water pump):

```
06 03 [counter] [status] ...
│  │      │        └─── Bit 0: 1=ON, 0=OFF
│  │      └──────────── Device counter (resolve via func_id)
│  └─────────────────── Subtype: RelayBasicLatchingStatus2
└────────────────────── Frame type: Relay status
```

These broadcasts are continuous and provide real-time state without polling individual devices.

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
- [Home Assistant Integration](https://github.com/manos/HomeAssistant-Lippert-OneControl-Integration)
