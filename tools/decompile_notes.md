# LippertConnect App Decompilation Notes

Reverse engineering notes for the LippertConnect Android app.

## App Info

- **Package**: `com.lci1.lippertconnect`
- **Version**: 6.2.2 (from APKPure)
- **Min SDK**: Android 9.0+
- **Size**: ~145MB

## Key Discovery: COBS Framing with CRC-8!

**The TCP protocol uses COBS (Consistent Overhead Byte Stuffing) encoding with CRC-8 checksum!**

### COBS Parameters (from decompiled `DirectConnectionMyRvLinkTcpIp.cs`)

```csharp
CobsEncoder = new CobsEncoder(true, true, (byte)0, 6);
CobsDecoder = new CobsDecoder(true, (byte)0, 6);
```

- `prependStartFrame = true` - Frames start with 0x00
- `useCrc = true` - CRC-8 appended before COBS encoding
- `frameByte = 0` - Frame delimiter is 0x00
- `numDataBits = 6` - Max 63 data bytes per COBS block

### CRC-8 Algorithm (from decompiled `Crc8.cs`)

- **Polynomial**: 0x8C (reflected) - This is CRC-8/MAXIM (1-Wire/DOW CRC)
- **Initial Value**: 0x55
- **Lookup Table**: Standard table for poly 0x8C reflected

```python
# Python implementation
CRC8_TABLE = [0x00, 0x5e, 0xbc, 0xe2, ...]  # 256 entries
def crc8(data, init=0x55):
    crc = init
    for byte in data:
        crc = CRC8_TABLE[(crc ^ byte) & 0xFF]
    return crc
```

### Protocol Stack

```
[TCP/IP (port 6969)]
    └── [COBS Framing (0x00 delimiters, 6-bit blocks)]
        └── [CRC-8 Validation (poly=0x8C, init=0x55)]
            └── [MyRvLink Command/Event Messages]
                └── [RV-C / Device Data]
```

## Command Format (from decompiled `MyRvLinkCommand.cs`)

### Base Command Structure

```
[ClientCommandId (2 bytes, LE)] [CommandType (1 byte)] [Payload...]
```

### ActionDimmable Command (CommandType = 67)

```
[ClientCommandId (2 bytes, LE)] [67] [DeviceTableId] [DeviceId] [LightCommand (8 bytes)]
```

### LightCommand Structure (from `LogicalDeviceLightDimmableCommand.cs`)

```
Byte 0: Command (DimmableLightCommand enum)
Byte 1: MaxBrightness (0-255)
Byte 2: Duration
Byte 3: CycleTime1 MSB
Byte 4: CycleTime1 LSB
Byte 5: CycleTime2 MSB
Byte 6: CycleTime2 LSB
Byte 7: Undefined (0x00)
```

### DimmableLightCommand Enum

| Value | Name | Description |
|-------|------|-------------|
| 0 | Off | Turn light off |
| 1 | On | Turn light on to MaxBrightness |
| 2 | Blink | Blink mode |
| 3 | Swell | Swell/pulse mode |
| 4 | Settings | Configure settings |
| 127 | Restore | Restore to last brightness |

### MyRvLinkCommandType Enum

| Value | Name |
|-------|------|
| 1 | GetDevices |
| 2 | GetDevicesMetadata |
| 64 | ActionSwitch |
| 65 | ActionMovement |
| 67 | ActionDimmable |
| 68 | ActionRgb |
| 69 | ActionHvac |

## Findings

### App Framework

**This is a Xamarin/.NET MAUI app!** The Java code is just bindings - actual logic is in .NET assemblies.

Successfully extracted **430 .NET assemblies** from `libassemblies.armeabi-v7a.blob.so` using LZ4 decompression.

### Package Structure

```
LippertConnect.apk (split APK bundle)
├── com.lci1.lippertconnect.apk      # Main APK (35MB)
├── config.armeabi_v7a.apk           # Native libs (116MB)
│   └── lib/armeabi-v7a/
│       ├── libassemblies.*.blob.so  # .NET assemblies (85MB!)
│       ├── libmonosgen-2.0.so       # Mono runtime
│       └── libxamarin-app.so        # Xamarin loader
├── config.en.apk                    # English resources
└── config.mdpi.apk                  # Medium DPI resources
```

### Key .NET DLLs Decompiled

| Extracted DLL | Original Name | Purpose |
|---------------|---------------|---------|
| `d.dll` | `OneControl.Direct.MyRvLinkTcpIp.dll` | **TCP Protocol Implementation!** |
| `assembly_0087_IDS.Net.dll` | `IDS.Portable.Common` | COBS, CRC-8, utilities |
| `assembly_0165_P.dll` | `OneControl.Devices` | Device commands (LightDimmable, etc.) |
| `assembly_0168_P.dll` | `OneControl.Direct.MyRvLink` | MyRvLink commands/events |

### Key Decompiled Files

