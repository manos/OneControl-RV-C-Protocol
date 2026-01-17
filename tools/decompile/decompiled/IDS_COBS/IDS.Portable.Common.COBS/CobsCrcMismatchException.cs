using System;
using System.Collections.Generic;
using IDS.Portable.Common.Extensions;

namespace IDS.Portable.Common.COBS;

public class CobsCrcMismatchException : global::System.Exception
{
	public CobsCrcMismatchException(byte computedCrc, byte payloadCrc, global::System.Collections.Generic.IReadOnlyList<byte> decodedData)
		: base($"CRC 0x{computedCrc:X} != 0x{payloadCrc:X} for {decodedData.DebugDump(0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)decodedData).Count)}")
	{
	}
}
