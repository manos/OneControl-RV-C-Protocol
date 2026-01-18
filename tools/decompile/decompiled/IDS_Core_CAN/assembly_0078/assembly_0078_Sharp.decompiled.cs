using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IDS.Core;
using IDS.Core.Collections;
using IDS.Core.Events;
using IDS.Core.Tasks;
using IDS.Core.Types;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(/*Could not decode attribute arguments.*/)]
[assembly: TargetFramework(".NETCoreApp,Version=v7.0", FrameworkDisplayName = ".NET 7.0")]
[assembly: AssemblyCompany("Innovative Design Solutions, Inc.\\r\\n6801 15 Mile Road\\r\\nSterling Heights, MI 48312\\r\\nwww.idselectronics.com")]
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyCopyright("Copyright © 2012-2025 Innovative Design Solutions, Inc.")]
[assembly: AssemblyDescription("IDS Core Library")]
[assembly: AssemblyFileVersion("4.6.4.0")]
[assembly: AssemblyInformationalVersion("4.6.4.0+e7121ede06f05d042cf24e77bdcf920e85f9188f")]
[assembly: AssemblyProduct("IDS.Core")]
[assembly: AssemblyTitle("IDS.Core")]
[assembly: AssemblyVersion("4.6.4.0")]
[module: RefSafetyRules(11)]
namespace Microsoft.CodeAnalysis
{
	[CompilerGenerated]
	[Embedded]
	internal sealed class EmbeddedAttribute : System.Attribute
	{
	}
}
namespace System.Runtime.CompilerServices
{
	[CompilerGenerated]
	[Embedded]
	[AttributeUsage(/*Could not decode attribute arguments.*/)]
	internal sealed class NullableAttribute : System.Attribute
	{
		public readonly byte[] NullableFlags;

		public NullableAttribute(byte P_0)
		{
			NullableFlags = new byte[1] { P_0 };
		}

		public NullableAttribute(byte[] P_0)
		{
			NullableFlags = P_0;
		}
	}
	[CompilerGenerated]
	[Embedded]
	[AttributeUsage(/*Could not decode attribute arguments.*/)]
	internal sealed class NullableContextAttribute : System.Attribute
	{
		public readonly byte Flag;

		public NullableContextAttribute(byte P_0)
		{
			Flag = P_0;
		}
	}
	[CompilerGenerated]
	[Embedded]
	[AttributeUsage(/*Could not decode attribute arguments.*/)]
	internal sealed class RefSafetyRulesAttribute : System.Attribute
	{
		public readonly int Version;

		public RefSafetyRulesAttribute(int P_0)
		{
			Version = P_0;
		}
	}
}
namespace System
{
	public static class Extensions
	{
		public static ITreeNode GetRootNode(this ITreeNode node)
		{
			while (node.Parent != null)
			{
				node = node.Parent;
			}
			return node;
		}

		public static bool IsRootNode(this ITreeNode node)
		{
			return node.GetRootNode() == null;
		}

		public static bool IsLeaf(this ITreeNode node)
		{
			return !node.HasChildren();
		}

		public static bool HasChildren(this ITreeNode node)
		{
			System.Collections.Generic.IEnumerator<ITreeNode> enumerator = node.Children.GetEnumerator();
			try
			{
				if (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					_ = enumerator.Current;
					return true;
				}
			}
			finally
			{
				((System.IDisposable)enumerator)?.Dispose();
			}
			return false;
		}

		public static int Level(this ITreeNode node)
		{
			int num = 0;
			while (node.Parent != null)
			{
				num++;
				node = node.Parent;
			}
			return num;
		}

		public static int GetNodeCount(this ITreeNode node, bool includeSubTrees)
		{
			int num = 0;
			System.Collections.Generic.IEnumerator<ITreeNode> enumerator = node.Children.GetEnumerator();
			try
			{
				while (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					ITreeNode current = enumerator.Current;
					num++;
					if (includeSubTrees)
					{
						num += current.GetNodeCount(includeSubTrees: true);
					}
				}
				return num;
			}
			finally
			{
				((System.IDisposable)enumerator)?.Dispose();
			}
		}

		public static ITreeNode FirstNode(this ITreeNode node)
		{
			System.Collections.Generic.IEnumerator<ITreeNode> enumerator = node.Children.GetEnumerator();
			try
			{
				if (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					return enumerator.Current;
				}
			}
			finally
			{
				((System.IDisposable)enumerator)?.Dispose();
			}
			return null;
		}

		public static ITreeNode NextNode(this ITreeNode node)
		{
			if (node.Parent != null)
			{
				bool flag = false;
				System.Collections.Generic.IEnumerator<ITreeNode> enumerator = node.Parent.Children.GetEnumerator();
				try
				{
					while (((System.Collections.IEnumerator)enumerator).MoveNext())
					{
						ITreeNode current = enumerator.Current;
						if (flag)
						{
							return current;
						}
						if (current == node)
						{
							flag = true;
						}
					}
				}
				finally
				{
					((System.IDisposable)enumerator)?.Dispose();
				}
			}
			return null;
		}

		public static bool IsRelatedNode(this ITreeNode n, ITreeNode node)
		{
			return n.GetRootNode() == node.GetRootNode();
		}

		public static void RemoveFromParent(this ITreeNode node)
		{
			node.Parent?.RemoveChild(node);
		}
	}
}
namespace IDS.Core
{
	public abstract class Bindable : INotifyPropertyChanged
	{
		[CompilerGenerated]
		private PropertyChangedEventHandler m_PropertyChanged;

