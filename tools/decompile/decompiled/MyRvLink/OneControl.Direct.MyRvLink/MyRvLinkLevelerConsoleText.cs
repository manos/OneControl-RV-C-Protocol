using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using IDS.Portable.Common;
using IDS.Portable.Common.Extensions;

namespace OneControl.Direct.MyRvLink;

public class MyRvLinkLevelerConsoleText : MyRvLinkEvent<MyRvLinkLevelerConsoleText>
{
	private const ushort NullCh = 32;

	public static readonly ushort[] UnicodeCharset;

	private const int DeviceTableIdIndex = 1;

	private const int DeviceIdIndex = 2;

	private const int ConsoleMessageStartIndex = 3;

	private const byte DelimiterByte = 0;

	private const int MaxLineLength = 32;

	private byte[] Utf16LineByteArray = new byte[64];

	public override MyRvLinkEventType EventType => MyRvLinkEventType.LevelerConsoleText;

	protected override int MinPayloadLength => 2;

	public byte DeviceId => _rawData[2];

	public byte DeviceTableId => _rawData[1];

	[field: CompilerGenerated]
	protected override byte[] _rawData
	{
		[CompilerGenerated]
		get;
	}

	protected MyRvLinkLevelerConsoleText(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		_rawData = new byte[((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count];
		for (int i = 0; i < ((global::System.Collections.Generic.IReadOnlyCollection<byte>)rawData).Count; i++)
		{
			_rawData[i] = rawData[i];
		}
	}

	public static MyRvLinkLevelerConsoleText Decode(global::System.Collections.Generic.IReadOnlyList<byte> rawData)
	{
		return new MyRvLinkLevelerConsoleText(rawData);
	}

	public List<string> GetConsoleMessages()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		List<string> val = new List<string>();
		int num = 3;
		try
		{
			ArraySegment<byte> utf8ByteArray = default(ArraySegment<byte>);
			for (int i = 3; i < _rawData.Length; i++)
			{
				if (_rawData[i] == 0)
				{
					utf8ByteArray._002Ector(_rawData, num, i - num);
					val.Add(GetUtf16(utf8ByteArray).Trim());
					num = i + 1;
				}
			}
		}
		catch (global::System.Exception ex)
		{
			TaggedLog.Error("RvLinkConsoleMessages", "Leveler text console decoding error - " + (object)ex, global::System.Array.Empty<object>());
		}
		return val;
	}

	private string GetUtf16(ArraySegment<byte> utf8ByteArray)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		lock (this)
		{
			int num = 0;
			ArrayExtension.Clear<byte>(Utf16LineByteArray);
			Enumerator<byte> enumerator = utf8ByteArray.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					byte current = enumerator.Current;
					ushort num2 = UnicodeCharset[current];
					Utf16LineByteArray[2 * num] = (byte)num2;
					Utf16LineByteArray[2 * num + 1] = (byte)(num2 >> 8);
					num++;
					if (num >= 32)
					{
						break;
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
			}
			return Encoding.Unicode.GetString(Utf16LineByteArray);
		}
	}

	static MyRvLinkLevelerConsoleText()
	{
		ushort[] array = new ushort[256];
		RuntimeHelpers.InitializeArray((global::System.Array)array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
		UnicodeCharset = array;
	}
}
