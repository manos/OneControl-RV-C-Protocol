using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Permissions;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Hardware.Usb;
using Android.Locations;
using Android.Net;
using Android.Net.Wifi;
using Android.Opengl;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Com.Android.VideoCodec;
using Com.Sonix.Decoder;
using Java.Interop;
using Java.Lang;
using Java.Net;
using Javax.Microedition.Khronos.Egl;
using Javax.Microedition.Khronos.Opengles;
using _Microsoft.Android.Resource.Designer;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(/*Could not decode attribute arguments.*/)]
[assembly: NamespaceMapping(Java = "com.android.opengl", Managed = "Com.Android.Opengl")]
[assembly: NamespaceMapping(Java = "com.android.player", Managed = "Com.Android.Player")]
[assembly: NamespaceMapping(Java = "com.android.VideoCodec", Managed = "Com.Android.VideoCodec")]
[assembly: NamespaceMapping(Java = "com.Hardware", Managed = "Com.Hardware")]
[assembly: NamespaceMapping(Java = "com.module", Managed = "Com.Module")]
[assembly: NamespaceMapping(Java = "com.rearcam", Managed = "Com.Rearcam")]
[assembly: NamespaceMapping(Java = "com.service", Managed = "Com.Service")]
[assembly: NamespaceMapping(Java = "com.sonix.decoder", Managed = "Com.Sonix.Decoder")]
[assembly: NamespaceMapping(Java = "com.sonix.library", Managed = "Com.Sonix.Library")]
[assembly: NamespaceMapping(Java = "com.sonix.UVCCam", Managed = "Com.Sonix.UVCCam")]
[assembly: TargetFramework(".NETCoreApp,Version=v9.0", FrameworkDisplayName = ".NET 9.0")]
[assembly: AssemblyCompany("ids.camera.insight.binding")]
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyFileVersion("1.1.0.0")]
[assembly: AssemblyInformationalVersion("1.1.0+1386bddfe8e3d97c4b7c90feb98d1e91162e2289")]
[assembly: AssemblyProduct("ids.camera.insight.binding")]
[assembly: AssemblyTitle("ids.camera.insight.binding")]
[assembly: TargetPlatform("Android35.0")]
[assembly: SupportedOSPlatform("Android21.0")]
[assembly: SecurityPermission((SecurityAction)8, SkipVerification = true)]
[assembly: AssemblyVersion("1.1.0.0")]
[module: UnverifiableCode]
[module: RefSafetyRules(11)]
internal delegate int _JniMarshal_PP_I(nint jnienv, nint klass);
internal delegate long _JniMarshal_PP_J(nint jnienv, nint klass);
internal delegate nint _JniMarshal_PP_L(nint jnienv, nint klass);
internal delegate void _JniMarshal_PP_V(nint jnienv, nint klass);
internal delegate bool _JniMarshal_PP_Z(nint jnienv, nint klass);
internal delegate void _JniMarshal_PPI_V(nint jnienv, nint klass, int p0);
internal delegate bool _JniMarshal_PPI_Z(nint jnienv, nint klass, int p0);
internal delegate void _JniMarshal_PPII_V(nint jnienv, nint klass, int p0, int p1);
internal delegate void _JniMarshal_PPIIIII_V(nint jnienv, nint klass, int p0, int p1, int p2, int p3, int p4);
internal delegate bool _JniMarshal_PPIIILL_Z(nint jnienv, nint klass, int p0, int p1, int p2, nint p3, nint p4);
internal delegate int _JniMarshal_PPIL_I(nint jnienv, nint klass, int p0, nint p1);
internal delegate void _JniMarshal_PPIL_V(nint jnienv, nint klass, int p0, nint p1);
internal delegate nint _JniMarshal_PPILI_L(nint jnienv, nint klass, int p0, nint p1, int p2);
internal delegate void _JniMarshal_PPILJ_V(nint jnienv, nint klass, int p0, nint p1, long p2);
internal delegate void _JniMarshal_PPJL_V(nint jnienv, nint klass, long p0, nint p1);
internal delegate int _JniMarshal_PPL_I(nint jnienv, nint klass, nint p0);
internal delegate nint _JniMarshal_PPL_L(nint jnienv, nint klass, nint p0);
internal delegate void _JniMarshal_PPL_V(nint jnienv, nint klass, nint p0);
internal delegate bool _JniMarshal_PPL_Z(nint jnienv, nint klass, nint p0);
internal delegate void _JniMarshal_PPLI_V(nint jnienv, nint klass, nint p0, int p1);
internal delegate void _JniMarshal_PPLII_V(nint jnienv, nint klass, nint p0, int p1, int p2);
internal delegate nint _JniMarshal_PPLIL_L(nint jnienv, nint klass, nint p0, int p1, nint p2);
internal delegate void _JniMarshal_PPLIL_V(nint jnienv, nint klass, nint p0, int p1, nint p2);
internal delegate void _JniMarshal_PPLL_V(nint jnienv, nint klass, nint p0, nint p1);
internal delegate void _JniMarshal_PPZ_V(nint jnienv, nint klass, bool p0);
namespace Java.Interop
{
	internal class __TypeRegistrations
	{
		public static void RegisterPackages()
		{
			TypeManager.RegisterPackages(new string[0], new Converter<string, global::System.Type>[0]);
		}

		[UnconditionalSuppressMessage("Trimming", "IL2057")]
		private static global::System.Type? Lookup(string[] mappings, string javaType)
		{
			string text = TypeManager.LookupTypeMapping(mappings, javaType);
			if (text == null)
			{
				return null;
			}
			return global::System.Type.GetType(text);
		}
	}
}
namespace Com.Sonix.UVCCam
{
	[Register("com/sonix/UVCCam/BatteryCam", DoNotGenerateAcw = true)]
	public class BatteryCam : Object
	{
		[Register("com/sonix/UVCCam/BatteryCam$AUX_INFO", DoNotGenerateAcw = true)]
		public class AUX_INFO : Object
		{
			[Register("AUDIO_FRAME")]
			public const int AudioFrame = 2;

			[Register("EVENT_DATA")]
			public const int EventData = 4;

			[Register("FW_CODE")]
			public const int FwCode = 3;

			[Register("I_FRAME")]
			public const int IFrame = 0;

			[Register("P_FRAME")]
			public const int PFrame = 1;

			private static readonly JniPeerMembers _members = (JniPeerMembers)new XAPeerMembers("com/sonix/UVCCam/BatteryCam$AUX_INFO", typeof(AUX_INFO));

			internal static nint class_ref
			{
				get
				{
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					JniObjectReference peerReference = _members.JniPeerType.PeerReference;
					return ((JniObjectReference)(ref peerReference)).Handle;
				}
			}

			[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
			[EditorBrowsable(/*Could not decode attribute arguments.*/)]
			public override JniPeerMembers JniPeerMembers => _members;

			[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
			[EditorBrowsable(/*Could not decode attribute arguments.*/)]
			protected override nint ThresholdClass
			{
				get
				{
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					JniObjectReference peerReference = _members.JniPeerType.PeerReference;
					return ((JniObjectReference)(ref peerReference)).Handle;
				}
			}

			[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
			[EditorBrowsable(/*Could not decode attribute arguments.*/)]
			protected override global::System.Type ThresholdType => _members.ManagedPeerType;

			protected AUX_INFO(nint javaReference, JniHandleOwnership transfer)
				: base((global::System.IntPtr)javaReference, transfer)
			{
			}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


			[Register(".ctor", "(Lcom/sonix/UVCCam/BatteryCam;)V", "")]
			public unsafe AUX_INFO(BatteryCam? __self)
				: base(global::System.IntPtr.Zero, (JniHandleOwnership)0)
			{
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_005d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0074: Unknown result type (might be due to invalid IL or missing references)
				//IL_0079: Unknown result type (might be due to invalid IL or missing references)
				string text = "(L" + JNIEnv.GetJniName(((MemberInfo)((object)this).GetType()).DeclaringType) + ";)V";
				if (((Object)this).Handle != global::System.IntPtr.Zero)
				{
					return;
				}
				try
				{
					JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
					global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((__self == null) ? global::System.IntPtr.Zero : ((Object)__self).Handle));
					JniObjectReference val = _members.InstanceMethods.StartCreateInstance(text, ((object)this).GetType(), ptr);
					((Object)this).SetHandle(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
					_members.InstanceMethods.FinishCreateInstance(text, (IJavaPeerable)(object)this, ptr);
				}
				finally
				{
					GC.KeepAlive((object)__self);
				}
			}
		}

		[Register("com/sonix/UVCCam/BatteryCam$Framecallback", "", "Com.Sonix.UVCCam.BatteryCam/IFramecallbackInvoker")]
		public interface IFramecallback : IJavaObject, global::System.IDisposable, IJavaPeerable
		{
			[Register("onAudio", "(I[BJ)V", "GetOnAudio_IarrayBJHandler:Com.Sonix.UVCCam.BatteryCam/IFramecallbackInvoker, ids.camera.insight.binding")]
			void OnAudio(int p0, byte[]? p1, long p2);

			[Register("onEvent", "(IIIII)V", "GetOnEvent_IIIIIHandler:Com.Sonix.UVCCam.BatteryCam/IFramecallbackInvoker, ids.camera.insight.binding")]
			void OnEvent(int p0, int p1, int p2, int p3, int p4);

			[Register("onVideo", "(I[BJ)V", "GetOnVideo_IarrayBJHandler:Com.Sonix.UVCCam.BatteryCam/IFramecallbackInvoker, ids.camera.insight.binding")]
			void OnVideo(int p0, byte[]? p1, long p2);
		}

		[Register("com/sonix/UVCCam/BatteryCam$Framecallback", DoNotGenerateAcw = true)]
		internal class IFramecallbackInvoker : Object, IFramecallback, IJavaObject, global::System.IDisposable, IJavaPeerable
		{
			private static readonly JniPeerMembers _members_com_sonix_UVCCam_BatteryCam_Framecallback = (JniPeerMembers)new XAPeerMembers("com/sonix/UVCCam/BatteryCam$Framecallback", typeof(IFramecallbackInvoker));

			private static global::System.Delegate? cb_onAudio_OnAudio_IarrayBJ_V;

			private static global::System.Delegate? cb_onEvent_OnEvent_IIIII_V;

			private static global::System.Delegate? cb_onVideo_OnVideo_IarrayBJ_V;

			private static nint java_class_ref
			{
				get
				{
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					JniObjectReference peerReference = _members_com_sonix_UVCCam_BatteryCam_Framecallback.JniPeerType.PeerReference;
					return ((JniObjectReference)(ref peerReference)).Handle;
				}
			}

			[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
			[EditorBrowsable(/*Could not decode attribute arguments.*/)]
			public override JniPeerMembers JniPeerMembers => _members_com_sonix_UVCCam_BatteryCam_Framecallback;

			[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
			[EditorBrowsable(/*Could not decode attribute arguments.*/)]
			protected override nint ThresholdClass
			{
				get
				{
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					JniObjectReference peerReference = _members_com_sonix_UVCCam_BatteryCam_Framecallback.JniPeerType.PeerReference;
					return ((JniObjectReference)(ref peerReference)).Handle;
				}
			}

			[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
			[EditorBrowsable(/*Could not decode attribute arguments.*/)]
			protected override global::System.Type ThresholdType => _members_com_sonix_UVCCam_BatteryCam_Framecallback.ManagedPeerType;

			public IFramecallbackInvoker(nint handle, JniHandleOwnership transfer)
				: base((global::System.IntPtr)handle, transfer)
			{
			}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


			private static global::System.Delegate GetOnAudio_IarrayBJHandler()
			{
				if (cb_onAudio_OnAudio_IarrayBJ_V == null)
				{
					cb_onAudio_OnAudio_IarrayBJ_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPILJ_V(n_OnAudio_IarrayBJ));
				}
				return cb_onAudio_OnAudio_IarrayBJ_V;
			}

			private static void n_OnAudio_IarrayBJ(nint jnienv, nint native__this, int p0, nint native_p1, long p2)
			{
				IFramecallback framecallback = Object.GetObject<IFramecallback>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
				byte[] array = (byte[])JNIEnv.GetArray((global::System.IntPtr)native_p1, (JniHandleOwnership)0, typeof(byte));
				framecallback.OnAudio(p0, array, p2);
				if (array != null)
				{
					JNIEnv.CopyArray(array, (global::System.IntPtr)native_p1);
				}
			}

			public unsafe void OnAudio(int p0, byte[]? p1, long p2)
			{
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_003e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				nint num = JNIEnv.NewArray(p1);
				try
				{
					JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[3];
					global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(p0));
					global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue((global::System.IntPtr)num));
					global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + (nint)2 * (nint)global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(p2));
					_members_com_sonix_UVCCam_BatteryCam_Framecallback.InstanceMethods.InvokeAbstractVoidMethod("onAudio.(I[BJ)V", (IJavaPeerable)(object)this, ptr);
				}
				finally
				{
					if (p1 != null)
					{
						JNIEnv.CopyArray((global::System.IntPtr)num, p1);
						JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
					}
					GC.KeepAlive((object)p1);
				}
			}

			private static global::System.Delegate GetOnEvent_IIIIIHandler()
			{
				if (cb_onEvent_OnEvent_IIIII_V == null)
				{
					cb_onEvent_OnEvent_IIIII_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPIIIII_V(n_OnEvent_IIIII));
				}
				return cb_onEvent_OnEvent_IIIII_V;
			}

			private static void n_OnEvent_IIIII(nint jnienv, nint native__this, int p0, int p1, int p2, int p3, int p4)
			{
				Object.GetObject<IFramecallback>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).OnEvent(p0, p1, p2, p3, p4);
			}

			public unsafe void OnEvent(int p0, int p1, int p2, int p3, int p4)
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_004e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				//IL_0065: Unknown result type (might be due to invalid IL or missing references)
				//IL_006a: Unknown result type (might be due to invalid IL or missing references)
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[5];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(p0));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(p1));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + (nint)2 * (nint)global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(p2));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + (nint)3 * (nint)global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(p3));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + (nint)4 * (nint)global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(p4));
				_members_com_sonix_UVCCam_BatteryCam_Framecallback.InstanceMethods.InvokeAbstractVoidMethod("onEvent.(IIIII)V", (IJavaPeerable)(object)this, ptr);
			}

			private static global::System.Delegate GetOnVideo_IarrayBJHandler()
			{
				if (cb_onVideo_OnVideo_IarrayBJ_V == null)
				{
					cb_onVideo_OnVideo_IarrayBJ_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPILJ_V(n_OnVideo_IarrayBJ));
				}
				return cb_onVideo_OnVideo_IarrayBJ_V;
			}

			private static void n_OnVideo_IarrayBJ(nint jnienv, nint native__this, int p0, nint native_p1, long p2)
			{
				IFramecallback framecallback = Object.GetObject<IFramecallback>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
				byte[] array = (byte[])JNIEnv.GetArray((global::System.IntPtr)native_p1, (JniHandleOwnership)0, typeof(byte));
				framecallback.OnVideo(p0, array, p2);
				if (array != null)
				{
					JNIEnv.CopyArray(array, (global::System.IntPtr)native_p1);
				}
			}

			public unsafe void OnVideo(int p0, byte[]? p1, long p2)
			{
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_003e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				nint num = JNIEnv.NewArray(p1);
				try
				{
					JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[3];
					global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(p0));
					global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue((global::System.IntPtr)num));
					global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + (nint)2 * (nint)global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(p2));
					_members_com_sonix_UVCCam_BatteryCam_Framecallback.InstanceMethods.InvokeAbstractVoidMethod("onVideo.(I[BJ)V", (IJavaPeerable)(object)this, ptr);
				}
				finally
				{
					if (p1 != null)
					{
						JNIEnv.CopyArray((global::System.IntPtr)num, p1);
						JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
					}
					GC.KeepAlive((object)p1);
				}
			}
		}

		[Register("PID")]
		public const int Pid = 33280;

		[Register("VID")]
		public const int Vid = 3141;

		private static readonly JniPeerMembers _members = (JniPeerMembers)new XAPeerMembers("com/sonix/UVCCam/BatteryCam", typeof(BatteryCam));

		private static global::System.Delegate? cb_getConnectDevice_GetConnectDevice_Landroid_hardware_usb_UsbDevice_;

		private static global::System.Delegate? cb_isPreview_IsPreview_Z;

		private static global::System.Delegate? cb_closeDevice_CloseDevice_V;

		private static global::System.Delegate? cb_getAPICmd_GetAPICmd_arrayB_I;

		private static global::System.Delegate? cb_getCombineCmd_GetCombineCmd_IarrayBI_arrayB;

		private static global::System.Delegate? cb_hasPermission_HasPermission_Landroid_hardware_usb_UsbDevice__Z;

		private static global::System.Delegate? cb_openDevice_OpenDevice_Landroid_hardware_usb_UsbDevice__Z;

		private static global::System.Delegate? cb_requestPermission_RequestPermission_Landroid_hardware_usb_UsbDevice__V;

		private static global::System.Delegate? cb_sendAudioData_SendAudioData_IarrayB_I;

		private static global::System.Delegate? cb_setAPICmd_SetAPICmd_arrayB_I;

		private static global::System.Delegate? cb_setFrameCallback_SetFrameCallback_Lcom_sonix_UVCCam_BatteryCam_Framecallback__V;

		private static global::System.Delegate? cb_startRead_StartRead_V;

		private static global::System.Delegate? cb_stopRead_StopRead_V;

		internal static nint class_ref
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		public override JniPeerMembers JniPeerMembers => _members;

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override nint ThresholdClass
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override global::System.Type ThresholdType => _members.ManagedPeerType;

		public unsafe virtual UsbDevice? ConnectDevice
		{
			[Register("getConnectDevice", "()Landroid/hardware/usb/UsbDevice;", "GetGetConnectDeviceHandler")]
			get
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference val = _members.InstanceMethods.InvokeVirtualObjectMethod("getConnectDevice.()Landroid/hardware/usb/UsbDevice;", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
				return Object.GetObject<UsbDevice>(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
			}
		}

		public unsafe virtual bool IsPreview
		{
			[Register("isPreview", "()Z", "GetIsPreviewHandler")]
			get
			{
				return _members.InstanceMethods.InvokeVirtualBooleanMethod("isPreview.()Z", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			}
		}

		protected BatteryCam(nint javaReference, JniHandleOwnership transfer)
			: base((global::System.IntPtr)javaReference, transfer)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		private static global::System.Delegate GetGetConnectDeviceHandler()
		{
			if (cb_getConnectDevice_GetConnectDevice_Landroid_hardware_usb_UsbDevice_ == null)
			{
				cb_getConnectDevice_GetConnectDevice_Landroid_hardware_usb_UsbDevice_ = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_L(n_GetConnectDevice));
			}
			return cb_getConnectDevice_GetConnectDevice_Landroid_hardware_usb_UsbDevice_;
		}

		private static nint n_GetConnectDevice(nint jnienv, nint native__this)
		{
			return JNIEnv.ToLocalJniHandle((IJavaObject)(object)Object.GetObject<BatteryCam>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).ConnectDevice);
		}

		private static global::System.Delegate GetIsPreviewHandler()
		{
			if (cb_isPreview_IsPreview_Z == null)
			{
				cb_isPreview_IsPreview_Z = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_Z(n_IsPreview));
			}
			return cb_isPreview_IsPreview_Z;
		}

		private static bool n_IsPreview(nint jnienv, nint native__this)
		{
			return Object.GetObject<BatteryCam>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).IsPreview;
		}

		[Register("byteArrayToLeInt", "([B)I", "")]
		public unsafe static int ByteArrayToLeInt(byte[]? b)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			nint num = JNIEnv.NewArray(b);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((global::System.IntPtr)num));
				return _members.StaticMethods.InvokeInt32Method("byteArrayToLeInt.([B)I", ptr);
			}
			finally
			{
				if (b != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num, b);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				}
				GC.KeepAlive((object)b);
			}
		}

		private static global::System.Delegate GetCloseDeviceHandler()
		{
			if (cb_closeDevice_CloseDevice_V == null)
			{
				cb_closeDevice_CloseDevice_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_CloseDevice));
			}
			return cb_closeDevice_CloseDevice_V;
		}

		private static void n_CloseDevice(nint jnienv, nint native__this)
		{
			Object.GetObject<BatteryCam>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).CloseDevice();
		}

		[Register("closeDevice", "()V", "GetCloseDeviceHandler")]
		public unsafe virtual void CloseDevice()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("closeDevice.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetGetAPICmd_arrayBHandler()
		{
			if (cb_getAPICmd_GetAPICmd_arrayB_I == null)
			{
				cb_getAPICmd_GetAPICmd_arrayB_I = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPL_I(n_GetAPICmd_arrayB));
			}
			return cb_getAPICmd_GetAPICmd_arrayB_I;
		}

		private static int n_GetAPICmd_arrayB(nint jnienv, nint native__this, nint native_resData)
		{
			BatteryCam batteryCam = Object.GetObject<BatteryCam>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			byte[] array = (byte[])JNIEnv.GetArray((global::System.IntPtr)native_resData, (JniHandleOwnership)0, typeof(byte));
			int aPICmd = batteryCam.GetAPICmd(array);
			if (array != null)
			{
				JNIEnv.CopyArray(array, (global::System.IntPtr)native_resData);
			}
			return aPICmd;
		}

		[Register("getAPICmd", "([B)I", "GetGetAPICmd_arrayBHandler")]
		public unsafe virtual int GetAPICmd(byte[]? resData)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			nint num = JNIEnv.NewArray(resData);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((global::System.IntPtr)num));
				return _members.InstanceMethods.InvokeVirtualInt32Method("getAPICmd.([B)I", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				if (resData != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num, resData);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				}
				GC.KeepAlive((object)resData);
			}
		}

		private static global::System.Delegate GetGetCombineCmd_IarrayBIHandler()
		{
			if (cb_getCombineCmd_GetCombineCmd_IarrayBI_arrayB == null)
			{
				cb_getCombineCmd_GetCombineCmd_IarrayBI_arrayB = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPILI_L(n_GetCombineCmd_IarrayBI));
			}
			return cb_getCombineCmd_GetCombineCmd_IarrayBI_arrayB;
		}

		private static nint n_GetCombineCmd_IarrayBI(nint jnienv, nint native__this, int opcode, nint native_compare, int length)
		{
			BatteryCam batteryCam = Object.GetObject<BatteryCam>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			byte[] array = (byte[])JNIEnv.GetArray((global::System.IntPtr)native_compare, (JniHandleOwnership)0, typeof(byte));
			global::System.IntPtr result = JNIEnv.NewArray(batteryCam.GetCombineCmd(opcode, array, length));
			if (array != null)
			{
				JNIEnv.CopyArray(array, (global::System.IntPtr)native_compare);
			}
			return result;
		}

		[Register("getCombineCmd", "(I[BI)[B", "GetGetCombineCmd_IarrayBIHandler")]
		public unsafe virtual byte[]? GetCombineCmd(int opcode, byte[]? compare, int length)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			nint num = JNIEnv.NewArray(compare);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[3];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(opcode));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue((global::System.IntPtr)num));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + (nint)2 * (nint)global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(length));
				JniObjectReference val = _members.InstanceMethods.InvokeVirtualObjectMethod("getCombineCmd.(I[BI)[B", (IJavaPeerable)(object)this, ptr);
				return (byte[])JNIEnv.GetArray(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1, typeof(byte));
			}
			finally
			{
				if (compare != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num, compare);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				}
				GC.KeepAlive((object)compare);
			}
		}

		[Register("getInstance", "(Landroid/content/Context;)Lcom/sonix/UVCCam/BatteryCam;", "")]
		public unsafe static BatteryCam? GetInstance(Context? ctx)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((ctx == null) ? global::System.IntPtr.Zero : ((Object)ctx).Handle));
				JniObjectReference val = _members.StaticMethods.InvokeObjectMethod("getInstance.(Landroid/content/Context;)Lcom/sonix/UVCCam/BatteryCam;", ptr);
				return Object.GetObject<BatteryCam>(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
			}
			finally
			{
				GC.KeepAlive((object)ctx);
			}
		}

		private static global::System.Delegate GetHasPermission_Landroid_hardware_usb_UsbDevice_Handler()
		{
			if (cb_hasPermission_HasPermission_Landroid_hardware_usb_UsbDevice__Z == null)
			{
				cb_hasPermission_HasPermission_Landroid_hardware_usb_UsbDevice__Z = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPL_Z(n_HasPermission_Landroid_hardware_usb_UsbDevice_));
			}
			return cb_hasPermission_HasPermission_Landroid_hardware_usb_UsbDevice__Z;
		}

		private static bool n_HasPermission_Landroid_hardware_usb_UsbDevice_(nint jnienv, nint native__this, nint native_device)
		{
			BatteryCam batteryCam = Object.GetObject<BatteryCam>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			UsbDevice device = Object.GetObject<UsbDevice>((global::System.IntPtr)native_device, (JniHandleOwnership)0);
			return batteryCam.HasPermission(device);
		}

		[Register("hasPermission", "(Landroid/hardware/usb/UsbDevice;)Z", "GetHasPermission_Landroid_hardware_usb_UsbDevice_Handler")]
		public unsafe virtual bool HasPermission(UsbDevice? device)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((device == null) ? global::System.IntPtr.Zero : ((Object)device).Handle));
				return _members.InstanceMethods.InvokeVirtualBooleanMethod("hasPermission.(Landroid/hardware/usb/UsbDevice;)Z", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)device);
			}
		}

		private static global::System.Delegate GetOpenDevice_Landroid_hardware_usb_UsbDevice_Handler()
		{
			if (cb_openDevice_OpenDevice_Landroid_hardware_usb_UsbDevice__Z == null)
			{
				cb_openDevice_OpenDevice_Landroid_hardware_usb_UsbDevice__Z = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPL_Z(n_OpenDevice_Landroid_hardware_usb_UsbDevice_));
			}
			return cb_openDevice_OpenDevice_Landroid_hardware_usb_UsbDevice__Z;
		}

		private static bool n_OpenDevice_Landroid_hardware_usb_UsbDevice_(nint jnienv, nint native__this, nint native_device)
		{
			BatteryCam batteryCam = Object.GetObject<BatteryCam>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			UsbDevice device = Object.GetObject<UsbDevice>((global::System.IntPtr)native_device, (JniHandleOwnership)0);
			return batteryCam.OpenDevice(device);
		}

		[Register("openDevice", "(Landroid/hardware/usb/UsbDevice;)Z", "GetOpenDevice_Landroid_hardware_usb_UsbDevice_Handler")]
		public unsafe virtual bool OpenDevice(UsbDevice? device)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((device == null) ? global::System.IntPtr.Zero : ((Object)device).Handle));
				return _members.InstanceMethods.InvokeVirtualBooleanMethod("openDevice.(Landroid/hardware/usb/UsbDevice;)Z", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)device);
			}
		}

		private static global::System.Delegate GetRequestPermission_Landroid_hardware_usb_UsbDevice_Handler()
		{
			if (cb_requestPermission_RequestPermission_Landroid_hardware_usb_UsbDevice__V == null)
			{
				cb_requestPermission_RequestPermission_Landroid_hardware_usb_UsbDevice__V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPL_V(n_RequestPermission_Landroid_hardware_usb_UsbDevice_));
			}
			return cb_requestPermission_RequestPermission_Landroid_hardware_usb_UsbDevice__V;
		}

		private static void n_RequestPermission_Landroid_hardware_usb_UsbDevice_(nint jnienv, nint native__this, nint native_device)
		{
			BatteryCam batteryCam = Object.GetObject<BatteryCam>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			UsbDevice device = Object.GetObject<UsbDevice>((global::System.IntPtr)native_device, (JniHandleOwnership)0);
			batteryCam.RequestPermission(device);
		}

		[Register("requestPermission", "(Landroid/hardware/usb/UsbDevice;)V", "GetRequestPermission_Landroid_hardware_usb_UsbDevice_Handler")]
		public unsafe virtual void RequestPermission(UsbDevice? device)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((device == null) ? global::System.IntPtr.Zero : ((Object)device).Handle));
				_members.InstanceMethods.InvokeVirtualVoidMethod("requestPermission.(Landroid/hardware/usb/UsbDevice;)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)device);
			}
		}

		private static global::System.Delegate GetSendAudioData_IarrayBHandler()
		{
			if (cb_sendAudioData_SendAudioData_IarrayB_I == null)
			{
				cb_sendAudioData_SendAudioData_IarrayB_I = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPIL_I(n_SendAudioData_IarrayB));
			}
			return cb_sendAudioData_SendAudioData_IarrayB_I;
		}

		private static int n_SendAudioData_IarrayB(nint jnienv, nint native__this, int txNo, nint native_data)
		{
			BatteryCam batteryCam = Object.GetObject<BatteryCam>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			byte[] array = (byte[])JNIEnv.GetArray((global::System.IntPtr)native_data, (JniHandleOwnership)0, typeof(byte));
			int result = batteryCam.SendAudioData(txNo, array);
			if (array != null)
			{
				JNIEnv.CopyArray(array, (global::System.IntPtr)native_data);
			}
			return result;
		}

		[Register("sendAudioData", "(I[B)I", "GetSendAudioData_IarrayBHandler")]
		public unsafe virtual int SendAudioData(int txNo, byte[]? data)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			nint num = JNIEnv.NewArray(data);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(txNo));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue((global::System.IntPtr)num));
				return _members.InstanceMethods.InvokeVirtualInt32Method("sendAudioData.(I[B)I", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				if (data != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num, data);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				}
				GC.KeepAlive((object)data);
			}
		}

		private static global::System.Delegate GetSetAPICmd_arrayBHandler()
		{
			if (cb_setAPICmd_SetAPICmd_arrayB_I == null)
			{
				cb_setAPICmd_SetAPICmd_arrayB_I = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPL_I(n_SetAPICmd_arrayB));
			}
			return cb_setAPICmd_SetAPICmd_arrayB_I;
		}

		private static int n_SetAPICmd_arrayB(nint jnienv, nint native__this, nint native_data)
		{
			BatteryCam batteryCam = Object.GetObject<BatteryCam>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			byte[] array = (byte[])JNIEnv.GetArray((global::System.IntPtr)native_data, (JniHandleOwnership)0, typeof(byte));
			int result = batteryCam.SetAPICmd(array);
			if (array != null)
			{
				JNIEnv.CopyArray(array, (global::System.IntPtr)native_data);
			}
			return result;
		}

		[Register("setAPICmd", "([B)I", "GetSetAPICmd_arrayBHandler")]
		public unsafe virtual int SetAPICmd(byte[]? data)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			nint num = JNIEnv.NewArray(data);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((global::System.IntPtr)num));
				return _members.InstanceMethods.InvokeVirtualInt32Method("setAPICmd.([B)I", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				if (data != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num, data);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				}
				GC.KeepAlive((object)data);
			}
		}

		private static global::System.Delegate GetSetFrameCallback_Lcom_sonix_UVCCam_BatteryCam_Framecallback_Handler()
		{
			if (cb_setFrameCallback_SetFrameCallback_Lcom_sonix_UVCCam_BatteryCam_Framecallback__V == null)
			{
				cb_setFrameCallback_SetFrameCallback_Lcom_sonix_UVCCam_BatteryCam_Framecallback__V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPL_V(n_SetFrameCallback_Lcom_sonix_UVCCam_BatteryCam_Framecallback_));
			}
			return cb_setFrameCallback_SetFrameCallback_Lcom_sonix_UVCCam_BatteryCam_Framecallback__V;
		}

		private static void n_SetFrameCallback_Lcom_sonix_UVCCam_BatteryCam_Framecallback_(nint jnienv, nint native__this, nint native__callback)
		{
			BatteryCam batteryCam = Object.GetObject<BatteryCam>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			IFramecallback frameCallback = Object.GetObject<IFramecallback>((global::System.IntPtr)native__callback, (JniHandleOwnership)0);
			batteryCam.SetFrameCallback(frameCallback);
		}

		[Register("setFrameCallback", "(Lcom/sonix/UVCCam/BatteryCam$Framecallback;)V", "GetSetFrameCallback_Lcom_sonix_UVCCam_BatteryCam_Framecallback_Handler")]
		public unsafe virtual void SetFrameCallback(IFramecallback? callback)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((callback == null) ? global::System.IntPtr.Zero : ((Object)callback).Handle));
				_members.InstanceMethods.InvokeVirtualVoidMethod("setFrameCallback.(Lcom/sonix/UVCCam/BatteryCam$Framecallback;)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)callback);
			}
		}

		private static global::System.Delegate GetStartReadHandler()
		{
			if (cb_startRead_StartRead_V == null)
			{
				cb_startRead_StartRead_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_StartRead));
			}
			return cb_startRead_StartRead_V;
		}

		private static void n_StartRead(nint jnienv, nint native__this)
		{
			Object.GetObject<BatteryCam>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).StartRead();
		}

		[Register("startRead", "()V", "GetStartReadHandler")]
		public unsafe virtual void StartRead()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("startRead.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetStopReadHandler()
		{
			if (cb_stopRead_StopRead_V == null)
			{
				cb_stopRead_StopRead_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_StopRead));
			}
			return cb_stopRead_StopRead_V;
		}

		private static void n_StopRead(nint jnienv, nint native__this)
		{
			Object.GetObject<BatteryCam>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).StopRead();
		}

		[Register("stopRead", "()V", "GetStopReadHandler")]
		public unsafe virtual void StopRead()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("stopRead.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}
	}
	[Register("com/sonix/UVCCam/Channel", DoNotGenerateAcw = true)]
	public class Channel : Object
	{
		[Register("AUDIO")]
		public const int Audio = 1;

		[Register("VIDEO")]
		public const int Video = 0;

		private static readonly JniPeerMembers _members = (JniPeerMembers)new XAPeerMembers("com/sonix/UVCCam/Channel", typeof(Channel));

		private static global::System.Delegate? cb_combinFrame_CombinFrame_arrayB_arrayB;

		internal static nint class_ref
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		public override JniPeerMembers JniPeerMembers => _members;

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override nint ThresholdClass
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override global::System.Type ThresholdType => _members.ManagedPeerType;

		protected Channel(nint javaReference, JniHandleOwnership transfer)
			: base((global::System.IntPtr)javaReference, transfer)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		[Register(".ctor", "(I)V", "")]
		public unsafe Channel(int channel)
			: base(global::System.IntPtr.Zero, (JniHandleOwnership)0)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			if (((Object)this).Handle == global::System.IntPtr.Zero)
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(channel));
				JniObjectReference val = _members.InstanceMethods.StartCreateInstance("(I)V", ((object)this).GetType(), ptr);
				((Object)this).SetHandle(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
				_members.InstanceMethods.FinishCreateInstance("(I)V", (IJavaPeerable)(object)this, ptr);
			}
		}

		private static global::System.Delegate GetCombinFrame_arrayBHandler()
		{
			if (cb_combinFrame_CombinFrame_arrayB_arrayB == null)
			{
				cb_combinFrame_CombinFrame_arrayB_arrayB = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPL_L(n_CombinFrame_arrayB));
			}
			return cb_combinFrame_CombinFrame_arrayB_arrayB;
		}

		private static nint n_CombinFrame_arrayB(nint jnienv, nint native__this, nint native_usbData)
		{
			Channel channel = Object.GetObject<Channel>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			byte[] array = (byte[])JNIEnv.GetArray((global::System.IntPtr)native_usbData, (JniHandleOwnership)0, typeof(byte));
			global::System.IntPtr result = JNIEnv.NewArray(channel.CombinFrame(array));
			if (array != null)
			{
				JNIEnv.CopyArray(array, (global::System.IntPtr)native_usbData);
			}
			return result;
		}

		[Register("combinFrame", "([B)[B", "GetCombinFrame_arrayBHandler")]
		public unsafe virtual byte[]? CombinFrame(byte[]? usbData)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			nint num = JNIEnv.NewArray(usbData);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((global::System.IntPtr)num));
				JniObjectReference val = _members.InstanceMethods.InvokeVirtualObjectMethod("combinFrame.([B)[B", (IJavaPeerable)(object)this, ptr);
				return (byte[])JNIEnv.GetArray(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1, typeof(byte));
			}
			finally
			{
				if (usbData != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num, usbData);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				}
				GC.KeepAlive((object)usbData);
			}
		}
	}
}
namespace Com.Sonix.Library
{
	[Register("com/sonix/library/BuildConfig", DoNotGenerateAcw = true)]
	public sealed class BuildConfig : Object
	{
		[Register("APPLICATION_ID")]
		public const string ApplicationId = "com.sonix.library";