		public event PropertyChangedEventHandler PropertyChanged
		{
			[CompilerGenerated]
			add
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected O, but got Unknown
				PropertyChangedEventHandler val = this.m_PropertyChanged;
				PropertyChangedEventHandler val2;
				do
				{
					val2 = val;
					PropertyChangedEventHandler val3 = (PropertyChangedEventHandler)System.Delegate.Combine((System.Delegate)(object)val2, (System.Delegate)(object)value);
					val = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.m_PropertyChanged, val3, val2);
				}
				while (val != val2);
			}
			[CompilerGenerated]
			remove
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected O, but got Unknown
				PropertyChangedEventHandler val = this.m_PropertyChanged;
				PropertyChangedEventHandler val2;
				do
				{
					val2 = val;
					PropertyChangedEventHandler val3 = (PropertyChangedEventHandler)System.Delegate.Remove((System.Delegate)(object)val2, (System.Delegate)(object)value);
					val = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.m_PropertyChanged, val3, val2);
				}
				while (val != val2);
			}
		}

		protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
		{
			if (object.Equals((object)storage, (object)value))
			{
				return false;
			}
			storage = value;
			OnPropertyChanged(propertyName);
			return true;
		}

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			PropertyChangedEventHandler obj = this.PropertyChanged;
			if (obj != null)
			{
				obj.Invoke((object)this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	public static class BuildInfo
	{
		public const int BuildYear = 2025;

		public const int BuildMonth = 10;

		public const int BuildDay = 21;

		public const int BuildHour = 14;

		public const int BuildMinute = 9;

		public const int BuildSecond = 22;

		public static readonly System.DateTime DateTime = new System.DateTime(2025, 10, 21, 14, 9, 22);
	}
	public static class CAN
	{
		public interface IAdapter : Comm.IAdapter, IEventSender, IDisposableManager, IDisposable, System.IDisposable
		{
			int BaudRate { get; }
		}

		public interface IAdapter<T> : Comm.IAdapter<T>, Comm.IAdapter, IEventSender, IDisposableManager, IDisposable, System.IDisposable, IAdapter where T : IMessage
		{
			bool Transmit(ID id, PAYLOAD payload = default(PAYLOAD));
		}

		public abstract class Adapter<T> : Comm.Adapter<T>, IAdapter<T>, Comm.IAdapter<T>, Comm.IAdapter, IEventSender, IDisposableManager, IDisposable, System.IDisposable, IAdapter where T : MessageBuffer, new()
		{
			private class TransmitRateManager
			{
				[CompilerGenerated]
				private sealed class <>c__DisplayClass3_0
				{
					[StructLayout((LayoutKind)3)]
					private struct <<-ctor>b__0>d : IAsyncStateMachine
					{
						public int <>1__state;

						public AsyncTaskMethodBuilder <>t__builder;

						public <>c__DisplayClass3_0 <>4__this;

						private Timer <timer>5__2;

						private ConfiguredTaskAwaiter <>u__1;

						private void MoveNext()
						{
							//IL_00af: Unknown result type (might be due to invalid IL or missing references)
							//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
							//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
							//IL_0037: Unknown result type (might be due to invalid IL or missing references)
							//IL_003c: Unknown result type (might be due to invalid IL or missing references)
							//IL_0074: Unknown result type (might be due to invalid IL or missing references)
							//IL_0079: Unknown result type (might be due to invalid IL or missing references)
							//IL_007d: Unknown result type (might be due to invalid IL or missing references)
							//IL_0082: Unknown result type (might be due to invalid IL or missing references)
							//IL_0097: Unknown result type (might be due to invalid IL or missing references)
							//IL_0099: Unknown result type (might be due to invalid IL or missing references)
							int num = <>1__state;
							<>c__DisplayClass3_0 <>c__DisplayClass3_ = <>4__this;
							try
							{
								if (num != 0)
								{
									<timer>5__2 = new Timer();
									goto IL_00d2;
								}
								ConfiguredTaskAwaiter val = <>u__1;
								<>u__1 = default(ConfiguredTaskAwaiter);
								num = (<>1__state = -1);
								goto IL_00cb;
								IL_00cb:
								((ConfiguredTaskAwaiter)(ref val)).GetResult();
								goto IL_00d2;
								IL_00d2:
								if (!<>c__DisplayClass3_.adapter.IsDisposed)
								{
									double num2 = <>c__DisplayClass3_.adapter.BaudRate;
									TimeSpan elapsedTimeAndReset = <timer>5__2.GetElapsedTimeAndReset();
									int num3 = (int)(num2 * ((TimeSpan)(ref elapsedTimeAndReset)).TotalSeconds);
									num3 = Math.Min(num3, <>c__DisplayClass3_.<>4__this.AccumulatedBits);
									Interlocked.Add(ref <>c__DisplayClass3_.<>4__this.AccumulatedBits, -num3);
									ConfiguredTaskAwaitable val2 = System.Threading.Tasks.Task.Delay(10).ConfigureAwait(false);
									val = ((ConfiguredTaskAwaitable)(ref val2)).GetAwaiter();
									if (!((ConfiguredTaskAwaiter)(ref val)).IsCompleted)
									{
										num = (<>1__state = 0);
										<>u__1 = val;
										((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <<-ctor>b__0>d>(ref val, ref this);
										return;
									}
									goto IL_00cb;
								}
							}
							catch (System.Exception exception)
							{
								<>1__state = -2;
								<timer>5__2 = null;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
								return;
							}
							<>1__state = -2;
							<timer>5__2 = null;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).SetResult();
						}

						[DebuggerHidden]
						private void SetStateMachine(IAsyncStateMachine stateMachine)
						{
							((AsyncTaskMethodBuilder)(ref <>t__builder)).SetStateMachine(stateMachine);
						}
					}

					public IAdapter adapter;

					public TransmitRateManager <>4__this;

					[AsyncStateMachine(typeof(Adapter<>.TransmitRateManager.<>c__DisplayClass3_0.<<-ctor>b__0>d))]
					internal System.Threading.Tasks.Task? <.ctor>b__0()
					{
						//IL_0002: Unknown result type (might be due to invalid IL or missing references)
						//IL_0007: Unknown result type (might be due to invalid IL or missing references)
						<<-ctor>b__0>d <<-ctor>b__0>d = default(<<-ctor>b__0>d);
						<<-ctor>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
						<<-ctor>b__0>d.<>4__this = this;
						<<-ctor>b__0>d.<>1__state = -1;
						((AsyncTaskMethodBuilder)(ref <<-ctor>b__0>d.<>t__builder)).Start<<<-ctor>b__0>d>(ref <<-ctor>b__0>d);
						return ((AsyncTaskMethodBuilder)(ref <<-ctor>b__0>d.<>t__builder)).Task;
					}
				}

				private const int MAX_PENDING_BITS_MS = 20;

				private readonly int Limit;

				private int AccumulatedBits;

				public bool TransmitAllowed => AccumulatedBits <= Limit;

				public TransmitRateManager(IAdapter adapter)
				{
					<>c__DisplayClass3_0 CS$<>8__locals4 = new <>c__DisplayClass3_0();
					CS$<>8__locals4.adapter = adapter;
					base..ctor();
					CS$<>8__locals4.<>4__this = this;
					Limit = CS$<>8__locals4.adapter.BaudRate * 20 / 1000;
					System.Threading.Tasks.Task.Run((Func<System.Threading.Tasks.Task>)([AsyncStateMachine(typeof(Adapter<>.TransmitRateManager.<>c__DisplayClass3_0.<<-ctor>b__0>d))] () =>
					{
						//IL_0002: Unknown result type (might be due to invalid IL or missing references)
						//IL_0007: Unknown result type (might be due to invalid IL or missing references)
						<>c__DisplayClass3_0.<<-ctor>b__0>d <<-ctor>b__0>d = default(<>c__DisplayClass3_0.<<-ctor>b__0>d);
						<<-ctor>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
						<<-ctor>b__0>d.<>4__this = CS$<>8__locals4;
						<<-ctor>b__0>d.<>1__state = -1;
						((AsyncTaskMethodBuilder)(ref <<-ctor>b__0>d.<>t__builder)).Start<<>c__DisplayClass3_0.<<-ctor>b__0>d>(ref <<-ctor>b__0>d);
						return ((AsyncTaskMethodBuilder)(ref <<-ctor>b__0>d.<>t__builder)).Task;
					}));
				}

				public void AddBits(MessageBuffer buffer)
				{
					Interlocked.Add(ref AccumulatedBits, buffer.EstimateNumberOfBitsInMessage());
				}
			}

			private readonly TransmitRateManager RateManger;

			public int BaudRate
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				private set;
			}

			public Adapter(string name, int baud_rate)
				: this(name, baud_rate, verbose: false)
			{
			}

			public Adapter(string name, int baud_rate, bool verbose)
				: base(name, verbose)
			{
				BaudRate = baud_rate;
				RateManger = new TransmitRateManager(this);
			}

			public override bool Transmit(T message)
			{
				if (!RateManger.TransmitAllowed)
				{
					return false;
				}
				if (!base.Transmit(message))
				{
					return false;
				}
				RateManger.AddBits(message);
				return true;
			}

			public virtual bool Transmit(ID id, PAYLOAD payload = default(PAYLOAD))
			{
				if (!RateManger.TransmitAllowed)
				{
					return false;
				}
				T val = ResourcePool<T>.GetObject();
				if (val == null)
				{
					return false;
				}
				try
				{
					val.ID = id;
					val.Payload = payload;
					val.SetTimeStamp();
					return Transmit(val);
				}
				finally
				{
					val?.ReturnToPool();
				}
			}
		}

		[JsonConverter(typeof(IdConverter))]
		public struct ID : IComparable<ID>, IEquatable<ID>
		{
			private class IdConverter : JsonConverter
			{
				public override bool CanConvert(System.Type objectType)
				{
					return objectType == typeof(ID);
				}

				public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
				{
					//IL_0008: Unknown result type (might be due to invalid IL or missing references)
					//IL_000f: Invalid comparison between Unknown and I4
					JToken val = JToken.Load(reader);
					if ((int)val.Type == 10)
					{
						return null;
					}
					return new ID(((object)val).ToString());
				}

				public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
				{
					//IL_0027: Unknown result type (might be due to invalid IL or missing references)
					if (value is ID iD)
					{
						JToken.FromObject((object)iD.JsonString).WriteTo(writer, System.Array.Empty<JsonConverter>());
						return;
					}
					throw new ArgumentException();
				}
			}

			private const uint EXTENDED = 2147483648u;

			private readonly uint mValue;

			private string JsonString
			{
				get
				{
					if (IsExtended)
					{
						return Value.ToString("X8");
					}
					return Value.ToString("X3");
				}
			}

			public uint Value => mValue & 0x1FFFFFFF;

			public bool IsExtended => mValue > 2047;

			public ID(uint value, bool isExtended)
			{
				if (isExtended)
				{
					mValue = (value & 0x1FFFFFFF) | 0x80000000u;
				}
				else
				{
					mValue = value & 0x7FF;
				}
			}

			private ID(string json)
			{
				mValue = uint.Parse(json, (NumberStyles)515);
				if (json.Length == 8)
				{
					mValue |= 2147483648u;
				}
			}

			public override string ToString()
			{
				if (IsExtended)
				{
					return "x" + Value.ToString("X8") + "h";
				}
				return Value.ToString("X3") + "h";
			}

			public int CompareTo(ID other)
			{
				return mValue.CompareTo(other.mValue);
			}

			public bool Equals(ID other)
			{
				return mValue.Equals(other.mValue);
			}

			public static explicit operator uint(ID id)
			{
				return id.Value;
			}
		}

		[DefaultMember("Item")]
		[JsonConverter(typeof(PayloadConverter))]
		public struct PAYLOAD : Comm.IByteBuffer, Comm.IByteList, System.Collections.Generic.IReadOnlyList<byte>, System.Collections.Generic.IEnumerable<byte>, System.Collections.IEnumerable, System.Collections.Generic.IReadOnlyCollection<byte>, IComparable<PAYLOAD>, IEquatable<PAYLOAD>
		{
			private class PayloadConverter : JsonConverter
			{
				public override bool CanConvert(System.Type objectType)
				{
					return objectType == typeof(PAYLOAD);
				}

				public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
				{
					//IL_0008: Unknown result type (might be due to invalid IL or missing references)
					//IL_000f: Invalid comparison between Unknown and I4
					JToken val = JToken.Load(reader);
					if ((int)val.Type == 10)
					{
						return null;
					}
					return new PAYLOAD(((object)val).ToString());
				}

				public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
				{
					//IL_0027: Unknown result type (might be due to invalid IL or missing references)
					if (value is PAYLOAD pAYLOAD)
					{
						JToken.FromObject((object)pAYLOAD.JsonString).WriteTo(writer, System.Array.Empty<JsonConverter>());
						return;
					}
					throw new ArgumentException();
				}
			}

			[CompilerGenerated]
			private sealed class <GetEnumerator>d__40 : System.Collections.Generic.IEnumerator<byte>, System.Collections.IEnumerator, System.IDisposable
			{
				private int <>1__state;

				private byte <>2__current;

				public PAYLOAD <>4__this;

				private int <n>5__2;

				byte System.Collections.Generic.IEnumerator<byte>.Current
				{
					[DebuggerHidden]
					get
					{
						return <>2__current;
					}
				}

				object System.Collections.IEnumerator.Current
				{
					[DebuggerHidden]
					get
					{
						return <>2__current;
					}
				}

				[DebuggerHidden]
				public <GetEnumerator>d__40(int <>1__state)
				{
					this.<>1__state = <>1__state;
				}

				[DebuggerHidden]
				void System.IDisposable.Dispose()
				{
				}

				private bool MoveNext()
				{
					switch (<>1__state)
					{
					default:
						return false;
					case 0:
						<>1__state = -1;
						<n>5__2 = 0;
						break;
					case 1:
						<>1__state = -1;
						<n>5__2++;
						break;
					}
					if (<n>5__2 < <>4__this.Length)
					{
						<>2__current = <>4__this[<n>5__2];
						<>1__state = 1;
						return true;
					}
					return false;
				}

				bool System.Collections.IEnumerator.MoveNext()
				{
					//ILSpy generated this explicit interface implementation from .override directive in MoveNext
					return this.MoveNext();
				}

				[DebuggerHidden]
				void System.Collections.IEnumerator.Reset()
				{
					//IL_0000: Unknown result type (might be due to invalid IL or missing references)
					throw new NotSupportedException();
				}
			}

			public const int CAPACITY = 8;

			private int mLength;

			private byte b0;

			private byte b1;

			private byte b2;

			private byte b3;

			private byte b4;

			private byte b5;

			private byte b6;

			private byte b7;

			public int Count => Length;

			public int Capacity
			{
				get
				{
					return 8;
				}
				set
				{
					//IL_0009: Unknown result type (might be due to invalid IL or missing references)
					if (value != 8)
					{
						throw new InvalidOperationException("CAN.PAYLOAD Capacity cannot be set");
					}
				}
			}

			private string JsonString
			{
				get
				{
					string text = "";
					System.Collections.Generic.IEnumerator<byte> enumerator = GetEnumerator();
					try
					{
						while (((System.Collections.IEnumerator)enumerator).MoveNext())
						{
							byte current = enumerator.Current;
							text += current.HexString();
						}
						return text;
					}
					finally
					{
						((System.IDisposable)enumerator)?.Dispose();
					}
				}
			}

			public int Length
			{
				get
				{
					return mLength;
				}
				set
				{
					//IL_000d: Unknown result type (might be due to invalid IL or missing references)
					if (value < 0 || value > 8)
					{
						throw new ArgumentException("Length");
					}
					mLength = value;
				}
			}

			public byte this[int index]
			{
				get
				{
					//IL_002b: Unknown result type (might be due to invalid IL or missing references)
					return index switch
					{
						0 => b0, 
						1 => b1, 
						2 => b2, 
						3 => b3, 
						4 => b4, 
						5 => b5, 
						6 => b6, 
						7 => b7, 
						_ => throw new ArgumentOutOfRangeException("len"), 
					};
				}
				set
				{
					//IL_002b: Unknown result type (might be due to invalid IL or missing references)
					switch (index)
					{
					default:
						throw new ArgumentOutOfRangeException("len");
					case 0:
						b0 = value;
						break;
					case 1:
						b1 = value;
						break;
					case 2:
						b2 = value;
						break;
					case 3:
						b3 = value;
						break;
					case 4:
						b4 = value;
						break;
					case 5:
						b5 = value;
						break;
					case 6:
						b6 = value;
						break;
					case 7:
						b7 = value;
						break;
					}
				}
			}

			public void Clear()
			{
				Length = 0;
			}

			public PAYLOAD(int length)
			{
				this = default(PAYLOAD);
				Length = length;
			}

			public PAYLOAD(System.Collections.Generic.IReadOnlyList<byte> data)
			{
				this = default(PAYLOAD);
				Append(data);
			}

			public PAYLOAD(System.Collections.Generic.IReadOnlyList<byte> data, int count)
			{
				this = default(PAYLOAD);
				Append(data, count);
			}

			public PAYLOAD(System.Collections.Generic.IReadOnlyList<byte> data, int index, int count)
			{
				this = default(PAYLOAD);
				Append(data, index, count);
			}

			public PAYLOAD(System.Collections.Generic.IEnumerable<byte> something)
			{
				this = default(PAYLOAD);
				Append(something);
			}

			public static PAYLOAD FromArgs(params object[] args)
			{
				//IL_0256: Unknown result type (might be due to invalid IL or missing references)
				PAYLOAD result = default(PAYLOAD);
				foreach (object obj in args)
				{
					if (obj is byte value)
					{
						result.Append(value);
						continue;
					}
					if (obj is sbyte value2)
					{
						result.Append(value2);
						continue;
					}
					if (obj is char value3)
					{
						result.Append(value3);
						continue;
					}
					if (obj is ushort value4)
					{
						result.Append(value4);
						continue;
					}
					if (obj is short value5)
					{
						result.Append(value5);
						continue;
					}
					if (obj is UInt24 value6)
					{
						result.Append(value6);
						continue;
					}
					if (obj is Int24 value7)
					{
						result.Append(value7);
						continue;
					}
					if (obj is uint value8)
					{
						result.Append(value8);
						continue;
					}
					if (obj is int value9)
					{
						result.Append(value9);
						continue;
					}
					if (obj is UInt40 value10)
					{
						result.Append(value10);
						continue;
					}
					if (obj is Int40 value11)
					{
						result.Append(value11);
						continue;
					}
					if (obj is UInt48 value12)
					{
						result.Append(value12);
						continue;
					}
					if (obj is Int48 value13)
					{
						result.Append(value13);
						continue;
					}
					if (obj is UInt56 value14)
					{
						result.Append(value14);
						continue;
					}
					if (obj is Int56 value15)
					{
						result.Append(value15);
						continue;
					}
					if (obj is ulong value16)
					{
						result.Append(value16);
						continue;
					}
					if (obj is long value17)
					{
						result.Append(value17);
						continue;
					}
					if (obj is System.Collections.Generic.IEnumerable<byte> bytes)
					{
						result.Append(bytes);
						continue;
					}
					throw new ArgumentException($"{obj} is type {obj.GetType()}");
				}
				return result;
			}

			public PAYLOAD(string json)
			{
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				if (json.Length < 0 || json.Length > 16 || (json.Length & 1) != 0)
				{
					throw new ArgumentException();
				}
				this = default(PAYLOAD);
				if (json.Length > 0)
				{
					ulong num = ulong.Parse(json, (NumberStyles)515);
					Length = json.Length >> 1;
					for (int num2 = Length - 1; num2 >= 0; num2--)
					{
						this[num2] = (byte)num;
						num >>= 8;
					}
				}
			}

			public static bool operator ==(PAYLOAD s1, PAYLOAD s2)
			{
				if (s1.mLength != s2.mLength)
				{
					return false;
				}
				for (int i = 0; i < s1.mLength; i++)
				{
					if (s1[i] != s2[i])
					{
						return false;
					}
				}
				return true;
			}

			public static bool operator !=(PAYLOAD s1, PAYLOAD s2)
			{
				return !(s1 == s2);
			}

			public override bool Equals(object obj)
			{
				if (obj is PAYLOAD pAYLOAD)
				{
					return pAYLOAD == this;
				}
				return false;
			}

			public bool Equals(PAYLOAD other)
			{
				return this == other;
			}

			public override int GetHashCode()
			{
				int num = mLength;
				for (int i = 0; i < mLength; i++)
				{
					num = num * 31 + this[i];
				}
				return num;
			}

			public int CompareTo(PAYLOAD other)
			{
				if (Length != other.Length)
				{
					return Length.CompareTo(other.Length);
				}
				for (int i = 0; i < Length; i++)
				{
					if (this[i] < other[i])
					{
						return -1;
					}
					if (this[i] > other[i])
					{
						return 1;
					}
				}
				return 0;
			}

			public void CopyTo(byte[] array, int index)
			{
				for (int i = 0; i < Length; i++)
				{
					array[index++] = this[i];
				}
			}

			public void CopyRangeTo(int sourceIndex, int count, byte[] array, int destIndex)
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				if (sourceIndex + count > Length)
				{
					throw new ArgumentOutOfRangeException();
				}
				for (int i = 0; i < count; i++)
				{
					array[destIndex++] = this[sourceIndex++];
				}
			}

			[IteratorStateMachine(typeof(<GetEnumerator>d__40))]
			public System.Collections.Generic.IEnumerator<byte> GetEnumerator()
			{
				for (int n = 0; n < Length; n++)
				{
					yield return this[n];
				}
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return (System.Collections.IEnumerator)GetEnumerator();
			}

			public void Append(byte value)
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				if (Length >= 8)
				{
					throw new InvalidOperationException("Can't append more than 8 bytes to a CAN.PAYLOAD");
				}
				this[Length++] = value;
			}

			public void Append(sbyte value)
			{
				Append((byte)value);
			}

			public void Append(char value)
			{
				Append((byte)value);
			}

			public void Append(short value)
			{
				Append((byte)(value >> 8));
				Append((byte)value);
			}

			public void Append(ushort value)
			{
				Append((byte)(value >> 8));
				Append((byte)value);
			}

			public void Append(Int24 value)
			{
				Append((byte)(value >> 16));
				Append((ushort)value);
			}

			public void Append(UInt24 value)
			{
				Append((byte)(value >> 16));
				Append((ushort)value);
			}

			public void Append(int value)
			{
				Append((ushort)(value >> 16));
				Append((ushort)value);
			}

			public void Append(uint value)
			{
				Append((ushort)(value >> 16));
				Append((ushort)value);
			}

			public void Append(Int40 value)
			{
				Append((uint)(value >> 8));
				Append((byte)value);
			}

			public void Append(UInt40 value)
			{
				Append((uint)(value >> 8));
				Append((byte)value);
			}

			public void Append(Int48 value)
			{
				Append((ushort)(value >> 32));
				Append((uint)value);
			}

			public void Append(UInt48 value)
			{
				Append((ushort)(value >> 32));
				Append((uint)value);
			}

			public void Append(Int56 value)
			{
				Append((uint)(value >> 24));
				Append((ushort)(value >> 8));
				Append((byte)value);
			}

			public void Append(UInt56 value)
			{
				Append((uint)(value >> 24));
				Append((ushort)(value >> 8));
				Append((byte)value);
			}

			public void Append(long value)
			{
				Append((uint)(value >> 32));
				Append((uint)value);
			}

			public void Append(ulong value)
			{
				Append((uint)(value >> 32));
				Append((uint)value);
			}

			public void Append(System.Collections.Generic.IEnumerable<byte> bytes)
			{
				System.Collections.Generic.IEnumerator<byte> enumerator = bytes.GetEnumerator();
				try
				{
					while (((System.Collections.IEnumerator)enumerator).MoveNext())
					{
						byte current = enumerator.Current;
						Append(current);
					}
				}
				finally
				{
					((System.IDisposable)enumerator)?.Dispose();
				}
			}

			public void Append(System.Collections.Generic.IReadOnlyList<byte> buffer)
			{
				System.Collections.Generic.IEnumerator<byte> enumerator = ((System.Collections.Generic.IEnumerable<byte>)buffer).GetEnumerator();
				try
				{
					while (((System.Collections.IEnumerator)enumerator).MoveNext())
					{
						byte current = enumerator.Current;
						Append(current);
					}
				}
				finally
				{
					((System.IDisposable)enumerator)?.Dispose();
				}
			}

			public void Append(System.Collections.Generic.IReadOnlyList<byte> buffer, int count)
			{
				for (int i = 0; i < count; i++)
				{
					Append(buffer[i]);
				}
			}

			public void Append(System.Collections.Generic.IReadOnlyList<byte> buffer, int index, int count)
			{
				for (int i = 0; i < count; i++)
				{
					Append(buffer[index++]);
				}
			}

			public void Append(Comm.IByteList buffer)
			{
				System.Collections.Generic.IEnumerator<byte> enumerator = ((System.Collections.Generic.IEnumerable<byte>)buffer).GetEnumerator();
				try
				{
					while (((System.Collections.IEnumerator)enumerator).MoveNext())
					{
						byte current = enumerator.Current;
						Append(current);
					}
				}
				finally
				{
					((System.IDisposable)enumerator)?.Dispose();
				}
			}

			public void Append(Comm.IByteList buffer, int count)
			{
				for (int i = 0; i < count; i++)
				{
					Append(((System.Collections.Generic.IReadOnlyList<byte>)buffer)[i]);
				}
			}

			public void Append(Comm.IByteList buffer, int index, int count)
			{
				for (int i = 0; i < count; i++)
				{
					Append(((System.Collections.Generic.IReadOnlyList<byte>)buffer)[index++]);
				}
			}

			public void Append(byte[] buffer)
			{
				Append(buffer, 0, buffer.Length);
			}

			public void Append(byte[] buffer, int count)
			{
				Append(buffer, 0, count);
			}

			public void Append(byte[] buffer, int index, int count)
			{
				for (int i = 0; i < count; i++)
				{
					Append(buffer[index++]);
				}
			}

			public override string ToString()
			{
				return ToString(dataonly: false);
			}

			public string ToString(bool dataonly)
			{
				return Comm.ToString(this, dataonly);
			}
		}

		public struct PACKET
		{
			[field: CompilerGenerated]
			public ID ID
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				set;
			}

			[field: CompilerGenerated]
			public PAYLOAD Payload
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				set;
			}

			public PACKET(ID id, PAYLOAD payload)
			{
				ID = id;
				Payload = payload;
			}
		}

		public interface IReadOnlyPacket : Comm.ITimeStamp
		{
			ID ID { get; }

			PAYLOAD Payload { get; }
		}

		public interface IPacket : IReadOnlyPacket, Comm.ITimeStamp
		{
			new ID ID { get; set; }

			new PAYLOAD Payload { get; set; }

			new TimeSpan Timestamp { get; set; }
		}

		public interface IMessage : Comm.IMessage, Comm.IByteList, System.Collections.Generic.IReadOnlyList<byte>, System.Collections.Generic.IEnumerable<byte>, System.Collections.IEnumerable, System.Collections.Generic.IReadOnlyCollection<byte>, Comm.ITimeStamp, IReadOnlyPacket
		{
		}

		public interface IMessageBuffer : Comm.IMessageBuffer, Comm.IMessage, Comm.IByteList, System.Collections.Generic.IReadOnlyList<byte>, System.Collections.Generic.IEnumerable<byte>, System.Collections.IEnumerable, System.Collections.Generic.IReadOnlyCollection<byte>, Comm.ITimeStamp, Comm.IByteBuffer, IMessage, IReadOnlyPacket, IPacket
		{
		}

		public class MessageBuffer : Comm.MessageBuffer, IMessageBuffer, Comm.IMessageBuffer, Comm.IMessage, Comm.IByteList, System.Collections.Generic.IReadOnlyList<byte>, System.Collections.Generic.IEnumerable<byte>, System.Collections.IEnumerable, System.Collections.Generic.IReadOnlyCollection<byte>, Comm.ITimeStamp, Comm.IByteBuffer, IMessage, IReadOnlyPacket, IPacket
		{
			[field: CompilerGenerated]
			public ID ID
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				set;
			}

			public PAYLOAD Payload
			{
				get
				{
					return new PAYLOAD((System.Collections.Generic.IReadOnlyList<byte>)this);
				}
				set
				{
					base.Length = 0;
					for (int i = 0; i < value.Length; i++)
					{
						Append(value[i]);
					}
				}
			}

			public override int Capacity
			{
				get
				{
					return 8;
				}
				set
				{
					//IL_0009: Unknown result type (might be due to invalid IL or missing references)
					if (value != 8)
					{
						throw new InvalidOperationException("CAN.MessageBuffer.Capacity cannot be changed");
					}
				}
			}

			public MessageBuffer()
				: base(8)
			{
			}

			protected override void ResetPoolObjectState()
			{
				ID = default(ID);
				base.ResetPoolObjectState();
			}

			public override void Clear()
			{
				ID = default(ID);
				base.Clear();
			}

			public override void CopyFrom(Comm.IMessage message)
			{
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				if (message is IMessage message2)
				{
					CopyFrom(message2);
					return;
				}
				throw new InvalidOperationException("Cannot copy Core.IMessage into CAN.MessageBuffer");
			}

			public virtual void CopyFrom(IMessage message)
			{
				ID = message.ID;
				base.CopyFrom(message);
			}

			public override string ToString()
			{
				return CAN.ToString((IReadOnlyPacket)this);
			}

			public override string ToString(bool dataonly)
			{
				if (dataonly)
				{
					return base.ToString(dataonly: true);
				}
				return ((object)this).ToString();
			}
		}

		public const uint _11_BIT_ID_MASK = 2047u;

		public const uint _29_BIT_ID_MASK = 536870911u;

		public static int EstimateNumberOfBitsInMessage(ID id, int dlc)
		{
			if (!id.IsExtended)
			{
				return 47 + 9 * dlc;
			}
			return 69 + 9 * dlc;
		}

		private static string ToString(ID id, PAYLOAD payload)
		{
			return $"ID = {id}, {payload.ToString(dataonly: false)}";
		}

		private static string ToString(IReadOnlyPacket packet)
		{
			return ToString(packet.ID, packet.Payload);
		}
	}
	public static class CANExtensions
	{
		public static int EstimateNumberOfBitsInMessage(this CAN.IReadOnlyPacket msg)
		{
			return CAN.EstimateNumberOfBitsInMessage(msg.ID, msg.Payload.Length);
		}
	}
	public static class Comm
	{
		public interface IPhysicalAddress : System.Collections.Generic.IReadOnlyList<byte>, System.Collections.Generic.IEnumerable<byte>, System.Collections.IEnumerable, System.Collections.Generic.IReadOnlyCollection<byte>, IEquatable<PhysicalAddress>, IComparable, IComparable<PhysicalAddress>
		{
		}

		[DefaultMember("Item")]
		public class PhysicalAddress : IPhysicalAddress, System.Collections.Generic.IReadOnlyList<byte>, System.Collections.Generic.IEnumerable<byte>, System.Collections.IEnumerable, System.Collections.Generic.IReadOnlyCollection<byte>, IEquatable<PhysicalAddress>, IComparable, IComparable<PhysicalAddress>
		{
			protected readonly byte[] Buffer;

			public int Count => Buffer.Length;

			public byte this[int index] => Buffer[index];

			public PhysicalAddress(int size)
			{
				Buffer = new byte[size];
			}

			public PhysicalAddress(byte[] buffer)
				: this(buffer.Length)
			{
				CopyFrom(buffer);
			}

			public PhysicalAddress(System.Collections.Generic.IReadOnlyList<byte> mac)
				: this(((System.Collections.Generic.IReadOnlyCollection<byte>)mac).Count)
			{
				CopyFrom(mac);
			}

			public PhysicalAddress(IPhysicalAddress mac)
				: this(((System.Collections.Generic.IReadOnlyCollection<byte>)mac).Count)
			{
				CopyFrom(mac);
			}

			public System.Collections.Generic.IEnumerator<byte> GetEnumerator()
			{
				return ((System.Collections.Generic.IEnumerable<byte>)Buffer).GetEnumerator();
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return (System.Collections.IEnumerator)GetEnumerator();
			}

			public void SetRandomMACValue()
			{
				ThreadLocalRandom.NextBytes(Buffer);
			}

			public override int GetHashCode()
			{
				int num = Buffer.Length;
				byte[] buffer = Buffer;
				foreach (byte b in buffer)
				{
					num = num * 31 + b;
				}
				return num;
			}

			public static bool operator ==(PhysicalAddress a1, PhysicalAddress a2)
			{
				return a1?.Equals(a2) ?? ((object)a2 == null);
			}

			public static bool operator !=(PhysicalAddress a1, PhysicalAddress a2)
			{
				return !(a1 == a2);
			}

			public bool Equals(PhysicalAddress other)
			{
				return CompareTo(other) == 0;
			}

			public override bool Equals(object obj)
			{
				if (obj == null || !(obj is PhysicalAddress))
				{
					return false;
				}
				return Equals(obj as PhysicalAddress);
			}

			public static int Compare(byte[] mac1, byte[] mac2)
			{
				int num = Math.Min(mac1.Length, mac2.Length);
				for (int i = 0; i < num; i++)
				{
					if (mac1[i] > mac2[i])
					{
						return 1;
					}
					if (mac1[i] < mac2[i])
					{
						return -1;
					}
				}
				if (mac1.Length < mac2.Length)
				{
					return -1;
				}
				if (mac1.Length > mac2.Length)
				{
					return 1;
				}
				return 0;
			}

			public int CompareTo(PhysicalAddress other)
			{
				if ((object)other == null)
				{
					return 1;
				}
				return Compare(Buffer, other.Buffer);
			}

			public int CompareTo(object obj)
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				if (obj != null && !(obj is PhysicalAddress))
				{
					throw new ArgumentException("Object must be of type PhysicalAddress.");
				}
				return CompareTo(obj as PhysicalAddress);
			}

			public void Clear()
			{
				for (int i = 0; i < Buffer.Length; i++)
				{
					Buffer[i] = 0;
				}
			}

			public void CopyFrom(byte[] mac)
			{
				Clear();
				int num = Math.Min(Buffer.Length, mac.Length);
				for (int i = 0; i < num; i++)
				{
					Buffer[i] = mac[i];
				}
			}

			public void CopyFrom(System.Collections.Generic.IReadOnlyList<byte> mac)
			{
				Clear();
				int num = Math.Min(Buffer.Length, ((System.Collections.Generic.IReadOnlyCollection<byte>)mac).Count);
				for (int i = 0; i < num; i++)
				{
					Buffer[i] = mac[i];
				}
			}

			public void CopyFrom(IPhysicalAddress mac)
			{
				for (int i = 0; i < Buffer.Length; i++)
				{
					Buffer[i] = ((System.Collections.Generic.IReadOnlyList<byte>)mac)[i];
				}
			}

			public static implicit operator ulong(PhysicalAddress mac)
			{
				ulong num = 0uL;
				for (int i = 0; i < mac.Buffer.Length; i++)
				{
					num <<= 8;
					num += mac.Buffer[i];
				}
				return num;
			}

			public override string ToString()
			{
				string text = "";
				for (int i = 0; i < Buffer.Length; i++)
				{
					if (i > 0)
					{
						text += ":";
					}
					text += Buffer[i].HexString();
				}
				return text;
			}
		}

		public interface IAdapter : IEventSender, IDisposableManager, IDisposable, System.IDisposable
		{
			ITreeNode TreeNode { get; }

			string Name { get; }

			IPhysicalAddress MAC { get; }

			int BackgroundTxMessagesPerSecond { get; set; }

			bool IsConnected { get; }

			TimeSpan TimeSinceAdapterOpened { get; }

			long BytesSent { get; }

			long BytesReceived { get; }

			long MessagesSent { get; }

			long MessagesReceived { get; }

			TimeSpan TimeSinceLastMessageTx { get; }

			TimeSpan TimeSinceLastMessageRx { get; }

			System.Threading.Tasks.Task<bool> OpenAsync(AsyncOperation obj);

			System.Threading.Tasks.Task<bool> CloseAsync(AsyncOperation obj);
		}

		public interface IAdapter<T> : IAdapter, IEventSender, IDisposableManager, IDisposable, System.IDisposable where T : IMessage
		{
			bool Transmit(T message);
		}

		public abstract class Adapter : DisposableManager, IAdapter, IEventSender, IDisposableManager, IDisposable, System.IDisposable
		{
			public enum ICON
			{
				DISCONNECTED = 1,
				CONNECTED = 3
			}

			private class TxSerializer : Disposable
			{
				public readonly Adapter Adapter;

				private readonly object CriticalSection = new object();

				private PeriodicTask TxSerializerTask;

				private int mMessagesPerSecond;

				private RoundRobinPublisher RoundRobin;

				private TransmitTurnEvent NextTurnEvent;

				private int MessagesSent;

				private Timer Timer = new Timer();

				private static readonly TimeSpan DEBUG_TIME = TimeSpan.FromSeconds(1.0);

				public int MessagesPerSecond
				{
					get
					{
						return mMessagesPerSecond;
					}
					set
					{
						value = Math.Max(0, value);
						if (mMessagesPerSecond == value || base.IsDisposed)
						{
							return;
						}
						mMessagesPerSecond = value;
						if (TxSerializerTask != null || value <= 0)
						{
							return;
						}
						lock (CriticalSection)
						{
							if (TxSerializerTask == null)
							{
								TxSerializerTask = new PeriodicTask(BackgroundTransmitTask);
							}
						}
					}
				}

				internal TxSerializer(Adapter adapter)
				{
					Adapter = adapter;
					Adapter.AddDisposable(this);
					RoundRobin = Adapter.Events.CreateRoundRobinPublisher<TransmitTurnEvent>();
					NextTurnEvent = new TransmitTurnEvent(Adapter);
				}

				private TimeSpan BackgroundTransmitTask()
				{
					//IL_003c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0048: Unknown result type (might be due to invalid IL or missing references)
					//IL_004d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0088: Unknown result type (might be due to invalid IL or missing references)
					//IL_008d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0060: Unknown result type (might be due to invalid IL or missing references)
					//IL_0065: Unknown result type (might be due to invalid IL or missing references)
					//IL_006a: Unknown result type (might be due to invalid IL or missing references)
					//IL_0138: Unknown result type (might be due to invalid IL or missing references)
					//IL_0119: Unknown result type (might be due to invalid IL or missing references)
					//IL_0124: Unknown result type (might be due to invalid IL or missing references)
					//IL_0129: Unknown result type (might be due to invalid IL or missing references)
					if (Adapter.IsDisposed || !Adapter.IsConnected || RoundRobin.SubscriberCount <= 0)
					{
						Timer.Reset();
						return TimeSpan.FromMilliseconds(25.0);
					}
					if (Timer.ElapsedTime >= DEBUG_TIME)
					{
						Timer timer = Timer;
						timer.ElapsedTime -= DEBUG_TIME;
						MessagesSent = 0;
					}
					double num = MessagesPerSecond;
					TimeSpan elapsedTime = Timer.ElapsedTime;
					int num2 = (int)(num * ((TimeSpan)(ref elapsedTime)).TotalSeconds);
					int subscriberCount = RoundRobin.SubscriberCount;
					for (int i = 0; i < subscriberCount; i++)
					{
						if (MessagesSent > num2)
						{
							break;
						}
						try
						{
							NextTurnEvent.Handled = false;
							RoundRobin.PublishNext(NextTurnEvent);
							if (NextTurnEvent.Handled)
							{
								MessagesSent++;
							}
						}
						catch (System.Exception)
						{
						}
					}
					if (MessagesSent >= num2)
					{
						return TimeSpan.FromSeconds((1.0 + (double)MessagesSent) / (double)MessagesPerSecond) - Timer.ElapsedTime;
					}
					return TimeSpan.FromMilliseconds(2.0);
				}

				public override void Dispose(bool disposing)
				{
					if (disposing)
					{
						lock (CriticalSection)
						{
							TxSerializerTask?.Dispose();
						}
					}
				}
			}

			protected class MessageRateMetrics
			{
				private class Item
				{
					private long mTotal;

					private int Count;

					public long Total => Interlocked.Read(ref mTotal);

					[field: CompilerGenerated]
					public int PerSecond
					{
						[CompilerGenerated]
						get;
						[CompilerGenerated]
						private set;
					}

					public void Reset()
					{
						int count = (PerSecond = 0);
						mTotal = (Count = count);
					}

					public void Update(int value)
					{
						Interlocked.Add(ref mTotal, (long)value);
						int num = Interlocked.Add(ref Count, value);
						if (PerSecond < num)
						{
							PerSecond = num;
						}
					}

					public void Task1sec()
					{
						PerSecond = Interlocked.Exchange(ref Count, 0);
					}
				}

				[CompilerGenerated]
				private sealed class <>c__DisplayClass7_0
				{
					[StructLayout((LayoutKind)3)]
					private struct <<-ctor>b__0>d : IAsyncStateMachine
					{
						public int <>1__state;

						public AsyncTaskMethodBuilder <>t__builder;

						public <>c__DisplayClass7_0 <>4__this;

						private ConfiguredTaskAwaiter <>u__1;

						private void MoveNext()
						{
							//IL_006d: Unknown result type (might be due to invalid IL or missing references)
							//IL_0072: Unknown result type (might be due to invalid IL or missing references)
							//IL_0079: Unknown result type (might be due to invalid IL or missing references)
							//IL_0103: Unknown result type (might be due to invalid IL or missing references)
							//IL_0108: Unknown result type (might be due to invalid IL or missing references)
							//IL_010f: Unknown result type (might be due to invalid IL or missing references)
							//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
							//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
							//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
							//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
							//IL_0032: Unknown result type (might be due to invalid IL or missing references)
							//IL_0037: Unknown result type (might be due to invalid IL or missing references)
							//IL_003a: Unknown result type (might be due to invalid IL or missing references)
							//IL_003f: Unknown result type (might be due to invalid IL or missing references)
							//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
							//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
							//IL_0053: Unknown result type (might be due to invalid IL or missing references)
							//IL_0054: Unknown result type (might be due to invalid IL or missing references)
							int num = <>1__state;
							<>c__DisplayClass7_0 <>c__DisplayClass7_ = <>4__this;
							try
							{
								ConfiguredTaskAwaiter val;
								if (num != 0)
								{
									if (num != 1)
									{
										goto IL_0125;
									}
									val = <>u__1;
									<>u__1 = default(ConfiguredTaskAwaiter);
									num = (<>1__state = -1);
									goto IL_011e;
								}
								val = <>u__1;
								<>u__1 = default(ConfiguredTaskAwaiter);
								num = (<>1__state = -1);
								goto IL_0088;
								IL_011e:
								((ConfiguredTaskAwaiter)(ref val)).GetResult();
								goto IL_0125;
								IL_0125:
								if (!<>c__DisplayClass7_.owner.IsDisposed)
								{
									ConfiguredTaskAwaitable val2;
									if (<>c__DisplayClass7_.<>4__this.Paused)
									{
										val2 = System.Threading.Tasks.Task.Delay(100).ConfigureAwait(false);
										val = ((ConfiguredTaskAwaitable)(ref val2)).GetAwaiter();
										if (!((ConfiguredTaskAwaiter)(ref val)).IsCompleted)
										{
											num = (<>1__state = 0);
											<>u__1 = val;
											((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <<-ctor>b__0>d>(ref val, ref this);
											return;
										}
										goto IL_0088;
									}
									<>c__DisplayClass7_.<>4__this.Messages.Task1sec();
									<>c__DisplayClass7_.<>4__this.Bytes.Task1sec();
									_ = <>c__DisplayClass7_.owner.Verbose;
									val2 = System.Threading.Tasks.Task.Delay(1000).ConfigureAwait(false);
									val = ((ConfiguredTaskAwaitable)(ref val2)).GetAwaiter();
									if (!((ConfiguredTaskAwaiter)(ref val)).IsCompleted)
									{
										num = (<>1__state = 1);
										<>u__1 = val;
										((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <<-ctor>b__0>d>(ref val, ref this);
										return;
									}
									goto IL_011e;
								}
								goto end_IL_000e;
								IL_0088:
								((ConfiguredTaskAwaiter)(ref val)).GetResult();
								goto IL_0125;
								end_IL_000e:;
							}
							catch (System.Exception exception)
							{
								<>1__state = -2;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
								return;
							}
							<>1__state = -2;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).SetResult();
						}

						[DebuggerHidden]
						private void SetStateMachine(IAsyncStateMachine stateMachine)
						{
							((AsyncTaskMethodBuilder)(ref <>t__builder)).SetStateMachine(stateMachine);
						}
					}

					public MessageRateMetrics <>4__this;

					public Adapter owner;

					[AsyncStateMachine(typeof(<<-ctor>b__0>d))]
					internal System.Threading.Tasks.Task? <.ctor>b__0()
					{
						//IL_0002: Unknown result type (might be due to invalid IL or missing references)
						//IL_0007: Unknown result type (might be due to invalid IL or missing references)
						<<-ctor>b__0>d <<-ctor>b__0>d = default(<<-ctor>b__0>d);
						<<-ctor>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
						<<-ctor>b__0>d.<>4__this = this;
						<<-ctor>b__0>d.<>1__state = -1;
						((AsyncTaskMethodBuilder)(ref <<-ctor>b__0>d.<>t__builder)).Start<<<-ctor>b__0>d>(ref <<-ctor>b__0>d);
						return ((AsyncTaskMethodBuilder)(ref <<-ctor>b__0>d.<>t__builder)).Task;
					}
				}

				private readonly string Text;

				private readonly bool Verbose;

				private readonly Timer mTimeSinceLastMessage = new Timer();

				private readonly Item Messages = new Item();

				private readonly Item Bytes = new Item();

				private bool Paused = true;

				public long TotalMessages => Messages.Total;

				public long TotalBytes => Bytes.Total;

				public int MessagesPerSecond => Messages.PerSecond;

				public int BytesPerSecond => Bytes.PerSecond;

				public TimeSpan TimeSinceLastMessage => mTimeSinceLastMessage.ElapsedTime;

				public MessageRateMetrics(Adapter owner, string text, bool verbose)
				{
					<>c__DisplayClass7_0 <>c__DisplayClass7_ = new <>c__DisplayClass7_0();
					<>c__DisplayClass7_.owner = owner;
					base..ctor();
					<>c__DisplayClass7_.<>4__this = this;
					Text = text;
					Verbose = verbose;
					System.Threading.Tasks.Task.Run((Func<System.Threading.Tasks.Task>)([AsyncStateMachine(typeof(<>c__DisplayClass7_0.<<-ctor>b__0>d))] () =>
					{
						//IL_0002: Unknown result type (might be due to invalid IL or missing references)
						//IL_0007: Unknown result type (might be due to invalid IL or missing references)
						<>c__DisplayClass7_0.<<-ctor>b__0>d <<-ctor>b__0>d = default(<>c__DisplayClass7_0.<<-ctor>b__0>d);
						<<-ctor>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
						<<-ctor>b__0>d.<>4__this = <>c__DisplayClass7_;
						<<-ctor>b__0>d.<>1__state = -1;
						((AsyncTaskMethodBuilder)(ref <<-ctor>b__0>d.<>t__builder)).Start<<>c__DisplayClass7_0.<<-ctor>b__0>d>(ref <<-ctor>b__0>d);
						return ((AsyncTaskMethodBuilder)(ref <<-ctor>b__0>d.<>t__builder)).Task;
					}));
				}

				public void Reset()
				{
					Messages.Reset();
					Bytes.Reset();
				}

				public void Stop()
				{
					Paused = true;
				}

				public void Start()
				{
					Paused = false;
				}

				public void OnMessageReceived(int length)
				{
					mTimeSinceLastMessage.Reset();
					Messages.Update(1);
					Bytes.Update(length);
				}
			}

			[StructLayout((LayoutKind)3)]
			[CompilerGenerated]
			private struct <CloseAsync>d__41 : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncTaskMethodBuilder<bool> <>t__builder;

				public Adapter <>4__this;

				public AsyncOperation obj;

				private TaskAwaiter<bool> <>u__1;

				private void MoveNext()
				{
					//IL_0059: Unknown result type (might be due to invalid IL or missing references)
					//IL_005e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0065: Unknown result type (might be due to invalid IL or missing references)
					//IL_0029: Unknown result type (might be due to invalid IL or missing references)
					//IL_002e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0042: Unknown result type (might be due to invalid IL or missing references)
					//IL_0043: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					Adapter adapter = <>4__this;
					bool result;
					try
					{
						TaskAwaiter<bool> val;
						if (num == 0)
						{
							val = <>u__1;
							<>u__1 = default(TaskAwaiter<bool>);
							num = (<>1__state = -1);
							goto IL_0074;
						}
						if (!adapter.IsDisposed)
						{
							val = adapter.DisconnectAsync(obj).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, <CloseAsync>d__41>(ref val, ref this);
								return;
							}
							goto IL_0074;
						}
						result = true;
						goto end_IL_000e;
						IL_0074:
						result = val.GetResult();
						end_IL_000e:;
					}
					catch (System.Exception exception)
					{
						<>1__state = -2;
						<>t__builder.SetException(exception);
						return;
					}
					<>1__state = -2;
					<>t__builder.SetResult(result);
				}

				[DebuggerHidden]
				private void SetStateMachine(IAsyncStateMachine stateMachine)
				{
					<>t__builder.SetStateMachine(stateMachine);
				}
			}

			[StructLayout((LayoutKind)3)]
			[CompilerGenerated]
			private struct <Dispose>d__56 : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncVoidMethodBuilder <>t__builder;

				public bool disposing;

				public Adapter <>4__this;

				private TaskAwaiter<bool> <>u__1;

				private void MoveNext()
				{
					//IL_0068: Unknown result type (might be due to invalid IL or missing references)
					//IL_006d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0074: Unknown result type (might be due to invalid IL or missing references)
					//IL_0026: Unknown result type (might be due to invalid IL or missing references)
					//IL_0035: Unknown result type (might be due to invalid IL or missing references)
					//IL_003a: Unknown result type (might be due to invalid IL or missing references)
					//IL_004e: Unknown result type (might be due to invalid IL or missing references)
					//IL_004f: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					Adapter adapter = <>4__this;
					try
					{
						TaskAwaiter<bool> val;
						if (num != 0)
						{
							if (!disposing)
							{
								goto IL_00e1;
							}
							val = adapter.CloseAsync(new AsyncOperation(TimeSpan.FromSeconds(5.0))).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								((AsyncVoidMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<bool>, <Dispose>d__56>(ref val, ref this);
								return;
							}
						}
						else
						{
							val = <>u__1;
							<>u__1 = default(TaskAwaiter<bool>);
							num = (<>1__state = -1);
						}
						val.GetResult();
						adapter.IsConnected = false;
						adapter.TransmitSerializer.Dispose();
						object eventLock = adapter.EventLock;
						bool flag = false;
						try
						{
							Monitor.Enter(eventLock, ref flag);
							new AdapterDisposedEvent(adapter).Publish();
							((System.IDisposable)adapter.Events).Dispose();
						}
						finally
						{
							if (num < 0 && flag)
							{
								Monitor.Exit(eventLock);
							}
						}
						adapter.Subscriptions.Dispose();
						goto IL_00e1;
						IL_00e1:
						((DisposableManager)adapter).Dispose(disposing);
					}
					catch (System.Exception exception)
					{
						<>1__state = -2;
						((AsyncVoidMethodBuilder)(ref <>t__builder)).SetException(exception);
						return;
					}
					<>1__state = -2;
					((AsyncVoidMethodBuilder)(ref <>t__builder)).SetResult();
				}

				[DebuggerHidden]
				private void SetStateMachine(IAsyncStateMachine stateMachine)
				{
					((AsyncVoidMethodBuilder)(ref <>t__builder)).SetStateMachine(stateMachine);
				}
			}

			[StructLayout((LayoutKind)3)]
			[CompilerGenerated]
			private struct <OpenAsync>d__40 : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncTaskMethodBuilder<bool> <>t__builder;

				public Adapter <>4__this;

				public AsyncOperation obj;

				private TaskAwaiter<bool> <>u__1;

				private void MoveNext()
				{
					//IL_0059: Unknown result type (might be due to invalid IL or missing references)
					//IL_005e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0065: Unknown result type (might be due to invalid IL or missing references)
					//IL_0029: Unknown result type (might be due to invalid IL or missing references)
					//IL_002e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0042: Unknown result type (might be due to invalid IL or missing references)
					//IL_0043: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					Adapter adapter = <>4__this;
					bool result;
					try
					{
						TaskAwaiter<bool> val;
						if (num == 0)
						{
							val = <>u__1;
							<>u__1 = default(TaskAwaiter<bool>);
							num = (<>1__state = -1);
							goto IL_0074;
						}
						if (!adapter.IsDisposed)
						{
							val = adapter.ConnectAsync(obj).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, <OpenAsync>d__40>(ref val, ref this);
								return;
							}
							goto IL_0074;
						}
						result = false;
						goto end_IL_000e;
						IL_0074:
						result = val.GetResult();
						end_IL_000e:;
					}
					catch (System.Exception exception)
					{
						<>1__state = -2;
						<>t__builder.SetException(exception);
						return;
					}
					<>1__state = -2;
					<>t__builder.SetResult(result);
				}

				[DebuggerHidden]
				private void SetStateMachine(IAsyncStateMachine stateMachine)
				{
					<>t__builder.SetStateMachine(stateMachine);
				}
			}

			public readonly bool Verbose;

			protected readonly object EventLock = new object();

			protected readonly SubscriptionManager Subscriptions = new SubscriptionManager();

			protected readonly MessageRateMetrics RxMetrics;

			protected readonly MessageRateMetrics TxMetrics;

			private readonly TxSerializer TransmitSerializer;

			private readonly AdapterOpenedEvent mAdapterOpenedEvent;

			private readonly AdapterClosedEvent mAdapterClosedEvent;

			private readonly Timer AdapterOpenedTime = new Timer();

			[field: CompilerGenerated]
			public IEventPublisher Events
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				private set;
			}

			[field: CompilerGenerated]
			public string Name
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				private set;
			}

			[field: CompilerGenerated]
			public ITreeNode TreeNode
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				private set;
			}

			[field: CompilerGenerated]
			public bool IsConnected
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				private set;
			}

			public TimeSpan TimeSinceAdapterOpened => AdapterOpenedTime.ElapsedTime;

			public int BackgroundTxMessagesPerSecond
			{
				get
				{
					return TransmitSerializer.MessagesPerSecond;
				}
				set
				{
					TransmitSerializer.MessagesPerSecond = value;
				}
			}

			public abstract IPhysicalAddress MAC { get; }

			public long BytesSent => TxMetrics.TotalBytes;

			public long BytesReceived => RxMetrics.TotalBytes;

			public long MessagesSent => TxMetrics.TotalMessages;

			public long MessagesReceived => RxMetrics.TotalMessages;

			public TimeSpan TimeSinceLastMessageTx => TxMetrics.TimeSinceLastMessage;

			public TimeSpan TimeSinceLastMessageRx => RxMetrics.TimeSinceLastMessage;

			static Adapter()
			{
				ImageCache.RegisterEnumImageReferences(typeof(ICON));
			}

			public Adapter(string name)
				: this(name, verbose: false)
			{
			}

			public Adapter(string name, bool verbose)
			{
				Name = name;
				Verbose = verbose;
				IsConnected = false;
				Events = new EventPublisher("IDS.Core.Communications.Adapter.Events");
				mAdapterOpenedEvent = new AdapterOpenedEvent(this);
				mAdapterClosedEvent = new AdapterClosedEvent(this);
				RxMetrics = new MessageRateMetrics(this, "Rx", verbose);
				TxMetrics = new MessageRateMetrics(this, "Tx", verbose);
				TransmitSerializer = new TxSerializer(this);
				TreeNode = IDS.Core.TreeNode.Create(this);
				AddDisposable(TreeNode);
				TreeNode.Text = Name;
				TreeNode.Icon = ICON.DISCONNECTED;
				TreeNode.Data = this;
			}

			protected abstract System.Threading.Tasks.Task<bool> ConnectAsync(AsyncOperation obj);

			protected abstract System.Threading.Tasks.Task<bool> DisconnectAsync(AsyncOperation obj);

			[AsyncStateMachine(typeof(<OpenAsync>d__40))]
			public async System.Threading.Tasks.Task<bool> OpenAsync(AsyncOperation obj)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				if (base.IsDisposed)
				{
					return false;
				}
				return await ConnectAsync(obj);
			}

			[AsyncStateMachine(typeof(<CloseAsync>d__41))]
			public async System.Threading.Tasks.Task<bool> CloseAsync(AsyncOperation obj)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				if (base.IsDisposed)
				{
					return true;
				}
				return await DisconnectAsync(obj);
			}

			protected void RaiseAdapterOpened()
			{
				if (base.IsDisposed || IsConnected)
				{
					return;
				}
				lock (EventLock)
				{
					if (!base.IsDisposed && !IsConnected)
					{
						IsConnected = true;
						TreeNode.Icon = ICON.CONNECTED;
						AdapterOpenedTime.Reset();
						RxMetrics.Reset();
						RxMetrics.Start();
						TxMetrics.Reset();
						TxMetrics.Start();
						mAdapterOpenedEvent.Publish();
					}
				}
			}

			protected void RaiseAdapterClosed()
			{
				if (base.IsDisposed || !IsConnected)
				{
					return;
				}
				lock (EventLock)
				{
					if (!base.IsDisposed && IsConnected)
					{
						IsConnected = false;
						TreeNode.Icon = ICON.DISCONNECTED;
						RxMetrics.Stop();
						TxMetrics.Stop();
						mAdapterClosedEvent.Publish();
					}
				}
			}

			[AsyncStateMachine(typeof(<Dispose>d__56))]
			public override void Dispose(bool disposing)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				<Dispose>d__56 <Dispose>d__ = default(<Dispose>d__56);
				<Dispose>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
				<Dispose>d__.<>4__this = this;
				<Dispose>d__.disposing = disposing;
				<Dispose>d__.<>1__state = -1;
				((AsyncVoidMethodBuilder)(ref <Dispose>d__.<>t__builder)).Start<<Dispose>d__56>(ref <Dispose>d__);
			}
		}

		public abstract class Adapter<T> : Adapter, IAdapter<T>, IAdapter, IEventSender, IDisposableManager, IDisposable, System.IDisposable where T : IMessageBuffer, new()
		{
			private readonly IMessageEncoder<T> Encoder;

			private readonly MessageDecoder<T> Decoder;

			private readonly AdapterRxEvent<T> AdapterRxEvent;

			private readonly AdapterRxEvent<T> DecodedRxEvent;

			private readonly AdapterTxEvent<T> AdapterTxEvent;

			private readonly MessageRateMetrics LowLevelRxMetrics;

			private readonly MessageRateMetrics LowLevelTxMetrics;

			public long LowLevelBytesSent => LowLevelTxMetrics.TotalBytes;

			public long LowLevelBytesReceived => LowLevelRxMetrics.TotalBytes;

			public long LowLevelMessagesSent => LowLevelTxMetrics.TotalMessages;

			public long LowLevelMessagesReceived => LowLevelRxMetrics.TotalMessages;

			public Adapter(string name)
				: this(name, (IMessageEncoder<T>)null, (MessageDecoder<T>)null, verbose: false)
			{
			}

			public Adapter(string name, IMessageEncoder<T> encoder)
				: this(name, encoder, (MessageDecoder<T>)null, verbose: false)
			{
			}

			public Adapter(string name, MessageDecoder<T> decoder)
				: this(name, (IMessageEncoder<T>)null, decoder, verbose: false)
			{
			}

			public Adapter(string name, bool verbose)
				: this(name, (IMessageEncoder<T>)null, (MessageDecoder<T>)null, verbose)
			{
			}

			public Adapter(string name, IMessageEncoder<T> encoder, bool verbose)
				: this(name, encoder, (MessageDecoder<T>)null, verbose)
			{
			}

			public Adapter(string name, MessageDecoder<T> decoder, bool verbose)
				: this(name, (IMessageEncoder<T>)null, decoder, verbose)
			{
			}

			public Adapter(string name, IMessageEncoder<T> encoder, MessageDecoder<T> decoder)
				: this(name, encoder, decoder, verbose: false)
			{
			}

			public Adapter(string name, IMessageEncoder<T> encoder, MessageDecoder<T> decoder, bool verbose)
				: base(name, verbose)
			{
				AdapterRxEvent = new AdapterRxEvent<T>(this);
				DecodedRxEvent = new AdapterRxEvent<T>(this);
				AdapterTxEvent = new AdapterTxEvent<T>(this);
				Encoder = encoder;
				if (Encoder != null)
				{
					AddDisposable(Encoder);
				}
				Decoder = decoder;
				if (Decoder != null)
				{
					AddDisposable(Decoder);
					Decoder.Action = OnDecodedMessageRx;
				}
				LowLevelTxMetrics = ((Encoder == null) ? TxMetrics : new MessageRateMetrics(this, "EncodedTx", verbose));
				LowLevelRxMetrics = ((Decoder == null) ? RxMetrics : new MessageRateMetrics(this, "EncodedRx", verbose));
				base.Events.Subscribe<AdapterOpenedEvent>(OnAdapterOpened, SubscriptionType.Strong, Subscriptions);
				base.Events.Subscribe<AdapterClosedEvent>(OnAdapterClosed, SubscriptionType.Strong, Subscriptions);
			}

			protected abstract bool TransmitRaw(T message);

			private void OnAdapterOpened(AdapterOpenedEvent message)
			{
				LowLevelRxMetrics.Reset();
				LowLevelRxMetrics.Start();
				LowLevelTxMetrics.Reset();
				LowLevelTxMetrics.Start();
			}

			private void OnAdapterClosed(AdapterClosedEvent message)
			{
				LowLevelRxMetrics.Stop();
				LowLevelTxMetrics.Stop();
			}

			protected void RaiseMessageRx(T rx, bool echo)
			{
				//IL_008d: Unknown result type (might be due to invalid IL or missing references)
				if (base.IsDisposed || !base.IsConnected)
				{
					return;
				}
				lock (EventLock)
				{
					if (!base.IsDisposed && base.IsConnected)
					{
						if (Decoder == null)
						{
							RxMetrics.OnMessageReceived(rx.Length);
							AdapterRxEvent.Publish(rx, echo);
						}
						else
						{
							LowLevelRxMetrics.OnMessageReceived(rx.Length);
							Decoder.DecodeFromStream(rx, echo, rx.Timestamp);
						}
					}
				}
			}

			private void OnDecodedMessageRx(T decoded, bool echo)
			{
				RxMetrics.OnMessageReceived(decoded.Length);
				DecodedRxEvent.Publish(decoded, echo);
			}

			public virtual bool Transmit(T message)
			{
				if (!base.IsConnected)
				{
					return false;
				}
				bool flag = false;
				if (Encoder != null)
				{
					T val = Encoder.Encode(message);
					try
					{
						flag = TransmitRaw(val);
						if (flag)
						{
							LowLevelTxMetrics.OnMessageReceived(val.Length);
						}
					}
					finally
					{
						if (val is ResourcePool.IObject { IsMemberOfPool: not false } obj)
						{
							obj.ReturnToPool();
						}
					}
				}
				else
				{
					flag = TransmitRaw(message);
				}
				if (flag && message != null)
				{
					TxMetrics.OnMessageReceived(message.Length);
					AdapterTxEvent.Publish(message);
				}
				return flag;
			}
		}

		public static class COBS
		{
			public class Decoder<T> : MessageDecoder<T> where T : MessageBuffer, new()
			{
				[CompilerGenerated]
				private sealed class <>c__DisplayClass6_0
				{
					[StructLayout((LayoutKind)3)]
					private struct <<Dispose>b__0>d : IAsyncStateMachine
					{
						public int <>1__state;

						public AsyncTaskMethodBuilder <>t__builder;

						public <>c__DisplayClass6_0 <>4__this;

						private ConfiguredTaskAwaiter <>u__1;

						private void MoveNext()
						{
							//IL_0054: Unknown result type (might be due to invalid IL or missing references)
							//IL_0059: Unknown result type (might be due to invalid IL or missing references)
							//IL_0060: Unknown result type (might be due to invalid IL or missing references)
							//IL_001c: Unknown result type (might be due to invalid IL or missing references)
							//IL_0021: Unknown result type (might be due to invalid IL or missing references)
							//IL_0024: Unknown result type (might be due to invalid IL or missing references)
							//IL_0029: Unknown result type (might be due to invalid IL or missing references)
							//IL_003d: Unknown result type (might be due to invalid IL or missing references)
							//IL_003e: Unknown result type (might be due to invalid IL or missing references)
							int num = <>1__state;
							<>c__DisplayClass6_0 <>c__DisplayClass6_ = <>4__this;
							try
							{
								ConfiguredTaskAwaiter val2;
								if (num != 0)
								{
									ConfiguredTaskAwaitable val = System.Threading.Tasks.Task.Delay(1000).ConfigureAwait(false);
									val2 = ((ConfiguredTaskAwaitable)(ref val)).GetAwaiter();
									if (!((ConfiguredTaskAwaiter)(ref val2)).IsCompleted)
									{
										num = (<>1__state = 0);
										<>u__1 = val2;
										((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <<Dispose>b__0>d>(ref val2, ref this);
										return;
									}
								}
								else
								{
									val2 = <>u__1;
									<>u__1 = default(ConfiguredTaskAwaiter);
									num = (<>1__state = -1);
								}
								((ConfiguredTaskAwaiter)(ref val2)).GetResult();
								<>c__DisplayClass6_.msg?.ReturnToPool();
							}
							catch (System.Exception exception)
							{
								<>1__state = -2;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
								return;
							}
							<>1__state = -2;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).SetResult();
						}

						[DebuggerHidden]
						private void SetStateMachine(IAsyncStateMachine stateMachine)
						{
							((AsyncTaskMethodBuilder)(ref <>t__builder)).SetStateMachine(stateMachine);
						}
					}

					public T msg;

					[AsyncStateMachine(typeof(Decoder<>.<>c__DisplayClass6_0.<<Dispose>b__0>d))]
					internal System.Threading.Tasks.Task? <Dispose>b__0()
					{
						//IL_0002: Unknown result type (might be due to invalid IL or missing references)
						//IL_0007: Unknown result type (might be due to invalid IL or missing references)
						<<Dispose>b__0>d <<Dispose>b__0>d = default(<<Dispose>b__0>d);
						<<Dispose>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
						<<Dispose>b__0>d.<>4__this = this;
						<<Dispose>b__0>d.<>1__state = -1;
						((AsyncTaskMethodBuilder)(ref <<Dispose>b__0>d.<>t__builder)).Start<<<Dispose>b__0>d>(ref <<Dispose>b__0>d);
						return ((AsyncTaskMethodBuilder)(ref <<Dispose>b__0>d.<>t__builder)).Task;
					}
				}

				private T Message = ResourcePool<T>.GetObject();

				private int CodeByte;

				public override void Reset()
				{
					Message?.Clear();
					CodeByte = 0;
				}

				public override void DecodeFromStream(System.Collections.Generic.IReadOnlyCollection<byte> stream, bool echo, TimeSpan timestamp)
				{
					//IL_0026: Unknown result type (might be due to invalid IL or missing references)
					System.Collections.Generic.IEnumerator<byte> enumerator = ((System.Collections.Generic.IEnumerable<byte>)stream).GetEnumerator();
					try
					{
						while (((System.Collections.IEnumerator)enumerator).MoveNext())
						{
							byte current = enumerator.Current;
							T val = DecodeByte(current);
							if (val == null)
							{
								continue;
							}
							val.Timestamp = timestamp;
							try
							{
								base.Action?.Invoke(val, echo);
							}
							finally
							{
								if (val.IsMemberOfPool)
								{
									val.ReturnToPool();
								}
							}
						}
					}
					finally
					{
						((System.IDisposable)enumerator)?.Dispose();
					}
				}

				private T DecodeByte(byte b)
				{
					if (base.IsDisposed)
					{
						return null;
					}
					if (b == 0)
					{
						int codeByte = CodeByte;
						CodeByte = 0;
						if (codeByte == 0 && Message.Length > 1)
						{
							byte b2 = Message[Message.Length - 1];
							ref T message = ref Message;
							int length = message.Length;
							message.Length = length - 1;
							if (CRC8.Calculate((System.Collections.Generic.IReadOnlyList<byte>)Message) == b2)
							{
								T val = Interlocked.Exchange<T>(ref Message, ResourcePool<T>.GetObject());
								if (val == null)
								{
									Message.ReturnToPool();
								}
								return val;
							}
						}
						Message.Clear();
						return null;
					}
					if (CodeByte <= 0)
					{
						CodeByte = b & 0xFF;
					}
					else
					{
						CodeByte--;
						Message?.Append(b);
					}
					if ((CodeByte & 0x3F) == 0)
					{
						while (CodeByte > 0)
						{
							Message?.Append((byte)0);
							CodeByte -= 64;
						}
					}
					return null;
				}

				public override void Dispose(bool disposing)
				{
					if (disposing)
					{
						<>c__DisplayClass6_0 <>c__DisplayClass6_ = new <>c__DisplayClass6_0();
						<>c__DisplayClass6_.msg = Interlocked.Exchange<T>(ref Message, (T)null);
						System.Threading.Tasks.Task.Run((Func<System.Threading.Tasks.Task>)([AsyncStateMachine(typeof(Decoder<>.<>c__DisplayClass6_0.<<Dispose>b__0>d))] () =>
						{
							//IL_0002: Unknown result type (might be due to invalid IL or missing references)
							//IL_0007: Unknown result type (might be due to invalid IL or missing references)
							<>c__DisplayClass6_0.<<Dispose>b__0>d <<Dispose>b__0>d = default(<>c__DisplayClass6_0.<<Dispose>b__0>d);
							<<Dispose>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
							<<Dispose>b__0>d.<>4__this = <>c__DisplayClass6_;
							<<Dispose>b__0>d.<>1__state = -1;
							((AsyncTaskMethodBuilder)(ref <<Dispose>b__0>d.<>t__builder)).Start<<>c__DisplayClass6_0.<<Dispose>b__0>d>(ref <<Dispose>b__0>d);
							return ((AsyncTaskMethodBuilder)(ref <<Dispose>b__0>d.<>t__builder)).Task;
						}));
					}
				}
			}

			public class Encoder<T> : Disposable, IMessageEncoder<T>, IDisposable, System.IDisposable where T : MessageBuffer, new()
			{
				public T Encode(T src)
				{
					if (base.IsDisposed)
					{
						return null;
					}
					T val = ResourcePool<T>.GetObject();
					val.Append((byte)0);
					if (src == null || src.Length <= 0)
					{
						return val;
					}
					byte b = CRC8.Calculate((System.Collections.Generic.IReadOnlyList<byte>)src);
					int num = 0;
					do
					{
						int length = val.Length;
						int num2 = 0;
						val.Append((byte)0);
						do
						{
							byte b2 = ((num < src.Length) ? src[num] : b);
							if (b2 == 0)
							{
								break;
							}
							num++;
							val.Append(b2);
						}
						while (++num2 < 63 && num <= src.Length);
						while (num <= src.Length && ((num < src.Length) ? src[num] : b) == 0)
						{
							num++;
							num2 += 64;
							if (num2 >= 192)
							{
								break;
							}
						}
						val[length] = (byte)num2;
					}
					while (num <= src.Length);
					val.Append((byte)0);
					return val;
				}

				public override void Dispose(bool disposing)
				{
				}
			}

			private const byte FRAME_CHARACTER = 0;

			private const int DATA_BIT_COUNT = 6;

			private const int FRAME_BYTE_COUNT_LSB = 64;

			private const int MAX_DATA_BYTES = 63;
		}

		public class AdapterOpenedEvent : Event
		{
			public readonly IAdapter Adapter;

			public AdapterOpenedEvent(IAdapter adapter)
				: base(adapter)
			{
				Adapter = adapter;
			}
		}

		public class AdapterClosedEvent : Event
		{
			public readonly IAdapter Adapter;

			public AdapterClosedEvent(IAdapter adapter)
				: base(adapter)
			{
				Adapter = adapter;
			}
		}

		public class AdapterDisposedEvent : Event
		{
			public readonly IAdapter Adapter;

			public AdapterDisposedEvent(IAdapter adapter)
				: base(adapter)
			{
				Adapter = adapter;
			}
		}

		[DefaultMember("Item")]
		public class AdapterRxEvent<T> : Event, IMessage, IByteList, System.Collections.Generic.IReadOnlyList<byte>, System.Collections.Generic.IEnumerable<byte>, System.Collections.IEnumerable, System.Collections.Generic.IReadOnlyCollection<byte>, ITimeStamp where T : IMessage, new()
		{
			public readonly IAdapter<T> Adapter;

			public T Message
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				private set;
			}

			public bool Echo
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				private set;
			}

			public int Length => Message.Length;

			public int Count => ((System.Collections.Generic.IReadOnlyCollection<byte>)Message).Count;

			public TimeSpan Timestamp => Message.Timestamp;

			public byte this[int index] => ((System.Collections.Generic.IReadOnlyList<byte>)Message)[index];

			public AdapterRxEvent(IAdapter<T> adapter)
				: base(adapter)
			{
				Adapter = adapter;
			}

			public void Publish(T message, bool echo)
			{
				Message = message;
				Echo = echo;
				Publish();
			}

			public void CopyTo(byte[] array, int index)
			{
				Message.CopyTo(array, index);
			}

			public void CopyRangeTo(int sourceIndex, int count, byte[] array, int destIndex)
			{
				Message.CopyRangeTo(sourceIndex, count, array, destIndex);
			}

			public string ToString(bool dataonly)
			{
				return ((object)Message/*cast due to .constrained prefix*/).ToString();
			}

			public System.Collections.Generic.IEnumerator<byte> GetEnumerator()
			{
				return ((System.Collections.Generic.IEnumerable<byte>)Message/*cast due to .constrained prefix*/).GetEnumerator();
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return (System.Collections.IEnumerator)((System.Collections.Generic.IEnumerable<byte>)Message/*cast due to .constrained prefix*/).GetEnumerator();
			}
		}

		[DefaultMember("Item")]
		public class AdapterTxEvent<T> : Event, IMessage, IByteList, System.Collections.Generic.IReadOnlyList<byte>, System.Collections.Generic.IEnumerable<byte>, System.Collections.IEnumerable, System.Collections.Generic.IReadOnlyCollection<byte>, ITimeStamp where T : IMessage, new()
		{
			public readonly IAdapter<T> Adapter;

			public T Message
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				private set;
			}

			public int Length => Message.Length;

			public int Count => ((System.Collections.Generic.IReadOnlyCollection<byte>)Message).Count;

			public TimeSpan Timestamp => Message.Timestamp;

			public byte this[int index] => ((System.Collections.Generic.IReadOnlyList<byte>)Message)[index];

			public AdapterTxEvent(IAdapter<T> adapter)
				: base(adapter)
			{
				Adapter = adapter;
			}

			public void Publish(T message)
			{
				Message = message;
				Publish();
			}

			public void CopyTo(byte[] array, int index)
			{
				Message.CopyTo(array, index);
			}

			public void CopyRangeTo(int sourceIndex, int count, byte[] array, int destIndex)
			{
				Message.CopyRangeTo(sourceIndex, count, array, destIndex);
			}

			public string ToString(bool dataonly)
			{
				return ((object)Message/*cast due to .constrained prefix*/).ToString();
			}

			public System.Collections.Generic.IEnumerator<byte> GetEnumerator()
			{
				return ((System.Collections.Generic.IEnumerable<byte>)Message/*cast due to .constrained prefix*/).GetEnumerator();
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return (System.Collections.IEnumerator)((System.Collections.Generic.IEnumerable<byte>)Message/*cast due to .constrained prefix*/).GetEnumerator();
			}
		}

		public class TransmitTurnEvent : Event
		{
			public readonly IAdapter Adapter;

			public bool Handled;

			public TransmitTurnEvent(IAdapter adapter)
				: base(adapter)
			{
				Adapter = adapter;
			}
		}

		public interface ITimeStamp
		{
			TimeSpan Timestamp { get; }
		}

		public interface IByteList : System.Collections.Generic.IReadOnlyList<byte>, System.Collections.Generic.IEnumerable<byte>, System.Collections.IEnumerable, System.Collections.Generic.IReadOnlyCollection<byte>
		{
			int Length { get; }

			void CopyTo(byte[] array, int index);

			void CopyRangeTo(int sourceIndex, int count, byte[] array, int destIndex);

			string ToString(bool dataonly);
		}

		[DefaultMember("Item")]
		public interface IByteBuffer : IByteList, System.Collections.Generic.IReadOnlyList<byte>, System.Collections.Generic.IEnumerable<byte>, System.Collections.IEnumerable, System.Collections.Generic.IReadOnlyCollection<byte>
		{
			new int Length { get; set; }

			byte this[int index] { get; set; }

			int Capacity { get; set; }

			void Clear();

			void Append(byte value);

			void Append(sbyte value);

			void Append(char value);

			void Append(short value);

			void Append(ushort value);

			void Append(Int24 value);

			void Append(UInt24 value);

			void Append(int value);

			void Append(uint value);

			void Append(Int40 value);

			void Append(UInt40 value);

			void Append(Int48 value);

			void Append(UInt48 value);

			void Append(Int56 value);

			void Append(UInt56 value);

			void Append(long value);

			void Append(ulong value);

			void Append(IByteList buffer);

			void Append(IByteList buffer, int count);

			void Append(IByteList buffer, int index, int count);

			void Append(byte[] buffer);

			void Append(byte[] buffer, int count);

			void Append(byte[] buffer, int index, int count);
		}

		public interface IMessage : IByteList, System.Collections.Generic.IReadOnlyList<byte>, System.Collections.Generic.IEnumerable<byte>, System.Collections.IEnumerable, System.Collections.Generic.IReadOnlyCollection<byte>, ITimeStamp
		{
		}

		public interface IMessageBuffer : IMessage, IByteList, System.Collections.Generic.IReadOnlyList<byte>, System.Collections.Generic.IEnumerable<byte>, System.Collections.IEnumerable, System.Collections.Generic.IReadOnlyCollection<byte>, ITimeStamp, IByteBuffer
		{
			new TimeSpan Timestamp { get; set; }
		}

		[DefaultMember("Item")]
		public class MessageBuffer : ResourcePool.Object, IMessageBuffer, IMessage, IByteList, System.Collections.Generic.IReadOnlyList<byte>, System.Collections.Generic.IEnumerable<byte>, System.Collections.IEnumerable, System.Collections.Generic.IReadOnlyCollection<byte>, ITimeStamp, IByteBuffer
		{
			[CompilerGenerated]
			private sealed class <GetEnumerator>d__22 : System.Collections.Generic.IEnumerator<byte>, System.Collections.IEnumerator, System.IDisposable
			{
				private int <>1__state;

				private byte <>2__current;

				public MessageBuffer <>4__this;

				private int <i>5__2;

				byte System.Collections.Generic.IEnumerator<byte>.Current
				{
					[DebuggerHidden]
					get
					{
						return <>2__current;
					}
				}

				object System.Collections.IEnumerator.Current
				{
					[DebuggerHidden]
					get
					{
						return <>2__current;
					}
				}

				[DebuggerHidden]
				public <GetEnumerator>d__22(int <>1__state)
				{
					this.<>1__state = <>1__state;
				}

				[DebuggerHidden]
				void System.IDisposable.Dispose()
				{
				}

				private bool MoveNext()
				{
					int num = <>1__state;
					MessageBuffer messageBuffer = <>4__this;
					switch (num)
					{
					default:
						return false;
					case 0:
						<>1__state = -1;
						<i>5__2 = 0;
						break;
					case 1:
						<>1__state = -1;
						<i>5__2++;
						break;
					}
					if (<i>5__2 < messageBuffer.Length)
					{
						<>2__current = messageBuffer.Data[<i>5__2];
						<>1__state = 1;
						return true;
					}
					return false;
				}

				bool System.Collections.IEnumerator.MoveNext()
				{
					//ILSpy generated this explicit interface implementation from .override directive in MoveNext
					return this.MoveNext();
				}

				[DebuggerHidden]
				void System.Collections.IEnumerator.Reset()
				{
					//IL_0000: Unknown result type (might be due to invalid IL or missing references)
					throw new NotSupportedException();
				}
			}

			private const int DEFAULT_CAPACITY = 64;

			private static readonly double StartTicks_sec = Timer.RawTicks_sec;

			[field: CompilerGenerated]
			public int Length
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				set;
			}

			public int Count => Length;

			[field: CompilerGenerated]
			public TimeSpan Timestamp
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				set;
			} = TimeSpan.Zero;

			[field: CompilerGenerated]
			public byte[] Data
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				private set;
			}

			public virtual int Capacity
			{
				get
				{
					return Data.Length;
				}
				set
				{
					if (value > Capacity)
					{
						byte[] array = new byte[value];
						if (Length > 0)
						{
							System.Array.Copy((System.Array)Data, (System.Array)array, Length);
						}
						Data = array;
					}
				}
			}

			public byte this[int index]
			{
				get
				{
					return Data[index];
				}
				set
				{
					Data[index] = value;
				}
			}

			public void SetTimeStamp()
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				Timestamp = TimeSpan.FromSeconds(Timer.RawTicks_sec - StartTicks_sec);
			}

			public MessageBuffer()
				: this(64)
			{
			}

			public MessageBuffer(int capacity)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				if (capacity < 1)
				{
					throw new ArgumentException("MessageBuffer.Capacity must be at least 1 byte");
				}
				Data = new byte[capacity];
			}

			[IteratorStateMachine(typeof(<GetEnumerator>d__22))]
			public System.Collections.Generic.IEnumerator<byte> GetEnumerator()
			{
				for (int i = 0; i < Length; i++)
				{
					yield return Data[i];
				}
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return (System.Collections.IEnumerator)GetEnumerator();
			}

			public virtual void Clear()
			{
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				Length = 0;
				Timestamp = TimeSpan.Zero;
			}

			protected override void ResetPoolObjectState()
			{
				Clear();
			}

			public void CopyTo(byte[] array, int index)
			{
				System.Array.Copy((System.Array)Data, 0, (System.Array)array, index, Length);
			}

			public void CopyRangeTo(int sourceIndex, int count, byte[] array, int destIndex)
			{
				System.Array.Copy((System.Array)Data, sourceIndex, (System.Array)array, destIndex, count);
			}

			public virtual void CopyFrom(IMessage src)
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				Length = 0;
				Append(src);
				Timestamp = src.Timestamp;
			}

			public virtual void CopyFrom(byte[] buffer)
			{
				Length = 0;
				Append(buffer);
			}

			public virtual void CopyFrom(byte[] buffer, int count)
			{
				Length = 0;
				Append(buffer, count);
			}

			public void Append(byte value)
			{
				EnsureCapacity(Length + 1);
				Data[Length++] = value;
			}

			public void Append(sbyte value)
			{
				Append((byte)value);
			}

			public void Append(char value)
			{
				Append((byte)value);
			}

			public void Append(short value)
			{
				Append((byte)(value >> 8));
				Append((byte)value);
			}

			public void Append(ushort value)
			{
				Append((byte)(value >> 8));
				Append((byte)value);
			}

			public void Append(Int24 value)
			{
				Append((byte)(value >> 16));
				Append((ushort)value);
			}

			public void Append(UInt24 value)
			{
				Append((byte)(value >> 16));
				Append((ushort)value);
			}

			public void Append(int value)
			{
				Append((ushort)(value >> 16));
				Append((ushort)value);
			}

			public void Append(uint value)
			{
				Append((ushort)(value >> 16));
				Append((ushort)value);
			}

			public void Append(Int40 value)
			{
				Append((uint)(value >> 8));
				Append((byte)value);
			}

			public void Append(UInt40 value)
			{
				Append((uint)(value >> 8));
				Append((byte)value);
			}

			public void Append(Int48 value)
			{
				Append((uint)(value >> 16));
				Append((ushort)value);
			}

			public void Append(UInt48 value)
			{
				Append((uint)(value >> 16));
				Append((ushort)value);
			}

			public void Append(Int56 value)
			{
				Append((uint)(value >> 24));
				Append((ushort)(value >> 8));
				Append((byte)value);
			}

			public void Append(UInt56 value)
			{
				Append((uint)(value >> 24));
				Append((ushort)(value >> 8));
				Append((byte)value);
			}

			public void Append(long value)
			{
				Append((uint)(value >> 32));
				Append((uint)value);
			}

			public void Append(ulong value)
			{
				Append((uint)(value >> 32));
				Append((uint)value);
			}

			public void Append(IByteList buffer)
			{
				Append(buffer, ((System.Collections.Generic.IReadOnlyCollection<byte>)buffer).Count);
			}

			public void Append(IByteList buffer, int count)
			{
				Append(buffer, 0, count);
			}

			public void Append(IByteList buffer, int index, int count)
			{
				EnsureCapacity(Length + count);
				buffer.CopyRangeTo(index, count, Data, Length);
				Length += count;
			}

			public void Append(byte[] buffer)
			{
				Append(buffer, buffer.Length);
			}

			public void Append(byte[] buffer, int count)
			{
				Append(buffer, 0, count);
			}

			public void Append(byte[] buffer, int index, int count)
			{
				EnsureCapacity(Length + count);
				System.Array.Copy((System.Array)buffer, index, (System.Array)Data, Length, count);
				Length += count;
			}

			private void EnsureCapacity(int min)
			{
				if (Capacity < min)
				{
					Capacity = Math.Max(min, Capacity * 2);
				}
			}

			public virtual string ToString()
			{
				return Comm.ToString((System.Collections.Generic.IReadOnlyList<byte>)this);
			}

			public virtual string ToString(bool dataonly)
			{
				return Comm.ToString((System.Collections.Generic.IReadOnlyList<byte>)this, dataonly);
			}
		}

		public abstract class MessageDecoder<T> : Disposable where T : IMessage
		{
			public Action<T, bool> Action
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				set;
			}

			public abstract void Reset();

			public abstract void DecodeFromStream(System.Collections.Generic.IReadOnlyCollection<byte> stream, bool echo, TimeSpan timestamp);
		}

		public interface IMessageEncoder<T> : IDisposable, System.IDisposable where T : IMessage
		{
			T Encode(T src);
		}

		public class SocketClient<T> : Adapter<T> where T : MessageBuffer, new()
		{
			private class NetworkInterfaceCardAddress : PhysicalAddress
			{
				public NetworkInterfaceCardAddress()
					: base(6)
				{
					SetRandomMACValue();
				}
			}

			private class ConnectionManager : Disposable
			{
				[CompilerGenerated]
				private sealed class <>c__DisplayClass12_0
				{
					public ConnectionManager <>4__this;

					public CancellationTokenSource cts;

					public Func<System.Threading.Tasks.Task?> <>9__0;

					public Func<System.Threading.Tasks.Task?> <>9__1;

					internal System.Threading.Tasks.Task? <HealthTask>b__0()
					{
						//IL_000c: Unknown result type (might be due to invalid IL or missing references)
						return <>4__this.RxBackgroundTask(cts.Token);
					}

					internal System.Threading.Tasks.Task? <HealthTask>b__1()
					{
						//IL_000c: Unknown result type (might be due to invalid IL or missing references)
						return <>4__this.TxBackgroundTask(cts.Token);
					}
				}

				[StructLayout((LayoutKind)3)]
				[CompilerGenerated]
				private struct <HealthTask>d__12 : IAsyncStateMachine
				{
					public int <>1__state;

					public AsyncTaskMethodBuilder <>t__builder;

					public ConnectionManager <>4__this;

					private <>c__DisplayClass12_0 <>8__1;

					private System.Threading.Tasks.Task <rxTask>5__2;

					private System.Threading.Tasks.Task <txTask>5__3;

					private object <>7__wrap3;

					private int <>7__wrap4;

					private TaskAwaiter <>u__1;

					private ConfiguredTaskAwaiter <>u__2;

					private void MoveNext()
					{
						//IL_0182: Unknown result type (might be due to invalid IL or missing references)
						//IL_018c: Unknown result type (might be due to invalid IL or missing references)
						//IL_0191: Unknown result type (might be due to invalid IL or missing references)
						//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
						//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
						//IL_047f: Unknown result type (might be due to invalid IL or missing references)
						//IL_0484: Unknown result type (might be due to invalid IL or missing references)
						//IL_048c: Unknown result type (might be due to invalid IL or missing references)
						//IL_0441: Unknown result type (might be due to invalid IL or missing references)
						//IL_0446: Unknown result type (might be due to invalid IL or missing references)
						//IL_044a: Unknown result type (might be due to invalid IL or missing references)
						//IL_044f: Unknown result type (might be due to invalid IL or missing references)
						//IL_050a: Unknown result type (might be due to invalid IL or missing references)
						//IL_050f: Unknown result type (might be due to invalid IL or missing references)
						//IL_0517: Unknown result type (might be due to invalid IL or missing references)
						//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
						//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
						//IL_04d5: Unknown result type (might be due to invalid IL or missing references)
						//IL_04da: Unknown result type (might be due to invalid IL or missing references)
						//IL_012a: Unknown result type (might be due to invalid IL or missing references)
						//IL_012f: Unknown result type (might be due to invalid IL or missing references)
						//IL_0136: Unknown result type (might be due to invalid IL or missing references)
						//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
						//IL_00d6: Expected O, but got Unknown
						//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
						//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
						//IL_020a: Unknown result type (might be due to invalid IL or missing references)
						//IL_0214: Expected O, but got Unknown
						//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
						//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
						//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
						//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
						//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
						//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
						//IL_0464: Unknown result type (might be due to invalid IL or missing references)
						//IL_0466: Unknown result type (might be due to invalid IL or missing references)
						//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
						//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
						//IL_0110: Unknown result type (might be due to invalid IL or missing references)
						//IL_0111: Unknown result type (might be due to invalid IL or missing references)
						//IL_024e: Unknown result type (might be due to invalid IL or missing references)
						//IL_0297: Unknown result type (might be due to invalid IL or missing references)
						//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
						//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
						//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
						//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
						//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
						int num = <>1__state;
						ConnectionManager connectionManager = <>4__this;
						try
						{
							ConfiguredTaskAwaitable val2;
							ConfiguredTaskAwaiter val3;
							switch (num)
							{
							default:
								<>8__1 = new <>c__DisplayClass12_0();
								<>8__1.<>4__this = <>4__this;
								<rxTask>5__2 = null;
								<txTask>5__3 = null;
								<>8__1.cts = null;
								<>7__wrap3 = null;
								<>7__wrap4 = 0;
								goto case 0;
							case 0:
							case 1:
							case 2:
								try
								{
									_ = 2;
									try
									{
										TaskAwaiter val;
										switch (num)
										{
										case 0:
											try
											{
												if (num != 0)
												{
													connectionManager.tcpClient = new TcpClient();
													val = connectionManager.tcpClient.ConnectAsync(connectionManager.Adapter.Address, connectionManager.Adapter.Port).GetAwaiter();
													if (!((TaskAwaiter)(ref val)).IsCompleted)
													{
														num = (<>1__state = 0);
														<>u__1 = val;
														((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <HealthTask>d__12>(ref val, ref this);
														return;
													}
												}
												else
												{
													val = <>u__1;
													<>u__1 = default(TaskAwaiter);
													num = (<>1__state = -1);
												}
												((TaskAwaiter)(ref val)).GetResult();
												if (connectionManager.IsDisposed)
												{
													break;
												}
												connectionManager.ClearTxQueue();
												connectionManager.IsConnected = true;
												goto IL_0172;
											}
											catch (System.Exception)
											{
												_ = connectionManager.Verbose;
												goto IL_0172;
											}
										case 1:
											val = <>u__1;
											<>u__1 = default(TaskAwaiter);
											num = (<>1__state = -1);
											goto IL_01da;
										case 2:
											val = <>u__1;
											<>u__1 = default(TaskAwaiter);
											num = (<>1__state = -1);
											goto IL_0308;
										default:
											{
												if (!connectionManager.IsDisposed)
												{
													connectionManager.IsConnected = false;
													goto IL_01e1;
												}
												goto end_IL_0072;
											}
											IL_0172:
											if (!connectionManager.IsConnected)
											{
												val = System.Threading.Tasks.Task.Delay(10, connectionManager.masterCTS.Token).GetAwaiter();
												if (!((TaskAwaiter)(ref val)).IsCompleted)
												{
													num = (<>1__state = 1);
													<>u__1 = val;
													((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <HealthTask>d__12>(ref val, ref this);
													return;
												}
												goto IL_01da;
											}
											goto IL_01e1;
											IL_0308:
											((TaskAwaiter)(ref val)).GetResult();
											if (connectionManager.IsDisposed)
											{
												break;
											}
											goto IL_031c;
											IL_01da:
											((TaskAwaiter)(ref val)).GetResult();
											goto IL_01e1;
											IL_01e1:
											if (!connectionManager.IsConnected)
											{
												if (connectionManager.IsDisposed)
												{
													break;
												}
												try
												{
													TcpClient tcpClient = connectionManager.tcpClient;
													if (tcpClient != null)
													{
														tcpClient.Dispose();
													}
												}
												catch (System.Exception)
												{
													_ = connectionManager.Verbose;
												}
												connectionManager.tcpClient = null;
												goto case 0;
											}
											if (connectionManager.IsDisposed)
											{
												break;
											}
											connectionManager.Adapter.RaiseAdapterOpened();
											<>8__1.cts = new CancellationTokenSource();
											<rxTask>5__2 = System.Threading.Tasks.Task.Run((Func<System.Threading.Tasks.Task>)(() => <>8__1.<>4__this.RxBackgroundTask(<>8__1.cts.Token)), <>8__1.cts.Token);
											<txTask>5__3 = System.Threading.Tasks.Task.Run((Func<System.Threading.Tasks.Task>)(() => <>8__1.<>4__this.TxBackgroundTask(<>8__1.cts.Token)), <>8__1.cts.Token);
											goto IL_031c;
											IL_031c:
											if (connectionManager.IsConnected)
											{
												val = System.Threading.Tasks.Task.Delay(10, connectionManager.masterCTS.Token).GetAwaiter();
												if (!((TaskAwaiter)(ref val)).IsCompleted)
												{
													num = (<>1__state = 2);
													<>u__1 = val;
													((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <HealthTask>d__12>(ref val, ref this);
													return;
												}
												goto IL_0308;
											}
											_ = connectionManager.Verbose;
											<>8__1.cts.Cancel();
											try
											{
												System.Threading.Tasks.Task.WaitAll(new System.Threading.Tasks.Task[2] { <rxTask>5__2, <txTask>5__3 });
											}
											catch (System.Exception)
											{
												_ = connectionManager.Verbose;
											}
											<>8__1.cts.Dispose();
											<>8__1.cts = null;
											<rxTask>5__2 = null;
											<txTask>5__3 = null;
											goto default;
										}
										goto IL_03a7;
										end_IL_0072:;
									}
									catch (System.Exception)
									{
										_ = connectionManager.Verbose;
									}
									goto end_IL_006d;
									IL_03a7:
									<>7__wrap4 = 1;
									end_IL_006d:;
								}
								catch (object obj)
								{
									<>7__wrap3 = obj;
								}
								connectionManager.IsConnected = false;
								_ = connectionManager.Verbose;
								if (connectionManager.tcpClient != null)
								{
									try
									{
										connectionManager.tcpClient.Dispose();
									}
									catch (System.Exception)
									{
										_ = connectionManager.Verbose;
									}
									connectionManager.tcpClient = null;
								}
								if (<>8__1.cts == null)
								{
									break;
								}
								try
								{
									_ = connectionManager.Verbose;
									<>8__1.cts.Cancel();
								}
								catch (System.Exception)
								{
									_ = connectionManager.Verbose;
								}
								if (<rxTask>5__2 != null)
								{
									goto case 3;
								}
								goto IL_04ae;
							case 3:
								try
								{
									if (num != 3)
									{
										_ = connectionManager.Verbose;
										val2 = <rxTask>5__2.ConfigureAwait(false);
										val3 = ((ConfiguredTaskAwaitable)(ref val2)).GetAwaiter();
										if (!((ConfiguredTaskAwaiter)(ref val3)).IsCompleted)
										{
											num = (<>1__state = 3);
											<>u__2 = val3;
											((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <HealthTask>d__12>(ref val3, ref this);
											return;
										}
									}
									else
									{
										val3 = <>u__2;
										<>u__2 = default(ConfiguredTaskAwaiter);
										num = (<>1__state = -1);
									}
									((ConfiguredTaskAwaiter)(ref val3)).GetResult();
								}
								catch (System.Exception)
								{
									_ = connectionManager.Verbose;
								}
								goto IL_04ae;
							case 4:
								{
									try
									{
										if (num != 4)
										{
											_ = connectionManager.Verbose;
											val2 = <txTask>5__3.ConfigureAwait(false);
											val3 = ((ConfiguredTaskAwaitable)(ref val2)).GetAwaiter();
											if (!((ConfiguredTaskAwaiter)(ref val3)).IsCompleted)
											{
												num = (<>1__state = 4);
												<>u__2 = val3;
												((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <HealthTask>d__12>(ref val3, ref this);
												return;
											}
										}
										else
										{
											val3 = <>u__2;
											<>u__2 = default(ConfiguredTaskAwaiter);
											num = (<>1__state = -1);
										}
										((ConfiguredTaskAwaiter)(ref val3)).GetResult();
									}
									catch (System.Exception)
									{
										_ = connectionManager.Verbose;
									}
									goto IL_053a;
								}
								IL_04ae:
								if (<txTask>5__3 != null)
								{
									goto case 4;
								}
								goto IL_053a;
								IL_053a:
								try
								{
									_ = connectionManager.Verbose;
									<>8__1.cts.Dispose();
								}
								catch (System.Exception)
								{
									_ = connectionManager.Verbose;
								}
								<>8__1.cts = null;
								break;
							}
							_ = connectionManager.Verbose;
							object obj2 = <>7__wrap3;
							if (obj2 != null)
							{
								ExceptionDispatchInfo.Capture((obj2 as System.Exception) ?? throw obj2).Throw();
							}
							int num2 = <>7__wrap4;
							if (num2 != 1)
							{
								<>7__wrap3 = null;
							}
						}
						catch (System.Exception exception)
						{
							<>1__state = -2;
							<>8__1 = null;
							<rxTask>5__2 = null;
							<txTask>5__3 = null;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
							return;
						}
						<>1__state = -2;
						<>8__1 = null;
						<rxTask>5__2 = null;
						<txTask>5__3 = null;
						((AsyncTaskMethodBuilder)(ref <>t__builder)).SetResult();
					}

					[DebuggerHidden]
					private void SetStateMachine(IAsyncStateMachine stateMachine)
					{
						((AsyncTaskMethodBuilder)(ref <>t__builder)).SetStateMachine(stateMachine);
					}
				}

				[StructLayout((LayoutKind)3)]
				[CompilerGenerated]
				private struct <RxBackgroundTask>d__13 : IAsyncStateMachine
				{
					public int <>1__state;

					public AsyncTaskMethodBuilder <>t__builder;

					public ConnectionManager <>4__this;

					public CancellationToken ct;

					private T <rx_buf>5__2;

					private T <>7__wrap2;

					private ConfiguredTaskAwaiter<int> <>u__1;

					private void MoveNext()
					{
						//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
						//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
						//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
						//IL_007e: Unknown result type (might be due to invalid IL or missing references)
						//IL_0089: Unknown result type (might be due to invalid IL or missing references)
						//IL_008e: Unknown result type (might be due to invalid IL or missing references)
						//IL_0092: Unknown result type (might be due to invalid IL or missing references)
						//IL_0097: Unknown result type (might be due to invalid IL or missing references)
						//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
						//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
						int num = <>1__state;
						ConnectionManager connectionManager = <>4__this;
						try
						{
							try
							{
								if (num != 0)
								{
									<rx_buf>5__2 = new T
									{
										Capacity = 65536
									};
									connectionManager.tcpClient.GetStream();
									goto IL_0146;
								}
								ConfiguredTaskAwaiter<int> val = <>u__1;
								<>u__1 = default(ConfiguredTaskAwaiter<int>);
								num = (<>1__state = -1);
								goto IL_00e0;
								IL_00e0:
								int result = val.GetResult();
								<>7__wrap2.Length = result;
								<>7__wrap2 = null;
								if (<rx_buf>5__2.Length != 0)
								{
									if (!connectionManager.IsDisposed)
									{
										<rx_buf>5__2.SetTimeStamp();
										connectionManager.Adapter.RaiseMessageRx(<rx_buf>5__2, echo: false);
									}
									goto IL_0146;
								}
								goto end_IL_0011;
								IL_0146:
								if (connectionManager.IsConnected && !((CancellationToken)(ref ct)).IsCancellationRequested)
								{
									<>7__wrap2 = <rx_buf>5__2;
									val = ((Stream)connectionManager.tcpClient.GetStream()).ReadAsync(<rx_buf>5__2.Data, 0, <rx_buf>5__2.Data.Length, ct).ConfigureAwait(false).GetAwaiter();
									if (!val.IsCompleted)
									{
										num = (<>1__state = 0);
										<>u__1 = val;
										((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter<int>, <RxBackgroundTask>d__13>(ref val, ref this);
										return;
									}
									goto IL_00e0;
								}
								<rx_buf>5__2 = null;
								end_IL_0011:;
							}
							catch (System.Exception)
							{
								_ = connectionManager.Verbose;
							}
							finally
							{
								if (num < 0)
								{
									connectionManager.IsConnected = false;
								}
							}
						}
						catch (System.Exception exception)
						{
							<>1__state = -2;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
							return;
						}
						<>1__state = -2;
						((AsyncTaskMethodBuilder)(ref <>t__builder)).SetResult();
					}

					[DebuggerHidden]
					private void SetStateMachine(IAsyncStateMachine stateMachine)
					{
						((AsyncTaskMethodBuilder)(ref <>t__builder)).SetStateMachine(stateMachine);
					}
				}

				[StructLayout((LayoutKind)3)]
				[CompilerGenerated]
				private struct <TxBackgroundTask>d__14 : IAsyncStateMachine
				{
					public int <>1__state;

					public AsyncTaskMethodBuilder <>t__builder;

					public ConnectionManager <>4__this;

					public CancellationToken ct;

					private MessageBuffer <txbuf>5__2;

					private Timer <flushtimer>5__3;

					private ConfiguredTaskAwaiter <>u__1;

					private void MoveNext()
					{
						//IL_0145: Unknown result type (might be due to invalid IL or missing references)
						//IL_014a: Unknown result type (might be due to invalid IL or missing references)
						//IL_0151: Unknown result type (might be due to invalid IL or missing references)
						//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
						//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
						//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
						//IL_0273: Unknown result type (might be due to invalid IL or missing references)
						//IL_0278: Unknown result type (might be due to invalid IL or missing references)
						//IL_027f: Unknown result type (might be due to invalid IL or missing references)
						//IL_0173: Unknown result type (might be due to invalid IL or missing references)
						//IL_017e: Unknown result type (might be due to invalid IL or missing references)
						//IL_0183: Unknown result type (might be due to invalid IL or missing references)
						//IL_0187: Unknown result type (might be due to invalid IL or missing references)
						//IL_018c: Unknown result type (might be due to invalid IL or missing references)
						//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
						//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
						//IL_0212: Unknown result type (might be due to invalid IL or missing references)
						//IL_0217: Unknown result type (might be due to invalid IL or missing references)
						//IL_022c: Unknown result type (might be due to invalid IL or missing references)
						//IL_0237: Unknown result type (might be due to invalid IL or missing references)
						//IL_023c: Unknown result type (might be due to invalid IL or missing references)
						//IL_0240: Unknown result type (might be due to invalid IL or missing references)
						//IL_0245: Unknown result type (might be due to invalid IL or missing references)
						//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
						//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
						//IL_0259: Unknown result type (might be due to invalid IL or missing references)
						//IL_025a: Unknown result type (might be due to invalid IL or missing references)
						//IL_0109: Unknown result type (might be due to invalid IL or missing references)
						//IL_010e: Unknown result type (might be due to invalid IL or missing references)
						//IL_0112: Unknown result type (might be due to invalid IL or missing references)
						//IL_0117: Unknown result type (might be due to invalid IL or missing references)
						//IL_012b: Unknown result type (might be due to invalid IL or missing references)
						//IL_012c: Unknown result type (might be due to invalid IL or missing references)
						int num = <>1__state;
						ConnectionManager connectionManager = <>4__this;
						try
						{
							_ = 2;
							try
							{
								ConfiguredTaskAwaiter val;
								ConfiguredTaskAwaitable val3;
								switch (num)
								{
								default:
									<txbuf>5__2 = new MessageBuffer(2048);
									<flushtimer>5__3 = new Timer();
									goto IL_0295;
								case 0:
									val = <>u__1;
									<>u__1 = default(ConfiguredTaskAwaiter);
									num = (<>1__state = -1);
									goto IL_0160;
								case 1:
									val = <>u__1;
									<>u__1 = default(ConfiguredTaskAwaiter);
									num = (<>1__state = -1);
									goto IL_01d5;
								case 2:
									{
										val = <>u__1;
										<>u__1 = default(ConfiguredTaskAwaiter);
										num = (<>1__state = -1);
										goto IL_028e;
									}
									IL_0295:
									if (!connectionManager.IsConnected || ((CancellationToken)(ref ct)).IsCancellationRequested)
									{
										break;
									}
									if (!connectionManager.IsDisposed)
									{
										T val2 = default(T);
										while (<txbuf>5__2.Length < 1024 && connectionManager.TxQueue.TryDequeue(ref val2))
										{
											try
											{
												if (<txbuf>5__2.Length == 0)
												{
													<flushtimer>5__3.Reset();
												}
												<txbuf>5__2.Append(val2);
											}
											finally
											{
												if (num < 0)
												{
													val2?.ReturnToPool();
												}
											}
										}
										if (<flushtimer>5__3.ElapsedTime >= SocketClient<T>.WRITE_FLUSH_TIME || <txbuf>5__2.Length >= 512)
										{
											val3 = ((Stream)connectionManager.tcpClient.GetStream()).WriteAsync(<txbuf>5__2.Data, 0, <txbuf>5__2.Length).ConfigureAwait(false);
											val = ((ConfiguredTaskAwaitable)(ref val3)).GetAwaiter();
											if (!((ConfiguredTaskAwaiter)(ref val)).IsCompleted)
											{
												num = (<>1__state = 0);
												<>u__1 = val;
												((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <TxBackgroundTask>d__14>(ref val, ref this);
												return;
											}
											goto IL_0160;
										}
										goto IL_01e8;
									}
									goto end_IL_0013;
									IL_01d5:
									((ConfiguredTaskAwaiter)(ref val)).GetResult();
									<txbuf>5__2.Length = 0;
									goto IL_01e8;
									IL_028e:
									((ConfiguredTaskAwaiter)(ref val)).GetResult();
									goto IL_0295;
									IL_01e8:
									if (connectionManager.TxQueue.Count <= 0)
									{
										int num2 = 5;
										if (<txbuf>5__2.Length > 0)
										{
											TimeSpan elapsedTime = <flushtimer>5__3.ElapsedTime;
											num2 = 10 - (int)((TimeSpan)(ref elapsedTime)).TotalMilliseconds;
										}
										if (num2 > 0)
										{
											val3 = System.Threading.Tasks.Task.Delay(num2, ct).ConfigureAwait(false);
											val = ((ConfiguredTaskAwaitable)(ref val3)).GetAwaiter();
											if (!((ConfiguredTaskAwaiter)(ref val)).IsCompleted)
											{
												num = (<>1__state = 2);
												<>u__1 = val;
												((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <TxBackgroundTask>d__14>(ref val, ref this);
												return;
											}
											goto IL_028e;
										}
									}
									goto IL_0295;
									IL_0160:
									((ConfiguredTaskAwaiter)(ref val)).GetResult();
									val3 = ((Stream)connectionManager.tcpClient.GetStream()).FlushAsync(ct).ConfigureAwait(false);
									val = ((ConfiguredTaskAwaitable)(ref val3)).GetAwaiter();
									if (!((ConfiguredTaskAwaiter)(ref val)).IsCompleted)
									{
										num = (<>1__state = 1);
										<>u__1 = val;
										((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <TxBackgroundTask>d__14>(ref val, ref this);
										return;
									}
									goto IL_01d5;
								}
								<txbuf>5__2 = null;
								<flushtimer>5__3 = null;
								end_IL_0013:;
							}
							catch (System.Exception)
							{
								_ = connectionManager.Verbose;
							}
							finally
							{
								if (num < 0)
								{
									connectionManager.IsConnected = false;
								}
							}
						}
						catch (System.Exception exception)
						{
							<>1__state = -2;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
							return;
						}
						<>1__state = -2;
						((AsyncTaskMethodBuilder)(ref <>t__builder)).SetResult();
					}

					[DebuggerHidden]
					private void SetStateMachine(IAsyncStateMachine stateMachine)
					{
						((AsyncTaskMethodBuilder)(ref <>t__builder)).SetStateMachine(stateMachine);
					}
				}

				private const int RAW_RX_BUF_SIZE = 65536;

				private const int RAW_TX_BUF_SIZE = 2048;

				private readonly bool Verbose;

				private SocketClient<T> Adapter;

				private TcpClient tcpClient;

				private CancellationTokenSource masterCTS = new CancellationTokenSource();

				private bool IsConnected;

				private readonly System.Threading.Tasks.Task healthTask;

				private ConcurrentQueue<T> TxQueue = new ConcurrentQueue<T>();

				public ConnectionManager(SocketClient<T> adapter, bool verbose)
				{
					//IL_0001: Unknown result type (might be due to invalid IL or missing references)
					//IL_000b: Expected O, but got Unknown
					//IL_0049: Unknown result type (might be due to invalid IL or missing references)
					Adapter = adapter;
					Adapter.AddDisposable(this);
					Verbose = verbose;
					healthTask = System.Threading.Tasks.Task.Run((Func<System.Threading.Tasks.Task>)HealthTask, masterCTS.Token);
				}

				private void ClearTxQueue()
				{
					T val = default(T);
					while (TxQueue.TryDequeue(ref val))
					{
						val?.ReturnToPool();
					}
				}

				public bool Transmit(T msg)
				{
					//IL_001c: Unknown result type (might be due to invalid IL or missing references)
					if (base.IsDisposed)
					{
						return false;
					}
					if (!msg.IsMemberOfPool)
					{
						throw new InvalidOperationException("only pooled Communications.MessageBuffers buffers are accepted by this Adapter");
					}
					if (IsConnected)
					{
						msg.Retain();
						TxQueue.Enqueue(msg);
						return true;
					}
					return false;
				}

				[AsyncStateMachine(typeof(SocketClient<>.ConnectionManager.<HealthTask>d__12))]
				private System.Threading.Tasks.Task HealthTask()
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					<HealthTask>d__12 <HealthTask>d__ = default(<HealthTask>d__12);
					<HealthTask>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
					<HealthTask>d__.<>4__this = this;
					<HealthTask>d__.<>1__state = -1;
					((AsyncTaskMethodBuilder)(ref <HealthTask>d__.<>t__builder)).Start<<HealthTask>d__12>(ref <HealthTask>d__);
					return ((AsyncTaskMethodBuilder)(ref <HealthTask>d__.<>t__builder)).Task;
				}

				[AsyncStateMachine(typeof(SocketClient<>.ConnectionManager.<RxBackgroundTask>d__13))]
				private System.Threading.Tasks.Task RxBackgroundTask(CancellationToken ct)
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					//IL_0016: Unknown result type (might be due to invalid IL or missing references)
					//IL_0017: Unknown result type (might be due to invalid IL or missing references)
					<RxBackgroundTask>d__13 <RxBackgroundTask>d__ = default(<RxBackgroundTask>d__13);
					<RxBackgroundTask>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
					<RxBackgroundTask>d__.<>4__this = this;
					<RxBackgroundTask>d__.ct = ct;
					<RxBackgroundTask>d__.<>1__state = -1;
					((AsyncTaskMethodBuilder)(ref <RxBackgroundTask>d__.<>t__builder)).Start<<RxBackgroundTask>d__13>(ref <RxBackgroundTask>d__);
					return ((AsyncTaskMethodBuilder)(ref <RxBackgroundTask>d__.<>t__builder)).Task;
				}

				[AsyncStateMachine(typeof(SocketClient<>.ConnectionManager.<TxBackgroundTask>d__14))]
				private System.Threading.Tasks.Task TxBackgroundTask(CancellationToken ct)
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					//IL_0016: Unknown result type (might be due to invalid IL or missing references)
					//IL_0017: Unknown result type (might be due to invalid IL or missing references)
					<TxBackgroundTask>d__14 <TxBackgroundTask>d__ = default(<TxBackgroundTask>d__14);
					<TxBackgroundTask>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
					<TxBackgroundTask>d__.<>4__this = this;
					<TxBackgroundTask>d__.ct = ct;
					<TxBackgroundTask>d__.<>1__state = -1;
					((AsyncTaskMethodBuilder)(ref <TxBackgroundTask>d__.<>t__builder)).Start<<TxBackgroundTask>d__14>(ref <TxBackgroundTask>d__);
					return ((AsyncTaskMethodBuilder)(ref <TxBackgroundTask>d__.<>t__builder)).Task;
				}

				public override void Dispose(bool disposing)
				{
					if (!disposing)
					{
						return;
					}
					_ = Verbose;
					CancellationTokenSource obj = masterCTS;
					if (obj != null)
					{
						obj.Cancel();
					}
					IsConnected = false;
					SocketClient<T> adapter = Adapter;
					Adapter = null;
					try
					{
						TcpClient obj2 = tcpClient;
						if (obj2 != null)
						{
							obj2.Dispose();
						}
					}
					catch
					{
					}
					tcpClient = null;
					_ = Verbose;
					try
					{
						healthTask.Wait();
					}
					catch (System.Exception)
					{
						_ = Verbose;
					}
					CancellationTokenSource obj4 = masterCTS;
					if (obj4 != null)
					{
						obj4.Dispose();
					}
					masterCTS = null;
					_ = Verbose;
					adapter?.RaiseAdapterClosed();
					ClearTxQueue();
				}
			}

			[StructLayout((LayoutKind)3)]
			[CompilerGenerated]
			private struct <ConnectAsync>d__26 : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncTaskMethodBuilder<bool> <>t__builder;

				public SocketClient<T> <>4__this;

				public AsyncOperation obj;

				private ConfiguredTaskAwaiter <>u__1;

				private void MoveNext()
				{
					//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
					//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
					//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
					//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
					//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
					//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
					//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
					//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
					//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					SocketClient<T> socketClient = <>4__this;
					bool result;
					try
					{
						if (num != 0 && socketClient.Connection == null)
						{
							object criticalSection = socketClient.CriticalSection;
							bool flag = false;
							try
							{
								Monitor.Enter(criticalSection, ref flag);
								if (!socketClient.IsDisposed)
								{
									if (socketClient.Connection == null)
									{
										socketClient.Connection = new ConnectionManager(socketClient, socketClient.Verbose);
									}
									goto end_IL_0023;
								}
								result = false;
								goto end_IL_000e;
								end_IL_0023:;
							}
							finally
							{
								if (num < 0 && flag)
								{
									Monitor.Exit(criticalSection);
								}
							}
						}
						try
						{
							if (num != 0)
							{
								goto IL_0069;
							}
							ConfiguredTaskAwaiter val = <>u__1;
							<>u__1 = default(ConfiguredTaskAwaiter);
							num = (<>1__state = -1);
							goto IL_0100;
							IL_0100:
							((ConfiguredTaskAwaiter)(ref val)).GetResult();
							goto IL_0069;
							IL_0069:
							obj.ThrowIfCancellationRequested();
							if (socketClient.IsConnected)
							{
								result = true;
							}
							else if (socketClient.IsDisposed)
							{
								result = false;
							}
							else
							{
								if (socketClient.Connection != null)
								{
									ConfiguredTaskAwaitable val2 = System.Threading.Tasks.Task.Delay(33).ConfigureAwait(false);
									val = ((ConfiguredTaskAwaitable)(ref val2)).GetAwaiter();
									if (!((ConfiguredTaskAwaiter)(ref val)).IsCompleted)
									{
										num = (<>1__state = 0);
										<>u__1 = val;
										<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <ConnectAsync>d__26>(ref val, ref this);
										return;
									}
									goto IL_0100;
								}
								result = false;
							}
						}
						finally
						{
							if (num < 0 && !socketClient.IsConnected)
							{
								socketClient.Connection = null;
							}
						}
						end_IL_000e:;
					}
					catch (System.Exception exception)
					{
						<>1__state = -2;
						<>t__builder.SetException(exception);
						return;
					}
					<>1__state = -2;
					<>t__builder.SetResult(result);
				}

				[DebuggerHidden]
				private void SetStateMachine(IAsyncStateMachine stateMachine)
				{
					<>t__builder.SetStateMachine(stateMachine);
				}
			}

			[StructLayout((LayoutKind)3)]
			[CompilerGenerated]
			private struct <DisconnectAsync>d__27 : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncTaskMethodBuilder<bool> <>t__builder;

				public SocketClient<T> <>4__this;

				public AsyncOperation obj;

				private ConfiguredTaskAwaiter <>u__1;

				private void MoveNext()
				{
					//IL_0091: Unknown result type (might be due to invalid IL or missing references)
					//IL_0096: Unknown result type (might be due to invalid IL or missing references)
					//IL_009d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0058: Unknown result type (might be due to invalid IL or missing references)
					//IL_005d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0061: Unknown result type (might be due to invalid IL or missing references)
					//IL_0066: Unknown result type (might be due to invalid IL or missing references)
					//IL_007a: Unknown result type (might be due to invalid IL or missing references)
					//IL_007b: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					SocketClient<T> socketClient = <>4__this;
					bool result;
					try
					{
						ConfiguredTaskAwaiter val;
						if (num == 0)
						{
							val = <>u__1;
							<>u__1 = default(ConfiguredTaskAwaiter);
							num = (<>1__state = -1);
							goto IL_00ac;
						}
						socketClient.Connection = null;
						if (socketClient.Connection != null)
						{
							goto IL_0027;
						}
						result = true;
						goto end_IL_000e;
						IL_00ac:
						((ConfiguredTaskAwaiter)(ref val)).GetResult();
						goto IL_0027;
						IL_0027:
						obj.ThrowIfCancellationRequested();
						if (socketClient.Connection == null)
						{
							result = true;
						}
						else
						{
							if (!socketClient.IsDisposed)
							{
								ConfiguredTaskAwaitable val2 = System.Threading.Tasks.Task.Delay(33).ConfigureAwait(false);
								val = ((ConfiguredTaskAwaitable)(ref val2)).GetAwaiter();
								if (!((ConfiguredTaskAwaiter)(ref val)).IsCompleted)
								{
									num = (<>1__state = 0);
									<>u__1 = val;
									<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <DisconnectAsync>d__27>(ref val, ref this);
									return;
								}
								goto IL_00ac;
							}
							result = true;
						}
						end_IL_000e:;
					}
					catch (System.Exception exception)
					{
						<>1__state = -2;
						<>t__builder.SetException(exception);
						return;
					}
					<>1__state = -2;
					<>t__builder.SetResult(result);
				}

				[DebuggerHidden]
				private void SetStateMachine(IAsyncStateMachine stateMachine)
				{
					<>t__builder.SetStateMachine(stateMachine);
				}
			}

			private static readonly TimeSpan WRITE_FLUSH_TIME = TimeSpan.FromMilliseconds(10.0);

			private readonly object CriticalSection = new object();

			private readonly PhysicalAddress mMac = new NetworkInterfaceCardAddress();

			private ConnectionManager mConnection;

			public string Address
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				private set;
			}

			public int Port
			{
				[CompilerGenerated]
				get;
				[CompilerGenerated]
				private set;
			}

			private ConnectionManager Connection
			{
				get
				{
					return mConnection;
				}
				set
				{
					if (mConnection == value)
					{
						return;
					}
					lock (CriticalSection)
					{
						if (mConnection == value)
						{
							return;
						}
						if (mConnection != null)
						{
							_ = Verbose;
							try
							{
								mConnection.Dispose();
							}
							catch
							{
							}
							mConnection = null;
							_ = Verbose;
							RaiseAdapterClosed();
						}
						mConnection = value;
					}
				}
			}

			public override IPhysicalAddress MAC => mMac;

			public SocketClient(string address, int port)
				: this(address, port, (IMessageEncoder<T>)null, (MessageDecoder<T>)null)
			{
			}

			public SocketClient(string address, int port, IMessageEncoder<T> encoder)
				: this(address, port, encoder, (MessageDecoder<T>)null)
			{
			}

			public SocketClient(string address, int port, MessageDecoder<T> decoder)
				: this(address, port, (IMessageEncoder<T>)null, decoder)
			{
			}

			public SocketClient(string address, int port, IMessageEncoder<T> encoder, MessageDecoder<T> decoder)
				: this(address, port, encoder, decoder, verbose: false)
			{
			}

			public SocketClient(string address, int port, IMessageEncoder<T> encoder, MessageDecoder<T> decoder, bool verbose)
				: this(((object)address).ToString() + ":" + port, address, port, encoder, decoder, verbose)
			{
			}

			public SocketClient(string name, string address, int port, IMessageEncoder<T> encoder, MessageDecoder<T> decoder)
				: this(name, address, port, encoder, decoder, verbose: false)
			{
			}

			public SocketClient(string name, string address, int port, IMessageEncoder<T> encoder, MessageDecoder<T> decoder, bool verbose)
				: base(name, encoder, decoder, verbose)
			{
				Address = address;
				Port = port;
			}

			[AsyncStateMachine(typeof(SocketClient<>.<ConnectAsync>d__26))]
			protected override async System.Threading.Tasks.Task<bool> ConnectAsync(AsyncOperation obj)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				if (Connection == null)
				{
					lock (CriticalSection)
					{
						if (base.IsDisposed)
						{
							return false;
						}
						if (Connection == null)
						{
							Connection = new ConnectionManager(this, Verbose);
						}
					}
				}
				try
				{
					while (true)
					{
						obj.ThrowIfCancellationRequested();
						if (base.IsConnected)
						{
							return true;
						}
						if (base.IsDisposed)
						{
							return false;
						}
						if (Connection == null)
						{
							break;
						}
						await System.Threading.Tasks.Task.Delay(33).ConfigureAwait(false);
					}
					return false;
				}
				finally
				{
					if (!base.IsConnected)
					{
						Connection = null;
					}
				}
			}

			[AsyncStateMachine(typeof(SocketClient<>.<DisconnectAsync>d__27))]
			protected override async System.Threading.Tasks.Task<bool> DisconnectAsync(AsyncOperation obj)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				Connection = null;
				if (Connection == null)
				{
					return true;
				}
				while (true)
				{
					obj.ThrowIfCancellationRequested();
					if (Connection == null)
					{
						return true;
					}
					if (base.IsDisposed)
					{
						break;
					}
					await System.Threading.Tasks.Task.Delay(33).ConfigureAwait(false);
				}
				return true;
			}

			protected override bool TransmitRaw(T buffer)
			{
				return Connection?.Transmit(buffer) ?? false;
			}

			public override void Dispose(bool disposing)
			{
				if (disposing)
				{
					Connection = null;
				}
				base.Dispose(disposing);
			}
		}

		private static StringBuilder sb = new StringBuilder();

		public static string ToString(System.Collections.Generic.IReadOnlyList<byte> msg)
		{
			return ToString(msg, dataonly: false);
		}

		public static string ToString(System.Collections.Generic.IReadOnlyList<byte> msg, bool dataonly)
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			if (((System.Collections.Generic.IReadOnlyCollection<byte>)msg).Count <= 0)
			{
				if (dataonly)
				{
					return "EMPTY";
				}
				return "Data[0] = [EMPTY]";
			}
			lock (sb)
			{
				sb.Clear();
				if (!dataonly)
				{
					StringBuilder val = sb;
					AppendInterpolatedStringHandler val2 = new AppendInterpolatedStringHandler(10, 1, val);
					((AppendInterpolatedStringHandler)(ref val2)).AppendLiteral("Data[");
					((AppendInterpolatedStringHandler)(ref val2)).AppendFormatted<int>(((System.Collections.Generic.IReadOnlyCollection<byte>)msg).Count);
					((AppendInterpolatedStringHandler)(ref val2)).AppendLiteral("] = [");
					val.Append(ref val2);
				}
				int num = 0;
				System.Collections.Generic.IEnumerator<byte> enumerator = ((System.Collections.Generic.IEnumerable<byte>)msg).GetEnumerator();
				try
				{
					while (((System.Collections.IEnumerator)enumerator).MoveNext())
					{
						byte current = enumerator.Current;
						if (num++ > 0)
						{
							sb.Append(' ');
						}
						sb.Append(current.HexString());
					}
				}
				finally
				{
					((System.IDisposable)enumerator)?.Dispose();
				}
				if (!dataonly)
				{
					sb.Append(']');
				}
				return ((object)sb).ToString();
			}
		}
	}
	public static class ByteExtensions
	{
		private static readonly string[] ByteString;

		static ByteExtensions()
		{
			ByteString = new string[256];
			for (int i = 0; i < ByteString.Length; i++)
			{
				ByteString[i] = i.ToString("X2");
			}
		}

		public static string HexString(this byte b)
		{
			return ByteString[b];
		}
	}
	public static class CommExtensions
	{
		public static sbyte GetINT8(this Comm.IByteList msg, int index)
		{
			return (sbyte)msg.GetUINT8(index);
		}

		public static byte GetUINT8(this Comm.IByteList msg, int index)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			if (index >= msg.Length)
			{
				throw new IndexOutOfRangeException();
			}
			return ((System.Collections.Generic.IReadOnlyList<byte>)msg)[index];
		}

		public static short GetINT16(this Comm.IByteList msg, int index)
		{
			return (short)((msg.GetUINT8(index) << 8) + msg.GetUINT8(index + 1));
		}

		public static ushort GetUINT16(this Comm.IByteList msg, int index)
		{
			return (ushort)((msg.GetUINT8(index) << 8) + msg.GetUINT8(index + 1));
		}

		public static Int24 GetINT24(this Comm.IByteList msg, int index)
		{
			return (Int24)((msg.GetUINT16(index) << 8) + msg.GetUINT8(index + 2));
		}

		public static UInt24 GetUINT24(this Comm.IByteList msg, int index)
		{
			return (UInt24)((msg.GetUINT16(index) << 8) + msg.GetUINT8(index + 2));
		}

		public static int GetINT32(this Comm.IByteList msg, int index)
		{
			return (msg.GetUINT16(index) << 16) + msg.GetUINT16(index + 2);
		}

		public static uint GetUINT32(this Comm.IByteList msg, int index)
		{
			return (uint)((msg.GetUINT16(index) << 16) + msg.GetUINT16(index + 2));
		}

		public static Int40 GetINT40(this Comm.IByteList msg, int index)
		{
			return (Int40)(((ulong)msg.GetUINT32(index) << 8) + msg.GetUINT8(index + 4));
		}

		public static UInt40 GetUINT40(this Comm.IByteList msg, int index)
		{
			return (UInt40)(((ulong)msg.GetUINT32(index) << 8) + msg.GetUINT8(index + 4));
		}

		public static Int48 GetINT48(this Comm.IByteList msg, int index)
		{
			return (Int48)(((ulong)msg.GetUINT32(index) << 16) + msg.GetUINT16(index + 4));
		}

		public static UInt48 GetUINT48(this Comm.IByteList msg, int index)
		{
			return (UInt48)(((ulong)msg.GetUINT32(index) << 16) + msg.GetUINT16(index + 4));
		}

		public static Int56 GetINT56(this Comm.IByteList msg, int index)
		{
			return (Int56)(((ulong)msg.GetUINT32(index) << 24) + (ulong)msg.GetUINT24(index + 4));
		}

		public static UInt56 GetUINT56(this Comm.IByteList msg, int index)
		{
			return (UInt56)(((ulong)msg.GetUINT32(index) << 24) + (ulong)msg.GetUINT24(index + 4));
		}

		public static long GetINT64(this Comm.IByteList msg, int index)
		{
			return (long)(((ulong)msg.GetUINT32(index) << 32) + msg.GetUINT32(index + 4));
		}

		public static ulong GetUINT64(this Comm.IByteList msg, int index)
		{
			return ((ulong)msg.GetUINT32(index) << 32) + msg.GetUINT32(index + 4);
		}
	}
	public class CRC32_LE
	{
		private static readonly uint[] crc32_le_table;

		public const uint RESET_VALUE = 4294967295u;

		[field: CompilerGenerated]
		public uint Value
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public static implicit operator uint(CRC32_LE crc)
		{
			return crc.Value;
		}

		public CRC32_LE()
		{
			Reset();
		}

		public void Reset()
		{
			Value = 4294967295u;
		}

		public void Update(byte b)
		{
			Value = Update(Value, b);
		}

		public void Update(System.Collections.Generic.IReadOnlyList<byte> buffer)
		{
			Update(buffer, ((System.Collections.Generic.IReadOnlyCollection<byte>)buffer).Count, 0u);
		}

		public void Update(System.Collections.Generic.IReadOnlyList<byte> buffer, int count)
		{
			Update(buffer, count, 0u);
		}

		public void Update(System.Collections.Generic.IReadOnlyList<byte> buffer, int count, uint offset)
		{
			Value = Calculate(Value, buffer, count, offset);
		}

		public static uint Calculate(System.Collections.Generic.IReadOnlyCollection<byte> bytes)
		{
			uint num = 4294967295u;
			System.Collections.Generic.IEnumerator<byte> enumerator = ((System.Collections.Generic.IEnumerable<byte>)bytes).GetEnumerator();
			try
			{
				while (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					byte current = enumerator.Current;
					num = Update(num, current);
				}
				return num;
			}
			finally
			{
				((System.IDisposable)enumerator)?.Dispose();
			}
		}

		public static uint Calculate(System.Collections.Generic.IReadOnlyList<byte> buffer, int count)
		{
			return Calculate(4294967295u, buffer, count, 0u);
		}

		public static uint Calculate(System.Collections.Generic.IReadOnlyList<byte> buffer, int count, uint offset)
		{
			return Calculate(4294967295u, buffer, count, offset);
		}

		public static uint Calculate(uint crc, System.Collections.Generic.IReadOnlyList<byte> buffer, int count, uint offset)
		{
			crc = Crc32_le(crc, (byte[])buffer, (uint)count, offset);
			return crc;
		}

		private static uint Calculate(uint crc, byte[] buf, int count)
		{
			crc = Crc32_le(crc, buf, (uint)count, 0u);
			return crc;
		}

		public static uint Update(uint crc, byte data)
		{
			return Crc32_le(crc, new byte[1] { data }, 1u, 0u);
		}

		public static uint Crc32_le(uint crc, byte[] buf, uint len, uint offset)
		{
			crc = ~crc;
			if (offset + len <= buf.Length)
			{
				for (uint num = offset; num < offset + len; num++)
				{
					crc = crc32_le_table[(crc ^ buf[num]) & 0xFF] ^ (crc >> 8);
				}
			}
			return ~crc;
		}

		static CRC32_LE()
		{
			uint[] array = new uint[256];
			RuntimeHelpers.InitializeArray((System.Array)array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			crc32_le_table = array;
		}
	}
	public class CRC32
	{
		private const uint RESET_VALUE = 4294967295u;

		private const uint CRC_POLYNOMIAL = 1947962583u;

		[field: CompilerGenerated]
		public uint Value
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public static implicit operator uint(CRC32 crc)
		{
			return crc.Value;
		}

		public CRC32()
		{
			Reset();
		}

		public void Reset()
		{
			Value = 4294967295u;
		}

		public void Update(byte b)
		{
			Value = Update(Value, b);
		}

		public void Update(System.Collections.Generic.IReadOnlyList<byte> buffer)
		{
			Update(buffer, ((System.Collections.Generic.IReadOnlyCollection<byte>)buffer).Count, 0);
		}

		public void Update(System.Collections.Generic.IReadOnlyList<byte> buffer, int count)
		{
			Update(buffer, count, 0);
		}

		public void Update(System.Collections.Generic.IReadOnlyList<byte> buffer, int count, int offset)
		{
			Value = Calculate(Value, buffer, count, offset);
		}

		public static uint Calculate(System.Collections.Generic.IReadOnlyCollection<byte> bytes)
		{
			uint num = 4294967295u;
			System.Collections.Generic.IEnumerator<byte> enumerator = ((System.Collections.Generic.IEnumerable<byte>)bytes).GetEnumerator();
			try
			{
				while (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					byte current = enumerator.Current;
					num = Update(num, current);
				}
				return num;
			}
			finally
			{
				((System.IDisposable)enumerator)?.Dispose();
			}
		}

		public static uint Calculate(System.Collections.Generic.IReadOnlyList<byte> buffer, int count)
		{
			return Calculate(4294967295u, buffer, count, 0);
		}

		public static uint Calculate(System.Collections.Generic.IReadOnlyList<byte> buffer, int count, int offset)
		{
			return Calculate(4294967295u, buffer, count, offset);
		}

		private static uint Calculate(uint crc, System.Collections.Generic.IReadOnlyList<byte> buffer, int count, int offset)
		{
			while (count-- > 0)
			{
				crc = Update(crc, buffer[offset++]);
			}
			return crc;
		}

		private static uint Update(uint crc, byte data)
		{
			crc ^= (uint)(data << 24);
			for (int i = 0; i < 8; i++)
			{
				crc = (((crc & 0x80000000u) != 2147483648u) ? (crc << 1) : ((crc << 1) ^ 0x741B8CD7));
			}
			return crc;
		}
	}
	public class CRC8
	{
		private const byte RESET_VALUE = 85;

		private static readonly byte[] Table;

		[field: CompilerGenerated]
		public byte Value
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public static implicit operator byte(CRC8 crc)
		{
			return crc.Value;
		}

		public CRC8()
		{
			Reset();
		}

		public void Reset()
		{
			Value = 85;
		}

		public void Update(byte b)
		{
			Value = Table[(Value ^ b) & 0xFF];
		}

		public void Update(System.Collections.Generic.IReadOnlyList<byte> buffer)
		{
			Update(buffer, ((System.Collections.Generic.IReadOnlyCollection<byte>)buffer).Count, 0);
		}

		public void Update(System.Collections.Generic.IReadOnlyList<byte> buffer, int count)
		{
			Update(buffer, count, 0);
		}

		public void Update(System.Collections.Generic.IReadOnlyList<byte> buffer, int count, int offset)
		{
			Value = Calculate(Value, buffer, count, offset);
		}

		public static byte Calculate(System.Collections.Generic.IReadOnlyList<byte> buffer)
		{
			return Calculate(85, buffer, ((System.Collections.Generic.IReadOnlyCollection<byte>)buffer).Count, 0);
		}

		public static byte Calculate(System.Collections.Generic.IReadOnlyList<byte> buffer, int count)
		{
			return Calculate(85, buffer, count, 0);
		}

		public static byte Calculate(System.Collections.Generic.IReadOnlyList<byte> buffer, int count, int offset)
		{
			return Calculate(85, buffer, count, offset);
		}

		private static byte Calculate(byte crc, System.Collections.Generic.IReadOnlyList<byte> buffer, int count, int offset)
		{
			if ((count & 1) != 0)
			{
				crc = Table[(crc ^ buffer[offset++]) & 0xFF];
			}
			count >>= 1;
			if ((count & 1) != 0)
			{
				crc = Table[(crc ^ buffer[offset++]) & 0xFF];
				crc = Table[(crc ^ buffer[offset++]) & 0xFF];
			}
			count >>= 1;
			while (count-- > 0)
			{
				crc = Table[(crc ^ buffer[offset++]) & 0xFF];
				crc = Table[(crc ^ buffer[offset++]) & 0xFF];
				crc = Table[(crc ^ buffer[offset++]) & 0xFF];
				crc = Table[(crc ^ buffer[offset++]) & 0xFF];
			}
			return crc;
		}

		public static byte Calculate(System.Collections.Generic.IReadOnlyCollection<byte> bytes)
		{
			byte b = 85;
			if (bytes != null)
			{
				System.Collections.Generic.IEnumerator<byte> enumerator = ((System.Collections.Generic.IEnumerable<byte>)bytes).GetEnumerator();
				try
				{
					while (((System.Collections.IEnumerator)enumerator).MoveNext())
					{
						byte current = enumerator.Current;
						b = Table[(b ^ current) & 0xFF];
					}
				}
				finally
				{
					((System.IDisposable)enumerator)?.Dispose();
				}
			}
			return b;
		}

		public static byte Calculate(byte[] bytes)
		{
			byte b = 85;
			if (bytes != null)
			{
				foreach (byte b2 in bytes)
				{
					b = Table[(b ^ b2) & 0xFF];
				}
			}
			return b;
		}

		static CRC8()
		{
			byte[] array = new byte[256];
			RuntimeHelpers.InitializeArray((System.Array)array, (RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
			Table = array;
		}
	}
	public interface IDisposable : System.IDisposable
	{
		bool IsDisposed { get; }

		void Dispose(bool disposing);
	}
	public abstract class Disposable : IDisposable, System.IDisposable
	{
		private int mIsDisposed;

		public bool IsDisposed => mIsDisposed != 0;

		public void Dispose()
		{
			if (!IsDisposed && Interlocked.Exchange(ref mIsDisposed, 1) == 0)
			{
				Dispose(disposing: true);
			}
		}

		public abstract void Dispose(bool disposing);
	}
	public interface IDisposableManager : IDisposable, System.IDisposable
	{
		void AddDisposable(IDisposable obj);

		void RemoveDisposable(IDisposable obj);
	}
	public class DisposableManager : Disposable, IDisposableManager, IDisposable, System.IDisposable
	{
		private object CriticalSection = new object();

		private List<WeakReference<IDisposable>> Items = new List<WeakReference<IDisposable>>();

		private int TimeSinceLastInventory;

		private bool ShouldInventory
		{
			get
			{
				int num = Math.Max(20, Items.Count / 10);
				return ++TimeSinceLastInventory > num;
			}
		}

		public void AddDisposable(IDisposable obj)
		{
			if (base.IsDisposed || obj == null)
			{
				return;
			}
			lock (CriticalSection)
			{
				if (!ContainsDisposable(obj))
				{
					if (ShouldInventory)
					{
						RemoveDisposable(null, inventory: true);
					}
					Items.Add(new WeakReference<IDisposable>(obj));
				}
			}
		}

		private bool ContainsDisposable(IDisposable obj)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			if (!base.IsDisposed)
			{
				lock (CriticalSection)
				{
					Enumerator<WeakReference<IDisposable>> enumerator = Items.GetEnumerator();
					try
					{
						IDisposable disposable = default(IDisposable);
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.TryGetTarget(ref disposable) && disposable == obj)
							{
								return true;
							}
						}
					}
					finally
					{
						((System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
					}
				}
			}
			return false;
		}

		public void RemoveDisposable(IDisposable obj)
		{
			RemoveDisposable(obj, ShouldInventory);
		}

		private void RemoveDisposable(IDisposable obj, bool inventory)
		{
			lock (CriticalSection)
			{
				if (inventory)
				{
					TimeSinceLastInventory = 0;
				}
				IDisposable disposable = default(IDisposable);
				for (int num = Items.Count - 1; num >= 0; num--)
				{
					if (!Items[num].TryGetTarget(ref disposable))
					{
						Items.RemoveAt(num);
					}
					else if (disposable == null)
					{
						Items.RemoveAt(num);
					}
					else if (disposable == obj)
					{
						Items.RemoveAt(num);
						if (!inventory)
						{
							break;
						}
					}
				}
			}
		}

		public override void Dispose(bool disposing)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			if (!disposing)
			{
				return;
			}
			lock (CriticalSection)
			{
				Enumerator<WeakReference<IDisposable>> enumerator = Items.GetEnumerator();
				try
				{
					IDisposable disposable = default(IDisposable);
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.TryGetTarget(ref disposable))
						{
							try
							{
								((System.IDisposable)disposable)?.Dispose();
							}
							catch
							{
							}
						}
					}
				}
				finally
				{
					((System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
				}
				Items.Clear();
				Items = null;
			}
		}
	}
	public interface IFreeRunningCounter
	{
		ulong ClockFrequency_hz { get; }

		long Ticks { get; }
	}
	public class FreeRunningCounter
	{
		private static IFreeRunningCounter mInstance = null;

		private static object CriticalSection = new object();

		public static IFreeRunningCounter Instance
		{
			get
			{
				return mInstance;
			}
			set
			{
				if (mInstance != null)
				{
					return;
				}
				lock (CriticalSection)
				{
					if (mInstance == null)
					{
						mInstance = value;
					}
				}
			}
		}

		public static ulong ClockFrequency_hz => mInstance.ClockFrequency_hz;

		public static long Ticks => mInstance.Ticks;
	}
	public class ImageCache
	{
		public interface Interface
		{
			void RegisterImageReference(System.Enum id);
		}

		private static readonly object CriticalSection = new object();

		private static Interface mInstance = null;

		public static Interface Instance
		{
			get
			{
				return mInstance;
			}
			set
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				if (mInstance != null)
				{
					throw new InvalidOperationException("Singleton failure, cannot set ImageCache.Instance more than once");
				}
				lock (CriticalSection)
				{
					if (value == null)
					{
						throw new System.Exception("ImageCache.Instance must be set to a valid reference");
					}
					if (mInstance == null)
					{
						mInstance = value;
					}
				}
			}
		}

		public static void RegisterEnumImageReferences(System.Type t)
		{
			foreach (System.Enum value in System.Enum.GetValues(t))
			{
				RegisterImageReference(value);
			}
		}

		public static void RegisterImageReference(System.Enum id)
		{
			if (Instance == null)
			{
				throw new System.Exception("Application derived ImageCache must be instatiated prior to use");
			}
			Instance.RegisterImageReference(id);
		}
	}
	[DefaultMember("Item")]
	public abstract class ImageCache<T> : ImageCache, ImageCache.Interface
	{
		private static readonly ConcurrentDictionary<System.Type, ConcurrentDictionary<System.Enum, T>> TopLevelDictionary = (ConcurrentDictionary<System.Type, ConcurrentDictionary<System.Enum, T>>)(object)new ConcurrentDictionary<System.Type, ConcurrentDictionary<System.Enum, System.Type>>();

		public unsafe T this[System.Enum id]
		{
			get
			{
				//IL_0071: Unknown result type (might be due to invalid IL or missing references)
				if (id == null)
				{
					return default(T);
				}
				ConcurrentDictionary<System.Enum, T> val = default(ConcurrentDictionary<System.Enum, T>);
				T result = default(T);
				if (((ConcurrentDictionary<System.Type, ConcurrentDictionary<System.Enum, System.Type>>)(object)TopLevelDictionary).TryGetValue(((object)id).GetType(), ref *(ConcurrentDictionary<System.Enum, System.Type>*)(&val)) && ((ConcurrentDictionary<System.Enum, System.Enum>)(object)val).TryGetValue(id, ref *(System.Enum*)(&result)))
				{
					return result;
				}
				throw new ArgumentOutOfRangeException($"Image was not registered: {((object)id).GetType().FullName}.{id}");
			}
		}

		protected abstract T LoadImageResource(System.Enum reference);

		public new unsafe void RegisterImageReference(System.Enum id)
		{
			System.Type type = ((object)id).GetType();
			ConcurrentDictionary<System.Enum, T> val = default(ConcurrentDictionary<System.Enum, T>);
			if (!((ConcurrentDictionary<System.Type, ConcurrentDictionary<System.Enum, System.Type>>)(object)TopLevelDictionary).TryGetValue(type, ref *(ConcurrentDictionary<System.Enum, System.Type>*)(&val)))
			{
				val = (ConcurrentDictionary<System.Enum, T>)(object)new ConcurrentDictionary<System.Enum, System.Enum>();
				((ConcurrentDictionary<System.Type, ConcurrentDictionary<System.Enum, System.Type>>)(object)TopLevelDictionary).TryAdd(type, (ConcurrentDictionary<System.Enum, System.Type>)(object)val);
				((ConcurrentDictionary<System.Type, ConcurrentDictionary<System.Enum, System.Type>>)(object)TopLevelDictionary).TryGetValue(type, ref *(ConcurrentDictionary<System.Enum, System.Type>*)(&val));
			}
			T val2 = default(T);
			if (!((ConcurrentDictionary<System.Enum, System.Enum>)(object)val).TryGetValue(id, ref *(System.Enum*)(&val2)))
			{
				val2 = LoadImageResource(id);
				if (val2 == null)
				{
					throw new System.Exception($"No image exists for registered id {type}.{id}");
				}
				((ConcurrentDictionary<System.Enum, System.Enum>)(object)val).TryAdd(id, (System.Enum)val2);
			}
		}

		private unsafe T GetImage(System.Enum id)
		{
			ConcurrentDictionary<System.Enum, T> val = default(ConcurrentDictionary<System.Enum, T>);
			T result = default(T);
			if (((ConcurrentDictionary<System.Type, ConcurrentDictionary<System.Enum, System.Type>>)(object)TopLevelDictionary).TryGetValue(((object)id).GetType(), ref *(ConcurrentDictionary<System.Enum, System.Type>*)(&val)) && ((ConcurrentDictionary<System.Enum, System.Enum>)(object)val).TryGetValue(id, ref *(System.Enum*)(&result)))
			{
				return result;
			}
			throw new System.Exception($"Image was not registered: {((object)id).GetType().FullName}.{id}");
		}
	}
	internal class ThreadSafeRandom
	{
		private readonly Random RNG;

		public ThreadSafeRandom()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			RNG = new Random();
		}

		public ThreadSafeRandom(int seed)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			RNG = new Random(seed);
		}

		public int Next()
		{
			lock (RNG)
			{
				return RNG.Next();
			}
		}

		public int Next(int maxValue)
		{
			lock (RNG)
			{
				return RNG.Next(maxValue);
			}
		}

		public int Next(int minValue, int maxValue)
		{
			lock (RNG)
			{
				return RNG.Next(minValue, maxValue);
			}
		}

		public void NextBytes(byte[] buffer)
		{
			lock (RNG)
			{
				RNG.NextBytes(buffer);
			}
		}

		public double NextDouble()
		{
			lock (RNG)
			{
				return RNG.NextDouble();
			}
		}
	}
	public static class ThreadLocalRandom
	{
		private static readonly ThreadSafeRandom globalRandom = new ThreadSafeRandom();

		private static readonly ThreadLocal<Random> threadRandom = new ThreadLocal<Random>((Func<Random>)RandomFactory);

		public static Random Instance => threadRandom.Value;

		private static Random RandomFactory()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Expected O, but got Unknown
			return new Random(globalRandom.Next());
		}

		public static int Next()
		{
			return Instance.Next();
		}

		public static int Next(int maxValue)
		{
			return Instance.Next(maxValue);
		}

		public static int Next(int minValue, int maxValue)
		{
			return Instance.Next(minValue, maxValue);
		}

		public static double NextDouble()
		{
			return Instance.NextDouble();
		}

		public static void NextBytes(byte[] buffer)
		{
			Instance.NextBytes(buffer);
		}
	}
	public class ResourcePool : Disposable
	{
		public interface IObject
		{
			bool IsMemberOfPool { get; }

			bool IsRetained { get; }

			void Retain();

			void ReturnToPool();
		}

		public abstract class Object : IObject
		{
			internal ResourcePool Pool;

			private int RetainCount;

			public bool IsMemberOfPool => Pool != null;

			public bool IsRetained
			{
				get
				{
					if (IsMemberOfPool)
					{
						return RetainCount > 0;
					}
					return false;
				}
			}

			public void Retain()
			{
				if (IsMemberOfPool && Interlocked.Increment(ref RetainCount) == 1)
				{
					Pool.InUseObjects.Add(this);
				}
			}

			public void ReturnToPool()
			{
				if (IsRetained && Interlocked.Decrement(ref RetainCount) == 0)
				{
					Pool.InUseObjects.Remove(this);
					ResetPoolObjectState();
					Pool.FreeObjects?.Enqueue(this);
				}
			}

			protected abstract void ResetPoolObjectState();
		}

		public readonly string Name;

		protected ConcurrentQueue<Object> FreeObjects = new ConcurrentQueue<Object>();

		protected ConcurrentHashSet<Object> InUseObjects = new ConcurrentHashSet<Object>();

		[field: CompilerGenerated]
		public int Capacity
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			internal set;
		}

		[field: CompilerGenerated]
		public bool Verbose
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		public int NumObjectsCreated => NumObjectsAvailable + NumObjectsInUse;

		public int NumObjectsAvailable
		{
			get
			{
				ConcurrentQueue<Object> freeObjects = FreeObjects;
				if (freeObjects == null)
				{
					return 0;
				}
				return Enumerable.Count<Object>((System.Collections.Generic.IEnumerable<Object>)freeObjects);
			}
		}

		public int NumObjectsInUse => InUseObjects.Count;

		protected ResourcePool(string debug_name)
			: this(debug_name, 0)
		{
		}

		protected ResourcePool(string debug_name, int max_capacity)
		{
			Name = debug_name;
			Capacity = max_capacity;
		}

		public override void Dispose(bool disposing)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			if (disposing)
			{
				if (NumObjectsInUse != 0)
				{
					throw new InvalidOperationException("Cannot dispose the resource pool while one or more pooled items are in use");
				}
				FreeObjects = null;
			}
		}
	}
	public class ResourcePool<T> : ResourcePool where T : ResourcePool.Object, new()
	{
		private static readonly ResourcePool<T> Instance = new ResourcePool<T>();

		public new static int Capacity
		{
			get
			{
				return ((ResourcePool)Instance).Capacity;
			}
			set
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				if (value < ((ResourcePool)Instance).Capacity)
				{
					throw new ArgumentException("ResourcePool.Capacity cannot be decreased");
				}
				((ResourcePool)Instance).Capacity = value;
			}
		}

		public new static bool Verbose
		{
			get
			{
				return ((ResourcePool)Instance).Verbose;
			}
			set
			{
				((ResourcePool)Instance).Verbose = value;
			}
		}

		public static T GetObject()
		{
			return Instance.Get();
		}

		public ResourcePool()
			: base($"ResourcePool<{typeof(T)}>")
		{
		}

		public ResourcePool(int max_capacity)
			: base($"ResourcePool<{typeof(T)}>", max_capacity)
		{
		}

		public T Get()
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			T val = null;
			Object obj = null;
			ConcurrentQueue<Object> freeObjects = FreeObjects;
			if (freeObjects != null && freeObjects.TryDequeue(ref obj))
			{
				val = obj as T;
			}
			if (val == null)
			{
				if (base.IsDisposed)
				{
					throw new InvalidOperationException("<" + Name + "> cannot Get() an object from a disposed ResourcePool");
				}
				if (base.Capacity > 0 && base.NumObjectsCreated >= base.Capacity)
				{
					throw new InvalidOperationException($"<{Name}> maximum limit of {base.Capacity} objects reached, cannot create any new objects");
				}
				val = new T
				{
					Pool = this
				};
			}
			val.Retain();
			return val;
		}
	}
	public class Statistics
	{
		private double Sum;

		private double SumSquared;

		private double mMean;

		private double mVariance;

		private double mStdev;

		private int CalcSamples;

		[field: CompilerGenerated]
		public int NumSamples
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public double Min
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public double Max
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public double Mean
		{
			get
			{
				Calculate();
				return mMean;
			}
		}

		public double Variance
		{
			get
			{
				Calculate();
				return mVariance;
			}
		}

		public double Stdev
		{
			get
			{
				Calculate();
				return mStdev;
			}
		}

		public void Reset()
		{
			NumSamples = (CalcSamples = 0);
			Min = 0.0;
			Max = 0.0;
			Sum = 0.0;
			SumSquared = 0.0;
			mMean = 0.0;
			mVariance = 0.0;
			mStdev = 0.0;
		}

		public void AddSample(double value)
		{
			if (NumSamples++ == 0)
			{
				double min = (Max = (mMean = value));
				Min = min;
			}
			if (value < Min)
			{
				Min = value;
			}
			if (value > Max)
			{
				Max = value;
			}
			Sum += value;
			SumSquared += value * value;
		}

		private void Calculate()
		{
			int numSamples = NumSamples;
			if (CalcSamples != numSamples)
			{
				CalcSamples = numSamples;
				mMean = Sum / (double)numSamples;
				mVariance = SumSquared / (double)numSamples - mMean * mMean;
				mStdev = Math.Sqrt(mVariance);
			}
		}

		public virtual string ToString()
		{
			return $"samples={NumSamples}, min={Min}, max={Max}, mean={Mean}, variance={Variance}, stdev={Stdev}";
		}
	}
	public class Timer
	{
		private static readonly long SysStartTicks;

		private static readonly double ScaleFactor_sec;

		private long mStartTicks;

		private long mPausedTicks;

		[field: CompilerGenerated]
		public static ulong ClockFrequency_hz
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public static long RawTicks => FreeRunningCounter.Ticks - SysStartTicks;

		public static double RawTicks_sec => (double)RawTicks * ScaleFactor_sec;

		private long StartTicks
		{
			get
			{
				return Interlocked.Read(ref mStartTicks);
			}
			set
			{
				Interlocked.Exchange(ref mStartTicks, value);
			}
		}

		private long PausedTicks
		{
			get
			{
				return Interlocked.Read(ref mPausedTicks);
			}
			set
			{
				Interlocked.Exchange(ref mPausedTicks, value);
			}
		}

		[field: CompilerGenerated]
		public bool IsRunning
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		private long Ticks
		{
			get
			{
				if (!IsRunning)
				{
					return PausedTicks;
				}
				return RawTicks;
			}
		}

		private long ElapsedTicks
		{
			get
			{
				return Ticks - StartTicks;
			}
			set
			{
				StartTicks = Ticks - value;
			}
		}

		public double ElapsedTime_sec
		{
			get
			{
				return (double)ElapsedTicks * ScaleFactor_sec;
			}
			set
			{
				ElapsedTicks = (long)(value * (double)ClockFrequency_hz);
			}
		}

		public TimeSpan ElapsedTime
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				return TimeSpan.FromSeconds(ElapsedTime_sec);
			}
			set
			{
				ElapsedTime_sec = ((TimeSpan)(ref value)).TotalSeconds;
			}
		}

		static Timer()
		{
			try
			{
				SysStartTicks = FreeRunningCounter.Ticks;
				ClockFrequency_hz = FreeRunningCounter.ClockFrequency_hz;
				ScaleFactor_sec = 1.0 / (double)ClockFrequency_hz;
			}
			catch
			{
				throw new System.Exception("Failed to access hardware clock, was one registered to IDS.FreeRunningCounter.Initialize() ?");
			}
		}

		public Timer(bool start = true)
		{
			StartTicks = RawTicks;
			if (start)
			{
				Reset();
			}
		}

		public void Reset()
		{
			StartTicks = RawTicks;
			IsRunning = true;
		}

		public void Start()
		{
			if (!IsRunning)
			{
				StartTicks = RawTicks - ElapsedTicks;
				IsRunning = true;
			}
		}

		public void Stop()
		{
			PausedTicks = Ticks;
			IsRunning = false;
		}

		public TimeSpan GetElapsedTimeAndReset()
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			long rawTicks = RawTicks;
			double num = (double)((IsRunning ? rawTicks : PausedTicks) - StartTicks) * ScaleFactor_sec;
			StartTicks = rawTicks;
			IsRunning = true;
			return TimeSpan.FromSeconds(num);
		}

		public virtual string ToString()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return FormatString(ElapsedTime);
		}

		public static string FormatString(TimeSpan span)
		{
			int num = (int)((TimeSpan)(ref span)).TotalSeconds;
			int num2 = num / 86400;
			num %= 86400;
			int num3 = num / 3600;
			num %= 3600;
			int num4 = num / 60;
			num %= 60;
			if (num2 <= 0)
			{
				if (num3 <= 0)
				{
					return $"{num4}:{num.ToString("00")}";
				}
				return $"{num3}:{num4.ToString("00")}:{num.ToString("00")}";
			}
			return $"{num2}:{num3.ToString("00")}:{num4.ToString("00")}:{num.ToString("00")}";
		}
	}
	public interface ITreeNode : IDisposable, System.IDisposable
	{
		string Text { get; set; }

		System.Enum Icon { get; set; }

		object Data { get; set; }

		ITreeNode Parent { get; }

		System.Collections.Generic.IEnumerable<ITreeNode> Children { get; }

		bool HasChildren { get; }

		int NumChildren { get; }

		bool IsSelected { get; set; }

		bool IsExpanded { get; set; }

		void AddChild(ITreeNode node);

		void RemoveChild(ITreeNode node);
	}
	public class TreeNode : Disposable, ITreeNode, IDisposable, System.IDisposable
	{
		public delegate ITreeNode TreeNodeFactory();

		public static TreeNodeFactory Factory = null;

		public static TreeNodeFactory GenericFactory = CreateGenericNode;

		private TreeNode mParent;

		private List<ITreeNode> mChildren = new List<ITreeNode>();

		[field: CompilerGenerated]
		public object Data
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public string Text
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public System.Enum Icon
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		public ITreeNode Parent => mParent;

		public System.Collections.Generic.IEnumerable<ITreeNode> Children => (System.Collections.Generic.IEnumerable<ITreeNode>)mChildren;

		[field: CompilerGenerated]
		public bool IsSelected
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		[field: CompilerGenerated]
		public bool IsExpanded
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		public bool HasChildren => mChildren.Count > 0;

		public int NumChildren => mChildren.Count;

		private static ITreeNode CreateGenericNode()
		{
			return new TreeNode();
		}

		public static ITreeNode Create()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			if (Factory == null)
			{
				throw new InvalidOperationException("Failed to create an IDS.Core.TreeNode as no IDS.Core.TreeNodeFactory has been registered with the Core.  A Factory must be registered during program initialization.  The Core provides a generic factory that can be used at IDS.Core.TreeNode.GenericFactory.");
			}
			return Factory();
		}

		public static ITreeNode Create(object data)
		{
			ITreeNode treeNode = Create();
			treeNode.Data = data;
			return treeNode;
		}

		private TreeNode()
		{
		}

		public void AddChild(ITreeNode node)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			if (base.IsDisposed)
			{
				return;
			}
			if (node == this)
			{
				throw new InvalidOperationException("ITreeNode.Add() a node cannot be added to itself");
			}
			if (node.Parent != this)
			{
				node.Parent?.RemoveChild(node);
				if (this.GetRootNode() == node)
				{
					throw new InvalidOperationException("ITreeNode.Add() parent node cannot be added as a child node");
				}
				if (node is TreeNode treeNode)
				{
					treeNode.mParent = this;
				}
				mChildren?.Add(node);
			}
		}

		public void RemoveChild(ITreeNode node)
		{
			if (node.Parent == this)
			{
				if (node is TreeNode treeNode)
				{
					treeNode.mParent = null;
				}
				mChildren?.Remove(node);
			}
		}

		public override void Dispose(bool disposing)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			if (!disposing)
			{
				return;
			}
			List<ITreeNode> val = mChildren;
			mChildren = null;
			Parent?.RemoveChild(this);
			mParent = null;
			if (val == null)
			{
				return;
			}
			Enumerator<ITreeNode> enumerator = val.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					((System.IDisposable)enumerator.Current)?.Dispose();
				}
			}
			finally
			{
				((System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
			}
			val.Clear();
		}
	}
}
namespace IDS.Core.Types
{
	public struct Int8
	{
		private sbyte Value;

