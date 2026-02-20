# Reverse Engineering Analysis

Working notes from reverse-engineering the Lippert OneControl protocol (January 2026).

## Summary

Successfully reverse-engineered the TCP protocol for controlling Lippert OneControl RV systems:

- ✅ COBS framing with CRC-8/MAXIM
- ✅ TEA cipher authentication (seed/key challenge-response)
- ✅ Light control (ON/OFF)
- ✅ Tank sensor reading
- ✅ Battery voltage reading
- ✅ Generator hour meter reading
- ✅ Generator start/stop control (cracked Jan 18, 2026!)
- ✅ Water heater control (Gas & Electric)
- ✅ Water pump control
- ✅ Relay status polling (live ON/OFF state from broadcasts)
- ✅ Auto-discovery via func_id (no packet capture needed!)
- ✅ Home Assistant integration (v0.3.0 via HACS)
- 🔧 Leveler control (protocol decoded, session management WIP)

## Key Discoveries

### 1. Universal Control Values

The biggest breakthrough: these values work for ALL latching relay devices (lights, water heaters, water pumps):

```python
PROTOCOL = 0x80
SESSION  = 0x80
CONN     = 0x40
DEVICE   = 0x04
```

### 2. Authentication Per Command

Each ON or OFF command requires fresh authentication. You cannot reuse a seed/key pair.

### 3. Seed Response Protocol

Regardless of which protocol you request (0x80, 0x82, 0x83), the seed response always comes back on Protocol 0x80.

### 4. Counter is VOLATILE (Critical Bug Fix!)

**CORRECTED**: Early analysis assumed counters were fixed per device. This was WRONG and caused a critical production bug where lights stopped responding after a controller reboot.

**Counter values are session addresses assigned by the controller and can change on any reboot.** The stable identifier is `func_id` (from device firmware), which never changes.

The fix was a fundamental architectural shift:
- **Before**: Config stored `counter` values directly. Broke after reboot.
- **After**: Config stores `func_id` values. Counter resolved dynamically every poll cycle from live `0x08 0x02` registration broadcasts.

This was the single most important discovery for building a reliable integration.

### 5. func_id Discovery

Devices broadcast their permanent `func_id` in `0x08 0x02` registration frames at byte offset 8. By continuously collecting these broadcasts, we build a live `func_id → counter` map that survives reboots.

## Confirmed Working Device Types

All of these are controlled and monitored via the Home Assistant integration:

| Category | func_ids | Protocol | Auth |
|----------|----------|----------|------|
| Lights | 32, 33, 41, 48, 49, 50, 57, 58, 59, 63, 122 | Latching relay (0x80) | TEA seed/key |
| Water Heaters | 3 (Gas), 4 (Electric) | Latching relay (0x80) | TEA seed/key |
| Water Pump | 5 | Latching relay (0x80) | TEA seed/key |
| Generator | 95 | Generator (0x81) | TEA seed/key |
| Tank Sensors | 67, 68, 69, 70, 71, 176 | Read-only broadcast | None |
| Leveler | 88 (Landing Gear) | Button sim (0x03) | None |

## Tank Sensors

Levels broadcast on `01 03` frames. Counter values are volatile — use func_id for stable identification:

| Tank | func_id | Frame Pattern |
|------|---------|---------------|
| Fresh | 67 | `01 03 [counter] [level]` |
| Grey | 68 | `01 03 [counter] [level]` |
| Black | 69 | `01 03 [counter] [level]` |
| LP | 70 | `01 03 [counter] [level]` |
| Generator Fuel | 71 | `01 03 [counter] [level]` |

## Battery Voltage

Voltage comes from the Generator Genie (func_id 95) broadcasts:

```
05 03 [counter] [state] [volt_hi] [volt_lo] [temp_hi] [temp_lo]
```

Voltage is 8.8 fixed point: `volt_hi + volt_lo / 256`

## Generator Hour Meter

Hours broadcast on `05 03` frames with a dedicated counter (0x80 for the hour meter subtype):

