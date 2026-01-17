using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using EnumsNET;
using IDS.App.Configuration.Json;
using IDS.App.FeatureFlags.Serializers;
using IDS.Portable.Common;
using IDS.Portable.Common.Extensions;
using LaunchDarkly.Sdk;
using LaunchDarkly.Sdk.Client;
using LaunchDarkly.Sdk.Client.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ids.portable.common.Extensions;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(/*Could not decode attribute arguments.*/)]
[assembly: InternalsVisibleTo("IDS.App.FeatureFlags.Tests")]
[assembly: TargetFramework(".NETCoreApp,Version=v9.0", FrameworkDisplayName = ".NET 9.0")]
[assembly: AssemblyCompany("IDS.App.Configuration")]
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyCopyright("2023-2025 Lippert, Inc")]
[assembly: AssemblyDescription("App Configuration Library")]
[assembly: AssemblyFileVersion("3.0.1.0")]
[assembly: AssemblyInformationalVersion("3.0.1+9ea22f91a8c41512d534dc3a9ba8da416c4ecb7c")]
[assembly: AssemblyProduct("IDS.App.Configuration")]
[assembly: AssemblyTitle("IDS.App.Configuration")]
[assembly: AssemblyMetadata("RepositoryUrl", "https://github.com/lci-ids/ids.app.configuration.git")]
[assembly: AssemblyVersion("3.0.1.0")]
[module: RefSafetyRules(11)]
namespace Microsoft.Maui.Controls.Generated
{
	[GeneratedCode("Microsoft.Maui.Controls.BindingSourceGen, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "1.0.0.0")]
	internal static class GeneratedBindingInterceptors
	{
		private static bool ShouldUseSetter(BindingMode mode, BindableProperty bindableProperty)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Invalid comparison between Unknown and I4
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Invalid comparison between Unknown and I4
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Invalid comparison between Unknown and I4
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Invalid comparison between Unknown and I4
			if ((int)mode != 3 && (int)mode != 1)
			{
				if ((int)mode == 0)
				{
					if ((int)bindableProperty.DefaultBindingMode != 3)
					{
						return (int)bindableProperty.DefaultBindingMode == 1;
					}
					return true;
				}
				return false;
			}
			return true;
		}

		private static bool ShouldUseSetter(BindingMode mode)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Invalid comparison between Unknown and I4
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Invalid comparison between Unknown and I4
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Invalid comparison between Unknown and I4
			if ((int)mode != 3 && (int)mode != 1)
			{
				return (int)mode == 0;
			}
			return true;
		}
	}
}
namespace IDS.App.Configuration.Json
{
	public interface IJsonSerializable
	{
		bool TryJsonSerialize(out string? json, ILogger logger);

		string? TryJsonSerialize(ILogger logger);
	}
	public abstract class JsonSerializable<TObject> : IJsonSerializable where TObject : class
	{
		public bool TryJsonSerialize(out string? json, ILogger logger)
		{
			try
			{
				json = JsonConvert.SerializeObject((object)this, (Formatting)1);
				return true;
			}
			catch (global::System.Exception ex)
			{
				LoggerExtensions.LogError(logger, "Error trying to JSON serialize: {ExMessage}\\n{ExStackTrace}", new object[2] { ex.Message, ex.StackTrace });
				json = null;
				return false;
			}
		}

		public string? TryJsonSerialize(ILogger logger)
		{
			if (TryJsonSerialize(out string json, logger))
			{
				return json;
			}
			return null;
		}

		public static bool TryJsonDeserialize(string json, out TObject? obj, ILogger logger)
		{
			try
			{
				obj = JsonConvert.DeserializeObject<TObject>(json);
				return true;
			}
			catch (global::System.Exception ex)
			{
				LoggerExtensions.LogError(logger, "Error trying to JSON deserialize: {ExMessage}\\n{ExStackTrace}", new object[2] { ex.Message, ex.StackTrace });
				obj = null;
				return false;
			}
		}

