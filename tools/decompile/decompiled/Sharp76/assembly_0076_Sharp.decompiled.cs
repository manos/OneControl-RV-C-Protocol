using System;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.Runtime;
using Android.Views;
using Com.Android.VideoCodec;
using Com.Hardware;
using Com.Rearcam;
using Com.Sonix.Decoder;
using Java.Interop;
using Java.Lang;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Layouts;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using SkiaSharp.Views.Maui.Controls.Hosting;
using _Microsoft.Android.Resource.Designer;
using ids.camera.insight.Audio;
using ids.camera.insight.Services.Insight;
using ids.camera.insight.Views.Insight;
using ids.camera.insight.Views.ParkingGrid;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(/*Could not decode attribute arguments.*/)]
[assembly: Preserve]
[assembly: XamlCompilation(/*Could not decode attribute arguments.*/)]
[assembly: XmlnsPrefix("http://lci1.com/schemas/insight", "insight")]
[assembly: XmlnsDefinition("http://lci1.com/schemas/insight", "ids.camera.insight.Views.Camera")]
[assembly: TargetFramework(".NETCoreApp,Version=v9.0", FrameworkDisplayName = ".NET 9.0")]
[assembly: AssemblyCompany("ids.camera.insight")]
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyFileVersion("1.1.0.0")]
[assembly: AssemblyInformationalVersion("1.1.0+1386bddfe8e3d97c4b7c90feb98d1e91162e2289")]
[assembly: AssemblyProduct("ids.camera.insight")]
[assembly: AssemblyTitle("ids.camera.insight")]
[assembly: TargetPlatform("Android35.0")]
[assembly: SupportedOSPlatform("Android21.0")]
[assembly: AssemblyVersion("1.1.0.0")]
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
namespace ids.camera.insight
{
	public static class AppBuilder
	{
		public static MauiAppBuilder UseInsight(this MauiAppBuilder builder)
		{
			ServiceCollectionServiceExtensions.AddSingleton<IInsightServiceInternal>(ServiceCollectionServiceExtensions.AddSingleton<IInsightService>(ServiceCollectionServiceExtensions.AddSingleton<InsightService>(HandlerMauiAppBuilderExtensions.ConfigureMauiHandlers(AppHostBuilderExtensions.UseSkiaSharp(builder).Services, (Action<IMauiHandlersCollection>)delegate(IMauiHandlersCollection handlers)
			{
				MauiHandlersCollectionExtensions.AddHandler(handlers, typeof(InsightView), typeof(InsightViewHandler));
			})), (Func<IServiceProvider, IInsightService>)((IServiceProvider sp) => ServiceProviderServiceExtensions.GetRequiredService<InsightService>(sp))), (Func<IServiceProvider, IInsightServiceInternal>)((IServiceProvider sp) => ServiceProviderServiceExtensions.GetRequiredService<InsightService>(sp)));
			return builder;
		}
	}
	public class Resource : Resource
	{
	}
}
namespace ids.camera.insight.Views.ParkingGrid
{
	internal class ParkingGridView : SKCanvasView
	{
		public static readonly BindableProperty LineWeightProperty = BindableProperty.Create("LineWeight", typeof(double), typeof(ParkingGridView), (object)8.0, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)delegate(BindableObject bindable, object _, object _)
		{
			ParkingGridView obj = bindable as ParkingGridView;
			if (obj != null)
			{
				((SKCanvasView)obj).InvalidateSurface();
			}
		}, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)((BindableObject _, object value) => Math.Max(1.0, (double)value)), (CreateDefaultValueDelegate)null);

		public static readonly BindableProperty AngleProperty = BindableProperty.Create("Angle", typeof(double), typeof(ParkingGridView), (object)45.0, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)delegate(BindableObject bindable, object _, object _)
		{
			ParkingGridView obj = bindable as ParkingGridView;
			if (obj != null)
			{
				((SKCanvasView)obj).InvalidateSurface();
			}
		}, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)((BindableObject _, object value) => Math.Min(360.0, Math.Max(0.0, (double)value))), (CreateDefaultValueDelegate)null);

		public static readonly BindableProperty SkewProperty = BindableProperty.Create("Skew", typeof(double), typeof(ParkingGridView), (object)0.0, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)delegate(BindableObject bindable, object _, object _)
		{
			ParkingGridView obj = bindable as ParkingGridView;
			if (obj != null)
			{
				((SKCanvasView)obj).InvalidateSurface();
			}
		}, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)((BindableObject _, object value) => Math.Min(10.0, Math.Max(-10.0, (double)value))), (CreateDefaultValueDelegate)null);

		public static readonly BindableProperty SXProperty = BindableProperty.Create("SX", typeof(double), typeof(ParkingGridView), (object)0.5, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)delegate(BindableObject bindable, object _, object _)
		{
			ParkingGridView obj = bindable as ParkingGridView;
			if (obj != null)
			{
				((SKCanvasView)obj).InvalidateSurface();
			}
		}, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)((BindableObject _, object value) => Math.Max(0.0, (double)value)), (CreateDefaultValueDelegate)null);

		public static readonly BindableProperty SYProperty = BindableProperty.Create("SY", typeof(double), typeof(ParkingGridView), (object)1.0, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)delegate(BindableObject bindable, object _, object _)
		{
			ParkingGridView obj = bindable as ParkingGridView;
			if (obj != null)
			{
				((SKCanvasView)obj).InvalidateSurface();
			}
		}, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)((BindableObject _, object value) => Math.Max(0.0, (double)value)), (CreateDefaultValueDelegate)null);

		public static readonly BindableProperty DXProperty = BindableProperty.Create("DX", typeof(double), typeof(ParkingGridView), (object)0.5, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)delegate(BindableObject bindable, object _, object _)
		{
			ParkingGridView obj = bindable as ParkingGridView;
			if (obj != null)
			{
				((SKCanvasView)obj).InvalidateSurface();
			}
		}, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)null, (CreateDefaultValueDelegate)null);

		public static readonly BindableProperty DYProperty = BindableProperty.Create("DY", typeof(double), typeof(ParkingGridView), (object)0.5, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)delegate(BindableObject bindable, object _, object _)
		{
			ParkingGridView obj = bindable as ParkingGridView;
			if (obj != null)
			{
				((SKCanvasView)obj).InvalidateSurface();
			}
		}, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)null, (CreateDefaultValueDelegate)null);

		public double LineWeight
		{
			get
			{
				return (double)((BindableObject)this).GetValue(LineWeightProperty);
			}
			set
			{
				((BindableObject)this).SetValue(LineWeightProperty, (object)value);
			}
		}

		public double Angle
		{
			get
			{
				return (double)((BindableObject)this).GetValue(AngleProperty);
			}
			set
			{
				((BindableObject)this).SetValue(AngleProperty, (object)value);
			}
		}

		public double Skew
		{
			get
			{
				return (double)((BindableObject)this).GetValue(SkewProperty);
			}
			set
			{
				((BindableObject)this).SetValue(SkewProperty, (object)value);
			}
		}

		public double SX
		{
			get
			{
				return (double)((BindableObject)this).GetValue(SXProperty);
			}
			set
			{
				((BindableObject)this).SetValue(SXProperty, (object)value);
			}
		}

		public double SY
		{
			get
			{
				return (double)((BindableObject)this).GetValue(SYProperty);
			}
			set
			{
				((BindableObject)this).SetValue(SYProperty, (object)value);
			}
		}

		public double DX
		{
			get
			{
				return (double)((BindableObject)this).GetValue(DXProperty);
			}
			set
			{
				((BindableObject)this).SetValue(DXProperty, (object)value);
			}
		}

		public double DY
		{
			get
			{
				return (double)((BindableObject)this).GetValue(DYProperty);
			}
			set
			{
				((BindableObject)this).SetValue(DYProperty, (object)value);
			}
		}

		protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Expected O, but got Unknown
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			((SKCanvasView)this).OnPaintSurface(e);
			SKCanvas canvas = e.Surface.Canvas;
			SKImageInfo info = e.Info;
			SKSizeI size = ((SKImageInfo)(ref info)).Size;
			int width = ((SKSizeI)(ref size)).Width;
			info = e.Info;
			size = ((SKImageInfo)(ref info)).Size;
			int height = ((SKSizeI)(ref size)).Height;
			float num = (float)((double)width * SX);
			float num2 = (float)((double)height * SY);
			float num3 = (float)((double)width * DX);
			float num4 = (float)((double)(-height) * DY);
			SKRect val = default(SKRect);
			((SKRect)(ref val))..ctor(num * -0.5f + num3, 0f - num2 + num4, num * 0.5f + num3, 0f);
			DisplayInfo mainDisplayInfo = DeviceDisplay.Current.MainDisplayInfo;
			float num5 = (float)((DisplayInfo)(ref mainDisplayInfo)).Density;
			float strokeWidth = (float)LineWeight * num5;
			canvas.Clear(SKColor.Empty);
			canvas.Translate((float)width / 2f, (float)height - num4);
			canvas.Skew((float)Skew, 0f);
			float num6 = (float)Math.Cos((double)(float)(Angle * Math.PI / 180.0));
			canvas.Scale(1f, num6);
			SKPaint val2 = new SKPaint();
			try
			{
				val2.Style = (SKPaintStyle)1;
				val2.Color = SKColors.Green;
				val2.StrokeWidth = strokeWidth;
				val2.IsAntialias = true;
				float num7 = ((SKRect)(ref val)).Height * 0.3f;
				canvas.DrawRect(((SKRect)(ref val)).Left, ((SKRect)(ref val)).Top, ((SKRect)(ref val)).Width, num7, val2);
				canvas.Translate(0f, num7);
				num7 = ((SKRect)(ref val)).Height * 0.4f;
				val2.Color = SKColors.Gold;
				canvas.DrawRect(((SKRect)(ref val)).Left, ((SKRect)(ref val)).Top, ((SKRect)(ref val)).Width, num7, val2);
				canvas.Translate(0f, num7);
				num7 = ((SKRect)(ref val)).Height * 0.3f;
				val2.Color = SKColors.Red;
				canvas.DrawRect(((SKRect)(ref val)).Left, ((SKRect)(ref val)).Top, ((SKRect)(ref val)).Width, num7, val2);
				canvas.Translate(0f, num7);
			}
			finally
			{
				((global::System.IDisposable)val2)?.Dispose();
			}
		}
	}
}
namespace ids.camera.insight.Views.Insight
{
	internal sealed class InsightView : View
	{
		public InsightView()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			((VisualElement)this).Unloaded += (EventHandler)([CompilerGenerated] (object? _, EventArgs _) =>
			{
				IViewHandler handler = ((VisualElement)this).Handler;
				if (handler != null)
				{
					((IElementHandler)handler).DisconnectHandler();
				}
			});
		}
	}
	internal class InsightViewHandler : ViewHandler<InsightView, InsightViewPlatform>
	{
		public InsightViewHandler()
			: base((IPropertyMapper)(object)ViewHandler.ViewMapper, (CommandMapper)(object)ViewHandler.ViewCommandMapper)
		{
		}

		protected override InsightViewPlatform CreatePlatformView()
		{
			return new InsightViewPlatform(base.Context);
		}

		protected override void ConnectHandler(InsightViewPlatform platformView)
		{
			platformView.HandlerWillConnect();
			base.ConnectHandler(platformView);
		}

		protected override void DisconnectHandler(InsightViewPlatform platformView)
		{
			platformView.HandlerWillDisconnect();
			base.DisconnectHandler(platformView);
		}
	}
	internal class InsightViewPlatform : SurfaceView, ISurfaceHolderCallback, IJavaObject, global::System.IDisposable, IJavaPeerable
	{
		private readonly ILogger _logger;

		private readonly IInsightServiceInternal _insightService;

		private double _aspectRatio;

		private global::System.IDisposable? _disposable;

		private Surface? _surface;

		public InsightViewPlatform(Context context)
			: base(context)
		{
			_logger = (ILogger)(object)LoggerFactoryExtensions.CreateLogger<InsightViewPlatform>(ServiceProviderServiceExtensions.GetRequiredService<ILoggerFactory>(((Element)Application.Current).Handler.MauiContext.Services));
			_insightService = ServiceProviderServiceExtensions.GetRequiredService<IInsightServiceInternal>(((Element)Application.Current).Handler.MauiContext.Services);
		}

		public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height)
		{
			if (_surface == null && holder.Surface != null)
			{
				_surface = holder.Surface;
				_insightService.RegisterSurface(_surface);
			}
		}

		public void SurfaceCreated(ISurfaceHolder holder)
		{
		}

		public void SurfaceDestroyed(ISurfaceHolder holder)
		{
			if (_surface != null)
			{
				_insightService.UnregisterSurface(_surface);
				_surface = null;
			}
		}

		internal void HandlerWillConnect()
		{
			Register();
		}

		internal void HandlerWillDisconnect()
		{
			Unregister();
		}

		private void Register()
		{
			Unregister();
			ISurfaceHolder holder = ((SurfaceView)this).Holder;
			if (holder != null)
			{
				holder.AddCallback((ISurfaceHolderCallback)(object)this);
			}
			_aspectRatio = _insightService.AspectRatio;
			_disposable = ObservableExtensions.Subscribe<EventPattern<double>>(Observable.FromEventPattern<double>((Action<EventHandler<double>>)([CompilerGenerated] (EventHandler<double> h) =>
			{
				_insightService.OnAspectRatio += h;
			}), (Action<EventHandler<double>>)([CompilerGenerated] (EventHandler<double> h) =>
			{
				_insightService.OnAspectRatio -= h;
			})), (Action<EventPattern<double>>)([CompilerGenerated] (EventPattern<double> ep) =>
			{
				_aspectRatio = ((EventPattern<object, double>)(object)ep).EventArgs;
				((View)this).Invalidate();
			}));
		}

		private void Unregister()
		{
			ISurfaceHolder holder = ((SurfaceView)this).Holder;
			if (holder != null)
			{
				holder.RemoveCallback((ISurfaceHolderCallback)(object)this);
			}
			try
			{
				_disposable?.Dispose();
			}
			catch
			{
			}
		}
	}
}
namespace ids.camera.insight.Views.Camera
{
	public class CameraView : Layout, ILayoutManager
	{
		private double _aspectRatio;

		private const string PipAnimationName = "PipAnimation";

		private readonly ILogger _logger;

		private readonly IInsightServiceInternal _insightService;

		private readonly PinchGestureRecognizer _pinchGestureRecognizer = new PinchGestureRecognizer();

		private readonly PanGestureRecognizer _panGestureRecognizer = new PanGestureRecognizer();

		private readonly TapGestureRecognizer _doubleTapGestureRecognizer = new TapGestureRecognizer
		{
			NumberOfTapsRequired = 2
		};

		private readonly InsightView _primaryCamView;

		private readonly InsightView _pip;

		private readonly ParkingGridView _parkingGridView;