		[Register("BUILD_TYPE")]
		public const string BuildType = "release";

		[Register("DEBUG")]
		public const bool Debug = false;

		[Register("FLAVOR")]
		public const string Flavor = "";

		[Register("VERSION_CODE")]
		public const int VersionCode = 1;

		[Register("VERSION_NAME")]
		public const string VersionName = "1.1";

		private static readonly JniPeerMembers _members = (JniPeerMembers)new XAPeerMembers("com/sonix/library/BuildConfig", typeof(BuildConfig));

		internal static nint class_ref
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		public override JniPeerMembers JniPeerMembers => _members;

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override nint ThresholdClass
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override global::System.Type ThresholdType => _members.ManagedPeerType;

		internal BuildConfig(nint javaReference, JniHandleOwnership transfer)
			: base((global::System.IntPtr)javaReference, transfer)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		[Register(".ctor", "()V", "")]
		public unsafe BuildConfig()
			: base(global::System.IntPtr.Zero, (JniHandleOwnership)0)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			if (((Object)this).Handle == global::System.IntPtr.Zero)
			{
				JniObjectReference val = _members.InstanceMethods.StartCreateInstance("()V", ((object)this).GetType(), (JniArgumentValue*)null);
				((Object)this).SetHandle(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
				_members.InstanceMethods.FinishCreateInstance("()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			}
		}
	}
}
namespace Com.Sonix.Decoder
{
	[Register("com/sonix/decoder/AudioObject", DoNotGenerateAcw = true)]
	public class AudioObject : Object
	{
		private static readonly JniPeerMembers _members = (JniPeerMembers)new XAPeerMembers("com/sonix/decoder/AudioObject", typeof(AudioObject));

		private static global::System.Delegate? cb_getTimeStamp_GetTimeStamp_J;

		private static global::System.Delegate? cb_getData_GetData_arrayB;

		[Register("mData")]
		public global::System.Collections.Generic.IList<byte>? MData
		{
			get
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference objectValue = _members.InstanceFields.GetObjectValue("mData.[B", (IJavaPeerable)(object)this);
				return (global::System.Collections.Generic.IList<byte>?)JavaArray<byte>.FromJniHandle(((JniObjectReference)(ref objectValue)).Handle, (JniHandleOwnership)1);
			}
			set
			{
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				nint num = JavaArray<byte>.ToLocalJniHandle(value);
				try
				{
					_members.InstanceFields.SetValue("mData.[B", (IJavaPeerable)(object)this, new JniObjectReference((global::System.IntPtr)num, (JniObjectReferenceType)0));
				}
				finally
				{
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				}
			}
		}

		[Register("mTimeStamp")]
		public long MTimeStamp
		{
			get
			{
				return _members.InstanceFields.GetInt64Value("mTimeStamp.J", (IJavaPeerable)(object)this);
			}
			set
			{
				_members.InstanceFields.SetValue("mTimeStamp.J", (IJavaPeerable)(object)this, value);
			}
		}

		internal static nint class_ref
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		public override JniPeerMembers JniPeerMembers => _members;

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override nint ThresholdClass
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override global::System.Type ThresholdType => _members.ManagedPeerType;

		public unsafe virtual long TimeStamp
		{
			[Register("getTimeStamp", "()J", "GetGetTimeStampHandler")]
			get
			{
				return _members.InstanceMethods.InvokeVirtualInt64Method("getTimeStamp.()J", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			}
		}

		protected AudioObject(nint javaReference, JniHandleOwnership transfer)
			: base((global::System.IntPtr)javaReference, transfer)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		[Register(".ctor", "([BJ)V", "")]
		public unsafe AudioObject(byte[]? data, long timeStamp)
			: base(global::System.IntPtr.Zero, (JniHandleOwnership)0)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			if (((Object)this).Handle != global::System.IntPtr.Zero)
			{
				return;
			}
			nint num = JNIEnv.NewArray(data);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((global::System.IntPtr)num));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(timeStamp));
				JniObjectReference val = _members.InstanceMethods.StartCreateInstance("([BJ)V", ((object)this).GetType(), ptr);
				((Object)this).SetHandle(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
				_members.InstanceMethods.FinishCreateInstance("([BJ)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				if (data != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num, data);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				}
				GC.KeepAlive((object)data);
			}
		}

		private static global::System.Delegate GetGetTimeStampHandler()
		{
			if (cb_getTimeStamp_GetTimeStamp_J == null)
			{
				cb_getTimeStamp_GetTimeStamp_J = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_J(n_GetTimeStamp));
			}
			return cb_getTimeStamp_GetTimeStamp_J;
		}

		private static long n_GetTimeStamp(nint jnienv, nint native__this)
		{
			return Object.GetObject<AudioObject>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).TimeStamp;
		}

		private static global::System.Delegate GetGetDataHandler()
		{
			if (cb_getData_GetData_arrayB == null)
			{
				cb_getData_GetData_arrayB = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_L(n_GetData));
			}
			return cb_getData_GetData_arrayB;
		}

		private static nint n_GetData(nint jnienv, nint native__this)
		{
			return JNIEnv.NewArray(Object.GetObject<AudioObject>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).GetData());
		}

		[Register("getData", "()[B", "GetGetDataHandler")]
		public unsafe virtual byte[]? GetData()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			JniObjectReference val = _members.InstanceMethods.InvokeVirtualObjectMethod("getData.()[B", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			return (byte[])JNIEnv.GetArray(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1, typeof(byte));
		}
	}
	[Register("com/sonix/decoder/FrameObject", DoNotGenerateAcw = true)]
	public class FrameObject : Object
	{
		private static readonly JniPeerMembers _members = (JniPeerMembers)new XAPeerMembers("com/sonix/decoder/FrameObject", typeof(FrameObject));

		private static global::System.Delegate? cb_getIFlag_GetIFlag_Z;

		private static global::System.Delegate? cb_setIFlag_SetIFlag_Z_V;

		private static global::System.Delegate? cb_getTimeStamp_GetTimeStamp_J;

		private static global::System.Delegate? cb_getData_GetData_arrayB;

		private static global::System.Delegate? cb_getShortData_GetShortData_arrayS;

		internal static nint class_ref
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		public override JniPeerMembers JniPeerMembers => _members;

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override nint ThresholdClass
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override global::System.Type ThresholdType => _members.ManagedPeerType;

		public unsafe virtual bool IFlag
		{
			[Register("getIFlag", "()Z", "GetGetIFlagHandler")]
			get
			{
				return _members.InstanceMethods.InvokeVirtualBooleanMethod("getIFlag.()Z", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			}
			[Register("setIFlag", "(Z)V", "GetSetIFlag_ZHandler")]
			set
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(value));
				_members.InstanceMethods.InvokeVirtualVoidMethod("setIFlag.(Z)V", (IJavaPeerable)(object)this, ptr);
			}
		}

		public unsafe virtual long TimeStamp
		{
			[Register("getTimeStamp", "()J", "GetGetTimeStampHandler")]
			get
			{
				return _members.InstanceMethods.InvokeVirtualInt64Method("getTimeStamp.()J", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			}
		}

		protected FrameObject(nint javaReference, JniHandleOwnership transfer)
			: base((global::System.IntPtr)javaReference, transfer)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		[Register(".ctor", "([BJ)V", "")]
		public unsafe FrameObject(byte[]? data, long timeStamp)
			: base(global::System.IntPtr.Zero, (JniHandleOwnership)0)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			if (((Object)this).Handle != global::System.IntPtr.Zero)
			{
				return;
			}
			nint num = JNIEnv.NewArray(data);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((global::System.IntPtr)num));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(timeStamp));
				JniObjectReference val = _members.InstanceMethods.StartCreateInstance("([BJ)V", ((object)this).GetType(), ptr);
				((Object)this).SetHandle(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
				_members.InstanceMethods.FinishCreateInstance("([BJ)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				if (data != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num, data);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				}
				GC.KeepAlive((object)data);
			}
		}

		[Register(".ctor", "([SJ)V", "")]
		public unsafe FrameObject(short[]? data, long timeStamp)
			: base(global::System.IntPtr.Zero, (JniHandleOwnership)0)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			if (((Object)this).Handle != global::System.IntPtr.Zero)
			{
				return;
			}
			nint num = JNIEnv.NewArray(data);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((global::System.IntPtr)num));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(timeStamp));
				JniObjectReference val = _members.InstanceMethods.StartCreateInstance("([SJ)V", ((object)this).GetType(), ptr);
				((Object)this).SetHandle(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
				_members.InstanceMethods.FinishCreateInstance("([SJ)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				if (data != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num, data);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				}
				GC.KeepAlive((object)data);
			}
		}

		private static global::System.Delegate GetGetIFlagHandler()
		{
			if (cb_getIFlag_GetIFlag_Z == null)
			{
				cb_getIFlag_GetIFlag_Z = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_Z(n_GetIFlag));
			}
			return cb_getIFlag_GetIFlag_Z;
		}

		private static bool n_GetIFlag(nint jnienv, nint native__this)
		{
			return Object.GetObject<FrameObject>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).IFlag;
		}

		private static global::System.Delegate GetSetIFlag_ZHandler()
		{
			if (cb_setIFlag_SetIFlag_Z_V == null)
			{
				cb_setIFlag_SetIFlag_Z_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPZ_V(n_SetIFlag_Z));
			}
			return cb_setIFlag_SetIFlag_Z_V;
		}

		private static void n_SetIFlag_Z(nint jnienv, nint native__this, bool isIFlag)
		{
			Object.GetObject<FrameObject>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).IFlag = isIFlag;
		}

		private static global::System.Delegate GetGetTimeStampHandler()
		{
			if (cb_getTimeStamp_GetTimeStamp_J == null)
			{
				cb_getTimeStamp_GetTimeStamp_J = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_J(n_GetTimeStamp));
			}
			return cb_getTimeStamp_GetTimeStamp_J;
		}

		private static long n_GetTimeStamp(nint jnienv, nint native__this)
		{
			return Object.GetObject<FrameObject>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).TimeStamp;
		}

		private static global::System.Delegate GetGetDataHandler()
		{
			if (cb_getData_GetData_arrayB == null)
			{
				cb_getData_GetData_arrayB = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_L(n_GetData));
			}
			return cb_getData_GetData_arrayB;
		}

		private static nint n_GetData(nint jnienv, nint native__this)
		{
			return JNIEnv.NewArray(Object.GetObject<FrameObject>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).GetData());
		}

		[Register("getData", "()[B", "GetGetDataHandler")]
		public unsafe virtual byte[]? GetData()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			JniObjectReference val = _members.InstanceMethods.InvokeVirtualObjectMethod("getData.()[B", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			return (byte[])JNIEnv.GetArray(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1, typeof(byte));
		}

		private static global::System.Delegate GetGetShortDataHandler()
		{
			if (cb_getShortData_GetShortData_arrayS == null)
			{
				cb_getShortData_GetShortData_arrayS = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_L(n_GetShortData));
			}
			return cb_getShortData_GetShortData_arrayS;
		}

		private static nint n_GetShortData(nint jnienv, nint native__this)
		{
			return JNIEnv.NewArray(Object.GetObject<FrameObject>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).GetShortData());
		}

		[Register("getShortData", "()[S", "GetGetShortDataHandler")]
		public unsafe virtual short[]? GetShortData()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			JniObjectReference val = _members.InstanceMethods.InvokeVirtualObjectMethod("getShortData.()[S", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			return (short[])JNIEnv.GetArray(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1, typeof(short));
		}
	}
	[Register("com/sonix/decoder/VideoObject", DoNotGenerateAcw = true)]
	public class VideoObject : Object
	{
		private static readonly JniPeerMembers _members = (JniPeerMembers)new XAPeerMembers("com/sonix/decoder/VideoObject", typeof(VideoObject));

		private static global::System.Delegate? cb_getChhannel_GetChhannel_I;

		private static global::System.Delegate? cb_getTimeStamp_GetTimeStamp_J;

		private static global::System.Delegate? cb_getData_GetData_arrayB;

		internal static nint class_ref
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		public override JniPeerMembers JniPeerMembers => _members;

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override nint ThresholdClass
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override global::System.Type ThresholdType => _members.ManagedPeerType;

		public unsafe virtual int Chhannel
		{
			[Register("getChhannel", "()I", "GetGetChhannelHandler")]
			get
			{
				return _members.InstanceMethods.InvokeVirtualInt32Method("getChhannel.()I", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			}
		}

		public unsafe virtual long TimeStamp
		{
			[Register("getTimeStamp", "()J", "GetGetTimeStampHandler")]
			get
			{
				return _members.InstanceMethods.InvokeVirtualInt64Method("getTimeStamp.()J", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			}
		}

		protected VideoObject(nint javaReference, JniHandleOwnership transfer)
			: base((global::System.IntPtr)javaReference, transfer)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		[Register(".ctor", "([BIJ)V", "")]
		public unsafe VideoObject(byte[]? data, int channel, long timeStamp)
			: base(global::System.IntPtr.Zero, (JniHandleOwnership)0)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			if (((Object)this).Handle != global::System.IntPtr.Zero)
			{
				return;
			}
			nint num = JNIEnv.NewArray(data);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[3];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((global::System.IntPtr)num));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(channel));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + (nint)2 * (nint)global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(timeStamp));
				JniObjectReference val = _members.InstanceMethods.StartCreateInstance("([BIJ)V", ((object)this).GetType(), ptr);
				((Object)this).SetHandle(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
				_members.InstanceMethods.FinishCreateInstance("([BIJ)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				if (data != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num, data);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				}
				GC.KeepAlive((object)data);
			}
		}

		private static global::System.Delegate GetGetChhannelHandler()
		{
			if (cb_getChhannel_GetChhannel_I == null)
			{
				cb_getChhannel_GetChhannel_I = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_I(n_GetChhannel));
			}
			return cb_getChhannel_GetChhannel_I;
		}

		private static int n_GetChhannel(nint jnienv, nint native__this)
		{
			return Object.GetObject<VideoObject>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).Chhannel;
		}

		private static global::System.Delegate GetGetTimeStampHandler()
		{
			if (cb_getTimeStamp_GetTimeStamp_J == null)
			{
				cb_getTimeStamp_GetTimeStamp_J = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_J(n_GetTimeStamp));
			}
			return cb_getTimeStamp_GetTimeStamp_J;
		}

		private static long n_GetTimeStamp(nint jnienv, nint native__this)
		{
			return Object.GetObject<VideoObject>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).TimeStamp;
		}

		private static global::System.Delegate GetGetDataHandler()
		{
			if (cb_getData_GetData_arrayB == null)
			{
				cb_getData_GetData_arrayB = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_L(n_GetData));
			}
			return cb_getData_GetData_arrayB;
		}

		private static nint n_GetData(nint jnienv, nint native__this)
		{
			return JNIEnv.NewArray(Object.GetObject<VideoObject>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).GetData());
		}

		[Register("getData", "()[B", "GetGetDataHandler")]
		public unsafe virtual byte[]? GetData()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			JniObjectReference val = _members.InstanceMethods.InvokeVirtualObjectMethod("getData.()[B", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			return (byte[])JNIEnv.GetArray(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1, typeof(byte));
		}
	}
}
namespace Com.Service
{
	[Register("com/service/Logcat", DoNotGenerateAcw = true)]
	public class Logcat : Object
	{
		[Register("TAG")]
		public const string Tag = "VLC/Util/Logcat";

		private static readonly JniPeerMembers _members = (JniPeerMembers)new XAPeerMembers("com/service/Logcat", typeof(Logcat));

		internal static nint class_ref
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		public override JniPeerMembers JniPeerMembers => _members;

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override nint ThresholdClass
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override global::System.Type ThresholdType => _members.ManagedPeerType;

		protected Logcat(nint javaReference, JniHandleOwnership transfer)
			: base((global::System.IntPtr)javaReference, transfer)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		[Register(".ctor", "()V", "")]
		public unsafe Logcat()
			: base(global::System.IntPtr.Zero, (JniHandleOwnership)0)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			if (((Object)this).Handle == global::System.IntPtr.Zero)
			{
				JniObjectReference val = _members.InstanceMethods.StartCreateInstance("()V", ((object)this).GetType(), (JniArgumentValue*)null);
				((Object)this).SetHandle(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
				_members.InstanceMethods.FinishCreateInstance("()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			}
		}

		[Register("getLogcat", "()Ljava/lang/String;", "")]
		public unsafe static string? GetLogcat()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			JniObjectReference val = _members.StaticMethods.InvokeObjectMethod("getLogcat.()Ljava/lang/String;", (JniArgumentValue*)null);
			return JNIEnv.GetString(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
		}

		[Register("writeLogcat", "(Ljava/lang/String;)V", "")]
		public unsafe static void WriteLogcat(string? filename)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			nint num = JNIEnv.NewString(filename);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((global::System.IntPtr)num));
				_members.StaticMethods.InvokeVoidMethod("writeLogcat.(Ljava/lang/String;)V", ptr);
			}
			finally
			{
				JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
			}
		}
	}
	[Register("com/service/VLCCrashHandler", DoNotGenerateAcw = true)]
	public class VLCCrashHandler : Object, IUncaughtExceptionHandler, IJavaObject, global::System.IDisposable, IJavaPeerable
	{
		private static readonly JniPeerMembers _members = (JniPeerMembers)new XAPeerMembers("com/service/VLCCrashHandler", typeof(VLCCrashHandler));

		private static global::System.Delegate? cb_uncaughtException_UncaughtException_Ljava_lang_Thread_Ljava_lang_Throwable__V;

		internal static nint class_ref
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		public override JniPeerMembers JniPeerMembers => _members;

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override nint ThresholdClass
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override global::System.Type ThresholdType => _members.ManagedPeerType;

		protected VLCCrashHandler(nint javaReference, JniHandleOwnership transfer)
			: base((global::System.IntPtr)javaReference, transfer)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		[Register(".ctor", "()V", "")]
		public unsafe VLCCrashHandler()
			: base(global::System.IntPtr.Zero, (JniHandleOwnership)0)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			if (((Object)this).Handle == global::System.IntPtr.Zero)
			{
				JniObjectReference val = _members.InstanceMethods.StartCreateInstance("()V", ((object)this).GetType(), (JniArgumentValue*)null);
				((Object)this).SetHandle(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
				_members.InstanceMethods.FinishCreateInstance("()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			}
		}

		private static global::System.Delegate GetUncaughtException_Ljava_lang_Thread_Ljava_lang_Throwable_Handler()
		{
			if (cb_uncaughtException_UncaughtException_Ljava_lang_Thread_Ljava_lang_Throwable__V == null)
			{
				cb_uncaughtException_UncaughtException_Ljava_lang_Thread_Ljava_lang_Throwable__V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPLL_V(n_UncaughtException_Ljava_lang_Thread_Ljava_lang_Throwable_));
			}
			return cb_uncaughtException_UncaughtException_Ljava_lang_Thread_Ljava_lang_Throwable__V;
		}

		private static void n_UncaughtException_Ljava_lang_Thread_Ljava_lang_Throwable_(nint jnienv, nint native__this, nint native_thread, nint native_ex)
		{
			VLCCrashHandler vLCCrashHandler = Object.GetObject<VLCCrashHandler>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			Thread thread = Object.GetObject<Thread>((global::System.IntPtr)native_thread, (JniHandleOwnership)0);
			Throwable ex = Object.GetObject<Throwable>((global::System.IntPtr)native_ex, (JniHandleOwnership)0);
			vLCCrashHandler.UncaughtException(thread, ex);
		}

		[Register("uncaughtException", "(Ljava/lang/Thread;Ljava/lang/Throwable;)V", "GetUncaughtException_Ljava_lang_Thread_Ljava_lang_Throwable_Handler")]
		public unsafe virtual void UncaughtException(Thread? thread, Throwable? ex)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((thread == null) ? global::System.IntPtr.Zero : ((Object)thread).Handle));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue((ex == null) ? global::System.IntPtr.Zero : ex.Handle));
				_members.InstanceMethods.InvokeVirtualVoidMethod("uncaughtException.(Ljava/lang/Thread;Ljava/lang/Throwable;)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)thread);
				GC.KeepAlive((object)ex);
			}
		}
	}
}
namespace Com.Rearcam
{
	[Register("com/rearcam/RealCam", DoNotGenerateAcw = true)]
	public class RealCam : Object
	{
		[Register("com/rearcam/RealCam$Callback", "", "Com.Rearcam.RealCam/ICallbackInvoker")]
		public interface ICallback : IJavaObject, global::System.IDisposable, IJavaPeerable
		{
			[Register("onAudio", "(J[B)V", "GetOnAudio_JarrayBHandler:Com.Rearcam.RealCam/ICallbackInvoker, ids.camera.insight.binding")]
			void OnAudio(long p0, byte[]? p1);

			[Register("onFWInfo", "([B)V", "GetOnFWInfo_arrayBHandler:Com.Rearcam.RealCam/ICallbackInvoker, ids.camera.insight.binding")]
			void OnFWInfo(byte[]? p0);

			[Register("onHeartBeatEvent", "(I)V", "GetOnHeartBeatEvent_IHandler:Com.Rearcam.RealCam/ICallbackInvoker, ids.camera.insight.binding")]
			void OnHeartBeatEvent(int p0);

			[Register("onNotify", "(I[B)V", "GetOnNotify_IarrayBHandler:Com.Rearcam.RealCam/ICallbackInvoker, ids.camera.insight.binding")]
			void OnNotify(int p0, byte[]? p1);

			[Register("onUpdateFWPercent", "(II)V", "GetOnUpdateFWPercent_IIHandler:Com.Rearcam.RealCam/ICallbackInvoker, ids.camera.insight.binding")]
			void OnUpdateFWPercent(int p0, int p1);

			[Register("onVideo", "(J[B)V", "GetOnVideo_JarrayBHandler:Com.Rearcam.RealCam/ICallbackInvoker, ids.camera.insight.binding")]
			void OnVideo(long p0, byte[]? p1);
		}

		[Register("com/rearcam/RealCam$Callback", DoNotGenerateAcw = true)]
		internal class ICallbackInvoker : Object, ICallback, IJavaObject, global::System.IDisposable, IJavaPeerable
		{
			private static readonly JniPeerMembers _members_com_rearcam_RealCam_Callback = (JniPeerMembers)new XAPeerMembers("com/rearcam/RealCam$Callback", typeof(ICallbackInvoker));

			private static global::System.Delegate? cb_onAudio_OnAudio_JarrayB_V;

			private static global::System.Delegate? cb_onFWInfo_OnFWInfo_arrayB_V;

			private static global::System.Delegate? cb_onHeartBeatEvent_OnHeartBeatEvent_I_V;

			private static global::System.Delegate? cb_onNotify_OnNotify_IarrayB_V;

			private static global::System.Delegate? cb_onUpdateFWPercent_OnUpdateFWPercent_II_V;

			private static global::System.Delegate? cb_onVideo_OnVideo_JarrayB_V;

			private static nint java_class_ref
			{
				get
				{
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					JniObjectReference peerReference = _members_com_rearcam_RealCam_Callback.JniPeerType.PeerReference;
					return ((JniObjectReference)(ref peerReference)).Handle;
				}
			}

