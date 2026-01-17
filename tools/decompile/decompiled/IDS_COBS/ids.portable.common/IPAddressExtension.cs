using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace IDS.Portable.Common;

public static class IPAddressExtension
{
	[StructLayout((LayoutKind)3)]
	[CompilerGenerated]
	private struct _003CParseNameAsync_003Ed__0 : IAsyncStateMachine
	{
		public int _003C_003E1__state;

		public AsyncTaskMethodBuilder<IPAddress> _003C_003Et__builder;

		public string name;

		private global::System.Exception _003CipParseException_003E5__2;

		private TaskAwaiter<IPAddress[]> _003C_003Eu__1;

		private void MoveNext()
		{
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			int num = _003C_003E1__state;
			IPAddress result = default(IPAddress);
			try
			{
				if (num == 0)
				{
					goto IL_0036;
				}
				int num2 = 0;
				object obj;
				try
				{
					result = IPAddress.Parse(name);
				}
				catch (global::System.Exception ex)
				{
					obj = ex;
					num2 = 1;
					goto IL_0022;
				}
				goto end_IL_0007;
				IL_0036:
				try
				{
					TaskAwaiter<IPAddress[]> val;
					if (num != 0)
					{
						if (name == null)
						{
							throw new ArgumentNullException("name");
						}
						val = Dns.GetHostAddressesAsync(name).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (_003C_003E1__state = 0);
							_003C_003Eu__1 = val;
							_003C_003Et__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IPAddress[]>, _003CParseNameAsync_003Ed__0>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = _003C_003Eu__1;
						_003C_003Eu__1 = default(TaskAwaiter<IPAddress[]>);
						num = (_003C_003E1__state = -1);
					}
					result = Enumerable.First<IPAddress>((global::System.Collections.Generic.IEnumerable<IPAddress>)val.GetResult());
				}
				catch
				{
					throw _003CipParseException_003E5__2;
				}
				goto end_IL_0007;
				IL_0022:
				if (num2 == 1)
				{
					_003CipParseException_003E5__2 = (global::System.Exception)obj;
					goto IL_0036;
				}
				end_IL_0007:;
			}
			catch (global::System.Exception exception)
			{
				_003C_003E1__state = -2;
				_003C_003Et__builder.SetException(exception);
				return;
			}
			_003C_003E1__state = -2;
			_003C_003Et__builder.SetResult(result);
		}

		[DebuggerHidden]
		private void SetStateMachine(IAsyncStateMachine stateMachine)
		{
			_003C_003Et__builder.SetStateMachine(stateMachine);
		}
	}

	[AsyncStateMachine(typeof(_003CParseNameAsync_003Ed__0))]
	public static async global::System.Threading.Tasks.Task<IPAddress> ParseNameAsync(this string name)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		IPAddress result = default(IPAddress);
		object obj;
		int num;
		try
		{
			result = IPAddress.Parse(name);
			return result;
		}
		catch (global::System.Exception ex)
		{
			obj = ex;
			num = 1;
		}
		if (num == 1)
		{
			global::System.Exception ipParseException = (global::System.Exception)obj;
			try
			{
				if (name == null)
				{
					throw new ArgumentNullException("name");
				}
				return Enumerable.First<IPAddress>((global::System.Collections.Generic.IEnumerable<IPAddress>)(await Dns.GetHostAddressesAsync(name)));
			}
			catch
			{
				throw ipParseException;
			}
		}
		return result;
	}
}
