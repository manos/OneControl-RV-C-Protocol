using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using System.Xml;
using App.Common.DualActionPage;
using App.Common.Interfaces;
using App.Common.Models;
using App.Common.Models.Device;
using App.Common.Pages.Pairing.CollectWiFiInfo;
using App.Common.Pages.Pairing.Connect;
using App.Common.Pages.Pairing.ConnectToGateway;
using App.Common.Pages.Pairing.Connections.Rv;
using App.Common.Pages.Pairing.Error;
using App.Common.Pages.Pairing.Interfaces;
using App.Common.Pages.Pairing.Resources;
using App.Common.Pages.Pairing.ScanWithCamera;
using App.Common.Pages.Pairing.SearchForDevices;
using App.Common.Pages.Pairing.SearchForDevices.SignalView;
using App.Common.Pages.Pairing.Success;
using App.Common.Pages.Pairing.Warning;
using App.Common.Pages.Permissions.Helpers;
using App.Toolkit.Extensions;
using App.Toolkit.Threading;
using AppAnalyticsLib;
using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using CommunityToolkit.Mvvm.Input;
using IDS.Core;
using IDS.Core.IDS_CAN;
using IDS.Core.Tasks;
using IDS.Net.Wifi;
using IDS.Portable.CAN;
using IDS.Portable.CAN.Com;
using IDS.Portable.Common;
using IDS.Portable.Devices.TPMS;
using IDS.Portable.LogicalDevice;
using IDS.Portable.LogicalDevice.Json;
using IDS.Portable.LogicalDevice.LogicalDevice;
using IDS.Portable.Services.ConnectionServices;
using IDS.UI.Controls;
using IDS.UI.Converters;
using IDS.UI.Extensions;
using IDS.UI.MarkupExtensions.BindingProxy;
using IDS.UI.MarkupExtensions.DisabledColor;
using IDS.UI.Modal.Dialog;
using IDS.UI.Pages.AppBar;
using IDS.UI.Pages.AppBar.Top;
using IDS.UI.Resources.Fonts.FontAwesome;
using IDS.UI.Views.Controls;
using IDS.UI.Views.RadioButtonList;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Xaml.Internals;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using Newtonsoft.Json;
using OneControl.Devices;
using OneControl.Devices.AccessoryGateway;
using OneControl.Direct.IdsCanAccessoryBle;
using OneControl.Direct.IdsCanAccessoryBle.Mopeka;
using OneControl.Direct.IdsCanAccessoryBle.ScanResults;
using OneControl.Direct.MyRvLink;
using OneControl.Direct.MyRvLinkBle;
using OneControl.Direct.MyRvLinkTcpIp;
using OneControl.UI.DualActionPage;
using Peridot.Extensions;
using Peridot.Modals.Dialog;
using Peridot.Navigation;
using Peridot.ViewModel;
using Peridot.Window;
using Plugin.BLE.Abstractions.Contracts;
using Polly;
using Polly.Retry;
using SkiaSharp.Extended.UI.Controls;
using SkiaSharp.Extended.UI.Controls.Converters;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;
using _Microsoft.Android.Resource.Designer;
using ids.portable.ble.BleManager;
using ids.portable.ble.BleScanner;
using ids.portable.ble.Platforms.Shared.BleScanner;
using ids.portable.ble.Platforms.Shared.ScanResults;
using ids.portable.ble.ScanResults;
using ids.portable.common.ObservableCollection;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(/*Could not decode attribute arguments.*/)]
[assembly: XmlnsPrefix("http://lci1.com/schemas/app.common", "appcommon")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common", AssemblyName = "App.Common")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common.DualActionPage", AssemblyName = "App.Common")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common.Interfaces", AssemblyName = "App.Common")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common.Models", AssemblyName = "App.Common")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common.Models.Device", AssemblyName = "App.Common")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common.Pages", AssemblyName = "App.Common.Pages")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common.Pages.CustomerCare", AssemblyName = "App.Common.Pages")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common.Pages.CustomerCare.AU", AssemblyName = "App.Common.Pages")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common.Pages.Pairing")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common.Pages.Pairing.CollectWiFiInfo")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common.Pages.Pairing.Connect")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common.Pages.Pairing.ConnectToAccessory")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common.Pages.Pairing.ConnectToBluetoothGateway")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common.Pages.Pairing.ConnectToGateway")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common.Pages.Pairing.ConnectToWiFiGateway")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common.Pages.Pairing.Connections")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common.Pages.Pairing.Connections.Rv")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common.Pages.Pairing.DualActionPage")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common.Pages.Pairing.Error")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common.Pages.Pairing.Interfaces")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common.Pages.Pairing.ScanWithCamera")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common.Pages.Pairing.SearchForDevices")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common.Pages.Pairing.SearchForDevices.SignalView")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common.Pages.Pairing.Success")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common.Pages.Pairing.Warning")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common.Pages.Permissions", AssemblyName = "App.Common.Pages.Permissions")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common.Pages.Permissions.Developer", AssemblyName = "App.Common.Pages.Permissions")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common.Pages.Permissions.Helpers", AssemblyName = "App.Common.Pages.Permissions")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common.Pages.Permissions.Interfaces", AssemblyName = "App.Common.Pages.Permissions")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common.Pages.Permissions.ViewModels", AssemblyName = "App.Common.Pages.Permissions")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common.Pages.Permissions.ViewModels.Permissions", AssemblyName = "App.Common.Pages.Permissions")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common.Pages.Permissions.ViewModels.Services", AssemblyName = "App.Common.Pages.Permissions")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "App.Common.Pages.ScanVINPage", AssemblyName = "App.Common.Pages")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "OneControl", AssemblyName = "App.Common")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "OneControl.UI", AssemblyName = "App.Common")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/app.common", "OneControl.UI.DualActionPage", AssemblyName = "App.Common")]
[assembly: TargetFramework(".NETCoreApp,Version=v9.0", FrameworkDisplayName = ".NET 9.0")]
[assembly: AssemblyCompany("Lippert")]
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyCopyright("Copyright © Lippert 2025")]
[assembly: AssemblyDescription("Common pairing page library for applications")]
[assembly: AssemblyFileVersion("2.0.1.0")]
[assembly: AssemblyInformationalVersion("2.0.1+725282cf8bfc379e50b50ecf0bbc74730a893b45")]
[assembly: AssemblyProduct("App.Common.Pages.Pairing")]
[assembly: AssemblyTitle("App.Common.Pages.Pairing")]
[assembly: AssemblyMetadata("RepositoryUrl", "https://github.com/lci-ids/app.common")]
[assembly: TargetPlatform("Android35.0")]
[assembly: SupportedOSPlatform("Android21.0")]
[assembly: XamlResourceId("App.Common.Pages.Pairing.CollectWiFiInfo.CollectionWiFiInfoPage.xaml", "CollectWiFiInfo/CollectionWiFiInfoPage.xaml", typeof(CollectWiFiInfoPage))]
[assembly: XamlResourceId("App.Common.Pages.Pairing.Error.ErrorPage.xaml", "Error/ErrorPage.xaml", typeof(ErrorPage))]
[assembly: XamlResourceId("App.Common.Pages.Pairing.ScanWithCamera.ScanView.xaml", "ScanWithCamera/ScanView.xaml", typeof(ScanView))]
[assembly: XamlResourceId("App.Common.Pages.Pairing.ScanWithCamera.ScanWithCameraPage.xaml", "ScanWithCamera/ScanWithCameraPage.xaml", typeof(ScanWithCameraPage))]
[assembly: XamlResourceId("App.Common.Pages.Pairing.SearchForDevices.SearchForDevicesPage.xaml", "SearchForDevices/SearchForDevicesPage.xaml", typeof(SearchForDevicesPage))]
[assembly: XamlResourceId("App.Common.Pages.Pairing.SearchForDevices.SignalView.SignalView.xaml", "SearchForDevices/SignalView/SignalView.xaml", typeof(SignalView))]
[assembly: XamlResourceId("App.Common.Pages.Pairing.Success.SuccessPage.xaml", "Success/SuccessPage.xaml", typeof(SuccessPage))]
[assembly: XamlResourceId("App.Common.Pages.Pairing.Warning.WarningPage.xaml", "Warning/WarningPage.xaml", typeof(WarningPage))]
[assembly: AssemblyVersion("2.0.1.0")]
[module: RefSafetyRules(11)]
namespace CommunityToolkit.Mvvm.ComponentModel.__Internals
{
	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[DebuggerNonUserCode]
	[ExcludeFromCodeCoverage]
	[EditorBrowsable(/*Could not decode attribute arguments.*/)]
	[Obsolete("This type is not intended to be used directly by user code")]
	internal static class __KnownINotifyPropertyChangingArgs
	{
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangingEventArgs Message = new PropertyChangingEventArgs("Message");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangingEventArgs Ssid = new PropertyChangingEventArgs("Ssid");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangingEventArgs Password = new PropertyChangingEventArgs("Password");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangingEventArgs SsidOnly = new PropertyChangingEventArgs("SsidOnly");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangingEventArgs IsPushToPair = new PropertyChangingEventArgs("IsPushToPair");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangingEventArgs State = new PropertyChangingEventArgs("State");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangingEventArgs Name = new PropertyChangingEventArgs("Name");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangingEventArgs StillCannotConnect = new PropertyChangingEventArgs("StillCannotConnect");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangingEventArgs HasCustomRetryText = new PropertyChangingEventArgs("HasCustomRetryText");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangingEventArgs Title = new PropertyChangingEventArgs("Title");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangingEventArgs IsScanning = new PropertyChangingEventArgs("IsScanning");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangingEventArgs BleScanResult = new PropertyChangingEventArgs("BleScanResult");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangingEventArgs Mac = new PropertyChangingEventArgs("Mac");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangingEventArgs Rssi = new PropertyChangingEventArgs("Rssi");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangingEventArgs IsSelected = new PropertyChangingEventArgs("IsSelected");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangingEventArgs IsPinPairable = new PropertyChangingEventArgs("IsPinPairable");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangingEventArgs IsPairingActive = new PropertyChangingEventArgs("IsPairingActive");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangingEventArgs SelectedSearchResult = new PropertyChangingEventArgs("SelectedSearchResult");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangingEventArgs HasResults = new PropertyChangingEventArgs("HasResults");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangingEventArgs AuxActionText = new PropertyChangingEventArgs("AuxActionText");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangingEventArgs HasAuxAction = new PropertyChangingEventArgs("HasAuxAction");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangingEventArgs AuxAction = new PropertyChangingEventArgs("AuxAction");
	}
	[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
	[DebuggerNonUserCode]
	[ExcludeFromCodeCoverage]
	[EditorBrowsable(/*Could not decode attribute arguments.*/)]
	[Obsolete("This type is not intended to be used directly by user code")]
	internal static class __KnownINotifyPropertyChangedArgs
	{
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangedEventArgs Message = new PropertyChangedEventArgs("Message");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangedEventArgs Ssid = new PropertyChangedEventArgs("Ssid");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangedEventArgs Password = new PropertyChangedEventArgs("Password");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangedEventArgs SsidOnly = new PropertyChangedEventArgs("SsidOnly");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangedEventArgs IsPushToPair = new PropertyChangedEventArgs("IsPushToPair");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangedEventArgs State = new PropertyChangedEventArgs("State");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangedEventArgs Name = new PropertyChangedEventArgs("Name");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangedEventArgs StillCannotConnect = new PropertyChangedEventArgs("StillCannotConnect");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangedEventArgs HasCustomRetryText = new PropertyChangedEventArgs("HasCustomRetryText");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangedEventArgs Title = new PropertyChangedEventArgs("Title");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangedEventArgs IsScanning = new PropertyChangedEventArgs("IsScanning");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangedEventArgs BleScanResult = new PropertyChangedEventArgs("BleScanResult");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangedEventArgs Mac = new PropertyChangedEventArgs("Mac");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangedEventArgs Rssi = new PropertyChangedEventArgs("Rssi");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangedEventArgs IsSelected = new PropertyChangedEventArgs("IsSelected");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangedEventArgs IsPinPairable = new PropertyChangedEventArgs("IsPinPairable");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangedEventArgs IsPairingActive = new PropertyChangedEventArgs("IsPairingActive");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangedEventArgs SelectedSearchResult = new PropertyChangedEventArgs("SelectedSearchResult");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangedEventArgs HasResults = new PropertyChangedEventArgs("HasResults");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangedEventArgs AuxActionText = new PropertyChangedEventArgs("AuxActionText");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangedEventArgs HasAuxAction = new PropertyChangedEventArgs("HasAuxAction");

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		[Obsolete("This field is not intended to be referenced directly by user code")]
		public static readonly PropertyChangedEventArgs AuxAction = new PropertyChangedEventArgs("AuxAction");
	}
}
namespace Microsoft.Maui.Controls
{
	[GeneratedCode("CommunityToolkit.Maui.SourceGenerators.Generators.TextColorToGenerator", "1.0.0.0")]
	[ExcludeFromCodeCoverage]
	internal static class ColorAnimationExtensions_Button
	{
		public static global::System.Threading.Tasks.Task<bool> TextColorTo(this Button element, Color color, uint rate = 16u, uint length = 250u, Easing? easing = null, CancellationToken token = default(CancellationToken))
		{
			//IL_010c: Expected O, but got Unknown
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			((CancellationToken)(ref token)).ThrowIfCancellationRequested();
			ArgumentNullException.ThrowIfNull((object)element, "element");
			ArgumentNullException.ThrowIfNull((object)color, "color");
			if (element == null)
			{
				throw new ArgumentException("Element must implement ITextStyle", "element");
			}
			if (element.TextColor == null)
			{
				Color val = (element.TextColor = Colors.Transparent);
			}
			TaskCompletionSource<bool> animationCompletionSource = new TaskCompletionSource<bool>();
			try
			{
				Animation val2 = new Animation();
				val2.Add(0.0, 1.0, GetRedTransformAnimation(element, color.Red));
				val2.Add(0.0, 1.0, GetGreenTransformAnimation(element, color.Green));
				val2.Add(0.0, 1.0, GetBlueTransformAnimation(element, color.Blue));
				val2.Add(0.0, 1.0, GetAlphaTransformAnimation(element, color.Alpha));
				val2.Commit((IAnimatable)(object)element, "TextColorTo", rate, length, easing, (Action<double, bool>)delegate
				{
					animationCompletionSource.SetResult(true);
				}, (Func<bool>)null);
			}
			catch (ArgumentException ex)
			{
				ArgumentException ex2 = ex;
				Trace.WriteLine($"{((MemberInfo)((global::System.Exception)(object)ex2).GetType()).Name} thrown in {typeof(ColorAnimationExtensions_Button).FullName}: {((global::System.Exception)(object)ex2).Message}");
				animationCompletionSource.SetResult(false);
			}
			return animationCompletionSource.Task.WaitAsync(token);
			[CompilerGenerated]
			static Animation GetAlphaTransformAnimation(Button val3, float targetAlpha)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					val3.TextColor = val3.TextColor.WithAlpha((float)v);
				}, (double)val3.TextColor.Alpha, (double)targetAlpha, (Easing)null, (Action)null);
			}
			[CompilerGenerated]
			static Animation GetBlueTransformAnimation(Button val3, float targetBlue)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					val3.TextColor = ColorConversionExtensions.WithBlue(val3.TextColor, v);
				}, (double)val3.TextColor.Blue, (double)targetBlue, (Easing)null, (Action)null);
			}
			[CompilerGenerated]
			static Animation GetGreenTransformAnimation(Button val3, float targetGreen)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					val3.TextColor = ColorConversionExtensions.WithGreen(val3.TextColor, v);
				}, (double)val3.TextColor.Green, (double)targetGreen, (Easing)null, (Action)null);
			}
			[CompilerGenerated]
			static Animation GetRedTransformAnimation(Button val3, float targetRed)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					val3.TextColor = ColorConversionExtensions.WithRed(val3.TextColor, v);
				}, (double)val3.TextColor.Red, (double)targetRed, (Easing)null, (Action)null);
			}
		}
	}
	[GeneratedCode("CommunityToolkit.Maui.SourceGenerators.Generators.TextColorToGenerator", "1.0.0.0")]
	[ExcludeFromCodeCoverage]
	internal static class ColorAnimationExtensions_DatePicker
	{
		public static global::System.Threading.Tasks.Task<bool> TextColorTo(this DatePicker element, Color color, uint rate = 16u, uint length = 250u, Easing? easing = null, CancellationToken token = default(CancellationToken))
		{
			//IL_010c: Expected O, but got Unknown
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			((CancellationToken)(ref token)).ThrowIfCancellationRequested();
			ArgumentNullException.ThrowIfNull((object)element, "element");
			ArgumentNullException.ThrowIfNull((object)color, "color");
			if (element == null)
			{
				throw new ArgumentException("Element must implement ITextStyle", "element");
			}
			if (element.TextColor == null)
			{
				Color val = (element.TextColor = Colors.Transparent);
			}
			TaskCompletionSource<bool> animationCompletionSource = new TaskCompletionSource<bool>();
			try
			{
				Animation val2 = new Animation();
				val2.Add(0.0, 1.0, GetRedTransformAnimation(element, color.Red));
				val2.Add(0.0, 1.0, GetGreenTransformAnimation(element, color.Green));
				val2.Add(0.0, 1.0, GetBlueTransformAnimation(element, color.Blue));
				val2.Add(0.0, 1.0, GetAlphaTransformAnimation(element, color.Alpha));
				val2.Commit((IAnimatable)(object)element, "TextColorTo", rate, length, easing, (Action<double, bool>)delegate
				{
					animationCompletionSource.SetResult(true);
				}, (Func<bool>)null);
			}
			catch (ArgumentException ex)
			{
				ArgumentException ex2 = ex;
				Trace.WriteLine($"{((MemberInfo)((global::System.Exception)(object)ex2).GetType()).Name} thrown in {typeof(ColorAnimationExtensions_DatePicker).FullName}: {((global::System.Exception)(object)ex2).Message}");
				animationCompletionSource.SetResult(false);
			}
			return animationCompletionSource.Task.WaitAsync(token);
			[CompilerGenerated]
			static Animation GetAlphaTransformAnimation(DatePicker val3, float targetAlpha)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					val3.TextColor = val3.TextColor.WithAlpha((float)v);
				}, (double)val3.TextColor.Alpha, (double)targetAlpha, (Easing)null, (Action)null);
			}
			[CompilerGenerated]
			static Animation GetBlueTransformAnimation(DatePicker val3, float targetBlue)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					val3.TextColor = ColorConversionExtensions.WithBlue(val3.TextColor, v);
				}, (double)val3.TextColor.Blue, (double)targetBlue, (Easing)null, (Action)null);
			}
			[CompilerGenerated]
			static Animation GetGreenTransformAnimation(DatePicker val3, float targetGreen)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					val3.TextColor = ColorConversionExtensions.WithGreen(val3.TextColor, v);
				}, (double)val3.TextColor.Green, (double)targetGreen, (Easing)null, (Action)null);
			}
			[CompilerGenerated]
			static Animation GetRedTransformAnimation(DatePicker val3, float targetRed)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					val3.TextColor = ColorConversionExtensions.WithRed(val3.TextColor, v);
				}, (double)val3.TextColor.Red, (double)targetRed, (Easing)null, (Action)null);
			}
		}
	}
	[GeneratedCode("CommunityToolkit.Maui.SourceGenerators.Generators.TextColorToGenerator", "1.0.0.0")]
	[ExcludeFromCodeCoverage]
	internal static class ColorAnimationExtensions_Editor
	{
		public static global::System.Threading.Tasks.Task<bool> TextColorTo(this Editor element, Color color, uint rate = 16u, uint length = 250u, Easing? easing = null, CancellationToken token = default(CancellationToken))
		{
			//IL_010c: Expected O, but got Unknown
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			((CancellationToken)(ref token)).ThrowIfCancellationRequested();
			ArgumentNullException.ThrowIfNull((object)element, "element");
			ArgumentNullException.ThrowIfNull((object)color, "color");
			if (element == null)
			{
				throw new ArgumentException("Element must implement ITextStyle", "element");
			}
			if (((InputView)element).TextColor == null)
			{
				Color val = (((InputView)element).TextColor = Colors.Transparent);
			}
			TaskCompletionSource<bool> animationCompletionSource = new TaskCompletionSource<bool>();
			try
			{
				Animation val2 = new Animation();
				val2.Add(0.0, 1.0, GetRedTransformAnimation(element, color.Red));
				val2.Add(0.0, 1.0, GetGreenTransformAnimation(element, color.Green));
				val2.Add(0.0, 1.0, GetBlueTransformAnimation(element, color.Blue));
				val2.Add(0.0, 1.0, GetAlphaTransformAnimation(element, color.Alpha));
				val2.Commit((IAnimatable)(object)element, "TextColorTo", rate, length, easing, (Action<double, bool>)delegate
				{
					animationCompletionSource.SetResult(true);
				}, (Func<bool>)null);
			}
			catch (ArgumentException ex)
			{
				ArgumentException ex2 = ex;
				Trace.WriteLine($"{((MemberInfo)((global::System.Exception)(object)ex2).GetType()).Name} thrown in {typeof(ColorAnimationExtensions_Editor).FullName}: {((global::System.Exception)(object)ex2).Message}");
				animationCompletionSource.SetResult(false);
			}
			return animationCompletionSource.Task.WaitAsync(token);
			[CompilerGenerated]
			static Animation GetAlphaTransformAnimation(Editor val3, float targetAlpha)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					((InputView)val3).TextColor = ((InputView)val3).TextColor.WithAlpha((float)v);
				}, (double)((InputView)val3).TextColor.Alpha, (double)targetAlpha, (Easing)null, (Action)null);
			}
			[CompilerGenerated]
			static Animation GetBlueTransformAnimation(Editor val3, float targetBlue)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					((InputView)val3).TextColor = ColorConversionExtensions.WithBlue(((InputView)val3).TextColor, v);
				}, (double)((InputView)val3).TextColor.Blue, (double)targetBlue, (Easing)null, (Action)null);
			}
			[CompilerGenerated]
			static Animation GetGreenTransformAnimation(Editor val3, float targetGreen)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					((InputView)val3).TextColor = ColorConversionExtensions.WithGreen(((InputView)val3).TextColor, v);
				}, (double)((InputView)val3).TextColor.Green, (double)targetGreen, (Easing)null, (Action)null);
			}
			[CompilerGenerated]
			static Animation GetRedTransformAnimation(Editor val3, float targetRed)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					((InputView)val3).TextColor = ColorConversionExtensions.WithRed(((InputView)val3).TextColor, v);
				}, (double)((InputView)val3).TextColor.Red, (double)targetRed, (Easing)null, (Action)null);
			}
		}
	}
	[GeneratedCode("CommunityToolkit.Maui.SourceGenerators.Generators.TextColorToGenerator", "1.0.0.0")]
	[ExcludeFromCodeCoverage]
	internal static class ColorAnimationExtensions_Entry
	{
		public static global::System.Threading.Tasks.Task<bool> TextColorTo(this Entry element, Color color, uint rate = 16u, uint length = 250u, Easing? easing = null, CancellationToken token = default(CancellationToken))
		{
			//IL_010c: Expected O, but got Unknown
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			((CancellationToken)(ref token)).ThrowIfCancellationRequested();
			ArgumentNullException.ThrowIfNull((object)element, "element");
			ArgumentNullException.ThrowIfNull((object)color, "color");
			if (element == null)
			{
				throw new ArgumentException("Element must implement ITextStyle", "element");
			}
			if (((InputView)element).TextColor == null)
			{
				Color val = (((InputView)element).TextColor = Colors.Transparent);
			}
			TaskCompletionSource<bool> animationCompletionSource = new TaskCompletionSource<bool>();
			try
			{
				Animation val2 = new Animation();
				val2.Add(0.0, 1.0, GetRedTransformAnimation(element, color.Red));
				val2.Add(0.0, 1.0, GetGreenTransformAnimation(element, color.Green));
				val2.Add(0.0, 1.0, GetBlueTransformAnimation(element, color.Blue));
				val2.Add(0.0, 1.0, GetAlphaTransformAnimation(element, color.Alpha));
				val2.Commit((IAnimatable)(object)element, "TextColorTo", rate, length, easing, (Action<double, bool>)delegate
				{
					animationCompletionSource.SetResult(true);
				}, (Func<bool>)null);
			}
			catch (ArgumentException ex)
			{
				ArgumentException ex2 = ex;
				Trace.WriteLine($"{((MemberInfo)((global::System.Exception)(object)ex2).GetType()).Name} thrown in {typeof(ColorAnimationExtensions_Entry).FullName}: {((global::System.Exception)(object)ex2).Message}");
				animationCompletionSource.SetResult(false);
			}
			return animationCompletionSource.Task.WaitAsync(token);
			[CompilerGenerated]
			static Animation GetAlphaTransformAnimation(Entry val3, float targetAlpha)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					((InputView)val3).TextColor = ((InputView)val3).TextColor.WithAlpha((float)v);
				}, (double)((InputView)val3).TextColor.Alpha, (double)targetAlpha, (Easing)null, (Action)null);
			}
			[CompilerGenerated]
			static Animation GetBlueTransformAnimation(Entry val3, float targetBlue)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					((InputView)val3).TextColor = ColorConversionExtensions.WithBlue(((InputView)val3).TextColor, v);
				}, (double)((InputView)val3).TextColor.Blue, (double)targetBlue, (Easing)null, (Action)null);
			}
			[CompilerGenerated]
			static Animation GetGreenTransformAnimation(Entry val3, float targetGreen)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					((InputView)val3).TextColor = ColorConversionExtensions.WithGreen(((InputView)val3).TextColor, v);
				}, (double)((InputView)val3).TextColor.Green, (double)targetGreen, (Easing)null, (Action)null);
			}
			[CompilerGenerated]
			static Animation GetRedTransformAnimation(Entry val3, float targetRed)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					((InputView)val3).TextColor = ColorConversionExtensions.WithRed(((InputView)val3).TextColor, v);
				}, (double)((InputView)val3).TextColor.Red, (double)targetRed, (Easing)null, (Action)null);
			}
		}
	}
	[GeneratedCode("CommunityToolkit.Maui.SourceGenerators.Generators.TextColorToGenerator", "1.0.0.0")]
	[ExcludeFromCodeCoverage]
	internal static class ColorAnimationExtensions_InputView
	{
		public static global::System.Threading.Tasks.Task<bool> TextColorTo(this InputView element, Color color, uint rate = 16u, uint length = 250u, Easing? easing = null, CancellationToken token = default(CancellationToken))
		{
			//IL_010c: Expected O, but got Unknown
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			((CancellationToken)(ref token)).ThrowIfCancellationRequested();
			ArgumentNullException.ThrowIfNull((object)element, "element");
			ArgumentNullException.ThrowIfNull((object)color, "color");
			if (element == null)
			{
				throw new ArgumentException("Element must implement ITextStyle", "element");
			}
			if (element.TextColor == null)
			{
				Color val = (element.TextColor = Colors.Transparent);
			}
			TaskCompletionSource<bool> animationCompletionSource = new TaskCompletionSource<bool>();
			try
			{
				Animation val2 = new Animation();
				val2.Add(0.0, 1.0, GetRedTransformAnimation(element, color.Red));
				val2.Add(0.0, 1.0, GetGreenTransformAnimation(element, color.Green));
				val2.Add(0.0, 1.0, GetBlueTransformAnimation(element, color.Blue));
				val2.Add(0.0, 1.0, GetAlphaTransformAnimation(element, color.Alpha));
				val2.Commit((IAnimatable)(object)element, "TextColorTo", rate, length, easing, (Action<double, bool>)delegate
				{
					animationCompletionSource.SetResult(true);
				}, (Func<bool>)null);
			}
			catch (ArgumentException ex)
			{
				ArgumentException ex2 = ex;
				Trace.WriteLine($"{((MemberInfo)((global::System.Exception)(object)ex2).GetType()).Name} thrown in {typeof(ColorAnimationExtensions_InputView).FullName}: {((global::System.Exception)(object)ex2).Message}");
				animationCompletionSource.SetResult(false);
			}
			return animationCompletionSource.Task.WaitAsync(token);
			[CompilerGenerated]
			static Animation GetAlphaTransformAnimation(InputView val3, float targetAlpha)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					val3.TextColor = val3.TextColor.WithAlpha((float)v);
				}, (double)val3.TextColor.Alpha, (double)targetAlpha, (Easing)null, (Action)null);
			}
			[CompilerGenerated]
			static Animation GetBlueTransformAnimation(InputView val3, float targetBlue)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					val3.TextColor = ColorConversionExtensions.WithBlue(val3.TextColor, v);
				}, (double)val3.TextColor.Blue, (double)targetBlue, (Easing)null, (Action)null);
			}
			[CompilerGenerated]
			static Animation GetGreenTransformAnimation(InputView val3, float targetGreen)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					val3.TextColor = ColorConversionExtensions.WithGreen(val3.TextColor, v);
				}, (double)val3.TextColor.Green, (double)targetGreen, (Easing)null, (Action)null);
			}
			[CompilerGenerated]
			static Animation GetRedTransformAnimation(InputView val3, float targetRed)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					val3.TextColor = ColorConversionExtensions.WithRed(val3.TextColor, v);
				}, (double)val3.TextColor.Red, (double)targetRed, (Easing)null, (Action)null);
			}
		}
	}
	[GeneratedCode("CommunityToolkit.Maui.SourceGenerators.Generators.TextColorToGenerator", "1.0.0.0")]
	[ExcludeFromCodeCoverage]
	internal static class ColorAnimationExtensions_Label
	{
		public static global::System.Threading.Tasks.Task<bool> TextColorTo(this Label element, Color color, uint rate = 16u, uint length = 250u, Easing? easing = null, CancellationToken token = default(CancellationToken))
		{
			//IL_010c: Expected O, but got Unknown
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			((CancellationToken)(ref token)).ThrowIfCancellationRequested();
			ArgumentNullException.ThrowIfNull((object)element, "element");
			ArgumentNullException.ThrowIfNull((object)color, "color");
			if (element == null)
			{
				throw new ArgumentException("Element must implement ITextStyle", "element");
			}
			if (element.TextColor == null)
			{
				Color val = (element.TextColor = Colors.Transparent);
			}
			TaskCompletionSource<bool> animationCompletionSource = new TaskCompletionSource<bool>();
			try
			{
				Animation val2 = new Animation();
				val2.Add(0.0, 1.0, GetRedTransformAnimation(element, color.Red));
				val2.Add(0.0, 1.0, GetGreenTransformAnimation(element, color.Green));
				val2.Add(0.0, 1.0, GetBlueTransformAnimation(element, color.Blue));
				val2.Add(0.0, 1.0, GetAlphaTransformAnimation(element, color.Alpha));
				val2.Commit((IAnimatable)(object)element, "TextColorTo", rate, length, easing, (Action<double, bool>)delegate
				{
					animationCompletionSource.SetResult(true);
				}, (Func<bool>)null);
			}
			catch (ArgumentException ex)
			{
				ArgumentException ex2 = ex;
				Trace.WriteLine($"{((MemberInfo)((global::System.Exception)(object)ex2).GetType()).Name} thrown in {typeof(ColorAnimationExtensions_Label).FullName}: {((global::System.Exception)(object)ex2).Message}");
				animationCompletionSource.SetResult(false);
			}
			return animationCompletionSource.Task.WaitAsync(token);
			[CompilerGenerated]
			static Animation GetAlphaTransformAnimation(Label val3, float targetAlpha)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					val3.TextColor = val3.TextColor.WithAlpha((float)v);
				}, (double)val3.TextColor.Alpha, (double)targetAlpha, (Easing)null, (Action)null);
			}
			[CompilerGenerated]
			static Animation GetBlueTransformAnimation(Label val3, float targetBlue)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					val3.TextColor = ColorConversionExtensions.WithBlue(val3.TextColor, v);
				}, (double)val3.TextColor.Blue, (double)targetBlue, (Easing)null, (Action)null);
			}
			[CompilerGenerated]
			static Animation GetGreenTransformAnimation(Label val3, float targetGreen)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					val3.TextColor = ColorConversionExtensions.WithGreen(val3.TextColor, v);
				}, (double)val3.TextColor.Green, (double)targetGreen, (Easing)null, (Action)null);
			}
			[CompilerGenerated]
			static Animation GetRedTransformAnimation(Label val3, float targetRed)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					val3.TextColor = ColorConversionExtensions.WithRed(val3.TextColor, v);
				}, (double)val3.TextColor.Red, (double)targetRed, (Easing)null, (Action)null);
			}
		}
	}
	[GeneratedCode("CommunityToolkit.Maui.SourceGenerators.Generators.TextColorToGenerator", "1.0.0.0")]
	[ExcludeFromCodeCoverage]
	internal static class ColorAnimationExtensions_Picker
	{
		public static global::System.Threading.Tasks.Task<bool> TextColorTo(this Picker element, Color color, uint rate = 16u, uint length = 250u, Easing? easing = null, CancellationToken token = default(CancellationToken))
		{
			//IL_010c: Expected O, but got Unknown
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			((CancellationToken)(ref token)).ThrowIfCancellationRequested();
			ArgumentNullException.ThrowIfNull((object)element, "element");
			ArgumentNullException.ThrowIfNull((object)color, "color");
			if (element == null)
			{
				throw new ArgumentException("Element must implement ITextStyle", "element");
			}
			if (element.TextColor == null)
			{
				Color val = (element.TextColor = Colors.Transparent);
			}
			TaskCompletionSource<bool> animationCompletionSource = new TaskCompletionSource<bool>();
			try
			{
				Animation val2 = new Animation();
				val2.Add(0.0, 1.0, GetRedTransformAnimation(element, color.Red));
				val2.Add(0.0, 1.0, GetGreenTransformAnimation(element, color.Green));
				val2.Add(0.0, 1.0, GetBlueTransformAnimation(element, color.Blue));
				val2.Add(0.0, 1.0, GetAlphaTransformAnimation(element, color.Alpha));
				val2.Commit((IAnimatable)(object)element, "TextColorTo", rate, length, easing, (Action<double, bool>)delegate
				{
					animationCompletionSource.SetResult(true);
				}, (Func<bool>)null);
			}
			catch (ArgumentException ex)
			{
				ArgumentException ex2 = ex;
				Trace.WriteLine($"{((MemberInfo)((global::System.Exception)(object)ex2).GetType()).Name} thrown in {typeof(ColorAnimationExtensions_Picker).FullName}: {((global::System.Exception)(object)ex2).Message}");
				animationCompletionSource.SetResult(false);
			}
			return animationCompletionSource.Task.WaitAsync(token);
			[CompilerGenerated]
			static Animation GetAlphaTransformAnimation(Picker val3, float targetAlpha)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					val3.TextColor = val3.TextColor.WithAlpha((float)v);
				}, (double)val3.TextColor.Alpha, (double)targetAlpha, (Easing)null, (Action)null);
			}
			[CompilerGenerated]
			static Animation GetBlueTransformAnimation(Picker val3, float targetBlue)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					val3.TextColor = ColorConversionExtensions.WithBlue(val3.TextColor, v);
				}, (double)val3.TextColor.Blue, (double)targetBlue, (Easing)null, (Action)null);
			}
			[CompilerGenerated]
			static Animation GetGreenTransformAnimation(Picker val3, float targetGreen)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					val3.TextColor = ColorConversionExtensions.WithGreen(val3.TextColor, v);
				}, (double)val3.TextColor.Green, (double)targetGreen, (Easing)null, (Action)null);
			}
			[CompilerGenerated]
			static Animation GetRedTransformAnimation(Picker val3, float targetRed)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					val3.TextColor = ColorConversionExtensions.WithRed(val3.TextColor, v);
				}, (double)val3.TextColor.Red, (double)targetRed, (Easing)null, (Action)null);
			}
		}
	}
	[GeneratedCode("CommunityToolkit.Maui.SourceGenerators.Generators.TextColorToGenerator", "1.0.0.0")]
	[ExcludeFromCodeCoverage]
	internal static class ColorAnimationExtensions_RadioButton
	{
		public static global::System.Threading.Tasks.Task<bool> TextColorTo(this RadioButton element, Color color, uint rate = 16u, uint length = 250u, Easing? easing = null, CancellationToken token = default(CancellationToken))
		{
			//IL_010c: Expected O, but got Unknown
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			((CancellationToken)(ref token)).ThrowIfCancellationRequested();
			ArgumentNullException.ThrowIfNull((object)element, "element");
			ArgumentNullException.ThrowIfNull((object)color, "color");
			if (element == null)
			{
				throw new ArgumentException("Element must implement ITextStyle", "element");
			}
			if (element.TextColor == null)
			{
				Color val = (element.TextColor = Colors.Transparent);
			}
			TaskCompletionSource<bool> animationCompletionSource = new TaskCompletionSource<bool>();
			try
			{
				Animation val2 = new Animation();
				val2.Add(0.0, 1.0, GetRedTransformAnimation(element, color.Red));
				val2.Add(0.0, 1.0, GetGreenTransformAnimation(element, color.Green));
				val2.Add(0.0, 1.0, GetBlueTransformAnimation(element, color.Blue));
				val2.Add(0.0, 1.0, GetAlphaTransformAnimation(element, color.Alpha));
				val2.Commit((IAnimatable)(object)element, "TextColorTo", rate, length, easing, (Action<double, bool>)delegate
				{
					animationCompletionSource.SetResult(true);
				}, (Func<bool>)null);
			}
			catch (ArgumentException ex)
			{
				ArgumentException ex2 = ex;
				Trace.WriteLine($"{((MemberInfo)((global::System.Exception)(object)ex2).GetType()).Name} thrown in {typeof(ColorAnimationExtensions_RadioButton).FullName}: {((global::System.Exception)(object)ex2).Message}");
				animationCompletionSource.SetResult(false);
			}
			return animationCompletionSource.Task.WaitAsync(token);
			[CompilerGenerated]
			static Animation GetAlphaTransformAnimation(RadioButton val3, float targetAlpha)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					val3.TextColor = val3.TextColor.WithAlpha((float)v);
				}, (double)val3.TextColor.Alpha, (double)targetAlpha, (Easing)null, (Action)null);
			}
			[CompilerGenerated]
			static Animation GetBlueTransformAnimation(RadioButton val3, float targetBlue)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					val3.TextColor = ColorConversionExtensions.WithBlue(val3.TextColor, v);
				}, (double)val3.TextColor.Blue, (double)targetBlue, (Easing)null, (Action)null);
			}
			[CompilerGenerated]
			static Animation GetGreenTransformAnimation(RadioButton val3, float targetGreen)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					val3.TextColor = ColorConversionExtensions.WithGreen(val3.TextColor, v);
				}, (double)val3.TextColor.Green, (double)targetGreen, (Easing)null, (Action)null);
			}
			[CompilerGenerated]
			static Animation GetRedTransformAnimation(RadioButton val3, float targetRed)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					val3.TextColor = ColorConversionExtensions.WithRed(val3.TextColor, v);
				}, (double)val3.TextColor.Red, (double)targetRed, (Easing)null, (Action)null);
			}
		}
	}
	[GeneratedCode("CommunityToolkit.Maui.SourceGenerators.Generators.TextColorToGenerator", "1.0.0.0")]
	[ExcludeFromCodeCoverage]
	internal static class ColorAnimationExtensions_SearchBar
	{
		public static global::System.Threading.Tasks.Task<bool> TextColorTo(this SearchBar element, Color color, uint rate = 16u, uint length = 250u, Easing? easing = null, CancellationToken token = default(CancellationToken))
		{
			//IL_010c: Expected O, but got Unknown
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			((CancellationToken)(ref token)).ThrowIfCancellationRequested();
			ArgumentNullException.ThrowIfNull((object)element, "element");
			ArgumentNullException.ThrowIfNull((object)color, "color");
			if (element == null)
			{
				throw new ArgumentException("Element must implement ITextStyle", "element");
			}
			if (((InputView)element).TextColor == null)
			{
				Color val = (((InputView)element).TextColor = Colors.Transparent);
			}
			TaskCompletionSource<bool> animationCompletionSource = new TaskCompletionSource<bool>();
			try
			{
				Animation val2 = new Animation();
				val2.Add(0.0, 1.0, GetRedTransformAnimation(element, color.Red));
				val2.Add(0.0, 1.0, GetGreenTransformAnimation(element, color.Green));
				val2.Add(0.0, 1.0, GetBlueTransformAnimation(element, color.Blue));
				val2.Add(0.0, 1.0, GetAlphaTransformAnimation(element, color.Alpha));
				val2.Commit((IAnimatable)(object)element, "TextColorTo", rate, length, easing, (Action<double, bool>)delegate
				{
					animationCompletionSource.SetResult(true);
				}, (Func<bool>)null);
			}
			catch (ArgumentException ex)
			{
				ArgumentException ex2 = ex;
				Trace.WriteLine($"{((MemberInfo)((global::System.Exception)(object)ex2).GetType()).Name} thrown in {typeof(ColorAnimationExtensions_SearchBar).FullName}: {((global::System.Exception)(object)ex2).Message}");
				animationCompletionSource.SetResult(false);
			}
			return animationCompletionSource.Task.WaitAsync(token);
			[CompilerGenerated]
			static Animation GetAlphaTransformAnimation(SearchBar val3, float targetAlpha)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					((InputView)val3).TextColor = ((InputView)val3).TextColor.WithAlpha((float)v);
				}, (double)((InputView)val3).TextColor.Alpha, (double)targetAlpha, (Easing)null, (Action)null);
			}
			[CompilerGenerated]
			static Animation GetBlueTransformAnimation(SearchBar val3, float targetBlue)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					((InputView)val3).TextColor = ColorConversionExtensions.WithBlue(((InputView)val3).TextColor, v);
				}, (double)((InputView)val3).TextColor.Blue, (double)targetBlue, (Easing)null, (Action)null);
			}
			[CompilerGenerated]
			static Animation GetGreenTransformAnimation(SearchBar val3, float targetGreen)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					((InputView)val3).TextColor = ColorConversionExtensions.WithGreen(((InputView)val3).TextColor, v);
				}, (double)((InputView)val3).TextColor.Green, (double)targetGreen, (Easing)null, (Action)null);
			}
			[CompilerGenerated]
			static Animation GetRedTransformAnimation(SearchBar val3, float targetRed)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					((InputView)val3).TextColor = ColorConversionExtensions.WithRed(((InputView)val3).TextColor, v);
				}, (double)((InputView)val3).TextColor.Red, (double)targetRed, (Easing)null, (Action)null);
			}
		}
	}
	[GeneratedCode("CommunityToolkit.Maui.SourceGenerators.Generators.TextColorToGenerator", "1.0.0.0")]
	[ExcludeFromCodeCoverage]
	internal static class ColorAnimationExtensions_TimePicker
	{
		public static global::System.Threading.Tasks.Task<bool> TextColorTo(this TimePicker element, Color color, uint rate = 16u, uint length = 250u, Easing? easing = null, CancellationToken token = default(CancellationToken))
		{
			//IL_010c: Expected O, but got Unknown
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			((CancellationToken)(ref token)).ThrowIfCancellationRequested();
			ArgumentNullException.ThrowIfNull((object)element, "element");
			ArgumentNullException.ThrowIfNull((object)color, "color");
			if (element == null)
			{
				throw new ArgumentException("Element must implement ITextStyle", "element");
			}
			if (element.TextColor == null)
			{
				Color val = (element.TextColor = Colors.Transparent);
			}
			TaskCompletionSource<bool> animationCompletionSource = new TaskCompletionSource<bool>();
			try
			{
				Animation val2 = new Animation();
				val2.Add(0.0, 1.0, GetRedTransformAnimation(element, color.Red));
				val2.Add(0.0, 1.0, GetGreenTransformAnimation(element, color.Green));
				val2.Add(0.0, 1.0, GetBlueTransformAnimation(element, color.Blue));
				val2.Add(0.0, 1.0, GetAlphaTransformAnimation(element, color.Alpha));
				val2.Commit((IAnimatable)(object)element, "TextColorTo", rate, length, easing, (Action<double, bool>)delegate
				{
					animationCompletionSource.SetResult(true);
				}, (Func<bool>)null);
			}
			catch (ArgumentException ex)
			{
				ArgumentException ex2 = ex;
				Trace.WriteLine($"{((MemberInfo)((global::System.Exception)(object)ex2).GetType()).Name} thrown in {typeof(ColorAnimationExtensions_TimePicker).FullName}: {((global::System.Exception)(object)ex2).Message}");
				animationCompletionSource.SetResult(false);
			}
			return animationCompletionSource.Task.WaitAsync(token);
			[CompilerGenerated]
			static Animation GetAlphaTransformAnimation(TimePicker val3, float targetAlpha)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					val3.TextColor = val3.TextColor.WithAlpha((float)v);
				}, (double)val3.TextColor.Alpha, (double)targetAlpha, (Easing)null, (Action)null);
			}
			[CompilerGenerated]
			static Animation GetBlueTransformAnimation(TimePicker val3, float targetBlue)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					val3.TextColor = ColorConversionExtensions.WithBlue(val3.TextColor, v);
				}, (double)val3.TextColor.Blue, (double)targetBlue, (Easing)null, (Action)null);
			}
			[CompilerGenerated]
			static Animation GetGreenTransformAnimation(TimePicker val3, float targetGreen)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					val3.TextColor = ColorConversionExtensions.WithGreen(val3.TextColor, v);
				}, (double)val3.TextColor.Green, (double)targetGreen, (Easing)null, (Action)null);
			}
			[CompilerGenerated]
			static Animation GetRedTransformAnimation(TimePicker val3, float targetRed)
			{
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				return new Animation((Action<double>)delegate(double v)
				{
					val3.TextColor = ColorConversionExtensions.WithRed(val3.TextColor, v);
				}, (double)val3.TextColor.Red, (double)targetRed, (Easing)null, (Action)null);
			}
		}
	}
}
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
namespace App.Common.Pages.Pairing
{
	public static class DefaultConnections
	{
		public static readonly RvGatewayCanConnectionTcpIpOctp DefaultRvDirectIdsCanConnectionOctp = new RvGatewayCanConnectionTcpIpOctp();

		public static readonly RvGatewayMyRvLinkConnectionTcpIpOctp DefaultRvGatewayMyRvLinkConnectionTcpIpOctp = new RvGatewayMyRvLinkConnectionTcpIpOctp();

		public static readonly RvGatewayCanConnectionDemo DefaultRvDirectConnectionDemo = new RvGatewayCanConnectionDemo();

		public static readonly RvGatewayCanConnectionNone DefaultRvDirectConnectionNone = new RvGatewayCanConnectionNone();
	}
	public enum GatewayType
	{
		Unknown,
		WiFi,
		Bluetooth,
		Aquafi,
		SonixCamera,
		WindSensor,
		BatteryMonitor,
		AntiLockBraking,
		SwayBraking
	}
	public enum ValueMatchType
	{
		Id,
		Password
	}
	public static class SharedGatewayParsingMetadata
	{
		public static readonly ReadOnlyCollection<GatewayType> TextSupportedGateways;

		public static readonly ReadOnlyCollection<GatewayType> QrSupportedGateways;

		public static readonly ReadOnlyDictionary<ValueMatchType, string> ValueMatchGroupKeys;

		public static readonly ReadOnlyDictionary<GatewayType, Regex> IdRegexDictionary;

		public static readonly ReadOnlyDictionary<GatewayType, Regex> PasswordRegexDictionary;

		public static readonly ReadOnlyDictionary<char, char> ReplacementDictionary;

		public static readonly ReadOnlyDictionary<ValueMatchType, ReadOnlyDictionary<GatewayType, Regex>> ValueMatchDictionariesDictionary;

		static SharedGatewayParsingMetadata()
		{
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Expected O, but got Unknown
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Expected O, but got Unknown
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Expected O, but got Unknown
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Expected O, but got Unknown
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Expected O, but got Unknown
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Expected O, but got Unknown
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Expected O, but got Unknown
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Expected O, but got Unknown
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Expected O, but got Unknown
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Expected O, but got Unknown
			List<GatewayType> obj = new List<GatewayType>();
			obj.Add(GatewayType.WindSensor);
			obj.Add(GatewayType.Aquafi);
			obj.Add(GatewayType.Bluetooth);
			obj.Add(GatewayType.WiFi);
			obj.Add(GatewayType.SonixCamera);
			obj.Add(GatewayType.AntiLockBraking);
			obj.Add(GatewayType.SwayBraking);
			obj.Add(GatewayType.Unknown);
			TextSupportedGateways = obj.AsReadOnly();
			List<GatewayType> obj2 = new List<GatewayType>();
			obj2.Add(GatewayType.WindSensor);
			obj2.Add(GatewayType.Aquafi);
			obj2.Add(GatewayType.Bluetooth);
			obj2.Add(GatewayType.WiFi);
			obj2.Add(GatewayType.SonixCamera);
			obj2.Add(GatewayType.BatteryMonitor);
			obj2.Add(GatewayType.AntiLockBraking);
			obj2.Add(GatewayType.SwayBraking);
			obj2.Add(GatewayType.Unknown);
			QrSupportedGateways = obj2.AsReadOnly();
			Dictionary<ValueMatchType, string> obj3 = new Dictionary<ValueMatchType, string>();
			obj3.Add(ValueMatchType.Id, "id");
			obj3.Add(ValueMatchType.Password, "pass");
			ValueMatchGroupKeys = new ReadOnlyDictionary<ValueMatchType, string>((IDictionary<ValueMatchType, string>)(object)obj3);
			Dictionary<GatewayType, Regex> obj4 = new Dictionary<GatewayType, Regex>();
			obj4.Add(GatewayType.Aquafi, new Regex("AquaFi.(?'id'[a-zA-Z0-9|]{6})", (RegexOptions)1));
			obj4.Add(GatewayType.Bluetooth, new Regex("LCIRemote|Remote(?'id'[a-zA-Z0-9|]{9})", (RegexOptions)17));
			obj4.Add(GatewayType.WiFi, new Regex("RV.(?'id'[a-zA-Z0-9|]{16})", (RegexOptions)1));
			obj4.Add(GatewayType.SonixCamera, new Regex("(?'id'INSIGHT_[a-z0-9]{6})", (RegexOptions)17));
			obj4.Add(GatewayType.WindSensor, new Regex("DT=2F&MAC=(?'id'[0-9A-Fa-f]{12})", (RegexOptions)1));
			obj4.Add(GatewayType.BatteryMonitor, new Regex("MAC=(?'id'[0-9A-Fa-f]{12})", (RegexOptions)1));
			IdRegexDictionary = new ReadOnlyDictionary<GatewayType, Regex>((IDictionary<GatewayType, Regex>)(object)obj4);
			Dictionary<GatewayType, Regex> obj5 = new Dictionary<GatewayType, Regex>();
			obj5.Add(GatewayType.Aquafi, new Regex("PASSWORD.{0,3}(?'pass'[a-zA-Z0-9|]{8})", (RegexOptions)1));
			obj5.Add(GatewayType.Bluetooth, new Regex("(?:PASSWORD|PW).{0,3}(?'pass'[0-9|]{6})", (RegexOptions)17));
			obj5.Add(GatewayType.WiFi, new Regex("PASSWORD.{0,3}(?'pass'[a-zA-Z0-9|]{8})", (RegexOptions)1));
			obj5.Add(GatewayType.AntiLockBraking, new Regex("pw=(?'pass'[a-zA-Z0-9|]{6})", (RegexOptions)1));
			PasswordRegexDictionary = new ReadOnlyDictionary<GatewayType, Regex>((IDictionary<GatewayType, Regex>)(object)obj5);
			Dictionary<char, char> obj6 = new Dictionary<char, char>();
			obj6.Add('|', '1');
			obj6.Add(' ', '_');
			ReplacementDictionary = new ReadOnlyDictionary<char, char>((IDictionary<char, char>)(object)obj6);
			Dictionary<ValueMatchType, ReadOnlyDictionary<GatewayType, Regex>> obj7 = new Dictionary<ValueMatchType, ReadOnlyDictionary<GatewayType, Regex>>();
			obj7.Add(ValueMatchType.Id, IdRegexDictionary);
			obj7.Add(ValueMatchType.Password, PasswordRegexDictionary);
			ValueMatchDictionariesDictionary = new ReadOnlyDictionary<ValueMatchType, ReadOnlyDictionary<GatewayType, Regex>>((IDictionary<ValueMatchType, ReadOnlyDictionary<GatewayType, Regex>>)(object)obj7);
		}
	}
	public class ParsedLabelTextResult
	{
		public static readonly ParsedLabelTextResult Empty = new ParsedLabelTextResult(GatewayType.Unknown, string.Empty, string.Empty);

		[field: CompilerGenerated]
		public bool IsValid
		{
			[CompilerGenerated]
			get;
		}

		[field: CompilerGenerated]
		public GatewayType GatewayType
		{
			[CompilerGenerated]
			get;
		}

		[field: CompilerGenerated]
		public string Id
		{
			[CompilerGenerated]
			get;
		}

		[field: CompilerGenerated]
		public string Password
		{
			[CompilerGenerated]
			get;
		}

		public ParsedLabelTextResult(GatewayType gatewayType, string id, string password = "")
		{
			GatewayType = gatewayType;
			Id = id;
			Password = password;
			IsValid = _CalculateIsValid();
		}

		private bool _CalculateIsValid()
		{
			if (GatewayType == GatewayType.Unknown)
			{
				if (!string.IsNullOrWhiteSpace(Id))
				{
					return !string.IsNullOrWhiteSpace(Password);
				}
				return false;
			}
			bool num = !SharedGatewayParsingMetadata.IdRegexDictionary.ContainsKey(GatewayType) || !string.IsNullOrWhiteSpace(Id);
			bool flag = !SharedGatewayParsingMetadata.PasswordRegexDictionary.ContainsKey(GatewayType) || !string.IsNullOrWhiteSpace(Password);
			return num && flag;
		}

		public virtual bool Equals(object? obj)
		{
			if (obj == null || base.GetType() != obj.GetType() || !(obj is ParsedLabelTextResult parsedLabelTextResult))
			{
				return false;
			}
			if (Id.Equals(parsedLabelTextResult.Id))
			{
				return Password.Equals(parsedLabelTextResult.Password);
			}
			return false;
		}

		public virtual int GetHashCode()
		{
			return ((object)Tuple.Create<string, string>(Id, Password)).GetHashCode();
		}

		public virtual string ToString()
		{
			return $"Id: {Id}, Gateway Type: {GatewayType}, Valid?: {IsValid}";
		}
	}
	public class LabelTextParser
	{
		private const string LogTag = "LabelTextParser";

		private const string QrIdKey = "devid";

		private const string QrPasswordKey = "pw";

		public static string OldWiFiGatewayQrText = "qr*1y";

		private GatewayType _suspectedGatewayType;

		[field: CompilerGenerated]
		public bool IsQrOnly
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			set;
		}

		private Dictionary<ValueMatchType, Func<string, GatewayType, string>> SanitizationMethodsDictionary
		{
			get
			{
				Dictionary<ValueMatchType, Func<string, GatewayType, string>> obj = new Dictionary<ValueMatchType, Func<string, GatewayType, string>>();
				obj.Add(ValueMatchType.Id, (Func<string, GatewayType, string>)SanitizeId);
				obj.Add(ValueMatchType.Password, (Func<string, GatewayType, string>)SanitizePassword);
				return obj;
			}
		}

		public ParsedLabelTextResult ParseLabelText(string imageText, string qrText)
		{
			if (string.IsNullOrWhiteSpace(imageText) && string.IsNullOrWhiteSpace(qrText))
			{
				return ParsedLabelTextResult.Empty;
			}
			ParsedLabelTextResult parsedLabelTextResult = ParsedLabelTextResult.Empty;
			if (!IsQrOnly)
			{
				parsedLabelTextResult = (SharedGatewayParsingMetadata.TextSupportedGateways.Contains(_suspectedGatewayType) ? ParseImageText(imageText) : ParsedLabelTextResult.Empty);
			}
			ParsedLabelTextResult parsedLabelTextResult2 = ((qrText == null || !qrText.EndsWith(OldWiFiGatewayQrText, (StringComparison)3)) ? (SharedGatewayParsingMetadata.QrSupportedGateways.Contains(_suspectedGatewayType) ? ParseQrText(qrText) : ParsedLabelTextResult.Empty) : (parsedLabelTextResult.IsValid ? parsedLabelTextResult : new ParsedLabelTextResult(GatewayType.WiFi, OldWiFiGatewayQrText, "_")));
			if (((object)parsedLabelTextResult).Equals((object)ParsedLabelTextResult.Empty) && ((object)parsedLabelTextResult2).Equals((object)ParsedLabelTextResult.Empty))
			{
				return ParsedLabelTextResult.Empty;
			}
			if (((object)parsedLabelTextResult2).Equals((object)ParsedLabelTextResult.Empty))
			{
				return parsedLabelTextResult;
			}
			return parsedLabelTextResult2;
		}

		private ParsedLabelTextResult ParseImageText(string imageText)
		{
			if (string.IsNullOrWhiteSpace(imageText))
			{
				return ParsedLabelTextResult.Empty;
			}
			bool flag = true;
			Dictionary<ValueMatchType, string> val = new Dictionary<ValueMatchType, string>();
			try
			{
				foreach (GatewayType value in global::System.Enum.GetValues(typeof(GatewayType)))
				{
					if (value == GatewayType.Unknown)
					{
						continue;
					}
					flag = true;
					foreach (ValueMatchType value2 in global::System.Enum.GetValues(typeof(ValueMatchType)))
					{
						ReadOnlyDictionary<GatewayType, Regex> val2 = SharedGatewayParsingMetadata.ValueMatchDictionariesDictionary[value2];
						if (!val2.ContainsKey(value))
						{
							val[value2] = string.Empty;
							continue;
						}
						Match val3 = val2[value].Match(imageText);
						if (((Group)val3).Success)
						{
							string text = SharedGatewayParsingMetadata.ValueMatchGroupKeys[value2];
							Func<string, GatewayType, string> val4 = SanitizationMethodsDictionary[value2];
							val[value2] = val4.Invoke(((Capture)val3.Groups[text]).Value, value);
							continue;
						}
						flag = false;
						break;
					}
					if (flag)
					{
						_suspectedGatewayType = value;
						TaggedLog.Debug("LabelTextParser", $"Suspected gateway type is '{_suspectedGatewayType}'", global::System.Array.Empty<object>());
						break;
					}
				}
			}
			catch (global::System.Exception ex)
			{
				Console.WriteLine((object)ex);
				throw;
			}
			if (!flag)
			{
				return ParsedLabelTextResult.Empty;
			}
			string id = val[ValueMatchType.Id];
			string password = val[ValueMatchType.Password];
			return new ParsedLabelTextResult(_suspectedGatewayType, id, password);
		}

		private ParsedLabelTextResult ParseQrText(string qrText)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrWhiteSpace(qrText))
			{
				return ParsedLabelTextResult.Empty;
			}
			if (!qrText.Trim().StartsWith("http"))
			{
				return ParseImageText(qrText);
			}
			NameValueCollection val = HttpUtility.ParseQueryString(new Uri(qrText).Query);
			if (!Enumerable.Contains<string>((global::System.Collections.Generic.IEnumerable<string>)val.AllKeys, "devid") || !Enumerable.Contains<string>((global::System.Collections.Generic.IEnumerable<string>)val.AllKeys, "pw"))
			{
				return ParseImageText(qrText);
			}
			string text = val["devid"];
			string password = val["pw"];
			GatewayType gatewayType = GatewayType.Unknown;
			foreach (GatewayType value in global::System.Enum.GetValues(typeof(GatewayType)))
			{
				if (value != GatewayType.Unknown && SharedGatewayParsingMetadata.IdRegexDictionary.ContainsKey(value) && ((Group)SharedGatewayParsingMetadata.IdRegexDictionary[value].Match(text)).Success)
				{
					gatewayType = value;
					break;
				}
			}
			ParsedLabelTextResult parsedLabelTextResult = new ParsedLabelTextResult(gatewayType, text, password);
			TaggedLog.Debug("LabelTextParser", $"Parsed '{parsedLabelTextResult}' from QR text: '{qrText}'", global::System.Array.Empty<object>());
			return parsedLabelTextResult;
		}

		private static string SanitizeId(string idText, GatewayType gatewayType)
		{
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			string text = FixIncorrectCharacters(idText);
			switch (gatewayType)
			{
			case GatewayType.Bluetooth:
				return "LCIRemote" + text;
			case GatewayType.WiFi:
				return "MyRV_" + text.ToUpper();
			case GatewayType.Aquafi:
				return "LCI_AquaFi_" + text;
			case GatewayType.SonixCamera:
			case GatewayType.WindSensor:
			case GatewayType.BatteryMonitor:
			case GatewayType.AntiLockBraking:
			case GatewayType.SwayBraking:
				return idText;
			default:
				throw new ArgumentOutOfRangeException("gatewayType", (object)gatewayType, (string)null);
			}
		}

		private string SanitizePassword(string passwordText, GatewayType gatewayType = GatewayType.Unknown)
		{
			return FixIncorrectCharacters(passwordText).ToUpper();
		}

		private string SanitizeVersion(string versionText, GatewayType gatewayType = GatewayType.Unknown)
		{
			return versionText;
		}

		public static string FixIncorrectCharacters(string text)
		{
			return Enumerable.Aggregate<KeyValuePair<char, char>, string>((global::System.Collections.Generic.IEnumerable<KeyValuePair<char, char>>)SharedGatewayParsingMetadata.ReplacementDictionary, text, (Func<string, KeyValuePair<char, char>, string>)((string accumulator, KeyValuePair<char, char> pair) => accumulator.Replace(pair.Key, pair.Value)));
		}
	}
	public abstract class PageViewModel<TDeviceModel, TLogicalDevice> : PageViewModel where TDeviceModel : class, IDeviceModel<TLogicalDevice> where TLogicalDevice : class, ILogicalDevice
	{
		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <OnResumeAsync>d__14 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public PageViewModel<TDeviceModel, TLogicalDevice> <>4__this;

			public ResumeReason reason;

			public INavigationParameters parameters;

			public CancellationToken cancellationToken;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_005c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0061: Unknown result type (might be due to invalid IL or missing references)
				//IL_0068: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c0: Expected O, but got Unknown
				//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c5: Expected O, but got Unknown
				//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fa: Expected O, but got Unknown
				//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ff: Expected O, but got Unknown
				int num = <>1__state;
				PageViewModel<TDeviceModel, TLogicalDevice> pageViewModel = <>4__this;
				try
				{
					TaskAwaiter val;
					if (num != 0)
					{
						val = pageViewModel.<>n__1(reason, parameters, cancellationToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnResumeAsync>d__14>(ref val, ref this);
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
					if (pageViewModel.DeviceModel != null && pageViewModel.Device != null)
					{
						pageViewModel._deviceModelPropertyChangedProxySource = new NotifyPropertyChangedProxySource((INotifyPropertyChanged)(object)pageViewModel.DeviceModel, new ProxyOnPropertyChanged(((ObservableObject)pageViewModel).OnPropertyChanged), (INotifyPropertyChanged)(object)pageViewModel, "DeviceModelPageViewModelNotifyPropertyChangedProxySource");
						DisposableMixins.DisposeWith<NotifyPropertyChangedProxySource>(pageViewModel._deviceModelPropertyChangedProxySource, ViewModelExtensions.PausedDisposable((IViewModel)(object)pageViewModel));
						pageViewModel._devicePropertyChangedProxySource = new NotifyPropertyChangedProxySource((INotifyPropertyChanged)(object)pageViewModel.Device, new ProxyOnPropertyChanged(((ObservableObject)pageViewModel).OnPropertyChanged), (INotifyPropertyChanged)(object)pageViewModel, "DevicePageViewModelNotifyPropertyChangedProxySource");
						DisposableMixins.DisposeWith<NotifyPropertyChangedProxySource>(pageViewModel._devicePropertyChangedProxySource, ViewModelExtensions.PausedDisposable((IViewModel)(object)pageViewModel));
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
		private struct <OnStartAsync>d__13 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public PageViewModel<TDeviceModel, TLogicalDevice> <>4__this;

			public INavigationParameters parameters;

			public CancellationToken cancellationToken;

			private TDeviceModel <deviceModel>5__2;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				//IL_005b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0062: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0102: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				PageViewModel<TDeviceModel, TLogicalDevice> pageViewModel = <>4__this;
				try
				{
					TaskAwaiter val;
					if (num != 0)
					{
						val = pageViewModel.<>n__0(parameters, cancellationToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnStartAsync>d__13>(ref val, ref this);
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
					INavigationParameters obj = parameters;
					bool? flag = ((obj != null) ? new bool?(INavigationParametersExtensions.TryGetDeviceModel<TDeviceModel>(obj, ref <deviceModel>5__2, (TDeviceModel)null)) : ((bool?)null));
					if (!flag.HasValue || flag != true || <deviceModel>5__2 == null)
					{
						throw new ArgumentException($"Parameter of type {typeof(TDeviceModel)} expected!");
					}
					pageViewModel.DeviceModel = <deviceModel>5__2;
					pageViewModel.Device = ((IDeviceModel<?>)(object)<deviceModel>5__2).Device;
					((ViewModel)pageViewModel).RaiseAllPropertiesChanged((ThreadInvokeOption)1);
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<deviceModel>5__2 = null;
					((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
					return;
				}
				<>1__state = -2;
				<deviceModel>5__2 = null;
				((AsyncTaskMethodBuilder)(ref <>t__builder)).SetResult();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				((AsyncTaskMethodBuilder)(ref <>t__builder)).SetStateMachine(stateMachine);
			}
		}

		protected const string DeviceModelPropertyChangedProxySourceName = "DeviceModelPageViewModelNotifyPropertyChangedProxySource";

		protected const string DevicePropertyChangedProxySourceName = "DevicePageViewModelNotifyPropertyChangedProxySource";

		private NotifyPropertyChangedProxySource? _deviceModelPropertyChangedProxySource;

		private NotifyPropertyChangedProxySource? _devicePropertyChangedProxySource;

		public TDeviceModel? DeviceModel
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public TLogicalDevice? Device
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		protected PageViewModel(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		[AsyncStateMachine(typeof(PageViewModel<, >.<OnStartAsync>d__13))]
		public override global::System.Threading.Tasks.Task OnStartAsync(INavigationParameters? parameters, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			<OnStartAsync>d__13 <OnStartAsync>d__ = default(<OnStartAsync>d__13);
			<OnStartAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<OnStartAsync>d__.<>4__this = this;
			<OnStartAsync>d__.parameters = parameters;
			<OnStartAsync>d__.cancellationToken = cancellationToken;
			<OnStartAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <OnStartAsync>d__.<>t__builder)).Start<<OnStartAsync>d__13>(ref <OnStartAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <OnStartAsync>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(PageViewModel<, >.<OnResumeAsync>d__14))]
		public override global::System.Threading.Tasks.Task OnResumeAsync(ResumeReason reason, INavigationParameters parameters, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			<OnResumeAsync>d__14 <OnResumeAsync>d__ = default(<OnResumeAsync>d__14);
			<OnResumeAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<OnResumeAsync>d__.<>4__this = this;
			<OnResumeAsync>d__.reason = reason;
			<OnResumeAsync>d__.parameters = parameters;
			<OnResumeAsync>d__.cancellationToken = cancellationToken;
			<OnResumeAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <OnResumeAsync>d__.<>t__builder)).Start<<OnResumeAsync>d__14>(ref <OnResumeAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <OnResumeAsync>d__.<>t__builder)).Task;
		}
	}
	public abstract class PageViewModel<TLogicalDevice> : PageViewModel where TLogicalDevice : class, ILogicalDevice
	{
		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <OnResumeAsync>d__10 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public PageViewModel<TLogicalDevice> <>4__this;

			public ResumeReason reason;

			public INavigationParameters parameters;

			public CancellationToken cancellationToken;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_005c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0061: Unknown result type (might be due to invalid IL or missing references)
				//IL_0068: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b0: Expected O, but got Unknown
				//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b5: Expected O, but got Unknown
				int num = <>1__state;
				PageViewModel<TLogicalDevice> pageViewModel = <>4__this;
				try
				{
					TaskAwaiter val;
					if (num != 0)
					{
						val = pageViewModel.<>n__1(reason, parameters, cancellationToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnResumeAsync>d__10>(ref val, ref this);
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
					if (pageViewModel.Device != null)
					{
						pageViewModel._devicePropertyChangedProxySource = new NotifyPropertyChangedProxySource((INotifyPropertyChanged)(object)pageViewModel.Device, new ProxyOnPropertyChanged(((ObservableObject)pageViewModel).OnPropertyChanged), (INotifyPropertyChanged)(object)pageViewModel, "ViewModelBaseDevice_PropertyChangedProxySourceName");
						DisposableMixins.DisposeWith<NotifyPropertyChangedProxySource>(pageViewModel._devicePropertyChangedProxySource, ViewModelExtensions.PausedDisposable((IViewModel)(object)pageViewModel));
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
		private struct <OnStartAsync>d__9 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public PageViewModel<TLogicalDevice> <>4__this;

			public INavigationParameters parameters;

			public CancellationToken cancellationToken;

			private TLogicalDevice <device>5__2;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				//IL_005b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0062: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0102: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				PageViewModel<TLogicalDevice> pageViewModel = <>4__this;
				try
				{
					TaskAwaiter val;
					if (num != 0)
					{
						val = pageViewModel.<>n__0(parameters, cancellationToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnStartAsync>d__9>(ref val, ref this);
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
					INavigationParameters obj = parameters;
					bool? flag = ((obj != null) ? new bool?(INavigationParametersExtensions.TryGetDevice<TLogicalDevice>(obj, ref <device>5__2, (TLogicalDevice)null)) : ((bool?)null));
					if (!flag.HasValue || flag != true || <device>5__2 == null)
					{
						throw new ArgumentException($"Parameter of type {typeof(TLogicalDevice)} expected!");
					}
					pageViewModel.Device = <device>5__2;
					((ViewModel)pageViewModel).RaiseAllPropertiesChanged((ThreadInvokeOption)1);
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<device>5__2 = null;
					((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
					return;
				}
				<>1__state = -2;
				<device>5__2 = null;
				((AsyncTaskMethodBuilder)(ref <>t__builder)).SetResult();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				((AsyncTaskMethodBuilder)(ref <>t__builder)).SetStateMachine(stateMachine);
			}
		}

		private NotifyPropertyChangedProxySource? _devicePropertyChangedProxySource;

		protected const string DevicePropertyChangedProxySourceName = "ViewModelBaseDevice_PropertyChangedProxySourceName";

		private ObservableCollection<ICellViewModel>? _settings;

		public TLogicalDevice? Device
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public bool IsOnline
		{
			get
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				TLogicalDevice? device = Device;
				return device != null && (int)((IDevicesCommon)device).ActiveConnection != 0;
			}
		}

		public bool ShouldClearSettings => true;

		public ObservableCollection<ICellViewModel>? Settings
		{
			get
			{
				return _settings ?? (_settings = (ObservableCollection<ICellViewModel>?)(object)new BaseObservableCollection<ICellViewModel>());
			}
			protected set
			{
				((ViewModel)this).SetProperty<ObservableCollection<ICellViewModel>>(ref _settings, value, "Settings", (ThreadInvokeOption)1, global::System.Array.Empty<string>());
			}
		}

		protected PageViewModel(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		[AsyncStateMachine(typeof(PageViewModel<>.<OnStartAsync>d__9))]
		public override global::System.Threading.Tasks.Task OnStartAsync(INavigationParameters? parameters, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			<OnStartAsync>d__9 <OnStartAsync>d__ = default(<OnStartAsync>d__9);
			<OnStartAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<OnStartAsync>d__.<>4__this = this;
			<OnStartAsync>d__.parameters = parameters;
			<OnStartAsync>d__.cancellationToken = cancellationToken;
			<OnStartAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <OnStartAsync>d__.<>t__builder)).Start<<OnStartAsync>d__9>(ref <OnStartAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <OnStartAsync>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(PageViewModel<>.<OnResumeAsync>d__10))]
		public override global::System.Threading.Tasks.Task OnResumeAsync(ResumeReason reason, INavigationParameters parameters, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			<OnResumeAsync>d__10 <OnResumeAsync>d__ = default(<OnResumeAsync>d__10);
			<OnResumeAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<OnResumeAsync>d__.<>4__this = this;
			<OnResumeAsync>d__.reason = reason;
			<OnResumeAsync>d__.parameters = parameters;
			<OnResumeAsync>d__.cancellationToken = cancellationToken;
			<OnResumeAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <OnResumeAsync>d__.<>t__builder)).Start<<OnResumeAsync>d__10>(ref <OnResumeAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <OnResumeAsync>d__.<>t__builder)).Task;
		}

		protected void ClearSettings(global::System.Collections.Generic.IReadOnlyList<ICellViewModel>? settings)
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Expected O, but got Unknown
			if (settings == null)
			{
				return;
			}
			((ViewModel)this).MainThreadService.BeginInvokeOnMainThread((Action)delegate
			{
				try
				{
					global::System.Collections.Generic.IEnumerator<ICellViewModel> enumerator = ((global::System.Collections.Generic.IEnumerable<ICellViewModel>)settings).GetEnumerator();
					try
					{
						while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
						{
							ICellViewModel current = enumerator.Current;
							if (current is IDisplaySettingViewModel displaySettingViewModel)
							{
								ClearSettings(displaySettingViewModel.Children);
							}
							global::System.IDisposable obj = current as global::System.IDisposable;
							if (obj != null)
							{
								IDisposableExtensions.TryDispose(obj);
							}
						}
					}
					finally
					{
						((global::System.IDisposable)enumerator)?.Dispose();
					}
					LoggerExtensions.LogDebug(((PageViewModel)this).Logger, "{SettingsCount} settings disposed", new object[1] { ((global::System.Collections.Generic.IReadOnlyCollection<ICellViewModel>)settings).Count });
				}
				catch (global::System.Exception ex)
				{
					LoggerExtensions.LogWarning(((PageViewModel)this).Logger, "Unable to dispose ${SettingsName} {ExMessage}", new object[2] { "settings", ex.Message });
				}
				settings = null;
			});
		}

		public virtual ObservableCollection<ICellViewModel> GetAllSettingsViewModels(CancellationToken cancellationToken = default(CancellationToken))
		{
			ObservableCollection<ICellViewModel> result = new ObservableCollection<ICellViewModel>();
			_ = Device;
			return result;
		}
	}
	public class Resource : Resource
	{
	}
}
namespace App.Common.Pages.Pairing.Warning
{
	[XamlFilePath("Warning/WarningPage.xaml")]
	public class WarningPage : ContentPage
	{
		public WarningPage()
		{
			InitializeComponent();
		}

		[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
		private void InitializeComponent()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected O, but got Unknown
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Expected O, but got Unknown
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Expected O, but got Unknown
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected O, but got Unknown
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Expected O, but got Unknown
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Expected O, but got Unknown
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Expected O, but got Unknown
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Expected O, but got Unknown
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Expected O, but got Unknown
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Expected O, but got Unknown
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Expected O, but got Unknown
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Expected O, but got Unknown
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Expected O, but got Unknown
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected O, but got Unknown
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Expected O, but got Unknown
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Expected O, but got Unknown
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Expected O, but got Unknown
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Expected O, but got Unknown
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Expected O, but got Unknown
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Expected O, but got Unknown
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Expected O, but got Unknown
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Expected O, but got Unknown
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Expected O, but got Unknown
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Expected O, but got Unknown
			//IL_0215: Expected O, but got Unknown
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_0236: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Expected O, but got Unknown
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Expected O, but got Unknown
			//IL_0259: Expected O, but got Unknown
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Expected O, but got Unknown
			//IL_029c: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a7: Expected O, but got Unknown
			//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Expected O, but got Unknown
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ca: Expected O, but got Unknown
			//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e0: Expected O, but got Unknown
			//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ea: Expected O, but got Unknown
			//IL_0314: Unknown result type (might be due to invalid IL or missing references)
			//IL_0341: Unknown result type (might be due to invalid IL or missing references)
			//IL_0346: Unknown result type (might be due to invalid IL or missing references)
			//IL_0355: Unknown result type (might be due to invalid IL or missing references)
			//IL_035f: Expected O, but got Unknown
			//IL_035a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0364: Expected O, but got Unknown
			//IL_0369: Expected O, but got Unknown
			//IL_0385: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d2: Expected O, but got Unknown
			//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d7: Expected O, but got Unknown
			//IL_03dc: Expected O, but got Unknown
			//IL_0426: Unknown result type (might be due to invalid IL or missing references)
			//IL_043c: Unknown result type (might be due to invalid IL or missing references)
			//IL_048f: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04da: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e4: Expected O, but got Unknown
			//IL_04df: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e9: Expected O, but got Unknown
			//IL_04ee: Expected O, but got Unknown
			//IL_0513: Unknown result type (might be due to invalid IL or missing references)
			//IL_0590: Unknown result type (might be due to invalid IL or missing references)
			//IL_0595: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ae: Expected O, but got Unknown
			//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b3: Expected O, but got Unknown
			//IL_05b8: Expected O, but got Unknown
			//IL_0617: Unknown result type (might be due to invalid IL or missing references)
			//IL_061c: Unknown result type (might be due to invalid IL or missing references)
			//IL_062b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0635: Expected O, but got Unknown
			//IL_0630: Unknown result type (might be due to invalid IL or missing references)
			//IL_063a: Expected O, but got Unknown
			//IL_063f: Expected O, but got Unknown
			//IL_065b: Unknown result type (might be due to invalid IL or missing references)
			//IL_068a: Unknown result type (might be due to invalid IL or missing references)
			//IL_068f: Unknown result type (might be due to invalid IL or missing references)
			//IL_069e: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a8: Expected O, but got Unknown
			//IL_06a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ad: Expected O, but got Unknown
			//IL_06b2: Expected O, but got Unknown
			//IL_071f: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0807: Unknown result type (might be due to invalid IL or missing references)
			//IL_0811: Expected O, but got Unknown
			//IL_080c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0816: Expected O, but got Unknown
			//IL_081b: Expected O, but got Unknown
			//IL_083e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0843: Unknown result type (might be due to invalid IL or missing references)
			//IL_0852: Unknown result type (might be due to invalid IL or missing references)
			//IL_085c: Expected O, but got Unknown
			//IL_0857: Unknown result type (might be due to invalid IL or missing references)
			//IL_0861: Expected O, but got Unknown
			//IL_0866: Expected O, but got Unknown
			//IL_08b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_08bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_08ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_08d4: Expected O, but got Unknown
			//IL_08cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_08d9: Expected O, but got Unknown
			//IL_08de: Expected O, but got Unknown
			//IL_0901: Unknown result type (might be due to invalid IL or missing references)
			//IL_0906: Unknown result type (might be due to invalid IL or missing references)
			//IL_0915: Unknown result type (might be due to invalid IL or missing references)
			//IL_091f: Expected O, but got Unknown
			//IL_091a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0924: Expected O, but got Unknown
			//IL_0929: Expected O, but got Unknown
			//IL_099c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a70: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a75: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a84: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a8e: Expected O, but got Unknown
			//IL_0a89: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a93: Expected O, but got Unknown
			//IL_0a98: Expected O, but got Unknown
			//IL_0abb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ac0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0acf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ad9: Expected O, but got Unknown
			//IL_0ad4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ade: Expected O, but got Unknown
			//IL_0ae3: Expected O, but got Unknown
			//IL_0b33: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b38: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b47: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b51: Expected O, but got Unknown
			//IL_0b4c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b56: Expected O, but got Unknown
			//IL_0b5b: Expected O, but got Unknown
			//IL_0b7e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b83: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b92: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b9c: Expected O, but got Unknown
			//IL_0b97: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ba1: Expected O, but got Unknown
			//IL_0ba6: Expected O, but got Unknown
			//IL_0c19: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			Window current = Window.Current;
			BindingExtension val = new BindingExtension();
			DynamicResourceExtension val2 = new DynamicResourceExtension();
			DynamicResourceExtension val3 = new DynamicResourceExtension();
			DynamicResourceExtension val4 = new DynamicResourceExtension();
			string warning_Title = Strings.Warning_Title;
			Label val5 = new Label();
			DynamicResourceExtension val6 = new DynamicResourceExtension();
			RoundRectangle val7 = new RoundRectangle();
			string proSolid = FontAwesomeFontFamily.ProSolid900;
			string circleExclamation = FontAwesomeGlyph.CircleExclamation;
			DynamicResourceExtension val8 = new DynamicResourceExtension();
			FontImageSource val9 = new FontImageSource();
			SKImageView val10 = new SKImageView();
			Border val11 = new Border();
			DynamicResourceExtension val12 = new DynamicResourceExtension();
			DynamicResourceExtension val13 = new DynamicResourceExtension();
			BindingExtension val14 = new BindingExtension();
			Label val15 = new Label();
			DynamicResourceExtension val16 = new DynamicResourceExtension();
			DynamicResourceExtension val17 = new DynamicResourceExtension();
			DynamicResourceExtension val18 = new DynamicResourceExtension();
			DynamicResourceExtension val19 = new DynamicResourceExtension();
			BindingExtension val20 = new BindingExtension();
			Button val21 = new Button();
			DynamicResourceExtension val22 = new DynamicResourceExtension();
			DynamicResourceExtension val23 = new DynamicResourceExtension();
			DynamicResourceExtension val24 = new DynamicResourceExtension();
			DynamicResourceExtension val25 = new DynamicResourceExtension();
			BindingExtension val26 = new BindingExtension();
			Button val27 = new Button();
			Grid val28 = new Grid();
			WarningPage warningPage;
			NameScope val29 = (NameScope)(((object)NameScope.GetNameScope((BindableObject)(object)(warningPage = this))) ?? ((object)new NameScope()));
			NameScope.SetNameScope((BindableObject)(object)warningPage, (INameScope)(object)val29);
			((Element)val28).transientNamescope = (INameScope)(object)val29;
			((Element)val5).transientNamescope = (INameScope)(object)val29;
			((Element)val11).transientNamescope = (INameScope)(object)val29;
			((Element)val7).transientNamescope = (INameScope)(object)val29;
			((Element)val10).transientNamescope = (INameScope)(object)val29;
			((Element)val9).transientNamescope = (INameScope)(object)val29;
			((Element)val15).transientNamescope = (INameScope)(object)val29;
			((Element)val21).transientNamescope = (INameScope)(object)val29;
			((Element)val27).transientNamescope = (INameScope)(object)val29;
			((BindableObject)warningPage).SetValue(NavigationPage.HasNavigationBarProperty, (object)false);
			((BindableObject)warningPage).SetValue(Window.IgnoreInsetsProperty, (object)true);
			val.Path = "Insets";
			val.Source = current;
			XamlServiceProvider val30 = new XamlServiceProvider();
			global::System.Type typeFromHandle = typeof(IXamlTypeResolver);
			XmlNamespaceResolver val31 = new XmlNamespaceResolver();
			val31.Add("", "http://schemas.microsoft.com/dotnet/2021/maui");
			val31.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			val31.Add("ui", "http://lci1.com/schemas/ui");
			val31.Add("peridot", "http://lci1.com/schemas/peridot");
			val31.Add("warning", "clr-namespace:App.Common.Pages.Pairing.Warning");
			val31.Add("resources", "clr-namespace:App.Common.Pages.Pairing.Resources");
			val30.Add(typeFromHandle, (object)new XamlTypeResolver((IXmlNamespaceResolver)val31, typeof(WarningPage).Assembly));
			BindingBase val32 = ((IMarkupExtension<BindingBase>)(object)val).ProvideValue((IServiceProvider)val30);
			((BindableObject)warningPage).SetBinding(Page.PaddingProperty, val32);
			val2.Key = "SecondaryContainer";
			XamlServiceProvider val33 = new XamlServiceProvider();
			val33.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(14, 14)));
			DynamicResource val34 = ((IMarkupExtension<DynamicResource>)(object)val2).ProvideValue((IServiceProvider)val33);
			((IDynamicResourceHandler)warningPage).SetDynamicResource(VisualElement.BackgroundColorProperty, val34.Key);
			((BindableObject)val28).SetValue(Grid.RowDefinitionsProperty, (object)new RowDefinitionCollection((RowDefinition[])(object)new RowDefinition[5]
			{
				new RowDefinition(new GridLength(64.0)),
				new RowDefinition(new GridLength(296.0)),
				new RowDefinition(GridLength.Star),
				new RowDefinition(new GridLength(56.0)),
				new RowDefinition(new GridLength(56.0))
			}));
			((BindableObject)val28).SetValue(Grid.RowSpacingProperty, (object)16.0);
			((BindableObject)val28).SetValue(Layout.PaddingProperty, (object)new Thickness(16.0));
			((BindableObject)val5).SetValue(Grid.RowProperty, (object)0);
			val3.Key = "H2_Label";
			XamlServiceProvider val35 = new XamlServiceProvider();
			val35.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(20, 20)));
			DynamicResource val36 = ((IMarkupExtension<DynamicResource>)(object)val3).ProvideValue((IServiceProvider)val35);
			((IDynamicResourceHandler)val5).SetDynamicResource(VisualElement.StyleProperty, val36.Key);
			((BindableObject)val5).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Center);
			((BindableObject)val5).SetValue(Label.HorizontalTextAlignmentProperty, (object)(TextAlignment)1);
			val4.Key = "OnSecondaryContainer";
			XamlServiceProvider val37 = new XamlServiceProvider();
			val37.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(23, 20)));
			DynamicResource val38 = ((IMarkupExtension<DynamicResource>)(object)val4).ProvideValue((IServiceProvider)val37);
			((IDynamicResourceHandler)val5).SetDynamicResource(Label.TextColorProperty, val38.Key);
			((BindableObject)val5).SetValue(Label.TextProperty, (object)warning_Title);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val28).Children).Add((IView)(object)val5);
			((BindableObject)val11).SetValue(Grid.RowProperty, (object)1);
			((BindableObject)val11).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
			((BindableObject)val11).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Start);
			((BindableObject)val11).SetValue(VisualElement.WidthRequestProperty, (object)248.0);
			((BindableObject)val11).SetValue(VisualElement.HeightRequestProperty, (object)248.0);
			((BindableObject)val11).SetValue(Border.PaddingProperty, (object)new Thickness(4.0));
			((BindableObject)val11).SetValue(Border.StrokeThicknessProperty, (object)4.0);
			val6.Key = "OnSecondaryContainer";
			XamlServiceProvider val39 = new XamlServiceProvider();
			val39.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(32, 21)));
			DynamicResource val40 = ((IMarkupExtension<DynamicResource>)(object)val6).ProvideValue((IServiceProvider)val39);
			((IDynamicResourceHandler)val11).SetDynamicResource(Border.StrokeProperty, val40.Key);
			((BindableObject)val7).SetValue(RoundRectangle.CornerRadiusProperty, (object)new CornerRadius(124.0));
			((BindableObject)val11).SetValue(Border.StrokeShapeProperty, (object)val7);
			((BindableObject)val10).SetValue(SKImageView.AspectProperty, (object)(Aspect)0);
			((BindableObject)val10).SetValue(SKImageView.HorizontalImageAlignmentProperty, (object)(ImageAlignment)1);
			((BindableObject)val10).SetValue(SKImageView.VerticalImageAlignmentProperty, (object)(ImageAlignment)1);
			((BindableObject)val9).SetValue(FontImageSource.FontFamilyProperty, (object)proSolid);
			((BindableObject)val9).SetValue(FontImageSource.GlyphProperty, (object)circleExclamation);
			val8.Key = "Secondary";
			XamlServiceProvider val41 = new XamlServiceProvider();
			val41.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(42, 42)));
			DynamicResource val42 = ((IMarkupExtension<DynamicResource>)(object)val8).ProvideValue((IServiceProvider)val41);
			((IDynamicResourceHandler)val9).SetDynamicResource(FontImageSource.ColorProperty, val42.Key);
			((BindableObject)val10).SetValue(SKImageView.ImageSourceProperty, (object)val9);
			((BindableObject)val11).SetValue(Border.ContentProperty, (object)val10);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val28).Children).Add((IView)(object)val11);
			((BindableObject)val15).SetValue(Grid.RowProperty, (object)2);
			val12.Key = "Body4_Label";
			XamlServiceProvider val43 = new XamlServiceProvider();
			val43.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(47, 20)));
			DynamicResource val44 = ((IMarkupExtension<DynamicResource>)(object)val12).ProvideValue((IServiceProvider)val43);
			((IDynamicResourceHandler)val15).SetDynamicResource(VisualElement.StyleProperty, val44.Key);
			((BindableObject)val15).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Center);
			((BindableObject)val15).SetValue(Label.HorizontalTextAlignmentProperty, (object)(TextAlignment)1);
			val13.Key = "OnSecondaryContainer";
			XamlServiceProvider val45 = new XamlServiceProvider();
			val45.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(50, 20)));
			DynamicResource val46 = ((IMarkupExtension<DynamicResource>)(object)val13).ProvideValue((IServiceProvider)val45);
			((IDynamicResourceHandler)val15).SetDynamicResource(Label.TextColorProperty, val46.Key);
			val14.Path = "Message";
			val14.TypedBinding = (TypedBindingBase)(object)new TypedBinding<WarningViewModel, string>((Func<WarningViewModel, ValueTuple<string, bool>>)([CompilerGenerated] (WarningViewModel P_0) => (P_0 != null) ? new ValueTuple<string, bool>(P_0.Message, true) : default(ValueTuple<string, bool>)), (Action<WarningViewModel, string>)([CompilerGenerated] (WarningViewModel P_0, string P_1) =>
			{
				if (P_0 != null)
				{
					P_0.Message = P_1;
				}
			}), new Tuple<Func<WarningViewModel, object>, string>[1]
			{
				new Tuple<Func<WarningViewModel, object>, string>((Func<WarningViewModel, object>)([CompilerGenerated] (WarningViewModel P_0) => P_0), "Message")
			});
			((BindingBase)val14.TypedBinding).Mode = val14.Mode;
			val14.TypedBinding.Converter = val14.Converter;
			val14.TypedBinding.ConverterParameter = val14.ConverterParameter;
			((BindingBase)val14.TypedBinding).StringFormat = val14.StringFormat;
			val14.TypedBinding.Source = val14.Source;
			val14.TypedBinding.UpdateSourceEventName = val14.UpdateSourceEventName;
			((BindingBase)val14.TypedBinding).FallbackValue = val14.FallbackValue;
			((BindingBase)val14.TypedBinding).TargetNullValue = val14.TargetNullValue;
			BindingBase typedBinding = (BindingBase)(object)val14.TypedBinding;
			((BindableObject)val15).SetBinding(Label.TextProperty, typedBinding);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val28).Children).Add((IView)(object)val15);
			((BindableObject)val21).SetValue(Grid.RowProperty, (object)3);
			val16.Key = "FontRegular";
			XamlServiceProvider val47 = new XamlServiceProvider();
			val47.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(53, 21)));
			DynamicResource val48 = ((IMarkupExtension<DynamicResource>)(object)val16).ProvideValue((IServiceProvider)val47);
			((IDynamicResourceHandler)val21).SetDynamicResource(Button.FontFamilyProperty, val48.Key);
			val17.Key = "Button_FontSize";
			XamlServiceProvider val49 = new XamlServiceProvider();
			val49.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(54, 21)));
			DynamicResource val50 = ((IMarkupExtension<DynamicResource>)(object)val17).ProvideValue((IServiceProvider)val49);
			((IDynamicResourceHandler)val21).SetDynamicResource(Button.FontSizeProperty, val50.Key);
			((BindableObject)val21).SetValue(VisualElement.HeightRequestProperty, (object)56.0);
			((BindableObject)val21).SetValue(Button.CornerRadiusProperty, (object)28);
			val18.Key = "SecondaryContainer";
			XamlServiceProvider val51 = new XamlServiceProvider();
			val51.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(57, 21)));
			DynamicResource val52 = ((IMarkupExtension<DynamicResource>)(object)val18).ProvideValue((IServiceProvider)val51);
			((IDynamicResourceHandler)val21).SetDynamicResource(VisualElement.BackgroundColorProperty, val52.Key);
			val19.Key = "OnSecondaryContainer";
			XamlServiceProvider val53 = new XamlServiceProvider();
			val53.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(58, 21)));
			DynamicResource val54 = ((IMarkupExtension<DynamicResource>)(object)val19).ProvideValue((IServiceProvider)val53);
			((IDynamicResourceHandler)val21).SetDynamicResource(Button.TextColorProperty, val54.Key);
			((BindableObject)val21).SetValue(Button.TextProperty, (object)"Back");
			val20.Path = "BackCommand";
			val20.TypedBinding = (TypedBindingBase)(object)new TypedBinding<WarningViewModel, IAsyncRelayCommand>((Func<WarningViewModel, ValueTuple<IAsyncRelayCommand, bool>>)([CompilerGenerated] (WarningViewModel P_0) => (P_0 != null) ? new ValueTuple<IAsyncRelayCommand, bool>(P_0.BackCommand, true) : default(ValueTuple<IAsyncRelayCommand, bool>)), (Action<WarningViewModel, IAsyncRelayCommand>)null, new Tuple<Func<WarningViewModel, object>, string>[1]
			{
				new Tuple<Func<WarningViewModel, object>, string>((Func<WarningViewModel, object>)([CompilerGenerated] (WarningViewModel P_0) => P_0), "BackCommand")
			});
			((BindingBase)val20.TypedBinding).Mode = val20.Mode;
			val20.TypedBinding.Converter = val20.Converter;
			val20.TypedBinding.ConverterParameter = val20.ConverterParameter;
			((BindingBase)val20.TypedBinding).StringFormat = val20.StringFormat;
			val20.TypedBinding.Source = val20.Source;
			val20.TypedBinding.UpdateSourceEventName = val20.UpdateSourceEventName;
			((BindingBase)val20.TypedBinding).FallbackValue = val20.FallbackValue;
			((BindingBase)val20.TypedBinding).TargetNullValue = val20.TargetNullValue;
			BindingBase typedBinding2 = (BindingBase)(object)val20.TypedBinding;
			((BindableObject)val21).SetBinding(Button.CommandProperty, typedBinding2);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val28).Children).Add((IView)(object)val21);
			((BindableObject)val27).SetValue(Grid.RowProperty, (object)4);
			val22.Key = "FontRegular";
			XamlServiceProvider val55 = new XamlServiceProvider();
			val55.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(62, 21)));
			DynamicResource val56 = ((IMarkupExtension<DynamicResource>)(object)val22).ProvideValue((IServiceProvider)val55);
			((IDynamicResourceHandler)val27).SetDynamicResource(Button.FontFamilyProperty, val56.Key);
			val23.Key = "Button_FontSize";
			XamlServiceProvider val57 = new XamlServiceProvider();
			val57.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(63, 21)));
			DynamicResource val58 = ((IMarkupExtension<DynamicResource>)(object)val23).ProvideValue((IServiceProvider)val57);
			((IDynamicResourceHandler)val27).SetDynamicResource(Button.FontSizeProperty, val58.Key);
			((BindableObject)val27).SetValue(VisualElement.HeightRequestProperty, (object)56.0);
			((BindableObject)val27).SetValue(Button.CornerRadiusProperty, (object)28);
			val24.Key = "Secondary";
			XamlServiceProvider val59 = new XamlServiceProvider();
			val59.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(66, 21)));
			DynamicResource val60 = ((IMarkupExtension<DynamicResource>)(object)val24).ProvideValue((IServiceProvider)val59);
			((IDynamicResourceHandler)val27).SetDynamicResource(VisualElement.BackgroundColorProperty, val60.Key);
			val25.Key = "OnSecondary";
			XamlServiceProvider val61 = new XamlServiceProvider();
			val61.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(67, 21)));
			DynamicResource val62 = ((IMarkupExtension<DynamicResource>)(object)val25).ProvideValue((IServiceProvider)val61);
			((IDynamicResourceHandler)val27).SetDynamicResource(Button.TextColorProperty, val62.Key);
			((BindableObject)val27).SetValue(Button.TextProperty, (object)"Continue");
			val26.Path = "AcceptCommand";
			val26.TypedBinding = (TypedBindingBase)(object)new TypedBinding<WarningViewModel, IAsyncRelayCommand>((Func<WarningViewModel, ValueTuple<IAsyncRelayCommand, bool>>)([CompilerGenerated] (WarningViewModel P_0) => (P_0 != null) ? new ValueTuple<IAsyncRelayCommand, bool>(P_0.AcceptCommand, true) : default(ValueTuple<IAsyncRelayCommand, bool>)), (Action<WarningViewModel, IAsyncRelayCommand>)null, new Tuple<Func<WarningViewModel, object>, string>[1]
			{
				new Tuple<Func<WarningViewModel, object>, string>((Func<WarningViewModel, object>)([CompilerGenerated] (WarningViewModel P_0) => P_0), "AcceptCommand")
			});
			((BindingBase)val26.TypedBinding).Mode = val26.Mode;
			val26.TypedBinding.Converter = val26.Converter;
			val26.TypedBinding.ConverterParameter = val26.ConverterParameter;
			((BindingBase)val26.TypedBinding).StringFormat = val26.StringFormat;
			val26.TypedBinding.Source = val26.Source;
			val26.TypedBinding.UpdateSourceEventName = val26.UpdateSourceEventName;
			((BindingBase)val26.TypedBinding).FallbackValue = val26.FallbackValue;
			((BindingBase)val26.TypedBinding).TargetNullValue = val26.TargetNullValue;
			BindingBase typedBinding3 = (BindingBase)(object)val26.TypedBinding;
			((BindableObject)val27).SetBinding(Button.CommandProperty, typedBinding3);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val28).Children).Add((IView)(object)val27);
			((BindableObject)warningPage).SetValue(ContentPage.ContentProperty, (object)val28);
		}
	}
	public abstract class WarningViewModel : PageViewModel
	{
		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <Back>d__3 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public WarningViewModel <>4__this;

			private TaskAwaiter<INavigationResult> <>u__1;

			private void MoveNext()
			{
				//IL_004d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_0059: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				WarningViewModel warningViewModel = <>4__this;
				try
				{
					TaskAwaiter<INavigationResult> val;
					if (num != 0)
					{
						val = ((PageViewModel)warningViewModel).NavigationService.GoBackAsync((INavigationParameters)null).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<INavigationResult>, <Back>d__3>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<INavigationResult>);
						num = (<>1__state = -1);
					}
					val.GetResult();
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

		[ObservableProperty]
		private string? _message = string.Empty;

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		private AsyncRelayCommand? acceptCommand;

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		private AsyncRelayCommand? backCommand;

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string? Message
		{
			get
			{
				return _message;
			}
			set
			{
				if (!EqualityComparer<string>.Default.Equals(_message, value))
				{
					((ObservableObject)this).OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Message);
					_message = value;
					((ObservableObject)this).OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Message);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public IAsyncRelayCommand AcceptCommand
		{
			get
			{
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Expected O, but got Unknown
				//IL_0024: Expected O, but got Unknown
				AsyncRelayCommand obj = acceptCommand;
				if (obj == null)
				{
					AsyncRelayCommand val = new AsyncRelayCommand((Func<global::System.Threading.Tasks.Task>)Accept);
					AsyncRelayCommand val2 = val;
					acceptCommand = val;
					obj = val2;
				}
				return (IAsyncRelayCommand)(object)obj;
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public IAsyncRelayCommand BackCommand
		{
			get
			{
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Expected O, but got Unknown
				//IL_0023: Expected O, but got Unknown
				AsyncRelayCommand obj = backCommand;
				if (obj == null)
				{
					AsyncRelayCommand val = new AsyncRelayCommand((Func<global::System.Threading.Tasks.Task>)Back);
					AsyncRelayCommand val2 = val;
					backCommand = val;
					obj = val2;
				}
				return (IAsyncRelayCommand)(object)obj;
			}
		}

		protected WarningViewModel(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		[RelayCommand(AllowConcurrentExecutions = false)]
		protected abstract global::System.Threading.Tasks.Task Accept();

		[AsyncStateMachine(typeof(<Back>d__3))]
		[RelayCommand(AllowConcurrentExecutions = false)]
		protected global::System.Threading.Tasks.Task Back()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			<Back>d__3 <Back>d__ = default(<Back>d__3);
			<Back>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<Back>d__.<>4__this = this;
			<Back>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <Back>d__.<>t__builder)).Start<<Back>d__3>(ref <Back>d__);
			return ((AsyncTaskMethodBuilder)(ref <Back>d__.<>t__builder)).Task;
		}
	}
}
namespace App.Common.Pages.Pairing.Success
{
	[XamlFilePath("Success/SuccessPage.xaml")]
	public class SuccessPage : ContentPage
	{
		public SuccessPage()
		{
			InitializeComponent();
		}

		[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
		private void InitializeComponent()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected O, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Expected O, but got Unknown
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Expected O, but got Unknown
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Expected O, but got Unknown
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected O, but got Unknown
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Expected O, but got Unknown
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Expected O, but got Unknown
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Expected O, but got Unknown
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Expected O, but got Unknown
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Expected O, but got Unknown
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected O, but got Unknown
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Expected O, but got Unknown
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Expected O, but got Unknown
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Expected O, but got Unknown
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Expected O, but got Unknown
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Expected O, but got Unknown
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Expected O, but got Unknown
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Expected O, but got Unknown
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Expected O, but got Unknown
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Expected O, but got Unknown
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Expected O, but got Unknown
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Expected O, but got Unknown
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Expected O, but got Unknown
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Expected O, but got Unknown
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Expected O, but got Unknown
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Expected O, but got Unknown
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Expected O, but got Unknown
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Expected O, but got Unknown
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Expected O, but got Unknown
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Expected O, but got Unknown
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Expected O, but got Unknown
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Expected O, but got Unknown
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Expected O, but got Unknown
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Expected O, but got Unknown
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Expected O, but got Unknown
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Expected O, but got Unknown
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Expected O, but got Unknown
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Expected O, but got Unknown
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Expected O, but got Unknown
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Expected O, but got Unknown
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Expected O, but got Unknown
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Expected O, but got Unknown
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Expected O, but got Unknown
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Expected O, but got Unknown
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Expected O, but got Unknown
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Expected O, but got Unknown
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Expected O, but got Unknown
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Expected O, but got Unknown
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Expected O, but got Unknown
			//IL_031c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0321: Unknown result type (might be due to invalid IL or missing references)
			//IL_032c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0331: Unknown result type (might be due to invalid IL or missing references)
			//IL_0341: Unknown result type (might be due to invalid IL or missing references)
			//IL_0351: Unknown result type (might be due to invalid IL or missing references)
			//IL_0361: Unknown result type (might be due to invalid IL or missing references)
			//IL_0371: Unknown result type (might be due to invalid IL or missing references)
			//IL_0395: Expected O, but got Unknown
			//IL_0390: Unknown result type (might be due to invalid IL or missing references)
			//IL_039a: Expected O, but got Unknown
			//IL_039f: Expected O, but got Unknown
			//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d9: Expected O, but got Unknown
			//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03de: Expected O, but got Unknown
			//IL_03e3: Expected O, but got Unknown
			//IL_0410: Unknown result type (might be due to invalid IL or missing references)
			//IL_0415: Unknown result type (might be due to invalid IL or missing references)
			//IL_041b: Expected O, but got Unknown
			//IL_0426: Unknown result type (might be due to invalid IL or missing references)
			//IL_042b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0431: Expected O, but got Unknown
			//IL_0433: Unknown result type (might be due to invalid IL or missing references)
			//IL_0438: Unknown result type (might be due to invalid IL or missing references)
			//IL_043e: Expected O, but got Unknown
			//IL_0440: Unknown result type (might be due to invalid IL or missing references)
			//IL_0445: Unknown result type (might be due to invalid IL or missing references)
			//IL_044b: Expected O, but got Unknown
			//IL_044d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0452: Unknown result type (might be due to invalid IL or missing references)
			//IL_0458: Expected O, but got Unknown
			//IL_0458: Unknown result type (might be due to invalid IL or missing references)
			//IL_0462: Expected O, but got Unknown
			//IL_0471: Unknown result type (might be due to invalid IL or missing references)
			//IL_0476: Unknown result type (might be due to invalid IL or missing references)
			//IL_047c: Expected O, but got Unknown
			//IL_047e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0483: Unknown result type (might be due to invalid IL or missing references)
			//IL_0489: Expected O, but got Unknown
			//IL_0489: Unknown result type (might be due to invalid IL or missing references)
			//IL_0493: Expected O, but got Unknown
			//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0501: Unknown result type (might be due to invalid IL or missing references)
			//IL_0510: Unknown result type (might be due to invalid IL or missing references)
			//IL_051a: Expected O, but got Unknown
			//IL_0515: Unknown result type (might be due to invalid IL or missing references)
			//IL_051f: Expected O, but got Unknown
			//IL_0524: Expected O, but got Unknown
			//IL_0540: Unknown result type (might be due to invalid IL or missing references)
			//IL_056f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0574: Unknown result type (might be due to invalid IL or missing references)
			//IL_0583: Unknown result type (might be due to invalid IL or missing references)
			//IL_058d: Expected O, but got Unknown
			//IL_0588: Unknown result type (might be due to invalid IL or missing references)
			//IL_0592: Expected O, but got Unknown
			//IL_0597: Expected O, but got Unknown
			//IL_05f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_060c: Unknown result type (might be due to invalid IL or missing references)
			//IL_065f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0696: Unknown result type (might be due to invalid IL or missing references)
			//IL_069b: Unknown result type (might be due to invalid IL or missing references)
			//IL_06aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b4: Expected O, but got Unknown
			//IL_06af: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b9: Expected O, but got Unknown
			//IL_06be: Expected O, but got Unknown
			//IL_06e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0760: Unknown result type (might be due to invalid IL or missing references)
			//IL_0765: Unknown result type (might be due to invalid IL or missing references)
			//IL_0774: Unknown result type (might be due to invalid IL or missing references)
			//IL_077e: Expected O, but got Unknown
			//IL_0779: Unknown result type (might be due to invalid IL or missing references)
			//IL_0783: Expected O, but got Unknown
			//IL_0788: Expected O, but got Unknown
			//IL_084a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0860: Unknown result type (might be due to invalid IL or missing references)
			//IL_0876: Unknown result type (might be due to invalid IL or missing references)
			//IL_08da: Unknown result type (might be due to invalid IL or missing references)
			//IL_08f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0929: Unknown result type (might be due to invalid IL or missing references)
			//IL_092e: Unknown result type (might be due to invalid IL or missing references)
			//IL_093d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0947: Expected O, but got Unknown
			//IL_0942: Unknown result type (might be due to invalid IL or missing references)
			//IL_094c: Expected O, but got Unknown
			//IL_0951: Expected O, but got Unknown
			//IL_09bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_09d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a0c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a11: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a20: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a2a: Expected O, but got Unknown
			//IL_0a25: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a2f: Expected O, but got Unknown
			//IL_0a34: Expected O, but got Unknown
			//IL_0aa0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ab6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0af4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b03: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b0d: Expected O, but got Unknown
			//IL_0b08: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b12: Expected O, but got Unknown
			//IL_0b17: Expected O, but got Unknown
			//IL_0bd9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c05: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c69: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c7f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cb8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cbd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ccc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cd6: Expected O, but got Unknown
			//IL_0cd1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0cdb: Expected O, but got Unknown
			//IL_0ce0: Expected O, but got Unknown
			//IL_0d4c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d62: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d9b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0da0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0daf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0db9: Expected O, but got Unknown
			//IL_0db4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dbe: Expected O, but got Unknown
			//IL_0dc3: Expected O, but got Unknown
			//IL_0e2f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e45: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e7e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e83: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e92: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e9c: Expected O, but got Unknown
			//IL_0e97: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ea1: Expected O, but got Unknown
			//IL_0ea6: Expected O, but got Unknown
			//IL_0f17: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f1c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f2b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f35: Expected O, but got Unknown
			//IL_0f30: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f3a: Expected O, but got Unknown
			//IL_0f3f: Expected O, but got Unknown
			//IL_0f5b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f8a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f8f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f9e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fa8: Expected O, but got Unknown
			//IL_0fa3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fad: Expected O, but got Unknown
			//IL_0fb2: Expected O, but got Unknown
			//IL_101f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1105: Unknown result type (might be due to invalid IL or missing references)
			//IL_110a: Unknown result type (might be due to invalid IL or missing references)
			//IL_1119: Unknown result type (might be due to invalid IL or missing references)
			//IL_1123: Expected O, but got Unknown
			//IL_111e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1128: Expected O, but got Unknown
			//IL_112d: Expected O, but got Unknown
			//IL_1149: Unknown result type (might be due to invalid IL or missing references)
			//IL_1178: Unknown result type (might be due to invalid IL or missing references)
			//IL_117d: Unknown result type (might be due to invalid IL or missing references)
			//IL_118c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1196: Expected O, but got Unknown
			//IL_1191: Unknown result type (might be due to invalid IL or missing references)
			//IL_119b: Expected O, but got Unknown
			//IL_11a0: Expected O, but got Unknown
			//IL_120d: Unknown result type (might be due to invalid IL or missing references)
			//IL_130d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1312: Unknown result type (might be due to invalid IL or missing references)
			//IL_1324: Unknown result type (might be due to invalid IL or missing references)
			//IL_132e: Expected O, but got Unknown
			//IL_1329: Unknown result type (might be due to invalid IL or missing references)
			//IL_1333: Expected O, but got Unknown
			//IL_1338: Expected O, but got Unknown
			//IL_135b: Unknown result type (might be due to invalid IL or missing references)
			//IL_1360: Unknown result type (might be due to invalid IL or missing references)
			//IL_1372: Unknown result type (might be due to invalid IL or missing references)
			//IL_137c: Expected O, but got Unknown
			//IL_1377: Unknown result type (might be due to invalid IL or missing references)
			//IL_1381: Expected O, but got Unknown
			//IL_1386: Expected O, but got Unknown
			//IL_13d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_13db: Unknown result type (might be due to invalid IL or missing references)
			//IL_13ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_13f7: Expected O, but got Unknown
			//IL_13f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_13fc: Expected O, but got Unknown
			//IL_1401: Expected O, but got Unknown
			//IL_1424: Unknown result type (might be due to invalid IL or missing references)
			//IL_1429: Unknown result type (might be due to invalid IL or missing references)
			//IL_143b: Unknown result type (might be due to invalid IL or missing references)
			//IL_1445: Expected O, but got Unknown
			//IL_1440: Unknown result type (might be due to invalid IL or missing references)
			//IL_144a: Expected O, but got Unknown
			//IL_144f: Expected O, but got Unknown
			//IL_14b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_15af: Unknown result type (might be due to invalid IL or missing references)
			//IL_16ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_176f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1774: Unknown result type (might be due to invalid IL or missing references)
			//IL_1786: Unknown result type (might be due to invalid IL or missing references)
			//IL_1790: Expected O, but got Unknown
			//IL_178b: Unknown result type (might be due to invalid IL or missing references)
			//IL_1795: Expected O, but got Unknown
			//IL_179a: Expected O, but got Unknown
			//IL_17bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_17c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_17d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_17de: Expected O, but got Unknown
			//IL_17d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_17e3: Expected O, but got Unknown
			//IL_17e8: Expected O, but got Unknown
			//IL_1838: Unknown result type (might be due to invalid IL or missing references)
			//IL_183d: Unknown result type (might be due to invalid IL or missing references)
			//IL_184f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1859: Expected O, but got Unknown
			//IL_1854: Unknown result type (might be due to invalid IL or missing references)
			//IL_185e: Expected O, but got Unknown
			//IL_1863: Expected O, but got Unknown
			//IL_1886: Unknown result type (might be due to invalid IL or missing references)
			//IL_188b: Unknown result type (might be due to invalid IL or missing references)
			//IL_189d: Unknown result type (might be due to invalid IL or missing references)
			//IL_18a7: Expected O, but got Unknown
			//IL_18a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_18ac: Expected O, but got Unknown
			//IL_18b1: Expected O, but got Unknown
			//IL_1924: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			Window current = Window.Current;
			BindingExtension val = new BindingExtension();
			DynamicResourceExtension val2 = new DynamicResourceExtension();
			DynamicResourceExtension val3 = new DynamicResourceExtension();
			DynamicResourceExtension val4 = new DynamicResourceExtension();
			Label val5 = new Label();
			DynamicResourceExtension val6 = new DynamicResourceExtension();
			RoundRectangle val7 = new RoundRectangle();
			string proSolid = FontAwesomeFontFamily.ProSolid900;
			string circleCheck = FontAwesomeGlyph.CircleCheck;
			DynamicResourceExtension val8 = new DynamicResourceExtension();
			FontImageSource val9 = new FontImageSource();
			SKImageView val10 = new SKImageView();
			Border val11 = new Border();
			string proSolid2 = FontAwesomeFontFamily.ProSolid900;
			string star = FontAwesomeGlyph.Star;
			DynamicResourceExtension val12 = new DynamicResourceExtension();
			FontImageSource val13 = new FontImageSource();
			SKImageView val14 = new SKImageView();
			string proSolid3 = FontAwesomeFontFamily.ProSolid900;
			string star2 = FontAwesomeGlyph.Star;
			DynamicResourceExtension val15 = new DynamicResourceExtension();
			FontImageSource val16 = new FontImageSource();
			SKImageView val17 = new SKImageView();
			string proSolid4 = FontAwesomeFontFamily.ProSolid900;
			string star3 = FontAwesomeGlyph.Star;
			DynamicResourceExtension val18 = new DynamicResourceExtension();
			FontImageSource val19 = new FontImageSource();
			SKImageView val20 = new SKImageView();
			Grid val21 = new Grid();
			string proSolid5 = FontAwesomeFontFamily.ProSolid900;
			string star4 = FontAwesomeGlyph.Star;
			DynamicResourceExtension val22 = new DynamicResourceExtension();
			FontImageSource val23 = new FontImageSource();
			SKImageView val24 = new SKImageView();
			string proSolid6 = FontAwesomeFontFamily.ProSolid900;
			string star5 = FontAwesomeGlyph.Star;
			DynamicResourceExtension val25 = new DynamicResourceExtension();
			FontImageSource val26 = new FontImageSource();
			SKImageView val27 = new SKImageView();
			string proSolid7 = FontAwesomeFontFamily.ProSolid900;
			string star6 = FontAwesomeGlyph.Star;
			DynamicResourceExtension val28 = new DynamicResourceExtension();
			FontImageSource val29 = new FontImageSource();
			SKImageView val30 = new SKImageView();
			Grid val31 = new Grid();
			DynamicResourceExtension val32 = new DynamicResourceExtension();
			DynamicResourceExtension val33 = new DynamicResourceExtension();
			BindingExtension val34 = new BindingExtension();
			Label val35 = new Label();
			DynamicResourceExtension val36 = new DynamicResourceExtension();
			DynamicResourceExtension val37 = new DynamicResourceExtension();
			BindingExtension val38 = new BindingExtension();
			Label val39 = new Label();
			DynamicResourceExtension val40 = new DynamicResourceExtension();
			DynamicResourceExtension val41 = new DynamicResourceExtension();
			DynamicResourceExtension val42 = new DynamicResourceExtension();
			DynamicResourceExtension val43 = new DynamicResourceExtension();
			BindingExtension val44 = new BindingExtension();
			BindingExtension val45 = new BindingExtension();
			BindingExtension val46 = new BindingExtension();
			Button val47 = new Button();
			DynamicResourceExtension val48 = new DynamicResourceExtension();
			DynamicResourceExtension val49 = new DynamicResourceExtension();
			DynamicResourceExtension val50 = new DynamicResourceExtension();
			DynamicResourceExtension val51 = new DynamicResourceExtension();
			BindingExtension val52 = new BindingExtension();
			Button val53 = new Button();
			VerticalStackLayout val54 = new VerticalStackLayout();
			Grid val55 = new Grid();
			SuccessPage successPage;
			NameScope val56 = (NameScope)(((object)NameScope.GetNameScope((BindableObject)(object)(successPage = this))) ?? ((object)new NameScope()));
			NameScope.SetNameScope((BindableObject)(object)successPage, (INameScope)(object)val56);
			((Element)val55).transientNamescope = (INameScope)(object)val56;
			((Element)val5).transientNamescope = (INameScope)(object)val56;
			((Element)val11).transientNamescope = (INameScope)(object)val56;
			((Element)val7).transientNamescope = (INameScope)(object)val56;
			((Element)val10).transientNamescope = (INameScope)(object)val56;
			((Element)val9).transientNamescope = (INameScope)(object)val56;
			((Element)val21).transientNamescope = (INameScope)(object)val56;
			((Element)val14).transientNamescope = (INameScope)(object)val56;
			((Element)val13).transientNamescope = (INameScope)(object)val56;
			((Element)val17).transientNamescope = (INameScope)(object)val56;
			((Element)val16).transientNamescope = (INameScope)(object)val56;
			((Element)val20).transientNamescope = (INameScope)(object)val56;
			((Element)val19).transientNamescope = (INameScope)(object)val56;
			((Element)val31).transientNamescope = (INameScope)(object)val56;
			((Element)val24).transientNamescope = (INameScope)(object)val56;
			((Element)val23).transientNamescope = (INameScope)(object)val56;
			((Element)val27).transientNamescope = (INameScope)(object)val56;
			((Element)val26).transientNamescope = (INameScope)(object)val56;
			((Element)val30).transientNamescope = (INameScope)(object)val56;
			((Element)val29).transientNamescope = (INameScope)(object)val56;
			((Element)val35).transientNamescope = (INameScope)(object)val56;
			((Element)val39).transientNamescope = (INameScope)(object)val56;
			((Element)val54).transientNamescope = (INameScope)(object)val56;
			((Element)val47).transientNamescope = (INameScope)(object)val56;
			((Element)val53).transientNamescope = (INameScope)(object)val56;
			((BindableObject)successPage).SetValue(NavigationPage.HasNavigationBarProperty, (object)false);
			((BindableObject)successPage).SetValue(Window.IgnoreInsetsProperty, (object)true);
			val.Path = "Insets";
			val.Source = current;
			XamlServiceProvider val57 = new XamlServiceProvider();
			global::System.Type typeFromHandle = typeof(IXamlTypeResolver);
			XmlNamespaceResolver val58 = new XmlNamespaceResolver();
			val58.Add("", "http://schemas.microsoft.com/dotnet/2021/maui");
			val58.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			val58.Add("ui", "http://lci1.com/schemas/ui");
			val58.Add("peridot", "http://lci1.com/schemas/peridot");
			val58.Add("success", "clr-namespace:App.Common.Pages.Pairing.Success");
			val57.Add(typeFromHandle, (object)new XamlTypeResolver((IXmlNamespaceResolver)val58, typeof(SuccessPage).Assembly));
			BindingBase val59 = ((IMarkupExtension<BindingBase>)(object)val).ProvideValue((IServiceProvider)val57);
			((BindableObject)successPage).SetBinding(Page.PaddingProperty, val59);
			val2.Key = "TertiaryContainer";
			XamlServiceProvider val60 = new XamlServiceProvider();
			val60.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(13, 14)));
			DynamicResource val61 = ((IMarkupExtension<DynamicResource>)(object)val2).ProvideValue((IServiceProvider)val60);
			((IDynamicResourceHandler)successPage).SetDynamicResource(VisualElement.BackgroundColorProperty, val61.Key);
			((BindableObject)val55).SetValue(Grid.RowDefinitionsProperty, (object)new RowDefinitionCollection((RowDefinition[])(object)new RowDefinition[5]
			{
				new RowDefinition(new GridLength(64.0)),
				new RowDefinition(new GridLength(296.0)),
				new RowDefinition(GridLength.Star),
				new RowDefinition(GridLength.Star),
				new RowDefinition(GridLength.Auto)
			}));
			((BindableObject)val55).SetValue(Grid.ColumnDefinitionsProperty, (object)new ColumnDefinitionCollection((ColumnDefinition[])(object)new ColumnDefinition[2]
			{
				new ColumnDefinition(GridLength.Star),
				new ColumnDefinition(GridLength.Star)
			}));
			((BindableObject)val55).SetValue(Grid.RowSpacingProperty, (object)16.0);
			((BindableObject)val55).SetValue(Layout.PaddingProperty, (object)new Thickness(16.0));
			((BindableObject)val5).SetValue(Grid.RowProperty, (object)0);
			((BindableObject)val5).SetValue(Grid.ColumnSpanProperty, (object)2);
			val3.Key = "H2_Label";
			XamlServiceProvider val62 = new XamlServiceProvider();
			val62.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(21, 20)));
			DynamicResource val63 = ((IMarkupExtension<DynamicResource>)(object)val3).ProvideValue((IServiceProvider)val62);
			((IDynamicResourceHandler)val5).SetDynamicResource(VisualElement.StyleProperty, val63.Key);
			((BindableObject)val5).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Center);
			((BindableObject)val5).SetValue(Label.HorizontalTextAlignmentProperty, (object)(TextAlignment)1);
			val4.Key = "OnTertiaryContainer";
			XamlServiceProvider val64 = new XamlServiceProvider();
			val64.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(24, 20)));
			DynamicResource val65 = ((IMarkupExtension<DynamicResource>)(object)val4).ProvideValue((IServiceProvider)val64);
			((IDynamicResourceHandler)val5).SetDynamicResource(Label.TextColorProperty, val65.Key);
			((BindableObject)val5).SetValue(Label.TextProperty, (object)"Congrats!");
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val55).Children).Add((IView)(object)val5);
			((BindableObject)val11).SetValue(Grid.RowProperty, (object)1);
			((BindableObject)val11).SetValue(Grid.ColumnSpanProperty, (object)2);
			((BindableObject)val11).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
			((BindableObject)val11).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Start);
			((BindableObject)val11).SetValue(VisualElement.WidthRequestProperty, (object)248.0);
			((BindableObject)val11).SetValue(VisualElement.HeightRequestProperty, (object)248.0);
			((BindableObject)val11).SetValue(Border.PaddingProperty, (object)new Thickness(4.0));
			((BindableObject)val11).SetValue(Border.StrokeThicknessProperty, (object)4.0);
			val6.Key = "OnTertiaryContainer";
			XamlServiceProvider val66 = new XamlServiceProvider();
			val66.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(34, 21)));
			DynamicResource val67 = ((IMarkupExtension<DynamicResource>)(object)val6).ProvideValue((IServiceProvider)val66);
			((IDynamicResourceHandler)val11).SetDynamicResource(Border.StrokeProperty, val67.Key);
			((BindableObject)val7).SetValue(RoundRectangle.CornerRadiusProperty, (object)new CornerRadius(124.0));
			((BindableObject)val11).SetValue(Border.StrokeShapeProperty, (object)val7);
			((BindableObject)val10).SetValue(SKImageView.AspectProperty, (object)(Aspect)0);
			((BindableObject)val10).SetValue(SKImageView.HorizontalImageAlignmentProperty, (object)(ImageAlignment)1);
			((BindableObject)val10).SetValue(SKImageView.VerticalImageAlignmentProperty, (object)(ImageAlignment)1);
			((BindableObject)val9).SetValue(FontImageSource.FontFamilyProperty, (object)proSolid);
			((BindableObject)val9).SetValue(FontImageSource.GlyphProperty, (object)circleCheck);
			val8.Key = "Tertiary";
			XamlServiceProvider val68 = new XamlServiceProvider();
			val68.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(44, 42)));
			DynamicResource val69 = ((IMarkupExtension<DynamicResource>)(object)val8).ProvideValue((IServiceProvider)val68);
			((IDynamicResourceHandler)val9).SetDynamicResource(FontImageSource.ColorProperty, val69.Key);
			((BindableObject)val10).SetValue(SKImageView.ImageSourceProperty, (object)val9);
			((BindableObject)val11).SetValue(Border.ContentProperty, (object)val10);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val55).Children).Add((IView)(object)val11);
			((BindableObject)val21).SetValue(Grid.RowProperty, (object)1);
			((BindableObject)val21).SetValue(Grid.ColumnProperty, (object)0);
			((BindableObject)val21).SetValue(VisualElement.WidthRequestProperty, (object)72.0);
			((BindableObject)val21).SetValue(VisualElement.HeightRequestProperty, (object)96.0);
			((BindableObject)val21).SetValue(View.MarginProperty, (object)new Thickness(0.0, 0.0, 96.0, 0.0));
			((BindableObject)val21).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
			((BindableObject)val21).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.End);
			((BindableObject)val21).SetValue(VisualElement.RotationProperty, (object)(-15.0));
			((BindableObject)val14).SetValue(VisualElement.WidthRequestProperty, (object)32.0);
			((BindableObject)val14).SetValue(VisualElement.HeightRequestProperty, (object)32.0);
			((BindableObject)val14).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.End);
			((BindableObject)val14).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Start);
			((BindableObject)val13).SetValue(FontImageSource.FontFamilyProperty, (object)proSolid2);
			((BindableObject)val13).SetValue(FontImageSource.GlyphProperty, (object)star);
			val12.Key = "Secondary";
			XamlServiceProvider val70 = new XamlServiceProvider();
			val70.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(60, 33)));
			DynamicResource val71 = ((IMarkupExtension<DynamicResource>)(object)val12).ProvideValue((IServiceProvider)val70);
			((IDynamicResourceHandler)val13).SetDynamicResource(FontImageSource.ColorProperty, val71.Key);
			((BindableObject)val14).SetValue(SKImageView.ImageSourceProperty, (object)val13);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val21).Children).Add((IView)(object)val14);
			((BindableObject)val17).SetValue(VisualElement.WidthRequestProperty, (object)24.0);
			((BindableObject)val17).SetValue(VisualElement.HeightRequestProperty, (object)24.0);
			((BindableObject)val17).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Start);
			((BindableObject)val17).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Center);
			((BindableObject)val16).SetValue(FontImageSource.FontFamilyProperty, (object)proSolid3);
			((BindableObject)val16).SetValue(FontImageSource.GlyphProperty, (object)star2);
			val15.Key = "Secondary";
			XamlServiceProvider val72 = new XamlServiceProvider();
			val72.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(68, 33)));
			DynamicResource val73 = ((IMarkupExtension<DynamicResource>)(object)val15).ProvideValue((IServiceProvider)val72);
			((IDynamicResourceHandler)val16).SetDynamicResource(FontImageSource.ColorProperty, val73.Key);
			((BindableObject)val17).SetValue(SKImageView.ImageSourceProperty, (object)val16);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val21).Children).Add((IView)(object)val17);
			((BindableObject)val20).SetValue(VisualElement.WidthRequestProperty, (object)48.0);
			((BindableObject)val20).SetValue(VisualElement.HeightRequestProperty, (object)48.0);
			((BindableObject)val20).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.End);
			((BindableObject)val20).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.End);
			((BindableObject)val19).SetValue(FontImageSource.FontFamilyProperty, (object)proSolid4);
			((BindableObject)val19).SetValue(FontImageSource.GlyphProperty, (object)star3);
			val18.Key = "Secondary";
			XamlServiceProvider val74 = new XamlServiceProvider();
			val74.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(76, 33)));
			DynamicResource val75 = ((IMarkupExtension<DynamicResource>)(object)val18).ProvideValue((IServiceProvider)val74);
			((IDynamicResourceHandler)val19).SetDynamicResource(FontImageSource.ColorProperty, val75.Key);
			((BindableObject)val20).SetValue(SKImageView.ImageSourceProperty, (object)val19);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val21).Children).Add((IView)(object)val20);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val55).Children).Add((IView)(object)val21);
			((BindableObject)val31).SetValue(Grid.RowProperty, (object)1);
			((BindableObject)val31).SetValue(Grid.ColumnProperty, (object)1);
			((BindableObject)val31).SetValue(VisualElement.WidthRequestProperty, (object)72.0);
			((BindableObject)val31).SetValue(VisualElement.HeightRequestProperty, (object)96.0);
			((BindableObject)val31).SetValue(View.MarginProperty, (object)new Thickness(96.0, 0.0, 0.0, 0.0));
			((BindableObject)val31).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
			((BindableObject)val31).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.End);
			((BindableObject)val31).SetValue(VisualElement.RotationProperty, (object)15.0);
			((BindableObject)val24).SetValue(VisualElement.WidthRequestProperty, (object)32.0);
			((BindableObject)val24).SetValue(VisualElement.HeightRequestProperty, (object)32.0);
			((BindableObject)val24).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Start);
			((BindableObject)val24).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Start);
			((BindableObject)val23).SetValue(FontImageSource.FontFamilyProperty, (object)proSolid5);
			((BindableObject)val23).SetValue(FontImageSource.GlyphProperty, (object)star4);
			val22.Key = "Secondary";
			XamlServiceProvider val76 = new XamlServiceProvider();
			val76.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(93, 33)));
			DynamicResource val77 = ((IMarkupExtension<DynamicResource>)(object)val22).ProvideValue((IServiceProvider)val76);
			((IDynamicResourceHandler)val23).SetDynamicResource(FontImageSource.ColorProperty, val77.Key);
			((BindableObject)val24).SetValue(SKImageView.ImageSourceProperty, (object)val23);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val31).Children).Add((IView)(object)val24);
			((BindableObject)val27).SetValue(VisualElement.WidthRequestProperty, (object)24.0);
			((BindableObject)val27).SetValue(VisualElement.HeightRequestProperty, (object)24.0);
			((BindableObject)val27).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.End);
			((BindableObject)val27).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Center);
			((BindableObject)val26).SetValue(FontImageSource.FontFamilyProperty, (object)proSolid6);
			((BindableObject)val26).SetValue(FontImageSource.GlyphProperty, (object)star5);
			val25.Key = "Secondary";
			XamlServiceProvider val78 = new XamlServiceProvider();
			val78.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(101, 33)));
			DynamicResource val79 = ((IMarkupExtension<DynamicResource>)(object)val25).ProvideValue((IServiceProvider)val78);
			((IDynamicResourceHandler)val26).SetDynamicResource(FontImageSource.ColorProperty, val79.Key);
			((BindableObject)val27).SetValue(SKImageView.ImageSourceProperty, (object)val26);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val31).Children).Add((IView)(object)val27);
			((BindableObject)val30).SetValue(VisualElement.WidthRequestProperty, (object)48.0);
			((BindableObject)val30).SetValue(VisualElement.HeightRequestProperty, (object)48.0);
			((BindableObject)val30).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Start);
			((BindableObject)val30).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.End);
			((BindableObject)val29).SetValue(FontImageSource.FontFamilyProperty, (object)proSolid7);
			((BindableObject)val29).SetValue(FontImageSource.GlyphProperty, (object)star6);
			val28.Key = "Secondary";
			XamlServiceProvider val80 = new XamlServiceProvider();
			val80.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(109, 33)));
			DynamicResource val81 = ((IMarkupExtension<DynamicResource>)(object)val28).ProvideValue((IServiceProvider)val80);
			((IDynamicResourceHandler)val29).SetDynamicResource(FontImageSource.ColorProperty, val81.Key);
			((BindableObject)val30).SetValue(SKImageView.ImageSourceProperty, (object)val29);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val31).Children).Add((IView)(object)val30);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val55).Children).Add((IView)(object)val31);
			((BindableObject)val35).SetValue(Grid.RowProperty, (object)2);
			((BindableObject)val35).SetValue(Grid.ColumnSpanProperty, (object)2);
			val32.Key = "H3_Label";
			XamlServiceProvider val82 = new XamlServiceProvider();
			val82.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(116, 20)));
			DynamicResource val83 = ((IMarkupExtension<DynamicResource>)(object)val32).ProvideValue((IServiceProvider)val82);
			((IDynamicResourceHandler)val35).SetDynamicResource(VisualElement.StyleProperty, val83.Key);
			((BindableObject)val35).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Center);
			((BindableObject)val35).SetValue(Label.HorizontalTextAlignmentProperty, (object)(TextAlignment)1);
			val33.Key = "OnTertiaryContainer";
			XamlServiceProvider val84 = new XamlServiceProvider();
			val84.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(119, 20)));
			DynamicResource val85 = ((IMarkupExtension<DynamicResource>)(object)val33).ProvideValue((IServiceProvider)val84);
			((IDynamicResourceHandler)val35).SetDynamicResource(Label.TextColorProperty, val85.Key);
			val34.Path = "Title";
			val34.TypedBinding = (TypedBindingBase)(object)new TypedBinding<SuccessViewModel, string>((Func<SuccessViewModel, ValueTuple<string, bool>>)([CompilerGenerated] (SuccessViewModel P_0) => (P_0 != null) ? new ValueTuple<string, bool>(P_0.Title, true) : default(ValueTuple<string, bool>)), (Action<SuccessViewModel, string>)([CompilerGenerated] (SuccessViewModel P_0, string P_1) =>
			{
				if (P_0 != null)
				{
					P_0.Title = P_1;
				}
			}), new Tuple<Func<SuccessViewModel, object>, string>[1]
			{
				new Tuple<Func<SuccessViewModel, object>, string>((Func<SuccessViewModel, object>)([CompilerGenerated] (SuccessViewModel P_0) => P_0), "Title")
			});
			((BindingBase)val34.TypedBinding).Mode = val34.Mode;
			val34.TypedBinding.Converter = val34.Converter;
			val34.TypedBinding.ConverterParameter = val34.ConverterParameter;
			((BindingBase)val34.TypedBinding).StringFormat = val34.StringFormat;
			val34.TypedBinding.Source = val34.Source;
			val34.TypedBinding.UpdateSourceEventName = val34.UpdateSourceEventName;
			((BindingBase)val34.TypedBinding).FallbackValue = val34.FallbackValue;
			((BindingBase)val34.TypedBinding).TargetNullValue = val34.TargetNullValue;
			BindingBase typedBinding = (BindingBase)(object)val34.TypedBinding;
			((BindableObject)val35).SetBinding(Label.TextProperty, typedBinding);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val55).Children).Add((IView)(object)val35);
			((BindableObject)val39).SetValue(Grid.RowProperty, (object)3);
			((BindableObject)val39).SetValue(Grid.ColumnSpanProperty, (object)2);
			val36.Key = "Body2_Label";
			XamlServiceProvider val86 = new XamlServiceProvider();
			val86.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(123, 20)));
			DynamicResource val87 = ((IMarkupExtension<DynamicResource>)(object)val36).ProvideValue((IServiceProvider)val86);
			((IDynamicResourceHandler)val39).SetDynamicResource(VisualElement.StyleProperty, val87.Key);
			((BindableObject)val39).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Center);
			((BindableObject)val39).SetValue(Label.HorizontalTextAlignmentProperty, (object)(TextAlignment)1);
			val37.Key = "OnTertiaryContainer";
			XamlServiceProvider val88 = new XamlServiceProvider();
			val88.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(126, 20)));
			DynamicResource val89 = ((IMarkupExtension<DynamicResource>)(object)val37).ProvideValue((IServiceProvider)val88);
			((IDynamicResourceHandler)val39).SetDynamicResource(Label.TextColorProperty, val89.Key);
			val38.Path = "Message";
			val38.TypedBinding = (TypedBindingBase)(object)new TypedBinding<SuccessViewModel, string>((Func<SuccessViewModel, ValueTuple<string, bool>>)([CompilerGenerated] (SuccessViewModel P_0) => (P_0 != null) ? new ValueTuple<string, bool>(P_0.Message, true) : default(ValueTuple<string, bool>)), (Action<SuccessViewModel, string>)([CompilerGenerated] (SuccessViewModel P_0, string P_1) =>
			{
				if (P_0 != null)
				{
					P_0.Message = P_1;
				}
			}), new Tuple<Func<SuccessViewModel, object>, string>[1]
			{
				new Tuple<Func<SuccessViewModel, object>, string>((Func<SuccessViewModel, object>)([CompilerGenerated] (SuccessViewModel P_0) => P_0), "Message")
			});
			((BindingBase)val38.TypedBinding).Mode = val38.Mode;
			val38.TypedBinding.Converter = val38.Converter;
			val38.TypedBinding.ConverterParameter = val38.ConverterParameter;
			((BindingBase)val38.TypedBinding).StringFormat = val38.StringFormat;
			val38.TypedBinding.Source = val38.Source;
			val38.TypedBinding.UpdateSourceEventName = val38.UpdateSourceEventName;
			((BindingBase)val38.TypedBinding).FallbackValue = val38.FallbackValue;
			((BindingBase)val38.TypedBinding).TargetNullValue = val38.TargetNullValue;
			BindingBase typedBinding2 = (BindingBase)(object)val38.TypedBinding;
			((BindableObject)val39).SetBinding(Label.TextProperty, typedBinding2);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val55).Children).Add((IView)(object)val39);
			((BindableObject)val54).SetValue(Grid.RowProperty, (object)4);
			((BindableObject)val54).SetValue(Grid.ColumnSpanProperty, (object)2);
			((BindableObject)val54).SetValue(StackBase.SpacingProperty, (object)16.0);
			val40.Key = "FontRegular";
			XamlServiceProvider val90 = new XamlServiceProvider();
			val90.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(131, 25)));
			DynamicResource val91 = ((IMarkupExtension<DynamicResource>)(object)val40).ProvideValue((IServiceProvider)val90);
			((IDynamicResourceHandler)val47).SetDynamicResource(Button.FontFamilyProperty, val91.Key);
			val41.Key = "Button_FontSize";
			XamlServiceProvider val92 = new XamlServiceProvider();
			val92.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(132, 25)));
			DynamicResource val93 = ((IMarkupExtension<DynamicResource>)(object)val41).ProvideValue((IServiceProvider)val92);
			((IDynamicResourceHandler)val47).SetDynamicResource(Button.FontSizeProperty, val93.Key);
			((BindableObject)val47).SetValue(VisualElement.HeightRequestProperty, (object)56.0);
			((BindableObject)val47).SetValue(Button.CornerRadiusProperty, (object)28);
			val42.Key = "TertiaryContainer";
			XamlServiceProvider val94 = new XamlServiceProvider();
			val94.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(135, 25)));
			DynamicResource val95 = ((IMarkupExtension<DynamicResource>)(object)val42).ProvideValue((IServiceProvider)val94);
			((IDynamicResourceHandler)val47).SetDynamicResource(VisualElement.BackgroundColorProperty, val95.Key);
			val43.Key = "OnTertiaryContainer";
			XamlServiceProvider val96 = new XamlServiceProvider();
			val96.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(136, 25)));
			DynamicResource val97 = ((IMarkupExtension<DynamicResource>)(object)val43).ProvideValue((IServiceProvider)val96);
			((IDynamicResourceHandler)val47).SetDynamicResource(Button.TextColorProperty, val97.Key);
			val44.Path = "HasAuxAction";
			val44.TypedBinding = (TypedBindingBase)(object)new TypedBinding<SuccessViewModel, bool>((Func<SuccessViewModel, ValueTuple<bool, bool>>)([CompilerGenerated] (SuccessViewModel P_0) => (P_0 != null) ? new ValueTuple<bool, bool>(P_0.HasAuxAction, true) : default(ValueTuple<bool, bool>)), (Action<SuccessViewModel, bool>)null, new Tuple<Func<SuccessViewModel, object>, string>[1]
			{
				new Tuple<Func<SuccessViewModel, object>, string>((Func<SuccessViewModel, object>)([CompilerGenerated] (SuccessViewModel P_0) => P_0), "HasAuxAction")
			});
			((BindingBase)val44.TypedBinding).Mode = val44.Mode;
			val44.TypedBinding.Converter = val44.Converter;
			val44.TypedBinding.ConverterParameter = val44.ConverterParameter;
			((BindingBase)val44.TypedBinding).StringFormat = val44.StringFormat;
			val44.TypedBinding.Source = val44.Source;
			val44.TypedBinding.UpdateSourceEventName = val44.UpdateSourceEventName;
			((BindingBase)val44.TypedBinding).FallbackValue = val44.FallbackValue;
			((BindingBase)val44.TypedBinding).TargetNullValue = val44.TargetNullValue;
			BindingBase typedBinding3 = (BindingBase)(object)val44.TypedBinding;
			((BindableObject)val47).SetBinding(VisualElement.IsVisibleProperty, typedBinding3);
			val45.Path = "AuxActionText";
			val45.TypedBinding = (TypedBindingBase)(object)new TypedBinding<SuccessViewModel, string>((Func<SuccessViewModel, ValueTuple<string, bool>>)([CompilerGenerated] (SuccessViewModel P_0) => (P_0 != null) ? new ValueTuple<string, bool>(P_0.AuxActionText, true) : default(ValueTuple<string, bool>)), (Action<SuccessViewModel, string>)([CompilerGenerated] (SuccessViewModel P_0, string P_1) =>
			{
				if (P_0 != null)
				{
					P_0.AuxActionText = P_1;
				}
			}), new Tuple<Func<SuccessViewModel, object>, string>[1]
			{
				new Tuple<Func<SuccessViewModel, object>, string>((Func<SuccessViewModel, object>)([CompilerGenerated] (SuccessViewModel P_0) => P_0), "AuxActionText")
			});
			((BindingBase)val45.TypedBinding).Mode = val45.Mode;
			val45.TypedBinding.Converter = val45.Converter;
			val45.TypedBinding.ConverterParameter = val45.ConverterParameter;
			((BindingBase)val45.TypedBinding).StringFormat = val45.StringFormat;
			val45.TypedBinding.Source = val45.Source;
			val45.TypedBinding.UpdateSourceEventName = val45.UpdateSourceEventName;
			((BindingBase)val45.TypedBinding).FallbackValue = val45.FallbackValue;
			((BindingBase)val45.TypedBinding).TargetNullValue = val45.TargetNullValue;
			BindingBase typedBinding4 = (BindingBase)(object)val45.TypedBinding;
			((BindableObject)val47).SetBinding(Button.TextProperty, typedBinding4);
			val46.Path = "AuxCommand";
			val46.TypedBinding = (TypedBindingBase)(object)new TypedBinding<SuccessViewModel, ICommand>((Func<SuccessViewModel, ValueTuple<ICommand, bool>>)([CompilerGenerated] (SuccessViewModel P_0) => (P_0 != null) ? new ValueTuple<ICommand, bool>(P_0.AuxCommand, true) : default(ValueTuple<ICommand, bool>)), (Action<SuccessViewModel, ICommand>)([CompilerGenerated] (SuccessViewModel P_0, ICommand P_1) =>
			{
				if (P_0 != null)
				{
					P_0.AuxCommand = P_1;
				}
			}), new Tuple<Func<SuccessViewModel, object>, string>[1]
			{
				new Tuple<Func<SuccessViewModel, object>, string>((Func<SuccessViewModel, object>)([CompilerGenerated] (SuccessViewModel P_0) => P_0), "AuxCommand")
			});
			((BindingBase)val46.TypedBinding).Mode = val46.Mode;
			val46.TypedBinding.Converter = val46.Converter;
			val46.TypedBinding.ConverterParameter = val46.ConverterParameter;
			((BindingBase)val46.TypedBinding).StringFormat = val46.StringFormat;
			val46.TypedBinding.Source = val46.Source;
			val46.TypedBinding.UpdateSourceEventName = val46.UpdateSourceEventName;
			((BindingBase)val46.TypedBinding).FallbackValue = val46.FallbackValue;
			((BindingBase)val46.TypedBinding).TargetNullValue = val46.TargetNullValue;
			BindingBase typedBinding5 = (BindingBase)(object)val46.TypedBinding;
			((BindableObject)val47).SetBinding(Button.CommandProperty, typedBinding5);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val54).Children).Add((IView)(object)val47);
			val48.Key = "FontRegular";
			XamlServiceProvider val98 = new XamlServiceProvider();
			val98.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(140, 25)));
			DynamicResource val99 = ((IMarkupExtension<DynamicResource>)(object)val48).ProvideValue((IServiceProvider)val98);
			((IDynamicResourceHandler)val53).SetDynamicResource(Button.FontFamilyProperty, val99.Key);
			val49.Key = "Button_FontSize";
			XamlServiceProvider val100 = new XamlServiceProvider();
			val100.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(141, 25)));
			DynamicResource val101 = ((IMarkupExtension<DynamicResource>)(object)val49).ProvideValue((IServiceProvider)val100);
			((IDynamicResourceHandler)val53).SetDynamicResource(Button.FontSizeProperty, val101.Key);
			((BindableObject)val53).SetValue(VisualElement.HeightRequestProperty, (object)56.0);
			((BindableObject)val53).SetValue(Button.CornerRadiusProperty, (object)28);
			val50.Key = "Tertiary";
			XamlServiceProvider val102 = new XamlServiceProvider();
			val102.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(144, 25)));
			DynamicResource val103 = ((IMarkupExtension<DynamicResource>)(object)val50).ProvideValue((IServiceProvider)val102);
			((IDynamicResourceHandler)val53).SetDynamicResource(VisualElement.BackgroundColorProperty, val103.Key);
			val51.Key = "OnTertiary";
			XamlServiceProvider val104 = new XamlServiceProvider();
			val104.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(145, 25)));
			DynamicResource val105 = ((IMarkupExtension<DynamicResource>)(object)val51).ProvideValue((IServiceProvider)val104);
			((IDynamicResourceHandler)val53).SetDynamicResource(Button.TextColorProperty, val105.Key);
			((BindableObject)val53).SetValue(Button.TextProperty, (object)"Continue");
			val52.Path = "FinishCommand";
			val52.TypedBinding = (TypedBindingBase)(object)new TypedBinding<SuccessViewModel, IAsyncRelayCommand>((Func<SuccessViewModel, ValueTuple<IAsyncRelayCommand, bool>>)([CompilerGenerated] (SuccessViewModel P_0) => (P_0 != null) ? new ValueTuple<IAsyncRelayCommand, bool>(P_0.FinishCommand, true) : default(ValueTuple<IAsyncRelayCommand, bool>)), (Action<SuccessViewModel, IAsyncRelayCommand>)null, new Tuple<Func<SuccessViewModel, object>, string>[1]
			{
				new Tuple<Func<SuccessViewModel, object>, string>((Func<SuccessViewModel, object>)([CompilerGenerated] (SuccessViewModel P_0) => P_0), "FinishCommand")
			});
			((BindingBase)val52.TypedBinding).Mode = val52.Mode;
			val52.TypedBinding.Converter = val52.Converter;
			val52.TypedBinding.ConverterParameter = val52.ConverterParameter;
			((BindingBase)val52.TypedBinding).StringFormat = val52.StringFormat;
			val52.TypedBinding.Source = val52.Source;
			val52.TypedBinding.UpdateSourceEventName = val52.UpdateSourceEventName;
			((BindingBase)val52.TypedBinding).FallbackValue = val52.FallbackValue;
			((BindingBase)val52.TypedBinding).TargetNullValue = val52.TargetNullValue;
			BindingBase typedBinding6 = (BindingBase)(object)val52.TypedBinding;
			((BindableObject)val53).SetBinding(Button.CommandProperty, typedBinding6);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val54).Children).Add((IView)(object)val53);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val55).Children).Add((IView)(object)val54);
			((BindableObject)successPage).SetValue(ContentPage.ContentProperty, (object)val55);
		}
	}
	public abstract class SuccessViewModel : PageViewModel
	{
		[CompilerGenerated]
		private sealed class <>c__DisplayClass38_0
		{
			[StructLayout((LayoutKind)3)]
			private struct <<OnAuxActionChanged>b__0>d : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncTaskMethodBuilder <>t__builder;

				public <>c__DisplayClass38_0 <>4__this;

				private TaskAwaiter <>u__1;

				private void MoveNext()
				{
					//IL_004c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0051: Unknown result type (might be due to invalid IL or missing references)
					//IL_0058: Unknown result type (might be due to invalid IL or missing references)
					//IL_001c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0021: Unknown result type (might be due to invalid IL or missing references)
					//IL_0035: Unknown result type (might be due to invalid IL or missing references)
					//IL_0036: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					<>c__DisplayClass38_0 <>c__DisplayClass38_ = <>4__this;
					try
					{
						TaskAwaiter val;
						if (num != 0)
						{
							val = <>c__DisplayClass38_.value.Invoke().GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <<OnAuxActionChanged>b__0>d>(ref val, ref this);
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

			public Func<global::System.Threading.Tasks.Task> value;

			[AsyncStateMachine(typeof(<<OnAuxActionChanged>b__0>d))]
			internal global::System.Threading.Tasks.Task <OnAuxActionChanged>b__0(CancellationToken _)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				<<OnAuxActionChanged>b__0>d <<OnAuxActionChanged>b__0>d = default(<<OnAuxActionChanged>b__0>d);
				<<OnAuxActionChanged>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<OnAuxActionChanged>b__0>d.<>4__this = this;
				<<OnAuxActionChanged>b__0>d.<>1__state = -1;
				((AsyncTaskMethodBuilder)(ref <<OnAuxActionChanged>b__0>d.<>t__builder)).Start<<<OnAuxActionChanged>b__0>d>(ref <<OnAuxActionChanged>b__0>d);
				return ((AsyncTaskMethodBuilder)(ref <<OnAuxActionChanged>b__0>d.<>t__builder)).Task;
			}
		}

		[ObservableProperty]
		private string? _title = string.Empty;

		[ObservableProperty]
		private string? _message = string.Empty;

		[ObservableProperty]
		[NotifyPropertyChangedFor("HasAuxAction")]
		private string? _auxActionText = string.Empty;

		[ObservableProperty]
		[NotifyPropertyChangedFor("HasAuxAction")]
		private Func<global::System.Threading.Tasks.Task>? _auxAction;

		private ICommand? _auxCommand;

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		private AsyncRelayCommand? finishCommand;

		public bool HasAuxAction
		{
			get
			{
				if (string.IsNullOrWhiteSpace(AuxActionText))
				{
					return AuxAction != null;
				}
				return false;
			}
		}

		public ICommand? AuxCommand
		{
			get
			{
				return _auxCommand;
			}
			set
			{
				((ViewModel)this).SetProperty<ICommand>(ref _auxCommand, value, "AuxCommand", (ThreadInvokeOption)1, global::System.Array.Empty<string>());
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string? Title
		{
			get
			{
				return _title;
			}
			set
			{
				if (!EqualityComparer<string>.Default.Equals(_title, value))
				{
					((ObservableObject)this).OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Title);
					_title = value;
					((ObservableObject)this).OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Title);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string? Message
		{
			get
			{
				return _message;
			}
			set
			{
				if (!EqualityComparer<string>.Default.Equals(_message, value))
				{
					((ObservableObject)this).OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Message);
					_message = value;
					((ObservableObject)this).OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Message);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string? AuxActionText
		{
			get
			{
				return _auxActionText;
			}
			set
			{
				if (!EqualityComparer<string>.Default.Equals(_auxActionText, value))
				{
					((ObservableObject)this).OnPropertyChanging(__KnownINotifyPropertyChangingArgs.AuxActionText);
					((ObservableObject)this).OnPropertyChanging(__KnownINotifyPropertyChangingArgs.HasAuxAction);
					_auxActionText = value;
					((ObservableObject)this).OnPropertyChanged(__KnownINotifyPropertyChangedArgs.AuxActionText);
					((ObservableObject)this).OnPropertyChanged(__KnownINotifyPropertyChangedArgs.HasAuxAction);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public Func<global::System.Threading.Tasks.Task>? AuxAction
		{
			get
			{
				return _auxAction;
			}
			set
			{
				if (!EqualityComparer<Func<global::System.Threading.Tasks.Task>>.Default.Equals(_auxAction, value))
				{
					((ObservableObject)this).OnPropertyChanging(__KnownINotifyPropertyChangingArgs.AuxAction);
					((ObservableObject)this).OnPropertyChanging(__KnownINotifyPropertyChangingArgs.HasAuxAction);
					_auxAction = value;
					OnAuxActionChanged(value);
					((ObservableObject)this).OnPropertyChanged(__KnownINotifyPropertyChangedArgs.AuxAction);
					((ObservableObject)this).OnPropertyChanged(__KnownINotifyPropertyChangedArgs.HasAuxAction);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public IAsyncRelayCommand FinishCommand
		{
			get
			{
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Expected O, but got Unknown
				//IL_0024: Expected O, but got Unknown
				AsyncRelayCommand obj = finishCommand;
				if (obj == null)
				{
					AsyncRelayCommand val = new AsyncRelayCommand((Func<global::System.Threading.Tasks.Task>)Finish);
					AsyncRelayCommand val2 = val;
					finishCommand = val;
					obj = val2;
				}
				return (IAsyncRelayCommand)(object)obj;
			}
		}

		protected SuccessViewModel(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		[RelayCommand(AllowConcurrentExecutions = false)]
		protected abstract global::System.Threading.Tasks.Task Finish();

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnAuxActionChanged(Func<global::System.Threading.Tasks.Task>? value)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			<>c__DisplayClass38_0 CS$<>8__locals3 = new <>c__DisplayClass38_0();
			CS$<>8__locals3.value = value;
			AuxCommand = (ICommand?)((CS$<>8__locals3.value == null) ? ((AsyncRelayCommand)null) : new AsyncRelayCommand((Func<CancellationToken, global::System.Threading.Tasks.Task>)([AsyncStateMachine(typeof(<>c__DisplayClass38_0.<<OnAuxActionChanged>b__0>d))] (CancellationToken _) =>
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				<>c__DisplayClass38_0.<<OnAuxActionChanged>b__0>d <<OnAuxActionChanged>b__0>d = default(<>c__DisplayClass38_0.<<OnAuxActionChanged>b__0>d);
				<<OnAuxActionChanged>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<OnAuxActionChanged>b__0>d.<>4__this = CS$<>8__locals3;
				<<OnAuxActionChanged>b__0>d.<>1__state = -1;
				((AsyncTaskMethodBuilder)(ref <<OnAuxActionChanged>b__0>d.<>t__builder)).Start<<>c__DisplayClass38_0.<<OnAuxActionChanged>b__0>d>(ref <<OnAuxActionChanged>b__0>d);
				return ((AsyncTaskMethodBuilder)(ref <<OnAuxActionChanged>b__0>d.<>t__builder)).Task;
			}), (AsyncRelayCommandOptions)0));
		}
	}
	public abstract class SuccessViewModel<TAccessoryDevice> : SuccessViewModel where TAccessoryDevice : ILogicalDevice
	{
		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <OnStartAsync>d__6 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public SuccessViewModel<TAccessoryDevice> <>4__this;

			public INavigationParameters parameters;

			public CancellationToken cancellationToken;

			private TAccessoryDevice <accessoryDevice>5__2;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				//IL_005b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0062: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				SuccessViewModel<TAccessoryDevice> successViewModel = <>4__this;
				try
				{
					TaskAwaiter val;
					if (num != 0)
					{
						val = successViewModel.<>n__0(parameters, cancellationToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnStartAsync>d__6>(ref val, ref this);
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
					INavigationParameters obj = parameters;
					if (((obj != null) ? new bool?(obj.TryGetValue<TAccessoryDevice>("AccessoryDeviceKey", ref <accessoryDevice>5__2)) : ((bool?)null)) ?? false)
					{
						successViewModel.AccessoryDevice = <accessoryDevice>5__2;
					}
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<accessoryDevice>5__2 = default(TAccessoryDevice);
					((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
					return;
				}
				<>1__state = -2;
				<accessoryDevice>5__2 = default(TAccessoryDevice);
				((AsyncTaskMethodBuilder)(ref <>t__builder)).SetResult();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				((AsyncTaskMethodBuilder)(ref <>t__builder)).SetStateMachine(stateMachine);
			}
		}

		public const string AccessoryDeviceKey = "AccessoryDeviceKey";

		protected TAccessoryDevice? AccessoryDevice
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		protected SuccessViewModel(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		[AsyncStateMachine(typeof(SuccessViewModel<>.<OnStartAsync>d__6))]
		public override global::System.Threading.Tasks.Task OnStartAsync(INavigationParameters? parameters, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			<OnStartAsync>d__6 <OnStartAsync>d__ = default(<OnStartAsync>d__6);
			<OnStartAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<OnStartAsync>d__.<>4__this = this;
			<OnStartAsync>d__.parameters = parameters;
			<OnStartAsync>d__.cancellationToken = cancellationToken;
			<OnStartAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <OnStartAsync>d__.<>t__builder)).Start<<OnStartAsync>d__6>(ref <OnStartAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <OnStartAsync>d__.<>t__builder)).Task;
		}
	}
}
namespace App.Common.Pages.Pairing.SearchForDevices
{
	public enum BleScanResultDeviceType
	{
		BleGateway,
		TireLinc,
		Accessory,
		Sway,
		Abs,
		RvLink,
		X180T,
		Mopeka,
		Unknown
	}
	public class BleScanResultModel : ObservableObject, IComparable<BleScanResultModel>
	{
		[ObservableProperty]
		private IBleScanResult _bleScanResult;

		[ObservableProperty]
		private string? _name;

		[ObservableProperty]
		private MAC _mac = new MAC();

		[ObservableProperty]
		private int _rssi;

		[ObservableProperty]
		private bool _isSelected;

		[ObservableProperty]
		private bool _isPinPairable;

		[ObservableProperty]
		private bool _isPushToPair;

		[ObservableProperty]
		private bool _isPairingActive;

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public IBleScanResult BleScanResult
		{
			get
			{
				return _bleScanResult;
			}
			[MemberNotNull("_bleScanResult")]
			set
			{
				if (!EqualityComparer<IBleScanResult>.Default.Equals(_bleScanResult, value))
				{
					((ObservableObject)this).OnPropertyChanging(__KnownINotifyPropertyChangingArgs.BleScanResult);
					_bleScanResult = value;
					((ObservableObject)this).OnPropertyChanged(__KnownINotifyPropertyChangedArgs.BleScanResult);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string? Name
		{
			get
			{
				return _name;
			}
			set
			{
				if (!EqualityComparer<string>.Default.Equals(_name, value))
				{
					((ObservableObject)this).OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Name);
					_name = value;
					((ObservableObject)this).OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Name);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public MAC Mac
		{
			get
			{
				return _mac;
			}
			[MemberNotNull("_mac")]
			set
			{
				if (!EqualityComparer<MAC>.Default.Equals(_mac, value))
				{
					((ObservableObject)this).OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Mac);
					_mac = value;
					((ObservableObject)this).OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Mac);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public int Rssi
		{
			get
			{
				return _rssi;
			}
			set
			{
				if (!EqualityComparer<int>.Default.Equals(_rssi, value))
				{
					((ObservableObject)this).OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Rssi);
					_rssi = value;
					((ObservableObject)this).OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Rssi);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool IsSelected
		{
			get
			{
				return _isSelected;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(_isSelected, value))
				{
					((ObservableObject)this).OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsSelected);
					_isSelected = value;
					((ObservableObject)this).OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsSelected);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool IsPinPairable
		{
			get
			{
				return _isPinPairable;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(_isPinPairable, value))
				{
					((ObservableObject)this).OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsPinPairable);
					_isPinPairable = value;
					((ObservableObject)this).OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsPinPairable);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool IsPushToPair
		{
			get
			{
				return _isPushToPair;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(_isPushToPair, value))
				{
					((ObservableObject)this).OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsPushToPair);
					_isPushToPair = value;
					((ObservableObject)this).OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsPushToPair);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool IsPairingActive
		{
			get
			{
				return _isPairingActive;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(_isPairingActive, value))
				{
					((ObservableObject)this).OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsPairingActive);
					_isPairingActive = value;
					((ObservableObject)this).OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsPairingActive);
				}
			}
		}

		public BleScanResultModel(IBleScanResult bleScanResult)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			BleScanResult = bleScanResult;
			Update(bleScanResult);
		}

		public void Update(IBleScanResult bleScanResult)
		{
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Invalid comparison between Unknown and I4
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Invalid comparison between Unknown and I4
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Invalid comparison between Unknown and I4
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Invalid comparison between Unknown and I4
			BleScanResult = bleScanResult;
			Name = bleScanResult.DeviceName.Replace("*", "");
			IdsCanAccessoryScanResult val = (IdsCanAccessoryScanResult)(object)((bleScanResult is IdsCanAccessoryScanResult) ? bleScanResult : null);
			object obj2;
			if (val == null || val.AccessoryMacAddress == null)
			{
				IBleScanResult obj = ((bleScanResult is MopekaScanResult) ? bleScanResult : null);
				obj2 = ((obj != null) ? ((MopekaScanResult)obj).ShortMAC : null);
				if (obj2 == null)
				{
					Guid deviceId = bleScanResult.DeviceId;
					obj2 = (object)new MAC(Enumerable.ToArray<byte>(Enumerable.Take<byte>(Enumerable.Skip<byte>((global::System.Collections.Generic.IEnumerable<byte>)((Guid)(ref deviceId)).ToByteArray(), 10), 6)));
				}
			}
			else
			{
				obj2 = val.AccessoryMacAddress;
			}
			Mac = (MAC)obj2;
			Rssi = bleScanResult.Rssi;
			BleGatewayScanResult val2 = (BleGatewayScanResult)(object)((bleScanResult is BleGatewayScanResult) ? bleScanResult : null);
			if (val2 == null)
			{
				IPairableDeviceScanResult val3 = (IPairableDeviceScanResult)(object)((bleScanResult is IPairableDeviceScanResult) ? bleScanResult : null);
				if (val3 == null)
				{
					IdsCanAccessoryScanResult val4 = (IdsCanAccessoryScanResult)(object)((bleScanResult is IdsCanAccessoryScanResult) ? bleScanResult : null);
					if (val4 == null)
					{
						MopekaScanResult val5 = (MopekaScanResult)(object)((bleScanResult is MopekaScanResult) ? bleScanResult : null);
						if (val5 == null)
						{
							if (bleScanResult is BleTirePressureMonitorScanResult)
							{
								IsPushToPair = true;
								IsPinPairable = false;
								IsPairingActive = bleScanResult.DeviceName.Contains('*');
							}
							else
							{
								IsPushToPair = false;
								IsPinPairable = false;
								IsPairingActive = false;
							}
						}
						else
						{
							IsPushToPair = true;
							IsPinPairable = false;
							IsPairingActive = val5.IsSyncPressed;
						}
					}
					else
					{
						IsPushToPair = true;
						IsPinPairable = false;
						IsPairingActive = val4.IsInLinkMode;
					}
				}
				else
				{
					IsPushToPair = (int)val3.PairingMethod == 3;
					IsPinPairable = (int)val3.PairingMethod == 2;
					IsPairingActive = val3.PairingEnabled;
				}
			}
			else
			{
				IsPushToPair = (int)val2.PairingMethod == 3;
				PairingMethod pairingMethod = val2.PairingMethod;
				bool isPinPairable = pairingMethod - 1 <= 1;
				IsPinPairable = isPinPairable;
				IsPairingActive = val2.PairingEnabled;
			}
		}

		public int CompareTo(BleScanResultModel? other)
		{
			if (this == other)
			{
				return 0;
			}
			if (other == null)
			{
				return 1;
			}
			int num = IsPairingActive.CompareTo(other.IsPairingActive);
			if (num != 0)
			{
				return num;
			}
			int num2 = IsPushToPair.CompareTo(other.IsPushToPair);
			if (num2 == 0)
			{
				return string.Compare(Name, other.Name, (StringComparison)4);
			}
			return num2;
		}
	}
	[ContentProperty("Text")]
	public class PairingPill : Border
	{
		public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(PairingPill), (object)null, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)null, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)null, (CreateDefaultValueDelegate)null);

		public static readonly BindableProperty TextColorProperty = BindableProperty.Create("TextColor", typeof(Color), typeof(PairingPill), (object)null, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)null, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)null, (CreateDefaultValueDelegate)null);

		public static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create("BackgroundColor", typeof(Color), typeof(PairingPill), (object)null, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)null, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)null, (CreateDefaultValueDelegate)null);

		public string Text
		{
			get
			{
				return (string)((BindableObject)this).GetValue(TextProperty);
			}
			set
			{
				((BindableObject)this).SetValue(TextProperty, (object)value);
			}
		}

		public Color TextColor
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected O, but got Unknown
				return (Color)((BindableObject)this).GetValue(TextColorProperty);
			}
			set
			{
				((BindableObject)this).SetValue(TextColorProperty, (object)value);
			}
		}

		public Color BackgroundColor
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected O, but got Unknown
				return (Color)((BindableObject)this).GetValue(BackgroundColorProperty);
			}
			set
			{
				((BindableObject)this).SetValue(BackgroundColorProperty, (object)value);
			}
		}

		public PairingPill()
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Expected O, but got Unknown
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Expected O, but got Unknown
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Expected O, but got Unknown
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Expected O, but got Unknown
			((VisualElement)this).HeightRequest = 36.0;
			((VisualElement)this).MinimumWidthRequest = 72.0;
			((Border)this).StrokeShape = (IShape)new RoundRectangle
			{
				CornerRadius = CornerRadius.op_Implicit(18.0)
			};
			((Element)this).SetDynamicResource(TextColorProperty, "OnTertiary");
			((Element)this).SetDynamicResource(BackgroundColorProperty, "Tertiary");
			FluentUIExtensions.DisableColor<PairingPill>(this, VisualElement.BackgroundColorProperty, (BindingBase)new Binding(BackgroundColorProperty.PropertyName, (BindingMode)0, (IValueConverter)null, (object)null, (string)null, (object)this), (BindingBase)null, (BindingBase)null);
			((Border)this).Content = (View)(object)FluentUIExtensions.DisableColor<Label>(FluentUIExtensions.Binding<Label>(FluentUIExtensions.DynamicResource<Label>(new Label
			{
				HeightRequest = 36.0,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				Padding = new Thickness(8.0, 0.0),
				HorizontalTextAlignment = (TextAlignment)1,
				VerticalTextAlignment = (TextAlignment)1
			}, VisualElement.StyleProperty, "Caption_Label"), Label.TextProperty, (BindingBase)new Binding(TextProperty.PropertyName, (BindingMode)0, (IValueConverter)null, (object)null, (string)null, (object)this)), Label.TextColorProperty, (BindingBase)new Binding(TextColorProperty.PropertyName, (BindingMode)0, (IValueConverter)null, (object)null, (string)null, (object)this), (BindingBase)null, (BindingBase)null);
		}
	}
	[XamlFilePath("SearchForDevices/SearchForDevicesPage.xaml")]
	public class SearchForDevicesPage : DualActionPage
	{
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_0
		{
			internal object?[]? parentValues;

			internal SearchForDevicesPage? root;

			internal NameScope? _scope0;

			internal NameScope? _scope1;

			internal NameScope? _scope2;

			internal NameScope? _scope3;

			internal NameScope? _scope4;

			internal NameScope? _scope5;

			internal NameScope? _scope6;

			internal NameScope? _scope7;

			internal NameScope? _scope8;

			internal NameScope? _scope9;

			internal NameScope? _scope10;

			internal NameScope? _scope11;

			internal NameScope? _scope12;

			internal NameScope? _scope13;

			internal NameScope? _scope14;

			internal NameScope? _scope15;

			internal NameScope? _scope16;

			internal NameScope? _scope17;

			internal NameScope? _scope18;

			internal NameScope? _scope19;

			internal NameScope? _scope20;

			internal NameScope? _scope21;

			internal NameScope? _scope22;

			internal NameScope? _scope23;

			internal NameScope? _scope24;

			internal NameScope? _scope25;

			internal NameScope? _scope26;

			internal NameScope? _scope27;

			internal NameScope? _scope28;

			internal NameScope? _scope29;

			internal NameScope? _scope30;

			internal NameScope? _scope31;

			internal NameScope? _scope32;

			internal NameScope? _scope33;

			internal NameScope? _scope34;

			internal NameScope? _scope35;

			internal NameScope? _scope36;

			internal NameScope? _scope37;

			internal NameScope? _scope38;

			internal NameScope? _scope39;

			internal NameScope? _scope40;

			internal NameScope? _scope41;

			internal NameScope? _scope42;

			internal NameScope? _scope43;

			internal NameScope? _scope44;

			internal object? LoadDataTemplate()
			{
				//IL_0165: Unknown result type (might be due to invalid IL or missing references)
				//IL_016c: Expected O, but got Unknown
				//IL_016c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0173: Expected O, but got Unknown
				//IL_0173: Unknown result type (might be due to invalid IL or missing references)
				//IL_017a: Expected O, but got Unknown
				//IL_017a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0181: Expected O, but got Unknown
				//IL_0181: Unknown result type (might be due to invalid IL or missing references)
				//IL_0188: Expected O, but got Unknown
				//IL_0192: Unknown result type (might be due to invalid IL or missing references)
				//IL_0199: Expected O, but got Unknown
				//IL_0199: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a0: Expected O, but got Unknown
				//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a7: Expected O, but got Unknown
				//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ae: Expected O, but got Unknown
				//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
				//IL_01bf: Expected O, but got Unknown
				//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d2: Expected O, but got Unknown
				//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d9: Expected O, but got Unknown
				//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
				//IL_01e0: Expected O, but got Unknown
				//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
				//IL_01e7: Expected O, but got Unknown
				//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ee: Expected O, but got Unknown
				//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
				//IL_01f5: Expected O, but got Unknown
				//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
				//IL_01fc: Expected O, but got Unknown
				//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
				//IL_0203: Expected O, but got Unknown
				//IL_0203: Unknown result type (might be due to invalid IL or missing references)
				//IL_020a: Expected O, but got Unknown
				//IL_020a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0211: Expected O, but got Unknown
				//IL_021b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0222: Expected O, but got Unknown
				//IL_0222: Unknown result type (might be due to invalid IL or missing references)
				//IL_0229: Expected O, but got Unknown
				//IL_0229: Unknown result type (might be due to invalid IL or missing references)
				//IL_0230: Expected O, but got Unknown
				//IL_0230: Unknown result type (might be due to invalid IL or missing references)
				//IL_0237: Expected O, but got Unknown
				//IL_0241: Unknown result type (might be due to invalid IL or missing references)
				//IL_0248: Expected O, but got Unknown
				//IL_0248: Unknown result type (might be due to invalid IL or missing references)
				//IL_024f: Expected O, but got Unknown
				//IL_0256: Unknown result type (might be due to invalid IL or missing references)
				//IL_025d: Expected O, but got Unknown
				//IL_0264: Unknown result type (might be due to invalid IL or missing references)
				//IL_026b: Expected O, but got Unknown
				//IL_0272: Unknown result type (might be due to invalid IL or missing references)
				//IL_0279: Expected O, but got Unknown
				//IL_0280: Unknown result type (might be due to invalid IL or missing references)
				//IL_0287: Expected O, but got Unknown
				//IL_028e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0295: Expected O, but got Unknown
				//IL_0295: Unknown result type (might be due to invalid IL or missing references)
				//IL_029c: Expected O, but got Unknown
				//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
				//IL_02aa: Expected O, but got Unknown
				//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
				//IL_02b1: Expected O, but got Unknown
				//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
				//IL_02b8: Expected O, but got Unknown
				//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
				//IL_02bf: Expected O, but got Unknown
				//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
				//IL_02c6: Expected O, but got Unknown
				//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
				//IL_02cd: Expected O, but got Unknown
				//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
				//IL_02de: Expected O, but got Unknown
				//IL_02de: Unknown result type (might be due to invalid IL or missing references)
				//IL_02e5: Expected O, but got Unknown
				//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
				//IL_02ec: Expected O, but got Unknown
				//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
				//IL_02f3: Expected O, but got Unknown
				//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
				//IL_0304: Expected O, but got Unknown
				//IL_030b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0312: Expected O, but got Unknown
				//IL_0312: Unknown result type (might be due to invalid IL or missing references)
				//IL_0319: Expected O, but got Unknown
				//IL_0319: Unknown result type (might be due to invalid IL or missing references)
				//IL_0320: Expected O, but got Unknown
				//IL_032e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0335: Expected O, but got Unknown
				//IL_0335: Unknown result type (might be due to invalid IL or missing references)
				//IL_033c: Expected O, but got Unknown
				//IL_033c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0343: Expected O, but got Unknown
				//IL_0343: Unknown result type (might be due to invalid IL or missing references)
				//IL_034a: Expected O, but got Unknown
				//IL_034a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0351: Expected O, but got Unknown
				//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
				//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
				//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
				//IL_0404: Expected O, but got Unknown
				//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
				//IL_0409: Expected O, but got Unknown
				//IL_040e: Expected O, but got Unknown
				//IL_0433: Unknown result type (might be due to invalid IL or missing references)
				//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
				//IL_056f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0574: Unknown result type (might be due to invalid IL or missing references)
				//IL_0583: Unknown result type (might be due to invalid IL or missing references)
				//IL_058d: Expected O, but got Unknown
				//IL_0588: Unknown result type (might be due to invalid IL or missing references)
				//IL_0592: Expected O, but got Unknown
				//IL_0597: Expected O, but got Unknown
				//IL_0627: Unknown result type (might be due to invalid IL or missing references)
				//IL_06e2: Unknown result type (might be due to invalid IL or missing references)
				//IL_06e7: Unknown result type (might be due to invalid IL or missing references)
				//IL_06f6: Unknown result type (might be due to invalid IL or missing references)
				//IL_0700: Expected O, but got Unknown
				//IL_06fb: Unknown result type (might be due to invalid IL or missing references)
				//IL_0705: Expected O, but got Unknown
				//IL_070a: Expected O, but got Unknown
				//IL_074e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0753: Unknown result type (might be due to invalid IL or missing references)
				//IL_0759: Expected O, but got Unknown
				//IL_0764: Unknown result type (might be due to invalid IL or missing references)
				//IL_0769: Unknown result type (might be due to invalid IL or missing references)
				//IL_076f: Expected O, but got Unknown
				//IL_076f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0779: Expected O, but got Unknown
				//IL_0788: Unknown result type (might be due to invalid IL or missing references)
				//IL_078d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0793: Expected O, but got Unknown
				//IL_079e: Unknown result type (might be due to invalid IL or missing references)
				//IL_07a3: Unknown result type (might be due to invalid IL or missing references)
				//IL_07a9: Expected O, but got Unknown
				//IL_07b4: Unknown result type (might be due to invalid IL or missing references)
				//IL_07b9: Unknown result type (might be due to invalid IL or missing references)
				//IL_07bf: Expected O, but got Unknown
				//IL_07bf: Unknown result type (might be due to invalid IL or missing references)
				//IL_07c9: Expected O, but got Unknown
				//IL_080d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0846: Unknown result type (might be due to invalid IL or missing references)
				//IL_084b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0856: Unknown result type (might be due to invalid IL or missing references)
				//IL_085b: Unknown result type (might be due to invalid IL or missing references)
				//IL_086b: Unknown result type (might be due to invalid IL or missing references)
				//IL_087b: Unknown result type (might be due to invalid IL or missing references)
				//IL_088b: Unknown result type (might be due to invalid IL or missing references)
				//IL_089b: Unknown result type (might be due to invalid IL or missing references)
				//IL_08ab: Unknown result type (might be due to invalid IL or missing references)
				//IL_08bb: Unknown result type (might be due to invalid IL or missing references)
				//IL_08cb: Unknown result type (might be due to invalid IL or missing references)
				//IL_08db: Unknown result type (might be due to invalid IL or missing references)
				//IL_08eb: Unknown result type (might be due to invalid IL or missing references)
				//IL_090f: Expected O, but got Unknown
				//IL_090a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0914: Expected O, but got Unknown
				//IL_0919: Expected O, but got Unknown
				//IL_0958: Unknown result type (might be due to invalid IL or missing references)
				//IL_0a2c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0a31: Unknown result type (might be due to invalid IL or missing references)
				//IL_0a40: Unknown result type (might be due to invalid IL or missing references)
				//IL_0a4a: Expected O, but got Unknown
				//IL_0a45: Unknown result type (might be due to invalid IL or missing references)
				//IL_0a4f: Expected O, but got Unknown
				//IL_0a54: Expected O, but got Unknown
				//IL_0a8a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0add: Unknown result type (might be due to invalid IL or missing references)
				//IL_0ae2: Unknown result type (might be due to invalid IL or missing references)
				//IL_0af1: Unknown result type (might be due to invalid IL or missing references)
				//IL_0afb: Expected O, but got Unknown
				//IL_0af6: Unknown result type (might be due to invalid IL or missing references)
				//IL_0b00: Expected O, but got Unknown
				//IL_0b05: Expected O, but got Unknown
				//IL_0b6f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0c79: Unknown result type (might be due to invalid IL or missing references)
				//IL_0d34: Unknown result type (might be due to invalid IL or missing references)
				//IL_0d39: Unknown result type (might be due to invalid IL or missing references)
				//IL_0d48: Unknown result type (might be due to invalid IL or missing references)
				//IL_0d52: Expected O, but got Unknown
				//IL_0d4d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0d57: Expected O, but got Unknown
				//IL_0d5c: Expected O, but got Unknown
				//IL_0dec: Unknown result type (might be due to invalid IL or missing references)
				//IL_0ea7: Unknown result type (might be due to invalid IL or missing references)
				//IL_0eac: Unknown result type (might be due to invalid IL or missing references)
				//IL_0ebb: Unknown result type (might be due to invalid IL or missing references)
				//IL_0ec5: Expected O, but got Unknown
				//IL_0ec0: Unknown result type (might be due to invalid IL or missing references)
				//IL_0eca: Expected O, but got Unknown
				//IL_0ecf: Expected O, but got Unknown
				//IL_0f34: Unknown result type (might be due to invalid IL or missing references)
				//IL_0f4a: Unknown result type (might be due to invalid IL or missing references)
				//IL_1007: Unknown result type (might be due to invalid IL or missing references)
				//IL_1174: Unknown result type (might be due to invalid IL or missing references)
				//IL_12e1: Unknown result type (might be due to invalid IL or missing references)
				//IL_13df: Unknown result type (might be due to invalid IL or missing references)
				//IL_14de: Unknown result type (might be due to invalid IL or missing references)
				//IL_14f4: Unknown result type (might be due to invalid IL or missing references)
				//IL_158f: Unknown result type (might be due to invalid IL or missing references)
				//IL_1643: Unknown result type (might be due to invalid IL or missing references)
				//IL_1648: Unknown result type (might be due to invalid IL or missing references)
				//IL_1657: Unknown result type (might be due to invalid IL or missing references)
				//IL_1661: Expected O, but got Unknown
				//IL_165c: Unknown result type (might be due to invalid IL or missing references)
				//IL_1666: Expected O, but got Unknown
				//IL_166b: Expected O, but got Unknown
				//IL_16e4: Unknown result type (might be due to invalid IL or missing references)
				//IL_179f: Unknown result type (might be due to invalid IL or missing references)
				//IL_17a4: Unknown result type (might be due to invalid IL or missing references)
				//IL_17b6: Unknown result type (might be due to invalid IL or missing references)
				//IL_17c0: Expected O, but got Unknown
				//IL_17bb: Unknown result type (might be due to invalid IL or missing references)
				//IL_17c5: Expected O, but got Unknown
				//IL_17ca: Expected O, but got Unknown
				//IL_185a: Unknown result type (might be due to invalid IL or missing references)
				//IL_1915: Unknown result type (might be due to invalid IL or missing references)
				//IL_191a: Unknown result type (might be due to invalid IL or missing references)
				//IL_192c: Unknown result type (might be due to invalid IL or missing references)
				//IL_1936: Expected O, but got Unknown
				//IL_1931: Unknown result type (might be due to invalid IL or missing references)
				//IL_193b: Expected O, but got Unknown
				//IL_1940: Expected O, but got Unknown
				//IL_19eb: Unknown result type (might be due to invalid IL or missing references)
				//IL_1a01: Unknown result type (might be due to invalid IL or missing references)
				//IL_1a1e: Unknown result type (might be due to invalid IL or missing references)
				//IL_1a23: Unknown result type (might be due to invalid IL or missing references)
				//IL_1a35: Unknown result type (might be due to invalid IL or missing references)
				//IL_1a3f: Expected O, but got Unknown
				//IL_1a3a: Unknown result type (might be due to invalid IL or missing references)
				//IL_1a44: Expected O, but got Unknown
				//IL_1a49: Expected O, but got Unknown
				//IL_1a60: Unknown result type (might be due to invalid IL or missing references)
				//IL_1a65: Unknown result type (might be due to invalid IL or missing references)
				//IL_1ad9: Unknown result type (might be due to invalid IL or missing references)
				//IL_1ade: Unknown result type (might be due to invalid IL or missing references)
				//IL_1ae1: Expected O, but got Unknown
				//IL_1ae6: Expected O, but got Unknown
				//IL_1ae6: Unknown result type (might be due to invalid IL or missing references)
				//IL_1af8: Unknown result type (might be due to invalid IL or missing references)
				//IL_1b0a: Unknown result type (might be due to invalid IL or missing references)
				//IL_1b15: Unknown result type (might be due to invalid IL or missing references)
				//IL_1b1a: Unknown result type (might be due to invalid IL or missing references)
				//IL_1b2a: Unknown result type (might be due to invalid IL or missing references)
				//IL_1b3a: Unknown result type (might be due to invalid IL or missing references)
				//IL_1b4a: Unknown result type (might be due to invalid IL or missing references)
				//IL_1b5a: Unknown result type (might be due to invalid IL or missing references)
				//IL_1b6a: Unknown result type (might be due to invalid IL or missing references)
				//IL_1b7a: Unknown result type (might be due to invalid IL or missing references)
				//IL_1b8a: Unknown result type (might be due to invalid IL or missing references)
				//IL_1b9a: Unknown result type (might be due to invalid IL or missing references)
				//IL_1baa: Unknown result type (might be due to invalid IL or missing references)
				//IL_1bce: Expected O, but got Unknown
				//IL_1bc9: Unknown result type (might be due to invalid IL or missing references)
				//IL_1bd3: Expected O, but got Unknown
				//IL_1bd3: Unknown result type (might be due to invalid IL or missing references)
				//IL_1be5: Unknown result type (might be due to invalid IL or missing references)
				//IL_1bef: Expected O, but got Unknown
				//IL_1bea: Unknown result type (might be due to invalid IL or missing references)
				//IL_1bf4: Expected O, but got Unknown
				//IL_1bf9: Expected O, but got Unknown
				//IL_1c61: Unknown result type (might be due to invalid IL or missing references)
				NameScope val = _scope0;
				NameScope val2 = _scope1;
				NameScope val3 = _scope2;
				NameScope val4 = _scope3;
				NameScope val5 = _scope4;
				NameScope val6 = _scope5;
				NameScope val7 = _scope6;
				NameScope val8 = _scope7;
				NameScope val9 = _scope8;
				NameScope val10 = _scope9;
				NameScope val11 = _scope10;
				NameScope val12 = _scope11;
				NameScope val13 = _scope12;
				NameScope val14 = _scope13;
				NameScope val15 = _scope14;
				NameScope val16 = _scope15;
				NameScope val17 = _scope16;
				NameScope val18 = _scope17;
				NameScope val19 = _scope18;
				NameScope val20 = _scope19;
				NameScope val21 = _scope20;
				NameScope val22 = _scope21;
				NameScope val23 = _scope22;
				NameScope val24 = _scope23;
				NameScope val25 = _scope24;
				NameScope val26 = _scope25;
				NameScope val27 = _scope26;
				NameScope val28 = _scope27;
				NameScope val29 = _scope28;
				NameScope val30 = _scope29;
				NameScope val31 = _scope30;
				NameScope val32 = _scope31;
				NameScope val33 = _scope32;
				NameScope val34 = _scope33;
				NameScope val35 = _scope34;
				NameScope val36 = _scope35;
				NameScope val37 = _scope36;
				NameScope val38 = _scope37;
				NameScope val39 = _scope38;
				NameScope val40 = _scope39;
				NameScope val41 = _scope40;
				NameScope val42 = _scope41;
				NameScope val43 = _scope42;
				NameScope val44 = _scope43;
				NameScope val45 = _scope44;
				DynamicResourceExtension val46 = new DynamicResourceExtension();
				RoundRectangle val47 = new RoundRectangle();
				BindingExtension val48 = new BindingExtension();
				DynamicResourceExtension val49 = new DynamicResourceExtension();
				Setter val50 = new Setter();
				DataTrigger val51 = new DataTrigger(typeof(Border));
				BindingExtension val52 = new BindingExtension();
				DynamicResourceExtension val53 = new DynamicResourceExtension();
				Setter val54 = new Setter();
				DataTrigger val55 = new DataTrigger(typeof(Border));
				global::System.Type typeFromHandle = typeof(SearchForDevicesViewModel);
				RelativeSourceExtension val56 = new RelativeSourceExtension();
				BindingExtension val57 = new BindingExtension();
				BindingExtension val58 = new BindingExtension();
				TapGestureRecognizer val59 = new TapGestureRecognizer();
				DynamicResourceExtension val60 = new DynamicResourceExtension();
				DynamicResourceExtension val61 = new DynamicResourceExtension();
				BindingExtension val62 = new BindingExtension();
				BindingExtension val63 = new BindingExtension();
				DynamicResourceExtension val64 = new DynamicResourceExtension();
				Setter val65 = new Setter();
				DataTrigger val66 = new DataTrigger(typeof(Label));
				BindingExtension val67 = new BindingExtension();
				DynamicResourceExtension val68 = new DynamicResourceExtension();
				Setter val69 = new Setter();
				DataTrigger val70 = new DataTrigger(typeof(Label));
				Label val71 = new Label();
				string searchForDevices_Pin = Strings.SearchForDevices_Pin;
				BindingExtension val72 = new BindingExtension();
				PairingPill pairingPill = new PairingPill();
				BoxView val73 = new BoxView();
				string searchForDevices_PushToPair = Strings.SearchForDevices_PushToPair;
				BindingExtension val74 = new BindingExtension();
				PairingPill pairingPill2 = new PairingPill();
				BoxView val75 = new BoxView();
				string searchForDevices_ReadyToPair = Strings.SearchForDevices_ReadyToPair;
				BindingExtension val76 = new BindingExtension();
				BindingExtension val77 = new BindingExtension();
				PairingPill pairingPill3 = new PairingPill();
				FlexLayout val78 = new FlexLayout();
				BindingExtension val79 = new BindingExtension();
				DynamicResourceExtension val80 = new DynamicResourceExtension();
				BindingExtension val81 = new BindingExtension();
				DynamicResourceExtension val82 = new DynamicResourceExtension();
				Setter val83 = new Setter();
				DataTrigger val84 = new DataTrigger(typeof(App.Common.Pages.Pairing.SearchForDevices.SignalView.SignalView));
				BindingExtension val85 = new BindingExtension();
				DynamicResourceExtension val86 = new DynamicResourceExtension();
				Setter val87 = new Setter();
				DataTrigger val88 = new DataTrigger(typeof(App.Common.Pages.Pairing.SearchForDevices.SignalView.SignalView));
				App.Common.Pages.Pairing.SearchForDevices.SignalView.SignalView signalView = new App.Common.Pages.Pairing.SearchForDevices.SignalView.SignalView();
				DynamicResourceExtension val89 = new DynamicResourceExtension();
				DisabledColorExtension val90 = new DisabledColorExtension();
				BindingExtension val91 = new BindingExtension();
				string proSolid = FontAwesomeFontFamily.ProSolid900;
				string circleCheck = FontAwesomeGlyph.CircleCheck;
				FontImageSource val92 = new FontImageSource();
				SKImageView val93 = new SKImageView();
				Grid val94 = new Grid();
				Border val95 = new Border();
				NameScope val96 = new NameScope();
				NameScope.SetNameScope((BindableObject)(object)val95, (INameScope)(object)val96);
				((Element)val47).transientNamescope = (INameScope)(object)val96;
				((Element)val94).transientNamescope = (INameScope)(object)val96;
				((Element)val59).transientNamescope = (INameScope)(object)val96;
				((Element)val71).transientNamescope = (INameScope)(object)val96;
				((Element)val78).transientNamescope = (INameScope)(object)val96;
				((Element)pairingPill).transientNamescope = (INameScope)(object)val96;
				((Element)val73).transientNamescope = (INameScope)(object)val96;
				((Element)pairingPill2).transientNamescope = (INameScope)(object)val96;
				((Element)val75).transientNamescope = (INameScope)(object)val96;
				((Element)pairingPill3).transientNamescope = (INameScope)(object)val96;
				((Element)signalView).transientNamescope = (INameScope)(object)val96;
				((Element)val93).transientNamescope = (INameScope)(object)val96;
				((Element)val90).transientNamescope = (INameScope)(object)val96;
				((Element)val92).transientNamescope = (INameScope)(object)val96;
				val46.Key = "Surface";
				XamlServiceProvider val97 = new XamlServiceProvider();
				val97.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(51, 29)));
				DynamicResource val98 = ((IMarkupExtension<DynamicResource>)(object)val46).ProvideValue((IServiceProvider)val97);
				((IDynamicResourceHandler)val95).SetDynamicResource(VisualElement.BackgroundColorProperty, val98.Key);
				((BindableObject)val47).SetValue(RoundRectangle.CornerRadiusProperty, (object)new CornerRadius(24.0));
				((BindableObject)val95).SetValue(Border.StrokeShapeProperty, (object)val47);
				val51.Value = "True";
				val48.Path = "IsSelected";
				val48.TypedBinding = (TypedBindingBase)(object)new TypedBinding<BleScanResultModel, bool>((Func<BleScanResultModel, ValueTuple<bool, bool>>)([CompilerGenerated] (BleScanResultModel? P_0) => (P_0 != null) ? new ValueTuple<bool, bool>(P_0.IsSelected, true) : default(ValueTuple<bool, bool>)), (Action<BleScanResultModel, bool>)([CompilerGenerated] (BleScanResultModel? P_0, bool P_1) =>
				{
					if (P_0 != null)
					{
						P_0.IsSelected = P_1;
					}
				}), new Tuple<Func<BleScanResultModel, object>, string>[1]
				{
					new Tuple<Func<BleScanResultModel, object>, string>((Func<BleScanResultModel, object>)([CompilerGenerated] (BleScanResultModel? P_0) => P_0), "IsSelected")
				});
				((BindingBase)val48.TypedBinding).Mode = val48.Mode;
				val48.TypedBinding.Converter = val48.Converter;
				val48.TypedBinding.ConverterParameter = val48.ConverterParameter;
				((BindingBase)val48.TypedBinding).StringFormat = val48.StringFormat;
				val48.TypedBinding.Source = val48.Source;
				val48.TypedBinding.UpdateSourceEventName = val48.UpdateSourceEventName;
				((BindingBase)val48.TypedBinding).FallbackValue = val48.FallbackValue;
				((BindingBase)val48.TypedBinding).TargetNullValue = val48.TargetNullValue;
				BindingBase typedBinding = (BindingBase)(object)val48.TypedBinding;
				val51.Binding = typedBinding;
				val50.Property = VisualElement.BackgroundColorProperty;
				val49.Key = "TertiaryContainer";
				XamlServiceProvider val99 = new XamlServiceProvider();
				val99.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(59, 68)));
				DynamicResource value = ((IMarkupExtension<DynamicResource>)(object)val49).ProvideValue((IServiceProvider)val99);
				val50.Value = value;
				((global::System.Collections.Generic.ICollection<Setter>)val51.Setters).Add(val50);
				((global::System.Collections.Generic.ICollection<TriggerBase>)((BindableObject)val95).GetValue(VisualElement.TriggersProperty)).Add((TriggerBase)(object)val51);
				val55.Value = "False";
				val52.Path = "IsSelected";
				val52.TypedBinding = (TypedBindingBase)(object)new TypedBinding<BleScanResultModel, bool>((Func<BleScanResultModel, ValueTuple<bool, bool>>)([CompilerGenerated] (BleScanResultModel? P_0) => (P_0 != null) ? new ValueTuple<bool, bool>(P_0.IsSelected, true) : default(ValueTuple<bool, bool>)), (Action<BleScanResultModel, bool>)([CompilerGenerated] (BleScanResultModel? P_0, bool P_1) =>
				{
					if (P_0 != null)
					{
						P_0.IsSelected = P_1;
					}
				}), new Tuple<Func<BleScanResultModel, object>, string>[1]
				{
					new Tuple<Func<BleScanResultModel, object>, string>((Func<BleScanResultModel, object>)([CompilerGenerated] (BleScanResultModel? P_0) => P_0), "IsSelected")
				});
				((BindingBase)val52.TypedBinding).Mode = val52.Mode;
				val52.TypedBinding.Converter = val52.Converter;
				val52.TypedBinding.ConverterParameter = val52.ConverterParameter;
				((BindingBase)val52.TypedBinding).StringFormat = val52.StringFormat;
				val52.TypedBinding.Source = val52.Source;
				val52.TypedBinding.UpdateSourceEventName = val52.UpdateSourceEventName;
				((BindingBase)val52.TypedBinding).FallbackValue = val52.FallbackValue;
				((BindingBase)val52.TypedBinding).TargetNullValue = val52.TargetNullValue;
				BindingBase typedBinding2 = (BindingBase)(object)val52.TypedBinding;
				val55.Binding = typedBinding2;
				val54.Property = VisualElement.BackgroundColorProperty;
				val53.Key = "Surface";
				XamlServiceProvider val100 = new XamlServiceProvider();
				val100.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(64, 68)));
				DynamicResource value2 = ((IMarkupExtension<DynamicResource>)(object)val53).ProvideValue((IServiceProvider)val100);
				val54.Value = value2;
				((global::System.Collections.Generic.ICollection<Setter>)val55.Setters).Add(val54);
				((global::System.Collections.Generic.ICollection<TriggerBase>)((BindableObject)val95).GetValue(VisualElement.TriggersProperty)).Add((TriggerBase)(object)val55);
				((BindableObject)val94).SetValue(Grid.RowDefinitionsProperty, (object)new RowDefinitionCollection((RowDefinition[])(object)new RowDefinition[2]
				{
					new RowDefinition(new GridLength(56.0)),
					new RowDefinition(new GridLength(36.0))
				}));
				((BindableObject)val94).SetValue(Grid.ColumnDefinitionsProperty, (object)new ColumnDefinitionCollection((ColumnDefinition[])(object)new ColumnDefinition[3]
				{
					new ColumnDefinition(GridLength.Star),
					new ColumnDefinition(new GridLength(48.0)),
					new ColumnDefinition(new GridLength(48.0))
				}));
				((BindableObject)val94).SetValue(Grid.RowSpacingProperty, (object)4.0);
				((BindableObject)val94).SetValue(Grid.ColumnSpacingProperty, (object)4.0);
				((BindableObject)val94).SetValue(Layout.PaddingProperty, (object)new Thickness(8.0));
				val57.Path = "SearchResultSelectedCommand";
				val56.AncestorType = typeFromHandle;
				RelativeBindingSource source = ((IMarkupExtension<RelativeBindingSource>)(object)val56).ProvideValue((IServiceProvider)null);
				val57.Source = source;
				XamlServiceProvider val101 = new XamlServiceProvider();
				global::System.Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver val102 = new XmlNamespaceResolver();
				val102.Add("", "http://schemas.microsoft.com/dotnet/2021/maui");
				val102.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				val102.Add("ui", "http://lci1.com/schemas/ui");
				val102.Add("skia", "clr-namespace:SkiaSharp.Extended.UI.Controls;assembly=SkiaSharp.Extended.UI");
				val102.Add("disabledColor", "clr-namespace:IDS.UI.MarkupExtensions.DisabledColor;assembly=ids.ui");
				val102.Add("toolkit", "http://schemas.microsoft.com/dotnet/2022/maui/toolkit");
				val102.Add("dualActionPage", "clr-namespace:App.Common.DualActionPage;assembly=App.Common");
				val102.Add("searchForDevices", "clr-namespace:App.Common.Pages.Pairing.SearchForDevices");
				val102.Add("signalView", "clr-namespace:App.Common.Pages.Pairing.SearchForDevices.SignalView");
				val102.Add("resources", "clr-namespace:App.Common.Pages.Pairing.Resources");
				val101.Add(typeFromHandle2, (object)new XamlTypeResolver((IXmlNamespaceResolver)val102, typeof(<InitializeComponent>_anonXamlCDataTemplate_0).Assembly));
				BindingBase val103 = ((IMarkupExtension<BindingBase>)(object)val57).ProvideValue((IServiceProvider)val101);
				((BindableObject)val59).SetBinding(TapGestureRecognizer.CommandProperty, val103);
				val58.Path = ".";
				val58.TypedBinding = (TypedBindingBase)(object)new TypedBinding<BleScanResultModel, BleScanResultModel>((Func<BleScanResultModel, ValueTuple<BleScanResultModel, bool>>)([CompilerGenerated] (BleScanResultModel? P_0) => new ValueTuple<BleScanResultModel, bool>(P_0, true)), (Action<BleScanResultModel, BleScanResultModel>)null, (Tuple<Func<BleScanResultModel, object>, string>[])null);
				((BindingBase)val58.TypedBinding).Mode = val58.Mode;
				val58.TypedBinding.Converter = val58.Converter;
				val58.TypedBinding.ConverterParameter = val58.ConverterParameter;
				((BindingBase)val58.TypedBinding).StringFormat = val58.StringFormat;
				val58.TypedBinding.Source = val58.Source;
				val58.TypedBinding.UpdateSourceEventName = val58.UpdateSourceEventName;
				((BindingBase)val58.TypedBinding).FallbackValue = val58.FallbackValue;
				((BindingBase)val58.TypedBinding).TargetNullValue = val58.TargetNullValue;
				BindingBase typedBinding3 = (BindingBase)(object)val58.TypedBinding;
				((BindableObject)val59).SetBinding(TapGestureRecognizer.CommandParameterProperty, typedBinding3);
				((global::System.Collections.Generic.ICollection<IGestureRecognizer>)((View)val94).GestureRecognizers).Add((IGestureRecognizer)(object)val59);
				((BindableObject)val71).SetValue(Grid.RowProperty, (object)0);
				val60.Key = "Body4_Label_Black";
				XamlServiceProvider val104 = new XamlServiceProvider();
				val104.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(80, 36)));
				DynamicResource val105 = ((IMarkupExtension<DynamicResource>)(object)val60).ProvideValue((IServiceProvider)val104);
				((IDynamicResourceHandler)val71).SetDynamicResource(VisualElement.StyleProperty, val105.Key);
				((BindableObject)val71).SetValue(VisualElement.HeightRequestProperty, (object)56.0);
				((BindableObject)val71).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Center);
				((BindableObject)val71).SetValue(Label.LineBreakModeProperty, (object)(LineBreakMode)2);
				((BindableObject)val71).SetValue(Label.MaxLinesProperty, (object)2);
				((BindableObject)val71).SetValue(Label.VerticalTextAlignmentProperty, (object)(TextAlignment)1);
				val61.Key = "OnSurface";
				XamlServiceProvider val106 = new XamlServiceProvider();
				val106.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(86, 36)));
				DynamicResource val107 = ((IMarkupExtension<DynamicResource>)(object)val61).ProvideValue((IServiceProvider)val106);
				((IDynamicResourceHandler)val71).SetDynamicResource(Label.TextColorProperty, val107.Key);
				val62.Mode = (BindingMode)2;
				val62.Path = "Name";
				val62.TypedBinding = (TypedBindingBase)(object)new TypedBinding<BleScanResultModel, string>((Func<BleScanResultModel, ValueTuple<string, bool>>)([CompilerGenerated] (BleScanResultModel? P_0) => (P_0 != null) ? new ValueTuple<string, bool>(P_0.Name, true) : default(ValueTuple<string, bool>)), (Action<BleScanResultModel, string>)null, new Tuple<Func<BleScanResultModel, object>, string>[1]
				{
					new Tuple<Func<BleScanResultModel, object>, string>((Func<BleScanResultModel, object>)([CompilerGenerated] (BleScanResultModel? P_0) => P_0), "Name")
				});
				((BindingBase)val62.TypedBinding).Mode = val62.Mode;
				val62.TypedBinding.Converter = val62.Converter;
				val62.TypedBinding.ConverterParameter = val62.ConverterParameter;
				((BindingBase)val62.TypedBinding).StringFormat = val62.StringFormat;
				val62.TypedBinding.Source = val62.Source;
				val62.TypedBinding.UpdateSourceEventName = val62.UpdateSourceEventName;
				((BindingBase)val62.TypedBinding).FallbackValue = val62.FallbackValue;
				((BindingBase)val62.TypedBinding).TargetNullValue = val62.TargetNullValue;
				BindingBase typedBinding4 = (BindingBase)(object)val62.TypedBinding;
				((BindableObject)val71).SetBinding(Label.TextProperty, typedBinding4);
				val66.Value = "True";
				val63.Path = "IsSelected";
				val63.TypedBinding = (TypedBindingBase)(object)new TypedBinding<BleScanResultModel, bool>((Func<BleScanResultModel, ValueTuple<bool, bool>>)([CompilerGenerated] (BleScanResultModel? P_0) => (P_0 != null) ? new ValueTuple<bool, bool>(P_0.IsSelected, true) : default(ValueTuple<bool, bool>)), (Action<BleScanResultModel, bool>)([CompilerGenerated] (BleScanResultModel? P_0, bool P_1) =>
				{
					if (P_0 != null)
					{
						P_0.IsSelected = P_1;
					}
				}), new Tuple<Func<BleScanResultModel, object>, string>[1]
				{
					new Tuple<Func<BleScanResultModel, object>, string>((Func<BleScanResultModel, object>)([CompilerGenerated] (BleScanResultModel? P_0) => P_0), "IsSelected")
				});
				((BindingBase)val63.TypedBinding).Mode = val63.Mode;
				val63.TypedBinding.Converter = val63.Converter;
				val63.TypedBinding.ConverterParameter = val63.ConverterParameter;
				((BindingBase)val63.TypedBinding).StringFormat = val63.StringFormat;
				val63.TypedBinding.Source = val63.Source;
				val63.TypedBinding.UpdateSourceEventName = val63.UpdateSourceEventName;
				((BindingBase)val63.TypedBinding).FallbackValue = val63.FallbackValue;
				((BindingBase)val63.TypedBinding).TargetNullValue = val63.TargetNullValue;
				BindingBase typedBinding5 = (BindingBase)(object)val63.TypedBinding;
				val66.Binding = typedBinding5;
				val65.Property = Label.TextColorProperty;
				val64.Key = "OnTertiaryContainer";
				XamlServiceProvider val108 = new XamlServiceProvider();
				val108.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(92, 70)));
				DynamicResource value3 = ((IMarkupExtension<DynamicResource>)(object)val64).ProvideValue((IServiceProvider)val108);
				val65.Value = value3;
				((global::System.Collections.Generic.ICollection<Setter>)val66.Setters).Add(val65);
				((global::System.Collections.Generic.ICollection<TriggerBase>)((BindableObject)val71).GetValue(VisualElement.TriggersProperty)).Add((TriggerBase)(object)val66);
				val70.Value = "False";
				val67.Path = "IsSelected";
				val67.TypedBinding = (TypedBindingBase)(object)new TypedBinding<BleScanResultModel, bool>((Func<BleScanResultModel, ValueTuple<bool, bool>>)([CompilerGenerated] (BleScanResultModel? P_0) => (P_0 != null) ? new ValueTuple<bool, bool>(P_0.IsSelected, true) : default(ValueTuple<bool, bool>)), (Action<BleScanResultModel, bool>)([CompilerGenerated] (BleScanResultModel? P_0, bool P_1) =>
				{
					if (P_0 != null)
					{
						P_0.IsSelected = P_1;
					}
				}), new Tuple<Func<BleScanResultModel, object>, string>[1]
				{
					new Tuple<Func<BleScanResultModel, object>, string>((Func<BleScanResultModel, object>)([CompilerGenerated] (BleScanResultModel? P_0) => P_0), "IsSelected")
				});
				((BindingBase)val67.TypedBinding).Mode = val67.Mode;
				val67.TypedBinding.Converter = val67.Converter;
				val67.TypedBinding.ConverterParameter = val67.ConverterParameter;
				((BindingBase)val67.TypedBinding).StringFormat = val67.StringFormat;
				val67.TypedBinding.Source = val67.Source;
				val67.TypedBinding.UpdateSourceEventName = val67.UpdateSourceEventName;
				((BindingBase)val67.TypedBinding).FallbackValue = val67.FallbackValue;
				((BindingBase)val67.TypedBinding).TargetNullValue = val67.TargetNullValue;
				BindingBase typedBinding6 = (BindingBase)(object)val67.TypedBinding;
				val70.Binding = typedBinding6;
				val69.Property = Label.TextColorProperty;
				val68.Key = "OnSurface";
				XamlServiceProvider val109 = new XamlServiceProvider();
				val109.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(97, 70)));
				DynamicResource value4 = ((IMarkupExtension<DynamicResource>)(object)val68).ProvideValue((IServiceProvider)val109);
				val69.Value = value4;
				((global::System.Collections.Generic.ICollection<Setter>)val70.Setters).Add(val69);
				((global::System.Collections.Generic.ICollection<TriggerBase>)((BindableObject)val71).GetValue(VisualElement.TriggersProperty)).Add((TriggerBase)(object)val70);
				((global::System.Collections.Generic.ICollection<IView>)((Layout)val94).Children).Add((IView)(object)val71);
				((BindableObject)val78).SetValue(Grid.RowProperty, (object)1);
				((BindableObject)val78).SetValue(Grid.ColumnProperty, (object)0);
				((BindableObject)val78).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Start);
				((BindableObject)val78).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.End);
				((BindableObject)val78).SetValue(FlexLayout.JustifyContentProperty, (object)(FlexJustify)3);
				((BindableObject)val78).SetValue(FlexLayout.AlignItemsProperty, (object)(FlexAlignItems)2);
				((BindableObject)val78).SetValue(FlexLayout.AlignContentProperty, (object)(FlexAlignContent)5);
				((BindableObject)val78).SetValue(FlexLayout.WrapProperty, (object)(FlexWrap)0);
				((BindableObject)pairingPill).SetValue(PairingPill.TextProperty, (object)searchForDevices_Pin);
				val72.Path = "IsPinPairable";
				val72.TypedBinding = (TypedBindingBase)(object)new TypedBinding<BleScanResultModel, bool>((Func<BleScanResultModel, ValueTuple<bool, bool>>)([CompilerGenerated] (BleScanResultModel? P_0) => (P_0 != null) ? new ValueTuple<bool, bool>(P_0.IsPinPairable, true) : default(ValueTuple<bool, bool>)), (Action<BleScanResultModel, bool>)([CompilerGenerated] (BleScanResultModel? P_0, bool P_1) =>
				{
					if (P_0 != null)
					{
						P_0.IsPinPairable = P_1;
					}
				}), new Tuple<Func<BleScanResultModel, object>, string>[1]
				{
					new Tuple<Func<BleScanResultModel, object>, string>((Func<BleScanResultModel, object>)([CompilerGenerated] (BleScanResultModel? P_0) => P_0), "IsPinPairable")
				});
				((BindingBase)val72.TypedBinding).Mode = val72.Mode;
				val72.TypedBinding.Converter = val72.Converter;
				val72.TypedBinding.ConverterParameter = val72.ConverterParameter;
				((BindingBase)val72.TypedBinding).StringFormat = val72.StringFormat;
				val72.TypedBinding.Source = val72.Source;
				val72.TypedBinding.UpdateSourceEventName = val72.UpdateSourceEventName;
				((BindingBase)val72.TypedBinding).FallbackValue = val72.FallbackValue;
				((BindingBase)val72.TypedBinding).TargetNullValue = val72.TargetNullValue;
				BindingBase typedBinding7 = (BindingBase)(object)val72.TypedBinding;
				((BindableObject)pairingPill).SetBinding(VisualElement.IsVisibleProperty, typedBinding7);
				((global::System.Collections.Generic.ICollection<IView>)((Layout)val78).Children).Add((IView)(object)pairingPill);
				((BindableObject)val73).SetValue(VisualElement.WidthRequestProperty, (object)2.0);
				((BindableObject)val73).SetValue(VisualElement.HeightRequestProperty, (object)2.0);
				((BindableObject)val73).SetValue(VisualElement.BackgroundColorProperty, (object)Colors.Transparent);
				((global::System.Collections.Generic.ICollection<IView>)((Layout)val78).Children).Add((IView)(object)val73);
				((BindableObject)pairingPill2).SetValue(PairingPill.TextProperty, (object)searchForDevices_PushToPair);
				val74.Path = "IsPushToPair";
				val74.TypedBinding = (TypedBindingBase)(object)new TypedBinding<BleScanResultModel, bool>((Func<BleScanResultModel, ValueTuple<bool, bool>>)([CompilerGenerated] (BleScanResultModel? P_0) => (P_0 != null) ? new ValueTuple<bool, bool>(P_0.IsPushToPair, true) : default(ValueTuple<bool, bool>)), (Action<BleScanResultModel, bool>)([CompilerGenerated] (BleScanResultModel? P_0, bool P_1) =>
				{
					if (P_0 != null)
					{
						P_0.IsPushToPair = P_1;
					}
				}), new Tuple<Func<BleScanResultModel, object>, string>[1]
				{
					new Tuple<Func<BleScanResultModel, object>, string>((Func<BleScanResultModel, object>)([CompilerGenerated] (BleScanResultModel? P_0) => P_0), "IsPushToPair")
				});
				((BindingBase)val74.TypedBinding).Mode = val74.Mode;
				val74.TypedBinding.Converter = val74.Converter;
				val74.TypedBinding.ConverterParameter = val74.ConverterParameter;
				((BindingBase)val74.TypedBinding).StringFormat = val74.StringFormat;
				val74.TypedBinding.Source = val74.Source;
				val74.TypedBinding.UpdateSourceEventName = val74.UpdateSourceEventName;
				((BindingBase)val74.TypedBinding).FallbackValue = val74.FallbackValue;
				((BindingBase)val74.TypedBinding).TargetNullValue = val74.TargetNullValue;
				BindingBase typedBinding8 = (BindingBase)(object)val74.TypedBinding;
				((BindableObject)pairingPill2).SetBinding(VisualElement.IsVisibleProperty, typedBinding8);
				((global::System.Collections.Generic.ICollection<IView>)((Layout)val78).Children).Add((IView)(object)pairingPill2);
				((BindableObject)val75).SetValue(VisualElement.WidthRequestProperty, (object)2.0);
				((BindableObject)val75).SetValue(VisualElement.HeightRequestProperty, (object)2.0);
				((BindableObject)val75).SetValue(VisualElement.BackgroundColorProperty, (object)Colors.Transparent);
				((global::System.Collections.Generic.ICollection<IView>)((Layout)val78).Children).Add((IView)(object)val75);
				((BindableObject)pairingPill3).SetValue(PairingPill.TextProperty, (object)searchForDevices_ReadyToPair);
				val76.Path = "IsPushToPair";
				val76.TypedBinding = (TypedBindingBase)(object)new TypedBinding<BleScanResultModel, bool>((Func<BleScanResultModel, ValueTuple<bool, bool>>)([CompilerGenerated] (BleScanResultModel? P_0) => (P_0 != null) ? new ValueTuple<bool, bool>(P_0.IsPushToPair, true) : default(ValueTuple<bool, bool>)), (Action<BleScanResultModel, bool>)([CompilerGenerated] (BleScanResultModel? P_0, bool P_1) =>
				{
					if (P_0 != null)
					{
						P_0.IsPushToPair = P_1;
					}
				}), new Tuple<Func<BleScanResultModel, object>, string>[1]
				{
					new Tuple<Func<BleScanResultModel, object>, string>((Func<BleScanResultModel, object>)([CompilerGenerated] (BleScanResultModel? P_0) => P_0), "IsPushToPair")
				});
				((BindingBase)val76.TypedBinding).Mode = val76.Mode;
				val76.TypedBinding.Converter = val76.Converter;
				val76.TypedBinding.ConverterParameter = val76.ConverterParameter;
				((BindingBase)val76.TypedBinding).StringFormat = val76.StringFormat;
				val76.TypedBinding.Source = val76.Source;
				val76.TypedBinding.UpdateSourceEventName = val76.UpdateSourceEventName;
				((BindingBase)val76.TypedBinding).FallbackValue = val76.FallbackValue;
				((BindingBase)val76.TypedBinding).TargetNullValue = val76.TargetNullValue;
				BindingBase typedBinding9 = (BindingBase)(object)val76.TypedBinding;
				((BindableObject)pairingPill3).SetBinding(VisualElement.IsVisibleProperty, typedBinding9);
				val77.Path = "IsPairingActive";
				val77.TypedBinding = (TypedBindingBase)(object)new TypedBinding<BleScanResultModel, bool>((Func<BleScanResultModel, ValueTuple<bool, bool>>)([CompilerGenerated] (BleScanResultModel? P_0) => (P_0 != null) ? new ValueTuple<bool, bool>(P_0.IsPairingActive, true) : default(ValueTuple<bool, bool>)), (Action<BleScanResultModel, bool>)([CompilerGenerated] (BleScanResultModel? P_0, bool P_1) =>
				{
					if (P_0 != null)
					{
						P_0.IsPairingActive = P_1;
					}
				}), new Tuple<Func<BleScanResultModel, object>, string>[1]
				{
					new Tuple<Func<BleScanResultModel, object>, string>((Func<BleScanResultModel, object>)([CompilerGenerated] (BleScanResultModel? P_0) => P_0), "IsPairingActive")
				});
				((BindingBase)val77.TypedBinding).Mode = val77.Mode;
				val77.TypedBinding.Converter = val77.Converter;
				val77.TypedBinding.ConverterParameter = val77.ConverterParameter;
				((BindingBase)val77.TypedBinding).StringFormat = val77.StringFormat;
				val77.TypedBinding.Source = val77.Source;
				val77.TypedBinding.UpdateSourceEventName = val77.UpdateSourceEventName;
				((BindingBase)val77.TypedBinding).FallbackValue = val77.FallbackValue;
				((BindingBase)val77.TypedBinding).TargetNullValue = val77.TargetNullValue;
				BindingBase typedBinding10 = (BindingBase)(object)val77.TypedBinding;
				((BindableObject)pairingPill3).SetBinding(VisualElement.IsEnabledProperty, typedBinding10);
				((global::System.Collections.Generic.ICollection<IView>)((Layout)val78).Children).Add((IView)(object)pairingPill3);
				((global::System.Collections.Generic.ICollection<IView>)((Layout)val94).Children).Add((IView)(object)val78);
				((BindableObject)signalView).SetValue(Grid.RowProperty, (object)0);
				((BindableObject)signalView).SetValue(Grid.RowSpanProperty, (object)2);
				((BindableObject)signalView).SetValue(Grid.ColumnProperty, (object)1);
				((BindableObject)signalView).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.End);
				((BindableObject)signalView).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Center);
				((BindableObject)signalView).SetValue(VisualElement.WidthRequestProperty, (object)36.0);
				((BindableObject)signalView).SetValue(VisualElement.HeightRequestProperty, (object)36.0);
				val79.Path = "Rssi";
				val79.TypedBinding = (TypedBindingBase)(object)new TypedBinding<BleScanResultModel, int>((Func<BleScanResultModel, ValueTuple<int, bool>>)([CompilerGenerated] (BleScanResultModel? P_0) => (P_0 != null) ? new ValueTuple<int, bool>(P_0.Rssi, true) : default(ValueTuple<int, bool>)), (Action<BleScanResultModel, int>)([CompilerGenerated] (BleScanResultModel? P_0, int P_1) =>
				{
					if (P_0 != null)
					{
						P_0.Rssi = P_1;
					}
				}), new Tuple<Func<BleScanResultModel, object>, string>[1]
				{
					new Tuple<Func<BleScanResultModel, object>, string>((Func<BleScanResultModel, object>)([CompilerGenerated] (BleScanResultModel? P_0) => P_0), "Rssi")
				});
				((BindingBase)val79.TypedBinding).Mode = val79.Mode;
				val79.TypedBinding.Converter = val79.Converter;
				val79.TypedBinding.ConverterParameter = val79.ConverterParameter;
				((BindingBase)val79.TypedBinding).StringFormat = val79.StringFormat;
				val79.TypedBinding.Source = val79.Source;
				val79.TypedBinding.UpdateSourceEventName = val79.UpdateSourceEventName;
				((BindingBase)val79.TypedBinding).FallbackValue = val79.FallbackValue;
				((BindingBase)val79.TypedBinding).TargetNullValue = val79.TargetNullValue;
				BindingBase typedBinding11 = (BindingBase)(object)val79.TypedBinding;
				((BindableObject)signalView).SetBinding(App.Common.Pages.Pairing.SearchForDevices.SignalView.SignalView.SignalProperty, typedBinding11);
				val80.Key = "OnSurface";
				XamlServiceProvider val110 = new XamlServiceProvider();
				val110.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(127, 52)));
				DynamicResource val111 = ((IMarkupExtension<DynamicResource>)(object)val80).ProvideValue((IServiceProvider)val110);
				((IDynamicResourceHandler)signalView).SetDynamicResource(App.Common.Pages.Pairing.SearchForDevices.SignalView.SignalView.TintProperty, val111.Key);
				val84.Value = "True";
				val81.Path = "IsSelected";
				val81.TypedBinding = (TypedBindingBase)(object)new TypedBinding<BleScanResultModel, bool>((Func<BleScanResultModel, ValueTuple<bool, bool>>)([CompilerGenerated] (BleScanResultModel? P_0) => (P_0 != null) ? new ValueTuple<bool, bool>(P_0.IsSelected, true) : default(ValueTuple<bool, bool>)), (Action<BleScanResultModel, bool>)([CompilerGenerated] (BleScanResultModel? P_0, bool P_1) =>
				{
					if (P_0 != null)
					{
						P_0.IsSelected = P_1;
					}
				}), new Tuple<Func<BleScanResultModel, object>, string>[1]
				{
					new Tuple<Func<BleScanResultModel, object>, string>((Func<BleScanResultModel, object>)([CompilerGenerated] (BleScanResultModel? P_0) => P_0), "IsSelected")
				});
				((BindingBase)val81.TypedBinding).Mode = val81.Mode;
				val81.TypedBinding.Converter = val81.Converter;
				val81.TypedBinding.ConverterParameter = val81.ConverterParameter;
				((BindingBase)val81.TypedBinding).StringFormat = val81.StringFormat;
				val81.TypedBinding.Source = val81.Source;
				val81.TypedBinding.UpdateSourceEventName = val81.UpdateSourceEventName;
				((BindingBase)val81.TypedBinding).FallbackValue = val81.FallbackValue;
				((BindingBase)val81.TypedBinding).TargetNullValue = val81.TargetNullValue;
				BindingBase typedBinding12 = (BindingBase)(object)val81.TypedBinding;
				val84.Binding = typedBinding12;
				val83.Property = App.Common.Pages.Pairing.SearchForDevices.SignalView.SignalView.TintProperty;
				val82.Key = "OnTertiaryContainer";
				XamlServiceProvider val112 = new XamlServiceProvider();
				val112.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(132, 65)));
				DynamicResource value5 = ((IMarkupExtension<DynamicResource>)(object)val82).ProvideValue((IServiceProvider)val112);
				val83.Value = value5;
				((global::System.Collections.Generic.ICollection<Setter>)val84.Setters).Add(val83);
				((global::System.Collections.Generic.ICollection<TriggerBase>)((BindableObject)signalView).GetValue(VisualElement.TriggersProperty)).Add((TriggerBase)(object)val84);
				val88.Value = "False";
				val85.Path = "IsSelected";
				val85.TypedBinding = (TypedBindingBase)(object)new TypedBinding<BleScanResultModel, bool>((Func<BleScanResultModel, ValueTuple<bool, bool>>)([CompilerGenerated] (BleScanResultModel? P_0) => (P_0 != null) ? new ValueTuple<bool, bool>(P_0.IsSelected, true) : default(ValueTuple<bool, bool>)), (Action<BleScanResultModel, bool>)([CompilerGenerated] (BleScanResultModel? P_0, bool P_1) =>
				{
					if (P_0 != null)
					{
						P_0.IsSelected = P_1;
					}
				}), new Tuple<Func<BleScanResultModel, object>, string>[1]
				{
					new Tuple<Func<BleScanResultModel, object>, string>((Func<BleScanResultModel, object>)([CompilerGenerated] (BleScanResultModel? P_0) => P_0), "IsSelected")
				});
				((BindingBase)val85.TypedBinding).Mode = val85.Mode;
				val85.TypedBinding.Converter = val85.Converter;
				val85.TypedBinding.ConverterParameter = val85.ConverterParameter;
				((BindingBase)val85.TypedBinding).StringFormat = val85.StringFormat;
				val85.TypedBinding.Source = val85.Source;
				val85.TypedBinding.UpdateSourceEventName = val85.UpdateSourceEventName;
				((BindingBase)val85.TypedBinding).FallbackValue = val85.FallbackValue;
				((BindingBase)val85.TypedBinding).TargetNullValue = val85.TargetNullValue;
				BindingBase typedBinding13 = (BindingBase)(object)val85.TypedBinding;
				val88.Binding = typedBinding13;
				val87.Property = App.Common.Pages.Pairing.SearchForDevices.SignalView.SignalView.TintProperty;
				val86.Key = "OnSurface";
				XamlServiceProvider val113 = new XamlServiceProvider();
				val113.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(137, 65)));
				DynamicResource value6 = ((IMarkupExtension<DynamicResource>)(object)val86).ProvideValue((IServiceProvider)val113);
				val87.Value = value6;
				((global::System.Collections.Generic.ICollection<Setter>)val88.Setters).Add(val87);
				((global::System.Collections.Generic.ICollection<TriggerBase>)((BindableObject)signalView).GetValue(VisualElement.TriggersProperty)).Add((TriggerBase)(object)val88);
				((global::System.Collections.Generic.ICollection<IView>)((Layout)val94).Children).Add((IView)(object)signalView);
				((BindableObject)val93).SetValue(Grid.RowProperty, (object)0);
				((BindableObject)val93).SetValue(Grid.RowSpanProperty, (object)2);
				((BindableObject)val93).SetValue(Grid.ColumnProperty, (object)2);
				((BindableObject)val93).SetValue(VisualElement.WidthRequestProperty, (object)36.0);
				((BindableObject)val93).SetValue(VisualElement.HeightRequestProperty, (object)36.0);
				((BindableObject)val93).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.End);
				((BindableObject)val93).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Center);
				val89.Key = "Tertiary";
				XamlServiceProvider val114 = new XamlServiceProvider();
				val114.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(148, 45)));
				DynamicResource val115 = ((IMarkupExtension<DynamicResource>)(object)val89).ProvideValue((IServiceProvider)val114);
				((IDynamicResourceHandler)val90).SetDynamicResource(BindingProxyExtension<DisabledColorBindingProxy, Color>.ValueProperty, val115.Key);
				XamlServiceProvider val116 = new XamlServiceProvider();
				global::System.Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = parentValues.Length) + 3];
				global::System.Array.Copy((global::System.Array)parentValues, 0, (global::System.Array)array, 3, num);
				array[0] = val93;
				array[1] = val94;
				array[2] = val95;
				SimpleValueTargetProvider val117 = new SimpleValueTargetProvider(array, (object)SKImageView.TintProperty, (INameScope[])(object)new NameScope[7] { val96, val96, val96, val96, val44, val7, val }, (object)root);
				object obj = (object)val117;
				val116.Add(typeFromHandle3, (object)val117);
				val116.Add(typeof(IReferenceProvider), obj);
				val116.Add(typeof(IRootObjectProvider), obj);
				global::System.Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver val118 = new XmlNamespaceResolver();
				val118.Add("", "http://schemas.microsoft.com/dotnet/2021/maui");
				val118.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				val118.Add("ui", "http://lci1.com/schemas/ui");
				val118.Add("skia", "clr-namespace:SkiaSharp.Extended.UI.Controls;assembly=SkiaSharp.Extended.UI");
				val118.Add("disabledColor", "clr-namespace:IDS.UI.MarkupExtensions.DisabledColor;assembly=ids.ui");
				val118.Add("toolkit", "http://schemas.microsoft.com/dotnet/2022/maui/toolkit");
				val118.Add("dualActionPage", "clr-namespace:App.Common.DualActionPage;assembly=App.Common");
				val118.Add("searchForDevices", "clr-namespace:App.Common.Pages.Pairing.SearchForDevices");
				val118.Add("signalView", "clr-namespace:App.Common.Pages.Pairing.SearchForDevices.SignalView");
				val118.Add("resources", "clr-namespace:App.Common.Pages.Pairing.Resources");
				val116.Add(typeFromHandle4, (object)new XamlTypeResolver((IXmlNamespaceResolver)val118, typeof(<InitializeComponent>_anonXamlCDataTemplate_0).Assembly));
				val116.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(148, 45)));
				BindingBase val119 = ((IMarkupExtension<BindingBase>)(object)val90).ProvideValue((IServiceProvider)val116);
				((BindableObject)val93).SetBinding(SKImageView.TintProperty, val119);
				val91.Path = "IsSelected";
				val91.TypedBinding = (TypedBindingBase)(object)new TypedBinding<BleScanResultModel, bool>((Func<BleScanResultModel, ValueTuple<bool, bool>>)([CompilerGenerated] (BleScanResultModel? P_0) => (P_0 != null) ? new ValueTuple<bool, bool>(P_0.IsSelected, true) : default(ValueTuple<bool, bool>)), (Action<BleScanResultModel, bool>)([CompilerGenerated] (BleScanResultModel? P_0, bool P_1) =>
				{
					if (P_0 != null)
					{
						P_0.IsSelected = P_1;
					}
				}), new Tuple<Func<BleScanResultModel, object>, string>[1]
				{
					new Tuple<Func<BleScanResultModel, object>, string>((Func<BleScanResultModel, object>)([CompilerGenerated] (BleScanResultModel? P_0) => P_0), "IsSelected")
				});
				((BindingBase)val91.TypedBinding).Mode = val91.Mode;
				val91.TypedBinding.Converter = val91.Converter;
				val91.TypedBinding.ConverterParameter = val91.ConverterParameter;
				((BindingBase)val91.TypedBinding).StringFormat = val91.StringFormat;
				val91.TypedBinding.Source = val91.Source;
				val91.TypedBinding.UpdateSourceEventName = val91.UpdateSourceEventName;
				((BindingBase)val91.TypedBinding).FallbackValue = val91.FallbackValue;
				((BindingBase)val91.TypedBinding).TargetNullValue = val91.TargetNullValue;
				BindingBase typedBinding14 = (BindingBase)(object)val91.TypedBinding;
				((BindableObject)val93).SetBinding(VisualElement.IsEnabledProperty, typedBinding14);
				((BindableObject)val92).SetValue(FontImageSource.FontFamilyProperty, (object)proSolid);
				((BindableObject)val92).SetValue(FontImageSource.GlyphProperty, (object)circleCheck);
				((BindableObject)val93).SetValue(SKImageView.ImageSourceProperty, (object)val92);
				((global::System.Collections.Generic.ICollection<IView>)((Layout)val94).Children).Add((IView)(object)val93);
				((BindableObject)val95).SetValue(Border.ContentProperty, (object)val94);
				return val95;
			}
		}

		public static readonly BindableProperty EmptyViewProperty = BindableProperty.Create("EmptyView", typeof(View), typeof(SearchForDevicesPage), (object)null, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)null, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)null, (CreateDefaultValueDelegate)null);

		public View? EmptyView
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected O, but got Unknown
				return (View)((BindableObject)this).GetValue(EmptyViewProperty);
			}
			set
			{
				((BindableObject)this).SetValue(EmptyViewProperty, (object)value);
			}
		}

		public SearchForDevicesPage()
		{
			InitializeComponent();
		}

		[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
		private void InitializeComponent()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Expected O, but got Unknown
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Expected O, but got Unknown
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Expected O, but got Unknown
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Expected O, but got Unknown
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Expected O, but got Unknown
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Expected O, but got Unknown
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Expected O, but got Unknown
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Expected O, but got Unknown
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Expected O, but got Unknown
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Expected O, but got Unknown
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_0320: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0446: Unknown result type (might be due to invalid IL or missing references)
			//IL_049a: Unknown result type (might be due to invalid IL or missing references)
			//IL_049f: Unknown result type (might be due to invalid IL or missing references)
			//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_04af: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_04df: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_050f: Unknown result type (might be due to invalid IL or missing references)
			//IL_051f: Unknown result type (might be due to invalid IL or missing references)
			//IL_052f: Unknown result type (might be due to invalid IL or missing references)
			//IL_053f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0563: Expected O, but got Unknown
			//IL_055e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0568: Expected O, but got Unknown
			//IL_056d: Expected O, but got Unknown
			//IL_0599: Unknown result type (might be due to invalid IL or missing references)
			//IL_059e: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b7: Expected O, but got Unknown
			//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05bc: Expected O, but got Unknown
			//IL_05c1: Expected O, but got Unknown
			//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_060c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0611: Unknown result type (might be due to invalid IL or missing references)
			//IL_0620: Unknown result type (might be due to invalid IL or missing references)
			//IL_062a: Expected O, but got Unknown
			//IL_0625: Unknown result type (might be due to invalid IL or missing references)
			//IL_062f: Expected O, but got Unknown
			//IL_0634: Expected O, but got Unknown
			//IL_069c: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0701: Unknown result type (might be due to invalid IL or missing references)
			//IL_0711: Unknown result type (might be due to invalid IL or missing references)
			//IL_0721: Unknown result type (might be due to invalid IL or missing references)
			//IL_0731: Unknown result type (might be due to invalid IL or missing references)
			//IL_0741: Unknown result type (might be due to invalid IL or missing references)
			//IL_0765: Expected O, but got Unknown
			//IL_0760: Unknown result type (might be due to invalid IL or missing references)
			//IL_076a: Expected O, but got Unknown
			//IL_076f: Expected O, but got Unknown
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			BindingExtension val = new BindingExtension();
			AndMultiConverter val2 = new AndMultiConverter();
			BindingExtension val3 = new BindingExtension();
			BindingExtension val4 = new BindingExtension();
			SKLottieView val5 = new SKLottieView();
			global::System.Type typeFromHandle = typeof(DualActionPage);
			RelativeSourceExtension val6 = new RelativeSourceExtension();
			BindingExtension val7 = new BindingExtension();
			ContentView val8 = new ContentView();
			DynamicResourceExtension val9 = new DynamicResourceExtension();
			DynamicResourceExtension val10 = new DynamicResourceExtension();
			string searchForDevices_Searching = Strings.SearchForDevices_Searching;
			global::System.Type typeFromHandle2 = typeof(DualActionPage);
			RelativeSourceExtension val11 = new RelativeSourceExtension();
			IsNullConverter val12 = new IsNullConverter();
			BindingExtension val13 = new BindingExtension();
			Label val14 = new Label();
			VerticalStackLayout val15 = new VerticalStackLayout();
			DataTemplate val16 = new DataTemplate();
			RadioButtonList val17 = new RadioButtonList();
			SearchForDevicesPage searchForDevicesPage;
			NameScope val18 = (NameScope)(((object)NameScope.GetNameScope((BindableObject)(object)(searchForDevicesPage = this))) ?? ((object)new NameScope()));
			NameScope.SetNameScope((BindableObject)(object)searchForDevicesPage, (INameScope)(object)val18);
			((Element)val17).transientNamescope = (INameScope)(object)val18;
			((Element)val15).transientNamescope = (INameScope)(object)val18;
			((Element)val5).transientNamescope = (INameScope)(object)val18;
			((Element)val8).transientNamescope = (INameScope)(object)val18;
			((Element)val14).transientNamescope = (INameScope)(object)val18;
			val.Path = "Message";
			val.TypedBinding = (TypedBindingBase)(object)new TypedBinding<SearchForDevicesViewModel, string>((Func<SearchForDevicesViewModel, ValueTuple<string, bool>>)([CompilerGenerated] (SearchForDevicesViewModel? P_0) => (P_0 != null) ? new ValueTuple<string, bool>(P_0.Message, true) : default(ValueTuple<string, bool>)), (Action<SearchForDevicesViewModel, string>)([CompilerGenerated] (SearchForDevicesViewModel? P_0, string? P_1) =>
			{
				if (P_0 != null)
				{
					P_0.Message = P_1;
				}
			}), new Tuple<Func<SearchForDevicesViewModel, object>, string>[1]
			{
				new Tuple<Func<SearchForDevicesViewModel, object>, string>((Func<SearchForDevicesViewModel, object>)([CompilerGenerated] (SearchForDevicesViewModel? P_0) => P_0), "Message")
			});
			((BindingBase)val.TypedBinding).Mode = val.Mode;
			val.TypedBinding.Converter = val.Converter;
			val.TypedBinding.ConverterParameter = val.ConverterParameter;
			((BindingBase)val.TypedBinding).StringFormat = val.StringFormat;
			val.TypedBinding.Source = val.Source;
			val.TypedBinding.UpdateSourceEventName = val.UpdateSourceEventName;
			((BindingBase)val.TypedBinding).FallbackValue = val.FallbackValue;
			((BindingBase)val.TypedBinding).TargetNullValue = val.TargetNullValue;
			BindingBase typedBinding = (BindingBase)(object)val.TypedBinding;
			((BindableObject)searchForDevicesPage).SetBinding(DualActionPage.InstructionsProperty, typedBinding);
			((VisualElement)searchForDevicesPage).Resources.Add("AndMultiConverter", (object)val2);
			((BindableObject)val17).SetValue(Grid.RowProperty, (object)1);
			val3.Path = "SearchResults";
			val3.TypedBinding = (TypedBindingBase)(object)new TypedBinding<SearchForDevicesViewModel, global::System.Collections.Generic.IEnumerable<BleScanResultModel>>((Func<SearchForDevicesViewModel, ValueTuple<global::System.Collections.Generic.IEnumerable<BleScanResultModel>, bool>>)([CompilerGenerated] (SearchForDevicesViewModel? P_0) => (P_0 != null) ? new ValueTuple<global::System.Collections.Generic.IEnumerable<BleScanResultModel>, bool>(P_0.SearchResults, true) : default(ValueTuple<global::System.Collections.Generic.IEnumerable<BleScanResultModel>, bool>)), (Action<SearchForDevicesViewModel, global::System.Collections.Generic.IEnumerable<BleScanResultModel>>)null, new Tuple<Func<SearchForDevicesViewModel, object>, string>[1]
			{
				new Tuple<Func<SearchForDevicesViewModel, object>, string>((Func<SearchForDevicesViewModel, object>)([CompilerGenerated] (SearchForDevicesViewModel? P_0) => P_0), "SearchResults")
			});
			((BindingBase)val3.TypedBinding).Mode = val3.Mode;
			val3.TypedBinding.Converter = val3.Converter;
			val3.TypedBinding.ConverterParameter = val3.ConverterParameter;
			((BindingBase)val3.TypedBinding).StringFormat = val3.StringFormat;
			val3.TypedBinding.Source = val3.Source;
			val3.TypedBinding.UpdateSourceEventName = val3.UpdateSourceEventName;
			((BindingBase)val3.TypedBinding).FallbackValue = val3.FallbackValue;
			((BindingBase)val3.TypedBinding).TargetNullValue = val3.TargetNullValue;
			BindingBase typedBinding2 = (BindingBase)(object)val3.TypedBinding;
			((BindableObject)val17).SetBinding(RadioButtonList.ItemsSourceProperty, typedBinding2);
			val4.Path = "SelectedSearchResult";
			val4.TypedBinding = (TypedBindingBase)(object)new TypedBinding<SearchForDevicesViewModel, BleScanResultModel>((Func<SearchForDevicesViewModel, ValueTuple<BleScanResultModel, bool>>)([CompilerGenerated] (SearchForDevicesViewModel? P_0) => (P_0 != null) ? new ValueTuple<BleScanResultModel, bool>(P_0.SelectedSearchResult, true) : default(ValueTuple<BleScanResultModel, bool>)), (Action<SearchForDevicesViewModel, BleScanResultModel>)([CompilerGenerated] (SearchForDevicesViewModel? P_0, BleScanResultModel? P_1) =>
			{
				if (P_0 != null)
				{
					P_0.SelectedSearchResult = P_1;
				}
			}), new Tuple<Func<SearchForDevicesViewModel, object>, string>[1]
			{
				new Tuple<Func<SearchForDevicesViewModel, object>, string>((Func<SearchForDevicesViewModel, object>)([CompilerGenerated] (SearchForDevicesViewModel? P_0) => P_0), "SelectedSearchResult")
			});
			((BindingBase)val4.TypedBinding).Mode = val4.Mode;
			val4.TypedBinding.Converter = val4.Converter;
			val4.TypedBinding.ConverterParameter = val4.ConverterParameter;
			((BindingBase)val4.TypedBinding).StringFormat = val4.StringFormat;
			val4.TypedBinding.Source = val4.Source;
			val4.TypedBinding.UpdateSourceEventName = val4.UpdateSourceEventName;
			((BindingBase)val4.TypedBinding).FallbackValue = val4.FallbackValue;
			((BindingBase)val4.TypedBinding).TargetNullValue = val4.TargetNullValue;
			BindingBase typedBinding3 = (BindingBase)(object)val4.TypedBinding;
			((BindableObject)val17).SetBinding(RadioButtonList.SelectedItemProperty, typedBinding3);
			((BindableObject)val15).SetValue(StackBase.SpacingProperty, (object)16.0);
			((BindableObject)val5).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Center);
			((BindableObject)val5).SetValue(VisualElement.WidthRequestProperty, (object)248.0);
			((BindableObject)val5).SetValue(VisualElement.HeightRequestProperty, (object)248.0);
			((BindableObject)val5).SetValue(SKLottieView.RepeatCountProperty, (object)(-1));
			((BindableObject)val5).SetValue(SKLottieView.RepeatModeProperty, (object)(SKLottieRepeatMode)0);
			((BindableObject)val5).SetValue(SKLottieView.SourceProperty, ((TypeConverter)new SKLottieImageSourceConverter()).ConvertFromInvariantString("OneControl/Resources/Lottie/connecting_spinner.json"));
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val15).Children).Add((IView)(object)val5);
			val7.Path = "EmptyView";
			val6.Mode = (RelativeBindingSourceMode)3;
			val6.AncestorType = typeFromHandle;
			RelativeBindingSource source = ((IMarkupExtension<RelativeBindingSource>)(object)val6).ProvideValue((IServiceProvider)null);
			val7.Source = source;
			XamlServiceProvider val19 = new XamlServiceProvider();
			global::System.Type typeFromHandle3 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver val20 = new XmlNamespaceResolver();
			val20.Add("", "http://schemas.microsoft.com/dotnet/2021/maui");
			val20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			val20.Add("ui", "http://lci1.com/schemas/ui");
			val20.Add("skia", "clr-namespace:SkiaSharp.Extended.UI.Controls;assembly=SkiaSharp.Extended.UI");
			val20.Add("disabledColor", "clr-namespace:IDS.UI.MarkupExtensions.DisabledColor;assembly=ids.ui");
			val20.Add("toolkit", "http://schemas.microsoft.com/dotnet/2022/maui/toolkit");
			val20.Add("dualActionPage", "clr-namespace:App.Common.DualActionPage;assembly=App.Common");
			val20.Add("searchForDevices", "clr-namespace:App.Common.Pages.Pairing.SearchForDevices");
			val20.Add("signalView", "clr-namespace:App.Common.Pages.Pairing.SearchForDevices.SignalView");
			val20.Add("resources", "clr-namespace:App.Common.Pages.Pairing.Resources");
			val19.Add(typeFromHandle3, (object)new XamlTypeResolver((IXmlNamespaceResolver)val20, typeof(SearchForDevicesPage).Assembly));
			BindingBase val21 = ((IMarkupExtension<BindingBase>)(object)val7).ProvideValue((IServiceProvider)val19);
			((BindableObject)val8).SetBinding(ContentView.ContentProperty, val21);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val15).Children).Add((IView)(object)val8);
			val9.Key = "Body2_Label";
			XamlServiceProvider val22 = new XamlServiceProvider();
			val22.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(36, 28)));
			DynamicResource val23 = ((IMarkupExtension<DynamicResource>)(object)val9).ProvideValue((IServiceProvider)val22);
			((IDynamicResourceHandler)val14).SetDynamicResource(VisualElement.StyleProperty, val23.Key);
			((BindableObject)val14).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Start);
			((BindableObject)val14).SetValue(Label.HorizontalTextAlignmentProperty, (object)(TextAlignment)1);
			val10.Key = "OnSurface";
			XamlServiceProvider val24 = new XamlServiceProvider();
			val24.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(39, 28)));
			DynamicResource val25 = ((IMarkupExtension<DynamicResource>)(object)val10).ProvideValue((IServiceProvider)val24);
			((IDynamicResourceHandler)val14).SetDynamicResource(Label.TextColorProperty, val25.Key);
			((BindableObject)val14).SetValue(Label.TextProperty, (object)searchForDevices_Searching);
			val13.Path = "EmptyView";
			val11.Mode = (RelativeBindingSourceMode)3;
			val11.AncestorType = typeFromHandle2;
			RelativeBindingSource source2 = ((IMarkupExtension<RelativeBindingSource>)(object)val11).ProvideValue((IServiceProvider)null);
			val13.Source = source2;
			ICommunityToolkitValueConverter converter = ((IMarkupExtension<ICommunityToolkitValueConverter>)(object)val12).ProvideValue((IServiceProvider)null);
			val13.Converter = (IValueConverter)(object)converter;
			XamlServiceProvider val26 = new XamlServiceProvider();
			global::System.Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver val27 = new XmlNamespaceResolver();
			val27.Add("", "http://schemas.microsoft.com/dotnet/2021/maui");
			val27.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			val27.Add("ui", "http://lci1.com/schemas/ui");
			val27.Add("skia", "clr-namespace:SkiaSharp.Extended.UI.Controls;assembly=SkiaSharp.Extended.UI");
			val27.Add("disabledColor", "clr-namespace:IDS.UI.MarkupExtensions.DisabledColor;assembly=ids.ui");
			val27.Add("toolkit", "http://schemas.microsoft.com/dotnet/2022/maui/toolkit");
			val27.Add("dualActionPage", "clr-namespace:App.Common.DualActionPage;assembly=App.Common");
			val27.Add("searchForDevices", "clr-namespace:App.Common.Pages.Pairing.SearchForDevices");
			val27.Add("signalView", "clr-namespace:App.Common.Pages.Pairing.SearchForDevices.SignalView");
			val27.Add("resources", "clr-namespace:App.Common.Pages.Pairing.Resources");
			val26.Add(typeFromHandle4, (object)new XamlTypeResolver((IXmlNamespaceResolver)val27, typeof(SearchForDevicesPage).Assembly));
			BindingBase val28 = ((IMarkupExtension<BindingBase>)(object)val13).ProvideValue((IServiceProvider)val26);
			((BindableObject)val14).SetBinding(VisualElement.IsVisibleProperty, val28);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val15).Children).Add((IView)(object)val14);
			((BindableObject)val17).SetValue(RadioButtonList.EmptyViewProperty, (object)val15);
			NameScope? _scope0 = val18;
			NameScope? _scope1 = val18;
			NameScope? _scope2 = val18;
			NameScope? _scope3 = val18;
			NameScope? _scope4 = val18;
			NameScope? _scope5 = val18;
			NameScope? _scope6 = val18;
			NameScope? _scope7 = val18;
			NameScope? _scope8 = val18;
			NameScope? _scope9 = val18;
			NameScope? _scope10 = val18;
			NameScope? _scope11 = val18;
			NameScope? _scope12 = val18;
			NameScope? _scope13 = val18;
			NameScope? _scope14 = val18;
			NameScope? _scope15 = val18;
			NameScope? _scope16 = val18;
			NameScope? _scope17 = val18;
			NameScope? _scope18 = val18;
			NameScope? _scope19 = val18;
			NameScope? _scope20 = val18;
			NameScope? _scope21 = val18;
			NameScope? _scope22 = val18;
			NameScope? _scope23 = val18;
			NameScope? _scope24 = val18;
			NameScope? _scope25 = val18;
			NameScope? _scope26 = val18;
			NameScope? _scope27 = val18;
			NameScope? _scope28 = val18;
			NameScope? _scope29 = val18;
			NameScope? _scope30 = val18;
			NameScope? _scope31 = val18;
			NameScope? _scope32 = val18;
			NameScope? _scope33 = val18;
			NameScope? _scope34 = val18;
			NameScope? _scope35 = val18;
			NameScope? _scope36 = val18;
			NameScope? _scope37 = val18;
			NameScope? _scope38 = val18;
			NameScope? _scope39 = val18;
			NameScope? _scope40 = val18;
			NameScope? _scope41 = val18;
			NameScope? _scope42 = val18;
			NameScope? _scope43 = val18;
			NameScope? _scope44 = val18;
			object[] array = new object[0 + 3];
			array[0] = val16;
			array[1] = val17;
			array[2] = searchForDevicesPage;
			object?[]? parentValues = array;
			SearchForDevicesPage? root = searchForDevicesPage;
			((ElementTemplate)val16).LoadTemplate = delegate
			{
				//IL_0165: Unknown result type (might be due to invalid IL or missing references)
				//IL_016c: Expected O, but got Unknown
				//IL_016c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0173: Expected O, but got Unknown
				//IL_0173: Unknown result type (might be due to invalid IL or missing references)
				//IL_017a: Expected O, but got Unknown
				//IL_017a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0181: Expected O, but got Unknown
				//IL_0181: Unknown result type (might be due to invalid IL or missing references)
				//IL_0188: Expected O, but got Unknown
				//IL_0192: Unknown result type (might be due to invalid IL or missing references)
				//IL_0199: Expected O, but got Unknown
				//IL_0199: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a0: Expected O, but got Unknown
				//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a7: Expected O, but got Unknown
				//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ae: Expected O, but got Unknown
				//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
				//IL_01bf: Expected O, but got Unknown
				//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d2: Expected O, but got Unknown
				//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d9: Expected O, but got Unknown
				//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
				//IL_01e0: Expected O, but got Unknown
				//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
				//IL_01e7: Expected O, but got Unknown
				//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ee: Expected O, but got Unknown
				//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
				//IL_01f5: Expected O, but got Unknown
				//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
				//IL_01fc: Expected O, but got Unknown
				//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
				//IL_0203: Expected O, but got Unknown
				//IL_0203: Unknown result type (might be due to invalid IL or missing references)
				//IL_020a: Expected O, but got Unknown
				//IL_020a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0211: Expected O, but got Unknown
				//IL_021b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0222: Expected O, but got Unknown
				//IL_0222: Unknown result type (might be due to invalid IL or missing references)
				//IL_0229: Expected O, but got Unknown
				//IL_0229: Unknown result type (might be due to invalid IL or missing references)
				//IL_0230: Expected O, but got Unknown
				//IL_0230: Unknown result type (might be due to invalid IL or missing references)
				//IL_0237: Expected O, but got Unknown
				//IL_0241: Unknown result type (might be due to invalid IL or missing references)
				//IL_0248: Expected O, but got Unknown
				//IL_0248: Unknown result type (might be due to invalid IL or missing references)
				//IL_024f: Expected O, but got Unknown
				//IL_0256: Unknown result type (might be due to invalid IL or missing references)
				//IL_025d: Expected O, but got Unknown
				//IL_0264: Unknown result type (might be due to invalid IL or missing references)
				//IL_026b: Expected O, but got Unknown
				//IL_0272: Unknown result type (might be due to invalid IL or missing references)
				//IL_0279: Expected O, but got Unknown
				//IL_0280: Unknown result type (might be due to invalid IL or missing references)
				//IL_0287: Expected O, but got Unknown
				//IL_028e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0295: Expected O, but got Unknown
				//IL_0295: Unknown result type (might be due to invalid IL or missing references)
				//IL_029c: Expected O, but got Unknown
				//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
				//IL_02aa: Expected O, but got Unknown
				//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
				//IL_02b1: Expected O, but got Unknown
				//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
				//IL_02b8: Expected O, but got Unknown
				//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
				//IL_02bf: Expected O, but got Unknown
				//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
				//IL_02c6: Expected O, but got Unknown
				//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
				//IL_02cd: Expected O, but got Unknown
				//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
				//IL_02de: Expected O, but got Unknown
				//IL_02de: Unknown result type (might be due to invalid IL or missing references)
				//IL_02e5: Expected O, but got Unknown
				//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
				//IL_02ec: Expected O, but got Unknown
				//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
				//IL_02f3: Expected O, but got Unknown
				//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
				//IL_0304: Expected O, but got Unknown
				//IL_030b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0312: Expected O, but got Unknown
				//IL_0312: Unknown result type (might be due to invalid IL or missing references)
				//IL_0319: Expected O, but got Unknown
				//IL_0319: Unknown result type (might be due to invalid IL or missing references)
				//IL_0320: Expected O, but got Unknown
				//IL_032e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0335: Expected O, but got Unknown
				//IL_0335: Unknown result type (might be due to invalid IL or missing references)
				//IL_033c: Expected O, but got Unknown
				//IL_033c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0343: Expected O, but got Unknown
				//IL_0343: Unknown result type (might be due to invalid IL or missing references)
				//IL_034a: Expected O, but got Unknown
				//IL_034a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0351: Expected O, but got Unknown
				//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
				//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
				//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
				//IL_0404: Expected O, but got Unknown
				//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
				//IL_0409: Expected O, but got Unknown
				//IL_040e: Expected O, but got Unknown
				//IL_0433: Unknown result type (might be due to invalid IL or missing references)
				//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
				//IL_056f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0574: Unknown result type (might be due to invalid IL or missing references)
				//IL_0583: Unknown result type (might be due to invalid IL or missing references)
				//IL_058d: Expected O, but got Unknown
				//IL_0588: Unknown result type (might be due to invalid IL or missing references)
				//IL_0592: Expected O, but got Unknown
				//IL_0597: Expected O, but got Unknown
				//IL_0627: Unknown result type (might be due to invalid IL or missing references)
				//IL_06e2: Unknown result type (might be due to invalid IL or missing references)
				//IL_06e7: Unknown result type (might be due to invalid IL or missing references)
				//IL_06f6: Unknown result type (might be due to invalid IL or missing references)
				//IL_0700: Expected O, but got Unknown
				//IL_06fb: Unknown result type (might be due to invalid IL or missing references)
				//IL_0705: Expected O, but got Unknown
				//IL_070a: Expected O, but got Unknown
				//IL_074e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0753: Unknown result type (might be due to invalid IL or missing references)
				//IL_0759: Expected O, but got Unknown
				//IL_0764: Unknown result type (might be due to invalid IL or missing references)
				//IL_0769: Unknown result type (might be due to invalid IL or missing references)
				//IL_076f: Expected O, but got Unknown
				//IL_076f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0779: Expected O, but got Unknown
				//IL_0788: Unknown result type (might be due to invalid IL or missing references)
				//IL_078d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0793: Expected O, but got Unknown
				//IL_079e: Unknown result type (might be due to invalid IL or missing references)
				//IL_07a3: Unknown result type (might be due to invalid IL or missing references)
				//IL_07a9: Expected O, but got Unknown
				//IL_07b4: Unknown result type (might be due to invalid IL or missing references)
				//IL_07b9: Unknown result type (might be due to invalid IL or missing references)
				//IL_07bf: Expected O, but got Unknown
				//IL_07bf: Unknown result type (might be due to invalid IL or missing references)
				//IL_07c9: Expected O, but got Unknown
				//IL_080d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0846: Unknown result type (might be due to invalid IL or missing references)
				//IL_084b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0856: Unknown result type (might be due to invalid IL or missing references)
				//IL_085b: Unknown result type (might be due to invalid IL or missing references)
				//IL_086b: Unknown result type (might be due to invalid IL or missing references)
				//IL_087b: Unknown result type (might be due to invalid IL or missing references)
				//IL_088b: Unknown result type (might be due to invalid IL or missing references)
				//IL_089b: Unknown result type (might be due to invalid IL or missing references)
				//IL_08ab: Unknown result type (might be due to invalid IL or missing references)
				//IL_08bb: Unknown result type (might be due to invalid IL or missing references)
				//IL_08cb: Unknown result type (might be due to invalid IL or missing references)
				//IL_08db: Unknown result type (might be due to invalid IL or missing references)
				//IL_08eb: Unknown result type (might be due to invalid IL or missing references)
				//IL_090f: Expected O, but got Unknown
				//IL_090a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0914: Expected O, but got Unknown
				//IL_0919: Expected O, but got Unknown
				//IL_0958: Unknown result type (might be due to invalid IL or missing references)
				//IL_0a2c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0a31: Unknown result type (might be due to invalid IL or missing references)
				//IL_0a40: Unknown result type (might be due to invalid IL or missing references)
				//IL_0a4a: Expected O, but got Unknown
				//IL_0a45: Unknown result type (might be due to invalid IL or missing references)
				//IL_0a4f: Expected O, but got Unknown
				//IL_0a54: Expected O, but got Unknown
				//IL_0a8a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0add: Unknown result type (might be due to invalid IL or missing references)
				//IL_0ae2: Unknown result type (might be due to invalid IL or missing references)
				//IL_0af1: Unknown result type (might be due to invalid IL or missing references)
				//IL_0afb: Expected O, but got Unknown
				//IL_0af6: Unknown result type (might be due to invalid IL or missing references)
				//IL_0b00: Expected O, but got Unknown
				//IL_0b05: Expected O, but got Unknown
				//IL_0b6f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0c79: Unknown result type (might be due to invalid IL or missing references)
				//IL_0d34: Unknown result type (might be due to invalid IL or missing references)
				//IL_0d39: Unknown result type (might be due to invalid IL or missing references)
				//IL_0d48: Unknown result type (might be due to invalid IL or missing references)
				//IL_0d52: Expected O, but got Unknown
				//IL_0d4d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0d57: Expected O, but got Unknown
				//IL_0d5c: Expected O, but got Unknown
				//IL_0dec: Unknown result type (might be due to invalid IL or missing references)
				//IL_0ea7: Unknown result type (might be due to invalid IL or missing references)
				//IL_0eac: Unknown result type (might be due to invalid IL or missing references)
				//IL_0ebb: Unknown result type (might be due to invalid IL or missing references)
				//IL_0ec5: Expected O, but got Unknown
				//IL_0ec0: Unknown result type (might be due to invalid IL or missing references)
				//IL_0eca: Expected O, but got Unknown
				//IL_0ecf: Expected O, but got Unknown
				//IL_0f34: Unknown result type (might be due to invalid IL or missing references)
				//IL_0f4a: Unknown result type (might be due to invalid IL or missing references)
				//IL_1007: Unknown result type (might be due to invalid IL or missing references)
				//IL_1174: Unknown result type (might be due to invalid IL or missing references)
				//IL_12e1: Unknown result type (might be due to invalid IL or missing references)
				//IL_13df: Unknown result type (might be due to invalid IL or missing references)
				//IL_14de: Unknown result type (might be due to invalid IL or missing references)
				//IL_14f4: Unknown result type (might be due to invalid IL or missing references)
				//IL_158f: Unknown result type (might be due to invalid IL or missing references)
				//IL_1643: Unknown result type (might be due to invalid IL or missing references)
				//IL_1648: Unknown result type (might be due to invalid IL or missing references)
				//IL_1657: Unknown result type (might be due to invalid IL or missing references)
				//IL_1661: Expected O, but got Unknown
				//IL_165c: Unknown result type (might be due to invalid IL or missing references)
				//IL_1666: Expected O, but got Unknown
				//IL_166b: Expected O, but got Unknown
				//IL_16e4: Unknown result type (might be due to invalid IL or missing references)
				//IL_179f: Unknown result type (might be due to invalid IL or missing references)
				//IL_17a4: Unknown result type (might be due to invalid IL or missing references)
				//IL_17b6: Unknown result type (might be due to invalid IL or missing references)
				//IL_17c0: Expected O, but got Unknown
				//IL_17bb: Unknown result type (might be due to invalid IL or missing references)
				//IL_17c5: Expected O, but got Unknown
				//IL_17ca: Expected O, but got Unknown
				//IL_185a: Unknown result type (might be due to invalid IL or missing references)
				//IL_1915: Unknown result type (might be due to invalid IL or missing references)
				//IL_191a: Unknown result type (might be due to invalid IL or missing references)
				//IL_192c: Unknown result type (might be due to invalid IL or missing references)
				//IL_1936: Expected O, but got Unknown
				//IL_1931: Unknown result type (might be due to invalid IL or missing references)
				//IL_193b: Expected O, but got Unknown
				//IL_1940: Expected O, but got Unknown
				//IL_19eb: Unknown result type (might be due to invalid IL or missing references)
				//IL_1a01: Unknown result type (might be due to invalid IL or missing references)
				//IL_1a1e: Unknown result type (might be due to invalid IL or missing references)
				//IL_1a23: Unknown result type (might be due to invalid IL or missing references)
				//IL_1a35: Unknown result type (might be due to invalid IL or missing references)
				//IL_1a3f: Expected O, but got Unknown
				//IL_1a3a: Unknown result type (might be due to invalid IL or missing references)
				//IL_1a44: Expected O, but got Unknown
				//IL_1a49: Expected O, but got Unknown
				//IL_1a60: Unknown result type (might be due to invalid IL or missing references)
				//IL_1a65: Unknown result type (might be due to invalid IL or missing references)
				//IL_1ad9: Unknown result type (might be due to invalid IL or missing references)
				//IL_1ade: Unknown result type (might be due to invalid IL or missing references)
				//IL_1ae1: Expected O, but got Unknown
				//IL_1ae6: Expected O, but got Unknown
				//IL_1ae6: Unknown result type (might be due to invalid IL or missing references)
				//IL_1af8: Unknown result type (might be due to invalid IL or missing references)
				//IL_1b0a: Unknown result type (might be due to invalid IL or missing references)
				//IL_1b15: Unknown result type (might be due to invalid IL or missing references)
				//IL_1b1a: Unknown result type (might be due to invalid IL or missing references)
				//IL_1b2a: Unknown result type (might be due to invalid IL or missing references)
				//IL_1b3a: Unknown result type (might be due to invalid IL or missing references)
				//IL_1b4a: Unknown result type (might be due to invalid IL or missing references)
				//IL_1b5a: Unknown result type (might be due to invalid IL or missing references)
				//IL_1b6a: Unknown result type (might be due to invalid IL or missing references)
				//IL_1b7a: Unknown result type (might be due to invalid IL or missing references)
				//IL_1b8a: Unknown result type (might be due to invalid IL or missing references)
				//IL_1b9a: Unknown result type (might be due to invalid IL or missing references)
				//IL_1baa: Unknown result type (might be due to invalid IL or missing references)
				//IL_1bce: Expected O, but got Unknown
				//IL_1bc9: Unknown result type (might be due to invalid IL or missing references)
				//IL_1bd3: Expected O, but got Unknown
				//IL_1bd3: Unknown result type (might be due to invalid IL or missing references)
				//IL_1be5: Unknown result type (might be due to invalid IL or missing references)
				//IL_1bef: Expected O, but got Unknown
				//IL_1bea: Unknown result type (might be due to invalid IL or missing references)
				//IL_1bf4: Expected O, but got Unknown
				//IL_1bf9: Expected O, but got Unknown
				//IL_1c61: Unknown result type (might be due to invalid IL or missing references)
				NameScope val29 = _scope0;
				NameScope val30 = _scope1;
				NameScope val31 = _scope2;
				NameScope val32 = _scope3;
				NameScope val33 = _scope4;
				NameScope val34 = _scope5;
				NameScope val35 = _scope6;
				NameScope val36 = _scope7;
				NameScope val37 = _scope8;
				NameScope val38 = _scope9;
				NameScope val39 = _scope10;
				NameScope val40 = _scope11;
				NameScope val41 = _scope12;
				NameScope val42 = _scope13;
				NameScope val43 = _scope14;
				NameScope val44 = _scope15;
				NameScope val45 = _scope16;
				NameScope val46 = _scope17;
				NameScope val47 = _scope18;
				NameScope val48 = _scope19;
				NameScope val49 = _scope20;
				NameScope val50 = _scope21;
				NameScope val51 = _scope22;
				NameScope val52 = _scope23;
				NameScope val53 = _scope24;
				NameScope val54 = _scope25;
				NameScope val55 = _scope26;
				NameScope val56 = _scope27;
				NameScope val57 = _scope28;
				NameScope val58 = _scope29;
				NameScope val59 = _scope30;
				NameScope val60 = _scope31;
				NameScope val61 = _scope32;
				NameScope val62 = _scope33;
				NameScope val63 = _scope34;
				NameScope val64 = _scope35;
				NameScope val65 = _scope36;
				NameScope val66 = _scope37;
				NameScope val67 = _scope38;
				NameScope val68 = _scope39;
				NameScope val69 = _scope40;
				NameScope val70 = _scope41;
				NameScope val71 = _scope42;
				NameScope val72 = _scope43;
				NameScope val73 = _scope44;
				DynamicResourceExtension val74 = new DynamicResourceExtension();
				RoundRectangle val75 = new RoundRectangle();
				BindingExtension val76 = new BindingExtension();
				DynamicResourceExtension val77 = new DynamicResourceExtension();
				Setter val78 = new Setter();
				DataTrigger val79 = new DataTrigger(typeof(Border));
				BindingExtension val80 = new BindingExtension();
				DynamicResourceExtension val81 = new DynamicResourceExtension();
				Setter val82 = new Setter();
				DataTrigger val83 = new DataTrigger(typeof(Border));
				global::System.Type typeFromHandle5 = typeof(SearchForDevicesViewModel);
				RelativeSourceExtension val84 = new RelativeSourceExtension();
				BindingExtension val85 = new BindingExtension();
				BindingExtension val86 = new BindingExtension();
				TapGestureRecognizer val87 = new TapGestureRecognizer();
				DynamicResourceExtension val88 = new DynamicResourceExtension();
				DynamicResourceExtension val89 = new DynamicResourceExtension();
				BindingExtension val90 = new BindingExtension();
				BindingExtension val91 = new BindingExtension();
				DynamicResourceExtension val92 = new DynamicResourceExtension();
				Setter val93 = new Setter();
				DataTrigger val94 = new DataTrigger(typeof(Label));
				BindingExtension val95 = new BindingExtension();
				DynamicResourceExtension val96 = new DynamicResourceExtension();
				Setter val97 = new Setter();
				DataTrigger val98 = new DataTrigger(typeof(Label));
				Label val99 = new Label();
				string searchForDevices_Pin = Strings.SearchForDevices_Pin;
				BindingExtension val100 = new BindingExtension();
				PairingPill pairingPill = new PairingPill();
				BoxView val101 = new BoxView();
				string searchForDevices_PushToPair = Strings.SearchForDevices_PushToPair;
				BindingExtension val102 = new BindingExtension();
				PairingPill pairingPill2 = new PairingPill();
				BoxView val103 = new BoxView();
				string searchForDevices_ReadyToPair = Strings.SearchForDevices_ReadyToPair;
				BindingExtension val104 = new BindingExtension();
				BindingExtension val105 = new BindingExtension();
				PairingPill pairingPill3 = new PairingPill();
				FlexLayout val106 = new FlexLayout();
				BindingExtension val107 = new BindingExtension();
				DynamicResourceExtension val108 = new DynamicResourceExtension();
				BindingExtension val109 = new BindingExtension();
				DynamicResourceExtension val110 = new DynamicResourceExtension();
				Setter val111 = new Setter();
				DataTrigger val112 = new DataTrigger(typeof(App.Common.Pages.Pairing.SearchForDevices.SignalView.SignalView));
				BindingExtension val113 = new BindingExtension();
				DynamicResourceExtension val114 = new DynamicResourceExtension();
				Setter val115 = new Setter();
				DataTrigger val116 = new DataTrigger(typeof(App.Common.Pages.Pairing.SearchForDevices.SignalView.SignalView));
				App.Common.Pages.Pairing.SearchForDevices.SignalView.SignalView signalView = new App.Common.Pages.Pairing.SearchForDevices.SignalView.SignalView();
				DynamicResourceExtension val117 = new DynamicResourceExtension();
				DisabledColorExtension val118 = new DisabledColorExtension();
				BindingExtension val119 = new BindingExtension();
				string proSolid = FontAwesomeFontFamily.ProSolid900;
				string circleCheck = FontAwesomeGlyph.CircleCheck;
				FontImageSource val120 = new FontImageSource();
				SKImageView val121 = new SKImageView();
				Grid val122 = new Grid();
				Border val123 = new Border();
				NameScope val124 = new NameScope();
				NameScope.SetNameScope((BindableObject)(object)val123, (INameScope)(object)val124);
				((Element)val75).transientNamescope = (INameScope)(object)val124;
				((Element)val122).transientNamescope = (INameScope)(object)val124;
				((Element)val87).transientNamescope = (INameScope)(object)val124;
				((Element)val99).transientNamescope = (INameScope)(object)val124;
				((Element)val106).transientNamescope = (INameScope)(object)val124;
				((Element)pairingPill).transientNamescope = (INameScope)(object)val124;
				((Element)val101).transientNamescope = (INameScope)(object)val124;
				((Element)pairingPill2).transientNamescope = (INameScope)(object)val124;
				((Element)val103).transientNamescope = (INameScope)(object)val124;
				((Element)pairingPill3).transientNamescope = (INameScope)(object)val124;
				((Element)signalView).transientNamescope = (INameScope)(object)val124;
				((Element)val121).transientNamescope = (INameScope)(object)val124;
				((Element)val118).transientNamescope = (INameScope)(object)val124;
				((Element)val120).transientNamescope = (INameScope)(object)val124;
				val74.Key = "Surface";
				XamlServiceProvider val125 = new XamlServiceProvider();
				val125.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(51, 29)));
				DynamicResource val126 = ((IMarkupExtension<DynamicResource>)(object)val74).ProvideValue((IServiceProvider)val125);
				((IDynamicResourceHandler)val123).SetDynamicResource(VisualElement.BackgroundColorProperty, val126.Key);
				((BindableObject)val75).SetValue(RoundRectangle.CornerRadiusProperty, (object)new CornerRadius(24.0));
				((BindableObject)val123).SetValue(Border.StrokeShapeProperty, (object)val75);
				val79.Value = "True";
				val76.Path = "IsSelected";
				val76.TypedBinding = (TypedBindingBase)(object)new TypedBinding<BleScanResultModel, bool>((Func<BleScanResultModel, ValueTuple<bool, bool>>)([CompilerGenerated] (BleScanResultModel? P_0) => (P_0 != null) ? new ValueTuple<bool, bool>(P_0.IsSelected, true) : default(ValueTuple<bool, bool>)), (Action<BleScanResultModel, bool>)([CompilerGenerated] (BleScanResultModel? P_0, bool P_1) =>
				{
					if (P_0 != null)
					{
						P_0.IsSelected = P_1;
					}
				}), new Tuple<Func<BleScanResultModel, object>, string>[1]
				{
					new Tuple<Func<BleScanResultModel, object>, string>((Func<BleScanResultModel, object>)([CompilerGenerated] (BleScanResultModel? P_0) => P_0), "IsSelected")
				});
				((BindingBase)val76.TypedBinding).Mode = val76.Mode;
				val76.TypedBinding.Converter = val76.Converter;
				val76.TypedBinding.ConverterParameter = val76.ConverterParameter;
				((BindingBase)val76.TypedBinding).StringFormat = val76.StringFormat;
				val76.TypedBinding.Source = val76.Source;
				val76.TypedBinding.UpdateSourceEventName = val76.UpdateSourceEventName;
				((BindingBase)val76.TypedBinding).FallbackValue = val76.FallbackValue;
				((BindingBase)val76.TypedBinding).TargetNullValue = val76.TargetNullValue;
				BindingBase typedBinding4 = (BindingBase)(object)val76.TypedBinding;
				val79.Binding = typedBinding4;
				val78.Property = VisualElement.BackgroundColorProperty;
				val77.Key = "TertiaryContainer";
				XamlServiceProvider val127 = new XamlServiceProvider();
				val127.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(59, 68)));
				DynamicResource value = ((IMarkupExtension<DynamicResource>)(object)val77).ProvideValue((IServiceProvider)val127);
				val78.Value = value;
				((global::System.Collections.Generic.ICollection<Setter>)val79.Setters).Add(val78);
				((global::System.Collections.Generic.ICollection<TriggerBase>)((BindableObject)val123).GetValue(VisualElement.TriggersProperty)).Add((TriggerBase)(object)val79);
				val83.Value = "False";
				val80.Path = "IsSelected";
				val80.TypedBinding = (TypedBindingBase)(object)new TypedBinding<BleScanResultModel, bool>((Func<BleScanResultModel, ValueTuple<bool, bool>>)([CompilerGenerated] (BleScanResultModel? P_0) => (P_0 != null) ? new ValueTuple<bool, bool>(P_0.IsSelected, true) : default(ValueTuple<bool, bool>)), (Action<BleScanResultModel, bool>)([CompilerGenerated] (BleScanResultModel? P_0, bool P_1) =>
				{
					if (P_0 != null)
					{
						P_0.IsSelected = P_1;
					}
				}), new Tuple<Func<BleScanResultModel, object>, string>[1]
				{
					new Tuple<Func<BleScanResultModel, object>, string>((Func<BleScanResultModel, object>)([CompilerGenerated] (BleScanResultModel? P_0) => P_0), "IsSelected")
				});
				((BindingBase)val80.TypedBinding).Mode = val80.Mode;
				val80.TypedBinding.Converter = val80.Converter;
				val80.TypedBinding.ConverterParameter = val80.ConverterParameter;
				((BindingBase)val80.TypedBinding).StringFormat = val80.StringFormat;
				val80.TypedBinding.Source = val80.Source;
				val80.TypedBinding.UpdateSourceEventName = val80.UpdateSourceEventName;
				((BindingBase)val80.TypedBinding).FallbackValue = val80.FallbackValue;
				((BindingBase)val80.TypedBinding).TargetNullValue = val80.TargetNullValue;
				BindingBase typedBinding5 = (BindingBase)(object)val80.TypedBinding;
				val83.Binding = typedBinding5;
				val82.Property = VisualElement.BackgroundColorProperty;
				val81.Key = "Surface";
				XamlServiceProvider val128 = new XamlServiceProvider();
				val128.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(64, 68)));
				DynamicResource value2 = ((IMarkupExtension<DynamicResource>)(object)val81).ProvideValue((IServiceProvider)val128);
				val82.Value = value2;
				((global::System.Collections.Generic.ICollection<Setter>)val83.Setters).Add(val82);
				((global::System.Collections.Generic.ICollection<TriggerBase>)((BindableObject)val123).GetValue(VisualElement.TriggersProperty)).Add((TriggerBase)(object)val83);
				((BindableObject)val122).SetValue(Grid.RowDefinitionsProperty, (object)new RowDefinitionCollection((RowDefinition[])(object)new RowDefinition[2]
				{
					new RowDefinition(new GridLength(56.0)),
					new RowDefinition(new GridLength(36.0))
				}));
				((BindableObject)val122).SetValue(Grid.ColumnDefinitionsProperty, (object)new ColumnDefinitionCollection((ColumnDefinition[])(object)new ColumnDefinition[3]
				{
					new ColumnDefinition(GridLength.Star),
					new ColumnDefinition(new GridLength(48.0)),
					new ColumnDefinition(new GridLength(48.0))
				}));
				((BindableObject)val122).SetValue(Grid.RowSpacingProperty, (object)4.0);
				((BindableObject)val122).SetValue(Grid.ColumnSpacingProperty, (object)4.0);
				((BindableObject)val122).SetValue(Layout.PaddingProperty, (object)new Thickness(8.0));
				val85.Path = "SearchResultSelectedCommand";
				val84.AncestorType = typeFromHandle5;
				RelativeBindingSource source3 = ((IMarkupExtension<RelativeBindingSource>)(object)val84).ProvideValue((IServiceProvider)null);
				val85.Source = source3;
				XamlServiceProvider val129 = new XamlServiceProvider();
				global::System.Type typeFromHandle6 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver val130 = new XmlNamespaceResolver();
				val130.Add("", "http://schemas.microsoft.com/dotnet/2021/maui");
				val130.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				val130.Add("ui", "http://lci1.com/schemas/ui");
				val130.Add("skia", "clr-namespace:SkiaSharp.Extended.UI.Controls;assembly=SkiaSharp.Extended.UI");
				val130.Add("disabledColor", "clr-namespace:IDS.UI.MarkupExtensions.DisabledColor;assembly=ids.ui");
				val130.Add("toolkit", "http://schemas.microsoft.com/dotnet/2022/maui/toolkit");
				val130.Add("dualActionPage", "clr-namespace:App.Common.DualActionPage;assembly=App.Common");
				val130.Add("searchForDevices", "clr-namespace:App.Common.Pages.Pairing.SearchForDevices");
				val130.Add("signalView", "clr-namespace:App.Common.Pages.Pairing.SearchForDevices.SignalView");
				val130.Add("resources", "clr-namespace:App.Common.Pages.Pairing.Resources");
				val129.Add(typeFromHandle6, (object)new XamlTypeResolver((IXmlNamespaceResolver)val130, typeof(<InitializeComponent>_anonXamlCDataTemplate_0).Assembly));
				BindingBase val131 = ((IMarkupExtension<BindingBase>)(object)val85).ProvideValue((IServiceProvider)val129);
				((BindableObject)val87).SetBinding(TapGestureRecognizer.CommandProperty, val131);
				val86.Path = ".";
				val86.TypedBinding = (TypedBindingBase)(object)new TypedBinding<BleScanResultModel, BleScanResultModel>((Func<BleScanResultModel, ValueTuple<BleScanResultModel, bool>>)([CompilerGenerated] (BleScanResultModel? P_0) => new ValueTuple<BleScanResultModel, bool>(P_0, true)), (Action<BleScanResultModel, BleScanResultModel>)null, (Tuple<Func<BleScanResultModel, object>, string>[])null);
				((BindingBase)val86.TypedBinding).Mode = val86.Mode;
				val86.TypedBinding.Converter = val86.Converter;
				val86.TypedBinding.ConverterParameter = val86.ConverterParameter;
				((BindingBase)val86.TypedBinding).StringFormat = val86.StringFormat;
				val86.TypedBinding.Source = val86.Source;
				val86.TypedBinding.UpdateSourceEventName = val86.UpdateSourceEventName;
				((BindingBase)val86.TypedBinding).FallbackValue = val86.FallbackValue;
				((BindingBase)val86.TypedBinding).TargetNullValue = val86.TargetNullValue;
				BindingBase typedBinding6 = (BindingBase)(object)val86.TypedBinding;
				((BindableObject)val87).SetBinding(TapGestureRecognizer.CommandParameterProperty, typedBinding6);
				((global::System.Collections.Generic.ICollection<IGestureRecognizer>)((View)val122).GestureRecognizers).Add((IGestureRecognizer)(object)val87);
				((BindableObject)val99).SetValue(Grid.RowProperty, (object)0);
				val88.Key = "Body4_Label_Black";
				XamlServiceProvider val132 = new XamlServiceProvider();
				val132.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(80, 36)));
				DynamicResource val133 = ((IMarkupExtension<DynamicResource>)(object)val88).ProvideValue((IServiceProvider)val132);
				((IDynamicResourceHandler)val99).SetDynamicResource(VisualElement.StyleProperty, val133.Key);
				((BindableObject)val99).SetValue(VisualElement.HeightRequestProperty, (object)56.0);
				((BindableObject)val99).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Center);
				((BindableObject)val99).SetValue(Label.LineBreakModeProperty, (object)(LineBreakMode)2);
				((BindableObject)val99).SetValue(Label.MaxLinesProperty, (object)2);
				((BindableObject)val99).SetValue(Label.VerticalTextAlignmentProperty, (object)(TextAlignment)1);
				val89.Key = "OnSurface";
				XamlServiceProvider val134 = new XamlServiceProvider();
				val134.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(86, 36)));
				DynamicResource val135 = ((IMarkupExtension<DynamicResource>)(object)val89).ProvideValue((IServiceProvider)val134);
				((IDynamicResourceHandler)val99).SetDynamicResource(Label.TextColorProperty, val135.Key);
				val90.Mode = (BindingMode)2;
				val90.Path = "Name";
				val90.TypedBinding = (TypedBindingBase)(object)new TypedBinding<BleScanResultModel, string>((Func<BleScanResultModel, ValueTuple<string, bool>>)([CompilerGenerated] (BleScanResultModel? P_0) => (P_0 != null) ? new ValueTuple<string, bool>(P_0.Name, true) : default(ValueTuple<string, bool>)), (Action<BleScanResultModel, string>)null, new Tuple<Func<BleScanResultModel, object>, string>[1]
				{
					new Tuple<Func<BleScanResultModel, object>, string>((Func<BleScanResultModel, object>)([CompilerGenerated] (BleScanResultModel? P_0) => P_0), "Name")
				});
				((BindingBase)val90.TypedBinding).Mode = val90.Mode;
				val90.TypedBinding.Converter = val90.Converter;
				val90.TypedBinding.ConverterParameter = val90.ConverterParameter;
				((BindingBase)val90.TypedBinding).StringFormat = val90.StringFormat;
				val90.TypedBinding.Source = val90.Source;
				val90.TypedBinding.UpdateSourceEventName = val90.UpdateSourceEventName;
				((BindingBase)val90.TypedBinding).FallbackValue = val90.FallbackValue;
				((BindingBase)val90.TypedBinding).TargetNullValue = val90.TargetNullValue;
				BindingBase typedBinding7 = (BindingBase)(object)val90.TypedBinding;
				((BindableObject)val99).SetBinding(Label.TextProperty, typedBinding7);
				val94.Value = "True";
				val91.Path = "IsSelected";
				val91.TypedBinding = (TypedBindingBase)(object)new TypedBinding<BleScanResultModel, bool>((Func<BleScanResultModel, ValueTuple<bool, bool>>)([CompilerGenerated] (BleScanResultModel? P_0) => (P_0 != null) ? new ValueTuple<bool, bool>(P_0.IsSelected, true) : default(ValueTuple<bool, bool>)), (Action<BleScanResultModel, bool>)([CompilerGenerated] (BleScanResultModel? P_0, bool P_1) =>
				{
					if (P_0 != null)
					{
						P_0.IsSelected = P_1;
					}
				}), new Tuple<Func<BleScanResultModel, object>, string>[1]
				{
					new Tuple<Func<BleScanResultModel, object>, string>((Func<BleScanResultModel, object>)([CompilerGenerated] (BleScanResultModel? P_0) => P_0), "IsSelected")
				});
				((BindingBase)val91.TypedBinding).Mode = val91.Mode;
				val91.TypedBinding.Converter = val91.Converter;
				val91.TypedBinding.ConverterParameter = val91.ConverterParameter;
				((BindingBase)val91.TypedBinding).StringFormat = val91.StringFormat;
				val91.TypedBinding.Source = val91.Source;
				val91.TypedBinding.UpdateSourceEventName = val91.UpdateSourceEventName;
				((BindingBase)val91.TypedBinding).FallbackValue = val91.FallbackValue;
				((BindingBase)val91.TypedBinding).TargetNullValue = val91.TargetNullValue;
				BindingBase typedBinding8 = (BindingBase)(object)val91.TypedBinding;
				val94.Binding = typedBinding8;
				val93.Property = Label.TextColorProperty;
				val92.Key = "OnTertiaryContainer";
				XamlServiceProvider val136 = new XamlServiceProvider();
				val136.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(92, 70)));
				DynamicResource value3 = ((IMarkupExtension<DynamicResource>)(object)val92).ProvideValue((IServiceProvider)val136);
				val93.Value = value3;
				((global::System.Collections.Generic.ICollection<Setter>)val94.Setters).Add(val93);
				((global::System.Collections.Generic.ICollection<TriggerBase>)((BindableObject)val99).GetValue(VisualElement.TriggersProperty)).Add((TriggerBase)(object)val94);
				val98.Value = "False";
				val95.Path = "IsSelected";
				val95.TypedBinding = (TypedBindingBase)(object)new TypedBinding<BleScanResultModel, bool>((Func<BleScanResultModel, ValueTuple<bool, bool>>)([CompilerGenerated] (BleScanResultModel? P_0) => (P_0 != null) ? new ValueTuple<bool, bool>(P_0.IsSelected, true) : default(ValueTuple<bool, bool>)), (Action<BleScanResultModel, bool>)([CompilerGenerated] (BleScanResultModel? P_0, bool P_1) =>
				{
					if (P_0 != null)
					{
						P_0.IsSelected = P_1;
					}
				}), new Tuple<Func<BleScanResultModel, object>, string>[1]
				{
					new Tuple<Func<BleScanResultModel, object>, string>((Func<BleScanResultModel, object>)([CompilerGenerated] (BleScanResultModel? P_0) => P_0), "IsSelected")
				});
				((BindingBase)val95.TypedBinding).Mode = val95.Mode;
				val95.TypedBinding.Converter = val95.Converter;
				val95.TypedBinding.ConverterParameter = val95.ConverterParameter;
				((BindingBase)val95.TypedBinding).StringFormat = val95.StringFormat;
				val95.TypedBinding.Source = val95.Source;
				val95.TypedBinding.UpdateSourceEventName = val95.UpdateSourceEventName;
				((BindingBase)val95.TypedBinding).FallbackValue = val95.FallbackValue;
				((BindingBase)val95.TypedBinding).TargetNullValue = val95.TargetNullValue;
				BindingBase typedBinding9 = (BindingBase)(object)val95.TypedBinding;
				val98.Binding = typedBinding9;
				val97.Property = Label.TextColorProperty;
				val96.Key = "OnSurface";
				XamlServiceProvider val137 = new XamlServiceProvider();
				val137.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(97, 70)));
				DynamicResource value4 = ((IMarkupExtension<DynamicResource>)(object)val96).ProvideValue((IServiceProvider)val137);
				val97.Value = value4;
				((global::System.Collections.Generic.ICollection<Setter>)val98.Setters).Add(val97);
				((global::System.Collections.Generic.ICollection<TriggerBase>)((BindableObject)val99).GetValue(VisualElement.TriggersProperty)).Add((TriggerBase)(object)val98);
				((global::System.Collections.Generic.ICollection<IView>)((Layout)val122).Children).Add((IView)(object)val99);
				((BindableObject)val106).SetValue(Grid.RowProperty, (object)1);
				((BindableObject)val106).SetValue(Grid.ColumnProperty, (object)0);
				((BindableObject)val106).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.Start);
				((BindableObject)val106).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.End);
				((BindableObject)val106).SetValue(FlexLayout.JustifyContentProperty, (object)(FlexJustify)3);
				((BindableObject)val106).SetValue(FlexLayout.AlignItemsProperty, (object)(FlexAlignItems)2);
				((BindableObject)val106).SetValue(FlexLayout.AlignContentProperty, (object)(FlexAlignContent)5);
				((BindableObject)val106).SetValue(FlexLayout.WrapProperty, (object)(FlexWrap)0);
				((BindableObject)pairingPill).SetValue(PairingPill.TextProperty, (object)searchForDevices_Pin);
				val100.Path = "IsPinPairable";
				val100.TypedBinding = (TypedBindingBase)(object)new TypedBinding<BleScanResultModel, bool>((Func<BleScanResultModel, ValueTuple<bool, bool>>)([CompilerGenerated] (BleScanResultModel? P_0) => (P_0 != null) ? new ValueTuple<bool, bool>(P_0.IsPinPairable, true) : default(ValueTuple<bool, bool>)), (Action<BleScanResultModel, bool>)([CompilerGenerated] (BleScanResultModel? P_0, bool P_1) =>
				{
					if (P_0 != null)
					{
						P_0.IsPinPairable = P_1;
					}
				}), new Tuple<Func<BleScanResultModel, object>, string>[1]
				{
					new Tuple<Func<BleScanResultModel, object>, string>((Func<BleScanResultModel, object>)([CompilerGenerated] (BleScanResultModel? P_0) => P_0), "IsPinPairable")
				});
				((BindingBase)val100.TypedBinding).Mode = val100.Mode;
				val100.TypedBinding.Converter = val100.Converter;
				val100.TypedBinding.ConverterParameter = val100.ConverterParameter;
				((BindingBase)val100.TypedBinding).StringFormat = val100.StringFormat;
				val100.TypedBinding.Source = val100.Source;
				val100.TypedBinding.UpdateSourceEventName = val100.UpdateSourceEventName;
				((BindingBase)val100.TypedBinding).FallbackValue = val100.FallbackValue;
				((BindingBase)val100.TypedBinding).TargetNullValue = val100.TargetNullValue;
				BindingBase typedBinding10 = (BindingBase)(object)val100.TypedBinding;
				((BindableObject)pairingPill).SetBinding(VisualElement.IsVisibleProperty, typedBinding10);
				((global::System.Collections.Generic.ICollection<IView>)((Layout)val106).Children).Add((IView)(object)pairingPill);
				((BindableObject)val101).SetValue(VisualElement.WidthRequestProperty, (object)2.0);
				((BindableObject)val101).SetValue(VisualElement.HeightRequestProperty, (object)2.0);
				((BindableObject)val101).SetValue(VisualElement.BackgroundColorProperty, (object)Colors.Transparent);
				((global::System.Collections.Generic.ICollection<IView>)((Layout)val106).Children).Add((IView)(object)val101);
				((BindableObject)pairingPill2).SetValue(PairingPill.TextProperty, (object)searchForDevices_PushToPair);
				val102.Path = "IsPushToPair";
				val102.TypedBinding = (TypedBindingBase)(object)new TypedBinding<BleScanResultModel, bool>((Func<BleScanResultModel, ValueTuple<bool, bool>>)([CompilerGenerated] (BleScanResultModel? P_0) => (P_0 != null) ? new ValueTuple<bool, bool>(P_0.IsPushToPair, true) : default(ValueTuple<bool, bool>)), (Action<BleScanResultModel, bool>)([CompilerGenerated] (BleScanResultModel? P_0, bool P_1) =>
				{
					if (P_0 != null)
					{
						P_0.IsPushToPair = P_1;
					}
				}), new Tuple<Func<BleScanResultModel, object>, string>[1]
				{
					new Tuple<Func<BleScanResultModel, object>, string>((Func<BleScanResultModel, object>)([CompilerGenerated] (BleScanResultModel? P_0) => P_0), "IsPushToPair")
				});
				((BindingBase)val102.TypedBinding).Mode = val102.Mode;
				val102.TypedBinding.Converter = val102.Converter;
				val102.TypedBinding.ConverterParameter = val102.ConverterParameter;
				((BindingBase)val102.TypedBinding).StringFormat = val102.StringFormat;
				val102.TypedBinding.Source = val102.Source;
				val102.TypedBinding.UpdateSourceEventName = val102.UpdateSourceEventName;
				((BindingBase)val102.TypedBinding).FallbackValue = val102.FallbackValue;
				((BindingBase)val102.TypedBinding).TargetNullValue = val102.TargetNullValue;
				BindingBase typedBinding11 = (BindingBase)(object)val102.TypedBinding;
				((BindableObject)pairingPill2).SetBinding(VisualElement.IsVisibleProperty, typedBinding11);
				((global::System.Collections.Generic.ICollection<IView>)((Layout)val106).Children).Add((IView)(object)pairingPill2);
				((BindableObject)val103).SetValue(VisualElement.WidthRequestProperty, (object)2.0);
				((BindableObject)val103).SetValue(VisualElement.HeightRequestProperty, (object)2.0);
				((BindableObject)val103).SetValue(VisualElement.BackgroundColorProperty, (object)Colors.Transparent);
				((global::System.Collections.Generic.ICollection<IView>)((Layout)val106).Children).Add((IView)(object)val103);
				((BindableObject)pairingPill3).SetValue(PairingPill.TextProperty, (object)searchForDevices_ReadyToPair);
				val104.Path = "IsPushToPair";
				val104.TypedBinding = (TypedBindingBase)(object)new TypedBinding<BleScanResultModel, bool>((Func<BleScanResultModel, ValueTuple<bool, bool>>)([CompilerGenerated] (BleScanResultModel? P_0) => (P_0 != null) ? new ValueTuple<bool, bool>(P_0.IsPushToPair, true) : default(ValueTuple<bool, bool>)), (Action<BleScanResultModel, bool>)([CompilerGenerated] (BleScanResultModel? P_0, bool P_1) =>
				{
					if (P_0 != null)
					{
						P_0.IsPushToPair = P_1;
					}
				}), new Tuple<Func<BleScanResultModel, object>, string>[1]
				{
					new Tuple<Func<BleScanResultModel, object>, string>((Func<BleScanResultModel, object>)([CompilerGenerated] (BleScanResultModel? P_0) => P_0), "IsPushToPair")
				});
				((BindingBase)val104.TypedBinding).Mode = val104.Mode;
				val104.TypedBinding.Converter = val104.Converter;
				val104.TypedBinding.ConverterParameter = val104.ConverterParameter;
				((BindingBase)val104.TypedBinding).StringFormat = val104.StringFormat;
				val104.TypedBinding.Source = val104.Source;
				val104.TypedBinding.UpdateSourceEventName = val104.UpdateSourceEventName;
				((BindingBase)val104.TypedBinding).FallbackValue = val104.FallbackValue;
				((BindingBase)val104.TypedBinding).TargetNullValue = val104.TargetNullValue;
				BindingBase typedBinding12 = (BindingBase)(object)val104.TypedBinding;
				((BindableObject)pairingPill3).SetBinding(VisualElement.IsVisibleProperty, typedBinding12);
				val105.Path = "IsPairingActive";
				val105.TypedBinding = (TypedBindingBase)(object)new TypedBinding<BleScanResultModel, bool>((Func<BleScanResultModel, ValueTuple<bool, bool>>)([CompilerGenerated] (BleScanResultModel? P_0) => (P_0 != null) ? new ValueTuple<bool, bool>(P_0.IsPairingActive, true) : default(ValueTuple<bool, bool>)), (Action<BleScanResultModel, bool>)([CompilerGenerated] (BleScanResultModel? P_0, bool P_1) =>
				{
					if (P_0 != null)
					{
						P_0.IsPairingActive = P_1;
					}
				}), new Tuple<Func<BleScanResultModel, object>, string>[1]
				{
					new Tuple<Func<BleScanResultModel, object>, string>((Func<BleScanResultModel, object>)([CompilerGenerated] (BleScanResultModel? P_0) => P_0), "IsPairingActive")
				});
				((BindingBase)val105.TypedBinding).Mode = val105.Mode;
				val105.TypedBinding.Converter = val105.Converter;
				val105.TypedBinding.ConverterParameter = val105.ConverterParameter;
				((BindingBase)val105.TypedBinding).StringFormat = val105.StringFormat;
				val105.TypedBinding.Source = val105.Source;
				val105.TypedBinding.UpdateSourceEventName = val105.UpdateSourceEventName;
				((BindingBase)val105.TypedBinding).FallbackValue = val105.FallbackValue;
				((BindingBase)val105.TypedBinding).TargetNullValue = val105.TargetNullValue;
				BindingBase typedBinding13 = (BindingBase)(object)val105.TypedBinding;
				((BindableObject)pairingPill3).SetBinding(VisualElement.IsEnabledProperty, typedBinding13);
				((global::System.Collections.Generic.ICollection<IView>)((Layout)val106).Children).Add((IView)(object)pairingPill3);
				((global::System.Collections.Generic.ICollection<IView>)((Layout)val122).Children).Add((IView)(object)val106);
				((BindableObject)signalView).SetValue(Grid.RowProperty, (object)0);
				((BindableObject)signalView).SetValue(Grid.RowSpanProperty, (object)2);
				((BindableObject)signalView).SetValue(Grid.ColumnProperty, (object)1);
				((BindableObject)signalView).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.End);
				((BindableObject)signalView).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Center);
				((BindableObject)signalView).SetValue(VisualElement.WidthRequestProperty, (object)36.0);
				((BindableObject)signalView).SetValue(VisualElement.HeightRequestProperty, (object)36.0);
				val107.Path = "Rssi";
				val107.TypedBinding = (TypedBindingBase)(object)new TypedBinding<BleScanResultModel, int>((Func<BleScanResultModel, ValueTuple<int, bool>>)([CompilerGenerated] (BleScanResultModel? P_0) => (P_0 != null) ? new ValueTuple<int, bool>(P_0.Rssi, true) : default(ValueTuple<int, bool>)), (Action<BleScanResultModel, int>)([CompilerGenerated] (BleScanResultModel? P_0, int P_1) =>
				{
					if (P_0 != null)
					{
						P_0.Rssi = P_1;
					}
				}), new Tuple<Func<BleScanResultModel, object>, string>[1]
				{
					new Tuple<Func<BleScanResultModel, object>, string>((Func<BleScanResultModel, object>)([CompilerGenerated] (BleScanResultModel? P_0) => P_0), "Rssi")
				});
				((BindingBase)val107.TypedBinding).Mode = val107.Mode;
				val107.TypedBinding.Converter = val107.Converter;
				val107.TypedBinding.ConverterParameter = val107.ConverterParameter;
				((BindingBase)val107.TypedBinding).StringFormat = val107.StringFormat;
				val107.TypedBinding.Source = val107.Source;
				val107.TypedBinding.UpdateSourceEventName = val107.UpdateSourceEventName;
				((BindingBase)val107.TypedBinding).FallbackValue = val107.FallbackValue;
				((BindingBase)val107.TypedBinding).TargetNullValue = val107.TargetNullValue;
				BindingBase typedBinding14 = (BindingBase)(object)val107.TypedBinding;
				((BindableObject)signalView).SetBinding(App.Common.Pages.Pairing.SearchForDevices.SignalView.SignalView.SignalProperty, typedBinding14);
				val108.Key = "OnSurface";
				XamlServiceProvider val138 = new XamlServiceProvider();
				val138.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(127, 52)));
				DynamicResource val139 = ((IMarkupExtension<DynamicResource>)(object)val108).ProvideValue((IServiceProvider)val138);
				((IDynamicResourceHandler)signalView).SetDynamicResource(App.Common.Pages.Pairing.SearchForDevices.SignalView.SignalView.TintProperty, val139.Key);
				val112.Value = "True";
				val109.Path = "IsSelected";
				val109.TypedBinding = (TypedBindingBase)(object)new TypedBinding<BleScanResultModel, bool>((Func<BleScanResultModel, ValueTuple<bool, bool>>)([CompilerGenerated] (BleScanResultModel? P_0) => (P_0 != null) ? new ValueTuple<bool, bool>(P_0.IsSelected, true) : default(ValueTuple<bool, bool>)), (Action<BleScanResultModel, bool>)([CompilerGenerated] (BleScanResultModel? P_0, bool P_1) =>
				{
					if (P_0 != null)
					{
						P_0.IsSelected = P_1;
					}
				}), new Tuple<Func<BleScanResultModel, object>, string>[1]
				{
					new Tuple<Func<BleScanResultModel, object>, string>((Func<BleScanResultModel, object>)([CompilerGenerated] (BleScanResultModel? P_0) => P_0), "IsSelected")
				});
				((BindingBase)val109.TypedBinding).Mode = val109.Mode;
				val109.TypedBinding.Converter = val109.Converter;
				val109.TypedBinding.ConverterParameter = val109.ConverterParameter;
				((BindingBase)val109.TypedBinding).StringFormat = val109.StringFormat;
				val109.TypedBinding.Source = val109.Source;
				val109.TypedBinding.UpdateSourceEventName = val109.UpdateSourceEventName;
				((BindingBase)val109.TypedBinding).FallbackValue = val109.FallbackValue;
				((BindingBase)val109.TypedBinding).TargetNullValue = val109.TargetNullValue;
				BindingBase typedBinding15 = (BindingBase)(object)val109.TypedBinding;
				val112.Binding = typedBinding15;
				val111.Property = App.Common.Pages.Pairing.SearchForDevices.SignalView.SignalView.TintProperty;
				val110.Key = "OnTertiaryContainer";
				XamlServiceProvider val140 = new XamlServiceProvider();
				val140.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(132, 65)));
				DynamicResource value5 = ((IMarkupExtension<DynamicResource>)(object)val110).ProvideValue((IServiceProvider)val140);
				val111.Value = value5;
				((global::System.Collections.Generic.ICollection<Setter>)val112.Setters).Add(val111);
				((global::System.Collections.Generic.ICollection<TriggerBase>)((BindableObject)signalView).GetValue(VisualElement.TriggersProperty)).Add((TriggerBase)(object)val112);
				val116.Value = "False";
				val113.Path = "IsSelected";
				val113.TypedBinding = (TypedBindingBase)(object)new TypedBinding<BleScanResultModel, bool>((Func<BleScanResultModel, ValueTuple<bool, bool>>)([CompilerGenerated] (BleScanResultModel? P_0) => (P_0 != null) ? new ValueTuple<bool, bool>(P_0.IsSelected, true) : default(ValueTuple<bool, bool>)), (Action<BleScanResultModel, bool>)([CompilerGenerated] (BleScanResultModel? P_0, bool P_1) =>
				{
					if (P_0 != null)
					{
						P_0.IsSelected = P_1;
					}
				}), new Tuple<Func<BleScanResultModel, object>, string>[1]
				{
					new Tuple<Func<BleScanResultModel, object>, string>((Func<BleScanResultModel, object>)([CompilerGenerated] (BleScanResultModel? P_0) => P_0), "IsSelected")
				});
				((BindingBase)val113.TypedBinding).Mode = val113.Mode;
				val113.TypedBinding.Converter = val113.Converter;
				val113.TypedBinding.ConverterParameter = val113.ConverterParameter;
				((BindingBase)val113.TypedBinding).StringFormat = val113.StringFormat;
				val113.TypedBinding.Source = val113.Source;
				val113.TypedBinding.UpdateSourceEventName = val113.UpdateSourceEventName;
				((BindingBase)val113.TypedBinding).FallbackValue = val113.FallbackValue;
				((BindingBase)val113.TypedBinding).TargetNullValue = val113.TargetNullValue;
				BindingBase typedBinding16 = (BindingBase)(object)val113.TypedBinding;
				val116.Binding = typedBinding16;
				val115.Property = App.Common.Pages.Pairing.SearchForDevices.SignalView.SignalView.TintProperty;
				val114.Key = "OnSurface";
				XamlServiceProvider val141 = new XamlServiceProvider();
				val141.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(137, 65)));
				DynamicResource value6 = ((IMarkupExtension<DynamicResource>)(object)val114).ProvideValue((IServiceProvider)val141);
				val115.Value = value6;
				((global::System.Collections.Generic.ICollection<Setter>)val116.Setters).Add(val115);
				((global::System.Collections.Generic.ICollection<TriggerBase>)((BindableObject)signalView).GetValue(VisualElement.TriggersProperty)).Add((TriggerBase)(object)val116);
				((global::System.Collections.Generic.ICollection<IView>)((Layout)val122).Children).Add((IView)(object)signalView);
				((BindableObject)val121).SetValue(Grid.RowProperty, (object)0);
				((BindableObject)val121).SetValue(Grid.RowSpanProperty, (object)2);
				((BindableObject)val121).SetValue(Grid.ColumnProperty, (object)2);
				((BindableObject)val121).SetValue(VisualElement.WidthRequestProperty, (object)36.0);
				((BindableObject)val121).SetValue(VisualElement.HeightRequestProperty, (object)36.0);
				((BindableObject)val121).SetValue(View.HorizontalOptionsProperty, (object)LayoutOptions.End);
				((BindableObject)val121).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Center);
				val117.Key = "Tertiary";
				XamlServiceProvider val142 = new XamlServiceProvider();
				val142.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(148, 45)));
				DynamicResource val143 = ((IMarkupExtension<DynamicResource>)(object)val117).ProvideValue((IServiceProvider)val142);
				((IDynamicResourceHandler)val118).SetDynamicResource(BindingProxyExtension<DisabledColorBindingProxy, Color>.ValueProperty, val143.Key);
				XamlServiceProvider val144 = new XamlServiceProvider();
				global::System.Type typeFromHandle7 = typeof(IProvideValueTarget);
				int num;
				object[] array2 = new object[(num = parentValues.Length) + 3];
				global::System.Array.Copy((global::System.Array)parentValues, 0, (global::System.Array)array2, 3, num);
				array2[0] = val121;
				array2[1] = val122;
				array2[2] = val123;
				SimpleValueTargetProvider val145 = new SimpleValueTargetProvider(array2, (object)SKImageView.TintProperty, (INameScope[])(object)new NameScope[7] { val124, val124, val124, val124, val72, val35, val29 }, (object)root);
				object obj = (object)val145;
				val144.Add(typeFromHandle7, (object)val145);
				val144.Add(typeof(IReferenceProvider), obj);
				val144.Add(typeof(IRootObjectProvider), obj);
				global::System.Type typeFromHandle8 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver val146 = new XmlNamespaceResolver();
				val146.Add("", "http://schemas.microsoft.com/dotnet/2021/maui");
				val146.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				val146.Add("ui", "http://lci1.com/schemas/ui");
				val146.Add("skia", "clr-namespace:SkiaSharp.Extended.UI.Controls;assembly=SkiaSharp.Extended.UI");
				val146.Add("disabledColor", "clr-namespace:IDS.UI.MarkupExtensions.DisabledColor;assembly=ids.ui");
				val146.Add("toolkit", "http://schemas.microsoft.com/dotnet/2022/maui/toolkit");
				val146.Add("dualActionPage", "clr-namespace:App.Common.DualActionPage;assembly=App.Common");
				val146.Add("searchForDevices", "clr-namespace:App.Common.Pages.Pairing.SearchForDevices");
				val146.Add("signalView", "clr-namespace:App.Common.Pages.Pairing.SearchForDevices.SignalView");
				val146.Add("resources", "clr-namespace:App.Common.Pages.Pairing.Resources");
				val144.Add(typeFromHandle8, (object)new XamlTypeResolver((IXmlNamespaceResolver)val146, typeof(<InitializeComponent>_anonXamlCDataTemplate_0).Assembly));
				val144.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(148, 45)));
				BindingBase val147 = ((IMarkupExtension<BindingBase>)(object)val118).ProvideValue((IServiceProvider)val144);
				((BindableObject)val121).SetBinding(SKImageView.TintProperty, val147);
				val119.Path = "IsSelected";
				val119.TypedBinding = (TypedBindingBase)(object)new TypedBinding<BleScanResultModel, bool>((Func<BleScanResultModel, ValueTuple<bool, bool>>)([CompilerGenerated] (BleScanResultModel? P_0) => (P_0 != null) ? new ValueTuple<bool, bool>(P_0.IsSelected, true) : default(ValueTuple<bool, bool>)), (Action<BleScanResultModel, bool>)([CompilerGenerated] (BleScanResultModel? P_0, bool P_1) =>
				{
					if (P_0 != null)
					{
						P_0.IsSelected = P_1;
					}
				}), new Tuple<Func<BleScanResultModel, object>, string>[1]
				{
					new Tuple<Func<BleScanResultModel, object>, string>((Func<BleScanResultModel, object>)([CompilerGenerated] (BleScanResultModel? P_0) => P_0), "IsSelected")
				});
				((BindingBase)val119.TypedBinding).Mode = val119.Mode;
				val119.TypedBinding.Converter = val119.Converter;
				val119.TypedBinding.ConverterParameter = val119.ConverterParameter;
				((BindingBase)val119.TypedBinding).StringFormat = val119.StringFormat;
				val119.TypedBinding.Source = val119.Source;
				val119.TypedBinding.UpdateSourceEventName = val119.UpdateSourceEventName;
				((BindingBase)val119.TypedBinding).FallbackValue = val119.FallbackValue;
				((BindingBase)val119.TypedBinding).TargetNullValue = val119.TargetNullValue;
				BindingBase typedBinding17 = (BindingBase)(object)val119.TypedBinding;
				((BindableObject)val121).SetBinding(VisualElement.IsEnabledProperty, typedBinding17);
				((BindableObject)val120).SetValue(FontImageSource.FontFamilyProperty, (object)proSolid);
				((BindableObject)val120).SetValue(FontImageSource.GlyphProperty, (object)circleCheck);
				((BindableObject)val121).SetValue(SKImageView.ImageSourceProperty, (object)val120);
				((global::System.Collections.Generic.ICollection<IView>)((Layout)val122).Children).Add((IView)(object)val121);
				((BindableObject)val123).SetValue(Border.ContentProperty, (object)val122);
				return val123;
			};
			((BindableObject)val17).SetValue(RadioButtonList.ItemTemplateProperty, (object)val16);
			((BindableObject)searchForDevicesPage).SetValue(DualActionPage.ContentProperty, (object)val17);
		}
	}
	public abstract class SearchForDevicesViewModel : DualActionViewModel
	{
		[CompilerGenerated]
		private sealed class <>c__DisplayClass23_0
		{
			[StructLayout((LayoutKind)3)]
			private struct <<Search>b__0>d : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncTaskMethodBuilder <>t__builder;

				public <>c__DisplayClass23_0 <>4__this;

				private TaskAwaiter <>u__1;

				private void MoveNext()
				{
					//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
					//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
					//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
					//IL_0087: Unknown result type (might be due to invalid IL or missing references)
					//IL_0091: Unknown result type (might be due to invalid IL or missing references)
					//IL_0096: Unknown result type (might be due to invalid IL or missing references)
					//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
					//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					<>c__DisplayClass23_0 CS$<>8__locals7 = <>4__this;
					try
					{
						try
						{
							if (num != 0)
							{
								goto IL_00e6;
							}
							TaskAwaiter val = <>u__1;
							<>u__1 = default(TaskAwaiter);
							num = (<>1__state = -1);
							goto IL_00df;
							IL_00df:
							((TaskAwaiter)(ref val)).GetResult();
							goto IL_00e6;
							IL_00e6:
							if (!((CancellationToken)(ref CS$<>8__locals7.cancellationToken)).IsCancellationRequested)
							{
								CS$<>8__locals7.<>4__this._bleScannerService.Stop();
								CS$<>8__locals7.<>4__this._bleScannerService.Start(false);
								val = CS$<>8__locals7.<>4__this._bleScannerService.TryGetDevicesAsync<IBleScanResult>((Action<BleScanResultOperation, IBleScanResult>)delegate(BleScanResultOperation _, IBleScanResult bleScanResult)
								{
									BleScanResultModel bleScanResultModel = new BleScanResultModel(bleScanResult);
									if (CS$<>8__locals7.<>4__this.IncludeScanResult(bleScanResultModel))
									{
										CS$<>8__locals7.<>4__this.AddOrUpdateScanResults(bleScanResultModel);
									}
								}, (Func<IBleScanResult, BleScannerCommandControl>)((IBleScanResult _) => (BleScannerCommandControl)1), CS$<>8__locals7.cancellationToken).GetAwaiter();
								if (!((TaskAwaiter)(ref val)).IsCompleted)
								{
									num = (<>1__state = 0);
									<>u__1 = val;
									((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <<Search>b__0>d>(ref val, ref this);
									return;
								}
								goto IL_00df;
							}
						}
						catch (global::System.Exception ex)
						{
							Console.WriteLine($"Error while searching for devices {ex}");
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

			public SearchForDevicesViewModel <>4__this;

			public CancellationToken cancellationToken;

			public Action<BleScanResultOperation, IBleScanResult> <>9__1;

			[AsyncStateMachine(typeof(<<Search>b__0>d))]
			internal global::System.Threading.Tasks.Task? <Search>b__0()
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				<<Search>b__0>d <<Search>b__0>d = default(<<Search>b__0>d);
				<<Search>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<Search>b__0>d.<>4__this = this;
				<<Search>b__0>d.<>1__state = -1;
				((AsyncTaskMethodBuilder)(ref <<Search>b__0>d.<>t__builder)).Start<<<Search>b__0>d>(ref <<Search>b__0>d);
				return ((AsyncTaskMethodBuilder)(ref <<Search>b__0>d.<>t__builder)).Task;
			}

			internal void <Search>b__1(BleScanResultOperation _, IBleScanResult bleScanResult)
			{
				BleScanResultModel bleScanResultModel = new BleScanResultModel(bleScanResult);
				if (<>4__this.IncludeScanResult(bleScanResultModel))
				{
					<>4__this.AddOrUpdateScanResults(bleScanResultModel);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <>c__DisplayClass24_0
		{
			[StructLayout((LayoutKind)3)]
			private struct <<AddOrUpdateScanResults>b__0>d : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncVoidMethodBuilder <>t__builder;

				public <>c__DisplayClass24_0 <>4__this;

				private TaskAwaiter <>u__1;

				private void MoveNext()
				{
					//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
					//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
					//IL_0060: Unknown result type (might be due to invalid IL or missing references)
					//IL_0065: Unknown result type (might be due to invalid IL or missing references)
					//IL_006c: Unknown result type (might be due to invalid IL or missing references)
					//IL_002d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0032: Unknown result type (might be due to invalid IL or missing references)
					//IL_0046: Unknown result type (might be due to invalid IL or missing references)
					//IL_0047: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					<>c__DisplayClass24_0 <>c__DisplayClass24_ = <>4__this;
					try
					{
						if (num == 0 || <>c__DisplayClass24_.<>4__this._minimumSearchTask != null)
						{
							try
							{
								TaskAwaiter val;
								if (num != 0)
								{
									val = <>c__DisplayClass24_.<>4__this._minimumSearchTask.GetAwaiter();
									if (!((TaskAwaiter)(ref val)).IsCompleted)
									{
										num = (<>1__state = 0);
										<>u__1 = val;
										((AsyncVoidMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <<AddOrUpdateScanResults>b__0>d>(ref val, ref this);
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
								<>c__DisplayClass24_.<>4__this.HasResults = true;
							}
							catch
							{
							}
						}
						BleScanResultModel bleScanResultModel = default(BleScanResultModel);
						if (!<>c__DisplayClass24_.<>4__this._searchResultsSet.TryGetValue(<>c__DisplayClass24_.bleScanResultModel.BleScanResult.DeviceId, ref bleScanResultModel))
						{
							bleScanResultModel = <>c__DisplayClass24_.bleScanResultModel;
							<>c__DisplayClass24_.<>4__this._searchResultsSet.Add(<>c__DisplayClass24_.bleScanResultModel.BleScanResult.DeviceId, <>c__DisplayClass24_.bleScanResultModel);
							((Collection<BleScanResultModel>)(object)<>c__DisplayClass24_.<>4__this._searchResults).Add(<>c__DisplayClass24_.bleScanResultModel);
							<>c__DisplayClass24_.<>4__this.DeviceAdded((global::System.Collections.Generic.IReadOnlyList<BleScanResultModel>)<>c__DisplayClass24_.<>4__this._searchResults);
						}
						bleScanResultModel.Update(<>c__DisplayClass24_.bleScanResultModel.BleScanResult);
					}
					catch (global::System.Exception exception)
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

			public SearchForDevicesViewModel <>4__this;

			public BleScanResultModel bleScanResultModel;

			[AsyncStateMachine(typeof(<<AddOrUpdateScanResults>b__0>d))]
			internal void <AddOrUpdateScanResults>b__0()
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				<<AddOrUpdateScanResults>b__0>d <<AddOrUpdateScanResults>b__0>d = default(<<AddOrUpdateScanResults>b__0>d);
				<<AddOrUpdateScanResults>b__0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
				<<AddOrUpdateScanResults>b__0>d.<>4__this = this;
				<<AddOrUpdateScanResults>b__0>d.<>1__state = -1;
				((AsyncVoidMethodBuilder)(ref <<AddOrUpdateScanResults>b__0>d.<>t__builder)).Start<<<AddOrUpdateScanResults>b__0>d>(ref <<AddOrUpdateScanResults>b__0>d);
			}
		}

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <OnResumeAsync>d__21 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public SearchForDevicesViewModel <>4__this;

			public ResumeReason reason;

			public INavigationParameters parameters;

			public CancellationToken cancellationToken;

			private TaskAwaiter <>u__1;

			private TaskAwaiter<PermissionsRequestState> <>u__2;

			private TaskAwaiter<INavigationResult> <>u__3;

			private void MoveNext()
			{
				//IL_006b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0070: Unknown result type (might be due to invalid IL or missing references)
				//IL_0077: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
				//IL_014e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0153: Unknown result type (might be due to invalid IL or missing references)
				//IL_015b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0094: Unknown result type (might be due to invalid IL or missing references)
				//IL_009e: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
				//IL_0109: Expected I4, but got Unknown
				//IL_0051: Unknown result type (might be due to invalid IL or missing references)
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
				//IL_0176: Unknown result type (might be due to invalid IL or missing references)
				//IL_011c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0121: Unknown result type (might be due to invalid IL or missing references)
				//IL_0136: Unknown result type (might be due to invalid IL or missing references)
				//IL_0138: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				SearchForDevicesViewModel searchForDevicesViewModel = <>4__this;
				try
				{
					TaskAwaiter val3;
					TaskAwaiter<PermissionsRequestState> val2;
					TaskAwaiter<INavigationResult> val;
					PermissionsRequestState result;
					switch (num)
					{
					default:
						val3 = searchForDevicesViewModel.<>n__0(reason, parameters, cancellationToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val3)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val3;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnResumeAsync>d__21>(ref val3, ref this);
							return;
						}
						goto IL_0086;
					case 0:
						val3 = <>u__1;
						<>u__1 = default(TaskAwaiter);
						num = (<>1__state = -1);
						goto IL_0086;
					case 1:
						val2 = <>u__2;
						<>u__2 = default(TaskAwaiter<PermissionsRequestState>);
						num = (<>1__state = -1);
						goto IL_00ef;
					case 2:
						{
							val = <>u__3;
							<>u__3 = default(TaskAwaiter<INavigationResult>);
							num = (<>1__state = -1);
							break;
						}
						IL_00ef:
						result = val2.GetResult();
						switch ((int)result)
						{
						case 1:
							goto end_IL_000e;
						default:
							val = ((PageViewModel)searchForDevicesViewModel).NavigationService.GoBackAsync((INavigationParameters)null).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 2);
								<>u__3 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<INavigationResult>, <OnResumeAsync>d__21>(ref val, ref this);
								return;
							}
							break;
						case 0:
							searchForDevicesViewModel.Search(ViewModelExtensions.PausedCancellationToken((IViewModel)(object)searchForDevicesViewModel));
							goto end_IL_000e;
						}
						break;
						IL_0086:
						((TaskAwaiter)(ref val3)).GetResult();
						val2 = searchForDevicesViewModel._bluetoothPermissionHandler.RequestPermissionsAsync(cancellationToken).GetAwaiter();
						if (!val2.IsCompleted)
						{
							num = (<>1__state = 1);
							<>u__2 = val2;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<PermissionsRequestState>, <OnResumeAsync>d__21>(ref val2, ref this);
							return;
						}
						goto IL_00ef;
					}
					val.GetResult();
					end_IL_000e:;
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

		private static readonly TimeSpan MinimumSearchTime = TimeSpan.FromSeconds(3L);

		private readonly IBluetoothPermissionHandler _bluetoothPermissionHandler;

		private readonly IBleScannerService _bleScannerService;

		protected readonly Dictionary<Guid, BleScanResultModel> _searchResultsSet = new Dictionary<Guid, BleScanResultModel>();

		protected readonly SortedObservableCollection<BleScanResultModel> _searchResults;

		protected global::System.Threading.Tasks.Task? _minimumSearchTask;

		[ObservableProperty]
		private string _message = string.Empty;

		[ObservableProperty]
		protected BleScanResultModel? _selectedSearchResult;

		[ObservableProperty]
		private bool _hasResults;

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		private RelayCommand<BleScanResultModel?>? searchResultSelectedCommand;

		public global::System.Collections.Generic.IEnumerable<BleScanResultModel> SearchResults => (global::System.Collections.Generic.IEnumerable<BleScanResultModel>)_searchResults;

		protected virtual IComparer<BleScanResultModel>? SearchResultsComparer => null;

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string Message
		{
			get
			{
				return _message;
			}
			[MemberNotNull("_message")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(_message, value))
				{
					((ObservableObject)this).OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Message);
					_message = value;
					((ObservableObject)this).OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Message);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public BleScanResultModel? SelectedSearchResult
		{
			get
			{
				return _selectedSearchResult;
			}
			set
			{
				if (!EqualityComparer<BleScanResultModel>.Default.Equals(_selectedSearchResult, value))
				{
					((ObservableObject)this).OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SelectedSearchResult);
					_selectedSearchResult = value;
					OnSelectedSearchResultChanged(value);
					((ObservableObject)this).OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SelectedSearchResult);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool HasResults
		{
			get
			{
				return _hasResults;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(_hasResults, value))
				{
					((ObservableObject)this).OnPropertyChanging(__KnownINotifyPropertyChangingArgs.HasResults);
					_hasResults = value;
					OnHasResultsChanged(value);
					((ObservableObject)this).OnPropertyChanged(__KnownINotifyPropertyChangedArgs.HasResults);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public IRelayCommand<BleScanResultModel?> SearchResultSelectedCommand => (IRelayCommand<BleScanResultModel?>)(object)(searchResultSelectedCommand ?? (searchResultSelectedCommand = new RelayCommand<BleScanResultModel>((Action<BleScanResultModel>)SearchResultSelected)));

		protected SearchForDevicesViewModel(IServiceProvider serviceProvider, IBluetoothPermissionHandler bluetoothPermissionHandler, IBleScannerService bleScannerService)
			: base(serviceProvider)
		{
			_bluetoothPermissionHandler = bluetoothPermissionHandler;
			_bleScannerService = bleScannerService;
			((DualActionViewModel)this).BackAction = [CompilerGenerated] () => ((PageViewModel)this).NavigationService.GoBackAsync((INavigationParameters)null);
			UpdatePrimaryAction();
			IComparer<BleScanResultModel> searchResultsComparer = SearchResultsComparer;
			_searchResults = ((searchResultsComparer != null) ? new SortedObservableCollection<BleScanResultModel>(searchResultsComparer) : new SortedObservableCollection<BleScanResultModel>());
		}

		[RelayCommand]
		private void SearchResultSelected(BleScanResultModel? selectedSearchResult)
		{
			SelectedSearchResult = selectedSearchResult;
		}

		private void UpdatePrimaryAction()
		{
			if (HasResults)
			{
				ContinueAction();
			}
			else
			{
				CancelAction();
			}
		}

		private void ContinueAction()
		{
			((DualActionViewModel)this).PrimaryAction = [CompilerGenerated] () => (SelectedSearchResult == null) ? global::System.Threading.Tasks.Task.FromResult<bool>(false) : ConnectWithDeviceAsync(SelectedSearchResult);
			((DualActionViewModel)this).PrimaryActionStyle = (ActionStyle)0;
			((DualActionViewModel)this).PrimaryActionText = Strings.AddAndExplore_Action_Continue;
			((DualActionViewModel)this).PrimaryActionEnabled = false;
		}

		private void CancelAction()
		{
			((DualActionViewModel)this).PrimaryAction = ((DualActionViewModel)this).BackAction;
			((DualActionViewModel)this).PrimaryActionStyle = (ActionStyle)2;
			((DualActionViewModel)this).PrimaryActionText = Strings.AddAndExplore_Action_Cancel;
			((DualActionViewModel)this).PrimaryActionEnabled = true;
		}

		protected virtual void HasResultsChanged(bool hasResults)
		{
		}

		protected abstract bool IncludeScanResult(BleScanResultModel bleScanResultModel);

		protected abstract global::System.Threading.Tasks.Task ConnectWithDeviceAsync(BleScanResultModel bleScanResultModel);

		[AsyncStateMachine(typeof(<OnResumeAsync>d__21))]
		public override global::System.Threading.Tasks.Task OnResumeAsync(ResumeReason reason, INavigationParameters parameters, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			<OnResumeAsync>d__21 <OnResumeAsync>d__ = default(<OnResumeAsync>d__21);
			<OnResumeAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<OnResumeAsync>d__.<>4__this = this;
			<OnResumeAsync>d__.reason = reason;
			<OnResumeAsync>d__.parameters = parameters;
			<OnResumeAsync>d__.cancellationToken = cancellationToken;
			<OnResumeAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <OnResumeAsync>d__.<>t__builder)).Start<<OnResumeAsync>d__21>(ref <OnResumeAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <OnResumeAsync>d__.<>t__builder)).Task;
		}

		public override void OnPause(PauseReason reason)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((PageViewModel)this).OnPause(reason);
			ClearScanResults();
		}

		private void Search(CancellationToken cancellationToken)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			<>c__DisplayClass23_0 CS$<>8__locals5 = new <>c__DisplayClass23_0();
			CS$<>8__locals5.<>4__this = this;
			CS$<>8__locals5.cancellationToken = cancellationToken;
			_minimumSearchTask = global::System.Threading.Tasks.Task.Delay(MinimumSearchTime, CS$<>8__locals5.cancellationToken);
			global::System.Threading.Tasks.Task.Run((Func<global::System.Threading.Tasks.Task>)([AsyncStateMachine(typeof(<>c__DisplayClass23_0.<<Search>b__0>d))] () =>
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				<>c__DisplayClass23_0.<<Search>b__0>d <<Search>b__0>d = default(<>c__DisplayClass23_0.<<Search>b__0>d);
				<<Search>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<Search>b__0>d.<>4__this = CS$<>8__locals5;
				<<Search>b__0>d.<>1__state = -1;
				((AsyncTaskMethodBuilder)(ref <<Search>b__0>d.<>t__builder)).Start<<>c__DisplayClass23_0.<<Search>b__0>d>(ref <<Search>b__0>d);
				return ((AsyncTaskMethodBuilder)(ref <<Search>b__0>d.<>t__builder)).Task;
			}), CS$<>8__locals5.cancellationToken);
		}

		protected virtual void AddOrUpdateScanResults(BleScanResultModel bleScanResultModel)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected O, but got Unknown
			<>c__DisplayClass24_0 CS$<>8__locals3 = new <>c__DisplayClass24_0();
			CS$<>8__locals3.<>4__this = this;
			CS$<>8__locals3.bleScanResultModel = bleScanResultModel;
			((ViewModel)this).MainThreadService.BeginInvokeOnMainThread((Action)([AsyncStateMachine(typeof(<>c__DisplayClass24_0.<<AddOrUpdateScanResults>b__0>d))] () =>
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				<>c__DisplayClass24_0.<<AddOrUpdateScanResults>b__0>d <<AddOrUpdateScanResults>b__0>d = default(<>c__DisplayClass24_0.<<AddOrUpdateScanResults>b__0>d);
				<<AddOrUpdateScanResults>b__0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
				<<AddOrUpdateScanResults>b__0>d.<>4__this = CS$<>8__locals3;
				<<AddOrUpdateScanResults>b__0>d.<>1__state = -1;
				((AsyncVoidMethodBuilder)(ref <<AddOrUpdateScanResults>b__0>d.<>t__builder)).Start<<>c__DisplayClass24_0.<<AddOrUpdateScanResults>b__0>d>(ref <<AddOrUpdateScanResults>b__0>d);
			}));
		}

		private void ClearScanResults()
		{
			HasResults = false;
			_searchResultsSet.Clear();
			((Collection<BleScanResultModel>)(object)_searchResults).Clear();
		}

		protected virtual void DeviceAdded(global::System.Collections.Generic.IReadOnlyList<BleScanResultModel> devices)
		{
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnSelectedSearchResultChanged(BleScanResultModel? value)
		{
			global::System.Collections.Generic.IEnumerator<BleScanResultModel> enumerator = SearchResults.GetEnumerator();
			try
			{
				while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
				{
					enumerator.Current.IsSelected = false;
				}
			}
			finally
			{
				((global::System.IDisposable)enumerator)?.Dispose();
			}
			((DualActionViewModel)this).PrimaryActionEnabled = value != null;
			if (value != null)
			{
				value.IsSelected = true;
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnHasResultsChanged(bool value)
		{
			HasResultsChanged(value);
			UpdatePrimaryAction();
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private global::System.Threading.Tasks.Task <>n__0(ResumeReason reason, INavigationParameters parameters, CancellationToken cancellationToken)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			return ((PageViewModel)this).OnResumeAsync(reason, parameters, cancellationToken);
		}
	}
}
namespace App.Common.Pages.Pairing.SearchForDevices.SignalView
{
	public static class SignalViewImages
	{
		private static readonly string s_baseIconResource = "resource://" + typeof(SignalViewImages).Namespace + ".Resources.Images.{0}.svg?assembly=" + typeof(SignalViewImages).Assembly.GetName().Name;

		public static readonly string signal_highest = string.Format(s_baseIconResource, (object)"signal_highest");

		public static readonly string signal_high = string.Format(s_baseIconResource, (object)"signal_high");

		public static readonly string signal_medium = string.Format(s_baseIconResource, (object)"signal_medium");

		public static readonly string signal_low = string.Format(s_baseIconResource, (object)"signal_low");

		public static readonly string signal_lowest = string.Format(s_baseIconResource, (object)"signal_lowest");
	}
	[XamlFilePath("SearchForDevices/SignalView/SignalView.xaml")]
	public class SignalView : AbsoluteLayout
	{
		public static readonly BindableProperty SignalProperty = BindableProperty.Create("Signal", typeof(int), typeof(SignalView), (object)0, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)delegate(BindableObject bindable, object _, object newValue)
		{
			SignalView signalView = (SignalView)(object)bindable;
			int num = (int)newValue;
			if (num >= -80)
			{
				if (num < -55)
				{
					if (num >= -67)
					{
						signalView.IsHighest = false;
						signalView.IsHigh = true;
						signalView.IsMedium = true;
						signalView.IsLow = true;
						signalView.IsLowest = true;
					}
					else
					{
						signalView.IsHighest = false;
						signalView.IsHigh = false;
						signalView.IsMedium = true;
						signalView.IsLow = true;
						signalView.IsLowest = true;
					}
				}
				else
				{
					signalView.IsHighest = true;
					signalView.IsHigh = true;
					signalView.IsMedium = true;
					signalView.IsLow = true;
					signalView.IsLowest = true;
				}
			}
			else if (num >= -90)
			{
				signalView.IsHighest = false;
				signalView.IsHigh = false;
				signalView.IsMedium = false;
				signalView.IsLow = true;
				signalView.IsLowest = true;
			}
			else
			{
				signalView.IsHighest = false;
				signalView.IsHigh = false;
				signalView.IsMedium = false;
				signalView.IsLow = false;
				signalView.IsLowest = true;
			}
		}, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)null, (CreateDefaultValueDelegate)null);

		public static readonly BindableProperty TintProperty = BindableProperty.Create("Tint", typeof(Color), typeof(SignalView), (object)null, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)null, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)null, (CreateDefaultValueDelegate)null);

		private bool _isHighest;

		private bool _isHigh;

		private bool _isMedium;

		private bool _isLow;

		private bool _isLowest;

		public int Signal
		{
			get
			{
				return (int)((BindableObject)this).GetValue(SignalProperty);
			}
			set
			{
				((BindableObject)this).SetValue(SignalProperty, (object)value);
			}
		}

		public Color Tint
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected O, but got Unknown
				return (Color)((BindableObject)this).GetValue(TintProperty);
			}
			set
			{
				((BindableObject)this).SetValue(TintProperty, (object)value);
			}
		}

		public bool IsHighest
		{
			get
			{
				return _isHighest;
			}
			private set
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Expected O, but got Unknown
				_isHighest = value;
				MainThread.BeginInvokeOnMainThread((Action)([CompilerGenerated] () =>
				{
					((BindableObject)this).OnPropertyChanged("IsHighest");
				}));
			}
		}

		public bool IsHigh
		{
			get
			{
				return _isHigh;
			}
			private set
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Expected O, but got Unknown
				_isHigh = value;
				MainThread.BeginInvokeOnMainThread((Action)([CompilerGenerated] () =>
				{
					((BindableObject)this).OnPropertyChanged("IsHigh");
				}));
			}
		}

		public bool IsMedium
		{
			get
			{
				return _isMedium;
			}
			private set
			{
				_isMedium = value;
				((BindableObject)this).OnPropertyChanged("IsMedium");
			}
		}

		public bool IsLow
		{
			get
			{
				return _isLow;
			}
			private set
			{
				_isLow = value;
				((BindableObject)this).OnPropertyChanged("IsLow");
			}
		}

		public bool IsLowest
		{
			get
			{
				return _isLowest;
			}
			private set
			{
				_isLowest = value;
				((BindableObject)this).OnPropertyChanged("IsLowest");
			}
		}

		public SignalView()
		{
			InitializeComponent();
		}

		[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
		private void InitializeComponent()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected O, but got Unknown
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected O, but got Unknown
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Expected O, but got Unknown
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Expected O, but got Unknown
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Expected O, but got Unknown
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Expected O, but got Unknown
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Expected O, but got Unknown
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Expected O, but got Unknown
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Expected O, but got Unknown
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Expected O, but got Unknown
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Expected O, but got Unknown
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Expected O, but got Unknown
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Expected O, but got Unknown
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Expected O, but got Unknown
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Expected O, but got Unknown
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Expected O, but got Unknown
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Expected O, but got Unknown
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Expected O, but got Unknown
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Expected O, but got Unknown
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Expected O, but got Unknown
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Expected O, but got Unknown
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Expected O, but got Unknown
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a5: Expected O, but got Unknown
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02aa: Expected O, but got Unknown
			//IL_02af: Expected O, but got Unknown
			//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0301: Unknown result type (might be due to invalid IL or missing references)
			//IL_0306: Unknown result type (might be due to invalid IL or missing references)
			//IL_0316: Unknown result type (might be due to invalid IL or missing references)
			//IL_0326: Unknown result type (might be due to invalid IL or missing references)
			//IL_0336: Unknown result type (might be due to invalid IL or missing references)
			//IL_0346: Unknown result type (might be due to invalid IL or missing references)
			//IL_036a: Expected O, but got Unknown
			//IL_0365: Unknown result type (might be due to invalid IL or missing references)
			//IL_036f: Expected O, but got Unknown
			//IL_0374: Expected O, but got Unknown
			//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0435: Unknown result type (might be due to invalid IL or missing references)
			//IL_043a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0445: Unknown result type (might be due to invalid IL or missing references)
			//IL_044a: Unknown result type (might be due to invalid IL or missing references)
			//IL_045a: Unknown result type (might be due to invalid IL or missing references)
			//IL_046a: Unknown result type (might be due to invalid IL or missing references)
			//IL_047a: Unknown result type (might be due to invalid IL or missing references)
			//IL_048a: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ae: Expected O, but got Unknown
			//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b3: Expected O, but got Unknown
			//IL_04b8: Expected O, but got Unknown
			//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_050a: Unknown result type (might be due to invalid IL or missing references)
			//IL_050f: Unknown result type (might be due to invalid IL or missing references)
			//IL_051f: Unknown result type (might be due to invalid IL or missing references)
			//IL_052f: Unknown result type (might be due to invalid IL or missing references)
			//IL_053f: Unknown result type (might be due to invalid IL or missing references)
			//IL_054f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0573: Expected O, but got Unknown
			//IL_056e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0578: Expected O, but got Unknown
			//IL_057d: Expected O, but got Unknown
			//IL_05d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_063e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0643: Unknown result type (might be due to invalid IL or missing references)
			//IL_064e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0653: Unknown result type (might be due to invalid IL or missing references)
			//IL_0663: Unknown result type (might be due to invalid IL or missing references)
			//IL_0673: Unknown result type (might be due to invalid IL or missing references)
			//IL_0683: Unknown result type (might be due to invalid IL or missing references)
			//IL_0693: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b7: Expected O, but got Unknown
			//IL_06b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_06bc: Expected O, but got Unknown
			//IL_06c1: Expected O, but got Unknown
			//IL_0703: Unknown result type (might be due to invalid IL or missing references)
			//IL_0708: Unknown result type (might be due to invalid IL or missing references)
			//IL_0713: Unknown result type (might be due to invalid IL or missing references)
			//IL_0718: Unknown result type (might be due to invalid IL or missing references)
			//IL_0728: Unknown result type (might be due to invalid IL or missing references)
			//IL_0738: Unknown result type (might be due to invalid IL or missing references)
			//IL_0748: Unknown result type (might be due to invalid IL or missing references)
			//IL_0758: Unknown result type (might be due to invalid IL or missing references)
			//IL_077c: Expected O, but got Unknown
			//IL_0777: Unknown result type (might be due to invalid IL or missing references)
			//IL_0781: Expected O, but got Unknown
			//IL_0786: Expected O, but got Unknown
			//IL_07e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0847: Unknown result type (might be due to invalid IL or missing references)
			//IL_084c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0857: Unknown result type (might be due to invalid IL or missing references)
			//IL_085c: Unknown result type (might be due to invalid IL or missing references)
			//IL_086c: Unknown result type (might be due to invalid IL or missing references)
			//IL_087c: Unknown result type (might be due to invalid IL or missing references)
			//IL_088c: Unknown result type (might be due to invalid IL or missing references)
			//IL_089c: Unknown result type (might be due to invalid IL or missing references)
			//IL_08c0: Expected O, but got Unknown
			//IL_08bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_08c5: Expected O, but got Unknown
			//IL_08ca: Expected O, but got Unknown
			//IL_090c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0911: Unknown result type (might be due to invalid IL or missing references)
			//IL_091c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0921: Unknown result type (might be due to invalid IL or missing references)
			//IL_0931: Unknown result type (might be due to invalid IL or missing references)
			//IL_0941: Unknown result type (might be due to invalid IL or missing references)
			//IL_0951: Unknown result type (might be due to invalid IL or missing references)
			//IL_0961: Unknown result type (might be due to invalid IL or missing references)
			//IL_0985: Expected O, but got Unknown
			//IL_0980: Unknown result type (might be due to invalid IL or missing references)
			//IL_098a: Expected O, but got Unknown
			//IL_098f: Expected O, but got Unknown
			//IL_09ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a50: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a55: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a60: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a65: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a75: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a85: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a95: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aa5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ac9: Expected O, but got Unknown
			//IL_0ac4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ace: Expected O, but got Unknown
			//IL_0ad3: Expected O, but got Unknown
			//IL_0b15: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b1a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b25: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b2a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b3a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b4a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b5a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b6a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b8e: Expected O, but got Unknown
			//IL_0b89: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b93: Expected O, but got Unknown
			//IL_0b98: Expected O, but got Unknown
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			string signal_highest = SignalViewImages.signal_highest;
			global::System.Type typeFromHandle = typeof(SignalView);
			RelativeSourceExtension val = new RelativeSourceExtension();
			BindingExtension val2 = new BindingExtension();
			global::System.Type typeFromHandle2 = typeof(SignalView);
			RelativeSourceExtension val3 = new RelativeSourceExtension();
			BindingExtension val4 = new BindingExtension();
			SKImageView val5 = new SKImageView();
			string signal_high = SignalViewImages.signal_high;
			global::System.Type typeFromHandle3 = typeof(SignalView);
			RelativeSourceExtension val6 = new RelativeSourceExtension();
			BindingExtension val7 = new BindingExtension();
			global::System.Type typeFromHandle4 = typeof(SignalView);
			RelativeSourceExtension val8 = new RelativeSourceExtension();
			BindingExtension val9 = new BindingExtension();
			SKImageView val10 = new SKImageView();
			string signal_medium = SignalViewImages.signal_medium;
			global::System.Type typeFromHandle5 = typeof(SignalView);
			RelativeSourceExtension val11 = new RelativeSourceExtension();
			BindingExtension val12 = new BindingExtension();
			global::System.Type typeFromHandle6 = typeof(SignalView);
			RelativeSourceExtension val13 = new RelativeSourceExtension();
			BindingExtension val14 = new BindingExtension();
			SKImageView val15 = new SKImageView();
			string signal_low = SignalViewImages.signal_low;
			global::System.Type typeFromHandle7 = typeof(SignalView);
			RelativeSourceExtension val16 = new RelativeSourceExtension();
			BindingExtension val17 = new BindingExtension();
			global::System.Type typeFromHandle8 = typeof(SignalView);
			RelativeSourceExtension val18 = new RelativeSourceExtension();
			BindingExtension val19 = new BindingExtension();
			SKImageView val20 = new SKImageView();
			string signal_lowest = SignalViewImages.signal_lowest;
			global::System.Type typeFromHandle9 = typeof(SignalView);
			RelativeSourceExtension val21 = new RelativeSourceExtension();
			BindingExtension val22 = new BindingExtension();
			global::System.Type typeFromHandle10 = typeof(SignalView);
			RelativeSourceExtension val23 = new RelativeSourceExtension();
			BindingExtension val24 = new BindingExtension();
			SKImageView val25 = new SKImageView();
			SignalView signalView;
			NameScope val26 = (NameScope)(((object)NameScope.GetNameScope((BindableObject)(object)(signalView = this))) ?? ((object)new NameScope()));
			NameScope.SetNameScope((BindableObject)(object)signalView, (INameScope)(object)val26);
			((Element)val5).transientNamescope = (INameScope)(object)val26;
			((Element)val10).transientNamescope = (INameScope)(object)val26;
			((Element)val15).transientNamescope = (INameScope)(object)val26;
			((Element)val20).transientNamescope = (INameScope)(object)val26;
			((Element)val25).transientNamescope = (INameScope)(object)val26;
			((BindableObject)val5).SetValue(AbsoluteLayout.LayoutFlagsProperty, (object)(AbsoluteLayoutFlags)(-1));
			((BindableObject)val5).SetValue(AbsoluteLayout.LayoutBoundsProperty, (object)new Rect(0.0, 0.0, 1.0, 1.0));
			((BindableObject)val5).SetValue(SKImageView.AspectProperty, (object)(Aspect)0);
			((BindableObject)val5).SetValue(SKImageView.ImageSourceProperty, (object)ImageSource.op_Implicit(signal_highest));
			val.Mode = (RelativeBindingSourceMode)3;
			val.AncestorType = typeFromHandle;
			RelativeBindingSource source = ((IMarkupExtension<RelativeBindingSource>)(object)val).ProvideValue((IServiceProvider)null);
			val2.Source = source;
			val2.Path = "IsHighest";
			XamlServiceProvider val27 = new XamlServiceProvider();
			global::System.Type typeFromHandle11 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver val28 = new XmlNamespaceResolver();
			val28.Add("", "http://schemas.microsoft.com/dotnet/2021/maui");
			val28.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			val28.Add("ui", "http://lci1.com/schemas/ui");
			val28.Add("xaml", "clr-namespace:Microsoft.Maui.Controls.Xaml;assembly=Microsoft.Maui.Controls.Xaml");
			val28.Add("signalView", "clr-namespace:App.Common.Pages.Pairing.SearchForDevices.SignalView");
			val27.Add(typeFromHandle11, (object)new XamlTypeResolver((IXmlNamespaceResolver)val28, typeof(SignalView).Assembly));
			BindingBase val29 = ((IMarkupExtension<BindingBase>)(object)val2).ProvideValue((IServiceProvider)val27);
			((BindableObject)val5).SetBinding(VisualElement.IsVisibleProperty, val29);
			val3.Mode = (RelativeBindingSourceMode)3;
			val3.AncestorType = typeFromHandle2;
			RelativeBindingSource source2 = ((IMarkupExtension<RelativeBindingSource>)(object)val3).ProvideValue((IServiceProvider)null);
			val4.Source = source2;
			val4.Path = "Tint";
			XamlServiceProvider val30 = new XamlServiceProvider();
			global::System.Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver val31 = new XmlNamespaceResolver();
			val31.Add("", "http://schemas.microsoft.com/dotnet/2021/maui");
			val31.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			val31.Add("ui", "http://lci1.com/schemas/ui");
			val31.Add("xaml", "clr-namespace:Microsoft.Maui.Controls.Xaml;assembly=Microsoft.Maui.Controls.Xaml");
			val31.Add("signalView", "clr-namespace:App.Common.Pages.Pairing.SearchForDevices.SignalView");
			val30.Add(typeFromHandle12, (object)new XamlTypeResolver((IXmlNamespaceResolver)val31, typeof(SignalView).Assembly));
			BindingBase val32 = ((IMarkupExtension<BindingBase>)(object)val4).ProvideValue((IServiceProvider)val30);
			((BindableObject)val5).SetBinding(SKImageView.TintProperty, val32);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)signalView).Children).Add((IView)(object)val5);
			((BindableObject)val10).SetValue(AbsoluteLayout.LayoutFlagsProperty, (object)(AbsoluteLayoutFlags)(-1));
			((BindableObject)val10).SetValue(AbsoluteLayout.LayoutBoundsProperty, (object)new Rect(0.0, 0.0, 1.0, 1.0));
			((BindableObject)val10).SetValue(SKImageView.AspectProperty, (object)(Aspect)0);
			((BindableObject)val10).SetValue(SKImageView.ImageSourceProperty, (object)ImageSource.op_Implicit(signal_high));
			val6.Mode = (RelativeBindingSourceMode)3;
			val6.AncestorType = typeFromHandle3;
			RelativeBindingSource source3 = ((IMarkupExtension<RelativeBindingSource>)(object)val6).ProvideValue((IServiceProvider)null);
			val7.Source = source3;
			val7.Path = "IsHigh";
			XamlServiceProvider val33 = new XamlServiceProvider();
			global::System.Type typeFromHandle13 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver val34 = new XmlNamespaceResolver();
			val34.Add("", "http://schemas.microsoft.com/dotnet/2021/maui");
			val34.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			val34.Add("ui", "http://lci1.com/schemas/ui");
			val34.Add("xaml", "clr-namespace:Microsoft.Maui.Controls.Xaml;assembly=Microsoft.Maui.Controls.Xaml");
			val34.Add("signalView", "clr-namespace:App.Common.Pages.Pairing.SearchForDevices.SignalView");
			val33.Add(typeFromHandle13, (object)new XamlTypeResolver((IXmlNamespaceResolver)val34, typeof(SignalView).Assembly));
			BindingBase val35 = ((IMarkupExtension<BindingBase>)(object)val7).ProvideValue((IServiceProvider)val33);
			((BindableObject)val10).SetBinding(VisualElement.IsVisibleProperty, val35);
			val8.Mode = (RelativeBindingSourceMode)3;
			val8.AncestorType = typeFromHandle4;
			RelativeBindingSource source4 = ((IMarkupExtension<RelativeBindingSource>)(object)val8).ProvideValue((IServiceProvider)null);
			val9.Source = source4;
			val9.Path = "Tint";
			XamlServiceProvider val36 = new XamlServiceProvider();
			global::System.Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver val37 = new XmlNamespaceResolver();
			val37.Add("", "http://schemas.microsoft.com/dotnet/2021/maui");
			val37.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			val37.Add("ui", "http://lci1.com/schemas/ui");
			val37.Add("xaml", "clr-namespace:Microsoft.Maui.Controls.Xaml;assembly=Microsoft.Maui.Controls.Xaml");
			val37.Add("signalView", "clr-namespace:App.Common.Pages.Pairing.SearchForDevices.SignalView");
			val36.Add(typeFromHandle14, (object)new XamlTypeResolver((IXmlNamespaceResolver)val37, typeof(SignalView).Assembly));
			BindingBase val38 = ((IMarkupExtension<BindingBase>)(object)val9).ProvideValue((IServiceProvider)val36);
			((BindableObject)val10).SetBinding(SKImageView.TintProperty, val38);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)signalView).Children).Add((IView)(object)val10);
			((BindableObject)val15).SetValue(AbsoluteLayout.LayoutFlagsProperty, (object)(AbsoluteLayoutFlags)(-1));
			((BindableObject)val15).SetValue(AbsoluteLayout.LayoutBoundsProperty, (object)new Rect(0.0, 0.0, 1.0, 1.0));
			((BindableObject)val15).SetValue(SKImageView.AspectProperty, (object)(Aspect)0);
			((BindableObject)val15).SetValue(SKImageView.ImageSourceProperty, (object)ImageSource.op_Implicit(signal_medium));
			val11.Mode = (RelativeBindingSourceMode)3;
			val11.AncestorType = typeFromHandle5;
			RelativeBindingSource source5 = ((IMarkupExtension<RelativeBindingSource>)(object)val11).ProvideValue((IServiceProvider)null);
			val12.Source = source5;
			val12.Path = "IsMedium";
			XamlServiceProvider val39 = new XamlServiceProvider();
			global::System.Type typeFromHandle15 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver val40 = new XmlNamespaceResolver();
			val40.Add("", "http://schemas.microsoft.com/dotnet/2021/maui");
			val40.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			val40.Add("ui", "http://lci1.com/schemas/ui");
			val40.Add("xaml", "clr-namespace:Microsoft.Maui.Controls.Xaml;assembly=Microsoft.Maui.Controls.Xaml");
			val40.Add("signalView", "clr-namespace:App.Common.Pages.Pairing.SearchForDevices.SignalView");
			val39.Add(typeFromHandle15, (object)new XamlTypeResolver((IXmlNamespaceResolver)val40, typeof(SignalView).Assembly));
			BindingBase val41 = ((IMarkupExtension<BindingBase>)(object)val12).ProvideValue((IServiceProvider)val39);
			((BindableObject)val15).SetBinding(VisualElement.IsVisibleProperty, val41);
			val13.Mode = (RelativeBindingSourceMode)3;
			val13.AncestorType = typeFromHandle6;
			RelativeBindingSource source6 = ((IMarkupExtension<RelativeBindingSource>)(object)val13).ProvideValue((IServiceProvider)null);
			val14.Source = source6;
			val14.Path = "Tint";
			XamlServiceProvider val42 = new XamlServiceProvider();
			global::System.Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver val43 = new XmlNamespaceResolver();
			val43.Add("", "http://schemas.microsoft.com/dotnet/2021/maui");
			val43.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			val43.Add("ui", "http://lci1.com/schemas/ui");
			val43.Add("xaml", "clr-namespace:Microsoft.Maui.Controls.Xaml;assembly=Microsoft.Maui.Controls.Xaml");
			val43.Add("signalView", "clr-namespace:App.Common.Pages.Pairing.SearchForDevices.SignalView");
			val42.Add(typeFromHandle16, (object)new XamlTypeResolver((IXmlNamespaceResolver)val43, typeof(SignalView).Assembly));
			BindingBase val44 = ((IMarkupExtension<BindingBase>)(object)val14).ProvideValue((IServiceProvider)val42);
			((BindableObject)val15).SetBinding(SKImageView.TintProperty, val44);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)signalView).Children).Add((IView)(object)val15);
			((BindableObject)val20).SetValue(AbsoluteLayout.LayoutFlagsProperty, (object)(AbsoluteLayoutFlags)(-1));
			((BindableObject)val20).SetValue(AbsoluteLayout.LayoutBoundsProperty, (object)new Rect(0.0, 0.0, 1.0, 1.0));
			((BindableObject)val20).SetValue(SKImageView.AspectProperty, (object)(Aspect)0);
			((BindableObject)val20).SetValue(SKImageView.ImageSourceProperty, (object)ImageSource.op_Implicit(signal_low));
			val16.Mode = (RelativeBindingSourceMode)3;
			val16.AncestorType = typeFromHandle7;
			RelativeBindingSource source7 = ((IMarkupExtension<RelativeBindingSource>)(object)val16).ProvideValue((IServiceProvider)null);
			val17.Source = source7;
			val17.Path = "IsLow";
			XamlServiceProvider val45 = new XamlServiceProvider();
			global::System.Type typeFromHandle17 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver val46 = new XmlNamespaceResolver();
			val46.Add("", "http://schemas.microsoft.com/dotnet/2021/maui");
			val46.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			val46.Add("ui", "http://lci1.com/schemas/ui");
			val46.Add("xaml", "clr-namespace:Microsoft.Maui.Controls.Xaml;assembly=Microsoft.Maui.Controls.Xaml");
			val46.Add("signalView", "clr-namespace:App.Common.Pages.Pairing.SearchForDevices.SignalView");
			val45.Add(typeFromHandle17, (object)new XamlTypeResolver((IXmlNamespaceResolver)val46, typeof(SignalView).Assembly));
			BindingBase val47 = ((IMarkupExtension<BindingBase>)(object)val17).ProvideValue((IServiceProvider)val45);
			((BindableObject)val20).SetBinding(VisualElement.IsVisibleProperty, val47);
			val18.Mode = (RelativeBindingSourceMode)3;
			val18.AncestorType = typeFromHandle8;
			RelativeBindingSource source8 = ((IMarkupExtension<RelativeBindingSource>)(object)val18).ProvideValue((IServiceProvider)null);
			val19.Source = source8;
			val19.Path = "Tint";
			XamlServiceProvider val48 = new XamlServiceProvider();
			global::System.Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver val49 = new XmlNamespaceResolver();
			val49.Add("", "http://schemas.microsoft.com/dotnet/2021/maui");
			val49.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			val49.Add("ui", "http://lci1.com/schemas/ui");
			val49.Add("xaml", "clr-namespace:Microsoft.Maui.Controls.Xaml;assembly=Microsoft.Maui.Controls.Xaml");
			val49.Add("signalView", "clr-namespace:App.Common.Pages.Pairing.SearchForDevices.SignalView");
			val48.Add(typeFromHandle18, (object)new XamlTypeResolver((IXmlNamespaceResolver)val49, typeof(SignalView).Assembly));
			BindingBase val50 = ((IMarkupExtension<BindingBase>)(object)val19).ProvideValue((IServiceProvider)val48);
			((BindableObject)val20).SetBinding(SKImageView.TintProperty, val50);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)signalView).Children).Add((IView)(object)val20);
			((BindableObject)val25).SetValue(AbsoluteLayout.LayoutFlagsProperty, (object)(AbsoluteLayoutFlags)(-1));
			((BindableObject)val25).SetValue(AbsoluteLayout.LayoutBoundsProperty, (object)new Rect(0.0, 0.0, 1.0, 1.0));
			((BindableObject)val25).SetValue(SKImageView.AspectProperty, (object)(Aspect)0);
			((BindableObject)val25).SetValue(SKImageView.ImageSourceProperty, (object)ImageSource.op_Implicit(signal_lowest));
			val21.Mode = (RelativeBindingSourceMode)3;
			val21.AncestorType = typeFromHandle9;
			RelativeBindingSource source9 = ((IMarkupExtension<RelativeBindingSource>)(object)val21).ProvideValue((IServiceProvider)null);
			val22.Source = source9;
			val22.Path = "IsLowest";
			XamlServiceProvider val51 = new XamlServiceProvider();
			global::System.Type typeFromHandle19 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver val52 = new XmlNamespaceResolver();
			val52.Add("", "http://schemas.microsoft.com/dotnet/2021/maui");
			val52.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			val52.Add("ui", "http://lci1.com/schemas/ui");
			val52.Add("xaml", "clr-namespace:Microsoft.Maui.Controls.Xaml;assembly=Microsoft.Maui.Controls.Xaml");
			val52.Add("signalView", "clr-namespace:App.Common.Pages.Pairing.SearchForDevices.SignalView");
			val51.Add(typeFromHandle19, (object)new XamlTypeResolver((IXmlNamespaceResolver)val52, typeof(SignalView).Assembly));
			BindingBase val53 = ((IMarkupExtension<BindingBase>)(object)val22).ProvideValue((IServiceProvider)val51);
			((BindableObject)val25).SetBinding(VisualElement.IsVisibleProperty, val53);
			val23.Mode = (RelativeBindingSourceMode)3;
			val23.AncestorType = typeFromHandle10;
			RelativeBindingSource source10 = ((IMarkupExtension<RelativeBindingSource>)(object)val23).ProvideValue((IServiceProvider)null);
			val24.Source = source10;
			val24.Path = "Tint";
			XamlServiceProvider val54 = new XamlServiceProvider();
			global::System.Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver val55 = new XmlNamespaceResolver();
			val55.Add("", "http://schemas.microsoft.com/dotnet/2021/maui");
			val55.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			val55.Add("ui", "http://lci1.com/schemas/ui");
			val55.Add("xaml", "clr-namespace:Microsoft.Maui.Controls.Xaml;assembly=Microsoft.Maui.Controls.Xaml");
			val55.Add("signalView", "clr-namespace:App.Common.Pages.Pairing.SearchForDevices.SignalView");
			val54.Add(typeFromHandle20, (object)new XamlTypeResolver((IXmlNamespaceResolver)val55, typeof(SignalView).Assembly));
			BindingBase val56 = ((IMarkupExtension<BindingBase>)(object)val24).ProvideValue((IServiceProvider)val54);
			((BindableObject)val25).SetBinding(SKImageView.TintProperty, val56);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)signalView).Children).Add((IView)(object)val25);
		}
	}
}
namespace App.Common.Pages.Pairing.ScanWithCamera
{
	public class CustomCameraBarcodeReaderViewHandler : CameraBarcodeReaderViewHandler
	{
		public static PropertyMapper<ICameraBarcodeReaderView, CustomCameraBarcodeReaderViewHandler> CustomCameraBarcodeReaderMapper = new PropertyMapper<ICameraBarcodeReaderView, CustomCameraBarcodeReaderViewHandler>((IPropertyMapper[])(object)new IPropertyMapper[1] { (IPropertyMapper)CameraBarcodeReaderViewHandler.CameraBarcodeReaderViewMapper }) { ["IsTorchOn"] = IgnoreMapping };

		public CustomCameraBarcodeReaderViewHandler()
			: base((PropertyMapper)(object)CustomCameraBarcodeReaderMapper)
		{
		}

		public CustomCameraBarcodeReaderViewHandler(PropertyMapper mapper = null)
			: base((PropertyMapper)(((object)mapper) ?? ((object)CustomCameraBarcodeReaderMapper)))
		{
		}

		public static void IgnoreMapping(IViewHandler handler, IView view)
		{
		}
	}
	public class QrParser
	{
		public delegate string TransformTextDelegate(string text, GatewayType type);

		public static readonly string BluetoothGatewayIdPrefix = "LCIRemote";

		public static readonly string WifiGatewayIdPrefix = "MyRV_";

		public static readonly string AquafiGatewayIdPrefix = "LCI_AquaFi_";

		public static readonly string OldWiFiGatewayQrText = "qr*1y";

		public static readonly string QrIdKey = "devid";

		public static readonly string QrPasswordKey = "pw";

		private Dictionary<ValueMatchType, TransformTextDelegate> SanitizationMethodsDictionary
		{
			get
			{
				Dictionary<ValueMatchType, TransformTextDelegate> obj = new Dictionary<ValueMatchType, TransformTextDelegate>();
				obj.Add(ValueMatchType.Id, (TransformTextDelegate)SanitizeId);
				obj.Add(ValueMatchType.Password, (TransformTextDelegate)SanitizePassword);
				return obj;
			}
		}

		public ScannableDeviceLabel Parse(string qrCode, global::System.Collections.Generic.IEnumerable<GatewayType> targetDevices = null)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrWhiteSpace(qrCode))
			{
				throw new ArgumentException("Cannot parse QR code: '" + qrCode + "'");
			}
			if (IsOldWifiGateway(qrCode))
			{
				throw new ArgumentException("QR code unsupported.");
			}
			ScannableDeviceLabel scannableDeviceLabel = ((!IsUri(qrCode)) ? ParseAsRawText(qrCode) : ParseAsUri(qrCode));
			if (targetDevices == null || Enumerable.Contains<GatewayType>(targetDevices, scannableDeviceLabel.Device))
			{
				return scannableDeviceLabel;
			}
			throw new ArgumentException($"Parsed QR code for device '{scannableDeviceLabel.Device}' but expected a different target device.");
		}

		private bool IsOldWifiGateway(string qrCode)
		{
			return qrCode.EndsWith(OldWiFiGatewayQrText, (StringComparison)3);
		}

		private bool IsUri(string qrCode)
		{
			return qrCode.Trim().StartsWith("http");
		}

		private ScannableDeviceLabel ParseAsUri(string qrCode)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			NameValueCollection val = HttpUtility.ParseQueryString(new Uri(qrCode).Query);
			if (!Enumerable.Contains<string>((global::System.Collections.Generic.IEnumerable<string>)val.AllKeys, QrIdKey) || !Enumerable.Contains<string>((global::System.Collections.Generic.IEnumerable<string>)val.AllKeys, QrPasswordKey))
			{
				return ParseAsRawText(qrCode);
			}
			string text = val[QrIdKey];
			string password = val[QrPasswordKey];
			GatewayType device = GatewayType.Unknown;
			foreach (GatewayType value in global::System.Enum.GetValues(typeof(GatewayType)))
			{
				if (value != GatewayType.Unknown && SharedGatewayParsingMetadata.IdRegexDictionary.ContainsKey(value) && ((Group)SharedGatewayParsingMetadata.IdRegexDictionary[value].Match(text)).Success)
				{
					device = value;
					break;
				}
			}
			return new ScannableDeviceLabel(device, text, password);
		}

		private ScannableDeviceLabel ParseAsRawText(string qrCode)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			string id;
			string password;
			GatewayType num = FindSuspectedGatewayType(qrCode, out id, out password);
			if (num == GatewayType.Unknown)
			{
				throw new ArgumentException("QR code not associated with a known gateway type.");
			}
			return new ScannableDeviceLabel(num, id, password);
		}

		private GatewayType FindSuspectedGatewayType(string qrCode, out string id, out string password)
		{
			Dictionary<ValueMatchType, string> val = new Dictionary<ValueMatchType, string>();
			foreach (GatewayType value in global::System.Enum.GetValues(typeof(GatewayType)))
			{
				if (value == GatewayType.Unknown)
				{
					continue;
				}
				bool flag = true;
				foreach (ValueMatchType value2 in global::System.Enum.GetValues(typeof(ValueMatchType)))
				{
					ReadOnlyDictionary<GatewayType, Regex> val2 = SharedGatewayParsingMetadata.ValueMatchDictionariesDictionary[value2];
					if (!val2.ContainsKey(value))
					{
						val[value2] = string.Empty;
						continue;
					}
					Match val3 = val2[value].Match(qrCode);
					if (((Group)val3).Success)
					{
						string text = SharedGatewayParsingMetadata.ValueMatchGroupKeys[value2];
						TransformTextDelegate transformTextDelegate = SanitizationMethodsDictionary[value2];
						val[value2] = transformTextDelegate(((Capture)val3.Groups[text]).Value, value);
						continue;
					}
					flag = false;
					break;
				}
				if (flag)
				{
					id = val[ValueMatchType.Id];
					password = val[ValueMatchType.Password];
					return value;
				}
			}
			id = null;
			password = null;
			return GatewayType.Unknown;
		}

		private static string SanitizeId(string idText, GatewayType gatewayType)
		{
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			switch (gatewayType)
			{
			case GatewayType.Bluetooth:
				return BluetoothGatewayIdPrefix + idText;
			case GatewayType.WiFi:
				return WifiGatewayIdPrefix + idText.ToUpper();
			case GatewayType.Aquafi:
				return AquafiGatewayIdPrefix + idText;
			case GatewayType.SonixCamera:
			case GatewayType.BatteryMonitor:
				return idText;
			default:
				throw new ArgumentOutOfRangeException("gatewayType", (object)gatewayType, (string)null);
			}
		}

		private string SanitizePassword(string passwordText, GatewayType gatewayType)
		{
			return passwordText.ToUpper();
		}
	}
	public class ScannableDeviceLabel
	{
		[field: CompilerGenerated]
		public GatewayType Device
		{
			[CompilerGenerated]
			get;
		}

		[field: CompilerGenerated]
		public string Id
		{
			[CompilerGenerated]
			get;
		}

		[field: CompilerGenerated]
		public string Password
		{
			[CompilerGenerated]
			get;
		}

		public ScannableDeviceLabel(GatewayType device, string id, string password)
		{
			Device = device;
			Id = id;
			Password = password;
		}

		public ParsedLabelTextResult ToParsedLabelText()
		{
			return new ParsedLabelTextResult(Device, Id, Password);
		}
	}
	public abstract class ScanResultParser
	{
		private static readonly ErrorScanResultParser UnrecognizedCode = new ErrorScanResultParser("Unrecognized QR code");

		private static readonly ErrorScanResultParser MissingRequireKeys = new ErrorScanResultParser("Missing required uri query keys");

		public static ScanResultParser TryParseQrCode(string qrCodeString)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if (IsUriString(qrCodeString))
				{
					NameValueCollection val = HttpUtility.ParseQueryString(new Uri(qrCodeString, (UriKind)1).Query);
					if (Enumerable.Contains<string>((global::System.Collections.Generic.IEnumerable<string>)val.AllKeys, "devid") && Enumerable.Contains<string>((global::System.Collections.Generic.IEnumerable<string>)val.AllKeys, "pw"))
					{
						return ParseGatewayQrCode(val);
					}
					if ((Enumerable.Contains<string>((global::System.Collections.Generic.IEnumerable<string>)val.AllKeys, "MAC") && Enumerable.Contains<string>((global::System.Collections.Generic.IEnumerable<string>)val.AllKeys, "DT")) || Enumerable.Contains<string>((global::System.Collections.Generic.IEnumerable<string>)val.AllKeys, "DN"))
					{
						return ParseDeviceQrCode(val);
					}
				}
				else
				{
					string[] array = qrCodeString.Split(" ", (StringSplitOptions)0) ?? global::System.Array.Empty<string>();
					if (array.Length > 1 && array[0].StartsWith("SSID:") && array[1].StartsWith("INSIGHT"))
					{
						return new WiFiDeviceScanResultParser(array[1], string.Empty);
					}
				}
				return UnrecognizedCode;
			}
			catch (global::System.Exception ex)
			{
				return new ErrorScanResultParser($"Invalid device QR code; {((MemberInfo)ex.GetType()).Name}-{ex.Message}; QR code URI: \"{qrCodeString}\"");
			}
		}

		private static ScanResultParser ParseGatewayQrCode(NameValueCollection valueCollection)
		{
			string text = valueCollection.Get("devid");
			string text2 = valueCollection.Get("pw");
			if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(text2))
			{
				return MissingRequireKeys;
			}
			return new GatewayScanResultParser(text, text2);
		}

		private static ScanResultParser ParseDeviceQrCode(NameValueCollection valueCollection)
		{
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Expected O, but got Unknown
			string text = valueCollection.Get("DT");
			string text2 = valueCollection.Get("DN");
			string text3 = valueCollection.Get("MAC") ?? string.Empty;
			if (string.IsNullOrEmpty(text3))
			{
				return MissingRequireKeys;
			}
			text3 = text3.Replace(":", "");
			MAC mac = new MAC(PhysicalAddress.Parse(text3.Trim().ToUpper()).GetAddressBytes());
			if (!string.IsNullOrWhiteSpace(text))
			{
				return ProcessQrCodeData(text, mac);
			}
			if (!string.IsNullOrWhiteSpace(text2))
			{
				return ProcessLegacyQrCodeData(text2, mac);
			}
			return MissingRequireKeys;
		}

		private static ScanResultParser ProcessQrCodeData(string deviceType, MAC mac)
		{
			byte b;
			try
			{
				b = (byte)Convert.ToInt32(deviceType, 16);
			}
			catch
			{
				return UnrecognizedCode;
			}
			return b switch
			{
				49 => new DeviceScanResultParser(DEVICE_TYPE.op_Implicit((byte)49), mac), 
				47 => new DeviceScanResultParser(DEVICE_TYPE.op_Implicit((byte)47), mac), 
				_ => UnrecognizedCode, 
			};
		}

		private static ScanResultParser ProcessLegacyQrCodeData(string deviceName, MAC mac)
		{
			Match obj = DeviceScanResultParser.DeviceNameRegEx.Match(deviceName);
			Group obj2 = obj.Groups["device_type_prefix"];
			string text = ((obj2 != null) ? ((Capture)obj2).Value : null) ?? string.Empty;
			Group obj3 = obj.Groups["part_number"];
			if (((obj3 != null) ? ((Capture)obj3).Value : null) == null)
			{
				_ = string.Empty;
			}
			if (!(text == "BMO"))
			{
				if (text == "AWS")
				{
					return new DeviceScanResultParser(DEVICE_TYPE.op_Implicit((byte)47), mac);
				}
				return UnrecognizedCode;
			}
			return new DeviceScanResultParser(DEVICE_TYPE.op_Implicit((byte)49), mac);
		}

		private static bool IsUriString(string qrCodeString)
		{
			return qrCodeString.Trim().StartsWith("http");
		}
	}
	public class ErrorScanResultParser : ScanResultParser
	{
		[field: CompilerGenerated]
		public string Error
		{
			[CompilerGenerated]
			get;
		}

		public ErrorScanResultParser(string error)
		{
			Error = error;
		}
	}
	public class GatewayScanResultParser : ScanResultParser
	{
		public const string DEVICE_NAME_KEY = "devid";

		public const string PW_KEY = "pw";

		[field: CompilerGenerated]
		public string Name
		{
			[CompilerGenerated]
			get;
		}

		[field: CompilerGenerated]
		public string Pin
		{
			[CompilerGenerated]
			get;
		}

		public GatewayScanResultParser(string name, string pin)
		{
			Name = name;
			Pin = pin;
		}
	}
	public class DeviceScanResultParser : ScanResultParser
	{
		public const string DEVICE_TYPE_KEY = "DT";

		public const string DEVICE_NAME_KEY = "DN";

		public const string MAC_KEY = "MAC";

		public const string BATTERY_MONITOR_DEVICE_TYPE_PREFIX = "BMO";

		public const string WIND_SENSOR_DEVICE_TYPE_PREFIX = "AWS";

		public static readonly Regex DeviceNameRegEx = new Regex("LCI(?<device_type_prefix>.+?)(?<part_number>[0-9]{5})");

		public const string DeviceTypePrefixGroup = "device_type_prefix";

		public const string PartNumberGroup = "part_number";

		[field: CompilerGenerated]
		public DEVICE_TYPE Type
		{
			[CompilerGenerated]
			get;
		}

		[field: CompilerGenerated]
		public MAC Mac
		{
			[CompilerGenerated]
			get;
		}

		public DeviceScanResultParser(DEVICE_TYPE type, MAC mac)
		{
			Type = type;
			Mac = mac;
		}
	}
	public class WiFiDeviceScanResultParser : ScanResultParser
	{
		public const string DEVICE_SSID_KEY = "SSID:";

		public const string DEVICE_SSID_VALUE_KEY = "INSIGHT";

		[field: CompilerGenerated]
		public string Ssid
		{
			[CompilerGenerated]
			get;
		}

		[field: CompilerGenerated]
		public string Password
		{
			[CompilerGenerated]
			get;
		}

		public WiFiDeviceScanResultParser(string ssid, string password)
		{
			Ssid = ssid;
			Password = password;
		}
	}
	[XamlFilePath("ScanWithCamera/ScanView.xaml")]
	public class ScanView : ContentView
	{
		[CompilerGenerated]
		private sealed class <>c__DisplayClass24_0
		{
			[StructLayout((LayoutKind)3)]
			private struct <<OnPropertyChanged>b__0>d : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncTaskMethodBuilder <>t__builder;

				public <>c__DisplayClass24_0 <>4__this;

				private <>c__DisplayClass24_1 <>8__1;

				private TaskAwaiter<PermissionStatus> <>u__1;

				private TaskAwaiter <>u__2;

				private void MoveNext()
				{
					//IL_0085: Unknown result type (might be due to invalid IL or missing references)
					//IL_008a: Unknown result type (might be due to invalid IL or missing references)
					//IL_0091: Unknown result type (might be due to invalid IL or missing references)
					//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
					//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
					//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
					//IL_018f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0194: Unknown result type (might be due to invalid IL or missing references)
					//IL_019c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0052: Unknown result type (might be due to invalid IL or missing references)
					//IL_0057: Unknown result type (might be due to invalid IL or missing references)
					//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
					//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
					//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
					//IL_00aa: Invalid comparison between Unknown and I4
					//IL_006b: Unknown result type (might be due to invalid IL or missing references)
					//IL_006c: Unknown result type (might be due to invalid IL or missing references)
					//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
					//IL_00ae: Invalid comparison between Unknown and I4
					//IL_015a: Unknown result type (might be due to invalid IL or missing references)
					//IL_015f: Unknown result type (might be due to invalid IL or missing references)
					//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
					//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
					//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
					//IL_0174: Unknown result type (might be due to invalid IL or missing references)
					//IL_0176: Unknown result type (might be due to invalid IL or missing references)
					//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
					//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					<>c__DisplayClass24_0 <>c__DisplayClass24_ = <>4__this;
					try
					{
						if ((uint)num <= 2u)
						{
							goto IL_003b;
						}
						<>8__1 = new <>c__DisplayClass24_1();
						<>8__1.CS$<>8__locals1 = <>c__DisplayClass24_;
						<>8__1.cameraBarcodeReaderView = null;
						goto IL_01c6;
						IL_003b:
						try
						{
							TaskAwaiter<PermissionStatus> val2;
							TaskAwaiter val;
							PermissionStatus result;
							switch (num)
							{
							default:
								val2 = Permissions.CheckStatusAsync<Camera>().GetAwaiter();
								if (!val2.IsCompleted)
								{
									num = (<>1__state = 0);
									<>u__1 = val2;
									((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, <<OnPropertyChanged>b__0>d>(ref val2, ref this);
									return;
								}
								goto IL_00a0;
							case 0:
								val2 = <>u__1;
								<>u__1 = default(TaskAwaiter<PermissionStatus>);
								num = (<>1__state = -1);
								goto IL_00a0;
							case 1:
								val = <>u__2;
								<>u__2 = default(TaskAwaiter);
								num = (<>1__state = -1);
								goto IL_010e;
							case 2:
								{
									val = <>u__2;
									<>u__2 = default(TaskAwaiter);
									num = (<>1__state = -1);
									break;
								}
								IL_010e:
								((TaskAwaiter)(ref val)).GetResult();
								goto end_IL_003b;
								IL_00a0:
								result = val2.GetResult();
								if ((int)result != 3 && (int)result != 5)
								{
									val = global::System.Threading.Tasks.Task.Delay(100, <>c__DisplayClass24_.permissionCt).GetAwaiter();
									if (!((TaskAwaiter)(ref val)).IsCompleted)
									{
										num = (<>1__state = 1);
										<>u__2 = val;
										((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <<OnPropertyChanged>b__0>d>(ref val, ref this);
										return;
									}
									goto IL_010e;
								}
								val = <>c__DisplayClass24_.<>4__this._mainThreadService.InvokeOnMainThreadAsync((Func<global::System.Threading.Tasks.Task>)([AsyncStateMachine(typeof(<>c__DisplayClass24_1.<<OnPropertyChanged>b__1>d))] () =>
								{
									//IL_0002: Unknown result type (might be due to invalid IL or missing references)
									//IL_0007: Unknown result type (might be due to invalid IL or missing references)
									<>c__DisplayClass24_1.<<OnPropertyChanged>b__1>d <<OnPropertyChanged>b__1>d = default(<>c__DisplayClass24_1.<<OnPropertyChanged>b__1>d);
									<<OnPropertyChanged>b__1>d.<>t__builder = AsyncTaskMethodBuilder.Create();
									<<OnPropertyChanged>b__1>d.<>4__this = <>8__1;
									<<OnPropertyChanged>b__1>d.<>1__state = -1;
									((AsyncTaskMethodBuilder)(ref <<OnPropertyChanged>b__1>d.<>t__builder)).Start<<>c__DisplayClass24_1.<<OnPropertyChanged>b__1>d>(ref <<OnPropertyChanged>b__1>d);
									return ((AsyncTaskMethodBuilder)(ref <<OnPropertyChanged>b__1>d.<>t__builder)).Task;
								})).GetAwaiter();
								if (!((TaskAwaiter)(ref val)).IsCompleted)
								{
									num = (<>1__state = 2);
									<>u__2 = val;
									((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <<OnPropertyChanged>b__0>d>(ref val, ref this);
									return;
								}
								break;
							}
							((TaskAwaiter)(ref val)).GetResult();
							if (<>8__1.cameraBarcodeReaderView == null)
							{
								goto end_IL_003b;
							}
							goto end_IL_000e;
							end_IL_003b:;
						}
						catch
						{
						}
						goto IL_01c6;
						IL_01c6:
						if (<>8__1.cameraBarcodeReaderView == null && !((CancellationToken)(ref <>c__DisplayClass24_.permissionCt)).IsCancellationRequested)
						{
							goto IL_003b;
						}
						end_IL_000e:;
					}
					catch (global::System.Exception exception)
					{
						<>1__state = -2;
						<>8__1 = null;
						((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
						return;
					}
					<>1__state = -2;
					<>8__1 = null;
					((AsyncTaskMethodBuilder)(ref <>t__builder)).SetResult();
				}

				[DebuggerHidden]
				private void SetStateMachine(IAsyncStateMachine stateMachine)
				{
					((AsyncTaskMethodBuilder)(ref <>t__builder)).SetStateMachine(stateMachine);
				}
			}

			public CancellationToken permissionCt;

			public ScanView <>4__this;

			public Action<EventPattern<BarcodeDetectionEventArgs>> <>9__4;

			[AsyncStateMachine(typeof(<<OnPropertyChanged>b__0>d))]
			internal global::System.Threading.Tasks.Task? <OnPropertyChanged>b__0()
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				<<OnPropertyChanged>b__0>d <<OnPropertyChanged>b__0>d = default(<<OnPropertyChanged>b__0>d);
				<<OnPropertyChanged>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<OnPropertyChanged>b__0>d.<>4__this = this;
				<<OnPropertyChanged>b__0>d.<>1__state = -1;
				((AsyncTaskMethodBuilder)(ref <<OnPropertyChanged>b__0>d.<>t__builder)).Start<<<OnPropertyChanged>b__0>d>(ref <<OnPropertyChanged>b__0>d);
				return ((AsyncTaskMethodBuilder)(ref <<OnPropertyChanged>b__0>d.<>t__builder)).Task;
			}

			internal void <OnPropertyChanged>b__4(EventPattern<BarcodeDetectionEventArgs> ep)
			{
				ICommand barcodesDetectedCommand = <>4__this.BarcodesDetectedCommand;
				if (((barcodesDetectedCommand != null) ? new bool?(barcodesDetectedCommand.CanExecute((object)((EventPattern<object, BarcodeDetectionEventArgs>)(object)ep).EventArgs)) : ((bool?)null)) ?? false)
				{
					ICommand barcodesDetectedCommand2 = <>4__this.BarcodesDetectedCommand;
					if (barcodesDetectedCommand2 != null)
					{
						barcodesDetectedCommand2.Execute((object)((EventPattern<object, BarcodeDetectionEventArgs>)(object)ep).EventArgs);
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class <>c__DisplayClass24_1
		{
			[StructLayout((LayoutKind)3)]
			private struct <<OnPropertyChanged>b__1>d : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncTaskMethodBuilder <>t__builder;

				public <>c__DisplayClass24_1 <>4__this;

				private <>c__DisplayClass24_2 <>8__1;

				private ValueTaskAwaiter <>u__1;

				private void MoveNext()
				{
					//IL_0072: Unknown result type (might be due to invalid IL or missing references)
					//IL_0077: Unknown result type (might be due to invalid IL or missing references)
					//IL_007e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0032: Unknown result type (might be due to invalid IL or missing references)
					//IL_003f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0044: Unknown result type (might be due to invalid IL or missing references)
					//IL_0058: Unknown result type (might be due to invalid IL or missing references)
					//IL_0059: Unknown result type (might be due to invalid IL or missing references)
					//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
					//IL_00ef: Expected O, but got Unknown
					//IL_0113: Unknown result type (might be due to invalid IL or missing references)
					//IL_011d: Expected O, but got Unknown
					//IL_0141: Unknown result type (might be due to invalid IL or missing references)
					//IL_014b: Expected O, but got Unknown
					//IL_016f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0179: Expected O, but got Unknown
					int num = <>1__state;
					<>c__DisplayClass24_1 <>c__DisplayClass24_ = <>4__this;
					try
					{
						ValueTaskAwaiter val;
						if (num != 0)
						{
							<>8__1 = new <>c__DisplayClass24_2();
							val = ((LazyView)<>c__DisplayClass24_.CS$<>8__locals1.<>4__this.LazyView).LoadViewAsync(<>c__DisplayClass24_.CS$<>8__locals1.permissionCt).GetAwaiter();
							if (!((ValueTaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<ValueTaskAwaiter, <<OnPropertyChanged>b__1>d>(ref val, ref this);
								return;
							}
						}
						else
						{
							val = <>u__1;
							<>u__1 = default(ValueTaskAwaiter);
							num = (<>1__state = -1);
						}
						((ValueTaskAwaiter)(ref val)).GetResult();
						ref CameraBarcodeReaderView reference = ref <>c__DisplayClass24_.cameraBarcodeReaderView;
						View content = ((ContentView)<>c__DisplayClass24_.CS$<>8__locals1.<>4__this.LazyView).Content;
						reference = (CameraBarcodeReaderView)(object)((content is CameraBarcodeReaderView) ? content : null);
						if (<>c__DisplayClass24_.cameraBarcodeReaderView != null)
						{
							((BindableObject)<>c__DisplayClass24_.cameraBarcodeReaderView).SetBinding(CameraBarcodeReaderView.OptionsProperty, (BindingBase)new Binding(BarcodeReaderOptionsProperty.PropertyName, (BindingMode)0, (IValueConverter)null, (object)null, (string)null, (object)<>c__DisplayClass24_.CS$<>8__locals1.<>4__this));
							((BindableObject)<>c__DisplayClass24_.cameraBarcodeReaderView).SetBinding(CameraBarcodeReaderView.IsDetectingProperty, (BindingBase)new Binding(IsDetectingProperty.PropertyName, (BindingMode)0, (IValueConverter)null, (object)null, (string)null, (object)<>c__DisplayClass24_.CS$<>8__locals1.<>4__this));
							((BindableObject)<>c__DisplayClass24_.cameraBarcodeReaderView).SetBinding(CameraBarcodeReaderView.CameraLocationProperty, (BindingBase)new Binding(CameraLocationProperty.PropertyName, (BindingMode)0, (IValueConverter)null, (object)null, (string)null, (object)<>c__DisplayClass24_.CS$<>8__locals1.<>4__this));
							((BindableObject)<>c__DisplayClass24_.cameraBarcodeReaderView).SetBinding(CameraBarcodeReaderView.IsTorchOnProperty, (BindingBase)new Binding(IsTorchOnProperty.PropertyName, (BindingMode)0, (IValueConverter)null, (object)null, (string)null, (object)<>c__DisplayClass24_.CS$<>8__locals1.<>4__this));
							<>8__1.cbrv = <>c__DisplayClass24_.cameraBarcodeReaderView;
							<>c__DisplayClass24_.CS$<>8__locals1.<>4__this._barcodeDetectedDisposable = ObservableExtensions.Subscribe<EventPattern<BarcodeDetectionEventArgs>>(Observable.FromEventPattern<BarcodeDetectionEventArgs>((Action<EventHandler<BarcodeDetectionEventArgs>>)delegate(EventHandler<BarcodeDetectionEventArgs> h)
							{
								<>8__1.cbrv.BarcodesDetected += h;
							}, (Action<EventHandler<BarcodeDetectionEventArgs>>)delegate(EventHandler<BarcodeDetectionEventArgs> h)
							{
								<>8__1.cbrv.BarcodesDetected -= h;
							}), (Action<EventPattern<BarcodeDetectionEventArgs>>)delegate(EventPattern<BarcodeDetectionEventArgs> ep)
							{
								ICommand barcodesDetectedCommand = <>c__DisplayClass24_.CS$<>8__locals1.<>4__this.BarcodesDetectedCommand;
								if (((barcodesDetectedCommand != null) ? new bool?(barcodesDetectedCommand.CanExecute((object)((EventPattern<object, BarcodeDetectionEventArgs>)(object)ep).EventArgs)) : ((bool?)null)) ?? false)
								{
									ICommand barcodesDetectedCommand2 = <>c__DisplayClass24_.CS$<>8__locals1.<>4__this.BarcodesDetectedCommand;
									if (barcodesDetectedCommand2 != null)
									{
										barcodesDetectedCommand2.Execute((object)((EventPattern<object, BarcodeDetectionEventArgs>)(object)ep).EventArgs);
									}
								}
							});
						}
					}
					catch (global::System.Exception exception)
					{
						<>1__state = -2;
						<>8__1 = null;
						((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
						return;
					}
					<>1__state = -2;
					<>8__1 = null;
					((AsyncTaskMethodBuilder)(ref <>t__builder)).SetResult();
				}

				[DebuggerHidden]
				private void SetStateMachine(IAsyncStateMachine stateMachine)
				{
					((AsyncTaskMethodBuilder)(ref <>t__builder)).SetStateMachine(stateMachine);
				}
			}

			public CameraBarcodeReaderView cameraBarcodeReaderView;

			public <>c__DisplayClass24_0 CS$<>8__locals1;

			public Func<global::System.Threading.Tasks.Task> <>9__1;

			[AsyncStateMachine(typeof(<<OnPropertyChanged>b__1>d))]
			internal global::System.Threading.Tasks.Task <OnPropertyChanged>b__1()
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				<<OnPropertyChanged>b__1>d <<OnPropertyChanged>b__1>d = default(<<OnPropertyChanged>b__1>d);
				<<OnPropertyChanged>b__1>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<OnPropertyChanged>b__1>d.<>4__this = this;
				<<OnPropertyChanged>b__1>d.<>1__state = -1;
				((AsyncTaskMethodBuilder)(ref <<OnPropertyChanged>b__1>d.<>t__builder)).Start<<<OnPropertyChanged>b__1>d>(ref <<OnPropertyChanged>b__1>d);
				return ((AsyncTaskMethodBuilder)(ref <<OnPropertyChanged>b__1>d.<>t__builder)).Task;
			}
		}

		[CompilerGenerated]
		private sealed class <>c__DisplayClass24_2
		{
			public CameraBarcodeReaderView cbrv;

			internal void <OnPropertyChanged>b__2(EventHandler<BarcodeDetectionEventArgs> h)
			{
				cbrv.BarcodesDetected += h;
			}

			internal void <OnPropertyChanged>b__3(EventHandler<BarcodeDetectionEventArgs> h)
			{
				cbrv.BarcodesDetected -= h;
			}
		}

		private readonly IMainThreadService _mainThreadService;

		private CancellationTokenSource? _permissionCts;

		private global::System.IDisposable? _barcodeDetectedDisposable;

		public static readonly BindableProperty BarcodeReaderOptionsProperty = BindableProperty.Create("BarcodeReaderOptions", typeof(BarcodeReaderOptions), typeof(ScanView), (object)null, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)null, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)null, (CreateDefaultValueDelegate)null);

		public static readonly BindableProperty IsDetectingProperty = BindableProperty.Create("IsDetecting", typeof(bool), typeof(ScanView), (object)false, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)null, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)null, (CreateDefaultValueDelegate)null);

		public static readonly BindableProperty CameraLocationProperty = BindableProperty.Create("CameraLocation", typeof(CameraLocation), typeof(ScanView), (object)(CameraLocation)0, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)null, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)null, (CreateDefaultValueDelegate)null);

		public static readonly BindableProperty IsTorchOnProperty = BindableProperty.Create("IsTorchOn", typeof(bool), typeof(ScanView), (object)false, (BindingMode)1, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)null, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)null, (CreateDefaultValueDelegate)null);

		public static readonly BindableProperty BarcodesDetectedCommandProperty = BindableProperty.Create("BarcodesDetectedCommand", typeof(ICommand), typeof(ScanView), (object)null, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)null, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)null, (CreateDefaultValueDelegate)null);

		[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
		private LazyView<CameraBarcodeReaderView> LazyView;

		public BarcodeReaderOptions BarcodeReaderOptions
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected O, but got Unknown
				return (BarcodeReaderOptions)((BindableObject)this).GetValue(BarcodeReaderOptionsProperty);
			}
			set
			{
				((BindableObject)this).SetValue(BarcodeReaderOptionsProperty, (object)value);
			}
		}

		public bool IsDetecting
		{
			get
			{
				return (bool)((BindableObject)this).GetValue(IsDetectingProperty);
			}
			set
			{
				((BindableObject)this).SetValue(IsDetectingProperty, (object)value);
			}
		}

		public CameraLocation CameraLocation
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				return (CameraLocation)((BindableObject)this).GetValue(CameraLocationProperty);
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				((BindableObject)this).SetValue(CameraLocationProperty, (object)value);
			}
		}

		public bool IsTorchOn
		{
			get
			{
				return (bool)((BindableObject)this).GetValue(IsTorchOnProperty);
			}
			set
			{
				((BindableObject)this).SetValue(IsTorchOnProperty, (object)value);
			}
		}

		public ICommand BarcodesDetectedCommand
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected O, but got Unknown
				return (ICommand)((BindableObject)this).GetValue(BarcodesDetectedCommandProperty);
			}
			set
			{
				((BindableObject)this).SetValue(BarcodesDetectedCommandProperty, (object)value);
			}
		}

		public ScanView()
		{
			Application current = Application.Current;
			object obj;
			if (current == null)
			{
				obj = null;
			}
			else
			{
				IElementHandler handler = ((Element)current).Handler;
				if (handler == null)
				{
					obj = null;
				}
				else
				{
					IMauiContext mauiContext = handler.MauiContext;
					obj = ((mauiContext != null) ? ServiceProviderServiceExtensions.GetRequiredService<IMainThreadService>(mauiContext.Services) : null);
				}
			}
			if (obj == null)
			{
				throw new global::System.Exception("MAUI Application cannot be null");
			}
			_mainThreadService = (IMainThreadService)obj;
			InitializeComponent();
		}

		protected override void OnPropertyChanged(string? propertyName = null)
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Expected O, but got Unknown
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			<>c__DisplayClass24_0 CS$<>8__locals4 = new <>c__DisplayClass24_0();
			CS$<>8__locals4.<>4__this = this;
			((Element)this).OnPropertyChanged(propertyName);
			if (!string.Equals(propertyName, VisualElement.WindowProperty.PropertyName))
			{
				return;
			}
			global::System.IDisposable? barcodeDetectedDisposable = _barcodeDetectedDisposable;
			if (barcodeDetectedDisposable != null)
			{
				DisposableExtensions.TryDispose(barcodeDetectedDisposable);
			}
			CancellationTokenSource? permissionCts = _permissionCts;
			if (permissionCts != null)
			{
				CancellationTokenSourceExtensions.TryCancelAndDispose(permissionCts);
			}
			_permissionCts = new CancellationTokenSource();
			CS$<>8__locals4.permissionCt = _permissionCts.Token;
			if (((VisualElement)this).Window != null)
			{
				global::System.Threading.Tasks.Task.Run((Func<global::System.Threading.Tasks.Task>)([AsyncStateMachine(typeof(<>c__DisplayClass24_0.<<OnPropertyChanged>b__0>d))] () =>
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					<>c__DisplayClass24_0.<<OnPropertyChanged>b__0>d <<OnPropertyChanged>b__0>d = default(<>c__DisplayClass24_0.<<OnPropertyChanged>b__0>d);
					<<OnPropertyChanged>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
					<<OnPropertyChanged>b__0>d.<>4__this = CS$<>8__locals4;
					<<OnPropertyChanged>b__0>d.<>1__state = -1;
					((AsyncTaskMethodBuilder)(ref <<OnPropertyChanged>b__0>d.<>t__builder)).Start<<>c__DisplayClass24_0.<<OnPropertyChanged>b__0>d>(ref <<OnPropertyChanged>b__0>d);
					return ((AsyncTaskMethodBuilder)(ref <<OnPropertyChanged>b__0>d.<>t__builder)).Task;
				}), CS$<>8__locals4.permissionCt);
			}
		}

		[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
		[MemberNotNull("LazyView")]
		private void InitializeComponent()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			LazyView<CameraBarcodeReaderView> val = new LazyView<CameraBarcodeReaderView>();
			ScanView scanView;
			NameScope val2 = (NameScope)(((object)NameScope.GetNameScope((BindableObject)(object)(scanView = this))) ?? ((object)new NameScope()));
			NameScope.SetNameScope((BindableObject)(object)scanView, (INameScope)(object)val2);
			((Element)val).transientNamescope = (INameScope)(object)val2;
			((INameScope)val2).RegisterName("LazyView", (object)val);
			if (((Element)val).StyleId == null)
			{
				((Element)val).StyleId = "LazyView";
			}
			LazyView = val;
			((BindableObject)scanView).SetValue(ContentView.ContentProperty, (object)val);
		}
	}
	[XamlFilePath("ScanWithCamera/ScanWithCameraPage.xaml")]
	public class ScanWithCameraPage : TopAppBarPage
	{
		public ScanWithCameraPage()
		{
			InitializeComponent();
		}

		[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
		private void InitializeComponent()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected O, but got Unknown
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Expected O, but got Unknown
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Expected O, but got Unknown
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Expected O, but got Unknown
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected O, but got Unknown
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Expected O, but got Unknown
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Expected O, but got Unknown
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Expected O, but got Unknown
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c4: Expected O, but got Unknown
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d1: Expected O, but got Unknown
			//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02db: Expected O, but got Unknown
			//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f5: Expected O, but got Unknown
			//IL_0300: Unknown result type (might be due to invalid IL or missing references)
			//IL_0305: Unknown result type (might be due to invalid IL or missing references)
			//IL_030b: Expected O, but got Unknown
			//IL_030d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0312: Unknown result type (might be due to invalid IL or missing references)
			//IL_0318: Expected O, but got Unknown
			//IL_0318: Unknown result type (might be due to invalid IL or missing references)
			//IL_0322: Expected O, but got Unknown
			//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05da: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f3: Expected O, but got Unknown
			//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f8: Expected O, but got Unknown
			//IL_05fd: Expected O, but got Unknown
			//IL_0652: Unknown result type (might be due to invalid IL or missing references)
			//IL_0657: Unknown result type (might be due to invalid IL or missing references)
			//IL_0666: Unknown result type (might be due to invalid IL or missing references)
			//IL_0670: Expected O, but got Unknown
			//IL_066b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0675: Expected O, but got Unknown
			//IL_067a: Expected O, but got Unknown
			//IL_06c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0742: Unknown result type (might be due to invalid IL or missing references)
			//IL_0747: Unknown result type (might be due to invalid IL or missing references)
			//IL_0756: Unknown result type (might be due to invalid IL or missing references)
			//IL_0760: Expected O, but got Unknown
			//IL_075b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0765: Expected O, but got Unknown
			//IL_076a: Expected O, but got Unknown
			//IL_07d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ef: Expected O, but got Unknown
			//IL_07ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f4: Expected O, but got Unknown
			//IL_07f9: Expected O, but got Unknown
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			BindingExtension val = new BindingExtension();
			BindingExtension val2 = new BindingExtension();
			BindingExtension val3 = new BindingExtension();
			BindingExtension val4 = new BindingExtension();
			BarcodeReaderOptions val5 = new BarcodeReaderOptions();
			ScanView scanView = new ScanView();
			DynamicResourceExtension val6 = new DynamicResourceExtension();
			BoxView val7 = new BoxView();
			DynamicResourceExtension val8 = new DynamicResourceExtension();
			BoxView val9 = new BoxView();
			BoxView val10 = new BoxView();
			DynamicResourceExtension val11 = new DynamicResourceExtension();
			BoxView val12 = new BoxView();
			DynamicResourceExtension val13 = new DynamicResourceExtension();
			BoxView val14 = new BoxView();
			Grid val15 = new Grid();
			ScanWithCameraPage scanWithCameraPage;
			NameScope val16 = (NameScope)(((object)NameScope.GetNameScope((BindableObject)(object)(scanWithCameraPage = this))) ?? ((object)new NameScope()));
			NameScope.SetNameScope((BindableObject)(object)scanWithCameraPage, (INameScope)(object)val16);
			((Element)val15).transientNamescope = (INameScope)(object)val16;
			((Element)scanView).transientNamescope = (INameScope)(object)val16;
			((Element)val7).transientNamescope = (INameScope)(object)val16;
			((Element)val9).transientNamescope = (INameScope)(object)val16;
			((Element)val10).transientNamescope = (INameScope)(object)val16;
			((Element)val12).transientNamescope = (INameScope)(object)val16;
			((Element)val14).transientNamescope = (INameScope)(object)val16;
			val.Path = "Title";
			val.TypedBinding = (TypedBindingBase)(object)new TypedBinding<ScanWithCameraViewModel, string>((Func<ScanWithCameraViewModel, ValueTuple<string, bool>>)([CompilerGenerated] (ScanWithCameraViewModel P_0) => (P_0 != null) ? new ValueTuple<string, bool>(P_0.Title, true) : default(ValueTuple<string, bool>)), (Action<ScanWithCameraViewModel, string>)([CompilerGenerated] (ScanWithCameraViewModel P_0, string P_1) =>
			{
				if (P_0 != null)
				{
					P_0.Title = P_1;
				}
			}), new Tuple<Func<ScanWithCameraViewModel, object>, string>[1]
			{
				new Tuple<Func<ScanWithCameraViewModel, object>, string>((Func<ScanWithCameraViewModel, object>)([CompilerGenerated] (ScanWithCameraViewModel P_0) => P_0), "Title")
			});
			((BindingBase)val.TypedBinding).Mode = val.Mode;
			val.TypedBinding.Converter = val.Converter;
			val.TypedBinding.ConverterParameter = val.ConverterParameter;
			((BindingBase)val.TypedBinding).StringFormat = val.StringFormat;
			val.TypedBinding.Source = val.Source;
			val.TypedBinding.UpdateSourceEventName = val.UpdateSourceEventName;
			((BindingBase)val.TypedBinding).FallbackValue = val.FallbackValue;
			((BindingBase)val.TypedBinding).TargetNullValue = val.TargetNullValue;
			BindingBase typedBinding = (BindingBase)(object)val.TypedBinding;
			((BindableObject)scanWithCameraPage).SetBinding(AppBarPage.TitleProperty, typedBinding);
			val2.Path = "BackCommand";
			val2.TypedBinding = (TypedBindingBase)(object)new TypedBinding<ScanWithCameraViewModel, IAsyncRelayCommand>((Func<ScanWithCameraViewModel, ValueTuple<IAsyncRelayCommand, bool>>)([CompilerGenerated] (ScanWithCameraViewModel P_0) => (P_0 != null) ? new ValueTuple<IAsyncRelayCommand, bool>(P_0.BackCommand, true) : default(ValueTuple<IAsyncRelayCommand, bool>)), (Action<ScanWithCameraViewModel, IAsyncRelayCommand>)null, new Tuple<Func<ScanWithCameraViewModel, object>, string>[1]
			{
				new Tuple<Func<ScanWithCameraViewModel, object>, string>((Func<ScanWithCameraViewModel, object>)([CompilerGenerated] (ScanWithCameraViewModel P_0) => P_0), "BackCommand")
			});
			((BindingBase)val2.TypedBinding).Mode = val2.Mode;
			val2.TypedBinding.Converter = val2.Converter;
			val2.TypedBinding.ConverterParameter = val2.ConverterParameter;
			((BindingBase)val2.TypedBinding).StringFormat = val2.StringFormat;
			val2.TypedBinding.Source = val2.Source;
			val2.TypedBinding.UpdateSourceEventName = val2.UpdateSourceEventName;
			((BindingBase)val2.TypedBinding).FallbackValue = val2.FallbackValue;
			((BindingBase)val2.TypedBinding).TargetNullValue = val2.TargetNullValue;
			BindingBase typedBinding2 = (BindingBase)(object)val2.TypedBinding;
			((BindableObject)scanWithCameraPage).SetBinding(AppBarPage.NavigationCommandProperty, typedBinding2);
			((BindableObject)val15).SetValue(Grid.RowDefinitionsProperty, (object)new RowDefinitionCollection((RowDefinition[])(object)new RowDefinition[3]
			{
				new RowDefinition(GridLength.Star),
				new RowDefinition(new GridLength(300.0)),
				new RowDefinition(GridLength.Star)
			}));
			((BindableObject)val15).SetValue(Grid.ColumnDefinitionsProperty, (object)new ColumnDefinitionCollection((ColumnDefinition[])(object)new ColumnDefinition[3]
			{
				new ColumnDefinition(GridLength.Star),
				new ColumnDefinition(new GridLength(300.0)),
				new ColumnDefinition(GridLength.Star)
			}));
			((BindableObject)val15).SetValue(Grid.RowSpacingProperty, (object)0.0);
			((BindableObject)val15).SetValue(Grid.ColumnSpanProperty, (object)0);
			((BindableObject)scanView).SetValue(Grid.RowProperty, (object)0);
			((BindableObject)scanView).SetValue(Grid.ColumnProperty, (object)0);
			((BindableObject)scanView).SetValue(Grid.RowSpanProperty, (object)3);
			((BindableObject)scanView).SetValue(Grid.ColumnSpanProperty, (object)3);
			val3.Path = "IsScanning";
			val3.TypedBinding = (TypedBindingBase)(object)new TypedBinding<ScanWithCameraViewModel, bool>((Func<ScanWithCameraViewModel, ValueTuple<bool, bool>>)([CompilerGenerated] (ScanWithCameraViewModel P_0) => (P_0 != null) ? new ValueTuple<bool, bool>(P_0.IsScanning, true) : default(ValueTuple<bool, bool>)), (Action<ScanWithCameraViewModel, bool>)([CompilerGenerated] (ScanWithCameraViewModel P_0, bool P_1) =>
			{
				if (P_0 != null)
				{
					P_0.IsScanning = P_1;
				}
			}), new Tuple<Func<ScanWithCameraViewModel, object>, string>[1]
			{
				new Tuple<Func<ScanWithCameraViewModel, object>, string>((Func<ScanWithCameraViewModel, object>)([CompilerGenerated] (ScanWithCameraViewModel P_0) => P_0), "IsScanning")
			});
			((BindingBase)val3.TypedBinding).Mode = val3.Mode;
			val3.TypedBinding.Converter = val3.Converter;
			val3.TypedBinding.ConverterParameter = val3.ConverterParameter;
			((BindingBase)val3.TypedBinding).StringFormat = val3.StringFormat;
			val3.TypedBinding.Source = val3.Source;
			val3.TypedBinding.UpdateSourceEventName = val3.UpdateSourceEventName;
			((BindingBase)val3.TypedBinding).FallbackValue = val3.FallbackValue;
			((BindingBase)val3.TypedBinding).TargetNullValue = val3.TargetNullValue;
			BindingBase typedBinding3 = (BindingBase)(object)val3.TypedBinding;
			((BindableObject)scanView).SetBinding(ScanView.IsDetectingProperty, typedBinding3);
			val4.Path = "BarcodeDetectedCommand";
			val4.TypedBinding = (TypedBindingBase)(object)new TypedBinding<ScanWithCameraViewModel, IRelayCommand<BarcodeDetectionEventArgs>>((Func<ScanWithCameraViewModel, ValueTuple<IRelayCommand<BarcodeDetectionEventArgs>, bool>>)([CompilerGenerated] (ScanWithCameraViewModel P_0) => (P_0 != null) ? new ValueTuple<IRelayCommand<BarcodeDetectionEventArgs>, bool>(P_0.BarcodeDetectedCommand, true) : default(ValueTuple<IRelayCommand<BarcodeDetectionEventArgs>, bool>)), (Action<ScanWithCameraViewModel, IRelayCommand<BarcodeDetectionEventArgs>>)null, new Tuple<Func<ScanWithCameraViewModel, object>, string>[1]
			{
				new Tuple<Func<ScanWithCameraViewModel, object>, string>((Func<ScanWithCameraViewModel, object>)([CompilerGenerated] (ScanWithCameraViewModel P_0) => P_0), "BarcodeDetectedCommand")
			});
			((BindingBase)val4.TypedBinding).Mode = val4.Mode;
			val4.TypedBinding.Converter = val4.Converter;
			val4.TypedBinding.ConverterParameter = val4.ConverterParameter;
			((BindingBase)val4.TypedBinding).StringFormat = val4.StringFormat;
			val4.TypedBinding.Source = val4.Source;
			val4.TypedBinding.UpdateSourceEventName = val4.UpdateSourceEventName;
			((BindingBase)val4.TypedBinding).FallbackValue = val4.FallbackValue;
			((BindingBase)val4.TypedBinding).TargetNullValue = val4.TargetNullValue;
			BindingBase typedBinding4 = (BindingBase)(object)val4.TypedBinding;
			((BindableObject)scanView).SetBinding(ScanView.BarcodesDetectedCommandProperty, typedBinding4);
			val5.set_Formats((BarcodeFormat)2048);
			val5.set_TryHarder(true);
			((BindableObject)scanView).SetValue(ScanView.BarcodeReaderOptionsProperty, (object)val5);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val15).Children).Add((IView)(object)scanView);
			((BindableObject)val7).SetValue(Grid.RowProperty, (object)0);
			((BindableObject)val7).SetValue(Grid.ColumnProperty, (object)0);
			((BindableObject)val7).SetValue(Grid.ColumnSpanProperty, (object)3);
			val6.Key = "Scrim";
			XamlServiceProvider val17 = new XamlServiceProvider();
			val17.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(31, 22)));
			DynamicResource val18 = ((IMarkupExtension<DynamicResource>)(object)val6).ProvideValue((IServiceProvider)val17);
			((IDynamicResourceHandler)val7).SetDynamicResource(VisualElement.BackgroundColorProperty, val18.Key);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val15).Children).Add((IView)(object)val7);
			((BindableObject)val9).SetValue(Grid.RowProperty, (object)1);
			((BindableObject)val9).SetValue(Grid.ColumnProperty, (object)0);
			val8.Key = "Scrim";
			XamlServiceProvider val19 = new XamlServiceProvider();
			val19.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(34, 22)));
			DynamicResource val20 = ((IMarkupExtension<DynamicResource>)(object)val8).ProvideValue((IServiceProvider)val19);
			((IDynamicResourceHandler)val9).SetDynamicResource(VisualElement.BackgroundColorProperty, val20.Key);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val15).Children).Add((IView)(object)val9);
			((BindableObject)val10).SetValue(Grid.RowProperty, (object)1);
			((BindableObject)val10).SetValue(Grid.ColumnProperty, (object)1);
			((BindableObject)val10).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Center);
			((BindableObject)val10).SetValue(VisualElement.HeightRequestProperty, (object)2.0);
			((BindableObject)val10).SetValue(VisualElement.BackgroundColorProperty, (object)Colors.Red);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val15).Children).Add((IView)(object)val10);
			((BindableObject)val12).SetValue(Grid.RowProperty, (object)1);
			((BindableObject)val12).SetValue(Grid.ColumnProperty, (object)2);
			val11.Key = "Scrim";
			XamlServiceProvider val21 = new XamlServiceProvider();
			val21.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(42, 22)));
			DynamicResource val22 = ((IMarkupExtension<DynamicResource>)(object)val11).ProvideValue((IServiceProvider)val21);
			((IDynamicResourceHandler)val12).SetDynamicResource(VisualElement.BackgroundColorProperty, val22.Key);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val15).Children).Add((IView)(object)val12);
			((BindableObject)val14).SetValue(Grid.RowProperty, (object)2);
			((BindableObject)val14).SetValue(Grid.ColumnProperty, (object)0);
			((BindableObject)val14).SetValue(Grid.ColumnSpanProperty, (object)3);
			val13.Key = "Scrim";
			XamlServiceProvider val23 = new XamlServiceProvider();
			val23.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(46, 22)));
			DynamicResource val24 = ((IMarkupExtension<DynamicResource>)(object)val13).ProvideValue((IServiceProvider)val23);
			((IDynamicResourceHandler)val14).SetDynamicResource(VisualElement.BackgroundColorProperty, val24.Key);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val15).Children).Add((IView)(object)val14);
			((BindableObject)scanWithCameraPage).SetValue(AppBarPage.ContentProperty, (object)val15);
		}
	}
	public abstract class ScanWithCameraViewModel : PageViewModel
	{
		[CompilerGenerated]
		private sealed class <>c__DisplayClass7_0
		{
			[StructLayout((LayoutKind)3)]
			private struct <<BarcodeDetected>b__0>d : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncTaskMethodBuilder <>t__builder;

				public <>c__DisplayClass7_0 <>4__this;

				private TaskAwaiter <>u__1;

				private void MoveNext()
				{
					//IL_0052: Unknown result type (might be due to invalid IL or missing references)
					//IL_0057: Unknown result type (might be due to invalid IL or missing references)
					//IL_005e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0022: Unknown result type (might be due to invalid IL or missing references)
					//IL_0027: Unknown result type (might be due to invalid IL or missing references)
					//IL_003b: Unknown result type (might be due to invalid IL or missing references)
					//IL_003c: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					<>c__DisplayClass7_0 <>c__DisplayClass7_ = <>4__this;
					try
					{
						TaskAwaiter val;
						if (num != 0)
						{
							val = <>c__DisplayClass7_.<>4__this.LabelScanned(<>c__DisplayClass7_.scanResultParser).GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <<BarcodeDetected>b__0>d>(ref val, ref this);
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

			public ScanWithCameraViewModel <>4__this;

			public ScanResultParser scanResultParser;

			[AsyncStateMachine(typeof(<<BarcodeDetected>b__0>d))]
			internal global::System.Threading.Tasks.Task <BarcodeDetected>b__0()
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				<<BarcodeDetected>b__0>d <<BarcodeDetected>b__0>d = default(<<BarcodeDetected>b__0>d);
				<<BarcodeDetected>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<BarcodeDetected>b__0>d.<>4__this = this;
				<<BarcodeDetected>b__0>d.<>1__state = -1;
				((AsyncTaskMethodBuilder)(ref <<BarcodeDetected>b__0>d.<>t__builder)).Start<<<BarcodeDetected>b__0>d>(ref <<BarcodeDetected>b__0>d);
				return ((AsyncTaskMethodBuilder)(ref <<BarcodeDetected>b__0>d.<>t__builder)).Task;
			}
		}

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <Back>d__5 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public ScanWithCameraViewModel <>4__this;

			private TaskAwaiter<INavigationResult> <>u__1;

			private void MoveNext()
			{
				//IL_004d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_0059: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				ScanWithCameraViewModel scanWithCameraViewModel = <>4__this;
				try
				{
					TaskAwaiter<INavigationResult> val;
					if (num != 0)
					{
						val = ((PageViewModel)scanWithCameraViewModel).NavigationService.GoBackAsync((INavigationParameters)null).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<INavigationResult>, <Back>d__5>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<INavigationResult>);
						num = (<>1__state = -1);
					}
					val.GetResult();
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
		private struct <OnResumeAsync>d__8 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public ScanWithCameraViewModel <>4__this;

			public ResumeReason reason;

			public INavigationParameters parameters;

			public CancellationToken cancellationToken;

			private TaskAwaiter <>u__1;

			private TaskAwaiter<PermissionStatus> <>u__2;

			private TaskAwaiter<INavigationResult> <>u__3;

			private void MoveNext()
			{
				//IL_006f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0074: Unknown result type (might be due to invalid IL or missing references)
				//IL_007b: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
				//IL_014d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0152: Unknown result type (might be due to invalid IL or missing references)
				//IL_015a: Unknown result type (might be due to invalid IL or missing references)
				//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
				//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
				//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_0096: Unknown result type (might be due to invalid IL or missing references)
				//IL_009b: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ef: Invalid comparison between Unknown and I4
				//IL_0055: Unknown result type (might be due to invalid IL or missing references)
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
				//IL_0185: Unknown result type (might be due to invalid IL or missing references)
				//IL_018a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0118: Unknown result type (might be due to invalid IL or missing references)
				//IL_011d: Unknown result type (might be due to invalid IL or missing references)
				//IL_019f: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
				//IL_0132: Unknown result type (might be due to invalid IL or missing references)
				//IL_0134: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				ScanWithCameraViewModel scanWithCameraViewModel = <>4__this;
				try
				{
					TaskAwaiter val3;
					TaskAwaiter<PermissionStatus> val2;
					TaskAwaiter<INavigationResult> val;
					bool flag;
					switch (num)
					{
					default:
						val3 = scanWithCameraViewModel.<>n__0(reason, parameters, cancellationToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val3)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val3;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnResumeAsync>d__8>(ref val3, ref this);
							return;
						}
						goto IL_008a;
					case 0:
						val3 = <>u__1;
						<>u__1 = default(TaskAwaiter);
						num = (<>1__state = -1);
						goto IL_008a;
					case 1:
						val2 = <>u__2;
						<>u__2 = default(TaskAwaiter<PermissionStatus>);
						num = (<>1__state = -1);
						goto IL_00e7;
					case 2:
						val = <>u__3;
						<>u__3 = default(TaskAwaiter<INavigationResult>);
						num = (<>1__state = -1);
						goto IL_0169;
					case 3:
						{
							val = <>u__3;
							<>u__3 = default(TaskAwaiter<INavigationResult>);
							num = (<>1__state = -1);
							goto IL_01d3;
						}
						IL_01d3:
						val.GetResult();
						goto end_IL_000e;
						IL_008a:
						((TaskAwaiter)(ref val3)).GetResult();
						val2 = Permissions.CheckStatusAsync<Camera>().GetAwaiter();
						if (!val2.IsCompleted)
						{
							num = (<>1__state = 1);
							<>u__2 = val2;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, <OnResumeAsync>d__8>(ref val2, ref this);
							return;
						}
						goto IL_00e7;
						IL_0169:
						val.GetResult();
						goto end_IL_000e;
						IL_00e7:
						flag = (int)val2.GetResult() == 3;
						if (!scanWithCameraViewModel._permissionRequested)
						{
							if (flag)
							{
								break;
							}
							scanWithCameraViewModel._permissionRequested = true;
							val = ((PageViewModel)scanWithCameraViewModel).NavigationService.NavigateAsync("CameraPermissionViewModel", (INavigationParameters)null).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 2);
								<>u__3 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<INavigationResult>, <OnResumeAsync>d__8>(ref val, ref this);
								return;
							}
							goto IL_0169;
						}
						if (flag)
						{
							break;
						}
						val = ((PageViewModel)scanWithCameraViewModel).NavigationService.GoBackAsync((INavigationParameters)null).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 3);
							<>u__3 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<INavigationResult>, <OnResumeAsync>d__8>(ref val, ref this);
							return;
						}
						goto IL_01d3;
					}
					scanWithCameraViewModel.IsScanning = true;
					end_IL_000e:;
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

		private bool _permissionRequested;

		[ObservableProperty]
		private string? _title;

		[ObservableProperty]
		private bool _isScanning;

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		private AsyncRelayCommand? backCommand;

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		private RelayCommand<BarcodeDetectionEventArgs>? barcodeDetectedCommand;

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string? Title
		{
			get
			{
				return _title;
			}
			set
			{
				if (!EqualityComparer<string>.Default.Equals(_title, value))
				{
					((ObservableObject)this).OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Title);
					_title = value;
					((ObservableObject)this).OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Title);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool IsScanning
		{
			get
			{
				return _isScanning;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(_isScanning, value))
				{
					((ObservableObject)this).OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsScanning);
					_isScanning = value;
					((ObservableObject)this).OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsScanning);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public IAsyncRelayCommand BackCommand
		{
			get
			{
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Expected O, but got Unknown
				//IL_0023: Expected O, but got Unknown
				AsyncRelayCommand obj = backCommand;
				if (obj == null)
				{
					AsyncRelayCommand val = new AsyncRelayCommand((Func<global::System.Threading.Tasks.Task>)Back);
					AsyncRelayCommand val2 = val;
					backCommand = val;
					obj = val2;
				}
				return (IAsyncRelayCommand)(object)obj;
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public IRelayCommand<BarcodeDetectionEventArgs> BarcodeDetectedCommand => (IRelayCommand<BarcodeDetectionEventArgs>)(object)(barcodeDetectedCommand ?? (barcodeDetectedCommand = new RelayCommand<BarcodeDetectionEventArgs>((Action<BarcodeDetectionEventArgs>)BarcodeDetected)));

		protected ScanWithCameraViewModel(IServiceProvider serviceProvider, IMainThreadService mainThreadService)
		{
			<mainThreadService>P = mainThreadService;
			_title = Strings.ScanWithCamera_Title;
			((PageViewModel)this)..ctor(serviceProvider);
		}

		[AsyncStateMachine(typeof(<Back>d__5))]
		[RelayCommand(AllowConcurrentExecutions = false)]
		private global::System.Threading.Tasks.Task Back()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			<Back>d__5 <Back>d__ = default(<Back>d__5);
			<Back>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<Back>d__.<>4__this = this;
			<Back>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <Back>d__.<>t__builder)).Start<<Back>d__5>(ref <Back>d__);
			return ((AsyncTaskMethodBuilder)(ref <Back>d__.<>t__builder)).Task;
		}

		protected abstract global::System.Threading.Tasks.Task LabelScanned(ScanResultParser scanResultParser);

		[RelayCommand]
		private void BarcodeDetected(BarcodeDetectionEventArgs arg)
		{
			<>c__DisplayClass7_0 CS$<>8__locals4 = new <>c__DisplayClass7_0();
			CS$<>8__locals4.<>4__this = this;
			BarcodeResult obj = Enumerable.FirstOrDefault<BarcodeResult>((global::System.Collections.Generic.IEnumerable<BarcodeResult>)arg.Results);
			CS$<>8__locals4.scanResultParser = ScanResultParser.TryParseQrCode(((obj != null) ? obj.Value.Trim() : null) ?? string.Empty);
			if (!(CS$<>8__locals4.scanResultParser is ErrorScanResultParser))
			{
				<mainThreadService>P.InvokeOnMainThreadAsync((Func<global::System.Threading.Tasks.Task>)([AsyncStateMachine(typeof(<>c__DisplayClass7_0.<<BarcodeDetected>b__0>d))] () =>
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					<>c__DisplayClass7_0.<<BarcodeDetected>b__0>d <<BarcodeDetected>b__0>d = default(<>c__DisplayClass7_0.<<BarcodeDetected>b__0>d);
					<<BarcodeDetected>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
					<<BarcodeDetected>b__0>d.<>4__this = CS$<>8__locals4;
					<<BarcodeDetected>b__0>d.<>1__state = -1;
					((AsyncTaskMethodBuilder)(ref <<BarcodeDetected>b__0>d.<>t__builder)).Start<<>c__DisplayClass7_0.<<BarcodeDetected>b__0>d>(ref <<BarcodeDetected>b__0>d);
					return ((AsyncTaskMethodBuilder)(ref <<BarcodeDetected>b__0>d.<>t__builder)).Task;
				}));
			}
		}

		[AsyncStateMachine(typeof(<OnResumeAsync>d__8))]
		public override global::System.Threading.Tasks.Task OnResumeAsync(ResumeReason reason, INavigationParameters parameters, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			<OnResumeAsync>d__8 <OnResumeAsync>d__ = default(<OnResumeAsync>d__8);
			<OnResumeAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<OnResumeAsync>d__.<>4__this = this;
			<OnResumeAsync>d__.reason = reason;
			<OnResumeAsync>d__.parameters = parameters;
			<OnResumeAsync>d__.cancellationToken = cancellationToken;
			<OnResumeAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <OnResumeAsync>d__.<>t__builder)).Start<<OnResumeAsync>d__8>(ref <OnResumeAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <OnResumeAsync>d__.<>t__builder)).Task;
		}

		public override void OnPause(PauseReason reason)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((PageViewModel)this).OnPause(reason);
			IsScanning = false;
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private global::System.Threading.Tasks.Task <>n__0(ResumeReason reason, INavigationParameters parameters, CancellationToken cancellationToken)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			return ((PageViewModel)this).OnResumeAsync(reason, parameters, cancellationToken);
		}
	}
}
namespace App.Common.Pages.Pairing.Resources
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	public class Strings
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		public static ResourceManager ResourceManager
		{
			get
			{
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Expected O, but got Unknown
				if (object.Equals((object)null, (object)resourceMan))
				{
					resourceMan = new ResourceManager("App.Common.Pages.Pairing.Resources.Strings", typeof(Strings).Assembly);
				}
				return resourceMan;
			}
		}

		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		public static CultureInfo Culture
		{
			get
			{
				return resourceCulture;
			}
			set
			{
				resourceCulture = value;
			}
		}

		public static string CollectWiFiInfo_Ssid_Placeholder => ResourceManager.GetString("CollectWiFiInfo_Ssid_Placeholder", resourceCulture);

		public static string CollectWiFiInfo_Password_Placeholder => ResourceManager.GetString("CollectWiFiInfo_Password_Placeholder", resourceCulture);

		public static string AddAndExplore_Action_IDontSeeMyDevice => ResourceManager.GetString("AddAndExplore_Action_IDontSeeMyDevice", resourceCulture);

		public static string AddAndExplore_Action_Continue => ResourceManager.GetString("AddAndExplore_Action_Continue", resourceCulture);

		public static string AddAndExplore_Action_Cancel => ResourceManager.GetString("AddAndExplore_Action_Cancel", resourceCulture);

		public static string AddAndExplore_Action_TryAgain => ResourceManager.GetString("AddAndExplore_Action_TryAgain", resourceCulture);

		public static string AddAndExplore_Action_ImReady => ResourceManager.GetString("AddAndExplore_Action_ImReady", resourceCulture);

		public static string AddAndExplore_BeforeYouBegin_Title => ResourceManager.GetString("AddAndExplore_BeforeYouBegin_Title", resourceCulture);

		public static string ConnectToBluetoothGateway_LegacyPinPrompt_Title => ResourceManager.GetString("ConnectToBluetoothGateway_LegacyPinPrompt_Title", resourceCulture);

		public static string ConnectToBluetoothGateway_LegacyPinPrompt_Message => ResourceManager.GetString("ConnectToBluetoothGateway_LegacyPinPrompt_Message", resourceCulture);

		public static string ConnectToBluetoothGateway_LegacyPinPrompt_Placeholder => ResourceManager.GetString("ConnectToBluetoothGateway_LegacyPinPrompt_Placeholder", resourceCulture);

		public static string ConnectToBluetoothGateway_LegacyPinPrompt_Action_Confirm => ResourceManager.GetString("ConnectToBluetoothGateway_LegacyPinPrompt_Action_Confirm", resourceCulture);

		public static string ConnectToBluetoothGateway_LegacyPinPrompt_Action_Cancel => ResourceManager.GetString("ConnectToBluetoothGateway_LegacyPinPrompt_Action_Cancel", resourceCulture);

		public static string ConnectToGateway_Message_Connecting => ResourceManager.GetString("ConnectToGateway_Message_Connecting", resourceCulture);

		public static string ConnectToGateway_Message_Connected => ResourceManager.GetString("ConnectToGateway_Message_Connected", resourceCulture);

		public static string ConnectToGateway_Message_Finishing => ResourceManager.GetString("ConnectToGateway_Message_Finishing", resourceCulture);

		public static string ConnectToGateway_Message_Finished => ResourceManager.GetString("ConnectToGateway_Message_Finished", resourceCulture);

		public static string ConnectToWiFiGateway_Message => ResourceManager.GetString("ConnectToWiFiGateway_Message", resourceCulture);

		public static string Error_SomethingWentWrong => ResourceManager.GetString("Error_SomethingWentWrong", resourceCulture);

		public static string Error_StillAProblem => ResourceManager.GetString("Error_StillAProblem", resourceCulture);

		public static string ScanWithCamera_Title => ResourceManager.GetString("ScanWithCamera_Title", resourceCulture);

		public static string SearchForDevices_Searching => ResourceManager.GetString("SearchForDevices_Searching", resourceCulture);

		public static string SearchForDevices_Pin => ResourceManager.GetString("SearchForDevices_Pin", resourceCulture);

		public static string SearchForDevices_PushToPair => ResourceManager.GetString("SearchForDevices_PushToPair", resourceCulture);

		public static string SearchForDevices_ReadyToPair => ResourceManager.GetString("SearchForDevices_ReadyToPair", resourceCulture);

		public static string Warning_Title => ResourceManager.GetString("Warning_Title", resourceCulture);

		public static string Error_TryAgain => ResourceManager.GetString("Error_TryAgain", resourceCulture);

		public static string AddAndExplore_Success_Message => ResourceManager.GetString("AddAndExplore_Success_Message", resourceCulture);

		public static string AddAndExplore_Action_Back => ResourceManager.GetString("AddAndExplore_Action_Back", resourceCulture);

		public static string Troubleshooting_Title => ResourceManager.GetString("Troubleshooting_Title", resourceCulture);

		public static string demo => ResourceManager.GetString("demo", resourceCulture);

		public static string no_connection => ResourceManager.GetString("no_connection", resourceCulture);

		internal Strings()
		{
		}
	}
}
namespace App.Common.Pages.Pairing.Resources.Images
{
	public static class Images
	{
		private static readonly string s_baseIconResource = "resource://" + typeof(Images).Namespace + ".{0}.svg?assembly=" + typeof(Images).Assembly.GetName().Name;

		[field: CompilerGenerated]
		public static string Lippert_QR_Label
		{
			[CompilerGenerated]
			get;
		} = string.Format(s_baseIconResource, (object)"Lippert_QR_Label");

		[field: CompilerGenerated]
		public static string Connect_Ble_Button
		{
			[CompilerGenerated]
			get;
		} = string.Format(s_baseIconResource, (object)"Connect_Ble_Button");

		[field: CompilerGenerated]
		public static string App_Pairing_Ble_Button
		{
			[CompilerGenerated]
			get;
		} = string.Format(s_baseIconResource, (object)"App_Pairing_Ble_Button");

		[field: CompilerGenerated]
		public static string Lippert_QR_Label_WiFi
		{
			[CompilerGenerated]
			get;
		} = string.Format(s_baseIconResource, (object)"Lippert_QR_Label_WiFi");

		[field: CompilerGenerated]
		public static string SPMP
		{
			[CompilerGenerated]
			get;
		} = string.Format(s_baseIconResource, (object)"SPMP");

		[field: CompilerGenerated]
		public static string MP
		{
			[CompilerGenerated]
			get;
		} = string.Format(s_baseIconResource, (object)"MP");

		[field: CompilerGenerated]
		public static string OCTP
		{
			[CompilerGenerated]
			get;
		} = string.Format(s_baseIconResource, (object)"OCTP");
	}
}
namespace App.Common.Pages.Pairing.Interfaces
{
	public interface IAppSettingsConnection
	{
		IRvGatewayConnection SelectedRvGatewayConnection { get; }

		bool SelectedRvHideDevicesFromGatewayCan { get; }

		bool SelectedRvHideDevicesFromRemote { get; }

		byte[]? MacAddress { get; set; }

		void SetSelectedRvGatewayConnection(IRvGatewayConnection? selectedRv, bool saveSelectedRv);

		void SetSelectedRvHideDevicesFromGatewayCan(bool hideDevices, bool autoSave = true);

		void SelectedRvSetHideDevicesFromRemote(bool hideDevices, bool autoSave = true);
	}
	public interface IAppPairingSettings : IAppSettingsConnection, IAppSettingsConnectionAbs, IAppSettingsYearMakeModel, IAppSettingsBase
	{
	}
	public interface IAppSettingsBase
	{
		void Save();
	}
	public interface IAppSettingsConnectionAbs
	{
		IRvGatewayConnection SelectedBrakingSystemGatewayConnection { get; }

		global::System.Threading.Tasks.Task SetSelectedBrakingSystemGatewayConnection(IRvGatewayConnection selectedAbs, bool saveSelectedAbs);
	}
	public interface IAppSettingsYearMakeModel
	{
		IRvKind RvKind { get; }

		bool IsRvKindConfirmed { get; }

		global::System.DateTime EnterRvDetailsSkippedDateTime { get; }

		bool? VehicleHasHardware { get; }

		void SetRvKind(IRvKind rvKind, bool autoSave = true);

		void SetRvKindConfirmed(bool isRvKindConfirmed, bool autosave = true);

		void SetEnterRvDetailsSkippedDateTime(global::System.DateTime enterRvDetailSkippedDateTime, bool autoSave = true);

		void SetVehicleHasHardware(bool vehicleHasHardware, bool autoSave = true);

		void ClearYmmfSettings(bool autoSave = true);
	}
	public interface IDisplaySettingViewModel
	{
		global::System.Collections.Generic.IReadOnlyList<ICellViewModel>? Children { get; }
	}
	public interface IRvGatewayConnection : IComparable, IJsonSerializable, IEndPointConnectionWithSerialization, IEndPointConnection, IJsonSerializerClass, IDirectConnection
	{
		ILogicalDeviceTag? LogicalDeviceTagConnection { get; }

		string DeviceSourceToken { get; }
	}
}
namespace App.Common.Pages.Pairing.Error
{
	[XamlFilePath("Error/ErrorPage.xaml")]
	public class ErrorPage : DualActionPage
	{
		public ErrorPage()
		{
			InitializeComponent();
		}

		[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
		private void InitializeComponent()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Expected O, but got Unknown
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Expected O, but got Unknown
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Expected O, but got Unknown
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected O, but got Unknown
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Expected O, but got Unknown
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Expected O, but got Unknown
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Expected O, but got Unknown
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Expected O, but got Unknown
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Expected O, but got Unknown
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Expected O, but got Unknown
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Expected O, but got Unknown
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Expected O, but got Unknown
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Expected O, but got Unknown
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Expected O, but got Unknown
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Expected O, but got Unknown
			//IL_013d: Expected O, but got Unknown
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Expected O, but got Unknown
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Expected O, but got Unknown
			//IL_020d: Expected O, but got Unknown
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ca: Expected O, but got Unknown
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cf: Expected O, but got Unknown
			//IL_02d4: Expected O, but got Unknown
			//IL_0321: Unknown result type (might be due to invalid IL or missing references)
			//IL_0326: Unknown result type (might be due to invalid IL or missing references)
			//IL_0335: Unknown result type (might be due to invalid IL or missing references)
			//IL_033f: Expected O, but got Unknown
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0344: Expected O, but got Unknown
			//IL_0349: Expected O, but got Unknown
			//IL_037e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0383: Unknown result type (might be due to invalid IL or missing references)
			//IL_0392: Unknown result type (might be due to invalid IL or missing references)
			//IL_039c: Expected O, but got Unknown
			//IL_0397: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a1: Expected O, but got Unknown
			//IL_03a6: Expected O, but got Unknown
			//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0403: Expected O, but got Unknown
			//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0408: Expected O, but got Unknown
			//IL_040d: Expected O, but got Unknown
			//IL_0442: Unknown result type (might be due to invalid IL or missing references)
			//IL_0447: Unknown result type (might be due to invalid IL or missing references)
			//IL_0456: Unknown result type (might be due to invalid IL or missing references)
			//IL_0460: Expected O, but got Unknown
			//IL_045b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0465: Expected O, but got Unknown
			//IL_046a: Expected O, but got Unknown
			//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_06aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_06af: Unknown result type (might be due to invalid IL or missing references)
			//IL_06be: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c8: Expected O, but got Unknown
			//IL_06c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06cd: Expected O, but got Unknown
			//IL_06d2: Expected O, but got Unknown
			//IL_0707: Unknown result type (might be due to invalid IL or missing references)
			//IL_070c: Unknown result type (might be due to invalid IL or missing references)
			//IL_071b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0725: Expected O, but got Unknown
			//IL_0720: Unknown result type (might be due to invalid IL or missing references)
			//IL_072a: Expected O, but got Unknown
			//IL_072f: Expected O, but got Unknown
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			DynamicResourceExtension val = new DynamicResourceExtension();
			DynamicResourceExtension val2 = new DynamicResourceExtension();
			RoundRectangle val3 = new RoundRectangle();
			string proSolid = FontAwesomeFontFamily.ProSolid900;
			string circleExclamation = FontAwesomeGlyph.CircleExclamation;
			DynamicResourceExtension val4 = new DynamicResourceExtension();
			FontImageSource val5 = new FontImageSource();
			SKImageView val6 = new SKImageView();
			Border val7 = new Border();
			DynamicResourceExtension val8 = new DynamicResourceExtension();
			DynamicResourceExtension val9 = new DynamicResourceExtension();
			string error_SomethingWentWrong = Strings.Error_SomethingWentWrong;
			Label val10 = new Label();
			DynamicResourceExtension val11 = new DynamicResourceExtension();
			DynamicResourceExtension val12 = new DynamicResourceExtension();
			BindingExtension val13 = new BindingExtension();
			Label val14 = new Label();
			InvertedBoolConverter val15 = new InvertedBoolConverter();
			BindingExtension val16 = new BindingExtension();
			DynamicResourceExtension val17 = new DynamicResourceExtension();
			DynamicResourceExtension val18 = new DynamicResourceExtension();
			string error_TryAgain = Strings.Error_TryAgain;
			Label val19 = new Label();
			VerticalStackLayout val20 = new VerticalStackLayout();
			ErrorPage errorPage;
			NameScope val21 = (NameScope)(((object)NameScope.GetNameScope((BindableObject)(object)(errorPage = this))) ?? ((object)new NameScope()));
			NameScope.SetNameScope((BindableObject)(object)errorPage, (INameScope)(object)val21);
			((Element)val20).transientNamescope = (INameScope)(object)val21;
			((Element)val7).transientNamescope = (INameScope)(object)val21;
			((Element)val3).transientNamescope = (INameScope)(object)val21;
			((Element)val6).transientNamescope = (INameScope)(object)val21;
			((Element)val5).transientNamescope = (INameScope)(object)val21;
			((Element)val10).transientNamescope = (INameScope)(object)val21;
			((Element)val14).transientNamescope = (INameScope)(object)val21;
			((Element)val19).transientNamescope = (INameScope)(object)val21;
			val.Key = "DualActionPageErrorStyle";
			XamlServiceProvider val22 = new XamlServiceProvider();
			val22.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(12, 43)));
			DynamicResource val23 = ((IMarkupExtension<DynamicResource>)(object)val).ProvideValue((IServiceProvider)val22);
			((IDynamicResourceHandler)errorPage).SetDynamicResource(VisualElement.StyleProperty, val23.Key);
			((BindableObject)val20).SetValue(StackBase.SpacingProperty, (object)16.0);
			((BindableObject)val7).SetValue(VisualElement.WidthRequestProperty, (object)248.0);
			((BindableObject)val7).SetValue(VisualElement.HeightRequestProperty, (object)248.0);
			((BindableObject)val7).SetValue(Border.PaddingProperty, (object)new Thickness(4.0));
			((BindableObject)val7).SetValue(Border.StrokeThicknessProperty, (object)4.0);
			val2.Key = "OnErrorContainer";
			XamlServiceProvider val24 = new XamlServiceProvider();
			val24.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(19, 21)));
			DynamicResource val25 = ((IMarkupExtension<DynamicResource>)(object)val2).ProvideValue((IServiceProvider)val24);
			((IDynamicResourceHandler)val7).SetDynamicResource(Border.StrokeProperty, val25.Key);
			((BindableObject)val3).SetValue(RoundRectangle.CornerRadiusProperty, (object)new CornerRadius(124.0));
			((BindableObject)val7).SetValue(Border.StrokeShapeProperty, (object)val3);
			((BindableObject)val6).SetValue(SKImageView.AspectProperty, (object)(Aspect)0);
			((BindableObject)val6).SetValue(SKImageView.HorizontalImageAlignmentProperty, (object)(ImageAlignment)1);
			((BindableObject)val6).SetValue(SKImageView.VerticalImageAlignmentProperty, (object)(ImageAlignment)1);
			((BindableObject)val5).SetValue(FontImageSource.FontFamilyProperty, (object)proSolid);
			((BindableObject)val5).SetValue(FontImageSource.GlyphProperty, (object)circleExclamation);
			val4.Key = "Error";
			XamlServiceProvider val26 = new XamlServiceProvider();
			val26.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(29, 42)));
			DynamicResource val27 = ((IMarkupExtension<DynamicResource>)(object)val4).ProvideValue((IServiceProvider)val26);
			((IDynamicResourceHandler)val5).SetDynamicResource(FontImageSource.ColorProperty, val27.Key);
			((BindableObject)val6).SetValue(SKImageView.ImageSourceProperty, (object)val5);
			((BindableObject)val7).SetValue(Border.ContentProperty, (object)val6);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val20).Children).Add((IView)(object)val7);
			val8.Key = "Body4_Label_Black";
			XamlServiceProvider val28 = new XamlServiceProvider();
			val28.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(33, 20)));
			DynamicResource val29 = ((IMarkupExtension<DynamicResource>)(object)val8).ProvideValue((IServiceProvider)val28);
			((IDynamicResourceHandler)val10).SetDynamicResource(VisualElement.StyleProperty, val29.Key);
			((BindableObject)val10).SetValue(Label.HorizontalTextAlignmentProperty, (object)(TextAlignment)1);
			val9.Key = "OnErrorContainer";
			XamlServiceProvider val30 = new XamlServiceProvider();
			val30.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(35, 20)));
			DynamicResource val31 = ((IMarkupExtension<DynamicResource>)(object)val9).ProvideValue((IServiceProvider)val30);
			((IDynamicResourceHandler)val10).SetDynamicResource(Label.TextColorProperty, val31.Key);
			((BindableObject)val10).SetValue(Label.TextProperty, (object)error_SomethingWentWrong);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val20).Children).Add((IView)(object)val10);
			val11.Key = "Body4_Label";
			XamlServiceProvider val32 = new XamlServiceProvider();
			val32.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(38, 20)));
			DynamicResource val33 = ((IMarkupExtension<DynamicResource>)(object)val11).ProvideValue((IServiceProvider)val32);
			((IDynamicResourceHandler)val14).SetDynamicResource(VisualElement.StyleProperty, val33.Key);
			((BindableObject)val14).SetValue(Label.HorizontalTextAlignmentProperty, (object)(TextAlignment)1);
			val12.Key = "OnErrorContainer";
			XamlServiceProvider val34 = new XamlServiceProvider();
			val34.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(40, 20)));
			DynamicResource val35 = ((IMarkupExtension<DynamicResource>)(object)val12).ProvideValue((IServiceProvider)val34);
			((IDynamicResourceHandler)val14).SetDynamicResource(Label.TextColorProperty, val35.Key);
			val13.Path = "Message";
			val13.TypedBinding = (TypedBindingBase)(object)new TypedBinding<ErrorViewModel, string>((Func<ErrorViewModel, ValueTuple<string, bool>>)([CompilerGenerated] (ErrorViewModel P_0) => (P_0 != null) ? new ValueTuple<string, bool>(P_0.Message, true) : default(ValueTuple<string, bool>)), (Action<ErrorViewModel, string>)([CompilerGenerated] (ErrorViewModel P_0, string P_1) =>
			{
				if (P_0 != null)
				{
					P_0.Message = P_1;
				}
			}), new Tuple<Func<ErrorViewModel, object>, string>[1]
			{
				new Tuple<Func<ErrorViewModel, object>, string>((Func<ErrorViewModel, object>)([CompilerGenerated] (ErrorViewModel P_0) => P_0), "Message")
			});
			((BindingBase)val13.TypedBinding).Mode = val13.Mode;
			val13.TypedBinding.Converter = val13.Converter;
			val13.TypedBinding.ConverterParameter = val13.ConverterParameter;
			((BindingBase)val13.TypedBinding).StringFormat = val13.StringFormat;
			val13.TypedBinding.Source = val13.Source;
			val13.TypedBinding.UpdateSourceEventName = val13.UpdateSourceEventName;
			((BindingBase)val13.TypedBinding).FallbackValue = val13.FallbackValue;
			((BindingBase)val13.TypedBinding).TargetNullValue = val13.TargetNullValue;
			BindingBase typedBinding = (BindingBase)(object)val13.TypedBinding;
			((BindableObject)val14).SetBinding(Label.TextProperty, typedBinding);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val20).Children).Add((IView)(object)val14);
			ICommunityToolkitValueConverter converter = ((IMarkupExtension<ICommunityToolkitValueConverter>)(object)val15).ProvideValue((IServiceProvider)null);
			val16.Converter = (IValueConverter)(object)converter;
			val16.Path = "HasCustomRetryText";
			val16.TypedBinding = (TypedBindingBase)(object)new TypedBinding<ErrorViewModel, bool>((Func<ErrorViewModel, ValueTuple<bool, bool>>)([CompilerGenerated] (ErrorViewModel P_0) => (P_0 != null) ? new ValueTuple<bool, bool>(P_0.HasCustomRetryText, true) : default(ValueTuple<bool, bool>)), (Action<ErrorViewModel, bool>)([CompilerGenerated] (ErrorViewModel P_0, bool P_1) =>
			{
				if (P_0 != null)
				{
					P_0.HasCustomRetryText = P_1;
				}
			}), new Tuple<Func<ErrorViewModel, object>, string>[1]
			{
				new Tuple<Func<ErrorViewModel, object>, string>((Func<ErrorViewModel, object>)([CompilerGenerated] (ErrorViewModel P_0) => P_0), "HasCustomRetryText")
			});
			((BindingBase)val16.TypedBinding).Mode = val16.Mode;
			val16.TypedBinding.Converter = val16.Converter;
			val16.TypedBinding.ConverterParameter = val16.ConverterParameter;
			((BindingBase)val16.TypedBinding).StringFormat = val16.StringFormat;
			val16.TypedBinding.Source = val16.Source;
			val16.TypedBinding.UpdateSourceEventName = val16.UpdateSourceEventName;
			((BindingBase)val16.TypedBinding).FallbackValue = val16.FallbackValue;
			((BindingBase)val16.TypedBinding).TargetNullValue = val16.TargetNullValue;
			BindingBase typedBinding2 = (BindingBase)(object)val16.TypedBinding;
			((BindableObject)val19).SetBinding(VisualElement.IsVisibleProperty, typedBinding2);
			val17.Key = "Body4_Label";
			XamlServiceProvider val36 = new XamlServiceProvider();
			val36.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(44, 20)));
			DynamicResource val37 = ((IMarkupExtension<DynamicResource>)(object)val17).ProvideValue((IServiceProvider)val36);
			((IDynamicResourceHandler)val19).SetDynamicResource(VisualElement.StyleProperty, val37.Key);
			((BindableObject)val19).SetValue(Label.HorizontalTextAlignmentProperty, (object)(TextAlignment)1);
			val18.Key = "OnErrorContainer";
			XamlServiceProvider val38 = new XamlServiceProvider();
			val38.Add(typeof(IXmlLineInfoProvider), (object)new XmlLineInfoProvider((IXmlLineInfo)new XmlLineInfo(46, 20)));
			DynamicResource val39 = ((IMarkupExtension<DynamicResource>)(object)val18).ProvideValue((IServiceProvider)val38);
			((IDynamicResourceHandler)val19).SetDynamicResource(Label.TextColorProperty, val39.Key);
			((BindableObject)val19).SetValue(Label.TextProperty, (object)error_TryAgain);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val20).Children).Add((IView)(object)val19);
			((BindableObject)errorPage).SetValue(DualActionPage.ContentProperty, (object)val20);
		}
	}
	public abstract class ErrorViewModel : ConnectViewModel
	{
		public enum ErrorNavigationReason
		{
			Cancel,
			Retry
		}

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <OnResumeAsync>d__8 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public ErrorViewModel <>4__this;

			public ResumeReason reason;

			public INavigationParameters parameters;

			public CancellationToken cancellationToken;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0059: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0065: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				ErrorViewModel errorViewModel = <>4__this;
				try
				{
					TaskAwaiter val;
					if (num != 0)
					{
						val = ((ConnectViewModel)errorViewModel).OnResumeAsync(reason, parameters, cancellationToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnResumeAsync>d__8>(ref val, ref this);
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
					errorViewModel.StillCannotConnect = errorViewModel.ErrorCount > 1;
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
		private struct <OnStartAsync>d__7 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public ErrorViewModel <>4__this;

			public INavigationParameters parameters;

			public CancellationToken cancellationToken;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_005f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				ErrorViewModel errorViewModel = <>4__this;
				try
				{
					TaskAwaiter val;
					if (num != 0)
					{
						val = errorViewModel.<>n__0(parameters, cancellationToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnStartAsync>d__7>(ref val, ref this);
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
					errorViewModel.PassedInParameters = parameters;
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

		public static string ErrorNavigationReasonKey = "ErrorNavigationReasonKey";

		public INavigationParameters PassedInParameters;

		[ObservableProperty]
		private bool _stillCannotConnect;

		[ObservableProperty]
		private string _message = string.Empty;

		[ObservableProperty]
		private bool _hasCustomRetryText;

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool StillCannotConnect
		{
			get
			{
				return _stillCannotConnect;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(_stillCannotConnect, value))
				{
					((ObservableObject)this).OnPropertyChanging(__KnownINotifyPropertyChangingArgs.StillCannotConnect);
					_stillCannotConnect = value;
					((ObservableObject)this).OnPropertyChanged(__KnownINotifyPropertyChangedArgs.StillCannotConnect);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string Message
		{
			get
			{
				return _message;
			}
			[MemberNotNull("_message")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(_message, value))
				{
					((ObservableObject)this).OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Message);
					_message = value;
					((ObservableObject)this).OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Message);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool HasCustomRetryText
		{
			get
			{
				return _hasCustomRetryText;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(_hasCustomRetryText, value))
				{
					((ObservableObject)this).OnPropertyChanging(__KnownINotifyPropertyChangingArgs.HasCustomRetryText);
					_hasCustomRetryText = value;
					((ObservableObject)this).OnPropertyChanged(__KnownINotifyPropertyChangedArgs.HasCustomRetryText);
				}
			}
		}

		protected ErrorViewModel(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			((DualActionViewModel)this).BackAction = [CompilerGenerated] () => ((DualActionViewModel)this).PrimaryAction?.Invoke() ?? global::System.Threading.Tasks.Task.CompletedTask;
			((DualActionViewModel)this).PrimaryAction = [CompilerGenerated] () => ((PageViewModel)this).NavigationService.GoBackAsync((INavigationParameters)null);
			((DualActionViewModel)this).PrimaryActionStyle = (ActionStyle)0;
			((DualActionViewModel)this).PrimaryActionText = Strings.AddAndExplore_Action_TryAgain;
			((DualActionViewModel)this).SecondaryAction = [CompilerGenerated] () => ((PageViewModel)this).NavigationService.GoBackToRootAsync((INavigationParameters)null);
			((DualActionViewModel)this).SecondaryActionStyle = (ActionStyle)3;
			((DualActionViewModel)this).SecondaryActionText = Strings.AddAndExplore_Action_Cancel;
		}

		[AsyncStateMachine(typeof(<OnStartAsync>d__7))]
		public override global::System.Threading.Tasks.Task OnStartAsync(INavigationParameters? parameters, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			<OnStartAsync>d__7 <OnStartAsync>d__ = default(<OnStartAsync>d__7);
			<OnStartAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<OnStartAsync>d__.<>4__this = this;
			<OnStartAsync>d__.parameters = parameters;
			<OnStartAsync>d__.cancellationToken = cancellationToken;
			<OnStartAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <OnStartAsync>d__.<>t__builder)).Start<<OnStartAsync>d__7>(ref <OnStartAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <OnStartAsync>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(<OnResumeAsync>d__8))]
		public override global::System.Threading.Tasks.Task OnResumeAsync(ResumeReason reason, INavigationParameters parameters, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			<OnResumeAsync>d__8 <OnResumeAsync>d__ = default(<OnResumeAsync>d__8);
			<OnResumeAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<OnResumeAsync>d__.<>4__this = this;
			<OnResumeAsync>d__.reason = reason;
			<OnResumeAsync>d__.parameters = parameters;
			<OnResumeAsync>d__.cancellationToken = cancellationToken;
			<OnResumeAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <OnResumeAsync>d__.<>t__builder)).Start<<OnResumeAsync>d__8>(ref <OnResumeAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <OnResumeAsync>d__.<>t__builder)).Task;
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private global::System.Threading.Tasks.Task <>n__0(INavigationParameters? parameters, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			return ((PageViewModel)this).OnStartAsync(parameters, cancellationToken);
		}
	}
}
namespace App.Common.Pages.Pairing.DualActionPage
{
	public abstract class DualActionViewModel<TDeviceModel, TLogicalDevice> : DualActionViewModel where TDeviceModel : class, IDeviceModel<TLogicalDevice> where TLogicalDevice : class, ILogicalDevice
	{
		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <OnResumeAsync>d__14 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public DualActionViewModel<TDeviceModel, TLogicalDevice> <>4__this;

			public ResumeReason reason;

			public INavigationParameters parameters;

			public CancellationToken cancellationToken;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_005c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0061: Unknown result type (might be due to invalid IL or missing references)
				//IL_0068: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c0: Expected O, but got Unknown
				//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c5: Expected O, but got Unknown
				//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fa: Expected O, but got Unknown
				//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ff: Expected O, but got Unknown
				int num = <>1__state;
				DualActionViewModel<TDeviceModel, TLogicalDevice> dualActionViewModel = <>4__this;
				try
				{
					TaskAwaiter val;
					if (num != 0)
					{
						val = dualActionViewModel.<>n__1(reason, parameters, cancellationToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnResumeAsync>d__14>(ref val, ref this);
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
					if (dualActionViewModel.DeviceModel != null && dualActionViewModel.Device != null)
					{
						dualActionViewModel._deviceModelPropertyChangedProxySource = new NotifyPropertyChangedProxySource((INotifyPropertyChanged)(object)dualActionViewModel.DeviceModel, new ProxyOnPropertyChanged(((ObservableObject)dualActionViewModel).OnPropertyChanged), (INotifyPropertyChanged)(object)dualActionViewModel, "DeviceModelPageViewModelNotifyPropertyChangedProxySource");
						DisposableMixins.DisposeWith<NotifyPropertyChangedProxySource>(dualActionViewModel._deviceModelPropertyChangedProxySource, ViewModelExtensions.PausedDisposable((IViewModel)(object)dualActionViewModel));
						dualActionViewModel._devicePropertyChangedProxySource = new NotifyPropertyChangedProxySource((INotifyPropertyChanged)(object)dualActionViewModel.Device, new ProxyOnPropertyChanged(((ObservableObject)dualActionViewModel).OnPropertyChanged), (INotifyPropertyChanged)(object)dualActionViewModel, "DevicePageViewModelNotifyPropertyChangedProxySource");
						DisposableMixins.DisposeWith<NotifyPropertyChangedProxySource>(dualActionViewModel._devicePropertyChangedProxySource, ViewModelExtensions.PausedDisposable((IViewModel)(object)dualActionViewModel));
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
		private struct <OnStartAsync>d__13 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public DualActionViewModel<TDeviceModel, TLogicalDevice> <>4__this;

			public INavigationParameters parameters;

			public CancellationToken cancellationToken;

			private TDeviceModel <deviceModel>5__2;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				//IL_005b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0062: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0102: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				DualActionViewModel<TDeviceModel, TLogicalDevice> dualActionViewModel = <>4__this;
				try
				{
					TaskAwaiter val;
					if (num != 0)
					{
						val = dualActionViewModel.<>n__0(parameters, cancellationToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnStartAsync>d__13>(ref val, ref this);
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
					INavigationParameters obj = parameters;
					bool? flag = ((obj != null) ? new bool?(INavigationParametersExtensions.TryGetDeviceModel<TDeviceModel>(obj, ref <deviceModel>5__2, (TDeviceModel)null)) : ((bool?)null));
					if (!flag.HasValue || flag != true || <deviceModel>5__2 == null)
					{
						throw new ArgumentException($"Parameter of type {typeof(TDeviceModel)} expected!");
					}
					dualActionViewModel.DeviceModel = <deviceModel>5__2;
					dualActionViewModel.Device = ((IDeviceModel<?>)(object)<deviceModel>5__2).Device;
					((ViewModel)dualActionViewModel).RaiseAllPropertiesChanged((ThreadInvokeOption)1);
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<deviceModel>5__2 = null;
					((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
					return;
				}
				<>1__state = -2;
				<deviceModel>5__2 = null;
				((AsyncTaskMethodBuilder)(ref <>t__builder)).SetResult();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				((AsyncTaskMethodBuilder)(ref <>t__builder)).SetStateMachine(stateMachine);
			}
		}

		protected const string DeviceModelPropertyChangedProxySourceName = "DeviceModelPageViewModelNotifyPropertyChangedProxySource";

		protected const string DevicePropertyChangedProxySourceName = "DevicePageViewModelNotifyPropertyChangedProxySource";

		private NotifyPropertyChangedProxySource? _deviceModelPropertyChangedProxySource;

		private NotifyPropertyChangedProxySource? _devicePropertyChangedProxySource;

		public TDeviceModel? DeviceModel
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public TLogicalDevice? Device
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		protected DualActionViewModel(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		[AsyncStateMachine(typeof(DualActionViewModel<, >.<OnStartAsync>d__13))]
		public override global::System.Threading.Tasks.Task OnStartAsync(INavigationParameters? parameters, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			<OnStartAsync>d__13 <OnStartAsync>d__ = default(<OnStartAsync>d__13);
			<OnStartAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<OnStartAsync>d__.<>4__this = this;
			<OnStartAsync>d__.parameters = parameters;
			<OnStartAsync>d__.cancellationToken = cancellationToken;
			<OnStartAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <OnStartAsync>d__.<>t__builder)).Start<<OnStartAsync>d__13>(ref <OnStartAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <OnStartAsync>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(DualActionViewModel<, >.<OnResumeAsync>d__14))]
		public override global::System.Threading.Tasks.Task OnResumeAsync(ResumeReason reason, INavigationParameters parameters, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			<OnResumeAsync>d__14 <OnResumeAsync>d__ = default(<OnResumeAsync>d__14);
			<OnResumeAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<OnResumeAsync>d__.<>4__this = this;
			<OnResumeAsync>d__.reason = reason;
			<OnResumeAsync>d__.parameters = parameters;
			<OnResumeAsync>d__.cancellationToken = cancellationToken;
			<OnResumeAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <OnResumeAsync>d__.<>t__builder)).Start<<OnResumeAsync>d__14>(ref <OnResumeAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <OnResumeAsync>d__.<>t__builder)).Task;
		}
	}
	public abstract class DualActionViewModel<TLogicalDevice> : DualActionViewModel where TLogicalDevice : class, ILogicalDevice
	{
		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <OnResumeAsync>d__10 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public DualActionViewModel<TLogicalDevice> <>4__this;

			public ResumeReason reason;

			public INavigationParameters parameters;

			public CancellationToken cancellationToken;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_005c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0061: Unknown result type (might be due to invalid IL or missing references)
				//IL_0068: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b0: Expected O, but got Unknown
				//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b5: Expected O, but got Unknown
				int num = <>1__state;
				DualActionViewModel<TLogicalDevice> dualActionViewModel = <>4__this;
				try
				{
					TaskAwaiter val;
					if (num != 0)
					{
						val = dualActionViewModel.<>n__1(reason, parameters, cancellationToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnResumeAsync>d__10>(ref val, ref this);
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
					if (dualActionViewModel.Device != null)
					{
						dualActionViewModel._devicePropertyChangedProxySource = new NotifyPropertyChangedProxySource((INotifyPropertyChanged)(object)dualActionViewModel.Device, new ProxyOnPropertyChanged(((ObservableObject)dualActionViewModel).OnPropertyChanged), (INotifyPropertyChanged)(object)dualActionViewModel, "ViewModelBaseDevice_PropertyChangedProxySourceName");
						DisposableMixins.DisposeWith<NotifyPropertyChangedProxySource>(dualActionViewModel._devicePropertyChangedProxySource, ViewModelExtensions.PausedDisposable((IViewModel)(object)dualActionViewModel));
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
		private struct <OnStartAsync>d__9 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public DualActionViewModel<TLogicalDevice> <>4__this;

			public INavigationParameters parameters;

			public CancellationToken cancellationToken;

			private TLogicalDevice <device>5__2;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				//IL_005b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0062: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0102: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				DualActionViewModel<TLogicalDevice> dualActionViewModel = <>4__this;
				try
				{
					TaskAwaiter val;
					if (num != 0)
					{
						val = dualActionViewModel.<>n__0(parameters, cancellationToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnStartAsync>d__9>(ref val, ref this);
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
					INavigationParameters obj = parameters;
					bool? flag = ((obj != null) ? new bool?(INavigationParametersExtensions.TryGetDevice<TLogicalDevice>(obj, ref <device>5__2, (TLogicalDevice)null)) : ((bool?)null));
					if (!flag.HasValue || flag != true || <device>5__2 == null)
					{
						throw new ArgumentException($"Parameter of type {typeof(TLogicalDevice)} expected!");
					}
					dualActionViewModel.Device = <device>5__2;
					((ViewModel)dualActionViewModel).RaiseAllPropertiesChanged((ThreadInvokeOption)1);
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<device>5__2 = null;
					((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
					return;
				}
				<>1__state = -2;
				<device>5__2 = null;
				((AsyncTaskMethodBuilder)(ref <>t__builder)).SetResult();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				((AsyncTaskMethodBuilder)(ref <>t__builder)).SetStateMachine(stateMachine);
			}
		}

		private NotifyPropertyChangedProxySource? _devicePropertyChangedProxySource;

		protected const string DevicePropertyChangedProxySourceName = "ViewModelBaseDevice_PropertyChangedProxySourceName";

		private ObservableCollection<ICellViewModel>? _settings;

		public TLogicalDevice? Device
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public bool IsOnline
		{
			get
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				TLogicalDevice? device = Device;
				return device != null && (int)((IDevicesCommon)device).ActiveConnection != 0;
			}
		}

		public bool ShouldClearSettings => true;

		public ObservableCollection<ICellViewModel>? Settings
		{
			get
			{
				return _settings ?? (_settings = (ObservableCollection<ICellViewModel>?)(object)new BaseObservableCollection<ICellViewModel>());
			}
			protected set
			{
				((ViewModel)this).SetProperty<ObservableCollection<ICellViewModel>>(ref _settings, value, "Settings", (ThreadInvokeOption)1, global::System.Array.Empty<string>());
			}
		}

		protected DualActionViewModel(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		[AsyncStateMachine(typeof(DualActionViewModel<>.<OnStartAsync>d__9))]
		public override global::System.Threading.Tasks.Task OnStartAsync(INavigationParameters? parameters, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			<OnStartAsync>d__9 <OnStartAsync>d__ = default(<OnStartAsync>d__9);
			<OnStartAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<OnStartAsync>d__.<>4__this = this;
			<OnStartAsync>d__.parameters = parameters;
			<OnStartAsync>d__.cancellationToken = cancellationToken;
			<OnStartAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <OnStartAsync>d__.<>t__builder)).Start<<OnStartAsync>d__9>(ref <OnStartAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <OnStartAsync>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(DualActionViewModel<>.<OnResumeAsync>d__10))]
		public override global::System.Threading.Tasks.Task OnResumeAsync(ResumeReason reason, INavigationParameters parameters, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			<OnResumeAsync>d__10 <OnResumeAsync>d__ = default(<OnResumeAsync>d__10);
			<OnResumeAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<OnResumeAsync>d__.<>4__this = this;
			<OnResumeAsync>d__.reason = reason;
			<OnResumeAsync>d__.parameters = parameters;
			<OnResumeAsync>d__.cancellationToken = cancellationToken;
			<OnResumeAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <OnResumeAsync>d__.<>t__builder)).Start<<OnResumeAsync>d__10>(ref <OnResumeAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <OnResumeAsync>d__.<>t__builder)).Task;
		}

		protected void ClearSettings(global::System.Collections.Generic.IReadOnlyList<ICellViewModel>? settings)
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Expected O, but got Unknown
			if (settings == null)
			{
				return;
			}
			((ViewModel)this).MainThreadService.BeginInvokeOnMainThread((Action)delegate
			{
				try
				{
					global::System.Collections.Generic.IEnumerator<ICellViewModel> enumerator = ((global::System.Collections.Generic.IEnumerable<ICellViewModel>)settings).GetEnumerator();
					try
					{
						while (((global::System.Collections.IEnumerator)enumerator).MoveNext())
						{
							ICellViewModel current = enumerator.Current;
							if (current is IDisplaySettingViewModel displaySettingViewModel)
							{
								ClearSettings(displaySettingViewModel.Children);
							}
							global::System.IDisposable obj = current as global::System.IDisposable;
							if (obj != null)
							{
								IDisposableExtensions.TryDispose(obj);
							}
						}
					}
					finally
					{
						((global::System.IDisposable)enumerator)?.Dispose();
					}
					LoggerExtensions.LogDebug(((PageViewModel)this).Logger, "{SettingsCount} settings disposed", new object[1] { ((global::System.Collections.Generic.IReadOnlyCollection<ICellViewModel>)settings).Count });
				}
				catch (global::System.Exception ex)
				{
					LoggerExtensions.LogWarning(((PageViewModel)this).Logger, "Unable to dispose $settings " + ex.Message, global::System.Array.Empty<object>());
				}
				settings = null;
			});
		}

		public virtual ObservableCollection<ICellViewModel> GetAllSettingsViewModels(CancellationToken cancellationToken = default(CancellationToken))
		{
			ObservableCollection<ICellViewModel> result = new ObservableCollection<ICellViewModel>();
			_ = Device;
			return result;
		}
	}
}
namespace App.Common.Pages.Pairing.ConnectToWiFiGateway
{
	public abstract class ConnectToWiFiGatewayViewModel : ConnectToGatewayViewModel
	{
		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <Connect>d__6 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public ConnectToWiFiGatewayViewModel <>4__this;

			public CancellationToken cancellationToken;

			private int <>7__wrap1;

			private global::System.Exception <e>5__3;

			private TaskAwaiter<ConnectionResult> <>u__1;

			private TaskAwaiter <>u__2;

			private void MoveNext()
			{
				//IL_026f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0274: Unknown result type (might be due to invalid IL or missing references)
				//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
				//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
				//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
				//IL_0289: Unknown result type (might be due to invalid IL or missing references)
				//IL_028b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0097: Unknown result type (might be due to invalid IL or missing references)
				//IL_009c: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
				//IL_0101: Unknown result type (might be due to invalid IL or missing references)
				//IL_0109: Unknown result type (might be due to invalid IL or missing references)
				//IL_016b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0170: Unknown result type (might be due to invalid IL or missing references)
				//IL_0178: Unknown result type (might be due to invalid IL or missing references)
				//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
				//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
				//IL_005a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0064: Unknown result type (might be due to invalid IL or missing references)
				//IL_0069: Unknown result type (might be due to invalid IL or missing references)
				//IL_012c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0136: Unknown result type (might be due to invalid IL or missing references)
				//IL_013b: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
				//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
				//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
				//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
				//IL_007d: Unknown result type (might be due to invalid IL or missing references)
				//IL_007e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0150: Unknown result type (might be due to invalid IL or missing references)
				//IL_0152: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				ConnectToWiFiGatewayViewModel connectToWiFiGatewayViewModel = <>4__this;
				try
				{
					TaskAwaiter val;
					if ((uint)num > 3u)
					{
						if (num == 4)
						{
							val = <>u__2;
							<>u__2 = default(TaskAwaiter);
							num = (<>1__state = -1);
							goto IL_02bd;
						}
						<>7__wrap1 = 0;
					}
					try
					{
						TaskAwaiter<ConnectionResult> val2;
						switch (num)
						{
						default:
							connectToWiFiGatewayViewModel.State = ConnectState.Connecting;
							val2 = connectToWiFiGatewayViewModel.<wifiAdapter>P.ConnectAsync(connectToWiFiGatewayViewModel._ssid, connectToWiFiGatewayViewModel._password ?? string.Empty, cancellationToken).GetAwaiter();
							if (!val2.IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val2;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<ConnectionResult>, <Connect>d__6>(ref val2, ref this);
								return;
							}
							goto IL_00b2;
						case 0:
							val2 = <>u__1;
							<>u__1 = default(TaskAwaiter<ConnectionResult>);
							num = (<>1__state = -1);
							goto IL_00b2;
						case 1:
							val = <>u__2;
							<>u__2 = default(TaskAwaiter);
							num = (<>1__state = -1);
							goto IL_0118;
						case 2:
							val = <>u__2;
							<>u__2 = default(TaskAwaiter);
							num = (<>1__state = -1);
							goto IL_0187;
						case 3:
							{
								val = <>u__2;
								<>u__2 = default(TaskAwaiter);
								num = (<>1__state = -1);
								break;
							}
							IL_0187:
							((TaskAwaiter)(ref val)).GetResult();
							connectToWiFiGatewayViewModel.State = ConnectState.Finishing;
							val = connectToWiFiGatewayViewModel.AddGatewayConnection(new RvGatewayCanConnectionTcpIpWifiGateway(connectToWiFiGatewayViewModel._ssid, connectToWiFiGatewayViewModel._password ?? string.Empty), checkRvKind: true, isBrakingSystem: false, cancellationToken).GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 3);
								<>u__2 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <Connect>d__6>(ref val, ref this);
								return;
							}
							break;
							IL_011f:
							connectToWiFiGatewayViewModel.State = ConnectState.Connected;
							val = global::System.Threading.Tasks.Task.Delay(2000, cancellationToken).GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 2);
								<>u__2 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <Connect>d__6>(ref val, ref this);
								return;
							}
							goto IL_0187;
							IL_0118:
							((TaskAwaiter)(ref val)).GetResult();
							goto IL_011f;
							IL_00b2:
							if ((int)val2.GetResult() != 0)
							{
								val = connectToWiFiGatewayViewModel.Failed("Failed to connect to Wi-Fi gateway").GetAwaiter();
								if (!((TaskAwaiter)(ref val)).IsCompleted)
								{
									num = (<>1__state = 1);
									<>u__2 = val;
									((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <Connect>d__6>(ref val, ref this);
									return;
								}
								goto IL_0118;
							}
							goto IL_011f;
						}
						((TaskAwaiter)(ref val)).GetResult();
					}
					catch (global::System.Exception ex) when (((Func<bool>)delegate
					{
						// Could not convert BlockContainer to single expression
						<e>5__3 = ex;
						return !((CancellationToken)(ref cancellationToken)).IsCancellationRequested;
					}).Invoke())
					{
						<>7__wrap1 = 1;
					}
					int num2 = <>7__wrap1;
					if (num2 == 1)
					{
						val = connectToWiFiGatewayViewModel.Failed("Failed to connect to Wi-Fi gateway", <e>5__3).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 4);
							<>u__2 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <Connect>d__6>(ref val, ref this);
							return;
						}
						goto IL_02bd;
					}
					goto IL_02c4;
					IL_02bd:
					((TaskAwaiter)(ref val)).GetResult();
					goto IL_02c4;
					IL_02c4:
					<e>5__3 = null;
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
		private struct <OnResumeAsync>d__5 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public ConnectToWiFiGatewayViewModel <>4__this;

			public ResumeReason reason;

			public INavigationParameters parameters;

			public CancellationToken cancellationToken;

			private TaskAwaiter <>u__1;

			private TaskAwaiter<PermissionsRequestState> <>u__2;

			private TaskAwaiter<INavigationResult> <>u__3;

			private void MoveNext()
			{
				//IL_006b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0070: Unknown result type (might be due to invalid IL or missing references)
				//IL_0077: Unknown result type (might be due to invalid IL or missing references)
				//IL_011d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0122: Unknown result type (might be due to invalid IL or missing references)
				//IL_012a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0198: Unknown result type (might be due to invalid IL or missing references)
				//IL_019d: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				//IL_008e: Unknown result type (might be due to invalid IL or missing references)
				//IL_013b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0140: Unknown result type (might be due to invalid IL or missing references)
				//IL_0141: Unknown result type (might be due to invalid IL or missing references)
				//IL_0153: Expected I4, but got Unknown
				//IL_0051: Unknown result type (might be due to invalid IL or missing references)
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
				//IL_0166: Unknown result type (might be due to invalid IL or missing references)
				//IL_016b: Unknown result type (might be due to invalid IL or missing references)
				//IL_00de: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
				//IL_0180: Unknown result type (might be due to invalid IL or missing references)
				//IL_0182: Unknown result type (might be due to invalid IL or missing references)
				//IL_0102: Unknown result type (might be due to invalid IL or missing references)
				//IL_0104: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				ConnectToWiFiGatewayViewModel connectToWiFiGatewayViewModel = <>4__this;
				try
				{
					TaskAwaiter val3;
					TaskAwaiter<PermissionsRequestState> val2;
					TaskAwaiter<INavigationResult> val;
					PermissionsRequestState result;
					switch (num)
					{
					default:
						val3 = ((ConnectViewModel)connectToWiFiGatewayViewModel).OnResumeAsync(reason, parameters, cancellationToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val3)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val3;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnResumeAsync>d__5>(ref val3, ref this);
							return;
						}
						goto IL_0086;
					case 0:
						val3 = <>u__1;
						<>u__1 = default(TaskAwaiter);
						num = (<>1__state = -1);
						goto IL_0086;
					case 1:
						val2 = <>u__2;
						<>u__2 = default(TaskAwaiter<PermissionsRequestState>);
						num = (<>1__state = -1);
						goto IL_0139;
					case 2:
						{
							val = <>u__3;
							<>u__3 = default(TaskAwaiter<INavigationResult>);
							num = (<>1__state = -1);
							break;
						}
						IL_0139:
						result = val2.GetResult();
						switch ((int)result)
						{
						case 1:
							goto end_IL_000e;
						default:
							val = ((PageViewModel)connectToWiFiGatewayViewModel).NavigationService.GoBackAsync((INavigationParameters)null).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 2);
								<>u__3 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<INavigationResult>, <OnResumeAsync>d__5>(ref val, ref this);
								return;
							}
							break;
						case 0:
							connectToWiFiGatewayViewModel.Connect(ViewModelExtensions.StoppedCancellationToken((IViewModel)(object)connectToWiFiGatewayViewModel));
							goto end_IL_000e;
						}
						break;
						IL_0086:
						((TaskAwaiter)(ref val3)).GetResult();
						if ((int)reason == 0)
						{
							connectToWiFiGatewayViewModel._ssid = WiFiConnectHelpers.GetSsid(parameters);
							connectToWiFiGatewayViewModel._password = WiFiConnectHelpers.GetPassword(parameters);
						}
						connectToWiFiGatewayViewModel.Name = connectToWiFiGatewayViewModel._ssid ?? string.Empty;
						connectToWiFiGatewayViewModel.Message = Strings.ConnectToWiFiGateway_Message;
						val2 = connectToWiFiGatewayViewModel.<wifiPermissionsHandler>P.RequestPermissionsAsync(cancellationToken).GetAwaiter();
						if (!val2.IsCompleted)
						{
							num = (<>1__state = 1);
							<>u__2 = val2;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<PermissionsRequestState>, <OnResumeAsync>d__5>(ref val2, ref this);
							return;
						}
						goto IL_0139;
					}
					val.GetResult();
					end_IL_000e:;
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

		private string? _password;

		private string? _ssid;

		protected ConnectToWiFiGatewayViewModel(IServiceProvider serviceProvider, IWiFiPermissionsHandler wifiPermissionsHandler, IWifiAdapter wifiAdapter, ILogicalDeviceService logicalDeviceService)
		{
			<wifiPermissionsHandler>P = wifiPermissionsHandler;
			<wifiAdapter>P = wifiAdapter;
			base..ctor(serviceProvider, logicalDeviceService);
		}

		[AsyncStateMachine(typeof(<OnResumeAsync>d__5))]
		public override global::System.Threading.Tasks.Task OnResumeAsync(ResumeReason reason, INavigationParameters parameters, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			<OnResumeAsync>d__5 <OnResumeAsync>d__ = default(<OnResumeAsync>d__5);
			<OnResumeAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<OnResumeAsync>d__.<>4__this = this;
			<OnResumeAsync>d__.reason = reason;
			<OnResumeAsync>d__.parameters = parameters;
			<OnResumeAsync>d__.cancellationToken = cancellationToken;
			<OnResumeAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <OnResumeAsync>d__.<>t__builder)).Start<<OnResumeAsync>d__5>(ref <OnResumeAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <OnResumeAsync>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(<Connect>d__6))]
		private global::System.Threading.Tasks.Task Connect(CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			<Connect>d__6 <Connect>d__ = default(<Connect>d__6);
			<Connect>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<Connect>d__.<>4__this = this;
			<Connect>d__.cancellationToken = cancellationToken;
			<Connect>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <Connect>d__.<>t__builder)).Start<<Connect>d__6>(ref <Connect>d__);
			return ((AsyncTaskMethodBuilder)(ref <Connect>d__.<>t__builder)).Task;
		}
	}
}
namespace App.Common.Pages.Pairing.ConnectToGateway
{
	public class ConnectToGatewayPage : ConnectPage
	{
		private static readonly EqualConverter EqualConverter = new EqualConverter();

		private static readonly OrMultiConverter OrMultiConverter = new OrMultiConverter();

		private readonly Label _message;

		private readonly Label _name;

		public new static readonly BindableProperty ContentProperty = BindableProperty.Create("Content", typeof(View), typeof(ConnectToGatewayPage), (object)null, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)null, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)null, (CreateDefaultValueDelegate)null);

		public new static readonly BindableProperty ControlTemplateProperty = BindableProperty.Create("ControlTemplate", typeof(ControlTemplate), typeof(ConnectToGatewayPage), (object)null, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)null, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)null, (CreateDefaultValueDelegate)null);

		public new View? Content
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected O, but got Unknown
				return (View)((BindableObject)this).GetValue(ContentProperty);
			}
			set
			{
				((BindableObject)this).SetValue(ContentProperty, (object)value);
			}
		}

		public new ControlTemplate? ControlTemplate
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected O, but got Unknown
				return (ControlTemplate)((BindableObject)this).GetValue(ControlTemplateProperty);
			}
			set
			{
				((BindableObject)this).SetValue(ControlTemplateProperty, (object)value);
			}
		}

		public ConnectToGatewayPage()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			//IL_0043: Expected O, but got Unknown
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Expected O, but got Unknown
			//IL_0081: Expected O, but got Unknown
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Expected O, but got Unknown
			//IL_00ec: Expected O, but got Unknown
			VerticalStackLayout val = new VerticalStackLayout
			{
				Spacing = 16.0
			};
			global::System.Collections.Generic.IList<IView> children = ((Layout)val).Children;
			Label val2 = new Label
			{
				HorizontalTextAlignment = (TextAlignment)1,
				LineBreakMode = (LineBreakMode)1,
				MaxLines = 3
			};
			Label val3 = val2;
			_message = val2;
			((global::System.Collections.Generic.ICollection<IView>)children).Add((IView)(object)FluentUIExtensions.DynamicResource<Label>(FluentUIExtensions.DynamicResource<Label>(val3, VisualElement.StyleProperty, "Body4_Label"), Label.TextColorProperty, "OnSurface"));
			global::System.Collections.Generic.IList<IView> children2 = ((Layout)val).Children;
			Label val4 = new Label
			{
				HorizontalTextAlignment = (TextAlignment)1
			};
			val3 = val4;
			_name = val4;
			((global::System.Collections.Generic.ICollection<IView>)children2).Add((IView)(object)FluentUIExtensions.DynamicResource<Label>(FluentUIExtensions.DynamicResource<Label>(val3, VisualElement.StyleProperty, "Body3_Span_Black"), Label.TextColorProperty, "OnSurface"));
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val).Children).Add((IView)(object)FluentUIExtensions.Binding<ContentView>(FluentUIExtensions.Binding<ContentView>(new ContentView(), ContentView.ContentProperty, ContentProperty, (BindableObject)(object)this, ((AppBarPage)this).LifetimeDisposable, (BindingMode)0, (IValueConverter)null, (object)null), TemplatedView.ControlTemplateProperty, ControlTemplateProperty, (BindableObject)(object)this, ((AppBarPage)this).LifetimeDisposable, (BindingMode)0, (IValueConverter)null, (object)null));
			base.Content = (View?)val;
		}

		protected override void OnBindingContextChanged()
		{
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Expected O, but got Unknown
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Expected O, but got Unknown
			//IL_00c9: Expected O, but got Unknown
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Expected O, but got Unknown
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Expected O, but got Unknown
			//IL_0132: Expected O, but got Unknown
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Expected O, but got Unknown
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Expected O, but got Unknown
			//IL_019b: Expected O, but got Unknown
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Expected O, but got Unknown
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Expected O, but got Unknown
			//IL_0204: Expected O, but got Unknown
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Expected O, but got Unknown
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_0268: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Expected O, but got Unknown
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_029c: Expected O, but got Unknown
			//IL_02a1: Expected O, but got Unknown
			((global::System.Collections.Generic.ICollection<TriggerBase>)((VisualElement)_message).Triggers).Clear();
			((BindableObject)_message).RemoveBinding(Button.TextProperty);
			((BindableObject)_name).RemoveBinding(Label.TextProperty);
			((BindableObject)_name).RemoveBinding(VisualElement.IsVisibleProperty);
			((DualActionPage)this).OnBindingContextChanged();
			if (((BindableObject)this).BindingContext is ConnectToGatewayViewModel)
			{
				Label message = _message;
				TriggerBase[] array = new TriggerBase[4];
				DataTrigger val = new DataTrigger(typeof(Label));
				object bindingContext = ((BindableObject)this).BindingContext;
				val.Binding = (BindingBase)new Binding("State", (BindingMode)0, (IValueConverter)(object)EqualConverter, (object)ConnectState.Connecting, (string)null, bindingContext);
				val.Value = true;
				((global::System.Collections.Generic.ICollection<Setter>)val.Setters).Add(new Setter
				{
					Property = Label.TextProperty,
					Value = Strings.ConnectToGateway_Message_Connecting
				});
				array[0] = (TriggerBase)val;
				DataTrigger val2 = new DataTrigger(typeof(Label));
				bindingContext = ((BindableObject)this).BindingContext;
				val2.Binding = (BindingBase)new Binding("State", (BindingMode)0, (IValueConverter)(object)EqualConverter, (object)ConnectState.Connected, (string)null, bindingContext);
				val2.Value = true;
				((global::System.Collections.Generic.ICollection<Setter>)val2.Setters).Add(new Setter
				{
					Property = Label.TextProperty,
					Value = Strings.ConnectToGateway_Message_Connected
				});
				array[1] = (TriggerBase)val2;
				DataTrigger val3 = new DataTrigger(typeof(Label));
				bindingContext = ((BindableObject)this).BindingContext;
				val3.Binding = (BindingBase)new Binding("State", (BindingMode)0, (IValueConverter)(object)EqualConverter, (object)ConnectState.Finishing, (string)null, bindingContext);
				val3.Value = true;
				((global::System.Collections.Generic.ICollection<Setter>)val3.Setters).Add(new Setter
				{
					Property = Label.TextProperty,
					Value = Strings.ConnectToGateway_Message_Finishing
				});
				array[2] = (TriggerBase)val3;
				DataTrigger val4 = new DataTrigger(typeof(Label));
				bindingContext = ((BindableObject)this).BindingContext;
				val4.Binding = (BindingBase)new Binding("State", (BindingMode)0, (IValueConverter)(object)EqualConverter, (object)ConnectState.Finished, (string)null, bindingContext);
				val4.Value = true;
				((global::System.Collections.Generic.ICollection<Setter>)val4.Setters).Add(new Setter
				{
					Property = Label.TextProperty,
					Value = Strings.ConnectToGateway_Message_Finished
				});
				array[3] = (TriggerBase)val4;
				FluentUIExtensions.AddTriggers<Label>(message, (global::System.Collections.Generic.IEnumerable<TriggerBase>)new <>z__ReadOnlyArray<TriggerBase>((TriggerBase[])(object)array));
				Label obj = FluentUIExtensions.Binding<Label>(_name, Label.TextProperty, (BindingBase)new Binding("Name", (BindingMode)0, (IValueConverter)null, (object)null, (string)null, ((BindableObject)this).BindingContext));
				BindableProperty isVisibleProperty = VisualElement.IsVisibleProperty;
				MultiBinding val5 = new MultiBinding
				{
					Converter = (IMultiValueConverter)(object)OrMultiConverter
				};
				global::System.Collections.Generic.IList<BindingBase> bindings = val5.Bindings;
				bindingContext = ((BindableObject)this).BindingContext;
				((global::System.Collections.Generic.ICollection<BindingBase>)bindings).Add((BindingBase)new Binding("State", (BindingMode)0, (IValueConverter)(object)EqualConverter, (object)ConnectState.Connecting, (string)null, bindingContext));
				global::System.Collections.Generic.IList<BindingBase> bindings2 = val5.Bindings;
				bindingContext = ((BindableObject)this).BindingContext;
				((global::System.Collections.Generic.ICollection<BindingBase>)bindings2).Add((BindingBase)new Binding("State", (BindingMode)0, (IValueConverter)(object)EqualConverter, (object)ConnectState.Connected, (string)null, bindingContext));
				FluentUIExtensions.Binding<Label>(obj, isVisibleProperty, (BindingBase)val5);
			}
		}
	}
	public enum ConnectState
	{
		Idle,
		Connecting,
		Connected,
		Finishing,
		Finished,
		Failed
	}
	public abstract class ConnectToGatewayViewModel : ConnectViewModel
	{
		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <<AddGatewayConnection>b__12_0>d : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<INavigationResult> <>t__builder;

			public ConnectToGatewayViewModel <>4__this;

			private TaskAwaiter<INavigationResult> <>u__1;

			private void MoveNext()
			{
				//IL_004c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0051: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				ConnectToGatewayViewModel connectToGatewayViewModel = <>4__this;
				INavigationResult result;
				try
				{
					TaskAwaiter<INavigationResult> val;
					if (num != 0)
					{
						val = connectToGatewayViewModel._notificationRegistrationService.RequestNotificationPermissionAsync().GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<INavigationResult>, <<AddGatewayConnection>b__12_0>d>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<INavigationResult>);
						num = (<>1__state = -1);
					}
					result = val.GetResult();
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

		[CompilerGenerated]
		private sealed class <>c__DisplayClass15_0
		{
			public IRvGatewayConnection connection;

			public ILogicalDeviceSourceDirect deviceSource;

			internal bool <WaitForGatewayConnectionAsync>b__0(ILogicalDeviceSourceDirect source)
			{
				return string.Equals(((ILogicalDeviceSource)source).DeviceSourceToken, connection.DeviceSourceToken, (StringComparison)5);
			}

			internal bool <WaitForGatewayConnectionAsync>b__1(ILogicalDevice ld)
			{
				return ld.IsAssociatedWithDeviceSource((ILogicalDeviceSource)(object)deviceSource);
			}
		}

		[CompilerGenerated]
		private sealed class <>c__DisplayClass16_0
		{
			[StructLayout((LayoutKind)3)]
			private struct <<GetRvKindAsync>b__0>d : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncTaskMethodBuilder<IRvKind> <>t__builder;

				public <>c__DisplayClass16_0 <>4__this;

				private CancellationTokenSource <cts>5__2;

				private CancellationToken <ct>5__3;

				private TaskAwaiter<bool> <>u__1;

				private void MoveNext()
				{
					//IL_0163: Unknown result type (might be due to invalid IL or missing references)
					//IL_0013: Unknown result type (might be due to invalid IL or missing references)
					//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
					//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
					//IL_0105: Unknown result type (might be due to invalid IL or missing references)
					//IL_0032: Unknown result type (might be due to invalid IL or missing references)
					//IL_0043: Unknown result type (might be due to invalid IL or missing references)
					//IL_0048: Unknown result type (might be due to invalid IL or missing references)
					//IL_018d: Unknown result type (might be due to invalid IL or missing references)
					//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
					//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
					//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
					//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
					//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
					//IL_00df: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					<>c__DisplayClass16_0 <>c__DisplayClass16_ = <>4__this;
					IRvKind result;
					try
					{
						if (num != 0)
						{
							<cts>5__2 = CancellationTokenSource.CreateLinkedTokenSource(<>c__DisplayClass16_.cancellationToken);
						}
						try
						{
							if (num != 0)
							{
								<cts>5__2.CancelAfter(TimeSpan.FromSeconds(15L));
								<ct>5__3 = <cts>5__2.Token;
								goto IL_011c;
							}
							TaskAwaiter<bool> val = <>u__1;
							<>u__1 = default(TaskAwaiter<bool>);
							num = (<>1__state = -1);
							goto IL_0114;
							IL_0114:
							val.GetResult();
							goto IL_011c;
							IL_011c:
							if (!((CancellationToken)(ref <ct>5__3)).IsCancellationRequested)
							{
								if (((IEquatable<IRvKind>)(object)<>c__DisplayClass16_.<>4__this._appSettings?.RvKind).Equals((IRvKind)(object)RvKind.None) ?? false)
								{
									val = TaskExtension.TryDelay(TimeSpan.FromSeconds(1L), <ct>5__3).GetAwaiter();
									if (!val.IsCompleted)
									{
										num = (<>1__state = 0);
										<>u__1 = val;
										<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, <<GetRvKindAsync>b__0>d>(ref val, ref this);
										return;
									}
									goto IL_0114;
								}
								result = <>c__DisplayClass16_.<>4__this._appSettings?.RvKind;
							}
							else
							{
								result = (IRvKind)(object)RvKind.None;
							}
						}
						finally
						{
							if (num < 0 && <cts>5__2 != null)
							{
								((global::System.IDisposable)<cts>5__2).Dispose();
							}
						}
					}
					catch (global::System.Exception exception)
					{
						<>1__state = -2;
						<cts>5__2 = null;
						<ct>5__3 = default(CancellationToken);
						<>t__builder.SetException(exception);
						return;
					}
					<>1__state = -2;
					<cts>5__2 = null;
					<ct>5__3 = default(CancellationToken);
					<>t__builder.SetResult(result);
				}

				[DebuggerHidden]
				private void SetStateMachine(IAsyncStateMachine stateMachine)
				{
					<>t__builder.SetStateMachine(stateMachine);
				}
			}

			public CancellationToken cancellationToken;

			public ConnectToGatewayViewModel <>4__this;

			[AsyncStateMachine(typeof(<<GetRvKindAsync>b__0>d))]
			internal async global::System.Threading.Tasks.Task<IRvKind>? <GetRvKindAsync>b__0()
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
				try
				{
					cts.CancelAfter(TimeSpan.FromSeconds(15L));
					CancellationToken ct = cts.Token;
					while (!((CancellationToken)(ref ct)).IsCancellationRequested)
					{
						if ((!(((IEquatable<IRvKind>)(object)<>4__this._appSettings?.RvKind).Equals((IRvKind)(object)RvKind.None))) ?? true)
						{
							return <>4__this._appSettings?.RvKind;
						}
						await TaskExtension.TryDelay(TimeSpan.FromSeconds(1L), ct);
					}
					return (IRvKind)(object)RvKind.None;
				}
				finally
				{
					((global::System.IDisposable)cts)?.Dispose();
				}
			}
		}

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <AddGatewayConnection>d__12 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public bool isBrakingSystem;

			public ConnectToGatewayViewModel <>4__this;

			public IRvGatewayConnection connection;

			public bool checkRvKind;

			public CancellationToken cancellationToken;

			private object <>7__wrap1;

			private int <>7__wrap2;

			private TaskAwaiter <>u__1;

			private TaskAwaiter<IRvKind?> <>u__2;

			private void MoveNext()
			{
				//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
				//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
				//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
				//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
				//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
				//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
				//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
				//IL_0099: Unknown result type (might be due to invalid IL or missing references)
				//IL_009e: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
				//IL_0145: Unknown result type (might be due to invalid IL or missing references)
				//IL_014a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0151: Unknown result type (might be due to invalid IL or missing references)
				//IL_016a: Unknown result type (might be due to invalid IL or missing references)
				//IL_016f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0176: Unknown result type (might be due to invalid IL or missing references)
				//IL_0212: Unknown result type (might be due to invalid IL or missing references)
				//IL_0217: Unknown result type (might be due to invalid IL or missing references)
				//IL_021e: Unknown result type (might be due to invalid IL or missing references)
				//IL_024a: Unknown result type (might be due to invalid IL or missing references)
				//IL_01df: Unknown result type (might be due to invalid IL or missing references)
				//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
				//IL_0261: Unknown result type (might be due to invalid IL or missing references)
				//IL_0267: Invalid comparison between Unknown and I4
				//IL_0066: Unknown result type (might be due to invalid IL or missing references)
				//IL_006b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0108: Unknown result type (might be due to invalid IL or missing references)
				//IL_0112: Unknown result type (might be due to invalid IL or missing references)
				//IL_0117: Unknown result type (might be due to invalid IL or missing references)
				//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
				//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
				//IL_007f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0080: Unknown result type (might be due to invalid IL or missing references)
				//IL_012b: Unknown result type (might be due to invalid IL or missing references)
				//IL_012c: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				ConnectToGatewayViewModel CS$<>8__locals12 = <>4__this;
				try
				{
					TaskAwaiter val;
					if ((uint)num > 3u)
					{
						if (num == 4)
						{
							val = <>u__1;
							<>u__1 = default(TaskAwaiter);
							num = (<>1__state = -1);
							goto IL_0308;
						}
						<>7__wrap2 = 0;
					}
					try
					{
						ILogger logger;
						switch (num)
						{
						default:
							if (!isBrakingSystem)
							{
								CS$<>8__locals12.RemoveOldLogicalDevices();
							}
							if (isBrakingSystem)
							{
								val = (CS$<>8__locals12._appSettings?.SetSelectedBrakingSystemGatewayConnection(connection, saveSelectedAbs: true)).GetAwaiter();
								if (!((TaskAwaiter)(ref val)).IsCompleted)
								{
									num = (<>1__state = 0);
									<>u__1 = val;
									((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <AddGatewayConnection>d__12>(ref val, ref this);
									return;
								}
								goto IL_00b4;
							}
							CS$<>8__locals12._appSettings?.SetSelectedRvGatewayConnection(connection, saveSelectedRv: true);
							goto IL_00d5;
						case 0:
							val = <>u__1;
							<>u__1 = default(TaskAwaiter);
							num = (<>1__state = -1);
							goto IL_00b4;
						case 1:
							val = <>u__1;
							<>u__1 = default(TaskAwaiter);
							num = (<>1__state = -1);
							goto IL_0160;
						case 2:
						{
							TaskAwaiter<IRvKind> val2 = <>u__2;
							<>u__2 = default(TaskAwaiter<IRvKind>);
							num = (<>1__state = -1);
							bool? flag = ((IEquatable<IRvKind>)(object)val2.GetResult())?.Equals((IRvKind)(object)RvKind.None);
							if (flag.HasValue)
							{
								_ = flag == true;
							}
							goto IL_01be;
						}
						case 3:
							{
								val = <>u__1;
								<>u__1 = default(TaskAwaiter);
								num = (<>1__state = -1);
								break;
							}
							IL_00d5:
							if (checkRvKind)
							{
								CS$<>8__locals12._appSettings?.ClearYmmfSettings();
							}
							CS$<>8__locals12._appSettings?.Save();
							val = CS$<>8__locals12.WaitForGatewayConnectionAsync(connection, cancellationToken).GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 1);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <AddGatewayConnection>d__12>(ref val, ref this);
								return;
							}
							goto IL_0160;
							IL_0160:
							((TaskAwaiter)(ref val)).GetResult();
							goto IL_01be;
							IL_00b4:
							((TaskAwaiter)(ref val)).GetResult();
							goto IL_00d5;
							IL_01be:
							logger = ((PageViewModel)CS$<>8__locals12).Logger;
							if (logger != null)
							{
								LoggerExtensions.LogDebug(logger, "Connected to gateway successfully", global::System.Array.Empty<object>());
							}
							val = CS$<>8__locals12.Succeeded().GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 3);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <AddGatewayConnection>d__12>(ref val, ref this);
								return;
							}
							break;
						}
						((TaskAwaiter)(ref val)).GetResult();
						INotificationRegistrationService? notificationRegistrationService = CS$<>8__locals12._notificationRegistrationService;
						NotificationCapability? val3 = ((notificationRegistrationService != null) ? new NotificationCapability?(notificationRegistrationService.NotificationCapability) : ((NotificationCapability?)null));
						if (val3.HasValue && (int)val3.GetValueOrDefault() == 1)
						{
							((ViewModel)CS$<>8__locals12).MainThreadService.InvokeOnMainThreadDeferredAsync<INavigationResult>((Func<global::System.Threading.Tasks.Task<INavigationResult>>)([AsyncStateMachine(typeof(<<AddGatewayConnection>b__12_0>d))] [CompilerGenerated] async () => await CS$<>8__locals12._notificationRegistrationService.RequestNotificationPermissionAsync()));
						}
					}
					catch (global::System.Exception ex)
					{
						<>7__wrap1 = ex;
						<>7__wrap2 = 1;
					}
					int num2 = <>7__wrap2;
					if (num2 == 1)
					{
						global::System.Exception e = (global::System.Exception)<>7__wrap1;
						val = CS$<>8__locals12.Failed("Exception connecting to gateway", e).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 4);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <AddGatewayConnection>d__12>(ref val, ref this);
							return;
						}
						goto IL_0308;
					}
					goto IL_030f;
					IL_0308:
					((TaskAwaiter)(ref val)).GetResult();
					goto IL_030f;
					IL_030f:
					<>7__wrap1 = null;
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
		private struct <Failed>d__13 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public global::System.Exception e;

			public ConnectToGatewayViewModel <>4__this;

			public string message;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
				//IL_0101: Unknown result type (might be due to invalid IL or missing references)
				//IL_0108: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				ConnectToGatewayViewModel connectToGatewayViewModel = <>4__this;
				try
				{
					TaskAwaiter val;
					if (num != 0)
					{
						if (e != null)
						{
							IAppAnalytics appAnalytics = ((PageViewModel)connectToGatewayViewModel).AppAnalytics;
							if (appAnalytics != null)
							{
								global::System.Exception ex = e;
								Dictionary<string, string> obj = new Dictionary<string, string>();
								obj.Add("errorCount", connectToGatewayViewModel.ErrorCount.ToString());
								obj.Add("message", message);
								appAnalytics.TrackError(ex, obj);
							}
							ILogger logger = ((PageViewModel)connectToGatewayViewModel).Logger;
							if (logger != null)
							{
								LoggerExtensions.LogError(logger, e, "Failed gateway connection: {Message}", new object[1] { message });
							}
						}
						else
						{
							ILogger logger2 = ((PageViewModel)connectToGatewayViewModel).Logger;
							if (logger2 != null)
							{
								LoggerExtensions.LogError(logger2, "Failed gateway connection: {Message}", new object[1] { message });
							}
						}
						connectToGatewayViewModel.ErrorCount++;
						connectToGatewayViewModel.State = ConnectState.Failed;
						val = connectToGatewayViewModel.ConnectionFailed().GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <Failed>d__13>(ref val, ref this);
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
		private struct <GetRvKindAsync>d__16 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<IRvKind> <>t__builder;

			public CancellationToken cancellationToken;

			public ConnectToGatewayViewModel <>4__this;

			private TaskAwaiter<IRvKind> <>u__1;

			private void MoveNext()
			{
				//IL_006f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0074: Unknown result type (might be due to invalid IL or missing references)
				//IL_007b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0044: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_0059: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				IRvKind result;
				try
				{
					TaskAwaiter<IRvKind> val;
					if (num != 0)
					{
						<>c__DisplayClass16_0 CS$<>8__locals4 = new <>c__DisplayClass16_0
						{
							cancellationToken = cancellationToken,
							<>4__this = <>4__this
						};
						val = global::System.Threading.Tasks.Task.Run<IRvKind>((Func<global::System.Threading.Tasks.Task<IRvKind>>)([AsyncStateMachine(typeof(<>c__DisplayClass16_0.<<GetRvKindAsync>b__0>d))] async () =>
						{
							//IL_0002: Unknown result type (might be due to invalid IL or missing references)
							//IL_0007: Unknown result type (might be due to invalid IL or missing references)
							CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(CS$<>8__locals4.cancellationToken);
							try
							{
								cts.CancelAfter(TimeSpan.FromSeconds(15L));
								CancellationToken ct = cts.Token;
								while (!((CancellationToken)(ref ct)).IsCancellationRequested)
								{
									if ((!(((IEquatable<IRvKind>)(object)CS$<>8__locals4.<>4__this._appSettings?.RvKind).Equals((IRvKind)(object)RvKind.None))) ?? true)
									{
										return CS$<>8__locals4.<>4__this._appSettings?.RvKind;
									}
									await TaskExtension.TryDelay(TimeSpan.FromSeconds(1L), ct);
								}
								return (IRvKind)(object)RvKind.None;
							}
							finally
							{
								((global::System.IDisposable)cts)?.Dispose();
							}
						}), CS$<>8__locals4.cancellationToken).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IRvKind>, <GetRvKindAsync>d__16>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<IRvKind>);
						num = (<>1__state = -1);
					}
					result = val.GetResult();
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

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <Succeeded>d__14 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public ConnectToGatewayViewModel <>4__this;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0069: Unknown result type (might be due to invalid IL or missing references)
				//IL_006e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0075: Unknown result type (might be due to invalid IL or missing references)
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_003e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				ConnectToGatewayViewModel connectToGatewayViewModel = <>4__this;
				try
				{
					TaskAwaiter val;
					if (num != 0)
					{
						ILogger logger = ((PageViewModel)connectToGatewayViewModel).Logger;
						if (logger != null)
						{
							LoggerExtensions.LogDebug(logger, "Connected to gateway", global::System.Array.Empty<object>());
						}
						connectToGatewayViewModel.State = ConnectState.Finished;
						val = connectToGatewayViewModel.ConnectionSucceeded().GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <Succeeded>d__14>(ref val, ref this);
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
		private struct <WaitForGatewayConnectionAsync>d__15 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public IRvGatewayConnection connection;

			private <>c__DisplayClass15_0 <>8__1;

			public ConnectToGatewayViewModel <>4__this;

			public CancellationToken cancellationToken;

			private global::System.DateTime <startTime>5__2;

			private ILogicalDevice <device>5__3;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
				//IL_0104: Unknown result type (might be due to invalid IL or missing references)
				//IL_010b: Unknown result type (might be due to invalid IL or missing references)
				//IL_019d: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
				//IL_0277: Unknown result type (might be due to invalid IL or missing references)
				//IL_027c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0283: Unknown result type (might be due to invalid IL or missing references)
				//IL_030e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0313: Unknown result type (might be due to invalid IL or missing references)
				//IL_031a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0390: Unknown result type (might be due to invalid IL or missing references)
				//IL_0395: Unknown result type (might be due to invalid IL or missing references)
				//IL_039c: Unknown result type (might be due to invalid IL or missing references)
				//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
				//IL_0135: Unknown result type (might be due to invalid IL or missing references)
				//IL_013a: Unknown result type (might be due to invalid IL or missing references)
				//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
				//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
				//IL_016a: Unknown result type (might be due to invalid IL or missing references)
				//IL_016f: Unknown result type (might be due to invalid IL or missing references)
				//IL_02db: Unknown result type (might be due to invalid IL or missing references)
				//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
				//IL_0183: Unknown result type (might be due to invalid IL or missing references)
				//IL_0184: Unknown result type (might be due to invalid IL or missing references)
				//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
				//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
				//IL_0356: Unknown result type (might be due to invalid IL or missing references)
				//IL_0360: Unknown result type (might be due to invalid IL or missing references)
				//IL_0365: Unknown result type (might be due to invalid IL or missing references)
				//IL_0379: Unknown result type (might be due to invalid IL or missing references)
				//IL_037a: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
				//IL_023a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0244: Unknown result type (might be due to invalid IL or missing references)
				//IL_0249: Unknown result type (might be due to invalid IL or missing references)
				//IL_025d: Unknown result type (might be due to invalid IL or missing references)
				//IL_025e: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				ConnectToGatewayViewModel connectToGatewayViewModel = <>4__this;
				try
				{
					TaskAwaiter val;
					TimeSpan val2;
					ILogger logger;
					ILogger logger2;
					switch (num)
					{
					default:
						<>8__1 = new <>c__DisplayClass15_0();
						<>8__1.connection = connection;
						<>8__1.deviceSource = null;
						<startTime>5__2 = global::System.DateTime.Now;
						goto IL_0121;
					case 0:
						val = <>u__1;
						<>u__1 = default(TaskAwaiter);
						num = (<>1__state = -1);
						goto IL_011a;
					case 1:
						val = <>u__1;
						<>u__1 = default(TaskAwaiter);
						num = (<>1__state = -1);
						goto IL_01b8;
					case 2:
						val = <>u__1;
						<>u__1 = default(TaskAwaiter);
						num = (<>1__state = -1);
						goto IL_0292;
					case 3:
						val = <>u__1;
						<>u__1 = default(TaskAwaiter);
						num = (<>1__state = -1);
						goto IL_0329;
					case 4:
						{
							val = <>u__1;
							<>u__1 = default(TaskAwaiter);
							num = (<>1__state = -1);
							goto IL_03ab;
						}
						IL_0121:
						while (<>8__1.deviceSource == null)
						{
							val2 = connectToGatewayViewModel.Elapsed(<startTime>5__2);
							if (!(((TimeSpan)(ref val2)).TotalSeconds < ((TimeSpan)(ref DeviceTimeout)).TotalSeconds))
							{
								break;
							}
							<>8__1.deviceSource = Enumerable.FirstOrDefault<ILogicalDeviceSourceDirect>(connectToGatewayViewModel.<logicalDeviceService>P.DeviceSourceManager.DeviceSources, (Func<ILogicalDeviceSourceDirect, bool>)((ILogicalDeviceSourceDirect source) => string.Equals(((ILogicalDeviceSource)source).DeviceSourceToken, <>8__1.connection.DeviceSourceToken, (StringComparison)5)));
							if (<>8__1.deviceSource != null)
							{
								continue;
							}
							goto IL_00a1;
						}
						if (<>8__1.deviceSource == null)
						{
							val = connectToGatewayViewModel.Failed("Could not find device source").GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 1);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <WaitForGatewayConnectionAsync>d__15>(ref val, ref this);
								return;
							}
							goto IL_01b8;
						}
						<device>5__3 = null;
						<startTime>5__2 = global::System.DateTime.Now;
						goto IL_0299;
						IL_00a1:
						logger = ((PageViewModel)connectToGatewayViewModel).Logger;
						if (logger != null)
						{
							LoggerExtensions.LogDebug(logger, "Connecting to gateway, waiting for device source from logical device service", global::System.Array.Empty<object>());
						}
						val = global::System.Threading.Tasks.Task.Delay(1000, cancellationToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <WaitForGatewayConnectionAsync>d__15>(ref val, ref this);
							return;
						}
						goto IL_011a;
						IL_03ab:
						((TaskAwaiter)(ref val)).GetResult();
						goto IL_03b2;
						IL_0329:
						((TaskAwaiter)(ref val)).GetResult();
						break;
						IL_0292:
						((TaskAwaiter)(ref val)).GetResult();
						goto IL_0299;
						IL_011a:
						((TaskAwaiter)(ref val)).GetResult();
						goto IL_0121;
						IL_0219:
						logger2 = ((PageViewModel)connectToGatewayViewModel).Logger;
						if (logger2 != null)
						{
							LoggerExtensions.LogDebug(logger2, "Connecting to gateway, waiting for a device to appear in logical device service for device source", global::System.Array.Empty<object>());
						}
						val = global::System.Threading.Tasks.Task.Delay(1000, cancellationToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 2);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <WaitForGatewayConnectionAsync>d__15>(ref val, ref this);
							return;
						}
						goto IL_0292;
						IL_0299:
						while (<device>5__3 == null)
						{
							val2 = connectToGatewayViewModel.Elapsed(<startTime>5__2);
							if (!(((TimeSpan)(ref val2)).TotalSeconds < ((TimeSpan)(ref DeviceTimeout)).TotalSeconds))
							{
								break;
							}
							ILogicalDeviceManager deviceManager = connectToGatewayViewModel.<logicalDeviceService>P.DeviceManager;
							<device>5__3 = ((deviceManager != null) ? Enumerable.FirstOrDefault<ILogicalDevice>(deviceManager.LogicalDevices, (Func<ILogicalDevice, bool>)((ILogicalDevice ld) => ld.IsAssociatedWithDeviceSource((ILogicalDeviceSource)(object)<>8__1.deviceSource))) : null);
							if (<device>5__3 != null)
							{
								continue;
							}
							goto IL_0219;
						}
						if (<device>5__3 == null)
						{
							val = connectToGatewayViewModel.Failed("Could not find device").GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 3);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <WaitForGatewayConnectionAsync>d__15>(ref val, ref this);
								return;
							}
							goto IL_0329;
						}
						goto IL_03b2;
						IL_03b2:
						if ((int)((IDevicesCommon)<device>5__3).ActiveConnection == 0)
						{
							ILogger logger3 = ((PageViewModel)connectToGatewayViewModel).Logger;
							if (logger3 != null)
							{
								LoggerExtensions.LogDebug(logger3, "Connecting to gateway, waiting for a device to come online", global::System.Array.Empty<object>());
							}
							val = global::System.Threading.Tasks.Task.Delay(1000, cancellationToken).GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 4);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <WaitForGatewayConnectionAsync>d__15>(ref val, ref this);
								return;
							}
							goto IL_03ab;
						}
						break;
						IL_01b8:
						((TaskAwaiter)(ref val)).GetResult();
						break;
					}
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<>8__1 = null;
					<device>5__3 = null;
					((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
					return;
				}
				<>1__state = -2;
				<>8__1 = null;
				<device>5__3 = null;
				((AsyncTaskMethodBuilder)(ref <>t__builder)).SetResult();
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				((AsyncTaskMethodBuilder)(ref <>t__builder)).SetStateMachine(stateMachine);
			}
		}

		private static readonly TimeSpan DeviceTimeout = TimeSpan.FromSeconds(60L);

		private readonly IAppPairingSettings? _appSettings;

		private readonly INotificationRegistrationService? _notificationRegistrationService;

		[ObservableProperty]
		private ConnectState _state;

		[ObservableProperty]
		private string? _name;

		[ObservableProperty]
		private string? _message;

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public ConnectState State
		{
			get
			{
				return _state;
			}
			set
			{
				if (!EqualityComparer<ConnectState>.Default.Equals(_state, value))
				{
					((ObservableObject)this).OnPropertyChanging(__KnownINotifyPropertyChangingArgs.State);
					_state = value;
					((ObservableObject)this).OnPropertyChanged(__KnownINotifyPropertyChangedArgs.State);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string? Name
		{
			get
			{
				return _name;
			}
			set
			{
				if (!EqualityComparer<string>.Default.Equals(_name, value))
				{
					((ObservableObject)this).OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Name);
					_name = value;
					((ObservableObject)this).OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Name);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string? Message
		{
			get
			{
				return _message;
			}
			set
			{
				if (!EqualityComparer<string>.Default.Equals(_message, value))
				{
					((ObservableObject)this).OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Message);
					_message = value;
					((ObservableObject)this).OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Message);
				}
			}
		}

		protected ConnectToGatewayViewModel(IServiceProvider serviceProvider, ILogicalDeviceService logicalDeviceService)
		{
			<logicalDeviceService>P = logicalDeviceService;
			_appSettings = ServiceProviderServiceExtensions.GetRequiredService<IAppPairingSettings>(serviceProvider);
			_notificationRegistrationService = ServiceProviderServiceExtensions.GetService<INotificationRegistrationService>(serviceProvider);
			base..ctor(serviceProvider);
		}

		protected abstract bool IsBrakingSystem(IBleScanResult bleScanResult);

		protected abstract global::System.Threading.Tasks.Task ConnectionFailed();

		protected abstract global::System.Threading.Tasks.Task ConnectionSucceeded();

		public override bool OnConfirmNavigation(NavigationReason reason, INavigationParameters parameters)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Invalid comparison between Unknown and I4
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			bool flag = (((int)reason == 2 || (int)reason == 10) ? true : false);
			if (flag && State == ConnectState.Finished)
			{
				return false;
			}
			return base.OnConfirmNavigation(reason, parameters);
		}

		[AsyncStateMachine(typeof(<AddGatewayConnection>d__12))]
		protected global::System.Threading.Tasks.Task AddGatewayConnection(IRvGatewayConnection connection, bool checkRvKind, bool isBrakingSystem, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			<AddGatewayConnection>d__12 <AddGatewayConnection>d__ = default(<AddGatewayConnection>d__12);
			<AddGatewayConnection>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<AddGatewayConnection>d__.<>4__this = this;
			<AddGatewayConnection>d__.connection = connection;
			<AddGatewayConnection>d__.checkRvKind = checkRvKind;
			<AddGatewayConnection>d__.isBrakingSystem = isBrakingSystem;
			<AddGatewayConnection>d__.cancellationToken = cancellationToken;
			<AddGatewayConnection>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <AddGatewayConnection>d__.<>t__builder)).Start<<AddGatewayConnection>d__12>(ref <AddGatewayConnection>d__);
			return ((AsyncTaskMethodBuilder)(ref <AddGatewayConnection>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(<Failed>d__13))]
		protected global::System.Threading.Tasks.Task Failed(string message, global::System.Exception? e = null)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			<Failed>d__13 <Failed>d__ = default(<Failed>d__13);
			<Failed>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<Failed>d__.<>4__this = this;
			<Failed>d__.message = message;
			<Failed>d__.e = e;
			<Failed>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <Failed>d__.<>t__builder)).Start<<Failed>d__13>(ref <Failed>d__);
			return ((AsyncTaskMethodBuilder)(ref <Failed>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(<Succeeded>d__14))]
		private global::System.Threading.Tasks.Task Succeeded()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			<Succeeded>d__14 <Succeeded>d__ = default(<Succeeded>d__14);
			<Succeeded>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<Succeeded>d__.<>4__this = this;
			<Succeeded>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <Succeeded>d__.<>t__builder)).Start<<Succeeded>d__14>(ref <Succeeded>d__);
			return ((AsyncTaskMethodBuilder)(ref <Succeeded>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(<WaitForGatewayConnectionAsync>d__15))]
		private global::System.Threading.Tasks.Task WaitForGatewayConnectionAsync(IRvGatewayConnection connection, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			<WaitForGatewayConnectionAsync>d__15 <WaitForGatewayConnectionAsync>d__ = default(<WaitForGatewayConnectionAsync>d__15);
			<WaitForGatewayConnectionAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<WaitForGatewayConnectionAsync>d__.<>4__this = this;
			<WaitForGatewayConnectionAsync>d__.connection = connection;
			<WaitForGatewayConnectionAsync>d__.cancellationToken = cancellationToken;
			<WaitForGatewayConnectionAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <WaitForGatewayConnectionAsync>d__.<>t__builder)).Start<<WaitForGatewayConnectionAsync>d__15>(ref <WaitForGatewayConnectionAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <WaitForGatewayConnectionAsync>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(<GetRvKindAsync>d__16))]
		private async global::System.Threading.Tasks.Task<IRvKind?> GetRvKindAsync(CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			return await global::System.Threading.Tasks.Task.Run<IRvKind>((Func<global::System.Threading.Tasks.Task<IRvKind>>)([AsyncStateMachine(typeof(<>c__DisplayClass16_0.<<GetRvKindAsync>b__0>d))] async () =>
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
				try
				{
					cts.CancelAfter(TimeSpan.FromSeconds(15L));
					CancellationToken ct = cts.Token;
					while (!((CancellationToken)(ref ct)).IsCancellationRequested)
					{
						if ((!(((IEquatable<IRvKind>)(object)_appSettings?.RvKind).Equals((IRvKind)(object)RvKind.None))) ?? true)
						{
							return _appSettings?.RvKind;
						}
						await TaskExtension.TryDelay(TimeSpan.FromSeconds(1L), ct);
					}
					return (IRvKind)(object)RvKind.None;
				}
				finally
				{
					((global::System.IDisposable)cts)?.Dispose();
				}
			}), cancellationToken);
		}

		private TimeSpan Elapsed(global::System.DateTime startTime)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return global::System.DateTime.Now - startTime;
		}

		private void RemoveOldLogicalDevices()
		{
			_appSettings?.SetSelectedRvGatewayConnection(DefaultConnections.DefaultRvDirectConnectionNone, saveSelectedRv: false);
			ILogicalDeviceManager deviceManager = <logicalDeviceService>P.DeviceManager;
			if (deviceManager == null)
			{
				return;
			}
			deviceManager.RemoveLogicalDevice((Func<ILogicalDevice, bool>)delegate(ILogicalDevice logicalDevice)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				if ((int)((IDevicesCommon)logicalDevice).ActiveConnection != 0)
				{
					return false;
				}
				ILogicalDeviceAccessory val = (ILogicalDeviceAccessory)(object)((logicalDevice is ILogicalDeviceAccessory) ? logicalDevice : null);
				return val == null || val.AllowAutoOfflineLogicalDeviceRemoval;
			});
		}
	}
}
namespace App.Common.Pages.Pairing.ConnectToBluetoothGateway
{
	public abstract class ConnectToBluetoothGatewayViewModel : ConnectToGatewayViewModel
	{
		[CompilerGenerated]
		private sealed class <>c__DisplayClass9_0
		{
			[StructLayout((LayoutKind)3)]
			private struct <<Connect>b__1>d : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncValueTaskMethodBuilder<IDevice> <>t__builder;

				public <>c__DisplayClass9_0 <>4__this;

				public CancellationToken ct;

				private TaskAwaiter<IDevice?> <>u__1;

				private void MoveNext()
				{
					//IL_0062: Unknown result type (might be due to invalid IL or missing references)
					//IL_0067: Unknown result type (might be due to invalid IL or missing references)
					//IL_006e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0022: Unknown result type (might be due to invalid IL or missing references)
					//IL_0028: Unknown result type (might be due to invalid IL or missing references)
					//IL_0032: Unknown result type (might be due to invalid IL or missing references)
					//IL_0037: Unknown result type (might be due to invalid IL or missing references)
					//IL_004b: Unknown result type (might be due to invalid IL or missing references)
					//IL_004c: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					<>c__DisplayClass9_0 <>c__DisplayClass9_ = <>4__this;
					IDevice result2;
					try
					{
						TaskAwaiter<IDevice> val;
						if (num != 0)
						{
							val = <>c__DisplayClass9_.<>4__this.<bleManager>P.TryConnectToDeviceAsync(<>c__DisplayClass9_.bleScanResult.DeviceId, ct).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IDevice>, <<Connect>b__1>d>(ref val, ref this);
								return;
							}
						}
						else
						{
							val = <>u__1;
							<>u__1 = default(TaskAwaiter<IDevice>);
							num = (<>1__state = -1);
						}
						IDevice result = val.GetResult();
						result2 = result ?? throw new global::System.Exception("Did not connect to device.");
					}
					catch (global::System.Exception exception)
					{
						<>1__state = -2;
						<>t__builder.SetException(exception);
						return;
					}
					<>1__state = -2;
					<>t__builder.SetResult(result2);
				}

				[DebuggerHidden]
				private void SetStateMachine(IAsyncStateMachine stateMachine)
				{
					<>t__builder.SetStateMachine(stateMachine);
				}
			}

			public ConnectToBluetoothGatewayViewModel <>4__this;

			public IBleScanResult bleScanResult;

			[AsyncStateMachine(typeof(<<Connect>b__1>d))]
			internal global::System.Threading.Tasks.ValueTask<IDevice> <Connect>b__1(CancellationToken ct)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				<<Connect>b__1>d <<Connect>b__1>d = default(<<Connect>b__1>d);
				<<Connect>b__1>d.<>t__builder = AsyncValueTaskMethodBuilder<IDevice>.Create();
				<<Connect>b__1>d.<>4__this = this;
				<<Connect>b__1>d.ct = ct;
				<<Connect>b__1>d.<>1__state = -1;
				<<Connect>b__1>d.<>t__builder.Start<<<Connect>b__1>d>(ref <<Connect>b__1>d);
				return <<Connect>b__1>d.<>t__builder.Task;
			}
		}

		[CompilerGenerated]
		private sealed class <>c__DisplayClass9_1
		{
			[StructLayout((LayoutKind)3)]
			private struct <<Connect>b__0>d : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncValueTaskMethodBuilder<string> <>t__builder;

				public <>c__DisplayClass9_1 <>4__this;

				public CancellationToken ct;

				private TaskAwaiter<string?> <>u__1;

				private void MoveNext()
				{
					//IL_005d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0062: Unknown result type (might be due to invalid IL or missing references)
					//IL_0069: Unknown result type (might be due to invalid IL or missing references)
					//IL_0023: Unknown result type (might be due to invalid IL or missing references)
					//IL_002d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0032: Unknown result type (might be due to invalid IL or missing references)
					//IL_0046: Unknown result type (might be due to invalid IL or missing references)
					//IL_0047: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					<>c__DisplayClass9_1 <>c__DisplayClass9_ = <>4__this;
					string result;
					try
					{
						TaskAwaiter<string> val;
						if (num != 0)
						{
							val = <>c__DisplayClass9_.CS$<>8__locals1.<>4__this.ConnectToLegacyGatewayAsync((IBleGatewayScanResult)(object)<>c__DisplayClass9_.legacyBleGatewayScanResult, ct).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, <<Connect>b__0>d>(ref val, ref this);
								return;
							}
						}
						else
						{
							val = <>u__1;
							<>u__1 = default(TaskAwaiter<string>);
							num = (<>1__state = -1);
						}
						result = val.GetResult() ?? throw new global::System.Exception("Did not connect to device.");
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

			public BleGatewayScanResult legacyBleGatewayScanResult;

			public <>c__DisplayClass9_0 CS$<>8__locals1;

			[AsyncStateMachine(typeof(<<Connect>b__0>d))]
			internal global::System.Threading.Tasks.ValueTask<string> <Connect>b__0(CancellationToken ct)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				<<Connect>b__0>d <<Connect>b__0>d = default(<<Connect>b__0>d);
				<<Connect>b__0>d.<>t__builder = AsyncValueTaskMethodBuilder<string>.Create();
				<<Connect>b__0>d.<>4__this = this;
				<<Connect>b__0>d.ct = ct;
				<<Connect>b__0>d.<>1__state = -1;
				<<Connect>b__0>d.<>t__builder.Start<<<Connect>b__0>d>(ref <<Connect>b__0>d);
				return <<Connect>b__0>d.<>t__builder.Task;
			}
		}

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <Connect>d__9 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public ConnectToBluetoothGatewayViewModel <>4__this;

			public IBleScanResult bleScanResult;

			public CancellationToken cancellationToken;

			private <>c__DisplayClass9_1 <>8__1;

			private int <>7__wrap1;

			private global::System.Exception <e>5__3;

			private bool <isLegacyPinGateway>5__4;

			private string <pin>5__5;

			private IRvGatewayConnection <connection>5__6;

			private ValueTaskAwaiter<string> <>u__1;

			private TaskAwaiter <>u__2;

			private ValueTaskAwaiter<IDevice> <>u__3;

			private void MoveNext()
			{
				//IL_05d4: Unknown result type (might be due to invalid IL or missing references)
				//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
				//IL_0606: Unknown result type (might be due to invalid IL or missing references)
				//IL_060b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0613: Unknown result type (might be due to invalid IL or missing references)
				//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
				//IL_05f0: Unknown result type (might be due to invalid IL or missing references)
				//IL_0148: Unknown result type (might be due to invalid IL or missing references)
				//IL_014d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0155: Unknown result type (might be due to invalid IL or missing references)
				//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
				//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
				//IL_024e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0253: Unknown result type (might be due to invalid IL or missing references)
				//IL_025b: Unknown result type (might be due to invalid IL or missing references)
				//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
				//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
				//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
				//IL_0445: Unknown result type (might be due to invalid IL or missing references)
				//IL_044a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0452: Unknown result type (might be due to invalid IL or missing references)
				//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
				//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
				//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
				//IL_0542: Unknown result type (might be due to invalid IL or missing references)
				//IL_0547: Unknown result type (might be due to invalid IL or missing references)
				//IL_054f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0503: Unknown result type (might be due to invalid IL or missing references)
				//IL_050d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0512: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d5: Invalid comparison between Unknown and I4
				//IL_0191: Unknown result type (might be due to invalid IL or missing references)
				//IL_0196: Unknown result type (might be due to invalid IL or missing references)
				//IL_0527: Unknown result type (might be due to invalid IL or missing references)
				//IL_0529: Unknown result type (might be due to invalid IL or missing references)
				//IL_032f: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
				//IL_0288: Unknown result type (might be due to invalid IL or missing references)
				//IL_028d: Unknown result type (might be due to invalid IL or missing references)
				//IL_020b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0219: Unknown result type (might be due to invalid IL or missing references)
				//IL_021e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0105: Unknown result type (might be due to invalid IL or missing references)
				//IL_0113: Unknown result type (might be due to invalid IL or missing references)
				//IL_0118: Unknown result type (might be due to invalid IL or missing references)
				//IL_0473: Unknown result type (might be due to invalid IL or missing references)
				//IL_047d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0482: Unknown result type (might be due to invalid IL or missing references)
				//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
				//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
				//IL_0233: Unknown result type (might be due to invalid IL or missing references)
				//IL_0235: Unknown result type (might be due to invalid IL or missing references)
				//IL_012d: Unknown result type (might be due to invalid IL or missing references)
				//IL_012f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0497: Unknown result type (might be due to invalid IL or missing references)
				//IL_0499: Unknown result type (might be due to invalid IL or missing references)
				//IL_0362: Unknown result type (might be due to invalid IL or missing references)
				//IL_0369: Unknown result type (might be due to invalid IL or missing references)
				//IL_0410: Unknown result type (might be due to invalid IL or missing references)
				//IL_0415: Unknown result type (might be due to invalid IL or missing references)
				//IL_038c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0393: Unknown result type (might be due to invalid IL or missing references)
				//IL_042a: Unknown result type (might be due to invalid IL or missing references)
				//IL_042c: Unknown result type (might be due to invalid IL or missing references)
				//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
				//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				ConnectToBluetoothGatewayViewModel connectToBluetoothGatewayViewModel = <>4__this;
				try
				{
					TaskAwaiter val;
					<>c__DisplayClass9_0 cS$<>8__locals = default(<>c__DisplayClass9_0);
					if ((uint)num > 6u)
					{
						if (num == 7)
						{
							val = <>u__2;
							<>u__2 = default(TaskAwaiter);
							num = (<>1__state = -1);
							goto IL_0622;
						}
						cS$<>8__locals = new <>c__DisplayClass9_0
						{
							<>4__this = <>4__this,
							bleScanResult = bleScanResult
						};
						<>7__wrap1 = 0;
					}
					try
					{
						bool flag;
						ValueTaskAwaiter<string> val3;
						ValueTaskAwaiter<IDevice> val2;
						IDevice result;
						bool flag2;
						IBleScanResult val4;
						MyRvLinkBleGatewayScanResult val5;
						string result2;
						switch (num)
						{
						default:
						{
							<>8__1 = new <>c__DisplayClass9_1();
							<>8__1.CS$<>8__locals1 = cS$<>8__locals;
							connectToBluetoothGatewayViewModel.State = ConnectState.Connecting;
							<isLegacyPinGateway>5__4 = false;
							<pin>5__5 = string.Empty;
							ref BleGatewayScanResult reference = ref <>8__1.legacyBleGatewayScanResult;
							IBleScanResult obj = <>8__1.CS$<>8__locals1.bleScanResult;
							reference = (BleGatewayScanResult)(object)((obj is BleGatewayScanResult) ? obj : null);
							if (<>8__1.legacyBleGatewayScanResult != null)
							{
								PairingMethod pairingMethod = <>8__1.legacyBleGatewayScanResult.PairingMethod;
								if (pairingMethod - 1 <= 1)
								{
									flag = true;
									goto IL_00df;
								}
							}
							flag = false;
							goto IL_00df;
						}
						case 0:
							val3 = <>u__1;
							<>u__1 = default(ValueTaskAwaiter<string>);
							num = (<>1__state = -1);
							goto IL_0164;
						case 1:
							val = <>u__2;
							<>u__2 = default(TaskAwaiter);
							num = (<>1__state = -1);
							goto IL_01e2;
						case 2:
							val2 = <>u__3;
							<>u__3 = default(ValueTaskAwaiter<IDevice>);
							num = (<>1__state = -1);
							goto IL_026a;
						case 3:
							val = <>u__2;
							<>u__2 = default(TaskAwaiter);
							num = (<>1__state = -1);
							goto IL_02d9;
						case 4:
							val = <>u__2;
							<>u__2 = default(TaskAwaiter);
							num = (<>1__state = -1);
							goto IL_0461;
						case 5:
							val = <>u__2;
							<>u__2 = default(TaskAwaiter);
							num = (<>1__state = -1);
							goto IL_04ce;
						case 6:
							{
								val = <>u__2;
								<>u__2 = default(TaskAwaiter);
								num = (<>1__state = -1);
								break;
							}
							IL_01e2:
							((TaskAwaiter)(ref val)).GetResult();
							goto end_IL_003f;
							IL_026a:
							result = val2.GetResult();
							if ((result ?? null) == null)
							{
								val = connectToBluetoothGatewayViewModel.Failed("Failed to connect to gateway").GetAwaiter();
								if (!((TaskAwaiter)(ref val)).IsCompleted)
								{
									num = (<>1__state = 3);
									<>u__2 = val;
									((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <Connect>d__9>(ref val, ref this);
									return;
								}
								goto IL_02d9;
							}
							goto IL_02e5;
							IL_04ce:
							((TaskAwaiter)(ref val)).GetResult();
							connectToBluetoothGatewayViewModel.State = ConnectState.Finishing;
							flag2 = connectToBluetoothGatewayViewModel.IsBrakingSystem(<>8__1.CS$<>8__locals1.bleScanResult);
							val = connectToBluetoothGatewayViewModel.AddGatewayConnection(<connection>5__6, !flag2, flag2, cancellationToken).GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 6);
								<>u__2 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <Connect>d__9>(ref val, ref this);
								return;
							}
							break;
							IL_00df:
							if (flag)
							{
								<isLegacyPinGateway>5__4 = true;
								val3 = connectToBluetoothGatewayViewModel._bleDeviceConnectionPipeline.ExecuteAsync<string>((Func<CancellationToken, global::System.Threading.Tasks.ValueTask<string>>)([AsyncStateMachine(typeof(<>c__DisplayClass9_1.<<Connect>b__0>d))] (CancellationToken ct) =>
								{
									//IL_0002: Unknown result type (might be due to invalid IL or missing references)
									//IL_0007: Unknown result type (might be due to invalid IL or missing references)
									//IL_0016: Unknown result type (might be due to invalid IL or missing references)
									//IL_0017: Unknown result type (might be due to invalid IL or missing references)
									<>c__DisplayClass9_1.<<Connect>b__0>d <<Connect>b__0>d = default(<>c__DisplayClass9_1.<<Connect>b__0>d);
									<<Connect>b__0>d.<>t__builder = AsyncValueTaskMethodBuilder<string>.Create();
									<<Connect>b__0>d.<>4__this = <>8__1;
									<<Connect>b__0>d.ct = ct;
									<<Connect>b__0>d.<>1__state = -1;
									<<Connect>b__0>d.<>t__builder.Start<<>c__DisplayClass9_1.<<Connect>b__0>d>(ref <<Connect>b__0>d);
									return <<Connect>b__0>d.<>t__builder.Task;
								}), cancellationToken).GetAwaiter();
								if (!val3.IsCompleted)
								{
									num = (<>1__state = 0);
									<>u__1 = val3;
									((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<ValueTaskAwaiter<string>, <Connect>d__9>(ref val3, ref this);
									return;
								}
								goto IL_0164;
							}
							val2 = connectToBluetoothGatewayViewModel._bleDeviceConnectionPipeline.ExecuteAsync<IDevice>((Func<CancellationToken, global::System.Threading.Tasks.ValueTask<IDevice>>)([AsyncStateMachine(typeof(<>c__DisplayClass9_0.<<Connect>b__1>d))] (CancellationToken ct) =>
							{
								//IL_0002: Unknown result type (might be due to invalid IL or missing references)
								//IL_0007: Unknown result type (might be due to invalid IL or missing references)
								//IL_0016: Unknown result type (might be due to invalid IL or missing references)
								//IL_0017: Unknown result type (might be due to invalid IL or missing references)
								<>c__DisplayClass9_0.<<Connect>b__1>d <<Connect>b__1>d = default(<>c__DisplayClass9_0.<<Connect>b__1>d);
								<<Connect>b__1>d.<>t__builder = AsyncValueTaskMethodBuilder<IDevice>.Create();
								<<Connect>b__1>d.<>4__this = <>8__1.CS$<>8__locals1;
								<<Connect>b__1>d.ct = ct;
								<<Connect>b__1>d.<>1__state = -1;
								<<Connect>b__1>d.<>t__builder.Start<<>c__DisplayClass9_0.<<Connect>b__1>d>(ref <<Connect>b__1>d);
								return <<Connect>b__1>d.<>t__builder.Task;
							}), cancellationToken).GetAwaiter();
							if (!val2.IsCompleted)
							{
								num = (<>1__state = 2);
								<>u__3 = val2;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<ValueTaskAwaiter<IDevice>, <Connect>d__9>(ref val2, ref this);
								return;
							}
							goto IL_026a;
							IL_02d9:
							((TaskAwaiter)(ref val)).GetResult();
							goto end_IL_003f;
							IL_02e5:
							connectToBluetoothGatewayViewModel.State = ConnectState.Connected;
							val4 = <>8__1.CS$<>8__locals1.bleScanResult;
							val5 = (MyRvLinkBleGatewayScanResult)(object)((val4 is MyRvLinkBleGatewayScanResult) ? val4 : null);
							if (val5 == null)
							{
								IBleGatewayScanResult val6 = (IBleGatewayScanResult)(object)((val4 is IBleGatewayScanResult) ? val4 : null);
								if (val6 == null)
								{
									val = connectToBluetoothGatewayViewModel.Failed($"Unsupported gateway type: {<>8__1.CS$<>8__locals1.bleScanResult}").GetAwaiter();
									if (!((TaskAwaiter)(ref val)).IsCompleted)
									{
										num = (<>1__state = 4);
										<>u__2 = val;
										((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <Connect>d__9>(ref val, ref this);
										return;
									}
									goto IL_0461;
								}
								if (!<isLegacyPinGateway>5__4)
								{
									X180TGatewayScanResult val7 = (X180TGatewayScanResult)(object)((val4 is X180TGatewayScanResult) ? val4 : null);
									if (val7 != null)
									{
										<connection>5__6 = new RvGatewayCanConnectionBle(((BleScanResult)val7).DeviceName, string.Empty, ((BleScanResult)val7).DeviceId, val7.AdvertisedGatewayVersion);
									}
									else
									{
										IBleGatewayScanResult val8 = val6;
										<connection>5__6 = new RvGatewayCanConnectionBle(((IBleScanResult)val8).DeviceName, string.Empty, ((IBleScanResult)val8).DeviceId, val8.AdvertisedGatewayVersion);
									}
								}
								else
								{
									<connection>5__6 = new RvGatewayCanConnectionBle(((IBleScanResult)val6).DeviceName, <pin>5__5, ((IBleScanResult)val6).DeviceId, val6.AdvertisedGatewayVersion);
								}
							}
							else
							{
								<connection>5__6 = new RvGatewayMyRvLinkConnectionBle(((BleScanResult)val5).DeviceId, ((BleScanResult)val5).DeviceName);
							}
							val = global::System.Threading.Tasks.Task.Delay(2000, cancellationToken).GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 5);
								<>u__2 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <Connect>d__9>(ref val, ref this);
								return;
							}
							goto IL_04ce;
							IL_0164:
							result2 = val3.GetResult();
							<pin>5__5 = result2;
							if (string.IsNullOrWhiteSpace(<pin>5__5))
							{
								val = connectToBluetoothGatewayViewModel.Failed("Failed to connect to gateway").GetAwaiter();
								if (!((TaskAwaiter)(ref val)).IsCompleted)
								{
									num = (<>1__state = 1);
									<>u__2 = val;
									((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <Connect>d__9>(ref val, ref this);
									return;
								}
								goto IL_01e2;
							}
							goto IL_02e5;
							IL_0461:
							((TaskAwaiter)(ref val)).GetResult();
							goto end_IL_003f;
						}
						((TaskAwaiter)(ref val)).GetResult();
						<>8__1 = null;
						<pin>5__5 = null;
						<connection>5__6 = null;
						goto IL_05b6;
						end_IL_003f:;
					}
					catch (global::System.Exception ex) when (((Func<bool>)delegate
					{
						// Could not convert BlockContainer to single expression
						<e>5__3 = ex;
						return !((CancellationToken)(ref cancellationToken)).IsCancellationRequested;
					}).Invoke())
					{
						<>7__wrap1 = 1;
						goto IL_05b6;
					}
					goto end_IL_000e;
					IL_05b6:
					int num2 = <>7__wrap1;
					if (num2 == 1)
					{
						val = connectToBluetoothGatewayViewModel.Failed("Failed to connect to gateway", <e>5__3).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 7);
							<>u__2 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <Connect>d__9>(ref val, ref this);
							return;
						}
						goto IL_0622;
					}
					goto IL_0629;
					IL_0622:
					((TaskAwaiter)(ref val)).GetResult();
					goto IL_0629;
					IL_0629:
					<e>5__3 = null;
					end_IL_000e:;
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
		private struct <ConnectToLegacyGatewayAsync>d__10 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<string> <>t__builder;

			public ConnectToBluetoothGatewayViewModel <>4__this;

			public IBleGatewayScanResult bleGatewayScanResult;

			public CancellationToken cancellationToken;

			private DialogResult<string> <result>5__2;

			private AsyncOperation <asyncOperation>5__3;

			private BleCommunicationsAdapter <bleCommunicationsAdapter>5__4;

			private TaskAwaiter<DialogResult<string>> <>u__1;

			private TaskAwaiter<bool> <>u__2;

			private void MoveNext()
			{
				//IL_008c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0091: Unknown result type (might be due to invalid IL or missing references)
				//IL_0099: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
				//IL_004d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0057: Unknown result type (might be due to invalid IL or missing references)
				//IL_005c: Unknown result type (might be due to invalid IL or missing references)
				//IL_00db: Unknown result type (might be due to invalid IL or missing references)
				//IL_0071: Unknown result type (might be due to invalid IL or missing references)
				//IL_0073: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fa: Expected O, but got Unknown
				//IL_010c: Unknown result type (might be due to invalid IL or missing references)
				//IL_012d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0138: Unknown result type (might be due to invalid IL or missing references)
				//IL_0142: Expected O, but got Unknown
				//IL_013d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0147: Expected O, but got Unknown
				//IL_0192: Unknown result type (might be due to invalid IL or missing references)
				//IL_0197: Unknown result type (might be due to invalid IL or missing references)
				//IL_019f: Unknown result type (might be due to invalid IL or missing references)
				//IL_015d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0162: Unknown result type (might be due to invalid IL or missing references)
				//IL_0177: Unknown result type (might be due to invalid IL or missing references)
				//IL_0179: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				ConnectToBluetoothGatewayViewModel connectToBluetoothGatewayViewModel = <>4__this;
				string result2;
				try
				{
					TaskAwaiter<DialogResult<string>> val;
					if (num != 0)
					{
						if (num == 1)
						{
							goto IL_00fb;
						}
						CanAdapterFactory.InitCore();
						val = DialogServiceExtensions.EnterPasswordAsync(connectToBluetoothGatewayViewModel.<dialogService>P, Strings.ConnectToBluetoothGateway_LegacyPinPrompt_Title, string.Format(Strings.ConnectToBluetoothGateway_LegacyPinPrompt_Message, (object)((IBleScanResult)bleGatewayScanResult).DeviceName), Strings.ConnectToBluetoothGateway_LegacyPinPrompt_Placeholder, Strings.ConnectToBluetoothGateway_LegacyPinPrompt_Action_Confirm, Strings.ConnectToBluetoothGateway_LegacyPinPrompt_Action_Cancel, cancellationToken).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<DialogResult<string>>, <ConnectToLegacyGatewayAsync>d__10>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<DialogResult<string>>);
						num = (<>1__state = -1);
					}
					DialogResult<string> result = val.GetResult();
					<result>5__2 = result;
					if ((int)((DialogResult)<result>5__2).DismissalReason != 0 || string.IsNullOrWhiteSpace(<result>5__2.Result))
					{
						throw new OperationCanceledException("User cancelled PIN entry");
					}
					<asyncOperation>5__3 = new AsyncOperation(TimeSpan.FromSeconds(15L), cancellationToken);
					goto IL_00fb;
					IL_00fb:
					try
					{
						if (num != 1)
						{
							<bleCommunicationsAdapter>5__4 = new BleCommunicationsAdapter(connectToBluetoothGatewayViewModel.<bleManager>P, ((IBleScanResult)bleGatewayScanResult).DeviceId, ((IBleScanResult)bleGatewayScanResult).DeviceName, <result>5__2.Result, bleGatewayScanResult.AdvertisedGatewayVersion, new PhysicalAddress(new byte[6]));
						}
						try
						{
							TaskAwaiter<bool> val2;
							if (num != 1)
							{
								val2 = ((Adapter)<bleCommunicationsAdapter>5__4).OpenAsync(<asyncOperation>5__3).GetAwaiter();
								if (!val2.IsCompleted)
								{
									num = (<>1__state = 1);
									<>u__2 = val2;
									<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, <ConnectToLegacyGatewayAsync>d__10>(ref val2, ref this);
									return;
								}
							}
							else
							{
								val2 = <>u__2;
								<>u__2 = default(TaskAwaiter<bool>);
								num = (<>1__state = -1);
							}
							result2 = ((val2.GetResult() && !string.IsNullOrWhiteSpace(<result>5__2.Result)) ? <result>5__2.Result : null);
						}
						finally
						{
							if (num < 0 && <bleCommunicationsAdapter>5__4 != null)
							{
								((global::System.IDisposable)<bleCommunicationsAdapter>5__4).Dispose();
							}
						}
					}
					finally
					{
						if (num < 0 && <asyncOperation>5__3 != null)
						{
							((global::System.IDisposable)<asyncOperation>5__3).Dispose();
						}
					}
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<result>5__2 = null;
					<asyncOperation>5__3 = null;
					<bleCommunicationsAdapter>5__4 = null;
					<>t__builder.SetException(exception);
					return;
				}
				<>1__state = -2;
				<result>5__2 = null;
				<asyncOperation>5__3 = null;
				<bleCommunicationsAdapter>5__4 = null;
				<>t__builder.SetResult(result2);
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				<>t__builder.SetStateMachine(stateMachine);
			}
		}

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <OnResumeAsync>d__8 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public ConnectToBluetoothGatewayViewModel <>4__this;

			public ResumeReason reason;

			public INavigationParameters parameters;

			public CancellationToken cancellationToken;

			private TaskAwaiter <>u__1;

			private TaskAwaiter<PermissionsRequestState> <>u__2;

			private TaskAwaiter<INavigationResult> <>u__3;

			private void MoveNext()
			{
				//IL_0076: Unknown result type (might be due to invalid IL or missing references)
				//IL_007b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0082: Unknown result type (might be due to invalid IL or missing references)
				//IL_00de: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
				//IL_015c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0161: Unknown result type (might be due to invalid IL or missing references)
				//IL_0169: Unknown result type (might be due to invalid IL or missing references)
				//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				//IL_0048: Unknown result type (might be due to invalid IL or missing references)
				//IL_009f: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
				//IL_0101: Unknown result type (might be due to invalid IL or missing references)
				//IL_0102: Unknown result type (might be due to invalid IL or missing references)
				//IL_0114: Expected I4, but got Unknown
				//IL_005c: Unknown result type (might be due to invalid IL or missing references)
				//IL_005d: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
				//IL_0127: Unknown result type (might be due to invalid IL or missing references)
				//IL_012c: Unknown result type (might be due to invalid IL or missing references)
				//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
				//IL_0199: Unknown result type (might be due to invalid IL or missing references)
				//IL_019e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0141: Unknown result type (might be due to invalid IL or missing references)
				//IL_0143: Unknown result type (might be due to invalid IL or missing references)
				//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
				//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				ConnectToBluetoothGatewayViewModel connectToBluetoothGatewayViewModel = <>4__this;
				try
				{
					TaskAwaiter val;
					TaskAwaiter<PermissionsRequestState> val3;
					TaskAwaiter<INavigationResult> val2;
					PermissionsRequestState result;
					switch (num)
					{
					default:
						connectToBluetoothGatewayViewModel.State = ConnectState.Idle;
						val = ((ConnectViewModel)connectToBluetoothGatewayViewModel).OnResumeAsync(reason, parameters, cancellationToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnResumeAsync>d__8>(ref val, ref this);
							return;
						}
						goto IL_0091;
					case 0:
						val = <>u__1;
						<>u__1 = default(TaskAwaiter);
						num = (<>1__state = -1);
						goto IL_0091;
					case 1:
						val3 = <>u__2;
						<>u__2 = default(TaskAwaiter<PermissionsRequestState>);
						num = (<>1__state = -1);
						goto IL_00fa;
					case 2:
						val2 = <>u__3;
						<>u__3 = default(TaskAwaiter<INavigationResult>);
						num = (<>1__state = -1);
						goto IL_0178;
					case 3:
						{
							val = <>u__1;
							<>u__1 = default(TaskAwaiter);
							num = (<>1__state = -1);
							goto IL_01e4;
						}
						IL_0178:
						val2.GetResult();
						break;
						IL_0091:
						((TaskAwaiter)(ref val)).GetResult();
						val3 = connectToBluetoothGatewayViewModel.<bluetoothPermissionHandler>P.RequestPermissionsAsync(cancellationToken).GetAwaiter();
						if (!val3.IsCompleted)
						{
							num = (<>1__state = 1);
							<>u__2 = val3;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<PermissionsRequestState>, <OnResumeAsync>d__8>(ref val3, ref this);
							return;
						}
						goto IL_00fa;
						IL_01e4:
						((TaskAwaiter)(ref val)).GetResult();
						break;
						IL_00fa:
						result = val3.GetResult();
						switch ((int)result)
						{
						case 1:
							break;
						default:
							goto IL_011b;
						case 0:
							goto IL_0185;
						}
						break;
						IL_0185:
						if (connectToBluetoothGatewayViewModel._bleScanResult == null)
						{
							val = connectToBluetoothGatewayViewModel.Failed("No gateway scan result provided").GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 3);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnResumeAsync>d__8>(ref val, ref this);
								return;
							}
							goto IL_01e4;
						}
						connectToBluetoothGatewayViewModel.Connect(connectToBluetoothGatewayViewModel._bleScanResult, ViewModelExtensions.StoppedCancellationToken((IViewModel)(object)connectToBluetoothGatewayViewModel));
						break;
						IL_011b:
						val2 = ((PageViewModel)connectToBluetoothGatewayViewModel).NavigationService.GoBackAsync((INavigationParameters)null).GetAwaiter();
						if (!val2.IsCompleted)
						{
							num = (<>1__state = 2);
							<>u__3 = val2;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<INavigationResult>, <OnResumeAsync>d__8>(ref val2, ref this);
							return;
						}
						goto IL_0178;
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
		private struct <OnStartAsync>d__7 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public ConnectToBluetoothGatewayViewModel <>4__this;

			public INavigationParameters parameters;

			public CancellationToken cancellationToken;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				//IL_005b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0062: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a8: Invalid comparison between Unknown and I4
				int num = <>1__state;
				ConnectToBluetoothGatewayViewModel connectToBluetoothGatewayViewModel = <>4__this;
				try
				{
					TaskAwaiter val;
					if (num != 0)
					{
						val = connectToBluetoothGatewayViewModel.<>n__0(parameters, cancellationToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnStartAsync>d__7>(ref val, ref this);
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
					INavigationParameters obj = parameters;
					if (obj != null)
					{
						INavigationParametersExtensions.TryGetValueOf<IBleScanResult>(obj, ref connectToBluetoothGatewayViewModel._bleScanResult, (IBleScanResult)null);
					}
					IBleScanResult? bleScanResult = connectToBluetoothGatewayViewModel._bleScanResult;
					IPairableDeviceScanResult val2 = (IPairableDeviceScanResult)(object)((bleScanResult is IPairableDeviceScanResult) ? bleScanResult : null);
					connectToBluetoothGatewayViewModel.IsPushToPair = val2 != null && (int)val2.PairingMethod == 3;
					IBleScanResult? bleScanResult2 = connectToBluetoothGatewayViewModel._bleScanResult;
					connectToBluetoothGatewayViewModel.Name = ((bleScanResult2 != null) ? bleScanResult2.DeviceName : null);
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

		private readonly ResiliencePipeline _bleDeviceConnectionPipeline;

		private IBleScanResult? _bleScanResult;

		[ObservableProperty]
		private bool _isPushToPair;

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool IsPushToPair
		{
			get
			{
				return _isPushToPair;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(_isPushToPair, value))
				{
					((ObservableObject)this).OnPropertyChanging(__KnownINotifyPropertyChangingArgs.IsPushToPair);
					_isPushToPair = value;
					((ObservableObject)this).OnPropertyChanged(__KnownINotifyPropertyChangedArgs.IsPushToPair);
				}
			}
		}

		protected ConnectToBluetoothGatewayViewModel(IServiceProvider serviceProvider, IBluetoothPermissionHandler bluetoothPermissionHandler, IBleManager bleManager, IDialogService dialogService, ILogicalDeviceService logicalDeviceService)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Expected O, but got Unknown
			//IL_0034: Expected O, but got Unknown
			//IL_0034: Expected O, but got Unknown
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			<bluetoothPermissionHandler>P = bluetoothPermissionHandler;
			<bleManager>P = bleManager;
			<dialogService>P = dialogService;
			ResiliencePipelineBuilder val = new ResiliencePipelineBuilder();
			RetryStrategyOptions val2 = new RetryStrategyOptions();
			((RetryStrategyOptions<object>)val2).MaxRetryAttempts = 3;
			((RetryStrategyOptions<object>)val2).UseJitter = true;
			_bleDeviceConnectionPipeline = TimeoutResiliencePipelineBuilderExtensions.AddTimeout<ResiliencePipelineBuilder>(RetryResiliencePipelineBuilderExtensions.AddRetry(val, val2), TimeSpan.FromMinutes(1L)).Build();
			base..ctor(serviceProvider, logicalDeviceService);
		}

		[AsyncStateMachine(typeof(<OnStartAsync>d__7))]
		public override global::System.Threading.Tasks.Task OnStartAsync(INavigationParameters? parameters, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			<OnStartAsync>d__7 <OnStartAsync>d__ = default(<OnStartAsync>d__7);
			<OnStartAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<OnStartAsync>d__.<>4__this = this;
			<OnStartAsync>d__.parameters = parameters;
			<OnStartAsync>d__.cancellationToken = cancellationToken;
			<OnStartAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <OnStartAsync>d__.<>t__builder)).Start<<OnStartAsync>d__7>(ref <OnStartAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <OnStartAsync>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(<OnResumeAsync>d__8))]
		public override global::System.Threading.Tasks.Task OnResumeAsync(ResumeReason reason, INavigationParameters parameters, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			<OnResumeAsync>d__8 <OnResumeAsync>d__ = default(<OnResumeAsync>d__8);
			<OnResumeAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<OnResumeAsync>d__.<>4__this = this;
			<OnResumeAsync>d__.reason = reason;
			<OnResumeAsync>d__.parameters = parameters;
			<OnResumeAsync>d__.cancellationToken = cancellationToken;
			<OnResumeAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <OnResumeAsync>d__.<>t__builder)).Start<<OnResumeAsync>d__8>(ref <OnResumeAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <OnResumeAsync>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(<Connect>d__9))]
		private global::System.Threading.Tasks.Task Connect(IBleScanResult bleScanResult, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			<Connect>d__9 <Connect>d__ = default(<Connect>d__9);
			<Connect>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<Connect>d__.<>4__this = this;
			<Connect>d__.bleScanResult = bleScanResult;
			<Connect>d__.cancellationToken = cancellationToken;
			<Connect>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <Connect>d__.<>t__builder)).Start<<Connect>d__9>(ref <Connect>d__);
			return ((AsyncTaskMethodBuilder)(ref <Connect>d__.<>t__builder)).Task;
		}

		[AsyncStateMachine(typeof(<ConnectToLegacyGatewayAsync>d__10))]
		private async global::System.Threading.Tasks.Task<string?> ConnectToLegacyGatewayAsync(IBleGatewayScanResult bleGatewayScanResult, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			CanAdapterFactory.InitCore();
			DialogResult<string> result = await DialogServiceExtensions.EnterPasswordAsync(<dialogService>P, Strings.ConnectToBluetoothGateway_LegacyPinPrompt_Title, string.Format(Strings.ConnectToBluetoothGateway_LegacyPinPrompt_Message, (object)((IBleScanResult)bleGatewayScanResult).DeviceName), Strings.ConnectToBluetoothGateway_LegacyPinPrompt_Placeholder, Strings.ConnectToBluetoothGateway_LegacyPinPrompt_Action_Confirm, Strings.ConnectToBluetoothGateway_LegacyPinPrompt_Action_Cancel, cancellationToken);
			if ((int)((DialogResult)result).DismissalReason != 0 || string.IsNullOrWhiteSpace(result.Result))
			{
				throw new OperationCanceledException("User cancelled PIN entry");
			}
			AsyncOperation asyncOperation = new AsyncOperation(TimeSpan.FromSeconds(15L), cancellationToken);
			try
			{
				BleCommunicationsAdapter bleCommunicationsAdapter = new BleCommunicationsAdapter(<bleManager>P, ((IBleScanResult)bleGatewayScanResult).DeviceId, ((IBleScanResult)bleGatewayScanResult).DeviceName, result.Result, bleGatewayScanResult.AdvertisedGatewayVersion, new PhysicalAddress(new byte[6]));
				try
				{
					return (await ((Adapter)bleCommunicationsAdapter).OpenAsync(asyncOperation) && !string.IsNullOrWhiteSpace(result.Result)) ? result.Result : null;
				}
				finally
				{
					((global::System.IDisposable)bleCommunicationsAdapter)?.Dispose();
				}
			}
			finally
			{
				((global::System.IDisposable)asyncOperation)?.Dispose();
			}
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private global::System.Threading.Tasks.Task <>n__0(INavigationParameters? parameters, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			return ((PageViewModel)this).OnStartAsync(parameters, cancellationToken);
		}
	}
}
namespace App.Common.Pages.Pairing.ConnectToAccessory
{
	public abstract class ConnectToAccessoryPage : ConnectPage
	{
	}
	public abstract class ConnectToAccessoryViewModel<TAccessoryDevice> : SearchForDevicesViewModel where TAccessoryDevice : ILogicalDevice
	{
		[CompilerGenerated]
		private sealed class <>c__DisplayClass19_0
		{
			[StructLayout((LayoutKind)3)]
			private struct <<OnResumeAsync>b__0>d : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncTaskMethodBuilder <>t__builder;

				public <>c__DisplayClass19_0 <>4__this;

				private TaskAwaiter <>u__1;

				private void MoveNext()
				{
					//IL_005b: Unknown result type (might be due to invalid IL or missing references)
					//IL_0060: Unknown result type (might be due to invalid IL or missing references)
					//IL_0067: Unknown result type (might be due to invalid IL or missing references)
					//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
					//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
					//IL_0100: Unknown result type (might be due to invalid IL or missing references)
					//IL_001e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0028: Unknown result type (might be due to invalid IL or missing references)
					//IL_002d: Unknown result type (might be due to invalid IL or missing references)
					//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
					//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
					//IL_0041: Unknown result type (might be due to invalid IL or missing references)
					//IL_0042: Unknown result type (might be due to invalid IL or missing references)
					//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
					//IL_00de: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					<>c__DisplayClass19_0 CS$<>8__locals5 = <>4__this;
					try
					{
						TaskAwaiter val;
						if (num != 0)
						{
							if (num == 1)
							{
								val = <>u__1;
								<>u__1 = default(TaskAwaiter);
								num = (<>1__state = -1);
								goto IL_010f;
							}
							val = global::System.Threading.Tasks.Task.Delay(60000, CS$<>8__locals5.searchCt).GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <<OnResumeAsync>b__0>d>(ref val, ref this);
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
						CS$<>8__locals5.<>4__this._errorCount = CS$<>8__locals5.<>4__this._errorCount + 1;
						val = ((ViewModel)CS$<>8__locals5.<>4__this).MainThreadService.InvokeOnMainThreadAsync((Func<global::System.Threading.Tasks.Task>)([AsyncStateMachine(typeof(ConnectToAccessoryViewModel<>.<>c__DisplayClass19_0.<<OnResumeAsync>b__1>d))] () =>
						{
							//IL_0002: Unknown result type (might be due to invalid IL or missing references)
							//IL_0007: Unknown result type (might be due to invalid IL or missing references)
							<<OnResumeAsync>b__1>d <<OnResumeAsync>b__1>d = default(<<OnResumeAsync>b__1>d);
							<<OnResumeAsync>b__1>d.<>t__builder = AsyncTaskMethodBuilder.Create();
							<<OnResumeAsync>b__1>d.<>4__this = CS$<>8__locals5;
							<<OnResumeAsync>b__1>d.<>1__state = -1;
							((AsyncTaskMethodBuilder)(ref <<OnResumeAsync>b__1>d.<>t__builder)).Start<<<OnResumeAsync>b__1>d>(ref <<OnResumeAsync>b__1>d);
							return ((AsyncTaskMethodBuilder)(ref <<OnResumeAsync>b__1>d.<>t__builder)).Task;
						})).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 1);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <<OnResumeAsync>b__0>d>(ref val, ref this);
							return;
						}
						goto IL_010f;
						IL_010f:
						((TaskAwaiter)(ref val)).GetResult();
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
			private struct <<OnResumeAsync>b__1>d : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncTaskMethodBuilder <>t__builder;

				public <>c__DisplayClass19_0 <>4__this;

				private TaskAwaiter <>u__1;

				private void MoveNext()
				{
					//IL_004c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0051: Unknown result type (might be due to invalid IL or missing references)
					//IL_0058: Unknown result type (might be due to invalid IL or missing references)
					//IL_001c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0021: Unknown result type (might be due to invalid IL or missing references)
					//IL_0035: Unknown result type (might be due to invalid IL or missing references)
					//IL_0036: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					<>c__DisplayClass19_0 <>c__DisplayClass19_ = <>4__this;
					try
					{
						TaskAwaiter val;
						if (num != 0)
						{
							val = <>c__DisplayClass19_.<>4__this.ErrorNoAccessoryFoundAsync().GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <<OnResumeAsync>b__1>d>(ref val, ref this);
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

			public CancellationToken searchCt;

			public ConnectToAccessoryViewModel<TAccessoryDevice> <>4__this;

			public Func<global::System.Threading.Tasks.Task> <>9__1;

			[AsyncStateMachine(typeof(ConnectToAccessoryViewModel<>.<>c__DisplayClass19_0.<<OnResumeAsync>b__0>d))]
			internal global::System.Threading.Tasks.Task? <OnResumeAsync>b__0()
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				<<OnResumeAsync>b__0>d <<OnResumeAsync>b__0>d = default(<<OnResumeAsync>b__0>d);
				<<OnResumeAsync>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<OnResumeAsync>b__0>d.<>4__this = this;
				<<OnResumeAsync>b__0>d.<>1__state = -1;
				((AsyncTaskMethodBuilder)(ref <<OnResumeAsync>b__0>d.<>t__builder)).Start<<<OnResumeAsync>b__0>d>(ref <<OnResumeAsync>b__0>d);
				return ((AsyncTaskMethodBuilder)(ref <<OnResumeAsync>b__0>d.<>t__builder)).Task;
			}

			[AsyncStateMachine(typeof(ConnectToAccessoryViewModel<>.<>c__DisplayClass19_0.<<OnResumeAsync>b__1>d))]
			internal global::System.Threading.Tasks.Task <OnResumeAsync>b__1()
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				<<OnResumeAsync>b__1>d <<OnResumeAsync>b__1>d = default(<<OnResumeAsync>b__1>d);
				<<OnResumeAsync>b__1>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<OnResumeAsync>b__1>d.<>4__this = this;
				<<OnResumeAsync>b__1>d.<>1__state = -1;
				((AsyncTaskMethodBuilder)(ref <<OnResumeAsync>b__1>d.<>t__builder)).Start<<<OnResumeAsync>b__1>d>(ref <<OnResumeAsync>b__1>d);
				return ((AsyncTaskMethodBuilder)(ref <<OnResumeAsync>b__1>d.<>t__builder)).Task;
			}
		}

		[CompilerGenerated]
		private sealed class <>c__DisplayClass22_0
		{
			[StructLayout((LayoutKind)3)]
			private struct <<ConnectWithDeviceAsync>b__0>d : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncTaskMethodBuilder <>t__builder;

				public <>c__DisplayClass22_0 <>4__this;

				private int <>7__wrap1;

				private TaskAwaiter<TAccessoryDevice?> <>u__1;

				private TaskAwaiter <>u__2;

				private void MoveNext()
				{
					//IL_0226: Unknown result type (might be due to invalid IL or missing references)
					//IL_022b: Unknown result type (might be due to invalid IL or missing references)
					//IL_0258: Unknown result type (might be due to invalid IL or missing references)
					//IL_025d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0265: Unknown result type (might be due to invalid IL or missing references)
					//IL_0240: Unknown result type (might be due to invalid IL or missing references)
					//IL_0242: Unknown result type (might be due to invalid IL or missing references)
					//IL_008d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0092: Unknown result type (might be due to invalid IL or missing references)
					//IL_0099: Unknown result type (might be due to invalid IL or missing references)
					//IL_0113: Unknown result type (might be due to invalid IL or missing references)
					//IL_0118: Unknown result type (might be due to invalid IL or missing references)
					//IL_0120: Unknown result type (might be due to invalid IL or missing references)
					//IL_0182: Unknown result type (might be due to invalid IL or missing references)
					//IL_0187: Unknown result type (might be due to invalid IL or missing references)
					//IL_018f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0050: Unknown result type (might be due to invalid IL or missing references)
					//IL_005a: Unknown result type (might be due to invalid IL or missing references)
					//IL_005f: Unknown result type (might be due to invalid IL or missing references)
					//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
					//IL_01b6: Invalid comparison between Unknown and I4
					//IL_0073: Unknown result type (might be due to invalid IL or missing references)
					//IL_0074: Unknown result type (might be due to invalid IL or missing references)
					//IL_014d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0152: Unknown result type (might be due to invalid IL or missing references)
					//IL_00de: Unknown result type (might be due to invalid IL or missing references)
					//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
					//IL_0167: Unknown result type (might be due to invalid IL or missing references)
					//IL_0169: Unknown result type (might be due to invalid IL or missing references)
					//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
					//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					<>c__DisplayClass22_0 CS$<>8__locals15 = <>4__this;
					try
					{
						TaskAwaiter val;
						if ((uint)num > 2u)
						{
							if (num == 3)
							{
								val = <>u__2;
								<>u__2 = default(TaskAwaiter);
								num = (<>1__state = -1);
								goto IL_0274;
							}
							<>7__wrap1 = 0;
						}
						try
						{
							TaskAwaiter<TAccessoryDevice> val2;
							TAccessoryDevice result;
							switch (num)
							{
							default:
								val2 = CS$<>8__locals15.<>4__this.ConnectToAccessoryDeviceAsync(CS$<>8__locals15.bleScanResultModel, CS$<>8__locals15.<>4__this._accessoryRegistrationManager, ViewModelExtensions.PausedCancellationToken((IViewModel)(object)CS$<>8__locals15.<>4__this)).GetAwaiter();
								if (!val2.IsCompleted)
								{
									num = (<>1__state = 0);
									<>u__1 = val2;
									((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter<TAccessoryDevice>, <<ConnectWithDeviceAsync>b__0>d>(ref val2, ref this);
									return;
								}
								goto IL_00a8;
							case 0:
								val2 = <>u__1;
								<>u__1 = default(TaskAwaiter<TAccessoryDevice>);
								num = (<>1__state = -1);
								goto IL_00a8;
							case 1:
								val = <>u__2;
								<>u__2 = default(TaskAwaiter);
								num = (<>1__state = -1);
								goto IL_012f;
							case 2:
								{
									val = <>u__2;
									<>u__2 = default(TaskAwaiter);
									num = (<>1__state = -1);
									break;
								}
								IL_012f:
								((TaskAwaiter)(ref val)).GetResult();
								goto end_IL_0021;
								IL_00a8:
								result = val2.GetResult();
								if (result == null)
								{
									CS$<>8__locals15.<>4__this._errorCount = CS$<>8__locals15.<>4__this._errorCount + 1;
									val = CS$<>8__locals15.<>4__this.ErrorConnectingToAccessoryAsync().GetAwaiter();
									if (!((TaskAwaiter)(ref val)).IsCompleted)
									{
										num = (<>1__state = 1);
										<>u__2 = val;
										((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <<ConnectWithDeviceAsync>b__0>d>(ref val, ref this);
										return;
									}
									goto IL_012f;
								}
								val = CS$<>8__locals15.<>4__this.ConnectedToAccessoryAsync(CS$<>8__locals15.bleScanResultModel, result).GetAwaiter();
								if (!((TaskAwaiter)(ref val)).IsCompleted)
								{
									num = (<>1__state = 2);
									<>u__2 = val;
									((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <<ConnectWithDeviceAsync>b__0>d>(ref val, ref this);
									return;
								}
								break;
							}
							((TaskAwaiter)(ref val)).GetResult();
							if ((int)CS$<>8__locals15.<>4__this._notificationRegistrationService.NotificationCapability == 1)
							{
								((ViewModel)CS$<>8__locals15.<>4__this).MainThreadService.InvokeOnMainThreadDeferredAsync<INavigationResult>((Func<global::System.Threading.Tasks.Task<INavigationResult>>)([AsyncStateMachine(typeof(ConnectToAccessoryViewModel<>.<>c__DisplayClass22_0.<<ConnectWithDeviceAsync>b__1>d))] async () => await CS$<>8__locals15.<>4__this._notificationRegistrationService.RequestNotificationPermissionAsync()));
							}
							end_IL_0021:;
						}
						catch
						{
							<>7__wrap1 = 1;
						}
						int num2 = <>7__wrap1;
						if (num2 == 1)
						{
							CS$<>8__locals15.<>4__this._errorCount = CS$<>8__locals15.<>4__this._errorCount + 1;
							val = CS$<>8__locals15.<>4__this.ErrorConnectingToAccessoryAsync().GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 3);
								<>u__2 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <<ConnectWithDeviceAsync>b__0>d>(ref val, ref this);
								return;
							}
							goto IL_0274;
						}
						goto end_IL_000e;
						IL_0274:
						((TaskAwaiter)(ref val)).GetResult();
						end_IL_000e:;
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
			private struct <<ConnectWithDeviceAsync>b__1>d : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncTaskMethodBuilder<INavigationResult> <>t__builder;

				public <>c__DisplayClass22_0 <>4__this;

				private TaskAwaiter<INavigationResult> <>u__1;

				private void MoveNext()
				{
					//IL_0051: Unknown result type (might be due to invalid IL or missing references)
					//IL_0056: Unknown result type (might be due to invalid IL or missing references)
					//IL_005d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0021: Unknown result type (might be due to invalid IL or missing references)
					//IL_0026: Unknown result type (might be due to invalid IL or missing references)
					//IL_003a: Unknown result type (might be due to invalid IL or missing references)
					//IL_003b: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					<>c__DisplayClass22_0 <>c__DisplayClass22_ = <>4__this;
					INavigationResult result;
					try
					{
						TaskAwaiter<INavigationResult> val;
						if (num != 0)
						{
							val = <>c__DisplayClass22_.<>4__this._notificationRegistrationService.RequestNotificationPermissionAsync().GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<INavigationResult>, <<ConnectWithDeviceAsync>b__1>d>(ref val, ref this);
								return;
							}
						}
						else
						{
							val = <>u__1;
							<>u__1 = default(TaskAwaiter<INavigationResult>);
							num = (<>1__state = -1);
						}
						result = val.GetResult();
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

			public ConnectToAccessoryViewModel<TAccessoryDevice> <>4__this;

			public BleScanResultModel bleScanResultModel;

			public Func<global::System.Threading.Tasks.Task<INavigationResult>> <>9__1;

			[AsyncStateMachine(typeof(ConnectToAccessoryViewModel<>.<>c__DisplayClass22_0.<<ConnectWithDeviceAsync>b__0>d))]
			internal global::System.Threading.Tasks.Task <ConnectWithDeviceAsync>b__0()
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				<<ConnectWithDeviceAsync>b__0>d <<ConnectWithDeviceAsync>b__0>d = default(<<ConnectWithDeviceAsync>b__0>d);
				<<ConnectWithDeviceAsync>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<ConnectWithDeviceAsync>b__0>d.<>4__this = this;
				<<ConnectWithDeviceAsync>b__0>d.<>1__state = -1;
				((AsyncTaskMethodBuilder)(ref <<ConnectWithDeviceAsync>b__0>d.<>t__builder)).Start<<<ConnectWithDeviceAsync>b__0>d>(ref <<ConnectWithDeviceAsync>b__0>d);
				return ((AsyncTaskMethodBuilder)(ref <<ConnectWithDeviceAsync>b__0>d.<>t__builder)).Task;
			}

			[AsyncStateMachine(typeof(ConnectToAccessoryViewModel<>.<>c__DisplayClass22_0.<<ConnectWithDeviceAsync>b__1>d))]
			internal async global::System.Threading.Tasks.Task<INavigationResult> <ConnectWithDeviceAsync>b__1()
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				return await <>4__this._notificationRegistrationService.RequestNotificationPermissionAsync();
			}
		}

		[CompilerGenerated]
		private sealed class <>c__DisplayClass23_0
		{
			[StructLayout((LayoutKind)3)]
			private struct <<DeviceAdded>b__0>d : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncTaskMethodBuilder <>t__builder;

				public <>c__DisplayClass23_0 <>4__this;

				private TaskAwaiter <>u__1;

				private void MoveNext()
				{
					//IL_006f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0074: Unknown result type (might be due to invalid IL or missing references)
					//IL_007b: Unknown result type (might be due to invalid IL or missing references)
					//IL_0108: Unknown result type (might be due to invalid IL or missing references)
					//IL_010d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0114: Unknown result type (might be due to invalid IL or missing references)
					//IL_0032: Unknown result type (might be due to invalid IL or missing references)
					//IL_003c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0041: Unknown result type (might be due to invalid IL or missing references)
					//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
					//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
					//IL_0055: Unknown result type (might be due to invalid IL or missing references)
					//IL_0056: Unknown result type (might be due to invalid IL or missing references)
					//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
					//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					<>c__DisplayClass23_0 CS$<>8__locals5 = <>4__this;
					try
					{
						TaskAwaiter val;
						if (num != 0)
						{
							if (num == 1)
							{
								val = <>u__1;
								<>u__1 = default(TaskAwaiter);
								num = (<>1__state = -1);
								goto IL_0123;
							}
							val = global::System.Threading.Tasks.Task.Delay(ConnectToAccessoryViewModel<TAccessoryDevice>.Random.Next(10000, 40000), ViewModelExtensions.PausedCancellationToken((IViewModel)(object)CS$<>8__locals5.<>4__this)).GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <<DeviceAdded>b__0>d>(ref val, ref this);
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
						CS$<>8__locals5.<>4__this._errorCount = CS$<>8__locals5.<>4__this._errorCount + 1;
						val = ((ViewModel)CS$<>8__locals5.<>4__this).MainThreadService.InvokeOnMainThreadAsync((Func<global::System.Threading.Tasks.Task>)([AsyncStateMachine(typeof(ConnectToAccessoryViewModel<>.<>c__DisplayClass23_0.<<DeviceAdded>b__2>d))] () =>
						{
							//IL_0002: Unknown result type (might be due to invalid IL or missing references)
							//IL_0007: Unknown result type (might be due to invalid IL or missing references)
							<<DeviceAdded>b__2>d <<DeviceAdded>b__2>d = default(<<DeviceAdded>b__2>d);
							<<DeviceAdded>b__2>d.<>t__builder = AsyncTaskMethodBuilder.Create();
							<<DeviceAdded>b__2>d.<>4__this = CS$<>8__locals5;
							<<DeviceAdded>b__2>d.<>1__state = -1;
							((AsyncTaskMethodBuilder)(ref <<DeviceAdded>b__2>d.<>t__builder)).Start<<<DeviceAdded>b__2>d>(ref <<DeviceAdded>b__2>d);
							return ((AsyncTaskMethodBuilder)(ref <<DeviceAdded>b__2>d.<>t__builder)).Task;
						})).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 1);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <<DeviceAdded>b__0>d>(ref val, ref this);
							return;
						}
						goto IL_0123;
						IL_0123:
						((TaskAwaiter)(ref val)).GetResult();
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
			private struct <<DeviceAdded>b__1>d : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncTaskMethodBuilder <>t__builder;

				public <>c__DisplayClass23_0 <>4__this;

				private TaskAwaiter <>u__1;

				private void MoveNext()
				{
					//IL_0060: Unknown result type (might be due to invalid IL or missing references)
					//IL_0065: Unknown result type (might be due to invalid IL or missing references)
					//IL_006c: Unknown result type (might be due to invalid IL or missing references)
					//IL_009f: Unknown result type (might be due to invalid IL or missing references)
					//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
					//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
					//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
					//IL_00db: Unknown result type (might be due to invalid IL or missing references)
					//IL_0023: Unknown result type (might be due to invalid IL or missing references)
					//IL_002d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0032: Unknown result type (might be due to invalid IL or missing references)
					//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
					//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
					//IL_0046: Unknown result type (might be due to invalid IL or missing references)
					//IL_0047: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					<>c__DisplayClass23_0 <>c__DisplayClass23_ = <>4__this;
					try
					{
						TaskAwaiter val;
						if (num != 0)
						{
							if (num == 1)
							{
								val = <>u__1;
								<>u__1 = default(TaskAwaiter);
								num = (<>1__state = -1);
								goto IL_00ea;
							}
							val = global::System.Threading.Tasks.Task.Delay(3000, ViewModelExtensions.PausedCancellationToken((IViewModel)(object)<>c__DisplayClass23_.<>4__this)).GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <<DeviceAdded>b__1>d>(ref val, ref this);
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
						<>c__DisplayClass23_.<>4__this._errorCount = 0;
						val = <>c__DisplayClass23_.<>4__this.ConnectWithDeviceAsync(<>c__DisplayClass23_.device).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 1);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <<DeviceAdded>b__1>d>(ref val, ref this);
							return;
						}
						goto IL_00ea;
						IL_00ea:
						((TaskAwaiter)(ref val)).GetResult();
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
			private struct <<DeviceAdded>b__2>d : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncTaskMethodBuilder <>t__builder;

				public <>c__DisplayClass23_0 <>4__this;

				private TaskAwaiter <>u__1;

				private void MoveNext()
				{
					//IL_004c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0051: Unknown result type (might be due to invalid IL or missing references)
					//IL_0058: Unknown result type (might be due to invalid IL or missing references)
					//IL_001c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0021: Unknown result type (might be due to invalid IL or missing references)
					//IL_0035: Unknown result type (might be due to invalid IL or missing references)
					//IL_0036: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					<>c__DisplayClass23_0 <>c__DisplayClass23_ = <>4__this;
					try
					{
						TaskAwaiter val;
						if (num != 0)
						{
							val = <>c__DisplayClass23_.<>4__this.ErrorMultipleAccessoriesFoundAsync().GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <<DeviceAdded>b__2>d>(ref val, ref this);
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

			public ConnectToAccessoryViewModel<TAccessoryDevice> <>4__this;

			public BleScanResultModel device;

			public Func<global::System.Threading.Tasks.Task> <>9__2;

			[AsyncStateMachine(typeof(ConnectToAccessoryViewModel<>.<>c__DisplayClass23_0.<<DeviceAdded>b__0>d))]
			internal global::System.Threading.Tasks.Task? <DeviceAdded>b__0()
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				<<DeviceAdded>b__0>d <<DeviceAdded>b__0>d = default(<<DeviceAdded>b__0>d);
				<<DeviceAdded>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<DeviceAdded>b__0>d.<>4__this = this;
				<<DeviceAdded>b__0>d.<>1__state = -1;
				((AsyncTaskMethodBuilder)(ref <<DeviceAdded>b__0>d.<>t__builder)).Start<<<DeviceAdded>b__0>d>(ref <<DeviceAdded>b__0>d);
				return ((AsyncTaskMethodBuilder)(ref <<DeviceAdded>b__0>d.<>t__builder)).Task;
			}

			[AsyncStateMachine(typeof(ConnectToAccessoryViewModel<>.<>c__DisplayClass23_0.<<DeviceAdded>b__2>d))]
			internal global::System.Threading.Tasks.Task <DeviceAdded>b__2()
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				<<DeviceAdded>b__2>d <<DeviceAdded>b__2>d = default(<<DeviceAdded>b__2>d);
				<<DeviceAdded>b__2>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<DeviceAdded>b__2>d.<>4__this = this;
				<<DeviceAdded>b__2>d.<>1__state = -1;
				((AsyncTaskMethodBuilder)(ref <<DeviceAdded>b__2>d.<>t__builder)).Start<<<DeviceAdded>b__2>d>(ref <<DeviceAdded>b__2>d);
				return ((AsyncTaskMethodBuilder)(ref <<DeviceAdded>b__2>d.<>t__builder)).Task;
			}

			[AsyncStateMachine(typeof(ConnectToAccessoryViewModel<>.<>c__DisplayClass23_0.<<DeviceAdded>b__1>d))]
			internal global::System.Threading.Tasks.Task? <DeviceAdded>b__1()
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				<<DeviceAdded>b__1>d <<DeviceAdded>b__1>d = default(<<DeviceAdded>b__1>d);
				<<DeviceAdded>b__1>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<DeviceAdded>b__1>d.<>4__this = this;
				<<DeviceAdded>b__1>d.<>1__state = -1;
				((AsyncTaskMethodBuilder)(ref <<DeviceAdded>b__1>d.<>t__builder)).Start<<<DeviceAdded>b__1>d>(ref <<DeviceAdded>b__1>d);
				return ((AsyncTaskMethodBuilder)(ref <<DeviceAdded>b__1>d.<>t__builder)).Task;
			}
		}

		[CompilerGenerated]
		private sealed class <>c__DisplayClass25_0
		{
			public MAC deviceMac;

			public Func<ILogicalDevice, bool> <>9__0;

			internal bool <WaitAccessoryDeviceOnlineAsync>b__0(ILogicalDevice ld)
			{
				if (ld is TAccessoryDevice)
				{
					return (PhysicalAddress)(object)ld.LogicalId.ProductMacAddress == (PhysicalAddress)(object)deviceMac;
				}
				return false;
			}
		}

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <ConnectWithDeviceAsync>d__22 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public ConnectToAccessoryViewModel<TAccessoryDevice> <>4__this;

			public BleScanResultModel bleScanResultModel;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0076: Unknown result type (might be due to invalid IL or missing references)
				//IL_007b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0082: Unknown result type (might be due to invalid IL or missing references)
				//IL_0046: Unknown result type (might be due to invalid IL or missing references)
				//IL_004b: Unknown result type (might be due to invalid IL or missing references)
				//IL_005f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0060: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				ConnectToAccessoryViewModel<TAccessoryDevice> connectToAccessoryViewModel = <>4__this;
				try
				{
					TaskAwaiter val;
					if (num != 0)
					{
						<>c__DisplayClass22_0 CS$<>8__locals1 = new <>c__DisplayClass22_0
						{
							<>4__this = <>4__this,
							bleScanResultModel = bleScanResultModel
						};
						val = ((ViewModel)connectToAccessoryViewModel).MainThreadService.InvokeOnMainThreadAsync((Func<global::System.Threading.Tasks.Task>)([AsyncStateMachine(typeof(ConnectToAccessoryViewModel<>.<>c__DisplayClass22_0.<<ConnectWithDeviceAsync>b__0>d))] () =>
						{
							//IL_0002: Unknown result type (might be due to invalid IL or missing references)
							//IL_0007: Unknown result type (might be due to invalid IL or missing references)
							<>c__DisplayClass22_0.<<ConnectWithDeviceAsync>b__0>d <<ConnectWithDeviceAsync>b__0>d = default(<>c__DisplayClass22_0.<<ConnectWithDeviceAsync>b__0>d);
							<<ConnectWithDeviceAsync>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
							<<ConnectWithDeviceAsync>b__0>d.<>4__this = CS$<>8__locals1;
							<<ConnectWithDeviceAsync>b__0>d.<>1__state = -1;
							((AsyncTaskMethodBuilder)(ref <<ConnectWithDeviceAsync>b__0>d.<>t__builder)).Start<<>c__DisplayClass22_0.<<ConnectWithDeviceAsync>b__0>d>(ref <<ConnectWithDeviceAsync>b__0>d);
							return ((AsyncTaskMethodBuilder)(ref <<ConnectWithDeviceAsync>b__0>d.<>t__builder)).Task;
						})).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <ConnectWithDeviceAsync>d__22>(ref val, ref this);
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
		private struct <LinkToAccessoryGatewayAsync>d__24 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<bool> <>t__builder;

			public ConnectToAccessoryViewModel<TAccessoryDevice> <>4__this;

			public MAC accessoryMacAddress;

			public CancellationToken cancellationToken;

			private ILogicalDeviceAccessoryGateway <accessoryGateway>5__2;

			private TaskAwaiter<bool> <>u__1;

			private TaskAwaiter<CommandResult> <>u__2;

			private void MoveNext()
			{
				//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00de: Unknown result type (might be due to invalid IL or missing references)
				//IL_0149: Unknown result type (might be due to invalid IL or missing references)
				//IL_014e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0156: Unknown result type (might be due to invalid IL or missing references)
				//IL_010a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0114: Unknown result type (might be due to invalid IL or missing references)
				//IL_0119: Unknown result type (might be due to invalid IL or missing references)
				//IL_0167: Unknown result type (might be due to invalid IL or missing references)
				//IL_016c: Unknown result type (might be due to invalid IL or missing references)
				//IL_016d: Unknown result type (might be due to invalid IL or missing references)
				//IL_012e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0130: Unknown result type (might be due to invalid IL or missing references)
				//IL_018a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0092: Unknown result type (might be due to invalid IL or missing references)
				//IL_009c: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				ConnectToAccessoryViewModel<TAccessoryDevice> connectToAccessoryViewModel = <>4__this;
				bool result;
				try
				{
					_ = 1;
					try
					{
						TaskAwaiter<bool> val;
						if (num == 0)
						{
							val = <>u__1;
							<>u__1 = default(TaskAwaiter<bool>);
							num = (<>1__state = -1);
							goto IL_00ed;
						}
						TaskAwaiter<CommandResult> val2;
						if (num == 1)
						{
							val2 = <>u__2;
							<>u__2 = default(TaskAwaiter<CommandResult>);
							num = (<>1__state = -1);
							goto IL_0165;
						}
						ILogicalDeviceManager deviceManager = connectToAccessoryViewModel._logicalDeviceService.DeviceManager;
						<accessoryGateway>5__2 = ((deviceManager != null) ? Enumerable.FirstOrDefault<ILogicalDeviceAccessoryGateway>((global::System.Collections.Generic.IEnumerable<ILogicalDeviceAccessoryGateway>)deviceManager.FindLogicalDevices<ILogicalDeviceAccessoryGateway>((Func<ILogicalDeviceAccessoryGateway, bool>)((ILogicalDeviceAccessoryGateway logicalDevice) => (int)((IDevicesCommon)logicalDevice).ActiveConnection != 0))) : null);
						if (<accessoryGateway>5__2 != null)
						{
							val = <accessoryGateway>5__2.IsDeviceLinkedAsync(accessoryMacAddress, cancellationToken).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, <LinkToAccessoryGatewayAsync>d__24>(ref val, ref this);
								return;
							}
							goto IL_00ed;
						}
						LoggerExtensions.LogInformation(((PageViewModel)connectToAccessoryViewModel).Logger, "No accessory gateway found to link to", global::System.Array.Empty<object>());
						result = false;
						goto end_IL_0013;
						IL_0165:
						CommandResult result2 = val2.GetResult();
						if ((int)result2 == 0)
						{
							result = true;
						}
						else
						{
							LoggerExtensions.LogDebug(((PageViewModel)connectToAccessoryViewModel).Logger, "Failed to link to accessory gateway: {LinkResult}", new object[1] { result2 });
							result = false;
						}
						goto end_IL_0013;
						IL_00ed:
						if (!val.GetResult())
						{
							val2 = <accessoryGateway>5__2.LinkDeviceAsync(accessoryMacAddress, cancellationToken).GetAwaiter();
							if (!val2.IsCompleted)
							{
								num = (<>1__state = 1);
								<>u__2 = val2;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CommandResult>, <LinkToAccessoryGatewayAsync>d__24>(ref val2, ref this);
								return;
							}
							goto IL_0165;
						}
						result = true;
						end_IL_0013:;
					}
					catch (global::System.Exception) when (((CancellationToken)(ref cancellationToken)).IsCancellationRequested)
					{
						LoggerExtensions.LogDebug(((PageViewModel)connectToAccessoryViewModel).Logger, "Linking to accessory gateway was cancelled", global::System.Array.Empty<object>());
						goto IL_01ea;
					}
					catch (global::System.Exception ex2)
					{
						LoggerExtensions.LogError(((PageViewModel)connectToAccessoryViewModel).Logger, ex2, "Error linking to accessory gateway", global::System.Array.Empty<object>());
						goto IL_01ea;
					}
					goto end_IL_000e;
					IL_01ea:
					result = false;
					end_IL_000e:;
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

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <OnResumeAsync>d__19 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public ConnectToAccessoryViewModel<TAccessoryDevice> <>4__this;

			public ResumeReason reason;

			public INavigationParameters parameters;

			public CancellationToken cancellationToken;

			private <>c__DisplayClass19_0 <>8__1;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0078: Unknown result type (might be due to invalid IL or missing references)
				//IL_007d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0084: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0045: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Unknown result type (might be due to invalid IL or missing references)
				//IL_005f: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
				//IL_0101: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				ConnectToAccessoryViewModel<TAccessoryDevice> connectToAccessoryViewModel = <>4__this;
				try
				{
					TaskAwaiter val;
					if (num != 0)
					{
						<>8__1 = new <>c__DisplayClass19_0();
						<>8__1.<>4__this = <>4__this;
						val = ((SearchForDevicesViewModel)connectToAccessoryViewModel).OnResumeAsync(reason, parameters, cancellationToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnResumeAsync>d__19>(ref val, ref this);
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
					int errorCount = default(int);
					if (parameters.TryGetValue<int>("ErrorCountKey", ref errorCount))
					{
						connectToAccessoryViewModel._errorCount = errorCount;
					}
					connectToAccessoryViewModel._multipleAccessoriesFound = false;
					connectToAccessoryViewModel._accessoryFound = false;
					connectToAccessoryViewModel._searchCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
					<>8__1.searchCt = connectToAccessoryViewModel._searchCts.Token;
					global::System.Threading.Tasks.Task.Run((Func<global::System.Threading.Tasks.Task>)([AsyncStateMachine(typeof(ConnectToAccessoryViewModel<>.<>c__DisplayClass19_0.<<OnResumeAsync>b__0>d))] () =>
					{
						//IL_0002: Unknown result type (might be due to invalid IL or missing references)
						//IL_0007: Unknown result type (might be due to invalid IL or missing references)
						<>c__DisplayClass19_0.<<OnResumeAsync>b__0>d <<OnResumeAsync>b__0>d = default(<>c__DisplayClass19_0.<<OnResumeAsync>b__0>d);
						<<OnResumeAsync>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
						<<OnResumeAsync>b__0>d.<>4__this = <>8__1;
						<<OnResumeAsync>b__0>d.<>1__state = -1;
						((AsyncTaskMethodBuilder)(ref <<OnResumeAsync>b__0>d.<>t__builder)).Start<<>c__DisplayClass19_0.<<OnResumeAsync>b__0>d>(ref <<OnResumeAsync>b__0>d);
						return ((AsyncTaskMethodBuilder)(ref <<OnResumeAsync>b__0>d.<>t__builder)).Task;
					}), <>8__1.searchCt);
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<>8__1 = null;
					((AsyncTaskMethodBuilder)(ref <>t__builder)).SetException(exception);
					return;
				}
				<>1__state = -2;
				<>8__1 = null;
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
		private struct <WaitAccessoryDeviceOnlineAsync>d__25 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<TAccessoryDevice> <>t__builder;

			public MAC deviceMac;

			public ConnectToAccessoryViewModel<TAccessoryDevice> <>4__this;

			private <>c__DisplayClass25_0 <>8__1;

			public CancellationToken cancellationToken;

			private TAccessoryDevice <accessoryDevice>5__2;

			private global::System.DateTime <startTime>5__3;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0120: Unknown result type (might be due to invalid IL or missing references)
				//IL_0125: Unknown result type (might be due to invalid IL or missing references)
				//IL_012d: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
				//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
				//IL_0157: Unknown result type (might be due to invalid IL or missing references)
				//IL_015c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0213: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
				//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
				//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
				//IL_0105: Unknown result type (might be due to invalid IL or missing references)
				//IL_0107: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				ConnectToAccessoryViewModel<TAccessoryDevice> connectToAccessoryViewModel = <>4__this;
				TAccessoryDevice result;
				try
				{
					if ((uint)num > 1u)
					{
						<>8__1 = new <>c__DisplayClass25_0();
						<>8__1.deviceMac = deviceMac;
					}
					try
					{
						TaskAwaiter val;
						if (num != 0)
						{
							if (num != 1)
							{
								<accessoryDevice>5__2 = default(TAccessoryDevice);
								<startTime>5__3 = global::System.DateTime.Now;
								goto IL_0143;
							}
							val = <>u__1;
							<>u__1 = default(TaskAwaiter);
							num = (<>1__state = -1);
							goto IL_0206;
						}
						val = <>u__1;
						<>u__1 = default(TaskAwaiter);
						num = (<>1__state = -1);
						goto IL_013c;
						IL_00c6:
						LoggerExtensions.LogDebug(((PageViewModel)connectToAccessoryViewModel).Logger, "Connecting to accessory, waiting for a accessory to appear in logical device service", global::System.Array.Empty<object>());
						val = global::System.Threading.Tasks.Task.Delay(1000, cancellationToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, <WaitAccessoryDeviceOnlineAsync>d__25>(ref val, ref this);
							return;
						}
						goto IL_013c;
						IL_013c:
						((TaskAwaiter)(ref val)).GetResult();
						goto IL_0143;
						IL_020d:
						if ((int)((IDevicesCommon)<accessoryDevice>5__2).ActiveConnection == 0)
						{
							LoggerExtensions.LogDebug(((PageViewModel)connectToAccessoryViewModel).Logger, "Connecting to accessory, waiting for a accessory to come online", global::System.Array.Empty<object>());
							val = global::System.Threading.Tasks.Task.Delay(1000, cancellationToken).GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 1);
								<>u__1 = val;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, <WaitAccessoryDeviceOnlineAsync>d__25>(ref val, ref this);
								return;
							}
							goto IL_0206;
						}
						result = <accessoryDevice>5__2;
						goto end_IL_002f;
						IL_0206:
						((TaskAwaiter)(ref val)).GetResult();
						goto IL_020d;
						IL_0143:
						while (<accessoryDevice>5__2 == null)
						{
							TimeSpan val2 = connectToAccessoryViewModel.Elapsed(<startTime>5__3);
							if (!(((TimeSpan)(ref val2)).TotalMilliseconds < 60000.0))
							{
								break;
							}
							ILogicalDeviceManager deviceManager = connectToAccessoryViewModel._logicalDeviceService.DeviceManager;
							object obj;
							if (deviceManager == null)
							{
								obj = null;
							}
							else
							{
								global::System.Collections.Generic.IEnumerable<ILogicalDevice> logicalDevices = deviceManager.LogicalDevices;
								obj = ((logicalDevices != null) ? Enumerable.FirstOrDefault<ILogicalDevice>(logicalDevices, (Func<ILogicalDevice, bool>)((ILogicalDevice ld) => ld is TAccessoryDevice && (PhysicalAddress)(object)ld.LogicalId.ProductMacAddress == (PhysicalAddress)(object)<>8__1.deviceMac)) : null);
							}
							if (obj is TAccessoryDevice val3)
							{
								<accessoryDevice>5__2 = val3;
								continue;
							}
							goto IL_00c6;
						}
						if (<accessoryDevice>5__2 != null)
						{
							goto IL_020d;
						}
						result = default(TAccessoryDevice);
						end_IL_002f:;
					}
					catch (global::System.Exception) when (((CancellationToken)(ref cancellationToken)).IsCancellationRequested)
					{
						LoggerExtensions.LogDebug(((PageViewModel)connectToAccessoryViewModel).Logger, "Cancelled while waiting for accessory to come online", global::System.Array.Empty<object>());
						goto IL_027c;
					}
					catch (global::System.Exception ex2)
					{
						LoggerExtensions.LogError(((PageViewModel)connectToAccessoryViewModel).Logger, ex2, "Error while waiting for accessory to come online", global::System.Array.Empty<object>());
						goto IL_027c;
					}
					goto end_IL_000e;
					IL_027c:
					result = default(TAccessoryDevice);
					end_IL_000e:;
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<>8__1 = null;
					<>t__builder.SetException(exception);
					return;
				}
				<>1__state = -2;
				<>8__1 = null;
				<>t__builder.SetResult(result);
			}

			[DebuggerHidden]
			private void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				<>t__builder.SetStateMachine(stateMachine);
			}
		}

		private readonly IAccessoryRegistrationManager _accessoryRegistrationManager;

		private readonly ILogicalDeviceService _logicalDeviceService;

		private readonly INotificationRegistrationService _notificationRegistrationService;

		private const int MinMultiDeviceTimeoutMs = 10000;

		private const int MaxMultiDeviceTimeoutMs = 40000;

		private const int MaxDeviceSearchPeriodMs = 60000;

		private const int MultiDeviceConnectionDelayMs = 3000;

		private static readonly Random Random = new Random((int)global::System.DateTime.Now.Ticks);

		private CancellationTokenSource? _searchCts;

		private bool _multipleAccessoriesFound;

		private bool _accessoryFound;

		private int _errorCount;

		protected ConnectToAccessoryViewModel(IServiceProvider serviceProvider, IBluetoothPermissionHandler bluetoothPermissionHandler, IBleScannerService bleScannerService, IAccessoryRegistrationManager accessoryRegistrationManager, ILogicalDeviceService logicalDeviceService, INotificationRegistrationService notificationRegistrationService)
			: base(serviceProvider, bluetoothPermissionHandler, bleScannerService)
		{
			_accessoryRegistrationManager = accessoryRegistrationManager;
			_logicalDeviceService = logicalDeviceService;
			_notificationRegistrationService = notificationRegistrationService;
			((DualActionViewModel)this).BackAction = [CompilerGenerated] () => ((PageViewModel)this).NavigationService.GoBackAsync((INavigationParameters)null);
			((DualActionViewModel)this).PrimaryAction = [CompilerGenerated] () => ((PageViewModel)this).NavigationService.GoBackAsync((INavigationParameters)null);
			((DualActionViewModel)this).PrimaryActionStyle = (ActionStyle)2;
			((DualActionViewModel)this).PrimaryActionText = Strings.AddAndExplore_Action_Cancel;
			((DualActionViewModel)this).PrimaryActionEnabled = true;
		}

		protected abstract bool IncludeAccessory(BleScanResultModel bleScanResult);

		protected abstract global::System.Threading.Tasks.Task<TAccessoryDevice?> ConnectToAccessoryDeviceAsync(BleScanResultModel bleScanResultModel, IAccessoryRegistrationManager accessoryRegistrationManager, CancellationToken cancellationToken);

		protected abstract global::System.Threading.Tasks.Task ErrorConnectingToAccessoryAsync();

		protected abstract global::System.Threading.Tasks.Task ErrorMultipleAccessoriesFoundAsync();

		protected abstract global::System.Threading.Tasks.Task ErrorNoAccessoryFoundAsync();

		protected abstract global::System.Threading.Tasks.Task ConnectedToAccessoryAsync(BleScanResultModel bleScanResultModel, TAccessoryDevice accessoryDevice);

		[AsyncStateMachine(typeof(ConnectToAccessoryViewModel<>.<OnResumeAsync>d__19))]
		public override global::System.Threading.Tasks.Task OnResumeAsync(ResumeReason reason, INavigationParameters parameters, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			<OnResumeAsync>d__19 <OnResumeAsync>d__ = default(<OnResumeAsync>d__19);
			<OnResumeAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<OnResumeAsync>d__.<>4__this = this;
			<OnResumeAsync>d__.reason = reason;
			<OnResumeAsync>d__.parameters = parameters;
			<OnResumeAsync>d__.cancellationToken = cancellationToken;
			<OnResumeAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <OnResumeAsync>d__.<>t__builder)).Start<<OnResumeAsync>d__19>(ref <OnResumeAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <OnResumeAsync>d__.<>t__builder)).Task;
		}

		public override bool OnConfirmNavigation(NavigationReason reason, INavigationParameters parameters)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			int? num = default(int?);
			if (!parameters.TryGetValue<int?>("ErrorCountKey", ref num))
			{
				parameters.Add("ErrorCountKey", (object)_errorCount);
			}
			return ((PageViewModel)this).OnConfirmNavigation(reason, parameters);
		}

		protected override bool IncludeScanResult(BleScanResultModel bleScanResultModel)
		{
			IBleScanResult bleScanResult = bleScanResultModel.BleScanResult;
			if (!(bleScanResult is IdsCanAccessoryScanResult) && !(bleScanResult is MopekaScanResult) && !(bleScanResult is BleTirePressureMonitorScanResult))
			{
				return false;
			}
			if (bleScanResultModel.IsPairingActive)
			{
				return IncludeAccessory(bleScanResultModel);
			}
			return false;
		}

		[AsyncStateMachine(typeof(ConnectToAccessoryViewModel<>.<ConnectWithDeviceAsync>d__22))]
		protected override global::System.Threading.Tasks.Task ConnectWithDeviceAsync(BleScanResultModel bleScanResultModel)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			<ConnectWithDeviceAsync>d__22 <ConnectWithDeviceAsync>d__ = default(<ConnectWithDeviceAsync>d__22);
			<ConnectWithDeviceAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ConnectWithDeviceAsync>d__.<>4__this = this;
			<ConnectWithDeviceAsync>d__.bleScanResultModel = bleScanResultModel;
			<ConnectWithDeviceAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <ConnectWithDeviceAsync>d__.<>t__builder)).Start<<ConnectWithDeviceAsync>d__22>(ref <ConnectWithDeviceAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <ConnectWithDeviceAsync>d__.<>t__builder)).Task;
		}

		protected override void DeviceAdded(global::System.Collections.Generic.IReadOnlyList<BleScanResultModel> devices)
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			<>c__DisplayClass23_0 CS$<>8__locals5 = new <>c__DisplayClass23_0();
			CS$<>8__locals5.<>4__this = this;
			if (((global::System.Collections.Generic.IReadOnlyCollection<BleScanResultModel>)devices).Count < 0)
			{
				return;
			}
			if (((global::System.Collections.Generic.IReadOnlyCollection<BleScanResultModel>)devices).Count > 1)
			{
				if (_multipleAccessoriesFound)
				{
					return;
				}
				_multipleAccessoriesFound = true;
				global::System.Threading.Tasks.Task.Run((Func<global::System.Threading.Tasks.Task>)([AsyncStateMachine(typeof(ConnectToAccessoryViewModel<>.<>c__DisplayClass23_0.<<DeviceAdded>b__0>d))] () =>
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					<>c__DisplayClass23_0.<<DeviceAdded>b__0>d <<DeviceAdded>b__0>d = default(<>c__DisplayClass23_0.<<DeviceAdded>b__0>d);
					<<DeviceAdded>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
					<<DeviceAdded>b__0>d.<>4__this = CS$<>8__locals5;
					<<DeviceAdded>b__0>d.<>1__state = -1;
					((AsyncTaskMethodBuilder)(ref <<DeviceAdded>b__0>d.<>t__builder)).Start<<>c__DisplayClass23_0.<<DeviceAdded>b__0>d>(ref <<DeviceAdded>b__0>d);
					return ((AsyncTaskMethodBuilder)(ref <<DeviceAdded>b__0>d.<>t__builder)).Task;
				}), ViewModelExtensions.PausedCancellationToken((IViewModel)(object)this));
			}
			if (_accessoryFound)
			{
				return;
			}
			CS$<>8__locals5.device = Enumerable.FirstOrDefault<BleScanResultModel>((global::System.Collections.Generic.IEnumerable<BleScanResultModel>)devices);
			if (CS$<>8__locals5.device != null)
			{
				CancellationTokenSource? searchCts = _searchCts;
				if (searchCts != null)
				{
					CancellationTokenSourceExtensions.TryCancelAndDispose(searchCts);
				}
				global::System.Threading.Tasks.Task.Run((Func<global::System.Threading.Tasks.Task>)([AsyncStateMachine(typeof(ConnectToAccessoryViewModel<>.<>c__DisplayClass23_0.<<DeviceAdded>b__1>d))] () =>
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					<>c__DisplayClass23_0.<<DeviceAdded>b__1>d <<DeviceAdded>b__1>d = default(<>c__DisplayClass23_0.<<DeviceAdded>b__1>d);
					<<DeviceAdded>b__1>d.<>t__builder = AsyncTaskMethodBuilder.Create();
					<<DeviceAdded>b__1>d.<>4__this = CS$<>8__locals5;
					<<DeviceAdded>b__1>d.<>1__state = -1;
					((AsyncTaskMethodBuilder)(ref <<DeviceAdded>b__1>d.<>t__builder)).Start<<>c__DisplayClass23_0.<<DeviceAdded>b__1>d>(ref <<DeviceAdded>b__1>d);
					return ((AsyncTaskMethodBuilder)(ref <<DeviceAdded>b__1>d.<>t__builder)).Task;
				}), ViewModelExtensions.PausedCancellationToken((IViewModel)(object)this));
			}
		}

		[AsyncStateMachine(typeof(ConnectToAccessoryViewModel<>.<LinkToAccessoryGatewayAsync>d__24))]
		protected async global::System.Threading.Tasks.Task<bool> LinkToAccessoryGatewayAsync(MAC accessoryMacAddress, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			_ = 1;
			try
			{
				ILogicalDeviceManager deviceManager = _logicalDeviceService.DeviceManager;
				ILogicalDeviceAccessoryGateway accessoryGateway = ((deviceManager != null) ? Enumerable.FirstOrDefault<ILogicalDeviceAccessoryGateway>((global::System.Collections.Generic.IEnumerable<ILogicalDeviceAccessoryGateway>)deviceManager.FindLogicalDevices<ILogicalDeviceAccessoryGateway>((Func<ILogicalDeviceAccessoryGateway, bool>)((ILogicalDeviceAccessoryGateway logicalDevice) => (int)((IDevicesCommon)logicalDevice).ActiveConnection != 0))) : null);
				if (accessoryGateway == null)
				{
					LoggerExtensions.LogInformation(((PageViewModel)this).Logger, "No accessory gateway found to link to", global::System.Array.Empty<object>());
					return false;
				}
				if (await accessoryGateway.IsDeviceLinkedAsync(accessoryMacAddress, cancellationToken))
				{
					return true;
				}
				CommandResult val = await accessoryGateway.LinkDeviceAsync(accessoryMacAddress, cancellationToken);
				if ((int)val == 0)
				{
					return true;
				}
				LoggerExtensions.LogDebug(((PageViewModel)this).Logger, "Failed to link to accessory gateway: {LinkResult}", new object[1] { val });
				return false;
			}
			catch (global::System.Exception) when (((CancellationToken)(ref cancellationToken)).IsCancellationRequested)
			{
				LoggerExtensions.LogDebug(((PageViewModel)this).Logger, "Linking to accessory gateway was cancelled", global::System.Array.Empty<object>());
			}
			catch (global::System.Exception ex2)
			{
				LoggerExtensions.LogError(((PageViewModel)this).Logger, ex2, "Error linking to accessory gateway", global::System.Array.Empty<object>());
			}
			return false;
		}

		[AsyncStateMachine(typeof(ConnectToAccessoryViewModel<>.<WaitAccessoryDeviceOnlineAsync>d__25))]
		protected async global::System.Threading.Tasks.Task<TAccessoryDevice?> WaitAccessoryDeviceOnlineAsync(MAC deviceMac, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				TAccessoryDevice accessoryDevice = default(TAccessoryDevice);
				global::System.DateTime startTime = global::System.DateTime.Now;
				while (accessoryDevice == null)
				{
					TimeSpan val = Elapsed(startTime);
					if (!(((TimeSpan)(ref val)).TotalMilliseconds < 60000.0))
					{
						break;
					}
					ILogicalDeviceManager deviceManager = _logicalDeviceService.DeviceManager;
					object obj;
					if (deviceManager == null)
					{
						obj = null;
					}
					else
					{
						global::System.Collections.Generic.IEnumerable<ILogicalDevice> logicalDevices = deviceManager.LogicalDevices;
						obj = ((logicalDevices != null) ? Enumerable.FirstOrDefault<ILogicalDevice>(logicalDevices, (Func<ILogicalDevice, bool>)((ILogicalDevice ld) => ld is TAccessoryDevice && (PhysicalAddress)(object)ld.LogicalId.ProductMacAddress == (PhysicalAddress)(object)deviceMac)) : null);
					}
					if (obj is TAccessoryDevice val2)
					{
						accessoryDevice = val2;
						continue;
					}
					LoggerExtensions.LogDebug(((PageViewModel)this).Logger, "Connecting to accessory, waiting for a accessory to appear in logical device service", global::System.Array.Empty<object>());
					await global::System.Threading.Tasks.Task.Delay(1000, cancellationToken);
				}
				if (accessoryDevice == null)
				{
					return default(TAccessoryDevice);
				}
				while ((int)((IDevicesCommon)accessoryDevice).ActiveConnection == 0)
				{
					LoggerExtensions.LogDebug(((PageViewModel)this).Logger, "Connecting to accessory, waiting for a accessory to come online", global::System.Array.Empty<object>());
					await global::System.Threading.Tasks.Task.Delay(1000, cancellationToken);
				}
				return accessoryDevice;
			}
			catch (global::System.Exception) when (((CancellationToken)(ref cancellationToken)).IsCancellationRequested)
			{
				LoggerExtensions.LogDebug(((PageViewModel)this).Logger, "Cancelled while waiting for accessory to come online", global::System.Array.Empty<object>());
			}
			catch (global::System.Exception ex2)
			{
				LoggerExtensions.LogError(((PageViewModel)this).Logger, ex2, "Error while waiting for accessory to come online", global::System.Array.Empty<object>());
			}
			return default(TAccessoryDevice);
		}

		private TimeSpan Elapsed(global::System.DateTime startTime)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return global::System.DateTime.Now - startTime;
		}
	}
}
namespace App.Common.Pages.Pairing.Connections.Rv
{
	public static class DirectConnectionExtension
	{
		public static ILogicalDeviceTag? MakeLogicalDeviceTagSource(this IDirectConnection gatewayConnection)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Expected O, but got Unknown
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Expected O, but got Unknown
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Expected O, but got Unknown
			IDirectIdsCanConnectionBle val = (IDirectIdsCanConnectionBle)(object)((gatewayConnection is IDirectIdsCanConnectionBle) ? gatewayConnection : null);
			if (val == null)
			{
				IDirectIdsCanConnectionTcpIpWifi val2 = (IDirectIdsCanConnectionTcpIpWifi)(object)((gatewayConnection is IDirectIdsCanConnectionTcpIpWifi) ? gatewayConnection : null);
				if (val2 == null)
				{
					IDirectIdsCanConnectionTcpIpWired val3 = (IDirectIdsCanConnectionTcpIpWired)(object)((gatewayConnection is IDirectIdsCanConnectionTcpIpWired) ? gatewayConnection : null);
					if (val3 == null)
					{
						IDirectMyRvLinkConnectionBle val4 = (IDirectMyRvLinkConnectionBle)(object)((gatewayConnection is IDirectMyRvLinkConnectionBle) ? gatewayConnection : null);
						if (val4 != null)
						{
							return (ILogicalDeviceTag?)new LogicalDeviceTagSourceMyRvLinkBle(((IEndPointConnectionBle)val4).ConnectionId, ((IEndPointConnectionBle)val4).ConnectionGuid);
						}
						return null;
					}
					return (ILogicalDeviceTag?)new LogicalDeviceTagSourceDirect(((IEndPointConnectionTcpIp)val3).ConnectionIpAddress ?? "Direct");
				}
				return (ILogicalDeviceTag?)new LogicalDeviceTagSourceWifi(((IEndPointConnectionTcpIpWifi)val2).ConnectionSsid ?? "WiFi");
			}
			return (ILogicalDeviceTag?)new LogicalDeviceTagSourceBle(((IEndPointConnectionBle)val).ConnectionId);
		}
	}
	public class RvGatewayCanConnectionBle : RvGatewayConnectionBase<RvGatewayCanConnectionBle>, IDirectIdsCanConnectionBle, IEndPointConnectionBle, IEndPointConnectionWithPassword, IEndPointConnection, IDirectIdsCanConnection, IDirectConnection, IRvGatewayIdsCanConnection, IRvGatewayConnection, IComparable, IJsonSerializable, IEndPointConnectionWithSerialization, IJsonSerializerClass
	{
		[JsonIgnore]
		public override string ConnectionNameFriendly => "BLE";

		[JsonProperty]
		[field: CompilerGenerated]
		public string ConnectionId
		{
			[CompilerGenerated]
			get;
		}

		[JsonProperty]
		[field: CompilerGenerated]
		public string ConnectionPassword
		{
			[CompilerGenerated]
			get;
		}

		[JsonProperty]
		[field: CompilerGenerated]
		public Guid ConnectionGuid
		{
			[CompilerGenerated]
			get;
		}

		[JsonProperty]
		[field: CompilerGenerated]
		public GatewayVersion GatewayVersion
		{
			[CompilerGenerated]
			get;
		}

		[JsonIgnore]
		public override string DeviceSourceToken => ((object)ConnectionGuid/*cast due to .constrained prefix*/).ToString();

		static RvGatewayCanConnectionBle()
		{
			RvGatewayConnectionBase<RvGatewayCanConnectionBle>.RegisterJsonSerializer();
		}

		[JsonConstructor]
		public RvGatewayCanConnectionBle(string connectionId, string connectionPassword, Guid connectionGuid, GatewayVersion gatewayVersion)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			ConnectionId = connectionId;
			ConnectionPassword = connectionPassword;
			ConnectionGuid = connectionGuid;
			GatewayVersion = gatewayVersion;
		}
	}
	public interface IRvDirectConnectionDemo : IRvDirectConnectionNone, IDirectConnectionNone, IEndPointConnectionNone, IEndPointConnection, IDirectConnection
	{
	}
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvGatewayCanConnectionDemo : RvGatewayConnectionBase<RvGatewayCanConnectionDemo>, IRvDirectConnectionDemo, IRvDirectConnectionNone, IDirectConnectionNone, IEndPointConnectionNone, IEndPointConnection, IDirectConnection
	{
		[JsonIgnore]
		[field: CompilerGenerated]
		public override string ConnectionNameFriendly
		{
			[CompilerGenerated]
			get;
		} = Strings.demo;

		[JsonIgnore]
		[field: CompilerGenerated]
		public override ILogicalDeviceTag LogicalDeviceTagConnection
		{
			[CompilerGenerated]
			get;
		} = (ILogicalDeviceTag)new LogicalDeviceTagSourceDemo();

		[JsonIgnore]
		[field: CompilerGenerated]
		public override string DeviceSourceToken
		{
			[CompilerGenerated]
			get;
		}

		static RvGatewayCanConnectionDemo()
		{
			RvGatewayConnectionBase<RvGatewayCanConnectionDemo>.RegisterJsonSerializer();
		}

		public RvGatewayCanConnectionDemo()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			DeviceSourceToken = "Demo-Connection";
		}
	}
	public interface IRvDirectConnectionNone : IDirectConnectionNone, IEndPointConnectionNone, IEndPointConnection, IDirectConnection
	{
	}
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvGatewayCanConnectionNone : RvGatewayConnectionBase<RvGatewayCanConnectionNone>, IRvDirectConnectionNone, IDirectConnectionNone, IEndPointConnectionNone, IEndPointConnection, IDirectConnection
	{
		[JsonIgnore]
		[field: CompilerGenerated]
		public override string ConnectionNameFriendly
		{
			[CompilerGenerated]
			get;
		} = Strings.no_connection;

		[JsonIgnore]
		[field: CompilerGenerated]
		public override ILogicalDeviceTag LogicalDeviceTagConnection
		{
			[CompilerGenerated]
			get;
		} = (ILogicalDeviceTag)new LogicalDeviceTagSourceNone();

		[JsonIgnore]
		[field: CompilerGenerated]
		public override string DeviceSourceToken
		{
			[CompilerGenerated]
			get;
		}

		static RvGatewayCanConnectionNone()
		{
			RvGatewayConnectionBase<RvGatewayCanConnectionNone>.RegisterJsonSerializer();
		}

		public RvGatewayCanConnectionNone()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			DeviceSourceToken = "None-Connection";
		}
	}
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvGatewayCanConnectionTcpIpOctp : RvGatewayConnectionBase<RvGatewayCanConnectionTcpIpOctp>, IDirectIdsCanConnectionTcpIpWired, IEndPointConnectionTcpIpWired, IEndPointConnectionTcpIp, IEndPointConnection, IDirectIdsCanConnectionTcpIp, IDirectIdsCanConnection, IDirectConnection, IRvGatewayIdsCanConnection, IRvGatewayConnection, IComparable, IJsonSerializable, IEndPointConnectionWithSerialization, IJsonSerializerClass
	{
		[JsonIgnore]
		public override string ConnectionNameFriendly => "OCTP";

		[JsonIgnore]
		[field: CompilerGenerated]
		public string ConnectionIpAddress
		{
			[CompilerGenerated]
			get;
		}

		[JsonIgnore]
		[field: CompilerGenerated]
		public override string DeviceSourceToken
		{
			[CompilerGenerated]
			get;
		}

		static RvGatewayCanConnectionTcpIpOctp()
		{
			RvGatewayConnectionBase<RvGatewayCanConnectionTcpIpOctp>.RegisterJsonSerializer();
		}

		[JsonConstructor]
		public RvGatewayCanConnectionTcpIpOctp()
		{
			ConnectionIpAddress = ((object)CanAdapterFactory.DefaultLocalhostIpAddress).ToString();
			DeviceSourceToken = "OCTP-TCP-IP";
		}
	}
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvGatewayCanConnectionTcpIpWifiGateway : RvGatewayConnectionBase<RvGatewayCanConnectionTcpIpWifiGateway>, IDirectIdsCanConnectionTcpIpWifi, IEndPointConnectionTcpIpWifi, IEndPointConnectionTcpIp, IEndPointConnection, IEndPointConnectionWithPassword, IDirectIdsCanConnectionTcpIp, IDirectIdsCanConnection, IDirectConnection, IRvGatewayIdsCanConnection, IRvGatewayConnection, IComparable, IJsonSerializable, IEndPointConnectionWithSerialization, IJsonSerializerClass
	{
		[JsonIgnore]
		public override string ConnectionNameFriendly => ConnectionSsid;

		[JsonIgnore]
		[field: CompilerGenerated]
		public string? ConnectionIpAddress
		{
			[CompilerGenerated]
			get;
		}

		[JsonProperty]
		[field: CompilerGenerated]
		public string? ConnectionSsid
		{
			[CompilerGenerated]
			get;
		}

		[JsonProperty]
		[field: CompilerGenerated]
		public string ConnectionPassword
		{
			[CompilerGenerated]
			get;
		}

		[JsonIgnore]
		[field: CompilerGenerated]
		public override string DeviceSourceToken
		{
			[CompilerGenerated]
			get;
		}

		static RvGatewayCanConnectionTcpIpWifiGateway()
		{
			RvGatewayConnectionBase<RvGatewayCanConnectionTcpIpWifiGateway>.RegisterJsonSerializer();
		}

		[JsonConstructor]
		public RvGatewayCanConnectionTcpIpWifiGateway(string connectionSsid, string connectionPassword)
		{
			ConnectionIpAddress = ((object)CanAdapterFactory.DefaultMyrvGatewayIpAddress).ToString();
			ConnectionSsid = connectionSsid;
			ConnectionPassword = connectionPassword;
			object obj = ConnectionSsid ?? ConnectionIpAddress ?? "DefaultWiFi";
			if (obj == null)
			{
				obj = "";
			}
			DeviceSourceToken = (string)obj;
		}
	}
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvGatewayCanConnectionTcpIpWired : RvGatewayConnectionBase<RvGatewayCanConnectionTcpIpWired>, IDirectIdsCanConnectionTcpIpWired, IEndPointConnectionTcpIpWired, IEndPointConnectionTcpIp, IEndPointConnection, IDirectIdsCanConnectionTcpIp, IDirectIdsCanConnection, IDirectConnection, IRvGatewayIdsCanConnection, IRvGatewayConnection, IComparable, IJsonSerializable, IEndPointConnectionWithSerialization, IJsonSerializerClass
	{
		[JsonIgnore]
		public override string ConnectionNameFriendly => "Direct Connection";

		[JsonProperty]
		[field: CompilerGenerated]
		public string ConnectionIpAddress
		{
			[CompilerGenerated]
			get;
		}

		[JsonIgnore]
		[field: CompilerGenerated]
		public override string DeviceSourceToken
		{
			[CompilerGenerated]
			get;
		}

		static RvGatewayCanConnectionTcpIpWired()
		{
			RvGatewayConnectionBase<RvGatewayCanConnectionTcpIpWired>.RegisterJsonSerializer();
		}

		[JsonConstructor]
		public RvGatewayCanConnectionTcpIpWired(string connectionIpAddress)
		{
			ConnectionIpAddress = connectionIpAddress ?? ((object)CanAdapterFactory.DefaultLocalhostIpAddress).ToString();
			DeviceSourceToken = "Wired" + (connectionIpAddress ?? string.Empty);
		}
	}
	public interface IRvGatewayIdsCanConnection : IRvGatewayConnection, IComparable, IJsonSerializable, IEndPointConnectionWithSerialization, IEndPointConnection, IJsonSerializerClass, IDirectConnection, IDirectIdsCanConnection
	{
	}
	public interface IRvGatewayMyRvLinkConnection : IRvGatewayConnection, IComparable, IJsonSerializable, IEndPointConnectionWithSerialization, IEndPointConnection, IJsonSerializerClass, IDirectConnection, IDirectMyRvLinkConnection
	{
	}
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public abstract class RvGatewayConnectionBase<TSerializable> : JsonSerializable<TSerializable>, IRvGatewayConnection, IComparable, IJsonSerializable, IEndPointConnectionWithSerialization, IEndPointConnection, IJsonSerializerClass, IDirectConnection where TSerializable : class
	{
		public const string LogTag = "RvGatewayConnectionBase";

		[JsonProperty]
		public string? SerializerClass
		{
			get
			{
				global::System.Type type = ((object)this).GetType();
				if (type == null)
				{
					return null;
				}
				return ((MemberInfo)type).Name;
			}
		}

		[JsonIgnore]
		public abstract string ConnectionNameFriendly { get; }

		[JsonIgnore]
		public virtual ILogicalDeviceTag? LogicalDeviceTagConnection => ((IDirectConnection)(object)this).MakeLogicalDeviceTagSource();

		[JsonIgnore]
		public abstract string DeviceSourceToken { get; }

		public virtual int CompareTo(object? obj)
		{
			if (obj == null)
			{
				return 1;
			}
			if (this == obj)
			{
				return 0;
			}
			if (!(obj is IRvGatewayIdsCanConnection rvGatewayIdsCanConnection))
			{
				return 1;
			}
			int num = string.CompareOrdinal(ConnectionNameFriendly, ((IEndPointConnection)rvGatewayIdsCanConnection).ConnectionNameFriendly);
			if (num != 0)
			{
				return num;
			}
			if (!((IEquatable<ILogicalDeviceTag>)(object)LogicalDeviceTagConnection).Equals(rvGatewayIdsCanConnection.LogicalDeviceTagConnection))
			{
				return 1;
			}
			return 0;
		}

		public override string ToString()
		{
			return "'" + ConnectionNameFriendly + "'";
		}

		protected static void RegisterJsonSerializer()
		{
			global::System.Type typeFromHandle = typeof(TSerializable);
			TypeRegistry.Register(((MemberInfo)typeFromHandle).Name, typeFromHandle);
		}

		protected Guid StringToGuid(string rawString)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			byte[] array = new byte[16];
			byte[] bytes = Encoding.ASCII.GetBytes(rawString);
			Buffer.BlockCopy((global::System.Array)bytes, 0, (global::System.Array)array, 0, Math.Min(16, bytes.Length));
			return new Guid(array);
		}
	}
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvGatewayMyRvLinkConnectionBle : RvGatewayConnectionBase<RvGatewayMyRvLinkConnectionBle>, IDirectMyRvLinkConnectionBle, IEndPointConnectionBle, IDirectMyRvLinkConnection, IDirectConnection, IEndPointConnection, IRvGatewayMyRvLinkConnection, IRvGatewayConnection, IComparable, IJsonSerializable, IEndPointConnectionWithSerialization, IJsonSerializerClass
	{
		[JsonIgnore]
		public override string ConnectionNameFriendly
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Expected I4, but got Unknown
				RvLinkGatewayType gatewayType = GatewayType;
				switch ((int)gatewayType)
				{
				case 2:
					return "ABS";
				case 3:
					return "SWAY";
				case 0:
				case 1:
					return "BLE";
				default:
					return "BLE-UNKNOWN";
				}
			}
		}

		[JsonProperty]
		[field: CompilerGenerated]
		public string ConnectionId
		{
			[CompilerGenerated]
			get;
		}

		[JsonProperty]
		[field: CompilerGenerated]
		public Guid ConnectionGuid
		{
			[CompilerGenerated]
			get;
		}

		[JsonIgnore]
		public RvLinkGatewayType GatewayType => MyRvLinkBleGatewayScanResult.GatewayTypeFromDeviceName(ConnectionId);

		[JsonIgnore]
		public override string DeviceSourceToken => ((object)ConnectionGuid/*cast due to .constrained prefix*/).ToString();

		static RvGatewayMyRvLinkConnectionBle()
		{
			RvGatewayConnectionBase<RvGatewayMyRvLinkConnectionBle>.RegisterJsonSerializer();
		}

		[JsonConstructor]
		public RvGatewayMyRvLinkConnectionBle(Guid connectionGuid, string connectionId)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			ConnectionId = connectionId;
			ConnectionGuid = connectionGuid;
		}
	}
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvGatewayMyRvLinkConnectionTcpIpOctp : RvGatewayConnectionBase<RvGatewayMyRvLinkConnectionTcpIpOctp>, IDirectIdsCanConnectionTcpIpWired, IEndPointConnectionTcpIpWired, IEndPointConnectionTcpIp, IEndPointConnection, IDirectIdsCanConnectionTcpIp, IDirectIdsCanConnection, IDirectConnection, IDirectMyRvLinkConnectionTcpIp, IDirectMyRvLinkConnection, IRvGatewayMyRvLinkConnection, IRvGatewayConnection, IComparable, IJsonSerializable, IEndPointConnectionWithSerialization, IJsonSerializerClass
	{
		[JsonIgnore]
		public override string ConnectionNameFriendly => "OCTP";

		[JsonIgnore]
		[field: CompilerGenerated]
		public string ConnectionIpAddress
		{
			[CompilerGenerated]
			get;
		}

		[JsonIgnore]
		[field: CompilerGenerated]
		public override string DeviceSourceToken
		{
			[CompilerGenerated]
			get;
		}

		static RvGatewayMyRvLinkConnectionTcpIpOctp()
		{
			RvGatewayConnectionBase<RvGatewayMyRvLinkConnectionTcpIpOctp>.RegisterJsonSerializer();
		}

		[JsonConstructor]
		public RvGatewayMyRvLinkConnectionTcpIpOctp()
		{
			ConnectionIpAddress = ((object)CanAdapterFactory.DefaultLocalhostIpAddress).ToString();
			DeviceSourceToken = "OCTP-TCP-IP-RvLink";
		}
	}
	[JsonObject(/*Could not decode attribute arguments.*/)]
	public class RvGatewayMyRvLinkConnectionTcpIpWired : RvGatewayConnectionBase<RvGatewayMyRvLinkConnectionTcpIpWired>, IDirectIdsCanConnectionTcpIpWired, IEndPointConnectionTcpIpWired, IEndPointConnectionTcpIp, IEndPointConnection, IDirectIdsCanConnectionTcpIp, IDirectIdsCanConnection, IDirectConnection, IDirectMyRvLinkConnectionTcpIp, IDirectMyRvLinkConnection, IRvGatewayMyRvLinkConnection, IRvGatewayConnection, IComparable, IJsonSerializable, IEndPointConnectionWithSerialization, IJsonSerializerClass
	{
		[JsonIgnore]
		public override string ConnectionNameFriendly => "Direct Connection RvLink";

		[JsonProperty]
		[field: CompilerGenerated]
		public string ConnectionIpAddress
		{
			[CompilerGenerated]
			get;
		}

		[JsonIgnore]
		[field: CompilerGenerated]
		public override string DeviceSourceToken
		{
			[CompilerGenerated]
			get;
		}

		static RvGatewayMyRvLinkConnectionTcpIpWired()
		{
			RvGatewayConnectionBase<RvGatewayMyRvLinkConnectionTcpIpWired>.RegisterJsonSerializer();
		}

		[JsonConstructor]
		public RvGatewayMyRvLinkConnectionTcpIpWired(string connectionIpAddress)
		{
			ConnectionIpAddress = connectionIpAddress ?? ((object)CanAdapterFactory.DefaultLocalhostIpAddress).ToString();
			DeviceSourceToken = "WiredRvLink" + (connectionIpAddress ?? string.Empty);
		}
	}
}
namespace App.Common.Pages.Pairing.Connect
{
	public abstract class ConnectPage : DualActionPage
	{
		public static readonly BindableProperty ContentProperty = BindableProperty.Create("Content", typeof(View), typeof(ConnectPage), (object)null, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)null, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)null, (CreateDefaultValueDelegate)null);

		public static readonly BindableProperty ControlTemplateProperty = BindableProperty.Create("ControlTemplate", typeof(ControlTemplate), typeof(ConnectPage), (object)null, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)null, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)null, (CreateDefaultValueDelegate)null);

		public View? Content
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected O, but got Unknown
				return (View)((BindableObject)this).GetValue(ContentProperty);
			}
			set
			{
				((BindableObject)this).SetValue(ContentProperty, (object)value);
			}
		}

		public ControlTemplate? ControlTemplate
		{
			get
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected O, but got Unknown
				return (ControlTemplate)((BindableObject)this).GetValue(ControlTemplateProperty);
			}
			set
			{
				((BindableObject)this).SetValue(ControlTemplateProperty, (object)value);
			}
		}

		protected ConnectPage()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Expected O, but got Unknown
			//IL_00b9: Expected O, but got Unknown
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Expected O, but got Unknown
			//IL_010b: Expected O, but got Unknown
			Grid val = new Grid();
			((DefinitionCollection<RowDefinition>)(object)val.RowDefinitions).Add(new RowDefinition(GridLength.op_Implicit(248.0)));
			((DefinitionCollection<RowDefinition>)(object)val.RowDefinitions).Add(new RowDefinition(GridLength.Star));
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val).Children).Add((IView)(object)FluentUIExtensions.GridRow<SKLottieView>(new SKLottieView
			{
				WidthRequest = 248.0,
				HeightRequest = 248.0,
				Margin = Thickness.op_Implicit(0.0),
				Padding = Thickness.op_Implicit(0.0),
				RepeatCount = -1,
				RepeatMode = (SKLottieRepeatMode)0,
				Source = (SKLottieImageSource)SKLottieImageSource.FromFile("OneControl/Resources/Lottie/connecting_spinner.json")
			}, 0));
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val).Children).Add((IView)(object)FluentUIExtensions.Binding<ContentView>(FluentUIExtensions.Binding<ContentView>(FluentUIExtensions.GridRow<ContentView>(new ContentView(), 1), ContentView.ContentProperty, ContentProperty, (BindableObject)(object)this, ((AppBarPage)this).LifetimeDisposable, (BindingMode)0, (IValueConverter)null, (object)null), TemplatedView.ControlTemplateProperty, ControlTemplateProperty, (BindableObject)(object)this, ((AppBarPage)this).LifetimeDisposable, (BindingMode)0, (IValueConverter)null, (object)null));
			((DualActionPage)this).Content = (View)val;
		}
	}
	public abstract class ConnectViewModel : DualActionViewModel
	{
		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <OnResumeAsync>d__3 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder <>t__builder;

			public ConnectViewModel <>4__this;

			public ResumeReason reason;

			public INavigationParameters parameters;

			public CancellationToken cancellationToken;

			private TaskAwaiter <>u__1;

			private void MoveNext()
			{
				//IL_0059: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0065: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				ConnectViewModel connectViewModel = <>4__this;
				try
				{
					TaskAwaiter val;
					if (num != 0)
					{
						val = connectViewModel.<>n__0(reason, parameters, cancellationToken).GetAwaiter();
						if (!((TaskAwaiter)(ref val)).IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <OnResumeAsync>d__3>(ref val, ref this);
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
					int errorCount = default(int);
					if (parameters.TryGetValue<int>("ErrorCountKey", ref errorCount))
					{
						connectViewModel.ErrorCount = errorCount;
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

		public const string ErrorCountKey = "ErrorCountKey";

		protected int ErrorCount;

		protected ConnectViewModel(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			((DualActionViewModel)this).BackAction = [CompilerGenerated] () => ((PageViewModel)this).NavigationService.GoBackAsync((INavigationParameters)null);
			((DualActionViewModel)this).PrimaryAction = [CompilerGenerated] () => ((PageViewModel)this).NavigationService.GoBackAsync((INavigationParameters)null);
			((DualActionViewModel)this).PrimaryActionStyle = (ActionStyle)2;
			((DualActionViewModel)this).PrimaryActionText = Strings.AddAndExplore_Action_Cancel;
		}

		[AsyncStateMachine(typeof(<OnResumeAsync>d__3))]
		public override global::System.Threading.Tasks.Task OnResumeAsync(ResumeReason reason, INavigationParameters parameters, CancellationToken cancellationToken)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			<OnResumeAsync>d__3 <OnResumeAsync>d__ = default(<OnResumeAsync>d__3);
			<OnResumeAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<OnResumeAsync>d__.<>4__this = this;
			<OnResumeAsync>d__.reason = reason;
			<OnResumeAsync>d__.parameters = parameters;
			<OnResumeAsync>d__.cancellationToken = cancellationToken;
			<OnResumeAsync>d__.<>1__state = -1;
			((AsyncTaskMethodBuilder)(ref <OnResumeAsync>d__.<>t__builder)).Start<<OnResumeAsync>d__3>(ref <OnResumeAsync>d__);
			return ((AsyncTaskMethodBuilder)(ref <OnResumeAsync>d__.<>t__builder)).Task;
		}

		public override bool OnConfirmNavigation(NavigationReason reason, INavigationParameters parameters)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			int? num = default(int?);
			if (!parameters.TryGetValue<int?>("ErrorCountKey", ref num))
			{
				parameters.Add("ErrorCountKey", (object)ErrorCount);
			}
			return ((PageViewModel)this).OnConfirmNavigation(reason, parameters);
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private global::System.Threading.Tasks.Task <>n__0(ResumeReason reason, INavigationParameters parameters, CancellationToken cancellationToken)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			return ((PageViewModel)this).OnResumeAsync(reason, parameters, cancellationToken);
		}
	}
	public class WiFiConnectHelpers
	{
		public const string SsidKey = "SsidKey";

		public const string PasswordKey = "PasswordKey";

		public static INavigationParameters PutConnectParameters(string ssid, string? password = null)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			NavigationParameters val = new NavigationParameters();
			val.Add("SsidKey", (object)ssid);
			NavigationParameters val2 = val;
			if (!string.IsNullOrWhiteSpace(password))
			{
				val2.Add("PasswordKey", (object)password);
			}
			return (INavigationParameters)(object)val2;
		}

		public static string GetSsid(INavigationParameters parameters)
		{
			string text = default(string);
			parameters.TryGetValue<string>("SsidKey", ref text);
			return text ?? string.Empty;
		}

		public static string GetPassword(INavigationParameters parameters)
		{
			string text = default(string);
			parameters.TryGetValue<string>("PasswordKey", ref text);
			return text ?? string.Empty;
		}
	}
}
namespace App.Common.Pages.Pairing.CollectWiFiInfo
{
	[XamlFilePath("CollectWiFiInfo/CollectionWiFiInfoPage.xaml")]
	public class CollectWiFiInfoPage : DualActionPage
	{
		public CollectWiFiInfoPage()
		{
			InitializeComponent();
		}

		[GeneratedCode("Microsoft.Maui.Controls.SourceGen", "1.0.0.0")]
		private void InitializeComponent()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected O, but got Unknown
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Expected O, but got Unknown
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Expected O, but got Unknown
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Expected O, but got Unknown
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Expected O, but got Unknown
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected O, but got Unknown
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Expected O, but got Unknown
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0259: Expected O, but got Unknown
			//IL_025e: Expected O, but got Unknown
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0316: Unknown result type (might be due to invalid IL or missing references)
			//IL_0467: Unknown result type (might be due to invalid IL or missing references)
			//IL_0565: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			BindingExtension val = new BindingExtension();
			global::System.Type typeFromHandle = typeof(DualActionPage);
			RelativeSourceExtension val2 = new RelativeSourceExtension();
			BindingExtension val3 = new BindingExtension();
			string collectWiFiInfo_Ssid_Placeholder = Strings.CollectWiFiInfo_Ssid_Placeholder;
			BindingExtension val4 = new BindingExtension();
			MaterialEntry val5 = new MaterialEntry();
			string collectWiFiInfo_Password_Placeholder = Strings.CollectWiFiInfo_Password_Placeholder;
			InvertedBoolConverter val6 = new InvertedBoolConverter();
			BindingExtension val7 = new BindingExtension();
			BindingExtension val8 = new BindingExtension();
			MaterialEntry val9 = new MaterialEntry();
			VerticalStackLayout val10 = new VerticalStackLayout();
			ContentView val11 = new ContentView();
			CollectWiFiInfoPage collectWiFiInfoPage;
			NameScope val12 = (NameScope)(((object)NameScope.GetNameScope((BindableObject)(object)(collectWiFiInfoPage = this))) ?? ((object)new NameScope()));
			NameScope.SetNameScope((BindableObject)(object)collectWiFiInfoPage, (INameScope)(object)val12);
			((Element)val11).transientNamescope = (INameScope)(object)val12;
			((Element)val10).transientNamescope = (INameScope)(object)val12;
			((Element)val5).transientNamescope = (INameScope)(object)val12;
			((Element)val9).transientNamescope = (INameScope)(object)val12;
			val.Path = "Message";
			val.TypedBinding = (TypedBindingBase)(object)new TypedBinding<CollectWiFiInfoViewModel, string>((Func<CollectWiFiInfoViewModel, ValueTuple<string, bool>>)([CompilerGenerated] (CollectWiFiInfoViewModel P_0) => (P_0 != null) ? new ValueTuple<string, bool>(P_0.Message, true) : default(ValueTuple<string, bool>)), (Action<CollectWiFiInfoViewModel, string>)([CompilerGenerated] (CollectWiFiInfoViewModel P_0, string P_1) =>
			{
				if (P_0 != null)
				{
					P_0.Message = P_1;
				}
			}), new Tuple<Func<CollectWiFiInfoViewModel, object>, string>[1]
			{
				new Tuple<Func<CollectWiFiInfoViewModel, object>, string>((Func<CollectWiFiInfoViewModel, object>)([CompilerGenerated] (CollectWiFiInfoViewModel P_0) => P_0), "Message")
			});
			((BindingBase)val.TypedBinding).Mode = val.Mode;
			val.TypedBinding.Converter = val.Converter;
			val.TypedBinding.ConverterParameter = val.ConverterParameter;
			((BindingBase)val.TypedBinding).StringFormat = val.StringFormat;
			val.TypedBinding.Source = val.Source;
			val.TypedBinding.UpdateSourceEventName = val.UpdateSourceEventName;
			((BindingBase)val.TypedBinding).FallbackValue = val.FallbackValue;
			((BindingBase)val.TypedBinding).TargetNullValue = val.TargetNullValue;
			BindingBase typedBinding = (BindingBase)(object)val.TypedBinding;
			((BindableObject)collectWiFiInfoPage).SetBinding(DualActionPage.InstructionsProperty, typedBinding);
			val3.Path = "FixedContentHeight";
			val2.Mode = (RelativeBindingSourceMode)3;
			val2.AncestorType = typeFromHandle;
			RelativeBindingSource source = ((IMarkupExtension<RelativeBindingSource>)(object)val2).ProvideValue((IServiceProvider)null);
			val3.Source = source;
			XamlServiceProvider val13 = new XamlServiceProvider();
			global::System.Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver val14 = new XmlNamespaceResolver();
			val14.Add("", "http://schemas.microsoft.com/dotnet/2021/maui");
			val14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			val14.Add("ui", "http://lci1.com/schemas/ui");
			val14.Add("collectWiFiInfo", "clr-namespace:App.Common.Pages.Pairing.CollectWiFiInfo");
			val14.Add("dualActionPage", "clr-namespace:App.Common.DualActionPage;assembly=App.Common");
			val14.Add("resources", "clr-namespace:App.Common.Pages.Pairing.Resources");
			val14.Add("toolkit", "http://schemas.microsoft.com/dotnet/2022/maui/toolkit");
			val13.Add(typeFromHandle2, (object)new XamlTypeResolver((IXmlNamespaceResolver)val14, typeof(CollectWiFiInfoPage).Assembly));
			BindingBase val15 = ((IMarkupExtension<BindingBase>)(object)val3).ProvideValue((IServiceProvider)val13);
			((BindableObject)val11).SetBinding(VisualElement.HeightRequestProperty, val15);
			((BindableObject)val10).SetValue(StackBase.SpacingProperty, (object)16.0);
			((BindableObject)val10).SetValue(View.VerticalOptionsProperty, (object)LayoutOptions.Center);
			((BindableObject)val5).SetValue(MaterialEntry.PlaceholderProperty, (object)collectWiFiInfo_Ssid_Placeholder);
			((BindableObject)val5).SetValue(MaterialEntry.ClearButtonVisibilityProperty, (object)(ClearButtonVisibility)1);
			val4.Path = "Ssid";
			val4.TypedBinding = (TypedBindingBase)(object)new TypedBinding<CollectWiFiInfoViewModel, string>((Func<CollectWiFiInfoViewModel, ValueTuple<string, bool>>)([CompilerGenerated] (CollectWiFiInfoViewModel P_0) => (P_0 != null) ? new ValueTuple<string, bool>(P_0.Ssid, true) : default(ValueTuple<string, bool>)), (Action<CollectWiFiInfoViewModel, string>)([CompilerGenerated] (CollectWiFiInfoViewModel P_0, string P_1) =>
			{
				if (P_0 != null)
				{
					P_0.Ssid = P_1;
				}
			}), new Tuple<Func<CollectWiFiInfoViewModel, object>, string>[1]
			{
				new Tuple<Func<CollectWiFiInfoViewModel, object>, string>((Func<CollectWiFiInfoViewModel, object>)([CompilerGenerated] (CollectWiFiInfoViewModel P_0) => P_0), "Ssid")
			});
			((BindingBase)val4.TypedBinding).Mode = val4.Mode;
			val4.TypedBinding.Converter = val4.Converter;
			val4.TypedBinding.ConverterParameter = val4.ConverterParameter;
			((BindingBase)val4.TypedBinding).StringFormat = val4.StringFormat;
			val4.TypedBinding.Source = val4.Source;
			val4.TypedBinding.UpdateSourceEventName = val4.UpdateSourceEventName;
			((BindingBase)val4.TypedBinding).FallbackValue = val4.FallbackValue;
			((BindingBase)val4.TypedBinding).TargetNullValue = val4.TargetNullValue;
			BindingBase typedBinding2 = (BindingBase)(object)val4.TypedBinding;
			((BindableObject)val5).SetBinding(MaterialEntry.TextProperty, typedBinding2);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val10).Children).Add((IView)(object)val5);
			((BindableObject)val9).SetValue(MaterialEntry.PlaceholderProperty, (object)collectWiFiInfo_Password_Placeholder);
			((BindableObject)val9).SetValue(MaterialEntry.IsPasswordProperty, (object)true);
			((BindableObject)val9).SetValue(MaterialEntry.ClearButtonVisibilityProperty, (object)(ClearButtonVisibility)1);
			val7.Path = "SsidOnly";
			ICommunityToolkitValueConverter converter = ((IMarkupExtension<ICommunityToolkitValueConverter>)(object)val6).ProvideValue((IServiceProvider)null);
			val7.Converter = (IValueConverter)(object)converter;
			val7.TypedBinding = (TypedBindingBase)(object)new TypedBinding<CollectWiFiInfoViewModel, bool>((Func<CollectWiFiInfoViewModel, ValueTuple<bool, bool>>)([CompilerGenerated] (CollectWiFiInfoViewModel P_0) => (P_0 != null) ? new ValueTuple<bool, bool>(P_0.SsidOnly, true) : default(ValueTuple<bool, bool>)), (Action<CollectWiFiInfoViewModel, bool>)([CompilerGenerated] (CollectWiFiInfoViewModel P_0, bool P_1) =>
			{
				if (P_0 != null)
				{
					P_0.SsidOnly = P_1;
				}
			}), new Tuple<Func<CollectWiFiInfoViewModel, object>, string>[1]
			{
				new Tuple<Func<CollectWiFiInfoViewModel, object>, string>((Func<CollectWiFiInfoViewModel, object>)([CompilerGenerated] (CollectWiFiInfoViewModel P_0) => P_0), "SsidOnly")
			});
			((BindingBase)val7.TypedBinding).Mode = val7.Mode;
			val7.TypedBinding.Converter = val7.Converter;
			val7.TypedBinding.ConverterParameter = val7.ConverterParameter;
			((BindingBase)val7.TypedBinding).StringFormat = val7.StringFormat;
			val7.TypedBinding.Source = val7.Source;
			val7.TypedBinding.UpdateSourceEventName = val7.UpdateSourceEventName;
			((BindingBase)val7.TypedBinding).FallbackValue = val7.FallbackValue;
			((BindingBase)val7.TypedBinding).TargetNullValue = val7.TargetNullValue;
			BindingBase typedBinding3 = (BindingBase)(object)val7.TypedBinding;
			((BindableObject)val9).SetBinding(VisualElement.IsVisibleProperty, typedBinding3);
			val8.Path = "Password";
			val8.TypedBinding = (TypedBindingBase)(object)new TypedBinding<CollectWiFiInfoViewModel, string>((Func<CollectWiFiInfoViewModel, ValueTuple<string, bool>>)([CompilerGenerated] (CollectWiFiInfoViewModel P_0) => (P_0 != null) ? new ValueTuple<string, bool>(P_0.Password, true) : default(ValueTuple<string, bool>)), (Action<CollectWiFiInfoViewModel, string>)([CompilerGenerated] (CollectWiFiInfoViewModel P_0, string P_1) =>
			{
				if (P_0 != null)
				{
					P_0.Password = P_1;
				}
			}), new Tuple<Func<CollectWiFiInfoViewModel, object>, string>[1]
			{
				new Tuple<Func<CollectWiFiInfoViewModel, object>, string>((Func<CollectWiFiInfoViewModel, object>)([CompilerGenerated] (CollectWiFiInfoViewModel P_0) => P_0), "Password")
			});
			((BindingBase)val8.TypedBinding).Mode = val8.Mode;
			val8.TypedBinding.Converter = val8.Converter;
			val8.TypedBinding.ConverterParameter = val8.ConverterParameter;
			((BindingBase)val8.TypedBinding).StringFormat = val8.StringFormat;
			val8.TypedBinding.Source = val8.Source;
			val8.TypedBinding.UpdateSourceEventName = val8.UpdateSourceEventName;
			((BindingBase)val8.TypedBinding).FallbackValue = val8.FallbackValue;
			((BindingBase)val8.TypedBinding).TargetNullValue = val8.TargetNullValue;
			BindingBase typedBinding4 = (BindingBase)(object)val8.TypedBinding;
			((BindableObject)val9).SetBinding(MaterialEntry.TextProperty, typedBinding4);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)val10).Children).Add((IView)(object)val9);
			((BindableObject)val11).SetValue(ContentView.ContentProperty, (object)val10);
			((BindableObject)collectWiFiInfoPage).SetValue(DualActionPage.ContentProperty, (object)val11);
		}
	}
	public abstract class CollectWiFiInfoViewModel : ConnectViewModel
	{
		[ObservableProperty]
		private string _message = string.Empty;

		[ObservableProperty]
		private string _ssid = string.Empty;

		[ObservableProperty]
		private string _password = string.Empty;

		[ObservableProperty]
		private bool _ssidOnly;

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string Message
		{
			get
			{
				return _message;
			}
			[MemberNotNull("_message")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(_message, value))
				{
					((ObservableObject)this).OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Message);
					_message = value;
					((ObservableObject)this).OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Message);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string Ssid
		{
			get
			{
				return _ssid;
			}
			[MemberNotNull("_ssid")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(_ssid, value))
				{
					((ObservableObject)this).OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Ssid);
					_ssid = value;
					OnSsidChanged(value);
					((ObservableObject)this).OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Ssid);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public string Password
		{
			get
			{
				return _password;
			}
			[MemberNotNull("_password")]
			set
			{
				if (!EqualityComparer<string>.Default.Equals(_password, value))
				{
					((ObservableObject)this).OnPropertyChanging(__KnownINotifyPropertyChangingArgs.Password);
					_password = value;
					OnPasswordChanged(value);
					((ObservableObject)this).OnPropertyChanged(__KnownINotifyPropertyChangedArgs.Password);
				}
			}
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		[ExcludeFromCodeCoverage]
		public bool SsidOnly
		{
			get
			{
				return _ssidOnly;
			}
			set
			{
				if (!EqualityComparer<bool>.Default.Equals(_ssidOnly, value))
				{
					((ObservableObject)this).OnPropertyChanging(__KnownINotifyPropertyChangingArgs.SsidOnly);
					_ssidOnly = value;
					((ObservableObject)this).OnPropertyChanged(__KnownINotifyPropertyChangedArgs.SsidOnly);
				}
			}
		}

		protected CollectWiFiInfoViewModel(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			UpdatePrimaryActionEnabled();
		}

		protected virtual void OnSsidAndPasswordUpdated(string ssid, string password)
		{
			UpdatePrimaryActionEnabled();
		}

		private void UpdatePrimaryActionEnabled()
		{
			((DualActionViewModel)this).PrimaryActionEnabled = (SsidOnly ? (!string.IsNullOrWhiteSpace(Ssid)) : (!string.IsNullOrWhiteSpace(Ssid) && !string.IsNullOrWhiteSpace(Password)));
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnSsidChanged(string value)
		{
			OnSsidAndPasswordUpdated(value, Password);
		}

		[GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.4.0.0")]
		private void OnPasswordChanged(string value)
		{
			OnSsidAndPasswordUpdated(Ssid, value);
		}
	}
}
[CompilerGenerated]
internal sealed class <>z__ReadOnlyArray<T> : global::System.Collections.IEnumerable, global::System.Collections.ICollection, global::System.Collections.IList, global::System.Collections.Generic.IEnumerable<T>, global::System.Collections.Generic.IReadOnlyCollection<T>, global::System.Collections.Generic.IReadOnlyList<T>, global::System.Collections.Generic.ICollection<T>, global::System.Collections.Generic.IList<T>
{
	int global::System.Collections.ICollection.Count => _items.Length;

	bool global::System.Collections.ICollection.IsSynchronized => false;

	object global::System.Collections.ICollection.SyncRoot => this;

	object? global::System.Collections.IList.this[int index]
	{
		get
		{
			return _items[index];
		}
		set
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			throw new NotSupportedException();
		}
	}

	bool global::System.Collections.IList.IsFixedSize => true;

	bool global::System.Collections.IList.IsReadOnly => true;

	int global::System.Collections.Generic.IReadOnlyCollection<T>.Count => _items.Length;

	T global::System.Collections.Generic.IReadOnlyList<T>.this[int index] => _items[index];

	int global::System.Collections.Generic.ICollection<T>.Count => _items.Length;

	bool global::System.Collections.Generic.ICollection<T>.IsReadOnly => true;

	T global::System.Collections.Generic.IList<T>.this[int index]
	{
		get
		{
			return _items[index];
		}
		set
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			throw new NotSupportedException();
		}
	}

	public <>z__ReadOnlyArray(T[] items)
	{
		_items = items;
	}

	global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
	{
		return ((global::System.Collections.IEnumerable)(object)_items).GetEnumerator();
	}

	void global::System.Collections.ICollection.CopyTo(global::System.Array array, int index)
	{
		((global::System.Collections.ICollection)(object)_items).CopyTo(array, index);
	}

	int global::System.Collections.IList.Add(object? value)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		throw new NotSupportedException();
	}

	void global::System.Collections.IList.Clear()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		throw new NotSupportedException();
	}

	bool global::System.Collections.IList.Contains(object? value)
	{
		return ((global::System.Collections.IList)(object)_items).Contains(value);
	}

	int global::System.Collections.IList.IndexOf(object? value)
	{
		return ((global::System.Collections.IList)(object)_items).IndexOf(value);
	}

	void global::System.Collections.IList.Insert(int index, object? value)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		throw new NotSupportedException();
	}

	void global::System.Collections.IList.Remove(object? value)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		throw new NotSupportedException();
	}

	void global::System.Collections.IList.RemoveAt(int index)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		throw new NotSupportedException();
	}

	global::System.Collections.Generic.IEnumerator<T> global::System.Collections.Generic.IEnumerable<T>.GetEnumerator()
	{
		return ((global::System.Collections.Generic.IEnumerable<T>)_items).GetEnumerator();
	}

	void global::System.Collections.Generic.ICollection<T>.Add(T item)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		throw new NotSupportedException();
	}

	void global::System.Collections.Generic.ICollection<T>.Clear()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		throw new NotSupportedException();
	}

	bool global::System.Collections.Generic.ICollection<T>.Contains(T item)
	{
		return ((global::System.Collections.Generic.ICollection<T>)_items).Contains(item);
	}

	void global::System.Collections.Generic.ICollection<T>.CopyTo(T[] array, int arrayIndex)
	{
		((global::System.Collections.Generic.ICollection<T>)_items).CopyTo(array, arrayIndex);
	}

	bool global::System.Collections.Generic.ICollection<T>.Remove(T item)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		throw new NotSupportedException();
	}

	int global::System.Collections.Generic.IList<T>.IndexOf(T item)
	{
		return ((global::System.Collections.Generic.IList<T>)_items).IndexOf(item);
	}

	void global::System.Collections.Generic.IList<T>.Insert(int index, T item)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		throw new NotSupportedException();
	}

	void global::System.Collections.Generic.IList<T>.RemoveAt(int index)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		throw new NotSupportedException();
	}
}
