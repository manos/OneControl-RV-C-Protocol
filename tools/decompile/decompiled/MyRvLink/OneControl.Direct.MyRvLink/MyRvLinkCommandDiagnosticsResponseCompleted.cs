using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using IDS.Portable.Common;
using IDS.Portable.Common.Extensions;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkCommandDiagnosticsResponseCompleted : MyRvLinkCommandResponseSuccess
{
	public const string LogTag = "MyRvLinkCommandGetDevicePidResponseCompleted";

	public const byte Separator = 255;

	protected override int MinExtendedDataLength => 1;

	public global::System.Collections.Generic.IReadOnlyList<MyRvLinkCommandType> EnabledDiagnosticCommands => (global::System.Collections.Generic.IReadOnlyList<MyRvLinkCommandType>)DecodeEnabledDiagnosticCommands();

	public global::System.Collections.Generic.IReadOnlyList<MyRvLinkEventType> EnabledDiagnosticEvents => (global::System.Collections.Generic.IReadOnlyList<MyRvLinkEventType>)DecodeEnabledDiagnosticEvents();

	public MyRvLinkCommandDiagnosticsResponseCompleted(ushort clientCommandId, global::System.Collections.Generic.IReadOnlyCollection<MyRvLinkCommandType> commands, global::System.Collections.Generic.IReadOnlyCollection<MyRvLinkEventType> events)
		: base(clientCommandId, commandCompleted: true, EncodeExtendedData(commands, events))
	{
	}

	public MyRvLinkCommandDiagnosticsResponseCompleted(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
		: base(rawData)
	{
	}

	public MyRvLinkCommandDiagnosticsResponseCompleted(MyRvLinkCommandResponseSuccess response)
		: base(response.ClientCommandId, response.IsCommandCompleted, response.ExtendedData)
	{
	}

	protected List<MyRvLinkCommandType> DecodeEnabledDiagnosticCommands()
	{
		List<MyRvLinkCommandType> val = new List<MyRvLinkCommandType>();
		if (base.ExtendedData == null)
		{
			return val;
		}
		int num = 0;
		while (num < ((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count)
		{
			byte b = base.ExtendedData[num++];
			if (b == 255)
			{
				break;
			}
			val.Add((MyRvLinkCommandType)b);
		}
		return val;
	}

	protected List<MyRvLinkEventType> DecodeEnabledDiagnosticEvents()
	{
		List<MyRvLinkEventType> val = new List<MyRvLinkEventType>();
		if (base.ExtendedData == null)
		{
			return val;
		}
		int num = 0;
		bool flag = false;
		while (num < ((global::System.Collections.Generic.IReadOnlyCollection<byte>)base.ExtendedData).Count)
		{
			byte b = base.ExtendedData[num++];
			if (b == 255)
			{
				if (flag)
				{
					TaggedLog.Warning("MyRvLinkCommandGetDevicePidResponseCompleted", "Invalid response, found multiple separators", global::System.Array.Empty<object>());
					break;
				}
				flag = true;
			}
			else if (flag)
			{
				val.Add((MyRvLinkEventType)b);
			}
		}
		return val;
	}

	private static global::System.Collections.Generic.IReadOnlyList<byte> EncodeExtendedData(global::System.Collections.Generic.IReadOnlyCollection<MyRvLinkCommandType> commands, global::System.Collections.Generic.IReadOnlyCollection<MyRvLinkEventType> events)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		byte[] array = new byte[commands.Count + 1 + events.Count];
		int num = 0;
		global::System.Collections.Generic.IEnumerator<MyRvLinkCommandType> enumerator = ((global::System.Collections.Generic.IEnumerable<MyRvLinkCommandType>)commands).GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
			{
				MyRvLinkCommandType current = enumerator.Current;
				if ((byte)current == 255)
				{
					throw new ArgumentException("commands", $"Invalid command value of 0x{255:X}");
				}
				array[num++] = (byte)current;
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator)?.Dispose();
		}
		array[num++] = 255;
		global::System.Collections.Generic.IEnumerator<MyRvLinkEventType> enumerator2 = ((global::System.Collections.Generic.IEnumerable<MyRvLinkEventType>)events).GetEnumerator();
		try
		{
			while (((global::System.Collections.IEnumerator)enumerator2).MoveNext())
			{
				MyRvLinkEventType current2 = enumerator2.Current;
				if ((byte)current2 == 255)
				{
					throw new ArgumentException("commands", $"Invalid event value of 0x{255:X}");
				}
				array[num++] = (byte)current2;
			}
		}
		finally
		{
			((global::System.IDisposable)enumerator2)?.Dispose();
		}
		return (global::System.Collections.Generic.IReadOnlyList<byte>)(object)new ArraySegment<byte>(array, 0, num);
	}

	public override string ToString()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		StringBuilder val = new StringBuilder($"Command(0x{base.ClientCommandId:X4}) Response {"MyRvLinkCommandDiagnosticsResponseCompleted"}");
		global::System.Collections.Generic.IReadOnlyList<MyRvLinkCommandType> enabledDiagnosticCommands = EnabledDiagnosticCommands;
		AppendInterpolatedStringHandler val3 = default(AppendInterpolatedStringHandler);
		StringBuilder val2;
		if (((global::System.Collections.Generic.IReadOnlyCollection<MyRvLinkCommandType>)enabledDiagnosticCommands).Count == 0)
		{
			val2 = val;
			StringBuilder obj = val2;
			((AppendInterpolatedStringHandler)(ref val3))._002Ector(34, 1, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(Environment.NewLine);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("    NO Commands In Diagnostic Mode");
			obj.Append(ref val3);
		}
		else
		{
			global::System.Collections.Generic.IEnumerator<MyRvLinkCommandType> enumerator = ((global::System.Collections.Generic.IEnumerable<MyRvLinkCommandType>)enabledDiagnosticCommands).GetEnumerator();
			try
			{
				while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
				{
					MyRvLinkCommandType current = enumerator.Current;
					val2 = val;
					StringBuilder obj2 = val2;
					val3 = new AppendInterpolatedStringHandler(38, 3, val2);
					((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(Environment.NewLine);
					((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("    ");
					((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<MyRvLinkCommandType>(current);
					((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("(0x");
					((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<int>((int)current, "X");
					((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(") - Command Diagnostics Enabled");
					obj2.Append(ref val3);
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator)?.Dispose();
			}
		}
		global::System.Collections.Generic.IReadOnlyList<MyRvLinkEventType> enabledDiagnosticEvents = EnabledDiagnosticEvents;
		if (((global::System.Collections.Generic.IReadOnlyCollection<MyRvLinkEventType>)enabledDiagnosticEvents).Count == 0)
		{
			val2 = val;
			StringBuilder obj3 = val2;
			((AppendInterpolatedStringHandler)(ref val3))._002Ector(32, 1, val2);
			((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(Environment.NewLine);
			((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("    NO Events In Diagnostic Mode");
			obj3.Append(ref val3);
		}
		else
		{
			global::System.Collections.Generic.IEnumerator<MyRvLinkEventType> enumerator2 = ((global::System.Collections.Generic.IEnumerable<MyRvLinkEventType>)enabledDiagnosticEvents).GetEnumerator();
			try
			{
				while (((global::System.Collections.IEnumerator)enumerator2).MoveNext())
				{
					MyRvLinkEventType current2 = enumerator2.Current;
					val2 = val;
					StringBuilder obj4 = val2;
					val3 = new AppendInterpolatedStringHandler(36, 3, val2);
					((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(Environment.NewLine);
					((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("    ");
					((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<MyRvLinkEventType>(current2);
					((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("(0x");
					((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted<int>((int)current2, "X");
					((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral(") - Event Diagnostics Enabled");
					obj4.Append(ref val3);
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator2)?.Dispose();
			}
		}
		global::System.Collections.Generic.IReadOnlyList<byte> readOnlyList = Encode();
		val2 = val;
		StringBuilder obj5 = val2;
		((AppendInterpolatedStringHandler)(ref val3))._002Ector(14, 2, val2);
		((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(Environment.NewLine);
		((AppendInterpolatedStringHandler)(ref val3)).AppendLiteral("    Raw Data: ");
		((AppendInterpolatedStringHandler)(ref val3)).AppendFormatted(ArrayExtension.DebugDump(readOnlyList, 0, ((global::System.Collections.Generic.IReadOnlyCollection<byte>)readOnlyList).Count, " ", false));
		obj5.Append(ref val3);
		return ((object)val).ToString();
	}
}