			[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
			[EditorBrowsable(/*Could not decode attribute arguments.*/)]
			public override JniPeerMembers JniPeerMembers => _members_com_rearcam_RealCam_Callback;

			[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
			[EditorBrowsable(/*Could not decode attribute arguments.*/)]
			protected override nint ThresholdClass
			{
				get
				{
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					JniObjectReference peerReference = _members_com_rearcam_RealCam_Callback.JniPeerType.PeerReference;
					return ((JniObjectReference)(ref peerReference)).Handle;
				}
			}

			[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
			[EditorBrowsable(/*Could not decode attribute arguments.*/)]
			protected override global::System.Type ThresholdType => _members_com_rearcam_RealCam_Callback.ManagedPeerType;

			public ICallbackInvoker(nint handle, JniHandleOwnership transfer)
				: base((global::System.IntPtr)handle, transfer)
			{
			}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


			private static global::System.Delegate GetOnAudio_JarrayBHandler()
			{
				if (cb_onAudio_OnAudio_JarrayB_V == null)
				{
					cb_onAudio_OnAudio_JarrayB_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPJL_V(n_OnAudio_JarrayB));
				}
				return cb_onAudio_OnAudio_JarrayB_V;
			}

			private static void n_OnAudio_JarrayB(nint jnienv, nint native__this, long p0, nint native_p1)
			{
				ICallback callback = Object.GetObject<ICallback>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
				byte[] array = (byte[])JNIEnv.GetArray((global::System.IntPtr)native_p1, (JniHandleOwnership)0, typeof(byte));
				callback.OnAudio(p0, array);
				if (array != null)
				{
					JNIEnv.CopyArray(array, (global::System.IntPtr)native_p1);
				}
			}

			public unsafe void OnAudio(long p0, byte[]? p1)
			{
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				nint num = JNIEnv.NewArray(p1);
				try
				{
					JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
					global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(p0));
					global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue((global::System.IntPtr)num));
					_members_com_rearcam_RealCam_Callback.InstanceMethods.InvokeAbstractVoidMethod("onAudio.(J[B)V", (IJavaPeerable)(object)this, ptr);
				}
				finally
				{
					if (p1 != null)
					{
						JNIEnv.CopyArray((global::System.IntPtr)num, p1);
						JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
					}
					GC.KeepAlive((object)p1);
				}
			}

			private static global::System.Delegate GetOnFWInfo_arrayBHandler()
			{
				if (cb_onFWInfo_OnFWInfo_arrayB_V == null)
				{
					cb_onFWInfo_OnFWInfo_arrayB_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPL_V(n_OnFWInfo_arrayB));
				}
				return cb_onFWInfo_OnFWInfo_arrayB_V;
			}

			private static void n_OnFWInfo_arrayB(nint jnienv, nint native__this, nint native_p0)
			{
				ICallback callback = Object.GetObject<ICallback>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
				byte[] array = (byte[])JNIEnv.GetArray((global::System.IntPtr)native_p0, (JniHandleOwnership)0, typeof(byte));
				callback.OnFWInfo(array);
				if (array != null)
				{
					JNIEnv.CopyArray(array, (global::System.IntPtr)native_p0);
				}
			}

			public unsafe void OnFWInfo(byte[]? p0)
			{
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				nint num = JNIEnv.NewArray(p0);
				try
				{
					JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
					global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((global::System.IntPtr)num));
					_members_com_rearcam_RealCam_Callback.InstanceMethods.InvokeAbstractVoidMethod("onFWInfo.([B)V", (IJavaPeerable)(object)this, ptr);
				}
				finally
				{
					if (p0 != null)
					{
						JNIEnv.CopyArray((global::System.IntPtr)num, p0);
						JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
					}
					GC.KeepAlive((object)p0);
				}
			}

			private static global::System.Delegate GetOnHeartBeatEvent_IHandler()
			{
				if (cb_onHeartBeatEvent_OnHeartBeatEvent_I_V == null)
				{
					cb_onHeartBeatEvent_OnHeartBeatEvent_I_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPI_V(n_OnHeartBeatEvent_I));
				}
				return cb_onHeartBeatEvent_OnHeartBeatEvent_I_V;
			}

			private static void n_OnHeartBeatEvent_I(nint jnienv, nint native__this, int p0)
			{
				Object.GetObject<ICallback>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).OnHeartBeatEvent(p0);
			}

			public unsafe void OnHeartBeatEvent(int p0)
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(p0));
				_members_com_rearcam_RealCam_Callback.InstanceMethods.InvokeAbstractVoidMethod("onHeartBeatEvent.(I)V", (IJavaPeerable)(object)this, ptr);
			}

			private static global::System.Delegate GetOnNotify_IarrayBHandler()
			{
				if (cb_onNotify_OnNotify_IarrayB_V == null)
				{
					cb_onNotify_OnNotify_IarrayB_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPIL_V(n_OnNotify_IarrayB));
				}
				return cb_onNotify_OnNotify_IarrayB_V;
			}

			private static void n_OnNotify_IarrayB(nint jnienv, nint native__this, int p0, nint native_p1)
			{
				ICallback callback = Object.GetObject<ICallback>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
				byte[] array = (byte[])JNIEnv.GetArray((global::System.IntPtr)native_p1, (JniHandleOwnership)0, typeof(byte));
				callback.OnNotify(p0, array);
				if (array != null)
				{
					JNIEnv.CopyArray(array, (global::System.IntPtr)native_p1);
				}
			}

			public unsafe void OnNotify(int p0, byte[]? p1)
			{
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				nint num = JNIEnv.NewArray(p1);
				try
				{
					JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
					global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(p0));
					global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue((global::System.IntPtr)num));
					_members_com_rearcam_RealCam_Callback.InstanceMethods.InvokeAbstractVoidMethod("onNotify.(I[B)V", (IJavaPeerable)(object)this, ptr);
				}
				finally
				{
					if (p1 != null)
					{
						JNIEnv.CopyArray((global::System.IntPtr)num, p1);
						JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
					}
					GC.KeepAlive((object)p1);
				}
			}

			private static global::System.Delegate GetOnUpdateFWPercent_IIHandler()
			{
				if (cb_onUpdateFWPercent_OnUpdateFWPercent_II_V == null)
				{
					cb_onUpdateFWPercent_OnUpdateFWPercent_II_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPII_V(n_OnUpdateFWPercent_II));
				}
				return cb_onUpdateFWPercent_OnUpdateFWPercent_II_V;
			}

			private static void n_OnUpdateFWPercent_II(nint jnienv, nint native__this, int p0, int p1)
			{
				Object.GetObject<ICallback>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).OnUpdateFWPercent(p0, p1);
			}

			public unsafe void OnUpdateFWPercent(int p0, int p1)
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(p0));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(p1));
				_members_com_rearcam_RealCam_Callback.InstanceMethods.InvokeAbstractVoidMethod("onUpdateFWPercent.(II)V", (IJavaPeerable)(object)this, ptr);
			}

			private static global::System.Delegate GetOnVideo_JarrayBHandler()
			{
				if (cb_onVideo_OnVideo_JarrayB_V == null)
				{
					cb_onVideo_OnVideo_JarrayB_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPJL_V(n_OnVideo_JarrayB));
				}
				return cb_onVideo_OnVideo_JarrayB_V;
			}

			private static void n_OnVideo_JarrayB(nint jnienv, nint native__this, long p0, nint native_p1)
			{
				ICallback callback = Object.GetObject<ICallback>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
				byte[] array = (byte[])JNIEnv.GetArray((global::System.IntPtr)native_p1, (JniHandleOwnership)0, typeof(byte));
				callback.OnVideo(p0, array);
				if (array != null)
				{
					JNIEnv.CopyArray(array, (global::System.IntPtr)native_p1);
				}
			}

			public unsafe void OnVideo(long p0, byte[]? p1)
			{
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				nint num = JNIEnv.NewArray(p1);
				try
				{
					JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
					global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(p0));
					global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue((global::System.IntPtr)num));
					_members_com_rearcam_RealCam_Callback.InstanceMethods.InvokeAbstractVoidMethod("onVideo.(J[B)V", (IJavaPeerable)(object)this, ptr);
				}
				finally
				{
					if (p1 != null)
					{
						JNIEnv.CopyArray((global::System.IntPtr)num, p1);
						JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
					}
					GC.KeepAlive((object)p1);
				}
			}
		}

		[Register("APP_OTA_UPDT_BURN_SF")]
		public const int AppOtaUpdtBurnSf = 4;

		[Register("APP_OTA_UPDT_CRC_OK")]
		public const int AppOtaUpdtCrcOk = 3;

		[Register("APP_OTA_UPDT_HDR_TAG_OK")]
		public const int AppOtaUpdtHdrTagOk = 2;

		[Register("APP_OTA_UPDT_SF_TRAN_OK")]
		public const int AppOtaUpdtSfTranOk = 1;

		[Register("APP_OTA_UPDT_SUCCESS")]
		public const int AppOtaUpdtSuccess = 0;

		[Register("E_FW_DATA_BT")]
		public const int EFwDataBt = 4;

		[Register("E_FW_DATA_MASK")]
		public const int EFwDataMask = 5;

		[Register("E_FW_DATA_TX")]
		public const int EFwDataTx = 1;

		[Register("E_OPCODE_GET")]
		public const int EOpcodeGet = 1;

		[Register("E_OPCODE_SET")]
		public const int EOpcodeSet = 0;

		[Register("E_TWC_CMD_OK")]
		public const int ETwcCmdOk = 0;

		[Register("E_VDO_STOP_CERTIFICATE_FAIL")]
		public const int EVdoStopCertificateFail = 2;

		[Register("E_VDO_STOP_NO_RESOURCE_CONNECT")]
		public const int EVdoStopNoResourceConnect = 1;

		[Register("FW_OTA_UPDT_FAIL_APP_ERR")]
		public const int FwOtaUpdtFailAppErr = 20;

		[Register("FW_OTA_UPDT_FAIL_CRC_ERR")]
		public const int FwOtaUpdtFailCrcErr = 1;

		[Register("FW_OTA_UPDT_FAIL_FS_ERR")]
		public const int FwOtaUpdtFailFsErr = 2;

		[Register("FW_OTA_UPDT_FAIL_IMG_ERR")]
		public const int FwOtaUpdtFailImgErr = 3;

		[Register("FW_OTA_UPDT_FAIL_IQ_ERR")]
		public const int FwOtaUpdtFailIqErr = 7;

		[Register("FW_OTA_UPDT_FAIL_PROFILE_ERR")]
		public const int FwOtaUpdtFailProfileErr = 6;

		[Register("FW_OTA_UPDT_FAIL_SIZE_IMG_HS_ERR")]
		public const int FwOtaUpdtFailSizeImgHsErr = 8;

		[Register("FW_OTA_UPDT_FAIL_SIZE_IQ_ERR")]
		public const int FwOtaUpdtFailSizeIqErr = 10;

		[Register("FW_OTA_UPDT_FAIL_SIZE_PROFILE_ERR")]
		public const int FwOtaUpdtFailSizeProfileErr = 9;

		[Register("FW_OTA_UPDT_FAIL_TAG_ERR")]
		public const int FwOtaUpdtFailTagErr = 4;

		[Register("FW_OTA_UPDT_FAIL_TOTAL_SIZE_ERR")]
		public const int FwOtaUpdtFailTotalSizeErr = 11;

		[Register("FW_OTA_UPDT_FAIL_TYPE_ERR")]
		public const int FwOtaUpdtFailTypeErr = 5;

		[Register("FW_OTA_UPDT_FAIL_WIFI_LOST_ERR")]
		public const int FwOtaUpdtFailWifiLostErr = 12;

		[Register("UI_TWC_ADD_VOL_CTL")]
		public const int UiTwcAddVolCtl = 140;

		[Register("UI_TWC_CHANGE_RF_MODE")]
		public const int UiTwcChangeRfMode = 142;

		[Register("UI_TWC_PAIRING")]
		public const int UiTwcPairing = 145;

		[Register("UI_TWC_PUSHTALK_FUNC")]
		public const int UiTwcPushtalkFunc = 141;

		[Register("UI_TWC_SET_PF_PASSWORD")]
		public const int UiTwcSetPfPassword = 144;

		[Register("WD_OP_FRM_OK")]
		public const int WdOpFrmOk = 7;

		[Register("WD_OP_NOTIFY_FW_UPDATE")]
		public const int WdOpNotifyFwUpdate = 20;

		[Register("WD_OP_SEND_FWUPDATE_FAIL")]
		public const int WdOpSendFwupdateFail = 22;

		[Register("WD_OP_SEND_FWUPDATE_OK")]
		public const int WdOpSendFwupdateOk = 21;

		[Register("WD_OP_VDO_IQ_BRIGHTNESS")]
		public const int WdOpVdoIqBrightness = 128;

		[Register("WD_OP_VDO_IQ_CHROMA")]
		public const int WdOpVdoIqChroma = 129;

		[Register("WD_OP_VDO_IQ_CONTRACT")]
		public const int WdOpVdoIqContract = 130;

		[Register("WD_OP_VDO_IQ_SATURATION")]
		public const int WdOpVdoIqSaturation = 131;

		[Register("WD_OP_VDO_PARM_MIRROR_FLIP")]
		public const int WdOpVdoParmMirrorFlip = 48;

		[Register("WD_OP_VDO_SENSOR_POWER_FREQ")]
		public const int WdOpVdoSensorPowerFreq = 64;

		[Register("WD_OP_VDO_STOP")]
		public const int WdOpVdoStop = 1;

		private static readonly JniPeerMembers _members = (JniPeerMembers)new XAPeerMembers("com/rearcam/RealCam", typeof(RealCam));

		[Register("BEACON_DATA")]
		public static global::System.Collections.Generic.IList<byte>? BeaconData
		{
			get
			{
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference objectValue = _members.StaticFields.GetObjectValue("BEACON_DATA.[B");
				return (global::System.Collections.Generic.IList<byte>?)JavaArray<byte>.FromJniHandle(((JniObjectReference)(ref objectValue)).Handle, (JniHandleOwnership)1);
			}
		}

		[Register("BEACON_DATA1")]
		public static global::System.Collections.Generic.IList<byte>? BeaconData1
		{
			get
			{
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference objectValue = _members.StaticFields.GetObjectValue("BEACON_DATA1.[B");
				return (global::System.Collections.Generic.IList<byte>?)JavaArray<byte>.FromJniHandle(((JniObjectReference)(ref objectValue)).Handle, (JniHandleOwnership)1);
			}
		}

		[Register("BEACON_DATA2")]
		public static global::System.Collections.Generic.IList<byte>? BeaconData2
		{
			get
			{
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference objectValue = _members.StaticFields.GetObjectValue("BEACON_DATA2.[B");
				return (global::System.Collections.Generic.IList<byte>?)JavaArray<byte>.FromJniHandle(((JniObjectReference)(ref objectValue)).Handle, (JniHandleOwnership)1);
			}
		}

		[Register("DEVICE_FW_BT_VERSION")]
		public static global::System.Collections.Generic.IList<int>? DeviceFwBtVersion
		{
			get
			{
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference objectValue = _members.StaticFields.GetObjectValue("DEVICE_FW_BT_VERSION.[I");
				return (global::System.Collections.Generic.IList<int>?)JavaArray<int>.FromJniHandle(((JniObjectReference)(ref objectValue)).Handle, (JniHandleOwnership)1);
			}
			set
			{
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				nint num = JavaArray<int>.ToLocalJniHandle(value);
				try
				{
					_members.StaticFields.SetValue("DEVICE_FW_BT_VERSION.[I", new JniObjectReference((global::System.IntPtr)num, (JniObjectReferenceType)0));
				}
				finally
				{
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				}
			}
		}

		[Register("DEVICE_FW_TX_VERSION")]
		public static global::System.Collections.Generic.IList<int>? DeviceFwTxVersion
		{
			get
			{
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference objectValue = _members.StaticFields.GetObjectValue("DEVICE_FW_TX_VERSION.[I");
				return (global::System.Collections.Generic.IList<int>?)JavaArray<int>.FromJniHandle(((JniObjectReference)(ref objectValue)).Handle, (JniHandleOwnership)1);
			}
			set
			{
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				nint num = JavaArray<int>.ToLocalJniHandle(value);
				try
				{
					_members.StaticFields.SetValue("DEVICE_FW_TX_VERSION.[I", new JniObjectReference((global::System.IntPtr)num, (JniObjectReferenceType)0));
				}
				finally
				{
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				}
			}
		}

		[Register("mSyncThreadSleep")]
		public static int MSyncThreadSleep
		{
			get
			{
				return _members.StaticFields.GetInt32Value("mSyncThreadSleep.I");
			}
			set
			{
				_members.StaticFields.SetValue("mSyncThreadSleep.I", value);
			}
		}

		[Register("packetSize")]
		public static int PacketSize
		{
			get
			{
				return _members.StaticFields.GetInt32Value("packetSize.I");
			}
			set
			{
				_members.StaticFields.SetValue("packetSize.I", value);
			}
		}

		internal static nint class_ref
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		public override JniPeerMembers JniPeerMembers => _members;

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override nint ThresholdClass
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override global::System.Type ThresholdType => _members.ManagedPeerType;

		public unsafe static int Bitrate
		{
			[Register("getBitrate", "()I", "")]
			get
			{
				return _members.StaticMethods.InvokeInt32Method("getBitrate.()I", (JniArgumentValue*)null);
			}
		}

		public unsafe static bool IsInit
		{
			[Register("isInit", "()Z", "")]
			get
			{
				return _members.StaticMethods.InvokeBooleanMethod("isInit.()Z", (JniArgumentValue*)null);
			}
		}

		public unsafe static int UdpPort
		{
			[Register("getUdpPort", "()I", "")]
			get
			{
				return _members.StaticMethods.InvokeInt32Method("getUdpPort.()I", (JniArgumentValue*)null);
			}
		}

		protected RealCam(nint javaReference, JniHandleOwnership transfer)
			: base((global::System.IntPtr)javaReference, transfer)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		[Register(".ctor", "()V", "")]
		public unsafe RealCam()
			: base(global::System.IntPtr.Zero, (JniHandleOwnership)0)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			if (((Object)this).Handle == global::System.IntPtr.Zero)
			{
				JniObjectReference val = _members.InstanceMethods.StartCreateInstance("()V", ((object)this).GetType(), (JniArgumentValue*)null);
				((Object)this).SetHandle(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
				_members.InstanceMethods.FinishCreateInstance("()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			}
		}

		[Register("audio32Decode", "([B[BI)I", "")]
		public unsafe static int Audio32Decode(byte[]? p0, byte[]? p1, int p2)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			nint num = JNIEnv.NewArray(p0);
			nint num2 = JNIEnv.NewArray(p1);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[3];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((global::System.IntPtr)num));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue((global::System.IntPtr)num2));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + (nint)2 * (nint)global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(p2));
				return _members.StaticMethods.InvokeInt32Method("audio32Decode.([B[BI)I", ptr);
			}
			finally
			{
				if (p0 != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num, p0);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				}
				if (p1 != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num2, p1);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num2);
				}
				GC.KeepAlive((object)p0);
				GC.KeepAlive((object)p1);
			}
		}

		[Register("audio32DecodeInit", "()Z", "")]
		public unsafe static bool Audio32DecodeInit()
		{
			return _members.StaticMethods.InvokeBooleanMethod("audio32DecodeInit.()Z", (JniArgumentValue*)null);
		}

		[Register("audio32Encode", "([B[BI)I", "")]
		public unsafe static int Audio32Encode(byte[]? p0, byte[]? p1, int p2)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			nint num = JNIEnv.NewArray(p0);
			nint num2 = JNIEnv.NewArray(p1);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[3];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((global::System.IntPtr)num));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue((global::System.IntPtr)num2));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + (nint)2 * (nint)global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(p2));
				return _members.StaticMethods.InvokeInt32Method("audio32Encode.([B[BI)I", ptr);
			}
			finally
			{
				if (p0 != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num, p0);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				}
				if (p1 != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num2, p1);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num2);
				}
				GC.KeepAlive((object)p0);
				GC.KeepAlive((object)p1);
			}
		}

		[Register("audio32EncodeInit", "()Z", "")]
		public unsafe static bool Audio32EncodeInit()
		{
			return _members.StaticMethods.InvokeBooleanMethod("audio32EncodeInit.()Z", (JniArgumentValue*)null);
		}

		[Register("checkFw", "([B)Z", "")]
		public unsafe static bool CheckFw(byte[]? fw)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			nint num = JNIEnv.NewArray(fw);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((global::System.IntPtr)num));
				return _members.StaticMethods.InvokeBooleanMethod("checkFw.([B)Z", ptr);
			}
			finally
			{
				if (fw != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num, fw);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				}
				GC.KeepAlive((object)fw);
			}
		}

		[Register("intToByteShort", "(S)[B", "")]
		public unsafe static byte[]? IntToByteShort(short s)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
			global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(s));
			JniObjectReference val = _members.StaticMethods.InvokeObjectMethod("intToByteShort.(S)[B", ptr);
			return (byte[])JNIEnv.GetArray(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1, typeof(byte));
		}

		[Register("interruptUpdateFw", "()V", "")]
		public unsafe static void InterruptUpdateFw()
		{
			_members.StaticMethods.InvokeVoidMethod("interruptUpdateFw.()V", (JniArgumentValue*)null);
		}

		[Register("libInit", "(Landroid/content/Context;I)V", "")]
		public unsafe static void LibInit(Context? context, int timeout)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((context == null) ? global::System.IntPtr.Zero : ((Object)context).Handle));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(timeout));
				_members.StaticMethods.InvokeVoidMethod("libInit.(Landroid/content/Context;I)V", ptr);
			}
			finally
			{
				GC.KeepAlive((object)context);
			}
		}

		[Register("libUninit", "()V", "")]
		public unsafe static void LibUninit()
		{
			_members.StaticMethods.InvokeVoidMethod("libUninit.()V", (JniArgumentValue*)null);
		}

		[Register("sendAudioData", "([BI)V", "")]
		public unsafe static void SendAudioData(byte[]? p0, int p1)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			nint num = JNIEnv.NewArray(p0);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((global::System.IntPtr)num));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(p1));
				_members.StaticMethods.InvokeVoidMethod("sendAudioData.([BI)V", ptr);
			}
			finally
			{
				if (p0 != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num, p0);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				}
				GC.KeepAlive((object)p0);
			}
		}

		[Register("sendStartCmd", "([B)V", "")]
		public unsafe static void SendStartCmd(byte[]? p0)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			nint num = JNIEnv.NewArray(p0);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((global::System.IntPtr)num));
				_members.StaticMethods.InvokeVoidMethod("sendStartCmd.([B)V", ptr);
			}
			finally
			{
				if (p0 != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num, p0);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				}
				GC.KeepAlive((object)p0);
			}
		}

		[Register("sendStopCmd", "()V", "")]
		public unsafe static void SendStopCmd()
		{
			_members.StaticMethods.InvokeVoidMethod("sendStopCmd.()V", (JniArgumentValue*)null);
		}

		[Register("sendSyncCmd", "()V", "")]
		public unsafe static void SendSyncCmd()
		{
			_members.StaticMethods.InvokeVoidMethod("sendSyncCmd.()V", (JniArgumentValue*)null);
		}

		[Register("sendTWCCmd", "(B[B)V", "")]
		public unsafe static void SendTWCCmd(sbyte opCode, byte[]? data)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			nint num = JNIEnv.NewArray(data);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(opCode));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue((global::System.IntPtr)num));
				_members.StaticMethods.InvokeVoidMethod("sendTWCCmd.(B[B)V", ptr);
			}
			finally
			{
				if (data != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num, data);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				}
				GC.KeepAlive((object)data);
			}
		}

		[Register("sendTWCCmd", "(B[BI)V", "")]
		public unsafe static void SendTWCCmd(sbyte opCode, byte[]? data, int count)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			nint num = JNIEnv.NewArray(data);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[3];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(opCode));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue((global::System.IntPtr)num));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + (nint)2 * (nint)global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(count));
				_members.StaticMethods.InvokeVoidMethod("sendTWCCmd.(B[BI)V", ptr);
			}
			finally
			{
				if (data != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num, data);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				}
				GC.KeepAlive((object)data);
			}
		}

		[Register("setCallback", "(Lcom/rearcam/RealCam$Callback;)V", "")]
		public unsafe static void SetCallback(ICallback? callback)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((callback == null) ? global::System.IntPtr.Zero : ((Object)callback).Handle));
				_members.StaticMethods.InvokeVoidMethod("setCallback.(Lcom/rearcam/RealCam$Callback;)V", ptr);
			}
			finally
			{
				GC.KeepAlive((object)callback);
			}
		}

		[Register("setHeartBeatSleep", "(I[B)V", "")]
		public unsafe static void SetHeartBeatSleep(int miliScond, byte[]? data)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			nint num = JNIEnv.NewArray(data);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(miliScond));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue((global::System.IntPtr)num));
				_members.StaticMethods.InvokeVoidMethod("setHeartBeatSleep.(I[B)V", ptr);
			}
			finally
			{
				if (data != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num, data);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				}
				GC.KeepAlive((object)data);
			}
		}

		[Register("setLibraryLogEnable", "(Z)V", "")]
		public unsafe static void SetLibraryLogEnable(bool enable)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
			global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(enable));
			_members.StaticMethods.InvokeVoidMethod("setLibraryLogEnable.(Z)V", ptr);
		}

		[Register("setPreviewInfo", "(II)V", "")]
		public unsafe static void SetPreviewInfo(int p0, int p1)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
			global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(p0));
			global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(p1));
			_members.StaticMethods.InvokeVoidMethod("setPreviewInfo.(II)V", ptr);
		}

		[Register("setWEP", "([B)Z", "")]
		public unsafe static bool SetWEP(byte[]? pwd)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			nint num = JNIEnv.NewArray(pwd);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((global::System.IntPtr)num));
				return _members.StaticMethods.InvokeBooleanMethod("setWEP.([B)Z", ptr);
			}
			finally
			{
				if (pwd != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num, pwd);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				}
				GC.KeepAlive((object)pwd);
			}
		}

		[Register("stopStartCmd", "()V", "")]
		public unsafe static void StopStartCmd()
		{
			_members.StaticMethods.InvokeVoidMethod("stopStartCmd.()V", (JniArgumentValue*)null);
		}

		[Register("stopSyncCmd", "()V", "")]
		public unsafe static void StopSyncCmd()
		{
			_members.StaticMethods.InvokeVoidMethod("stopSyncCmd.()V", (JniArgumentValue*)null);
		}

		[Register("updateFw", "(III[BLjava/lang/String;)V", "")]
		public unsafe static void UpdateFw(int p0, int p1, int p2, byte[]? p3, string? p4)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			nint num = JNIEnv.NewArray(p3);
			nint num2 = JNIEnv.NewString(p4);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[5];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(p0));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(p1));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + (nint)2 * (nint)global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(p2));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + (nint)3 * (nint)global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue((global::System.IntPtr)num));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + (nint)4 * (nint)global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue((global::System.IntPtr)num2));
				_members.StaticMethods.InvokeVoidMethod("updateFw.(III[BLjava/lang/String;)V", ptr);
			}
			finally
			{
				if (p3 != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num, p3);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				}
				JNIEnv.DeleteLocalRef((global::System.IntPtr)num2);
				GC.KeepAlive((object)p3);
			}
		}
	}
}
namespace Com.Module
{
	[Register("com/module/AudioRecordController", DoNotGenerateAcw = true)]
	public class AudioRecordController : Object
	{
		[Register("com/module/AudioRecordController$AudioFrameCallback", "", "Com.Module.AudioRecordController/IAudioFrameCallbackInvoker")]
		public interface IAudioFrameCallback : IJavaObject, global::System.IDisposable, IJavaPeerable
		{
			[Register("onAudio", "([B)V", "GetOnAudio_arrayBHandler:Com.Module.AudioRecordController/IAudioFrameCallbackInvoker, ids.camera.insight.binding")]
			void OnAudio(byte[]? p0);
		}

		[Register("com/module/AudioRecordController$AudioFrameCallback", DoNotGenerateAcw = true)]
		internal class IAudioFrameCallbackInvoker : Object, IAudioFrameCallback, IJavaObject, global::System.IDisposable, IJavaPeerable
		{
			private static readonly JniPeerMembers _members_com_module_AudioRecordController_AudioFrameCallback = (JniPeerMembers)new XAPeerMembers("com/module/AudioRecordController$AudioFrameCallback", typeof(IAudioFrameCallbackInvoker));

			private static global::System.Delegate? cb_onAudio_OnAudio_arrayB_V;

			private static nint java_class_ref
			{
				get
				{
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					JniObjectReference peerReference = _members_com_module_AudioRecordController_AudioFrameCallback.JniPeerType.PeerReference;
					return ((JniObjectReference)(ref peerReference)).Handle;
				}
			}

			[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
			[EditorBrowsable(/*Could not decode attribute arguments.*/)]
			public override JniPeerMembers JniPeerMembers => _members_com_module_AudioRecordController_AudioFrameCallback;

			[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
			[EditorBrowsable(/*Could not decode attribute arguments.*/)]
			protected override nint ThresholdClass
			{
				get
				{
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					JniObjectReference peerReference = _members_com_module_AudioRecordController_AudioFrameCallback.JniPeerType.PeerReference;
					return ((JniObjectReference)(ref peerReference)).Handle;
				}
			}

			[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
			[EditorBrowsable(/*Could not decode attribute arguments.*/)]
			protected override global::System.Type ThresholdType => _members_com_module_AudioRecordController_AudioFrameCallback.ManagedPeerType;

			public IAudioFrameCallbackInvoker(nint handle, JniHandleOwnership transfer)
				: base((global::System.IntPtr)handle, transfer)
			{
			}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


			private static global::System.Delegate GetOnAudio_arrayBHandler()
			{
				if (cb_onAudio_OnAudio_arrayB_V == null)
				{
					cb_onAudio_OnAudio_arrayB_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPL_V(n_OnAudio_arrayB));
				}
				return cb_onAudio_OnAudio_arrayB_V;
			}

			private static void n_OnAudio_arrayB(nint jnienv, nint native__this, nint native_p0)
			{
				IAudioFrameCallback audioFrameCallback = Object.GetObject<IAudioFrameCallback>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
				byte[] array = (byte[])JNIEnv.GetArray((global::System.IntPtr)native_p0, (JniHandleOwnership)0, typeof(byte));
				audioFrameCallback.OnAudio(array);
				if (array != null)
				{
					JNIEnv.CopyArray(array, (global::System.IntPtr)native_p0);
				}
			}

			public unsafe void OnAudio(byte[]? p0)
			{
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				nint num = JNIEnv.NewArray(p0);
				try
				{
					JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
					global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((global::System.IntPtr)num));
					_members_com_module_AudioRecordController_AudioFrameCallback.InstanceMethods.InvokeAbstractVoidMethod("onAudio.([B)V", (IJavaPeerable)(object)this, ptr);
				}
				finally
				{
					if (p0 != null)
					{
						JNIEnv.CopyArray((global::System.IntPtr)num, p0);
						JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
					}
					GC.KeepAlive((object)p0);
				}
			}
		}

		private static readonly JniPeerMembers _members = (JniPeerMembers)new XAPeerMembers("com/module/AudioRecordController", typeof(AudioRecordController));

		private static global::System.Delegate? cb_isRecord_IsRecord_Z;