		public bool Bit0
		{
			get
			{
				return (Value & 1) != 0;
			}
			set
			{
				if (value)
				{
					Value |= 1;
				}
				else
				{
					Value &= -2;
				}
			}
		}

		public bool Bit1
		{
			get
			{
				return (Value & 2) != 0;
			}
			set
			{
				if (value)
				{
					Value |= 2;
				}
				else
				{
					Value &= -3;
				}
			}
		}

		public bool Bit2
		{
			get
			{
				return (Value & 4) != 0;
			}
			set
			{
				if (value)
				{
					Value |= 4;
				}
				else
				{
					Value &= -5;
				}
			}
		}

		public bool Bit3
		{
			get
			{
				return (Value & 8) != 0;
			}
			set
			{
				if (value)
				{
					Value |= 8;
				}
				else
				{
					Value &= -9;
				}
			}
		}

		public bool Bit4
		{
			get
			{
				return (Value & 0x10) != 0;
			}
			set
			{
				if (value)
				{
					Value |= 16;
				}
				else
				{
					Value &= -17;
				}
			}
		}

		public bool Bit5
		{
			get
			{
				return (Value & 0x20) != 0;
			}
			set
			{
				if (value)
				{
					Value |= 32;
				}
				else
				{
					Value &= -33;
				}
			}
		}