| File | Key Classes |
|------|-------------|
| `DirectConnectionMyRvLinkTcpIp.cs` | CobsEncoder/Decoder init, TCP connection |
| `Crc8.cs` | CRC-8 lookup table, Calculate method |
| `CobsEncoder.cs` | COBS encoding algorithm |
| `CobsDecoder.cs` | COBS decoding algorithm |
| `MyRvLinkCommandActionDimmable.cs` | Dimmable light command structure |
| `LogicalDeviceLightDimmableCommand.cs` | Light command data format |

## Example Commands

### Light ON (100%)

Raw command (before COBS):
```
00 00  # ClientCommandId = 0
43     # CommandType = ActionDimmable (67)
00     # DeviceTableId
21     # DeviceId (e.g., Kitchen)
01     # Command = On
C8     # MaxBrightness = 200
00     # Duration
00 00  # CycleTime1
00 00  # CycleTime2
00     # Undefined
```

After COBS+CRC encoding:
```
00 80 41 43 C3 21 01 C8 C0 01 XX 00
```

### Light OFF

Raw command:
```
01 00  # ClientCommandId = 1
43     # CommandType = ActionDimmable
00     # DeviceTableId
21     # DeviceId
00     # Command = Off
00 00 00 00 00 00 00  # Rest zeros
```

## New Discovery: Type 0x47 Toggle Format!

### From Live Traffic Analysis

Captured toggle commands use type `0x47` (71), NOT the `ActionDimmable` (67) we expected!

#### COBS-Encoded Wire Format
```
00 47 08 07 SS SS fe 64 86 81 01 01 CRC 00
│  │  │  │  │  │  │    │  │  │  │   │
│  │  │  │  │  │  │    │  │  │  │   └─ CRC-8
│  │  │  │  │  │  │    │  │  │  └───── Command (01=On, 02=Off?)
│  │  │  │  │  │  │    │  │  └──────── Code byte (81 = 1 data + 2 zeros)
│  │  │  │  │  │  │    │  └─────────── Counter (86, 87, 88, 89...)
│  │  │  │  │  │  │    └────────────── Device address
│  │  │  │  │  │  └─────────────────── Unknown (fe)
│  │  │  │  │  └────────────────────── Session ID
│  │  │  │  └───────────────────────── Session ID (repeated)
│  │  │  └──────────────────────────── Message subtype? (07)
│  │  └─────────────────────────────── Code byte (47 = 7 data + 1 zero)
│  └────────────────────────────────── Frame start
```

#### COBS-Decoded Raw Payload
```
08 07 SS SS fe 64 86 00 01 00 00
```

### CRC Verification SUCCESS ✅

```python
# Captured from app (session 0x30):
captured_wire = "47 08 07 30 30 fe 64 86 81 01 01 a5"
decoded_raw = "08 07 30 30 fe 64 86 00 01 00 00"

# CRC-8/MAXIM calculation:
crc8(bytes.fromhex("0807303030fe6486000100 00"), init=0x55) == 0xA5  # ✅ MATCH!
```

### Registration Packet

```
# App → Controller (request registration)
00 81 08 09 SS 1c 88 43 4f af 67 82 CRC 00

# Controller → App (acknowledge registration)  
00 81 18 09 SS 1c 88 43 4f af 67 82 CRC 00
```

- `81` = Registration message type
- `08` / `18` = Request / Response
- `09` = Length or subtype
- `SS` = Session ID (assigned by controller)
- `434faf67` = App/device identifier

### What We Still Don't Know

1. **What is `fe 64`?** - Device address, but how does it map to device ID `0x28`?
2. **Why doesn't our session work?** - We register OK, but toggle commands fail
3. **Type 0xC7 variant** - Some toggles use `0xC7` instead of `0x47`

## Implementation Status

- [x] COBS encoder/decoder implemented (`rvc/cobs.py`)
- [x] CRC-8 with correct polynomial and init value
- [x] Command builder (`rvc/commands.py`)
- [x] CRC calculation verified against captured traffic
- [x] Registration packet exchange working
- [ ] **CONTROL NOT WORKING** - Commands accepted but devices don't respond
- [ ] Device discovery (GetDevices command)
- [ ] Event/status parsing

## Files Created

- `rvc/cobs.py` - COBS encoder/decoder with CRC-8
- `rvc/commands.py` - Command builder with proper format
- `tools/decode_capture.py` - TCP capture decoder
- `tools/extract_assemblies.py` - .NET assembly extractor
- `tools/decompile/decompiled/` - Decompiled C# source

## Decompilation Commands

```bash
# Extract assemblies
python3 tools/extract_assemblies.py

# Decompile with ILSpy
~/.dotnet/tools/ilspycmd -o output_dir input.dll

# Key DLLs to decompile:
# - d.dll (MyRvLinkTcpIp)
# - assembly_0087_IDS.Net.dll (COBS/CRC)
# - assembly_0165_P.dll (Devices)
# - assembly_0168_P.dll (MyRvLink)
```

## Resources

- JADX GUI: `jadx-gui tools/decompile/LippertConnect.apk`
- Output folder: `tools/decompile/output/`
- Extracted DLLs: `tools/decompile/extracted_dlls/`
- Decompiled C#: `tools/decompile/decompiled/`
