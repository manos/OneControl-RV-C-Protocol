# Reverse Engineering Analysis

Working notes from reverse-engineering the Lippert OneControl protocol (January 2026).

## Summary

Successfully reverse-engineered the TCP protocol for controlling Lippert OneControl RV systems:

- ✅ COBS framing with CRC-8/MAXIM
- ✅ TEA cipher authentication (seed/key challenge-response)
- ✅ Light control (ON/OFF)
- ✅ Tank sensor reading
- ✅ Generator hour meter reading
- ✅ Auto-discovery (no packet capture needed!)
- ✅ Generator start/stop (cracked Jan 18, 2026!)
- ✅ Leveler control (button commands, no auth needed)

## Key Discoveries

### 1. Universal Control Values

The biggest breakthrough: these values work for ALL devices:

```python
PROTOCOL = 0x80
SESSION  = 0x80
CONN     = 0x40
DEVICE   = 0x04
# Only the COUNTER is device-specific
```

### 2. Authentication Per Command

Each ON or OFF command requires fresh authentication. You cannot reuse a seed/key pair.

### 3. Seed Response Protocol

Regardless of which protocol you request (0x80, 0x82, 0x83), the seed response always comes back on Protocol 0x80.

### 4. Counter is Fixed

The counter byte does NOT increment during a command sequence. It stays fixed for the device.

## Confirmed Working Devices

| Device | Counter | Notes |
|--------|---------|-------|
| Kitchen Ceiling Light | 0x28 | First device cracked |
| Living Room Ceiling | 0x77 | Working |
| Bed Ceiling Light | 0xFB | Working |
| Porch Light | 0xCF | Working |
| Awning Light | 0x15 | Working |
| Scare Light | 0x86 | Working |

## Tank Sensors

Levels broadcast on `01 03` frames:

| Tank | Counter | Frame Example |
|------|---------|---------------|
| Grey | 0x04 | `01 03 04 [level]` |
| Fresh | 0x3E | `01 03 3E [level]` |
| Black | 0x86 | `01 03 86 [level]` |
| LP | 0x10 | `01 03 10 [level]` |

## Battery Voltage

Voltage comes from the **Generator Genie** (counter 0x87) broadcasts:

```
05 03 87 [state] [volt_hi] [volt_lo] [temp_hi] [temp_lo]
```

Voltage is 8.8 fixed point: `volt_hi + volt_lo / 256`

**Usage:**
```python
from rvc.onecontrol import read_battery_voltage

voltage = await read_battery_voltage()
if voltage:
    print(f"Battery: {voltage:.2f}V")
```

## Generator Hour Meter

Hours broadcast on `05 03` frames:

```
05 03 80 [uint32 BE seconds] [status]
```

Operating seconds ÷ 3600 = hours.

## Generator Control (WORKING! ✅)

The Generator Genie (counter 0x87) uses a **different protocol** than lights!

### Key Differences from Lights

| Aspect | Lights | Generator |
|--------|--------|-----------|
| Protocol | 0x80 | **0x81** |
| Control frame type | 0x00 | **0x01** |
| Seed device type | 42 00 04 | 42 00 04 |
| Key device type | 43 00 04 | 43 00 04 |
| ON value | 0x01 | **0x02** |
| OFF value | 0x00 | **0x01** |

### Command Sequence

```
1. Seed request:   02 81 [conn] 87 42 00 04
2. Seed response:  06 82 1d 7a 42 00 04 [seed]  (from controller)
3. Key transmit:   06 81 [conn] 87 43 00 04 [key]
4. ON command:     01 81 [conn+2] 87 00 02
5. OFF command:    01 81 [conn+2] 87 00 01
```

### Status Broadcast

Generator state in `05 03 87 [state] ...`:
- 0x00 = Off
- 0x01 = Priming
- 0x02 = Starting
- 0x03 = Running
- 0x04 = Stopping

### Critical Discovery

**The control command uses frame type 0x01, NOT 0x00!**

This was the key insight that made generator control work. Lights use:
```
00 80 [conn+2] [counter] [value]
```

But generator uses:
```
01 81 [conn+2] 87 00 [cmd]
```

Where `cmd` is 0x02 for ON, 0x01 for OFF (matching `GeneratorGenieCommand` enum).

## Device Types

From decompiled OneControl app:

| Type | Category | Control |
|------|----------|---------|
| Latching Relay | Lights, Water Heater | ON/OFF toggle |
| Momentary H-Bridge | Slides, Awnings | Momentary press |
| Tank Sensor | Fresh, Grey, Black, LP | Read-only |
| Hour Meter | Generator | Read-only |
| Generator Genie | Generator | ON/OFF (not working) |
| Leveler Type 3 | Levelers | Motor control |

## Safety Notes

**ONLY actuate lights without explicit confirmation.**