		public bool Bit6
		{
			get
			{
				return (Value & 0x40) != 0;
			}
			set
			{
				if (value)
				{
					Value |= 64;
				}
				else
				{
					Value &= -65;
				}
			}
		}

		public bool Bit7
		{
			get
			{
				return (Value & 0x80) != 0;
			}
			set
			{
				if (value)
				{
					Value |= -128;
				}
				else
				{
					Value &= 127;
				}
			}
		}

		private Int8(sbyte value)
		{
			Value = value;
		}

		public static implicit operator Int8(sbyte a)
		{
			return new Int8(a);
		}

		public static implicit operator sbyte(Int8 a)
		{
			return a.Value;
		}
	}
	public struct UInt8
	{
		private byte Value;

		public bool Bit0
		{
			get
			{
				return (Value & 1) != 0;
			}
			set
			{
				if (value)
				{
					Value |= 1;
				}
				else
				{
					Value &= 254;
				}
			}
		}

		public bool Bit1
		{
			get
			{
				return (Value & 2) != 0;
			}
			set
			{
				if (value)
				{
					Value |= 2;
				}
				else
				{
					Value &= 253;
				}
			}
		}

		public bool Bit2
		{
			get
			{
				return (Value & 4) != 0;
			}
			set
			{
				if (value)
				{
					Value |= 4;
				}
				else
				{
					Value &= 251;
				}
			}
		}