		public static TObject? TryJsonDeserialize(string json, ILogger logger)
		{
			if (TryJsonDeserialize(json, out TObject obj, logger))
			{
				return obj;
			}
			return null;
		}
	}
}
namespace IDS.App.Settings
{
	public class AppSettingsPlatform
	{
	}
	public interface IAppSettingsPlatform
	{
		global::System.Threading.Tasks.Task<bool> ShowNativeDialog(string title, string text, string okText, string cancelText, string iconUrl, Uri okLink);
	}
}
namespace IDS.App.FeatureFlags
{
	public static class AppBuilder
	{
		public static MauiAppBuilder UseAppFeatureFlags(this MauiAppBuilder builder, Action<FeatureFlagBuilder> configure, string ldKey, string uniqueDeviceId)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			FeatureFlagBuilder featureFlagBuilder = new FeatureFlagBuilder();
			configure.Invoke(featureFlagBuilder);
			Context val = Context.New(ldKey);
			TimeSpan val2 = TimeSpan.FromSeconds(5L);
			LdClient.InitAsync(ldKey, (AutoEnvAttributes)0, val, val2);
			ServiceCollectionServiceExtensions.AddSingleton<IFeatureFlagProvider>(builder.Services, (IFeatureFlagProvider)featureFlagBuilder);
			ServiceCollectionServiceExtensions.AddSingleton<IAppFeatureFlagService>(builder.Services, (Func<IServiceProvider, IAppFeatureFlagService>)((IServiceProvider serviceProvider) => new AppFeatureFlagService(featureFlagBuilder, ServiceProviderServiceExtensions.GetRequiredService<ILoggerFactory>(serviceProvider), featureFlagBuilder.AppFeatureEnvironment, uniqueDeviceId)));
			return builder;
		}
	}
	public enum AppFeatureExposedVia
	{
		None,
		Experimental,
		Developer,
		Test
	}
	[AttributeUsage(/*Could not decode attribute arguments.*/)]
	public class AppFeatureDefaultAttribute : global::System.Attribute
	{
		public readonly AppFeatureExposedVia ExposeVia;

		public string Title;

		public string Description;

		[field: CompilerGenerated]
		public AppFeatureDefaultOptions Options
		{
			[CompilerGenerated]
			get;
		}

		[field: CompilerGenerated]
		public bool Mutable
		{
			[CompilerGenerated]
			get;
		}

		public AppFeatureDefaultAttribute(AppFeatureExposedVia exposedVia, string title, string description, AppFeatureDefaultOptions options = AppFeatureDefaultOptions.None)
		{
			Options = options;
			ExposeVia = exposedVia;
			Title = title ?? string.Empty;
			Description = description ?? string.Empty;
			Console.WriteLine($"options.HasFlag(AppFeatureDefaultOptions.EnableForDebug): {((global::System.Enum)options).HasFlag((global::System.Enum)AppFeatureDefaultOptions.EnableForDebug)}");
			Mutable = !((global::System.Enum)options).HasFlag((global::System.Enum)AppFeatureDefaultOptions.Immutable);
		}

		public AppFeatureDefaultAttribute(AppFeatureDefaultOptions options = AppFeatureDefaultOptions.None)
			: this(AppFeatureExposedVia.None, string.Empty, string.Empty, options)
		{
		}
	}
	[AttributeUsage(/*Could not decode attribute arguments.*/)]
	public class AppFeatureReleasedAttribute : AppFeatureDefaultAttribute
	{
		public AppFeatureReleasedAttribute()
			: base(AppFeatureDefaultOptions.Enable | AppFeatureDefaultOptions.Immutable)
		{
		}
	}
	[Flags]
	public enum AppFeatureDefaultOptions : ushort
	{
		None = 0,
		EnableForRelease = 1,
		EnableForDebug = 2,
		Immutable = 0x8000,
		Enable = 3
	}
	public enum AppFeatureEnableState
	{
		Disabled,
		Enabled,
		EnabledByLaunchDarkly,
		EnabledByUser
	}
	public enum AppFeatureEnvironment
	{
		Any,
		OneControl,
		OneControlWeRv,
		OneControlBt,
		Compass,
		Keystone,
		OneControlOctp,
		Brinkley,
		EmberLink,
		VersaLink
	}
	[AttributeUsage(/*Could not decode attribute arguments.*/)]
	public class AppFeatureSupportedEnvironmentsAttribute : global::System.Attribute
	{
		[field: CompilerGenerated]
		public global::System.Collections.Generic.IReadOnlyList<AppFeatureEnvironment> Environments
		{
			[CompilerGenerated]
			get;
		}

		public AppFeatureSupportedEnvironmentsAttribute(params AppFeatureEnvironment[] environments)
		{
			List<AppFeatureEnvironment> obj;
			if (environments.Length != 0)
			{
				obj = Enumerable.ToList<AppFeatureEnvironment>((global::System.Collections.Generic.IEnumerable<AppFeatureEnvironment>)environments);
			}
			else
			{
				int num = 1;
				obj = new List<AppFeatureEnvironment>(num);
				CollectionsMarshal.SetCount<AppFeatureEnvironment>(obj, num);
				global::System.Span<AppFeatureEnvironment> span = CollectionsMarshal.AsSpan<AppFeatureEnvironment>(obj);
				int num2 = 0;
				span[num2] = AppFeatureEnvironment.Any;
			}
			Environments = (global::System.Collections.Generic.IReadOnlyList<AppFeatureEnvironment>)obj;
		}
	}
	public static class AppFeatureExtension
	{
		public static AppFeatureDefaultAttribute? GetAppFeatureEnabledAttribute(this global::System.Enum appFeature, ILogger logger)
		{
			try
			{
				FieldInfo field = ((object)appFeature).GetType().GetField(((object)appFeature).ToString());
				return (field != null) ? CustomAttributeExtensions.GetCustomAttribute<AppFeatureDefaultAttribute>((MemberInfo)(object)field, false) : null;
			}
			catch (global::System.Exception ex)
			{
				LoggerExtensions.LogError(logger, "Error looking up {AppFeatureDefaultAttributeName} for {AppFeature}: {ExMessage}", new object[3] { "AppFeatureDefaultAttribute", appFeature, ex.Message });
				return null;
			}
		}

		public static AppFeatureSupportedEnvironmentsAttribute? GetAppFeatureSupportedEnvironmentsAttribute(this global::System.Enum appFeature, ILogger logger)
		{
			try
			{
				FieldInfo field = ((object)appFeature).GetType().GetField(((object)appFeature).ToString());
				return (field != null) ? CustomAttributeExtensions.GetCustomAttribute<AppFeatureSupportedEnvironmentsAttribute>((MemberInfo)(object)field, false) : null;
			}
			catch (global::System.Exception ex)
			{
				LoggerExtensions.LogError(logger, "Error looking up {AppFeatureSupportedEnvironmentsAttributeName} for {AppFeature}: {ExMessage}", new object[3] { "AppFeatureSupportedEnvironmentsAttribute", appFeature, ex.Message });
				return null;
			}
		}

		public static AppFeatureLaunchDarklyAttribute? GetAppFeatureLaunchDarklyAttribute(this global::System.Enum appFeature, ILogger logger)
		{
			try
			{
				FieldInfo field = ((object)appFeature).GetType().GetField(((object)appFeature).ToString());
				return (field != null) ? CustomAttributeExtensions.GetCustomAttribute<AppFeatureLaunchDarklyAttribute>((MemberInfo)(object)field, false) : null;
			}
			catch (global::System.Exception ex)
			{
				LoggerExtensions.LogError(logger, "Error looking up {AppFeatureLaunchDarklyAttributeName} for {AppFeature}: {ExMessage}", new object[3] { "AppFeatureLaunchDarklyAttribute", appFeature, ex.Message });
				return null;
			}
		}

		public static AppFeatureReleasedAttribute? GetAppFeatureReleasedAttribute(this global::System.Enum appFeature, ILogger logger)
		{
			try
			{
				FieldInfo field = ((object)appFeature).GetType().GetField(((object)appFeature).ToString());
				return (field != null) ? CustomAttributeExtensions.GetCustomAttribute<AppFeatureReleasedAttribute>((MemberInfo)(object)field, false) : null;
			}
			catch (global::System.Exception ex)
			{
				LoggerExtensions.LogError(logger, "Error looking up {AppFeatureReleasedAttributeName} for {AppFeature}: {ExMessage}", new object[3] { "AppFeatureReleasedAttribute", appFeature, ex.Message });
				return null;
			}
		}
	}
	public interface IAppFeatureFlagService
	{
		global::System.Collections.Generic.IEnumerable<global::System.Enum> FeatureFlags { get; }

		bool IsFeatureMutable(global::System.Enum feature);

		bool TrySetFeatureEnabledState(global::System.Enum feature, AppFeatureEnableState enableState);

		bool IsFeatureEnabled(global::System.Enum feature, AppFeatureEnvironment? environment = null);

		global::System.Threading.Tasks.Task<bool> UpdateUserInfoAsync(string emailAddress);

		AppFeatureEnableState GetFeatureEnableState(global::System.Enum feature, AppFeatureEnvironment? environmentOptional = null);

		global::System.Collections.Generic.IEnumerable<ValueTuple<global::System.Enum, AppFeatureDefaultAttribute>> GetAppFeatures(AppFeatureExposedVia exposedVia, AppFeatureEnvironment filterByEnvironment, bool excludeReleasedExperimentalFeatures);

		void SetUniqueDeviceId(string uniqueDeviceId);
	}
	internal class AppFeatureFlagService : IAppFeatureFlagService
	{
		[CompilerGenerated]
		private sealed class <GetAppFeatures>d__34 : global::System.Collections.Generic.IEnumerable<ValueTuple<global::System.Enum, AppFeatureDefaultAttribute>>, global::System.Collections.IEnumerable, global::System.Collections.Generic.IEnumerator<ValueTuple<global::System.Enum, AppFeatureDefaultAttribute>>, global::System.Collections.IEnumerator, global::System.IDisposable
		{
			private int <>1__state;

			private ValueTuple<global::System.Enum, AppFeatureDefaultAttribute> <>2__current;

			private int <>l__initialThreadId;

			public AppFeatureFlagService <>4__this;

			private AppFeatureExposedVia exposedVia;

			public AppFeatureExposedVia <>3__exposedVia;

			private bool excludeReleasedExperimentalFeatures;

			public bool <>3__excludeReleasedExperimentalFeatures;

			private AppFeatureEnvironment filterByEnvironment;

			public AppFeatureEnvironment <>3__filterByEnvironment;

			private global::System.Collections.Generic.IEnumerator<global::System.Enum> <>7__wrap1;

			ValueTuple<global::System.Enum, AppFeatureDefaultAttribute> global::System.Collections.Generic.IEnumerator<ValueTuple<global::System.Enum, AppFeatureDefaultAttribute>>.Current
			{
				[DebuggerHidden]
				get
				{
					//IL_0001: Unknown result type (might be due to invalid IL or missing references)
					return <>2__current;
				}
			}

			object global::System.Collections.IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					//IL_0001: Unknown result type (might be due to invalid IL or missing references)
					return <>2__current;
				}
			}

			[DebuggerHidden]
			public <GetAppFeatures>d__34(int <>1__state)
			{
				this.<>1__state = <>1__state;
				<>l__initialThreadId = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void global::System.IDisposable.Dispose()
			{
				int num = <>1__state;
				if (num == -3 || num == 1)
				{
					try
					{
					}
					finally
					{
						<>m__Finally1();
					}
				}
				<>7__wrap1 = null;
				<>1__state = -2;
			}

			private bool MoveNext()
			{
				//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
				try
				{
					int num = <>1__state;
					AppFeatureFlagService appFeatureFlagService = <>4__this;
					switch (num)
					{
					default:
						return false;
					case 0:
						<>1__state = -1;
						<>7__wrap1 = Enumerable.SelectMany<global::System.Type, global::System.Enum>((global::System.Collections.Generic.IEnumerable<global::System.Type>)appFeatureFlagService._provider.FeatureFlags, (Func<global::System.Type, global::System.Collections.Generic.IEnumerable<global::System.Enum>>)((global::System.Type featureFlags) => Enumerable.OfType<global::System.Enum>((global::System.Collections.IEnumerable)Enumerable.Select<EnumMember, global::System.Enum>((global::System.Collections.Generic.IEnumerable<EnumMember>)Enums.GetMembers(featureFlags, (EnumMemberSelection)0), (Func<EnumMember, global::System.Enum>)((EnumMember member) => member.Value as global::System.Enum))))).GetEnumerator();
						<>1__state = -3;
						break;
					case 1:
						<>1__state = -3;
						break;
					}
					while (((global::System.Collections.IEnumerator)<>7__wrap1).MoveNext())
					{
						global::System.Enum current = <>7__wrap1.Current;
						AppFeatureDefaultAttribute attribute = EnumExtensions.GetAttribute<AppFeatureDefaultAttribute>(current, false);
						if (attribute != null && attribute.ExposeVia == exposedVia)
						{
							global::System.Collections.Generic.IReadOnlyList<AppFeatureEnvironment> supportedEnvironments = GetSupportedEnvironments(((object)current).GetType(), current);
							if (!((((global::System.Collections.Generic.IReadOnlyCollection<AppFeatureEnvironment>)supportedEnvironments).Count == 0) & excludeReleasedExperimentalFeatures) && (filterByEnvironment == AppFeatureEnvironment.Any || Enumerable.Contains<AppFeatureEnvironment>((global::System.Collections.Generic.IEnumerable<AppFeatureEnvironment>)supportedEnvironments, filterByEnvironment)) && (!((exposedVia == AppFeatureExposedVia.Experimental) & excludeReleasedExperimentalFeatures) || current.GetAppFeatureReleasedAttribute(appFeatureFlagService._logger) == null))
							{
								<>2__current = new ValueTuple<global::System.Enum, AppFeatureDefaultAttribute>(current, attribute);
								<>1__state = 1;
								return true;
							}
						}
					}
					<>m__Finally1();
					<>7__wrap1 = null;
					return false;
				}
				catch
				{
					//try-fault
					((global::System.IDisposable)this).Dispose();
					throw;
				}
			}

			bool global::System.Collections.IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			private void <>m__Finally1()
			{
				<>1__state = -1;
				if (<>7__wrap1 != null)
				{
					((global::System.IDisposable)<>7__wrap1).Dispose();
				}
			}

			[DebuggerHidden]
			void global::System.Collections.IEnumerator.Reset()
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			global::System.Collections.Generic.IEnumerator<ValueTuple<global::System.Enum, AppFeatureDefaultAttribute>> global::System.Collections.Generic.IEnumerable<ValueTuple<global::System.Enum, AppFeatureDefaultAttribute>>.GetEnumerator()
			{
				<GetAppFeatures>d__34 <GetAppFeatures>d__;
				if (<>1__state == -2 && <>l__initialThreadId == Environment.CurrentManagedThreadId)
				{
					<>1__state = 0;
					<GetAppFeatures>d__ = this;
				}
				else
				{
					<GetAppFeatures>d__ = new <GetAppFeatures>d__34(0)
					{
						<>4__this = <>4__this
					};
				}
				<GetAppFeatures>d__.exposedVia = <>3__exposedVia;
				<GetAppFeatures>d__.filterByEnvironment = <>3__filterByEnvironment;
				<GetAppFeatures>d__.excludeReleasedExperimentalFeatures = <>3__excludeReleasedExperimentalFeatures;
				return <GetAppFeatures>d__;
			}

			[DebuggerHidden]
			global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
			{
				return (global::System.Collections.IEnumerator)((global::System.Collections.Generic.IEnumerable<ValueTuple<global::System.Enum, AppFeatureDefaultAttribute>>)this).GetEnumerator();
			}
		}

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <InitLaunchDarkly>d__20 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public AppFeatureFlagService <>4__this;

			public string uniqueDeviceId;

			public Configuration launchDarklyMobileKey;

			private TaskAwaiter<LdClient> <>u__1;

			private void MoveNext()
			{
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_009e: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				//IL_005b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0062: Unknown result type (might be due to invalid IL or missing references)
				//IL_0063: Unknown result type (might be due to invalid IL or missing references)
				//IL_0069: Unknown result type (might be due to invalid IL or missing references)
				//IL_006e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0083: Unknown result type (might be due to invalid IL or missing references)
				//IL_0085: Unknown result type (might be due to invalid IL or missing references)
				//IL_0133: Unknown result type (might be due to invalid IL or missing references)
				//IL_0138: Unknown result type (might be due to invalid IL or missing references)
				//IL_0144: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				AppFeatureFlagService appFeatureFlagService = <>4__this;
				try
				{
					TimeSpan val = default(TimeSpan);
					if (num != 0)
					{
						val = TimeSpan.FromSeconds(10L);
					}
					try
					{
						TaskAwaiter<LdClient> val2;
						if (num == 0)
						{
							val2 = <>u__1;
							<>u__1 = default(TaskAwaiter<LdClient>);
							num = (<>1__state = -1);
							goto IL_00ba;
						}
						appFeatureFlagService._uniqueDeviceId = uniqueDeviceId;
						if (Interlocked.Exchange(ref appFeatureFlagService._didInitLaunchDarkly, 1) != 1 && appFeatureFlagService._featureFlagService == null)
						{
							Context val3 = Context.Builder(uniqueDeviceId).Build();
							val2 = LdClient.InitAsync(launchDarklyMobileKey, val3, val).GetAwaiter();
							if (!val2.IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val2;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<LdClient>, <InitLaunchDarkly>d__20>(ref val2, ref this);
								return;
							}
							goto IL_00ba;
						}
						goto end_IL_001b;
						IL_00ba:
						LdClient result = val2.GetResult();
						appFeatureFlagService._featureFlagService = (ILdClient?)(object)result;
						LoggerExtensions.LogInformation(appFeatureFlagService._logger, "Feature Flag Configured For {TestEnvironment} for app {UniqueDeviceId} SUCCESS", new object[2]
						{
							appFeatureFlagService.IsTestEnvironment ? "Test" : "Production",
							uniqueDeviceId
						});
						appFeatureFlagService._featureFlagService.FlagTracker.FlagValueChanged += appFeatureFlagService.FlagTrackerOnFlagValueChanged;
						global::System.Collections.Generic.IEnumerator<KeyValuePair<string, LdValue>> enumerator = ((global::System.Collections.Generic.IEnumerable<KeyValuePair<string, LdValue>>)appFeatureFlagService._featureFlagService.AllFlags()).GetEnumerator();
						try
						{
							while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
							{
								KeyValuePair<string, LdValue> current = enumerator.Current;
								appFeatureFlagService.UpdateFeatureFlag(current.Key, current.Value);
							}
						}
						finally
						{
							if (num < 0)
							{
								((global::System.IDisposable)enumerator)?.Dispose();
							}
						}
						Watchdog? updateFeatureWatchdog = appFeatureFlagService._updateFeatureWatchdog;
						if (updateFeatureWatchdog != null)
						{
							updateFeatureWatchdog.TryPet(true);
						}
						end_IL_001b:;
					}
					catch (global::System.Exception ex)
					{
						LoggerExtensions.LogWarning(appFeatureFlagService._logger, ex, "Feature Flag Configured For {TestEnvironment} for app {UniqueDeviceId} FAILED: {Message}", new object[3]
						{
							appFeatureFlagService.IsTestEnvironment ? "Test" : "Production",
							uniqueDeviceId,
							ex.Message
						});
					}
				}
				catch (global::System.Exception exception)
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
		private struct <UpdateUserInfoAsync>d__22 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<bool> <>t__builder;

			public string emailAddress;

			public AppFeatureFlagService <>4__this;

			private TaskAwaiter<bool> <>u__1;

			private void MoveNext()
			{
				//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0044: Unknown result type (might be due to invalid IL or missing references)
				//IL_0083: Unknown result type (might be due to invalid IL or missing references)
				//IL_0088: Unknown result type (might be due to invalid IL or missing references)
				//IL_008e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0094: Unknown result type (might be due to invalid IL or missing references)
				//IL_0099: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				AppFeatureFlagService appFeatureFlagService = <>4__this;
				bool result;
				try
				{
					if (num != 0 && string.IsNullOrWhiteSpace(emailAddress))
					{
						result = false;
					}
					else
					{
						try
						{
							TaskAwaiter<bool> val;
							if (num == 0)
							{
								val = <>u__1;
								<>u__1 = default(TaskAwaiter<bool>);
								num = (<>1__state = -1);
								goto IL_00e5;
							}
							Context context = LdClient.Instance.Context;
							LdValue value = ((Context)(ref context)).GetValue("email");
							string asString = ((LdValue)(ref value)).AsString;
							if (asString == null || !asString.Equals(emailAddress))
							{
								Context val2 = Context.Builder(appFeatureFlagService._uniqueDeviceId).Set("email", emailAddress).Build();
								val = LdClient.Instance.IdentifyAsync(val2).GetAwaiter();
								if (!val.IsCompleted)
								{
									num = (<>1__state = 0);
									<>u__1 = val;
									<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, <UpdateUserInfoAsync>d__22>(ref val, ref this);
									return;
								}
								goto IL_00e5;
							}
							result = false;
							goto end_IL_0026;
							IL_00e5:
							result = val.GetResult();
							end_IL_0026:;
						}
						catch (global::System.Exception ex)
						{
							LoggerExtensions.LogWarning(appFeatureFlagService._logger, ex, "Could not update user email address {EmailAddress} for app {UniqueDeviceId} FAILED: {Message}", new object[3] { emailAddress, appFeatureFlagService._uniqueDeviceId, ex.Message });
							result = false;
						}
					}
				}
				catch (global::System.Exception exception)
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

		private const int NotifyFeatureUpdateTimeMs = 250;

		private const int NotifyFeatureUpdateTimeMaxMs = 1000;

		private const string EmailAddressKey = "email";

		private readonly ILogger _logger;

		private string _uniqueDeviceId = string.Empty;

		private ILdClient? _featureFlagService;

		private readonly Watchdog? _updateFeatureWatchdog;

		private AppFeaturesSerializable _loadedFeatures;

		private readonly IFeatureFlagProvider _provider;

		private readonly AppFeatureEnvironment _appFeatureEnvironment;

		private int _didInitLaunchDarkly;

		private readonly ConcurrentDictionary<global::System.Enum, AppFeatureEnableState> _featureState;

		[field: CompilerGenerated]
		public bool IsTestEnvironment
		{
			[CompilerGenerated]
			get;
		}

		public global::System.Collections.Generic.IEnumerable<global::System.Enum> FeatureFlags => (global::System.Collections.Generic.IEnumerable<global::System.Enum>)_featureState.Keys;

		public AppFeatureFlagService(IFeatureFlagProvider provider, ILoggerFactory loggerFactory, AppFeatureEnvironment appFeatureEnvironment, string uniqueDeviceId)
		{
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Expected O, but got Unknown
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Expected O, but got Unknown
			IsTestEnvironment = provider.IsDebugMode;
			_logger = (ILogger)(object)LoggerFactoryExtensions.CreateLogger<AppFeatureFlagService>(loggerFactory);
			_provider = provider;
			_updateFeatureWatchdog = null;
			_appFeatureEnvironment = appFeatureEnvironment;
			_featureState = new ConcurrentDictionary<global::System.Enum, AppFeatureEnableState>();
			_loadedFeatures = ((AppFeaturesSerializable.TryLoad(out AppFeaturesSerializable appFeatures, _logger) && appFeatures != null) ? appFeatures : AppFeaturesSerializable.MakeAppFeaturesSerializable((IReadOnlyDictionary<global::System.Enum, AppFeatureEnableState>)(object)new Dictionary<global::System.Enum, AppFeatureEnableState>(), _logger));
			_uniqueDeviceId = uniqueDeviceId;
			global::System.Collections.Generic.IEnumerator<global::System.Enum> enumerator = Enumerable.SelectMany<global::System.Type, global::System.Enum>((global::System.Collections.Generic.IEnumerable<global::System.Type>)provider.FeatureFlags, (Func<global::System.Type, global::System.Collections.Generic.IEnumerable<global::System.Enum>>)((global::System.Type featureFlags) => Enumerable.OfType<global::System.Enum>((global::System.Collections.IEnumerable)Enumerable.Select<EnumMember, global::System.Enum>((global::System.Collections.Generic.IEnumerable<EnumMember>)Enums.GetMembers(featureFlags, (EnumMemberSelection)0), (Func<EnumMember, global::System.Enum>)((EnumMember member) => member.Value as global::System.Enum))))).GetEnumerator();
			try
			{
				while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
				{
					global::System.Enum current = enumerator.Current;
					TrySetFeatureEnabledState(((object)current).GetType(), current, GetFeatureFlagState(current), init: true);
					AppFeatureEnableState? appFeatureEnabledState = _loadedFeatures.GetAppFeatureEnabledState(current, _logger);
					if (appFeatureEnabledState.HasValue)
					{
						TrySetFeatureEnabledState(((object)current).GetType(), current, appFeatureEnabledState.Value, init: false);
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator)?.Dispose();
			}
			_updateFeatureWatchdog = new Watchdog(TimeSpan.FromMilliseconds(250L, 0L), TimeSpan.FromMilliseconds(1000L, 0L), new Action(FeaturesUpdated), true);
			_updateFeatureWatchdog.TryPet(true);
		}

		public void SetUniqueDeviceId(string uniqueDeviceId)
		{
			_uniqueDeviceId = uniqueDeviceId;
		}

		private AppFeatureEnableState GetFeatureFlagState(global::System.Enum featureFlag)
		{
			AppFeatureDefaultAttribute appFeatureEnabledAttribute = featureFlag.GetAppFeatureEnabledAttribute(_logger);
			if (appFeatureEnabledAttribute == null)
			{
				return AppFeatureEnableState.Disabled;
			}
			if (((global::System.Enum)appFeatureEnabledAttribute.Options).HasFlag((global::System.Enum)AppFeatureDefaultOptions.EnableForDebug) && _provider.IsDebugMode)
			{
				return AppFeatureEnableState.Enabled;
			}
			if (((global::System.Enum)appFeatureEnabledAttribute.Options).HasFlag((global::System.Enum)AppFeatureDefaultOptions.EnableForRelease))
			{
				return AppFeatureEnableState.Enabled;
			}
			return AppFeatureEnableState.Disabled;
		}

		private void FeaturesUpdated()
		{
			AppFeaturesSerializable appFeaturesSerializable = AppFeaturesSerializable.MakeAppFeaturesSerializable((IReadOnlyDictionary<global::System.Enum, AppFeatureEnableState>)(object)_featureState, _logger);
			if (!appFeaturesSerializable.Equals(_loadedFeatures))
			{
				AppFeaturesSerializable.TrySave(appFeaturesSerializable, _logger);
			}
			_loadedFeatures = appFeaturesSerializable;
		}

		[AsyncStateMachine(typeof(<InitLaunchDarkly>d__20))]
		[Obsolete("Xamarin Forms is deprecated, use AppBuilder for MAUI")]
		private global::System.Threading.Tasks.Task InitLaunchDarkly(Configuration launchDarklyMobileKey, string uniqueDeviceId)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			<InitLaunchDarkly>d__20 <InitLaunchDarkly>d__ = default(<InitLaunchDarkly>d__20);
			<InitLaunchDarkly>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<InitLaunchDarkly>d__.<>4__this = this;
			<InitLaunchDarkly>d__.launchDarklyMobileKey = launchDarklyMobileKey;
			<InitLaunchDarkly>d__.uniqueDeviceId = uniqueDeviceId;
			<InitLaunchDarkly>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <InitLaunchDarkly>d__.<>t__builder)).Start<<InitLaunchDarkly>d__20>(ref <InitLaunchDarkly>d__);
			return ((AsyncTaskMethodBuilder)(ref <InitLaunchDarkly>d__.<>t__builder)).Task;
		}

		private void FlagTrackerOnFlagValueChanged(object sender, FlagValueChangeEvent eventArgs)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			UpdateFeatureFlag(((FlagValueChangeEvent)(ref eventArgs)).Key, ((FlagValueChangeEvent)(ref eventArgs)).NewValue);
		}

		[AsyncStateMachine(typeof(<UpdateUserInfoAsync>d__22))]
		public async global::System.Threading.Tasks.Task<bool> UpdateUserInfoAsync(string emailAddress)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrWhiteSpace(emailAddress))
			{
				return false;
			}
			try
			{
				Context context = LdClient.Instance.Context;
				LdValue value = ((Context)(ref context)).GetValue("email");
				if (((LdValue)(ref value)).AsString?.Equals(emailAddress) ?? false)
				{
					return false;
				}
				Context val = Context.Builder(_uniqueDeviceId).Set("email", emailAddress).Build();
				return await LdClient.Instance.IdentifyAsync(val);
			}
			catch (global::System.Exception ex)
			{
				LoggerExtensions.LogWarning(_logger, ex, "Could not update user email address {EmailAddress} for app {UniqueDeviceId} FAILED: {Message}", new object[3] { emailAddress, _uniqueDeviceId, ex.Message });
				return false;
			}
		}

		private global::System.Enum? FindAppFeatureForLaunchDarklyFeature(string flagKey)
		{
			global::System.Collections.Generic.IEnumerator<global::System.Enum> enumerator = ((global::System.Collections.Generic.IEnumerable<global::System.Enum>)_featureState.Keys).GetEnumerator();
			try
			{
				while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
				{
					global::System.Enum current = enumerator.Current;
					string text = current.GetAppFeatureLaunchDarklyAttribute(_logger)?.KeyName;
					if (text != null && string.Compare(flagKey, text, (StringComparison)5) == 0)
					{
						return current;
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator)?.Dispose();
			}
			LoggerExtensions.LogDebug(_logger, "Looking for feature flag {FlagKey} wasn't found in {Name}", new object[2] { flagKey, "FindAppFeatureForLaunchDarklyFeature" });
			return null;
		}

		private bool UpdateFeatureFlag(string flagKey, LdValue launchDarklyValue)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Invalid comparison between Unknown and I4
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			if ((int)((LdValue)(ref launchDarklyValue)).Type != 1)
			{
				LoggerExtensions.LogWarning(_logger, "Ignoring event arg type {Type}", new object[1] { ((LdValue)(ref launchDarklyValue)).Type });
				return false;
			}
			bool asBool = ((LdValue)(ref launchDarklyValue)).AsBool;
			return UpdateFeatureFlag(flagKey, asBool);
		}

		private bool UpdateFeatureFlag(string flagKey, bool enabled)
		{
			global::System.Enum obj = FindAppFeatureForLaunchDarklyFeature(flagKey);
			if (obj == null)
			{
				LoggerExtensions.LogDebug(_logger, "Feature Flag Unable to update because {FlagKey} wasn't found in {Name}", new object[2] { flagKey, "AppFeatureEnableState" });
				return false;
			}
			AppFeatureEnableState appFeatureEnableState = GetFeatureEnableState(obj, AppFeatureEnvironment.Any);
			switch (appFeatureEnableState)
			{
			case AppFeatureEnableState.Disabled:
				if (enabled)
				{
					appFeatureEnableState = AppFeatureEnableState.EnabledByLaunchDarkly;
				}
				break;
			case AppFeatureEnableState.EnabledByLaunchDarkly:
				if (!enabled)
				{
					appFeatureEnableState = AppFeatureEnableState.Disabled;
				}
				break;
			case AppFeatureEnableState.Enabled:
			case AppFeatureEnableState.EnabledByUser:
				if (!enabled)
				{
					LoggerExtensions.LogInformation(_logger, "Feature Flag Updated {FlagKey} was ignored as feature is enabled by {FeatureEnableState}", new object[2] { flagKey, appFeatureEnableState });
				}
				break;
			default:
				LoggerExtensions.LogInformation(_logger, "Feature Flag Updated {FlagKey} was ignored as unknown current feature enable state {FeatureEnableState}", new object[2] { flagKey, appFeatureEnableState });
				break;
			}
			bool num = TrySetFeatureEnabledState(obj, appFeatureEnableState);
			if (num)
			{
				AppFeaturesUpdatedMessage.SendMessage(obj);
			}
			return num;
		}

		public bool IsFeatureMutable(global::System.Enum feature)
		{
			return feature.GetAppFeatureEnabledAttribute(_logger)?.Mutable ?? false;
		}

		private static global::System.Collections.Generic.IReadOnlyList<AppFeatureEnvironment> GetSupportedEnvironments(global::System.Type T, global::System.Enum feature)
		{
			object obj = Enumerable.FirstOrDefault<AppFeatureSupportedEnvironmentsAttribute>(Enumerable.Select<EnumMember, AppFeatureSupportedEnvironmentsAttribute>(Enumerable.Where<EnumMember>(Enumerable.AsEnumerable<EnumMember>((global::System.Collections.Generic.IEnumerable<EnumMember>)Enums.GetMembers(T, (EnumMemberSelection)0)), (Func<EnumMember, bool>)((EnumMember member) => member.Value.Equals((object)feature))), (Func<EnumMember, AppFeatureSupportedEnvironmentsAttribute>)((EnumMember member) => Enumerable.FirstOrDefault<AppFeatureSupportedEnvironmentsAttribute>(Enumerable.OfType<AppFeatureSupportedEnvironmentsAttribute>((global::System.Collections.IEnumerable)member.Attributes)))))?.Environments;
			if (obj == null)
			{
				obj = new List<AppFeatureEnvironment>();
				((List<AppFeatureEnvironment>)obj).Add(AppFeatureEnvironment.Any);
			}
			return (global::System.Collections.Generic.IReadOnlyList<AppFeatureEnvironment>)obj;
		}

		public AppFeatureEnableState GetFeatureEnableState(global::System.Enum feature, AppFeatureEnvironment? environmentOptional = null)
		{
			return GetFeatureEnableState(((object)feature).GetType(), feature);
		}

		private AppFeatureEnableState GetFeatureEnableState(global::System.Type T, global::System.Enum feature)
		{
			if (_appFeatureEnvironment != AppFeatureEnvironment.Any)
			{
				global::System.Collections.Generic.IReadOnlyList<AppFeatureEnvironment> supportedEnvironments = GetSupportedEnvironments(T, feature);
				if (!Enumerable.Contains<AppFeatureEnvironment>((global::System.Collections.Generic.IEnumerable<AppFeatureEnvironment>)supportedEnvironments, AppFeatureEnvironment.Any) && !Enumerable.Any<AppFeatureEnvironment>((global::System.Collections.Generic.IEnumerable<AppFeatureEnvironment>)supportedEnvironments, (Func<AppFeatureEnvironment, bool>)([CompilerGenerated] (AppFeatureEnvironment item) => ((object)item/*cast due to .constrained prefix*/).Equals((object)_appFeatureEnvironment))))
				{
					return AppFeatureEnableState.Disabled;
				}
			}
			AppFeatureEnableState result = default(AppFeatureEnableState);
			if (_featureState.TryGetValue(feature, ref result))
			{
				return result;
			}
			return AppFeatureEnableState.Disabled;
		}

		public bool IsFeatureEnabled(global::System.Enum feature, AppFeatureEnvironment? environment = null)
		{
			return GetFeatureEnableState(feature, environment) switch
			{
				AppFeatureEnableState.Disabled => false, 
				AppFeatureEnableState.Enabled => true, 
				AppFeatureEnableState.EnabledByLaunchDarkly => true, 
				AppFeatureEnableState.EnabledByUser => true, 
				_ => false, 
			};
		}

		public bool TrySetFeatureEnabledState(global::System.Enum feature, AppFeatureEnableState enableState)
		{
			return TrySetFeatureEnabledState(((object)feature).GetType(), feature, enableState, init: false);
		}

		private bool TrySetFeatureEnabledState(global::System.Type type, global::System.Enum feature, AppFeatureEnableState enableState, bool init)
		{
			if (init)
			{
				LoggerExtensions.LogInformation(_logger, "Feature {Feature} init {EnableState}", new object[2] { feature, enableState });
			}
			else
			{
				AppFeatureEnableState featureEnableState = GetFeatureEnableState(feature, AppFeatureEnvironment.Any);
				if (featureEnableState == enableState)
				{
					return true;
				}
				if (!IsFeatureMutable(feature))
				{
					LoggerExtensions.LogWarning(_logger, "Feature {Feature} is not allowed to be changed from {CurrentEnableState} to {EnableState} because it is immutable", new object[3] { feature, featureEnableState, enableState });
					return false;
				}
				LoggerExtensions.LogInformation(_logger, "Feature {Feature} changing from {CurrentEnableState} to {EnableState}", new object[3] { feature, featureEnableState, enableState });
			}
			_featureState[feature] = enableState;
			Watchdog? updateFeatureWatchdog = _updateFeatureWatchdog;
			if (updateFeatureWatchdog != null)
			{
				updateFeatureWatchdog.TryPet(true);
			}
			return true;
		}

		[IteratorStateMachine(typeof(<GetAppFeatures>d__34))]
		public global::System.Collections.Generic.IEnumerable<ValueTuple<global::System.Enum, AppFeatureDefaultAttribute>> GetAppFeatures(AppFeatureExposedVia exposedVia, AppFeatureEnvironment filterByEnvironment, bool excludeReleasedExperimentalFeatures)
		{
			global::System.Collections.Generic.IEnumerator<global::System.Enum> enumerator = Enumerable.SelectMany<global::System.Type, global::System.Enum>((global::System.Collections.Generic.IEnumerable<global::System.Type>)_provider.FeatureFlags, (Func<global::System.Type, global::System.Collections.Generic.IEnumerable<global::System.Enum>>)((global::System.Type featureFlags) => Enumerable.OfType<global::System.Enum>((global::System.Collections.IEnumerable)Enumerable.Select<EnumMember, global::System.Enum>((global::System.Collections.Generic.IEnumerable<EnumMember>)Enums.GetMembers(featureFlags, (EnumMemberSelection)0), (Func<EnumMember, global::System.Enum>)((EnumMember member) => member.Value as global::System.Enum))))).GetEnumerator();
			try
			{
				while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
				{
					global::System.Enum current = enumerator.Current;
					AppFeatureDefaultAttribute attribute = EnumExtensions.GetAttribute<AppFeatureDefaultAttribute>(current, false);
					if (attribute != null && attribute.ExposeVia == exposedVia)
					{
						global::System.Collections.Generic.IReadOnlyList<AppFeatureEnvironment> supportedEnvironments = GetSupportedEnvironments(((object)current).GetType(), current);
						if (!(((global::System.Collections.Generic.IReadOnlyCollection<AppFeatureEnvironment>)supportedEnvironments).Count == 0 && excludeReleasedExperimentalFeatures) && (filterByEnvironment == AppFeatureEnvironment.Any || Enumerable.Contains<AppFeatureEnvironment>((global::System.Collections.Generic.IEnumerable<AppFeatureEnvironment>)supportedEnvironments, filterByEnvironment)) && (!(exposedVia == AppFeatureExposedVia.Experimental && excludeReleasedExperimentalFeatures) || current.GetAppFeatureReleasedAttribute(_logger) == null))
						{
							yield return new ValueTuple<global::System.Enum, AppFeatureDefaultAttribute>(current, attribute);
						}
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator)?.Dispose();
			}
		}
	}
	[AttributeUsage(/*Could not decode attribute arguments.*/)]
	public class AppFeatureLaunchDarklyAttribute : global::System.Attribute
	{
		[field: CompilerGenerated]
		public string KeyName
		{
			[CompilerGenerated]
			get;
		}

		public AppFeatureLaunchDarklyAttribute(string keyName)
		{
			KeyName = keyName;
		}
	}
	public class AppFeaturesUpdatedMessage : ValueChangedMessage<global::System.Enum>
	{
		public static void SendMessage(global::System.Enum value)
		{
			IMessengerExtensions.Send<AppFeaturesUpdatedMessage>((IMessenger)(object)WeakReferenceMessenger.Default, new AppFeaturesUpdatedMessage(value));
		}

		public AppFeaturesUpdatedMessage(global::System.Enum value)
			: base(value)
		{
		}
	}
	public sealed class FeatureFlagBuilder : IFeatureFlagProvider
	{
		[field: CompilerGenerated]
		List<global::System.Type> IFeatureFlagProvider.FeatureFlags
		{
			[CompilerGenerated]
			get;
		} = new List<global::System.Type>();

		[field: CompilerGenerated]
		public Configuration LaunchDarklyConfig
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public string DeviceId
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public bool IsDebugMode
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		[field: CompilerGenerated]
		public AppFeatureEnvironment AppFeatureEnvironment
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public FeatureFlagBuilder SetDebugMode(bool debug)
		{
			IsDebugMode = debug;
			return this;
		}

		internal FeatureFlagBuilder()
		{
		}

		public FeatureFlagBuilder AddFeatureFlagEnum<T>() where T : global::System.Enum
		{
			((IFeatureFlagProvider)this).FeatureFlags.Add(typeof(T));
			return this;
		}

		public FeatureFlagBuilder SetLaunchDarklyMobileKey(string key)
		{
			LaunchDarklyConfig = Configuration.Builder(key, (AutoEnvAttributes)0).Build();
			return this;
		}

		public FeatureFlagBuilder AddDeviceId(string deviceId)
		{
			DeviceId = deviceId;
			return this;
		}

		public FeatureFlagBuilder SetAppFeatureEnvironment(AppFeatureEnvironment appFeatureEnvironment)
		{
			AppFeatureEnvironment = appFeatureEnvironment;
			return this;
		}
	}
	public interface IFeatureFlagProvider
	{
		List<global::System.Type> FeatureFlags { get; }

		Configuration LaunchDarklyConfig { get; }

		string DeviceId { get; }

		bool IsDebugMode { get; }
	}
}
namespace IDS.App.FeatureFlags.Serializers
{
	public class AppFeatureFlagsSerializersException : global::System.Exception
	{
		public AppFeatureFlagsSerializersException(string message)
			: base(message)
		{
		}
	}
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public sealed class AppFeaturesSerializable : JsonSerializable<AppFeaturesSerializable>, IEquatable<AppFeaturesSerializable>
	{
		public const string AppFeaturesFilename = "OneControlAppFeaturesV1.json";

		public const string V1EnumName = "AppFeature";

		[JsonProperty("FeaturesEnabled")]
		private readonly IReadOnlyDictionary<string, string> _featuresEnabled;

		[JsonProperty]
		[JsonConverter(typeof(VersionConverter))]
		[field: CompilerGenerated]
		public Version AppFeaturesVersion
		{
			[CompilerGenerated]
			get;
		}

		[JsonConstructor]
		private AppFeaturesSerializable(Version appFeaturesVersion, Dictionary<string, string> featuresEnabled)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			if (appFeaturesVersion.Major < 2)
			{
				_featuresEnabled = (IReadOnlyDictionary<string, string>)(object)new Dictionary<string, string>();
				Enumerator<string, string> enumerator = featuresEnabled.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, string> current = enumerator.Current;
						string text = "AppFeature." + current.Key;
						(_featuresEnabled as Dictionary<string, string>)?.Add(text, current.Value);
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
				}
			}
			else
			{
				_featuresEnabled = (IReadOnlyDictionary<string, string>)(object)featuresEnabled;
			}
			AppFeaturesVersion = appFeaturesVersion;
		}

		public static AppFeaturesSerializable MakeAppFeaturesSerializable(IReadOnlyDictionary<global::System.Enum, AppFeatureEnableState> featuresEnabled, ILogger logger)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			Version appFeaturesVersion = new Version(2, 0);
			Dictionary<string, string> val = new Dictionary<string, string>();
			global::System.Collections.Generic.IEnumerator<KeyValuePair<global::System.Enum, AppFeatureEnableState>> enumerator = ((global::System.Collections.Generic.IEnumerable<KeyValuePair<global::System.Enum, AppFeatureEnableState>>)featuresEnabled).GetEnumerator();
			try
			{
				while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
				{
					KeyValuePair<global::System.Enum, AppFeatureEnableState> current = enumerator.Current;
					try
					{
						string text = ((MemberInfo)((object)current.Key).GetType()).Name + "." + global::System.Enum.GetName(((object)current.Key).GetType(), (object)current.Key);
						string text2 = global::System.Enum.GetName(typeof(AppFeatureEnableState), (object)current.Value) ?? throw new global::System.Exception($"Feature Value is Invalid {current.Key}");
						val[text] = text2;
					}
					catch (global::System.Exception ex)
					{
						LoggerExtensions.LogWarning(logger, "MakeAppFeaturesSerializable ignoring feature for {EntryKey} = {EntryValue} because {ExMessage}", new object[3] { current.Key, current.Value, ex.Message });
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator)?.Dispose();
			}
			return new AppFeaturesSerializable(appFeaturesVersion, val);
		}

		public AppFeatureEnableState? GetAppFeatureEnabledState(global::System.Enum featureFlag, ILogger logger)
		{
			string text = ((MemberInfo)((object)featureFlag).GetType()).Name + "." + global::System.Enum.GetName(((object)featureFlag).GetType(), (object)featureFlag);
			string text2 = default(string);
			if (!_featuresEnabled.TryGetValue(text, ref text2))
			{
				LoggerExtensions.LogWarning(logger, "Unable to find feature {FeatureFlag}. Feature disabled", new object[1] { featureFlag });
				return null;
			}
			AppFeatureEnableState appFeatureEnableState = default(AppFeatureEnableState);
			if (!Enum<AppFeatureEnableState>.TryConvert((object)text2, ref appFeatureEnableState))
			{
				LoggerExtensions.LogWarning(logger, "Disabling {FeatureKey} as unknown feature state was given {FeatureValue}", new object[2] { text, text2 });
				appFeatureEnableState = AppFeatureEnableState.Disabled;
			}
			return appFeatureEnableState;
		}

		public static bool TrySave(AppFeaturesSerializable appFeatures, ILogger logger)
		{
			try
			{
				string text = JsonConvert.SerializeObject((object)appFeatures, (Formatting)1);
				FileExtension.SaveText("OneControlAppFeaturesV1.json", text, (FileIoLocation)0);
				LoggerExtensions.LogInformation(logger, "Saved OneControlAppFeaturesV1.json", global::System.Array.Empty<object>());
				return true;
			}
			catch (global::System.Exception ex)
			{
				LoggerExtensions.LogError(logger, "Unable to save given {AppFeaturesSerializableName}: {ExMessage}", new object[2] { "AppFeaturesSerializable", ex.Message });
				return false;
			}
		}

		public static bool TryLoad(out AppFeaturesSerializable? appFeatures, ILogger logger)
		{
			try
			{
				appFeatures = null;
				string text = FileExtension.LoadText("OneControlAppFeaturesV1.json", (FileIoLocation)0);
				if (string.IsNullOrWhiteSpace(text))
				{
					throw new AppFeatureFlagsSerializersException("json is null or empty");
				}
				appFeatures = JsonConvert.DeserializeObject<AppFeaturesSerializable>(text);
				LoggerExtensions.LogInformation(logger, "Loaded OneControlAppFeaturesV1.json", global::System.Array.Empty<object>());
			}
			catch (global::System.Exception ex)
			{
				LoggerExtensions.LogWarning(logger, "Unable to load {AppFeaturesSerializableName}: {ExMessage}", new object[2] { "AppFeaturesSerializable", ex.Message });
				appFeatures = null;
			}
			return appFeatures != null;
		}

		public bool Equals(AppFeaturesSerializable? other)
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			if (other == null)
			{
				return false;
			}
			if (this == other)
			{
				return true;
			}
			if (!AppFeaturesVersion.Equals(other.AppFeaturesVersion))
			{
				return false;
			}
			if (((global::System.Collections.Generic.IReadOnlyCollection<KeyValuePair<string, string>>)_featuresEnabled).Count != ((global::System.Collections.Generic.IReadOnlyCollection<KeyValuePair<string, string>>)other._featuresEnabled).Count)
			{
				return false;
			}
			global::System.Collections.Generic.IEnumerator<KeyValuePair<string, string>> enumerator = ((global::System.Collections.Generic.IEnumerable<KeyValuePair<string, string>>)_featuresEnabled).GetEnumerator();
			try
			{
				string text = default(string);
				while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
				{
					KeyValuePair<string, string> current = enumerator.Current;
					if (!other._featuresEnabled.TryGetValue(current.Key, ref text))
					{
						return false;
					}
					if (current.Value != text)
					{
						return false;
					}
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator)?.Dispose();
			}
			return true;
		}

		public override bool Equals(object? obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (this == obj)
			{
				return true;
			}
			if (obj.GetType() == ((object)this).GetType())
			{
				return Equals(obj as AppFeaturesSerializable);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine<Version, IReadOnlyDictionary<string, string>>(AppFeaturesVersion, _featuresEnabled);
		}
	}
}