Dangerous devices require user confirmation:
- **Water Pump** - Burns out without water
- **Water Heater** - Fire hazard if empty
- **Slides** - Can hit objects/people
- **Awnings** - Can hit obstacles
- **Levelers/Jacks** - Can cause damage/injury
- **Generator** - Fuel/safety concerns

## Leveler Control (CAPTURED!)

**Status:** Packet capture complete, analysis done ✅

### What We Know

Your leveler is a **Lippert Motorized 4-Point Hydraulic Leveler (Sprinter)**.
- Protocol type: **Leveler Type 3**
- Command type: `0x53` (Leveler3ButtonCommand = 83 decimal)
- Expected FUNCTION_NAME: `88` (Landing Gear)

### Key Capture Findings (Jan 18, 2026)

**1. Status Broadcasts from Controller:**
```
43 08 03 94 44 2d 5f 80 80 41 80 01 10
```
- Broadcast every ~1 second while leveler screen active
- Contains current button state: `41 80 01 10` (button=0x10 = AutoLevel)

**2. Button Command Structure (iPhone → Controller):**
```
03 80 7a 01 [table_id] [device_id] [screen] [button]
```
- Frame type: `0x03` (NOT 0x00 like lights!)
- Protocol: `0x80`
- Prefix: `7a 01`
- table_id: `0x41` (65)
- device_id: `0x01` or `0x08`
- screen: `0x02`
- button: single byte (0x10, 0x40, 0x80)

**3. NO AUTHENTICATION** required for leveler commands!

### Observed Button Values

| Button | Value | Observed in Capture |
|--------|-------|---------------------|
| AutoLevel | 0x10 | ✅ Multiple times |
| Enter | 0x40 | ✅ Confirmation |
| Cancel? | 0x80 | ✅ End of sequence |
| Retract | 0x20 | Not captured |

### Button Codes (from decompiled code)

| Button | Code | Purpose |
|--------|------|---------|
| AutoLevel | 0x0010 | Start auto-leveling |
| Retract | 0x0020 | Retract all jacks |
| Back | 0x0200 | Cancel operation |
| Enter | 0x0040 | Confirm |
| Front | 0x0008 | Manual front jack |
| Rear | 0x0004 | Manual rear jack |
| Left | 0x0002 | Manual left jack |
| Right | 0x0001 | Manual right jack |

### Auto-Level Sequence (captured)

1. **Press AutoLevel** (button=0x10) - activates auto-level
2. **Press Enter** (button=0x40) - confirms operation
3. *(Leveler runs)*
4. **Press Cancel** (button=0x80) - stops operation

### Why 0x80 for Cancel?

The decompiled code shows `Back = 0x0200` (512), but we captured `0x80` (128).
Possible explanations:
- Different button for on-screen "Cancel" vs "Back"
- Screen-specific button mapping
- 8-bit truncation of some value

### Discovery Note

**The leveler does NOT appear in idle discovery** because:
1. ACC/Engine power must be ON
2. Parking brake must be ENGAGED
3. Controller is physically powered off otherwise (safety interlock)

### Implementation Plan

```python
async def leveler_auto_level(host: str = DEFAULT_HOST) -> bool:
    """Start auto-leveling sequence."""
    # Send frame type 0x03 with button=0x10
    await send_leveler_button(table=0x41, device=0x01, screen=0x02, button=0x10)
    await asyncio.sleep(0.5)
    # Confirm with Enter
    await send_leveler_button(table=0x41, device=0x08, screen=0x02, button=0x40)
    return True

async def leveler_cancel(host: str = DEFAULT_HOST) -> bool:
    """Cancel current leveler operation."""
    await send_leveler_button(table=0x41, device=0x01, screen=0x02, button=0x80)
    return True

async def leveler_retract(host: str = DEFAULT_HOST) -> bool:
    """Retract all jacks."""
    # May need button=0x20, needs testing
    await send_leveler_button(table=0x41, device=0x01, screen=0x02, button=0x20)
    await asyncio.sleep(0.5)
    await send_leveler_button(table=0x41, device=0x08, screen=0x02, button=0x40)
    return True
```

### Key Difference from Lights

- **NO seed/key authentication needed** for leveler commands
- Uses **button-press simulation**, not ON/OFF states
- Uses **frame type 0x03** (not 0x00)
- Requires **confirmation press** (Enter) after most commands

## Future Work

- [ ] Generator start/stop control
- [ ] Leveler control (capture needed)
- [ ] Home Assistant integration
- [ ] Dimming support
- [ ] Device confirmation UI during setup
- [ ] Status polling from broadcasts
- [ ] "All Lights" master control

## Tools Used

- Wireshark/tcpdump for packet capture
- .NET decompiler for Lippert app analysis
- Python for protocol implementation

## Source Files

- `rvc/onecontrol.py` - Main client implementation
- `tools/auto_discover.py` - Device discovery
- `captures/*.pcap` - Reference packet captures