		public bool Bit3
		{
			get
			{
				return (Value & 8) != 0;
			}
			set
			{
				if (value)
				{
					Value |= 8;
				}
				else
				{
					Value &= 247;
				}
			}
		}

		public bool Bit4
		{
			get
			{
				return (Value & 0x10) != 0;
			}
			set
			{
				if (value)
				{
					Value |= 16;
				}
				else
				{
					Value &= 239;
				}
			}
		}

		public bool Bit5
		{
			get
			{
				return (Value & 0x20) != 0;
			}
			set
			{
				if (value)
				{
					Value |= 32;
				}
				else
				{
					Value &= 223;
				}
			}
		}

		public bool Bit6
		{
			get
			{
				return (Value & 0x40) != 0;
			}
			set
			{
				if (value)
				{
					Value |= 64;
				}
				else
				{
					Value &= 191;
				}
			}
		}

		public bool Bit7
		{
			get
			{
				return (Value & 0x80) != 0;
			}
			set
			{
				if (value)
				{
					Value |= 128;
				}
				else
				{
					Value &= 127;
				}
			}
		}

		private UInt8(byte value)
		{
			Value = value;
		}

		public static implicit operator UInt8(byte a)
		{
			return new UInt8(a);
		}

		public static implicit operator byte(UInt8 a)
		{
			return a.Value;
		}
	}
	public struct Int24 : IComparable<Int24>, IEquatable<Int24>, System.IFormattable
	{
		public static readonly Int24 Zero = (Int24)0;

		public static readonly Int24 MinValue = (Int24)(-8388608);

		public static readonly Int24 MaxValue = (Int24)8388607;

		private const int SIGN = 8388608;

		private const int SIGN_EXTEND = -8388608;

		private readonly int Value;

		private Int24(int value)
		{
			if ((value & 0x800000) != 0)
			{
				Value = value | -8388608;
			}
			else
			{
				Value = value & MaxValue.Value;
			}
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public static Int24 Parse(string s)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			if (!TryParse(s, out var result))
			{
				throw new FormatException();
			}
			return result;
		}

		public static Int24 Parse(string s, NumberStyles style)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return Parse(s, style, (IFormatProvider)(object)NumberFormatInfo.CurrentInfo);
		}

		public static Int24 Parse(string s, IFormatProvider provider)
		{
			return Parse(s, (NumberStyles)7, provider);
		}

