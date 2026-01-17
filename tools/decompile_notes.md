# LippertConnect App Decompilation Notes

Reverse engineering notes for the LippertConnect Android app.

## App Info

- **Package**: `com.lci1.lippertconnect`
- **Version**: 6.2.2 (from APKPure)
- **Min SDK**: Android 9.0+
- **Size**: ~145MB

## Search Targets

### Network/Protocol Related
- [ ] `6969` - TCP port
- [ ] `192.168.1` - Controller IP
- [ ] `socket` - Socket operations
- [ ] `connect` - Connection logic
- [ ] `write` / `send` - Data transmission
- [ ] `read` / `receive` - Data reception

### Protocol Constants
- [ ] `0x45` or `\x45` - Message type
- [ ] `0x43` or `\x43` - Message type
- [ ] `0x41` or `\x41` - Message type
- [ ] `83ac` or `0x83ac` - Magic bytes
- [ ] `83ae` - Magic bytes
- [ ] `80 84` - Magic bytes

### Control Logic
- [ ] `toggle` - Toggle function
- [ ] `light` - Light control
- [ ] `dimmer` - Dimmer control
- [ ] `command` - Command building
- [ ] `onecontrol` - Product name

### Authentication/Security
- [ ] `encrypt` - Encryption
- [ ] `decrypt` - Decryption
- [ ] `sign` - Signing
- [ ] `hash` - Hashing
- [ ] `token` - Auth tokens
- [ ] `session` - Session management
- [ ] `checksum` - Checksums

### Bluetooth (backup check)
- [ ] `BluetoothGatt` - BLE GATT
- [ ] `characteristic` - BLE characteristics
- [ ] `uuid` - Service/characteristic UUIDs

## Findings

### App Framework

**This is a Xamarin/.NET MAUI app!** The Java code is just bindings - actual logic is in .NET assemblies.

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

### Key .NET DLLs Found (in blob)

| DLL | Purpose |
|-----|---------|
| `OneControl.Direct.MyRvLinkTcpIp.dll` | **TCP Protocol Implementation!** |
| `OneControl.Direct.IdsCan.dll` | CAN bus protocol |
| `OneControl.Direct.MyRvLink.dll` | General MyRV Link |
| `OneControl.Direct.MyRvLinkBle.dll` | Bluetooth LE protocol |
| `OneControl.Direct.IdsCanAccessoryBle.dll` | BLE accessories |
| `OneControl.Devices.dll` | Device definitions |
| `OneControl.dll` | Core library |
| `IDS.Portable.CAN.dll` | Portable CAN implementation |
| `DotNetty.Transport.dll` | Network transport |

### Key Methods Found

| Method | DLL | Purpose |
|--------|-----|---------|
| `SendCommandRaw` | Unknown | Send raw commands |
| `SetBrightness` | Unknown | Dimmer control |
| `WriteBytes` | Unknown | Low-level write |
| `IsLight` | Unknown | Light detection |
| `CommandMapp` | Unknown | Command mapping |

### Key Classes Found

| Class | Purpose | Notes |
|-------|---------|-------|
| `TcpClient` | TCP communication | Standard .NET |
| `DeviceModel` | Device representation | |
| `ForInvalidCommand` | Error handling | |

### Command Building Logic

```java
// Paste relevant code snippets here
```

### Magic Byte Sources

```java
// Where are 83ac, 83ae, etc. defined?
```

### Checksum Algorithm

```java
// How are the changing values (2bc2, 51, fd) computed?
```

### Authentication Flow

```
(Sequence diagram or description)
```

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