```
05 03 80 [uint32 BE seconds] [status]
```

Operating seconds / 3600 = hours.

## Relay Status Broadcasts (0x06 0x03)

All latching relay devices (lights, water heaters, water pump) continuously broadcast their ON/OFF state:

```
06 03 [counter] [status_byte] ...
```

Bit 0 of `status_byte` indicates ON (1) or OFF (0). This provides live status without needing to poll individual devices, and is the primary mechanism for keeping the HA integration in sync with physical state.

## Generator Control (WORKING! ✅)

Cracked Jan 18, 2026. The Generator Genie (func_id 95) uses a **different protocol** than latching relays.

### Key Differences from Lights

| Aspect | Lights | Generator |
|--------|--------|-----------|
| Protocol byte | 0x80 | **0x81** |
| Connection byte | 0x40 | **0xE8** |
| Control frame type | 0x00 | **0x01** |
| ON value | 0x01 | **0x02** |
| OFF value | 0x00 | **0x01** |

### Command Sequence

```
1. Seed request:   02 81 E8 [counter] 42 00 04
2. Seed response:  06 [80|82] ... 42 00 04 [seed]  (from controller)
3. Key transmit:   06 81 E8 [counter] 43 00 04 [key]
4. ON command:     01 81 EA [counter] 00 02
5. OFF command:    01 81 EA [counter] 00 01
```

### Status Broadcast

Generator state in `05 03 [counter] [state] ...`:
- 0x00 = Off
- 0x01 = Priming
- 0x02 = Starting
- 0x03 = Running
- 0x04 = Stopping

### Critical Discovery

**The control command uses frame type 0x01, NOT 0x00!** This was the key insight that made generator control work. Also, the connection byte is `0xE8` (not `0x40`) and the control connection is `0xEA` (`0xE8 + 2`).

## Water Heater & Water Pump Control (WORKING! ✅)

Water heaters (Gas func_id 3, Electric func_id 4) and water pump (func_id 5) use the **exact same latching relay protocol as lights**. No protocol differences at all — same seed/key auth, same frame types, same ON/OFF values.

### Safety Notes

- **Water Heater**: Do not turn on if water tank is empty (fire hazard)
- **Water Pump**: Can burn out without water supply

## Device Types

From decompiled OneControl app:

| Type | Category | Control | Auth |
|------|----------|---------|------|
| Latching Relay | Lights, Water Heater, Water Pump | ON/OFF toggle | TEA seed/key |
| Momentary H-Bridge | Slides, Awnings | Momentary press | TEA seed/key |
| Tank Sensor | Fresh, Grey, Black, LP | Read-only | None |
| Generator Genie | Generator | ON/OFF (0x81 protocol) | TEA seed/key |
| Leveler Type 3 | Hydraulic leveler | Button simulation | None |

## CRITICAL: Function ID Confusion

**WARNING**: Some func_ids use the same toggle protocol but are NOT lights!

### Dangerous Misidentifications Found

| func_id | Name in Enum | Actual Device | Risk |
|---------|--------------|---------------|------|
| 105 | AWNING | Awning MOTOR | Extends/retracts awning |
| 107 | WATER_TANK_HEATER | Heating pad under tank | NOT a light! |

### Safe Light func_ids (confirmed)

Only these should be auto-discovered as lights:
```
32, 33, 41, 48, 49, 50, 57, 58, 59, 63, 122
```

### Why This Happens

The OneControl protocol uses the same command structure for:
- Latching relays (lights)
- Momentary H-bridges (motors)
- Water heater relays

The func_id tells you WHAT it is, but the control protocol is identical.
**Always verify func_id mappings before toggling unknown devices!**

## Leveler Control (Protocol Decoded, Session WIP)

### What We Know

The leveler is a **Lippert Motorized 4-Point Hydraulic Leveler (Sprinter)**.
- func_id: **88** (Landing Gear)
- Command type: `0x53` (Leveler3ButtonCommand = 83 decimal)
- Protocol: Button-press simulation via frame type `0x03`