		public static Int24 Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			if (!TryParse(s, style, provider, out var result))
			{
				throw new FormatException();
			}
			return result;
		}

		public static bool TryParse(string s, out Int24 result)
		{
			return TryParse(s, (NumberStyles)7, (IFormatProvider)(object)NumberFormatInfo.CurrentInfo, out result);
		}

		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out Int24 result)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			int value = default(int);
			if (!int.TryParse(s, style, provider, ref value))
			{
				result = Zero;
				return false;
			}
			result = new Int24(value);
			return true;
		}

		public int CompareTo(Int24 other)
		{
			return Value.CompareTo(other.Value);
		}

		public bool Equals(Int24 other)
		{
			return Value == other.Value;
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		public string ToString(string format)
		{
			return Value.ToString(format);
		}

		public string ToString(IFormatProvider provider)
		{
			return Value.ToString((string)null, provider);
		}

		public string ToString(string format, IFormatProvider provider)
		{
			return Value.ToString(format, provider);
		}

		public static implicit operator Int24(sbyte a)
		{
			return new Int24(a);
		}

		public static implicit operator Int24(byte a)
		{
			return new Int24(a);
		}

		public static implicit operator Int24(short a)
		{
			return new Int24(a);
		}

		public static implicit operator Int24(ushort a)
		{
			return new Int24(a);
		}

		public static explicit operator Int24(int a)
		{
			return new Int24(a);
		}

		public static explicit operator Int24(Int40 a)
		{
			return new Int24((int)a);
		}

		public static explicit operator Int24(Int48 a)
		{
			return new Int24((int)a);
		}

		public static explicit operator Int24(Int56 a)
		{
			return new Int24((int)a);
		}

		public static explicit operator Int24(long a)
		{
			return new Int24((int)a);
		}

		public static explicit operator Int24(UInt24 a)
		{
			return new Int24(a);
		}

		public static explicit operator Int24(uint a)
		{
			return new Int24((int)a);
		}

		public static explicit operator Int24(UInt40 a)
		{
			return new Int24((int)a);
		}

		public static explicit operator Int24(UInt48 a)
		{
			return new Int24((int)a);
		}

		public static explicit operator Int24(UInt56 a)
		{
			return new Int24((int)a);
		}

		public static explicit operator Int24(ulong a)
		{
			return new Int24((int)a);
		}

		public static explicit operator Int24(float a)
		{
			return new Int24((int)a);
		}

		public static explicit operator Int24(double a)
		{
			return new Int24((int)a);
		}

		public static explicit operator Int24(decimal a)
		{
			return new Int24((int)a);
		}

		public static explicit operator Int24(BigInteger a)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return new Int24((int)a);
		}

		public static implicit operator int(Int24 a)
		{
			return a.Value;
		}

		public static implicit operator Int40(Int24 a)
		{
			return a.Value;
		}

		public static implicit operator Int48(Int24 a)
		{
			return a.Value;
		}

		public static implicit operator Int56(Int24 a)
		{
			return a.Value;
		}

		public static implicit operator long(Int24 a)
		{
			return a.Value;
		}

		public static implicit operator float(Int24 a)
		{
			return a.Value;
		}

		public static implicit operator double(Int24 a)
		{
			return a.Value;
		}

		public static implicit operator decimal(Int24 a)
		{
			return decimal.op_Implicit(a.Value);
		}

		public static implicit operator BigInteger(Int24 a)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return BigInteger.op_Implicit(a.Value);
		}

		public static explicit operator sbyte(Int24 a)
		{
			return (sbyte)a.Value;
		}

		public static explicit operator short(Int24 a)
		{
			return (short)a.Value;
		}

		public static explicit operator byte(Int24 a)
		{
			return (byte)a.Value;
		}

		public static explicit operator ushort(Int24 a)
		{
			return (ushort)a.Value;
		}

		public static explicit operator UInt24(Int24 a)
		{
			return (UInt24)a.Value;
		}

		public static explicit operator uint(Int24 a)
		{
			return (uint)a.Value;
		}

		public static explicit operator UInt40(Int24 a)
		{
			return (UInt40)a.Value;
		}

		public static explicit operator UInt48(Int24 a)
		{
			return (UInt48)a.Value;
		}

		public static explicit operator UInt56(Int24 a)
		{
			return (UInt56)a.Value;
		}

		public static explicit operator ulong(Int24 a)
		{
			return (ulong)a.Value;
		}

		public static explicit operator UInt128(Int24 a)
		{
			return (UInt128)a.Value;
		}

		public static Int24 operator <<(Int24 a, int b)
		{
			return new Int24(a.Value << b);
		}

		public static Int24 operator >>(Int24 a, int b)
		{
			return new Int24(a.Value >> b);
		}

		public static Int24 operator &(Int24 a, Int24 b)
		{
			return new Int24(a.Value & b.Value);
		}

		public static Int24 operator |(Int24 a, Int24 b)
		{
			return new Int24(a.Value | b.Value);
		}

		public static Int24 operator ^(Int24 a, Int24 b)
		{
			return new Int24(a.Value ^ b.Value);
		}

		public static Int24 operator ~(Int24 a)
		{
			return new Int24(~a.Value);
		}

		public static Int24 operator +(Int24 a)
		{
			return a;
		}

		public static Int24 operator -(Int24 a)
		{
			return new Int24(-a.Value);
		}

		public static Int24 operator +(Int24 a, Int24 b)
		{
			return new Int24(a.Value + b.Value);
		}

		public static Int24 operator ++(Int24 a)
		{
			return new Int24(a.Value + 1);
		}

		public static Int24 operator -(Int24 a, Int24 b)
		{
			return new Int24(a.Value - b.Value);
		}

		public static Int24 operator --(Int24 a)
		{
			return new Int24(a.Value - 1);
		}

		public static Int24 operator *(Int24 a, Int24 b)
		{
			return new Int24(a.Value * b.Value);
		}

		public static Int24 operator /(Int24 a, Int24 b)
		{
			return new Int24(a.Value / b.Value);
		}

		public static Int24 operator %(Int24 a, Int24 b)
		{
			return new Int24(a.Value % b.Value);
		}
	}
	public struct UInt24 : IComparable<UInt24>, IEquatable<UInt24>, System.IFormattable
	{
		public static readonly UInt24 Zero = (byte)0;

		public static readonly UInt24 MinValue = Zero;

		public static readonly UInt24 MaxValue = ~MinValue;

		private const uint MASK = 16777215u;

		private readonly uint Value;

		private UInt24(uint value)
		{
			Value = value & 0xFFFFFF;
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public static UInt24 Parse(string s)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			if (!TryParse(s, out var result))
			{
				throw new FormatException();
			}
			return result;
		}

		public static UInt24 Parse(string s, NumberStyles style)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return Parse(s, style, (IFormatProvider)(object)NumberFormatInfo.CurrentInfo);
		}

		public static UInt24 Parse(string s, IFormatProvider provider)
		{
			return Parse(s, (NumberStyles)7, provider);
		}

		public static UInt24 Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			if (!TryParse(s, style, provider, out var result))
			{
				throw new FormatException();
			}
			return result;
		}

		public static bool TryParse(string s, out UInt24 result)
		{
			return TryParse(s, (NumberStyles)7, (IFormatProvider)(object)NumberFormatInfo.CurrentInfo, out result);
		}

		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out UInt24 result)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			uint value = default(uint);
			if (!uint.TryParse(s, style, provider, ref value))
			{
				result = Zero;
				return false;
			}
			result = new UInt24(value);
			return true;
		}

		public int CompareTo(UInt24 other)
		{
			return Value.CompareTo(other.Value);
		}

		public bool Equals(UInt24 other)
		{
			return Value == other.Value;
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		public string ToString(string format)
		{
			return Value.ToString(format);
		}

		public string ToString(IFormatProvider provider)
		{
			return Value.ToString((string)null, provider);
		}

		public string ToString(string format, IFormatProvider provider)
		{
			return Value.ToString(format, provider);
		}

		public static implicit operator UInt24(byte a)
		{
			return new UInt24(a);
		}

		public static implicit operator UInt24(ushort a)
		{
			return new UInt24(a);
		}

		public static explicit operator UInt24(sbyte a)
		{
			return new UInt24((uint)a);
		}

		public static explicit operator UInt24(short a)
		{
			return new UInt24((uint)a);
		}

		public static explicit operator UInt24(Int24 a)
		{
			return new UInt24((uint)a);
		}

		public static explicit operator UInt24(int a)
		{
			return new UInt24((uint)a);
		}

		public static explicit operator UInt24(Int40 a)
		{
			return new UInt24((uint)a);
		}

		public static explicit operator UInt24(Int48 a)
		{
			return new UInt24((uint)a);
		}

		public static explicit operator UInt24(Int56 a)
		{
			return new UInt24((uint)a);
		}

		public static explicit operator UInt24(long a)
		{
			return new UInt24((uint)a);
		}

		public static explicit operator UInt24(uint a)
		{
			return new UInt24(a);
		}

		public static explicit operator UInt24(UInt40 a)
		{
			return new UInt24((uint)a);
		}

		public static explicit operator UInt24(UInt48 a)
		{
			return new UInt24((uint)a);
		}

		public static explicit operator UInt24(UInt56 a)
		{
			return new UInt24((uint)a);
		}

		public static explicit operator UInt24(ulong a)
		{
			return new UInt24((uint)a);
		}

		public static explicit operator UInt24(float a)
		{
			return new UInt24((uint)a);
		}

		public static explicit operator UInt24(double a)
		{
			return new UInt24((uint)a);
		}

		public static explicit operator UInt24(decimal a)
		{
			return new UInt24((uint)a);
		}

		public static explicit operator UInt24(BigInteger a)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return new UInt24((uint)a);
		}

		public static implicit operator int(UInt24 a)
		{
			return (int)a.Value;
		}

		public static implicit operator uint(UInt24 a)
		{
			return a.Value;
		}

		public static implicit operator Int40(UInt24 a)
		{
			return a.Value;
		}

		public static implicit operator UInt40(UInt24 a)
		{
			return a.Value;
		}

		public static implicit operator Int48(UInt24 a)
		{
			return a.Value;
		}

		public static implicit operator UInt48(UInt24 a)
		{
			return a.Value;
		}

		public static implicit operator Int56(UInt24 a)
		{
			return a.Value;
		}

		public static implicit operator UInt56(UInt24 a)
		{
			return a.Value;
		}

		public static implicit operator long(UInt24 a)
		{
			return a.Value;
		}

		public static implicit operator ulong(UInt24 a)
		{
			return a.Value;
		}

		public static implicit operator UInt128(UInt24 a)
		{
			return a.Value;
		}

		public static implicit operator float(UInt24 a)
		{
			return a.Value;
		}

		public static implicit operator double(UInt24 a)
		{
			return a.Value;
		}

		public static implicit operator decimal(UInt24 a)
		{
			return decimal.op_Implicit(a.Value);
		}

		public static implicit operator BigInteger(UInt24 a)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return BigInteger.op_Implicit(a.Value);
		}

		public static explicit operator sbyte(UInt24 a)
		{
			return (sbyte)a.Value;
		}

		public static explicit operator short(UInt24 a)
		{
			return (short)a.Value;
		}

		public static explicit operator Int24(UInt24 a)
		{
			return (Int24)a.Value;
		}

		public static explicit operator byte(UInt24 a)
		{
			return (byte)a.Value;
		}

		public static explicit operator ushort(UInt24 a)
		{
			return (ushort)a.Value;
		}

		public static UInt24 operator <<(UInt24 a, int b)
		{
			return new UInt24(a.Value << b);
		}

		public static UInt24 operator >>(UInt24 a, int b)
		{
			return new UInt24(a.Value >> b);
		}

		public static UInt24 operator &(UInt24 a, UInt24 b)
		{
			return new UInt24(a.Value & b.Value);
		}

		public static UInt24 operator |(UInt24 a, UInt24 b)
		{
			return new UInt24(a.Value | b.Value);
		}

		public static UInt24 operator ^(UInt24 a, UInt24 b)
		{
			return new UInt24(a.Value ^ b.Value);
		}

		public static UInt24 operator ~(UInt24 a)
		{
			return new UInt24(~a.Value);
		}

		public static UInt24 operator +(UInt24 a)
		{
			return a;
		}

		public static UInt24 operator -(UInt24 a)
		{
			return new UInt24((uint)(0uL - (ulong)a.Value));
		}

		public static UInt24 operator +(UInt24 a, UInt24 b)
		{
			return new UInt24(a.Value + b.Value);
		}

		public static UInt24 operator ++(UInt24 a)
		{
			return new UInt24(a.Value + 1);
		}

		public static UInt24 operator -(UInt24 a, UInt24 b)
		{
			return new UInt24(a.Value - b.Value);
		}

		public static UInt24 operator --(UInt24 a)
		{
			return new UInt24(a.Value - 1);
		}

		public static UInt24 operator *(UInt24 a, UInt24 b)
		{
			return new UInt24(a.Value * b.Value);
		}

		public static UInt24 operator /(UInt24 a, UInt24 b)
		{
			return new UInt24(a.Value / b.Value);
		}

		public static UInt24 operator %(UInt24 a, UInt24 b)
		{
			return new UInt24(a.Value % b.Value);
		}
	}
	public struct Int40 : IComparable<Int40>, IEquatable<Int40>, System.IFormattable
	{
		public static readonly Int40 Zero = 0;

		public static readonly Int40 MinValue = (Int40)(-549755813888L);

		public static readonly Int40 MaxValue = (Int40)549755813887L;

		private const long SIGN = 549755813888L;

		private const long SIGN_EXTEND = -549755813888L;

		private readonly long Value;

		private Int40(long value)
		{
			if ((value & 0x8000000000L) != 0L)
			{
				Value = value | -549755813888L;
			}
			else
			{
				Value = value & MaxValue.Value;
			}
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public static Int40 Parse(string s)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			if (!TryParse(s, out var result))
			{
				throw new FormatException();
			}
			return result;
		}

		public static Int40 Parse(string s, NumberStyles style)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return Parse(s, style, (IFormatProvider)(object)NumberFormatInfo.CurrentInfo);
		}

		public static Int40 Parse(string s, IFormatProvider provider)
		{
			return Parse(s, (NumberStyles)7, provider);
		}

		public static Int40 Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			if (!TryParse(s, style, provider, out var result))
			{
				throw new FormatException();
			}
			return result;
		}

		public static bool TryParse(string s, out Int40 result)
		{
			return TryParse(s, (NumberStyles)7, (IFormatProvider)(object)NumberFormatInfo.CurrentInfo, out result);
		}

		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out Int40 result)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			long value = default(long);
			if (!long.TryParse(s, style, provider, ref value))
			{
				result = Zero;
				return false;
			}
			result = new Int40(value);
			return true;
		}

		public int CompareTo(Int40 other)
		{
			return Value.CompareTo(other.Value);
		}

		public bool Equals(Int40 other)
		{
			return Value == other.Value;
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		public string ToString(string format)
		{
			return Value.ToString(format);
		}

		public string ToString(IFormatProvider provider)
		{
			return Value.ToString((string)null, provider);
		}

		public string ToString(string format, IFormatProvider provider)
		{
			return Value.ToString(format, provider);
		}

		public static implicit operator Int40(sbyte a)
		{
			return new Int40(a);
		}

		public static implicit operator Int40(byte a)
		{
			return new Int40(a);
		}

		public static implicit operator Int40(short a)
		{
			return new Int40(a);
		}

		public static implicit operator Int40(ushort a)
		{
			return new Int40(a);
		}

		public static implicit operator Int40(Int24 a)
		{
			return new Int40(a);
		}

		public static implicit operator Int40(UInt24 a)
		{
			return new Int40(a);
		}

		public static implicit operator Int40(int a)
		{
			return new Int40(a);
		}

		public static implicit operator Int40(uint a)
		{
			return new Int40(a);
		}

		public static explicit operator Int40(Int48 a)
		{
			return new Int40(a);
		}

		public static explicit operator Int40(Int56 a)
		{
			return new Int40(a);
		}

		public static explicit operator Int40(long a)
		{
			return new Int40(a);
		}

		public static explicit operator Int40(UInt40 a)
		{
			return new Int40(a);
		}

		public static explicit operator Int40(UInt48 a)
		{
			return new Int40(a);
		}

		public static explicit operator Int40(UInt56 a)
		{
			return new Int40(a);
		}

		public static explicit operator Int40(ulong a)
		{
			return new Int40((long)a);
		}

		public static explicit operator Int40(float a)
		{
			return new Int40((long)a);
		}

		public static explicit operator Int40(double a)
		{
			return new Int40((long)a);
		}

		public static explicit operator Int40(decimal a)
		{
			return new Int40((long)a);
		}

		public static explicit operator Int40(BigInteger a)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return new Int40((long)a);
		}

		public static implicit operator Int48(Int40 a)
		{
			return (Int48)a.Value;
		}

		public static implicit operator Int56(Int40 a)
		{
			return (Int56)a.Value;
		}

		public static implicit operator long(Int40 a)
		{
			return a.Value;
		}

		public static implicit operator float(Int40 a)
		{
			return a.Value;
		}

		public static implicit operator double(Int40 a)
		{
			return a.Value;
		}

		public static implicit operator decimal(Int40 a)
		{
			return decimal.op_Implicit(a.Value);
		}

		public static implicit operator BigInteger(Int40 a)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return BigInteger.op_Implicit(a.Value);
		}

		public static explicit operator sbyte(Int40 a)
		{
			return (sbyte)a.Value;
		}

		public static explicit operator short(Int40 a)
		{
			return (short)a.Value;
		}

		public static explicit operator Int24(Int40 a)
		{
			return (Int24)a.Value;
		}

		public static explicit operator int(Int40 a)
		{
			return (int)a.Value;
		}

		public static explicit operator byte(Int40 a)
		{
			return (byte)a.Value;
		}

		public static explicit operator ushort(Int40 a)
		{
			return (ushort)a.Value;
		}

		public static explicit operator UInt24(Int40 a)
		{
			return (UInt24)a.Value;
		}

		public static explicit operator uint(Int40 a)
		{
			return (uint)a.Value;
		}

		public static explicit operator UInt40(Int40 a)
		{
			return (UInt40)a.Value;
		}

		public static explicit operator UInt48(Int40 a)
		{
			return (UInt48)a.Value;
		}

		public static explicit operator UInt56(Int40 a)
		{
			return (UInt56)a.Value;
		}

		public static explicit operator ulong(Int40 a)
		{
			return (ulong)a.Value;
		}

		public static explicit operator UInt128(Int40 a)
		{
			return (UInt128)a.Value;
		}

		public static Int40 operator <<(Int40 a, int b)
		{
			return new Int40(a.Value << b);
		}

		public static Int40 operator >>(Int40 a, int b)
		{
			return new Int40(a.Value >> b);
		}

		public static Int40 operator &(Int40 a, Int40 b)
		{
			return new Int40(a.Value & b.Value);
		}

		public static Int40 operator |(Int40 a, Int40 b)
		{
			return new Int40(a.Value | b.Value);
		}

		public static Int40 operator ^(Int40 a, Int40 b)
		{
			return new Int40(a.Value ^ b.Value);
		}

		public static Int40 operator ~(Int40 a)
		{
			return new Int40(~a.Value);
		}

		public static Int40 operator +(Int40 a)
		{
			return a;
		}

		public static Int40 operator -(Int40 a)
		{
			return new Int40(-a.Value);
		}

		public static Int40 operator +(Int40 a, Int40 b)
		{
			return new Int40(a.Value + b.Value);
		}

		public static Int40 operator ++(Int40 a)
		{
			return new Int40(a.Value + 1);
		}

		public static Int40 operator -(Int40 a, Int40 b)
		{
			return new Int40(a.Value - b.Value);
		}

		public static Int40 operator --(Int40 a)
		{
			return new Int40(a.Value - 1);
		}

		public static Int40 operator *(Int40 a, Int40 b)
		{
			return new Int40(a.Value * b.Value);
		}

		public static Int40 operator /(Int40 a, Int40 b)
		{
			return new Int40(a.Value / b.Value);
		}

		public static Int40 operator %(Int40 a, Int40 b)
		{
			return new Int40(a.Value % b.Value);
		}
	}
	public struct UInt40 : IComparable<UInt40>, IEquatable<UInt40>, System.IFormattable
	{
		public static readonly UInt40 Zero = (byte)0;

		public static readonly UInt40 MinValue = Zero;

		public static readonly UInt40 MaxValue = ~MinValue;

		private const ulong MASK = 281474976710655uL;

		private readonly ulong Value;

		private UInt40(ulong value)
		{
			Value = value & 0xFFFFFFFFFFFFL;
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public static UInt40 Parse(string s)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			if (!TryParse(s, out var result))
			{
				throw new FormatException();
			}
			return result;
		}

		public static UInt40 Parse(string s, NumberStyles style)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return Parse(s, style, (IFormatProvider)(object)NumberFormatInfo.CurrentInfo);
		}

		public static UInt40 Parse(string s, IFormatProvider provider)
		{
			return Parse(s, (NumberStyles)7, provider);
		}

		public static UInt40 Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			if (!TryParse(s, style, provider, out var result))
			{
				throw new FormatException();
			}
			return result;
		}

		public static bool TryParse(string s, out UInt40 result)
		{
			return TryParse(s, (NumberStyles)7, (IFormatProvider)(object)NumberFormatInfo.CurrentInfo, out result);
		}

		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out UInt40 result)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			ulong value = default(ulong);
			if (!ulong.TryParse(s, style, provider, ref value))
			{
				result = Zero;
				return false;
			}
			result = new UInt40(value);
			return true;
		}

		public int CompareTo(UInt40 other)
		{
			return Value.CompareTo(other.Value);
		}

		public bool Equals(UInt40 other)
		{
			return Value == other.Value;
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		public string ToString(string format)
		{
			return Value.ToString(format);
		}

		public string ToString(IFormatProvider provider)
		{
			return Value.ToString((string)null, provider);
		}

		public string ToString(string format, IFormatProvider provider)
		{
			return Value.ToString(format, provider);
		}

		public static implicit operator UInt40(byte a)
		{
			return new UInt40(a);
		}

		public static implicit operator UInt40(ushort a)
		{
			return new UInt40(a);
		}

		public static implicit operator UInt40(UInt24 a)
		{
			return new UInt40(a);
		}

		public static implicit operator UInt40(uint a)
		{
			return new UInt40(a);
		}

		public static explicit operator UInt40(sbyte a)
		{
			return new UInt40((ulong)a);
		}

		public static explicit operator UInt40(short a)
		{
			return new UInt40((ulong)a);
		}

		public static explicit operator UInt40(Int24 a)
		{
			return new UInt40((ulong)a);
		}

		public static explicit operator UInt40(int a)
		{
			return new UInt40((ulong)a);
		}

		public static explicit operator UInt40(Int40 a)
		{
			return new UInt40((ulong)a);
		}

		public static explicit operator UInt40(Int48 a)
		{
			return new UInt40((ulong)a);
		}

		public static explicit operator UInt40(Int56 a)
		{
			return new UInt40((ulong)a);
		}

		public static explicit operator UInt40(long a)
		{
			return new UInt40((ulong)a);
		}

		public static explicit operator UInt40(UInt48 a)
		{
			return new UInt40(a);
		}

		public static explicit operator UInt40(UInt56 a)
		{
			return new UInt40(a);
		}

		public static explicit operator UInt40(ulong a)
		{
			return new UInt40(a);
		}

		public static explicit operator UInt40(float a)
		{
			return new UInt40((ulong)a);
		}

		public static explicit operator UInt40(double a)
		{
			return new UInt40((ulong)a);
		}

		public static explicit operator UInt40(decimal a)
		{
			return new UInt40((ulong)a);
		}

		public static explicit operator UInt40(BigInteger a)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return new UInt40((ulong)a);
		}

		public static implicit operator Int48(UInt40 a)
		{
			return (Int48)a.Value;
		}

		public static implicit operator UInt48(UInt40 a)
		{
			return (UInt48)a.Value;
		}

		public static implicit operator Int56(UInt40 a)
		{
			return (Int56)a.Value;
		}

		public static implicit operator UInt56(UInt40 a)
		{
			return (UInt56)a.Value;
		}

		public static implicit operator long(UInt40 a)
		{
			return (long)a.Value;
		}

		public static implicit operator ulong(UInt40 a)
		{
			return a.Value;
		}

		public static implicit operator UInt128(UInt40 a)
		{
			return a.Value;
		}

		public static implicit operator float(UInt40 a)
		{
			return a.Value;
		}

		public static implicit operator double(UInt40 a)
		{
			return a.Value;
		}

		public static implicit operator decimal(UInt40 a)
		{
			return decimal.op_Implicit(a.Value);
		}

		public static implicit operator BigInteger(UInt40 a)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return BigInteger.op_Implicit(a.Value);
		}

		public static explicit operator sbyte(UInt40 a)
		{
			return (sbyte)a.Value;
		}

		public static explicit operator short(UInt40 a)
		{
			return (short)a.Value;
		}

		public static explicit operator Int24(UInt40 a)
		{
			return (Int24)a.Value;
		}

		public static explicit operator int(UInt40 a)
		{
			return (int)a.Value;
		}

		public static explicit operator Int40(UInt40 a)
		{
			return (Int40)a.Value;
		}

		public static explicit operator byte(UInt40 a)
		{
			return (byte)a.Value;
		}

		public static explicit operator ushort(UInt40 a)
		{
			return (ushort)a.Value;
		}

		public static explicit operator UInt24(UInt40 a)
		{
			return (UInt24)a.Value;
		}

		public static explicit operator uint(UInt40 a)
		{
			return (uint)a.Value;
		}

		public static UInt40 operator <<(UInt40 a, int b)
		{
			return new UInt40(a.Value << b);
		}

		public static UInt40 operator >>(UInt40 a, int b)
		{
			return new UInt40(a.Value >> b);
		}

		public static UInt40 operator &(UInt40 a, UInt40 b)
		{
			return new UInt40(a.Value & b.Value);
		}

		public static UInt40 operator |(UInt40 a, UInt40 b)
		{
			return new UInt40(a.Value | b.Value);
		}

		public static UInt40 operator ^(UInt40 a, UInt40 b)
		{
			return new UInt40(a.Value ^ b.Value);
		}

		public static UInt40 operator ~(UInt40 a)
		{
			return new UInt40(~a.Value);
		}

		public static UInt40 operator +(UInt40 a)
		{
			return a;
		}

		public static UInt40 operator -(UInt40 a)
		{
			return new UInt40(0L - a.Value);
		}

		public static UInt40 operator +(UInt40 a, UInt40 b)
		{
			return new UInt40(a.Value + b.Value);
		}

		public static UInt40 operator ++(UInt40 a)
		{
			return new UInt40(a.Value + 1);
		}

		public static UInt40 operator -(UInt40 a, UInt40 b)
		{
			return new UInt40(a.Value - b.Value);
		}

		public static UInt40 operator --(UInt40 a)
		{
			return new UInt40(a.Value - 1);
		}

		public static UInt40 operator *(UInt40 a, UInt40 b)
		{
			return new UInt40(a.Value * b.Value);
		}

		public static UInt40 operator /(UInt40 a, UInt40 b)
		{
			return new UInt40(a.Value / b.Value);
		}

		public static UInt40 operator %(UInt40 a, UInt40 b)
		{
			return new UInt40(a.Value % b.Value);
		}
	}
	public struct Int48 : IComparable<Int48>, IEquatable<Int48>, System.IFormattable
	{
		public static readonly Int48 Zero = 0;

		public static readonly Int48 MinValue = (Int48)(-140737488355328L);

		public static readonly Int48 MaxValue = (Int48)140737488355327L;

		private const long SIGN = 140737488355328L;

		private const long SIGN_EXTEND = -140737488355328L;

		private readonly long Value;

		private Int48(long value)
		{
			if ((value & 0x800000000000L) != 0L)
			{
				Value = value | -140737488355328L;
			}
			else
			{
				Value = value & MaxValue.Value;
			}
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public static Int48 Parse(string s)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			if (!TryParse(s, out var result))
			{
				throw new FormatException();
			}
			return result;
		}

		public static Int48 Parse(string s, NumberStyles style)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return Parse(s, style, (IFormatProvider)(object)NumberFormatInfo.CurrentInfo);
		}

		public static Int48 Parse(string s, IFormatProvider provider)
		{
			return Parse(s, (NumberStyles)7, provider);
		}

		public static Int48 Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			if (!TryParse(s, style, provider, out var result))
			{
				throw new FormatException();
			}
			return result;
		}

		public static bool TryParse(string s, out Int48 result)
		{
			return TryParse(s, (NumberStyles)7, (IFormatProvider)(object)NumberFormatInfo.CurrentInfo, out result);
		}

		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out Int48 result)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			long value = default(long);
			if (!long.TryParse(s, style, provider, ref value))
			{
				result = Zero;
				return false;
			}
			result = new Int48(value);
			return true;
		}

		public int CompareTo(Int48 other)
		{
			return Value.CompareTo(other.Value);
		}

		public bool Equals(Int48 other)
		{
			return Value == other.Value;
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		public string ToString(string format)
		{
			return Value.ToString(format);
		}

		public string ToString(IFormatProvider provider)
		{
			return Value.ToString((string)null, provider);
		}

		public string ToString(string format, IFormatProvider provider)
		{
			return Value.ToString(format, provider);
		}

		public static implicit operator Int48(sbyte a)
		{
			return new Int48(a);
		}

		public static implicit operator Int48(byte a)
		{
			return new Int48(a);
		}

		public static implicit operator Int48(short a)
		{
			return new Int48(a);
		}

		public static implicit operator Int48(ushort a)
		{
			return new Int48(a);
		}

		public static implicit operator Int48(Int24 a)
		{
			return new Int48(a);
		}

		public static implicit operator Int48(UInt24 a)
		{
			return new Int48(a);
		}

		public static implicit operator Int48(int a)
		{
			return new Int48(a);
		}

		public static implicit operator Int48(uint a)
		{
			return new Int48(a);
		}

		public static implicit operator Int48(Int40 a)
		{
			return new Int48(a);
		}

		public static implicit operator Int48(UInt40 a)
		{
			return new Int48(a);
		}

		public static explicit operator Int48(Int56 a)
		{
			return new Int48(a);
		}

		public static explicit operator Int48(long a)
		{
			return new Int48(a);
		}

		public static explicit operator Int48(UInt48 a)
		{
			return new Int48(a);
		}

		public static explicit operator Int48(UInt56 a)
		{
			return new Int48(a);
		}

		public static explicit operator Int48(ulong a)
		{
			return new Int48((long)a);
		}

		public static explicit operator Int48(float a)
		{
			return new Int48((long)a);
		}

		public static explicit operator Int48(double a)
		{
			return new Int48((long)a);
		}

		public static explicit operator Int48(decimal a)
		{
			return new Int48((long)a);
		}

		public static explicit operator Int48(BigInteger a)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return new Int48((long)a);
		}

		public static implicit operator Int56(Int48 a)
		{
			return (Int56)a.Value;
		}

		public static implicit operator long(Int48 a)
		{
			return a.Value;
		}

		public static implicit operator float(Int48 a)
		{
			return a.Value;
		}

		public static implicit operator double(Int48 a)
		{
			return a.Value;
		}

		public static implicit operator decimal(Int48 a)
		{
			return decimal.op_Implicit(a.Value);
		}

		public static implicit operator BigInteger(Int48 a)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return BigInteger.op_Implicit(a.Value);
		}

		public static explicit operator sbyte(Int48 a)
		{
			return (sbyte)a.Value;
		}

		public static explicit operator short(Int48 a)
		{
			return (short)a.Value;
		}

		public static explicit operator Int24(Int48 a)
		{
			return (Int24)a.Value;
		}

		public static explicit operator int(Int48 a)
		{
			return (int)a.Value;
		}

		public static explicit operator Int40(Int48 a)
		{
			return (Int40)a.Value;
		}

		public static explicit operator byte(Int48 a)
		{
			return (byte)a.Value;
		}

		public static explicit operator ushort(Int48 a)
		{
			return (ushort)a.Value;
		}

		public static explicit operator UInt24(Int48 a)
		{
			return (UInt24)a.Value;
		}

		public static explicit operator uint(Int48 a)
		{
			return (uint)a.Value;
		}

		public static explicit operator UInt40(Int48 a)
		{
			return (UInt40)a.Value;
		}

		public static explicit operator UInt48(Int48 a)
		{
			return (UInt48)a.Value;
		}

		public static explicit operator UInt56(Int48 a)
		{
			return (UInt56)a.Value;
		}

		public static explicit operator ulong(Int48 a)
		{
			return (ulong)a.Value;
		}

		public static explicit operator UInt128(Int48 a)
		{
			return (UInt128)a.Value;
		}

		public static Int48 operator <<(Int48 a, int b)
		{
			return new Int48(a.Value << b);
		}

		public static Int48 operator >>(Int48 a, int b)
		{
			return new Int48(a.Value >> b);
		}

		public static Int48 operator &(Int48 a, Int48 b)
		{
			return new Int48(a.Value & b.Value);
		}

		public static Int48 operator |(Int48 a, Int48 b)
		{
			return new Int48(a.Value | b.Value);
		}

		public static Int48 operator ^(Int48 a, Int48 b)
		{
			return new Int48(a.Value ^ b.Value);
		}

		public static Int48 operator ~(Int48 a)
		{
			return new Int48(~a.Value);
		}

		public static Int48 operator +(Int48 a)
		{
			return a;
		}

		public static Int48 operator -(Int48 a)
		{
			return new Int48(-a.Value);
		}

		public static Int48 operator +(Int48 a, Int48 b)
		{
			return new Int48(a.Value + b.Value);
		}

		public static Int48 operator ++(Int48 a)
		{
			return new Int48(a.Value + 1);
		}

		public static Int48 operator -(Int48 a, Int48 b)
		{
			return new Int48(a.Value - b.Value);
		}

		public static Int48 operator --(Int48 a)
		{
			return new Int48(a.Value - 1);
		}

		public static Int48 operator *(Int48 a, Int48 b)
		{
			return new Int48(a.Value * b.Value);
		}

		public static Int48 operator /(Int48 a, Int48 b)
		{
			return new Int48(a.Value / b.Value);
		}

		public static Int48 operator %(Int48 a, Int48 b)
		{
			return new Int48(a.Value % b.Value);
		}
	}
	public struct UInt48 : IComparable<UInt48>, IEquatable<UInt48>, System.IFormattable
	{
		public static readonly UInt48 Zero = (byte)0;

		public static readonly UInt48 MinValue = Zero;

		public static readonly UInt48 MaxValue = ~MinValue;

		private const ulong MASK = 281474976710655uL;

		private readonly ulong Value;

		private UInt48(ulong value)
		{
			Value = value & 0xFFFFFFFFFFFFL;
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public static UInt48 Parse(string s)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			if (!TryParse(s, out var result))
			{
				throw new FormatException();
			}
			return result;
		}

		public static UInt48 Parse(string s, NumberStyles style)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return Parse(s, style, (IFormatProvider)(object)NumberFormatInfo.CurrentInfo);
		}

		public static UInt48 Parse(string s, IFormatProvider provider)
		{
			return Parse(s, (NumberStyles)7, provider);
		}

		public static UInt48 Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			if (!TryParse(s, style, provider, out var result))
			{
				throw new FormatException();
			}
			return result;
		}

		public static bool TryParse(string s, out UInt48 result)
		{
			return TryParse(s, (NumberStyles)7, (IFormatProvider)(object)NumberFormatInfo.CurrentInfo, out result);
		}

		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out UInt48 result)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			ulong value = default(ulong);
			if (!ulong.TryParse(s, style, provider, ref value))
			{
				result = Zero;
				return false;
			}
			result = new UInt48(value);
			return true;
		}

		public int CompareTo(UInt48 other)
		{
			return Value.CompareTo(other.Value);
		}

		public bool Equals(UInt48 other)
		{
			return Value == other.Value;
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		public string ToString(string format)
		{
			return Value.ToString(format);
		}

		public string ToString(IFormatProvider provider)
		{
			return Value.ToString((string)null, provider);
		}

		public string ToString(string format, IFormatProvider provider)
		{
			return Value.ToString(format, provider);
		}

		public static implicit operator UInt48(byte a)
		{
			return new UInt48(a);
		}

		public static implicit operator UInt48(ushort a)
		{
			return new UInt48(a);
		}

		public static implicit operator UInt48(UInt24 a)
		{
			return new UInt48(a);
		}

		public static implicit operator UInt48(uint a)
		{
			return new UInt48(a);
		}

		public static implicit operator UInt48(UInt40 a)
		{
			return new UInt48(a);
		}

		public static explicit operator UInt48(sbyte a)
		{
			return new UInt48((ulong)a);
		}

		public static explicit operator UInt48(short a)
		{
			return new UInt48((ulong)a);
		}

		public static explicit operator UInt48(Int24 a)
		{
			return new UInt48((ulong)a);
		}

		public static explicit operator UInt48(int a)
		{
			return new UInt48((ulong)a);
		}

		public static explicit operator UInt48(Int40 a)
		{
			return new UInt48((ulong)a);
		}

		public static explicit operator UInt48(Int48 a)
		{
			return new UInt48((ulong)a);
		}

		public static explicit operator UInt48(Int56 a)
		{
			return new UInt48((ulong)a);
		}

		public static explicit operator UInt48(UInt56 a)
		{
			return new UInt48(a);
		}

		public static explicit operator UInt48(long a)
		{
			return new UInt48((ulong)a);
		}

		public static explicit operator UInt48(ulong a)
		{
			return new UInt48(a);
		}

		public static explicit operator UInt48(float a)
		{
			return new UInt48((ulong)a);
		}

		public static explicit operator UInt48(double a)
		{
			return new UInt48((ulong)a);
		}

		public static explicit operator UInt48(decimal a)
		{
			return new UInt48((ulong)a);
		}

		public static explicit operator UInt48(BigInteger a)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return new UInt48((ulong)a);
		}

		public static implicit operator Int56(UInt48 a)
		{
			return (Int56)a.Value;
		}

		public static implicit operator UInt56(UInt48 a)
		{
			return (UInt56)a.Value;
		}

		public static implicit operator long(UInt48 a)
		{
			return (long)a.Value;
		}

		public static implicit operator ulong(UInt48 a)
		{
			return a.Value;
		}

		public static implicit operator UInt128(UInt48 a)
		{
			return a.Value;
		}

		public static implicit operator float(UInt48 a)
		{
			return a.Value;
		}

		public static implicit operator double(UInt48 a)
		{
			return a.Value;
		}

		public static implicit operator decimal(UInt48 a)
		{
			return decimal.op_Implicit(a.Value);
		}

		public static implicit operator BigInteger(UInt48 a)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return BigInteger.op_Implicit(a.Value);
		}

		public static explicit operator sbyte(UInt48 a)
		{
			return (sbyte)a.Value;
		}

		public static explicit operator short(UInt48 a)
		{
			return (short)a.Value;
		}

		public static explicit operator Int24(UInt48 a)
		{
			return (Int24)a.Value;
		}

		public static explicit operator int(UInt48 a)
		{
			return (int)a.Value;
		}

		public static explicit operator Int40(UInt48 a)
		{
			return (Int40)a.Value;
		}

		public static explicit operator Int48(UInt48 a)
		{
			return (Int48)a.Value;
		}

		public static explicit operator byte(UInt48 a)
		{
			return (byte)a.Value;
		}

		public static explicit operator ushort(UInt48 a)
		{
			return (ushort)a.Value;
		}

		public static explicit operator UInt24(UInt48 a)
		{
			return (UInt24)a.Value;
		}

		public static explicit operator uint(UInt48 a)
		{
			return (uint)a.Value;
		}

		public static explicit operator UInt40(UInt48 a)
		{
			return (UInt40)a.Value;
		}

		public static UInt48 operator <<(UInt48 a, int b)
		{
			return new UInt48(a.Value << b);
		}

		public static UInt48 operator >>(UInt48 a, int b)
		{
			return new UInt48(a.Value >> b);
		}

		public static UInt48 operator &(UInt48 a, UInt48 b)
		{
			return new UInt48(a.Value & b.Value);
		}

		public static UInt48 operator |(UInt48 a, UInt48 b)
		{
			return new UInt48(a.Value | b.Value);
		}

		public static UInt48 operator ^(UInt48 a, UInt48 b)
		{
			return new UInt48(a.Value ^ b.Value);
		}

		public static UInt48 operator ~(UInt48 a)
		{
			return new UInt48(~a.Value);
		}

		public static UInt48 operator +(UInt48 a)
		{
			return a;
		}

		public static UInt48 operator -(UInt48 a)
		{
			return new UInt48(0L - a.Value);
		}

		public static UInt48 operator +(UInt48 a, UInt48 b)
		{
			return new UInt48(a.Value + b.Value);
		}

		public static UInt48 operator ++(UInt48 a)
		{
			return new UInt48(a.Value + 1);
		}

		public static UInt48 operator -(UInt48 a, UInt48 b)
		{
			return new UInt48(a.Value - b.Value);
		}

		public static UInt48 operator --(UInt48 a)
		{
			return new UInt48(a.Value - 1);
		}

		public static UInt48 operator *(UInt48 a, UInt48 b)
		{
			return new UInt48(a.Value * b.Value);
		}

		public static UInt48 operator /(UInt48 a, UInt48 b)
		{
			return new UInt48(a.Value / b.Value);
		}

		public static UInt48 operator %(UInt48 a, UInt48 b)
		{
			return new UInt48(a.Value % b.Value);
		}
	}
	public struct Int56 : IComparable<Int56>, IEquatable<Int56>, System.IFormattable
	{
		public static readonly Int56 Zero = 0;

		public static readonly Int56 MinValue = (Int56)(-36028797018963968L);

		public static readonly Int56 MaxValue = (Int56)36028797018963967L;

		private const long SIGN = 36028797018963968L;

		private const long SIGN_EXTEND = -36028797018963968L;

		private readonly long Value;

		private Int56(long value)
		{
			if ((value & 0x80000000000000L) != 0L)
			{
				Value = value | -36028797018963968L;
			}
			else
			{
				Value = value & MaxValue.Value;
			}
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public static Int56 Parse(string s)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			if (!TryParse(s, out var result))
			{
				throw new FormatException();
			}
			return result;
		}

		public static Int56 Parse(string s, NumberStyles style)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return Parse(s, style, (IFormatProvider)(object)NumberFormatInfo.CurrentInfo);
		}

		public static Int56 Parse(string s, IFormatProvider provider)
		{
			return Parse(s, (NumberStyles)7, provider);
		}

		public static Int56 Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			if (!TryParse(s, style, provider, out var result))
			{
				throw new FormatException();
			}
			return result;
		}

		public static bool TryParse(string s, out Int56 result)
		{
			return TryParse(s, (NumberStyles)7, (IFormatProvider)(object)NumberFormatInfo.CurrentInfo, out result);
		}

		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out Int56 result)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			long value = default(long);
			if (!long.TryParse(s, style, provider, ref value))
			{
				result = Zero;
				return false;
			}
			result = new Int56(value);
			return true;
		}

		public int CompareTo(Int56 other)
		{
			return Value.CompareTo(other.Value);
		}

		public bool Equals(Int56 other)
		{
			return Value == other.Value;
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		public string ToString(string format)
		{
			return Value.ToString(format);
		}

		public string ToString(IFormatProvider provider)
		{
			return Value.ToString((string)null, provider);
		}

		public string ToString(string format, IFormatProvider provider)
		{
			return Value.ToString(format, provider);
		}

		public static implicit operator Int56(sbyte a)
		{
			return new Int56(a);
		}

		public static implicit operator Int56(byte a)
		{
			return new Int56(a);
		}

		public static implicit operator Int56(short a)
		{
			return new Int56(a);
		}

		public static implicit operator Int56(ushort a)
		{
			return new Int56(a);
		}

		public static implicit operator Int56(Int24 a)
		{
			return new Int56(a);
		}

		public static implicit operator Int56(UInt24 a)
		{
			return new Int56(a);
		}

		public static implicit operator Int56(int a)
		{
			return new Int56(a);
		}

		public static implicit operator Int56(uint a)
		{
			return new Int56(a);
		}

		public static implicit operator Int56(Int40 a)
		{
			return new Int56(a);
		}

		public static implicit operator Int56(UInt40 a)
		{
			return new Int56(a);
		}

		public static implicit operator Int56(Int48 a)
		{
			return new Int56(a);
		}

		public static implicit operator Int56(UInt48 a)
		{
			return new Int56(a);
		}

		public static explicit operator Int56(long a)
		{
			return new Int56(a);
		}

		public static explicit operator Int56(UInt56 a)
		{
			return new Int56(a);
		}

		public static explicit operator Int56(ulong a)
		{
			return new Int56((long)a);
		}

		public static explicit operator Int56(float a)
		{
			return new Int56((long)a);
		}

		public static explicit operator Int56(double a)
		{
			return new Int56((long)a);
		}

		public static explicit operator Int56(decimal a)
		{
			return new Int56((long)a);
		}

		public static explicit operator Int56(BigInteger a)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return new Int56((long)a);
		}

		public static implicit operator long(Int56 a)
		{
			return a.Value;
		}

		public static implicit operator float(Int56 a)
		{
			return a.Value;
		}

		public static implicit operator double(Int56 a)
		{
			return a.Value;
		}

		public static implicit operator decimal(Int56 a)
		{
			return decimal.op_Implicit(a.Value);
		}

		public static implicit operator BigInteger(Int56 a)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return BigInteger.op_Implicit(a.Value);
		}

		public static explicit operator sbyte(Int56 a)
		{
			return (sbyte)a.Value;
		}

		public static explicit operator short(Int56 a)
		{
			return (short)a.Value;
		}

		public static explicit operator Int24(Int56 a)
		{
			return (Int24)a.Value;
		}

		public static explicit operator int(Int56 a)
		{
			return (int)a.Value;
		}

		public static explicit operator Int40(Int56 a)
		{
			return (Int40)a.Value;
		}

		public static explicit operator Int48(Int56 a)
		{
			return (Int48)a.Value;
		}

		public static explicit operator byte(Int56 a)
		{
			return (byte)a.Value;
		}

		public static explicit operator ushort(Int56 a)
		{
			return (ushort)a.Value;
		}

		public static explicit operator UInt24(Int56 a)
		{
			return (UInt24)a.Value;
		}

		public static explicit operator uint(Int56 a)
		{
			return (uint)a.Value;
		}

		public static explicit operator UInt40(Int56 a)
		{
			return (UInt40)a.Value;
		}

		public static explicit operator UInt48(Int56 a)
		{
			return (UInt48)a.Value;
		}

		public static explicit operator UInt56(Int56 a)
		{
			return (UInt56)a.Value;
		}

		public static explicit operator ulong(Int56 a)
		{
			return (ulong)a.Value;
		}

		public static explicit operator UInt128(Int56 a)
		{
			return (UInt128)a.Value;
		}

		public static Int56 operator <<(Int56 a, int b)
		{
			return new Int56(a.Value << b);
		}

		public static Int56 operator >>(Int56 a, int b)
		{
			return new Int56(a.Value >> b);
		}

		public static Int56 operator &(Int56 a, Int56 b)
		{
			return new Int56(a.Value & b.Value);
		}

		public static Int56 operator |(Int56 a, Int56 b)
		{
			return new Int56(a.Value | b.Value);
		}

		public static Int56 operator ^(Int56 a, Int56 b)
		{
			return new Int56(a.Value ^ b.Value);
		}

		public static Int56 operator ~(Int56 a)
		{
			return new Int56(~a.Value);
		}

		public static Int56 operator +(Int56 a)
		{
			return a;
		}

		public static Int56 operator -(Int56 a)
		{
			return new Int56(-a.Value);
		}

		public static Int56 operator +(Int56 a, Int56 b)
		{
			return new Int56(a.Value + b.Value);
		}

		public static Int56 operator ++(Int56 a)
		{
			return new Int56(a.Value + 1);
		}

		public static Int56 operator -(Int56 a, Int56 b)
		{
			return new Int56(a.Value - b.Value);
		}

		public static Int56 operator --(Int56 a)
		{
			return new Int56(a.Value - 1);
		}

		public static Int56 operator *(Int56 a, Int56 b)
		{
			return new Int56(a.Value * b.Value);
		}

		public static Int56 operator /(Int56 a, Int56 b)
		{
			return new Int56(a.Value / b.Value);
		}

		public static Int56 operator %(Int56 a, Int56 b)
		{
			return new Int56(a.Value % b.Value);
		}
	}
	public struct UInt56 : IComparable<UInt56>, IEquatable<UInt56>, System.IFormattable
	{
		public static readonly UInt56 Zero = (byte)0;

		public static readonly UInt56 MinValue = Zero;

		public static readonly UInt56 MaxValue = ~MinValue;

		private const ulong MASK = 72057594037927935uL;

		private readonly ulong Value;

		private UInt56(ulong value)
		{
			Value = value & 0xFFFFFFFFFFFFFFL;
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public static UInt56 Parse(string s)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			if (!TryParse(s, out var result))
			{
				throw new FormatException();
			}
			return result;
		}

		public static UInt56 Parse(string s, NumberStyles style)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return Parse(s, style, (IFormatProvider)(object)NumberFormatInfo.CurrentInfo);
		}

		public static UInt56 Parse(string s, IFormatProvider provider)
		{
			return Parse(s, (NumberStyles)7, provider);
		}

		public static UInt56 Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			if (!TryParse(s, style, provider, out var result))
			{
				throw new FormatException();
			}
			return result;
		}

		public static bool TryParse(string s, out UInt56 result)
		{
			return TryParse(s, (NumberStyles)7, (IFormatProvider)(object)NumberFormatInfo.CurrentInfo, out result);
		}

		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out UInt56 result)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			ulong value = default(ulong);
			if (!ulong.TryParse(s, style, provider, ref value))
			{
				result = Zero;
				return false;
			}
			result = new UInt56(value);
			return true;
		}

		public int CompareTo(UInt56 other)
		{
			return Value.CompareTo(other.Value);
		}

		public bool Equals(UInt56 other)
		{
			return Value == other.Value;
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		public string ToString(string format)
		{
			return Value.ToString(format);
		}

		public string ToString(IFormatProvider provider)
		{
			return Value.ToString((string)null, provider);
		}

		public string ToString(string format, IFormatProvider provider)
		{
			return Value.ToString(format, provider);
		}

		public static implicit operator UInt56(byte a)
		{
			return new UInt56(a);
		}

		public static implicit operator UInt56(ushort a)
		{
			return new UInt56(a);
		}

		public static implicit operator UInt56(UInt24 a)
		{
			return new UInt56(a);
		}

		public static implicit operator UInt56(uint a)
		{
			return new UInt56(a);
		}

		public static implicit operator UInt56(UInt40 a)
		{
			return new UInt56(a);
		}

		public static implicit operator UInt56(UInt48 a)
		{
			return new UInt56(a);
		}

		public static explicit operator UInt56(sbyte a)
		{
			return new UInt56((ulong)a);
		}

		public static explicit operator UInt56(short a)
		{
			return new UInt56((ulong)a);
		}

		public static explicit operator UInt56(Int24 a)
		{
			return new UInt56((ulong)a);
		}

		public static explicit operator UInt56(int a)
		{
			return new UInt56((ulong)a);
		}

		public static explicit operator UInt56(Int40 a)
		{
			return new UInt56((ulong)a);
		}

		public static explicit operator UInt56(Int48 a)
		{
			return new UInt56((ulong)a);
		}

		public static explicit operator UInt56(Int56 a)
		{
			return new UInt56((ulong)a);
		}

		public static explicit operator UInt56(long a)
		{
			return new UInt56((ulong)a);
		}

		public static explicit operator UInt56(ulong a)
		{
			return new UInt56(a);
		}

		public static explicit operator UInt56(float a)
		{
			return new UInt56((ulong)a);
		}

		public static explicit operator UInt56(double a)
		{
			return new UInt56((ulong)a);
		}

		public static explicit operator UInt56(decimal a)
		{
			return new UInt56((ulong)a);
		}

		public static explicit operator UInt56(BigInteger a)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return new UInt56((ulong)a);
		}

		public static implicit operator long(UInt56 a)
		{
			return (long)a.Value;
		}

		public static implicit operator ulong(UInt56 a)
		{
			return a.Value;
		}

		public static implicit operator UInt128(UInt56 a)
		{
			return a.Value;
		}

		public static implicit operator float(UInt56 a)
		{
			return a.Value;
		}

		public static implicit operator double(UInt56 a)
		{
			return a.Value;
		}

		public static implicit operator decimal(UInt56 a)
		{
			return decimal.op_Implicit(a.Value);
		}

		public static implicit operator BigInteger(UInt56 a)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return BigInteger.op_Implicit(a.Value);
		}

		public static explicit operator sbyte(UInt56 a)
		{
			return (sbyte)a.Value;
		}

		public static explicit operator short(UInt56 a)
		{
			return (short)a.Value;
		}

		public static explicit operator Int24(UInt56 a)
		{
			return (Int24)a.Value;
		}

		public static explicit operator int(UInt56 a)
		{
			return (int)a.Value;
		}

		public static explicit operator Int40(UInt56 a)
		{
			return (Int40)a.Value;
		}

		public static explicit operator Int48(UInt56 a)
		{
			return (Int48)a.Value;
		}

		public static explicit operator Int56(UInt56 a)
		{
			return (Int56)a.Value;
		}

		public static explicit operator byte(UInt56 a)
		{
			return (byte)a.Value;
		}

		public static explicit operator ushort(UInt56 a)
		{
			return (ushort)a.Value;
		}

		public static explicit operator UInt24(UInt56 a)
		{
			return (UInt24)a.Value;
		}

		public static explicit operator uint(UInt56 a)
		{
			return (uint)a.Value;
		}

		public static explicit operator UInt40(UInt56 a)
		{
			return (UInt40)a.Value;
		}

		public static explicit operator UInt48(UInt56 a)
		{
			return (UInt48)a.Value;
		}

		public static UInt56 operator <<(UInt56 a, int b)
		{
			return new UInt56(a.Value << b);
		}

		public static UInt56 operator >>(UInt56 a, int b)
		{
			return new UInt56(a.Value >> b);
		}

		public static UInt56 operator &(UInt56 a, UInt56 b)
		{
			return new UInt56(a.Value & b.Value);
		}

		public static UInt56 operator |(UInt56 a, UInt56 b)
		{
			return new UInt56(a.Value | b.Value);
		}

		public static UInt56 operator ^(UInt56 a, UInt56 b)
		{
			return new UInt56(a.Value ^ b.Value);
		}

		public static UInt56 operator ~(UInt56 a)
		{
			return new UInt56(~a.Value);
		}

		public static UInt56 operator +(UInt56 a)
		{
			return a;
		}

		public static UInt56 operator -(UInt56 a)
		{
			return new UInt56(0L - a.Value);
		}

		public static UInt56 operator +(UInt56 a, UInt56 b)
		{
			return new UInt56(a.Value + b.Value);
		}

		public static UInt56 operator ++(UInt56 a)
		{
			return new UInt56(a.Value + 1);
		}

		public static UInt56 operator -(UInt56 a, UInt56 b)
		{
			return new UInt56(a.Value - b.Value);
		}

		public static UInt56 operator --(UInt56 a)
		{
			return new UInt56(a.Value - 1);
		}

		public static UInt56 operator *(UInt56 a, UInt56 b)
		{
			return new UInt56(a.Value * b.Value);
		}

		public static UInt56 operator /(UInt56 a, UInt56 b)
		{
			return new UInt56(a.Value / b.Value);
		}

		public static UInt56 operator %(UInt56 a, UInt56 b)
		{
			return new UInt56(a.Value % b.Value);
		}
	}
	public struct UInt128 : IComparable, IComparable<UInt128>, IEquatable<UInt128>, System.IFormattable
	{
		private ulong hi;

		private ulong lo;

		public static readonly UInt128 MaxValue = ~(UInt128)0;

		public static readonly UInt128 MinValue = (UInt128)0;

		private static readonly UInt128 Zero = (UInt128)0;

		private static readonly double TwoPow64 = 1.8446744073709552E+19;

		private static readonly byte[] BitLengthTable = Enumerable.ToArray<byte>(Enumerable.Select<int, byte>(Enumerable.Range(0, 256), (Func<int, byte>)delegate(int value)
		{
			int num = 0;
			while (value != 0)
			{
				value >>= 1;
				num++;
			}
			return (byte)num;
		}));

		public ulong Hi64 => hi;

		public ulong Lo64 => lo;

		private uint R0 => (uint)lo;

		private uint R1 => (uint)(lo >> 32);

		private uint R2 => (uint)hi;

		private uint R3 => (uint)(hi >> 32);

		private UInt128(uint hihi, uint hilo, uint lohi, uint lolo)
		{
			lo = ((ulong)lohi << 32) | lolo;
			hi = ((ulong)hihi << 32) | hilo;
		}

		private UInt128(ulong hi, ulong lo)
		{
			this.hi = hi;
			this.lo = lo;
		}

		public UInt128(BigInteger value)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			int sign = ((BigInteger)(ref value)).Sign;
			if (sign == -1)
			{
				value = -value;
			}
			hi = (ulong)(value >> 64);
			lo = (ulong)value;
			if (sign == -1)
			{
				Negate();
			}
		}

		public UInt128(decimal value)
		{
			int[] bits = decimal.GetBits(decimal.Truncate(value));
			UInt128 uInt = new UInt128(0u, (uint)bits[2], (uint)bits[1], (uint)bits[0]);
			if (value < 0m)
			{
				uInt = -uInt;
			}
			hi = uInt.hi;
			lo = uInt.lo;
		}

		public UInt128(double value)
		{
			bool num = value < 0.0;
			if (num)
			{
				value = 0.0 - value;
			}
			if (value <= 1.8446744073709552E+19)
			{
				hi = 0uL;
				lo = (ulong)value;
			}
			else
			{
				hi = 0uL;
				int num2 = Math.Max((int)Math.Ceiling(Math.Log(value, 2.0)) - 63, 0);
				lo = (ulong)(value / Math.Pow(2.0, (double)num2));
				if (num2 >= 64)
				{
					hi = lo << num2 - 64;
					lo = 0uL;
				}
				else if (num2 != 0)
				{
					hi = (hi << num2) | (lo >> 64 - num2);
					lo <<= num2;
				}
			}
			if (num)
			{
				Negate();
			}
		}

		public UInt128(ulong value)
		{
			hi = 0uL;
			lo = value;
		}

		public UInt128(long value)
		{
			hi = (ulong)((value < 0) ? (-1) : 0);
			lo = (ulong)value;
		}

		public override int GetHashCode()
		{
			return lo.GetHashCode() ^ hi.GetHashCode();
		}

		public static UInt128 Parse(string s)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			if (!TryParse(s, out var result))
			{
				throw new FormatException();
			}
			return result;
		}

		public static UInt128 Parse(string s, NumberStyles style)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return Parse(s, style, (IFormatProvider)(object)NumberFormatInfo.CurrentInfo);
		}

		public static UInt128 Parse(string s, IFormatProvider provider)
		{
			return Parse(s, (NumberStyles)7, provider);
		}

		public static UInt128 Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			if (!TryParse(s, style, provider, out var result))
			{
				throw new FormatException();
			}
			return result;
		}

		public static bool TryParse(string s, out UInt128 result)
		{
			return TryParse(s, (NumberStyles)7, (IFormatProvider)(object)NumberFormatInfo.CurrentInfo, out result);
		}

		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out UInt128 result)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			BigInteger value = default(BigInteger);
			if (!BigInteger.TryParse(s, style, provider, ref value))
			{
				result = Zero;
				return false;
			}
			result = new UInt128(value);
			return true;
		}

		public override string ToString()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			return ((object)(BigInteger)this/*cast due to .constrained prefix*/).ToString();
		}

		public string ToString(string format)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			BigInteger val = this;
			return ((BigInteger)(ref val)).ToString(format);
		}

		public string ToString(IFormatProvider provider)
		{
			return ToString(null, provider);
		}

		public string ToString(string format, IFormatProvider provider)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			BigInteger val = this;
			return ((BigInteger)(ref val)).ToString(format, provider);
		}

		public static implicit operator UInt128(byte a)
		{
			return new UInt128((long)a);
		}

		public static implicit operator UInt128(ushort a)
		{
			return new UInt128((long)a);
		}

		public static implicit operator UInt128(UInt24 a)
		{
			return new UInt128((long)a);
		}

		public static implicit operator UInt128(uint a)
		{
			return new UInt128((long)a);
		}

		public static implicit operator UInt128(UInt48 a)
		{
			return new UInt128((long)a);
		}

		public static implicit operator UInt128(ulong a)
		{
			return new UInt128(a);
		}

		public static explicit operator UInt128(sbyte a)
		{
			return new UInt128((long)a);
		}

		public static explicit operator UInt128(short a)
		{
			return new UInt128((long)a);
		}

		public static explicit operator UInt128(Int24 a)
		{
			return new UInt128((long)a);
		}

		public static explicit operator UInt128(int a)
		{
			return new UInt128((long)a);
		}

		public static explicit operator UInt128(Int48 a)
		{
			return new UInt128((long)a);
		}

		public static explicit operator UInt128(long a)
		{
			return new UInt128(a);
		}

		public static explicit operator UInt128(float a)
		{
			return new UInt128((double)a);
		}

		public static explicit operator UInt128(double a)
		{
			return new UInt128(a);
		}

		public static explicit operator UInt128(decimal a)
		{
			return new UInt128(a);
		}

		public static explicit operator UInt128(BigInteger a)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return new UInt128(a);
		}

		public static implicit operator BigInteger(UInt128 a)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			return (BigInteger.op_Implicit(a.hi) << 64) | BigInteger.op_Implicit(a.lo);
		}

		public static explicit operator decimal(UInt128 a)
		{
			if (a.hi == 0L)
			{
				return decimal.op_Implicit(a.lo);
			}
			int num = Math.Max(0, 32 - GetNumBits64(a.hi));
			_ = a >> num;
			return new decimal((int)a.R0, (int)a.R1, (int)a.R2, false, (byte)num);
		}

		public static explicit operator double(UInt128 a)
		{
			return (double)a.hi * TwoPow64 + (double)a.lo;
		}

		public static explicit operator float(UInt128 a)
		{
			return (float)(double)a;
		}

		public static explicit operator long(UInt128 a)
		{
			return (long)a.lo;
		}

		public static explicit operator ulong(UInt128 a)
		{
			return a.lo;
		}

		public static explicit operator int(UInt128 a)
		{
			return (int)a.lo;
		}

		public static explicit operator uint(UInt128 a)
		{
			return (uint)a.lo;
		}

		public static explicit operator short(UInt128 a)
		{
			return (short)a.lo;
		}

		public static explicit operator ushort(UInt128 a)
		{
			return (ushort)a.lo;
		}

		public static explicit operator byte(UInt128 a)
		{
			return (byte)a.lo;
		}

		public static explicit operator sbyte(UInt128 a)
		{
			return (sbyte)a.lo;
		}

		public static UInt128 operator <<(UInt128 a, int b)
		{
			if (b == 0)
			{
				return a;
			}
			if (b >= 64)
			{
				return new UInt128(a.lo << b - 64, 0uL);
			}
			return new UInt128((a.hi << b) | (a.lo >> 64 - b), a.lo << b);
		}

		public static UInt128 operator >>(UInt128 a, int b)
		{
			if (b > 64)
			{
				return new UInt128(a.hi >> b - 64);
			}
			switch (b)
			{
			case 64:
				return new UInt128(a.hi);
			case 0:
				return a;
			default:
			{
				UInt128 result = default(UInt128);
				result.lo = (a.lo >> b) | (a.hi << 64 - b);
				result.hi = a.hi >> b;
				return result;
			}
			}
		}

		public static UInt128 operator &(UInt128 a, UInt128 b)
		{
			return new UInt128(a.hi & b.hi, a.lo & b.lo);
		}

		public static uint operator &(UInt128 a, uint b)
		{
			return (uint)(int)a.lo & b;
		}

		public static uint operator &(uint a, UInt128 b)
		{
			return a & (uint)(int)b.lo;
		}

		public static ulong operator &(UInt128 a, ulong b)
		{
			return a.lo & b;
		}

		public static ulong operator &(ulong a, UInt128 b)
		{
			return a & b.lo;
		}

		public static UInt128 operator |(UInt128 a, UInt128 b)
		{
			return new UInt128(a.hi | b.hi, a.lo | b.lo);
		}

		public static UInt128 operator ^(UInt128 a, UInt128 b)
		{
			return new UInt128(a.hi ^ b.hi, a.lo ^ b.lo);
		}

		public static UInt128 operator ~(UInt128 value)
		{
			return new UInt128(~value.hi, ~value.lo);
		}

		public static UInt128 operator +(UInt128 value)
		{
			return value;
		}

		public static UInt128 operator -(UInt128 value)
		{
			UInt128 result = value;
			result.Negate();
			return result;
		}

		public static UInt128 operator +(UInt128 a, UInt128 b)
		{
			UInt128 result = default(UInt128);
			result.lo = a.lo + b.lo;
			result.hi = a.hi + b.hi;
			if (result.lo < a.lo && result.lo < b.lo)
			{
				result.hi++;
			}
			return result;
		}

		public static UInt128 operator +(UInt128 a, ulong b)
		{
			UInt128 result = a;
			result.lo += b;
			if (result.lo < a.lo && result.lo < b)
			{
				result.hi++;
			}
			return result;
		}

		public static UInt128 operator +(ulong a, UInt128 b)
		{
			return b + a;
		}

		public static UInt128 operator ++(UInt128 a)
		{
			return a + 1uL;
		}

		public static UInt128 operator -(UInt128 a, UInt128 b)
		{
			UInt128 result = a;
			result.lo -= b.lo;
			result.hi -= b.hi;
			if (a.lo < b.lo)
			{
				result.hi--;
			}
			return result;
		}

		public static UInt128 operator -(UInt128 a, ulong b)
		{
			UInt128 result = a;
			result.lo -= b;
			if (a.lo < b)
			{
				result.hi--;
			}
			return result;
		}

		public static UInt128 operator -(ulong a, UInt128 b)
		{
			UInt128 result = default(UInt128);
			result.lo = a - b.lo;
			result.hi = 0 - b.hi;
			if (a < b.lo)
			{
				result.hi--;
			}
			return result;
		}

		public static UInt128 operator --(UInt128 a)
		{
			return a - 1uL;
		}

		public static UInt128 operator *(UInt128 a, uint b)
		{
			long num = (uint)a.lo;
			ulong num2 = a.lo >> 32;
			ulong num3 = (ulong)(num * b);
			uint num4 = (uint)num3;
			num3 = (num3 >> 32) + num2 * b;
			UInt128 result = default(UInt128);
			result.lo = (num3 << 32) | num4;
			result.hi = num3 >> 32;
			if (a.hi != 0L)
			{
				result.hi += a.hi * b;
			}
			return result;
		}

		public static UInt128 operator *(uint a, UInt128 b)
		{
			return b * a;
		}

		public static UInt128 operator *(UInt128 a, ulong b)
		{
			ulong num = (uint)a.lo;
			ulong num2 = a.lo >> 32;
			ulong num3 = (uint)b;
			ulong num4 = b >> 32;
			ulong num5 = num * num3;
			uint num6 = (uint)num5;
			num5 = (num5 >> 32) + num * num4;
			ulong num7 = num5 >> 32;
			num5 = (uint)num5 + num2 * num3;
			UInt128 result = default(UInt128);
			result.lo = (num5 << 32) | num6;
			result.hi = (num5 >> 32) + num7 + num2 * num4;
			result.hi += a.hi * b;
			return result;
		}

		public static UInt128 operator *(ulong a, UInt128 b)
		{
			return b * a;
		}

		public static UInt128 operator *(UInt128 a, UInt128 b)
		{
			UInt128 result = a * b.lo;
			result.hi += a.lo * b.hi;
			return result;
		}

		public static UInt128 operator /(UInt128 a, UInt128 b)
		{
			if (LessThan(ref a, ref b))
			{
				return Zero;
			}
			if (b.hi == 0L)
			{
				return a / b.lo;
			}
			UInt128 rem;
			if (b.hi <= 4294967295u)
			{
				return DivRem96(out rem, ref a, ref b);
			}
			UInt128 rem2;
			return DivRem128(out rem2, ref a, ref b);
		}

		public static UInt128 operator /(UInt128 a, ulong b)
		{
			if (a.hi == 0L)
			{
				return new UInt128(a.lo / b);
			}
			uint num = (uint)b;
			UInt128 result = default(UInt128);
			if (b == num)
			{
				if (a.hi <= 4294967295u)
				{
					uint r = a.R2;
					uint num2 = r / num;
					ulong num3 = ((ulong)(r - num2 * num) << 32) | a.R1;
					uint num4 = (uint)(num3 / num);
					uint num5 = (uint)(((num3 - num4 * num << 32) | a.R0) / num);
					result = new UInt128(num2, ((ulong)num4 << 32) | num5);
				}
				else
				{
					uint r2 = a.R3;
					uint num6 = r2 / num;
					ulong num7 = ((ulong)(r2 - num6 * num) << 32) | a.R2;
					uint num8 = (uint)(num7 / num);
					ulong num9 = (num7 - num8 * num << 32) | a.R1;
					uint num10 = (uint)(num9 / num);
					uint num11 = (uint)(((num9 - num10 * num << 32) | a.R0) / num);
					result = new UInt128(((ulong)num6 << 32) | num8, ((ulong)num10 << 32) | num11);
				}
			}
			else if (a.hi <= 4294967295u)
			{
				result.lo = (result.hi = 0uL);
				int numBits = GetNumBits32((uint)(b >> 32));
				int num12 = 32 - numBits;
				ulong num13 = b << num12;
				uint v = (uint)(num13 >> 32);
				uint v2 = (uint)num13;
				uint u = a.R0;
				uint u2 = a.R1;
				uint u3 = a.R2;
				uint u4 = 0u;
				if (num12 != 0)
				{
					u4 = u3 >> numBits;
					u3 = (u3 << num12) | (u2 >> numBits);
					u2 = (u2 << num12) | (u >> numBits);
					u <<= num12;
				}
				uint num14 = DivRem(u4, ref u3, ref u2, v, v2);
				uint num15 = DivRem(u3, ref u2, ref u, v, v2);
				result = new UInt128(0uL, ((ulong)num14 << 32) | num15);
			}
			else
			{
				int numBits2 = GetNumBits32((uint)(b >> 32));
				int num16 = 32 - numBits2;
				ulong num17 = b << num16;
				uint v3 = (uint)(num17 >> 32);
				uint v4 = (uint)num17;
				uint u5 = a.R0;
				uint u6 = a.R1;
				uint u7 = a.R2;
				uint u8 = a.R3;
				uint u9 = 0u;
				if (num16 != 0)
				{
					u9 = u8 >> numBits2;
					u8 = (u8 << num16) | (u7 >> numBits2);
					u7 = (u7 << num16) | (u6 >> numBits2);
					u6 = (u6 << num16) | (u5 >> numBits2);
					u5 <<= num16;
				}
				result.hi = DivRem(u9, ref u8, ref u7, v3, v4);
				uint num18 = DivRem(u8, ref u7, ref u6, v3, v4);
				uint num19 = DivRem(u7, ref u6, ref u5, v3, v4);
				result.lo = ((ulong)num18 << 32) | num19;
			}
			return result;
		}

		public static UInt128 operator %(UInt128 a, UInt128 b)
		{
			if (LessThan(ref a, ref b))
			{
				return a;
			}
			if (b.hi == 0L)
			{
				return new UInt128(a % b.lo);
			}
			UInt128 rem;
			if (b.hi <= 4294967295u)
			{
				DivRem96(out rem, ref a, ref b);
			}
			else
			{
				DivRem128(out rem, ref a, ref b);
			}
			return rem;
		}

		public static ulong operator %(UInt128 a, ulong b)
		{
			if (a.hi == 0L)
			{
				return a.lo % b;
			}
			uint num = (uint)b;
			if (b == num)
			{
				return (a.hi <= 4294967295u) ? Remainder96(ref a, num) : Remainder128(ref a, num);
			}
			int numBits = GetNumBits32((uint)(b >> 32));
			int num2 = 32 - numBits;
			ulong num3 = b << num2;
			uint v = (uint)(num3 >> 32);
			uint v2 = (uint)num3;
			uint u = a.R0;
			uint u2 = a.R1;
			uint u3 = a.R2;
			if (a.hi <= 4294967295u)
			{
				uint u4 = 0u;
				if (num2 != 0)
				{
					u4 = u3 >> numBits;
					u3 = (u3 << num2) | (u2 >> numBits);
					u2 = (u2 << num2) | (u >> numBits);
					u <<= num2;
				}
				DivRem(u4, ref u3, ref u2, v, v2);
				DivRem(u3, ref u2, ref u, v, v2);
				return (((ulong)u2 << 32) | u) >> num2;
			}
			uint u5 = a.R3;
			uint u6 = 0u;
			if (num2 != 0)
			{
				u6 = u5 >> numBits;
				u5 = (u5 << num2) | (u3 >> numBits);
				u3 = (u3 << num2) | (u2 >> numBits);
				u2 = (u2 << num2) | (u >> numBits);
				u <<= num2;
			}
			DivRem(u6, ref u5, ref u3, v, v2);
			DivRem(u5, ref u3, ref u2, v, v2);
			DivRem(u3, ref u2, ref u, v, v2);
			return (((ulong)u2 << 32) | u) >> num2;
		}

		public static ulong operator %(UInt128 a, uint b)
		{
			if (a.hi == 0L)
			{
				return (uint)(a.lo % b);
			}
			if (a.hi <= 4294967295u)
			{
				return Remainder96(ref a, b);
			}
			return Remainder128(ref a, b);
		}

		public static bool operator <(UInt128 a, UInt128 b)
		{
			return LessThan(ref a, ref b);
		}

		public static bool operator <(UInt128 a, int b)
		{
			return LessThan(ref a, b);
		}

		public static bool operator <(int a, UInt128 b)
		{
			return LessThan(a, ref b);
		}

		public static bool operator <(UInt128 a, uint b)
		{
			return LessThan(ref a, b);
		}

		public static bool operator <(uint a, UInt128 b)
		{
			return LessThan(a, ref b);
		}

		public static bool operator <(UInt128 a, long b)
		{
			return LessThan(ref a, b);
		}

		public static bool operator <(long a, UInt128 b)
		{
			return LessThan(a, ref b);
		}

		public static bool operator <(UInt128 a, ulong b)
		{
			return LessThan(ref a, b);
		}

		public static bool operator <(ulong a, UInt128 b)
		{
			return LessThan(a, ref b);
		}

		public static bool operator <=(UInt128 a, UInt128 b)
		{
			return !LessThan(ref b, ref a);
		}

		public static bool operator <=(UInt128 a, int b)
		{
			return !LessThan(b, ref a);
		}

		public static bool operator <=(int a, UInt128 b)
		{
			return !LessThan(ref b, a);
		}

		public static bool operator <=(UInt128 a, uint b)
		{
			return !LessThan(b, ref a);
		}

		public static bool operator <=(uint a, UInt128 b)
		{
			return !LessThan(ref b, a);
		}

		public static bool operator <=(UInt128 a, long b)
		{
			return !LessThan(b, ref a);
		}

		public static bool operator <=(long a, UInt128 b)
		{
			return !LessThan(ref b, a);
		}

		public static bool operator <=(UInt128 a, ulong b)
		{
			return !LessThan(b, ref a);
		}

		public static bool operator <=(ulong a, UInt128 b)
		{
			return !LessThan(ref b, a);
		}

		public static bool operator >(UInt128 a, UInt128 b)
		{
			return LessThan(ref b, ref a);
		}

		public static bool operator >(UInt128 a, int b)
		{
			return LessThan(b, ref a);
		}

		public static bool operator >(int a, UInt128 b)
		{
			return LessThan(ref b, a);
		}

		public static bool operator >(UInt128 a, uint b)
		{
			return LessThan(b, ref a);
		}

		public static bool operator >(uint a, UInt128 b)
		{
			return LessThan(ref b, a);
		}

		public static bool operator >(UInt128 a, long b)
		{
			return LessThan(b, ref a);
		}

		public static bool operator >(long a, UInt128 b)
		{
			return LessThan(ref b, a);
		}

		public static bool operator >(UInt128 a, ulong b)
		{
			return LessThan(b, ref a);
		}

		public static bool operator >(ulong a, UInt128 b)
		{
			return LessThan(ref b, a);
		}

		public static bool operator >=(UInt128 a, UInt128 b)
		{
			return !LessThan(ref a, ref b);
		}

		public static bool operator >=(UInt128 a, int b)
		{
			return !LessThan(ref a, b);
		}

		public static bool operator >=(int a, UInt128 b)
		{
			return !LessThan(a, ref b);
		}

		public static bool operator >=(UInt128 a, uint b)
		{
			return !LessThan(ref a, b);
		}

		public static bool operator >=(uint a, UInt128 b)
		{
			return !LessThan(a, ref b);
		}

		public static bool operator >=(UInt128 a, long b)
		{
			return !LessThan(ref a, b);
		}

		public static bool operator >=(long a, UInt128 b)
		{
			return !LessThan(a, ref b);
		}

		public static bool operator >=(UInt128 a, ulong b)
		{
			return !LessThan(ref a, b);
		}

		public static bool operator >=(ulong a, UInt128 b)
		{
			return !LessThan(a, ref b);
		}

		public static bool operator ==(UInt128 a, UInt128 b)
		{
			return a.Equals(b);
		}

		public static bool operator ==(UInt128 a, int b)
		{
			return a.Equals(b);
		}

		public static bool operator ==(int a, UInt128 b)
		{
			return b.Equals(a);
		}

		public static bool operator ==(UInt128 a, uint b)
		{
			return a.Equals(b);
		}

		public static bool operator ==(uint a, UInt128 b)
		{
			return b.Equals(a);
		}

		public static bool operator ==(UInt128 a, long b)
		{
			return a.Equals(b);
		}

		public static bool operator ==(long a, UInt128 b)
		{
			return b.Equals(a);
		}

		public static bool operator ==(UInt128 a, ulong b)
		{
			return a.Equals(b);
		}

		public static bool operator ==(ulong a, UInt128 b)
		{
			return b.Equals(a);
		}

		public static bool operator !=(UInt128 a, UInt128 b)
		{
			return !a.Equals(b);
		}

		public static bool operator !=(UInt128 a, int b)
		{
			return !a.Equals(b);
		}

		public static bool operator !=(int a, UInt128 b)
		{
			return !b.Equals(a);
		}

		public static bool operator !=(UInt128 a, uint b)
		{
			return !a.Equals(b);
		}

		public static bool operator !=(uint a, UInt128 b)
		{
			return !b.Equals(a);
		}

		public static bool operator !=(UInt128 a, long b)
		{
			return !a.Equals(b);
		}

		public static bool operator !=(long a, UInt128 b)
		{
			return !b.Equals(a);
		}

		public static bool operator !=(UInt128 a, ulong b)
		{
			return !a.Equals(b);
		}

		public static bool operator !=(ulong a, UInt128 b)
		{
			return !b.Equals(a);
		}

		public int CompareTo(UInt128 other)
		{
			if (hi == other.hi)
			{
				return lo.CompareTo(other.lo);
			}
			return hi.CompareTo(other.hi);
		}

		public int CompareTo(int other)
		{
			if (hi == 0L && other >= 0)
			{
				return lo.CompareTo((ulong)other);
			}
			return 1;
		}

		public int CompareTo(uint other)
		{
			if (hi == 0L)
			{
				return lo.CompareTo((ulong)other);
			}
			return 1;
		}

		public int CompareTo(long other)
		{
			if (hi == 0L && other >= 0)
			{
				return lo.CompareTo((ulong)other);
			}
			return 1;
		}

		public int CompareTo(ulong other)
		{
			if (hi == 0L)
			{
				return lo.CompareTo(other);
			}
			return 1;
		}

		public int CompareTo(object obj)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			if (obj == null)
			{
				return 1;
			}
			if (!(obj is UInt128 other))
			{
				throw new ArgumentException();
			}
			return CompareTo(other);
		}

		private static bool LessThan(ref UInt128 a, long b)
		{
			if (b >= 0 && a.hi == 0L)
			{
				return a.lo < (ulong)b;
			}
			return false;
		}

		private static bool LessThan(long a, ref UInt128 b)
		{
			if (a >= 0 && b.hi == 0L)
			{
				return (ulong)a < b.lo;
			}
			return true;
		}

		private static bool LessThan(ref UInt128 a, ulong b)
		{
			if (a.hi == 0L)
			{
				return a.lo < b;
			}
			return false;
		}

		private static bool LessThan(ulong a, ref UInt128 b)
		{
			if (b.hi == 0L)
			{
				return a < b.lo;
			}
			return true;
		}

		private static bool LessThan(ref UInt128 a, ref UInt128 b)
		{
			if (a.hi != b.hi)
			{
				return a.hi < b.hi;
			}
			return a.lo < b.lo;
		}

		public static bool Equals(ref UInt128 a, ref UInt128 b)
		{
			if (a.lo == b.lo)
			{
				return a.hi == b.hi;
			}
			return false;
		}

		public bool Equals(UInt128 other)
		{
			if (lo == other.lo)
			{
				return hi == other.hi;
			}
			return false;
		}

		public bool Equals(int other)
		{
			if (other >= 0 && lo == (uint)other)
			{
				return hi == 0;
			}
			return false;
		}

		public bool Equals(uint other)
		{
			if (lo == other)
			{
				return hi == 0;
			}
			return false;
		}

		public bool Equals(long other)
		{
			if (other >= 0 && lo == (ulong)other)
			{
				return hi == 0;
			}
			return false;
		}

		public bool Equals(ulong other)
		{
			if (lo == other)
			{
				return hi == 0;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is UInt128))
			{
				return false;
			}
			return Equals((UInt128)obj);
		}

		private void Negate()
		{
			bool num = lo != 0;
			lo = 0 - lo;
			hi = 0 - hi;
			if (num)
			{
				hi--;
			}
		}

		private static uint Remainder96(ref UInt128 u, uint v)
		{
			return (uint)((((((ulong)(u.R2 % v) << 32) | u.R1) % v << 32) | u.R0) % v);
		}

		private static uint Remainder128(ref UInt128 u, uint v)
		{
			return (uint)((((((((ulong)(u.R3 % v) << 32) | u.R2) % v << 32) | u.R1) % v << 32) | u.R0) % v);
		}

		private static ulong DivRem96(out UInt128 rem, ref UInt128 a, ref UInt128 b)
		{
			int shift = 32 - GetNumBits32(b.R2);
			LeftShift64(out var result, ref b, shift);
			int u = (int)LeftShift64(out rem, ref a, shift);
			uint r = result.R2;
			uint r2 = result.R1;
			uint r3 = result.R0;
			uint u2 = rem.R3;
			uint u3 = rem.R2;
			uint u4 = rem.R1;
			uint u5 = rem.R0;
			uint num = DivRem((uint)u, ref u2, ref u3, ref u4, r, r2, r3);
			uint num2 = DivRem(u2, ref u3, ref u4, ref u5, r, r2, r3);
			rem = new UInt128(0u, u3, u4, u5);
			ulong result2 = ((ulong)num << 32) | num2;
			RightShift64(ref rem, shift);
			return result2;
		}

		private static uint DivRem128(out UInt128 rem, ref UInt128 a, ref UInt128 b)
		{
			int shift = 32 - GetNumBits32(b.R3);
			LeftShift64(out var result, ref b, shift);
			int u = (int)LeftShift64(out rem, ref a, shift);
			uint u2 = rem.R3;
			uint u3 = rem.R2;
			uint u4 = rem.R1;
			uint u5 = rem.R0;
			uint result2 = DivRem((uint)u, ref u2, ref u3, ref u4, ref u5, result.R3, result.R2, result.R1, result.R0);
			rem = new UInt128(u2, u3, u4, u5);
			RightShift64(ref rem, shift);
			return result2;
		}

		private static ulong Q(uint u0, uint u1, uint u2, uint v1, uint v2)
		{
			ulong num = ((ulong)u0 << 32) | u1;
			ulong num2 = ((u0 == v1) ? 4294967295u : (num / v1));
			ulong num3 = num - num2 * v1;
			if (num3 == (uint)num3 && v2 * num2 > ((num3 << 32) | u2))
			{
				num2--;
				num3 += v1;
				if (num3 == (uint)num3 && v2 * num2 > ((num3 << 32) | u2))
				{
					num2--;
					num3 += v1;
				}
			}
			return num2;
		}

		private static uint DivRem(uint u0, ref uint u1, ref uint u2, uint v1, uint v2)
		{
			ulong num = Q(u0, u1, u2, v1, v2);
			ulong num2 = num * v2;
			long num3 = (long)u2 - (long)(uint)num2;
			num2 >>= 32;
			u2 = (uint)num3;
			num3 >>= 32;
			num2 += num * v1;
			num3 += (long)u1 - (long)(uint)num2;
			num2 >>= 32;
			u1 = (uint)num3;
			num3 >>= 32;
			num3 += (long)u0 - (long)(uint)num2;
			if (num3 != 0L)
			{
				num--;
				num2 = (ulong)u2 + (ulong)v2;
				u2 = (uint)num2;
				num2 >>= 32;
				num2 += (ulong)((long)u1 + (long)v1);
				u1 = (uint)num2;
			}
			return (uint)num;
		}

		private static uint DivRem(uint u0, ref uint u1, ref uint u2, ref uint u3, uint v1, uint v2, uint v3)
		{
			ulong num = Q(u0, u1, u2, v1, v2);
			ulong num2 = num * v3;
			long num3 = (long)u3 - (long)(uint)num2;
			num2 >>= 32;
			u3 = (uint)num3;
			num3 >>= 32;
			num2 += num * v2;
			num3 += (long)u2 - (long)(uint)num2;
			num2 >>= 32;
			u2 = (uint)num3;
			num3 >>= 32;
			num2 += num * v1;
			num3 += (long)u1 - (long)(uint)num2;
			num2 >>= 32;
			u1 = (uint)num3;
			num3 >>= 32;
			num3 += (long)u0 - (long)(uint)num2;
			if (num3 != 0L)
			{
				num--;
				num2 = (ulong)u3 + (ulong)v3;
				u3 = (uint)num2;
				num2 >>= 32;
				num2 += (ulong)((long)u2 + (long)v2);
				u2 = (uint)num2;
				num2 >>= 32;
				num2 += (ulong)((long)u1 + (long)v1);
				u1 = (uint)num2;
			}
			return (uint)num;
		}

		private static uint DivRem(uint u0, ref uint u1, ref uint u2, ref uint u3, ref uint u4, uint v1, uint v2, uint v3, uint v4)
		{
			ulong num = Q(u0, u1, u2, v1, v2);
			ulong num2 = num * v4;
			long num3 = (long)u4 - (long)(uint)num2;
			num2 >>= 32;
			u4 = (uint)num3;
			num3 >>= 32;
			num2 += num * v3;
			num3 += (long)u3 - (long)(uint)num2;
			num2 >>= 32;
			u3 = (uint)num3;
			num3 >>= 32;
			num2 += num * v2;
			num3 += (long)u2 - (long)(uint)num2;
			num2 >>= 32;
			u2 = (uint)num3;
			num3 >>= 32;
			num2 += num * v1;
			num3 += (long)u1 - (long)(uint)num2;
			num2 >>= 32;
			u1 = (uint)num3;
			num3 >>= 32;
			num3 += (long)u0 - (long)(uint)num2;
			if (num3 != 0L)
			{
				num--;
				num2 = (ulong)u4 + (ulong)v4;
				u4 = (uint)num2;
				num2 >>= 32;
				num2 += (ulong)((long)u3 + (long)v3);
				u3 = (uint)num2;
				num2 >>= 32;
				num2 += (ulong)((long)u2 + (long)v2);
				u2 = (uint)num2;
				num2 >>= 32;
				num2 += (ulong)((long)u1 + (long)v1);
				u1 = (uint)num2;
			}
			return (uint)num;
		}

		private static ulong LeftShift64(out UInt128 result, ref UInt128 value, int shift)
		{
			if (shift == 0)
			{
				result = value;
				return 0uL;
			}
			int num = 64 - shift;
			result.hi = (value.hi << shift) | (value.lo >> num);
			result.lo = value.lo << shift;
			return value.hi >> num;
		}

		private static void RightShift64(ref UInt128 value, int shift)
		{
			if (shift != 0)
			{
				value.lo = (value.hi << 64 - shift) | (value.lo >> shift);
				value.hi >>= shift;
			}
		}

		private static int GetNumBits64(ulong value)
		{
			uint num = (uint)(value >> 32);
			if (num == 0)
			{
				return GetNumBits32((uint)value);
			}
			return 32 + GetNumBits32(num);
		}

		private static int GetNumBits32(uint value)
		{
			ushort num = (ushort)(value >> 16);
			if (num == 0)
			{
				return GetNumBits16((ushort)value);
			}
			return 16 + GetNumBits16(num);
		}

		private static int GetNumBits16(ushort value)
		{
			byte b = (byte)(value >> 8);
			if (b == 0)
			{
				return BitLengthTable[value];
			}
			return 8 + BitLengthTable[b];
		}
	}
}
namespace IDS.Core.Tasks
{
	public class AsyncOperation : System.IDisposable
	{
		private const int MAX_TIMEOUT_MS = 2147483647;