		public static readonly BindableProperty AspectFillProperty = BindableProperty.Create("AspectFill", typeof(bool), typeof(CameraView), (object)false, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)delegate(BindableObject bindable, object _, object _)
		{
			CameraView obj = bindable as CameraView;
			if (obj != null)
			{
				((VisualElement)obj).InvalidateMeasure();
			}
		}, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)null, (CreateDefaultValueDelegate)null);

		public static readonly BindableProperty PipEnabledProperty = BindableProperty.Create("PipEnabled", typeof(bool), typeof(CameraView), (object)true, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)delegate(BindableObject bindable, object _, object _)
		{
			CameraView obj = bindable as CameraView;
			if (obj != null)
			{
				((VisualElement)obj).InvalidateMeasure();
			}
		}, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)null, (CreateDefaultValueDelegate)null);

		public static readonly BindableProperty PipScaleProperty = BindableProperty.Create("PipScale", typeof(double), typeof(CameraView), (object)0.33, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)delegate(BindableObject bindable, object _, object _)
		{
			CameraView obj = bindable as CameraView;
			if (obj != null)
			{
				((VisualElement)obj).InvalidateMeasure();
			}
		}, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)((BindableObject _, object value) => Math.Min(1.0, Math.Max(0.0, (double)value))), (CreateDefaultValueDelegate)null);

		public static readonly BindableProperty PipLeftMarginProperty = BindableProperty.Create("PipLeftMargin", typeof(double), typeof(CameraView), (object)16.0, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)delegate(BindableObject bindable, object _, object _)
		{
			CameraView obj = bindable as CameraView;
			if (obj != null)
			{
				((VisualElement)obj).InvalidateMeasure();
			}
		}, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)null, (CreateDefaultValueDelegate)null);

		public static readonly BindableProperty PipTopMarginProperty = BindableProperty.Create("PipTopMargin", typeof(double), typeof(CameraView), (object)16.0, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)delegate(BindableObject bindable, object _, object _)
		{
			CameraView obj = bindable as CameraView;
			if (obj != null)
			{
				((VisualElement)obj).InvalidateMeasure();
			}
		}, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)null, (CreateDefaultValueDelegate)null);

		public static readonly BindableProperty ShowParkingGuideProperty = BindableProperty.Create("ShowParkingGuide", typeof(bool), typeof(CameraView), (object)true, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)delegate(BindableObject bindable, object _, object value)
		{
			CameraView cameraView = bindable as CameraView;
			bool isVisible = (bool)value;
			if (cameraView != null)
			{
				((VisualElement)cameraView._parkingGridView).IsVisible = isVisible;
			}
		}, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)null, (CreateDefaultValueDelegate)null);

		public static readonly BindableProperty LineWeightProperty = BindableProperty.Create("LineWeight", typeof(double), typeof(CameraView), ParkingGridView.LineWeightProperty.DefaultValue, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)null, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)null, (CreateDefaultValueDelegate)null);

		public static readonly BindableProperty AngleProperty = BindableProperty.Create("Angle", typeof(double), typeof(CameraView), ParkingGridView.LineWeightProperty.DefaultValue, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)null, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)null, (CreateDefaultValueDelegate)null);

		public static readonly BindableProperty SkewProperty = BindableProperty.Create("Skew", typeof(double), typeof(CameraView), ParkingGridView.SkewProperty.DefaultValue, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)null, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)null, (CreateDefaultValueDelegate)null);

		public static readonly BindableProperty SXProperty = BindableProperty.Create("SX", typeof(double), typeof(CameraView), ParkingGridView.SXProperty.DefaultValue, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)null, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)null, (CreateDefaultValueDelegate)null);

		public static readonly BindableProperty SYProperty = BindableProperty.Create("SY", typeof(double), typeof(CameraView), ParkingGridView.SYProperty.DefaultValue, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)null, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)null, (CreateDefaultValueDelegate)null);

		public static readonly BindableProperty DXProperty = BindableProperty.Create("DX", typeof(double), typeof(CameraView), ParkingGridView.DXProperty.DefaultValue, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)null, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)null, (CreateDefaultValueDelegate)null);

		public static readonly BindableProperty DYProperty = BindableProperty.Create("DY", typeof(double), typeof(CameraView), ParkingGridView.DYProperty.DefaultValue, (BindingMode)2, (ValidateValueDelegate)null, (BindingPropertyChangedDelegate)null, (BindingPropertyChangingDelegate)null, (CoerceValueDelegate)null, (CreateDefaultValueDelegate)null);

		private double _startScale;

		private double _startTranslationX;

		private double _startTranslationY;

		public bool AspectFill
		{
			get
			{
				return (bool)((BindableObject)this).GetValue(AspectFillProperty);
			}
			set
			{
				((BindableObject)this).SetValue(AspectFillProperty, (object)value);
			}
		}

		public bool PipEnabled
		{
			get
			{
				return (bool)((BindableObject)this).GetValue(PipEnabledProperty);
			}
			set
			{
				((BindableObject)this).SetValue(PipEnabledProperty, (object)value);
			}
		}

		public double PipScale
		{
			get
			{
				return (double)((BindableObject)this).GetValue(PipScaleProperty);
			}
			set
			{
				((BindableObject)this).SetValue(PipScaleProperty, (object)value);
			}
		}

		public double PipLeftMargin
		{
			get
			{
				return (double)((BindableObject)this).GetValue(PipLeftMarginProperty);
			}
			set
			{
				((BindableObject)this).SetValue(PipLeftMarginProperty, (object)value);
			}
		}

		public double PipTopMargin
		{
			get
			{
				return (double)((BindableObject)this).GetValue(PipTopMarginProperty);
			}
			set
			{
				((BindableObject)this).SetValue(PipTopMarginProperty, (object)value);
			}
		}

		public bool ShowParkingGuide
		{
			get
			{
				return (bool)((BindableObject)this).GetValue(ShowParkingGuideProperty);
			}
			set
			{
				((BindableObject)this).SetValue(ShowParkingGuideProperty, (object)value);
			}
		}

		public double LineWeight
		{
			get
			{
				return (double)((BindableObject)this).GetValue(LineWeightProperty);
			}
			set
			{
				((BindableObject)this).SetValue(LineWeightProperty, (object)value);
			}
		}

		public double Angle
		{
			get
			{
				return (double)((BindableObject)this).GetValue(AngleProperty);
			}
			set
			{
				((BindableObject)this).SetValue(AngleProperty, (object)value);
			}
		}

		public double Skew
		{
			get
			{
				return (double)((BindableObject)this).GetValue(SkewProperty);
			}
			set
			{
				((BindableObject)this).SetValue(SkewProperty, (object)value);
			}
		}

		public double SX
		{
			get
			{
				return (double)((BindableObject)this).GetValue(SXProperty);
			}
			set
			{
				((BindableObject)this).SetValue(SXProperty, (object)value);
			}
		}

		public double SY
		{
			get
			{
				return (double)((BindableObject)this).GetValue(SYProperty);
			}
			set
			{
				((BindableObject)this).SetValue(SYProperty, (object)value);
			}
		}

		public double DX
		{
			get
			{
				return (double)((BindableObject)this).GetValue(DXProperty);
			}
			set
			{
				((BindableObject)this).SetValue(DXProperty, (object)value);
			}
		}

		public double DY
		{
			get
			{
				return (double)((BindableObject)this).GetValue(DYProperty);
			}
			set
			{
				((BindableObject)this).SetValue(DYProperty, (object)value);
			}
		}

		public CameraView()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Expected O, but got Unknown
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Expected O, but got Unknown
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Expected O, but got Unknown
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Expected O, but got Unknown
			//IL_025b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Expected O, but got Unknown
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0289: Expected O, but got Unknown
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Expected O, but got Unknown
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			InsightView insightView = new InsightView();
			((VisualElement)insightView).InputTransparent = true;
			_primaryCamView = insightView;
			InsightView insightView2 = new InsightView();
			((VisualElement)insightView2).InputTransparent = true;
			_pip = insightView2;
			ParkingGridView parkingGridView = new ParkingGridView();
			((VisualElement)parkingGridView).InputTransparent = true;
			_parkingGridView = parkingGridView;
			_startScale = 1.0;
			((Layout)this)..ctor();
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
					obj = ((mauiContext != null) ? mauiContext.Services : null);
				}
			}
			if (obj == null)
			{
				throw new ApplicationException("No instance of MAUI Application was found.");
			}
			IServiceProvider val = (IServiceProvider)obj;
			_logger = (ILogger)(object)LoggerFactoryExtensions.CreateLogger<InsightViewPlatform>(ServiceProviderServiceExtensions.GetRequiredService<ILoggerFactory>(val));
			_insightService = ServiceProviderServiceExtensions.GetRequiredService<IInsightServiceInternal>(val);
			_insightService.OnAspectRatio += OnAspectRatio;
			_aspectRatio = _insightService.AspectRatio;
			ObservableExtensions.Subscribe<EventPattern<PinchGestureUpdatedEventArgs>>(Observable.FromEventPattern<PinchGestureUpdatedEventArgs>((Action<EventHandler<PinchGestureUpdatedEventArgs>>)([CompilerGenerated] (EventHandler<PinchGestureUpdatedEventArgs> h) =>
			{
				_pinchGestureRecognizer.PinchUpdated += h;
			}), (Action<EventHandler<PinchGestureUpdatedEventArgs>>)([CompilerGenerated] (EventHandler<PinchGestureUpdatedEventArgs> h) =>
			{
				_pinchGestureRecognizer.PinchUpdated -= h;
			})), (Action<EventPattern<PinchGestureUpdatedEventArgs>>)([CompilerGenerated] (EventPattern<PinchGestureUpdatedEventArgs> ep) =>
			{
				OnPinchGesture(((EventPattern<object, PinchGestureUpdatedEventArgs>)(object)ep).Sender, ((EventPattern<object, PinchGestureUpdatedEventArgs>)(object)ep).EventArgs);
			}));
			ObservableExtensions.Subscribe<EventPattern<PanUpdatedEventArgs>>(Observable.FromEventPattern<PanUpdatedEventArgs>((Action<EventHandler<PanUpdatedEventArgs>>)([CompilerGenerated] (EventHandler<PanUpdatedEventArgs> h) =>
			{
				_panGestureRecognizer.PanUpdated += h;
			}), (Action<EventHandler<PanUpdatedEventArgs>>)([CompilerGenerated] (EventHandler<PanUpdatedEventArgs> h) =>
			{
				_panGestureRecognizer.PanUpdated -= h;
			})), (Action<EventPattern<PanUpdatedEventArgs>>)([CompilerGenerated] (EventPattern<PanUpdatedEventArgs> ep) =>
			{
				OnPanGesture(((EventPattern<object, PanUpdatedEventArgs>)(object)ep).Sender, ((EventPattern<object, PanUpdatedEventArgs>)(object)ep).EventArgs);
			}));
			ObservableExtensions.Subscribe<EventPattern<TappedEventArgs>>(Observable.FromEventPattern<TappedEventArgs>((Action<EventHandler<TappedEventArgs>>)([CompilerGenerated] (EventHandler<TappedEventArgs> h) =>
			{
				_doubleTapGestureRecognizer.Tapped += h;
			}), (Action<EventHandler<TappedEventArgs>>)([CompilerGenerated] (EventHandler<TappedEventArgs> h) =>
			{
				_doubleTapGestureRecognizer.Tapped -= h;
			})), (Action<EventPattern<TappedEventArgs>>)([CompilerGenerated] (EventPattern<TappedEventArgs> ep) =>
			{
				ResetPanAndZoom();
			}));
			((global::System.Collections.Generic.ICollection<IGestureRecognizer>)((View)this).GestureRecognizers).Add((IGestureRecognizer)(object)_pinchGestureRecognizer);
			((global::System.Collections.Generic.ICollection<IGestureRecognizer>)((View)this).GestureRecognizers).Add((IGestureRecognizer)(object)_panGestureRecognizer);
			((global::System.Collections.Generic.ICollection<IGestureRecognizer>)((View)this).GestureRecognizers).Add((IGestureRecognizer)(object)_doubleTapGestureRecognizer);
			((BindableObject)_parkingGridView).SetBinding(ParkingGridView.LineWeightProperty, (BindingBase)new Binding(LineWeightProperty.PropertyName, (BindingMode)0, (IValueConverter)null, (object)null, (string)null, (object)this));
			((BindableObject)_parkingGridView).SetBinding(ParkingGridView.AngleProperty, (BindingBase)new Binding(AngleProperty.PropertyName, (BindingMode)0, (IValueConverter)null, (object)null, (string)null, (object)this));
			((BindableObject)_parkingGridView).SetBinding(ParkingGridView.SkewProperty, (BindingBase)new Binding(SkewProperty.PropertyName, (BindingMode)0, (IValueConverter)null, (object)null, (string)null, (object)this));
			((BindableObject)_parkingGridView).SetBinding(ParkingGridView.DXProperty, (BindingBase)new Binding(DXProperty.PropertyName, (BindingMode)0, (IValueConverter)null, (object)null, (string)null, (object)this));
			((BindableObject)_parkingGridView).SetBinding(ParkingGridView.DYProperty, (BindingBase)new Binding(DYProperty.PropertyName, (BindingMode)0, (IValueConverter)null, (object)null, (string)null, (object)this));
			((BindableObject)_parkingGridView).SetBinding(ParkingGridView.SXProperty, (BindingBase)new Binding(SXProperty.PropertyName, (BindingMode)0, (IValueConverter)null, (object)null, (string)null, (object)this));
			((BindableObject)_parkingGridView).SetBinding(ParkingGridView.SYProperty, (BindingBase)new Binding(SYProperty.PropertyName, (BindingMode)0, (IValueConverter)null, (object)null, (string)null, (object)this));
			((global::System.Collections.Generic.ICollection<IView>)((Layout)this).Children).Add((IView)(object)_primaryCamView);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)this).Children).Add((IView)(object)_pip);
			((global::System.Collections.Generic.ICollection<IView>)((Layout)this).Children).Add((IView)(object)_parkingGridView);
			((VisualElement)this).BackgroundColor = Colors.Black;
			((Layout)this).IsClippedToBounds = true;
		}

		private void InvalidateLayout()
		{
			((IView)this).InvalidateArrange();
		}

		protected override ILayoutManager CreateLayoutManager()
		{
			return (ILayoutManager)(object)this;
		}

		public Size Measure(double widthConstraint, double heightConstraint)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			return new Size(double.IsPositiveInfinity(widthConstraint) ? 1.7976931348623157E+308 : widthConstraint, double.IsPositiveInfinity(heightConstraint) ? 1.7976931348623157E+308 : heightConstraint);
		}

		public Size ArrangeChildren(Rect bounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			Size size = GetSize(((Rect)(ref bounds)).Size, _aspectRatio);
			((IView)_primaryCamView).Arrange(new Rect(new Point(((Rect)(ref bounds)).X + (((Rect)(ref bounds)).Width - ((Size)(ref size)).Width) / 2.0, ((Rect)(ref bounds)).Y + (((Rect)(ref bounds)).Height - ((Size)(ref size)).Height) / 2.0), size));
			if (AspectFill)
			{
				((VisualElement)_primaryCamView).Scale = Math.Max(((Rect)(ref bounds)).Width / ((VisualElement)_primaryCamView).Width, ((Rect)(ref bounds)).Height / ((VisualElement)_primaryCamView).Height);
				((VisualElement)_parkingGridView).Scale = ((VisualElement)_primaryCamView).Scale;
			}
			if (ShowParkingGuide)
			{
				((IView)_parkingGridView).Arrange(new Rect(new Point(((Rect)(ref bounds)).X + (((Rect)(ref bounds)).Width - ((Size)(ref size)).Width) / 2.0, ((Rect)(ref bounds)).Y + (((Rect)(ref bounds)).Height - ((Size)(ref size)).Height) / 2.0), size));
			}
			HideShowPip();
			if (((VisualElement)_pip).IsVisible)
			{
				Size size2 = GetSize(new Size(PipScale * ((Rect)(ref bounds)).Width, PipScale * ((Rect)(ref bounds)).Height), _aspectRatio);
				((IView)_pip).Arrange(new Rect(new Point(((Rect)(ref bounds)).X + PipLeftMargin, ((Rect)(ref bounds)).Y + PipTopMargin), size2));
			}
			return ((Rect)(ref bounds)).Size;
		}

		public void OnAspectRatio(object? sender, double aspectRatio)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected O, but got Unknown
			LoggerExtensions.LogDebug(_logger, "Camera Aspect Ratio Changed {0}", new object[1] { aspectRatio });
			_aspectRatio = aspectRatio;
			((BindableObject)this).Dispatcher.Dispatch(new Action(InvalidateLayout));
		}

		protected override void OnSizeAllocated(double width, double height)
		{
			ResetPanAndZoom();
			((VisualElement)this).OnSizeAllocated(width, height);
		}

		protected override void OnChildAdded(Element child)
		{
			if ((object)child == _primaryCamView || (object)child == _pip || (object)child == _parkingGridView)
			{
				((VisualElement)this).OnChildAdded(child);
			}
		}

		public void OnPinchGesture(object? sender, PinchGestureUpdatedEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Invalid comparison between Unknown and I4
			if ((int)e.Status == 0)
			{
				_startScale = ((VisualElement)_primaryCamView).Scale;
				_startTranslationX = ((VisualElement)_primaryCamView).TranslationX;
				_startTranslationY = ((VisualElement)_primaryCamView).TranslationY;
			}
			if ((int)e.Status == 1)
			{
				double scale = ((VisualElement)_primaryCamView).Scale;
				scale += (e.Scale - 1.0) * _startScale;
				scale = Math.Max(1.0, scale);
				((VisualElement)_parkingGridView).Scale = scale;
				((VisualElement)_primaryCamView).Scale = scale;
				HideShowPip();
				double translation = GetTranslation(((VisualElement)_primaryCamView).Width * ((VisualElement)_primaryCamView).Scale, ((VisualElement)this).Width, _startTranslationX, 0.0);
				double translation2 = GetTranslation(((VisualElement)_primaryCamView).Height * ((VisualElement)_primaryCamView).Scale, ((VisualElement)this).Height, _startTranslationY, 0.0);
				((VisualElement)_parkingGridView).TranslationX = translation;
				((VisualElement)_parkingGridView).TranslationY = translation2;
				((VisualElement)_primaryCamView).TranslationX = translation;
				((VisualElement)_primaryCamView).TranslationY = translation2;
			}
		}

		private void OnPanGesture(object? sender, PanUpdatedEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Invalid comparison between Unknown and I4
			if ((int)e.StatusType == 0)
			{
				_startTranslationX = ((VisualElement)_primaryCamView).TranslationX;
				_startTranslationY = ((VisualElement)_primaryCamView).TranslationY;
			}
			if ((int)e.StatusType == 1)
			{
				double translation = GetTranslation(((VisualElement)_primaryCamView).Width * ((VisualElement)_primaryCamView).Scale, ((VisualElement)this).Width, _startTranslationX, e.TotalX);
				double translation2 = GetTranslation(((VisualElement)_primaryCamView).Height * ((VisualElement)_primaryCamView).Scale, ((VisualElement)this).Height, _startTranslationY, e.TotalY);
				((VisualElement)_parkingGridView).TranslationX = translation;
				((VisualElement)_parkingGridView).TranslationY = translation2;
				((VisualElement)_primaryCamView).TranslationX = translation;
				((VisualElement)_primaryCamView).TranslationY = translation2;
			}
		}

		private void ResetPanAndZoom()
		{
			((VisualElement)_parkingGridView).Scale = 1.0;
			((VisualElement)_parkingGridView).TranslationX = 0.0;
			((VisualElement)_parkingGridView).TranslationY = 0.0;
			((VisualElement)_primaryCamView).Scale = 1.0;
			((VisualElement)_primaryCamView).TranslationX = 0.0;
			((VisualElement)_primaryCamView).TranslationY = 0.0;
			HideShowPip();
		}

		private void HideShowPip()
		{
			if (PipEnabled && ((VisualElement)_primaryCamView).Scale > 1.0)
			{
				((VisualElement)_pip).IsVisible = true;
			}
			else
			{
				((VisualElement)_pip).IsVisible = false;
			}
		}

		private static double GetTranslation(double childSize, double parentSize, double translation, double deltaTranslation)
		{
			if (childSize <= parentSize)
			{
				return 0.0;
			}
			double num = translation + deltaTranslation;
			double num2 = (childSize - parentSize) / 2.0;
			double num3 = 0.0 - num2 + num;
			if (num3 > 0.0)
			{
				num = num2;
			}
			if (num3 + childSize < parentSize)
			{
				num = 0.0 - num2;
			}
			return num;
		}

		private static Size GetSize(Size parent, double aspectRatio)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			if (((Size)(ref parent)).Width == 0.0 || ((Size)(ref parent)).Height == 0.0)
			{
				return Size.Zero;
			}
			double num = ((Size)(ref parent)).Width / ((Size)(ref parent)).Height;
			double num2 = ((Size)(ref parent)).Width;
			double num3 = ((Size)(ref parent)).Height;
			if (aspectRatio > num)
			{
				num3 = ((Size)(ref parent)).Width / aspectRatio;
			}
			else if (aspectRatio < num)
			{
				num2 = ((Size)(ref parent)).Height * aspectRatio;
			}
			return new Size(num2, num3);
		}
	}
}
namespace ids.camera.insight.Services.Insight
{
	public enum ConnectResult
	{
		Success,
		InvalidPassword,
		CameraUnavailable,
		Failed,
		Cancelled,
		Timeout
	}
	public enum DisconnectReason
	{
		Normal,
		PasswordSet,
		ChangedRFMode,
		EnteredPairingMode,
		VideoLost,
		HeartbeatFailed,
		ConnectionLost
	}
	[Flags]
	public enum CameraMirrorFlip
	{
		None = 0,
		Mirror = 1,
		Flip = 2,
		MirrorFlip = 3
	}
	internal enum VideoStoppedReason : byte
	{
		ResourceUnavailable = 1,
		CertificateError
	}
	internal enum StatusCode : byte
	{
		CommandOk = 0,
		VideoStopped = 1,
		FrameOk = 7,
		FwUpdateOk = 21,
		FwUpdateFail = 22,
		MirrorFlip = 48,
		Brightness = 128,
		Chroma = 129,
		Contrast = 130,
		Saturation = 131,
		ChangeRFMode = 142,
		AddVolumeCtl = 140,
		PushTalkStatus = 141,
		SetPassword = 144,
		EnterPairingMode = 145,
		GetResolution = 146
	}
	public interface IInsightService
	{
		double AspectRatio { get; }