		private static global::System.Delegate? cb_setAudioCallback_SetAudioCallback_Lcom_module_AudioRecordController_AudioFrameCallback__V;

		private static global::System.Delegate? cb_startRecord_StartRecord_Z;

		private static global::System.Delegate? cb_stopRecord_StopRecord_V;

		internal static nint class_ref
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		public override JniPeerMembers JniPeerMembers => _members;

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override nint ThresholdClass
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override global::System.Type ThresholdType => _members.ManagedPeerType;

		public unsafe virtual bool IsRecord
		{
			[Register("isRecord", "()Z", "GetIsRecordHandler")]
			get
			{
				return _members.InstanceMethods.InvokeVirtualBooleanMethod("isRecord.()Z", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			}
		}

		protected AudioRecordController(nint javaReference, JniHandleOwnership transfer)
			: base((global::System.IntPtr)javaReference, transfer)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		[Register(".ctor", "(ILandroid/app/Activity;)V", "")]
		public unsafe AudioRecordController(int sampleRate, Activity? context)
			: base(global::System.IntPtr.Zero, (JniHandleOwnership)0)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			if (((Object)this).Handle != global::System.IntPtr.Zero)
			{
				return;
			}
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(sampleRate));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue((context == null) ? global::System.IntPtr.Zero : ((Object)context).Handle));
				JniObjectReference val = _members.InstanceMethods.StartCreateInstance("(ILandroid/app/Activity;)V", ((object)this).GetType(), ptr);
				((Object)this).SetHandle(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
				_members.InstanceMethods.FinishCreateInstance("(ILandroid/app/Activity;)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)context);
			}
		}

		private static global::System.Delegate GetIsRecordHandler()
		{
			if (cb_isRecord_IsRecord_Z == null)
			{
				cb_isRecord_IsRecord_Z = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_Z(n_IsRecord));
			}
			return cb_isRecord_IsRecord_Z;
		}

		private static bool n_IsRecord(nint jnienv, nint native__this)
		{
			return Object.GetObject<AudioRecordController>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).IsRecord;
		}

		private static global::System.Delegate GetSetAudioCallback_Lcom_module_AudioRecordController_AudioFrameCallback_Handler()
		{
			if (cb_setAudioCallback_SetAudioCallback_Lcom_module_AudioRecordController_AudioFrameCallback__V == null)
			{
				cb_setAudioCallback_SetAudioCallback_Lcom_module_AudioRecordController_AudioFrameCallback__V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPL_V(n_SetAudioCallback_Lcom_module_AudioRecordController_AudioFrameCallback_));
			}
			return cb_setAudioCallback_SetAudioCallback_Lcom_module_AudioRecordController_AudioFrameCallback__V;
		}

		private static void n_SetAudioCallback_Lcom_module_AudioRecordController_AudioFrameCallback_(nint jnienv, nint native__this, nint native__callback)
		{
			AudioRecordController audioRecordController = Object.GetObject<AudioRecordController>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			IAudioFrameCallback audioCallback = Object.GetObject<IAudioFrameCallback>((global::System.IntPtr)native__callback, (JniHandleOwnership)0);
			audioRecordController.SetAudioCallback(audioCallback);
		}

		[Register("setAudioCallback", "(Lcom/module/AudioRecordController$AudioFrameCallback;)V", "GetSetAudioCallback_Lcom_module_AudioRecordController_AudioFrameCallback_Handler")]
		public unsafe virtual void SetAudioCallback(IAudioFrameCallback? callback)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((callback == null) ? global::System.IntPtr.Zero : ((Object)callback).Handle));
				_members.InstanceMethods.InvokeVirtualVoidMethod("setAudioCallback.(Lcom/module/AudioRecordController$AudioFrameCallback;)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)callback);
			}
		}

		private static global::System.Delegate GetStartRecordHandler()
		{
			if (cb_startRecord_StartRecord_Z == null)
			{
				cb_startRecord_StartRecord_Z = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_Z(n_StartRecord));
			}
			return cb_startRecord_StartRecord_Z;
		}

		private static bool n_StartRecord(nint jnienv, nint native__this)
		{
			return Object.GetObject<AudioRecordController>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).StartRecord();
		}

		[Register("startRecord", "()Z", "GetStartRecordHandler")]
		public unsafe virtual bool StartRecord()
		{
			return _members.InstanceMethods.InvokeVirtualBooleanMethod("startRecord.()Z", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetStopRecordHandler()
		{
			if (cb_stopRecord_StopRecord_V == null)
			{
				cb_stopRecord_StopRecord_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_StopRecord));
			}
			return cb_stopRecord_StopRecord_V;
		}

		private static void n_StopRecord(nint jnienv, nint native__this)
		{
			Object.GetObject<AudioRecordController>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).StopRecord();
		}

		[Register("stopRecord", "()V", "GetStopRecordHandler")]
		public unsafe virtual void StopRecord()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("stopRecord.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}
	}
	[Register("com/module/AudioUtil", DoNotGenerateAcw = true)]
	public class AudioUtil : Object
	{
		private static readonly JniPeerMembers _members = (JniPeerMembers)new XAPeerMembers("com/module/AudioUtil", typeof(AudioUtil));

		private static global::System.Delegate? cb_getQueueSize_GetQueueSize_I;

		private static global::System.Delegate? cb_clearQueue_ClearQueue_V;

		private static global::System.Delegate? cb_onPause_OnPause_V;

		private static global::System.Delegate? cb_onResume_OnResume_V;

		private static global::System.Delegate? cb_putAudioData_PutAudioData_Lcom_sonix_decoder_AudioObject__V;

		private static global::System.Delegate? cb_releaseAudioTrack_ReleaseAudioTrack_V;

		private static global::System.Delegate? cb_setupAudioTrack_SetupAudioTrack_V;

		private static global::System.Delegate? cb_startPlay_StartPlay_V;

		private static global::System.Delegate? cb_stopPlay_StopPlay_V;

		internal static nint class_ref
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		public override JniPeerMembers JniPeerMembers => _members;

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override nint ThresholdClass
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override global::System.Type ThresholdType => _members.ManagedPeerType;

		public unsafe virtual int QueueSize
		{
			[Register("getQueueSize", "()I", "GetGetQueueSizeHandler")]
			get
			{
				return _members.InstanceMethods.InvokeVirtualInt32Method("getQueueSize.()I", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			}
		}

		protected AudioUtil(nint javaReference, JniHandleOwnership transfer)
			: base((global::System.IntPtr)javaReference, transfer)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		[Register(".ctor", "(Landroid/app/Activity;)V", "")]
		public unsafe AudioUtil(Activity? context)
			: base(global::System.IntPtr.Zero, (JniHandleOwnership)0)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			if (((Object)this).Handle != global::System.IntPtr.Zero)
			{
				return;
			}
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((context == null) ? global::System.IntPtr.Zero : ((Object)context).Handle));
				JniObjectReference val = _members.InstanceMethods.StartCreateInstance("(Landroid/app/Activity;)V", ((object)this).GetType(), ptr);
				((Object)this).SetHandle(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
				_members.InstanceMethods.FinishCreateInstance("(Landroid/app/Activity;)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)context);
			}
		}

		private static global::System.Delegate GetGetQueueSizeHandler()
		{
			if (cb_getQueueSize_GetQueueSize_I == null)
			{
				cb_getQueueSize_GetQueueSize_I = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_I(n_GetQueueSize));
			}
			return cb_getQueueSize_GetQueueSize_I;
		}

		private static int n_GetQueueSize(nint jnienv, nint native__this)
		{
			return Object.GetObject<AudioUtil>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).QueueSize;
		}

		[Register("Audio32DecodeInit", "()V", "")]
		public unsafe static void Audio32DecodeInit()
		{
			_members.StaticMethods.InvokeVoidMethod("Audio32DecodeInit.()V", (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetClearQueueHandler()
		{
			if (cb_clearQueue_ClearQueue_V == null)
			{
				cb_clearQueue_ClearQueue_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_ClearQueue));
			}
			return cb_clearQueue_ClearQueue_V;
		}

		private static void n_ClearQueue(nint jnienv, nint native__this)
		{
			Object.GetObject<AudioUtil>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).ClearQueue();
		}

		[Register("clearQueue", "()V", "GetClearQueueHandler")]
		public unsafe virtual void ClearQueue()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("clearQueue.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetOnPauseHandler()
		{
			if (cb_onPause_OnPause_V == null)
			{
				cb_onPause_OnPause_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_OnPause));
			}
			return cb_onPause_OnPause_V;
		}

		private static void n_OnPause(nint jnienv, nint native__this)
		{
			Object.GetObject<AudioUtil>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).OnPause();
		}

		[Register("onPause", "()V", "GetOnPauseHandler")]
		public unsafe virtual void OnPause()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("onPause.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetOnResumeHandler()
		{
			if (cb_onResume_OnResume_V == null)
			{
				cb_onResume_OnResume_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_OnResume));
			}
			return cb_onResume_OnResume_V;
		}

		private static void n_OnResume(nint jnienv, nint native__this)
		{
			Object.GetObject<AudioUtil>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).OnResume();
		}

		[Register("onResume", "()V", "GetOnResumeHandler")]
		public unsafe virtual void OnResume()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("onResume.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetPutAudioData_Lcom_sonix_decoder_AudioObject_Handler()
		{
			if (cb_putAudioData_PutAudioData_Lcom_sonix_decoder_AudioObject__V == null)
			{
				cb_putAudioData_PutAudioData_Lcom_sonix_decoder_AudioObject__V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPL_V(n_PutAudioData_Lcom_sonix_decoder_AudioObject_));
			}
			return cb_putAudioData_PutAudioData_Lcom_sonix_decoder_AudioObject__V;
		}

		private static void n_PutAudioData_Lcom_sonix_decoder_AudioObject_(nint jnienv, nint native__this, nint native_data)
		{
			AudioUtil audioUtil = Object.GetObject<AudioUtil>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			AudioObject data = Object.GetObject<AudioObject>((global::System.IntPtr)native_data, (JniHandleOwnership)0);
			audioUtil.PutAudioData(data);
		}

		[Register("putAudioData", "(Lcom/sonix/decoder/AudioObject;)V", "GetPutAudioData_Lcom_sonix_decoder_AudioObject_Handler")]
		public unsafe virtual void PutAudioData(AudioObject? data)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((data == null) ? global::System.IntPtr.Zero : ((Object)data).Handle));
				_members.InstanceMethods.InvokeVirtualVoidMethod("putAudioData.(Lcom/sonix/decoder/AudioObject;)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)data);
			}
		}

		private static global::System.Delegate GetReleaseAudioTrackHandler()
		{
			if (cb_releaseAudioTrack_ReleaseAudioTrack_V == null)
			{
				cb_releaseAudioTrack_ReleaseAudioTrack_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_ReleaseAudioTrack));
			}
			return cb_releaseAudioTrack_ReleaseAudioTrack_V;
		}

		private static void n_ReleaseAudioTrack(nint jnienv, nint native__this)
		{
			Object.GetObject<AudioUtil>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).ReleaseAudioTrack();
		}

		[Register("releaseAudioTrack", "()V", "GetReleaseAudioTrackHandler")]
		public unsafe virtual void ReleaseAudioTrack()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("releaseAudioTrack.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetSetupAudioTrackHandler()
		{
			if (cb_setupAudioTrack_SetupAudioTrack_V == null)
			{
				cb_setupAudioTrack_SetupAudioTrack_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_SetupAudioTrack));
			}
			return cb_setupAudioTrack_SetupAudioTrack_V;
		}

		private static void n_SetupAudioTrack(nint jnienv, nint native__this)
		{
			Object.GetObject<AudioUtil>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).SetupAudioTrack();
		}

		[Register("setupAudioTrack", "()V", "GetSetupAudioTrackHandler")]
		public unsafe virtual void SetupAudioTrack()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("setupAudioTrack.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetStartPlayHandler()
		{
			if (cb_startPlay_StartPlay_V == null)
			{
				cb_startPlay_StartPlay_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_StartPlay));
			}
			return cb_startPlay_StartPlay_V;
		}

		private static void n_StartPlay(nint jnienv, nint native__this)
		{
			Object.GetObject<AudioUtil>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).StartPlay();
		}

		[Register("startPlay", "()V", "GetStartPlayHandler")]
		public unsafe virtual void StartPlay()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("startPlay.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetStopPlayHandler()
		{
			if (cb_stopPlay_StopPlay_V == null)
			{
				cb_stopPlay_StopPlay_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_StopPlay));
			}
			return cb_stopPlay_StopPlay_V;
		}

		private static void n_StopPlay(nint jnienv, nint native__this)
		{
			Object.GetObject<AudioUtil>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).StopPlay();
		}

		[Register("stopPlay", "()V", "GetStopPlayHandler")]
		public unsafe virtual void StopPlay()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("stopPlay.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}
	}
	[Register("com/module/NetworkUtil", DoNotGenerateAcw = true)]
	public class NetworkUtil : Object
	{
		private static readonly JniPeerMembers _members = (JniPeerMembers)new XAPeerMembers("com/module/NetworkUtil", typeof(NetworkUtil));

		private static global::System.Delegate? cb_getConnectivityManager_GetConnectivityManager_Landroid_net_ConnectivityManager_;

		private static global::System.Delegate? cb_isMobileNetworkConnected_IsMobileNetworkConnected_Z;

		private static global::System.Delegate? cb_isWifiConnect_IsWifiConnect_Z;

		private static global::System.Delegate? cb_getLocationManager_GetLocationManager_Landroid_location_LocationManager_;

		private static global::System.Delegate? cb_getSsid_GetSsid_Ljava_lang_String_;

		private static global::System.Delegate? cb_getWifiManager_GetWifiManager_Landroid_net_wifi_WifiManager_;

		private static global::System.Delegate? cb_isNetworkConnected_IsNetworkConnected_Ljava_lang_String__Z;

		private static global::System.Delegate? cb_registerNetwork_RegisterNetwork_Landroid_net_NetworkRequest_Landroid_net_ConnectivityManager_NetworkCallback__V;

		private static global::System.Delegate? cb_unregisterNetwork_UnregisterNetwork_V;

		internal static nint class_ref
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		public override JniPeerMembers JniPeerMembers => _members;

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override nint ThresholdClass
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override global::System.Type ThresholdType => _members.ManagedPeerType;

		public unsafe virtual ConnectivityManager? ConnectivityManager
		{
			[Register("getConnectivityManager", "()Landroid/net/ConnectivityManager;", "GetGetConnectivityManagerHandler")]
			get
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference val = _members.InstanceMethods.InvokeVirtualObjectMethod("getConnectivityManager.()Landroid/net/ConnectivityManager;", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
				return Object.GetObject<ConnectivityManager>(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
			}
		}

		public unsafe virtual bool IsMobileNetworkConnected
		{
			[Register("isMobileNetworkConnected", "()Z", "GetIsMobileNetworkConnectedHandler")]
			get
			{
				return _members.InstanceMethods.InvokeVirtualBooleanMethod("isMobileNetworkConnected.()Z", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			}
		}

		public unsafe virtual bool IsWifiConnect
		{
			[Register("isWifiConnect", "()Z", "GetIsWifiConnectHandler")]
			get
			{
				return _members.InstanceMethods.InvokeVirtualBooleanMethod("isWifiConnect.()Z", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			}
		}

		public unsafe virtual LocationManager? LocationManager
		{
			[Register("getLocationManager", "()Landroid/location/LocationManager;", "GetGetLocationManagerHandler")]
			get
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference val = _members.InstanceMethods.InvokeVirtualObjectMethod("getLocationManager.()Landroid/location/LocationManager;", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
				return Object.GetObject<LocationManager>(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
			}
		}

		public unsafe virtual string? Ssid
		{
			[Register("getSsid", "()Ljava/lang/String;", "GetGetSsidHandler")]
			get
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference val = _members.InstanceMethods.InvokeVirtualObjectMethod("getSsid.()Ljava/lang/String;", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
				return JNIEnv.GetString(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
			}
		}

		public unsafe virtual WifiManager? WifiManager
		{
			[Register("getWifiManager", "()Landroid/net/wifi/WifiManager;", "GetGetWifiManagerHandler")]
			get
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference val = _members.InstanceMethods.InvokeVirtualObjectMethod("getWifiManager.()Landroid/net/wifi/WifiManager;", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
				return Object.GetObject<WifiManager>(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
			}
		}

		protected NetworkUtil(nint javaReference, JniHandleOwnership transfer)
			: base((global::System.IntPtr)javaReference, transfer)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		[Register(".ctor", "(Landroid/content/Context;)V", "")]
		public unsafe NetworkUtil(Context? ctx)
			: base(global::System.IntPtr.Zero, (JniHandleOwnership)0)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			if (((Object)this).Handle != global::System.IntPtr.Zero)
			{
				return;
			}
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((ctx == null) ? global::System.IntPtr.Zero : ((Object)ctx).Handle));
				JniObjectReference val = _members.InstanceMethods.StartCreateInstance("(Landroid/content/Context;)V", ((object)this).GetType(), ptr);
				((Object)this).SetHandle(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
				_members.InstanceMethods.FinishCreateInstance("(Landroid/content/Context;)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)ctx);
			}
		}

		private static global::System.Delegate GetGetConnectivityManagerHandler()
		{
			if (cb_getConnectivityManager_GetConnectivityManager_Landroid_net_ConnectivityManager_ == null)
			{
				cb_getConnectivityManager_GetConnectivityManager_Landroid_net_ConnectivityManager_ = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_L(n_GetConnectivityManager));
			}
			return cb_getConnectivityManager_GetConnectivityManager_Landroid_net_ConnectivityManager_;
		}

		private static nint n_GetConnectivityManager(nint jnienv, nint native__this)
		{
			return JNIEnv.ToLocalJniHandle((IJavaObject)(object)Object.GetObject<NetworkUtil>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).ConnectivityManager);
		}

		private static global::System.Delegate GetIsMobileNetworkConnectedHandler()
		{
			if (cb_isMobileNetworkConnected_IsMobileNetworkConnected_Z == null)
			{
				cb_isMobileNetworkConnected_IsMobileNetworkConnected_Z = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_Z(n_IsMobileNetworkConnected));
			}
			return cb_isMobileNetworkConnected_IsMobileNetworkConnected_Z;
		}

		private static bool n_IsMobileNetworkConnected(nint jnienv, nint native__this)
		{
			return Object.GetObject<NetworkUtil>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).IsMobileNetworkConnected;
		}

		private static global::System.Delegate GetIsWifiConnectHandler()
		{
			if (cb_isWifiConnect_IsWifiConnect_Z == null)
			{
				cb_isWifiConnect_IsWifiConnect_Z = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_Z(n_IsWifiConnect));
			}
			return cb_isWifiConnect_IsWifiConnect_Z;
		}

		private static bool n_IsWifiConnect(nint jnienv, nint native__this)
		{
			return Object.GetObject<NetworkUtil>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).IsWifiConnect;
		}

		private static global::System.Delegate GetGetLocationManagerHandler()
		{
			if (cb_getLocationManager_GetLocationManager_Landroid_location_LocationManager_ == null)
			{
				cb_getLocationManager_GetLocationManager_Landroid_location_LocationManager_ = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_L(n_GetLocationManager));
			}
			return cb_getLocationManager_GetLocationManager_Landroid_location_LocationManager_;
		}

		private static nint n_GetLocationManager(nint jnienv, nint native__this)
		{
			return JNIEnv.ToLocalJniHandle((IJavaObject)(object)Object.GetObject<NetworkUtil>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).LocationManager);
		}

		private static global::System.Delegate GetGetSsidHandler()
		{
			if (cb_getSsid_GetSsid_Ljava_lang_String_ == null)
			{
				cb_getSsid_GetSsid_Ljava_lang_String_ = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_L(n_GetSsid));
			}
			return cb_getSsid_GetSsid_Ljava_lang_String_;
		}

		private static nint n_GetSsid(nint jnienv, nint native__this)
		{
			return JNIEnv.NewString(Object.GetObject<NetworkUtil>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).Ssid);
		}

		private static global::System.Delegate GetGetWifiManagerHandler()
		{
			if (cb_getWifiManager_GetWifiManager_Landroid_net_wifi_WifiManager_ == null)
			{
				cb_getWifiManager_GetWifiManager_Landroid_net_wifi_WifiManager_ = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_L(n_GetWifiManager));
			}
			return cb_getWifiManager_GetWifiManager_Landroid_net_wifi_WifiManager_;
		}

		private static nint n_GetWifiManager(nint jnienv, nint native__this)
		{
			return JNIEnv.ToLocalJniHandle((IJavaObject)(object)Object.GetObject<NetworkUtil>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).WifiManager);
		}

		private static global::System.Delegate GetIsNetworkConnected_Ljava_lang_String_Handler()
		{
			if (cb_isNetworkConnected_IsNetworkConnected_Ljava_lang_String__Z == null)
			{
				cb_isNetworkConnected_IsNetworkConnected_Ljava_lang_String__Z = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPL_Z(n_IsNetworkConnected_Ljava_lang_String_));
			}
			return cb_isNetworkConnected_IsNetworkConnected_Ljava_lang_String__Z;
		}

		private static bool n_IsNetworkConnected_Ljava_lang_String_(nint jnienv, nint native__this, nint native_ssid)
		{
			NetworkUtil networkUtil = Object.GetObject<NetworkUtil>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			string ssid = JNIEnv.GetString((global::System.IntPtr)native_ssid, (JniHandleOwnership)0);
			return networkUtil.IsNetworkConnected(ssid);
		}

		[Register("isNetworkConnected", "(Ljava/lang/String;)Z", "GetIsNetworkConnected_Ljava_lang_String_Handler")]
		public unsafe virtual bool IsNetworkConnected(string? ssid)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			nint num = JNIEnv.NewString(ssid);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((global::System.IntPtr)num));
				return _members.InstanceMethods.InvokeVirtualBooleanMethod("isNetworkConnected.(Ljava/lang/String;)Z", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
			}
		}

		private static global::System.Delegate GetRegisterNetwork_Landroid_net_NetworkRequest_Landroid_net_ConnectivityManager_NetworkCallback_Handler()
		{
			if (cb_registerNetwork_RegisterNetwork_Landroid_net_NetworkRequest_Landroid_net_ConnectivityManager_NetworkCallback__V == null)
			{
				cb_registerNetwork_RegisterNetwork_Landroid_net_NetworkRequest_Landroid_net_ConnectivityManager_NetworkCallback__V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPLL_V(n_RegisterNetwork_Landroid_net_NetworkRequest_Landroid_net_ConnectivityManager_NetworkCallback_));
			}
			return cb_registerNetwork_RegisterNetwork_Landroid_net_NetworkRequest_Landroid_net_ConnectivityManager_NetworkCallback__V;
		}

		private static void n_RegisterNetwork_Landroid_net_NetworkRequest_Landroid_net_ConnectivityManager_NetworkCallback_(nint jnienv, nint native__this, nint native_request, nint native_networkCallback)
		{
			NetworkUtil networkUtil = Object.GetObject<NetworkUtil>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			NetworkRequest request = Object.GetObject<NetworkRequest>((global::System.IntPtr)native_request, (JniHandleOwnership)0);
			NetworkCallback networkCallback = Object.GetObject<NetworkCallback>((global::System.IntPtr)native_networkCallback, (JniHandleOwnership)0);
			networkUtil.RegisterNetwork(request, networkCallback);
		}

		[Register("registerNetwork", "(Landroid/net/NetworkRequest;Landroid/net/ConnectivityManager$NetworkCallback;)V", "GetRegisterNetwork_Landroid_net_NetworkRequest_Landroid_net_ConnectivityManager_NetworkCallback_Handler")]
		public unsafe virtual void RegisterNetwork(NetworkRequest? request, NetworkCallback? networkCallback)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((request == null) ? global::System.IntPtr.Zero : ((Object)request).Handle));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue((networkCallback == null) ? global::System.IntPtr.Zero : ((Object)networkCallback).Handle));
				_members.InstanceMethods.InvokeVirtualVoidMethod("registerNetwork.(Landroid/net/NetworkRequest;Landroid/net/ConnectivityManager$NetworkCallback;)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)request);
				GC.KeepAlive((object)networkCallback);
			}
		}

		private static global::System.Delegate GetUnregisterNetworkHandler()
		{
			if (cb_unregisterNetwork_UnregisterNetwork_V == null)
			{
				cb_unregisterNetwork_UnregisterNetwork_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_UnregisterNetwork));
			}
			return cb_unregisterNetwork_UnregisterNetwork_V;
		}

		private static void n_UnregisterNetwork(nint jnienv, nint native__this)
		{
			Object.GetObject<NetworkUtil>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).UnregisterNetwork();
		}

		[Register("unregisterNetwork", "()V", "GetUnregisterNetworkHandler")]
		public unsafe virtual void UnregisterNetwork()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("unregisterNetwork.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}
	}
	[Register("com/module/UDP", DoNotGenerateAcw = true)]
	public class UDP : Object
	{
		[Register("com/module/UDP$OnUdpReceiveListener", "", "Com.Module.UDP/IOnUdpReceiveListenerInvoker")]
		public interface IOnUdpReceiveListener : IJavaObject, global::System.IDisposable, IJavaPeerable
		{
			[Register("onEvent", "(I)V", "GetOnEvent_IHandler:Com.Module.UDP/IOnUdpReceiveListenerInvoker, ids.camera.insight.binding")]
			void OnEvent(int p0);

			[Register("onReceive", "([B)V", "GetOnReceive_arrayBHandler:Com.Module.UDP/IOnUdpReceiveListenerInvoker, ids.camera.insight.binding")]
			void OnReceive(byte[]? p0);
		}

		[Register("com/module/UDP$OnUdpReceiveListener", DoNotGenerateAcw = true)]
		internal class IOnUdpReceiveListenerInvoker : Object, IOnUdpReceiveListener, IJavaObject, global::System.IDisposable, IJavaPeerable
		{
			private static readonly JniPeerMembers _members_com_module_UDP_OnUdpReceiveListener = (JniPeerMembers)new XAPeerMembers("com/module/UDP$OnUdpReceiveListener", typeof(IOnUdpReceiveListenerInvoker));

			private static global::System.Delegate? cb_onEvent_OnEvent_I_V;

			private static global::System.Delegate? cb_onReceive_OnReceive_arrayB_V;

			private static nint java_class_ref
			{
				get
				{
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					JniObjectReference peerReference = _members_com_module_UDP_OnUdpReceiveListener.JniPeerType.PeerReference;
					return ((JniObjectReference)(ref peerReference)).Handle;
				}
			}

			[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
			[EditorBrowsable(/*Could not decode attribute arguments.*/)]
			public override JniPeerMembers JniPeerMembers => _members_com_module_UDP_OnUdpReceiveListener;

			[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
			[EditorBrowsable(/*Could not decode attribute arguments.*/)]
			protected override nint ThresholdClass
			{
				get
				{
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					JniObjectReference peerReference = _members_com_module_UDP_OnUdpReceiveListener.JniPeerType.PeerReference;
					return ((JniObjectReference)(ref peerReference)).Handle;
				}
			}

			[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
			[EditorBrowsable(/*Could not decode attribute arguments.*/)]
			protected override global::System.Type ThresholdType => _members_com_module_UDP_OnUdpReceiveListener.ManagedPeerType;

			public IOnUdpReceiveListenerInvoker(nint handle, JniHandleOwnership transfer)
				: base((global::System.IntPtr)handle, transfer)
			{
			}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


			private static global::System.Delegate GetOnEvent_IHandler()
			{
				if (cb_onEvent_OnEvent_I_V == null)
				{
					cb_onEvent_OnEvent_I_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPI_V(n_OnEvent_I));
				}
				return cb_onEvent_OnEvent_I_V;
			}

			private static void n_OnEvent_I(nint jnienv, nint native__this, int p0)
			{
				Object.GetObject<IOnUdpReceiveListener>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).OnEvent(p0);
			}

			public unsafe void OnEvent(int p0)
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(p0));
				_members_com_module_UDP_OnUdpReceiveListener.InstanceMethods.InvokeAbstractVoidMethod("onEvent.(I)V", (IJavaPeerable)(object)this, ptr);
			}

			private static global::System.Delegate GetOnReceive_arrayBHandler()
			{
				if (cb_onReceive_OnReceive_arrayB_V == null)
				{
					cb_onReceive_OnReceive_arrayB_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPL_V(n_OnReceive_arrayB));
				}
				return cb_onReceive_OnReceive_arrayB_V;
			}

			private static void n_OnReceive_arrayB(nint jnienv, nint native__this, nint native_p0)
			{
				IOnUdpReceiveListener onUdpReceiveListener = Object.GetObject<IOnUdpReceiveListener>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
				byte[] array = (byte[])JNIEnv.GetArray((global::System.IntPtr)native_p0, (JniHandleOwnership)0, typeof(byte));
				onUdpReceiveListener.OnReceive(array);
				if (array != null)
				{
					JNIEnv.CopyArray(array, (global::System.IntPtr)native_p0);
				}
			}

			public unsafe void OnReceive(byte[]? p0)
			{
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				nint num = JNIEnv.NewArray(p0);
				try
				{
					JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
					global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((global::System.IntPtr)num));
					_members_com_module_UDP_OnUdpReceiveListener.InstanceMethods.InvokeAbstractVoidMethod("onReceive.([B)V", (IJavaPeerable)(object)this, ptr);
				}
				finally
				{
					if (p0 != null)
					{
						JNIEnv.CopyArray((global::System.IntPtr)num, p0);
						JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
					}
					GC.KeepAlive((object)p0);
				}
			}
		}

		public class EventEventArgs : EventArgs
		{
			private int p0;

			public int P0 => p0;

			public EventEventArgs(int p0)
			{
				this.p0 = p0;
			}
		}

		public class ReceiveEventArgs : EventArgs
		{
			private byte[]? p0;

			public byte[]? P0 => p0;

			public ReceiveEventArgs(byte[]? p0)
			{
				this.p0 = p0;
			}
		}

		[Register("mono/com/module/UDP_OnUdpReceiveListenerImplementor")]
		internal sealed class IOnUdpReceiveListenerImplementor : Object, IOnUdpReceiveListener, IJavaObject, global::System.IDisposable, IJavaPeerable
		{
			private object? sender;

			public EventHandler<EventEventArgs>? OnEventHandler;

			public EventHandler<ReceiveEventArgs>? OnReceiveHandler;

			public unsafe IOnUdpReceiveListenerImplementor(object sender)
				: base(global::System.IntPtr.Zero, (JniHandleOwnership)0)
			{
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				if (((Object)this).Handle == global::System.IntPtr.Zero)
				{
					JniObjectReference val = ((Object)this).JniPeerMembers.InstanceMethods.StartCreateInstance("()V", ((object)this).GetType(), (JniArgumentValue*)null);
					((Object)this).SetHandle(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
					((Object)this).JniPeerMembers.InstanceMethods.FinishCreateInstance("()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
					this.sender = sender;
				}
			}

			public void OnEvent(int p0)
			{
				OnEventHandler?.Invoke(sender, new EventEventArgs(p0));
			}

			public void OnReceive(byte[]? p0)
			{
				OnReceiveHandler?.Invoke(sender, new ReceiveEventArgs(p0));
			}

			internal static bool __IsEmpty(IOnUdpReceiveListenerImplementor value)
			{
				if (value.OnEventHandler == null)
				{
					return value.OnReceiveHandler == null;
				}
				return false;
			}
		}

		[Register("EVENT_SOCKE_TIMEOUT")]
		public const int EventSockeTimeout = 2;

		[Register("EVENT_STOP_RECEIVE")]
		public const int EventStopReceive = 1;

		private static readonly JniPeerMembers _members = (JniPeerMembers)new XAPeerMembers("com/module/UDP", typeof(UDP));

		private static global::System.Delegate? cb_isConnect_IsConnect_Z;

		private static global::System.Delegate? cb_getPort_GetPort_I;

		private static global::System.Delegate? cb_close_Close_V;

		private static global::System.Delegate? cb_create_Create_Z;

		private static global::System.Delegate? cb_create_Create_I_Z;

		private static global::System.Delegate? cb_send_Send_Ljava_lang_String_IarrayB_V;

		private static global::System.Delegate? cb_sendSync_SendSync_Ljava_lang_String_IarrayB_arrayB;

		private static global::System.Delegate? cb_setSocket_SetSocket_Ljava_net_DatagramSocket__V;

		private static global::System.Delegate? cb_setSocketTimeout_SetSocketTimeout_I_V;

		private static global::System.Delegate? cb_setUDPListener_SetUDPListener_Lcom_module_UDP_OnUdpReceiveListener__V;

		private static global::System.Delegate? cb_startReceive_StartReceive_V;

		private static global::System.Delegate? cb_stopReceive_StopReceive_V;

		private WeakReference? weak_implementor_SetUDPListener;

		[Register("isRunning")]
		public bool IsRunning
		{
			get
			{
				return _members.InstanceFields.GetBooleanValue("isRunning.Z", (IJavaPeerable)(object)this);
			}
			set
			{
				_members.InstanceFields.SetValue("isRunning.Z", (IJavaPeerable)(object)this, value);
			}
		}

		[Register("SOCKET_TIMEOUT")]
		public int SocketTimeout
		{
			get
			{
				return _members.InstanceFields.GetInt32Value("SOCKET_TIMEOUT.I", (IJavaPeerable)(object)this);
			}
			set
			{
				_members.InstanceFields.SetValue("SOCKET_TIMEOUT.I", (IJavaPeerable)(object)this, value);
			}
		}

		internal static nint class_ref
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		public override JniPeerMembers JniPeerMembers => _members;

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override nint ThresholdClass
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override global::System.Type ThresholdType => _members.ManagedPeerType;

		public unsafe virtual bool IsConnect
		{
			[Register("isConnect", "()Z", "GetIsConnectHandler")]
			get
			{
				return _members.InstanceMethods.InvokeVirtualBooleanMethod("isConnect.()Z", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			}
		}

		public unsafe virtual int Port
		{
			[Register("getPort", "()I", "GetGetPortHandler")]
			get
			{
				return _members.InstanceMethods.InvokeVirtualInt32Method("getPort.()I", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			}
		}

		public unsafe static UDP? TmpUDP
		{
			[Register("getTmpUDP", "()Lcom/module/UDP;", "")]
			get
			{
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference val = _members.StaticMethods.InvokeObjectMethod("getTmpUDP.()Lcom/module/UDP;", (JniArgumentValue*)null);
				return Object.GetObject<UDP>(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
			}
			[Register("setTmpUDP", "(Lcom/module/UDP;)V", "")]
			set
			{
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				try
				{
					JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
					global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((value == null) ? global::System.IntPtr.Zero : ((Object)value).Handle));
					_members.StaticMethods.InvokeVoidMethod("setTmpUDP.(Lcom/module/UDP;)V", ptr);
				}
				finally
				{
					GC.KeepAlive((object)value);
				}
			}
		}

		public event EventHandler<EventEventArgs> Event
		{
			add
			{
				EventHelper.AddEventHandler<IOnUdpReceiveListener, IOnUdpReceiveListenerImplementor>(ref weak_implementor_SetUDPListener, (Func<IOnUdpReceiveListenerImplementor>)__CreateIOnUdpReceiveListenerImplementor, (Action<IOnUdpReceiveListener>)SetUDPListener, (Action<IOnUdpReceiveListenerImplementor>)delegate(IOnUdpReceiveListenerImplementor __h)
				{
					__h.OnEventHandler = (EventHandler<EventEventArgs>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)__h.OnEventHandler, (global::System.Delegate)(object)value);
				});
			}
			remove
			{
				EventHelper.RemoveEventHandler<IOnUdpReceiveListener, IOnUdpReceiveListenerImplementor>(ref weak_implementor_SetUDPListener, (Func<IOnUdpReceiveListenerImplementor, bool>)IOnUdpReceiveListenerImplementor.__IsEmpty, (Action<IOnUdpReceiveListener>)delegate
				{
					SetUDPListener(null);
				}, (Action<IOnUdpReceiveListenerImplementor>)delegate(IOnUdpReceiveListenerImplementor __h)
				{
					__h.OnEventHandler = (EventHandler<EventEventArgs>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)__h.OnEventHandler, (global::System.Delegate)(object)value);
				});
			}
		}

		public event EventHandler<ReceiveEventArgs> Receive
		{
			add
			{
				EventHelper.AddEventHandler<IOnUdpReceiveListener, IOnUdpReceiveListenerImplementor>(ref weak_implementor_SetUDPListener, (Func<IOnUdpReceiveListenerImplementor>)__CreateIOnUdpReceiveListenerImplementor, (Action<IOnUdpReceiveListener>)SetUDPListener, (Action<IOnUdpReceiveListenerImplementor>)delegate(IOnUdpReceiveListenerImplementor __h)
				{
					__h.OnReceiveHandler = (EventHandler<ReceiveEventArgs>)(object)global::System.Delegate.Combine((global::System.Delegate)(object)__h.OnReceiveHandler, (global::System.Delegate)(object)value);
				});
			}
			remove
			{
				EventHelper.RemoveEventHandler<IOnUdpReceiveListener, IOnUdpReceiveListenerImplementor>(ref weak_implementor_SetUDPListener, (Func<IOnUdpReceiveListenerImplementor, bool>)IOnUdpReceiveListenerImplementor.__IsEmpty, (Action<IOnUdpReceiveListener>)delegate
				{
					SetUDPListener(null);
				}, (Action<IOnUdpReceiveListenerImplementor>)delegate(IOnUdpReceiveListenerImplementor __h)
				{
					__h.OnReceiveHandler = (EventHandler<ReceiveEventArgs>)(object)global::System.Delegate.Remove((global::System.Delegate)(object)__h.OnReceiveHandler, (global::System.Delegate)(object)value);
				});
			}
		}

		protected UDP(nint javaReference, JniHandleOwnership transfer)
			: base((global::System.IntPtr)javaReference, transfer)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		[Register(".ctor", "(Landroid/content/Context;)V", "")]
		public unsafe UDP(Context? cnt)
			: base(global::System.IntPtr.Zero, (JniHandleOwnership)0)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			if (((Object)this).Handle != global::System.IntPtr.Zero)
			{
				return;
			}
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((cnt == null) ? global::System.IntPtr.Zero : ((Object)cnt).Handle));
				JniObjectReference val = _members.InstanceMethods.StartCreateInstance("(Landroid/content/Context;)V", ((object)this).GetType(), ptr);
				((Object)this).SetHandle(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
				_members.InstanceMethods.FinishCreateInstance("(Landroid/content/Context;)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)cnt);
			}
		}

		[Register(".ctor", "(Landroid/content/Context;I)V", "")]
		public unsafe UDP(Context? cnt, int timeout)
			: base(global::System.IntPtr.Zero, (JniHandleOwnership)0)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			if (((Object)this).Handle != global::System.IntPtr.Zero)
			{
				return;
			}
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((cnt == null) ? global::System.IntPtr.Zero : ((Object)cnt).Handle));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(timeout));
				JniObjectReference val = _members.InstanceMethods.StartCreateInstance("(Landroid/content/Context;I)V", ((object)this).GetType(), ptr);
				((Object)this).SetHandle(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
				_members.InstanceMethods.FinishCreateInstance("(Landroid/content/Context;I)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)cnt);
			}
		}

		private static global::System.Delegate GetIsConnectHandler()
		{
			if (cb_isConnect_IsConnect_Z == null)
			{
				cb_isConnect_IsConnect_Z = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_Z(n_IsConnect));
			}
			return cb_isConnect_IsConnect_Z;
		}

		private static bool n_IsConnect(nint jnienv, nint native__this)
		{
			return Object.GetObject<UDP>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).IsConnect;
		}

		private static global::System.Delegate GetGetPortHandler()
		{
			if (cb_getPort_GetPort_I == null)
			{
				cb_getPort_GetPort_I = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_I(n_GetPort));
			}
			return cb_getPort_GetPort_I;
		}

		private static int n_GetPort(nint jnienv, nint native__this)
		{
			return Object.GetObject<UDP>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).Port;
		}

		private static global::System.Delegate GetCloseHandler()
		{
			if (cb_close_Close_V == null)
			{
				cb_close_Close_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_Close));
			}
			return cb_close_Close_V;
		}

		private static void n_Close(nint jnienv, nint native__this)
		{
			Object.GetObject<UDP>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).Close();
		}

		[Register("close", "()V", "GetCloseHandler")]
		public unsafe virtual void Close()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("close.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetCreateHandler()
		{
			if (cb_create_Create_Z == null)
			{
				cb_create_Create_Z = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_Z(n_Create));
			}
			return cb_create_Create_Z;
		}

		private static bool n_Create(nint jnienv, nint native__this)
		{
			return Object.GetObject<UDP>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).Create();
		}

		[Register("create", "()Z", "GetCreateHandler")]
		public unsafe virtual bool Create()
		{
			return _members.InstanceMethods.InvokeVirtualBooleanMethod("create.()Z", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetCreate_IHandler()
		{
			if (cb_create_Create_I_Z == null)
			{
				cb_create_Create_I_Z = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPI_Z(n_Create_I));
			}
			return cb_create_Create_I_Z;
		}

		private static bool n_Create_I(nint jnienv, nint native__this, int port)
		{
			return Object.GetObject<UDP>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).Create(port);
		}

		[Register("create", "(I)Z", "GetCreate_IHandler")]
		public unsafe virtual bool Create(int port)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
			global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(port));
			return _members.InstanceMethods.InvokeVirtualBooleanMethod("create.(I)Z", (IJavaPeerable)(object)this, ptr);
		}

		private static global::System.Delegate GetSend_Ljava_lang_String_IarrayBHandler()
		{
			if (cb_send_Send_Ljava_lang_String_IarrayB_V == null)
			{
				cb_send_Send_Ljava_lang_String_IarrayB_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPLIL_V(n_Send_Ljava_lang_String_IarrayB));
			}
			return cb_send_Send_Ljava_lang_String_IarrayB_V;
		}

		private static void n_Send_Ljava_lang_String_IarrayB(nint jnienv, nint native__this, nint native_ip, int port, nint native_msg)
		{
			UDP uDP = Object.GetObject<UDP>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			string ip = JNIEnv.GetString((global::System.IntPtr)native_ip, (JniHandleOwnership)0);
			byte[] array = (byte[])JNIEnv.GetArray((global::System.IntPtr)native_msg, (JniHandleOwnership)0, typeof(byte));
			uDP.Send(ip, port, array);
			if (array != null)
			{
				JNIEnv.CopyArray(array, (global::System.IntPtr)native_msg);
			}
		}

		[Register("send", "(Ljava/lang/String;I[B)V", "GetSend_Ljava_lang_String_IarrayBHandler")]
		public unsafe virtual void Send(string? ip, int port, byte[]? msg)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			nint num = JNIEnv.NewString(ip);
			nint num2 = JNIEnv.NewArray(msg);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[3];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((global::System.IntPtr)num));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(port));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + (nint)2 * (nint)global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue((global::System.IntPtr)num2));
				_members.InstanceMethods.InvokeVirtualVoidMethod("send.(Ljava/lang/String;I[B)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				if (msg != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num2, msg);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num2);
				}
				GC.KeepAlive((object)msg);
			}
		}

		private static global::System.Delegate GetSendSync_Ljava_lang_String_IarrayBHandler()
		{
			if (cb_sendSync_SendSync_Ljava_lang_String_IarrayB_arrayB == null)
			{
				cb_sendSync_SendSync_Ljava_lang_String_IarrayB_arrayB = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPLIL_L(n_SendSync_Ljava_lang_String_IarrayB));
			}
			return cb_sendSync_SendSync_Ljava_lang_String_IarrayB_arrayB;
		}

		private static nint n_SendSync_Ljava_lang_String_IarrayB(nint jnienv, nint native__this, nint native_ip, int port, nint native_msg)
		{
			UDP uDP = Object.GetObject<UDP>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			string ip = JNIEnv.GetString((global::System.IntPtr)native_ip, (JniHandleOwnership)0);
			byte[] array = (byte[])JNIEnv.GetArray((global::System.IntPtr)native_msg, (JniHandleOwnership)0, typeof(byte));
			global::System.IntPtr result = JNIEnv.NewArray(uDP.SendSync(ip, port, array));
			if (array != null)
			{
				JNIEnv.CopyArray(array, (global::System.IntPtr)native_msg);
			}
			return result;
		}

		[Register("sendSync", "(Ljava/lang/String;I[B)[B", "GetSendSync_Ljava_lang_String_IarrayBHandler")]
		public unsafe virtual byte[]? SendSync(string? ip, int port, byte[]? msg)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			nint num = JNIEnv.NewString(ip);
			nint num2 = JNIEnv.NewArray(msg);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[3];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((global::System.IntPtr)num));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(port));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + (nint)2 * (nint)global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue((global::System.IntPtr)num2));
				JniObjectReference val = _members.InstanceMethods.InvokeVirtualObjectMethod("sendSync.(Ljava/lang/String;I[B)[B", (IJavaPeerable)(object)this, ptr);
				return (byte[])JNIEnv.GetArray(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1, typeof(byte));
			}
			finally
			{
				JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				if (msg != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num2, msg);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num2);
				}
				GC.KeepAlive((object)msg);
			}
		}

		private static global::System.Delegate GetSetSocket_Ljava_net_DatagramSocket_Handler()
		{
			if (cb_setSocket_SetSocket_Ljava_net_DatagramSocket__V == null)
			{
				cb_setSocket_SetSocket_Ljava_net_DatagramSocket__V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPL_V(n_SetSocket_Ljava_net_DatagramSocket_));
			}
			return cb_setSocket_SetSocket_Ljava_net_DatagramSocket__V;
		}

		private static void n_SetSocket_Ljava_net_DatagramSocket_(nint jnienv, nint native__this, nint native_socket)
		{
			UDP uDP = Object.GetObject<UDP>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			DatagramSocket socket = Object.GetObject<DatagramSocket>((global::System.IntPtr)native_socket, (JniHandleOwnership)0);
			uDP.SetSocket(socket);
		}

		[Register("setSocket", "(Ljava/net/DatagramSocket;)V", "GetSetSocket_Ljava_net_DatagramSocket_Handler")]
		public unsafe virtual void SetSocket(DatagramSocket? socket)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((socket == null) ? global::System.IntPtr.Zero : ((Object)socket).Handle));
				_members.InstanceMethods.InvokeVirtualVoidMethod("setSocket.(Ljava/net/DatagramSocket;)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)socket);
			}
		}

		private static global::System.Delegate GetSetSocketTimeout_IHandler()
		{
			if (cb_setSocketTimeout_SetSocketTimeout_I_V == null)
			{
				cb_setSocketTimeout_SetSocketTimeout_I_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPI_V(n_SetSocketTimeout_I));
			}
			return cb_setSocketTimeout_SetSocketTimeout_I_V;
		}

		private static void n_SetSocketTimeout_I(nint jnienv, nint native__this, int timeoutInMillisecond)
		{
			Object.GetObject<UDP>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).SetSocketTimeout(timeoutInMillisecond);
		}

		[Register("setSocketTimeout", "(I)V", "GetSetSocketTimeout_IHandler")]
		public unsafe virtual void SetSocketTimeout(int timeoutInMillisecond)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
			global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(timeoutInMillisecond));
			_members.InstanceMethods.InvokeVirtualVoidMethod("setSocketTimeout.(I)V", (IJavaPeerable)(object)this, ptr);
		}

		private static global::System.Delegate GetSetUDPListener_Lcom_module_UDP_OnUdpReceiveListener_Handler()
		{
			if (cb_setUDPListener_SetUDPListener_Lcom_module_UDP_OnUdpReceiveListener__V == null)
			{
				cb_setUDPListener_SetUDPListener_Lcom_module_UDP_OnUdpReceiveListener__V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPL_V(n_SetUDPListener_Lcom_module_UDP_OnUdpReceiveListener_));
			}
			return cb_setUDPListener_SetUDPListener_Lcom_module_UDP_OnUdpReceiveListener__V;
		}

		private static void n_SetUDPListener_Lcom_module_UDP_OnUdpReceiveListener_(nint jnienv, nint native__this, nint native_listener)
		{
			UDP uDP = Object.GetObject<UDP>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			IOnUdpReceiveListener uDPListener = Object.GetObject<IOnUdpReceiveListener>((global::System.IntPtr)native_listener, (JniHandleOwnership)0);
			uDP.SetUDPListener(uDPListener);
		}

		[Register("setUDPListener", "(Lcom/module/UDP$OnUdpReceiveListener;)V", "GetSetUDPListener_Lcom_module_UDP_OnUdpReceiveListener_Handler")]
		public unsafe virtual void SetUDPListener(IOnUdpReceiveListener? listener)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((listener == null) ? global::System.IntPtr.Zero : ((Object)listener).Handle));
				_members.InstanceMethods.InvokeVirtualVoidMethod("setUDPListener.(Lcom/module/UDP$OnUdpReceiveListener;)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)listener);
			}
		}

		private static global::System.Delegate GetStartReceiveHandler()
		{
			if (cb_startReceive_StartReceive_V == null)
			{
				cb_startReceive_StartReceive_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_StartReceive));
			}
			return cb_startReceive_StartReceive_V;
		}

		private static void n_StartReceive(nint jnienv, nint native__this)
		{
			Object.GetObject<UDP>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).StartReceive();
		}

		[Register("startReceive", "()V", "GetStartReceiveHandler")]
		public unsafe virtual void StartReceive()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("startReceive.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetStopReceiveHandler()
		{
			if (cb_stopReceive_StopReceive_V == null)
			{
				cb_stopReceive_StopReceive_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_StopReceive));
			}
			return cb_stopReceive_StopReceive_V;
		}

		private static void n_StopReceive(nint jnienv, nint native__this)
		{
			Object.GetObject<UDP>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).StopReceive();
		}

		[Register("stopReceive", "()V", "GetStopReceiveHandler")]
		public unsafe virtual void StopReceive()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("stopReceive.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private IOnUdpReceiveListenerImplementor __CreateIOnUdpReceiveListenerImplementor()
		{
			return new IOnUdpReceiveListenerImplementor(this);
		}
	}
}
namespace Com.Hardware
{
	[Register("com/Hardware/Decoder", DoNotGenerateAcw = true)]
	public class Decoder : Object
	{
		[Register("com/Hardware/Decoder$Notify", "", "Com.Hardware.Decoder/INotifyInvoker")]
		public interface INotify : IJavaObject, global::System.IDisposable, IJavaPeerable
		{
			[Register("onNotify", "(I)V", "GetOnNotify_IHandler:Com.Hardware.Decoder/INotifyInvoker, ids.camera.insight.binding")]
			void OnNotify(int p0);
		}