		public static readonly TimeSpan MAX_TIMEOUT = TimeSpan.FromMilliseconds(2147483647.0);

		public readonly TimeSpan Timeout;

		private readonly CancellationTokenSource CTS;

		private readonly Action<float, string> ProgressDelegate;

		private readonly Timer Timer = new Timer();

		[field: CompilerGenerated]
		public float PercentComplete
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public string Status
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public CancellationToken CancellationToken => CTS.Token;

		public bool IsCancellationRequested => CTS.IsCancellationRequested;

		public TimeSpan ElapsedTime => Timer.ElapsedTime;

		public bool ProgressRequested => ProgressDelegate != null;

		public TimeSpan? EstimatedTotalTime
		{
			get
			{
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_004b: Unknown result type (might be due to invalid IL or missing references)
				if (PercentComplete <= 0f)
				{
					return null;
				}
				TimeSpan elapsedTime = ElapsedTime;
				double totalSeconds = ((TimeSpan)(ref elapsedTime)).TotalSeconds;
				if (totalSeconds <= 1.0)
				{
					return null;
				}
				return TimeSpan.FromSeconds(totalSeconds / (double)(PercentComplete / 100f));
			}
		}

		public TimeSpan? EstimatedRemainingTime
		{
			get
			{
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				TimeSpan? estimatedTotalTime = EstimatedTotalTime;
				TimeSpan elapsedTime = ElapsedTime;
				if (!estimatedTotalTime.HasValue)
				{
					return null;
				}
				return estimatedTotalTime.GetValueOrDefault() - elapsedTime;
			}
		}

		public TimeSpan TimeUntilTimeout => Timeout - ElapsedTime;