### Key Capture Findings (Jan 2026)

From analyzing `leveler2.pcap` and `leveler_new.pcap`:

**1. NO AUTHENTICATION** required for leveler commands.

**2. Button Command Structure:**
```
03 80 [conn_hi+2] [conn_lo] 41 [device] 02 [button]
```
- `conn_lo` = the leveler's current counter (volatile, from func_id 88)
- `conn_hi` = derived from session context

**3. Persistent TCP Session Required:**
Unlike lights (which use fresh connections per command), the leveler requires a single persistent TCP connection for the entire interaction, with keepalives.

**4. 5-Frame Registration Sequence:**
Before any button commands, the app performs:
1. `0x01` REGISTER (with `0x01` as last byte, not `0x00`)
2. `0x08 0x41` IDENTITY (subtype `0x41`, not `0x00`)
3. `0x08 0x02` DEVICE REGISTRATION
4. `0x04 0x01` TYPE 04 frame
5. `0x03 0x03` KEEPALIVE

### Button Codes

| Button | Code | Purpose |
|--------|------|---------|
| AutoLevel | 0x10 | Start auto-leveling |
| Retract | 0x20 | Retract all jacks |
| Enter | 0x40 | Confirm operation |
| Cancel | 0x80 | Cancel/stop operation |
| Front | 0x08 | Manual front jack |
| Rear | 0x04 | Manual rear jack |
| Left | 0x02 | Manual left jack |
| Right | 0x01 | Manual right jack |

### Auto-Level Sequence (from captures)

1. **Press AutoLevel** (button=0x10, device=0x01)
2. **Press Enter** (button=0x40, device=0x08) — confirms operation
3. *(Leveler runs, keepalives sent every ~3s)*
4. **Press Cancel** (button=0x80) — stops if needed

### Three Critical Bugs Found in Initial Implementation

1. **Incorrect Registration**: Was sending a single wrong `0x02` frame instead of the 5-frame sequence.
2. **Hardcoded Connection Bytes**: `conn_lo` must be the leveler's current counter (volatile), not a fixed value.
3. **Broken Session Continuity**: Was opening a new TCP connection per button press instead of maintaining a persistent session with keepalives.

### Discovery Note

**The leveler does NOT appear in idle discovery** because:
1. ACC/Engine power must be ON
2. Parking brake must be ENGAGED
3. Controller is physically powered off otherwise (safety interlock)

### Implementation Plan

A persistent session approach that:
1. Opens one TCP connection
2. Performs the full 5-frame registration
3. Sends screen navigation and status poll frames
4. Sends button commands within the same session
5. Sends `0x03 0x03` keepalives periodically
6. Resolves the leveler counter dynamically from func_id 88

## Safety Notes

**ONLY actuate lights without explicit confirmation.**

Dangerous devices require user confirmation:
- **Water Pump** - Burns out without water
- **Water Heater** - Fire hazard if empty
- **Slides** - Can hit objects/people
- **Awnings** - Can hit obstacles
- **Levelers/Jacks** - Can cause damage/injury; requires ACC + parking brake
- **Generator** - Fuel/safety concerns

## Future Work

- [ ] Leveler HA entities (persistent session management needed)
- [ ] Dimming support
- [ ] "All Lights" master control
- [ ] Awning extend/retract (protocol known, safety UX needed)

### Dropped Features
- ~~Slide control~~ - Users operate while watching, too dangerous for remote control

## Tools Used

- Wireshark/tcpdump for packet capture
- Python scripts for SLL2 pcap parsing (controller captures use Linux cooked capture format)
- .NET decompiler for Lippert IDS app analysis
- Python for protocol implementation

## Source Files

- `rvc/onecontrol.py` - Main client implementation (`OneControlClient` class)
- `rvc/protocol.py` - COBS encoding, CRC-8, TEA cipher
- `rvc/device_names.py` - func_id → device name mapping
- `tools/auto_discover.py` - Device discovery
- `captures/*.pcap` - Reference packet captures