		bool Connected { get; }

		bool Mute { get; set; }

		event EventHandler<CameraDisconnectedEventArgs>? OnCameraDisconnected;

		event EventHandler<double>? OnAspectRatio;

		event EventHandler<byte>? OnBrightness;

		event EventHandler<byte>? OnChroma;

		event EventHandler<byte>? OnContrast;

		event EventHandler<byte>? OnSaturation;

		event EventHandler<byte>? OnMirrorFlip;

		global::System.Threading.Tasks.Task<ConnectResult> ConnectAsync(string password, CancellationToken ct, uint videoFrameTimeoutMs = 1000u, uint heartbeatTimeoutMs = 5000u);

		void Disconnect();

		global::System.Threading.Tasks.Task<bool> SetPassword(string password, CancellationToken ct);

		global::System.Threading.Tasks.Task<bool> ChangeRFModeAsync(CancellationToken ct);

		global::System.Threading.Tasks.Task<bool> EnterPairingModeAsync(CancellationToken ct);

		global::System.Threading.Tasks.Task<Version> GetFirmwareVersionAsync(CancellationToken ct);

		global::System.Threading.Tasks.Task<byte> GetBrightnessAsync(CancellationToken ct);

		global::System.Threading.Tasks.Task<bool> SetBrightnessAsync(byte value, CancellationToken ct);

		global::System.Threading.Tasks.Task<byte> GetChromaAsync(CancellationToken ct);

		global::System.Threading.Tasks.Task<bool> SetChromaAsync(byte value, CancellationToken ct);

		global::System.Threading.Tasks.Task<byte> GetContrastAsync(CancellationToken ct);

		global::System.Threading.Tasks.Task<bool> SetContrastAsync(byte value, CancellationToken ct);

		global::System.Threading.Tasks.Task<byte> GetSaturationAsync(CancellationToken ct);

		global::System.Threading.Tasks.Task<bool> SetSaturationAsync(byte value, CancellationToken ct);

		global::System.Threading.Tasks.Task<byte> GetMirrorFlipAsync(CancellationToken ct);

		global::System.Threading.Tasks.Task<bool> SetMirrorFlipAsync(CameraMirrorFlip value, CancellationToken ct);