		public AsyncOperation(TimeSpan timeout)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			TimeSpan val = default(TimeSpan);
			((TimeSpan)(ref val))..ctor(0, 0, 10, 0);
			Timeout = ((TimeSpan)(ref timeout)).Add(val);
			CTS = new CancellationTokenSource((int)((TimeSpan)(ref timeout)).TotalMilliseconds);
		}

		public AsyncOperation(TimeSpan timeout, CancellationToken token)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			Timeout = timeout;
			CTS = CancellationTokenSource.CreateLinkedTokenSource(token);
			CTS.CancelAfter((int)((TimeSpan)(ref timeout)).TotalMilliseconds);
		}

		public AsyncOperation(TimeSpan timeout, Action<float, string> handler)
			: this(timeout)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			ProgressDelegate = handler;
		}

		public AsyncOperation(TimeSpan timeout, CancellationToken token, Action<float, string> handler)
			: this(timeout, token)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			ProgressDelegate = handler;
		}

		public void Dispose()
		{
			CTS.Dispose();
		}

		public void Cancel()
		{
			CTS.Cancel();
		}

		public void ThrowIfCancellationRequested()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			CancellationToken token = CTS.Token;
			((CancellationToken)(ref token)).ThrowIfCancellationRequested();
		}

		public void ReportProgress(string status)
		{
			if (status != null)
			{
				Status = status;
			}
			ProgressDelegate?.Invoke(PercentComplete, Status);
		}

		public void ReportProgress(float percent_complete, string status = null)
		{
			PercentComplete = percent_complete;
			if (status != null)
			{
				Status = status;
			}
			ProgressDelegate?.Invoke(PercentComplete, Status);
		}
	}
	public sealed class CancellableTask : System.Threading.Tasks.Task
	{
		[CompilerGenerated]
		private sealed class <>c__DisplayClass8_0
		{
			[StructLayout((LayoutKind)3)]
			private struct <<Run>b__1>d : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncVoidMethodBuilder <>t__builder;

				public <>c__DisplayClass8_0 <>4__this;

				private ConfiguredTaskAwaiter <>u__1;

				private void MoveNext()
				{
					//IL_006d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0072: Unknown result type (might be due to invalid IL or missing references)
					//IL_0079: Unknown result type (might be due to invalid IL or missing references)
					//IL_002a: Unknown result type (might be due to invalid IL or missing references)
					//IL_0035: Unknown result type (might be due to invalid IL or missing references)
					//IL_003a: Unknown result type (might be due to invalid IL or missing references)
					//IL_003d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0042: Unknown result type (might be due to invalid IL or missing references)
					//IL_0056: Unknown result type (might be due to invalid IL or missing references)
					//IL_0057: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					<>c__DisplayClass8_0 <>c__DisplayClass8_ = <>4__this;
					try
					{
						try
						{
							ConfiguredTaskAwaiter val2;
							if (num != 0)
							{
								ConfiguredTaskAwaitable val = <>c__DisplayClass8_.t.UserAction.Invoke(<>c__DisplayClass8_.t.CTS.Token).ConfigureAwait(false);
								val2 = ((ConfiguredTaskAwaitable)(ref val)).GetAwaiter();
								if (!((ConfiguredTaskAwaiter)(ref val2)).IsCompleted)
								{
									num = (<>1__state = 0);
									<>u__1 = val2;
									((AsyncVoidMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <<Run>b__1>d>(ref val2, ref this);
									return;
								}
							}
							else
							{
								val2 = <>u__1;
								<>u__1 = default(ConfiguredTaskAwaiter);
								num = (<>1__state = -1);
							}
							((ConfiguredTaskAwaiter)(ref val2)).GetResult();
						}
						catch (System.Exception ex)
						{
							throw ex;
						}
						finally
						{
							_ = 0;
						}
					}
					catch (System.Exception exception)
					{
						<>1__state = -2;
						((AsyncVoidMethodBuilder)(ref <>t__builder)).SetException(exception);
						return;
					}
					<>1__state = -2;
					((AsyncVoidMethodBuilder)(ref <>t__builder)).SetResult();
				}

				[DebuggerHidden]
				private void SetStateMachine(IAsyncStateMachine stateMachine)
				{
					((AsyncVoidMethodBuilder)(ref <>t__builder)).SetStateMachine(stateMachine);
				}
			}

			public Action start_action;

			public CancellableTask t;

			internal void <Run>b__0()
			{
				start_action.Invoke();
			}

			[AsyncStateMachine(typeof(<<Run>b__1>d))]
			internal void <Run>b__1()
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				<<Run>b__1>d <<Run>b__1>d = default(<<Run>b__1>d);
				<<Run>b__1>d.<>t__builder = AsyncVoidMethodBuilder.Create();
				<<Run>b__1>d.<>4__this = this;
				<<Run>b__1>d.<>1__state = -1;
				((AsyncVoidMethodBuilder)(ref <<Run>b__1>d.<>t__builder)).Start<<<Run>b__1>d>(ref <<Run>b__1>d);
			}
		}

		private readonly Func<CancellationToken, System.Threading.Tasks.Task> UserAction;

		private CancellationTokenSource CTS;

		[field: CompilerGenerated]
		public bool IsDisposed
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		private string Name
		{
			get
			{
				if (((System.Delegate)(object)UserAction).Target == null)
				{
					return ((object)UserAction).GetType().FullName;
				}
				return ((System.Delegate)(object)UserAction).Target.GetType().FullName;
			}
		}

		public static CancellableTask Run(Func<CancellationToken, System.Threading.Tasks.Task> action)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			//IL_0025: Expected O, but got Unknown
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			<>c__DisplayClass8_0 CS$<>8__locals7 = new <>c__DisplayClass8_0();
			CS$<>8__locals7.start_action = null;
			CS$<>8__locals7.t = new CancellableTask(action, (Action)delegate
			{
				CS$<>8__locals7.start_action.Invoke();
			}, new CancellationTokenSource());
			CS$<>8__locals7.start_action = (Action)([AsyncStateMachine(typeof(<>c__DisplayClass8_0.<<Run>b__1>d))] () =>
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				<>c__DisplayClass8_0.<<Run>b__1>d <<Run>b__1>d = default(<>c__DisplayClass8_0.<<Run>b__1>d);
				<<Run>b__1>d.<>t__builder = AsyncVoidMethodBuilder.Create();
				<<Run>b__1>d.<>4__this = CS$<>8__locals7;
				<<Run>b__1>d.<>1__state = -1;
				((AsyncVoidMethodBuilder)(ref <<Run>b__1>d.<>t__builder)).Start<<>c__DisplayClass8_0.<<Run>b__1>d>(ref <<Run>b__1>d);
			});
			((System.Threading.Tasks.Task)CS$<>8__locals7.t).Start();
			return CS$<>8__locals7.t;
		}

		private CancellableTask(Func<CancellationToken, System.Threading.Tasks.Task> user_action, Action startup_action, CancellationTokenSource cts)
			: base(startup_action, cts.Token)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			UserAction = user_action;
			CTS = cts;
		}

		public void Cancel()
		{
			CancellationTokenSource val = Interlocked.Exchange<CancellationTokenSource>(ref CTS, (CancellationTokenSource)null);
			CancelAndWaitForCompletion(val);
			if (val != null)
			{
				base.Dispose();
			}
		}

		private void CancelAndWaitForCompletion(CancellationTokenSource cts)
		{
			try
			{
				if (cts != null)
				{
					cts.Cancel();
				}
			}
			catch
			{
			}
			try
			{
				if (cts != null)
				{
					cts.Dispose();
				}
			}
			catch
			{
			}
			try
			{
				base.Wait();
			}
			catch
			{
			}
		}

		protected void Dispose(bool disposing)
		{
			if (!IsDisposed)
			{
				CancellationTokenSource cts = Interlocked.Exchange<CancellationTokenSource>(ref CTS, (CancellationTokenSource)null);
				CancelAndWaitForCompletion(cts);
			}
			base.Dispose(disposing);
			IsDisposed = true;
		}
	}
	public class PeriodicTask : Disposable
	{
		public enum Type
		{
			FixedDelay,
			FixedRate
		}

		private class TimeMeasurer
		{
			private static readonly TimeSpan OneSecond = TimeSpan.FromSeconds(1.0);

			private Timer Timer = new Timer();

			private Timer DumpTime = new Timer();

			private int Count;

			private TimeSpan TotalTime = TimeSpan.Zero;

			private TimeSpan TotalSleep = TimeSpan.Zero;

			private int TaskCount;

			public void Reset()
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				TotalTime = TimeSpan.Zero;
				TotalSleep = TimeSpan.Zero;
				Count = 0;
				DumpTime.Reset();
			}

			public void Start(int task_count)
			{
				TaskCount = task_count;
				Timer.Reset();
			}

			public void Stop(TimeSpan sleep_time)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_004d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				TotalTime += Timer.ElapsedTime;
				if (((TimeSpan)(ref sleep_time)).Ticks > 0)
				{
					TotalSleep += sleep_time;
				}
				Count++;
				if (DumpTime.ElapsedTime > OneSecond)
				{
					_ = ((TimeSpan)(ref TotalTime)).TotalSeconds / (double)Count;
					_ = ((TimeSpan)(ref TotalSleep)).TotalMilliseconds / (double)Count;
					Reset();
				}
			}
		}

		private class ActionScheduler
		{
			private readonly Action Action;

			private readonly TimeSpan Period;

			private readonly Type TaskType;

			private Timer Timer;

			public ActionScheduler(Action action, TimeSpan period, Type tasktype)
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				Action = action;
				Period = period;
				TaskType = tasktype;
			}

			public void Resume()
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				if (Timer != null && Timer.ElapsedTime > TimeSpan.Zero)
				{
					Timer.ElapsedTime = TimeSpan.Zero;
				}
			}

			public TimeSpan Invoke()
			{
				//IL_006c: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				//IL_004f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0065: Unknown result type (might be due to invalid IL or missing references)
				//IL_005f: Unknown result type (might be due to invalid IL or missing references)
				Action.Invoke();
				if (TaskType == Type.FixedRate)
				{
					if (Timer == null)
					{
						Timer = new Timer();
					}
					Timer timer = Timer;
					timer.ElapsedTime -= Period;
					TimeSpan elapsedTime = Timer.ElapsedTime;
					long ticks = ((TimeSpan)(ref elapsedTime)).Ticks;
					if (ticks < 0)
					{
						return TimeSpan.FromTicks(-ticks);
					}
					return TimeSpan.Zero;
				}
				return Period;
			}
		}

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <<-ctor>b__20_0>d : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public PeriodicTask <>4__this;

			private TaskAwaiter<Timer> <>u__1;

			private void MoveNext()
			{
				//IL_0047: Unknown result type (might be due to invalid IL or missing references)
				//IL_004c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				PeriodicTask periodicTask = <>4__this;
				try
				{
					TaskAwaiter<Timer> val;
					if (num != 0)
					{
						val = periodicTask.GetTimerAsync().GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<Timer>, <<-ctor>b__20_0>d>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<Timer>);
						num = (<>1__state = -1);
					}
					MasterBackgroundTask(val.GetResult());
				}
				catch (System.Exception exception)
				{
					<>1__state = -2;
					((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
					return;
				}
				<>1__state = -2;
				((AsyncTaskMethodBuilder)(ref <>t__builder)).SetResult();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				((AsyncTaskMethodBuilder)(ref <>t__builder)).SetStateMachine(stateMachine);
			}
		}

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <GetTimerAsync>d__25 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<Timer> <>t__builder;

			private ConfiguredTaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				//IL_004f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				Timer result;
				try
				{
					if (num != 0)
					{
						goto IL_000a;
					}
					ConfiguredTaskAwaiter val = <>u__1;
					<>u__1 = default(ConfiguredTaskAwaiter);
					num = (<>1__state = -1);
					goto IL_0065;
					IL_0065:
					((ConfiguredTaskAwaiter)(ref val)).GetResult();
					if (FreeRunningCounter.Instance == null)
					{
						goto IL_000a;
					}
					result = new Timer();
					goto end_IL_0007;
					IL_000a:
					ConfiguredTaskAwaitable val2 = System.Threading.Tasks.Task.Delay(50).ConfigureAwait(false);
					val = ((ConfiguredTaskAwaitable)(ref val2)).GetAwaiter();
					if (!((ConfiguredTaskAwaiter)(ref val)).IsCompleted)
					{
						num = (<>1__state = 0);
						<>u__1 = val;
						<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <GetTimerAsync>d__25>(ref val, ref this);
						return;
					}
					goto IL_0065;
					end_IL_0007:;
				}
				catch (System.Exception exception)
				{
					<>1__state = -2;
					<>t__builder.SetException(exception);
					return;
				}
				<>1__state = -2;
				<>t__builder.SetResult(result);
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				<>t__builder.SetStateMachine(stateMachine);
			}
		}

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <PrivateTaskAsync>d__26 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public PeriodicTask <>4__this;

			public CancellationToken token;

			private Timer <timer>5__2;

			private TaskAwaiter<Timer> <>u__1;

			private ConfiguredTaskAwaiter <>u__2;

			private void MoveNext()
			{
				//IL_0051: Unknown result type (might be due to invalid IL or missing references)
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				//IL_005d: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
				//IL_0100: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				//IL_0086: Unknown result type (might be due to invalid IL or missing references)
				//IL_008b: Unknown result type (might be due to invalid IL or missing references)
				//IL_008e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0090: Unknown result type (might be due to invalid IL or missing references)
				//IL_0095: Unknown result type (might be due to invalid IL or missing references)
				//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
				//IL_00be: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00da: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				PeriodicTask periodicTask = <>4__this;
				try
				{
					ConfiguredTaskAwaiter val;
					TaskAwaiter<Timer> val2;
					if (num != 0)
					{
						if (num == 1)
						{
							val = <>u__2;
							<>u__2 = default(ConfiguredTaskAwaiter);
							num = (<>1__state = -1);
							goto IL_010f;
						}
						val2 = periodicTask.GetTimerAsync().GetAwaiter();
						if (!val2.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val2;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<Timer>, <PrivateTaskAsync>d__26>(ref val2, ref this);
							return;
						}
					}
					else
					{
						val2 = <>u__1;
						<>u__1 = default(TaskAwaiter<Timer>);
						num = (<>1__state = -1);
					}
					Timer result = val2.GetResult();
					<timer>5__2 = result;
					goto IL_0130;
					IL_010f:
					((ConfiguredTaskAwaiter)(ref val)).GetResult();
					goto IL_0130;
					IL_0130:
					while (!periodicTask.IsDisposed && !((CancellationToken)(ref token)).IsCancellationRequested)
					{
						TimeSpan elapsedTimeAndReset = <timer>5__2.GetElapsedTimeAndReset();
						TimeSpan val3 = periodicTask.ScheduleInvoke(elapsedTimeAndReset);
						int num2 = (int)((TimeSpan)(ref val3)).TotalMilliseconds;
						if (num2 >= 20)
						{
							ConfiguredTaskAwaitable val4 = System.Threading.Tasks.Task.Delay(num2, token).ConfigureAwait(true);
							val = ((ConfiguredTaskAwaitable)(ref val4)).GetAwaiter();
							if (!((ConfiguredTaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 1);
								<>u__2 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<ConfiguredTaskAwaiter, <PrivateTaskAsync>d__26>(ref val, ref this);
								return;
							}
							goto IL_010f;
						}
						if (num2 >= 0)
						{
							((CancellationToken)(ref token)).WaitHandle.WaitOne(num2);
						}
					}
				}
				catch (System.Exception exception)
				{
					<>1__state = -2;
					<timer>5__2 = null;
					((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
					return;
				}
				<>1__state = -2;
				<timer>5__2 = null;
				((AsyncTaskMethodBuilder)(ref <>t__builder)).SetResult();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				((AsyncTaskMethodBuilder)(ref <>t__builder)).SetStateMachine(stateMachine);
			}
		}

		private static readonly TimeSpan DEFAULT_DELAY = TimeSpan.FromSeconds(0.1);

		private static readonly object CriticalSection = new object();

		private static readonly LinkedList<PeriodicTask> TaskList = new LinkedList<PeriodicTask>();

		private static int MasterBackgroundTaskRunning = 0;

		private static readonly AutoResetEvent WakeMasterBackgroundTaskSignal = new AutoResetEvent(true);

		protected readonly Func<TimeSpan> UserAction;

		private readonly CancellableTask PrivateTask;

		private TimeSpan Delay = TimeSpan.Zero;

		[field: CompilerGenerated]
		public bool IsRunning
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public bool IsSynchronous => PrivateTask == null;

		public PeriodicTask(Action action, TimeSpan period, Type tasktype, bool synchronous = true)
			: this(action, period, TimeSpan.Zero, tasktype, synchronous)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)


		public PeriodicTask(Action action, TimeSpan period, TimeSpan delay, Type tasktype, bool synchronous = true)
			: this(new ActionScheduler(action, period, tasktype).Invoke, delay, synchronous)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)


		public PeriodicTask(Func<TimeSpan> action, bool synchronous = true)
			: this(action, TimeSpan.Zero, synchronous)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		public PeriodicTask(Func<TimeSpan> action, TimeSpan delay, bool synchronous = true)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			UserAction = action;
			Delay = delay;
			if (((TimeSpan)(ref Delay)).Ticks < 0)
			{
				Delay = TimeSpan.Zero;
			}
			if (!synchronous)
			{
				PrivateTask = CancellableTask.Run(PrivateTaskAsync);
			}
			else
			{
				lock (TaskList)
				{
					TaskList.AddLast(this);
				}
				if (Interlocked.Exchange(ref MasterBackgroundTaskRunning, 1) == 0)
				{
					System.Threading.Tasks.Task.Run((Func<System.Threading.Tasks.Task>)([AsyncStateMachine(typeof(<<-ctor>b__20_0>d))] [CompilerGenerated] () =>
					{
						//IL_0002: Unknown result type (might be due to invalid IL or missing references)
						//IL_0007: Unknown result type (might be due to invalid IL or missing references)
						<<-ctor>b__20_0>d <<-ctor>b__20_0>d = default(<<-ctor>b__20_0>d);
						<<-ctor>b__20_0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
						<<-ctor>b__20_0>d.<>4__this = this;
						<<-ctor>b__20_0>d.<>1__state = -1;
						((AsyncTaskMethodBuilder)(ref <<-ctor>b__20_0>d.<>t__builder)).Start<<<-ctor>b__20_0>d>(ref <<-ctor>b__20_0>d);
						return ((AsyncTaskMethodBuilder)(ref <<-ctor>b__20_0>d.<>t__builder)).Task;
					}));
				}
			}
			IsRunning = true;
			if (IsSynchronous)
			{
				((EventWaitHandle)WakeMasterBackgroundTaskSignal).Set();
			}
		}

		public override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Pause();
				((System.Threading.Tasks.Task)PrivateTask)?.Dispose();
			}
		}

		public void Pause()
		{
			IsRunning = false;
		}

		public void Resume()
		{
			if (!IsRunning)
			{
				(((System.Delegate)(object)UserAction).Target as ActionScheduler)?.Resume();
				IsRunning = true;
				if (IsSynchronous)
				{
					((EventWaitHandle)WakeMasterBackgroundTaskSignal).Set();
				}
			}
		}

		private TimeSpan ScheduleInvoke(TimeSpan delta)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			Delay -= delta;
			if (base.IsDisposed || !IsRunning)
			{
				return DEFAULT_DELAY;
			}
			if (((TimeSpan)(ref Delay)).Ticks <= 0)
			{
				try
				{
					Delay = UserAction.Invoke();
				}
				catch
				{
					Delay = DEFAULT_DELAY;
				}
			}
			if (((TimeSpan)(ref Delay)).Ticks > 0)
			{
				return Delay;
			}
			return TimeSpan.Zero;
		}

		[AsyncStateMachine(typeof(<GetTimerAsync>d__25))]
		private async System.Threading.Tasks.Task<Timer> GetTimerAsync()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			do
			{
				await System.Threading.Tasks.Task.Delay(50).ConfigureAwait(false);
			}
			while (FreeRunningCounter.Instance == null);
			return new Timer();
		}

		[AsyncStateMachine(typeof(<PrivateTaskAsync>d__26))]
		private System.Threading.Tasks.Task PrivateTaskAsync(CancellationToken token)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			<PrivateTaskAsync>d__26 <PrivateTaskAsync>d__ = default(<PrivateTaskAsync>d__26);
			<PrivateTaskAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<PrivateTaskAsync>d__.<>4__this = this;
			<PrivateTaskAsync>d__.token = token;
			<PrivateTaskAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <PrivateTaskAsync>d__.<>t__builder)).Start<<PrivateTaskAsync>d__26>(ref <PrivateTaskAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <PrivateTaskAsync>d__.<>t__builder)).Task;
		}

		private static void MasterBackgroundTask(Timer t)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			_ = TaskScheduler.Current.Id;
			while (!Environment.HasShutdownStarted)
			{
				TimeSpan val = DEFAULT_DELAY;
				TimeSpan elapsedTimeAndReset = t.GetElapsedTimeAndReset();
				LinkedListNode<PeriodicTask> val2;
				lock (TaskList)
				{
					val2 = TaskList.First;
				}
				if (val2 != null)
				{
					do
					{
						if (!val2.Value.IsDisposed)
						{
							TimeSpan val3 = val2.Value.ScheduleInvoke(elapsedTimeAndReset);
							if (val > val3)
							{
								val = val3;
							}
						}
						lock (TaskList)
						{
							LinkedListNode<PeriodicTask> next = val2.Next;
							if (val2.Value.IsDisposed)
							{
								TaskList.Remove(val2);
							}
							val2 = next;
						}
					}
					while (val2 != null);
				}
				int num = (int)((TimeSpan)(ref val)).TotalMilliseconds;
				if (num < 1)
				{
					num = 1;
				}
				((WaitHandle)WakeMasterBackgroundTaskSignal).WaitOne(num);
			}
		}
	}
}
namespace IDS.Core.Events
{
	public enum SubscriptionType
	{
		Weak,
		Strong
	}
	public interface IEventSender
	{
		IEventPublisher Events { get; }
	}
	public interface IEventPublisher : IDisposable, System.IDisposable
	{
		RoundRobinPublisher CreateRoundRobinPublisher<T>() where T : Event;

		SubscriptionToken Subscribe<T>(Action<T> deliveryAction, SubscriptionType reference) where T : Event;

		void Subscribe<T>(Action<T> deliveryAction, SubscriptionType reference, SubscriptionManager manager) where T : Event;

		bool HasSubscriptionsFor<T>() where T : Event;

		int CountSubscriptionsFor<T>() where T : Event;

		void Publish<T>(T e) where T : Event;

		void Publish(Event e);

		void Publish(Event e, System.Type eventType);

		void RequestPurge(System.Type eventType);

		void RequestPurgeAll();
	}
	public abstract class Event
	{
		private SubscriptionList List;

		[field: CompilerGenerated]
		public object Sender
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public bool CanSelfPublish => List != null;

		protected Event(object sender)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (sender == null)
			{
				throw new ArgumentNullException("sender");
			}
			if (sender is EventPublisher eventPublisher)
			{
				List = eventPublisher.GetValidSubscriptionList(base.GetType());
			}
			else if (sender is IEventSender eventSender)
			{
				List = (eventSender.Events as EventPublisher)?.GetValidSubscriptionList(base.GetType());
			}
			else
			{
				List = null;
			}
			Sender = sender;
		}

		public void Publish()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			if (List == null)
			{
				throw new InvalidOperationException(base.GetType().FullName + " cannot self publish as the sender is not an IEventSender");
			}
			List.Publish(this);
		}
	}
	internal abstract class Subscription
	{
		private readonly EventPublisher Publisher;

		private readonly System.Type EventType;

		[field: CompilerGenerated]
		public bool IsDisposed
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public abstract bool IsAlive { get; }

		public abstract bool DoInvoke(object e);

		protected Subscription(EventPublisher publisher, System.Type event_type)
		{
			Publisher = publisher;
			EventType = event_type;
			IsDisposed = false;
		}

		public bool Invoke(object e)
		{
			if (!IsDisposed)
			{
				if (DoInvoke(e))
				{
					return true;
				}
				IsDisposed = true;
			}
			return false;
		}

		public void Dispose()
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				Publisher.RequestPurge(EventType);
			}
		}
	}
	internal abstract class Subscription<T> : Subscription where T : Event
	{
		protected abstract bool TypedInvoke(T e);

		protected Subscription(EventPublisher publisher)
			: base(publisher, typeof(T))
		{
		}

		public sealed override bool DoInvoke(object e)
		{
			if (e is T e2)
			{
				return TypedInvoke(e2);
			}
			throw new System.Exception($"Unexpected event <{e}>");
		}
	}
	internal sealed class StrongSubscription<T> : Subscription<T> where T : Event
	{
		private readonly Action<T> Action;

		public sealed override bool IsAlive => true;

		public StrongSubscription(EventPublisher publisher, Action<T> action)
			: base(publisher)
		{
			Action = action;
		}

		protected sealed override bool TypedInvoke(T e)
		{
			Action?.Invoke(e);
			return true;
		}
	}
	internal sealed class WeakSubscription<T> : Subscription<T> where T : Event
	{
		private readonly WeakReference<Action<T>> wr;

		public unsafe sealed override bool IsAlive
		{
			get
			{
				Action<T> val = default(Action<T>);
				if (!((WeakReference<Action<Action<T>>>)(object)wr).TryGetTarget(ref *(Action<Action<T>>*)(&val)))
				{
					return false;
				}
				return val != null;
			}
		}

		public WeakSubscription(EventPublisher publisher, Action<T> action)
			: base(publisher)
		{
			wr = (WeakReference<Action<T>>)(object)new WeakReference<Action<Action<T>>>((Action<Action<T>>)(object)action);
		}

		protected unsafe sealed override bool TypedInvoke(T e)
		{
			Action<T> val = default(Action<T>);
			if (!((WeakReference<Action<Action<T>>>)(object)wr).TryGetTarget(ref *(Action<Action<T>>*)(&val)))
			{
				return false;
			}
			if (val == null)
			{
				return false;
			}
			val.Invoke(e);
			return true;
		}
	}
	internal class SubscriptionList : Disposable
	{
		public readonly EventPublisher Publisher;

		public readonly System.Type EventType;

		private List<Subscription> InUse = new List<Subscription>();

		private List<Subscription> Free = new List<Subscription>();

		private ConcurrentQueue<Subscription> NewItems = new ConcurrentQueue<Subscription>();

		private int Enumerating;

		private int Purging;

		public int Count => InUse.Count + NewItems.Count;

		public bool IsEmpty => Count <= 0;

		public SubscriptionList(EventPublisher publisher, System.Type event_type)
		{
			Publisher = publisher;
			EventType = event_type;
		}

		public void Clear()
		{
			lock (this)
			{
				InUse.Clear();
				Free.Clear();
			}
			Subscription subscription = default(Subscription);
			while (NewItems.TryDequeue(ref subscription))
			{
			}
		}

		public void Add(Subscription subscription)
		{
			if (!base.IsDisposed && subscription != null)
			{
				NewItems.Enqueue(subscription);
			}
		}

		private void AddNewSubscriptionsFromQueue()
		{
			if (Enumerating != 0 || NewItems.IsEmpty)
			{
				return;
			}
			Subscription subscription = default(Subscription);
			while (NewItems.TryDequeue(ref subscription))
			{
				if (subscription != null && subscription.IsAlive && !subscription.IsDisposed)
				{
					InUse.Add(subscription);
				}
			}
		}

		public Subscription GetNextSubscription(ref int index)
		{
			lock (this)
			{
				if (Enumerating == 0)
				{
					AddNewSubscriptionsFromQueue();
				}
				int count = InUse.Count;
				if (index < 0 || index >= count)
				{
					index = 0;
				}
				if (index < count)
				{
					return InUse[index++];
				}
				return null;
			}
		}

		public void Publish(Event e)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			if (base.IsDisposed || IsEmpty)
			{
				return;
			}
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			bool flag = true;
			lock (this)
			{
				if (Enumerating == 0)
				{
					AddNewSubscriptionsFromQueue();
				}
				Interlocked.Increment(ref Enumerating);
				try
				{
					Enumerator<Subscription> enumerator = InUse.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							Subscription current = enumerator.Current;
							flag &= current.Invoke(e);
						}
					}
					finally
					{
						((System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
					}
				}
				finally
				{
					Interlocked.Decrement(ref Enumerating);
				}
			}
			if (!flag)
			{
				RequestPurge();
			}
		}

		public void RequestPurge()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected O, but got Unknown
			if (base.IsDisposed || InUse.Count <= 0 || Interlocked.Exchange(ref Purging, 1) != 0)
			{
				return;
			}
			System.Threading.Tasks.Task.Run((Action)([CompilerGenerated] () =>
			{
				//IL_002a: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				Interlocked.Exchange(ref Purging, 1);
				try
				{
					lock (this)
					{
						Free.Clear();
						Enumerator<Subscription> enumerator = InUse.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								Subscription current = enumerator.Current;
								if (current != null && current.IsAlive && !current.IsDisposed)
								{
									Free.Add(current);
								}
							}
						}
						finally
						{
							((System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
						}
						Free = Interlocked.Exchange<List<Subscription>>(ref InUse, Free);
						Free.Clear();
						AddNewSubscriptionsFromQueue();
					}
				}
				finally
				{
					Interlocked.Exchange(ref Purging, 0);
				}
			}));
		}

		public override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Clear();
			}
		}
	}
	public class RoundRobinPublisher : Disposable
	{
		private SubscriptionList List;

		private System.Type EventType;

		private int Index;

		public int SubscriberCount => List.Count;

		internal RoundRobinPublisher(SubscriptionList subscriberList, System.Type eventType)
		{
			List = subscriberList;
			EventType = eventType;
		}

		public void PublishNext(Event e)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException(((object)this).GetType().FullName);
			}
			if (e != null && !List.IsEmpty)
			{
				Subscription nextSubscription = List.GetNextSubscription(ref Index);
				if (nextSubscription == null || !nextSubscription.Invoke(e))
				{
					List.RequestPurge();
				}
			}
		}

		public override void Dispose(bool disposing)
		{
			if (disposing)
			{
				List = null;
				EventType = null;
			}
		}
	}
	public class EventPublisher : Disposable, IEventPublisher, IDisposable, System.IDisposable
	{
		private readonly ConcurrentDictionary<System.Type, SubscriptionList> SubscriptionLists = new ConcurrentDictionary<System.Type, SubscriptionList>();

		public readonly string Name;

		public EventPublisher(string name)
		{
			Name = name;
		}

		private SubscriptionList GetSubscriptionList(System.Type eventType)
		{
			SubscriptionList result = default(SubscriptionList);
			SubscriptionLists.TryGetValue(eventType, ref result);
			return result;
		}

		internal SubscriptionList GetValidSubscriptionList(System.Type eventType)
		{
			SubscriptionList subscriptionList = GetSubscriptionList(eventType);
			if (subscriptionList != null)
			{
				return subscriptionList;
			}
			if (base.IsDisposed)
			{
				return null;
			}
			return SubscriptionLists.GetOrAdd(eventType, (Func<System.Type, SubscriptionList>)((System.Type type) => new SubscriptionList(this, eventType)));
		}

		public RoundRobinPublisher CreateRoundRobinPublisher<T>() where T : Event
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException(((object)this).GetType().FullName);
			}
			return new RoundRobinPublisher(GetValidSubscriptionList(typeof(T)), typeof(T));
		}

		public void Subscribe<T>(Action<T> deliveryAction, SubscriptionType type, SubscriptionManager manager) where T : Event
		{
			manager?.AddSubscription(Subscribe<T>(deliveryAction, type));
		}

		public SubscriptionToken Subscribe<T>(Action<T> deliveryAction, SubscriptionType type) where T : Event
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException(((object)this).GetType().FullName);
			}
			if (deliveryAction == null)
			{
				throw new ArgumentNullException("deliveryAction");
			}
			Subscription subscription = type switch
			{
				SubscriptionType.Strong => new StrongSubscription<T>(this, deliveryAction), 
				SubscriptionType.Weak => new WeakSubscription<T>(this, deliveryAction), 
				_ => throw new ArgumentOutOfRangeException("type", $"SubscriptionType <{type}> unexpected"), 
			};
			SubscriptionList validSubscriptionList = GetValidSubscriptionList(typeof(T));
			if (validSubscriptionList == null)
			{
				return null;
			}
			validSubscriptionList.Add(subscription);
			return new SubscriptionToken(subscription, deliveryAction);
		}

		public bool HasSubscriptionsFor<T>() where T : Event
		{
			return CountSubscriptionsFor<T>() > 0;
		}

		public int CountSubscriptionsFor<T>() where T : Event
		{
			return GetSubscriptionList(typeof(T))?.Count ?? 0;
		}

		public void Publish<T>(T e) where T : Event
		{
			if (typeof(T) == typeof(Event))
			{
				Publish(e, ((object)e).GetType());
			}
			else
			{
				Publish(e, typeof(T));
			}
		}

		public void Publish(Event e)
		{
			Publish(e, ((object)e).GetType());
		}

		public void Publish(Event e, System.Type eventType)
		{
			if (!base.IsDisposed)
			{
				GetSubscriptionList(eventType)?.Publish(e);
			}
		}

		public void RequestPurge(System.Type eventType)
		{
			if (!base.IsDisposed)
			{
				GetSubscriptionList(eventType)?.RequestPurge();
			}
		}

		public void RequestPurgeAll()
		{
			if (base.IsDisposed)
			{
				return;
			}
			System.Collections.Generic.IEnumerator<SubscriptionList> enumerator = ((System.Collections.Generic.IEnumerable<SubscriptionList>)SubscriptionLists.Values).GetEnumerator();
			try
			{
				while (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					enumerator.Current.RequestPurge();
				}
			}
			finally
			{
				((System.IDisposable)enumerator)?.Dispose();
			}
		}

		public override void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}
			System.Collections.Generic.IEnumerator<SubscriptionList> enumerator = ((System.Collections.Generic.IEnumerable<SubscriptionList>)SubscriptionLists.Values).GetEnumerator();
			try
			{
				while (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					enumerator.Current.Dispose();
				}
			}
			finally
			{
				((System.IDisposable)enumerator)?.Dispose();
			}
			SubscriptionLists.Clear();
		}
	}
	public sealed class SubscriptionToken : Disposable
	{
		private Subscription mSubscription;

		private readonly object[] DependentObjects;

		internal SubscriptionToken(Subscription subscription, params object[] dependentObjects)
		{
			mSubscription = subscription;
			DependentObjects = dependentObjects;
		}

		public override void Dispose(bool disposing)
		{
			if (disposing)
			{
				mSubscription.Dispose();
				mSubscription = null;
				GC.SuppressFinalize((object)this);
			}
		}
	}
	public class SubscriptionManager : Disposable, System.Collections.Generic.IEnumerable<SubscriptionToken>, System.Collections.IEnumerable
	{
		private List<SubscriptionToken> Tokens = new List<SubscriptionToken>();

		public int Count => Tokens.Count;

		public void AddSubscription(SubscriptionToken item)
		{
			if (!base.IsDisposed)
			{
				Tokens.Add(item);
			}
		}

		public bool Contains(SubscriptionToken item)
		{
			return Tokens.Contains(item);
		}

		public System.Collections.Generic.IEnumerator<SubscriptionToken> GetEnumerator()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return (System.Collections.Generic.IEnumerator<SubscriptionToken>)(object)Tokens.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return (System.Collections.IEnumerator)(object)Tokens.GetEnumerator();
		}

		public void KillSubscription(SubscriptionToken item)
		{
			if (Tokens.Remove(item))
			{
				item.Dispose();
			}
		}

		public void CancelAllSubscriptions()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			if (Tokens.Count <= 0)
			{
				return;
			}
			Enumerator<SubscriptionToken> enumerator = Interlocked.Exchange<List<SubscriptionToken>>(ref Tokens, new List<SubscriptionToken>()).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.Dispose();
				}
			}
			finally
			{
				((System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
			}
		}

		public override void Dispose(bool disposing)
		{
			if (disposing)
			{
				CancelAllSubscriptions();
			}
		}
	}
}
namespace IDS.Core.Collections
{
	public class ConcurrentHashSet<T> : System.Collections.Generic.ICollection<T>, System.Collections.Generic.IEnumerable<T>, System.Collections.IEnumerable
	{
		private readonly ConcurrentDictionary<T, byte> Collection = new ConcurrentDictionary<T, byte>();

		public int Count => Collection.Count;

		public bool IsReadOnly => false;

		public bool Contains(T item)
		{
			return Collection.ContainsKey(item);
		}

		public void Clear()
		{
			Collection.Clear();
		}

		public void Add(T item)
		{
			Collection.AddOrUpdate(item, (byte)0, (Func<T, byte, byte>)((T k, byte v) => 0));
		}

		public bool Remove(T item)
		{
			byte b = default(byte);
			return Collection.TryRemove(item, ref b);
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return (System.Collections.IEnumerator)((System.Collections.Generic.IEnumerable<T>)Collection.Keys).GetEnumerator();
		}

		public System.Collections.Generic.IEnumerator<T> GetEnumerator()
		{
			return ((System.Collections.Generic.IEnumerable<T>)Collection.Keys).GetEnumerator();
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			Collection.Keys.CopyTo(array, arrayIndex);
		}
	}
}
