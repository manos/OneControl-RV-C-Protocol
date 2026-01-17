#!/usr/bin/env python3
"""
Example: Monitor OneControl traffic and print decoded messages.

Usage:
    python monitor.py [host] [--duration SECONDS]
    
Examples:
    python monitor.py                     # Monitor 192.168.1.1 for 60s
    python monitor.py 192.168.1.1         # Specify host
    python monitor.py --duration 300      # Monitor for 5 minutes
"""

import asyncio
import argparse
import logging
import sys
from datetime import datetime

# Add parent directory to path for development
sys.path.insert(0, str(__file__).rsplit('/', 2)[0])

from rvc import OneControlClient, RVCDecoder


async def monitor(host: str, port: int = 6969, duration: float = 60.0):
    """Monitor and display OneControl messages."""
    
    print(f"╔{'═'*58}╗")
    print(f"║  OneControl RV-C Monitor                                 ║")
    print(f"╠{'═'*58}╣")
    print(f"║  Host: {host:<49} ║")
    print(f"║  Port: {port:<49} ║")
    print(f"║  Duration: {duration:.0f} seconds{' '*(38-len(str(int(duration))))} ║")
    print(f"╚{'═'*58}╝")
    print()
    
    message_count = 0
    start_time = datetime.now()
    
    try:
        async with OneControlClient(host, port) as client:
            print(f"✓ Connected to {host}:{port}")
            print(f"  Listening for RV-C messages...\n")
            print("─" * 60)
            
            try:
                async with asyncio.timeout(duration):
                    async for message in client.messages():
                        message_count += 1
                        elapsed = (datetime.now() - start_time).total_seconds()
                        
                        print(f"\n[{elapsed:6.1f}s] {message.dgn_name}")
                        print(f"         DGN: 0x{message.dgn:05X}  Source: 0x{message.source_address:02X}  Instance: {message.instance}")
                        
                        for key, value in message.data.items():
                            if key not in ('instance', 'raw_bytes'):
                                print(f"         • {key}: {value}")
                        
            except asyncio.TimeoutError:
                pass
                
    except ConnectionRefusedError:
        print(f"✗ Connection refused - is OneControl at {host}:{port}?")
        return
    except OSError as e:
        print(f"✗ Connection failed: {e}")
        return
    
    print("\n" + "─" * 60)
    print(f"\n✓ Monitoring complete")
    print(f"  Duration: {(datetime.now() - start_time).total_seconds():.1f} seconds")
    print(f"  Messages: {message_count}")


def main():
    parser = argparse.ArgumentParser(
        description='Monitor OneControl RV-C traffic',
        formatter_class=argparse.RawDescriptionHelpFormatter,
        epilog="""
Examples:
  %(prog)s                          Monitor default host for 60s
  %(prog)s 192.168.1.1              Monitor specific host
  %(prog)s --duration 300           Monitor for 5 minutes
  %(prog)s -v                       Verbose (debug) output
        """
    )
    parser.add_argument(
        'host',
        nargs='?',
        default='192.168.1.1',
        help='OneControl host IP (default: 192.168.1.1)'
    )
    parser.add_argument(
        '-p', '--port',
        type=int,
        default=6969,
        help='TCP port (default: 6969)'
    )
    parser.add_argument(
        '-d', '--duration',
        type=float,
        default=60.0,
        help='Monitoring duration in seconds (default: 60)'
    )
    parser.add_argument(
        '-v', '--verbose',
        action='store_true',
        help='Enable debug logging'
    )
    
    args = parser.parse_args()
    
    if args.verbose:
        logging.basicConfig(
            level=logging.DEBUG,
            format='%(asctime)s %(levelname)s %(name)s: %(message)s'
        )
    
    try:
        asyncio.run(monitor(args.host, args.port, args.duration))
    except KeyboardInterrupt:
        print("\n\nInterrupted by user")


if __name__ == '__main__':
    main()