		global::System.Threading.Tasks.Task<double> GetAspectRatio(CancellationToken ct);
	}
	public class CameraDisconnectedEventArgs : EventArgs
	{
		[field: CompilerGenerated]
		public DisconnectReason Reason
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			init;
		}
	}
	internal interface IInsightServiceInternal : IInsightService
	{
		event EventHandler<HeartbeatEventArgs>? OnHeartbeat;

		event EventHandler<HeartbeatErrorEventArgs>? OnHeartbeatError;

		event EventHandler<VideoFrameReceivedEventArgs>? OnVideoFrameReceived;

		event EventHandler<StatusCodeEventArgs>? OnStatusCode;

		void RegisterSurface(Surface surface);

		void UnregisterSurface(Surface surface);

		bool Connect(string password, uint heartbeatTimeoutMs);

		new void Disconnect();

		void UnmuteAudio();

		void MuteAudio();

		void GetParameter(byte parameter);

		void SetPassword(string password);

		void SetParameter(byte parameter, byte[] data);

		void SetParameter(byte parameter, byte value);
	}
	[RequiredMember]
	internal class HeartbeatEventArgs : EventArgs
	{
		[RequiredMember]
		[field: CompilerGenerated]
		public byte[] Data
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			init;
		}

		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public HeartbeatEventArgs()
		{
		}
	}
	internal class HeartbeatErrorEventArgs : EventArgs
	{
	}
	internal class VideoFrameReceivedEventArgs : EventArgs
	{
	}
	[RequiredMember]
	internal class StatusCodeEventArgs : EventArgs
	{
		[RequiredMember]
		[field: CompilerGenerated]
		public StatusCode StatusCode
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			init;
		}

		[RequiredMember]
		[field: CompilerGenerated]
		public byte[] Data
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			init;
		}

		[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
		[CompilerFeatureRequired("RequiredMembers")]
		public StatusCodeEventArgs()
		{
		}
	}
	internal class InsightService : IInsightServiceInternal, IInsightService
	{
		private class DecoderNotify : Object, INotify, IJavaObject, global::System.IDisposable, IJavaPeerable
		{
			private readonly Surface _surface;

			[CompilerGenerated]
			private EventHandler<DecoderErrorEventArgs>? m_OnDecoderError;

			public event EventHandler<DecoderErrorEventArgs>? OnDecoderError
			{
				[CompilerGenerated]
				add
				{
					EventHandler<DecoderErrorEventArgs> val = this.m_OnDecoderError;
					EventHandler<DecoderErrorEventArgs> val2;
					do
					{
						val2 = val;
						EventHandler<DecoderErrorEventArgs> val3 = (EventHandler<DecoderErrorEventArgs>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
						val = Interlocked.CompareExchange<EventHandler<DecoderErrorEventArgs>>(ref this.m_OnDecoderError, val3, val2);
					}
					while (val != val2);
				}
				[CompilerGenerated]
				remove
				{
					EventHandler<DecoderErrorEventArgs> val = this.m_OnDecoderError;
					EventHandler<DecoderErrorEventArgs> val2;
					do
					{
						val2 = val;
						EventHandler<DecoderErrorEventArgs> val3 = (EventHandler<DecoderErrorEventArgs>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
						val = Interlocked.CompareExchange<EventHandler<DecoderErrorEventArgs>>(ref this.m_OnDecoderError, val3, val2);
					}
					while (val != val2);
				}
			}

			public DecoderNotify(Surface surface)
			{
				_surface = surface;
			}

			public void OnNotify(int p0)
			{
				if (-100 == p0)
				{
					this.OnDecoderError?.Invoke((object)this, new DecoderErrorEventArgs(_surface));
				}
			}
		}

		private class EmptyCallback : Object, ICallback, IJavaObject, global::System.IDisposable, IJavaPeerable
		{
			public void OnAudio(long p0, byte[]? p1)
			{
			}

			public void OnFWInfo(byte[]? p0)
			{
			}

			public void OnHeartBeatEvent(int p0)
			{
			}

			public void OnNotify(int p0, byte[]? p1)
			{
			}

			public void OnUpdateFWPercent(int p0, int p1)
			{
			}

			public void OnVideo(long p0, byte[]? p1)
			{
			}
		}

		private class RealCamCallback : Object, ICallback, IJavaObject, global::System.IDisposable, IJavaPeerable
		{
			private readonly ILogger _logger;

			private readonly Action<long, byte[]>? _onVideo;

			private readonly Action<long, byte[]>? _onAudio;

			private readonly Action<byte[]>? _onHeartbeat;

			private readonly Action? _onHeartbeatError;

			private readonly Action<StatusCode, byte[]>? _onStatusCode;

			internal RealCamCallback(ILogger logger, Action<long, byte[]> onVideo, Action<long, byte[]> onAudio, Action<byte[]> onHeartbeat, Action onHeartbeatError, Action<StatusCode, byte[]> onStatusCode)
			{
				_logger = logger;
				_onVideo = onVideo;
				_onAudio = onAudio;
				_onHeartbeat = onHeartbeat;
				_onHeartbeatError = onHeartbeatError;
				_onStatusCode = onStatusCode;
			}

			public RealCamCallback(nint handle, JniHandleOwnership transfer)
				: base((global::System.IntPtr)handle, transfer)
			{
			}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


			public void OnVideo(long p0, byte[]? p1)
			{
				_onVideo?.Invoke(p0, p1);
			}

			public void OnAudio(long p0, byte[]? p1)
			{
				_onAudio?.Invoke(p0, p1);
			}

			public void OnNotify(int p0, byte[]? p1)
			{
				StatusCode statusCode = (StatusCode)p0;
				LoggerExtensions.LogDebug(_logger, "{0}: code={1}, data={{ {2} }}", new object[3]
				{
					"OnNotify",
					statusCode,
					BitConverter.ToString(p1)
				});
				_onStatusCode?.Invoke(statusCode, p1);
			}

			public void OnFWInfo(byte[]? p0)
			{
				_onHeartbeat?.Invoke(p0);
			}

			public void OnHeartBeatEvent(int p0)
			{
				LoggerExtensions.LogError(_logger, "Error receiving heartbeat: {0}", new object[1] { p0 });
				Action? onHeartbeatError = _onHeartbeatError;
				if (onHeartbeatError != null)
				{
					onHeartbeatError.Invoke();
				}
			}

			public void OnUpdateFWPercent(int p0, int p1)
			{
			}
		}

		private class DecoderErrorEventArgs : EventArgs
		{
			[field: CompilerGenerated]
			public Surface Surface
			{
				[CompilerGenerated]
				get;
			}

			internal DecoderErrorEventArgs(Surface surface)
			{
				Surface = surface;
			}
		}

		[CompilerGenerated]
		private sealed class <>c__DisplayClass107_0
		{
			public Timer videoWatchdog;

			internal void <ConnectAsync>b__13(ElapsedEventHandler h)
			{
				videoWatchdog.Elapsed += h;
			}

			internal void <ConnectAsync>b__14(ElapsedEventHandler h)
			{
				videoWatchdog.Elapsed -= h;
			}

			internal void <ConnectAsync>b__18(EventPattern<VideoFrameReceivedEventArgs> ep)
			{
				videoWatchdog.Stop();
				videoWatchdog.Start();
			}
		}

		[CompilerGenerated]
		private sealed class <>c__DisplayClass53_0
		{
			public InsightService <>4__this;

			public CancellationToken connectionCt;

			public Action<global::System.Threading.Tasks.Task<double>> <>9__1;

			internal void <ConnectAsync>b__0(global::System.Threading.Tasks.Task<Version> task)
			{
				//IL_0057: Unknown result type (might be due to invalid IL or missing references)
				Version result = task.Result;
				LoggerExtensions.LogInformation((ILogger)(object)<>4__this._logger, "{0}: Firmware Version = {1}", new object[2]
				{
					"ConnectAsync",
					((object)result).ToString()
				});
				if (result.Major >= 21 || (result.Major == 21 && result.Minor >= 3))
				{
					<>4__this.GetAspectRatio(connectionCt).ContinueWith((Action<global::System.Threading.Tasks.Task<double>>)delegate(global::System.Threading.Tasks.Task<double> task2)
					{
						<>4__this.AspectRatio = task2.Result;
						LoggerExtensions.LogInformation((ILogger)(object)<>4__this._logger, "{0}: Aspect Ratio = {1}", new object[2] { "ConnectAsync", <>4__this.AspectRatio });
						<>4__this.OnAspectRatio?.Invoke((object)<>4__this, <>4__this.AspectRatio);
					});
				}
				else
				{
					LoggerExtensions.LogDebug((ILogger)(object)<>4__this._logger, "Aspect Ratio is not supported by this Firmware {0}", new object[1] { ((object)result).ToString() });
				}
			}

			internal void <ConnectAsync>b__1(global::System.Threading.Tasks.Task<double> task)
			{
				<>4__this.AspectRatio = task.Result;
				LoggerExtensions.LogInformation((ILogger)(object)<>4__this._logger, "{0}: Aspect Ratio = {1}", new object[2] { "ConnectAsync", <>4__this.AspectRatio });
				<>4__this.OnAspectRatio?.Invoke((object)<>4__this, <>4__this.AspectRatio);
			}
		}

		[StructLayout((LayoutKind)3)]
		[CompilerGenerated]
		private struct <ChangeRFModeAsync>d__56 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<bool> <>t__builder;

			public InsightService <>4__this;

			public CancellationToken ct;

			private object <>u__1;

			private void MoveNext()
			{
				//IL_0087: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				InsightService CS$<>8__locals5 = <>4__this;
				bool result;
				try
				{
					AsyncSubject<EventPattern<StatusCodeEventArgs>> val;
					if (num == 0)
					{
						val = (AsyncSubject<EventPattern<StatusCodeEventArgs>>)<>u__1;
						<>u__1 = null;
						num = (<>1__state = -1);
						goto IL_00db;
					}
					if (CS$<>8__locals5.Connected)
					{
						CS$<>8__locals5.SetParameter(142, new byte[1]);
						val = Observable.RunAsync<EventPattern<StatusCodeEventArgs>>(Observable.FirstAsync<EventPattern<StatusCodeEventArgs>>(Observable.Where<EventPattern<StatusCodeEventArgs>>(Observable.FromEventPattern<StatusCodeEventArgs>((Action<EventHandler<StatusCodeEventArgs>>)([CompilerGenerated] (EventHandler<StatusCodeEventArgs> h) =>
						{
							((IInsightServiceInternal)CS$<>8__locals5).OnStatusCode += h;
						}), (Action<EventHandler<StatusCodeEventArgs>>)([CompilerGenerated] (EventHandler<StatusCodeEventArgs> h) =>
						{
							((IInsightServiceInternal)CS$<>8__locals5).OnStatusCode -= h;
						})), (Func<EventPattern<StatusCodeEventArgs>, bool>)((EventPattern<StatusCodeEventArgs> ep) => ((EventPattern<object, StatusCodeEventArgs>)(object)ep).EventArgs.StatusCode == StatusCode.ChangeRFMode)), (Func<EventPattern<StatusCodeEventArgs>, bool>)([CompilerGenerated] (EventPattern<StatusCodeEventArgs> ep) => CS$<>8__locals5._changedRFMode = true)), ct).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitOnCompleted<AsyncSubject<EventPattern<StatusCodeEventArgs>>, <ChangeRFModeAsync>d__56>(ref val, ref this);
							return;
						}
						goto IL_00db;
					}
					result = false;
					goto end_IL_000e;
					IL_00db:
					val.GetResult();
					result = true;
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
		private struct <ConnectAsync>d__107 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<ConnectResult> <>t__builder;

			public InsightService <>4__this;

			public CancellationToken ct;

			public string password;

			public uint heartbeatTimeoutMs;

			private <>c__DisplayClass107_0 <>8__1;

			public uint videoFrameTimeoutMs;

			private CancellationTokenSource <linkedCts>5__2;

			private global::System.Threading.Tasks.Task<EventPattern<HeartbeatErrorEventArgs>> <heartbeatFailedTask>5__3;

			private global::System.Threading.Tasks.Task<EventPattern<StatusCodeEventArgs>> <certificateErrorTask>5__4;

			private global::System.Threading.Tasks.Task<EventPattern<StatusCodeEventArgs>> <resourceUnavailableErrorTask>5__5;

			private TaskAwaiter<global::System.Threading.Tasks.Task> <>u__1;

			private void MoveNext()
			{
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_0230: Unknown result type (might be due to invalid IL or missing references)
				//IL_0235: Unknown result type (might be due to invalid IL or missing references)
				//IL_023d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0072: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
				//IL_012a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0162: Unknown result type (might be due to invalid IL or missing references)
				//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
				//IL_0200: Unknown result type (might be due to invalid IL or missing references)
				//IL_0311: Unknown result type (might be due to invalid IL or missing references)
				//IL_031b: Expected O, but got Unknown
				//IL_0215: Unknown result type (might be due to invalid IL or missing references)
				//IL_0217: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				InsightService CS$<>8__locals33 = <>4__this;
				ConnectResult result;
				try
				{
					if (num != 0)
					{
						LoggerExtensions.LogDebug((ILogger)(object)CS$<>8__locals33._logger, "connecting to camera", global::System.Array.Empty<object>());
						<linkedCts>5__2 = CancellationTokenSource.CreateLinkedTokenSource(ct);
					}
					try
					{
						TaskAwaiter<global::System.Threading.Tasks.Task> val;
						if (num == 0)
						{
							val = <>u__1;
							<>u__1 = default(TaskAwaiter<global::System.Threading.Tasks.Task>);
							num = (<>1__state = -1);
							goto IL_024c;
						}
						<>8__1 = new <>c__DisplayClass107_0();
						<heartbeatFailedTask>5__3 = TaskObservableExtensions.ToTask<EventPattern<HeartbeatErrorEventArgs>>(Observable.FirstAsync<EventPattern<HeartbeatErrorEventArgs>>(Observable.FromEventPattern<HeartbeatErrorEventArgs>((Action<EventHandler<HeartbeatErrorEventArgs>>)([CompilerGenerated] (EventHandler<HeartbeatErrorEventArgs> h) =>
						{
							((IInsightServiceInternal)CS$<>8__locals33).OnHeartbeatError += h;
						}), (Action<EventHandler<HeartbeatErrorEventArgs>>)([CompilerGenerated] (EventHandler<HeartbeatErrorEventArgs> h) =>
						{
							((IInsightServiceInternal)CS$<>8__locals33).OnHeartbeatError -= h;
						}))), <linkedCts>5__2.Token);
						<certificateErrorTask>5__4 = TaskObservableExtensions.ToTask<EventPattern<StatusCodeEventArgs>>(Observable.FirstAsync<EventPattern<StatusCodeEventArgs>>(Observable.Where<EventPattern<StatusCodeEventArgs>>(Observable.FromEventPattern<StatusCodeEventArgs>((Action<EventHandler<StatusCodeEventArgs>>)([CompilerGenerated] (EventHandler<StatusCodeEventArgs> h) =>
						{
							((IInsightServiceInternal)CS$<>8__locals33).OnStatusCode += h;
						}), (Action<EventHandler<StatusCodeEventArgs>>)([CompilerGenerated] (EventHandler<StatusCodeEventArgs> h) =>
						{
							((IInsightServiceInternal)CS$<>8__locals33).OnStatusCode -= h;
						})), (Func<EventPattern<StatusCodeEventArgs>, bool>)((EventPattern<StatusCodeEventArgs> ep) => ((EventPattern<object, StatusCodeEventArgs>)(object)ep).EventArgs.StatusCode == StatusCode.VideoStopped && ((EventPattern<object, StatusCodeEventArgs>)(object)ep).EventArgs.Data.Length != 0 && ((EventPattern<object, StatusCodeEventArgs>)(object)ep).EventArgs.Data[0] == 2))), <linkedCts>5__2.Token);
						<resourceUnavailableErrorTask>5__5 = TaskObservableExtensions.ToTask<EventPattern<StatusCodeEventArgs>>(Observable.FirstAsync<EventPattern<StatusCodeEventArgs>>(Observable.Where<EventPattern<StatusCodeEventArgs>>(Observable.FromEventPattern<StatusCodeEventArgs>((Action<EventHandler<StatusCodeEventArgs>>)([CompilerGenerated] (EventHandler<StatusCodeEventArgs> h) =>
						{
							((IInsightServiceInternal)CS$<>8__locals33).OnStatusCode += h;
						}), (Action<EventHandler<StatusCodeEventArgs>>)([CompilerGenerated] (EventHandler<StatusCodeEventArgs> h) =>
						{
							((IInsightServiceInternal)CS$<>8__locals33).OnStatusCode -= h;
						})), (Func<EventPattern<StatusCodeEventArgs>, bool>)((EventPattern<StatusCodeEventArgs> ep) => ((EventPattern<object, StatusCodeEventArgs>)(object)ep).EventArgs.StatusCode == StatusCode.VideoStopped && ((EventPattern<object, StatusCodeEventArgs>)(object)ep).EventArgs.Data.Length != 0 && ((EventPattern<object, StatusCodeEventArgs>)(object)ep).EventArgs.Data[0] == 1))), <linkedCts>5__2.Token);
						global::System.Threading.Tasks.Task<EventPattern<HeartbeatEventArgs>> task = TaskObservableExtensions.ToTask<EventPattern<HeartbeatEventArgs>>(Observable.FirstAsync<EventPattern<HeartbeatEventArgs>>(Observable.Skip<EventPattern<HeartbeatEventArgs>>(Observable.FromEventPattern<HeartbeatEventArgs>((Action<EventHandler<HeartbeatEventArgs>>)([CompilerGenerated] (EventHandler<HeartbeatEventArgs> h) =>
						{
							((IInsightServiceInternal)CS$<>8__locals33).OnHeartbeat += h;
						}), (Action<EventHandler<HeartbeatEventArgs>>)([CompilerGenerated] (EventHandler<HeartbeatEventArgs> h) =>
						{
							((IInsightServiceInternal)CS$<>8__locals33).OnHeartbeat -= h;
						})), 4)), ct);
						if (CS$<>8__locals33.Connect(password, heartbeatTimeoutMs))
						{
							<>y__InlineArray4<global::System.Threading.Tasks.Task> buffer = default(<>y__InlineArray4<global::System.Threading.Tasks.Task>);
							<PrivateImplementationDetails>.InlineArrayElementRef<<>y__InlineArray4<global::System.Threading.Tasks.Task>, global::System.Threading.Tasks.Task>(ref buffer, 0) = <heartbeatFailedTask>5__3;
							<PrivateImplementationDetails>.InlineArrayElementRef<<>y__InlineArray4<global::System.Threading.Tasks.Task>, global::System.Threading.Tasks.Task>(ref buffer, 1) = <certificateErrorTask>5__4;
							<PrivateImplementationDetails>.InlineArrayElementRef<<>y__InlineArray4<global::System.Threading.Tasks.Task>, global::System.Threading.Tasks.Task>(ref buffer, 2) = <resourceUnavailableErrorTask>5__5;
							<PrivateImplementationDetails>.InlineArrayElementRef<<>y__InlineArray4<global::System.Threading.Tasks.Task>, global::System.Threading.Tasks.Task>(ref buffer, 3) = task;
							val = global::System.Threading.Tasks.Task.WhenAny(<PrivateImplementationDetails>.InlineArrayAsReadOnlySpan<<>y__InlineArray4<global::System.Threading.Tasks.Task>, global::System.Threading.Tasks.Task>(in buffer, 4)).GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<global::System.Threading.Tasks.Task>, <ConnectAsync>d__107>(ref val, ref this);
								return;
							}
							goto IL_024c;
						}
						LoggerExtensions.LogError((ILogger)(object)CS$<>8__locals33._logger, "Failed to connect: Internal connection error", global::System.Array.Empty<object>());
						CS$<>8__locals33.Disconnect(null);
						result = ConnectResult.Failed;
						goto end_IL_000e;
						IL_024c:
						val.GetResult();
						if (((global::System.Threading.Tasks.Task)<heartbeatFailedTask>5__3).IsCompleted)
						{
							LoggerExtensions.LogError((ILogger)(object)CS$<>8__locals33._logger, "Failed to connect: No heartbeat received", global::System.Array.Empty<object>());
							result = ConnectResult.Timeout;
						}
						else if (((global::System.Threading.Tasks.Task)<certificateErrorTask>5__4).IsCompleted)
						{
							LoggerExtensions.LogError((ILogger)(object)CS$<>8__locals33._logger, "Failed to connect: Certificate error", global::System.Array.Empty<object>());
							result = ConnectResult.InvalidPassword;
						}
						else
						{
							if (!((global::System.Threading.Tasks.Task)<resourceUnavailableErrorTask>5__5).IsCompleted)
							{
								CS$<>8__locals33._onHeartbeatFailedDisposable = ObservableExtensions.Subscribe<EventPattern<HeartbeatErrorEventArgs>>(Observable.FromEventPattern<HeartbeatErrorEventArgs>((Action<EventHandler<HeartbeatErrorEventArgs>>)([CompilerGenerated] (EventHandler<HeartbeatErrorEventArgs> h) =>
								{
									((IInsightServiceInternal)CS$<>8__locals33).OnHeartbeatError += h;
								}), (Action<EventHandler<HeartbeatErrorEventArgs>>)([CompilerGenerated] (EventHandler<HeartbeatErrorEventArgs> h) =>
								{
									((IInsightServiceInternal)CS$<>8__locals33).OnHeartbeatError -= h;
								})), (Action<EventPattern<HeartbeatErrorEventArgs>>)([CompilerGenerated] (EventPattern<HeartbeatErrorEventArgs> ep) =>
								{
									LoggerExtensions.LogError((ILogger)(object)CS$<>8__locals33._logger, "Disconnecting: No heartbeat received", global::System.Array.Empty<object>());
									CS$<>8__locals33.Disconnect(DisconnectReason.HeartbeatFailed);
								}));
								<>8__1.videoWatchdog = new Timer((double)videoFrameTimeoutMs);
								CS$<>8__locals33._onVideoWatchdogElapsedDisposable = ObservableExtensions.Subscribe<EventPattern<ElapsedEventArgs>>(Observable.FromEventPattern<ElapsedEventHandler, ElapsedEventArgs>((Action<ElapsedEventHandler>)delegate(ElapsedEventHandler h)
								{
									<>8__1.videoWatchdog.Elapsed += h;
								}, (Action<ElapsedEventHandler>)delegate(ElapsedEventHandler h)
								{
									<>8__1.videoWatchdog.Elapsed -= h;
								}), (Action<EventPattern<ElapsedEventArgs>>)([CompilerGenerated] (EventPattern<ElapsedEventArgs> ep) =>
								{
									LoggerExtensions.LogError((ILogger)(object)CS$<>8__locals33._logger, "Disconnecting: No video frame received", global::System.Array.Empty<object>());
									CS$<>8__locals33.Disconnect(DisconnectReason.VideoLost);
								}));
								<>8__1.videoWatchdog.AutoReset = false;
								<>8__1.videoWatchdog.Start();
								CS$<>8__locals33._videoWatchdog = <>8__1.videoWatchdog;
								CS$<>8__locals33._onVideoFrameReceivedDisposable = ObservableExtensions.Subscribe<EventPattern<VideoFrameReceivedEventArgs>>(Observable.FromEventPattern<VideoFrameReceivedEventArgs>((Action<EventHandler<VideoFrameReceivedEventArgs>>)([CompilerGenerated] (EventHandler<VideoFrameReceivedEventArgs> h) =>
								{
									((IInsightServiceInternal)CS$<>8__locals33).OnVideoFrameReceived += h;
								}), (Action<EventHandler<VideoFrameReceivedEventArgs>>)([CompilerGenerated] (EventHandler<VideoFrameReceivedEventArgs> h) =>
								{
									((IInsightServiceInternal)CS$<>8__locals33).OnVideoFrameReceived -= h;
								})), (Action<EventPattern<VideoFrameReceivedEventArgs>>)delegate
								{
									<>8__1.videoWatchdog.Stop();
									<>8__1.videoWatchdog.Start();
								});
								object connectionLock = _connectionLock;
								bool flag = false;
								try
								{
									Monitor.Enter(connectionLock, ref flag);
									CS$<>8__locals33.Connected = true;
								}
								finally
								{
									if (num < 0 && flag)
									{
										Monitor.Exit(connectionLock);
									}
								}
								<>8__1 = null;
								<heartbeatFailedTask>5__3 = null;
								<certificateErrorTask>5__4 = null;
								<resourceUnavailableErrorTask>5__5 = null;
								goto end_IL_0038;
							}
							LoggerExtensions.LogError((ILogger)(object)CS$<>8__locals33._logger, "Failed to connect: Resource unavailable error", global::System.Array.Empty<object>());
							result = ConnectResult.CameraUnavailable;
						}
						goto end_IL_000e;
						end_IL_0038:;
					}
					catch (global::System.Exception ex)
					{
						if (<linkedCts>5__2.IsCancellationRequested)
						{
							LoggerExtensions.LogWarning((ILogger)(object)CS$<>8__locals33._logger, ex, "Connection to camera cancelled", global::System.Array.Empty<object>());
							result = ConnectResult.Cancelled;
						}
						else
						{
							LoggerExtensions.LogError((ILogger)(object)CS$<>8__locals33._logger, ex, "Error connecting to camera", global::System.Array.Empty<object>());
							result = ConnectResult.Failed;
						}
						goto end_IL_000e;
					}
					finally
					{
						if (num < 0)
						{
							try
							{
								<linkedCts>5__2.Cancel();
							}
							catch
							{
							}
							try
							{
								<linkedCts>5__2.Dispose();
							}
							catch
							{
							}
							object connectionLock = _connectionLock;
							bool flag = false;
							try
							{
								Monitor.Enter(connectionLock, ref flag);
								if (!CS$<>8__locals33.Connected)
								{
									CS$<>8__locals33.Disconnect(null);
								}
							}
							finally
							{
								if (num < 0 && flag)
								{
									Monitor.Exit(connectionLock);
								}
							}
						}
					}
					result = ((!CS$<>8__locals33.Connected) ? ConnectResult.Failed : ConnectResult.Success);
					end_IL_000e:;
				}
				catch (global::System.Exception exception)
				{
					<>1__state = -2;
					<linkedCts>5__2 = null;
					<>t__builder.SetException(exception);
					return;
				}
				<>1__state = -2;
				<linkedCts>5__2 = null;
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
		private struct <ConnectAsync>d__53 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<ConnectResult> <>t__builder;

			public InsightService <>4__this;

			public CancellationToken ct;

			public string password;

			public uint videoFrameTimeoutMs;

			public uint heartbeatTimeoutMs;

			private <>c__DisplayClass53_0 <>8__1;

			private int <instance>5__2;

			private TaskAwaiter<ConnectResult> <>u__1;

			private void MoveNext()
			{
				//IL_010a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0125: Unknown result type (might be due to invalid IL or missing references)
				//IL_012a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0149: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
				//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
				//IL_0194: Unknown result type (might be due to invalid IL or missing references)
				//IL_0199: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
				//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
				//IL_0262: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				InsightService insightService = <>4__this;
				ConnectResult result2;
				try
				{
					global::System.Threading.Tasks.Task<ConnectResult> task = default(global::System.Threading.Tasks.Task<ConnectResult>);
					if (num != 0)
					{
						<>8__1 = new <>c__DisplayClass53_0();
						<>8__1.<>4__this = <>4__this;
						<>8__1.connectionCt = CancellationToken.None;
						<instance>5__2 = _instance++;
						LoggerExtensions.LogInformation((ILogger)(object)insightService._logger, "##### {0}: Started ({1})", new object[2] { "ConnectAsync", <instance>5__2 });
						object connectionLock = _connectionLock;
						bool flag = false;
						try
						{
							Monitor.Enter(connectionLock, ref flag);
							if (insightService._connectionTask == null)
							{
								if (insightService.Connected)
								{
									LoggerExtensions.LogDebug((ILogger)(object)insightService._logger, "Already connected to camera", global::System.Array.Empty<object>());
									task = global::System.Threading.Tasks.Task.FromResult<ConnectResult>(ConnectResult.Success);
								}
								else
								{
									LoggerExtensions.LogDebug((ILogger)(object)insightService._logger, "Connecting to camera", global::System.Array.Empty<object>());
									try
									{
										CancellationTokenSource? connectionCts = insightService._connectionCts;
										if (connectionCts != null)
										{
											connectionCts.Cancel();
										}
									}
									catch
									{
									}
									try
									{
										CancellationTokenSource? connectionCts2 = insightService._connectionCts;
										if (connectionCts2 != null)
										{
											connectionCts2.Dispose();
										}
									}
									catch
									{
									}
									insightService._connectionCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
									<>8__1.connectionCt = insightService._connectionCts.Token;
									insightService._connectionTask = insightService.ConnectAsync(password, videoFrameTimeoutMs, heartbeatTimeoutMs, <>8__1.connectionCt);
									task = insightService._connectionTask;
								}
							}
							else
							{
								LoggerExtensions.LogDebug((ILogger)(object)insightService._logger, "Already connecting to camera", global::System.Array.Empty<object>());
								task = insightService._connectionTask;
							}
						}
						finally
						{
							if (num < 0 && flag)
							{
								Monitor.Exit(connectionLock);
							}
						}
					}
					ConnectResult result;
					try
					{
						TaskAwaiter<ConnectResult> val;
						if (num != 0)
						{
							val = task.GetAwaiter();
							if (!val.IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<ConnectResult>, <ConnectAsync>d__53>(ref val, ref this);
								return;
							}
						}
						else
						{
							val = <>u__1;
							<>u__1 = default(TaskAwaiter<ConnectResult>);
							num = (<>1__state = -1);
						}
						result = val.GetResult();
					}
					finally
					{
						if (num < 0)
						{
							object connectionLock = _connectionLock;
							bool flag = false;
							try
							{
								Monitor.Enter(connectionLock, ref flag);
								insightService._connectionTask = null;
							}
							finally
							{
								if (num < 0 && flag)
								{
									Monitor.Exit(connectionLock);
								}
							}
						}
					}
					LoggerExtensions.LogInformation((ILogger)(object)insightService._logger, "##### {0}: Finished ({1}) - connected={2}", new object[3] { "ConnectAsync", <instance>5__2, result });
					if (result == ConnectResult.Success)
					{
						insightService.GetFirmwareVersionAsync(<>8__1.connectionCt).ContinueWith((Action<global::System.Threading.Tasks.Task<Version>>)delegate(global::System.Threading.Tasks.Task<Version> task2)
						{
							//IL_0057: Unknown result type (might be due to invalid IL or missing references)
							Version result3 = task2.Result;
							LoggerExtensions.LogInformation((ILogger)(object)<>8__1.<>4__this._logger, "{0}: Firmware Version = {1}", new object[2]
							{
								"ConnectAsync",
								((object)result3).ToString()
							});
							if (result3.Major >= 21 || (result3.Major == 21 && result3.Minor >= 3))
							{
								<>8__1.<>4__this.GetAspectRatio(<>8__1.connectionCt).ContinueWith((Action<global::System.Threading.Tasks.Task<double>>)delegate(global::System.Threading.Tasks.Task<double> task3)
								{
									<>8__1.<>4__this.AspectRatio = task3.Result;
									LoggerExtensions.LogInformation((ILogger)(object)<>8__1.<>4__this._logger, "{0}: Aspect Ratio = {1}", new object[2]
									{
										"ConnectAsync",
										<>8__1.<>4__this.AspectRatio
									});
									<>8__1.<>4__this.OnAspectRatio?.Invoke((object)<>8__1.<>4__this, <>8__1.<>4__this.AspectRatio);
								});
							}
							else
							{
								LoggerExtensions.LogDebug((ILogger)(object)<>8__1.<>4__this._logger, "Aspect Ratio is not supported by this Firmware {0}", new object[1] { ((object)result3).ToString() });
							}
						});
					}
					result2 = result;
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
		private struct <EnterPairingModeAsync>d__57 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<bool> <>t__builder;

			public InsightService <>4__this;

			public CancellationToken ct;

			private object <>u__1;

			private void MoveNext()
			{
				//IL_0087: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				InsightService CS$<>8__locals5 = <>4__this;
				bool result;
				try
				{
					AsyncSubject<EventPattern<StatusCodeEventArgs>> val;
					if (num == 0)
					{
						val = (AsyncSubject<EventPattern<StatusCodeEventArgs>>)<>u__1;
						<>u__1 = null;
						num = (<>1__state = -1);
						goto IL_00db;
					}
					if (CS$<>8__locals5.Connected)
					{
						CS$<>8__locals5.SetParameter(145, new byte[1]);
						val = Observable.RunAsync<EventPattern<StatusCodeEventArgs>>(Observable.FirstAsync<EventPattern<StatusCodeEventArgs>>(Observable.Where<EventPattern<StatusCodeEventArgs>>(Observable.FromEventPattern<StatusCodeEventArgs>((Action<EventHandler<StatusCodeEventArgs>>)([CompilerGenerated] (EventHandler<StatusCodeEventArgs> h) =>
						{
							((IInsightServiceInternal)CS$<>8__locals5).OnStatusCode += h;
						}), (Action<EventHandler<StatusCodeEventArgs>>)([CompilerGenerated] (EventHandler<StatusCodeEventArgs> h) =>
						{
							((IInsightServiceInternal)CS$<>8__locals5).OnStatusCode -= h;
						})), (Func<EventPattern<StatusCodeEventArgs>, bool>)((EventPattern<StatusCodeEventArgs> ep) => ((EventPattern<object, StatusCodeEventArgs>)(object)ep).EventArgs.StatusCode == StatusCode.EnterPairingMode)), (Func<EventPattern<StatusCodeEventArgs>, bool>)([CompilerGenerated] (EventPattern<StatusCodeEventArgs> ep) => CS$<>8__locals5._enteredPairingMode = true)), ct).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitOnCompleted<AsyncSubject<EventPattern<StatusCodeEventArgs>>, <EnterPairingModeAsync>d__57>(ref val, ref this);
							return;
						}
						goto IL_00db;
					}
					result = false;
					goto end_IL_000e;
					IL_00db:
					val.GetResult();
					result = true;
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
		private struct <GetBrightnessAsync>d__59 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<byte> <>t__builder;

			public InsightService <>4__this;

			public CancellationToken ct;

			private TaskAwaiter<byte> <>u__1;

			private void MoveNext()
			{
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_0057: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				InsightService insightService = <>4__this;
				byte result;
				try
				{
					TaskAwaiter<byte> val;
					if (num != 0)
					{
						val = insightService.GetParameterAsync(StatusCode.Brightness, ct).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<byte>, <GetBrightnessAsync>d__59>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<byte>);
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
		private struct <GetChromaAsync>d__61 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<byte> <>t__builder;

			public InsightService <>4__this;

			public CancellationToken ct;

			private TaskAwaiter<byte> <>u__1;

			private void MoveNext()
			{
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_0057: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				InsightService insightService = <>4__this;
				byte result;
				try
				{
					TaskAwaiter<byte> val;
					if (num != 0)
					{
						val = insightService.GetParameterAsync(StatusCode.Chroma, ct).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<byte>, <GetChromaAsync>d__61>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<byte>);
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
		private struct <GetContrastAsync>d__63 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<byte> <>t__builder;

			public InsightService <>4__this;

			public CancellationToken ct;

			private TaskAwaiter<byte> <>u__1;

			private void MoveNext()
			{
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_0057: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				InsightService insightService = <>4__this;
				byte result;
				try
				{
					TaskAwaiter<byte> val;
					if (num != 0)
					{
						val = insightService.GetParameterAsync(StatusCode.Contrast, ct).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<byte>, <GetContrastAsync>d__63>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<byte>);
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
		private struct <GetMirrorFlipAsync>d__67 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<byte> <>t__builder;

			public InsightService <>4__this;

			public CancellationToken ct;

			private TaskAwaiter<byte> <>u__1;

			private void MoveNext()
			{
				//IL_004f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0054: Unknown result type (might be due to invalid IL or missing references)
				//IL_005b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				InsightService insightService = <>4__this;
				byte result;
				try
				{
					TaskAwaiter<byte> val;
					if (num != 0)
					{
						val = insightService.GetParameterAsync(StatusCode.MirrorFlip, ct).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<byte>, <GetMirrorFlipAsync>d__67>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<byte>);
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
		private struct <GetSaturationAsync>d__65 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<byte> <>t__builder;

			public InsightService <>4__this;

			public CancellationToken ct;

			private TaskAwaiter<byte> <>u__1;

			private void MoveNext()
			{
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_0057: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				InsightService insightService = <>4__this;
				byte result;
				try
				{
					TaskAwaiter<byte> val;
					if (num != 0)
					{
						val = insightService.GetParameterAsync(StatusCode.Saturation, ct).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<byte>, <GetSaturationAsync>d__65>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<byte>);
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
		private struct <SetBrightnessAsync>d__60 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<bool> <>t__builder;

			public InsightService <>4__this;

			public byte value;

			public CancellationToken ct;

			private TaskAwaiter<bool> <>u__1;

			private void MoveNext()
			{
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_005d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0064: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				InsightService insightService = <>4__this;
				bool result;
				try
				{
					TaskAwaiter<bool> val;
					if (num != 0)
					{
						val = insightService.SetParameterAsync(StatusCode.Brightness, value, ct).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, <SetBrightnessAsync>d__60>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<bool>);
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
		private struct <SetChromaAsync>d__62 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<bool> <>t__builder;

			public InsightService <>4__this;

			public byte value;

			public CancellationToken ct;

			private TaskAwaiter<bool> <>u__1;

			private void MoveNext()
			{
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_005d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0064: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				InsightService insightService = <>4__this;
				bool result;
				try
				{
					TaskAwaiter<bool> val;
					if (num != 0)
					{
						val = insightService.SetParameterAsync(StatusCode.Chroma, value, ct).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, <SetChromaAsync>d__62>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<bool>);
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
		private struct <SetContrastAsync>d__64 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<bool> <>t__builder;

			public InsightService <>4__this;

			public byte value;

			public CancellationToken ct;

			private TaskAwaiter<bool> <>u__1;

			private void MoveNext()
			{
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_005d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0064: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				InsightService insightService = <>4__this;
				bool result;
				try
				{
					TaskAwaiter<bool> val;
					if (num != 0)
					{
						val = insightService.SetParameterAsync(StatusCode.Contrast, value, ct).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, <SetContrastAsync>d__64>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<bool>);
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
		private struct <SetMirrorFlipAsync>d__68 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<bool> <>t__builder;

			public InsightService <>4__this;

			public CameraMirrorFlip value;

			public CancellationToken ct;

			private TaskAwaiter<bool> <>u__1;

			private void MoveNext()
			{
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				//IL_005b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0062: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0040: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				InsightService insightService = <>4__this;
				bool result;
				try
				{
					TaskAwaiter<bool> val;
					if (num != 0)
					{
						val = insightService.SetParameterAsync(StatusCode.MirrorFlip, (byte)value, ct).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, <SetMirrorFlipAsync>d__68>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<bool>);
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
		private struct <SetPassword>d__55 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<bool> <>t__builder;

			public InsightService <>4__this;

			public string password;

			public CancellationToken ct;

			private object <>u__1;

			private void MoveNext()
			{
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				//IL_009c: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				InsightService CS$<>8__locals5 = <>4__this;
				bool result;
				try
				{
					AsyncSubject<EventPattern<StatusCodeEventArgs>> val;
					if (num == 0)
					{
						val = (AsyncSubject<EventPattern<StatusCodeEventArgs>>)<>u__1;
						<>u__1 = null;
						num = (<>1__state = -1);
						goto IL_00f0;
					}
					if (CS$<>8__locals5.Connected)
					{
						if (password.Length > 20)
						{
							throw new ArgumentOutOfRangeException("password cannot exceed 20 characters");
						}
						CS$<>8__locals5.SetPassword(password);
						val = Observable.RunAsync<EventPattern<StatusCodeEventArgs>>(Observable.FirstAsync<EventPattern<StatusCodeEventArgs>>(Observable.Where<EventPattern<StatusCodeEventArgs>>(Observable.FromEventPattern<StatusCodeEventArgs>((Action<EventHandler<StatusCodeEventArgs>>)([CompilerGenerated] (EventHandler<StatusCodeEventArgs> h) =>
						{
							((IInsightServiceInternal)CS$<>8__locals5).OnStatusCode += h;
						}), (Action<EventHandler<StatusCodeEventArgs>>)([CompilerGenerated] (EventHandler<StatusCodeEventArgs> h) =>
						{
							((IInsightServiceInternal)CS$<>8__locals5).OnStatusCode -= h;
						})), (Func<EventPattern<StatusCodeEventArgs>, bool>)((EventPattern<StatusCodeEventArgs> ep) => ((EventPattern<object, StatusCodeEventArgs>)(object)ep).EventArgs.StatusCode == StatusCode.SetPassword)), (Func<EventPattern<StatusCodeEventArgs>, bool>)([CompilerGenerated] (EventPattern<StatusCodeEventArgs> ep) => CS$<>8__locals5._setPassword = true)), ct).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitOnCompleted<AsyncSubject<EventPattern<StatusCodeEventArgs>>, <SetPassword>d__55>(ref val, ref this);
							return;
						}
						goto IL_00f0;
					}
					result = false;
					goto end_IL_000e;
					IL_00f0:
					val.GetResult();
					result = true;
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
		private struct <SetSaturationAsync>d__66 : IAsyncStateMachine
		{
			public int <>1__state;

			public AsyncTaskMethodBuilder<bool> <>t__builder;

			public InsightService <>4__this;

			public byte value;

			public CancellationToken ct;

			private TaskAwaiter<bool> <>u__1;

			private void MoveNext()
			{
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_005d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0064: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				int num = <>1__state;
				InsightService insightService = <>4__this;
				bool result;
				try
				{
					TaskAwaiter<bool> val;
					if (num != 0)
					{
						val = insightService.SetParameterAsync(StatusCode.Saturation, value, ct).GetAwaiter();
						if (!val.IsCompleted)
						{
							num = (<>1__state = 0);
							<>u__1 = val;
							<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, <SetSaturationAsync>d__66>(ref val, ref this);
							return;
						}
					}
					else
					{
						val = <>u__1;
						<>u__1 = default(TaskAwaiter<bool>);
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

		private static readonly byte[] GetParameterData = new byte[1] { 1 };

		private readonly VideoHeader _videoHeader = new VideoHeader();

		private readonly AudioPlayer _audioPlayer = new AudioPlayer();

		private RealCamCallback? _realCamCallback;

		private static readonly object _lock = new object();

		private readonly Dictionary<Surface, Decoder?> _decoders = new Dictionary<Surface, Decoder>();

		private readonly Dictionary<Decoder, ValueTuple<DecoderNotify, global::System.IDisposable>> _decoderDisposables = new Dictionary<Decoder, ValueTuple<DecoderNotify, global::System.IDisposable>>();

		private const int _framerate = 15;

		private bool _ppsspsInit;

		public const uint DefaultVideoFrameTimeoutMs = 1000u;

		public const uint DefaultHeartbeatTimeoutMs = 5000u;

		private const int AspectRatioMajorVersion = 21;

		private const int AspectRatioMinorVersion = 3;

		private readonly ILogger<InsightService> _logger;

		private Timer? _videoWatchdog;

		private global::System.IDisposable? _onVideoWatchdogElapsedDisposable;

		private global::System.IDisposable? _onHeartbeatFailedDisposable;

		private global::System.IDisposable? _onVideoFrameReceivedDisposable;

		private global::System.IDisposable? _onConnectionLostDisposable;

		private static readonly object _connectionLock = new object();

		private global::System.Threading.Tasks.Task<ConnectResult>? _connectionTask;

		private CancellationTokenSource? _connectionCts;

		private bool _setPassword;

		private bool _changedRFMode;

		private bool _enteredPairingMode;

		private bool _mute = true;

		private static int _instance;

		private double _aspectRatio = 1.3333333333333333;

		[CompilerGenerated]
		private EventHandler<CameraDisconnectedEventArgs>? m_OnCameraDisconnected;

		[CompilerGenerated]
		private EventHandler<double>? m_OnAspectRatio;

		[CompilerGenerated]
		private EventHandler<byte>? m_OnBrightness;

		[CompilerGenerated]
		private EventHandler<byte>? m_OnChroma;

		[CompilerGenerated]
		private EventHandler<byte>? m_OnContrast;

		[CompilerGenerated]
		private EventHandler<byte>? m_OnSaturation;

		[CompilerGenerated]
		private EventHandler<byte>? m_OnMirrorFlip;

		[CompilerGenerated]
		private EventHandler<HeartbeatEventArgs>? m_OnHeartbeat;

		[CompilerGenerated]
		private EventHandler<HeartbeatErrorEventArgs>? m_OnHeartbeatError;

		[CompilerGenerated]
		private EventHandler<VideoFrameReceivedEventArgs>? m_OnVideoFrameReceived;

		[CompilerGenerated]
		private EventHandler<StatusCodeEventArgs>? m_OnStatusCode;

		[field: CompilerGenerated]
		public bool Connected
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		public bool Mute
		{
			get
			{
				return _mute;
			}
			set
			{
				if (value != _mute)
				{
					_mute = value;
					if (_mute)
					{
						((IInsightServiceInternal)this)?.MuteAudio();
					}
					else
					{
						((IInsightServiceInternal)this)?.UnmuteAudio();
					}
				}
			}
		}

		public double AspectRatio
		{
			get
			{
				return _aspectRatio;
			}
			private set
			{
				if (Math.Abs(_aspectRatio - value) > 5E-324)
				{
					_aspectRatio = value;
					this.OnAspectRatio?.Invoke((object)this, _aspectRatio);
				}
			}
		}

		public event EventHandler<CameraDisconnectedEventArgs>? OnCameraDisconnected
		{
			[CompilerGenerated]
			add
			{
				EventHandler<CameraDisconnectedEventArgs> val = this.m_OnCameraDisconnected;
				EventHandler<CameraDisconnectedEventArgs> val2;
				do
				{
					val2 = val;
					EventHandler<CameraDisconnectedEventArgs> val3 = (EventHandler<CameraDisconnectedEventArgs>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
					val = Interlocked.CompareExchange<EventHandler<CameraDisconnectedEventArgs>>(ref this.m_OnCameraDisconnected, val3, val2);
				}
				while (val != val2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler<CameraDisconnectedEventArgs> val = this.m_OnCameraDisconnected;
				EventHandler<CameraDisconnectedEventArgs> val2;
				do
				{
					val2 = val;
					EventHandler<CameraDisconnectedEventArgs> val3 = (EventHandler<CameraDisconnectedEventArgs>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
					val = Interlocked.CompareExchange<EventHandler<CameraDisconnectedEventArgs>>(ref this.m_OnCameraDisconnected, val3, val2);
				}
				while (val != val2);
			}
		}

		public event EventHandler<double>? OnAspectRatio
		{
			[CompilerGenerated]
			add
			{
				EventHandler<double> val = this.m_OnAspectRatio;
				EventHandler<double> val2;
				do
				{
					val2 = val;
					EventHandler<double> val3 = (EventHandler<double>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
					val = Interlocked.CompareExchange<EventHandler<double>>(ref this.m_OnAspectRatio, val3, val2);
				}
				while (val != val2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler<double> val = this.m_OnAspectRatio;
				EventHandler<double> val2;
				do
				{
					val2 = val;
					EventHandler<double> val3 = (EventHandler<double>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
					val = Interlocked.CompareExchange<EventHandler<double>>(ref this.m_OnAspectRatio, val3, val2);
				}
				while (val != val2);
			}
		}

		public event EventHandler<byte>? OnBrightness
		{
			[CompilerGenerated]
			add
			{
				EventHandler<byte> val = this.m_OnBrightness;
				EventHandler<byte> val2;
				do
				{
					val2 = val;
					EventHandler<byte> val3 = (EventHandler<byte>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
					val = Interlocked.CompareExchange<EventHandler<byte>>(ref this.m_OnBrightness, val3, val2);
				}
				while (val != val2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler<byte> val = this.m_OnBrightness;
				EventHandler<byte> val2;
				do
				{
					val2 = val;
					EventHandler<byte> val3 = (EventHandler<byte>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
					val = Interlocked.CompareExchange<EventHandler<byte>>(ref this.m_OnBrightness, val3, val2);
				}
				while (val != val2);
			}
		}

		public event EventHandler<byte>? OnChroma
		{
			[CompilerGenerated]
			add
			{
				EventHandler<byte> val = this.m_OnChroma;
				EventHandler<byte> val2;
				do
				{
					val2 = val;
					EventHandler<byte> val3 = (EventHandler<byte>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
					val = Interlocked.CompareExchange<EventHandler<byte>>(ref this.m_OnChroma, val3, val2);
				}
				while (val != val2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler<byte> val = this.m_OnChroma;
				EventHandler<byte> val2;
				do
				{
					val2 = val;
					EventHandler<byte> val3 = (EventHandler<byte>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
					val = Interlocked.CompareExchange<EventHandler<byte>>(ref this.m_OnChroma, val3, val2);
				}
				while (val != val2);
			}
		}

		public event EventHandler<byte>? OnContrast
		{
			[CompilerGenerated]
			add
			{
				EventHandler<byte> val = this.m_OnContrast;
				EventHandler<byte> val2;
				do
				{
					val2 = val;
					EventHandler<byte> val3 = (EventHandler<byte>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
					val = Interlocked.CompareExchange<EventHandler<byte>>(ref this.m_OnContrast, val3, val2);
				}
				while (val != val2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler<byte> val = this.m_OnContrast;
				EventHandler<byte> val2;
				do
				{
					val2 = val;
					EventHandler<byte> val3 = (EventHandler<byte>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
					val = Interlocked.CompareExchange<EventHandler<byte>>(ref this.m_OnContrast, val3, val2);
				}
				while (val != val2);
			}
		}

		public event EventHandler<byte>? OnSaturation
		{
			[CompilerGenerated]
			add
			{
				EventHandler<byte> val = this.m_OnSaturation;
				EventHandler<byte> val2;
				do
				{
					val2 = val;
					EventHandler<byte> val3 = (EventHandler<byte>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
					val = Interlocked.CompareExchange<EventHandler<byte>>(ref this.m_OnSaturation, val3, val2);
				}
				while (val != val2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler<byte> val = this.m_OnSaturation;
				EventHandler<byte> val2;
				do
				{
					val2 = val;
					EventHandler<byte> val3 = (EventHandler<byte>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
					val = Interlocked.CompareExchange<EventHandler<byte>>(ref this.m_OnSaturation, val3, val2);
				}
				while (val != val2);
			}
		}

		public event EventHandler<byte>? OnMirrorFlip
		{
			[CompilerGenerated]
			add
			{
				EventHandler<byte> val = this.m_OnMirrorFlip;
				EventHandler<byte> val2;
				do
				{
					val2 = val;
					EventHandler<byte> val3 = (EventHandler<byte>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
					val = Interlocked.CompareExchange<EventHandler<byte>>(ref this.m_OnMirrorFlip, val3, val2);
				}
				while (val != val2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler<byte> val = this.m_OnMirrorFlip;
				EventHandler<byte> val2;
				do
				{
					val2 = val;
					EventHandler<byte> val3 = (EventHandler<byte>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
					val = Interlocked.CompareExchange<EventHandler<byte>>(ref this.m_OnMirrorFlip, val3, val2);
				}
				while (val != val2);
			}
		}

		public event EventHandler<HeartbeatEventArgs>? OnHeartbeat
		{
			[CompilerGenerated]
			add
			{
				EventHandler<HeartbeatEventArgs> val = this.m_OnHeartbeat;
				EventHandler<HeartbeatEventArgs> val2;
				do
				{
					val2 = val;
					EventHandler<HeartbeatEventArgs> val3 = (EventHandler<HeartbeatEventArgs>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
					val = Interlocked.CompareExchange<EventHandler<HeartbeatEventArgs>>(ref this.m_OnHeartbeat, val3, val2);
				}
				while (val != val2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler<HeartbeatEventArgs> val = this.m_OnHeartbeat;
				EventHandler<HeartbeatEventArgs> val2;
				do
				{
					val2 = val;
					EventHandler<HeartbeatEventArgs> val3 = (EventHandler<HeartbeatEventArgs>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
					val = Interlocked.CompareExchange<EventHandler<HeartbeatEventArgs>>(ref this.m_OnHeartbeat, val3, val2);
				}
				while (val != val2);
			}
		}

		public event EventHandler<HeartbeatErrorEventArgs>? OnHeartbeatError
		{
			[CompilerGenerated]
			add
			{
				EventHandler<HeartbeatErrorEventArgs> val = this.m_OnHeartbeatError;
				EventHandler<HeartbeatErrorEventArgs> val2;
				do
				{
					val2 = val;
					EventHandler<HeartbeatErrorEventArgs> val3 = (EventHandler<HeartbeatErrorEventArgs>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
					val = Interlocked.CompareExchange<EventHandler<HeartbeatErrorEventArgs>>(ref this.m_OnHeartbeatError, val3, val2);
				}
				while (val != val2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler<HeartbeatErrorEventArgs> val = this.m_OnHeartbeatError;
				EventHandler<HeartbeatErrorEventArgs> val2;
				do
				{
					val2 = val;
					EventHandler<HeartbeatErrorEventArgs> val3 = (EventHandler<HeartbeatErrorEventArgs>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
					val = Interlocked.CompareExchange<EventHandler<HeartbeatErrorEventArgs>>(ref this.m_OnHeartbeatError, val3, val2);
				}
				while (val != val2);
			}
		}

		public event EventHandler<VideoFrameReceivedEventArgs>? OnVideoFrameReceived
		{
			[CompilerGenerated]
			add
			{
				EventHandler<VideoFrameReceivedEventArgs> val = this.m_OnVideoFrameReceived;
				EventHandler<VideoFrameReceivedEventArgs> val2;
				do
				{
					val2 = val;
					EventHandler<VideoFrameReceivedEventArgs> val3 = (EventHandler<VideoFrameReceivedEventArgs>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
					val = Interlocked.CompareExchange<EventHandler<VideoFrameReceivedEventArgs>>(ref this.m_OnVideoFrameReceived, val3, val2);
				}
				while (val != val2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler<VideoFrameReceivedEventArgs> val = this.m_OnVideoFrameReceived;
				EventHandler<VideoFrameReceivedEventArgs> val2;
				do
				{
					val2 = val;
					EventHandler<VideoFrameReceivedEventArgs> val3 = (EventHandler<VideoFrameReceivedEventArgs>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
					val = Interlocked.CompareExchange<EventHandler<VideoFrameReceivedEventArgs>>(ref this.m_OnVideoFrameReceived, val3, val2);
				}
				while (val != val2);
			}
		}

		public event EventHandler<StatusCodeEventArgs>? OnStatusCode
		{
			[CompilerGenerated]
			add
			{
				EventHandler<StatusCodeEventArgs> val = this.m_OnStatusCode;
				EventHandler<StatusCodeEventArgs> val2;
				do
				{
					val2 = val;
					EventHandler<StatusCodeEventArgs> val3 = (EventHandler<StatusCodeEventArgs>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
					val = Interlocked.CompareExchange<EventHandler<StatusCodeEventArgs>>(ref this.m_OnStatusCode, val3, val2);
				}
				while (val != val2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler<StatusCodeEventArgs> val = this.m_OnStatusCode;
				EventHandler<StatusCodeEventArgs> val2;
				do
				{
					val2 = val;
					EventHandler<StatusCodeEventArgs> val3 = (EventHandler<StatusCodeEventArgs>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)val2, (global::System.Delegate)(object)value);
					val = Interlocked.CompareExchange<EventHandler<StatusCodeEventArgs>>(ref this.m_OnStatusCode, val3, val2);
				}
				while (val != val2);
			}
		}

		public void RegisterSurface(Surface surface)
		{
			lock (_lock)
			{
				if (!_decoders.ContainsKey(surface))
				{
					_decoders.Add(surface, (Decoder)null);
				}
			}
		}

		public void UnregisterSurface(Surface surface)
		{
			lock (_lock)
			{
				Decoder val = default(Decoder);
				if (_decoders.TryGetValue(surface, ref val) && val != null)
				{
					CleanUpDecoder(val);
				}
				_decoders.Remove(surface);
			}
		}

		public void UnmuteAudio()
		{
			_audioPlayer.Start();
		}

		public void MuteAudio()
		{
			_audioPlayer.Stop();
		}

		public bool Connect(string password, uint heartbeatTimeoutMs)
		{
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected O, but got Unknown
			RealCam.SetWEP(string.IsNullOrWhiteSpace(password) ? null : Encoding.UTF8.GetBytes(password));
			_realCamCallback = new RealCamCallback((ILogger)(object)_logger, OnVideo, OnAudio, [CompilerGenerated] (byte[] data) =>
			{
				this.OnHeartbeat?.Invoke((object)this, new HeartbeatEventArgs
				{
					Data = data
				});
			}, (Action)([CompilerGenerated] () =>
			{
				this.OnHeartbeatError?.Invoke((object)this, new HeartbeatErrorEventArgs());
			}), [CompilerGenerated] (StatusCode code, byte[] data) =>
			{
				this.OnStatusCode?.Invoke((object)this, new StatusCodeEventArgs
				{
					StatusCode = code,
					Data = data
				});
			});
			RealCam.SetCallback((ICallback)(object)_realCamCallback);
			RealCam.SetHeartBeatSleep(RealCam.MSyncThreadSleep, Encoding.ASCII.GetBytes(((object)RealCam.BeaconData).ToString()));
			if (!RealCam.IsInit)
			{
				Activity currentActivity = Platform.CurrentActivity;
				RealCam.LibInit((currentActivity != null) ? ((Context)currentActivity).ApplicationContext : null, (int)heartbeatTimeoutMs);
			}
			RealCam.SendStartCmd(RealCam.IntToByteShort((short)RealCam.UdpPort));
			RealCam.SendSyncCmd();
			return true;
		}

		void IInsightServiceInternal.Disconnect()
		{
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			_audioPlayer.Stop();
			RealCam.StopSyncCmd();
			RealCam.StopStartCmd();
			RealCam.SendStopCmd();
			RealCam.LibUninit();
			RealCam.SetCallback((ICallback)null);
			RealCamCallback? realCamCallback = _realCamCallback;
			if (realCamCallback != null)
			{
				((Object)realCamCallback).Dispose();
			}
			_realCamCallback = null;
			_ppsspsInit = false;
			lock (_lock)
			{
				Enumerator<KeyValuePair<Surface, Decoder>> enumerator = Enumerable.ToList<KeyValuePair<Surface, Decoder>>((global::System.Collections.Generic.IEnumerable<KeyValuePair<Surface, Decoder>>)_decoders).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<Surface, Decoder> current = enumerator.Current;
						if (current.Value != null)
						{
							RemoveDecoderForSurface(current.Key);
						}
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
				}
			}
		}

		public void SetPassword(string password)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(password);
			byte b = (byte)bytes.Length;
			byte[] array = new byte[b + 1];
			array[0] = b;
			Buffer.BlockCopy((global::System.Array)bytes, 0, (global::System.Array)array, 1, (int)b);
			SetParameter(144, array);
		}

		public void SetParameter(byte parameter, byte value)
		{
			byte[] data = new byte[2] { 0, value };
			SetParameter(parameter, data);
		}

		public void SetParameter(byte parameter, byte[] data)
		{
			RealCam.SendTWCCmd((sbyte)parameter, data);
		}

		public void GetParameter(byte parameter)
		{
			RealCam.SendTWCCmd((sbyte)parameter, GetParameterData);
		}

		private void OnAudio(long timeStamp, byte[] audioData)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Expected O, but got Unknown
			AudioObject val = new AudioObject(audioData, timeStamp);
			byte[] data = val.GetData();
			int num = ((data != null) ? data.Length : 0);
			if (RealCam.IsInit && ((val != null) ? val.GetData() : null) != null && num > 0 && num % 40 == 0)
			{
				_audioPlayer.Enqueue(val);
			}
		}

		private void OnVideo(long p0, byte[] p1)
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Expected O, but got Unknown
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			if (!_ppsspsInit)
			{
				if (p1[4] != 103)
				{
					return;
				}
				_videoHeader.ParseHeader(p1);
				_ppsspsInit = true;
			}
			if (!_ppsspsInit)
			{
				return;
			}
			lock (_lock)
			{
				List<ValueTuple<Surface, Decoder>> val = null;
				Enumerator<Surface, Decoder> enumerator = _decoders.GetEnumerator();
				try
				{
					Surface val2 = default(Surface);
					Decoder val3 = default(Decoder);
					while (enumerator.MoveNext())
					{
						enumerator.Current.Deconstruct(ref val2, ref val3);
						Surface val4 = val2;
						Decoder val5 = val3;
						if (!val4.IsValid)
						{
							continue;
						}
						if (val5 == null)
						{
							DecoderNotify decoderNotify = new DecoderNotify(val4);
							global::System.IDisposable disposable = ObservableExtensions.Subscribe<EventPattern<DecoderErrorEventArgs>>(Observable.FromEventPattern<DecoderErrorEventArgs>((Action<EventHandler<DecoderErrorEventArgs>>)delegate(EventHandler<DecoderErrorEventArgs> h)
							{
								decoderNotify.OnDecoderError += h;
							}, (Action<EventHandler<DecoderErrorEventArgs>>)delegate(EventHandler<DecoderErrorEventArgs> h)
							{
								decoderNotify.OnDecoderError -= h;
							}), (Action<EventPattern<DecoderErrorEventArgs>>)([CompilerGenerated] (EventPattern<DecoderErrorEventArgs> ep) =>
							{
								RemoveDecoderForSurface(((EventPattern<object, DecoderErrorEventArgs>)(object)ep).EventArgs.Surface);
							}));
							val5 = new Decoder(val4, 1, (INotify)(object)decoderNotify);
							_decoderDisposables.Add(val5, new ValueTuple<DecoderNotify, global::System.IDisposable>(decoderNotify, disposable));
							val5.Initial(_videoHeader.Width, _videoHeader.Height, 15, _videoHeader.GetSPS(), _videoHeader.GetPPS());
							(val ?? (val = new List<ValueTuple<Surface, Decoder>>())).Add(new ValueTuple<Surface, Decoder>(val4, val5));
						}
						val5.Decode(p1);
					}
				}
				finally
				{
					((global::System.IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
				}
				if (val != null)
				{
					Enumerator<ValueTuple<Surface, Decoder>> enumerator2 = val.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							ValueTuple<Surface, Decoder> current = enumerator2.Current;
							Surface item = current.Item1;
							Decoder item2 = current.Item2;
							_decoders[item] = item2;
						}
					}
					finally
					{
						((global::System.IDisposable)enumerator2/*cast due to .constrained prefix*/).Dispose();
					}
				}
			}
			this.OnVideoFrameReceived?.Invoke((object)this, new VideoFrameReceivedEventArgs());
		}

		private void RemoveDecoderForSurface(Surface surface)
		{
			lock (_lock)
			{
				Decoder val = default(Decoder);
				if (_decoders.TryGetValue(surface, ref val) && val != null)
				{
					CleanUpDecoder(val);
					_decoders[surface] = null;
				}
			}
		}

		private void CleanUpDecoder(Decoder decoder)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			lock (_lock)
			{
				ValueTuple<DecoderNotify, global::System.IDisposable> val = default(ValueTuple<DecoderNotify, global::System.IDisposable>);
				if (_decoderDisposables.TryGetValue(decoder, ref val))
				{
					val.Item2?.Dispose();
					DecoderNotify item = val.Item1;
					if (item != null)
					{
						((Object)item).Dispose();
					}
					_decoderDisposables.Remove(decoder);
				}
				decoder.StopRunning();
				((Object)decoder).Dispose();
			}
		}

		public InsightService(ILogger<InsightService> logger)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			_logger = logger;
			((IInsightServiceInternal)this).OnStatusCode += OnStatusCodeReceived;
		}

		[AsyncStateMachine(typeof(<ConnectAsync>d__53))]
		public async global::System.Threading.Tasks.Task<ConnectResult> ConnectAsync(string password, CancellationToken ct, uint videoFrameTimeoutMs = 1000u, uint heartbeatTimeoutMs = 5000u)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			CancellationToken connectionCt = CancellationToken.None;
			int instance = _instance++;
			LoggerExtensions.LogInformation((ILogger)(object)_logger, "##### {0}: Started ({1})", new object[2] { "ConnectAsync", instance });
			global::System.Threading.Tasks.Task<ConnectResult> task;
			lock (_connectionLock)
			{
				if (_connectionTask == null)
				{
					if (Connected)
					{
						LoggerExtensions.LogDebug((ILogger)(object)_logger, "Already connected to camera", global::System.Array.Empty<object>());
						task = global::System.Threading.Tasks.Task.FromResult<ConnectResult>(ConnectResult.Success);
					}
					else
					{
						LoggerExtensions.LogDebug((ILogger)(object)_logger, "Connecting to camera", global::System.Array.Empty<object>());
						try
						{
							CancellationTokenSource? connectionCts = _connectionCts;
							if (connectionCts != null)
							{
								connectionCts.Cancel();
							}
						}
						catch
						{
						}
						try
						{
							CancellationTokenSource? connectionCts2 = _connectionCts;
							if (connectionCts2 != null)
							{
								connectionCts2.Dispose();
							}
						}
						catch
						{
						}
						_connectionCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
						connectionCt = _connectionCts.Token;
						_connectionTask = ConnectAsync(password, videoFrameTimeoutMs, heartbeatTimeoutMs, connectionCt);
						task = _connectionTask;
					}
				}
				else
				{
					LoggerExtensions.LogDebug((ILogger)(object)_logger, "Already connecting to camera", global::System.Array.Empty<object>());
					task = _connectionTask;
				}
			}
			ConnectResult connectResult;
			try
			{
				connectResult = await task;
			}
			finally
			{
				lock (_connectionLock)
				{
					_connectionTask = null;
				}
			}
			LoggerExtensions.LogInformation((ILogger)(object)_logger, "##### {0}: Finished ({1}) - connected={2}", new object[3] { "ConnectAsync", instance, connectResult });
			if (connectResult == ConnectResult.Success)
			{
				GetFirmwareVersionAsync(connectionCt).ContinueWith((Action<global::System.Threading.Tasks.Task<Version>>)delegate(global::System.Threading.Tasks.Task<Version> task2)
				{
					//IL_0057: Unknown result type (might be due to invalid IL or missing references)
					Version result = task2.Result;
					LoggerExtensions.LogInformation((ILogger)(object)_logger, "{0}: Firmware Version = {1}", new object[2]
					{
						"ConnectAsync",
						((object)result).ToString()
					});
					if (result.Major >= 21 || (result.Major == 21 && result.Minor >= 3))
					{
						GetAspectRatio(connectionCt).ContinueWith((Action<global::System.Threading.Tasks.Task<double>>)delegate(global::System.Threading.Tasks.Task<double> task3)
						{
							AspectRatio = task3.Result;
							LoggerExtensions.LogInformation((ILogger)(object)_logger, "{0}: Aspect Ratio = {1}", new object[2] { "ConnectAsync", AspectRatio });
							this.OnAspectRatio?.Invoke((object)this, AspectRatio);
						});
					}
					else
					{
						LoggerExtensions.LogDebug((ILogger)(object)_logger, "Aspect Ratio is not supported by this Firmware {0}", new object[1] { ((object)result).ToString() });
					}
				});
			}
			return connectResult;
		}

		public void Disconnect()
		{
			Disconnect(DisconnectReason.Normal);
		}

		[AsyncStateMachine(typeof(<SetPassword>d__55))]
		public async global::System.Threading.Tasks.Task<bool> SetPassword(string password, CancellationToken ct)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			if (!Connected)
			{
				return false;
			}
			if (password.Length > 20)
			{
				throw new ArgumentOutOfRangeException("password cannot exceed 20 characters");
			}
			SetPassword(password);
			await Observable.RunAsync<EventPattern<StatusCodeEventArgs>>(Observable.FirstAsync<EventPattern<StatusCodeEventArgs>>(Observable.Where<EventPattern<StatusCodeEventArgs>>(Observable.FromEventPattern<StatusCodeEventArgs>((Action<EventHandler<StatusCodeEventArgs>>)([CompilerGenerated] (EventHandler<StatusCodeEventArgs> h) =>
			{
				((IInsightServiceInternal)this).OnStatusCode += h;
			}), (Action<EventHandler<StatusCodeEventArgs>>)([CompilerGenerated] (EventHandler<StatusCodeEventArgs> h) =>
			{
				((IInsightServiceInternal)this).OnStatusCode -= h;
			})), (Func<EventPattern<StatusCodeEventArgs>, bool>)((EventPattern<StatusCodeEventArgs> ep) => ((EventPattern<object, StatusCodeEventArgs>)(object)ep).EventArgs.StatusCode == StatusCode.SetPassword)), (Func<EventPattern<StatusCodeEventArgs>, bool>)([CompilerGenerated] (EventPattern<StatusCodeEventArgs> ep) => _setPassword = true)), ct);
			return true;
		}

		[AsyncStateMachine(typeof(<ChangeRFModeAsync>d__56))]
		public async global::System.Threading.Tasks.Task<bool> ChangeRFModeAsync(CancellationToken ct)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			if (!Connected)
			{
				return false;
			}
			SetParameter(142, new byte[1]);
			await Observable.RunAsync<EventPattern<StatusCodeEventArgs>>(Observable.FirstAsync<EventPattern<StatusCodeEventArgs>>(Observable.Where<EventPattern<StatusCodeEventArgs>>(Observable.FromEventPattern<StatusCodeEventArgs>((Action<EventHandler<StatusCodeEventArgs>>)([CompilerGenerated] (EventHandler<StatusCodeEventArgs> h) =>
			{
				((IInsightServiceInternal)this).OnStatusCode += h;
			}), (Action<EventHandler<StatusCodeEventArgs>>)([CompilerGenerated] (EventHandler<StatusCodeEventArgs> h) =>
			{
				((IInsightServiceInternal)this).OnStatusCode -= h;
			})), (Func<EventPattern<StatusCodeEventArgs>, bool>)((EventPattern<StatusCodeEventArgs> ep) => ((EventPattern<object, StatusCodeEventArgs>)(object)ep).EventArgs.StatusCode == StatusCode.ChangeRFMode)), (Func<EventPattern<StatusCodeEventArgs>, bool>)([CompilerGenerated] (EventPattern<StatusCodeEventArgs> ep) => _changedRFMode = true)), ct);
			return true;
		}

		[AsyncStateMachine(typeof(<EnterPairingModeAsync>d__57))]
		public async global::System.Threading.Tasks.Task<bool> EnterPairingModeAsync(CancellationToken ct)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			if (!Connected)
			{
				return false;
			}
			SetParameter(145, new byte[1]);
			await Observable.RunAsync<EventPattern<StatusCodeEventArgs>>(Observable.FirstAsync<EventPattern<StatusCodeEventArgs>>(Observable.Where<EventPattern<StatusCodeEventArgs>>(Observable.FromEventPattern<StatusCodeEventArgs>((Action<EventHandler<StatusCodeEventArgs>>)([CompilerGenerated] (EventHandler<StatusCodeEventArgs> h) =>
			{
				((IInsightServiceInternal)this).OnStatusCode += h;
			}), (Action<EventHandler<StatusCodeEventArgs>>)([CompilerGenerated] (EventHandler<StatusCodeEventArgs> h) =>
			{
				((IInsightServiceInternal)this).OnStatusCode -= h;
			})), (Func<EventPattern<StatusCodeEventArgs>, bool>)((EventPattern<StatusCodeEventArgs> ep) => ((EventPattern<object, StatusCodeEventArgs>)(object)ep).EventArgs.StatusCode == StatusCode.EnterPairingMode)), (Func<EventPattern<StatusCodeEventArgs>, bool>)([CompilerGenerated] (EventPattern<StatusCodeEventArgs> ep) => _enteredPairingMode = true)), ct);
			return true;
		}

		public global::System.Threading.Tasks.Task<Version> GetFirmwareVersionAsync(CancellationToken ct)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			if (!Connected)
			{
				return global::System.Threading.Tasks.Task.FromResult<Version>(new Version(0, 0, 0, 0));
			}
			return TaskObservableExtensions.ToTask<Version>(Observable.FirstAsync<Version>(Observable.Select<EventPattern<HeartbeatEventArgs>, Version>(Observable.Where<EventPattern<HeartbeatEventArgs>>(Observable.FromEventPattern<HeartbeatEventArgs>((Action<EventHandler<HeartbeatEventArgs>>)([CompilerGenerated] (EventHandler<HeartbeatEventArgs> h) =>
			{
				((IInsightServiceInternal)this).OnHeartbeat += h;
			}), (Action<EventHandler<HeartbeatEventArgs>>)([CompilerGenerated] (EventHandler<HeartbeatEventArgs> h) =>
			{
				((IInsightServiceInternal)this).OnHeartbeat -= h;
			})), (Func<EventPattern<HeartbeatEventArgs>, bool>)((EventPattern<HeartbeatEventArgs> ep) => global::System.Array.IndexOf<byte>(((EventPattern<object, HeartbeatEventArgs>)(object)ep).EventArgs.Data, (byte)0) >= 9)), (Func<EventPattern<HeartbeatEventArgs>, Version>)delegate(EventPattern<HeartbeatEventArgs> ep)
			{
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Expected O, but got Unknown
				byte[] data = ((EventPattern<object, HeartbeatEventArgs>)(object)ep).EventArgs.Data;
				int num = global::System.Array.IndexOf<byte>(data, (byte)0);
				return new Version(Encoding.UTF8.GetString(data, 1, num - 2));
			})), ct);
		}

		[AsyncStateMachine(typeof(<GetBrightnessAsync>d__59))]
		public async global::System.Threading.Tasks.Task<byte> GetBrightnessAsync(CancellationToken ct)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			return await GetParameterAsync(StatusCode.Brightness, ct);
		}

		[AsyncStateMachine(typeof(<SetBrightnessAsync>d__60))]
		public async global::System.Threading.Tasks.Task<bool> SetBrightnessAsync(byte value, CancellationToken ct)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			return await SetParameterAsync(StatusCode.Brightness, value, ct);
		}

		[AsyncStateMachine(typeof(<GetChromaAsync>d__61))]
		public async global::System.Threading.Tasks.Task<byte> GetChromaAsync(CancellationToken ct)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			return await GetParameterAsync(StatusCode.Chroma, ct);
		}

		[AsyncStateMachine(typeof(<SetChromaAsync>d__62))]
		public async global::System.Threading.Tasks.Task<bool> SetChromaAsync(byte value, CancellationToken ct)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			return await SetParameterAsync(StatusCode.Chroma, value, ct);
		}

		[AsyncStateMachine(typeof(<GetContrastAsync>d__63))]
		public async global::System.Threading.Tasks.Task<byte> GetContrastAsync(CancellationToken ct)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			return await GetParameterAsync(StatusCode.Contrast, ct);
		}

		[AsyncStateMachine(typeof(<SetContrastAsync>d__64))]
		public async global::System.Threading.Tasks.Task<bool> SetContrastAsync(byte value, CancellationToken ct)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			return await SetParameterAsync(StatusCode.Contrast, value, ct);
		}

		[AsyncStateMachine(typeof(<GetSaturationAsync>d__65))]
		public async global::System.Threading.Tasks.Task<byte> GetSaturationAsync(CancellationToken ct)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			return await GetParameterAsync(StatusCode.Saturation, ct);
		}

		[AsyncStateMachine(typeof(<SetSaturationAsync>d__66))]
		public async global::System.Threading.Tasks.Task<bool> SetSaturationAsync(byte value, CancellationToken ct)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			return await SetParameterAsync(StatusCode.Saturation, value, ct);
		}

		[AsyncStateMachine(typeof(<GetMirrorFlipAsync>d__67))]
		public async global::System.Threading.Tasks.Task<byte> GetMirrorFlipAsync(CancellationToken ct)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			return await GetParameterAsync(StatusCode.MirrorFlip, ct);
		}

		[AsyncStateMachine(typeof(<SetMirrorFlipAsync>d__68))]
		public async global::System.Threading.Tasks.Task<bool> SetMirrorFlipAsync(CameraMirrorFlip value, CancellationToken ct)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			return await SetParameterAsync(StatusCode.MirrorFlip, (byte)value, ct);
		}

		public global::System.Threading.Tasks.Task<double> GetAspectRatio(CancellationToken ct)
		{
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			if (!Connected)
			{
				return global::System.Threading.Tasks.Task.FromResult<double>(AspectRatio);
			}
			GetParameter(146);
			return TaskObservableExtensions.ToTask<double>(Observable.FirstAsync<double>(Observable.Select<EventPattern<StatusCodeEventArgs>, double>(Observable.Where<EventPattern<StatusCodeEventArgs>>(Observable.FromEventPattern<StatusCodeEventArgs>((Action<EventHandler<StatusCodeEventArgs>>)([CompilerGenerated] (EventHandler<StatusCodeEventArgs> h) =>
			{
				((IInsightServiceInternal)this).OnStatusCode += h;
			}), (Action<EventHandler<StatusCodeEventArgs>>)([CompilerGenerated] (EventHandler<StatusCodeEventArgs> h) =>
			{
				((IInsightServiceInternal)this).OnStatusCode -= h;
			})), (Func<EventPattern<StatusCodeEventArgs>, bool>)((EventPattern<StatusCodeEventArgs> ep) => ((EventPattern<object, StatusCodeEventArgs>)(object)ep).EventArgs.StatusCode == StatusCode.GetResolution && ((EventPattern<object, StatusCodeEventArgs>)(object)ep).EventArgs.Data.Length >= 4)), (Func<EventPattern<StatusCodeEventArgs>, double>)([CompilerGenerated] (EventPattern<StatusCodeEventArgs> ep) =>
			{
				byte[] data = ((EventPattern<object, StatusCodeEventArgs>)(object)ep).EventArgs.Data;
				byte[] array = new byte[2];
				global::System.Array.Copy((global::System.Array)data, (global::System.Array)array, 2);
				short num = BitConverter.ToInt16(array, 0);
				global::System.Array.Copy((global::System.Array)data, 2, (global::System.Array)array, 0, 2);
				short num2 = BitConverter.ToInt16(array, 0);
				double num3 = (double)num / (double)num2;
				LoggerExtensions.LogDebug((ILogger)(object)_logger, "Camera Resolution {0}x{1} - Aspect {2}", new object[3] { num, num2, num3 });
				return num3;
			}))), ct);
		}

		[AsyncStateMachine(typeof(<ConnectAsync>d__107))]
		private async global::System.Threading.Tasks.Task<ConnectResult> ConnectAsync(string password, uint videoFrameTimeoutMs, uint heartbeatTimeoutMs, CancellationToken ct)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			LoggerExtensions.LogDebug((ILogger)(object)_logger, "connecting to camera", global::System.Array.Empty<object>());
			CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
			try
			{
				global::System.Threading.Tasks.Task<EventPattern<HeartbeatErrorEventArgs>> heartbeatFailedTask = TaskObservableExtensions.ToTask<EventPattern<HeartbeatErrorEventArgs>>(Observable.FirstAsync<EventPattern<HeartbeatErrorEventArgs>>(Observable.FromEventPattern<HeartbeatErrorEventArgs>((Action<EventHandler<HeartbeatErrorEventArgs>>)([CompilerGenerated] (EventHandler<HeartbeatErrorEventArgs> h) =>
				{
					((IInsightServiceInternal)this).OnHeartbeatError += h;
				}), (Action<EventHandler<HeartbeatErrorEventArgs>>)([CompilerGenerated] (EventHandler<HeartbeatErrorEventArgs> h) =>
				{
					((IInsightServiceInternal)this).OnHeartbeatError -= h;
				}))), linkedCts.Token);
				global::System.Threading.Tasks.Task<EventPattern<StatusCodeEventArgs>> certificateErrorTask = TaskObservableExtensions.ToTask<EventPattern<StatusCodeEventArgs>>(Observable.FirstAsync<EventPattern<StatusCodeEventArgs>>(Observable.Where<EventPattern<StatusCodeEventArgs>>(Observable.FromEventPattern<StatusCodeEventArgs>((Action<EventHandler<StatusCodeEventArgs>>)([CompilerGenerated] (EventHandler<StatusCodeEventArgs> h) =>
				{
					((IInsightServiceInternal)this).OnStatusCode += h;
				}), (Action<EventHandler<StatusCodeEventArgs>>)([CompilerGenerated] (EventHandler<StatusCodeEventArgs> h) =>
				{
					((IInsightServiceInternal)this).OnStatusCode -= h;
				})), (Func<EventPattern<StatusCodeEventArgs>, bool>)((EventPattern<StatusCodeEventArgs> ep) => ((EventPattern<object, StatusCodeEventArgs>)(object)ep).EventArgs.StatusCode == StatusCode.VideoStopped && ((EventPattern<object, StatusCodeEventArgs>)(object)ep).EventArgs.Data.Length != 0 && ((EventPattern<object, StatusCodeEventArgs>)(object)ep).EventArgs.Data[0] == 2))), linkedCts.Token);
				global::System.Threading.Tasks.Task<EventPattern<StatusCodeEventArgs>> resourceUnavailableErrorTask = TaskObservableExtensions.ToTask<EventPattern<StatusCodeEventArgs>>(Observable.FirstAsync<EventPattern<StatusCodeEventArgs>>(Observable.Where<EventPattern<StatusCodeEventArgs>>(Observable.FromEventPattern<StatusCodeEventArgs>((Action<EventHandler<StatusCodeEventArgs>>)([CompilerGenerated] (EventHandler<StatusCodeEventArgs> h) =>
				{
					((IInsightServiceInternal)this).OnStatusCode += h;
				}), (Action<EventHandler<StatusCodeEventArgs>>)([CompilerGenerated] (EventHandler<StatusCodeEventArgs> h) =>
				{
					((IInsightServiceInternal)this).OnStatusCode -= h;
				})), (Func<EventPattern<StatusCodeEventArgs>, bool>)((EventPattern<StatusCodeEventArgs> ep) => ((EventPattern<object, StatusCodeEventArgs>)(object)ep).EventArgs.StatusCode == StatusCode.VideoStopped && ((EventPattern<object, StatusCodeEventArgs>)(object)ep).EventArgs.Data.Length != 0 && ((EventPattern<object, StatusCodeEventArgs>)(object)ep).EventArgs.Data[0] == 1))), linkedCts.Token);
				global::System.Threading.Tasks.Task<EventPattern<HeartbeatEventArgs>> task = TaskObservableExtensions.ToTask<EventPattern<HeartbeatEventArgs>>(Observable.FirstAsync<EventPattern<HeartbeatEventArgs>>(Observable.Skip<EventPattern<HeartbeatEventArgs>>(Observable.FromEventPattern<HeartbeatEventArgs>((Action<EventHandler<HeartbeatEventArgs>>)([CompilerGenerated] (EventHandler<HeartbeatEventArgs> h) =>
				{
					((IInsightServiceInternal)this).OnHeartbeat += h;
				}), (Action<EventHandler<HeartbeatEventArgs>>)([CompilerGenerated] (EventHandler<HeartbeatEventArgs> h) =>
				{
					((IInsightServiceInternal)this).OnHeartbeat -= h;
				})), 4)), ct);
				if (!Connect(password, heartbeatTimeoutMs))
				{
					LoggerExtensions.LogError((ILogger)(object)_logger, "Failed to connect: Internal connection error", global::System.Array.Empty<object>());
					Disconnect(null);
					return ConnectResult.Failed;
				}
				<>y__InlineArray4<global::System.Threading.Tasks.Task> buffer = default(<>y__InlineArray4<global::System.Threading.Tasks.Task>);
				<PrivateImplementationDetails>.InlineArrayElementRef<<>y__InlineArray4<global::System.Threading.Tasks.Task>, global::System.Threading.Tasks.Task>(ref buffer, 0) = heartbeatFailedTask;
				<PrivateImplementationDetails>.InlineArrayElementRef<<>y__InlineArray4<global::System.Threading.Tasks.Task>, global::System.Threading.Tasks.Task>(ref buffer, 1) = certificateErrorTask;
				<PrivateImplementationDetails>.InlineArrayElementRef<<>y__InlineArray4<global::System.Threading.Tasks.Task>, global::System.Threading.Tasks.Task>(ref buffer, 2) = resourceUnavailableErrorTask;
				<PrivateImplementationDetails>.InlineArrayElementRef<<>y__InlineArray4<global::System.Threading.Tasks.Task>, global::System.Threading.Tasks.Task>(ref buffer, 3) = task;
				await global::System.Threading.Tasks.Task.WhenAny(<PrivateImplementationDetails>.InlineArrayAsReadOnlySpan<<>y__InlineArray4<global::System.Threading.Tasks.Task>, global::System.Threading.Tasks.Task>(in buffer, 4));
				if (((global::System.Threading.Tasks.Task)heartbeatFailedTask).IsCompleted)
				{
					LoggerExtensions.LogError((ILogger)(object)_logger, "Failed to connect: No heartbeat received", global::System.Array.Empty<object>());
					return ConnectResult.Timeout;
				}
				if (((global::System.Threading.Tasks.Task)certificateErrorTask).IsCompleted)
				{
					LoggerExtensions.LogError((ILogger)(object)_logger, "Failed to connect: Certificate error", global::System.Array.Empty<object>());
					return ConnectResult.InvalidPassword;
				}
				if (((global::System.Threading.Tasks.Task)resourceUnavailableErrorTask).IsCompleted)
				{
					LoggerExtensions.LogError((ILogger)(object)_logger, "Failed to connect: Resource unavailable error", global::System.Array.Empty<object>());
					return ConnectResult.CameraUnavailable;
				}
				_onHeartbeatFailedDisposable = ObservableExtensions.Subscribe<EventPattern<HeartbeatErrorEventArgs>>(Observable.FromEventPattern<HeartbeatErrorEventArgs>((Action<EventHandler<HeartbeatErrorEventArgs>>)([CompilerGenerated] (EventHandler<HeartbeatErrorEventArgs> h) =>
				{
					((IInsightServiceInternal)this).OnHeartbeatError += h;
				}), (Action<EventHandler<HeartbeatErrorEventArgs>>)([CompilerGenerated] (EventHandler<HeartbeatErrorEventArgs> h) =>
				{
					((IInsightServiceInternal)this).OnHeartbeatError -= h;
				})), (Action<EventPattern<HeartbeatErrorEventArgs>>)([CompilerGenerated] (EventPattern<HeartbeatErrorEventArgs> ep) =>
				{
					LoggerExtensions.LogError((ILogger)(object)_logger, "Disconnecting: No heartbeat received", global::System.Array.Empty<object>());
					Disconnect(DisconnectReason.HeartbeatFailed);
				}));
				Timer videoWatchdog = new Timer((double)videoFrameTimeoutMs);
				_onVideoWatchdogElapsedDisposable = ObservableExtensions.Subscribe<EventPattern<ElapsedEventArgs>>(Observable.FromEventPattern<ElapsedEventHandler, ElapsedEventArgs>((Action<ElapsedEventHandler>)delegate(ElapsedEventHandler h)
				{
					videoWatchdog.Elapsed += h;
				}, (Action<ElapsedEventHandler>)delegate(ElapsedEventHandler h)
				{
					videoWatchdog.Elapsed -= h;
				}), (Action<EventPattern<ElapsedEventArgs>>)([CompilerGenerated] (EventPattern<ElapsedEventArgs> ep) =>
				{
					LoggerExtensions.LogError((ILogger)(object)_logger, "Disconnecting: No video frame received", global::System.Array.Empty<object>());
					Disconnect(DisconnectReason.VideoLost);
				}));
				videoWatchdog.AutoReset = false;
				videoWatchdog.Start();
				_videoWatchdog = videoWatchdog;
				_onVideoFrameReceivedDisposable = ObservableExtensions.Subscribe<EventPattern<VideoFrameReceivedEventArgs>>(Observable.FromEventPattern<VideoFrameReceivedEventArgs>((Action<EventHandler<VideoFrameReceivedEventArgs>>)([CompilerGenerated] (EventHandler<VideoFrameReceivedEventArgs> h) =>
				{
					((IInsightServiceInternal)this).OnVideoFrameReceived += h;
				}), (Action<EventHandler<VideoFrameReceivedEventArgs>>)([CompilerGenerated] (EventHandler<VideoFrameReceivedEventArgs> h) =>
				{
					((IInsightServiceInternal)this).OnVideoFrameReceived -= h;
				})), (Action<EventPattern<VideoFrameReceivedEventArgs>>)delegate
				{
					videoWatchdog.Stop();
					videoWatchdog.Start();
				});
				lock (_connectionLock)
				{
					Connected = true;
				}
			}
			catch (global::System.Exception ex)
			{
				if (linkedCts.IsCancellationRequested)
				{
					LoggerExtensions.LogWarning((ILogger)(object)_logger, ex, "Connection to camera cancelled", global::System.Array.Empty<object>());
					return ConnectResult.Cancelled;
				}
				LoggerExtensions.LogError((ILogger)(object)_logger, ex, "Error connecting to camera", global::System.Array.Empty<object>());
				return ConnectResult.Failed;
			}
			finally
			{
				try
				{
					linkedCts.Cancel();
				}
				catch
				{
				}
				try
				{
					linkedCts.Dispose();
				}
				catch
				{
				}
				lock (_connectionLock)
				{
					if (!Connected)
					{
						Disconnect(null);
					}
				}
			}
			return (!Connected) ? ConnectResult.Failed : ConnectResult.Success;
		}

		private void Disconnect(DisconnectReason? reason)
		{
			int num = _instance++;
			LoggerExtensions.LogInformation((ILogger)(object)_logger, "##### {0}: Started ({1})", new object[2] { "Disconnect", num });
			lock (_connectionLock)
			{
				try
				{
					CancellationTokenSource? connectionCts = _connectionCts;
					if (connectionCts != null)
					{
						connectionCts.Cancel();
					}
				}
				catch
				{
				}
				try
				{
					CancellationTokenSource? connectionCts2 = _connectionCts;
					if (connectionCts2 != null)
					{
						connectionCts2.Dispose();
					}
				}
				catch
				{
				}
				Connected = false;
				if (_setPassword)
				{
					reason = DisconnectReason.PasswordSet;
				}
				if (_changedRFMode)
				{
					reason = DisconnectReason.ChangedRFMode;
				}
				if (_enteredPairingMode)
				{
					reason = DisconnectReason.EnteredPairingMode;
				}
				_setPassword = false;
				_changedRFMode = false;
				_enteredPairingMode = false;
				_onConnectionLostDisposable?.Dispose();
				_onConnectionLostDisposable = null;
				_onVideoFrameReceivedDisposable?.Dispose();
				_onVideoFrameReceivedDisposable = null;
				_onVideoWatchdogElapsedDisposable?.Dispose();
				_onVideoWatchdogElapsedDisposable = null;
				Timer? videoWatchdog = _videoWatchdog;
				if (videoWatchdog != null)
				{
					videoWatchdog.Stop();
				}
				Timer? videoWatchdog2 = _videoWatchdog;
				if (videoWatchdog2 != null)
				{
					((Component)videoWatchdog2).Dispose();
				}
				_videoWatchdog = null;
				_onHeartbeatFailedDisposable?.Dispose();
				_onHeartbeatFailedDisposable = null;
				((IInsightServiceInternal)this).Disconnect();
			}
			if (reason.HasValue)
			{
				this.OnCameraDisconnected?.Invoke((object)this, new CameraDisconnectedEventArgs
				{
					Reason = reason.Value
				});
			}
			LoggerExtensions.LogInformation((ILogger)(object)_logger, "##### {0}: Finished ({1})", new object[2] { "Disconnect", num });
		}

		private global::System.Threading.Tasks.Task<byte> GetParameterAsync(StatusCode statusCode, CancellationToken ct)
		{
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				GetParameter((byte)statusCode);
				return TaskObservableExtensions.ToTask<byte>(Observable.FirstAsync<byte>(Observable.Timeout<byte>(Observable.Select<EventPattern<StatusCodeEventArgs>, byte>(Observable.Where<EventPattern<StatusCodeEventArgs>>(Observable.FromEventPattern<StatusCodeEventArgs>((Action<EventHandler<StatusCodeEventArgs>>)delegate(EventHandler<StatusCodeEventArgs> h)
				{
					((IInsightServiceInternal)this).OnStatusCode += h;
				}, (Action<EventHandler<StatusCodeEventArgs>>)delegate(EventHandler<StatusCodeEventArgs> h)
				{
					((IInsightServiceInternal)this).OnStatusCode -= h;
				}), (Func<EventPattern<StatusCodeEventArgs>, bool>)((EventPattern<StatusCodeEventArgs> ep) => ((EventPattern<object, StatusCodeEventArgs>)(object)ep).EventArgs.StatusCode == statusCode)), (Func<EventPattern<StatusCodeEventArgs>, byte>)((EventPattern<StatusCodeEventArgs> ep) => ((EventPattern<object, StatusCodeEventArgs>)(object)ep).EventArgs.Data[0])), TimeSpan.FromSeconds(3L))), ct);
			}
			catch
			{
				LoggerExtensions.LogDebug((ILogger)(object)_logger, "Error getting {0}", new object[1] { statusCode });
				return global::System.Threading.Tasks.Task.FromResult<byte>((byte)0);
			}
		}

		private global::System.Threading.Tasks.Task<bool> SetParameterAsync(StatusCode statusCode, byte value, CancellationToken ct)
		{
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				SetParameter((byte)statusCode, value);
				return TaskObservableExtensions.ToTask<bool>(Observable.FirstAsync<bool>(Observable.Timeout<bool>(Observable.Select<EventPattern<StatusCodeEventArgs>, bool>(Observable.Where<EventPattern<StatusCodeEventArgs>>(Observable.FromEventPattern<StatusCodeEventArgs>((Action<EventHandler<StatusCodeEventArgs>>)delegate(EventHandler<StatusCodeEventArgs> h)
				{
					((IInsightServiceInternal)this).OnStatusCode += h;
				}, (Action<EventHandler<StatusCodeEventArgs>>)delegate(EventHandler<StatusCodeEventArgs> h)
				{
					((IInsightServiceInternal)this).OnStatusCode -= h;
				}), (Func<EventPattern<StatusCodeEventArgs>, bool>)((EventPattern<StatusCodeEventArgs> ep) => ((EventPattern<object, StatusCodeEventArgs>)(object)ep).EventArgs.StatusCode == statusCode)), (Func<EventPattern<StatusCodeEventArgs>, bool>)((EventPattern<StatusCodeEventArgs> ep) => true)), TimeSpan.FromSeconds(3L))), ct);
			}
			catch
			{
				LoggerExtensions.LogDebug((ILogger)(object)_logger, "Error setting {0}", new object[1] { statusCode });
				return global::System.Threading.Tasks.Task.FromResult<bool>(false);
			}
		}

		private void OnStatusCodeReceived(object sender, StatusCodeEventArgs eventArgs)
		{
			switch (eventArgs.StatusCode)
			{
			case StatusCode.Brightness:
				this.OnBrightness?.Invoke((object)this, eventArgs.Data[0]);
				break;
			case StatusCode.Chroma:
				this.OnChroma?.Invoke((object)this, eventArgs.Data[0]);
				break;
			case StatusCode.Contrast:
				this.OnContrast?.Invoke((object)this, eventArgs.Data[0]);
				break;
			case StatusCode.Saturation:
				this.OnSaturation?.Invoke((object)this, eventArgs.Data[0]);
				break;
			case StatusCode.MirrorFlip:
				this.OnMirrorFlip?.Invoke((object)this, eventArgs.Data[0]);
				break;
			}
		}
	}
}
namespace ids.camera.insight.Audio
{
	internal class AudioPlayer : AudioPlayer<AudioObject>
	{
		private const int SampleRateInHz = 16000;

		private const int AudioBufSize = 16000;

		private AudioTrack _audioTrack;

		public AudioPlayer()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			int minBufferSize = AudioTrack.GetMinBufferSize(16000, (ChannelOut)4, (Encoding)2);
			_audioTrack = new AudioTrack((Stream)3, 16000, (ChannelOut)4, (Encoding)2, minBufferSize, (AudioTrackMode)1);
			RealCam.Audio32DecodeInit();
		}

		protected override void StartAudio()
		{
			_audioTrack.Play();
		}

		protected override void StopAudio()
		{
			_audioTrack.Stop();
		}

		protected override void PlayAudio(AudioObject audioObject)
		{
			global::System.Collections.Generic.IList<byte> mData = audioObject.MData;
			if (mData != null && ((global::System.Collections.Generic.ICollection<byte>)mData).Count >= 1)
			{
				byte[] array = new byte[((global::System.Collections.Generic.ICollection<byte>)mData).Count];
				((global::System.Collections.Generic.ICollection<byte>)mData).CopyTo(array, 0);
				byte[] array2 = new byte[16000];
				int num = RealCam.Audio32Decode(array, array2, ((global::System.Collections.Generic.ICollection<byte>)mData).Count);
				byte[] array3 = new byte[num];
				global::System.Array.Copy((global::System.Array)array2, 0, (global::System.Array)array3, 0, num);
				_audioTrack.Write(array3, 0, array3.Length);
			}
		}
	}
	internal abstract class AudioPlayer<T>
	{
		[CompilerGenerated]
		private sealed class <>c__DisplayClass14_0
		{
			[StructLayout((LayoutKind)3)]
			private struct <<PlayAudioTask>b__0>d : IAsyncStateMachine
			{
				public int <>1__state;

				public AsyncTaskMethodBuilder <>t__builder;

				public <>c__DisplayClass14_0 <>4__this;

				private TaskAwaiter <>u__1;

				private void MoveNext()
				{
					//IL_0071: Unknown result type (might be due to invalid IL or missing references)
					//IL_0076: Unknown result type (might be due to invalid IL or missing references)
					//IL_007d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0034: Unknown result type (might be due to invalid IL or missing references)
					//IL_003e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0043: Unknown result type (might be due to invalid IL or missing references)
					//IL_0057: Unknown result type (might be due to invalid IL or missing references)
					//IL_0058: Unknown result type (might be due to invalid IL or missing references)
					int num = <>1__state;
					<>c__DisplayClass14_0 <>c__DisplayClass14_ = <>4__this;
					try
					{
						if (num != 0)
						{
							goto IL_00b3;
						}
						TaskAwaiter val = <>u__1;
						<>u__1 = default(TaskAwaiter);
						num = (<>1__state = -1);
						goto IL_008c;
						IL_008c:
						((TaskAwaiter)(ref val)).GetResult();
						T audioData = default(T);
						if (<>c__DisplayClass14_.<>4__this._audioQueue.TryDequeue(ref audioData))
						{
							<>c__DisplayClass14_.<>4__this.PlayAudio(audioData);
						}
						goto IL_00b3;
						IL_00b3:
						if (!((CancellationToken)(ref <>c__DisplayClass14_.ct)).IsCancellationRequested && <>c__DisplayClass14_.<>4__this._audioAvailable != null)
						{
							val = <>c__DisplayClass14_.<>4__this._audioAvailable.WaitAsync(<>c__DisplayClass14_.ct).GetAwaiter();
							if (!((TaskAwaiter)(ref val)).IsCompleted)
							{
								num = (<>1__state = 0);
								<>u__1 = val;
								((AsyncTaskMethodBuilder)(ref <>t__builder)).AwaitUnsafeOnCompleted<TaskAwaiter, <<PlayAudioTask>b__0>d>(ref val, ref this);
								return;
							}
							goto IL_008c;
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

			public AudioPlayer<T> <>4__this;

			public CancellationToken ct;

			[AsyncStateMachine(typeof(AudioPlayer<>.<>c__DisplayClass14_0.<<PlayAudioTask>b__0>d))]
			internal global::System.Threading.Tasks.Task? <PlayAudioTask>b__0()
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				<<PlayAudioTask>b__0>d <<PlayAudioTask>b__0>d = default(<<PlayAudioTask>b__0>d);
				<<PlayAudioTask>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<PlayAudioTask>b__0>d.<>4__this = this;
				<<PlayAudioTask>b__0>d.<>1__state = -1;
				((AsyncTaskMethodBuilder)(ref <<PlayAudioTask>b__0>d.<>t__builder)).Start<<<PlayAudioTask>b__0>d>(ref <<PlayAudioTask>b__0>d);
				return ((AsyncTaskMethodBuilder)(ref <<PlayAudioTask>b__0>d.<>t__builder)).Task;
			}
		}

		private readonly object Lock = new object();

		private CancellationTokenSource? _playAudioTaskCts;

		private readonly ConcurrentQueue<T> _audioQueue = new ConcurrentQueue<T>();

		private SemaphoreSlim? _audioAvailable;

		public bool Started
		{
			[CompilerGenerated]
			get;
			[CompilerGenerated]
			private set;
		}

		protected abstract void StartAudio();

		protected abstract void StopAudio();

		protected abstract void PlayAudio(T audioData);

		public void Start()
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Expected O, but got Unknown
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			lock (Lock)
			{
				if (!Started)
				{
					Started = true;
					Cleanup();
					_playAudioTaskCts = new CancellationTokenSource();
					_audioAvailable = new SemaphoreSlim(0, 2147483647);
					StartAudio();
					PlayAudioTask(_playAudioTaskCts.Token);
				}
			}
		}

		public void Stop()
		{
			lock (Lock)
			{
				if (Started)
				{
					Started = false;
					Cleanup();
					StopAudio();
				}
			}
		}

		public void Enqueue(T audioData)
		{
			lock (Lock)
			{
				if (!Started)
				{
					return;
				}
				_audioQueue.Enqueue(audioData);
				try
				{
					SemaphoreSlim? audioAvailable = _audioAvailable;
					if (audioAvailable != null)
					{
						audioAvailable.Release();
					}
				}
				catch
				{
				}
			}
		}

		private void PlayAudioTask(CancellationToken ct)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			<>c__DisplayClass14_0 CS$<>8__locals4 = new <>c__DisplayClass14_0();
			CS$<>8__locals4.<>4__this = this;
			CS$<>8__locals4.ct = ct;
			global::System.Threading.Tasks.Task.Run((Func<global::System.Threading.Tasks.Task>)([AsyncStateMachine(typeof(AudioPlayer<>.<>c__DisplayClass14_0.<<PlayAudioTask>b__0>d))] () =>
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				<>c__DisplayClass14_0.<<PlayAudioTask>b__0>d <<PlayAudioTask>b__0>d = default(<>c__DisplayClass14_0.<<PlayAudioTask>b__0>d);
				<<PlayAudioTask>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<PlayAudioTask>b__0>d.<>4__this = CS$<>8__locals4;
				<<PlayAudioTask>b__0>d.<>1__state = -1;
				((AsyncTaskMethodBuilder)(ref <<PlayAudioTask>b__0>d.<>t__builder)).Start<<>c__DisplayClass14_0.<<PlayAudioTask>b__0>d>(ref <<PlayAudioTask>b__0>d);
				return ((AsyncTaskMethodBuilder)(ref <<PlayAudioTask>b__0>d.<>t__builder)).Task;
			}), CS$<>8__locals4.ct);
		}

		private void Cleanup()
		{
			lock (Lock)
			{
				try
				{
					CancellationTokenSource? playAudioTaskCts = _playAudioTaskCts;
					if (playAudioTaskCts != null)
					{
						playAudioTaskCts.Cancel();
					}
				}
				catch
				{
				}
				try
				{
					CancellationTokenSource? playAudioTaskCts2 = _playAudioTaskCts;
					if (playAudioTaskCts2 != null)
					{
						playAudioTaskCts2.Dispose();
					}
				}
				catch
				{
				}
				T val = default(T);
				while (_audioQueue.TryDequeue(ref val))
				{
				}
				try
				{
					SemaphoreSlim? audioAvailable = _audioAvailable;
					if (audioAvailable != null)
					{
						audioAvailable.Dispose();
					}
				}
				catch
				{
				}
			}
		}
	}
}
[StructLayout((LayoutKind)3)]
[InlineArray(4)]
internal struct <>y__InlineArray4<T>
{
}