		[Register("com/Hardware/Decoder$Notify", DoNotGenerateAcw = true)]
		internal class INotifyInvoker : Object, INotify, IJavaObject, global::System.IDisposable, IJavaPeerable
		{
			private static readonly JniPeerMembers _members_com_Hardware_Decoder_Notify = (JniPeerMembers)new XAPeerMembers("com/Hardware/Decoder$Notify", typeof(INotifyInvoker));

			private static global::System.Delegate? cb_onNotify_OnNotify_I_V;

			private static nint java_class_ref
			{
				get
				{
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					JniObjectReference peerReference = _members_com_Hardware_Decoder_Notify.JniPeerType.PeerReference;
					return ((JniObjectReference)(ref peerReference)).Handle;
				}
			}

			[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
			[EditorBrowsable(/*Could not decode attribute arguments.*/)]
			public override JniPeerMembers JniPeerMembers => _members_com_Hardware_Decoder_Notify;

			[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
			[EditorBrowsable(/*Could not decode attribute arguments.*/)]
			protected override nint ThresholdClass
			{
				get
				{
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					JniObjectReference peerReference = _members_com_Hardware_Decoder_Notify.JniPeerType.PeerReference;
					return ((JniObjectReference)(ref peerReference)).Handle;
				}
			}

			[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
			[EditorBrowsable(/*Could not decode attribute arguments.*/)]
			protected override global::System.Type ThresholdType => _members_com_Hardware_Decoder_Notify.ManagedPeerType;

			public INotifyInvoker(nint handle, JniHandleOwnership transfer)
				: base((global::System.IntPtr)handle, transfer)
			{
			}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


			private static global::System.Delegate GetOnNotify_IHandler()
			{
				if (cb_onNotify_OnNotify_I_V == null)
				{
					cb_onNotify_OnNotify_I_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPI_V(n_OnNotify_I));
				}
				return cb_onNotify_OnNotify_I_V;
			}

			private static void n_OnNotify_I(nint jnienv, nint native__this, int p0)
			{
				Object.GetObject<INotify>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).OnNotify(p0);
			}

			public unsafe void OnNotify(int p0)
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(p0));
				_members_com_Hardware_Decoder_Notify.InstanceMethods.InvokeAbstractVoidMethod("onNotify.(I)V", (IJavaPeerable)(object)this, ptr);
			}
		}

		[Register("DECODE_DEQUEUE_INPUTBUFFER_ERROR")]
		public const int DecodeDequeueInputbufferError = -100;

		private static readonly JniPeerMembers _members = (JniPeerMembers)new XAPeerMembers("com/Hardware/Decoder", typeof(Decoder));

		private static global::System.Delegate? cb_getFPS_GetFPS_I;

		private static global::System.Delegate? cb_decode_Decode_arrayB_V;

		private static global::System.Delegate? cb_decodeAudio_DecodeAudio_arrayB_V;

		private static global::System.Delegate? cb_initial_Initial_IIIarrayBarrayB_Z;

		private static global::System.Delegate? cb_releaseDecoder_ReleaseDecoder_V;

		private static global::System.Delegate? cb_runAudioThread_RunAudioThread_I_V;

		private static global::System.Delegate? cb_setAudioData_SetAudioData_arrayB_V;

		private static global::System.Delegate? cb_stopRunning_StopRunning_Z;

		[Register("SPS_HD")]
		public static global::System.Collections.Generic.IList<byte>? SpsHd
		{
			get
			{
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference objectValue = _members.StaticFields.GetObjectValue("SPS_HD.[B");
				return (global::System.Collections.Generic.IList<byte>?)JavaArray<byte>.FromJniHandle(((JniObjectReference)(ref objectValue)).Handle, (JniHandleOwnership)1);
			}
		}

		internal static nint class_ref
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		public override JniPeerMembers JniPeerMembers => _members;

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override nint ThresholdClass
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override global::System.Type ThresholdType => _members.ManagedPeerType;

		public unsafe virtual int FPS
		{
			[Register("getFPS", "()I", "GetGetFPSHandler")]
			get
			{
				return _members.InstanceMethods.InvokeVirtualInt32Method("getFPS.()I", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			}
		}

		protected Decoder(nint javaReference, JniHandleOwnership transfer)
			: base((global::System.IntPtr)javaReference, transfer)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		[Register(".ctor", "(Landroid/view/Surface;ILcom/Hardware/Decoder$Notify;)V", "")]
		public unsafe Decoder(Surface? surface, int playerState, INotify? notify)
			: base(global::System.IntPtr.Zero, (JniHandleOwnership)0)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			if (((Object)this).Handle != global::System.IntPtr.Zero)
			{
				return;
			}
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[3];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((surface == null) ? global::System.IntPtr.Zero : ((Object)surface).Handle));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(playerState));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + (nint)2 * (nint)global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue((notify == null) ? global::System.IntPtr.Zero : ((Object)notify).Handle));
				JniObjectReference val = _members.InstanceMethods.StartCreateInstance("(Landroid/view/Surface;ILcom/Hardware/Decoder$Notify;)V", ((object)this).GetType(), ptr);
				((Object)this).SetHandle(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
				_members.InstanceMethods.FinishCreateInstance("(Landroid/view/Surface;ILcom/Hardware/Decoder$Notify;)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)surface);
				GC.KeepAlive((object)notify);
			}
		}

		private static global::System.Delegate GetGetFPSHandler()
		{
			if (cb_getFPS_GetFPS_I == null)
			{
				cb_getFPS_GetFPS_I = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_I(n_GetFPS));
			}
			return cb_getFPS_GetFPS_I;
		}

		private static int n_GetFPS(nint jnienv, nint native__this)
		{
			return Object.GetObject<Decoder>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).FPS;
		}

		private static global::System.Delegate GetDecode_arrayBHandler()
		{
			if (cb_decode_Decode_arrayB_V == null)
			{
				cb_decode_Decode_arrayB_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPL_V(n_Decode_arrayB));
			}
			return cb_decode_Decode_arrayB_V;
		}

		private static void n_Decode_arrayB(nint jnienv, nint native__this, nint native_data)
		{
			Decoder decoder = Object.GetObject<Decoder>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			byte[] array = (byte[])JNIEnv.GetArray((global::System.IntPtr)native_data, (JniHandleOwnership)0, typeof(byte));
			decoder.Decode(array);
			if (array != null)
			{
				JNIEnv.CopyArray(array, (global::System.IntPtr)native_data);
			}
		}

		[Register("decode", "([B)V", "GetDecode_arrayBHandler")]
		public unsafe virtual void Decode(byte[]? data)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			nint num = JNIEnv.NewArray(data);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((global::System.IntPtr)num));
				_members.InstanceMethods.InvokeVirtualVoidMethod("decode.([B)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				if (data != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num, data);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				}
				GC.KeepAlive((object)data);
			}
		}

		private static global::System.Delegate GetDecodeAudio_arrayBHandler()
		{
			if (cb_decodeAudio_DecodeAudio_arrayB_V == null)
			{
				cb_decodeAudio_DecodeAudio_arrayB_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPL_V(n_DecodeAudio_arrayB));
			}
			return cb_decodeAudio_DecodeAudio_arrayB_V;
		}

		private static void n_DecodeAudio_arrayB(nint jnienv, nint native__this, nint native_data)
		{
			Decoder decoder = Object.GetObject<Decoder>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			byte[] array = (byte[])JNIEnv.GetArray((global::System.IntPtr)native_data, (JniHandleOwnership)0, typeof(byte));
			decoder.DecodeAudio(array);
			if (array != null)
			{
				JNIEnv.CopyArray(array, (global::System.IntPtr)native_data);
			}
		}

		[Register("decodeAudio", "([B)V", "GetDecodeAudio_arrayBHandler")]
		public unsafe virtual void DecodeAudio(byte[]? data)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			nint num = JNIEnv.NewArray(data);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((global::System.IntPtr)num));
				_members.InstanceMethods.InvokeVirtualVoidMethod("decodeAudio.([B)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				if (data != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num, data);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				}
				GC.KeepAlive((object)data);
			}
		}

		private static global::System.Delegate GetInitial_IIIarrayBarrayBHandler()
		{
			if (cb_initial_Initial_IIIarrayBarrayB_Z == null)
			{
				cb_initial_Initial_IIIarrayBarrayB_Z = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPIIILL_Z(n_Initial_IIIarrayBarrayB));
			}
			return cb_initial_Initial_IIIarrayBarrayB_Z;
		}

		private static bool n_Initial_IIIarrayBarrayB(nint jnienv, nint native__this, int width, int height, int frameRate, nint native_header_sps, nint native_header_pps)
		{
			Decoder decoder = Object.GetObject<Decoder>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			byte[] array = (byte[])JNIEnv.GetArray((global::System.IntPtr)native_header_sps, (JniHandleOwnership)0, typeof(byte));
			byte[] array2 = (byte[])JNIEnv.GetArray((global::System.IntPtr)native_header_pps, (JniHandleOwnership)0, typeof(byte));
			bool result = decoder.Initial(width, height, frameRate, array, array2);
			if (array != null)
			{
				JNIEnv.CopyArray(array, (global::System.IntPtr)native_header_sps);
			}
			if (array2 != null)
			{
				JNIEnv.CopyArray(array2, (global::System.IntPtr)native_header_pps);
			}
			return result;
		}

		[Register("initial", "(III[B[B)Z", "GetInitial_IIIarrayBarrayBHandler")]
		public unsafe virtual bool Initial(int width, int height, int frameRate, byte[]? header_sps, byte[]? header_pps)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			nint num = JNIEnv.NewArray(header_sps);
			nint num2 = JNIEnv.NewArray(header_pps);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[5];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(width));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(height));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + (nint)2 * (nint)global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(frameRate));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + (nint)3 * (nint)global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue((global::System.IntPtr)num));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + (nint)4 * (nint)global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue((global::System.IntPtr)num2));
				return _members.InstanceMethods.InvokeVirtualBooleanMethod("initial.(III[B[B)Z", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				if (header_sps != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num, header_sps);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				}
				if (header_pps != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num2, header_pps);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num2);
				}
				GC.KeepAlive((object)header_sps);
				GC.KeepAlive((object)header_pps);
			}
		}

		private static global::System.Delegate GetReleaseDecoderHandler()
		{
			if (cb_releaseDecoder_ReleaseDecoder_V == null)
			{
				cb_releaseDecoder_ReleaseDecoder_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_ReleaseDecoder));
			}
			return cb_releaseDecoder_ReleaseDecoder_V;
		}

		private static void n_ReleaseDecoder(nint jnienv, nint native__this)
		{
			Object.GetObject<Decoder>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).ReleaseDecoder();
		}

		[Register("releaseDecoder", "()V", "GetReleaseDecoderHandler")]
		public unsafe virtual void ReleaseDecoder()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("releaseDecoder.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetRunAudioThread_IHandler()
		{
			if (cb_runAudioThread_RunAudioThread_I_V == null)
			{
				cb_runAudioThread_RunAudioThread_I_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPI_V(n_RunAudioThread_I));
			}
			return cb_runAudioThread_RunAudioThread_I_V;
		}

		private static void n_RunAudioThread_I(nint jnienv, nint native__this, int i)
		{
			Object.GetObject<Decoder>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).RunAudioThread(i);
		}

		[Register("runAudioThread", "(I)V", "GetRunAudioThread_IHandler")]
		public unsafe virtual void RunAudioThread(int i)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
			global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(i));
			_members.InstanceMethods.InvokeVirtualVoidMethod("runAudioThread.(I)V", (IJavaPeerable)(object)this, ptr);
		}

		private static global::System.Delegate GetSetAudioData_arrayBHandler()
		{
			if (cb_setAudioData_SetAudioData_arrayB_V == null)
			{
				cb_setAudioData_SetAudioData_arrayB_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPL_V(n_SetAudioData_arrayB));
			}
			return cb_setAudioData_SetAudioData_arrayB_V;
		}

		private static void n_SetAudioData_arrayB(nint jnienv, nint native__this, nint native_data)
		{
			Decoder decoder = Object.GetObject<Decoder>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			byte[] array = (byte[])JNIEnv.GetArray((global::System.IntPtr)native_data, (JniHandleOwnership)0, typeof(byte));
			decoder.SetAudioData(array);
			if (array != null)
			{
				JNIEnv.CopyArray(array, (global::System.IntPtr)native_data);
			}
		}

		[Register("setAudioData", "([B)V", "GetSetAudioData_arrayBHandler")]
		public unsafe virtual void SetAudioData(byte[]? data)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			nint num = JNIEnv.NewArray(data);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((global::System.IntPtr)num));
				_members.InstanceMethods.InvokeVirtualVoidMethod("setAudioData.([B)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				if (data != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num, data);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				}
				GC.KeepAlive((object)data);
			}
		}

		private static global::System.Delegate GetStopRunningHandler()
		{
			if (cb_stopRunning_StopRunning_Z == null)
			{
				cb_stopRunning_StopRunning_Z = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_Z(n_StopRunning));
			}
			return cb_stopRunning_StopRunning_Z;
		}

		private static bool n_StopRunning(nint jnienv, nint native__this)
		{
			return Object.GetObject<Decoder>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).StopRunning();
		}

		[Register("stopRunning", "()Z", "GetStopRunningHandler")]
		public unsafe virtual bool StopRunning()
		{
			return _members.InstanceMethods.InvokeVirtualBooleanMethod("stopRunning.()Z", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}
	}
}
namespace Com.Android.VideoCodec
{
	[Register("com/android/VideoCodec/H264HeaderParser", DoNotGenerateAcw = true)]
	public class H264HeaderParser : Object
	{
		private static readonly JniPeerMembers _members = (JniPeerMembers)new XAPeerMembers("com/android/VideoCodec/H264HeaderParser", typeof(H264HeaderParser));

