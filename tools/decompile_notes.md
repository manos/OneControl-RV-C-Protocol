# LippertConnect App Decompilation Notes

Reverse engineering notes for the LippertConnect Android app.

## App Info

- **Package**: `com.lci1.lippertconnect`
- **Version**: 6.2.2 (from APKPure)
- **Min SDK**: Android 9.0+
- **Size**: ~145MB

## Key Discovery: COBS Framing!

**The TCP protocol uses COBS (Consistent Overhead Byte Stuffing) encoding!**

This explains the `0x00` delimiters we saw in tcpdump captures. COBS is a framing protocol that:
- Uses `0x00` as frame delimiters
- Encodes data to eliminate `0x00` bytes within the payload
- Adds minimal overhead (1 byte per 254 bytes)

### Protocol Stack

```
[TCP/IP (port 6969)]
    └── [COBS Framing (0x00 delimiters)]
        └── [CRC Validation (CRC-8 or CRC-32)]
            └── [MyRvLink Command/Event Messages]
                └── [IDS CAN / RV-C Data]
```

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

### Key .NET DLLs Found

| Extracted DLL | Original Name | Purpose |
|---------------|---------------|---------|
| `d.dll` | `OneControl.Direct.MyRvLinkTcpIp.dll` | **TCP Protocol Implementation!** |
| `TcpIp.dll` | `OneControl.Direct.RvCloudIoT` | Cloud IoT direct commands |
| `assembly_0168_P.dll` | MyRvLink events/commands | Event definitions |
| `assembly_0169_P.dll` | MyRvLink COBS | COBS stream handling |
| `assembly_0087_IDS.Net.dll` | `IDS.Portable.Common` | COBS base, CRC |
| `assembly_0085_IDS.Net.dll` | IDS CAN connection | TCP/WiFi/BLE connections |
| `Gateway.dll` | Device gateway | Logical device handling |

### Key Methods Found

| Method | Location | Purpose |
|--------|----------|---------|
| `SendCommandRawAsync` | d.dll | **Send raw TCP commands** |
| `WriteDataAsync` | d.dll | Write to TCP stream |
| `ConnectAsync` | d.dll | TCP connection |
| `SendDirectCommandLightDimmable` | assembly_0168 | Dimmer control |
| `SendDirectCommandRelayBasicSwitch` | assembly_0168 | Switch control |
| `SendDirectCommandLightRgb` | assembly_0168 | RGB light control |
| `TryDecodeByte` | assembly_0087 | COBS decode |
| `AppendValueByte` | assembly_0087 | COBS encode |

### Key Classes Found

| Class | DLL | Purpose |
|-------|-----|---------|
| `DirectConnectionMyRvLinkTcpIp` | d.dll | Main TCP connection handler |
| `CobsEncoder` | d.dll | COBS encoding |
| `CobsDecoder` | d.dll | COBS decoding |
| `CobsStream` | assembly_0087/d.dll | COBS stream wrapper |
| `Crc32`, `Crc8` | assembly_0087 | CRC validation |
| `MyRvLinkEventDevicesSubByte` | assembly_0168 | Sub-byte event parsing |
| `MyRvLinkRelayBasicLatchingStatusType1` | assembly_0168 | Relay status |

### COBS Framing Details

From `assembly_0087_IDS.Net.dll`:
- `FrameByteCountLsb` - Frame has length byte (LSB)
- `PrependStartFrame` - Frames may have start marker
- `DefaultFrameByte` - Default frame delimiter byte
- `MaxCobsSourceBufferSize` - Max buffer size

### CRC/Checksum Algorithm

From `assembly_0087_IDS.Net.dll`:
- **CRC-32 LE** (Little Endian) - `Crc32Le`, `Crc32_le`
- **CRC-8** - for shorter messages
- `UseCrc` flag - CRC can be optional
- `payloadCrc`, `computedCrc` - CRC validation
- `CobsCrcMismatchException` - thrown on CRC error

### Command Types (from assembly_0168)

| Command | Purpose |
|---------|---------|
| `LogicalDeviceLightDimmableCommand` | Dimmer control |
| `LogicalDeviceLightRgbCommand` | RGB light |
| `LogicalDeviceClimateZoneCommand` | HVAC zone |
| `LogicalDeviceLevelerCommandType1` | Leveler |
| `LogicalDeviceRelayBasicStatusType1` | Relay status |
| `MyRvLinkRelayBasicLatchingStatusType1` | Latching relay |
| `MyRvLinkRelayHBridgeMomentaryStatusType1` | H-bridge relay |

### Protocol Constants

| Constant | Value | Purpose |
|----------|-------|---------|
| `DefaultPort` | 6969 | TCP port |
| `ConnectionSendDataTimeMs` | ? | Send timeout |
| `ConnectionReceiveDataTimeMs` | ? | Receive timeout |
| `MaxCobsSourceBufferSize` | ? | Max message size |

## Native Libraries

Check `lib/` folder for `.so` files that might contain:
- Protocol implementation
- Encryption routines
- Checksum calculations

### .so Files Found

| Library | Architecture | Size | Notes |
|---------|--------------|------|-------|
| (TBD) | | | |

## Interesting Strings

```
(Extracted strings that might be relevant)
```

## Next Steps

1. [ ] Search for port 6969
2. [ ] Find socket connection code
3. [ ] Locate command building functions
4. [ ] Identify checksum algorithm
5. [ ] Check for native crypto
6. [ ] Document message format

## Resources

- JADX GUI: `jadx-gui tools/decompile/LippertConnect.apk`
- Output folder: `tools/decompile/output/`
