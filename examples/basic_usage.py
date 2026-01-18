#!/usr/bin/env python3
"""
Basic Usage Examples for OneControl RV-C Protocol

Demonstrates light control, tank reading, and generator hours.

Prerequisites:
    - Connected to OneControl WiFi network (MyRV_xxxx)
    - Run auto_discover.py first to find your device counters
"""

import asyncio
import sys

sys.path.insert(0, '.')
from rvc.onecontrol import (
    control_light, LightConfig,
    read_tank_levels, read_battery_voltage, read_generator_hours
)


# Define lights using universal values + discovered counters
# Run: python tools/auto_discover.py to find your counters

KITCHEN = LightConfig(
    name="Kitchen",
    protocol=0x80,
    session=0x80,
    device=0x04,
    conn=0x40,
    counter=0x28,  # Your counter here
)

PORCH = LightConfig(
    name="Porch",
    protocol=0x80,
    session=0x80,
    device=0x04,
    conn=0x40,
    counter=0xCF,  # Your counter here
)


async def demo_lights():
    """Demonstrate light control."""
    print("=== Light Control Demo ===\n")
    
    print("Turning Kitchen ON...")
    result = await control_light(KITCHEN, on=True)
    print(f"  Result: {'Success' if result else 'Failed'}")
    
    await asyncio.sleep(2)
    
    print("Turning Kitchen OFF...")
    result = await control_light(KITCHEN, on=False)
    print(f"  Result: {'Success' if result else 'Failed'}")
    
    print()


async def demo_tanks():
    """Demonstrate tank level reading."""
    print("=== Tank Levels Demo ===\n")
    
    print("Reading tank levels...")
    levels = await read_tank_levels()
    
    if levels:
        for name, level in sorted(levels.items()):
            print(f"  {name}: {level}%")
    else:
        print("  No tank data received")
    
    print()


async def demo_battery():
    """Demonstrate battery voltage reading."""
    print("=== Battery Voltage Demo ===\n")
    
    print("Reading battery voltage...")
    voltage = await read_battery_voltage()
    
    if voltage is not None:
        print(f"  Battery: {voltage:.2f}V")
    else:
        print("  No battery data received")
    
    print()


async def demo_generator():
    """Demonstrate generator hour reading."""
    print("=== Generator Hours Demo ===\n")
    
    print("Reading generator hours...")
    hours = await read_generator_hours()
    
    if hours is not None:
        print(f"  Generator: {hours:.1f} hours")
    else:
        print("  No generator data received")
    
    print()


async def main():
    """Run all demos."""
    print("OneControl RV-C Protocol - Basic Usage Examples")
    print("=" * 50)
    print()
    
    # Uncomment the demos you want to run:
    
    # await demo_lights()
    await demo_tanks()
    await demo_battery()
    await demo_generator()
    
    print("Done!")


if __name__ == "__main__":
    asyncio.run(main())