		private static global::System.Delegate? cb_getHeight_GetHeight_I;

		private static global::System.Delegate? cb_getWidth_GetWidth_I;

		private static global::System.Delegate? cb_parse_Parse_arrayB_V;

		internal static nint class_ref
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		public override JniPeerMembers JniPeerMembers => _members;

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override nint ThresholdClass
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override global::System.Type ThresholdType => _members.ManagedPeerType;

		public unsafe virtual int Height
		{
			[Register("getHeight", "()I", "GetGetHeightHandler")]
			get
			{
				return _members.InstanceMethods.InvokeVirtualInt32Method("getHeight.()I", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			}
		}

		public unsafe virtual int Width
		{
			[Register("getWidth", "()I", "GetGetWidthHandler")]
			get
			{
				return _members.InstanceMethods.InvokeVirtualInt32Method("getWidth.()I", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			}
		}

		protected H264HeaderParser(nint javaReference, JniHandleOwnership transfer)
			: base((global::System.IntPtr)javaReference, transfer)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		[Register(".ctor", "()V", "")]
		public unsafe H264HeaderParser()
			: base(global::System.IntPtr.Zero, (JniHandleOwnership)0)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			if (((Object)this).Handle == global::System.IntPtr.Zero)
			{
				JniObjectReference val = _members.InstanceMethods.StartCreateInstance("()V", ((object)this).GetType(), (JniArgumentValue*)null);
				((Object)this).SetHandle(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
				_members.InstanceMethods.FinishCreateInstance("()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			}
		}

		private static global::System.Delegate GetGetHeightHandler()
		{
			if (cb_getHeight_GetHeight_I == null)
			{
				cb_getHeight_GetHeight_I = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_I(n_GetHeight));
			}
			return cb_getHeight_GetHeight_I;
		}

		private static int n_GetHeight(nint jnienv, nint native__this)
		{
			return Object.GetObject<H264HeaderParser>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).Height;
		}

		private static global::System.Delegate GetGetWidthHandler()
		{
			if (cb_getWidth_GetWidth_I == null)
			{
				cb_getWidth_GetWidth_I = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_I(n_GetWidth));
			}
			return cb_getWidth_GetWidth_I;
		}

		private static int n_GetWidth(nint jnienv, nint native__this)
		{
			return Object.GetObject<H264HeaderParser>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).Width;
		}

		private static global::System.Delegate GetParse_arrayBHandler()
		{
			if (cb_parse_Parse_arrayB_V == null)
			{
				cb_parse_Parse_arrayB_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPL_V(n_Parse_arrayB));
			}
			return cb_parse_Parse_arrayB_V;
		}

		private static void n_Parse_arrayB(nint jnienv, nint native__this, nint native_sps)
		{
			H264HeaderParser h264HeaderParser = Object.GetObject<H264HeaderParser>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			byte[] array = (byte[])JNIEnv.GetArray((global::System.IntPtr)native_sps, (JniHandleOwnership)0, typeof(byte));
			h264HeaderParser.Parse(array);
			if (array != null)
			{
				JNIEnv.CopyArray(array, (global::System.IntPtr)native_sps);
			}
		}

		[Register("parse", "([B)V", "GetParse_arrayBHandler")]
		public unsafe virtual void Parse(byte[]? sps)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			nint num = JNIEnv.NewArray(sps);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((global::System.IntPtr)num));
				_members.InstanceMethods.InvokeVirtualVoidMethod("parse.([B)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				if (sps != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num, sps);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				}
				GC.KeepAlive((object)sps);
			}
		}
	}
	[Register("com/android/VideoCodec/MyGLRenderer", DoNotGenerateAcw = true)]
	public class MyGLRenderer : Object, IRenderer, IJavaObject, global::System.IDisposable, IJavaPeerable
	{
		[Register("com/android/VideoCodec/MyGLRenderer$Callback", "", "Com.Android.VideoCodec.MyGLRenderer/ICallbackInvoker")]
		public interface ICallback : IJavaObject, global::System.IDisposable, IJavaPeerable
		{
			[Register("onSurfaceCreated", "(Landroid/graphics/SurfaceTexture;)V", "GetOnSurfaceCreated_Landroid_graphics_SurfaceTexture_Handler:Com.Android.VideoCodec.MyGLRenderer/ICallbackInvoker, ids.camera.insight.binding")]
			void OnSurfaceCreated(SurfaceTexture? p0);
		}

		[Register("com/android/VideoCodec/MyGLRenderer$Callback", DoNotGenerateAcw = true)]
		internal class ICallbackInvoker : Object, ICallback, IJavaObject, global::System.IDisposable, IJavaPeerable
		{
			private static readonly JniPeerMembers _members_com_android_VideoCodec_MyGLRenderer_Callback = (JniPeerMembers)new XAPeerMembers("com/android/VideoCodec/MyGLRenderer$Callback", typeof(ICallbackInvoker));

			private static global::System.Delegate? cb_onSurfaceCreated_OnSurfaceCreated_Landroid_graphics_SurfaceTexture__V;

			private static nint java_class_ref
			{
				get
				{
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					JniObjectReference peerReference = _members_com_android_VideoCodec_MyGLRenderer_Callback.JniPeerType.PeerReference;
					return ((JniObjectReference)(ref peerReference)).Handle;
				}
			}

			[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
			[EditorBrowsable(/*Could not decode attribute arguments.*/)]
			public override JniPeerMembers JniPeerMembers => _members_com_android_VideoCodec_MyGLRenderer_Callback;

			[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
			[EditorBrowsable(/*Could not decode attribute arguments.*/)]
			protected override nint ThresholdClass
			{
				get
				{
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					JniObjectReference peerReference = _members_com_android_VideoCodec_MyGLRenderer_Callback.JniPeerType.PeerReference;
					return ((JniObjectReference)(ref peerReference)).Handle;
				}
			}

			[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
			[EditorBrowsable(/*Could not decode attribute arguments.*/)]
			protected override global::System.Type ThresholdType => _members_com_android_VideoCodec_MyGLRenderer_Callback.ManagedPeerType;

			public ICallbackInvoker(nint handle, JniHandleOwnership transfer)
				: base((global::System.IntPtr)handle, transfer)
			{
			}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


			private static global::System.Delegate GetOnSurfaceCreated_Landroid_graphics_SurfaceTexture_Handler()
			{
				if (cb_onSurfaceCreated_OnSurfaceCreated_Landroid_graphics_SurfaceTexture__V == null)
				{
					cb_onSurfaceCreated_OnSurfaceCreated_Landroid_graphics_SurfaceTexture__V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPL_V(n_OnSurfaceCreated_Landroid_graphics_SurfaceTexture_));
				}
				return cb_onSurfaceCreated_OnSurfaceCreated_Landroid_graphics_SurfaceTexture__V;
			}

			private static void n_OnSurfaceCreated_Landroid_graphics_SurfaceTexture_(nint jnienv, nint native__this, nint native_p0)
			{
				ICallback callback = Object.GetObject<ICallback>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
				SurfaceTexture p = Object.GetObject<SurfaceTexture>((global::System.IntPtr)native_p0, (JniHandleOwnership)0);
				callback.OnSurfaceCreated(p);
			}

			public unsafe void OnSurfaceCreated(SurfaceTexture? p0)
			{
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				try
				{
					JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
					global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((p0 == null) ? global::System.IntPtr.Zero : ((Object)p0).Handle));
					_members_com_android_VideoCodec_MyGLRenderer_Callback.InstanceMethods.InvokeAbstractVoidMethod("onSurfaceCreated.(Landroid/graphics/SurfaceTexture;)V", (IJavaPeerable)(object)this, ptr);
				}
				finally
				{
					GC.KeepAlive((object)p0);
				}
			}
		}

		private static readonly JniPeerMembers _members = (JniPeerMembers)new XAPeerMembers("com/android/VideoCodec/MyGLRenderer", typeof(MyGLRenderer));

		private static global::System.Delegate? cb_onDrawFrame_OnDrawFrame_Ljavax_microedition_khronos_opengles_GL10__V;

		private static global::System.Delegate? cb_onSurfaceChanged_OnSurfaceChanged_Ljavax_microedition_khronos_opengles_GL10_II_V;

		private static global::System.Delegate? cb_onSurfaceCreated_OnSurfaceCreated_Ljavax_microedition_khronos_opengles_GL10_Ljavax_microedition_khronos_egl_EGLConfig__V;

		private static global::System.Delegate? cb_setCallback_SetCallback_Lcom_android_VideoCodec_MyGLRenderer_Callback__V;

		private static global::System.Delegate? cb_setSWDecoder_SetSWDecoder_Lcom_android_VideoCodec_SWDecoder__V;

		private static global::System.Delegate? cb_setShaderSource_SetShaderSource_Ljava_lang_String_Ljava_lang_String__V;

		internal static nint class_ref
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		public override JniPeerMembers JniPeerMembers => _members;

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override nint ThresholdClass
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override global::System.Type ThresholdType => _members.ManagedPeerType;

		protected MyGLRenderer(nint javaReference, JniHandleOwnership transfer)
			: base((global::System.IntPtr)javaReference, transfer)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		[Register(".ctor", "(Landroid/content/Context;)V", "")]
		public unsafe MyGLRenderer(Context? context)
			: base(global::System.IntPtr.Zero, (JniHandleOwnership)0)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			if (((Object)this).Handle != global::System.IntPtr.Zero)
			{
				return;
			}
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((context == null) ? global::System.IntPtr.Zero : ((Object)context).Handle));
				JniObjectReference val = _members.InstanceMethods.StartCreateInstance("(Landroid/content/Context;)V", ((object)this).GetType(), ptr);
				((Object)this).SetHandle(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
				_members.InstanceMethods.FinishCreateInstance("(Landroid/content/Context;)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)context);
			}
		}

		private static global::System.Delegate GetOnDrawFrame_Ljavax_microedition_khronos_opengles_GL10_Handler()
		{
			if (cb_onDrawFrame_OnDrawFrame_Ljavax_microedition_khronos_opengles_GL10__V == null)
			{
				cb_onDrawFrame_OnDrawFrame_Ljavax_microedition_khronos_opengles_GL10__V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPL_V(n_OnDrawFrame_Ljavax_microedition_khronos_opengles_GL10_));
			}
			return cb_onDrawFrame_OnDrawFrame_Ljavax_microedition_khronos_opengles_GL10__V;
		}

		private static void n_OnDrawFrame_Ljavax_microedition_khronos_opengles_GL10_(nint jnienv, nint native__this, nint native_gl)
		{
			MyGLRenderer myGLRenderer = Object.GetObject<MyGLRenderer>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			IGL10 gl = Object.GetObject<IGL10>((global::System.IntPtr)native_gl, (JniHandleOwnership)0);
			myGLRenderer.OnDrawFrame(gl);
		}

		[Register("onDrawFrame", "(Ljavax/microedition/khronos/opengles/GL10;)V", "GetOnDrawFrame_Ljavax_microedition_khronos_opengles_GL10_Handler")]
		public unsafe virtual void OnDrawFrame(IGL10? gl)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((gl == null) ? global::System.IntPtr.Zero : ((Object)gl).Handle));
				_members.InstanceMethods.InvokeVirtualVoidMethod("onDrawFrame.(Ljavax/microedition/khronos/opengles/GL10;)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)gl);
			}
		}

		private static global::System.Delegate GetOnSurfaceChanged_Ljavax_microedition_khronos_opengles_GL10_IIHandler()
		{
			if (cb_onSurfaceChanged_OnSurfaceChanged_Ljavax_microedition_khronos_opengles_GL10_II_V == null)
			{
				cb_onSurfaceChanged_OnSurfaceChanged_Ljavax_microedition_khronos_opengles_GL10_II_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPLII_V(n_OnSurfaceChanged_Ljavax_microedition_khronos_opengles_GL10_II));
			}
			return cb_onSurfaceChanged_OnSurfaceChanged_Ljavax_microedition_khronos_opengles_GL10_II_V;
		}

		private static void n_OnSurfaceChanged_Ljavax_microedition_khronos_opengles_GL10_II(nint jnienv, nint native__this, nint native_gl, int width, int height)
		{
			MyGLRenderer myGLRenderer = Object.GetObject<MyGLRenderer>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			IGL10 gl = Object.GetObject<IGL10>((global::System.IntPtr)native_gl, (JniHandleOwnership)0);
			myGLRenderer.OnSurfaceChanged(gl, width, height);
		}

		[Register("onSurfaceChanged", "(Ljavax/microedition/khronos/opengles/GL10;II)V", "GetOnSurfaceChanged_Ljavax_microedition_khronos_opengles_GL10_IIHandler")]
		public unsafe virtual void OnSurfaceChanged(IGL10? gl, int width, int height)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[3];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((gl == null) ? global::System.IntPtr.Zero : ((Object)gl).Handle));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(width));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + (nint)2 * (nint)global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(height));
				_members.InstanceMethods.InvokeVirtualVoidMethod("onSurfaceChanged.(Ljavax/microedition/khronos/opengles/GL10;II)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)gl);
			}
		}

		private static global::System.Delegate GetOnSurfaceCreated_Ljavax_microedition_khronos_opengles_GL10_Ljavax_microedition_khronos_egl_EGLConfig_Handler()
		{
			if (cb_onSurfaceCreated_OnSurfaceCreated_Ljavax_microedition_khronos_opengles_GL10_Ljavax_microedition_khronos_egl_EGLConfig__V == null)
			{
				cb_onSurfaceCreated_OnSurfaceCreated_Ljavax_microedition_khronos_opengles_GL10_Ljavax_microedition_khronos_egl_EGLConfig__V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPLL_V(n_OnSurfaceCreated_Ljavax_microedition_khronos_opengles_GL10_Ljavax_microedition_khronos_egl_EGLConfig_));
			}
			return cb_onSurfaceCreated_OnSurfaceCreated_Ljavax_microedition_khronos_opengles_GL10_Ljavax_microedition_khronos_egl_EGLConfig__V;
		}

		private static void n_OnSurfaceCreated_Ljavax_microedition_khronos_opengles_GL10_Ljavax_microedition_khronos_egl_EGLConfig_(nint jnienv, nint native__this, nint native_gl, nint native_config)
		{
			MyGLRenderer myGLRenderer = Object.GetObject<MyGLRenderer>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			IGL10 gl = Object.GetObject<IGL10>((global::System.IntPtr)native_gl, (JniHandleOwnership)0);
			EGLConfig config = Object.GetObject<EGLConfig>((global::System.IntPtr)native_config, (JniHandleOwnership)0);
			myGLRenderer.OnSurfaceCreated(gl, config);
		}

		[Register("onSurfaceCreated", "(Ljavax/microedition/khronos/opengles/GL10;Ljavax/microedition/khronos/egl/EGLConfig;)V", "GetOnSurfaceCreated_Ljavax_microedition_khronos_opengles_GL10_Ljavax_microedition_khronos_egl_EGLConfig_Handler")]
		public unsafe virtual void OnSurfaceCreated(IGL10? gl, EGLConfig? config)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((gl == null) ? global::System.IntPtr.Zero : ((Object)gl).Handle));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue((config == null) ? global::System.IntPtr.Zero : ((Object)config).Handle));
				_members.InstanceMethods.InvokeVirtualVoidMethod("onSurfaceCreated.(Ljavax/microedition/khronos/opengles/GL10;Ljavax/microedition/khronos/egl/EGLConfig;)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)gl);
				GC.KeepAlive((object)config);
			}
		}

		[Register("readRawTextFile", "(Landroid/content/Context;I)Ljava/lang/String;", "")]
		public unsafe static string? ReadRawTextFile(Context? context, int resId)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((context == null) ? global::System.IntPtr.Zero : ((Object)context).Handle));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(resId));
				JniObjectReference val = _members.StaticMethods.InvokeObjectMethod("readRawTextFile.(Landroid/content/Context;I)Ljava/lang/String;", ptr);
				return JNIEnv.GetString(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
			}
			finally
			{
				GC.KeepAlive((object)context);
			}
		}

		private static global::System.Delegate GetSetCallback_Lcom_android_VideoCodec_MyGLRenderer_Callback_Handler()
		{
			if (cb_setCallback_SetCallback_Lcom_android_VideoCodec_MyGLRenderer_Callback__V == null)
			{
				cb_setCallback_SetCallback_Lcom_android_VideoCodec_MyGLRenderer_Callback__V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPL_V(n_SetCallback_Lcom_android_VideoCodec_MyGLRenderer_Callback_));
			}
			return cb_setCallback_SetCallback_Lcom_android_VideoCodec_MyGLRenderer_Callback__V;
		}

		private static void n_SetCallback_Lcom_android_VideoCodec_MyGLRenderer_Callback_(nint jnienv, nint native__this, nint native__callback)
		{
			MyGLRenderer myGLRenderer = Object.GetObject<MyGLRenderer>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			ICallback callback = Object.GetObject<ICallback>((global::System.IntPtr)native__callback, (JniHandleOwnership)0);
			myGLRenderer.SetCallback(callback);
		}

		[Register("setCallback", "(Lcom/android/VideoCodec/MyGLRenderer$Callback;)V", "GetSetCallback_Lcom_android_VideoCodec_MyGLRenderer_Callback_Handler")]
		public unsafe virtual void SetCallback(ICallback? callback)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((callback == null) ? global::System.IntPtr.Zero : ((Object)callback).Handle));
				_members.InstanceMethods.InvokeVirtualVoidMethod("setCallback.(Lcom/android/VideoCodec/MyGLRenderer$Callback;)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)callback);
			}
		}

		private static global::System.Delegate GetSetSWDecoder_Lcom_android_VideoCodec_SWDecoder_Handler()
		{
			if (cb_setSWDecoder_SetSWDecoder_Lcom_android_VideoCodec_SWDecoder__V == null)
			{
				cb_setSWDecoder_SetSWDecoder_Lcom_android_VideoCodec_SWDecoder__V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPL_V(n_SetSWDecoder_Lcom_android_VideoCodec_SWDecoder_));
			}
			return cb_setSWDecoder_SetSWDecoder_Lcom_android_VideoCodec_SWDecoder__V;
		}

		private static void n_SetSWDecoder_Lcom_android_VideoCodec_SWDecoder_(nint jnienv, nint native__this, nint native_swDecoder)
		{
			MyGLRenderer myGLRenderer = Object.GetObject<MyGLRenderer>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			SWDecoder sWDecoder = Object.GetObject<SWDecoder>((global::System.IntPtr)native_swDecoder, (JniHandleOwnership)0);
			myGLRenderer.SetSWDecoder(sWDecoder);
		}

		[Register("setSWDecoder", "(Lcom/android/VideoCodec/SWDecoder;)V", "GetSetSWDecoder_Lcom_android_VideoCodec_SWDecoder_Handler")]
		public unsafe virtual void SetSWDecoder(SWDecoder? swDecoder)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((swDecoder == null) ? global::System.IntPtr.Zero : ((Object)swDecoder).Handle));
				_members.InstanceMethods.InvokeVirtualVoidMethod("setSWDecoder.(Lcom/android/VideoCodec/SWDecoder;)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)swDecoder);
			}
		}

		private static global::System.Delegate GetSetShaderSource_Ljava_lang_String_Ljava_lang_String_Handler()
		{
			if (cb_setShaderSource_SetShaderSource_Ljava_lang_String_Ljava_lang_String__V == null)
			{
				cb_setShaderSource_SetShaderSource_Ljava_lang_String_Ljava_lang_String__V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPLL_V(n_SetShaderSource_Ljava_lang_String_Ljava_lang_String_));
			}
			return cb_setShaderSource_SetShaderSource_Ljava_lang_String_Ljava_lang_String__V;
		}

		private static void n_SetShaderSource_Ljava_lang_String_Ljava_lang_String_(nint jnienv, nint native__this, nint native_vs, nint native_fs)
		{
			MyGLRenderer myGLRenderer = Object.GetObject<MyGLRenderer>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			string vs = JNIEnv.GetString((global::System.IntPtr)native_vs, (JniHandleOwnership)0);
			string fs = JNIEnv.GetString((global::System.IntPtr)native_fs, (JniHandleOwnership)0);
			myGLRenderer.SetShaderSource(vs, fs);
		}

		[Register("setShaderSource", "(Ljava/lang/String;Ljava/lang/String;)V", "GetSetShaderSource_Ljava_lang_String_Ljava_lang_String_Handler")]
		public unsafe virtual void SetShaderSource(string? vs, string? fs)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			nint num = JNIEnv.NewString(vs);
			nint num2 = JNIEnv.NewString(fs);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((global::System.IntPtr)num));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue((global::System.IntPtr)num2));
				_members.InstanceMethods.InvokeVirtualVoidMethod("setShaderSource.(Ljava/lang/String;Ljava/lang/String;)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				JNIEnv.DeleteLocalRef((global::System.IntPtr)num2);
			}
		}
	}
	[Register("com/android/VideoCodec/SWDecoder", DoNotGenerateAcw = true)]
	public class SWDecoder : Object
	{
		private static readonly JniPeerMembers _members = (JniPeerMembers)new XAPeerMembers("com/android/VideoCodec/SWDecoder", typeof(SWDecoder));

		private static global::System.Delegate? cb_getAddr_GetAddr_J;

		private static global::System.Delegate? cb_getDecodeNumber_GetDecodeNumber_I;

		private static global::System.Delegate? cb_isInit_IsInit_Z;

		private static global::System.Delegate? cb_dealloc_Dealloc_V;

		private static global::System.Delegate? cb_decode_Decode_arrayBI_V;

		private static global::System.Delegate? cb_init_Init_V;

		private static global::System.Delegate? cb_releaseGLES_ReleaseGLES_V;

		internal static nint class_ref
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		public override JniPeerMembers JniPeerMembers => _members;

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override nint ThresholdClass
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override global::System.Type ThresholdType => _members.ManagedPeerType;

		public unsafe virtual long Addr
		{
			[Register("getAddr", "()J", "GetGetAddrHandler")]
			get
			{
				return _members.InstanceMethods.InvokeVirtualInt64Method("getAddr.()J", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			}
		}

		public unsafe virtual int DecodeNumber
		{
			[Register("getDecodeNumber", "()I", "GetGetDecodeNumberHandler")]
			get
			{
				return _members.InstanceMethods.InvokeVirtualInt32Method("getDecodeNumber.()I", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			}
		}

		public unsafe virtual bool IsInit
		{
			[Register("isInit", "()Z", "GetIsInitHandler")]
			get
			{
				return _members.InstanceMethods.InvokeVirtualBooleanMethod("isInit.()Z", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			}
		}

		protected SWDecoder(nint javaReference, JniHandleOwnership transfer)
			: base((global::System.IntPtr)javaReference, transfer)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		[Register(".ctor", "(I)V", "")]
		public unsafe SWDecoder(int decoderNUm)
			: base(global::System.IntPtr.Zero, (JniHandleOwnership)0)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			if (((Object)this).Handle == global::System.IntPtr.Zero)
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(decoderNUm));
				JniObjectReference val = _members.InstanceMethods.StartCreateInstance("(I)V", ((object)this).GetType(), ptr);
				((Object)this).SetHandle(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
				_members.InstanceMethods.FinishCreateInstance("(I)V", (IJavaPeerable)(object)this, ptr);
			}
		}

		private static global::System.Delegate GetGetAddrHandler()
		{
			if (cb_getAddr_GetAddr_J == null)
			{
				cb_getAddr_GetAddr_J = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_J(n_GetAddr));
			}
			return cb_getAddr_GetAddr_J;
		}

		private static long n_GetAddr(nint jnienv, nint native__this)
		{
			return Object.GetObject<SWDecoder>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).Addr;
		}

		private static global::System.Delegate GetGetDecodeNumberHandler()
		{
			if (cb_getDecodeNumber_GetDecodeNumber_I == null)
			{
				cb_getDecodeNumber_GetDecodeNumber_I = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_I(n_GetDecodeNumber));
			}
			return cb_getDecodeNumber_GetDecodeNumber_I;
		}

		private static int n_GetDecodeNumber(nint jnienv, nint native__this)
		{
			return Object.GetObject<SWDecoder>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).DecodeNumber;
		}

		private static global::System.Delegate GetIsInitHandler()
		{
			if (cb_isInit_IsInit_Z == null)
			{
				cb_isInit_IsInit_Z = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_Z(n_IsInit));
			}
			return cb_isInit_IsInit_Z;
		}

		private static bool n_IsInit(nint jnienv, nint native__this)
		{
			return Object.GetObject<SWDecoder>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).IsInit;
		}

		private static global::System.Delegate GetDeallocHandler()
		{
			if (cb_dealloc_Dealloc_V == null)
			{
				cb_dealloc_Dealloc_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_Dealloc));
			}
			return cb_dealloc_Dealloc_V;
		}

		private static void n_Dealloc(nint jnienv, nint native__this)
		{
			Object.GetObject<SWDecoder>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).Dealloc();
		}

		[Register("dealloc", "()V", "GetDeallocHandler")]
		public unsafe virtual void Dealloc()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("dealloc.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetDecode_arrayBIHandler()
		{
			if (cb_decode_Decode_arrayBI_V == null)
			{
				cb_decode_Decode_arrayBI_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPLI_V(n_Decode_arrayBI));
			}
			return cb_decode_Decode_arrayBI_V;
		}

		private static void n_Decode_arrayBI(nint jnienv, nint native__this, nint native_buf, int size)
		{
			SWDecoder sWDecoder = Object.GetObject<SWDecoder>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			byte[] array = (byte[])JNIEnv.GetArray((global::System.IntPtr)native_buf, (JniHandleOwnership)0, typeof(byte));
			sWDecoder.Decode(array, size);
			if (array != null)
			{
				JNIEnv.CopyArray(array, (global::System.IntPtr)native_buf);
			}
		}

		[Register("decode", "([BI)V", "GetDecode_arrayBIHandler")]
		public unsafe virtual void Decode(byte[]? buf, int size)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			nint num = JNIEnv.NewArray(buf);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((global::System.IntPtr)num));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(size));
				_members.InstanceMethods.InvokeVirtualVoidMethod("decode.([BI)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				if (buf != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num, buf);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				}
				GC.KeepAlive((object)buf);
			}
		}

		private static global::System.Delegate GetInitHandler()
		{
			if (cb_init_Init_V == null)
			{
				cb_init_Init_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_Init));
			}
			return cb_init_Init_V;
		}

		private static void n_Init(nint jnienv, nint native__this)
		{
			Object.GetObject<SWDecoder>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).Init();
		}

		[Register("init", "()V", "GetInitHandler")]
		public unsafe virtual void Init()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("init.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetReleaseGLESHandler()
		{
			if (cb_releaseGLES_ReleaseGLES_V == null)
			{
				cb_releaseGLES_ReleaseGLES_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_ReleaseGLES));
			}
			return cb_releaseGLES_ReleaseGLES_V;
		}

		private static void n_ReleaseGLES(nint jnienv, nint native__this)
		{
			Object.GetObject<SWDecoder>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).ReleaseGLES();
		}

		[Register("releaseGLES", "()V", "GetReleaseGLESHandler")]
		public unsafe virtual void ReleaseGLES()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("releaseGLES.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}
	}
	[Register("com/android/VideoCodec/VideoDecoder", DoNotGenerateAcw = true)]
	public class VideoDecoder : Object
	{
		[Register("com/android/VideoCodec/VideoDecoder$OnNotifyCallback", "", "Com.Android.VideoCodec.VideoDecoder/IOnNotifyCallbackInvoker")]
		public interface IOnNotifyCallback : IJavaObject, global::System.IDisposable, IJavaPeerable
		{
			[Register("onNotify", "(Ljava/lang/String;I)V", "GetOnNotify_Ljava_lang_String_IHandler:Com.Android.VideoCodec.VideoDecoder/IOnNotifyCallbackInvoker, ids.camera.insight.binding")]
			void OnNotify(string? p0, int p1);
		}

		[Register("com/android/VideoCodec/VideoDecoder$OnNotifyCallback", DoNotGenerateAcw = true)]
		internal class IOnNotifyCallbackInvoker : Object, IOnNotifyCallback, IJavaObject, global::System.IDisposable, IJavaPeerable
		{
			private static readonly JniPeerMembers _members_com_android_VideoCodec_VideoDecoder_OnNotifyCallback = (JniPeerMembers)new XAPeerMembers("com/android/VideoCodec/VideoDecoder$OnNotifyCallback", typeof(IOnNotifyCallbackInvoker));

			private static global::System.Delegate? cb_onNotify_OnNotify_Ljava_lang_String_I_V;

			private static nint java_class_ref
			{
				get
				{
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					JniObjectReference peerReference = _members_com_android_VideoCodec_VideoDecoder_OnNotifyCallback.JniPeerType.PeerReference;
					return ((JniObjectReference)(ref peerReference)).Handle;
				}
			}

			[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
			[EditorBrowsable(/*Could not decode attribute arguments.*/)]
			public override JniPeerMembers JniPeerMembers => _members_com_android_VideoCodec_VideoDecoder_OnNotifyCallback;

			[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
			[EditorBrowsable(/*Could not decode attribute arguments.*/)]
			protected override nint ThresholdClass
			{
				get
				{
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					JniObjectReference peerReference = _members_com_android_VideoCodec_VideoDecoder_OnNotifyCallback.JniPeerType.PeerReference;
					return ((JniObjectReference)(ref peerReference)).Handle;
				}
			}

			[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
			[EditorBrowsable(/*Could not decode attribute arguments.*/)]
			protected override global::System.Type ThresholdType => _members_com_android_VideoCodec_VideoDecoder_OnNotifyCallback.ManagedPeerType;

			public IOnNotifyCallbackInvoker(nint handle, JniHandleOwnership transfer)
				: base((global::System.IntPtr)handle, transfer)
			{
			}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


			private static global::System.Delegate GetOnNotify_Ljava_lang_String_IHandler()
			{
				if (cb_onNotify_OnNotify_Ljava_lang_String_I_V == null)
				{
					cb_onNotify_OnNotify_Ljava_lang_String_I_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPLI_V(n_OnNotify_Ljava_lang_String_I));
				}
				return cb_onNotify_OnNotify_Ljava_lang_String_I_V;
			}

			private static void n_OnNotify_Ljava_lang_String_I(nint jnienv, nint native__this, nint native_p0, int p1)
			{
				IOnNotifyCallback onNotifyCallback = Object.GetObject<IOnNotifyCallback>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
				string p2 = JNIEnv.GetString((global::System.IntPtr)native_p0, (JniHandleOwnership)0);
				onNotifyCallback.OnNotify(p2, p1);
			}

			public unsafe void OnNotify(string? p0, int p1)
			{
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				nint num = JNIEnv.NewString(p0);
				try
				{
					JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
					global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((global::System.IntPtr)num));
					global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(p1));
					_members_com_android_VideoCodec_VideoDecoder_OnNotifyCallback.InstanceMethods.InvokeAbstractVoidMethod("onNotify.(Ljava/lang/String;I)V", (IJavaPeerable)(object)this, ptr);
				}
				finally
				{
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				}
			}
		}

		[Register("JNI_NOTIFY_CURRENT_RESOLUTION")]
		public const int JniNotifyCurrentResolution = 10;

		[Register("JNI_NOTIFY_DECODE_ERROR")]
		public const int JniNotifyDecodeError = 17;

		[Register("JNI_NOTIFY_GL_REQUEST_RENDER")]
		public const int JniNotifyGlRequestRender = 16;

		private static readonly JniPeerMembers _members = (JniPeerMembers)new XAPeerMembers("com/android/VideoCodec/VideoDecoder", typeof(VideoDecoder));

		internal static nint class_ref
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		public override JniPeerMembers JniPeerMembers => _members;

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override nint ThresholdClass
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override global::System.Type ThresholdType => _members.ManagedPeerType;

		public unsafe static int TextureId
		{
			[Register("getTextureId", "()I", "")]
			get
			{
				return _members.StaticMethods.InvokeInt32Method("getTextureId.()I", (JniArgumentValue*)null);
			}
		}

		protected VideoDecoder(nint javaReference, JniHandleOwnership transfer)
			: base((global::System.IntPtr)javaReference, transfer)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		[Register(".ctor", "()V", "")]
		public unsafe VideoDecoder()
			: base(global::System.IntPtr.Zero, (JniHandleOwnership)0)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			if (((Object)this).Handle == global::System.IntPtr.Zero)
			{
				JniObjectReference val = _members.InstanceMethods.StartCreateInstance("()V", ((object)this).GetType(), (JniArgumentValue*)null);
				((Object)this).SetHandle(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
				_members.InstanceMethods.FinishCreateInstance("()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			}
		}

		[Register("codecDeAlloc2", "(J)V", "")]
		public unsafe static void CodecDeAlloc2(long p0)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
			global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(p0));
			_members.StaticMethods.InvokeVoidMethod("codecDeAlloc2.(J)V", ptr);
		}

		[Register("decode", "([BIJ)V", "")]
		public unsafe static void Decode(byte[]? p0, int p1, long p2)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			nint num = JNIEnv.NewArray(p0);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[3];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((global::System.IntPtr)num));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(p1));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + (nint)2 * (nint)global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(p2));
				_members.StaticMethods.InvokeVoidMethod("decode.([BIJ)V", ptr);
			}
			finally
			{
				if (p0 != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num, p0);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				}
				GC.KeepAlive((object)p0);
			}
		}

		[Register("getDecoderAddr", "(I)J", "")]
		public unsafe static long GetDecoderAddr(int p0)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
			global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(p0));
			return _members.StaticMethods.InvokeInt64Method("getDecoderAddr.(I)J", ptr);
		}

		[Register("gldrawFrame", "(J)V", "")]
		public unsafe static void GldrawFrame(long p0)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
			global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(p0));
			_members.StaticMethods.InvokeVoidMethod("gldrawFrame.(J)V", ptr);
		}

		[Register("initGLES", "(Ljava/lang/String;Ljava/lang/String;J)V", "")]
		public unsafe static void InitGLES(string? p0, string? p1, long p2)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			nint num = JNIEnv.NewString(p0);
			nint num2 = JNIEnv.NewString(p1);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[3];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((global::System.IntPtr)num));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue((global::System.IntPtr)num2));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + (nint)2 * (nint)global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(p2));
				_members.StaticMethods.InvokeVoidMethod("initGLES.(Ljava/lang/String;Ljava/lang/String;J)V", ptr);
			}
			finally
			{
				JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				JNIEnv.DeleteLocalRef((global::System.IntPtr)num2);
			}
		}

		[Register("initGLESBuffer", "(J)V", "")]
		public unsafe static void InitGLESBuffer(long p0)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
			global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(p0));
			_members.StaticMethods.InvokeVoidMethod("initGLESBuffer.(J)V", ptr);
		}

		[Register("initialDecode", "(IJ)V", "")]
		public unsafe static void InitialDecode(int p0, long p1)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
			global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(p0));
			global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(p1));
			_members.StaticMethods.InvokeVoidMethod("initialDecode.(IJ)V", ptr);
		}

		[Register("onNotify", "(Ljava/lang/String;I)V", "")]
		public unsafe static void OnNotify(string? data, int type)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			nint num = JNIEnv.NewString(data);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((global::System.IntPtr)num));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(type));
				_members.StaticMethods.InvokeVoidMethod("onNotify.(Ljava/lang/String;I)V", ptr);
			}
			finally
			{
				JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
			}
		}

		[Register("releaseGLES", "(J)V", "")]
		public unsafe static void ReleaseGLES(long p0)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
			global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(p0));
			_members.StaticMethods.InvokeVoidMethod("releaseGLES.(J)V", ptr);
		}

		[Register("setNotifyCallback", "(Lcom/android/VideoCodec/VideoDecoder$OnNotifyCallback;)V", "")]
		public unsafe static void SetNotifyCallback(IOnNotifyCallback? notifyCallback)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((notifyCallback == null) ? global::System.IntPtr.Zero : ((Object)notifyCallback).Handle));
				_members.StaticMethods.InvokeVoidMethod("setNotifyCallback.(Lcom/android/VideoCodec/VideoDecoder$OnNotifyCallback;)V", ptr);
			}
			finally
			{
				GC.KeepAlive((object)notifyCallback);
			}
		}
	}
	[Register("com/android/VideoCodec/VideoHeader", DoNotGenerateAcw = true)]
	public class VideoHeader : Object
	{
		private static readonly JniPeerMembers _members = (JniPeerMembers)new XAPeerMembers("com/android/VideoCodec/VideoHeader", typeof(VideoHeader));

		private static global::System.Delegate? cb_getHeight_GetHeight_I;

		private static global::System.Delegate? cb_getWidth_GetWidth_I;

		private static global::System.Delegate? cb_getPPS_GetPPS_arrayB;

		private static global::System.Delegate? cb_getSPS_GetSPS_arrayB;

		private static global::System.Delegate? cb_getSPSPPS_GetSPSPPS_arrayB;

		private static global::System.Delegate? cb_parseHeader_ParseHeader_arrayB_V;

		internal static nint class_ref
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		public override JniPeerMembers JniPeerMembers => _members;

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override nint ThresholdClass
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override global::System.Type ThresholdType => _members.ManagedPeerType;

		public unsafe virtual int Height
		{
			[Register("getHeight", "()I", "GetGetHeightHandler")]
			get
			{
				return _members.InstanceMethods.InvokeVirtualInt32Method("getHeight.()I", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			}
		}

		public unsafe virtual int Width
		{
			[Register("getWidth", "()I", "GetGetWidthHandler")]
			get
			{
				return _members.InstanceMethods.InvokeVirtualInt32Method("getWidth.()I", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			}
		}

		protected VideoHeader(nint javaReference, JniHandleOwnership transfer)
			: base((global::System.IntPtr)javaReference, transfer)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		[Register(".ctor", "()V", "")]
		public unsafe VideoHeader()
			: base(global::System.IntPtr.Zero, (JniHandleOwnership)0)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			if (((Object)this).Handle == global::System.IntPtr.Zero)
			{
				JniObjectReference val = _members.InstanceMethods.StartCreateInstance("()V", ((object)this).GetType(), (JniArgumentValue*)null);
				((Object)this).SetHandle(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
				_members.InstanceMethods.FinishCreateInstance("()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			}
		}

		private static global::System.Delegate GetGetHeightHandler()
		{
			if (cb_getHeight_GetHeight_I == null)
			{
				cb_getHeight_GetHeight_I = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_I(n_GetHeight));
			}
			return cb_getHeight_GetHeight_I;
		}

		private static int n_GetHeight(nint jnienv, nint native__this)
		{
			return Object.GetObject<VideoHeader>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).Height;
		}

		private static global::System.Delegate GetGetWidthHandler()
		{
			if (cb_getWidth_GetWidth_I == null)
			{
				cb_getWidth_GetWidth_I = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_I(n_GetWidth));
			}
			return cb_getWidth_GetWidth_I;
		}

		private static int n_GetWidth(nint jnienv, nint native__this)
		{
			return Object.GetObject<VideoHeader>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).Width;
		}

		private static global::System.Delegate GetGetPPSHandler()
		{
			if (cb_getPPS_GetPPS_arrayB == null)
			{
				cb_getPPS_GetPPS_arrayB = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_L(n_GetPPS));
			}
			return cb_getPPS_GetPPS_arrayB;
		}

		private static nint n_GetPPS(nint jnienv, nint native__this)
		{
			return JNIEnv.NewArray(Object.GetObject<VideoHeader>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).GetPPS());
		}

		[Register("getPPS", "()[B", "GetGetPPSHandler")]
		public unsafe virtual byte[]? GetPPS()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			JniObjectReference val = _members.InstanceMethods.InvokeVirtualObjectMethod("getPPS.()[B", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			return (byte[])JNIEnv.GetArray(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1, typeof(byte));
		}

		private static global::System.Delegate GetGetSPSHandler()
		{
			if (cb_getSPS_GetSPS_arrayB == null)
			{
				cb_getSPS_GetSPS_arrayB = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_L(n_GetSPS));
			}
			return cb_getSPS_GetSPS_arrayB;
		}

		private static nint n_GetSPS(nint jnienv, nint native__this)
		{
			return JNIEnv.NewArray(Object.GetObject<VideoHeader>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).GetSPS());
		}

		[Register("getSPS", "()[B", "GetGetSPSHandler")]
		public unsafe virtual byte[]? GetSPS()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			JniObjectReference val = _members.InstanceMethods.InvokeVirtualObjectMethod("getSPS.()[B", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			return (byte[])JNIEnv.GetArray(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1, typeof(byte));
		}

		private static global::System.Delegate GetGetSPSPPSHandler()
		{
			if (cb_getSPSPPS_GetSPSPPS_arrayB == null)
			{
				cb_getSPSPPS_GetSPSPPS_arrayB = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_L(n_GetSPSPPS));
			}
			return cb_getSPSPPS_GetSPSPPS_arrayB;
		}

		private static nint n_GetSPSPPS(nint jnienv, nint native__this)
		{
			return JNIEnv.NewArray(Object.GetObject<VideoHeader>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).GetSPSPPS());
		}

		[Register("getSPSPPS", "()[B", "GetGetSPSPPSHandler")]
		public unsafe virtual byte[]? GetSPSPPS()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			JniObjectReference val = _members.InstanceMethods.InvokeVirtualObjectMethod("getSPSPPS.()[B", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			return (byte[])JNIEnv.GetArray(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1, typeof(byte));
		}

		private static global::System.Delegate GetParseHeader_arrayBHandler()
		{
			if (cb_parseHeader_ParseHeader_arrayB_V == null)
			{
				cb_parseHeader_ParseHeader_arrayB_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPL_V(n_ParseHeader_arrayB));
			}
			return cb_parseHeader_ParseHeader_arrayB_V;
		}

		private static void n_ParseHeader_arrayB(nint jnienv, nint native__this, nint native_rawData)
		{
			VideoHeader videoHeader = Object.GetObject<VideoHeader>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			byte[] array = (byte[])JNIEnv.GetArray((global::System.IntPtr)native_rawData, (JniHandleOwnership)0, typeof(byte));
			videoHeader.ParseHeader(array);
			if (array != null)
			{
				JNIEnv.CopyArray(array, (global::System.IntPtr)native_rawData);
			}
		}

		[Register("parseHeader", "([B)V", "GetParseHeader_arrayBHandler")]
		public unsafe virtual void ParseHeader(byte[]? rawData)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			nint num = JNIEnv.NewArray(rawData);
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((global::System.IntPtr)num));
				_members.InstanceMethods.InvokeVirtualVoidMethod("parseHeader.([B)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				if (rawData != null)
				{
					JNIEnv.CopyArray((global::System.IntPtr)num, rawData);
					JNIEnv.DeleteLocalRef((global::System.IntPtr)num);
				}
				GC.KeepAlive((object)rawData);
			}
		}
	}
}
namespace Com.Android.Player
{
	[Register("com/android/player/PlayerActivity", DoNotGenerateAcw = true)]
	public class PlayerActivity : Object
	{
		private static readonly JniPeerMembers _members = (JniPeerMembers)new XAPeerMembers("com/android/player/PlayerActivity", typeof(PlayerActivity));

		internal static nint class_ref
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		public override JniPeerMembers JniPeerMembers => _members;

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override nint ThresholdClass
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override global::System.Type ThresholdType => _members.ManagedPeerType;

		protected PlayerActivity(nint javaReference, JniHandleOwnership transfer)
			: base((global::System.IntPtr)javaReference, transfer)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		[Register(".ctor", "()V", "")]
		public unsafe PlayerActivity()
			: base(global::System.IntPtr.Zero, (JniHandleOwnership)0)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			if (((Object)this).Handle == global::System.IntPtr.Zero)
			{
				JniObjectReference val = _members.InstanceMethods.StartCreateInstance("()V", ((object)this).GetType(), (JniArgumentValue*)null);
				((Object)this).SetHandle(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
				_members.InstanceMethods.FinishCreateInstance("()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			}
		}

		[Register("setJvm", "()V", "")]
		public unsafe static void SetJvm()
		{
			_members.StaticMethods.InvokeVoidMethod("setJvm.()V", (JniArgumentValue*)null);
		}
	}
}
namespace Com.Android.Opengl
{
	[Register("com/android/opengl/GLESRendererImpl", DoNotGenerateAcw = true)]
	public class GLESRendererImpl : Object, IGLESRenderer, IJavaObject, global::System.IDisposable, IJavaPeerable
	{
		private static readonly JniPeerMembers _members = (JniPeerMembers)new XAPeerMembers("com/android/opengl/GLESRendererImpl", typeof(GLESRendererImpl));

		private static global::System.Delegate? cb_cleanPreview_CleanPreview_Z_V;

		private static global::System.Delegate? cb_onDestroy_OnDestroy_V;

		private static global::System.Delegate? cb_onDrawFrame_OnDrawFrame_V;

		private static global::System.Delegate? cb_onPause_OnPause_V;

		private static global::System.Delegate? cb_onResume_OnResume_V;

		private static global::System.Delegate? cb_onSurfaceChanged_OnSurfaceChanged_II_V;

		private static global::System.Delegate? cb_onSurfaceCreated_OnSurfaceCreated_V;

		internal static nint class_ref
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		public override JniPeerMembers JniPeerMembers => _members;

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override nint ThresholdClass
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override global::System.Type ThresholdType => _members.ManagedPeerType;

		protected GLESRendererImpl(nint javaReference, JniHandleOwnership transfer)
			: base((global::System.IntPtr)javaReference, transfer)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		[Register(".ctor", "(Landroid/content/Context;Lcom/android/VideoCodec/SWDecoder;)V", "")]
		public unsafe GLESRendererImpl(Context? context, SWDecoder? swDecoder)
			: base(global::System.IntPtr.Zero, (JniHandleOwnership)0)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			if (((Object)this).Handle != global::System.IntPtr.Zero)
			{
				return;
			}
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((context == null) ? global::System.IntPtr.Zero : ((Object)context).Handle));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue((swDecoder == null) ? global::System.IntPtr.Zero : ((Object)swDecoder).Handle));
				JniObjectReference val = _members.InstanceMethods.StartCreateInstance("(Landroid/content/Context;Lcom/android/VideoCodec/SWDecoder;)V", ((object)this).GetType(), ptr);
				((Object)this).SetHandle(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
				_members.InstanceMethods.FinishCreateInstance("(Landroid/content/Context;Lcom/android/VideoCodec/SWDecoder;)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)context);
				GC.KeepAlive((object)swDecoder);
			}
		}

		private static global::System.Delegate GetCleanPreview_ZHandler()
		{
			if (cb_cleanPreview_CleanPreview_Z_V == null)
			{
				cb_cleanPreview_CleanPreview_Z_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPZ_V(n_CleanPreview_Z));
			}
			return cb_cleanPreview_CleanPreview_Z_V;
		}

		private static void n_CleanPreview_Z(nint jnienv, nint native__this, bool isClean)
		{
			Object.GetObject<GLESRendererImpl>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).CleanPreview(isClean);
		}

		[Register("cleanPreview", "(Z)V", "GetCleanPreview_ZHandler")]
		public unsafe virtual void CleanPreview(bool isClean)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
			global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(isClean));
			_members.InstanceMethods.InvokeVirtualVoidMethod("cleanPreview.(Z)V", (IJavaPeerable)(object)this, ptr);
		}

		private static global::System.Delegate GetOnDestroyHandler()
		{
			if (cb_onDestroy_OnDestroy_V == null)
			{
				cb_onDestroy_OnDestroy_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_OnDestroy));
			}
			return cb_onDestroy_OnDestroy_V;
		}

		private static void n_OnDestroy(nint jnienv, nint native__this)
		{
			Object.GetObject<GLESRendererImpl>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).OnDestroy();
		}

		[Register("onDestroy", "()V", "GetOnDestroyHandler")]
		public unsafe virtual void OnDestroy()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("onDestroy.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetOnDrawFrameHandler()
		{
			if (cb_onDrawFrame_OnDrawFrame_V == null)
			{
				cb_onDrawFrame_OnDrawFrame_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_OnDrawFrame));
			}
			return cb_onDrawFrame_OnDrawFrame_V;
		}

		private static void n_OnDrawFrame(nint jnienv, nint native__this)
		{
			Object.GetObject<GLESRendererImpl>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).OnDrawFrame();
		}

		[Register("onDrawFrame", "()V", "GetOnDrawFrameHandler")]
		public unsafe virtual void OnDrawFrame()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("onDrawFrame.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetOnPauseHandler()
		{
			if (cb_onPause_OnPause_V == null)
			{
				cb_onPause_OnPause_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_OnPause));
			}
			return cb_onPause_OnPause_V;
		}

		private static void n_OnPause(nint jnienv, nint native__this)
		{
			Object.GetObject<GLESRendererImpl>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).OnPause();
		}

		[Register("onPause", "()V", "GetOnPauseHandler")]
		public unsafe virtual void OnPause()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("onPause.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetOnResumeHandler()
		{
			if (cb_onResume_OnResume_V == null)
			{
				cb_onResume_OnResume_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_OnResume));
			}
			return cb_onResume_OnResume_V;
		}

		private static void n_OnResume(nint jnienv, nint native__this)
		{
			Object.GetObject<GLESRendererImpl>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).OnResume();
		}

		[Register("onResume", "()V", "GetOnResumeHandler")]
		public unsafe virtual void OnResume()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("onResume.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetOnSurfaceChanged_IIHandler()
		{
			if (cb_onSurfaceChanged_OnSurfaceChanged_II_V == null)
			{
				cb_onSurfaceChanged_OnSurfaceChanged_II_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPII_V(n_OnSurfaceChanged_II));
			}
			return cb_onSurfaceChanged_OnSurfaceChanged_II_V;
		}

		private static void n_OnSurfaceChanged_II(nint jnienv, nint native__this, int width, int height)
		{
			Object.GetObject<GLESRendererImpl>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).OnSurfaceChanged(width, height);
		}

		[Register("onSurfaceChanged", "(II)V", "GetOnSurfaceChanged_IIHandler")]
		public unsafe virtual void OnSurfaceChanged(int width, int height)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
			global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(width));
			global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(height));
			_members.InstanceMethods.InvokeVirtualVoidMethod("onSurfaceChanged.(II)V", (IJavaPeerable)(object)this, ptr);
		}

		private static global::System.Delegate GetOnSurfaceCreatedHandler()
		{
			if (cb_onSurfaceCreated_OnSurfaceCreated_V == null)
			{
				cb_onSurfaceCreated_OnSurfaceCreated_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_OnSurfaceCreated));
			}
			return cb_onSurfaceCreated_OnSurfaceCreated_V;
		}

		private static void n_OnSurfaceCreated(nint jnienv, nint native__this)
		{
			Object.GetObject<GLESRendererImpl>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).OnSurfaceCreated();
		}

		[Register("onSurfaceCreated", "()V", "GetOnSurfaceCreatedHandler")]
		public unsafe virtual void OnSurfaceCreated()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("onSurfaceCreated.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		[Register("readRawTextFile", "(Landroid/content/Context;I)Ljava/lang/String;", "")]
		public unsafe static string? ReadRawTextFile(Context? context, int resId)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((context == null) ? global::System.IntPtr.Zero : ((Object)context).Handle));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(resId));
				JniObjectReference val = _members.StaticMethods.InvokeObjectMethod("readRawTextFile.(Landroid/content/Context;I)Ljava/lang/String;", ptr);
				return JNIEnv.GetString(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
			}
			finally
			{
				GC.KeepAlive((object)context);
			}
		}
	}
	[Register("com/android/opengl/GLESTVThread", DoNotGenerateAcw = true)]
	public class GLESTVThread : Thread
	{
		private static readonly JniPeerMembers _members = (JniPeerMembers)new XAPeerMembers("com/android/opengl/GLESTVThread", typeof(GLESTVThread));

		private static global::System.Delegate? cb_onDestroy_OnDestroy_V;

		private static global::System.Delegate? cb_onPause_OnPause_V;

		private static global::System.Delegate? cb_onResume_OnResume_V;

		private static global::System.Delegate? cb_onSurfaceChanged_OnSurfaceChanged_II_V;

		private static global::System.Delegate? cb_requestRender_RequestRender_V;

		private static global::System.Delegate? cb_setRenderMode_SetRenderMode_I_V;

		internal static nint class_ref
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		public override JniPeerMembers JniPeerMembers => _members;

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override nint ThresholdClass
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override global::System.Type ThresholdType => _members.ManagedPeerType;

		protected GLESTVThread(nint javaReference, JniHandleOwnership transfer)
			: base((global::System.IntPtr)javaReference, transfer)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		[Register(".ctor", "(Landroid/graphics/SurfaceTexture;Lcom/android/opengl/IGLESRenderer;)V", "")]
		public unsafe GLESTVThread(SurfaceTexture? surface, IGLESRenderer? renderer)
			: base(global::System.IntPtr.Zero, (JniHandleOwnership)0)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			if (((Object)this).Handle != global::System.IntPtr.Zero)
			{
				return;
			}
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((surface == null) ? global::System.IntPtr.Zero : ((Object)surface).Handle));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue((renderer == null) ? global::System.IntPtr.Zero : ((Object)renderer).Handle));
				JniObjectReference val = _members.InstanceMethods.StartCreateInstance("(Landroid/graphics/SurfaceTexture;Lcom/android/opengl/IGLESRenderer;)V", ((object)this).GetType(), ptr);
				((Object)this).SetHandle(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
				_members.InstanceMethods.FinishCreateInstance("(Landroid/graphics/SurfaceTexture;Lcom/android/opengl/IGLESRenderer;)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)surface);
				GC.KeepAlive((object)renderer);
			}
		}

		private static global::System.Delegate GetOnDestroyHandler()
		{
			if (cb_onDestroy_OnDestroy_V == null)
			{
				cb_onDestroy_OnDestroy_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_OnDestroy));
			}
			return cb_onDestroy_OnDestroy_V;
		}

		private static void n_OnDestroy(nint jnienv, nint native__this)
		{
			Object.GetObject<GLESTVThread>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).OnDestroy();
		}

		[Register("onDestroy", "()V", "GetOnDestroyHandler")]
		public unsafe virtual void OnDestroy()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("onDestroy.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetOnPauseHandler()
		{
			if (cb_onPause_OnPause_V == null)
			{
				cb_onPause_OnPause_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_OnPause));
			}
			return cb_onPause_OnPause_V;
		}

		private static void n_OnPause(nint jnienv, nint native__this)
		{
			Object.GetObject<GLESTVThread>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).OnPause();
		}

		[Register("onPause", "()V", "GetOnPauseHandler")]
		public unsafe virtual void OnPause()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("onPause.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetOnResumeHandler()
		{
			if (cb_onResume_OnResume_V == null)
			{
				cb_onResume_OnResume_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_OnResume));
			}
			return cb_onResume_OnResume_V;
		}

		private static void n_OnResume(nint jnienv, nint native__this)
		{
			Object.GetObject<GLESTVThread>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).OnResume();
		}

		[Register("onResume", "()V", "GetOnResumeHandler")]
		public unsafe virtual void OnResume()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("onResume.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetOnSurfaceChanged_IIHandler()
		{
			if (cb_onSurfaceChanged_OnSurfaceChanged_II_V == null)
			{
				cb_onSurfaceChanged_OnSurfaceChanged_II_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPII_V(n_OnSurfaceChanged_II));
			}
			return cb_onSurfaceChanged_OnSurfaceChanged_II_V;
		}

		private static void n_OnSurfaceChanged_II(nint jnienv, nint native__this, int width, int height)
		{
			Object.GetObject<GLESTVThread>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).OnSurfaceChanged(width, height);
		}

		[Register("onSurfaceChanged", "(II)V", "GetOnSurfaceChanged_IIHandler")]
		public unsafe virtual void OnSurfaceChanged(int width, int height)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
			global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(width));
			global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(height));
			_members.InstanceMethods.InvokeVirtualVoidMethod("onSurfaceChanged.(II)V", (IJavaPeerable)(object)this, ptr);
		}

		private static global::System.Delegate GetRequestRenderHandler()
		{
			if (cb_requestRender_RequestRender_V == null)
			{
				cb_requestRender_RequestRender_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_RequestRender));
			}
			return cb_requestRender_RequestRender_V;
		}

		private static void n_RequestRender(nint jnienv, nint native__this)
		{
			Object.GetObject<GLESTVThread>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).RequestRender();
		}

		[Register("requestRender", "()V", "GetRequestRenderHandler")]
		public unsafe virtual void RequestRender()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("requestRender.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetSetRenderMode_IHandler()
		{
			if (cb_setRenderMode_SetRenderMode_I_V == null)
			{
				cb_setRenderMode_SetRenderMode_I_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPI_V(n_SetRenderMode_I));
			}
			return cb_setRenderMode_SetRenderMode_I_V;
		}

		private static void n_SetRenderMode_I(nint jnienv, nint native__this, int mode)
		{
			Object.GetObject<GLESTVThread>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).SetRenderMode(mode);
		}

		[Register("setRenderMode", "(I)V", "GetSetRenderMode_IHandler")]
		public unsafe virtual void SetRenderMode(int mode)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
			global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(mode));
			_members.InstanceMethods.InvokeVirtualVoidMethod("setRenderMode.(I)V", (IJavaPeerable)(object)this, ptr);
		}
	}
	[Register("com/android/opengl/GLTextureView", DoNotGenerateAcw = true)]
	public class GLTextureView : TextureView, ISurfaceTextureListener, IJavaObject, global::System.IDisposable, IJavaPeerable
	{
		[Register("RENDERMODE_CONTINUOUSLY")]
		public const int RendermodeContinuously = 1;

		[Register("RENDERMODE_WHEN_DIRTY")]
		public const int RendermodeWhenDirty = 0;

		private static readonly JniPeerMembers _members = (JniPeerMembers)new XAPeerMembers("com/android/opengl/GLTextureView", typeof(GLTextureView));

		private static global::System.Delegate? cb_closeGlThread_CloseGlThread_V;

		private static global::System.Delegate? cb_onDestroy_OnDestroy_V;

		private static global::System.Delegate? cb_onPause_OnPause_V;

		private static global::System.Delegate? cb_onResume_OnResume_V;

		private static global::System.Delegate? cb_onSurfaceTextureAvailable_OnSurfaceTextureAvailable_Landroid_graphics_SurfaceTexture_II_V;

		private static global::System.Delegate? cb_onSurfaceTextureDestroyed_OnSurfaceTextureDestroyed_Landroid_graphics_SurfaceTexture__Z;

		private static global::System.Delegate? cb_onSurfaceTextureSizeChanged_OnSurfaceTextureSizeChanged_Landroid_graphics_SurfaceTexture_II_V;

		private static global::System.Delegate? cb_onSurfaceTextureUpdated_OnSurfaceTextureUpdated_Landroid_graphics_SurfaceTexture__V;

		private static global::System.Delegate? cb_openGlThread_OpenGlThread_Landroid_graphics_SurfaceTexture_II_V;

		private static global::System.Delegate? cb_requestRender_RequestRender_V;

		private static global::System.Delegate? cb_setRenderMode_SetRenderMode_I_V;

		private static global::System.Delegate? cb_setRenderer_SetRenderer_Lcom_android_opengl_GLESRendererImpl__V;

		private static global::System.Delegate? cb_startGlThread_StartGlThread_II_V;

		private static global::System.Delegate? cb_updateWidthAndHeight_UpdateWidthAndHeight_II_V;

		internal static nint class_ref
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		public override JniPeerMembers JniPeerMembers => _members;

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override nint ThresholdClass
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override global::System.Type ThresholdType => _members.ManagedPeerType;

		protected GLTextureView(nint javaReference, JniHandleOwnership transfer)
			: base((global::System.IntPtr)javaReference, transfer)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		[Register(".ctor", "(Landroid/content/Context;)V", "")]
		public unsafe GLTextureView(Context? context)
			: base(global::System.IntPtr.Zero, (JniHandleOwnership)0)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			if (((Object)this).Handle != global::System.IntPtr.Zero)
			{
				return;
			}
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((context == null) ? global::System.IntPtr.Zero : ((Object)context).Handle));
				JniObjectReference val = _members.InstanceMethods.StartCreateInstance("(Landroid/content/Context;)V", ((object)this).GetType(), ptr);
				((Object)this).SetHandle(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
				_members.InstanceMethods.FinishCreateInstance("(Landroid/content/Context;)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)context);
			}
		}

		[Register(".ctor", "(Landroid/content/Context;Landroid/util/AttributeSet;)V", "")]
		public unsafe GLTextureView(Context? context, IAttributeSet? attrs)
			: base(global::System.IntPtr.Zero, (JniHandleOwnership)0)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			if (((Object)this).Handle != global::System.IntPtr.Zero)
			{
				return;
			}
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((context == null) ? global::System.IntPtr.Zero : ((Object)context).Handle));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue((attrs == null) ? global::System.IntPtr.Zero : ((Object)attrs).Handle));
				JniObjectReference val = _members.InstanceMethods.StartCreateInstance("(Landroid/content/Context;Landroid/util/AttributeSet;)V", ((object)this).GetType(), ptr);
				((Object)this).SetHandle(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
				_members.InstanceMethods.FinishCreateInstance("(Landroid/content/Context;Landroid/util/AttributeSet;)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)context);
				GC.KeepAlive((object)attrs);
			}
		}

		private static global::System.Delegate GetCloseGlThreadHandler()
		{
			if (cb_closeGlThread_CloseGlThread_V == null)
			{
				cb_closeGlThread_CloseGlThread_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_CloseGlThread));
			}
			return cb_closeGlThread_CloseGlThread_V;
		}

		private static void n_CloseGlThread(nint jnienv, nint native__this)
		{
			Object.GetObject<GLTextureView>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).CloseGlThread();
		}

		[Register("closeGlThread", "()V", "GetCloseGlThreadHandler")]
		public unsafe virtual void CloseGlThread()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("closeGlThread.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetOnDestroyHandler()
		{
			if (cb_onDestroy_OnDestroy_V == null)
			{
				cb_onDestroy_OnDestroy_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_OnDestroy));
			}
			return cb_onDestroy_OnDestroy_V;
		}

		private static void n_OnDestroy(nint jnienv, nint native__this)
		{
			Object.GetObject<GLTextureView>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).OnDestroy();
		}

		[Register("onDestroy", "()V", "GetOnDestroyHandler")]
		public unsafe virtual void OnDestroy()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("onDestroy.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetOnPauseHandler()
		{
			if (cb_onPause_OnPause_V == null)
			{
				cb_onPause_OnPause_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_OnPause));
			}
			return cb_onPause_OnPause_V;
		}

		private static void n_OnPause(nint jnienv, nint native__this)
		{
			Object.GetObject<GLTextureView>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).OnPause();
		}

		[Register("onPause", "()V", "GetOnPauseHandler")]
		public unsafe virtual void OnPause()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("onPause.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetOnResumeHandler()
		{
			if (cb_onResume_OnResume_V == null)
			{
				cb_onResume_OnResume_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_OnResume));
			}
			return cb_onResume_OnResume_V;
		}

		private static void n_OnResume(nint jnienv, nint native__this)
		{
			Object.GetObject<GLTextureView>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).OnResume();
		}

		[Register("onResume", "()V", "GetOnResumeHandler")]
		public unsafe virtual void OnResume()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("onResume.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetOnSurfaceTextureAvailable_Landroid_graphics_SurfaceTexture_IIHandler()
		{
			if (cb_onSurfaceTextureAvailable_OnSurfaceTextureAvailable_Landroid_graphics_SurfaceTexture_II_V == null)
			{
				cb_onSurfaceTextureAvailable_OnSurfaceTextureAvailable_Landroid_graphics_SurfaceTexture_II_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPLII_V(n_OnSurfaceTextureAvailable_Landroid_graphics_SurfaceTexture_II));
			}
			return cb_onSurfaceTextureAvailable_OnSurfaceTextureAvailable_Landroid_graphics_SurfaceTexture_II_V;
		}

		private static void n_OnSurfaceTextureAvailable_Landroid_graphics_SurfaceTexture_II(nint jnienv, nint native__this, nint native_surface, int width, int height)
		{
			GLTextureView gLTextureView = Object.GetObject<GLTextureView>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			SurfaceTexture surface = Object.GetObject<SurfaceTexture>((global::System.IntPtr)native_surface, (JniHandleOwnership)0);
			gLTextureView.OnSurfaceTextureAvailable(surface, width, height);
		}

		[Register("onSurfaceTextureAvailable", "(Landroid/graphics/SurfaceTexture;II)V", "GetOnSurfaceTextureAvailable_Landroid_graphics_SurfaceTexture_IIHandler")]
		public unsafe virtual void OnSurfaceTextureAvailable(SurfaceTexture? surface, int width, int height)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[3];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((surface == null) ? global::System.IntPtr.Zero : ((Object)surface).Handle));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(width));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + (nint)2 * (nint)global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(height));
				_members.InstanceMethods.InvokeVirtualVoidMethod("onSurfaceTextureAvailable.(Landroid/graphics/SurfaceTexture;II)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)surface);
			}
		}

		private static global::System.Delegate GetOnSurfaceTextureDestroyed_Landroid_graphics_SurfaceTexture_Handler()
		{
			if (cb_onSurfaceTextureDestroyed_OnSurfaceTextureDestroyed_Landroid_graphics_SurfaceTexture__Z == null)
			{
				cb_onSurfaceTextureDestroyed_OnSurfaceTextureDestroyed_Landroid_graphics_SurfaceTexture__Z = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPL_Z(n_OnSurfaceTextureDestroyed_Landroid_graphics_SurfaceTexture_));
			}
			return cb_onSurfaceTextureDestroyed_OnSurfaceTextureDestroyed_Landroid_graphics_SurfaceTexture__Z;
		}

		private static bool n_OnSurfaceTextureDestroyed_Landroid_graphics_SurfaceTexture_(nint jnienv, nint native__this, nint native_surface)
		{
			GLTextureView gLTextureView = Object.GetObject<GLTextureView>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			SurfaceTexture surface = Object.GetObject<SurfaceTexture>((global::System.IntPtr)native_surface, (JniHandleOwnership)0);
			return gLTextureView.OnSurfaceTextureDestroyed(surface);
		}

		[Register("onSurfaceTextureDestroyed", "(Landroid/graphics/SurfaceTexture;)Z", "GetOnSurfaceTextureDestroyed_Landroid_graphics_SurfaceTexture_Handler")]
		public unsafe virtual bool OnSurfaceTextureDestroyed(SurfaceTexture? surface)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((surface == null) ? global::System.IntPtr.Zero : ((Object)surface).Handle));
				return _members.InstanceMethods.InvokeVirtualBooleanMethod("onSurfaceTextureDestroyed.(Landroid/graphics/SurfaceTexture;)Z", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)surface);
			}
		}

		private static global::System.Delegate GetOnSurfaceTextureSizeChanged_Landroid_graphics_SurfaceTexture_IIHandler()
		{
			if (cb_onSurfaceTextureSizeChanged_OnSurfaceTextureSizeChanged_Landroid_graphics_SurfaceTexture_II_V == null)
			{
				cb_onSurfaceTextureSizeChanged_OnSurfaceTextureSizeChanged_Landroid_graphics_SurfaceTexture_II_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPLII_V(n_OnSurfaceTextureSizeChanged_Landroid_graphics_SurfaceTexture_II));
			}
			return cb_onSurfaceTextureSizeChanged_OnSurfaceTextureSizeChanged_Landroid_graphics_SurfaceTexture_II_V;
		}

		private static void n_OnSurfaceTextureSizeChanged_Landroid_graphics_SurfaceTexture_II(nint jnienv, nint native__this, nint native_surface, int width, int height)
		{
			GLTextureView gLTextureView = Object.GetObject<GLTextureView>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			SurfaceTexture surface = Object.GetObject<SurfaceTexture>((global::System.IntPtr)native_surface, (JniHandleOwnership)0);
			gLTextureView.OnSurfaceTextureSizeChanged(surface, width, height);
		}

		[Register("onSurfaceTextureSizeChanged", "(Landroid/graphics/SurfaceTexture;II)V", "GetOnSurfaceTextureSizeChanged_Landroid_graphics_SurfaceTexture_IIHandler")]
		public unsafe virtual void OnSurfaceTextureSizeChanged(SurfaceTexture? surface, int width, int height)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[3];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((surface == null) ? global::System.IntPtr.Zero : ((Object)surface).Handle));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(width));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + (nint)2 * (nint)global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(height));
				_members.InstanceMethods.InvokeVirtualVoidMethod("onSurfaceTextureSizeChanged.(Landroid/graphics/SurfaceTexture;II)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)surface);
			}
		}

		private static global::System.Delegate GetOnSurfaceTextureUpdated_Landroid_graphics_SurfaceTexture_Handler()
		{
			if (cb_onSurfaceTextureUpdated_OnSurfaceTextureUpdated_Landroid_graphics_SurfaceTexture__V == null)
			{
				cb_onSurfaceTextureUpdated_OnSurfaceTextureUpdated_Landroid_graphics_SurfaceTexture__V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPL_V(n_OnSurfaceTextureUpdated_Landroid_graphics_SurfaceTexture_));
			}
			return cb_onSurfaceTextureUpdated_OnSurfaceTextureUpdated_Landroid_graphics_SurfaceTexture__V;
		}

		private static void n_OnSurfaceTextureUpdated_Landroid_graphics_SurfaceTexture_(nint jnienv, nint native__this, nint native_surface)
		{
			GLTextureView gLTextureView = Object.GetObject<GLTextureView>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			SurfaceTexture surface = Object.GetObject<SurfaceTexture>((global::System.IntPtr)native_surface, (JniHandleOwnership)0);
			gLTextureView.OnSurfaceTextureUpdated(surface);
		}

		[Register("onSurfaceTextureUpdated", "(Landroid/graphics/SurfaceTexture;)V", "GetOnSurfaceTextureUpdated_Landroid_graphics_SurfaceTexture_Handler")]
		public unsafe virtual void OnSurfaceTextureUpdated(SurfaceTexture? surface)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((surface == null) ? global::System.IntPtr.Zero : ((Object)surface).Handle));
				_members.InstanceMethods.InvokeVirtualVoidMethod("onSurfaceTextureUpdated.(Landroid/graphics/SurfaceTexture;)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)surface);
			}
		}

		private static global::System.Delegate GetOpenGlThread_Landroid_graphics_SurfaceTexture_IIHandler()
		{
			if (cb_openGlThread_OpenGlThread_Landroid_graphics_SurfaceTexture_II_V == null)
			{
				cb_openGlThread_OpenGlThread_Landroid_graphics_SurfaceTexture_II_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPLII_V(n_OpenGlThread_Landroid_graphics_SurfaceTexture_II));
			}
			return cb_openGlThread_OpenGlThread_Landroid_graphics_SurfaceTexture_II_V;
		}

		private static void n_OpenGlThread_Landroid_graphics_SurfaceTexture_II(nint jnienv, nint native__this, nint native_surface, int width, int height)
		{
			GLTextureView gLTextureView = Object.GetObject<GLTextureView>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			SurfaceTexture surface = Object.GetObject<SurfaceTexture>((global::System.IntPtr)native_surface, (JniHandleOwnership)0);
			gLTextureView.OpenGlThread(surface, width, height);
		}

		[Register("openGlThread", "(Landroid/graphics/SurfaceTexture;II)V", "GetOpenGlThread_Landroid_graphics_SurfaceTexture_IIHandler")]
		public unsafe virtual void OpenGlThread(SurfaceTexture? surface, int width, int height)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[3];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((surface == null) ? global::System.IntPtr.Zero : ((Object)surface).Handle));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(width));
				global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + (nint)2 * (nint)global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(height));
				_members.InstanceMethods.InvokeVirtualVoidMethod("openGlThread.(Landroid/graphics/SurfaceTexture;II)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)surface);
			}
		}

		private static global::System.Delegate GetRequestRenderHandler()
		{
			if (cb_requestRender_RequestRender_V == null)
			{
				cb_requestRender_RequestRender_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_RequestRender));
			}
			return cb_requestRender_RequestRender_V;
		}

		private static void n_RequestRender(nint jnienv, nint native__this)
		{
			Object.GetObject<GLTextureView>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).RequestRender();
		}

		[Register("requestRender", "()V", "GetRequestRenderHandler")]
		public unsafe virtual void RequestRender()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("requestRender.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetSetRenderMode_IHandler()
		{
			if (cb_setRenderMode_SetRenderMode_I_V == null)
			{
				cb_setRenderMode_SetRenderMode_I_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPI_V(n_SetRenderMode_I));
			}
			return cb_setRenderMode_SetRenderMode_I_V;
		}

		private static void n_SetRenderMode_I(nint jnienv, nint native__this, int mode)
		{
			Object.GetObject<GLTextureView>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).SetRenderMode(mode);
		}

		[Register("setRenderMode", "(I)V", "GetSetRenderMode_IHandler")]
		public unsafe virtual void SetRenderMode(int mode)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
			global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(mode));
			_members.InstanceMethods.InvokeVirtualVoidMethod("setRenderMode.(I)V", (IJavaPeerable)(object)this, ptr);
		}

		private static global::System.Delegate GetSetRenderer_Lcom_android_opengl_GLESRendererImpl_Handler()
		{
			if (cb_setRenderer_SetRenderer_Lcom_android_opengl_GLESRendererImpl__V == null)
			{
				cb_setRenderer_SetRenderer_Lcom_android_opengl_GLESRendererImpl__V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPL_V(n_SetRenderer_Lcom_android_opengl_GLESRendererImpl_));
			}
			return cb_setRenderer_SetRenderer_Lcom_android_opengl_GLESRendererImpl__V;
		}

		private static void n_SetRenderer_Lcom_android_opengl_GLESRendererImpl_(nint jnienv, nint native__this, nint native_renderer)
		{
			GLTextureView gLTextureView = Object.GetObject<GLTextureView>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			GLESRendererImpl renderer = Object.GetObject<GLESRendererImpl>((global::System.IntPtr)native_renderer, (JniHandleOwnership)0);
			gLTextureView.SetRenderer(renderer);
		}

		[Register("setRenderer", "(Lcom/android/opengl/GLESRendererImpl;)V", "GetSetRenderer_Lcom_android_opengl_GLESRendererImpl_Handler")]
		public unsafe virtual void SetRenderer(GLESRendererImpl? renderer)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((renderer == null) ? global::System.IntPtr.Zero : ((Object)renderer).Handle));
				_members.InstanceMethods.InvokeVirtualVoidMethod("setRenderer.(Lcom/android/opengl/GLESRendererImpl;)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)renderer);
			}
		}

		private static global::System.Delegate GetStartGlThread_IIHandler()
		{
			if (cb_startGlThread_StartGlThread_II_V == null)
			{
				cb_startGlThread_StartGlThread_II_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPII_V(n_StartGlThread_II));
			}
			return cb_startGlThread_StartGlThread_II_V;
		}

		private static void n_StartGlThread_II(nint jnienv, nint native__this, int width, int height)
		{
			Object.GetObject<GLTextureView>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).StartGlThread(width, height);
		}

		[Register("startGlThread", "(II)V", "GetStartGlThread_IIHandler")]
		public unsafe virtual void StartGlThread(int width, int height)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
			global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(width));
			global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(height));
			_members.InstanceMethods.InvokeVirtualVoidMethod("startGlThread.(II)V", (IJavaPeerable)(object)this, ptr);
		}

		private static global::System.Delegate GetUpdateWidthAndHeight_IIHandler()
		{
			if (cb_updateWidthAndHeight_UpdateWidthAndHeight_II_V == null)
			{
				cb_updateWidthAndHeight_UpdateWidthAndHeight_II_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPII_V(n_UpdateWidthAndHeight_II));
			}
			return cb_updateWidthAndHeight_UpdateWidthAndHeight_II_V;
		}

		private static void n_UpdateWidthAndHeight_II(nint jnienv, nint native__this, int width, int height)
		{
			Object.GetObject<GLTextureView>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).UpdateWidthAndHeight(width, height);
		}

		[Register("updateWidthAndHeight", "(II)V", "GetUpdateWidthAndHeight_IIHandler")]
		public unsafe virtual void UpdateWidthAndHeight(int width, int height)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
			global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(width));
			global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(height));
			_members.InstanceMethods.InvokeVirtualVoidMethod("updateWidthAndHeight.(II)V", (IJavaPeerable)(object)this, ptr);
		}
	}
	[Register("com/android/opengl/IGLESRenderer", "", "Com.Android.Opengl.IGLESRendererInvoker")]
	public interface IGLESRenderer : IJavaObject, global::System.IDisposable, IJavaPeerable
	{
		[Register("onDestroy", "()V", "GetOnDestroyHandler:Com.Android.Opengl.IGLESRendererInvoker, ids.camera.insight.binding")]
		void OnDestroy();

		[Register("onDrawFrame", "()V", "GetOnDrawFrameHandler:Com.Android.Opengl.IGLESRendererInvoker, ids.camera.insight.binding")]
		void OnDrawFrame();

		[Register("onPause", "()V", "GetOnPauseHandler:Com.Android.Opengl.IGLESRendererInvoker, ids.camera.insight.binding")]
		void OnPause();

		[Register("onResume", "()V", "GetOnResumeHandler:Com.Android.Opengl.IGLESRendererInvoker, ids.camera.insight.binding")]
		void OnResume();

		[Register("onSurfaceChanged", "(II)V", "GetOnSurfaceChanged_IIHandler:Com.Android.Opengl.IGLESRendererInvoker, ids.camera.insight.binding")]
		void OnSurfaceChanged(int p0, int p1);

		[Register("onSurfaceCreated", "()V", "GetOnSurfaceCreatedHandler:Com.Android.Opengl.IGLESRendererInvoker, ids.camera.insight.binding")]
		void OnSurfaceCreated();
	}
	[Register("com/android/opengl/IGLESRenderer", DoNotGenerateAcw = true)]
	internal class IGLESRendererInvoker : Object, IGLESRenderer, IJavaObject, global::System.IDisposable, IJavaPeerable
	{
		private static readonly JniPeerMembers _members_com_android_opengl_IGLESRenderer = (JniPeerMembers)new XAPeerMembers("com/android/opengl/IGLESRenderer", typeof(IGLESRendererInvoker));

		private static global::System.Delegate? cb_onDestroy_OnDestroy_V;

		private static global::System.Delegate? cb_onDrawFrame_OnDrawFrame_V;

		private static global::System.Delegate? cb_onPause_OnPause_V;

		private static global::System.Delegate? cb_onResume_OnResume_V;

		private static global::System.Delegate? cb_onSurfaceChanged_OnSurfaceChanged_II_V;

		private static global::System.Delegate? cb_onSurfaceCreated_OnSurfaceCreated_V;

		private static nint java_class_ref
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members_com_android_opengl_IGLESRenderer.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		public override JniPeerMembers JniPeerMembers => _members_com_android_opengl_IGLESRenderer;

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override nint ThresholdClass
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members_com_android_opengl_IGLESRenderer.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override global::System.Type ThresholdType => _members_com_android_opengl_IGLESRenderer.ManagedPeerType;

		public IGLESRendererInvoker(nint handle, JniHandleOwnership transfer)
			: base((global::System.IntPtr)handle, transfer)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		private static global::System.Delegate GetOnDestroyHandler()
		{
			if (cb_onDestroy_OnDestroy_V == null)
			{
				cb_onDestroy_OnDestroy_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_OnDestroy));
			}
			return cb_onDestroy_OnDestroy_V;
		}

		private static void n_OnDestroy(nint jnienv, nint native__this)
		{
			Object.GetObject<IGLESRenderer>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).OnDestroy();
		}

		public unsafe void OnDestroy()
		{
			_members_com_android_opengl_IGLESRenderer.InstanceMethods.InvokeAbstractVoidMethod("onDestroy.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetOnDrawFrameHandler()
		{
			if (cb_onDrawFrame_OnDrawFrame_V == null)
			{
				cb_onDrawFrame_OnDrawFrame_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_OnDrawFrame));
			}
			return cb_onDrawFrame_OnDrawFrame_V;
		}

		private static void n_OnDrawFrame(nint jnienv, nint native__this)
		{
			Object.GetObject<IGLESRenderer>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).OnDrawFrame();
		}

		public unsafe void OnDrawFrame()
		{
			_members_com_android_opengl_IGLESRenderer.InstanceMethods.InvokeAbstractVoidMethod("onDrawFrame.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetOnPauseHandler()
		{
			if (cb_onPause_OnPause_V == null)
			{
				cb_onPause_OnPause_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_OnPause));
			}
			return cb_onPause_OnPause_V;
		}

		private static void n_OnPause(nint jnienv, nint native__this)
		{
			Object.GetObject<IGLESRenderer>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).OnPause();
		}

		public unsafe void OnPause()
		{
			_members_com_android_opengl_IGLESRenderer.InstanceMethods.InvokeAbstractVoidMethod("onPause.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetOnResumeHandler()
		{
			if (cb_onResume_OnResume_V == null)
			{
				cb_onResume_OnResume_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_OnResume));
			}
			return cb_onResume_OnResume_V;
		}

		private static void n_OnResume(nint jnienv, nint native__this)
		{
			Object.GetObject<IGLESRenderer>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).OnResume();
		}

		public unsafe void OnResume()
		{
			_members_com_android_opengl_IGLESRenderer.InstanceMethods.InvokeAbstractVoidMethod("onResume.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}

		private static global::System.Delegate GetOnSurfaceChanged_IIHandler()
		{
			if (cb_onSurfaceChanged_OnSurfaceChanged_II_V == null)
			{
				cb_onSurfaceChanged_OnSurfaceChanged_II_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPII_V(n_OnSurfaceChanged_II));
			}
			return cb_onSurfaceChanged_OnSurfaceChanged_II_V;
		}

		private static void n_OnSurfaceChanged_II(nint jnienv, nint native__this, int p0, int p1)
		{
			Object.GetObject<IGLESRenderer>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).OnSurfaceChanged(p0, p1);
		}

		public unsafe void OnSurfaceChanged(int p0, int p1)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[2];
			global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue(p0));
			global::System.Runtime.CompilerServices.Unsafe.Write((byte*)ptr + global::System.Runtime.CompilerServices.Unsafe.SizeOf<JniArgumentValue>(), new JniArgumentValue(p1));
			_members_com_android_opengl_IGLESRenderer.InstanceMethods.InvokeAbstractVoidMethod("onSurfaceChanged.(II)V", (IJavaPeerable)(object)this, ptr);
		}

		private static global::System.Delegate GetOnSurfaceCreatedHandler()
		{
			if (cb_onSurfaceCreated_OnSurfaceCreated_V == null)
			{
				cb_onSurfaceCreated_OnSurfaceCreated_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_OnSurfaceCreated));
			}
			return cb_onSurfaceCreated_OnSurfaceCreated_V;
		}

		private static void n_OnSurfaceCreated(nint jnienv, nint native__this)
		{
			Object.GetObject<IGLESRenderer>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).OnSurfaceCreated();
		}

		public unsafe void OnSurfaceCreated()
		{
			_members_com_android_opengl_IGLESRenderer.InstanceMethods.InvokeAbstractVoidMethod("onSurfaceCreated.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}
	}
	[Register("com/android/opengl/PendingThreadAider", DoNotGenerateAcw = true)]
	public class PendingThreadAider : Object
	{
		private static readonly JniPeerMembers _members = (JniPeerMembers)new XAPeerMembers("com/android/opengl/PendingThreadAider", typeof(PendingThreadAider));

		private static global::System.Delegate? cb_addToPending_AddToPending_Ljava_lang_Runnable__V;

		private static global::System.Delegate? cb_runPendings_RunPendings_V;

		internal static nint class_ref
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		public override JniPeerMembers JniPeerMembers => _members;

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override nint ThresholdClass
		{
			get
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				JniObjectReference peerReference = _members.JniPeerType.PeerReference;
				return ((JniObjectReference)(ref peerReference)).Handle;
			}
		}

		[DebuggerBrowsable(/*Could not decode attribute arguments.*/)]
		[EditorBrowsable(/*Could not decode attribute arguments.*/)]
		protected override global::System.Type ThresholdType => _members.ManagedPeerType;

		protected PendingThreadAider(nint javaReference, JniHandleOwnership transfer)
			: base((global::System.IntPtr)javaReference, transfer)
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		[Register(".ctor", "()V", "")]
		public unsafe PendingThreadAider()
			: base(global::System.IntPtr.Zero, (JniHandleOwnership)0)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			if (((Object)this).Handle == global::System.IntPtr.Zero)
			{
				JniObjectReference val = _members.InstanceMethods.StartCreateInstance("()V", ((object)this).GetType(), (JniArgumentValue*)null);
				((Object)this).SetHandle(((JniObjectReference)(ref val)).Handle, (JniHandleOwnership)1);
				_members.InstanceMethods.FinishCreateInstance("()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
			}
		}

		private static global::System.Delegate GetAddToPending_Ljava_lang_Runnable_Handler()
		{
			if (cb_addToPending_AddToPending_Ljava_lang_Runnable__V == null)
			{
				cb_addToPending_AddToPending_Ljava_lang_Runnable__V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PPL_V(n_AddToPending_Ljava_lang_Runnable_));
			}
			return cb_addToPending_AddToPending_Ljava_lang_Runnable__V;
		}

		private static void n_AddToPending_Ljava_lang_Runnable_(nint jnienv, nint native__this, nint native_runnable)
		{
			PendingThreadAider pendingThreadAider = Object.GetObject<PendingThreadAider>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0);
			IRunnable runnable = Object.GetObject<IRunnable>((global::System.IntPtr)native_runnable, (JniHandleOwnership)0);
			pendingThreadAider.AddToPending(runnable);
		}

		[Register("addToPending", "(Ljava/lang/Runnable;)V", "GetAddToPending_Ljava_lang_Runnable_Handler")]
		public unsafe virtual void AddToPending(IRunnable? runnable)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				JniArgumentValue* ptr = (JniArgumentValue*)stackalloc JniArgumentValue[1];
				global::System.Runtime.CompilerServices.Unsafe.Write(ptr, new JniArgumentValue((runnable == null) ? global::System.IntPtr.Zero : ((Object)runnable).Handle));
				_members.InstanceMethods.InvokeVirtualVoidMethod("addToPending.(Ljava/lang/Runnable;)V", (IJavaPeerable)(object)this, ptr);
			}
			finally
			{
				GC.KeepAlive((object)runnable);
			}
		}

		private static global::System.Delegate GetRunPendingsHandler()
		{
			if (cb_runPendings_RunPendings_V == null)
			{
				cb_runPendings_RunPendings_V = JNINativeWrapper.CreateDelegate((global::System.Delegate)new _JniMarshal_PP_V(n_RunPendings));
			}
			return cb_runPendings_RunPendings_V;
		}

		private static void n_RunPendings(nint jnienv, nint native__this)
		{
			Object.GetObject<PendingThreadAider>((global::System.IntPtr)jnienv, (global::System.IntPtr)native__this, (JniHandleOwnership)0).RunPendings();
		}

		[Register("runPendings", "()V", "GetRunPendingsHandler")]
		public unsafe virtual void RunPendings()
		{
			_members.InstanceMethods.InvokeVirtualVoidMethod("runPendings.()V", (IJavaPeerable)(object)this, (JniArgumentValue*)null);
		}
	}
}
namespace ids.camera.insight.binding
{
	public class Resource : Resource
	{
	}
}
