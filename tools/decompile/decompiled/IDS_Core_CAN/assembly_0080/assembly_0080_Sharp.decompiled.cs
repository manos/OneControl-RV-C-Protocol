using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using Microsoft.CodeAnalysis;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(/*Could not decode attribute arguments.*/)]
[assembly: TargetFramework(".NETCoreApp,Version=v7.0", FrameworkDisplayName = ".NET 7.0")]
[assembly: AssemblyCompany("Innovative Design Solutions, Inc.\\r\\n6801 15 Mile Road\\r\\nSterling Heights, MI 48312\\r\\nwww.idselectronics.com")]
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyCopyright("Copyright © 2012-2025 Innovative Design Solutions, Inc.")]
[assembly: AssemblyDescription("IDS Core IDS-CAN Descriptors Library")]
[assembly: AssemblyFileVersion("4.6.4.0")]
[assembly: AssemblyInformationalVersion("4.6.4.0+e7121ede06f05d042cf24e77bdcf920e85f9188f")]
[assembly: AssemblyProduct("IDS.Core.IDS_CAN.Descriptors")]
[assembly: AssemblyTitle("IDS.Core.IDS_CAN.Descriptors")]
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
	internal sealed class RefSafetyRulesAttribute : System.Attribute
	{
		public readonly int Version;

		public RefSafetyRulesAttribute(int P_0)
		{
			Version = P_0;
		}
	}
}
namespace IDS.Core.IDS_CAN
{
	public sealed class PRODUCT_ID
	{
		private static readonly Dictionary<ushort, PRODUCT_ID> Lookup = new Dictionary<ushort, PRODUCT_ID>();

		private static readonly List<PRODUCT_ID> List = new List<PRODUCT_ID>();

		public static readonly PRODUCT_ID UNKNOWN = new PRODUCT_ID(0, 0, "UNKNOWN");

		public static readonly PRODUCT_ID IDS_CAN_NETWORK_ANALYZER_PC_TOOL = new PRODUCT_ID(1, 17513, "IDS-CAN Network Analyzer");

		public static readonly PRODUCT_ID LCI_LINCPAD_WIFI_HUB_ASSEMBLY = new PRODUCT_ID(2, 17778, "WiFi Gateway Controller");

		public static readonly PRODUCT_ID LCI_LINCPAD_2_MOTOR_VELOCITY_SYNC_INWALL_SLIDE_CONTROL_ASSEMBLY = new PRODUCT_ID(3, 17788, "Inwall Slide Control");

		public static readonly PRODUCT_ID LCI_LINCPAD_DOCKING_STATION_ASSEMBLY = new PRODUCT_ID(4, 17799, "Docking Station");

		public static readonly PRODUCT_ID LCI_MF_5F_3V_FUSE_MUX_RECEIVER_LINCPAD_ASSEMBLY = new PRODUCT_ID(5, 18438, "MultiFunction 5-Output w/Hydraulic Controller");

		public static readonly PRODUCT_ID LCI_MF_8F_5V_FUSE_MUX_RECEIVER_LINCPAD_ASSEMBLY = new PRODUCT_ID(6, 18439, "MultiFunction 8-Output w/Hydraulic Controller");

		public static readonly PRODUCT_ID LCI_LINCPAD_LIGHTING_CONTROL_ASSEMBLY = new PRODUCT_ID(7, 18447, "Lighting Control");

		public static readonly PRODUCT_ID LCI_LINCPAD_MULTIFUNCTION_8_OUTPUT_RECEIVER_ASSEMBLY = new PRODUCT_ID(8, 18448, "MultiFunction 8-Output Receiver");

		public static readonly PRODUCT_ID LCI_LINCPAD_MULTIFUNCTION_5_OUTPUT_RECEIVER_ASSEMBLY = new PRODUCT_ID(9, 18449, "MultiFunction 5-Output Receiver");

		public static readonly PRODUCT_ID LCI_LINCPAD_LEVEL_UP_LEVELING_CONTROL_ASSEMBLY = new PRODUCT_ID(10, 18450, "Level Up Leveling Controller");

		public static readonly PRODUCT_ID LCI_LINCPAD_TANK_MONITOR_CONTROL_ASSEMBLY = new PRODUCT_ID(11, 18451, "Tank Monitor Controller");

		public static readonly PRODUCT_ID LCI_LINCPAD_SWITCH_TO_CAN_CONVERTER_CONTROL_ASSEMBLY = new PRODUCT_ID(12, 18452, "Switch to CAN Converter");

		public static readonly PRODUCT_ID LCI_LINCPAD_LINC_TO_CAN_TOUCHPAD_ASSEMBLY = new PRODUCT_ID(13, 18453, "Linc to CAN TouchPad");

		public static readonly PRODUCT_ID LCI_LINCPAD_TABLET = new PRODUCT_ID(14, 18511, "MyRV Tablet");

		public static readonly PRODUCT_ID LCI_LINCPAD_6_LEG_HALL_EFFECT_EJ_LEVELER_ASSEMBLY = new PRODUCT_ID(15, 19251, "6-Leg Hall Effect EJ Leveler");

		public static readonly PRODUCT_ID LCI_LINCPAD_4PT_CAMPER_JACK_CONTROL_ASSEMBLY = new PRODUCT_ID(16, 19445, "4 Point Camper Jack Control");

		public static readonly PRODUCT_ID LCI_MYRV_4PT_5W_HALL_EFFECT_EJ_LEVELER_CONTROL_ASSEMBLY = new PRODUCT_ID(17, 20239, "4-Leg Hall Effect EJ Leveler");

		public static readonly PRODUCT_ID LCI_MYRV_5PT_HALL_EFFECT_HYBRID_EJ_TT_LEVELER_ASSY = new PRODUCT_ID(18, 20242, "5-Leg Hybrid EJ TT Leveler");

		public static readonly PRODUCT_ID LCI_MYRV_4PT_FOLDING_JACK_TT_LEVELER_ASSY = new PRODUCT_ID(19, 20334, "4-Leg Folding Jack TT Leveler");

		public static readonly PRODUCT_ID LCI_MYRV_HOUR_METER_WITH_START_STOP_DRIVE = new PRODUCT_ID(20, 20583, "MyRV Hour Meter");

		public static readonly PRODUCT_ID LCI_MYRV_RGB_LED_LIGHTING_CONTROLLER_ASSEMBLY = new PRODUCT_ID(21, 20590, "RGB LED Lighting Controller");

		public static readonly PRODUCT_ID LCI_BLE_MF_5F_3V_FUSE_MUX_RECEIVER_LINCTAB_ASSEMBLY = new PRODUCT_ID(22, 20678, "Bluetooth MultiFunction 5-Output w/Hydraulic Controller");

		public static readonly PRODUCT_ID LCI_BLE_MF_8F_5V_FUSE_MUX_RECEIVER_LINCTAB_ASSEMBLY = new PRODUCT_ID(23, 20679, "Bluetooth MultiFunction 8-Output w/Hydraulic Controller");

		public static readonly PRODUCT_ID LCI_LINCTAB_BLE_MULTIFUNCTION_8_OUTPUT_RECEIVER_ASSEMBLY = new PRODUCT_ID(24, 20680, "Bluetooth MultiFunction 8-Output Receiver");

		public static readonly PRODUCT_ID LCI_LINCTAB_BLE_MULTIFUNCTION_5_OUTPUT_RECEIVER_ASSEMBLY = new PRODUCT_ID(25, 20681, "Bluetooth MultiFunction 5-Output Receiver");

		public static readonly PRODUCT_ID LCI_IR_REMOTE_CONTROL_ASSEMBLY = new PRODUCT_ID(26, 20805, "Infrared Remote Control Dome");

		public static readonly PRODUCT_ID LCI_MYRV_HVAC_DUAL_ZONE_CONTROL_UNIT_ASSEMBLY = new PRODUCT_ID(27, 21068, "HVAC Dual Zone Controller");

		public static readonly PRODUCT_ID LCI_MULTICHANNEL_LED_CONTROLLER_ASSEMBLY = new PRODUCT_ID(28, 21184, "Multichannel LED Controller");

		public static readonly PRODUCT_ID LCI_MYRV_GENERATOR_GENIE_CONTROL_UNIT = new PRODUCT_ID(29, 21084, "Generator Genie Controller");

		public static readonly PRODUCT_ID LCI_MYRV_HVAC_SINGLE_ZONE_CONTROL_UNIT_ASSEMBLY = new PRODUCT_ID(30, 21186, "HVAC Single Zone Controller");

		public static readonly PRODUCT_ID LCI_MYRV_HVAC_DUAL_ZONE_CONTROL_UNIT_WITH_GEN_HOUR_METER = new PRODUCT_ID(31, 21187, "HVAC Dual Zone Controller w/ Gen Hr Meter");

		public static readonly PRODUCT_ID LCI_MYRV_HVAC_SINGLE_ZONE_CONTROL_UNIT_WITH_GEN_HOUR_METER = new PRODUCT_ID(32, 21188, "HVAC Single Zone Controller w/ Gen Hr Meter");

		public static readonly PRODUCT_ID LCI_CAN_TO_ETHERNET_GATEWAY = new PRODUCT_ID(33, 21049, "CAN To Ethernet Gateway");

		public static readonly PRODUCT_ID LCI_LEVEL_UP_UNITY_CONTROL_ASSY = new PRODUCT_ID(34, 21296, "Level Up Unity Controller");

		public static readonly PRODUCT_ID LCI_3PT_CLASS_C_HYDRAULIC_LEVELER_ASSEMBLY = new PRODUCT_ID(35, 21299, "3 Point Class C Hydraulic Leveler");

		public static readonly PRODUCT_ID LCI_MYRV_5IN_ONECONTROL_TOUCH_PANEL_ASSEMBLY = new PRODUCT_ID(36, 21115, "5\" OneControl Touch Panel");

		public static readonly PRODUCT_ID LCI_MYRV_7IN_ONECONTROL_TOUCH_PANEL_ASSEMBLY = new PRODUCT_ID(37, 21116, "7\" OneControl Touch Panel");

		public static readonly PRODUCT_ID LCI_MULTIFUNCTION_OMEGA_10_REVERSING_4_LATCHING = new PRODUCT_ID(38, 21400, "Multifunction Omega 10 + 4");

		public static readonly PRODUCT_ID LCI_TT_LEVELER_4_X_3K_C_JACK_ASSEMBLY = new PRODUCT_ID(39, 21417, "TT Leveler (4 x 3k C-Jack)");

		public static readonly PRODUCT_ID LCI_MYRV_MOTORIZED_4PT_HYDRAULIC_LEVELER_ASSEMBLY = new PRODUCT_ID(40, 21419, "Motorized 4 Point Hydraulic Leveler");

		public static readonly PRODUCT_ID LCI_MYRV_MOTORIZED_3PT_HYDRAULIC_LEVELER_ASSEMBLY = new PRODUCT_ID(41, 21420, "Motorized 3 Point Hydraulic Leveler");

		public static readonly PRODUCT_ID LCI_IPDM_CONTROLLER_ASSEMBLY = new PRODUCT_ID(42, 21421, "In Transit Power Disconnect Controller");

		public static readonly PRODUCT_ID LCI_TANK_MONITOR_V2_CONTROL_ASSEMBLY = new PRODUCT_ID(43, 21422, "Tank Monitor Controller V2");

		public static readonly PRODUCT_ID LCI_MYRV_SMART_ARM_AWNING_CONTROL_ASSEMBLY = new PRODUCT_ID(44, 21425, "SMART Arm Awning Controller");

		public static readonly PRODUCT_ID LCI_MYRV_10IN_ONECONTROL_TOUCH_PANEL_ASSEMBLY = new PRODUCT_ID(45, 21428, "10\" OneControl Touch Panel");

		public static readonly PRODUCT_ID LCI_ONECONTROL_ANDROID_MOBILE_APPLICATION = new PRODUCT_ID(46, 21429, "OneControl Android Mobile App");

		public static readonly PRODUCT_ID LCI_ONECONTROL_IOS_MOBILE_APPLICATION = new PRODUCT_ID(47, 21430, "OneControl iOS Mobile App");

		public static readonly PRODUCT_ID LCI_MYRV_TT_LEVELER_HD_ASSY = new PRODUCT_ID(48, 21460, "TT Leveler HD");

		public static readonly PRODUCT_ID LCI_MYRV_TT_LEVELER_SE_ASSY = new PRODUCT_ID(49, 21461, "TT Leveler SE");

		public static readonly PRODUCT_ID LCI_MYRV_TT_LEVELER_LT_ASSY = new PRODUCT_ID(50, 21462, "TT Leveler LT");

		public static readonly PRODUCT_ID LCI_MYRV_5W_6PT_GC_3_0_LEVELER_TYPE_3_ASSY = new PRODUCT_ID(51, 21463, "Ground Control 3.0 5th Wheel Leveler (6-Point)");

		public static readonly PRODUCT_ID LCI_MYRV_5W_4PT_GC_3_0_LEVELER_TYPE_3_ASSY = new PRODUCT_ID(52, 21464, "Ground Control 3.0 5th Wheel Leveler (4-Point)");

		public static readonly PRODUCT_ID LCI_MYRV_SMART_POWER_TONGUE_JACK_CONTROL_ASSY = new PRODUCT_ID(53, 21465, "Smart Power-Tongue Jack Controller");

		public static readonly PRODUCT_ID LCI_MULTIFUNCTION_OMEGA_8_REVERSING_4_LATCHING = new PRODUCT_ID(54, 21480, "Multifunction Omega 8 + 4");

		public static readonly PRODUCT_ID LCI_MULTIFUNCTION_OMEGA_6_REVERSING_4_LATCHING = new PRODUCT_ID(55, 21481, "Multifunction Omega 6 + 4");

		public static readonly PRODUCT_ID LCI_MYRV_MULTIFUNCTION_5_OUTPUT_RECEIVER_ASSEMBLY_20A = new PRODUCT_ID(56, 21656, "MultiFunction 5-Output Receiver (20A)");

		public static readonly PRODUCT_ID LCI_MYRV_TT_LEVELER_S_3K_GC = new PRODUCT_ID(57, 21817, "TT Leveler S-3K-GC");

		public static readonly PRODUCT_ID LCI_MYRV_TT_LEVELER_S_2K_GC = new PRODUCT_ID(58, 21818, "TT Leveler S-2K-GC");

		public static readonly PRODUCT_ID LCI_MYRV_TT_LEVELER_S_3K_3K = new PRODUCT_ID(59, 21819, "TT Leveler S-3K-3K");

		public static readonly PRODUCT_ID LCI_MYRV_TT_LEVELER_S_2K_3K = new PRODUCT_ID(60, 21820, "TT Leveler S-2K-3K");

		public static readonly PRODUCT_ID LCI_MYRV_TT_LEVELER_S_2K_2K = new PRODUCT_ID(61, 21821, "TT Leveler S-2K-2K");

		public static readonly PRODUCT_ID LCI_LED_LIGHTING_CONTROLLER_8_1_1_OUTPUT_CAN_ONLY_ASSEMBLY = new PRODUCT_ID(62, 21866, "LED Lighting Controller (8 dimming, 1 latching, 1 RGB)");

		public static readonly PRODUCT_ID LCI_LED_LIGHTING_CONTROLLER_8_OUTPUT_ASSEMBLY = new PRODUCT_ID(63, 21867, "LED Lighting Controller (8 dimming, digital inputs)");

		public static readonly PRODUCT_ID LCI_LED_LIGHTING_CONTROLLER_8_1_OUTPUT_CAN_ONLY_ASSEMBLY = new PRODUCT_ID(64, 21868, "LED Lighting Controller (8 dimming, 1 latching)");

		public static readonly PRODUCT_ID LCI_LED_LIGHTING_CONTROLLER_8_OUTPUT_CAN_ONLY_ASSEMBLY = new PRODUCT_ID(65, 21869, "LED Lighting Controller (8 dimming)");

		public static readonly PRODUCT_ID LCI_MYRV_TT_LEVELER_M_3K_3K_W_LCD_TOUCHPAD_ASSY = new PRODUCT_ID(66, 21878, "TT Leveler M-3K-3K with LCD TouchPad");

		public static readonly PRODUCT_ID LCI_9_ZONE_LED_LIGHTING_CONTROL_8_DIMMING_1_LATCHING_ASSEMBLY = new PRODUCT_ID(67, 21882, "LED Lighting Controller (8 dimming, 1 latching, digital inputs)");

		public static readonly PRODUCT_ID LCI_EUROSLIDE_ASSEMBLY = new PRODUCT_ID(68, 22017, "EuroSlide Controller");

		public static readonly PRODUCT_ID SIMULATED_PRODUCT = new PRODUCT_ID(69, 0, "Simulated Product");

		public static readonly PRODUCT_ID IDS_CAN_SNIFFER = new PRODUCT_ID(70, 0, "IDS-CAN Sniffer");

		public static readonly PRODUCT_ID LCI_MYRV_TT_LEVELER_M_2K_GC_W_LCD_TP_ASSSEMBLY = new PRODUCT_ID(71, 22170, "TT Leveler M-2K-GC w LCD TP");

		public static readonly PRODUCT_ID LCI_MYRV_TT_LEVELER_M_2K_3K_W_LCD_TP_ASSEMBLY = new PRODUCT_ID(72, 22172, "TT Leveler M-2K-3K w LCD TP");

		public static readonly PRODUCT_ID LCI_MYRV_TT_LEVELER_S_3K_GC_W_LCD_TP_ASSEMBLY = new PRODUCT_ID(73, 22173, "TT Leveler S-3K-GC w LCD TP");

		public static readonly PRODUCT_ID LCI_MYRV_TT_LEVELER_S_2K_GC_W_LCD_TP_ASSEMBLY = new PRODUCT_ID(74, 22174, "TT Leveler S-2K-GC w LCD TP");

		public static readonly PRODUCT_ID LCI_MYRV_TT_LEVELER_S_3K_3K_W_LCD_TP_ASSEMBLY = new PRODUCT_ID(75, 22175, "TT Leveler S-3K-3K w LCD TP");

		public static readonly PRODUCT_ID LCI_MYRV_TT_LEVELER_S_2K_3K_W_LCD_TP_ASSEMBLY = new PRODUCT_ID(76, 22176, "TT Leveler S-2K-3K w LCD TP");

		public static readonly PRODUCT_ID LCI_MYRV_TT_LEVELER_M_2K_GC_ASSEMBLY = new PRODUCT_ID(77, 22177, "TT Leveler M-2K-GC");

		public static readonly PRODUCT_ID LCI_MYRV_TT_LEVELER_M_2K_3K_ASSEMBLY = new PRODUCT_ID(78, 22178, "TT Leveler M-2K-3K");

		public static readonly PRODUCT_ID CLASS_C_STABILIZER_CONTROL_2K_C_JACKS_ASSEMBLY = new PRODUCT_ID(79, 22282, "Class C Stabilizer Control (2K C-Jacks)");

		public static readonly PRODUCT_ID MYRV_IN_WALL_SLIDE_CONTROLLER_ASSEMBLY = new PRODUCT_ID(80, 22368, "In-Wall Slide Controller");

		public static readonly PRODUCT_ID MYRV_HVAC_SINGLE_ZONE_CONTROL_WITH_AUTO_START_GEN_HOUR_METER = new PRODUCT_ID(81, 22409, "Single-Zone HVAC Control + Generator Genie w/ Auto-Start");

		public static readonly PRODUCT_ID MYRV_HVAC_DUAL_ZONE_CONTROL_WITH_AUTO_START_GEN_HOUR_METER = new PRODUCT_ID(82, 22410, "Dual-Zone HVAC Control + Generator Genie w/ Auto-Start");

		public static readonly PRODUCT_ID MYRV_AUTO_START_GENERATOR_GENIE_CONTROL_UNIT = new PRODUCT_ID(83, 22411, "Generator Genie Controller w/ Auto-Start");

		public static readonly PRODUCT_ID MYRV_PG_IN_WALL_SLIDE_CONTROLLER_ASSEMBLY_TOWABLE_W_MANUAL_PROGRAM = new PRODUCT_ID(84, 22383, "PG In-Wall Slide Controller (Towable w/ Manual)");

		public static readonly PRODUCT_ID MYRV_PG_IN_WALL_SLIDE_CONTROLLER_ASSEMBLY_MOTORIZED_W_AUTO_PROGRAM = new PRODUCT_ID(85, 22384, "PG In-Wall Slide Controller (Motorized w/ Auto)");

		public static readonly PRODUCT_ID MYRV_PG_IN_WALL_SLIDE_CONTROLLER_ASSEMBLY_MOTORIZED_W_MANUAL_PROGRAM = new PRODUCT_ID(86, 22385, "PG In-Wall Slide Controller (Motorized w/ Manual)");

		public static readonly PRODUCT_ID MYRV_HVAC_V2_SINGLE_ZONE_CONTROL_WITH_AUTO_START_GEN_HOUR_METER = new PRODUCT_ID(87, 22503, "HVAC V2 Single-Zone Control with Auto-Start Gen Hour Meter");

		public static readonly PRODUCT_ID MYRV_HVAC_V2_DUAL_ZONE_CONTROL_WITH_AUTO_START_GEN_HOUR_METER = new PRODUCT_ID(88, 22504, "HVAC V2 Dual-Zone Control with Auto-Start Gen Hour Meter");

		public static readonly PRODUCT_ID MYRV_HVAC_V2_TRIPLE_ZONE_CONTROL_WITH_AUTO_START_GEN_HOUR_METER = new PRODUCT_ID(89, 22505, "HVAC V2 Triple-Zone Control with Auto-Start Gen Hour Meter");

		public static readonly PRODUCT_ID MYRV_HVAC_V2_SINGLE_ZONE_CONTROL = new PRODUCT_ID(90, 22506, "HVAC V2 Single-Zone Control");

		public static readonly PRODUCT_ID MYRV_HVAC_V2_DUAL_ZONE_CONTROL = new PRODUCT_ID(91, 22507, "HVAC V2 Dual-Zone Control");

		public static readonly PRODUCT_ID MYRV_HVAC_V2_TRIPLE_ZONE_CONTROL = new PRODUCT_ID(92, 22508, "HVAC V2 Triple-Zone Control");

		public static readonly PRODUCT_ID MULTIFUNCTION_UNITY_PARTIAL_ASSEMBLY_1 = new PRODUCT_ID(93, 22765, "Multifunction Unity Relay Control");

		public static readonly PRODUCT_ID MULTIFUNCTION_UNITY_PARTIAL_ASSEMBLY_2 = new PRODUCT_ID(94, 22765, "Multifunction Unity HVAC Control");

		public static readonly PRODUCT_ID MULTIFUNCTION_UNITY_PARTIAL_ASSEMBLY_3 = new PRODUCT_ID(95, 22765, "Multifunction Unity Tank Monitor Control");

		public static readonly PRODUCT_ID MYRV_SWITCH_BLOCK_8BV001_ASSEMBLY = new PRODUCT_ID(96, 22814, "myRV Switch Block 8BV001");

		public static readonly PRODUCT_ID ONECONTROL_CLOUD_GATEWAY_ASSEMBLY = new PRODUCT_ID(97, 22829, "OneControl Cloud Gateway");

		public static readonly PRODUCT_ID CAN_TO_ETHERNET_GATEWAY_12V_OUT = new PRODUCT_ID(98, 23011, "CAN To Ethernet Gateway (12V out)");

		public static readonly PRODUCT_ID MYRV_CLASS_C_LEVELER_ASSEMBLY = new PRODUCT_ID(99, 23610, "myRV Class C Leveler Assembly");

		public static readonly PRODUCT_ID MYRV_BLUETOOTH_GATEWAY_ASSEMBLY = new PRODUCT_ID(100, 23357, "myRV Bluetooth Gateway Assembly");

		public static readonly PRODUCT_ID MULTIFUNCTION_UBER_PARTIAL_ASSEMBLY_1_RELAY_CONTROL = new PRODUCT_ID(101, 23649, "Multifunction Uber Partial Assembly 1 (Relay Control)");

		public static readonly PRODUCT_ID MULTIFUNCTION_UBER_PARTIAL_ASSEMBLY_2_HVAC_CONTROL = new PRODUCT_ID(102, 23649, "Multifunction Uber Partial Assembly 2 (HVAC Control)");

		public static readonly PRODUCT_ID MULTIFUNCTION_UBER_PARTIAL_ASSEMBLY_3_LIGHTING_CONTROL = new PRODUCT_ID(103, 23649, "Multifunction Uber Partial Assembly 3 (Lighting Control)");

		public static readonly PRODUCT_ID MULTIFUNCTION_BASE_5_OUT_ELECTRICAL_BLE_ASSEMBLY = new PRODUCT_ID(104, 23651, "Multifunction Base 5-out Electrical BLE Assembly");

		public static readonly PRODUCT_ID MULTIFUNCTION_BASE_5_OUT_ELECTRICAL_NON_BLE_ASSEMBLY = new PRODUCT_ID(105, 23652, "Multifunction Base 5-out Electrical Non-BLE Assembly");

		public static readonly PRODUCT_ID MULTIFUNCTION_BASE_8_OUT_ELECTRICAL_BLE_ASSEMBLY = new PRODUCT_ID(106, 23653, "Multifunction Base 8-out Electrical BLE Assembly");

		public static readonly PRODUCT_ID MULTIFUNCTION_BASE_8_OUT_ELECTRICAL_NON_BLE_ASSEMBLY = new PRODUCT_ID(107, 23654, "Multifunction Base 8-out Electrical Non-BLE Assembly");

		public static readonly PRODUCT_ID MULTIFUNCTION_BASE_5_OUT_HYDRAULIC_BLE_ASSEMBLY = new PRODUCT_ID(108, 23656, "Multifunction Base 5-out Hydraulic BLE Assembly");

		public static readonly PRODUCT_ID MULTIFUNCTION_BASE_5_OUT_HYDRAULIC_NON_BLE_ASSEMBLY = new PRODUCT_ID(109, 23657, "Multifunction Base 5-out Hydraulic Non-BLE Assembly");

		public static readonly PRODUCT_ID MULTIFUNCTION_BASE_8_OUT_HYDRAULIC_BLE_ASSEMBLY = new PRODUCT_ID(110, 23658, "Multifunction Base 8-out Hydraulic BLE Assembly");

		public static readonly PRODUCT_ID MULTIFUNCTION_BASE_8_OUT_HYDRAULIC_NON_BLE_ASSEMBLY = new PRODUCT_ID(111, 23659, "Multifunction Base 8-out Hydraulic Non-BLE Assembly");

		public static readonly PRODUCT_ID LCI_LINCTAB_2_MOTOR_VELOCITY_SYNC_INWALL_SLIDE_CONTROL_ASSEMBLY_OPTION_2 = new PRODUCT_ID(112, 23756, "Inwall Slide Control (option 2)");

		public static readonly PRODUCT_ID LCI_LINCTAB_2_MOTOR_VELOCITY_SYNC_INWALL_SLIDE_CONTROL_ASSEMBLY_OPTION_3 = new PRODUCT_ID(113, 23757, "Inwall Slide Control (option 3)");

		public static readonly PRODUCT_ID LCI_LINCTAB_2_MOTOR_VELOCITY_SYNC_INWALL_SLIDE_CONTROL_ASSEMBLY_OPTION_4 = new PRODUCT_ID(114, 23758, "Inwall Slide Control (option 4)");

		public static readonly PRODUCT_ID ANDROID_DEVICE = new PRODUCT_ID(115, 0, "Android Device");

		public static readonly PRODUCT_ID IOS_DEVICE = new PRODUCT_ID(116, 0, "iOS Device");

		public static readonly PRODUCT_ID WINDOWS_DEVICE = new PRODUCT_ID(117, 0, "Windows Device");

		public static readonly PRODUCT_ID MYRV_RV_C_THERMOSTAT_CONTROL = new PRODUCT_ID(118, 23904, "RV-C Thermostat Control");

		public static readonly PRODUCT_ID LCI_ONECONTROL_2_MOTOR_VELOCITY_SYNC_INWALL_SLIDE_CONTROL_ASSEMBLY_OPTION_2 = new PRODUCT_ID(119, 24152, "Velocity Sync Inwall Slide Control (option 2)");

		public static readonly PRODUCT_ID LCI_ONECONTROL_2_MOTOR_VELOCITY_SYNC_INWALL_SLIDE_CONTROL_ASSEMBLY_OPTION_3 = new PRODUCT_ID(120, 24153, "Velocity Sync Inwall Slide Control (option 3)");

		public static readonly PRODUCT_ID LCI_ONECONTROL_2_MOTOR_VELOCITY_SYNC_INWALL_SLIDE_CONTROL_ASSEMBLY_OPTION_4 = new PRODUCT_ID(121, 24154, "Velocity Sync Inwall Slide Control (option 4)");

		public static readonly PRODUCT_ID MULTIFUNCTION_UNITY_M2_5_PARTIAL_ASSEMBLY_1_RELAY_CONTROL = new PRODUCT_ID(122, 24243, "Multifunction Unity M2 5 Partial Assembly 1 (Relay Control)");

		public static readonly PRODUCT_ID MULTIFUNCTION_UNITY_M2_5_PARTIAL_ASSEMBLY_2_HVAC_CONTROL = new PRODUCT_ID(123, 24243, "Multifunction Unity M2 5 Partial Assembly 2 (HVAC Control)");

		public static readonly PRODUCT_ID MULTIFUNCTION_UNITY_M2_5_PARTIAL_ASSEMBLY_3_TANK_MONITOR = new PRODUCT_ID(124, 24243, "Multifunction Unity M2 5 Partial Assembly 3 (Tank Monitor)");

		public static readonly PRODUCT_ID MULTIFUNCTION_UNITY_M3_PARTIAL_ASSEMBLY_1_RELAY_CONTROL = new PRODUCT_ID(125, 24244, "Multifunction Unity M3 Partial Assembly 1 (Relay Control)");

		public static readonly PRODUCT_ID MULTIFUNCTION_UNITY_M3_PARTIAL_ASSEMBLY_2_HVAC_CONTROL = new PRODUCT_ID(126, 24244, "Multifunction Unity M3 Partial Assembly 2 (HVAC Control)");

		public static readonly PRODUCT_ID MULTIFUNCTION_UNITY_X3_PARTIAL_ASSEMBLY_1_RELAY_CONTROL = new PRODUCT_ID(127, 24245, "Multifunction Unity X3 Partial Assembly 1 (Relay Control)");

		public static readonly PRODUCT_ID MULTIFUNCTION_UNITY_X3_PARTIAL_ASSEMBLY_2_HVAC_CONTROL = new PRODUCT_ID(128, 24245, "Multifunction Unity X3 Partial Assembly 2 (HVAC Control)");

		public static readonly PRODUCT_ID LCI_TT_M_5K_5K_LEVELER_FINAL_ASSY = new PRODUCT_ID(129, 24285, "TT Leveler M-5K-5K");

		public static readonly PRODUCT_ID LCI_TT_S_5K_5K_LEVELER_FINAL_ASSY = new PRODUCT_ID(130, 24289, "TT Leveler S-5K-5K");

		public static readonly PRODUCT_ID ONECONTROL_FIFTH_WHEEL_LEVELER_6PT_GC_3_0_V2_ASSEMBLY = new PRODUCT_ID(131, 24247, "Ground Control 3.0 5th Wheel Leveler v2 (6-Point)");

		public static readonly PRODUCT_ID LEVEL_UP_UNITY_KNEELING_AXLE_LEVELER_ASSEMBLY = new PRODUCT_ID(132, 24321, "Level Up Unity (Kneeling Axle) Leveler");

		public static readonly PRODUCT_ID LCI_ONECONTROL_5W_6PT_GC_3_0_LEVELER_TYPE_3_ASSEMBLY = new PRODUCT_ID(133, 22284, "Ground Control 3.0 5th Wheel Leveler (6-Point)");

		public static readonly PRODUCT_ID LCI_ONECONTROL_5W_4PT_GC_3_0_LEVELER_TYPE_3_ASSEMBLY = new PRODUCT_ID(134, 22285, "Ground Control 3.0 5th Wheel Leveler (4-Point)");

		public static readonly PRODUCT_ID MULTIFUNCTION_UNITY_M3T_PARTIAL_ASSEMBLY_1_RELAY_CONTROL = new PRODUCT_ID(135, 24606, "Multifunction Unity M3T Partial Assembly 1 (Relay Control)");

		public static readonly PRODUCT_ID MULTIFUNCTION_UNITY_M3T_PARTIAL_ASSEMBLY_2_HVAC_CONTROL = new PRODUCT_ID(136, 24606, "Multifunction Unity M3T Partial Assembly 2 (HVAC Control)");

		public static readonly PRODUCT_ID MULTIFUNCTION_UNITY_X2_ASSEMBLY = new PRODUCT_ID(137, 24608, "Multifunction Unity X2");

		public static readonly PRODUCT_ID BLUETOOTH_GATEWAY_DAUGHTER_BOARD_XT_ASSEMBLY = new PRODUCT_ID(138, 24955, "Bluetooth Gateway Daughter Board XT Assembly");

		public static readonly PRODUCT_ID MULTIFUNCTION_UNITY_X1_ASSEMBLY = new PRODUCT_ID(139, 24951, "Multifunction Unity X1 Assembly");

		public static readonly PRODUCT_ID ONECONTROL_LEVEL_UP_ADVANTAGE_ASSY = new PRODUCT_ID(140, 24999, "OneControl Level-Up Advantage");

		public static readonly PRODUCT_ID ONECONTROL_GC_3_0_ADVANTAGE_6PT_FIFTH_WHEEL_LEVELER_ASSY = new PRODUCT_ID(141, 25055, "OneControl GC 3.0 Advantage 6pt Fifth Wheel Leveler");

		public static readonly PRODUCT_ID ONECONTROL_GC_3_0_ADVANTAGE_4PT_FIFTH_WHEEL_LEVELER_ASSY = new PRODUCT_ID(142, 25057, "OneControl GC 3.0 Advantage 4pt Fifth Wheel Leveler");

		public static readonly PRODUCT_ID LCI_ONECONTROL_RV_C_LEVELER_CONTROL_ASSEMBLY = new PRODUCT_ID(143, 25283, "LCI OneControl RV-C Leveler Control Assembly");

		public static readonly PRODUCT_ID MULTIFUNCTION_UNITY_X0_ASSEMBLY = new PRODUCT_ID(144, 25336, "Multifunction Unity X0 Assembly");

		public static readonly PRODUCT_ID MULTIFUNCTION_UNITY_X1_5_ASSEMBLY = new PRODUCT_ID(145, 25361, "Multifunction Unity X1.5 Assembly");

		public static readonly PRODUCT_ID MULTIFUNCTION_UNITY_X1_HD_JAYCO_ASSEMBLY = new PRODUCT_ID(146, 25366, "Multifunction Unity X1 HD JAYCO Assembly");

		public static readonly PRODUCT_ID AQUAFI_HOTSPOT_ASSEMBLY = new PRODUCT_ID(147, 25287, "AquaFi Hotspot Assembly");

		public static readonly PRODUCT_ID CELLULAR_GATEWAY_ASSEMBLY = new PRODUCT_ID(148, 24902, "Cellular Gateway Assembly");

		public static readonly PRODUCT_ID ONECONTROL_HOTSPOT_ASSEMBLY = new PRODUCT_ID(149, 23600, "OneControl Hotspot Assembly");

		public static readonly PRODUCT_ID BLUETOOTH_GATEWAY_DAUGHTER_BOARD_ESP32_PROGRAMMED_PCBA = new PRODUCT_ID(150, 25745, "Bluetooth Gateway Daughter Board Esp32 Programmed PCBA");

		public static readonly PRODUCT_ID ONECONTROL_LEVEL_UP_ADVANTAGE_SLIDE_ASSY = new PRODUCT_ID(151, 25499, "Onecontrol Level Up Advantage Slide Assy");

		public static readonly PRODUCT_ID TPMS_TIRE_LINC = new PRODUCT_ID(152, 25570, "TPMS Tire Linc");

		public static readonly PRODUCT_ID ONECONTROL_MOTORIZED_4PT_HYDRAULIC_LEVELER = new PRODUCT_ID(153, 25776, "OneControl Motorized 4pt Hydraulic Leveler");

		public static readonly PRODUCT_ID ONECONTROL_TT_LEVELER_ADVANTAGE_S_3K_3K_ASSEMBLY = new PRODUCT_ID(154, 25518, "OneControl TT Leveler Advantage S-3K-3K Assembly");

		public static readonly PRODUCT_ID ONECONTROL_TT_LEVELER_ADVANTAGE_S_3K_5K_ASSEMBLY = new PRODUCT_ID(155, 25520, "OneControl TT Leveler Advantage S-3K-5K Assembly");

		public static readonly PRODUCT_ID ONECONTROL_TT_LEVELER_ADVANTAGE_S_2K_3K_ASSEMBLY = new PRODUCT_ID(156, 25522, "OneControl TT Leveler Advantage S-2K-3K Assembly");

		public static readonly PRODUCT_ID ONECONTROL_TT_LEVELER_ADVANTAGE_S_2K_5K_ASSEMBLY = new PRODUCT_ID(157, 25524, "OneControl TT Leveler Advantage S-2K-5K Assembly");

		public static readonly PRODUCT_ID ONECONTROL_TT_LEVELER_ADVANTAGE_S_5K_5K_ASSEMBLY = new PRODUCT_ID(158, 25526, "OneControl TT Leveler Advantage S-5K-5K Assembly");

		public static readonly PRODUCT_ID EURO_SLIDE_SMART_ROOM_12VOLT_V3 = new PRODUCT_ID(159, 25801, "Euro Slide Smart Room 12Volt V3");

		public static readonly PRODUCT_ID ONECONTROL_4PT_TT_LEVELER_ADVANTAGE_ASSY = new PRODUCT_ID(160, 25934, "OneControl 4pt TT Leveler Advantage Assy");

		public static readonly PRODUCT_ID ONECONTROL_4PT_CLASS_A_ADVANTAGE_LEVELER_ASSEMBLY = new PRODUCT_ID(161, 26095, "OneControl 4pt Class A Advantage Leveler Assembly");

		public static readonly PRODUCT_ID MONITOR_PANEL_6X9_ASSEMBLY = new PRODUCT_ID(162, 26201, "Monitor Panel 6x9 Assembly");

		public static readonly PRODUCT_ID MULTIFUNCTION_UNITY_X180T_ASSEMBLY = new PRODUCT_ID(163, 26261, "Multifunction Unity X180T Assembly");

		public static readonly PRODUCT_ID BLUETOOTH_GATEWAY_DAUGHTER_BOARD_XT_V2_PCBA = new PRODUCT_ID(164, 26430, "Bluetooth Gateway Daughter Board XT V2 PCBA");

		public static readonly PRODUCT_ID LCI_SURESHADE_IOS_MOBILE_APPLICATION = new PRODUCT_ID(165, 26464, "LCI SureShade IOS Mobile Application");

		public static readonly PRODUCT_ID LCI_SURESHADE_ANDROID_MOBILE_APPLICATION = new PRODUCT_ID(166, 26465, "LCI SureShade Android Mobile Application");

		public static readonly PRODUCT_ID CAMERA_REAR_OBSERVATION_OEM_ASSEMBLY = new PRODUCT_ID(167, 23670, "Camera Rear Observation OEM Assembly");

		public static readonly PRODUCT_ID CAMERA_REAR_OBSERVATION_AFTERMARKET_ASSEMBLY = new PRODUCT_ID(168, 23671, "Camera Rear Observation AfterMarket Assembly");

		public static readonly PRODUCT_ID ONECONTROL_3PT_MOTORIZED_ADVANTAGE_LEVELER_ASSEMBLY = new PRODUCT_ID(169, 26711, "OneControl 3pt Motorized Advantage Leveler Assembly");

		public static readonly PRODUCT_ID MULTIFUNCTION_UNITY_X145_ASSEMBLY = new PRODUCT_ID(170, 26813, "Multifunction Unity X145 Assembly");

		public static readonly PRODUCT_ID ONECONTROL_BT_GW_PARTIAL_ASSEMBLY_1_RS485_GW = new PRODUCT_ID(171, 26853, "Onecontrol BT GW Partial Assembly 1 RS485 GW");

		public static readonly PRODUCT_ID ONECONTROL_BT_GW_PARTIAL_ASSEMBLY_2_RVLINK_TPMS_GW = new PRODUCT_ID(172, 26853, "Onecontrol BT GW Partial Assembly 2 RvLink TPMS GW");

		public static readonly PRODUCT_ID ONECONTROL_BT_GW_PARTIAL_ASSEMBLY_3_ACCESSORY_GW = new PRODUCT_ID(173, 26853, "Onecontrol BT GW Partial Assembly 3 Accessory GW");

		public static readonly PRODUCT_ID BOTTLECHECK_WIRELESS_LP_TANK_SENSOR = new PRODUCT_ID(174, 27074, "BOTTLECHECK Wireless LP Tank Sensor");

		public static readonly PRODUCT_ID DUMP_TRAILER_CONTROLLER_2_SWITCH_ASSEMBLY = new PRODUCT_ID(175, 27080, "Dump Trailer Controller (2 switch) Assembly");

		public static readonly PRODUCT_ID DUMP_TRAILER_CONTROLLER_4_SWITCH_ASSEMBLY = new PRODUCT_ID(176, 27081, "Dump Trailer Controller (4 switch) Assembly");

		public static readonly PRODUCT_ID CELLULAR_ROUTER_GEN2_HOTSPOT_ONLY = new PRODUCT_ID(177, 27293, "Cellular Router Gen2 Hotspot Only");

		public static readonly PRODUCT_ID CELLULAR_ROUTER_GEN2_TELEMATICS_ONLY = new PRODUCT_ID(178, 27294, "Cellular Router Gen2 Telematics Only");

		public static readonly PRODUCT_ID CELLULAR_ROUTER_GEN2_HOTSPOT_WITH_TELEMATICS = new PRODUCT_ID(179, 27295, "Cellular Router Gen2 Hotspot With Telematics");

		public static readonly PRODUCT_ID ONECONTROL_TEMPERATURE_SENSOR_BT_ASSEMBLY = new PRODUCT_ID(180, 27217, "OneControl Temperature Sensor BT Assembly");

		public static readonly PRODUCT_ID UNITY_X260_ASSEMBLY = new PRODUCT_ID(181, 27395, "Unity X260 Assembly");

		public static readonly PRODUCT_ID ABS_CONTROLLER_ASSEMBLY = new PRODUCT_ID(182, 27376, "ABS controller assembly");

		public static readonly PRODUCT_ID LCI_MYRV_RV_C_STANDALONE_THERMOSTAT_ASSEMBLY = new PRODUCT_ID(183, 27312, "Standalone Thermostat Assembly");

		public static readonly PRODUCT_ID BLUETOOTH_GATEWAY_DAUGHTER_BOARD_RVLINK_ESP32_PROGRAMMED_PCBA = new PRODUCT_ID(184, 26275, "Bluetooth Gateway Daughter Board RvLink ESP32 Programmed PCBA");

		public static readonly PRODUCT_ID PREMIUM_MONITOR_PANEL_ASSEMBLY = new PRODUCT_ID(185, 27521, "Premium Monitor Panel Assembly");

		public static readonly PRODUCT_ID RVC_HVAC_V2_SINGLE_ZONE_CONTROL_ASSEMBLY = new PRODUCT_ID(186, 27529, "RVC HVAC V2 Single Zone Control Assembly");

		public static readonly PRODUCT_ID ONECONTROL_LEVEL_UP_ADV_BT_AL = new PRODUCT_ID(187, 27431, "Onecontrol Level Up Advantage BT Assembly");

		public static readonly PRODUCT_ID ONECONTROL_LEVEL_UP_ADV_SLIDE_OUTPUT_BT_AL = new PRODUCT_ID(188, 27440, "Onecontrol Level Up Advantage Slide Output BT Assembly");

		public static readonly PRODUCT_ID ONECONTROL_GC_3_0_ADV_4PT_5_WHEEL_LEVELER_BT_AL = new PRODUCT_ID(189, 27451, "Onecontrol Gc 3 0 Advantage 4pt Fifth Wheel Leveler BT Assembly");

		public static readonly PRODUCT_ID ONECONTROL_GC_3_0_ADV_6PT_5_WHEEL_LEVELER_BT_AL = new PRODUCT_ID(190, 27444, "Onecontrol Gc 3 0 Advantage 6pt Fifth Wheel Leveler BT Assembly");

		public static readonly PRODUCT_ID ONECONTROL_GC_3_0_HD_ADV_6PT_5_WHEEL_LEVELER_BT_AL = new PRODUCT_ID(191, 27540, "Onecontrol Gc 3 0 Hd Advantage 6pt Fifth Wheel Leveler BT Assembly");

		public static readonly PRODUCT_ID ONECONTROL_GC_3_0_HD_ADV_6PT_5_WHEEL_LEVELER_AL = new PRODUCT_ID(192, 27541, "Onecontrol Gc 3 0 Hd Advantage 6pt Fifth Wheel Leveler Assembly");

		public static readonly PRODUCT_ID ONECONTROL_M_3K_3K_TT_ADV_LEVELER_BT_AL = new PRODUCT_ID(193, 27504, "Onecontrol M 3k 3k TT Advantage Leveler BT Assembly");

		public static readonly PRODUCT_ID ONECONTROL_M_3K_5K_TT_ADV_LEVELER_BT_AL = new PRODUCT_ID(194, 27505, "Onecontrol M 3k 5k TT Advantage Leveler BT Assembly");

		public static readonly PRODUCT_ID ONECONTROL_M_5K_5K_TT_ADV_LEVELER_BT_AL = new PRODUCT_ID(195, 27506, "Onecontrol M 5k 5k TT Advantage Leveler BT Assembly");

		public static readonly PRODUCT_ID ONECONTROL_M_3K_3K_TT_ADV_LEVELER_AL = new PRODUCT_ID(196, 27463, "Onecontrol M 3k 3k TT Advantage Leveler Assembly");

		public static readonly PRODUCT_ID ONECONTROL_M_3K_5K_TT_ADV_LEVELER_AL = new PRODUCT_ID(197, 27470, "Onecontrol M 3k 5k TT Advantage Leveler Assembly");

		public static readonly PRODUCT_ID ONECONTROL_M_5K_5K_TT_ADV_LEVELER_AL = new PRODUCT_ID(198, 27474, "Onecontrol M 5k 5k TT Advantage Leveler Assembly");

		public static readonly PRODUCT_ID RVC_HVAC_V2_SINGLE_ZONE_CONTROL_AL_OPTION_2 = new PRODUCT_ID(199, 27940, "RVC HVAC V2 Single Zone Control Assembly Option 2");

		public static readonly PRODUCT_ID RVC_HVAC_V2_SINGLE_ZONE_CONTROL_AL_OPTION_3 = new PRODUCT_ID(200, 27941, "RVC HVAC V2 Single Zone Control Assembly Option 3");

		public static readonly PRODUCT_ID RVC_HVAC_V2_SINGLE_ZONE_CONTROL_AL_OPTION_4 = new PRODUCT_ID(201, 27942, "RVC HVAC V2 Single Zone Control Assembly Option 4");

		public static readonly PRODUCT_ID CURT_GROUP_SWAY_COMMAND_LINE_2_0 = new PRODUCT_ID(202, 28887, "Curt Group Sway Command 2.0 Controller Assembly");

		public static readonly PRODUCT_ID CAN_RE_FLASH_BOOTLOADER = new PRODUCT_ID(203, 28295, "CAN Re-Flash Bootloader");

		public static readonly PRODUCT_ID DC_BATTERY_MONITOR = new PRODUCT_ID(204, 27317, "DC Battery Monitor");

		public static readonly PRODUCT_ID LIPPERT_ONE_WIND_SENSOR = new PRODUCT_ID(205, 27722, "Lippert One Wind Sensor");

		public static readonly PRODUCT_ID FIFTH_TANK_MONITOR_PANEL = new PRODUCT_ID(206, 28519, "Fifth Tank Monitor Panel");

		public static readonly PRODUCT_ID ONECONTROL_4PT_MOTORIZED_TRITON_ADVANTAGE_LEVELER_BT = new PRODUCT_ID(207, 28528, "OneControl 4pt Motorized Triton Advantage Leveler BT Assembly");

		public static readonly PRODUCT_ID ONECONTROL_5TH_WHEEL_TRITON_ADVANTAGE_LEVELER_SLIDE_BT = new PRODUCT_ID(208, 28729, "OneControl 5th Wheel Triton Advantage Leveler Slide BT Assembly");

		public static readonly PRODUCT_ID ONECONTROL_3PT_MOTORIZED_TRITON_ADVANTAGE_LEVELER_BT = new PRODUCT_ID(209, 28732, "OneControl 3pt Motorized Triton Advantage Leveler BT Assembly");

		public static readonly PRODUCT_ID LIPPERT_AM_BT_DOOR_LOCK_ASSEMBLY = new PRODUCT_ID(210, 28720, "Lippert AM BT Door Lock Assembly");

		public static readonly PRODUCT_ID UNITY_X270_ASSEMBLY = new PRODUCT_ID(211, 29620, "Unity X270 Assembly");

		public static readonly PRODUCT_ID CURT_ECHO_BRAKE_CONTROLLER = new PRODUCT_ID(212, 0, "Curt Echo Brake Controller");

		public static readonly PRODUCT_ID BASECAMP_LEVELER_5W_TOUCHPAD_ASSEMBLY = new PRODUCT_ID(213, 29932, "Basecamp Leveler 5W Touchpad Assembly");

		public static readonly PRODUCT_ID BASECAMP_LEVELER_MOTORIZED_TOUCHPAD_ASSEMBLY = new PRODUCT_ID(214, 29976, "Basecamp Leveler Motorized Touchpad Assembly");

		public static readonly PRODUCT_ID BASECAMP_HYDRAULIC_5W_LEVELER_ASSEMBLY = new PRODUCT_ID(215, 29942, "BaseCamp Hydraulic 5W Leveler Assembly");

		public static readonly PRODUCT_ID BASECAMP_MOTORIZED_4PT_LEVELER_ASSEMBLY = new PRODUCT_ID(216, 29946, "BaseCamp Motorized 4pt Leveler Assembly");

		public static readonly PRODUCT_ID BASECAMP_MOTORIZED_3PT_LEVELER_ASSEMBLY = new PRODUCT_ID(217, 30021, "BaseCamp Motorized 3pt Leveler Assembly");

		public static readonly PRODUCT_ID OC_PG_INWALL_SLIDE_CONTROL_TOWABLE_AUTO_OP2 = new PRODUCT_ID(218, 30047, "OC PG-Inwall Slide Control - Towable Auto op2");

		public static readonly PRODUCT_ID OC_PG_INWALL_SLIDE_CONTROL_TOWABLE_AUTO_OP3 = new PRODUCT_ID(219, 30048, "OC PG-Inwall Slide Control - Towable Auto op3");

		public static readonly PRODUCT_ID OC_PG_INWALL_SLIDE_CONTROL_TOWABLE_AUTO_OP4 = new PRODUCT_ID(220, 30049, "OC PG-Inwall Slide Control - Towable Auto op4");

		public static readonly PRODUCT_ID ABS_AUSTRALIA_CONTROLLER_ASSEMBLY = new PRODUCT_ID(221, 30126, "ABS Australia Controller Assembly");

		public static readonly PRODUCT_ID LP_TANK_SENSOR_ASSEMBLY = new PRODUCT_ID(222, 28236, "LCI LP Tank Sensor");

		public static readonly PRODUCT_ID FURRION_ONECONTROL_HEADLESS_STEREO_MAIN_ASSY = new PRODUCT_ID(223, 30287, "Furrion OneControl Headless Stereo Main Assy");

		public static readonly PRODUCT_ID FURRION_ONECONTROL_HEADLESS_STEREO_SATELLITE_ASSY = new PRODUCT_ID(224, 30288, "Furrion OneControl Headless Stereo Satellite Assy");

		public static readonly PRODUCT_ID SUPER_PREMIUM_MONITOR_PANEL = new PRODUCT_ID(225, 30255, "Super Premium Monitor Panel");

		public static readonly PRODUCT_ID CURT_TPMS_TIRE_LINC_AUTO = new PRODUCT_ID(226, 30033, "Curt TPMS Tire Linc Auto");

		public static readonly PRODUCT_ID ABS_SWAY_CONTROLLER_ASSEMBLY = new PRODUCT_ID(227, 30613, "ABS/Sway Controller Assembly");

		public static readonly PRODUCT_ID ABS_SWAY_PANIC_BRAKE_CONTROLLER_ASSEMBLY = new PRODUCT_ID(228, 30614, "ABS/Sway/Panic Brake Controller Assembly");

		public static readonly PRODUCT_ID LCI_ONECONTROL_2_MOTOR_VELOCITY_SYNC_INWALL_SLIDE_CONTROL_ASSEMBLY_OPTION_5 = new PRODUCT_ID(229, 30631, "Velocity Sync Inwall Slide Control (option 5)");

		public static readonly PRODUCT_ID OC_PG_INWALL_SLIDE_CONTROL_TOWABLE_AUTO_OP5 = new PRODUCT_ID(230, 30715, "OC PG-Inwall Slide Control - Towable Auto op5");

		public static readonly PRODUCT_ID UNITY_X180D_ASSEMBLY = new PRODUCT_ID(231, 30599, "Unity X180D Assembly");

		public static readonly PRODUCT_ID UNITY_X270D_ASSEMBLY = new PRODUCT_ID(232, 30606, "Unity X270D Assembly");

		public static readonly PRODUCT_ID EMB_ABS_CONTROLLER_ASSEMBLY = new PRODUCT_ID(233, 30332, "EMB ABS Controller Assembly");

		public static readonly PRODUCT_ID EMB_MOTOR_CONTROLLER_ASSEMBLY = new PRODUCT_ID(234, 30333, "EMB Motor Controller Assembly");

		public static readonly PRODUCT_ID LCI_SURESHADE_AWNING_ASSEMBLY = new PRODUCT_ID(235, 31483, "LCI SureShade Awning Assembly");

		public static readonly PRODUCT_ID ELITETRACK_TOWABLE_SLIDE_CONTROLLER_ASSEMBLY = new PRODUCT_ID(236, 30954, "EliteTrack Towable Slide Controller Assembly");

		public static readonly PRODUCT_ID ELITETRACK_MOTORIZED_SLIDE_CONTROLLER_ASSEMBLY = new PRODUCT_ID(237, 31604, "EliteTrack Motorized Slide Controller Assembly");

		public static readonly PRODUCT_ID BASECAMP_ELECTRIC_5W_LEVELER_ASSEMBLY = new PRODUCT_ID(238, 31165, "BaseCamp Electric 5W Leveler Assembly");

		public static readonly PRODUCT_ID ONECONTROL_M_PSX2_GC_TT_LEVELER_ADVANTAGE_ASSY = new PRODUCT_ID(239, 31782, "OneControl M-PSX2-GC TT Leveler Advantage Assy");

		public static readonly PRODUCT_ID FLIC_BUTTON = new PRODUCT_ID(240, 0, "FLIC Button");

		public static readonly PRODUCT_ID ELITETRACK_TOWABLE_SLIDE_CONTROLLER_ASSEMBLY_OPTION_2 = new PRODUCT_ID(241, 31840, "EliteTrack Towable Slide Controller Assembly - Option 2");

		public static readonly PRODUCT_ID ELITETRACK_TOWABLE_SLIDE_CONTROLLER_ASSEMBLY_OPTION_3 = new PRODUCT_ID(242, 31843, "EliteTrack Towable Slide Controller Assembly - Option 3");

		public static readonly PRODUCT_ID ELITETRACK_TOWABLE_SLIDE_CONTROLLER_ASSEMBLY_OPTION_4 = new PRODUCT_ID(243, 31846, "EliteTrack Towable Slide Controller Assembly - Option 4");

		public static readonly PRODUCT_ID ELITETRACK_TOWABLE_SLIDE_CONTROLLER_ASSEMBLY_OPTION_5 = new PRODUCT_ID(244, 31849, "EliteTrack Towable Slide Controller Assembly - Option 5");

		public static readonly PRODUCT_ID TPMS_2_5_HANDHELD_DISPLAY_ASSEMBLY = new PRODUCT_ID(245, 27061, "LoCap Display Assembly");

		public static readonly PRODUCT_ID UNITY_X4C_PARTIAL_ASSEMBLY = new PRODUCT_ID(246, 32586, "Unity X4C Partial Assembly 4 (RV-C Thermostat Control)");

		public static readonly PRODUCT_ID TT3_LEVELER_GD_BT_ASSY = new PRODUCT_ID(247, 32621, "TT3 Leveler Gate Defender Bluetooth");

		public static readonly PRODUCT_ID TT3_LEVELER_GD_ASSY = new PRODUCT_ID(248, 32622, "TT3 Leveler Gate Defender");

		public static readonly PRODUCT_ID TT3_LEVELER_M_BT_ASSY = new PRODUCT_ID(249, 32623, "TT3 Leveler Modified Bluetooth");

		public static readonly PRODUCT_ID TT3_LEVELER_M_ASSY = new PRODUCT_ID(250, 32624, "TT3 Leveler Modified");

		public static readonly PRODUCT_ID UNITY_X340_ASSEMBLY = new PRODUCT_ID(251, 32178, "Unity X340 Assembly");

		public static readonly PRODUCT_ID TT2_LEVELER_M_BT_ASSY = new PRODUCT_ID(252, 33095, "TT2 Leveler Modified Bluetooth");

		public static readonly PRODUCT_ID TT2_LEVELER_M_ASSY = new PRODUCT_ID(253, 33120, "TT2 Leveler Modified");

		public static readonly PRODUCT_ID TRUECOURSE_OEM_AUSTRALIA_CONTROLLER_ASSEMBLY = new PRODUCT_ID(254, 33318, "TrueCourse OEM Australia Controller Assembly");

		public static readonly PRODUCT_ID LATCHXTEND_DOOR_LOCK = new PRODUCT_ID(255, 0, "LatchXtend Door Lock");

		public static readonly PRODUCT_ID TT2S_LEVELER_M_BT_ASSY = new PRODUCT_ID(256, 33353, "TT2S Leveler Modified Bluetooth");

		public static readonly PRODUCT_ID TT2S_LEVELER_M_ASSY = new PRODUCT_ID(257, 33358, "TT2S Leveler Modified");

		public static readonly PRODUCT_ID TT3S_LEVELER_M_BT_ASSY = new PRODUCT_ID(258, 33361, "TT3S Leveler Modified Bluetooth");

		public static readonly PRODUCT_ID TT3S_LEVELER_M_ASSY = new PRODUCT_ID(259, 33367, "TT3S Leveler Modified");

		public static readonly PRODUCT_ID TPMS_TIRELINC_PRO_REPEATER = new PRODUCT_ID(260, 30571, "TPMS TireLinc Pro Repeater");

		public static readonly PRODUCT_ID HIL_TEST_BENCH = new PRODUCT_ID(261, 0, "HIL Test Bench");

		public static readonly PRODUCT_ID PLATINUM_XL_MONITOR_PANEL = new PRODUCT_ID(262, 33718, "Platinum XL Monitor Panel");

		public static readonly PRODUCT_ID ONECONTROL_GC_3_0_ADVANTAGE_ALTERNATE_4PT_FIFTH_WHEEL_LEVELER_ASSY = new PRODUCT_ID(263, 33899, "OneControl GC 3.0 Advantage Alternate 4pt Fifth Wheel Leveler");

		public static readonly PRODUCT_ID GD15_SURESLIDE_MOTORIZED_SLIDE_CONTROLLER_ASSEMBLY = new PRODUCT_ID(264, 33892, "GD15 SureSlide Motorized Slide Controller Assembly");

		public static readonly PRODUCT_ID BTGW_DB_RVLINK_ESP32_PROGRAMMED_PCBA_LEVELER = new PRODUCT_ID(265, 26275, "Bluetooth Gateway Daughter Board RvLink ESP32 Programmed PCBA on Leveler");

		public static readonly PRODUCT_ID AWNING_MOUNTED_WIND_SENSOR = new PRODUCT_ID(266, 34251, "Awning Mounted Wind Sensor");

		public static readonly PRODUCT_ID UNITY_X240D_ASSEMBLY = new PRODUCT_ID(267, 34553, "Unity X240D Assembly");

		public readonly ushort Value;

		public readonly int AssemblyPartNumber;

		public readonly string Name;

		public bool IsValid => this?.Value > 0;

		public static System.Collections.Generic.IEnumerable<PRODUCT_ID> GetEnumerator()
		{
			return (System.Collections.Generic.IEnumerable<PRODUCT_ID>)List;
		}

		private PRODUCT_ID(ushort value)
		{
			Value = value;
			AssemblyPartNumber = 0;
			Name = "UNKNOWN_" + value.ToString("X4");
			if (value > 0 && !Lookup.ContainsKey(value))
			{
				Lookup.Add(value, this);
			}
		}

		private PRODUCT_ID(ushort value, int assembly_number, string name)
		{
			Value = value;
			AssemblyPartNumber = assembly_number;
			Name = name.Trim();
			if (value > 0)
			{
				List.Add(this);
				Lookup.Add(value, this);
			}
		}

		public static implicit operator ushort(PRODUCT_ID msg)
		{
			return msg?.Value ?? 0;
		}

		public static implicit operator PRODUCT_ID(ushort value)
		{
			if (value == 0)
			{
				return UNKNOWN;
			}
			PRODUCT_ID result = default(PRODUCT_ID);
			if (!Lookup.TryGetValue(value, ref result))
			{
				return new PRODUCT_ID(value);
			}
			return result;
		}

		public string ToString()
		{
			return Name;
		}
	}
	public sealed class DEVICE_TYPE
	{
		private static readonly DEVICE_TYPE[] Table;

		private static readonly List<DEVICE_TYPE> List;

		public const byte UNKNOWN = 0;

		public const byte GENERIC = 1;

		public const byte TABLET = 2;

		public const byte LATCHING_RELAY = 3;

		public const byte MOMENTARY_RELAY = 4;

		public const byte LATCHING_H_BRIDGE = 5;

		public const byte MOMENTARY_H_BRIDGE = 6;

		public const byte LEVELER_TYPE_1 = 7;

		public const byte SWITCH = 8;

		public const byte TOUCHSCREEN_SWITCH = 9;

		public const byte TANK_SENSOR = 10;

		public const byte LEVELER_TYPE_2 = 11;

		public const byte HOUR_METER = 12;

		public const byte RGB_LIGHT = 13;

		public const byte REAL_TIME_CLOCK = 14;

		public const byte IR_REMOTE_CONTROL = 15;

		public const byte HVAC_CONTROL = 16;

		public const byte LEVELER_TYPE_3 = 17;

		public const byte CAN_TO_ETHERNET_GATEWAY = 18;

		public const byte IN_TRANSIT_POWER_DISCONNECT = 19;

		public const byte DIMMABLE_LIGHT = 20;

		public const byte ONECONTROL_TOUCH_PAD = 21;

		public const byte ANDROID_MOBILE_DEVICE = 22;

		public const byte IOS_MOBILE_DEVICE = 23;

		public const byte GENERATOR_GENIE = 24;

		public const byte TEMPERATURE_SENSOR = 25;

		public const byte AC_POWER_MONITOR = 26;

		public const byte DC_POWER_MONITOR = 27;

		public const byte SETEC_POWER_MANAGER = 28;

		public const byte ONECONTROL_CLOUD_GATEWAY = 29;

		public const byte LATCHING_RELAY_TYPE_2 = 30;

		public const byte MOMENTARY_RELAY_TYPE_2 = 31;

		public const byte LATCHING_H_BRIDGE_TYPE_2 = 32;

		public const byte MOMENTARY_H_BRIDGE_TYPE_2 = 33;

		public const byte ONECONTROL_APPLICATION = 34;

		public const byte CONFIGURATOR_APPLICATION = 35;

		public const byte BLUETOOTH_GATEWAY = 36;

		public const byte MAXX_FAN = 37;

		public const byte RAIN_SENSOR = 38;

		public const byte CHASSIS_INFO = 39;

		public const byte LEVELER_TYPE_4 = 40;

		public const byte WIFI_GATEWAY = 41;

		public const byte TPMS_TIRE_LINC = 42;

		public const byte MONITOR_PANEL = 43;

		public const byte ACCESSORY_GATEWAY = 44;

		public const byte CAMERA = 45;

		public const byte JAYCO_AUS_TBB_GW = 46;

		public const byte AWNING_SENSOR = 47;

		public const byte BRAKE_CONTROLLER = 48;

		public const byte BATTERY_MONITOR = 49;

		public const byte REFLASH_BOOTLOADER = 50;

		public const byte DOOR_LOCK = 51;

		public const byte AUDIBLE_ALERT = 52;

		public const byte ECHO_BRAKE_CONTROL = 53;

		public const byte OCTP_WITH_RVLINK = 54;

		public const byte ANGLE_SENSOR = 55;

		public const byte LEVELER_TYPE_5 = 56;

		public const byte BASECAMP_TOUCHPAD = 57;

		public const byte ELECTRIC_MECHANICAL_BRAKE_CONTROLLER = 58;

		public const byte HEADLESS_STEREO = 59;

		public const byte BUTTON_FLIC = 60;

		public const byte RGBW_LIGHT = 61;

		public const byte HIL_TEST_BENCH = 62;

		public readonly byte Value;

		public readonly string Name;

		public readonly ICON Icon;

		public bool IsValid => this?.Value > 0;

		public static System.Collections.Generic.IEnumerable<DEVICE_TYPE> GetEnumerator()
		{
			return (System.Collections.Generic.IEnumerable<DEVICE_TYPE>)List;
		}

		static DEVICE_TYPE()
		{
			Table = new DEVICE_TYPE[256];
			List = new List<DEVICE_TYPE>();
			new DEVICE_TYPE(0, "UNKNOWN", ICON.UNKNOWN);
			new DEVICE_TYPE(1, "Generic Device", ICON.GENERIC);
			new DEVICE_TYPE(2, "Tablet", ICON.TABLET);
			new DEVICE_TYPE(3, "Latching Relay (type 1)", ICON.LATCHING_RELAY);
			new DEVICE_TYPE(4, "Momentary Relay (type 1)", ICON.MOMENTARY_RELAY);
			new DEVICE_TYPE(5, "Latching H-Bridge (type 1)", ICON.LATCHING_H_BRIDGE);
			new DEVICE_TYPE(6, "Momentary H-Bridge (type 1)", ICON.MOMENTARY_H_BRIDGE);
			new DEVICE_TYPE(7, "Leveler Type 1", ICON.LEVELER);
			new DEVICE_TYPE(8, "Switch", ICON.SWITCH);
			new DEVICE_TYPE(9, "Touchscreen Switch", ICON.TOUCHSCREEN_SWITCH);
			new DEVICE_TYPE(10, "Tank Sensor", ICON.TANK_SENSOR);
			new DEVICE_TYPE(11, "Leveler Type 2", ICON.JACKS);
			new DEVICE_TYPE(12, "Hour Meter", ICON.HOUR_METER);
			new DEVICE_TYPE(13, "RGB Light", ICON.RGB_LIGHT);
			new DEVICE_TYPE(14, "Clock", ICON.CLOCK);
			new DEVICE_TYPE(15, "Infrared Remote Control", ICON.IR_REMOTE_CONTROL);
			new DEVICE_TYPE(16, "HVAC Control", ICON.HVAC_CONTROL);
			new DEVICE_TYPE(17, "Leveler Type 3", ICON.LEVELER);
			new DEVICE_TYPE(18, "CAN to Ethernet Gateway", ICON.NETWORK_BRIDGE);
			new DEVICE_TYPE(19, "In Transit Power Disconnect", ICON.IPDM);
			new DEVICE_TYPE(20, "Dimmable Light", ICON.DIMMABLE_LIGHT);
			new DEVICE_TYPE(21, "OneControl Touch Pad", ICON.OCTP);
			new DEVICE_TYPE(22, "Android Mobile Device", ICON.ANDROID);
			new DEVICE_TYPE(23, "iOS Mobile Device", ICON.IOS);
			new DEVICE_TYPE(24, "Generator Genie", ICON.GENERATOR);
			new DEVICE_TYPE(25, "Temperature Sensor", ICON.THERMOMETER);
			new DEVICE_TYPE(26, "AC Power Monitor", ICON.POWER_MONITOR);
			new DEVICE_TYPE(27, "DC Power Monitor", ICON.POWER_MONITOR);
			new DEVICE_TYPE(28, "Power Manager", ICON.POWER_MANAGER);
			new DEVICE_TYPE(29, "OneControl Cloud Gateway", ICON.CLOUD);
			new DEVICE_TYPE(30, "Latching Relay (type 2)", ICON.LATCHING_RELAY);
			new DEVICE_TYPE(31, "Momentary Relay (type 2)", ICON.MOMENTARY_RELAY);
			new DEVICE_TYPE(32, "Latching H-Bridge (type 2)", ICON.LATCHING_H_BRIDGE);
			new DEVICE_TYPE(33, "Momentary H-Bridge (type 2)", ICON.MOMENTARY_H_BRIDGE);
			new DEVICE_TYPE(34, "OneControl App", ICON.DIAGNOSTIC_TOOL);
			new DEVICE_TYPE(35, "Configurator App", ICON.DIAGNOSTIC_TOOL);
			new DEVICE_TYPE(36, "Bluetooth Gateway", ICON.BLUETOOTH);
			new DEVICE_TYPE(37, "MaxxFan", ICON.VENT_COVER);
			new DEVICE_TYPE(38, "Rain Sensor", ICON.RAIN_SENSOR);
			new DEVICE_TYPE(39, "Chassis Info", ICON.CHASSIS);
			new DEVICE_TYPE(40, "Leveler Type 4", ICON.LEVELER);
			new DEVICE_TYPE(41, "WiFi Gateway", ICON.GENERIC);
			new DEVICE_TYPE(42, "TPMS Tire Linc", ICON.TPMS);
			new DEVICE_TYPE(43, "Monitor Panel", ICON.GENERIC);
			new DEVICE_TYPE(44, "Accessory Gateway", ICON.TABLET);
			new DEVICE_TYPE(45, "Camera", ICON.GENERIC);
			new DEVICE_TYPE(46, "Jayco Aus TBB GW", ICON.GENERIC);
			new DEVICE_TYPE(47, "Awning Sensor", ICON.AWNING);
			new DEVICE_TYPE(48, "Brake controller", ICON.GENERIC);
			new DEVICE_TYPE(49, "Battery Monitor", ICON.GENERIC);
			new DEVICE_TYPE(50, "ReFlash Bootloader", ICON.GENERIC);
			new DEVICE_TYPE(51, "Door Lock", ICON.LOCK);
			new DEVICE_TYPE(52, "Audible Alert", ICON.GENERIC);
			new DEVICE_TYPE(53, "Echo Break Controller", ICON.GENERIC);
			new DEVICE_TYPE(54, "OCTP RvLink", ICON.OCTP);
			new DEVICE_TYPE(55, "Angle Sensor", ICON.GENERIC);
			new DEVICE_TYPE(56, "Leveler Type 5", ICON.LEVELER);
			new DEVICE_TYPE(57, "Basecamp Touchpad", ICON.GENERIC);
			new DEVICE_TYPE(58, "Electric Mechanical Brake Controller", ICON.GENERIC);
			new DEVICE_TYPE(59, "Headless Stereo", ICON.GENERIC);
			new DEVICE_TYPE(60, "Button Flic", ICON.GENERIC);
			new DEVICE_TYPE(61, "RGBW Light", ICON.RGB_LIGHT);
			new DEVICE_TYPE(62, "HIL Test Bench", ICON.GENERIC);
		}

		private DEVICE_TYPE(byte value)
			: this(value, "UNKNOWN_" + value.ToString("X2"), ICON.UNKNOWN)
		{
		}

		private DEVICE_TYPE(byte value, string name, ICON icon)
		{
			Name = name.Trim();
			Value = value;
			Icon = icon;
			List.Add(this);
			Table[value] = this;
		}

		public static implicit operator byte(DEVICE_TYPE msg)
		{
			return msg.Value;
		}

		public static implicit operator DEVICE_TYPE(byte value)
		{
			DEVICE_TYPE dEVICE_TYPE = Table[value];
			if (dEVICE_TYPE != null)
			{
				return dEVICE_TYPE;
			}
			if (value == 0)
			{
				return (byte)0;
			}
			return new DEVICE_TYPE(value);
		}

		public string ToString()
		{
			return Name;
		}
	}
	public sealed class FUNCTION_NAME
	{
		private static readonly Dictionary<ushort, FUNCTION_NAME> Lookup;

		private static readonly List<FUNCTION_NAME> NameList;

		public static readonly FUNCTION_NAME UNKNOWN;

		public const ushort DIAGNOSTIC_TOOL = 1;

		public const ushort MYRV_TABLET = 2;

		public const ushort GAS_WATER_HEATER = 3;

		public const ushort ELECTRIC_WATER_HEATER = 4;

		public const ushort WATER_PUMP = 5;

		public const ushort BATH_VENT = 6;

		public const ushort LIGHT = 7;

		public const ushort FLOOD_LIGHT = 8;

		public const ushort WORK_LIGHT = 9;

		public const ushort FRONT_BEDROOM_CEILING_LIGHT = 10;

		public const ushort FRONT_BEDROOM_OVERHEAD_LIGHT = 11;

		public const ushort FRONT_BEDROOM_VANITY_LIGHT = 12;

		public const ushort FRONT_BEDROOM_SCONCE_LIGHT = 13;

		public const ushort FRONT_BEDROOM_LOFT_LIGHT = 14;

		public const ushort REAR_BEDROOM_CEILING_LIGHT = 15;

		public const ushort REAR_BEDROOM_OVERHEAD_LIGHT = 16;

		public const ushort REAR_BEDROOM_VANITY_LIGHT = 17;

		public const ushort REAR_BEDROOM_SCONCE_LIGHT = 18;

		public const ushort REAR_BEDROOM_LOFT_LIGHT = 19;

		public const ushort LOFT_LIGHT = 20;

		public const ushort FRONT_HALL_LIGHT = 21;

		public const ushort REAR_HALL_LIGHT = 22;

		public const ushort FRONT_BATHROOM_LIGHT = 23;

		public const ushort FRONT_BATHROOM_VANITY_LIGHT = 24;

		public const ushort FRONT_BATHROOM_CEILING_LIGHT = 25;

		public const ushort FRONT_BATHROOM_SHOWER_LIGHT = 26;

		public const ushort FRONT_BATHROOM_SCONCE_LIGHT = 27;

		public const ushort REAR_BATHROOM_VANITY_LIGHT = 28;

		public const ushort REAR_BATHROOM_CEILING_LIGHT = 29;

		public const ushort REAR_BATHROOM_SHOWER_LIGHT = 30;

		public const ushort REAR_BATHROOM_SCONCE_LIGHT = 31;

		public const ushort KITCHEN_CEILING_LIGHT = 32;

		public const ushort KITCHEN_SCONCE_LIGHT = 33;

		public const ushort KITCHEN_PENDANTS_LIGHT = 34;

		public const ushort KITCHEN_RANGE_LIGHT = 35;

		public const ushort KITCHEN_COUNTER_LIGHT = 36;

		public const ushort KITCHEN_BAR_LIGHT = 37;

		public const ushort KITCHEN_ISLAND_LIGHT = 38;

		public const ushort KITCHEN_CHANDELIER_LIGHT = 39;

		public const ushort KITCHEN_UNDER_CABINET_LIGHT = 40;

		public const ushort LIVING_ROOM_CEILING_LIGHT = 41;

		public const ushort LIVING_ROOM_SCONCE_LIGHT = 42;

		public const ushort LIVING_ROOM_PENDANTS_LIGHT = 43;

		public const ushort LIVING_ROOM_BAR_LIGHT = 44;

		public const ushort GARAGE_CEILING_LIGHT = 45;

		public const ushort GARAGE_CABINET_LIGHT = 46;

		public const ushort SECURITY_LIGHT = 47;

		public const ushort PORCH_LIGHT = 48;

		public const ushort AWNING_LIGHT = 49;

		public const ushort BATHROOM_LIGHT = 50;

		public const ushort BATHROOM_VANITY_LIGHT = 51;

		public const ushort BATHROOM_CEILING_LIGHT = 52;

		public const ushort BATHROOM_SHOWER_LIGHT = 53;

		public const ushort BATHROOM_SCONCE_LIGHT = 54;

		public const ushort HALL_LIGHT = 55;

		public const ushort BUNK_ROOM_LIGHT = 56;

		public const ushort BEDROOM_LIGHT = 57;

		public const ushort LIVING_ROOM_LIGHT = 58;

		public const ushort KITCHEN_LIGHT = 59;

		public const ushort LOUNGE_LIGHT = 60;

		public const ushort CEILING_LIGHT = 61;

		public const ushort ENTRY_LIGHT = 62;

		public const ushort BED_CEILING_LIGHT = 63;

		public const ushort BEDROOM_LAV_LIGHT = 64;

		public const ushort SHOWER_LIGHT = 65;

		public const ushort GALLEY_LIGHT = 66;

		public const ushort FRESH_TANK = 67;

		public const ushort GREY_TANK = 68;

		public const ushort BLACK_TANK = 69;

		public const ushort FUEL_TANK = 70;

		public const ushort GENERATOR_FUEL_TANK = 71;

		public const ushort AUXILIARY_FUEL_TANK = 72;

		public const ushort FRONT_BATH_GREY_TANK = 73;

		public const ushort FRONT_BATH_FRESH_TANK = 74;

		public const ushort FRONT_BATH_BLACK_TANK = 75;

		public const ushort REAR_BATH_GREY_TANK = 76;

		public const ushort REAR_BATH_FRESH_TANK = 77;

		public const ushort REAR_BATH_BLACK_TANK = 78;

		public const ushort MAIN_BATH_GREY_TANK = 79;

		public const ushort MAIN_BATH_FRESH_TANK = 80;

		public const ushort MAIN_BATH_BLACK_TANK = 81;

		public const ushort GALLEY_GREY_TANK = 82;

		public const ushort GALLEY_FRESH_TANK = 83;

		public const ushort GALLEY_BLACK_TANK = 84;

		public const ushort KITCHEN_GREY_TANK = 85;

		public const ushort KITCHEN_FRESH_TANK = 86;

		public const ushort KITCHEN_BLACK_TANK = 87;

		public const ushort LANDING_GEAR = 88;

		public const ushort FRONT_STABILIZER = 89;

		public const ushort REAR_STABILIZER = 90;

		public const ushort TV_LIFT = 91;

		public const ushort BED_LIFT = 92;

		public const ushort BATH_VENT_COVER = 93;

		public const ushort DOOR_LOCK = 94;

		public const ushort GENERATOR = 95;

		public const ushort SLIDE = 96;

		public const ushort MAIN_SLIDE = 97;

		public const ushort BEDROOM_SLIDE = 98;

		public const ushort GALLEY_SLIDE = 99;

		public const ushort KITCHEN_SLIDE = 100;

		public const ushort CLOSET_SLIDE = 101;

		public const ushort OPTIONAL_SLIDE = 102;

		public const ushort DOOR_SIDE_SLIDE = 103;

		public const ushort OFF_DOOR_SLIDE = 104;

		public const ushort AWNING = 105;

		public const ushort LEVEL_UP_LEVELER = 106;

		public const ushort WATER_TANK_HEATER = 107;

		public const ushort MYRV_TOUCHSCREEN = 108;

		public const ushort LEVELER = 109;

		public const ushort VENT_COVER = 110;

		public const ushort FRONT_BEDROOM_VENT_COVER = 111;

		public const ushort BEDROOM_VENT_COVER = 112;

		public const ushort FRONT_BATHROOM_VENT_COVER = 113;

		public const ushort MAIN_BATHROOM_VENT_COVER = 114;

		public const ushort REAR_BATHROOM_VENT_COVER = 115;

		public const ushort KITCHEN_VENT_COVER = 116;

		public const ushort LIVING_ROOM_VENT_COVER = 117;

		public const ushort FOUR_LEG_TRUCK_CAMPLER_LEVELER = 118;

		public const ushort SIX_LEG_HALL_EFFECT_EJ_LEVELER = 119;

		public const ushort PATIO_LIGHT = 120;

		public const ushort HUTCH_LIGHT = 121;

		public const ushort SCARE_LIGHT = 122;

		public const ushort DINETTE_LIGHT = 123;

		public const ushort BAR_LIGHT = 124;

		public const ushort OVERHEAD_LIGHT = 125;

		public const ushort OVERHEAD_BAR_LIGHT = 126;

		public const ushort FOYER_LIGHT = 127;

		public const ushort RAMP_DOOR_LIGHT = 128;

		public const ushort ENTERTAINMENT_LIGHT = 129;

		public const ushort REAR_ENTRY_DOOR_LIGHT = 130;

		public const ushort CEILING_FAN_LIGHT = 131;

		public const ushort OVERHEAD_FAN_LIGHT = 132;

		public const ushort BUNK_SLIDE = 133;

		public const ushort BED_SLIDE = 134;

		public const ushort WARDROBE_SLIDE = 135;

		public const ushort ENTERTAINMENT_SLIDE = 136;

		public const ushort SOFA_SLIDE = 137;

		public const ushort PATIO_AWNING = 138;

		public const ushort REAR_AWNING = 139;

		public const ushort SIDE_AWNING = 140;

		public const ushort JACKS = 141;

		public const ushort LEVELER_2 = 142;

		public const ushort EXTERIOR_LIGHT = 143;

		public const ushort LOWER_ACCENT_LIGHT = 144;

		public const ushort UPPER_ACCENT_LIGHT = 145;

		public const ushort DS_SECURITY_LIGHT = 146;

		public const ushort ODS_SECURITY_LIGHT = 147;

		public const ushort SLIDE_IN_SLIDE = 148;

		public const ushort HITCH_LIGHT = 149;

		public const ushort CLOCK = 150;

		public const ushort TV = 151;

		public const ushort DVD = 152;

		public const ushort BLU_RAY = 153;

		public const ushort VCR = 154;

		public const ushort PVR = 155;

		public const ushort CABLE = 156;

		public const ushort SATELLITE = 157;

		public const ushort AUDIO = 158;

		public const ushort CD_PLAYER = 159;

		public const ushort TUNER = 160;

		public const ushort RADIO = 161;

		public const ushort SPEAKERS = 162;

		public const ushort GAME = 163;

		public const ushort CLOCK_RADIO = 164;

		public const ushort AUX = 165;

		public const ushort CLIMATE_ZONE = 166;

		public const ushort FIREPLACE = 167;

		public const ushort THERMOSTAT = 168;

		public const ushort FRONT_CAP_LIGHT = 169;

		public const ushort STEP_LIGHT = 170;

		public const ushort DS_FLOOD_LIGHT = 171;

		public const ushort INTERIOR_LIGHT = 172;

		public const ushort FRESH_TANK_HEATER = 173;

		public const ushort GREY_TANK_HEATER = 174;

		public const ushort BLACK_TANK_HEATER = 175;

		public const ushort LP_TANK = 176;

		public const ushort STALL_LIGHT = 177;

		public const ushort MAIN_LIGHT = 178;

		public const ushort BATH_LIGHT = 179;

		public const ushort BUNK_LIGHT = 180;

		public const ushort BED_LIGHT = 181;

		public const ushort CABINET_LIGHT = 182;

		public const ushort NETWORK_BRIDGE = 183;

		public const ushort ETHERNET_BRIDGE = 184;

		public const ushort WIFI_BRIDGE = 185;

		public const ushort IN_TRANSIT_POWER_DISCONNECT = 186;

		public const ushort LEVEL_UP_UNITY = 187;

		public const ushort TT_LEVELER = 188;

		public const ushort TRAVEL_TRAILER_LEVELER = 189;

		public const ushort FIFTH_WHEEL_LEVELER = 190;

		public const ushort FUEL_PUMP = 191;

		public const ushort MAIN_CLIMATE_ZONE = 192;

		public const ushort BEDROOM_CLIMATE_ZONE = 193;

		public const ushort GARAGE_CLIMATE_ZONE = 194;

		public const ushort COMPARTMENT_LIGHT = 195;

		public const ushort TRUNK_LIGHT = 196;

		public const ushort BAR_TV = 197;

		public const ushort BATHROOM_TV = 198;

		public const ushort BEDROOM_TV = 199;

		public const ushort BUNK_ROOM_TV = 200;

		public const ushort EXTERIOR_TV = 201;

		public const ushort FRONT_BATHROOM_TV = 202;

		public const ushort FRONT_BEDROOM_TV = 203;

		public const ushort GARAGE_TV = 204;

		public const ushort KITCHEN_TV = 205;

		public const ushort LIVING_ROOM_TV = 206;

		public const ushort LOFT_TV = 207;

		public const ushort LOUNGE_TV = 208;

		public const ushort MAIN_TV = 209;

		public const ushort PATIO_TV = 210;

		public const ushort REAR_BATHROOM_TV = 211;

		public const ushort REAR_BEDROOM_TV = 212;

		public const ushort BATHROOM_DOOR_LOCK = 213;

		public const ushort BEDROOM_DOOR_LOCK = 214;

		public const ushort FRONT_DOOR_LOCK = 215;

		public const ushort GARAGE_DOOR_LOCK = 216;

		public const ushort MAIN_DOOR_LOCK = 217;

		public const ushort PATIO_DOOR_LOCK = 218;

		public const ushort REAR_DOOR_LOCK = 219;

		public const ushort ACCENT_LIGHT = 220;

		public const ushort BATHROOM_ACCENT_LIGHT = 221;

		public const ushort BEDROOM_ACCENT_LIGHT = 222;

		public const ushort FRONT_BEDROOM_ACCENT_LIGHT = 223;

		public const ushort GARAGE_ACCENT_LIGHT = 224;

		public const ushort KITCHEN_ACCENT_LIGHT = 225;

		public const ushort PATIO_ACCENT_LIGHT = 226;

		public const ushort REAR_BEDROOM_ACCENT_LIGHT = 227;

		public const ushort BEDROOM_RADIO = 228;

		public const ushort BUNK_ROOM_RADIO = 229;

		public const ushort EXTERIOR_RADIO = 230;

		public const ushort FRONT_BEDROOM_RADIO = 231;

		public const ushort GARAGE_RADIO = 232;

		public const ushort KITCHEN_RADIO = 233;

		public const ushort LIVING_ROOM_RADIO = 234;

		public const ushort LOFT_RADIO = 235;

		public const ushort PATIO_RADIO = 236;

		public const ushort REAR_BEDROOM_RADIO = 237;

		public const ushort BEDROOM_ENTERTAINMENT_SYSTEM = 238;

		public const ushort BUNK_ROOM_ENTERTAINMENT_SYSTEM = 239;

		public const ushort ENTERTAINMENT_SYSTEM = 240;

		public const ushort EXTERIOR_ENTERTAINMENT_SYSTEM = 241;

		public const ushort FRONT_BEDROOM_ENTERTAINMENT_SYSTEM = 242;

		public const ushort GARAGE_ENTERTAINMENT_SYSTEM = 243;

		public const ushort KITCHEN_ENTERTAINMENT_SYSTEM = 244;

		public const ushort LIVING_ROOM_ENTERTAINMENT_SYSTEM = 245;

		public const ushort LOFT_ENTERTAINMENT_SYSTEM = 246;

		public const ushort MAIN_ENTERTAINMENT_SYSTEM = 247;

		public const ushort PATIO_ENTERTAINMENT_SYSTEM = 248;

		public const ushort REAR_BEDROOM_ENTERTAINMENT_SYSTEM = 249;

		public const ushort LEFT_STABILIZER = 250;

		public const ushort RIGHT_STABILIZER = 251;

		public const ushort STABILIZER = 252;

		public const ushort SOLAR = 253;

		public const ushort SOLAR_POWER = 254;

		public const ushort BATTERY = 255;

		public const ushort MAIN_BATTERY = 256;

		public const ushort AUX_BATTERY = 257;

		public const ushort SHORE_POWER = 258;

		public const ushort AC_POWER = 259;

		public const ushort AC_MAINS = 260;

		public const ushort AUX_POWER = 261;

		public const ushort OUTPUTS = 262;

		public const ushort RAMP_DOOR = 263;

		public const ushort FAN = 264;

		public const ushort BATH_FAN = 265;

		public const ushort REAR_FAN = 266;

		public const ushort FRONT_FAN = 267;

		public const ushort KITCHEN_FAN = 268;

		public const ushort CEILING_FAN = 269;

		public const ushort TANK_HEATER = 270;

		public const ushort FRONT_CEILING_LIGHT = 271;

		public const ushort REAR_CEILING_LIGHT = 272;

		public const ushort CARGO_LIGHT = 273;

		public const ushort FASCIA_LIGHT = 274;

		public const ushort SLIDE_CEILING_LIGHT = 275;

		public const ushort SLIDE_OVERHEAD_LIGHT = 276;

		public const ushort DECOR_LIGHT = 277;

		public const ushort READING_LIGHT = 278;

		public const ushort FRONT_READING_LIGHT = 279;

		public const ushort REAR_READING_LIGHT = 280;

		public const ushort LIVING_ROOM_CLIMATE_ZONE = 281;

		public const ushort FRONT_LIVING_ROOM_CLIMATE_ZONE = 282;

		public const ushort REAR_LIVING_ROOM_CLIMATE_ZONE = 283;

		public const ushort FRONT_BEDROOM_CLIMATE_ZONE = 284;

		public const ushort REAR_BEDROOM_CLIMATE_ZONE = 285;

		public const ushort BED_TILT = 286;

		public const ushort FRONT_BED_TILT = 287;

		public const ushort REAR_BED_TILT = 288;

		public const ushort MENS_LIGHT = 289;

		public const ushort WOMENS_LIGHT = 290;

		public const ushort SERVICE_LIGHT = 291;

		public const ushort ODS_FLOOD_LIGHT = 292;

		public const ushort UNDERBODY_ACCENT_LIGHT = 293;

		public const ushort SPEAKER_LIGHT = 294;

		public const ushort WATER_HEATER = 295;

		public const ushort WATER_HEATERS = 296;

		public const ushort AQUAFI = 297;

		public const ushort CONNECT_ANYWHERE = 298;

		public const ushort SLIDE_IF_EQUIP = 299;

		public const ushort AWNING_IF_EQUIP = 300;

		public const ushort AWNING_LIGHT_IF_EQUIP = 301;

		public const ushort INTERIOR_LIGHT_IF_EQUIP = 302;

		public const ushort WASTE_VALVE = 303;

		public const ushort TIRE_LINC = 304;

		public const ushort FRONT_LOCKER_LIGHT = 305;

		public const ushort REAR_LOCKER_LIGHT = 306;

		public const ushort REAR_AUX_POWER = 307;

		public const ushort ROCK_LIGHT = 308;

		public const ushort CHASSIS_LIGHT = 309;

		public const ushort EXTERIOR_SHOWER_LIGHT = 310;

		public const ushort LIVING_ROOM_ACCENT_LIGHT = 311;

		public const ushort REAR_FLOOD_LIGHT = 312;

		public const ushort PASSENGER_FLOOD_LIGHT = 313;

		public const ushort DRIVER_FLOOD_LIGHT = 314;

		public const ushort BATHROOM_SLIDE = 315;

		public const ushort ROOF_LIFT = 316;

		public const ushort YETI_PACKAGE = 317;

		public const ushort PROPANE_LOCKER = 318;

		public const ushort GARAGE_AWNING = 319;

		public const ushort MONITOR_PANEL = 320;

		public const ushort CAMERA = 321;

		public const ushort JAYCO_AUS_TBB_GW = 322;

		public const ushort GATEWAY_RVLINK = 323;

		public const ushort ACCESSORY_TEMPERATURE = 324;

		public const ushort ACCESSORY_REFRIGERATOR = 325;

		public const ushort ACCESSORY_FRIDGE = 326;

		public const ushort ACCESSORY_FREEZER = 327;

		public const ushort ACCESSORY_EXTERNAL = 328;

		public const ushort TRAILER_BRAKE_CONTROLLER = 329;

		public const ushort TEMP_REFRIGERATOR = 330;

		public const ushort TEMP_REFRIGERATOR_HOME = 331;

		public const ushort TEMP_FREEZER = 332;

		public const ushort TEMP_FREEZER_HOME = 333;

		public const ushort TEMP_COOLER = 334;

		public const ushort TEMP_KITCHEN = 335;

		public const ushort TEMP_LIVING_ROOM = 336;

		public const ushort TEMP_BEDROOM = 337;

		public const ushort TEMP_MASTER_BEDROOM = 338;

		public const ushort TEMP_GARAGE = 339;

		public const ushort TEMP_BASEMENT = 340;

		public const ushort TEMP_BATHROOM = 341;

		public const ushort TEMP_STORAGE_AREA = 342;

		public const ushort TEMP_DRIVERS_AREA = 343;

		public const ushort TEMP_BUNKS = 344;

		public const ushort LP_TANK_RV = 345;

		public const ushort LP_TANK_HOME = 346;

		public const ushort LP_TANK_CABIN = 347;

		public const ushort LP_TANK_BBQ = 348;

		public const ushort LP_TANK_GRILL = 349;

		public const ushort LP_TANK_SUBMARINE = 350;

		public const ushort LP_TANK_OTHER = 351;

		public const ushort ANTI_LOCK_BRAKING_SYSTEM = 352;

		public const ushort LOCAP_GATEWAY = 353;

		public const ushort BOOTLOADER = 354;

		public const ushort AUXILIARY_BATTERY = 355;

		public const ushort CHASSIS_BATTERY = 356;

		public const ushort HOUSE_BATTERY = 357;

		public const ushort KITCHEN_BATTERY = 358;

		public const ushort ELECTRONIC_SWAY_CONTROL = 359;

		public const ushort JACKS_LIGHTS = 360;

		public const ushort AWNING_SENSOR = 361;

		public const ushort INTERIOR_STEP_LIGHT = 362;

		public const ushort EXTERIOR_STEP_LIGHT = 363;

		public const ushort WIFI_BOOSTER = 364;

		public const ushort AUDIBLE_ALERT = 365;

		public const ushort SOFFIT_LIGHT = 366;

		public const ushort BATTERY_BANK = 367;

		public const ushort RV_BATTERY = 368;

		public const ushort SOLAR_BATTERY = 369;

		public const ushort TONGUE_BATTERY = 370;

		public const ushort AXLE1_BRAKECONTROLLER = 371;

		public const ushort AXLE2_BRAKECONTROLLER = 372;

		public const ushort AXLE3_BRAKECONTROLLER = 373;

		public const ushort LEAD_ACID = 374;

		public const ushort LIQUID_LEAD_ACID = 375;

		public const ushort GEL_LEAD_ACID = 376;

		public const ushort AGM_ABSORBENT_GLASS_MAT = 377;

		public const ushort LITHIUM = 378;

		public const ushort FRONT_AWNING = 379;

		public const ushort DINETTE_SLIDE = 380;

		public const ushort HOLDING_TANKS_HEATER = 381;

		public const ushort INVERTER = 382;

		public const ushort BATTERY_HEAT = 383;

		public const ushort CAMERA_POWER = 384;

		public const ushort PATIO_AWNING_LIGHT = 385;

		public const ushort GARAGE_AWNING_LIGHT = 386;

		public const ushort REAR_AWNING_LIGHT = 387;

		public const ushort SIDE_AWNING_LIGHT = 388;

		public const ushort SLIDE_AWNING_LIGHT = 389;

		public const ushort SLIDE_AWNING = 390;

		public const ushort FRONT_AWNING_LIGHT = 391;

		public const ushort CENTRAL_LIGHTS = 392;

		public const ushort RIGHT_SIDE_LIGHTS = 393;

		public const ushort LEFT_SIDE_LIGHTS = 394;

		public const ushort RIGHT_SCENE_LIGHTS = 395;

		public const ushort LEFT_SCENE_LIGHTS = 396;

		public const ushort REAR_SCENE_LIGHTS = 397;

		public const ushort COMPUTER_FAN = 398;

		public const ushort BATTERY_FAN = 399;

		public const ushort RIGHT_SLIDE_ROOM = 400;

		public const ushort LEFT_SLIDE_ROOM = 401;

		public const ushort DUMP_LIGHT = 402;

		public const ushort BASE_CAMP_TOUCHSCREEN = 403;

		public const ushort BASE_CAMP_LEVELER = 404;

		public const ushort REFRIGERATOR = 405;

		public const ushort KITCHEN_PENDANT_LIGHT = 406;

		public const ushort DOOR_SIDE_SOFA_SLIDE = 407;

		public const ushort OFF_DOOR_SIDE_SOFA_SLIDE = 408;

		public const ushort REAR_BED_SLIDE = 409;

		public const ushort THEATER_LIGHTS = 410;

		public const ushort UTILITY_CABINET_LIGHT = 411;

		public const ushort CHASE_LIGHT = 412;

		public const ushort FLOOR_LIGHTS = 413;

		public const ushort RTT_LIGHT = 414;

		public const ushort UPPER_POWER_SHADES = 415;

		public const ushort LOWER_POWER_SHADES = 416;

		public const ushort LIVING_ROOM_POWER_SHADES = 417;

		public const ushort BEDROOM_POWER_SHADES = 418;

		public const ushort BATHROOM_POWER_SHADES = 419;

		public const ushort BUNK_POWER_SHADES = 420;

		public const ushort LOFT_POWER_SHADES = 421;

		public const ushort FRONT_POWER_SHADES = 422;

		public const ushort REAR_POWER_SHADES = 423;

		public const ushort MAIN_POWER_SHADES = 424;

		public const ushort GARAGE_POWER_SHADES = 425;

		public const ushort DOOR_SIDE_POWER_SHADES = 426;

		public const ushort OFF_DOOR_SIDE_POWER_SHADES = 427;

		public const ushort FRESH_TANK_VALVE = 428;

		public const ushort GREY_TANK_VALVE = 429;

		public const ushort BLACK_TANK_VALVE = 430;

		public const ushort FRONT_BATH_GREY_TANK_VALVE = 431;

		public const ushort FRONT_BATH_FRESH_TANK_VALVE = 432;

		public const ushort FRONT_BATH_BLACK_TANK_VALVE = 433;

		public const ushort REAR_BATH_GREY_TANK_VALVE = 434;

		public const ushort REAR_BATH_FRESH_TANK_VALVE = 435;

		public const ushort REAR_BATH_BLACK_TANK_VALVE = 436;

		public const ushort MAIN_BATH_GREY_TANK_VALVE = 437;

		public const ushort MAIN_BATH_FRESH_TANK_VALVE = 438;

		public const ushort MAIN_BATH_BLACK_TANK_VALVE = 439;

		public const ushort GALLEY_BATH_GREY_TANK_VALVE = 440;

		public const ushort GALLEY_BATH_FRESH_TANK_VALVE = 441;

		public const ushort GALLEY_BATH_BLACK_TANK_VALVE = 442;

		public const ushort KITCHEN_BATH_GREY_TANK_VALVE = 443;

		public const ushort KITCHEN_BATH_FRESH_TANK_VALVE = 444;

		public const ushort KITCHEN_BATH_BLACK_TANK_VALVE = 445;

		public readonly ushort Value;

		public readonly string Name;

		public readonly ICON Icon;

		public bool IsValid => this?.Value > 0;

		public static System.Collections.Generic.IEnumerable<FUNCTION_NAME> GetEnumerator()
		{
			return (System.Collections.Generic.IEnumerable<FUNCTION_NAME>)NameList;
		}

		static FUNCTION_NAME()
		{
			Lookup = new Dictionary<ushort, FUNCTION_NAME>();
			NameList = new List<FUNCTION_NAME>();
			UNKNOWN = new FUNCTION_NAME(0, "UNKNOWN", ICON.UNKNOWN);
			new FUNCTION_NAME(1, "Diagnostic Tool", ICON.DIAGNOSTIC_TOOL);
			new FUNCTION_NAME(2, "MyRV Tablet", ICON.TABLET);
			new FUNCTION_NAME(3, "Gas Water Heater", ICON.GAS_WATER_HEATER);
			new FUNCTION_NAME(4, "Electric Water Heater", ICON.ELECTRIC_WATER_HEATER);
			new FUNCTION_NAME(5, "Water Pump", ICON.WATER_PUMP);
			new FUNCTION_NAME(6, "Bath Vent", ICON.VENT);
			new FUNCTION_NAME(7, "Light", ICON.LIGHT);
			new FUNCTION_NAME(8, "Flood Light", ICON.LIGHT);
			new FUNCTION_NAME(9, "Work Light", ICON.LIGHT);
			new FUNCTION_NAME(10, "Front Bedroom Ceiling Light", ICON.LIGHT);
			new FUNCTION_NAME(11, "Front Bedroom Overhead Light", ICON.LIGHT);
			new FUNCTION_NAME(12, "Front Bedroom Vanity Light", ICON.LIGHT);
			new FUNCTION_NAME(13, "Front Bedroom Sconce Light", ICON.LIGHT);
			new FUNCTION_NAME(14, "Front Bedroom Loft Light", ICON.LIGHT);
			new FUNCTION_NAME(15, "Rear Bedroom Ceiling Light", ICON.LIGHT);
			new FUNCTION_NAME(16, "Rear Bedroom Overhead Light", ICON.LIGHT);
			new FUNCTION_NAME(17, "Rear Bedroom Vanity Light", ICON.LIGHT);
			new FUNCTION_NAME(18, "Rear Bedroom Sconce Light", ICON.LIGHT);
			new FUNCTION_NAME(19, "Rear Bedroom Loft Light", ICON.LIGHT);
			new FUNCTION_NAME(20, "Loft Light", ICON.LIGHT);
			new FUNCTION_NAME(21, "Front Hall Light", ICON.LIGHT);
			new FUNCTION_NAME(22, "Rear Hall Light", ICON.LIGHT);
			new FUNCTION_NAME(23, "Front Bathroom Light", ICON.LIGHT);
			new FUNCTION_NAME(24, "Front Bathroom Vanity Light", ICON.LIGHT);
			new FUNCTION_NAME(25, "Front Bathroom Ceiling Light", ICON.LIGHT);
			new FUNCTION_NAME(26, "Front Bathroom Shower Light", ICON.LIGHT);
			new FUNCTION_NAME(27, "Front Bathroom Sconce Light", ICON.LIGHT);
			new FUNCTION_NAME(28, "Rear Bathroom Vanity Light", ICON.LIGHT);
			new FUNCTION_NAME(29, "Rear Bathroom Ceiling Light", ICON.LIGHT);
			new FUNCTION_NAME(30, "Rear Bathroom Shower Light", ICON.LIGHT);
			new FUNCTION_NAME(31, "Rear Bathroom Sconce Light", ICON.LIGHT);
			new FUNCTION_NAME(32, "Kitchen Ceiling Light", ICON.LIGHT);
			new FUNCTION_NAME(33, "Kitchen Sconce Light", ICON.LIGHT);
			new FUNCTION_NAME(34, "Kitchen Pendants Light", ICON.LIGHT);
			new FUNCTION_NAME(35, "Kitchen Range Light", ICON.LIGHT);
			new FUNCTION_NAME(36, "Kitchen Counter Light", ICON.LIGHT);
			new FUNCTION_NAME(37, "Kitchen Bar Light", ICON.LIGHT);
			new FUNCTION_NAME(38, "Kitchen Island Light", ICON.LIGHT);
			new FUNCTION_NAME(39, "Kitchen Chandelier Light", ICON.LIGHT);
			new FUNCTION_NAME(40, "Kitchen Under Cabinet Light", ICON.LIGHT);
			new FUNCTION_NAME(41, "Living Room Ceiling Light", ICON.LIGHT);
			new FUNCTION_NAME(42, "Living Room Sconce Light", ICON.LIGHT);
			new FUNCTION_NAME(43, "Living Room Pendants Light", ICON.LIGHT);
			new FUNCTION_NAME(44, "Living Room Bar Light", ICON.LIGHT);
			new FUNCTION_NAME(45, "Garage Ceiling Light", ICON.LIGHT);
			new FUNCTION_NAME(46, "Garage Cabinet Light", ICON.LIGHT);
			new FUNCTION_NAME(47, "Security Light", ICON.LIGHT);
			new FUNCTION_NAME(48, "Porch Light", ICON.LIGHT);
			new FUNCTION_NAME(49, "Awning Light", ICON.LIGHT);
			new FUNCTION_NAME(50, "Bathroom Light", ICON.LIGHT);
			new FUNCTION_NAME(51, "Bathroom Vanity Light", ICON.LIGHT);
			new FUNCTION_NAME(52, "Bathroom Ceiling Light", ICON.LIGHT);
			new FUNCTION_NAME(53, "Bathroom Shower Light", ICON.LIGHT);
			new FUNCTION_NAME(54, "Bathroom Sconce Light", ICON.LIGHT);
			new FUNCTION_NAME(55, "Hall Light", ICON.LIGHT);
			new FUNCTION_NAME(56, "Bunk Room Light", ICON.LIGHT);
			new FUNCTION_NAME(57, "Bedroom Light", ICON.LIGHT);
			new FUNCTION_NAME(58, "Living Room Light", ICON.LIGHT);
			new FUNCTION_NAME(59, "Kitchen Light", ICON.LIGHT);
			new FUNCTION_NAME(60, "Lounge Light", ICON.LIGHT);
			new FUNCTION_NAME(61, "Ceiling Light", ICON.LIGHT);
			new FUNCTION_NAME(62, "Entry Light", ICON.LIGHT);
			new FUNCTION_NAME(63, "Bed Ceiling Light", ICON.LIGHT);
			new FUNCTION_NAME(64, "Bedroom Lav Light", ICON.LIGHT);
			new FUNCTION_NAME(65, "Shower Light", ICON.LIGHT);
			new FUNCTION_NAME(66, "Galley Light", ICON.LIGHT);
			new FUNCTION_NAME(67, "Fresh Tank", ICON.FRESH_WATER_TANK);
			new FUNCTION_NAME(68, "Grey Tank", ICON.GREY_WATER_TANK);
			new FUNCTION_NAME(69, "Black Tank", ICON.BLACK_WATER_TANK);
			new FUNCTION_NAME(70, "Fuel Tank", ICON.FUEL_TANK);
			new FUNCTION_NAME(71, "Generator Fuel Tank", ICON.FUEL_TANK);
			new FUNCTION_NAME(72, "Auxiliary Fuel Tank", ICON.FUEL_TANK);
			new FUNCTION_NAME(73, "Front Bath Grey Tank", ICON.GREY_WATER_TANK);
			new FUNCTION_NAME(74, "Front Bath Fresh Tank", ICON.FRESH_WATER_TANK);
			new FUNCTION_NAME(75, "Front Bath Black Tank", ICON.BLACK_WATER_TANK);
			new FUNCTION_NAME(76, "Rear Bath Grey Tank", ICON.GREY_WATER_TANK);
			new FUNCTION_NAME(77, "Rear Bath Fresh Tank", ICON.FRESH_WATER_TANK);
			new FUNCTION_NAME(78, "Rear Bath Black Tank", ICON.BLACK_WATER_TANK);
			new FUNCTION_NAME(79, "Main Bath Grey Tank", ICON.GREY_WATER_TANK);
			new FUNCTION_NAME(80, "Main Bath Fresh Tank", ICON.FRESH_WATER_TANK);
			new FUNCTION_NAME(81, "Main Bath Black Tank", ICON.BLACK_WATER_TANK);
			new FUNCTION_NAME(82, "Galley Grey Tank", ICON.GREY_WATER_TANK);
			new FUNCTION_NAME(83, "Galley Fresh Tank", ICON.FRESH_WATER_TANK);
			new FUNCTION_NAME(84, "Galley Black Tank", ICON.BLACK_WATER_TANK);
			new FUNCTION_NAME(85, "Kitchen Grey Tank", ICON.GREY_WATER_TANK);
			new FUNCTION_NAME(86, "Kitchen Fresh Tank", ICON.FRESH_WATER_TANK);
			new FUNCTION_NAME(87, "Kitchen Black Tank", ICON.BLACK_WATER_TANK);
			new FUNCTION_NAME(88, "Landing Gear", ICON.LANDING_GEAR);
			new FUNCTION_NAME(89, "Front Stabilizer", ICON.STABILIZER);
			new FUNCTION_NAME(90, "Rear Stabilizer", ICON.STABILIZER);
			new FUNCTION_NAME(91, "TV Lift", ICON.TV_LIFT);
			new FUNCTION_NAME(92, "Bed Lift", ICON.BED_LIFT);
			new FUNCTION_NAME(93, "Bath Vent Cover", ICON.VENT_COVER);
			new FUNCTION_NAME(94, "Door Lock", ICON.LOCK);
			new FUNCTION_NAME(95, "Generator", ICON.GENERATOR);
			new FUNCTION_NAME(96, "Slide", ICON.SLIDE);
			new FUNCTION_NAME(97, "Main Slide", ICON.SLIDE);
			new FUNCTION_NAME(98, "Bedroom Slide", ICON.SLIDE);
			new FUNCTION_NAME(99, "Galley Slide", ICON.SLIDE);
			new FUNCTION_NAME(100, "Kitchen Slide", ICON.SLIDE);
			new FUNCTION_NAME(101, "Closet Slide", ICON.SLIDE);
			new FUNCTION_NAME(102, "Optional Slide", ICON.SLIDE);
			new FUNCTION_NAME(103, "Door Side Slide", ICON.SLIDE);
			new FUNCTION_NAME(104, "Off-Door Slide", ICON.SLIDE);
			new FUNCTION_NAME(105, "Awning", ICON.AWNING);
			new FUNCTION_NAME(106, "Level Up Leveler", ICON.LEVELER);
			new FUNCTION_NAME(107, "Water Tank Heater", ICON.WATER_TANK_HEATER);
			new FUNCTION_NAME(108, "MyRV Touchscreen", ICON.TOUCHSCREEN_SWITCH);
			new FUNCTION_NAME(109, "Leveler", ICON.LEVELER);
			new FUNCTION_NAME(110, "Vent Cover", ICON.VENT_COVER);
			new FUNCTION_NAME(111, "Front Bedroom Vent Cover", ICON.VENT_COVER);
			new FUNCTION_NAME(112, "Bedroom Vent Cover", ICON.VENT_COVER);
			new FUNCTION_NAME(113, "Front Bath Vent Cover", ICON.VENT_COVER);
			new FUNCTION_NAME(114, "Main Bath Vent Cover", ICON.VENT_COVER);
			new FUNCTION_NAME(115, "Rear Bath Vent Cover", ICON.VENT_COVER);
			new FUNCTION_NAME(116, "Kitchen Vent Cover", ICON.VENT_COVER);
			new FUNCTION_NAME(117, "Living Room Vent Cover", ICON.VENT_COVER);
			new FUNCTION_NAME(118, "4-Leg Truck Camper Leveler", ICON.LEVELER);
			new FUNCTION_NAME(119, "6-Leg Hall Effect EJ Leveler", ICON.LEVELER);
			new FUNCTION_NAME(120, "Patio Light", ICON.LIGHT);
			new FUNCTION_NAME(121, "Hutch Light", ICON.LIGHT);
			new FUNCTION_NAME(122, "Scare Light", ICON.LIGHT);
			new FUNCTION_NAME(123, "Dinette Light", ICON.LIGHT);
			new FUNCTION_NAME(124, "Bar Light", ICON.LIGHT);
			new FUNCTION_NAME(125, "Overhead Light", ICON.LIGHT);
			new FUNCTION_NAME(126, "Overhead Bar Light", ICON.LIGHT);
			new FUNCTION_NAME(127, "Foyer Light", ICON.LIGHT);
			new FUNCTION_NAME(128, "Ramp Door Light", ICON.LIGHT);
			new FUNCTION_NAME(129, "Entertainment Light", ICON.LIGHT);
			new FUNCTION_NAME(130, "Rear Entry Door Light", ICON.LIGHT);
			new FUNCTION_NAME(131, "Ceiling Fan Light", ICON.LIGHT);
			new FUNCTION_NAME(132, "Overhead Fan Light", ICON.LIGHT);
			new FUNCTION_NAME(133, "Bunk Slide", ICON.SLIDE);
			new FUNCTION_NAME(134, "Bed Slide", ICON.SLIDE);
			new FUNCTION_NAME(135, "Wardrobe Slide", ICON.SLIDE);
			new FUNCTION_NAME(136, "Entertainment Slide", ICON.SLIDE);
			new FUNCTION_NAME(137, "Sofa Slide", ICON.SLIDE);
			new FUNCTION_NAME(138, "Patio Awning", ICON.AWNING);
			new FUNCTION_NAME(139, "Rear Awning", ICON.AWNING);
			new FUNCTION_NAME(140, "Side Awning", ICON.AWNING);
			new FUNCTION_NAME(141, "Jacks", ICON.JACKS);
			new FUNCTION_NAME(142, "Leveler", ICON.JACKS);
			new FUNCTION_NAME(143, "Exterior Light", ICON.LIGHT);
			new FUNCTION_NAME(144, "Lower Accent Light", ICON.LIGHT);
			new FUNCTION_NAME(145, "Upper Accent Light", ICON.LIGHT);
			new FUNCTION_NAME(146, "DS Security Light", ICON.LIGHT);
			new FUNCTION_NAME(147, "ODS Security Light", ICON.LIGHT);
			new FUNCTION_NAME(148, "Slide In Slide", ICON.SLIDE);
			new FUNCTION_NAME(149, "Hitch Light", ICON.LIGHT);
			new FUNCTION_NAME(150, "Clock", ICON.CLOCK);
			new FUNCTION_NAME(151, "TV", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(152, "DVD", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(153, "Blu-ray", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(154, "VCR", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(155, "PVR", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(156, "Cable", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(157, "Satellite", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(158, "Audio", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(159, "CD Player", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(160, "Tuner", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(161, "Radio", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(162, "Speakers", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(163, "Game", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(164, "Clock Radio", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(165, "Aux", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(166, "Climate Zone", ICON.HVAC_CONTROL);
			new FUNCTION_NAME(167, "Fireplace", ICON.FIREPLACE);
			new FUNCTION_NAME(168, "Thermostat", ICON.HVAC_CONTROL);
			new FUNCTION_NAME(169, "Front Cap Light", ICON.LIGHT);
			new FUNCTION_NAME(170, "Step Light", ICON.LIGHT);
			new FUNCTION_NAME(171, "DS Flood Light", ICON.LIGHT);
			new FUNCTION_NAME(172, "Interior Light", ICON.LIGHT);
			new FUNCTION_NAME(173, "Fresh Tank Heater", ICON.WATER_TANK_HEATER);
			new FUNCTION_NAME(174, "Grey Tank Heater", ICON.WATER_TANK_HEATER);
			new FUNCTION_NAME(175, "Black Tank Heater", ICON.WATER_TANK_HEATER);
			new FUNCTION_NAME(176, "LP Tank", ICON.GAS_WATER_HEATER);
			new FUNCTION_NAME(177, "Stall Light", ICON.LIGHT);
			new FUNCTION_NAME(178, "Main Light", ICON.LIGHT);
			new FUNCTION_NAME(179, "Bath Light", ICON.LIGHT);
			new FUNCTION_NAME(180, "Bunk Light", ICON.LIGHT);
			new FUNCTION_NAME(181, "Bed Light", ICON.LIGHT);
			new FUNCTION_NAME(182, "Cabinet Light", ICON.LIGHT);
			new FUNCTION_NAME(183, "Network Bridge", ICON.NETWORK_BRIDGE);
			new FUNCTION_NAME(184, "Ethernet Bridge", ICON.NETWORK_BRIDGE);
			new FUNCTION_NAME(185, "WiFi Bridge", ICON.NETWORK_BRIDGE);
			new FUNCTION_NAME(186, "In Transit Power Disconnect", ICON.IPDM);
			new FUNCTION_NAME(187, "Level Up Unity", ICON.LEVELER);
			new FUNCTION_NAME(188, "TT Leveler", ICON.LEVELER);
			new FUNCTION_NAME(189, "Travel Trailer Leveler", ICON.LEVELER);
			new FUNCTION_NAME(190, "Fifth Wheel Leveler", ICON.LEVELER);
			new FUNCTION_NAME(191, "Fuel Pump", ICON.FUEL_PUMP);
			new FUNCTION_NAME(192, "Main Climate Zone", ICON.HVAC_CONTROL);
			new FUNCTION_NAME(193, "Bedroom Climate Zone", ICON.HVAC_CONTROL);
			new FUNCTION_NAME(194, "Garage Climate Zone", ICON.HVAC_CONTROL);
			new FUNCTION_NAME(195, "Compartment Light", ICON.LIGHT);
			new FUNCTION_NAME(196, "Trunk Light", ICON.LIGHT);
			new FUNCTION_NAME(197, "Bar TV", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(198, "Bathroom TV", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(199, "Bedroom TV", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(200, "Bunk Room TV", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(201, "Exterior TV", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(202, "Front Bathroom TV", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(203, "Front Bedroom TV", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(204, "Garage TV", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(205, "Kitchen TV", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(206, "Living Room TV", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(207, "Loft TV", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(208, "Lounge TV", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(209, "Main TV", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(210, "Patio TV", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(211, "Rear Bathroom TV", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(212, "Rear Bedroom TV", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(213, "Bathroom Door Lock", ICON.LOCK);
			new FUNCTION_NAME(214, "Bedroom Door Lock", ICON.LOCK);
			new FUNCTION_NAME(215, "Front Door Lock", ICON.LOCK);
			new FUNCTION_NAME(216, "Garage Door Lock", ICON.LOCK);
			new FUNCTION_NAME(217, "Main Door Lock", ICON.LOCK);
			new FUNCTION_NAME(218, "Patio Door Lock", ICON.LOCK);
			new FUNCTION_NAME(219, "Rear Door Lock", ICON.LOCK);
			new FUNCTION_NAME(220, "Accent Light", ICON.LIGHT);
			new FUNCTION_NAME(221, "Bathroom Accent Light", ICON.LIGHT);
			new FUNCTION_NAME(222, "Bedroom Accent Light", ICON.LIGHT);
			new FUNCTION_NAME(223, "Front Bedroom Accent Light", ICON.LIGHT);
			new FUNCTION_NAME(224, "Garage Accent Light", ICON.LIGHT);
			new FUNCTION_NAME(225, "Kitchen Accent Light", ICON.LIGHT);
			new FUNCTION_NAME(226, "Patio Accent Light", ICON.LIGHT);
			new FUNCTION_NAME(227, "Rear Bedroom Accent Light", ICON.LIGHT);
			new FUNCTION_NAME(228, "Bedroom Radio", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(229, "Bunk Room Radio", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(230, "Exterior Radio", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(231, "Front Bedroom Radio", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(232, "Garage Radio", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(233, "Kitchen Radio", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(234, "Living Room Radio", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(235, "Loft Radio", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(236, "Patio Radio", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(237, "Rear Bedroom Radio", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(238, "Bedroom Entertainment System", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(239, "Bunk Room Entertainment System", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(240, "Entertainment System", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(241, "Exterior Entertainment System", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(242, "Front Bedroom Entertainment System", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(243, "Garage Entertainment System", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(244, "Kitchen Entertainment System", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(245, "Living Room Entertainment System", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(246, "Loft Entertainment System", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(247, "Main Entertainment System", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(248, "Patio Entertainment System", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(249, "Rear Bedroom Entertainment System", ICON.IR_REMOTE_CONTROL);
			new FUNCTION_NAME(250, "Left Stabilizer", ICON.STABILIZER);
			new FUNCTION_NAME(251, "Right Stabilizer", ICON.STABILIZER);
			new FUNCTION_NAME(252, "Stabilizer", ICON.STABILIZER);
			new FUNCTION_NAME(253, "Solar", ICON.POWER_MANAGER);
			new FUNCTION_NAME(254, "Solar Power", ICON.POWER_MANAGER);
			new FUNCTION_NAME(255, "Battery", ICON.POWER_MANAGER);
			new FUNCTION_NAME(256, "Main Battery", ICON.POWER_MANAGER);
			new FUNCTION_NAME(257, "Aux Battery", ICON.POWER_MANAGER);
			new FUNCTION_NAME(258, "Shore Power", ICON.POWER_MANAGER);
			new FUNCTION_NAME(259, "AC Power", ICON.POWER_MANAGER);
			new FUNCTION_NAME(260, "AC Mains", ICON.POWER_MANAGER);
			new FUNCTION_NAME(261, "Aux Power", ICON.POWER_MANAGER);
			new FUNCTION_NAME(262, "Outputs", ICON.POWER_MANAGER);
			new FUNCTION_NAME(263, "Ramp Door", ICON.DOOR);
			new FUNCTION_NAME(264, "Fan", ICON.FAN);
			new FUNCTION_NAME(265, "Bath Fan", ICON.FAN);
			new FUNCTION_NAME(266, "Rear Fan", ICON.FAN);
			new FUNCTION_NAME(267, "Front Fan", ICON.FAN);
			new FUNCTION_NAME(268, "Kitchen Fan", ICON.FAN);
			new FUNCTION_NAME(269, "Ceiling Fan", ICON.FAN);
			new FUNCTION_NAME(270, "Tank Heater", ICON.WATER_TANK_HEATER);
			new FUNCTION_NAME(271, "Front Ceiling Light", ICON.LIGHT);
			new FUNCTION_NAME(272, "Rear Ceiling Light", ICON.LIGHT);
			new FUNCTION_NAME(273, "Cargo Light", ICON.LIGHT);
			new FUNCTION_NAME(274, "Fascia Light", ICON.LIGHT);
			new FUNCTION_NAME(275, "Slide Ceiling Light", ICON.LIGHT);
			new FUNCTION_NAME(276, "Slide Overhead Light", ICON.LIGHT);
			new FUNCTION_NAME(277, "Décor Light", ICON.LIGHT);
			new FUNCTION_NAME(278, "Reading Light", ICON.LIGHT);
			new FUNCTION_NAME(279, "Front Reading Light", ICON.LIGHT);
			new FUNCTION_NAME(280, "Rear Reading Light", ICON.LIGHT);
			new FUNCTION_NAME(281, "Living Room Climate Zone", ICON.HVAC_CONTROL);
			new FUNCTION_NAME(282, "Front Living Room Climate Zone", ICON.HVAC_CONTROL);
			new FUNCTION_NAME(283, "Rear Living Room Climate Zone", ICON.HVAC_CONTROL);
			new FUNCTION_NAME(284, "Front Bedroom Climate Zone", ICON.HVAC_CONTROL);
			new FUNCTION_NAME(285, "Rear Bedroom Climate Zone", ICON.HVAC_CONTROL);
			new FUNCTION_NAME(286, "Bed Tilt", ICON.BED_LIFT);
			new FUNCTION_NAME(287, "Front Bed Tilt", ICON.BED_LIFT);
			new FUNCTION_NAME(288, "Rear Bed Tilt", ICON.BED_LIFT);
			new FUNCTION_NAME(289, "Men's Light", ICON.LIGHT);
			new FUNCTION_NAME(290, "Women's Light", ICON.LIGHT);
			new FUNCTION_NAME(291, "Service Light", ICON.LIGHT);
			new FUNCTION_NAME(292, "ODS Flood Light", ICON.LIGHT);
			new FUNCTION_NAME(293, "Underbody Accent Light", ICON.LIGHT);
			new FUNCTION_NAME(294, "Speaker Light", ICON.LIGHT);
			new FUNCTION_NAME(295, "Water Heater", ICON.WATER_TANK_HEATER);
			new FUNCTION_NAME(296, "Water Heaters", ICON.WATER_TANK_HEATER);
			new FUNCTION_NAME(297, "AquaFi", ICON.GENERIC);
			new FUNCTION_NAME(298, "ConnectAnywhere", ICON.GENERIC);
			new FUNCTION_NAME(299, "Slide {0} (if equip)", ICON.SLIDE);
			new FUNCTION_NAME(300, "Awning {0} (if equip)", ICON.AWNING);
			new FUNCTION_NAME(301, "Awning Light {0} (if equip)", ICON.LIGHT);
			new FUNCTION_NAME(302, "Interior Light {0} (if equip)", ICON.LIGHT);
			new FUNCTION_NAME(303, "Waste valve", ICON.GENERIC);
			new FUNCTION_NAME(304, "Tire Linc", ICON.TPMS);
			new FUNCTION_NAME(305, "Front Locker Light", ICON.LIGHT);
			new FUNCTION_NAME(306, "Rear Locker Light", ICON.LIGHT);
			new FUNCTION_NAME(307, "Rear Aux Power", ICON.POWER_MANAGER);
			new FUNCTION_NAME(308, "Rock Light", ICON.LIGHT);
			new FUNCTION_NAME(309, "Chassis Light", ICON.LIGHT);
			new FUNCTION_NAME(310, "Exterior Shower Light", ICON.LIGHT);
			new FUNCTION_NAME(311, "Living Room Accent Light", ICON.LIGHT);
			new FUNCTION_NAME(312, "Rear Flood Light", ICON.LIGHT);
			new FUNCTION_NAME(313, "Passenger Flood Light", ICON.LIGHT);
			new FUNCTION_NAME(314, "Driver Flood Light", ICON.LIGHT);
			new FUNCTION_NAME(315, "Bathroom Slide", ICON.SLIDE);
			new FUNCTION_NAME(316, "Roof Lift", ICON.SLIDE);
			new FUNCTION_NAME(317, "Yeti Package", ICON.WATER_TANK_HEATER);
			new FUNCTION_NAME(318, "Propane Locker", ICON.WATER_TANK_HEATER);
			new FUNCTION_NAME(319, "Garage Awning", ICON.AWNING);
			new FUNCTION_NAME(320, "Monitor Panel", ICON.TOUCHSCREEN_SWITCH);
			new FUNCTION_NAME(321, "Camera", ICON.GENERIC);
			new FUNCTION_NAME(322, "Jayco Aus TBB GW", ICON.GENERIC);
			new FUNCTION_NAME(323, "GateWay RvLink", ICON.GENERIC);
			new FUNCTION_NAME(324, "Accessory Temperature", ICON.THERMOMETER);
			new FUNCTION_NAME(325, "Accessory Refrigerator", ICON.THERMOMETER);
			new FUNCTION_NAME(326, "Accessory Fridge", ICON.THERMOMETER);
			new FUNCTION_NAME(327, "Accessory Freezer", ICON.THERMOMETER);
			new FUNCTION_NAME(328, "Accessory External", ICON.THERMOMETER);
			new FUNCTION_NAME(329, "Trailer Brake Controller", ICON.GENERIC);
			new FUNCTION_NAME(330, "Refrigerator Temperature", ICON.THERMOMETER);
			new FUNCTION_NAME(331, "Refrigerator Temperature Home", ICON.THERMOMETER);
			new FUNCTION_NAME(332, "Freezer Temperature", ICON.THERMOMETER);
			new FUNCTION_NAME(333, "Freezer Temperature Home", ICON.THERMOMETER);
			new FUNCTION_NAME(334, "Cooler Temperature", ICON.THERMOMETER);
			new FUNCTION_NAME(335, "Kitchen Temperature", ICON.THERMOMETER);
			new FUNCTION_NAME(336, "Living Room Temperature", ICON.THERMOMETER);
			new FUNCTION_NAME(337, "Bedroom Temperature", ICON.THERMOMETER);
			new FUNCTION_NAME(338, "Master Bedroom Temperature", ICON.THERMOMETER);
			new FUNCTION_NAME(339, "Garage Temperature", ICON.GENERIC);
			new FUNCTION_NAME(340, "Basement Temperature", ICON.THERMOMETER);
			new FUNCTION_NAME(341, "Bathroom Temperature", ICON.THERMOMETER);
			new FUNCTION_NAME(342, "Storage Area Temperature", ICON.THERMOMETER);
			new FUNCTION_NAME(343, "Drivers Area Temperature", ICON.THERMOMETER);
			new FUNCTION_NAME(344, "Bunks Temperature", ICON.THERMOMETER);
			new FUNCTION_NAME(345, "RV Tank", ICON.LP_TANK_VALVE);
			new FUNCTION_NAME(346, "Home Tank", ICON.LP_TANK_VALVE);
			new FUNCTION_NAME(347, "Cabin Tank", ICON.LP_TANK_VALVE);
			new FUNCTION_NAME(348, "BBQ Tank", ICON.LP_TANK_VALVE);
			new FUNCTION_NAME(349, "Grill Tank", ICON.LP_TANK_VALVE);
			new FUNCTION_NAME(350, "Submarine Tank", ICON.LP_TANK_VALVE);
			new FUNCTION_NAME(351, "Other Tank", ICON.LP_TANK_VALVE);
			new FUNCTION_NAME(352, "Anti-Lock Braking System", ICON.GENERIC);
			new FUNCTION_NAME(353, "LoCAP Gateway", ICON.GENERIC);
			new FUNCTION_NAME(354, "BootLoader", ICON.GENERIC);
			new FUNCTION_NAME(355, "Auxiliary Battery", ICON.POWER_MONITOR);
			new FUNCTION_NAME(356, "Chassis Battery", ICON.POWER_MONITOR);
			new FUNCTION_NAME(357, "House Battery", ICON.POWER_MONITOR);
			new FUNCTION_NAME(358, "Kitchen Battery", ICON.POWER_MONITOR);
			new FUNCTION_NAME(359, "Electronic Sway Control", ICON.GENERIC);
			new FUNCTION_NAME(360, "Jack Lights", ICON.LIGHT);
			new FUNCTION_NAME(361, "Awning Sensor", ICON.AWNING);
			new FUNCTION_NAME(362, "Interior Step Light", ICON.LIGHT);
			new FUNCTION_NAME(363, "Exterior Step Light", ICON.LIGHT);
			new FUNCTION_NAME(364, "Wifi Booster", ICON.GENERIC);
			new FUNCTION_NAME(365, "Audible Alert", ICON.GENERIC);
			new FUNCTION_NAME(366, "Soffit Light", ICON.LIGHT);
			new FUNCTION_NAME(367, "Battery Bank", ICON.POWER_MANAGER);
			new FUNCTION_NAME(368, "RV Battery", ICON.POWER_MANAGER);
			new FUNCTION_NAME(369, "Solar Battery", ICON.POWER_MANAGER);
			new FUNCTION_NAME(370, "Tongue Battery", ICON.POWER_MANAGER);
			new FUNCTION_NAME(371, "Brake Controller Axle 1", ICON.GENERIC);
			new FUNCTION_NAME(372, "Brake Controller Axle 2", ICON.GENERIC);
			new FUNCTION_NAME(373, "Brake Controller Axle 3", ICON.GENERIC);
			new FUNCTION_NAME(374, "Lead-Acid", ICON.GENERIC);
			new FUNCTION_NAME(375, "Liquid Lead-Acid", ICON.GENERIC);
			new FUNCTION_NAME(376, "Gel Lead-Acid", ICON.GENERIC);
			new FUNCTION_NAME(377, "AGM - Absorbent Glass Mat", ICON.GENERIC);
			new FUNCTION_NAME(378, "Lithium", ICON.GENERIC);
			new FUNCTION_NAME(379, "Front Awning", ICON.AWNING);
			new FUNCTION_NAME(380, "Dinette Slide", ICON.SLIDE);
			new FUNCTION_NAME(381, "Holding Tanks Heater", ICON.GENERIC);
			new FUNCTION_NAME(382, "Inverter", ICON.GENERIC);
			new FUNCTION_NAME(383, "Battery Heat", ICON.THERMOMETER);
			new FUNCTION_NAME(384, "Camera Power", ICON.GENERIC);
			new FUNCTION_NAME(385, "Patio Awning Light", ICON.LIGHT);
			new FUNCTION_NAME(386, "Garage Awning Light", ICON.LIGHT);
			new FUNCTION_NAME(387, "Rear Awning Light", ICON.LIGHT);
			new FUNCTION_NAME(388, "Side Awning Light", ICON.LIGHT);
			new FUNCTION_NAME(389, "Slide Awning Light", ICON.LIGHT);
			new FUNCTION_NAME(390, "Slide Awning", ICON.AWNING);
			new FUNCTION_NAME(391, "Front Awning Light", ICON.LIGHT);
			new FUNCTION_NAME(392, "Central Lights", ICON.LIGHT);
			new FUNCTION_NAME(393, "Right Side Lights", ICON.LIGHT);
			new FUNCTION_NAME(394, "Left Side Lights", ICON.LIGHT);
			new FUNCTION_NAME(395, "Right Scene Lights", ICON.LIGHT);
			new FUNCTION_NAME(396, "Left Scene Lights", ICON.LIGHT);
			new FUNCTION_NAME(397, "Rear Scene Lights", ICON.LIGHT);
			new FUNCTION_NAME(398, "Computer Fan", ICON.FAN);
			new FUNCTION_NAME(399, "Battery Fan", ICON.FAN);
			new FUNCTION_NAME(400, "Right Slide Room", ICON.SLIDE);
			new FUNCTION_NAME(401, "Left Slide Room", ICON.SLIDE);
			new FUNCTION_NAME(402, "Dump Light", ICON.LIGHT);
			new FUNCTION_NAME(403, "Base Camp Touchscreen", ICON.TOUCHSCREEN_SWITCH);
			new FUNCTION_NAME(404, "Base Camp Leveler", ICON.LEVELER);
			new FUNCTION_NAME(405, "Refrigerator", ICON.GENERIC);
			new FUNCTION_NAME(406, "Kitchen Pendant Light", ICON.LIGHT);
			new FUNCTION_NAME(407, "Door Side Sofa Slide", ICON.SLIDE);
			new FUNCTION_NAME(408, "Off Door Side Sofa Slide", ICON.SLIDE);
			new FUNCTION_NAME(409, "Rear Bed Slide", ICON.SLIDE);
			new FUNCTION_NAME(410, "Theater Lights", ICON.LIGHT);
			new FUNCTION_NAME(411, "Utility Cabinet Light", ICON.LIGHT);
			new FUNCTION_NAME(412, "Chase Light", ICON.LIGHT);
			new FUNCTION_NAME(413, "Floor Lights", ICON.LIGHT);
			new FUNCTION_NAME(414, "Roof Top Tent Light", ICON.LIGHT);
			new FUNCTION_NAME(415, "Upper Power Shades", ICON.GENERIC);
			new FUNCTION_NAME(416, "Lower Power Shades", ICON.GENERIC);
			new FUNCTION_NAME(417, "Living Room Power Shades", ICON.GENERIC);
			new FUNCTION_NAME(418, "Bedroom Power Shades", ICON.GENERIC);
			new FUNCTION_NAME(419, "Bathroom Power Shades", ICON.GENERIC);
			new FUNCTION_NAME(420, "Bunk Power Shades", ICON.GENERIC);
			new FUNCTION_NAME(421, "Loft Power Shades", ICON.GENERIC);
			new FUNCTION_NAME(422, "Front Power Shades", ICON.GENERIC);
			new FUNCTION_NAME(423, "Rear Power Shades", ICON.GENERIC);
			new FUNCTION_NAME(424, "Main Power Shades", ICON.GENERIC);
			new FUNCTION_NAME(425, "Garage Power Shades", ICON.GENERIC);
			new FUNCTION_NAME(426, "Door Side Power Shades", ICON.GENERIC);
			new FUNCTION_NAME(427, "Off Door Side Power Shades", ICON.GENERIC);
			new FUNCTION_NAME(428, "Fresh Tank Valve", ICON.FRESH_WATER_TANK);
			new FUNCTION_NAME(429, "Grey Tank Valve", ICON.GREY_WATER_TANK);
			new FUNCTION_NAME(430, "Black Tank Valve", ICON.BLACK_WATER_TANK);
			new FUNCTION_NAME(431, "Front Bath Grey Tank Valve", ICON.GREY_WATER_TANK);
			new FUNCTION_NAME(432, "Front Bath Fresh Tank Valve", ICON.FRESH_WATER_TANK);
			new FUNCTION_NAME(433, "Front Bath Black Tank Valve", ICON.BLACK_WATER_TANK);
			new FUNCTION_NAME(434, "Rear Bath Grey Tank Valve", ICON.GREY_WATER_TANK);
			new FUNCTION_NAME(435, "Rear Bath Fresh Tank Valve", ICON.FRESH_WATER_TANK);
			new FUNCTION_NAME(436, "Rear Bath Black Tank Valve", ICON.BLACK_WATER_TANK);
			new FUNCTION_NAME(437, "Main Bath Grey Tank Valve", ICON.GREY_WATER_TANK);
			new FUNCTION_NAME(438, "Main Bath Fresh Tank Valve", ICON.FRESH_WATER_TANK);
			new FUNCTION_NAME(439, "Main Bath Black Tank Valve", ICON.BLACK_WATER_TANK);
			new FUNCTION_NAME(440, "Galley Grey Tank Valve", ICON.GREY_WATER_TANK);
			new FUNCTION_NAME(441, "Galley Fresh Tank Valve", ICON.FRESH_WATER_TANK);
			new FUNCTION_NAME(442, "Galley Black Tank Valve", ICON.BLACK_WATER_TANK);
			new FUNCTION_NAME(443, "Kitchen Grey Tank Valve", ICON.GREY_WATER_TANK);
			new FUNCTION_NAME(444, "Kitchen Fresh Tank Valve", ICON.FRESH_WATER_TANK);
			new FUNCTION_NAME(445, "Kitchen Black Tank Valve", ICON.BLACK_WATER_TANK);
		}

		private FUNCTION_NAME(ushort value)
			: this(value, "UNKNOWN_" + value.ToString("X4"), ICON.UNKNOWN)
		{
		}

		private FUNCTION_NAME(ushort value, string name, ICON icon)
		{
			Value = value;
			Name = name.Trim();
			Icon = icon;
			if (value > 0)
			{
				Lookup.Add(value, this);
				NameList.Add(this);
			}
		}

		public static implicit operator ushort(FUNCTION_NAME msg)
		{
			return msg?.Value ?? 0;
		}

		public static implicit operator FUNCTION_NAME(ushort value)
		{
			FUNCTION_NAME result = default(FUNCTION_NAME);
			if (Lookup.TryGetValue(value, ref result))
			{
				return result;
			}
			if (value == 0)
			{
				return UNKNOWN;
			}
			return new FUNCTION_NAME(value);
		}

		public static implicit operator FUNCTION_NAME(string s)
		{
			s = s.ToUpper().Trim();
			System.Collections.Generic.IEnumerator<FUNCTION_NAME> enumerator = GetEnumerator().GetEnumerator();
			try
			{
				while (((System.Collections.IEnumerator)enumerator).MoveNext())
				{
					FUNCTION_NAME current = enumerator.Current;
					if (((object)current).ToString().ToUpper() == s)
					{
						return current;
					}
				}
			}
			finally
			{
				((System.IDisposable)enumerator)?.Dispose();
			}
			return UNKNOWN;
		}

		public string ToString()
		{
			return Name;
		}
	}
	public sealed class PID
	{
		private enum FORMAT
		{
			CHAR1,
			CHAR2,
			CHAR3,
			CHAR4,
			CHAR5,
			CHAR6,
			INT8,
			UINT8,
			INT16,
			UINT16,
			INT24,
			UINT24,
			INT32,
			UINT32,
			INT40,
			UINT40,
			INT48,
			UINT48,
			DATE_TIME,
			DATE_TIME_EPOCH,
			IPV4,
			IPV6,
			MAC2,
			MAC6,
			ADDRESS16_DATA32,
			BYTE1,
			BYTE2,
			BYTE3,
			BYTE4,
			BYTE5,
			BYTE6
		}

		private class Formatter
		{
			private readonly FORMAT Format;

			private readonly string FormatString;

			private readonly double Scale;

			private readonly double Offset;

			internal Formatter(FORMAT f)
			{
				Format = f;
			}

			internal Formatter(FORMAT f, string s)
			{
				Format = f;
				FormatString = s;
			}

			internal Formatter(FORMAT f, string s, double scale)
			{
				Format = f;
				FormatString = s;
				Scale = scale;
			}

			internal Formatter(FORMAT f, string s, double scale, double offset)
			{
				Format = f;
				FormatString = s;
				Scale = scale;
				Offset = offset;
			}

			public string ToString(ulong value)
			{
				switch (Format)
				{
				case FORMAT.UINT8:
					value &= 0xFF;
					break;
				case FORMAT.UINT16:
					value &= 0xFFFF;
					break;
				case FORMAT.UINT24:
					value &= 0xFFFFFF;
					break;
				case FORMAT.UINT32:
					value &= 0xFFFFFFFFu;
					break;
				case FORMAT.UINT40:
					value &= 0xFFFFFFFFFFL;
					break;
				case FORMAT.UINT48:
					value &= 0xFFFFFFFFFFFFL;
					break;
				case FORMAT.INT8:
					value &= 0xFF;
					if ((value & 0x80) != 0L)
					{
						value |= 0xFFFFFFFFFFFFFF00uL;
					}
					break;
				case FORMAT.INT16:
					value &= 0xFFFF;
					if ((value & 0x8000) != 0L)
					{
						value |= 0xFFFFFFFFFFFF0000uL;
					}
					break;
				case FORMAT.INT24:
					value &= 0xFFFFFF;
					if ((value & 0x800000) != 0L)
					{
						value |= 0xFFFFFFFFFF000000uL;
					}
					break;
				case FORMAT.INT32:
					value &= 0xFFFFFFFFu;
					if ((value & 0x80000000u) != 0L)
					{
						value |= 0xFFFFFFFF00000000uL;
					}
					break;
				case FORMAT.INT40:
					value &= 0xFFFFFFFFFFL;
					if ((value & 0x8000000000L) != 0L)
					{
						value |= 0xFFFFFF0000000000uL;
					}
					break;
				case FORMAT.INT48:
					value &= 0xFFFFFFFFFFFFL;
					if ((value & 0x800000000000L) != 0L)
					{
						value |= 0xFFFF000000000000uL;
					}
					break;
				case FORMAT.CHAR1:
					return CharString(value, 1);
				case FORMAT.CHAR2:
					return CharString(value, 2);
				case FORMAT.CHAR3:
					return CharString(value, 3);
				case FORMAT.CHAR4:
					return CharString(value, 4);
				case FORMAT.CHAR5:
					return CharString(value, 5);
				case FORMAT.CHAR6:
					return CharString(value, 6);
				case FORMAT.BYTE1:
					return ByteString(value, 1);
				case FORMAT.BYTE2:
					return ByteString(value, 2);
				case FORMAT.BYTE3:
					return ByteString(value, 3);
				case FORMAT.BYTE4:
					return ByteString(value, 4);
				case FORMAT.BYTE5:
					return ByteString(value, 5);
				case FORMAT.BYTE6:
					return ByteString(value, 6);
				case FORMAT.DATE_TIME:
				{
					int num = 2000 + (byte)(value >> 40);
					int num2 = (byte)(value >> 32);
					int num3 = (byte)(value >> 24);
					int num4 = (byte)(value >> 16);
					int num5 = (byte)(value >> 8);
					int num6 = (byte)value;
					try
					{
						return new System.DateTime(num, num2, num3, num4, num5, num6).ToString();
					}
					catch (ArgumentOutOfRangeException)
					{
						return "00/00/2000 00:00:00 AM";
					}
				}
				case FORMAT.DATE_TIME_EPOCH:
					return new System.DateTime(2000, 1, 1, 0, 0, 0, (DateTimeKind)1).AddSeconds((double)value).ToString();
				case FORMAT.IPV4:
					return IPString(value, 4);
				case FORMAT.IPV6:
					return IPString(value, 6);
				case FORMAT.MAC2:
					return MACstring(value, 2);
				case FORMAT.MAC6:
					return MACstring(value, 6);
				case FORMAT.ADDRESS16_DATA32:
					return "Address: $" + ((ushort)((value >> 32) & 0xFFFF)).ToString("X4") + " Read data: $" + ((uint)(value & 0xFFFFFFFFu)).ToString("X8");
				}
				long num7 = (long)value;
				if (Scale == 0.0 && Offset == 0.0)
				{
					if (FormatString == null)
					{
						return num7.ToString();
					}
					return string.Format(FormatString, (object)num7);
				}
				double num8 = (double)num7 * 1.0;
				if (Scale != 0.0)
				{
					num8 *= Scale;
				}
				num8 += Offset;
				if (FormatString == null)
				{
					return num8.ToString();
				}
				return string.Format(FormatString, (object)num8);
			}

			private string ByteString(ulong value, int digits)
			{
				string text = "";
				if (digits < 1)
				{
					digits = 1;
				}
				if (digits > 8)
				{
					digits = 8;
				}
				int num = 8 - digits;
				value <<= num * 8;
				for (int i = 0; i < digits; i++)
				{
					if (i != 0)
					{
						text += " ";
					}
					byte b = (byte)(value >> 56);
					value <<= 8;
					string text2 = text;
					byte b2 = b;
					text = text2 + b2.ToString("X2");
				}
				return text;
			}

			private string CharString(ulong value, int digits)
			{
				string text = "";
				if (digits < 1)
				{
					digits = 1;
				}
				if (digits > 8)
				{
					digits = 8;
				}
				int num = 8 - digits;
				value <<= num * 8;
				for (int i = 0; i < digits; i++)
				{
					char c = (char)(value >> 56);
					value <<= 8;
					if (c == '\0')
					{
						break;
					}
					text += c;
				}
				return text;
			}

			private string IPString(ulong value, int digits)
			{
				string text = "";
				int num = 0;
				while (num < digits)
				{
					if (num != 0)
					{
						text = "." + text;
					}
					text = (byte)value + text;
					num++;
					value >>= 8;
				}
				return text;
			}

			private string MACstring(ulong value, int digits)
			{
				string text = "";
				int num = 0;
				while (num < digits)
				{
					if (num != 0)
					{
						text = ":" + text;
					}
					text = ((byte)value).ToString("X2") + text;
					num++;
					value >>= 8;
				}
				return text;
			}
		}

		private static readonly Dictionary<ushort, PID> Lookup = new Dictionary<ushort, PID>();

		private static readonly List<PID> List = new List<PID>();

		public static readonly PID UNKNOWN = new PID(0, "UNKNOWN", new Formatter(FORMAT.UINT48, "${0:X12}"), 0);

		public static readonly PID PRODUCTION_BYTES = new PID(1, "PRODUCTION_BYTES", new Formatter(FORMAT.UINT48, "${0:X8}"), 1);

		public static readonly PID CAN_ADAPTER_MAC = new PID(2, "CAN_ADAPTER_MAC", new Formatter(FORMAT.MAC6), 1);

		public static readonly PID IDS_CAN_CIRCUIT_ID = new PID(3, "IDS_CAN_CIRCUIT_ID", new Formatter(FORMAT.UINT32, "${0:X8}"), 2);

		public static readonly PID IDS_CAN_FUNCTION_NAME = new PID(4, "IDS_CAN_FUNCTION_NAME", new Formatter(FORMAT.UINT16, "${0:X4}"), 2);

		public static readonly PID IDS_CAN_FUNCTION_INSTANCE = new PID(5, "IDS_CAN_FUNCTION_INSTANCE", new Formatter(FORMAT.UINT8, "${0:X2}"), 2);

		public static readonly PID IDS_CAN_NUM_DEVICES_ON_NETWORK = new PID(6, "IDS_CAN_NUM_DEVICES_ON_NETWORK", new Formatter(FORMAT.UINT48), 0);

		public static readonly PID IDS_CAN_MAX_NETWORK_HEARTBEAT_TIME = new PID(7, "IDS_CAN_MAX_NETWORK_HEARTBEAT_TIME", new Formatter(FORMAT.UINT48), 0);

		public static readonly PID SERIAL_NUMBER = new PID(8, "SERIAL_NUMBER", new Formatter(FORMAT.UINT48, "${0:X12}"), 1);

		public static readonly PID CAN_BYTES_TX = new PID(9, "CAN_BYTES_TX", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID CAN_BYTES_RX = new PID(10, "CAN_BYTES_RX", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID CAN_MESSAGES_TX = new PID(11, "CAN_MESSAGES_TX", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID CAN_MESSAGES_RX = new PID(12, "CAN_MESSAGES_RX", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID CAN_TX_BUFFER_OVERFLOW_COUNT = new PID(13, "CAN_TX_BUFFER_OVERFLOW_COUNT", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID CAN_RX_BUFFER_OVERFLOW_COUNT = new PID(14, "CAN_RX_BUFFER_OVERFLOW_COUNT", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID CAN_TX_MAX_BYTES_QUEUED = new PID(15, "CAN_TX_MAX_BYTES_QUEUED", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID CAN_RX_MAX_BYTES_QUEUED = new PID(16, "CAN_RX_MAX_BYTES_QUEUED", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID UART_BYTES_TX = new PID(17, "UART_BYTES_TX", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID UART_BYTES_RX = new PID(18, "UART_BYTES_RX", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID UART_MESSAGES_TX = new PID(19, "UART_MESSAGES_TX", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID UART_MESSAGES_RX = new PID(20, "UART_MESSAGES_RX", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID UART_TX_BUFFER_OVERFLOW_COUNT = new PID(21, "UART_TX_BUFFER_OVERFLOW_COUNT", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID UART_RX_BUFFER_OVERFLOW_COUNT = new PID(22, "UART_RX_BUFFER_OVERFLOW_COUNT", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID UART_TX_MAX_BYTES_QUEUED = new PID(23, "UART_TX_MAX_BYTES_QUEUED", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID UART_RX_MAX_BYTES_QUEUED = new PID(24, "UART_RX_MAX_BYTES_QUEUED", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID WIFI_BYTES_TX = new PID(25, "WIFI_BYTES_TX", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID WIFI_BYTES_RX = new PID(26, "WIFI_BYTES_RX", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID WIFI_MESSAGES_TX = new PID(27, "WIFI_MESSAGES_TX", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID WIFI_MESSAGES_RX = new PID(28, "WIFI_MESSAGES_RX", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID WIFI_TX_BUFFER_OVERFLOW_COUNT = new PID(29, "WIFI_TX_BUFFER_OVERFLOW_COUNT", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID WIFI_RX_BUFFER_OVERFLOW_COUNT = new PID(30, "WIFI_RX_BUFFER_OVERFLOW_COUNT", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID WIFI_TX_MAX_BYTES_QUEUED = new PID(31, "WIFI_TX_MAX_BYTES_QUEUED", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID WIFI_RX_MAX_BYTES_QUEUED = new PID(32, "WIFI_RX_MAX_BYTES_QUEUED", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID WIFI_RSSI = new PID(33, "WIFI_RSSI", new Formatter(FORMAT.INT32, "{0:0.###} dBm", 1.52587890625E-05), 0);

		public static readonly PID RF_BYTES_TX = new PID(34, "RF_BYTES_TX", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID RF_BYTES_RX = new PID(35, "RF_BYTES_RX", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID RF_MESSAGES_TX = new PID(36, "RF_MESSAGES_TX", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID RF_MESSAGES_RX = new PID(37, "RF_MESSAGES_RX", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID RF_TX_BUFFER_OVERFLOW_COUNT = new PID(38, "RF_TX_BUFFER_OVERFLOW_COUNT", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID RF_RX_BUFFER_OVERFLOW_COUNT = new PID(39, "RF_RX_BUFFER_OVERFLOW_COUNT", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID RF_TX_MAX_BYTES_QUEUED = new PID(40, "RF_TX_MAX_BYTES_QUEUED", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID RF_RX_MAX_BYTES_QUEUED = new PID(41, "RF_RX_MAX_BYTES_QUEUED", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID RF_RSSI = new PID(42, "RF_RSSI", new Formatter(FORMAT.INT32, "{0:0.###} dBm", 1.52587890625E-05), 0);

		public static readonly PID BATTERY_VOLTAGE = new PID(43, "BATTERY_VOLTAGE", new Formatter(FORMAT.UINT32, "{0:0.###} V", 1.52587890625E-05), 0);

		public static readonly PID REGULATOR_VOLTAGE = new PID(44, "REGULATOR_VOLTAGE", new Formatter(FORMAT.UINT32, "{0:0.###} V", 1.52587890625E-05), 0);

		public static readonly PID NUM_TILT_SENSOR_AXES = new PID(45, "NUM_TILT_SENSOR_AXES", new Formatter(FORMAT.UINT32, "{0:#,###0}"), 0);

		public static readonly PID TILT_AXIS_1_ANGLE = new PID(46, "TILT_AXIS_1_ANGLE", new Formatter(FORMAT.INT32, "{0:0.###}°", 1.52587890625E-05), 0);

		public static readonly PID TILT_AXIS_2_ANGLE = new PID(47, "TILT_AXIS_2_ANGLE", new Formatter(FORMAT.INT32, "{0:0.###}°", 1.52587890625E-05), 0);

		public static readonly PID TILT_AXIS_3_ANGLE = new PID(48, "TILT_AXIS_3_ANGLE", new Formatter(FORMAT.INT32, "{0:0.###}°", 1.52587890625E-05), 0);

		public static readonly PID TILT_AXIS_4_ANGLE = new PID(49, "TILT_AXIS_4_ANGLE", new Formatter(FORMAT.INT32, "{0:0.###}°", 1.52587890625E-05), 0);

		public static readonly PID TILT_AXIS_5_ANGLE = new PID(50, "TILT_AXIS_5_ANGLE", new Formatter(FORMAT.INT32, "{0:0.###}°", 1.52587890625E-05), 0);

		public static readonly PID TILT_AXIS_6_ANGLE = new PID(51, "TILT_AXIS_6_ANGLE", new Formatter(FORMAT.INT32, "{0:0.###}°", 1.52587890625E-05), 0);

		public static readonly PID TILT_AXIS_7_ANGLE = new PID(52, "TILT_AXIS_7_ANGLE", new Formatter(FORMAT.INT32, "{0:0.###}°", 1.52587890625E-05), 0);

		public static readonly PID TILT_AXIS_8_ANGLE = new PID(53, "TILT_AXIS_8_ANGLE", new Formatter(FORMAT.INT32, "{0:0.###}°", 1.52587890625E-05), 0);

		public static readonly PID IDS_CAN_FIXED_ADDRESS = new PID(54, "IDS_CAN_FIXED_ADDRESS", new Formatter(FORMAT.UINT8, "${0:X2}"), 1);

		public static readonly PID FUSE_SETTING_1 = new PID(55, "FUSE_SETTING_1", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID FUSE_SETTING_2 = new PID(56, "FUSE_SETTING_2", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID FUSE_SETTING_3 = new PID(57, "FUSE_SETTING_3", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID FUSE_SETTING_4 = new PID(58, "FUSE_SETTING_4", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID FUSE_SETTING_5 = new PID(59, "FUSE_SETTING_5", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID FUSE_SETTING_6 = new PID(60, "FUSE_SETTING_6", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID FUSE_SETTING_7 = new PID(61, "FUSE_SETTING_7", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID FUSE_SETTING_8 = new PID(62, "FUSE_SETTING_8", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID FUSE_SETTING_9 = new PID(63, "FUSE_SETTING_9", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID FUSE_SETTING_10 = new PID(64, "FUSE_SETTING_10", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID FUSE_SETTING_11 = new PID(65, "FUSE_SETTING_11", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID FUSE_SETTING_12 = new PID(66, "FUSE_SETTING_12", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID FUSE_SETTING_13 = new PID(67, "FUSE_SETTING_13", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID FUSE_SETTING_14 = new PID(68, "FUSE_SETTING_14", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID FUSE_SETTING_15 = new PID(69, "FUSE_SETTING_15", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID FUSE_SETTING_16 = new PID(70, "FUSE_SETTING_16", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID MANUFACTURING_PID_1 = new PID(71, "MANUFACTURING_PID_1", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID MANUFACTURING_PID_2 = new PID(72, "MANUFACTURING_PID_2", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID MANUFACTURING_PID_3 = new PID(73, "MANUFACTURING_PID_3", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID MANUFACTURING_PID_4 = new PID(74, "MANUFACTURING_PID_4", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID MANUFACTURING_PID_5 = new PID(75, "MANUFACTURING_PID_5", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID MANUFACTURING_PID_6 = new PID(76, "MANUFACTURING_PID_6", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID MANUFACTURING_PID_7 = new PID(77, "MANUFACTURING_PID_7", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID MANUFACTURING_PID_8 = new PID(78, "MANUFACTURING_PID_8", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID MANUFACTURING_PID_9 = new PID(79, "MANUFACTURING_PID_9", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID MANUFACTURING_PID_10 = new PID(80, "MANUFACTURING_PID_10", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID MANUFACTURING_PID_11 = new PID(81, "MANUFACTURING_PID_11", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID MANUFACTURING_PID_12 = new PID(82, "MANUFACTURING_PID_12", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID MANUFACTURING_PID_13 = new PID(83, "MANUFACTURING_PID_13", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID MANUFACTURING_PID_14 = new PID(84, "MANUFACTURING_PID_14", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID MANUFACTURING_PID_15 = new PID(85, "MANUFACTURING_PID_15", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID MANUFACTURING_PID_16 = new PID(86, "MANUFACTURING_PID_16", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID MANUFACTURING_PID_17 = new PID(87, "MANUFACTURING_PID_17", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID MANUFACTURING_PID_18 = new PID(88, "MANUFACTURING_PID_18", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID MANUFACTURING_PID_19 = new PID(89, "MANUFACTURING_PID_19", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID MANUFACTURING_PID_20 = new PID(90, "MANUFACTURING_PID_20", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID MANUFACTURING_PID_21 = new PID(91, "MANUFACTURING_PID_21", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID MANUFACTURING_PID_22 = new PID(92, "MANUFACTURING_PID_22", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID MANUFACTURING_PID_23 = new PID(93, "MANUFACTURING_PID_23", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID MANUFACTURING_PID_24 = new PID(94, "MANUFACTURING_PID_24", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID MANUFACTURING_PID_25 = new PID(95, "MANUFACTURING_PID_25", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID MANUFACTURING_PID_26 = new PID(96, "MANUFACTURING_PID_26", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID MANUFACTURING_PID_27 = new PID(97, "MANUFACTURING_PID_27", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID MANUFACTURING_PID_28 = new PID(98, "MANUFACTURING_PID_28", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID MANUFACTURING_PID_29 = new PID(99, "MANUFACTURING_PID_29", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID MANUFACTURING_PID_30 = new PID(100, "MANUFACTURING_PID_30", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID MANUFACTURING_PID_31 = new PID(101, "MANUFACTURING_PID_31", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID MANUFACTURING_PID_32 = new PID(102, "MANUFACTURING_PID_32", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID METERED_TIME_SEC = new PID(103, "METERED_TIME_SEC", new Formatter(FORMAT.UINT32, "${0:X8}"), 2);

		public static readonly PID MAINTENANCE_PERIOD_SEC = new PID(104, "MAINTENANCE_PERIOD_SEC", new Formatter(FORMAT.UINT32, "${0:X8}"), 2);

		public static readonly PID LAST_MAINTENANCE_TIME_SEC = new PID(105, "LAST_MAINTENANCE_TIME_SEC", new Formatter(FORMAT.UINT32, "${0:X8}"), 2);

		public static readonly PID TIME_ZONE = new PID(106, "TIME_ZONE", new Formatter(FORMAT.UINT8, "${0:X2}"), 2);

		public static readonly PID RTC_TIME_SEC = new PID(107, "RTC_TIME_SEC", new Formatter(FORMAT.UINT8, "${0:X2}"), 2);

		public static readonly PID RTC_TIME_MIN = new PID(108, "RTC_TIME_MIN", new Formatter(FORMAT.UINT8, "${0:X2}"), 2);

		public static readonly PID RTC_TIME_HOUR = new PID(109, "RTC_TIME_HOUR", new Formatter(FORMAT.UINT8, "${0:X2}"), 2);

		public static readonly PID RTC_TIME_DAY = new PID(110, "RTC_TIME_DAY", new Formatter(FORMAT.UINT8, "${0:X2}"), 2);

		public static readonly PID RTC_TIME_MONTH = new PID(111, "RTC_TIME_MONTH", new Formatter(FORMAT.UINT8, "${0:X2}"), 2);

		public static readonly PID RTC_TIME_YEAR = new PID(112, "RTC_TIME_YEAR", new Formatter(FORMAT.UINT16, "${0:X4}"), 2);

		public static readonly PID RTC_EPOCH_SEC = new PID(113, "RTC_EPOCH_SEC", new Formatter(FORMAT.DATE_TIME_EPOCH), 2);

		public static readonly PID RTC_SET_TIME_SEC = new PID(114, "RTC_SET_TIME_SEC", new Formatter(FORMAT.DATE_TIME_EPOCH), 2);

		public static readonly PID BLE_DEVICE_NAME_1 = new PID(115, "BLE_DEVICE_NAME_1", new Formatter(FORMAT.CHAR6), 1);

		public static readonly PID BLE_DEVICE_NAME_2 = new PID(116, "BLE_DEVICE_NAME_2", new Formatter(FORMAT.CHAR6), 1);

		public static readonly PID BLE_DEVICE_NAME_3 = new PID(117, "BLE_DEVICE_NAME_3", new Formatter(FORMAT.CHAR6), 1);

		public static readonly PID BLE_PIN = new PID(118, "BLE_PIN", new Formatter(FORMAT.CHAR6), 1);

		public static readonly PID SYSTEM_UPTIME_MS = new PID(119, "SYSTEM_UPTIME_MS", new Formatter(FORMAT.UINT48, "{0:0.###} sec", 0.001), 0);

		public static readonly PID ETH_ADAPTER_MAC = new PID(120, "ETH_ADAPTER_MAC", new Formatter(FORMAT.UINT48, "${0:X12}"), 1);

		public static readonly PID ETH_BYTES_TX = new PID(121, "ETH_BYTES_TX", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID ETH_BYTES_RX = new PID(122, "ETH_BYTES_RX", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID ETH_MESSAGES_TX = new PID(123, "ETH_MESSAGES_TX", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID ETH_MESSAGES_RX = new PID(124, "ETH_MESSAGES_RX", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID ETH_TX_BUFFER_OVERFLOW_COUNT = new PID(125, "ETH_TX_BUFFER_OVERFLOW_COUNT", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID ETH_RX_BUFFER_OVERFLOW_COUNT = new PID(126, "ETH_RX_BUFFER_OVERFLOW_COUNT", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID ETH_PACKETS_TX_DISCARDED = new PID(127, "ETH_PACKETS_TX_DISCARDED", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID ETH_PACKETS_RX_DISCARDED = new PID(128, "ETH_PACKETS_RX_DISCARDED", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID ETH_PACKETS_TX_ERROR = new PID(129, "ETH_PACKETS_TX_ERROR", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID ETH_PACKETS_RX_ERROR = new PID(130, "ETH_PACKETS_RX_ERROR", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID ETH_PACKETS_TX_OVERFLOW = new PID(131, "ETH_PACKETS_TX_OVERFLOW", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID ETH_PACKETS_TX_LATE_COLLISION = new PID(132, "ETH_PACKETS_TX_LATE_COLLISION", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID ETH_PACKETS_TX_EXCESS_COLLISION = new PID(133, "ETH_PACKETS_TX_EXCESS_COLLISION", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID ETH_PACKETS_TX_UNDERFLOW = new PID(134, "ETH_PACKETS_TX_UNDERFLOW", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID ETH_PACKETS_RX_ALIGN_ERR = new PID(135, "ETH_PACKETS_RX_ALIGN_ERR", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID ETH_PACKETS_RX_CRC_ERR = new PID(136, "ETH_PACKETS_RX_CRC_ERR", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID ETH_PACKETS_RX_TRUNC_ERR = new PID(137, "ETH_PACKETS_RX_TRUNC_ERR", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID ETH_PACKETS_RX_LEN_ERR = new PID(138, "ETH_PACKETS_RX_LEN_ERR", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID ETH_PACKETS_RX_COLLISION = new PID(139, "ETH_PACKETS_RX_COLLISION", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID IP_ADDRESS = new PID(140, "IP_ADDRESS", new Formatter(FORMAT.IPV6), 1);

		public static readonly PID IP_SUBNETMASK = new PID(141, "IP_SUBNETMASK", new Formatter(FORMAT.IPV6), 1);

		public static readonly PID IP_GATEWAY = new PID(142, "IP_GATEWAY", new Formatter(FORMAT.IPV6), 1);

		public static readonly PID TCP_NUM_CONNECTIONS = new PID(143, "TCP_NUM_CONNECTIONS", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID AUX_BATTERY_VOLTAGE = new PID(144, "AUX_BATTERY_VOLTAGE", new Formatter(FORMAT.UINT32, "{0:0.###} V", 1.52587890625E-05), 0);

		public static readonly PID RGB_LIGHTING_GANG_ENABLE = new PID(145, "RGB_LIGHTING_GANG_ENABLE", new Formatter(FORMAT.UINT8, "${0:X2}"), 2);

		public static readonly PID INPUT_SWITCH_TYPE = new PID(146, "INPUT_SWITCH_TYPE", new Formatter(FORMAT.UINT8, "${0:X2}"), 2);

		public static readonly PID DOOR_LOCK_STATE = new PID(147, "DOOR_LOCK_STATE", new Formatter(FORMAT.UINT8, "${0:X2}"), 0);

		public static readonly PID GENERATOR_QUIET_HOURS_START_TIME = new PID(148, "GENERATOR_QUIET_HOURS_START_TIME", new Formatter(FORMAT.UINT16, "{0:0.###} hrs", 1.0 / 60.0), 2);

		public static readonly PID GENERATOR_QUIET_HOURS_END_TIME = new PID(149, "GENERATOR_QUIET_HOURS_END_TIME", new Formatter(FORMAT.UINT16, "{0:0.###} hrs", 1.0 / 60.0), 2);

		public static readonly PID GENERATOR_AUTO_START_LOW_VOLTAGE = new PID(150, "GENERATOR_AUTO_START_LOW_VOLTAGE", new Formatter(FORMAT.UINT32, "{0:0.###} V", 1.52587890625E-05), 2);

		public static readonly PID GENERATOR_AUTO_START_HI_TEMP_C = new PID(151, "GENERATOR_AUTO_START_HI_TEMP_C", new Formatter(FORMAT.INT32, "{0:0.###} °C", 1.52587890625E-05), 2);

		public static readonly PID GENERATOR_AUTO_RUN_DURATION_MINUTES = new PID(152, "GENERATOR_AUTO_RUN_DURATION_MINUTES", new Formatter(FORMAT.UINT16, "{0:#,###0} minutes"), 2);

		public static readonly PID GENERATOR_AUTO_RUN_MIN_OFF_TIME_MINUTES = new PID(153, "GENERATOR_AUTO_RUN_MIN_OFF_TIME_MINUTES", new Formatter(FORMAT.UINT16, "{0:#,###0} minutes"), 2);

		public static readonly PID SOFTWARE_BUILD_DATE_TIME = new PID(154, "SOFTWARE_BUILD_DATE_TIME", new Formatter(FORMAT.DATE_TIME), 0);

		public static readonly PID GENERATOR_QUIET_HOURS_ENABLED = new PID(155, "GENERATOR_QUIET_HOURS_ENABLED", new Formatter(FORMAT.UINT8, "${0:X2}"), 2);

		public static readonly PID SHORE_POWER_AMP_RATING = new PID(156, "SHORE_POWER_AMP_RATING", new Formatter(FORMAT.UINT32, "{0:0.###} A", 1.52587890625E-05), 2);

		public static readonly PID BATTERY_CAPACITY_AMP_HOURS = new PID(157, "BATTERY_CAPACITY_AMP_HOURS", new Formatter(FORMAT.UINT32, "{0:0.###} Amp-Hours", 1.52587890625E-05), 2);

		public static readonly PID PCB_ASSEMBLY_PART_NUMBER = new PID(158, "PCB_ASSEMBLY_PART_NUMBER", new Formatter(FORMAT.UINT48, "${0:X12}"), 1);

		public static readonly PID UNLOCK_PIN = new PID(159, "UNLOCK_PIN", new Formatter(FORMAT.UINT48), 2);

		public static readonly PID UNLOCK_PIN_MODE = new PID(160, "UNLOCK_PIN_MODE", new Formatter(FORMAT.UINT48), 2);

		public static readonly PID SIMULATE_ON_OFF_STYLE_LIGHT = new PID(161, "SIMULATE_ON_OFF_STYLE_LIGHT", new Formatter(FORMAT.UINT48), 2);

		public static readonly PID FAN_SPEED_CONTROL_TYPE = new PID(162, "FAN_SPEED_CONTROL_TYPE", new Formatter(FORMAT.UINT48), 2);

		public static readonly PID HVAC_CONTROL_TYPE = new PID(163, "HVAC_CONTROL_TYPE", new Formatter(FORMAT.UINT48), 2);

		public static readonly PID SOFTWARE_FUSE_RATING_AMPS = new PID(164, "SOFTWARE_FUSE_RATING_AMPS", new Formatter(FORMAT.UINT32, "{0:0.###} A", 1.52587890625E-05), 2);

		public static readonly PID SOFTWARE_FUSE_MAX_RATING_AMPS = new PID(165, "SOFTWARE_FUSE_MAX_RATING_AMPS", new Formatter(FORMAT.UINT32, "{0:0.###} A", 1.52587890625E-05), 2);

		public static readonly PID CUMMINS_ONAN_GENERATOR_FAULT_CODE = new PID(166, "CUMMINS_ONAN_GENERATOR_FAULT_CODE", new Formatter(FORMAT.UINT48), 2);

		public static readonly PID MOTOR_1_CURRENT_AMPS = new PID(167, "MOTOR_1_CURRENT_AMPS", new Formatter(FORMAT.INT32, "{0:0.###} A", 1.52587890625E-05), 0);

		public static readonly PID MOTOR_2_CURRENT_AMPS = new PID(168, "MOTOR_2_CURRENT_AMPS", new Formatter(FORMAT.INT32, "{0:0.###} A", 1.52587890625E-05), 0);

		public static readonly PID MOTOR_3_CURRENT_AMPS = new PID(169, "MOTOR_3_CURRENT_AMPS", new Formatter(FORMAT.INT32, "{0:0.###} A", 1.52587890625E-05), 0);

		public static readonly PID MOTOR_4_CURRENT_AMPS = new PID(170, "MOTOR_4_CURRENT_AMPS", new Formatter(FORMAT.INT32, "{0:0.###} A", 1.52587890625E-05), 0);

		public static readonly PID MOTOR_5_CURRENT_AMPS = new PID(171, "MOTOR_5_CURRENT_AMPS", new Formatter(FORMAT.INT32, "{0:0.###} A", 1.52587890625E-05), 0);

		public static readonly PID MOTOR_6_CURRENT_AMPS = new PID(172, "MOTOR_6_CURRENT_AMPS", new Formatter(FORMAT.INT32, "{0:0.###} A", 1.52587890625E-05), 0);

		public static readonly PID MOTOR_7_CURRENT_AMPS = new PID(173, "MOTOR_7_CURRENT_AMPS", new Formatter(FORMAT.INT32, "{0:0.###} A", 1.52587890625E-05), 0);

		public static readonly PID MOTOR_8_CURRENT_AMPS = new PID(174, "MOTOR_8_CURRENT_AMPS", new Formatter(FORMAT.INT32, "{0:0.###} A", 1.52587890625E-05), 0);

		public static readonly PID MOTOR_9_CURRENT_AMPS = new PID(175, "MOTOR_9_CURRENT_AMPS", new Formatter(FORMAT.INT32, "{0:0.###} A", 1.52587890625E-05), 0);

		public static readonly PID MOTOR_10_CURRENT_AMPS = new PID(176, "MOTOR_10_CURRENT_AMPS", new Formatter(FORMAT.INT32, "{0:0.###} A", 1.52587890625E-05), 0);

		public static readonly PID MOTOR_11_CURRENT_AMPS = new PID(177, "MOTOR_11_CURRENT_AMPS", new Formatter(FORMAT.INT32, "{0:0.###} A", 1.52587890625E-05), 0);

		public static readonly PID MOTOR_12_CURRENT_AMPS = new PID(178, "MOTOR_12_CURRENT_AMPS", new Formatter(FORMAT.INT32, "{0:0.###} A", 1.52587890625E-05), 0);

		public static readonly PID MOTOR_13_CURRENT_AMPS = new PID(179, "MOTOR_13_CURRENT_AMPS", new Formatter(FORMAT.INT32, "{0:0.###} A", 1.52587890625E-05), 0);

		public static readonly PID MOTOR_14_CURRENT_AMPS = new PID(180, "MOTOR_14_CURRENT_AMPS", new Formatter(FORMAT.INT32, "{0:0.###} A", 1.52587890625E-05), 0);

		public static readonly PID MOTOR_15_CURRENT_AMPS = new PID(181, "MOTOR_15_CURRENT_AMPS", new Formatter(FORMAT.INT32, "{0:0.###} A", 1.52587890625E-05), 0);

		public static readonly PID MOTOR_16_CURRENT_AMPS = new PID(182, "MOTOR_16_CURRENT_AMPS", new Formatter(FORMAT.INT32, "{0:0.###} A", 1.52587890625E-05), 0);

		public static readonly PID DEVICE_TYPE = new PID(183, "DEVICE_TYPE", new Formatter(FORMAT.UINT8, "${0:X2}"), 2);

		public static readonly PID IN_MOTION_LOCKOUT_BEHAVIOR = new PID(184, "IN_MOTION_LOCKOUT_BEHAVIOR", new Formatter(FORMAT.UINT8), 2);

		public static readonly PID RVC_DETECTED_NODES = new PID(185, "RVC_DETECTED_NODES", new Formatter(FORMAT.UINT48), 0);

		public static readonly PID RVC_LOST_NODES = new PID(186, "RVC_LOST_NODES", new Formatter(FORMAT.UINT48), 0);

		public static readonly PID RVC_BYTES_TX = new PID(187, "RVC_BYTES_TX", new Formatter(FORMAT.UINT48), 0);

		public static readonly PID RVC_BYTES_RX = new PID(188, "RVC_BYTES_RX", new Formatter(FORMAT.UINT48), 0);

		public static readonly PID RVC_MESSAGES_TX = new PID(189, "RVC_MESSAGES_TX", new Formatter(FORMAT.UINT48), 0);

		public static readonly PID RVC_MESSAGES_RX = new PID(190, "RVC_MESSAGES_RX", new Formatter(FORMAT.UINT48), 0);

		public static readonly PID RVC_TX_BUFFERS_FREE = new PID(191, "RVC_TX_BUFFERS_FREE", new Formatter(FORMAT.UINT48), 0);

		public static readonly PID RVC_TX_BUFFERS_USED = new PID(192, "RVC_TX_BUFFERS_USED", new Formatter(FORMAT.UINT48), 0);

		public static readonly PID RVC_RX_BUFFERS_FREE = new PID(193, "RVC_RX_BUFFERS_FREE", new Formatter(FORMAT.UINT48), 0);

		public static readonly PID RVC_RX_BUFFERS_USED = new PID(194, "RVC_RX_BUFFERS_USED", new Formatter(FORMAT.UINT48), 0);

		public static readonly PID RVC_TX_OUT_OF_BUFFERS_COUNT = new PID(195, "RVC_TX_OUT_OF_BUFFERS_COUNT", new Formatter(FORMAT.UINT48), 0);

		public static readonly PID RVC_RX_OUT_OF_BUFFERS_COUNT = new PID(196, "RVC_RX_OUT_OF_BUFFERS_COUNT", new Formatter(FORMAT.UINT48), 0);

		public static readonly PID RVC_TX_FAILURE_COUNT = new PID(197, "RVC_TX_FAILURE_COUNT", new Formatter(FORMAT.UINT48), 0);

		public static readonly PID RVC_DEFAULT_SRC_ADDR = new PID(198, "RVC_DEFAULT_SRC_ADDR", new Formatter(FORMAT.UINT48), 0);

		public static readonly PID RVC_DYNAMIC_ADDR = new PID(199, "RVC_DYNAMIC_ADDR", new Formatter(FORMAT.UINT48), 0);

		public static readonly PID RVC_MAKE = new PID(200, "RVC_MAKE", new Formatter(FORMAT.CHAR6), 1);

		public static readonly PID RVC_MODEL_1 = new PID(201, "RVC_MODEL_1", new Formatter(FORMAT.CHAR6), 1);

		public static readonly PID RVC_MODEL_2 = new PID(202, "RVC_MODEL_2", new Formatter(FORMAT.CHAR6), 1);

		public static readonly PID RVC_MODEL_3 = new PID(203, "RVC_MODEL_3", new Formatter(FORMAT.CHAR6), 1);

		public static readonly PID RVC_SERIAL = new PID(204, "RVC_SERIAL", new Formatter(FORMAT.CHAR6), 1);

		public static readonly PID RVC_ID_NUMBER = new PID(205, "RVC_ID_NUMBER", new Formatter(FORMAT.CHAR6), 1);

		public static readonly PID CLOUD_GATEWAY_ASSET_ID_PART_1 = new PID(206, "CLOUD_GATEWAY_ASSET_ID_PART_1", new Formatter(FORMAT.CHAR6), 1);

		public static readonly PID CLOUD_GATEWAY_ASSET_ID_PART_2 = new PID(207, "CLOUD_GATEWAY_ASSET_ID_PART_2", new Formatter(FORMAT.CHAR6), 1);

		public static readonly PID CLOUD_GATEWAY_ASSET_ID_PART_3 = new PID(208, "CLOUD_GATEWAY_ASSET_ID_PART_3", new Formatter(FORMAT.CHAR6), 1);

		public static readonly PID HVAC_ZONE_CAPABILITIES = new PID(209, "HVAC_ZONE_CAPABILITIES", new Formatter(FORMAT.UINT8), 2);

		public static readonly PID IGNITION_BEHAVIOR = new PID(210, "IGNITION_BEHAVIOR", new Formatter(FORMAT.UINT8), 2);

		public static readonly PID BLE_NUMBER_OF_FORWARDED_CAN_DEVICES = new PID(211, "BLE_NUMBER_OF_FORWARDED_CAN_DEVICES", new Formatter(FORMAT.UINT48), 0);

		public static readonly PID BLE_NUMBER_OF_CONNECTS = new PID(212, "BLE_NUMBER_OF_CONNECTS", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID BLE_NUMBER_OF_DISCONNECTS = new PID(213, "BLE_NUMBER_OF_DISCONNECTS", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID BLE_TOTAL_TRAFFIC = new PID(214, "BLE_TOTAL_TRAFFIC", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID BLE_WRITES_FROM_PHONE = new PID(215, "BLE_WRITES_FROM_PHONE", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID BLE_NOTIFICATIONS_TO_PHONE_SUCCESSFUL = new PID(216, "BLE_NOTIFICATIONS_TO_PHONE_SUCCESSFUL", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID BLE_NOTIFICATIONS_TO_PHONE_FAILURE = new PID(217, "BLE_NOTIFICATIONS_TO_PHONE_FAILURE", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID BLE_MTU_SIZE_CENTRAL = new PID(218, "BLE_MTU_SIZE_CENTRAL", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID BLE_MTU_SIZE_PERIPHERAL = new PID(219, "BLE_MTU_SIZE_PERIPHERAL", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID BLE_DATA_LENGTH_TIME = new PID(220, "BLE_DATA_LENGTH_TIME", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID BLE_SECURITY_UNLOCKED = new PID(221, "BLE_SECURITY_UNLOCKED", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID BLE_CLIENT_CONNECTED = new PID(222, "BLE_CLIENT_CONNECTED", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID BLE_CCCD_WRITTEN = new PID(223, "BLE_CCCD_WRITTEN", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID BLE_NUM_BUFFERS_FREE = new PID(224, "BLE_NUM_BUFFERS_FREE", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID BLE_LAST_TX_ERROR = new PID(225, "BLE_LAST_TX_ERROR", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID BLE_CONNECTED_DEVICE_RSSI = new PID(226, "BLE_CONNECTED_DEVICE_RSSI", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID BLE_DEAD_CLIENT_COUNTER = new PID(227, "BLE_DEAD_CLIENT_COUNTER", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID BLE_LAST_DISCONNECT_REASON = new PID(228, "BLE_LAST_DISCONNECT_REASON", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID BLE_SPI_RX_MSGS_DROPPED = new PID(229, "BLE_SPI_RX_MSGS_DROPPED", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID BLE_SPI_TX_MSGS_DROPPED = new PID(230, "BLE_SPI_TX_MSGS_DROPPED", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID LOW_VOLTAGE_BEHAVIOR = new PID(231, "LOW_VOLTAGE_BEHAVIOR", new Formatter(FORMAT.UINT8), 2);

		public static readonly PID DHCP_ENABLED = new PID(232, "DHCP_ENABLED", new Formatter(FORMAT.UINT8), 2);

		public static readonly PID UDP_DEVICE_NAME_1 = new PID(233, "UDP_DEVICE_NAME_1", new Formatter(FORMAT.CHAR6), 2);

		public static readonly PID UDP_DEVICE_NAME_2 = new PID(234, "UDP_DEVICE_NAME_2", new Formatter(FORMAT.CHAR6), 2);

		public static readonly PID UDP_DEVICE_NAME_3 = new PID(235, "UDP_DEVICE_NAME_3", new Formatter(FORMAT.CHAR6), 2);

		public static readonly PID TCP_BATCH_SIZE = new PID(236, "TCP_BATCH_SIZE", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID TCP_BATCH_TIME = new PID(237, "TCP_BATCH_TIME", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 2);

		public static readonly PID ON_OFF_INPUT_PIN = new PID(238, "ON_OFF_INPUT_PIN", new Formatter(FORMAT.UINT48), 2);

		public static readonly PID EXTEND_INPUT_PIN = new PID(239, "EXTEND_INPUT_PIN", new Formatter(FORMAT.UINT48), 2);

		public static readonly PID RETRACT_INPUT_PIN = new PID(240, "RETRACT_INPUT_PIN", new Formatter(FORMAT.UINT48), 2);

		public static readonly PID INPUT_PIN_COUNT = new PID(241, "INPUT_PIN_COUNT", new Formatter(FORMAT.UINT48, "{0:#,###0}"), 0);

		public static readonly PID DSI_FAULT_INPUT_PIN = new PID(242, "DSI_FAULT_INPUT_PIN", new Formatter(FORMAT.UINT48), 2);

		public static readonly PID DEVICE_ACTIVATION_TIMEOUT = new PID(243, "DEVICE_ACTIVATION_TIMEOUT", new Formatter(FORMAT.UINT48), 2);

		public static readonly PID LEVELER_UI_SUPPORTED_FEATURES = new PID(244, "LEVELER_UI_SUPPORTED_FEATURES", new Formatter(FORMAT.UINT32, "${0:X8}"), 0);

		public static readonly PID LEVELER_SENSOR_TOPOLOGY = new PID(245, "LEVELER_SENSOR_TOPOLOGY", new Formatter(FORMAT.UINT48), 0);

		public static readonly PID LEVELER_DRIVE_TYPE = new PID(246, "LEVELER_DRIVE_TYPE", new Formatter(FORMAT.UINT48), 0);

		public static readonly PID LEVELER_AUTO_MODE_PROGRESS = new PID(247, "LEVELER_AUTO_MODE_PROGRESS", new Formatter(FORMAT.UINT32, "${0:X8}"), 0);

		public static readonly PID LEFT_FRONT_JACK_STROKE_INCHES = new PID(248, "LEFT_FRONT_JACK_STROKE_INCHES", new Formatter(FORMAT.INT32, "{0:0.###}\"", 1.52587890625E-05), 0);

		public static readonly PID RIGHT_FRONT_JACK_STROKE_INCHES = new PID(249, "RIGHT_FRONT_JACK_STROKE_INCHES", new Formatter(FORMAT.INT32, "{0:0.###}\"", 1.52587890625E-05), 0);

		public static readonly PID LEFT_MIDDLE_JACK_STROKE_INCHES = new PID(250, "LEFT_MIDDLE_JACK_STROKE_INCHES", new Formatter(FORMAT.INT32, "{0:0.###}\"", 1.52587890625E-05), 0);

		public static readonly PID RIGHT_MIDDLE_JACK_STROKE_INCHES = new PID(251, "RIGHT_MIDDLE_JACK_STROKE_INCHES", new Formatter(FORMAT.INT32, "{0:0.###}\"", 1.52587890625E-05), 0);

		public static readonly PID LEFT_REAR_JACK_STROKE_INCHES = new PID(252, "LEFT_REAR_JACK_STROKE_INCHES", new Formatter(FORMAT.INT32, "{0:0.###}\"", 1.52587890625E-05), 0);

		public static readonly PID RIGHT_REAR_JACK_STROKE_INCHES = new PID(253, "RIGHT_REAR_JACK_STROKE_INCHES", new Formatter(FORMAT.INT32, "{0:0.###}\"", 1.52587890625E-05), 0);

		public static readonly PID LEFT_FRONT_JACK_MAX_STROKE_INCHES = new PID(254, "LEFT_FRONT_JACK_MAX_STROKE_INCHES", new Formatter(FORMAT.UINT32, "{0:0.###}\"", 1.52587890625E-05), 0);

		public static readonly PID RIGHT_FRONT_JACK_MAX_STROKE_INCHES = new PID(255, "RIGHT_FRONT_JACK_MAX_STROKE_INCHES", new Formatter(FORMAT.UINT32, "{0:0.###}\"", 1.52587890625E-05), 0);

		public static readonly PID LEFT_MIDDLE_JACK_MAX_STROKE_INCHES = new PID(256, "LEFT_MIDDLE_JACK_MAX_STROKE_INCHES", new Formatter(FORMAT.UINT32, "{0:0.###}\"", 1.52587890625E-05), 0);

		public static readonly PID RIGHT_MIDDLE_JACK_MAX_STROKE_INCHES = new PID(257, "RIGHT_MIDDLE_JACK_MAX_STROKE_INCHES", new Formatter(FORMAT.UINT32, "{0:0.###}\"", 1.52587890625E-05), 0);

		public static readonly PID LEFT_REAR_JACK_MAX_STROKE_INCHES = new PID(258, "LEFT_REAR_JACK_MAX_STROKE_INCHES", new Formatter(FORMAT.UINT32, "{0:0.###}\"", 1.52587890625E-05), 0);

		public static readonly PID RIGHT_REAR_JACK_MAX_STROKE_INCHES = new PID(259, "RIGHT_REAR_JACK_MAX_STROKE_INCHES", new Formatter(FORMAT.UINT32, "{0:0.###}\"", 1.52587890625E-05), 0);

		public static readonly PID PARKBRAKE_BEHAVIOR = new PID(260, "PARKBRAKE_BEHAVIOR", new Formatter(FORMAT.UINT48), 2);

		public static readonly PID EXTENDED_DEVICE_CAPABILITIES = new PID(261, "EXTENDED_DEVICE_CAPABILITIES", new Formatter(FORMAT.UINT48, "${0:X12}"), 2);

		public static readonly PID CLOUD_CAPABILITIES = new PID(262, "CLOUD_CAPABILITIES", new Formatter(FORMAT.UINT48, "${0:X12}"), 2);

		public static readonly PID RV_MAKE_ID = new PID(263, "RV_MAKE_ID", new Formatter(FORMAT.UINT32, "${0:X8}"), 2);

		public static readonly PID RV_MODEL_ID = new PID(264, "RV_MODEL_ID", new Formatter(FORMAT.UINT32, "${0:X8}"), 2);

		public static readonly PID RV_YEAR = new PID(265, "RV_YEAR", new Formatter(FORMAT.UINT16, "${0:X4}"), 2);

		public static readonly PID RV_FLOORPLAN_ID = new PID(266, "RV_FLOORPLAN_ID", new Formatter(FORMAT.UINT32, "${0:X8}"), 2);

		public static readonly PID FLOORPLAN_PART_NUM = new PID(267, "FLOORPLAN_PART_NUM", new Formatter(FORMAT.CHAR6), 2);

		public static readonly PID FLOORPLAN_WRITTEN_BY = new PID(268, "FLOORPLAN_WRITTEN_BY", new Formatter(FORMAT.UINT8), 2);

		public static readonly PID ASSEMBLY_PART_NUM = new PID(269, "ASSEMBLY_PART_NUM", new Formatter(FORMAT.CHAR6), 1);

		public static readonly PID ASSEMBLY_DATE_CODE = new PID(270, "ASSEMBLY_DATE_CODE", new Formatter(FORMAT.CHAR5), 1);

		public static readonly PID ASSEMBLY_SERIAL_NUM = new PID(271, "ASSEMBLY_SERIAL_NUM", new Formatter(FORMAT.CHAR5), 1);

		public static readonly PID LEVELER_AUTO_PROCESS_STEPS_1 = new PID(272, "LEVELER_AUTO_PROCESS_STEPS_1", new Formatter(FORMAT.MAC6), 0);

		public static readonly PID LEVELER_AUTO_PROCESS_STEPS_2 = new PID(273, "LEVELER_AUTO_PROCESS_STEPS_2", new Formatter(FORMAT.MAC6), 0);

		public static readonly PID LEVELER_AUTO_PROCESS_STEPS_3 = new PID(274, "LEVELER_AUTO_PROCESS_STEPS_3", new Formatter(FORMAT.MAC6), 0);

		public static readonly PID LEVELER_AUTO_PROCESS_STEPS_4 = new PID(275, "LEVELER_AUTO_PROCESS_STEPS_4", new Formatter(FORMAT.MAC6), 0);

		public static readonly PID LEVELER_AUTO_PROCESS_STEPS_5 = new PID(276, "LEVELER_AUTO_PROCESS_STEPS_5", new Formatter(FORMAT.MAC6), 0);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_01 = new PID(277, "MONITOR_PANEL_DEVICE_ID_01", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_02 = new PID(278, "MONITOR_PANEL_DEVICE_ID_02", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_03 = new PID(279, "MONITOR_PANEL_DEVICE_ID_03", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_04 = new PID(280, "MONITOR_PANEL_DEVICE_ID_04", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_05 = new PID(281, "MONITOR_PANEL_DEVICE_ID_05", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_06 = new PID(282, "MONITOR_PANEL_DEVICE_ID_06", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_07 = new PID(283, "MONITOR_PANEL_DEVICE_ID_07", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_08 = new PID(284, "MONITOR_PANEL_DEVICE_ID_08", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_09 = new PID(285, "MONITOR_PANEL_DEVICE_ID_09", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_10 = new PID(286, "MONITOR_PANEL_DEVICE_ID_10", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_11 = new PID(287, "MONITOR_PANEL_DEVICE_ID_11", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_12 = new PID(288, "MONITOR_PANEL_DEVICE_ID_12", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_13 = new PID(289, "MONITOR_PANEL_DEVICE_ID_13", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_14 = new PID(290, "MONITOR_PANEL_DEVICE_ID_14", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_15 = new PID(291, "MONITOR_PANEL_DEVICE_ID_15", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_16 = new PID(292, "MONITOR_PANEL_DEVICE_ID_16", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_17 = new PID(293, "MONITOR_PANEL_DEVICE_ID_17", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_18 = new PID(294, "MONITOR_PANEL_DEVICE_ID_18", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_19 = new PID(295, "MONITOR_PANEL_DEVICE_ID_19", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_20 = new PID(296, "MONITOR_PANEL_DEVICE_ID_20", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_21 = new PID(297, "MONITOR_PANEL_DEVICE_ID_21", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_22 = new PID(298, "MONITOR_PANEL_DEVICE_ID_22", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_23 = new PID(299, "MONITOR_PANEL_DEVICE_ID_23", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_24 = new PID(300, "MONITOR_PANEL_DEVICE_ID_24", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_25 = new PID(301, "MONITOR_PANEL_DEVICE_ID_25", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_26 = new PID(302, "MONITOR_PANEL_DEVICE_ID_26", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_27 = new PID(303, "MONITOR_PANEL_DEVICE_ID_27", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_28 = new PID(304, "MONITOR_PANEL_DEVICE_ID_28", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_29 = new PID(305, "MONITOR_PANEL_DEVICE_ID_29", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_30 = new PID(306, "MONITOR_PANEL_DEVICE_ID_30", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_31 = new PID(307, "MONITOR_PANEL_DEVICE_ID_31", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_32 = new PID(308, "MONITOR_PANEL_DEVICE_ID_32", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_33 = new PID(309, "MONITOR_PANEL_DEVICE_ID_33", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_34 = new PID(310, "MONITOR_PANEL_DEVICE_ID_34", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_35 = new PID(311, "MONITOR_PANEL_DEVICE_ID_35", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_36 = new PID(312, "MONITOR_PANEL_DEVICE_ID_36", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_37 = new PID(313, "MONITOR_PANEL_DEVICE_ID_37", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_38 = new PID(314, "MONITOR_PANEL_DEVICE_ID_38", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_39 = new PID(315, "MONITOR_PANEL_DEVICE_ID_39", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_40 = new PID(316, "MONITOR_PANEL_DEVICE_ID_40", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_41 = new PID(317, "MONITOR_PANEL_DEVICE_ID_41", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_42 = new PID(318, "MONITOR_PANEL_DEVICE_ID_42", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_43 = new PID(319, "MONITOR_PANEL_DEVICE_ID_43", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_44 = new PID(320, "MONITOR_PANEL_DEVICE_ID_44", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_45 = new PID(321, "MONITOR_PANEL_DEVICE_ID_45", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_46 = new PID(322, "MONITOR_PANEL_DEVICE_ID_46", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_47 = new PID(323, "MONITOR_PANEL_DEVICE_ID_47", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_DEVICE_ID_48 = new PID(324, "MONITOR_PANEL_DEVICE_ID_48", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID MONITOR_PANEL_CONTROL_TYPE_MOMENTARY_SWITCH = new PID(325, "MONITOR_PANEL_CONTROL_TYPE_MOMENTARY_SWITCH", new Formatter(FORMAT.UINT8, "${0:X2}"), 0);

		public static readonly PID MONITOR_PANEL_CONTROL_TYPE_LATCHING_SWITCH = new PID(326, "MONITOR_PANEL_CONTROL_TYPE_LATCHING_SWITCH", new Formatter(FORMAT.UINT8, "${0:X2}"), 0);

		public static readonly PID MONITOR_PANEL_CONTROL_TYPE_SUPPLY_TANK = new PID(327, "MONITOR_PANEL_CONTROL_TYPE_SUPPLY_TANK", new Formatter(FORMAT.UINT8, "${0:X2}"), 0);

		public static readonly PID MONITOR_PANEL_CONTROL_TYPE_WASTE_TANK = new PID(328, "MONITOR_PANEL_CONTROL_TYPE_WASTE_TANK", new Formatter(FORMAT.UINT8, "${0:X2}"), 0);

		public static readonly PID MONITOR_PANEL_CONFIGURATION = new PID(329, "MONITOR_PANEL_CONFIGURATION", new Formatter(FORMAT.UINT8, "${0:X2}"), 2);

		public static readonly PID BLE_PAIRING_MODE = new PID(330, "BLE_PAIRING_MODE", new Formatter(FORMAT.UINT8, "${0:X2}"), 2);

		public static readonly PID MONITOR_PANEL_CALIBRATION_PART_NBR = new PID(331, "MONITOR_PANEL_CALIBRATION_PART_NBR", new Formatter(FORMAT.CHAR6), 0);

		public static readonly PID READ_ADDRESS16BITS_DATA32BITS = new PID(332, "READ_ADDRESS16BITS_DATA32BITS", new Formatter(FORMAT.ADDRESS16_DATA32), 0);

		public static readonly PID WRITE_ADDRESS16BITS_DATA32BITS = new PID(333, "WRITE_ADDRESS16BITS_DATA32BITS", new Formatter(FORMAT.ADDRESS16_DATA32), 3);

		public static readonly PID TEMP_SENSOR_HIGH_TEMP_ALERT = new PID(334, "TEMP_SENSOR_HIGH_TEMP_ALERT ", new Formatter(FORMAT.INT16, "{0:0.#}°", 1.0 / 256.0), 2);

		public static readonly PID TEMP_SENSOR_LOW_TEMP_ALERT = new PID(335, "TEMP_SENSOR_LOW_TEMP_ALERT ", new Formatter(FORMAT.INT16, "{0:0.#}°", 1.0 / 256.0), 2);

		public static readonly PID ACC_GW_ADD_DEVICE_MAC = new PID(336, "ACC_GW_ADD_DEVICE_MAC", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID ACC_GW_WRITE_DEVICE_SW_NUM = new PID(337, "ACC_GW_WRITE_DEVICE_SW_NUM ", new Formatter(FORMAT.CHAR6), 2);

		public static readonly PID VEHICLE_CONFIGURATION = new PID(338, "VEHICLE_CONFIGURATION", new Formatter(FORMAT.UINT32, "${0:X8}"), 2);

		public static readonly PID TPMS_SENSOR_POSITION = new PID(339, "TPMS_SENSOR_POSITION", new Formatter(FORMAT.ADDRESS16_DATA32), 3);

		public static readonly PID TPMS_SENSOR_PRESURE_FAULT_LIMITS = new PID(340, "TPMS_SENSOR_PRESURE_FAULT_LIMITS", new Formatter(FORMAT.ADDRESS16_DATA32), 3);

		public static readonly PID TPMS_SENSOR_TEMPERATURE_FAULT_LIMITS = new PID(341, "TPMS_SENSOR_TEMPERATURE_FAULT_LIMITS", new Formatter(FORMAT.ADDRESS16_DATA32), 3);

		public static readonly PID TPMS_SENSOR_ID = new PID(342, "TPMS_SENSOR_ID", new Formatter(FORMAT.ADDRESS16_DATA32), 0);

		public static readonly PID SMART_ARM_WIND_EVENT_SETTING = new PID(343, "SMART_ARM_WIND_EVENT_SETTING", new Formatter(FORMAT.UINT8, "${0:X2}"), 2);

		public static readonly PID ACC_REQUEST_MODE = new PID(344, "ACC_REQUEST_MODE", new Formatter(FORMAT.UINT8, "${0:X2}"), 2);

		public static readonly PID ACCESSORY_SETTING_01 = new PID(345, "ACCESSORY_SETTING_01", new Formatter(FORMAT.BYTE6), 2);

		public static readonly PID ACCESSORY_SETTING_02 = new PID(346, "ACCESSORY_SETTING_02", new Formatter(FORMAT.BYTE6), 2);

		public static readonly PID ACCESSORY_SETTING_03 = new PID(347, "ACCESSORY_SETTING_03", new Formatter(FORMAT.BYTE6), 2);

		public static readonly PID ACCESSORY_SETTING_04 = new PID(348, "ACCESSORY_SETTING_04", new Formatter(FORMAT.BYTE6), 2);

		public static readonly PID ACCESSORY_SETTING_05 = new PID(349, "ACCESSORY_SETTING_05", new Formatter(FORMAT.BYTE6), 2);

		public static readonly PID ACCESSORY_SETTING_06 = new PID(350, "ACCESSORY_SETTING_06", new Formatter(FORMAT.BYTE6), 2);

		public static readonly PID ACCESSORY_SETTING_07 = new PID(351, "ACCESSORY_SETTING_07", new Formatter(FORMAT.BYTE6), 2);

		public static readonly PID ACCESSORY_SETTING_08 = new PID(352, "ACCESSORY_SETTING_08", new Formatter(FORMAT.BYTE6), 2);

		public static readonly PID ACCESSORY_SETTING_09 = new PID(353, "ACCESSORY_SETTING_09", new Formatter(FORMAT.BYTE6), 2);

		public static readonly PID ACCESSORY_SETTING_10 = new PID(354, "ACCESSORY_SETTING_10", new Formatter(FORMAT.BYTE6), 2);

		public static readonly PID ACCESSORY_SETTING_11 = new PID(355, "ACCESSORY_SETTING_11", new Formatter(FORMAT.BYTE6), 2);

		public static readonly PID ACCESSORY_SETTING_12 = new PID(356, "ACCESSORY_SETTING_12", new Formatter(FORMAT.BYTE6), 2);

		public static readonly PID ACCESSORY_SETTING_13 = new PID(357, "ACCESSORY_SETTING_13", new Formatter(FORMAT.BYTE6), 2);

		public static readonly PID ACCESSORY_SETTING_14 = new PID(358, "ACCESSORY_SETTING_14", new Formatter(FORMAT.BYTE6), 2);

		public static readonly PID ACCESSORY_SETTING_15 = new PID(359, "ACCESSORY_SETTING_15", new Formatter(FORMAT.BYTE6), 2);

		public static readonly PID ACCESSORY_SETTING_16 = new PID(360, "ACCESSORY_SETTING_16", new Formatter(FORMAT.BYTE6), 2);

		public static readonly PID TIRE_TRACK_WIDTH = new PID(361, "TIRE_TRACK_WIDTH", new Formatter(FORMAT.UINT32, "{0:0.###}\"", 1.52587890625E-05), 1);

		public static readonly PID TIRE_DIAMETER = new PID(362, "TIRE_DIAMETER", new Formatter(FORMAT.UINT32, "{0:0.###}\"", 1.52587890625E-05), 1);

		public static readonly PID ABS_RIM_TEETH_COUNT = new PID(363, "ABS_RIM_TEETH_COUNT", new Formatter(FORMAT.UINT8, "${0:X2}"), 1);

		public static readonly PID ABS_MAINTENANCE_PERIOD = new PID(364, "ABS_MAINTENANCE_PERIOD", new Formatter(FORMAT.UINT32), 1);

		public static readonly PID ILLUMINATION_SYNC = new PID(365, "ILLUMINATION_SYNC", new Formatter(FORMAT.UINT8, "${0:X2}"), 2);

		public static readonly PID RV_C_INSTANCE = new PID(366, "RV_C_INSTANCE", new Formatter(FORMAT.UINT8, "${0:X2}"), 2);

		public static readonly PID HVAC_CONTROL_TYPE_SETTING = new PID(367, "HVAC_CONTROL_TYPE_SETTING", new Formatter(FORMAT.UINT8, "${0:X2}"), 2);

		public static readonly PID ACTIVE_HVAC_CONTROL_TYPE = new PID(368, "ACTIVE_HVAC_CONTROL_TYPE", new Formatter(FORMAT.UINT8, "${0:X2}"), 0);

		public static readonly PID MONITOR_PANEL_CONTROL_TYPE_CONFIG_TANK = new PID(369, "MONITOR_PANEL_CONTROL_TYPE_CONFIG_TANK", new Formatter(FORMAT.UINT8, "${0:X2}"), 0);

		public static readonly PID NUMBER_OF_AXLES = new PID(370, "NUMBER_OF_AXLES", new Formatter(FORMAT.UINT8, "${0:X2}"), 1);

		public static readonly PID LAST_MAINTENANCE_ODOMETER = new PID(371, "LAST_MAINTENANCE_ODOMETER", new Formatter(FORMAT.UINT32), 2);

		public static readonly PID ACC_GW_NUM_DEVICES = new PID(372, "ACC_GW_NUM_DEVICES", new Formatter(FORMAT.UINT8), 0);

		public static readonly PID ACC_GW_MAC_HIGH = new PID(373, "ACC_GW_MAC_HIGH", new Formatter(FORMAT.ADDRESS16_DATA32), 0);

		public static readonly PID ACC_GW_MAC_LOW = new PID(374, "ACC_GW_MAC_LOW", new Formatter(FORMAT.ADDRESS16_DATA32), 0);

		public static readonly PID DEVICE_TYPE_AT_INDEX = new PID(375, "DEVICE_TYPE_AT_INDEX", new Formatter(FORMAT.ADDRESS16_DATA32), 0);

		public static readonly PID BRAKE_MODULE_ORIENTATION = new PID(376, "BRAKE_MODULE_ORIENTATION", new Formatter(FORMAT.UINT8), 1);

		public static readonly PID CORE_MICROCONTOLLER_RESET = new PID(377, "CORE_MICROCONTOLLER_RESET", new Formatter(FORMAT.CHAR6), 3);

		public static readonly PID PRODUCT_FW_PART_NUM = new PID(378, "PRODUCT_FW_PART_NUM", new Formatter(FORMAT.CHAR6), 1);

		public static readonly PID CORE_VERSION_INFO = new PID(379, "CORE_VERSION_INFO", new Formatter(FORMAT.BYTE6), 2);

		public static readonly PID PRODUCT_ID_NUM = new PID(380, "PRODUCT_ID_NUM", new Formatter(FORMAT.UINT16, "${0:X4}"), 1);

		public static readonly PID PRODUCT_ID_IN_CONFIG_BLOCK = new PID(381, "PRODUCT_ID_IN_CONFIG_BLOCK", new Formatter(FORMAT.UINT16, "${0:X4}"), 1);

		public static readonly PID LOCAP_VERSION_INFO = new PID(382, "LOCAP_VERSION_INFO", new Formatter(FORMAT.BYTE6), 2);

		public static readonly PID PRODUCT_FW_PART_NUM1 = new PID(383, "PRODUCT_FW_PART_NUM1", new Formatter(FORMAT.CHAR6), 1);

		public static readonly PID PRODUCT_FW_PART_NUM2 = new PID(384, "PRODUCT_FW_PART_NUM2", new Formatter(FORMAT.CHAR2), 1);

		public static readonly PID HBRIDGE_SAFETY_ALERT_CONFIG = new PID(385, "HBRIDGE_SAFETY_ALERT_CONFIG", new Formatter(FORMAT.UINT8, "${0:X2}"), 2);

		public static readonly PID AWNING_AUTO_PROTECTION_COUNT = new PID(386, "AWNING_AUTO_PROTECTION_COUNT", new Formatter(FORMAT.UINT16), 2);

		public static readonly PID MOMENTARY_HBRIDGE_CIRCUIT_ROLE = new PID(387, "MOMENTARY_HBRIDGE_CIRCUIT_ROLE", new Formatter(FORMAT.UINT8), 2);

		public static readonly PID SOUNDS_HIGHEST_CAPABLE = new PID(388, "SOUNDS_HIGHEST_CAPABLE", new Formatter(FORMAT.UINT8), 2);

		public static readonly PID SMART_ARM_VALANCE_CORRECTION = new PID(389, "SMART_ARM_VALANCE_CORRECTION", new Formatter(FORMAT.INT8), 2);

		public static readonly PID JUMP_TO_BOOT = new PID(390, "JUMP_TO_BOOT", new Formatter(FORMAT.BYTE6), 3);

		public static readonly PID OPTIONAL_CAPABILITIES_SUPPORTED = new PID(391, "OPTIONAL_CAPABILITIES_SUPPORTED", new Formatter(FORMAT.UINT8, "${0:X2}"), 0);

		public static readonly PID OPTIONAL_CAPABILITIES_ENABLED = new PID(392, "OPTIONAL_CAPABILITIES_ENABLED", new Formatter(FORMAT.UINT8, "${0:X2}"), 3);

		public static readonly PID OPTIONAL_CAPABILITIES_MANDATORY = new PID(393, "OPTIONAL_CAPABILITIES_MANDATORY", new Formatter(FORMAT.UINT8, "${0:X2}"), 0);

		public static readonly PID ABS_MODEL_VERSION = new PID(394, "ABS_MODEL_VERSION", new Formatter(FORMAT.UINT16), 0);

		public static readonly PID LOCKOUT_DISABLES_SWITCH_INPUT = new PID(395, "LOCKOUT_DISABLES_SWITCH_INPUT", new Formatter(FORMAT.UINT8), 2);

		public static readonly PID TANK_SENSOR_TYPE = new PID(396, "TANK_SENSOR_TYPE", new Formatter(FORMAT.UINT8, "${0:X2}"), 2);

		public static readonly PID TANK_SENSOR_CALIBRATION_MULTIPLIER = new PID(397, "TANK_SENSOR_CALIBRATION_MULTIPLIER", new Formatter(FORMAT.UINT8, "{0:###0}"), 2);

		public static readonly PID TANK_SENSOR_CALIBRATION_1 = new PID(398, "TANK_SENSOR_CALIBRATION_1", new Formatter(FORMAT.UINT40, "${0:X10}"), 2);

		public static readonly PID TANK_SENSOR_CALIBRATION_2 = new PID(399, "TANK_SENSOR_CALIBRATION_2", new Formatter(FORMAT.UINT40, "${0:X10}"), 2);

		public static readonly PID TANK_SENSOR_CALIBRATION_3 = new PID(400, "TANK_SENSOR_CALIBRATION_3", new Formatter(FORMAT.UINT40, "${0:X10}"), 2);

		public static readonly PID TANK_SENSOR_CALIBRATION_4 = new PID(401, "TANK_SENSOR_CALIBRATION_4", new Formatter(FORMAT.UINT40, "${0:X10}"), 2);

		public static readonly PID ABS_AUTO_CONFIG_STATUS = new PID(402, "ABS_AUTO_CONFIG_STATUS", new Formatter(FORMAT.UINT8, "${0:X2}"), 0);

		public static readonly PID OPTIONAL_CAPABILITIES_USER_DISABLED = new PID(403, "OPTIONAL_CAPABILITIES_USER_DISABLED", new Formatter(FORMAT.UINT8, "${0:X2}"), 2);

		public static readonly PID GENERATOR_TYPE = new PID(404, "GENERATOR_TYPE", new Formatter(FORMAT.UINT8), 2);

		public static readonly PID CONFIG_BUILD_DATE_TIME = new PID(405, "CONFIG_BUILD_DATE_TIME", new Formatter(FORMAT.DATE_TIME), 0);

		public static readonly PID TANK_CAPACITY_GALLONS = new PID(406, "TANK_CAPACITY_GALLONS", new Formatter(FORMAT.UINT8), 2);

		public static readonly PID LEVELER_SET_POINT_NAMES = new PID(407, "LEVELER_SET_POINT_NAMES", new Formatter(FORMAT.MAC6), 2);

		public static readonly PID CURRENT_STALL_THRESHOLD = new PID(408, "CURRENT_STALL_THRESHOLD", new Formatter(FORMAT.UINT48, "{0:0.###} A", 1.52587890625E-05), 2);

		public static readonly PID CURRENT_STALL_DEBOUNCE = new PID(409, "CURRENT_STALL_DEBOUNCE", new Formatter(FORMAT.UINT48, "{0:#,###0} ms"), 2);

		public static readonly PID SPEED_STALL_THRESHOLD = new PID(410, "SPEED_STALL_THRESHOLD", new Formatter(FORMAT.UINT48, "{0:#,###0} Hz"), 2);

		public static readonly PID SPEED_STALL_DEBOUNCE = new PID(411, "SPEED_STALL_DEBOUNCE", new Formatter(FORMAT.UINT48, "{0:#,###0} ms"), 2);

		public static readonly PID MOTOR_1_POSITION = new PID(412, "MOTOR_1_POSITION", new Formatter(FORMAT.INT32), 2);

		public static readonly PID MOTOR_2_POSITION = new PID(413, "MOTOR_2_POSITION", new Formatter(FORMAT.INT32), 2);

		public static readonly PID MOTOR_1_DISTANCE_TRAVELLED = new PID(414, "MOTOR_1_DISTANCE_TRAVELLED", new Formatter(FORMAT.INT32), 2);

		public static readonly PID MOTOR_2_DISTANCE_TRAVELLED = new PID(415, "MOTOR_2_DISTANCE_TRAVELLED", new Formatter(FORMAT.INT32), 2);

		public static readonly PID MOTOR_2_DISTANCE_TRAVELLED_UNCORRECTED = new PID(416, "MOTOR_2_DISTANCE_TRAVELLED_UNCORRECTED", new Formatter(FORMAT.INT32), 2);

		public static readonly PID MOTOR_1_RESISTANCE = new PID(417, "MOTOR_1_RESISTANCE", new Formatter(FORMAT.UINT48, "{0:0.###} Ohms", 1.52587890625E-05), 2);

		public static readonly PID MOTOR_2_RESISTANCE = new PID(418, "MOTOR_2_RESISTANCE", new Formatter(FORMAT.UINT48, "{0:0.###} Ohms", 1.52587890625E-05), 2);

		public static readonly PID POSITION_FEEDBACK_TYPE = new PID(419, "POSITION_FEEDBACK_TYPE", new Formatter(FORMAT.UINT8), 2);

		public static readonly PID GENERIC_INDEXABLE_1 = new PID(420, "GENERIC_INDEXABLE_1", new Formatter(FORMAT.ADDRESS16_DATA32), 2);

		public static readonly PID GENERIC_INDEXABLE_2 = new PID(421, "GENERIC_INDEXABLE_2", new Formatter(FORMAT.ADDRESS16_DATA32), 2);

		public static readonly PID GENERIC_INDEXABLE_3 = new PID(422, "GENERIC_INDEXABLE_3", new Formatter(FORMAT.ADDRESS16_DATA32), 2);

		public static readonly PID GENERIC_INDEXABLE_4 = new PID(423, "GENERIC_INDEXABLE_4", new Formatter(FORMAT.ADDRESS16_DATA32), 2);

		public static readonly PID IDSCAN_VERSION_INFO = new PID(424, "IDSCAN_VERSION_INFO", new Formatter(FORMAT.BYTE6), 2);

		public static readonly PID ODOMETER = new PID(425, "ODOMETER", new Formatter(FORMAT.UINT32), 2);

		public static readonly PID ODOMETER_RESET_COUNT = new PID(426, "ODOMETER_RESET_COUNT", new Formatter(FORMAT.UINT8), 2);

		public static readonly PID SOFTWARE_FUSE_MAX_RATING_MILLIAMPS = new PID(427, "SOFTWARE_FUSE_MAX_RATING_MILLIAMPS", new Formatter(FORMAT.UINT32), 2);

		public static readonly PID SOFTWARE_FUSE_RATING_MILLIAMPS = new PID(428, "SOFTWARE_FUSE_RATING_MILLIAMPS", new Formatter(FORMAT.UINT32), 2);

		public static readonly PID INVERT_HBRIDGE_BEHAVIOR = new PID(429, "INVERT_HBRIDGE_BEHAVIOR", new Formatter(FORMAT.UINT8), 2);

		public static readonly PID MAINTAIN_STATE_THROUGH_POWER_CYCLE = new PID(430, "MAINTAIN_STATE_THROUGH_POWER_CYCLE", new Formatter(FORMAT.UINT8), 2);

		public static readonly PID SET_ALL_LIGHTS_GROUP_BEHAVIOR = new PID(431, "SET_ALL_LIGHTS_GROUP_BEHAVIOR", new Formatter(FORMAT.UINT8), 2);

		public static readonly PID CRITICAL_TANK_STATE = new PID(432, "CRITICAL_TANK_STATE", new Formatter(FORMAT.UINT8), 2);

		public static readonly PID TANK_TYPE = new PID(433, "TANK_TYPE", new Formatter(FORMAT.UINT8), 2);

		public static readonly PID BRANDING = new PID(434, "BRANDING", new Formatter(FORMAT.UINT16), 2);

		public static readonly PID SWAY_MODEL_VERSION = new PID(435, "SWAY_MODEL_VERSION", new Formatter(FORMAT.UINT16), 0);

		public static readonly PID INPUT_DOES_NOT_CLEAR_LOCKOUT = new PID(436, "INPUT_DOES_NOT_CLEAR_LOCKOUT", new Formatter(FORMAT.UINT8), 2);

		public static readonly PID PAIRING_PRIORITY = new PID(437, "PAIRING_PRIORITY", new Formatter(FORMAT.UINT8), 2);

		public static readonly PID RGB_LIGHT_TYPE = new PID(438, "RGB_LIGHT_TYPE", new Formatter(FORMAT.UINT8), 2);

		public static readonly PID RV_PREPPED_FOR_TPMS = new PID(439, "RV_PREPPED_FOR_TPMS", new Formatter(FORMAT.UINT8), 2);

		public static readonly PID TANK_SENSOR_CALIBRATION_UID = new PID(440, "TANK_SENSOR_CALIBRATION_UID", new Formatter(FORMAT.UINT16), 2);

		public static readonly PID GROUP_ID_ADD = new PID(441, "GROUP_ID_ADD", new Formatter(FORMAT.UINT16), 2);

		public static readonly PID GROUP_ID_REMOVE = new PID(442, "GROUP_ID_REMOVE", new Formatter(FORMAT.UINT16), 2);

		public static readonly PID GROUP_ID_CAPACITY = new PID(443, "GROUP_ID_CAPACITY", new Formatter(FORMAT.UINT16), 0);

		public static readonly PID TEMPERATURE_SETPOINT_MINIMUM = new PID(444, "TEMPERATURE_SETPOINT_MINIMUM", new Formatter(FORMAT.INT8), 2);

		public static readonly PID TEMPERATURE_SETPOINT_MAXIMUM = new PID(445, "TEMPERATURE_SETPOINT_MAXIMUM", new Formatter(FORMAT.INT8), 2);

		public static readonly PID GENERATOR_AUTO_START_LOW_VOLTAGE_ENABLED = new PID(446, "GENERATOR_AUTO_START_LOW_VOLTAGE_ENABLED", new Formatter(FORMAT.UINT8), 2);

		public static readonly PID QUIET_HOURS_DISABLED_DAYS = new PID(447, "QUIET_HOURS_DISABLED_DAYS", new Formatter(FORMAT.UINT8), 2);

		public static readonly PID MAX_ADAPTIVE_CURRENT_STALL_THRESHOLD = new PID(448, "MAX_ADAPTIVE_CURRENT_STALL_THRESHOLD", new Formatter(FORMAT.UINT32, "{0:0.###} A", 1.52587890625E-05), 2);

		public static readonly PID ADAPTIVE_CURRENT_STALL_MARGIN = new PID(449, "ADAPTIVE_CURRENT_STALL_MARGIN", new Formatter(FORMAT.UINT32, "{0:0.###} A", 1.52587890625E-05), 2);

		public static readonly PID MAX_RELAXATION_DISTANCE = new PID(450, "MAX_RELAXATION_DISTANCE", new Formatter(FORMAT.UINT32, "{0:0.###}\"", 1.52587890625E-05), 2);

		public static readonly PID LOAD_TYPE = new PID(451, "LOAD_TYPE", new Formatter(FORMAT.UINT16), 2);

		public readonly ushort Value;

		public readonly string Name;

		private readonly Formatter Print;

		public readonly ushort Write_SessionId;

		public bool IsValid => this?.Value > 0;

		public static System.Collections.Generic.IEnumerable<PID> GetEnumerator()
		{
			return (System.Collections.Generic.IEnumerable<PID>)List;
		}

		private PID(ushort value)
		{
			Value = value;
			Name = "UNKNOWN_" + value.ToString("X4");
			Print = null;
			if (value > 0 && !Lookup.ContainsKey(value))
			{
				Lookup.Add(value, this);
			}
			Write_SessionId = 0;
		}

		private PID(ushort value, string name, Formatter print, ushort write_SessionId = 0)
		{
			Value = value;
			Name = name.Trim();
			Print = print;
			if (value > 0)
			{
				List.Add(this);
				Lookup.Add(value, this);
			}
			Write_SessionId = write_SessionId;
		}

		public string FormatValue(ulong value)
		{
			if (Print == null)
			{
				return value.ToString();
			}
			return Print.ToString(value);
		}

		public static implicit operator ushort(PID msg)
		{
			return msg?.Value ?? 0;
		}

		public static implicit operator PID(ushort value)
		{
			if (value == 0)
			{
				return UNKNOWN;
			}
			PID result = default(PID);
			if (!Lookup.TryGetValue(value, ref result))
			{
				return new PID(value);
			}
			return result;
		}

		public string ToString()
		{
			return Name;
		}
	}
	public sealed class BLOCK_ID
	{
		public enum BLOCK_PROPERTIES_OPTIONS : byte
		{
			BLOCK_PROPERTIES_FLAGS,
			BLOCK_PROPERTIES_READ_SESSION_ID,
			BLOCK_PROPERTIES_WRITE_SESSION_ID,
			BLOCK_PROPERTIES_CAPACITY,
			BLOCK_PROPERTIES_CURRENT_SIZE,
			BLOCK_PROPERTIES_CRC,
			BLOCK_PROPERTIES_VERIFY_CRC,
			BLOCK_PROPERTIES_START_ADDRESS
		}

		private static readonly Dictionary<ushort, BLOCK_ID> Lookup = new Dictionary<ushort, BLOCK_ID>();

		private static readonly List<BLOCK_ID> List = new List<BLOCK_ID>();

		public static readonly BLOCK_ID UNKNOWN = new BLOCK_ID(0, "UNKNOWN");

		public static readonly BLOCK_ID BLOCK_ID_GENERIC_1 = new BLOCK_ID(1, "GENERIC_1");

		public static readonly BLOCK_ID BLOCK_ID_MONITOR_PANEL = new BLOCK_ID(2, "MONITOR_PANEL");

		public static readonly BLOCK_ID BLOCK_ID_REFLASH = new BLOCK_ID(3, "REFLASH");

		public static readonly BLOCK_ID BLOCK_ID_LOCAP_GROUP_DATA = new BLOCK_ID(4, "BLOCK_ID_LOCAP_GROUP_DATA");

		public readonly ushort Value;

		public readonly string Name;

		public bool IsValid => this?.Value > 0;

		public static System.Collections.Generic.IEnumerable<BLOCK_ID> GetEnumerator()
		{
			return (System.Collections.Generic.IEnumerable<BLOCK_ID>)List;
		}

		private BLOCK_ID(ushort value, string name)
		{
			Value = value;
			Name = name.Trim();
			if (value > 0)
			{
				List.Add(this);
				Lookup.Add(value, this);
			}
		}

		public static implicit operator ushort(BLOCK_ID msg)
		{
			return msg?.Value ?? 0;
		}

		public static implicit operator BLOCK_ID(ushort value)
		{
			BLOCK_ID result = default(BLOCK_ID);
			if (Lookup.TryGetValue(value, ref result))
			{
				return result;
			}
			if (value == 0)
			{
				return UNKNOWN;
			}
			return new BLOCK_ID(value, "UNKNOWN_" + value.ToString("X4") + "h");
		}

		public string ToString()
		{
			return Name;
		}
	}
	public sealed class SESSION_ID
	{
		private static readonly Dictionary<ushort, SESSION_ID> Lookup;

		private static readonly List<SESSION_ID> List;

		public static readonly SESSION_ID UNKNOWN;

		public const ushort MANUFACTURING = 1;

		public const ushort DIAGNOSTIC = 2;

		public const ushort REPROGRAMMING = 3;

		public const ushort REMOTE_CONTROL = 4;

		public const ushort DAQ = 5;

		public readonly ushort Value;

		public readonly uint Cypher;

		public readonly string Name;

		public readonly string Description;

		public bool IsValid => this?.Value > 0;

		public static System.Collections.Generic.IEnumerable<SESSION_ID> GetEnumerator()
		{
			return (System.Collections.Generic.IEnumerable<SESSION_ID>)List;
		}

		static SESSION_ID()
		{
			Lookup = new Dictionary<ushort, SESSION_ID>();
			List = new List<SESSION_ID>();
			UNKNOWN = new SESSION_ID(0, 0u, "UNKNOWN", "Reserved, do not use");
			new SESSION_ID(1, 2976620821u, "MANUFACTURING", "Used to enable manufacturing features");
			new SESSION_ID(2, 3133065982u, "DIAGNOSTIC", "Used to enable diagnostic tool features");
			new SESSION_ID(3, 3735928559u, "REPROGRAMMING", "Used when reprogramming a device");
			new SESSION_ID(4, 2976579765u, "REMOTE_CONTROL", "Used when enabling remote control of the device");
			new SESSION_ID(5, 184594741u, "DAQ", "Used to enable DAQ features");
		}

		private SESSION_ID(ushort value, uint cypher, string name, string description)
		{
			Value = value;
			Cypher = cypher;
			Name = name.Trim();
			Description = description.Trim();
			if (value > 0)
			{
				List.Add(this);
				Lookup.Add(value, this);
			}
		}

		public static implicit operator ushort(SESSION_ID msg)
		{
			return msg?.Value ?? 0;
		}

		public static implicit operator SESSION_ID(ushort value)
		{
			SESSION_ID result = default(SESSION_ID);
			if (Lookup.TryGetValue(value, ref result))
			{
				return result;
			}
			return UNKNOWN;
		}

		public string ToString()
		{
			return Name;
		}

		public uint Encrypt(uint seed)
		{
			uint num = Cypher;
			int num2 = 32;
			uint num3 = 2654435769u;
			while (true)
			{
				seed += ((num << 4) + 1131376761) ^ (num + num3) ^ ((num >> 5) + 1919510376);
				if (--num2 <= 0)
				{
					break;
				}
				num += ((seed << 4) + 1948272964) ^ (seed + num3) ^ ((seed >> 5) + 1400073827);
				num3 += 2654435769u;
			}
			return seed;
		}
	}
	public enum DTC_ID : ushort
	{
		UNKNOWN,
		DTC_STORAGE_FAILURE,
		ECU_IS_DEFECTIVE,
		NVM_FAILURE,
		CONFIGURATION_FAILURE,
		BATTERY_VOLTAGE_LOW,
		BATTERY_VOLTAGE_HIGH,
		BLE_SUBSYSTEM_COMM_ERROR,
		MODULE_OVER_CURRENT,
		ETHERNET_COMM_ERROR,
		TILT_SENSOR_MALFUNCTION,
		LEVELER_ZERO_POINT_NOT_CONFIGURED,
		FUSE_CONFIGURATION_INVALID,
		OUTDOOR_TEMP_SENSOR_OPEN_CIRCUIT,
		OUTDOOR_TEMP_SENSOR_SHORT_CIRCUIT,
		TOUCH_PAD_COMM_FAILURE,
		REMOTE_SENSOR_COMM_FAILURE,
		REMOTE_SENSOR_POWER_SHORT_TO_GROUND,
		REMOTE_SENSOR_FAILURE,
		FRONT_REMOTE_SENSOR_COMM_FAILURE,
		FRONT_REMOTE_SENSOR_POWER_SHORT_TO_GROUND,
		FRONT_REMOTE_SENSOR_FAILURE,
		REAR_REMOTE_SENSOR_COMM_FAILURE,
		REAR_REMOTE_SENSOR_POWER_SHORT_TO_GROUND,
		REAR_REMOTE_SENSOR_FAILURE,
		HALL_EFFECT_SIGNAL_SHORT_TO_GND,
		HALL_EFFECT_SIGNAL_SHORT_TO_BATT,
		HALL_EFFECT_SIGNAL_FAULT,
		HALL_EFFECT_SIGNAL_CIRCUIT_OPEN,
		CAL_SWITCH_SHORT_TO_GND,
		CAL_SWITCH_SHORT_TO_BATT,
		CAL_SWITCH_SWITCH_STUCK,
		CAL_SWITCH_FAULT,
		CAL_SWITCH_CIRCUIT_OPEN,
		FLOAT_SWITCH_SHORT_TO_GND,
		FLOAT_SWITCH_SHORT_TO_BATT,
		FLOAT_SWITCH_SWITCH_STUCK,
		FLOAT_SWITCH_FAULT,
		FLOAT_SWITCH_CIRCUIT_OPEN,
		CLIMATE_ZONE_1_TEMP_SENSOR_OPEN_CIRCUIT,
		CLIMATE_ZONE_1_TEMP_SENSOR_SHORT_CIRCUIT,
		CLIMATE_ZONE_1_FREE_SENSOR_OPEN_CIRCUIT,
		CLIMATE_ZONE_1_FREE_SENSOR_SHORT_CIRCUIT,
		CLIMATE_ZONE_2_TEMP_SENSOR_OPEN_CIRCUIT,
		CLIMATE_ZONE_2_TEMP_SENSOR_SHORT_CIRCUIT,
		CLIMATE_ZONE_2_FREE_SENSOR_OPEN_CIRCUIT,
		CLIMATE_ZONE_2_FREE_SENSOR_SHORT_CIRCUIT,
		CLIMATE_ZONE_1_POWER_MISSING,
		CLIMATE_ZONE_2_POWER_MISSING,
		CLIMATE_ZONE_1_DIP_SWITCH_INVALID,
		CLIMATE_ZONE_2_DIP_SWITCH_INVALID,
		GENERATOR_FAILED_TO_START,
		GENERATOR_FAILED_TO_STOP,
		INPUT_SHORT_TO_GND,
		INPUT_SHORT_TO_BATT,
		INPUT_SWITCH_STUCK,
		INPUT_SWITCH_FAULT,
		INPUT_CIRCUIT_OPEN,
		INPUT_1_SHORT_TO_GND,
		INPUT_1_SHORT_TO_BATT,
		INPUT_1_SWITCH_STUCK,
		INPUT_1_SWITCH_FAULT,
		INPUT_1_CIRCUIT_OPEN,
		INPUT_2_SHORT_TO_GND,
		INPUT_2_SHORT_TO_BATT,
		INPUT_2_SWITCH_STUCK,
		INPUT_2_SWITCH_FAULT,
		INPUT_2_CIRCUIT_OPEN,
		INPUT_3_SHORT_TO_GND,
		INPUT_3_SHORT_TO_BATT,
		INPUT_3_SWITCH_STUCK,
		INPUT_3_SWITCH_FAULT,
		INPUT_3_CIRCUIT_OPEN,
		INPUT_4_SHORT_TO_GND,
		INPUT_4_SHORT_TO_BATT,
		INPUT_4_SWITCH_STUCK,
		INPUT_4_SWITCH_FAULT,
		INPUT_4_CIRCUIT_OPEN,
		INPUT_5_SHORT_TO_GND,
		INPUT_5_SHORT_TO_BATT,
		INPUT_5_SWITCH_STUCK,
		INPUT_5_SWITCH_FAULT,
		INPUT_5_CIRCUIT_OPEN,
		INPUT_6_SHORT_TO_GND,
		INPUT_6_SHORT_TO_BATT,
		INPUT_6_SWITCH_STUCK,
		INPUT_6_SWITCH_FAULT,
		INPUT_6_CIRCUIT_OPEN,
		INPUT_7_SHORT_TO_GND,
		INPUT_7_SHORT_TO_BATT,
		INPUT_7_SWITCH_STUCK,
		INPUT_7_SWITCH_FAULT,
		INPUT_7_CIRCUIT_OPEN,
		INPUT_8_SHORT_TO_GND,
		INPUT_8_SHORT_TO_BATT,
		INPUT_8_SWITCH_STUCK,
		INPUT_8_SWITCH_FAULT,
		INPUT_8_CIRCUIT_OPEN,
		INPUT_9_SHORT_TO_GND,
		INPUT_9_SHORT_TO_BATT,
		INPUT_9_SWITCH_STUCK,
		INPUT_9_SWITCH_FAULT,
		INPUT_9_CIRCUIT_OPEN,
		INPUT_10_SHORT_TO_GND,
		INPUT_10_SHORT_TO_BATT,
		INPUT_10_SWITCH_STUCK,
		INPUT_10_SWITCH_FAULT,
		INPUT_10_CIRCUIT_OPEN,
		INPUT_11_SHORT_TO_GND,
		INPUT_11_SHORT_TO_BATT,
		INPUT_11_SWITCH_STUCK,
		INPUT_11_SWITCH_FAULT,
		INPUT_11_CIRCUIT_OPEN,
		INPUT_12_SHORT_TO_GND,
		INPUT_12_SHORT_TO_BATT,
		INPUT_12_SWITCH_STUCK,
		INPUT_12_SWITCH_FAULT,
		INPUT_12_CIRCUIT_OPEN,
		INPUT_13_SHORT_TO_GND,
		INPUT_13_SHORT_TO_BATT,
		INPUT_13_SWITCH_STUCK,
		INPUT_13_SWITCH_FAULT,
		INPUT_13_CIRCUIT_OPEN,
		INPUT_14_SHORT_TO_GND,
		INPUT_14_SHORT_TO_BATT,
		INPUT_14_SWITCH_STUCK,
		INPUT_14_SWITCH_FAULT,
		INPUT_14_CIRCUIT_OPEN,
		INPUT_15_SHORT_TO_GND,
		INPUT_15_SHORT_TO_BATT,
		INPUT_15_SWITCH_STUCK,
		INPUT_15_SWITCH_FAULT,
		INPUT_15_CIRCUIT_OPEN,
		INPUT_16_SHORT_TO_GND,
		INPUT_16_SHORT_TO_BATT,
		INPUT_16_SWITCH_STUCK,
		INPUT_16_SWITCH_FAULT,
		INPUT_16_CIRCUIT_OPEN,
		OUTPUT_SHORT_TO_BATT,
		OUTPUT_SHORT_TO_GND,
		OUTPUT_PLUS_SHORT_TO_BATT,
		OUTPUT_PLUS_SHORT_TO_GND,
		OUTPUT_MINUS_SHORT_TO_BATT,
		OUTPUT_MINUS_SHORT_TO_GND,
		OUTPUT_CIRCUIT_FAILURE,
		OUTPUT_OPEN,
		OUTPUT_SHORT,
		OUTPUT_OVER_CURRENT,
		OUTPUT_UNDER_CURRENT,
		OUTPUT_1_SHORT_TO_BATT,
		OUTPUT_1_SHORT_TO_GND,
		OUTPUT_1_PLUS_SHORT_TO_BATT,
		OUTPUT_1_PLUS_SHORT_TO_GND,
		OUTPUT_1_MINUS_SHORT_TO_BATT,
		OUTPUT_1_MINUS_SHORT_TO_GND,
		OUTPUT_1_CIRCUIT_FAILURE,
		OUTPUT_1_OPEN,
		OUTPUT_1_SHORT,
		OUTPUT_1_OVER_CURRENT,
		OUTPUT_1_UNDER_CURRENT,
		OUTPUT_2_SHORT_TO_BATT,
		OUTPUT_2_SHORT_TO_GND,
		OUTPUT_2_PLUS_SHORT_TO_BATT,
		OUTPUT_2_PLUS_SHORT_TO_GND,
		OUTPUT_2_MINUS_SHORT_TO_BATT,
		OUTPUT_2_MINUS_SHORT_TO_GND,
		OUTPUT_2_CIRCUIT_FAILURE,
		OUTPUT_2_OPEN,
		OUTPUT_2_SHORT,
		OUTPUT_2_OVER_CURRENT,
		OUTPUT_2_UNDER_CURRENT,
		OUTPUT_3_SHORT_TO_BATT,
		OUTPUT_3_SHORT_TO_GND,
		OUTPUT_3_PLUS_SHORT_TO_BATT,
		OUTPUT_3_PLUS_SHORT_TO_GND,
		OUTPUT_3_MINUS_SHORT_TO_BATT,
		OUTPUT_3_MINUS_SHORT_TO_GND,
		OUTPUT_3_CIRCUIT_FAILURE,
		OUTPUT_3_OPEN,
		OUTPUT_3_SHORT,
		OUTPUT_3_OVER_CURRENT,
		OUTPUT_3_UNDER_CURRENT,
		OUTPUT_4_SHORT_TO_BATT,
		OUTPUT_4_SHORT_TO_GND,
		OUTPUT_4_PLUS_SHORT_TO_BATT,
		OUTPUT_4_PLUS_SHORT_TO_GND,
		OUTPUT_4_MINUS_SHORT_TO_BATT,
		OUTPUT_4_MINUS_SHORT_TO_GND,
		OUTPUT_4_CIRCUIT_FAILURE,
		OUTPUT_4_OPEN,
		OUTPUT_4_SHORT,
		OUTPUT_4_OVER_CURRENT,
		OUTPUT_4_UNDER_CURRENT,
		OUTPUT_5_SHORT_TO_BATT,
		OUTPUT_5_SHORT_TO_GND,
		OUTPUT_5_PLUS_SHORT_TO_BATT,
		OUTPUT_5_PLUS_SHORT_TO_GND,
		OUTPUT_5_MINUS_SHORT_TO_BATT,
		OUTPUT_5_MINUS_SHORT_TO_GND,
		OUTPUT_5_CIRCUIT_FAILURE,
		OUTPUT_5_OPEN,
		OUTPUT_5_SHORT,
		OUTPUT_5_OVER_CURRENT,
		OUTPUT_5_UNDER_CURRENT,
		OUTPUT_6_SHORT_TO_BATT,
		OUTPUT_6_SHORT_TO_GND,
		OUTPUT_6_PLUS_SHORT_TO_BATT,
		OUTPUT_6_PLUS_SHORT_TO_GND,
		OUTPUT_6_MINUS_SHORT_TO_BATT,
		OUTPUT_6_MINUS_SHORT_TO_GND,
		OUTPUT_6_CIRCUIT_FAILURE,
		OUTPUT_6_OPEN,
		OUTPUT_6_SHORT,
		OUTPUT_6_OVER_CURRENT,
		OUTPUT_6_UNDER_CURRENT,
		OUTPUT_7_SHORT_TO_BATT,
		OUTPUT_7_SHORT_TO_GND,
		OUTPUT_7_PLUS_SHORT_TO_BATT,
		OUTPUT_7_PLUS_SHORT_TO_GND,
		OUTPUT_7_MINUS_SHORT_TO_BATT,
		OUTPUT_7_MINUS_SHORT_TO_GND,
		OUTPUT_7_CIRCUIT_FAILURE,
		OUTPUT_7_OPEN,
		OUTPUT_7_SHORT,
		OUTPUT_7_OVER_CURRENT,
		OUTPUT_7_UNDER_CURRENT,
		OUTPUT_8_SHORT_TO_BATT,
		OUTPUT_8_SHORT_TO_GND,
		OUTPUT_8_PLUS_SHORT_TO_BATT,
		OUTPUT_8_PLUS_SHORT_TO_GND,
		OUTPUT_8_MINUS_SHORT_TO_BATT,
		OUTPUT_8_MINUS_SHORT_TO_GND,
		OUTPUT_8_CIRCUIT_FAILURE,
		OUTPUT_8_OPEN,
		OUTPUT_8_SHORT,
		OUTPUT_8_OVER_CURRENT,
		OUTPUT_8_UNDER_CURRENT,
		OUTPUT_9_SHORT_TO_BATT,
		OUTPUT_9_SHORT_TO_GND,
		OUTPUT_9_PLUS_SHORT_TO_BATT,
		OUTPUT_9_PLUS_SHORT_TO_GND,
		OUTPUT_9_MINUS_SHORT_TO_BATT,
		OUTPUT_9_MINUS_SHORT_TO_GND,
		OUTPUT_9_CIRCUIT_FAILURE,
		OUTPUT_9_OPEN,
		OUTPUT_9_SHORT,
		OUTPUT_9_OVER_CURRENT,
		OUTPUT_9_UNDER_CURRENT,
		OUTPUT_10_SHORT_TO_BATT,
		OUTPUT_10_SHORT_TO_GND,
		OUTPUT_10_PLUS_SHORT_TO_BATT,
		OUTPUT_10_PLUS_SHORT_TO_GND,
		OUTPUT_10_MINUS_SHORT_TO_BATT,
		OUTPUT_10_MINUS_SHORT_TO_GND,
		OUTPUT_10_CIRCUIT_FAILURE,
		OUTPUT_10_OPEN,
		OUTPUT_10_SHORT,
		OUTPUT_10_OVER_CURRENT,
		OUTPUT_10_UNDER_CURRENT,
		OUTPUT_11_SHORT_TO_BATT,
		OUTPUT_11_SHORT_TO_GND,
		OUTPUT_11_PLUS_SHORT_TO_BATT,
		OUTPUT_11_PLUS_SHORT_TO_GND,
		OUTPUT_11_MINUS_SHORT_TO_BATT,
		OUTPUT_11_MINUS_SHORT_TO_GND,
		OUTPUT_11_CIRCUIT_FAILURE,
		OUTPUT_11_OPEN,
		OUTPUT_11_SHORT,
		OUTPUT_11_OVER_CURRENT,
		OUTPUT_11_UNDER_CURRENT,
		OUTPUT_12_SHORT_TO_BATT,
		OUTPUT_12_SHORT_TO_GND,
		OUTPUT_12_PLUS_SHORT_TO_BATT,
		OUTPUT_12_PLUS_SHORT_TO_GND,
		OUTPUT_12_MINUS_SHORT_TO_BATT,
		OUTPUT_12_MINUS_SHORT_TO_GND,
		OUTPUT_12_CIRCUIT_FAILURE,
		OUTPUT_12_OPEN,
		OUTPUT_12_SHORT,
		OUTPUT_12_OVER_CURRENT,
		OUTPUT_12_UNDER_CURRENT,
		OUTPUT_13_SHORT_TO_BATT,
		OUTPUT_13_SHORT_TO_GND,
		OUTPUT_13_PLUS_SHORT_TO_BATT,
		OUTPUT_13_PLUS_SHORT_TO_GND,
		OUTPUT_13_MINUS_SHORT_TO_BATT,
		OUTPUT_13_MINUS_SHORT_TO_GND,
		OUTPUT_13_CIRCUIT_FAILURE,
		OUTPUT_13_OPEN,
		OUTPUT_13_SHORT,
		OUTPUT_13_OVER_CURRENT,
		OUTPUT_13_UNDER_CURRENT,
		OUTPUT_14_SHORT_TO_BATT,
		OUTPUT_14_SHORT_TO_GND,
		OUTPUT_14_PLUS_SHORT_TO_BATT,
		OUTPUT_14_PLUS_SHORT_TO_GND,
		OUTPUT_14_MINUS_SHORT_TO_BATT,
		OUTPUT_14_MINUS_SHORT_TO_GND,
		OUTPUT_14_CIRCUIT_FAILURE,
		OUTPUT_14_OPEN,
		OUTPUT_14_SHORT,
		OUTPUT_14_OVER_CURRENT,
		OUTPUT_14_UNDER_CURRENT,
		OUTPUT_15_SHORT_TO_BATT,
		OUTPUT_15_SHORT_TO_GND,
		OUTPUT_15_PLUS_SHORT_TO_BATT,
		OUTPUT_15_PLUS_SHORT_TO_GND,
		OUTPUT_15_MINUS_SHORT_TO_BATT,
		OUTPUT_15_MINUS_SHORT_TO_GND,
		OUTPUT_15_CIRCUIT_FAILURE,
		OUTPUT_15_OPEN,
		OUTPUT_15_SHORT,
		OUTPUT_15_OVER_CURRENT,
		OUTPUT_15_UNDER_CURRENT,
		OUTPUT_16_SHORT_TO_BATT,
		OUTPUT_16_SHORT_TO_GND,
		OUTPUT_16_PLUS_SHORT_TO_BATT,
		OUTPUT_16_PLUS_SHORT_TO_GND,
		OUTPUT_16_MINUS_SHORT_TO_BATT,
		OUTPUT_16_MINUS_SHORT_TO_GND,
		OUTPUT_16_CIRCUIT_FAILURE,
		OUTPUT_16_OPEN,
		OUTPUT_16_SHORT,
		OUTPUT_16_OVER_CURRENT,
		OUTPUT_16_UNDER_CURRENT,
		LIGHT_OUTPUT_SHORT_TO_BATT,
		LIGHT_OUTPUT_SHORT_TO_GND,
		LIGHT_OUTPUT_PLUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_PLUS_SHORT_TO_GND,
		LIGHT_OUTPUT_MINUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_MINUS_SHORT_TO_GND,
		LIGHT_OUTPUT_CIRCUIT_FAILURE,
		LIGHT_OUTPUT_OPEN,
		LIGHT_OUTPUT_SHORT,
		LIGHT_OUTPUT_OVER_CURRENT,
		LIGHT_OUTPUT_UNDER_CURRENT,
		LIGHT_OUTPUT_1_SHORT_TO_BATT,
		LIGHT_OUTPUT_1_SHORT_TO_GND,
		LIGHT_OUTPUT_1_PLUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_1_PLUS_SHORT_TO_GND,
		LIGHT_OUTPUT_1_MINUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_1_MINUS_SHORT_TO_GND,
		LIGHT_OUTPUT_1_CIRCUIT_FAILURE,
		LIGHT_OUTPUT_1_OPEN,
		LIGHT_OUTPUT_1_SHORT,
		LIGHT_OUTPUT_1_OVER_CURRENT,
		LIGHT_OUTPUT_1_UNDER_CURRENT,
		LIGHT_OUTPUT_2_SHORT_TO_BATT,
		LIGHT_OUTPUT_2_SHORT_TO_GND,
		LIGHT_OUTPUT_2_PLUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_2_PLUS_SHORT_TO_GND,
		LIGHT_OUTPUT_2_MINUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_2_MINUS_SHORT_TO_GND,
		LIGHT_OUTPUT_2_CIRCUIT_FAILURE,
		LIGHT_OUTPUT_2_OPEN,
		LIGHT_OUTPUT_2_SHORT,
		LIGHT_OUTPUT_2_OVER_CURRENT,
		LIGHT_OUTPUT_2_UNDER_CURRENT,
		LIGHT_OUTPUT_3_SHORT_TO_BATT,
		LIGHT_OUTPUT_3_SHORT_TO_GND,
		LIGHT_OUTPUT_3_PLUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_3_PLUS_SHORT_TO_GND,
		LIGHT_OUTPUT_3_MINUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_3_MINUS_SHORT_TO_GND,
		LIGHT_OUTPUT_3_CIRCUIT_FAILURE,
		LIGHT_OUTPUT_3_OPEN,
		LIGHT_OUTPUT_3_SHORT,
		LIGHT_OUTPUT_3_OVER_CURRENT,
		LIGHT_OUTPUT_3_UNDER_CURRENT,
		LIGHT_OUTPUT_4_SHORT_TO_BATT,
		LIGHT_OUTPUT_4_SHORT_TO_GND,
		LIGHT_OUTPUT_4_PLUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_4_PLUS_SHORT_TO_GND,
		LIGHT_OUTPUT_4_MINUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_4_MINUS_SHORT_TO_GND,
		LIGHT_OUTPUT_4_CIRCUIT_FAILURE,
		LIGHT_OUTPUT_4_OPEN,
		LIGHT_OUTPUT_4_SHORT,
		LIGHT_OUTPUT_4_OVER_CURRENT,
		LIGHT_OUTPUT_4_UNDER_CURRENT,
		LIGHT_OUTPUT_5_SHORT_TO_BATT,
		LIGHT_OUTPUT_5_SHORT_TO_GND,
		LIGHT_OUTPUT_5_PLUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_5_PLUS_SHORT_TO_GND,
		LIGHT_OUTPUT_5_MINUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_5_MINUS_SHORT_TO_GND,
		LIGHT_OUTPUT_5_CIRCUIT_FAILURE,
		LIGHT_OUTPUT_5_OPEN,
		LIGHT_OUTPUT_5_SHORT,
		LIGHT_OUTPUT_5_OVER_CURRENT,
		LIGHT_OUTPUT_5_UNDER_CURRENT,
		LIGHT_OUTPUT_6_SHORT_TO_BATT,
		LIGHT_OUTPUT_6_SHORT_TO_GND,
		LIGHT_OUTPUT_6_PLUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_6_PLUS_SHORT_TO_GND,
		LIGHT_OUTPUT_6_MINUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_6_MINUS_SHORT_TO_GND,
		LIGHT_OUTPUT_6_CIRCUIT_FAILURE,
		LIGHT_OUTPUT_6_OPEN,
		LIGHT_OUTPUT_6_SHORT,
		LIGHT_OUTPUT_6_OVER_CURRENT,
		LIGHT_OUTPUT_6_UNDER_CURRENT,
		LIGHT_OUTPUT_7_SHORT_TO_BATT,
		LIGHT_OUTPUT_7_SHORT_TO_GND,
		LIGHT_OUTPUT_7_PLUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_7_PLUS_SHORT_TO_GND,
		LIGHT_OUTPUT_7_MINUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_7_MINUS_SHORT_TO_GND,
		LIGHT_OUTPUT_7_CIRCUIT_FAILURE,
		LIGHT_OUTPUT_7_OPEN,
		LIGHT_OUTPUT_7_SHORT,
		LIGHT_OUTPUT_7_OVER_CURRENT,
		LIGHT_OUTPUT_7_UNDER_CURRENT,
		LIGHT_OUTPUT_8_SHORT_TO_BATT,
		LIGHT_OUTPUT_8_SHORT_TO_GND,
		LIGHT_OUTPUT_8_PLUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_8_PLUS_SHORT_TO_GND,
		LIGHT_OUTPUT_8_MINUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_8_MINUS_SHORT_TO_GND,
		LIGHT_OUTPUT_8_CIRCUIT_FAILURE,
		LIGHT_OUTPUT_8_OPEN,
		LIGHT_OUTPUT_8_SHORT,
		LIGHT_OUTPUT_8_OVER_CURRENT,
		LIGHT_OUTPUT_8_UNDER_CURRENT,
		LIGHT_OUTPUT_9_SHORT_TO_BATT,
		LIGHT_OUTPUT_9_SHORT_TO_GND,
		LIGHT_OUTPUT_9_PLUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_9_PLUS_SHORT_TO_GND,
		LIGHT_OUTPUT_9_MINUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_9_MINUS_SHORT_TO_GND,
		LIGHT_OUTPUT_9_CIRCUIT_FAILURE,
		LIGHT_OUTPUT_9_OPEN,
		LIGHT_OUTPUT_9_SHORT,
		LIGHT_OUTPUT_9_OVER_CURRENT,
		LIGHT_OUTPUT_9_UNDER_CURRENT,
		LIGHT_OUTPUT_10_SHORT_TO_BATT,
		LIGHT_OUTPUT_10_SHORT_TO_GND,
		LIGHT_OUTPUT_10_PLUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_10_PLUS_SHORT_TO_GND,
		LIGHT_OUTPUT_10_MINUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_10_MINUS_SHORT_TO_GND,
		LIGHT_OUTPUT_10_CIRCUIT_FAILURE,
		LIGHT_OUTPUT_10_OPEN,
		LIGHT_OUTPUT_10_SHORT,
		LIGHT_OUTPUT_10_OVER_CURRENT,
		LIGHT_OUTPUT_10_UNDER_CURRENT,
		LIGHT_OUTPUT_11_SHORT_TO_BATT,
		LIGHT_OUTPUT_11_SHORT_TO_GND,
		LIGHT_OUTPUT_11_PLUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_11_PLUS_SHORT_TO_GND,
		LIGHT_OUTPUT_11_MINUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_11_MINUS_SHORT_TO_GND,
		LIGHT_OUTPUT_11_CIRCUIT_FAILURE,
		LIGHT_OUTPUT_11_OPEN,
		LIGHT_OUTPUT_11_SHORT,
		LIGHT_OUTPUT_11_OVER_CURRENT,
		LIGHT_OUTPUT_11_UNDER_CURRENT,
		LIGHT_OUTPUT_12_SHORT_TO_BATT,
		LIGHT_OUTPUT_12_SHORT_TO_GND,
		LIGHT_OUTPUT_12_PLUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_12_PLUS_SHORT_TO_GND,
		LIGHT_OUTPUT_12_MINUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_12_MINUS_SHORT_TO_GND,
		LIGHT_OUTPUT_12_CIRCUIT_FAILURE,
		LIGHT_OUTPUT_12_OPEN,
		LIGHT_OUTPUT_12_SHORT,
		LIGHT_OUTPUT_12_OVER_CURRENT,
		LIGHT_OUTPUT_12_UNDER_CURRENT,
		LIGHT_OUTPUT_13_SHORT_TO_BATT,
		LIGHT_OUTPUT_13_SHORT_TO_GND,
		LIGHT_OUTPUT_13_PLUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_13_PLUS_SHORT_TO_GND,
		LIGHT_OUTPUT_13_MINUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_13_MINUS_SHORT_TO_GND,
		LIGHT_OUTPUT_13_CIRCUIT_FAILURE,
		LIGHT_OUTPUT_13_OPEN,
		LIGHT_OUTPUT_13_SHORT,
		LIGHT_OUTPUT_13_OVER_CURRENT,
		LIGHT_OUTPUT_13_UNDER_CURRENT,
		LIGHT_OUTPUT_14_SHORT_TO_BATT,
		LIGHT_OUTPUT_14_SHORT_TO_GND,
		LIGHT_OUTPUT_14_PLUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_14_PLUS_SHORT_TO_GND,
		LIGHT_OUTPUT_14_MINUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_14_MINUS_SHORT_TO_GND,
		LIGHT_OUTPUT_14_CIRCUIT_FAILURE,
		LIGHT_OUTPUT_14_OPEN,
		LIGHT_OUTPUT_14_SHORT,
		LIGHT_OUTPUT_14_OVER_CURRENT,
		LIGHT_OUTPUT_14_UNDER_CURRENT,
		LIGHT_OUTPUT_15_SHORT_TO_BATT,
		LIGHT_OUTPUT_15_SHORT_TO_GND,
		LIGHT_OUTPUT_15_PLUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_15_PLUS_SHORT_TO_GND,
		LIGHT_OUTPUT_15_MINUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_15_MINUS_SHORT_TO_GND,
		LIGHT_OUTPUT_15_CIRCUIT_FAILURE,
		LIGHT_OUTPUT_15_OPEN,
		LIGHT_OUTPUT_15_SHORT,
		LIGHT_OUTPUT_15_OVER_CURRENT,
		LIGHT_OUTPUT_15_UNDER_CURRENT,
		LIGHT_OUTPUT_16_SHORT_TO_BATT,
		LIGHT_OUTPUT_16_SHORT_TO_GND,
		LIGHT_OUTPUT_16_PLUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_16_PLUS_SHORT_TO_GND,
		LIGHT_OUTPUT_16_MINUS_SHORT_TO_BATT,
		LIGHT_OUTPUT_16_MINUS_SHORT_TO_GND,
		LIGHT_OUTPUT_16_CIRCUIT_FAILURE,
		LIGHT_OUTPUT_16_OPEN,
		LIGHT_OUTPUT_16_SHORT,
		LIGHT_OUTPUT_16_OVER_CURRENT,
		LIGHT_OUTPUT_16_UNDER_CURRENT,
		RGB_OUTPUT_R_SHORT_TO_BATT,
		RGB_OUTPUT_R_SHORT_TO_GND,
		RGB_OUTPUT_R_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_R_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_R_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_R_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_R_CIRCUIT_FAILURE,
		RGB_OUTPUT_R_OPEN,
		RGB_OUTPUT_R_SHORT,
		RGB_OUTPUT_R_OVER_CURRENT,
		RGB_OUTPUT_R_UNDER_CURRENT,
		RGB_OUTPUT_G_SHORT_TO_BATT,
		RGB_OUTPUT_G_SHORT_TO_GND,
		RGB_OUTPUT_G_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_G_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_G_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_G_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_G_CIRCUIT_FAILURE,
		RGB_OUTPUT_G_OPEN,
		RGB_OUTPUT_G_SHORT,
		RGB_OUTPUT_G_OVER_CURRENT,
		RGB_OUTPUT_G_UNDER_CURRENT,
		RGB_OUTPUT_B_SHORT_TO_BATT,
		RGB_OUTPUT_B_SHORT_TO_GND,
		RGB_OUTPUT_B_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_B_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_B_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_B_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_B_CIRCUIT_FAILURE,
		RGB_OUTPUT_B_OPEN,
		RGB_OUTPUT_B_SHORT,
		RGB_OUTPUT_B_OVER_CURRENT,
		RGB_OUTPUT_B_UNDER_CURRENT,
		RGB_OUTPUT_R1_SHORT_TO_BATT,
		RGB_OUTPUT_R1_SHORT_TO_GND,
		RGB_OUTPUT_R1_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_R1_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_R1_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_R1_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_R1_CIRCUIT_FAILURE,
		RGB_OUTPUT_R1_OPEN,
		RGB_OUTPUT_R1_SHORT,
		RGB_OUTPUT_R1_OVER_CURRENT,
		RGB_OUTPUT_R1_UNDER_CURRENT,
		RGB_OUTPUT_G1_SHORT_TO_BATT,
		RGB_OUTPUT_G1_SHORT_TO_GND,
		RGB_OUTPUT_G1_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_G1_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_G1_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_G1_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_G1_CIRCUIT_FAILURE,
		RGB_OUTPUT_G1_OPEN,
		RGB_OUTPUT_G1_SHORT,
		RGB_OUTPUT_G1_OVER_CURRENT,
		RGB_OUTPUT_G1_UNDER_CURRENT,
		RGB_OUTPUT_B1_SHORT_TO_BATT,
		RGB_OUTPUT_B1_SHORT_TO_GND,
		RGB_OUTPUT_B1_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_B1_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_B1_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_B1_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_B1_CIRCUIT_FAILURE,
		RGB_OUTPUT_B1_OPEN,
		RGB_OUTPUT_B1_SHORT,
		RGB_OUTPUT_B1_OVER_CURRENT,
		RGB_OUTPUT_B1_UNDER_CURRENT,
		RGB_OUTPUT_R2_SHORT_TO_BATT,
		RGB_OUTPUT_R2_SHORT_TO_GND,
		RGB_OUTPUT_R2_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_R2_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_R2_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_R2_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_R2_CIRCUIT_FAILURE,
		RGB_OUTPUT_R2_OPEN,
		RGB_OUTPUT_R2_SHORT,
		RGB_OUTPUT_R2_OVER_CURRENT,
		RGB_OUTPUT_R2_UNDER_CURRENT,
		RGB_OUTPUT_G2_SHORT_TO_BATT,
		RGB_OUTPUT_G2_SHORT_TO_GND,
		RGB_OUTPUT_G2_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_G2_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_G2_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_G2_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_G2_CIRCUIT_FAILURE,
		RGB_OUTPUT_G2_OPEN,
		RGB_OUTPUT_G2_SHORT,
		RGB_OUTPUT_G2_OVER_CURRENT,
		RGB_OUTPUT_G2_UNDER_CURRENT,
		RGB_OUTPUT_B2_SHORT_TO_BATT,
		RGB_OUTPUT_B2_SHORT_TO_GND,
		RGB_OUTPUT_B2_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_B2_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_B2_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_B2_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_B2_CIRCUIT_FAILURE,
		RGB_OUTPUT_B2_OPEN,
		RGB_OUTPUT_B2_SHORT,
		RGB_OUTPUT_B2_OVER_CURRENT,
		RGB_OUTPUT_B2_UNDER_CURRENT,
		RGB_OUTPUT_R3_SHORT_TO_BATT,
		RGB_OUTPUT_R3_SHORT_TO_GND,
		RGB_OUTPUT_R3_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_R3_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_R3_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_R3_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_R3_CIRCUIT_FAILURE,
		RGB_OUTPUT_R3_OPEN,
		RGB_OUTPUT_R3_SHORT,
		RGB_OUTPUT_R3_OVER_CURRENT,
		RGB_OUTPUT_R3_UNDER_CURRENT,
		RGB_OUTPUT_G3_SHORT_TO_BATT,
		RGB_OUTPUT_G3_SHORT_TO_GND,
		RGB_OUTPUT_G3_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_G3_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_G3_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_G3_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_G3_CIRCUIT_FAILURE,
		RGB_OUTPUT_G3_OPEN,
		RGB_OUTPUT_G3_SHORT,
		RGB_OUTPUT_G3_OVER_CURRENT,
		RGB_OUTPUT_G3_UNDER_CURRENT,
		RGB_OUTPUT_B3_SHORT_TO_BATT,
		RGB_OUTPUT_B3_SHORT_TO_GND,
		RGB_OUTPUT_B3_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_B3_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_B3_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_B3_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_B3_CIRCUIT_FAILURE,
		RGB_OUTPUT_B3_OPEN,
		RGB_OUTPUT_B3_SHORT,
		RGB_OUTPUT_B3_OVER_CURRENT,
		RGB_OUTPUT_B3_UNDER_CURRENT,
		RGB_OUTPUT_R4_SHORT_TO_BATT,
		RGB_OUTPUT_R4_SHORT_TO_GND,
		RGB_OUTPUT_R4_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_R4_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_R4_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_R4_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_R4_CIRCUIT_FAILURE,
		RGB_OUTPUT_R4_OPEN,
		RGB_OUTPUT_R4_SHORT,
		RGB_OUTPUT_R4_OVER_CURRENT,
		RGB_OUTPUT_R4_UNDER_CURRENT,
		RGB_OUTPUT_G4_SHORT_TO_BATT,
		RGB_OUTPUT_G4_SHORT_TO_GND,
		RGB_OUTPUT_G4_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_G4_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_G4_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_G4_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_G4_CIRCUIT_FAILURE,
		RGB_OUTPUT_G4_OPEN,
		RGB_OUTPUT_G4_SHORT,
		RGB_OUTPUT_G4_OVER_CURRENT,
		RGB_OUTPUT_G4_UNDER_CURRENT,
		RGB_OUTPUT_B4_SHORT_TO_BATT,
		RGB_OUTPUT_B4_SHORT_TO_GND,
		RGB_OUTPUT_B4_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_B4_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_B4_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_B4_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_B4_CIRCUIT_FAILURE,
		RGB_OUTPUT_B4_OPEN,
		RGB_OUTPUT_B4_SHORT,
		RGB_OUTPUT_B4_OVER_CURRENT,
		RGB_OUTPUT_B4_UNDER_CURRENT,
		RGB_OUTPUT_R5_SHORT_TO_BATT,
		RGB_OUTPUT_R5_SHORT_TO_GND,
		RGB_OUTPUT_R5_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_R5_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_R5_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_R5_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_R5_CIRCUIT_FAILURE,
		RGB_OUTPUT_R5_OPEN,
		RGB_OUTPUT_R5_SHORT,
		RGB_OUTPUT_R5_OVER_CURRENT,
		RGB_OUTPUT_R5_UNDER_CURRENT,
		RGB_OUTPUT_G5_SHORT_TO_BATT,
		RGB_OUTPUT_G5_SHORT_TO_GND,
		RGB_OUTPUT_G5_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_G5_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_G5_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_G5_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_G5_CIRCUIT_FAILURE,
		RGB_OUTPUT_G5_OPEN,
		RGB_OUTPUT_G5_SHORT,
		RGB_OUTPUT_G5_OVER_CURRENT,
		RGB_OUTPUT_G5_UNDER_CURRENT,
		RGB_OUTPUT_B5_SHORT_TO_BATT,
		RGB_OUTPUT_B5_SHORT_TO_GND,
		RGB_OUTPUT_B5_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_B5_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_B5_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_B5_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_B5_CIRCUIT_FAILURE,
		RGB_OUTPUT_B5_OPEN,
		RGB_OUTPUT_B5_SHORT,
		RGB_OUTPUT_B5_OVER_CURRENT,
		RGB_OUTPUT_B5_UNDER_CURRENT,
		RGB_OUTPUT_R6_SHORT_TO_BATT,
		RGB_OUTPUT_R6_SHORT_TO_GND,
		RGB_OUTPUT_R6_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_R6_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_R6_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_R6_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_R6_CIRCUIT_FAILURE,
		RGB_OUTPUT_R6_OPEN,
		RGB_OUTPUT_R6_SHORT,
		RGB_OUTPUT_R6_OVER_CURRENT,
		RGB_OUTPUT_R6_UNDER_CURRENT,
		RGB_OUTPUT_G6_SHORT_TO_BATT,
		RGB_OUTPUT_G6_SHORT_TO_GND,
		RGB_OUTPUT_G6_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_G6_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_G6_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_G6_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_G6_CIRCUIT_FAILURE,
		RGB_OUTPUT_G6_OPEN,
		RGB_OUTPUT_G6_SHORT,
		RGB_OUTPUT_G6_OVER_CURRENT,
		RGB_OUTPUT_G6_UNDER_CURRENT,
		RGB_OUTPUT_B6_SHORT_TO_BATT,
		RGB_OUTPUT_B6_SHORT_TO_GND,
		RGB_OUTPUT_B6_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_B6_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_B6_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_B6_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_B6_CIRCUIT_FAILURE,
		RGB_OUTPUT_B6_OPEN,
		RGB_OUTPUT_B6_SHORT,
		RGB_OUTPUT_B6_OVER_CURRENT,
		RGB_OUTPUT_B6_UNDER_CURRENT,
		RGB_OUTPUT_R7_SHORT_TO_BATT,
		RGB_OUTPUT_R7_SHORT_TO_GND,
		RGB_OUTPUT_R7_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_R7_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_R7_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_R7_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_R7_CIRCUIT_FAILURE,
		RGB_OUTPUT_R7_OPEN,
		RGB_OUTPUT_R7_SHORT,
		RGB_OUTPUT_R7_OVER_CURRENT,
		RGB_OUTPUT_R7_UNDER_CURRENT,
		RGB_OUTPUT_G7_SHORT_TO_BATT,
		RGB_OUTPUT_G7_SHORT_TO_GND,
		RGB_OUTPUT_G7_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_G7_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_G7_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_G7_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_G7_CIRCUIT_FAILURE,
		RGB_OUTPUT_G7_OPEN,
		RGB_OUTPUT_G7_SHORT,
		RGB_OUTPUT_G7_OVER_CURRENT,
		RGB_OUTPUT_G7_UNDER_CURRENT,
		RGB_OUTPUT_B7_SHORT_TO_BATT,
		RGB_OUTPUT_B7_SHORT_TO_GND,
		RGB_OUTPUT_B7_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_B7_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_B7_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_B7_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_B7_CIRCUIT_FAILURE,
		RGB_OUTPUT_B7_OPEN,
		RGB_OUTPUT_B7_SHORT,
		RGB_OUTPUT_B7_OVER_CURRENT,
		RGB_OUTPUT_B7_UNDER_CURRENT,
		RGB_OUTPUT_R8_SHORT_TO_BATT,
		RGB_OUTPUT_R8_SHORT_TO_GND,
		RGB_OUTPUT_R8_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_R8_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_R8_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_R8_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_R8_CIRCUIT_FAILURE,
		RGB_OUTPUT_R8_OPEN,
		RGB_OUTPUT_R8_SHORT,
		RGB_OUTPUT_R8_OVER_CURRENT,
		RGB_OUTPUT_R8_UNDER_CURRENT,
		RGB_OUTPUT_G8_SHORT_TO_BATT,
		RGB_OUTPUT_G8_SHORT_TO_GND,
		RGB_OUTPUT_G8_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_G8_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_G8_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_G8_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_G8_CIRCUIT_FAILURE,
		RGB_OUTPUT_G8_OPEN,
		RGB_OUTPUT_G8_SHORT,
		RGB_OUTPUT_G8_OVER_CURRENT,
		RGB_OUTPUT_G8_UNDER_CURRENT,
		RGB_OUTPUT_B8_SHORT_TO_BATT,
		RGB_OUTPUT_B8_SHORT_TO_GND,
		RGB_OUTPUT_B8_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_B8_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_B8_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_B8_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_B8_CIRCUIT_FAILURE,
		RGB_OUTPUT_B8_OPEN,
		RGB_OUTPUT_B8_SHORT,
		RGB_OUTPUT_B8_OVER_CURRENT,
		RGB_OUTPUT_B8_UNDER_CURRENT,
		RGB_OUTPUT_R9_SHORT_TO_BATT,
		RGB_OUTPUT_R9_SHORT_TO_GND,
		RGB_OUTPUT_R9_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_R9_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_R9_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_R9_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_R9_CIRCUIT_FAILURE,
		RGB_OUTPUT_R9_OPEN,
		RGB_OUTPUT_R9_SHORT,
		RGB_OUTPUT_R9_OVER_CURRENT,
		RGB_OUTPUT_R9_UNDER_CURRENT,
		RGB_OUTPUT_G9_SHORT_TO_BATT,
		RGB_OUTPUT_G9_SHORT_TO_GND,
		RGB_OUTPUT_G9_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_G9_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_G9_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_G9_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_G9_CIRCUIT_FAILURE,
		RGB_OUTPUT_G9_OPEN,
		RGB_OUTPUT_G9_SHORT,
		RGB_OUTPUT_G9_OVER_CURRENT,
		RGB_OUTPUT_G9_UNDER_CURRENT,
		RGB_OUTPUT_B9_SHORT_TO_BATT,
		RGB_OUTPUT_B9_SHORT_TO_GND,
		RGB_OUTPUT_B9_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_B9_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_B9_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_B9_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_B9_CIRCUIT_FAILURE,
		RGB_OUTPUT_B9_OPEN,
		RGB_OUTPUT_B9_SHORT,
		RGB_OUTPUT_B9_OVER_CURRENT,
		RGB_OUTPUT_B9_UNDER_CURRENT,
		RGB_OUTPUT_R10_SHORT_TO_BATT,
		RGB_OUTPUT_R10_SHORT_TO_GND,
		RGB_OUTPUT_R10_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_R10_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_R10_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_R10_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_R10_CIRCUIT_FAILURE,
		RGB_OUTPUT_R10_OPEN,
		RGB_OUTPUT_R10_SHORT,
		RGB_OUTPUT_R10_OVER_CURRENT,
		RGB_OUTPUT_R10_UNDER_CURRENT,
		RGB_OUTPUT_G10_SHORT_TO_BATT,
		RGB_OUTPUT_G10_SHORT_TO_GND,
		RGB_OUTPUT_G10_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_G10_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_G10_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_G10_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_G10_CIRCUIT_FAILURE,
		RGB_OUTPUT_G10_OPEN,
		RGB_OUTPUT_G10_SHORT,
		RGB_OUTPUT_G10_OVER_CURRENT,
		RGB_OUTPUT_G10_UNDER_CURRENT,
		RGB_OUTPUT_B10_SHORT_TO_BATT,
		RGB_OUTPUT_B10_SHORT_TO_GND,
		RGB_OUTPUT_B10_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_B10_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_B10_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_B10_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_B10_CIRCUIT_FAILURE,
		RGB_OUTPUT_B10_OPEN,
		RGB_OUTPUT_B10_SHORT,
		RGB_OUTPUT_B10_OVER_CURRENT,
		RGB_OUTPUT_B10_UNDER_CURRENT,
		RGB_OUTPUT_R11_SHORT_TO_BATT,
		RGB_OUTPUT_R11_SHORT_TO_GND,
		RGB_OUTPUT_R11_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_R11_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_R11_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_R11_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_R11_CIRCUIT_FAILURE,
		RGB_OUTPUT_R11_OPEN,
		RGB_OUTPUT_R11_SHORT,
		RGB_OUTPUT_R11_OVER_CURRENT,
		RGB_OUTPUT_R11_UNDER_CURRENT,
		RGB_OUTPUT_G11_SHORT_TO_BATT,
		RGB_OUTPUT_G11_SHORT_TO_GND,
		RGB_OUTPUT_G11_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_G11_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_G11_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_G11_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_G11_CIRCUIT_FAILURE,
		RGB_OUTPUT_G11_OPEN,
		RGB_OUTPUT_G11_SHORT,
		RGB_OUTPUT_G11_OVER_CURRENT,
		RGB_OUTPUT_G11_UNDER_CURRENT,
		RGB_OUTPUT_B11_SHORT_TO_BATT,
		RGB_OUTPUT_B11_SHORT_TO_GND,
		RGB_OUTPUT_B11_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_B11_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_B11_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_B11_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_B11_CIRCUIT_FAILURE,
		RGB_OUTPUT_B11_OPEN,
		RGB_OUTPUT_B11_SHORT,
		RGB_OUTPUT_B11_OVER_CURRENT,
		RGB_OUTPUT_B11_UNDER_CURRENT,
		RGB_OUTPUT_R12_SHORT_TO_BATT,
		RGB_OUTPUT_R12_SHORT_TO_GND,
		RGB_OUTPUT_R12_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_R12_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_R12_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_R12_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_R12_CIRCUIT_FAILURE,
		RGB_OUTPUT_R12_OPEN,
		RGB_OUTPUT_R12_SHORT,
		RGB_OUTPUT_R12_OVER_CURRENT,
		RGB_OUTPUT_R12_UNDER_CURRENT,
		RGB_OUTPUT_G12_SHORT_TO_BATT,
		RGB_OUTPUT_G12_SHORT_TO_GND,
		RGB_OUTPUT_G12_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_G12_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_G12_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_G12_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_G12_CIRCUIT_FAILURE,
		RGB_OUTPUT_G12_OPEN,
		RGB_OUTPUT_G12_SHORT,
		RGB_OUTPUT_G12_OVER_CURRENT,
		RGB_OUTPUT_G12_UNDER_CURRENT,
		RGB_OUTPUT_B12_SHORT_TO_BATT,
		RGB_OUTPUT_B12_SHORT_TO_GND,
		RGB_OUTPUT_B12_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_B12_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_B12_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_B12_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_B12_CIRCUIT_FAILURE,
		RGB_OUTPUT_B12_OPEN,
		RGB_OUTPUT_B12_SHORT,
		RGB_OUTPUT_B12_OVER_CURRENT,
		RGB_OUTPUT_B12_UNDER_CURRENT,
		RGB_OUTPUT_R13_SHORT_TO_BATT,
		RGB_OUTPUT_R13_SHORT_TO_GND,
		RGB_OUTPUT_R13_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_R13_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_R13_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_R13_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_R13_CIRCUIT_FAILURE,
		RGB_OUTPUT_R13_OPEN,
		RGB_OUTPUT_R13_SHORT,
		RGB_OUTPUT_R13_OVER_CURRENT,
		RGB_OUTPUT_R13_UNDER_CURRENT,
		RGB_OUTPUT_G13_SHORT_TO_BATT,
		RGB_OUTPUT_G13_SHORT_TO_GND,
		RGB_OUTPUT_G13_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_G13_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_G13_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_G13_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_G13_CIRCUIT_FAILURE,
		RGB_OUTPUT_G13_OPEN,
		RGB_OUTPUT_G13_SHORT,
		RGB_OUTPUT_G13_OVER_CURRENT,
		RGB_OUTPUT_G13_UNDER_CURRENT,
		RGB_OUTPUT_B13_SHORT_TO_BATT,
		RGB_OUTPUT_B13_SHORT_TO_GND,
		RGB_OUTPUT_B13_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_B13_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_B13_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_B13_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_B13_CIRCUIT_FAILURE,
		RGB_OUTPUT_B13_OPEN,
		RGB_OUTPUT_B13_SHORT,
		RGB_OUTPUT_B13_OVER_CURRENT,
		RGB_OUTPUT_B13_UNDER_CURRENT,
		RGB_OUTPUT_R14_SHORT_TO_BATT,
		RGB_OUTPUT_R14_SHORT_TO_GND,
		RGB_OUTPUT_R14_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_R14_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_R14_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_R14_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_R14_CIRCUIT_FAILURE,
		RGB_OUTPUT_R14_OPEN,
		RGB_OUTPUT_R14_SHORT,
		RGB_OUTPUT_R14_OVER_CURRENT,
		RGB_OUTPUT_R14_UNDER_CURRENT,
		RGB_OUTPUT_G14_SHORT_TO_BATT,
		RGB_OUTPUT_G14_SHORT_TO_GND,
		RGB_OUTPUT_G14_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_G14_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_G14_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_G14_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_G14_CIRCUIT_FAILURE,
		RGB_OUTPUT_G14_OPEN,
		RGB_OUTPUT_G14_SHORT,
		RGB_OUTPUT_G14_OVER_CURRENT,
		RGB_OUTPUT_G14_UNDER_CURRENT,
		RGB_OUTPUT_B14_SHORT_TO_BATT,
		RGB_OUTPUT_B14_SHORT_TO_GND,
		RGB_OUTPUT_B14_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_B14_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_B14_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_B14_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_B14_CIRCUIT_FAILURE,
		RGB_OUTPUT_B14_OPEN,
		RGB_OUTPUT_B14_SHORT,
		RGB_OUTPUT_B14_OVER_CURRENT,
		RGB_OUTPUT_B14_UNDER_CURRENT,
		RGB_OUTPUT_R15_SHORT_TO_BATT,
		RGB_OUTPUT_R15_SHORT_TO_GND,
		RGB_OUTPUT_R15_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_R15_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_R15_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_R15_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_R15_CIRCUIT_FAILURE,
		RGB_OUTPUT_R15_OPEN,
		RGB_OUTPUT_R15_SHORT,
		RGB_OUTPUT_R15_OVER_CURRENT,
		RGB_OUTPUT_R15_UNDER_CURRENT,
		RGB_OUTPUT_G15_SHORT_TO_BATT,
		RGB_OUTPUT_G15_SHORT_TO_GND,
		RGB_OUTPUT_G15_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_G15_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_G15_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_G15_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_G15_CIRCUIT_FAILURE,
		RGB_OUTPUT_G15_OPEN,
		RGB_OUTPUT_G15_SHORT,
		RGB_OUTPUT_G15_OVER_CURRENT,
		RGB_OUTPUT_G15_UNDER_CURRENT,
		RGB_OUTPUT_B15_SHORT_TO_BATT,
		RGB_OUTPUT_B15_SHORT_TO_GND,
		RGB_OUTPUT_B15_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_B15_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_B15_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_B15_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_B15_CIRCUIT_FAILURE,
		RGB_OUTPUT_B15_OPEN,
		RGB_OUTPUT_B15_SHORT,
		RGB_OUTPUT_B15_OVER_CURRENT,
		RGB_OUTPUT_B15_UNDER_CURRENT,
		RGB_OUTPUT_R16_SHORT_TO_BATT,
		RGB_OUTPUT_R16_SHORT_TO_GND,
		RGB_OUTPUT_R16_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_R16_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_R16_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_R16_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_R16_CIRCUIT_FAILURE,
		RGB_OUTPUT_R16_OPEN,
		RGB_OUTPUT_R16_SHORT,
		RGB_OUTPUT_R16_OVER_CURRENT,
		RGB_OUTPUT_R16_UNDER_CURRENT,
		RGB_OUTPUT_G16_SHORT_TO_BATT,
		RGB_OUTPUT_G16_SHORT_TO_GND,
		RGB_OUTPUT_G16_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_G16_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_G16_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_G16_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_G16_CIRCUIT_FAILURE,
		RGB_OUTPUT_G16_OPEN,
		RGB_OUTPUT_G16_SHORT,
		RGB_OUTPUT_G16_OVER_CURRENT,
		RGB_OUTPUT_G16_UNDER_CURRENT,
		RGB_OUTPUT_B16_SHORT_TO_BATT,
		RGB_OUTPUT_B16_SHORT_TO_GND,
		RGB_OUTPUT_B16_PLUS_SHORT_TO_BATT,
		RGB_OUTPUT_B16_PLUS_SHORT_TO_GND,
		RGB_OUTPUT_B16_MINUS_SHORT_TO_BATT,
		RGB_OUTPUT_B16_MINUS_SHORT_TO_GND,
		RGB_OUTPUT_B16_CIRCUIT_FAILURE,
		RGB_OUTPUT_B16_OPEN,
		RGB_OUTPUT_B16_SHORT,
		RGB_OUTPUT_B16_OVER_CURRENT,
		RGB_OUTPUT_B16_UNDER_CURRENT,
		HALL_EFFECT_POWER_SHORT_TO_BATT,
		HALL_EFFECT_POWER_SHORT_TO_GND,
		HALL_EFFECT_POWER_PLUS_SHORT_TO_BATT,
		HALL_EFFECT_POWER_PLUS_SHORT_TO_GND,
		HALL_EFFECT_POWER_MINUS_SHORT_TO_BATT,
		HALL_EFFECT_POWER_MINUS_SHORT_TO_GND,
		HALL_EFFECT_POWER_CIRCUIT_FAILURE,
		HALL_EFFECT_POWER_OPEN,
		HALL_EFFECT_POWER_SHORT,
		HALL_EFFECT_POWER_OVER_CURRENT,
		HALL_EFFECT_POWER_UNDER_CURRENT,
		JACK_LF_SHORT_TO_BATT,
		JACK_LF_SHORT_TO_GND,
		JACK_LF_PLUS_SHORT_TO_BATT,
		JACK_LF_PLUS_SHORT_TO_GND,
		JACK_LF_MINUS_SHORT_TO_BATT,
		JACK_LF_MINUS_SHORT_TO_GND,
		JACK_LF_CIRCUIT_FAILURE,
		JACK_LF_OPEN,
		JACK_LF_SHORT,
		JACK_LF_OVER_CURRENT,
		JACK_LF_UNDER_CURRENT,
		JACK_LF_OUT_OF_STROKE,
		JACK_LF_HALL_EFFECT_SIGNAL_LOST,
		JACK_LF_POSITION_LOST,
		JACK_LM_SHORT_TO_BATT,
		JACK_LM_SHORT_TO_GND,
		JACK_LM_PLUS_SHORT_TO_BATT,
		JACK_LM_PLUS_SHORT_TO_GND,
		JACK_LM_MINUS_SHORT_TO_BATT,
		JACK_LM_MINUS_SHORT_TO_GND,
		JACK_LM_CIRCUIT_FAILURE,
		JACK_LM_OPEN,
		JACK_LM_SHORT,
		JACK_LM_OVER_CURRENT,
		JACK_LM_UNDER_CURRENT,
		JACK_LM_OUT_OF_STROKE,
		JACK_LM_HALL_EFFECT_SIGNAL_LOST,
		JACK_LM_POSITION_LOST,
		JACK_LR_SHORT_TO_BATT,
		JACK_LR_SHORT_TO_GND,
		JACK_LR_PLUS_SHORT_TO_BATT,
		JACK_LR_PLUS_SHORT_TO_GND,
		JACK_LR_MINUS_SHORT_TO_BATT,
		JACK_LR_MINUS_SHORT_TO_GND,
		JACK_LR_CIRCUIT_FAILURE,
		JACK_LR_OPEN,
		JACK_LR_SHORT,
		JACK_LR_OVER_CURRENT,
		JACK_LR_UNDER_CURRENT,
		JACK_LR_OUT_OF_STROKE,
		JACK_LR_HALL_EFFECT_SIGNAL_LOST,
		JACK_LR_POSITION_LOST,
		JACK_RF_SHORT_TO_BATT,
		JACK_RF_SHORT_TO_GND,
		JACK_RF_PLUS_SHORT_TO_BATT,
		JACK_RF_PLUS_SHORT_TO_GND,
		JACK_RF_MINUS_SHORT_TO_BATT,
		JACK_RF_MINUS_SHORT_TO_GND,
		JACK_RF_CIRCUIT_FAILURE,
		JACK_RF_OPEN,
		JACK_RF_SHORT,
		JACK_RF_OVER_CURRENT,
		JACK_RF_UNDER_CURRENT,
		JACK_RF_OUT_OF_STROKE,
		JACK_RF_HALL_EFFECT_SIGNAL_LOST,
		JACK_RF_POSITION_LOST,
		JACK_RM_SHORT_TO_BATT,
		JACK_RM_SHORT_TO_GND,
		JACK_RM_PLUS_SHORT_TO_BATT,
		JACK_RM_PLUS_SHORT_TO_GND,
		JACK_RM_MINUS_SHORT_TO_BATT,
		JACK_RM_MINUS_SHORT_TO_GND,
		JACK_RM_CIRCUIT_FAILURE,
		JACK_RM_OPEN,
		JACK_RM_SHORT,
		JACK_RM_OVER_CURRENT,
		JACK_RM_UNDER_CURRENT,
		JACK_RM_OUT_OF_STROKE,
		JACK_RM_HALL_EFFECT_SIGNAL_LOST,
		JACK_RM_POSITION_LOST,
		JACK_RR_SHORT_TO_BATT,
		JACK_RR_SHORT_TO_GND,
		JACK_RR_PLUS_SHORT_TO_BATT,
		JACK_RR_PLUS_SHORT_TO_GND,
		JACK_RR_MINUS_SHORT_TO_BATT,
		JACK_RR_MINUS_SHORT_TO_GND,
		JACK_RR_CIRCUIT_FAILURE,
		JACK_RR_OPEN,
		JACK_RR_SHORT,
		JACK_RR_OVER_CURRENT,
		JACK_RR_UNDER_CURRENT,
		JACK_RR_OUT_OF_STROKE,
		JACK_RR_HALL_EFFECT_SIGNAL_LOST,
		JACK_RR_POSITION_LOST,
		TONGUE_JACK_SHORT_TO_BATT,
		TONGUE_JACK_SHORT_TO_GND,
		TONGUE_JACK_PLUS_SHORT_TO_BATT,
		TONGUE_JACK_PLUS_SHORT_TO_GND,
		TONGUE_JACK_MINUS_SHORT_TO_BATT,
		TONGUE_JACK_MINUS_SHORT_TO_GND,
		TONGUE_JACK_CIRCUIT_FAILURE,
		TONGUE_JACK_OPEN,
		TONGUE_JACK_SHORT,
		TONGUE_JACK_OVER_CURRENT,
		TONGUE_JACK_UNDER_CURRENT,
		TONGUE_JACK_OUT_OF_STROKE,
		TONGUE_JACK_HALL_EFFECT_SIGNAL_LOST,
		TONGUE_JACK_POSITION_LOST,
		STABILIZER_JACK_SHORT_TO_BATT,
		STABILIZER_JACK_SHORT_TO_GND,
		STABILIZER_JACK_PLUS_SHORT_TO_BATT,
		STABILIZER_JACK_PLUS_SHORT_TO_GND,
		STABILIZER_JACK_MINUS_SHORT_TO_BATT,
		STABILIZER_JACK_MINUS_SHORT_TO_GND,
		STABILIZER_JACK_CIRCUIT_FAILURE,
		STABILIZER_JACK_OPEN,
		STABILIZER_JACK_SHORT,
		STABILIZER_JACK_OVER_CURRENT,
		STABILIZER_JACK_UNDER_CURRENT,
		STABILIZER_JACK_OUT_OF_STROKE,
		STABILIZER_JACK_HALL_EFFECT_SIGNAL_LOST,
		STABILIZER_JACK_POSITION_LOST,
		FUSE_OPEN,
		FUSE_1_OPEN,
		FUSE_2_OPEN,
		FUSE_3_OPEN,
		FUSE_4_OPEN,
		FUSE_5_OPEN,
		FUSE_6_OPEN,
		FUSE_7_OPEN,
		FUSE_8_OPEN,
		FUSE_9_OPEN,
		FUSE_10_OPEN,
		FUSE_11_OPEN,
		FUSE_12_OPEN,
		FUSE_13_OPEN,
		FUSE_14_OPEN,
		FUSE_15_OPEN,
		FUSE_16_OPEN,
		FUSE_17_OPEN,
		FUSE_18_OPEN,
		FUSE_19_OPEN,
		FUSE_20_OPEN,
		INPUT_FUSE_OPEN,
		INPUT_1_FUSE_OPEN,
		INPUT_2_FUSE_OPEN,
		INPUT_3_FUSE_OPEN,
		INPUT_4_FUSE_OPEN,
		INPUT_5_FUSE_OPEN,
		INPUT_6_FUSE_OPEN,
		INPUT_7_FUSE_OPEN,
		INPUT_8_FUSE_OPEN,
		INPUT_9_FUSE_OPEN,
		INPUT_10_FUSE_OPEN,
		INPUT_11_FUSE_OPEN,
		INPUT_12_FUSE_OPEN,
		INPUT_13_FUSE_OPEN,
		INPUT_14_FUSE_OPEN,
		INPUT_15_FUSE_OPEN,
		INPUT_16_FUSE_OPEN,
		INPUT_17_FUSE_OPEN,
		INPUT_18_FUSE_OPEN,
		INPUT_19_FUSE_OPEN,
		INPUT_20_FUSE_OPEN,
		OUTPUT_FUSE_OPEN,
		OUTPUT_1_FUSE_OPEN,
		OUTPUT_2_FUSE_OPEN,
		OUTPUT_3_FUSE_OPEN,
		OUTPUT_4_FUSE_OPEN,
		OUTPUT_5_FUSE_OPEN,
		OUTPUT_6_FUSE_OPEN,
		OUTPUT_7_FUSE_OPEN,
		OUTPUT_8_FUSE_OPEN,
		OUTPUT_9_FUSE_OPEN,
		OUTPUT_10_FUSE_OPEN,
		OUTPUT_11_FUSE_OPEN,
		OUTPUT_12_FUSE_OPEN,
		OUTPUT_13_FUSE_OPEN,
		OUTPUT_14_FUSE_OPEN,
		OUTPUT_15_FUSE_OPEN,
		OUTPUT_16_FUSE_OPEN,
		OUTPUT_17_FUSE_OPEN,
		OUTPUT_18_FUSE_OPEN,
		OUTPUT_19_FUSE_OPEN,
		OUTPUT_20_FUSE_OPEN,
		GENERATOR_STARTED_UNEXPECTEDLY,
		GENERATOR_STOPPED_UNEXPECTEDLY,
		CLIMATE_ZONE_3_FREE_SENSOR_OPEN_CIRCUIT,
		CLIMATE_ZONE_3_FREE_SENSOR_SHORT_CIRCUIT,
		CLIMATE_ZONE_3_TEMP_SENSOR_OPEN_CIRCUIT,
		CLIMATE_ZONE_3_TEMP_SENSOR_SHORT_CIRCUIT,
		CLIMATE_ZONE_3_POWER_MISSING,
		CLIMATE_ZONE_3_DIP_SWITCH_INVALID,
		CLIMATE_ZONE_1_VOLTAGE_HIGH,
		CLIMATE_ZONE_1_VOLTAGE_LOW,
		CLIMATE_ZONE_2_VOLTAGE_HIGH,
		CLIMATE_ZONE_2_VOLTAGE_LOW,
		CLIMATE_ZONE_3_VOLTAGE_HIGH,
		CLIMATE_ZONE_3_VOLTAGE_LOW,
		EXTEND_SWITCH_SHORT_TO_GND,
		EXTEND_SWITCH_SHORT_TO_BATT,
		EXTEND_SWITCH_STUCK,
		EXTEND_SWITCH_FAULT,
		EXTEND_SWITCH_CIRCUIT_OPEN,
		RETRACT_SWITCH_SHORT_TO_GND,
		RETRACT_SWITCH_SHORT_TO_BATT,
		RETRACT_SWITCH_STUCK,
		RETRACT_SWITCH_FAULT,
		RETRACT_SWITCH_CIRCUIT_OPEN,
		PARK_BRAKE_SWITCH_SHORT_TO_GND,
		PARK_BRAKE_SWITCH_SHORT_TO_BATT,
		PARK_BRAKE_SWITCH_STUCK,
		PARK_BRAKE_SWITCH_FAULT,
		PARK_BRAKE_SWITCH_CIRCUIT_OPEN,
		MOTOR_1_SWITCH_SHORT_TO_GND,
		MOTOR_1_SWITCH_SHORT_TO_BATT,
		MOTOR_1_SWITCH_STUCK,
		MOTOR_1_SWITCH_FAULT,
		MOTOR_1_SWITCH_CIRCUIT_OPEN,
		MOTOR_2_SWITCH_SHORT_TO_GND,
		MOTOR_2_SWITCH_SHORT_TO_BATT,
		MOTOR_2_SWITCH_STUCK,
		MOTOR_2_SWITCH_FAULT,
		MOTOR_2_SWITCH_CIRCUIT_OPEN,
		MOTOR_3_SWITCH_SHORT_TO_GND,
		MOTOR_3_SWITCH_SHORT_TO_BATT,
		MOTOR_3_SWITCH_STUCK,
		MOTOR_3_SWITCH_FAULT,
		MOTOR_3_SWITCH_CIRCUIT_OPEN,
		MOTOR_4_SWITCH_SHORT_TO_GND,
		MOTOR_4_SWITCH_SHORT_TO_BATT,
		MOTOR_4_SWITCH_STUCK,
		MOTOR_4_SWITCH_FAULT,
		MOTOR_4_SWITCH_CIRCUIT_OPEN,
		MOTOR_5_SWITCH_SHORT_TO_GND,
		MOTOR_5_SWITCH_SHORT_TO_BATT,
		MOTOR_5_SWITCH_STUCK,
		MOTOR_5_SWITCH_FAULT,
		MOTOR_5_SWITCH_CIRCUIT_OPEN,
		MOTOR_6_SWITCH_SHORT_TO_GND,
		MOTOR_6_SWITCH_SHORT_TO_BATT,
		MOTOR_6_SWITCH_STUCK,
		MOTOR_6_SWITCH_FAULT,
		MOTOR_6_SWITCH_CIRCUIT_OPEN,
		MOTOR_7_SWITCH_SHORT_TO_GND,
		MOTOR_7_SWITCH_SHORT_TO_BATT,
		MOTOR_7_SWITCH_STUCK,
		MOTOR_7_SWITCH_FAULT,
		MOTOR_7_SWITCH_CIRCUIT_OPEN,
		MOTOR_8_SWITCH_SHORT_TO_GND,
		MOTOR_8_SWITCH_SHORT_TO_BATT,
		MOTOR_8_SWITCH_STUCK,
		MOTOR_8_SWITCH_FAULT,
		MOTOR_8_SWITCH_CIRCUIT_OPEN,
		MOTOR_9_SWITCH_SHORT_TO_GND,
		MOTOR_9_SWITCH_SHORT_TO_BATT,
		MOTOR_9_SWITCH_STUCK,
		MOTOR_9_SWITCH_FAULT,
		MOTOR_9_SWITCH_CIRCUIT_OPEN,
		MOTOR_10_SWITCH_SHORT_TO_GND,
		MOTOR_10_SWITCH_SHORT_TO_BATT,
		MOTOR_10_SWITCH_STUCK,
		MOTOR_10_SWITCH_FAULT,
		MOTOR_10_SWITCH_CIRCUIT_OPEN,
		MOTOR_11_SWITCH_SHORT_TO_GND,
		MOTOR_11_SWITCH_SHORT_TO_BATT,
		MOTOR_11_SWITCH_STUCK,
		MOTOR_11_SWITCH_FAULT,
		MOTOR_11_SWITCH_CIRCUIT_OPEN,
		MOTOR_12_SWITCH_SHORT_TO_GND,
		MOTOR_12_SWITCH_SHORT_TO_BATT,
		MOTOR_12_SWITCH_STUCK,
		MOTOR_12_SWITCH_FAULT,
		MOTOR_12_SWITCH_CIRCUIT_OPEN,
		MOTOR_13_SWITCH_SHORT_TO_GND,
		MOTOR_13_SWITCH_SHORT_TO_BATT,
		MOTOR_13_SWITCH_STUCK,
		MOTOR_13_SWITCH_FAULT,
		MOTOR_13_SWITCH_CIRCUIT_OPEN,
		MOTOR_14_SWITCH_SHORT_TO_GND,
		MOTOR_14_SWITCH_SHORT_TO_BATT,
		MOTOR_14_SWITCH_STUCK,
		MOTOR_14_SWITCH_FAULT,
		MOTOR_14_SWITCH_CIRCUIT_OPEN,
		MOTOR_15_SWITCH_SHORT_TO_GND,
		MOTOR_15_SWITCH_SHORT_TO_BATT,
		MOTOR_15_SWITCH_STUCK,
		MOTOR_15_SWITCH_FAULT,
		MOTOR_15_SWITCH_CIRCUIT_OPEN,
		MOTOR_16_SWITCH_SHORT_TO_GND,
		MOTOR_16_SWITCH_SHORT_TO_BATT,
		MOTOR_16_SWITCH_STUCK,
		MOTOR_16_SWITCH_FAULT,
		MOTOR_16_SWITCH_CIRCUIT_OPEN,
		HALL_EFFECT_1_SIGNAL_SHORT_TO_GND,
		HALL_EFFECT_1_SIGNAL_SHORT_TO_BATT,
		HALL_EFFECT_1_SIGNAL_FAULT,
		HALL_EFFECT_1_SIGNAL_CIRCUIT_OPEN,
		HALL_EFFECT_2_SIGNAL_SHORT_TO_GND,
		HALL_EFFECT_2_SIGNAL_SHORT_TO_BATT,
		HALL_EFFECT_2_SIGNAL_FAULT,
		HALL_EFFECT_2_SIGNAL_CIRCUIT_OPEN,
		HALL_EFFECT_3_SIGNAL_SHORT_TO_GND,
		HALL_EFFECT_3_SIGNAL_SHORT_TO_BATT,
		HALL_EFFECT_3_SIGNAL_FAULT,
		HALL_EFFECT_3_SIGNAL_CIRCUIT_OPEN,
		HALL_EFFECT_4_SIGNAL_SHORT_TO_GND,
		HALL_EFFECT_4_SIGNAL_SHORT_TO_BATT,
		HALL_EFFECT_4_SIGNAL_FAULT,
		HALL_EFFECT_4_SIGNAL_CIRCUIT_OPEN,
		HALL_EFFECT_5_SIGNAL_SHORT_TO_GND,
		HALL_EFFECT_5_SIGNAL_SHORT_TO_BATT,
		HALL_EFFECT_5_SIGNAL_FAULT,
		HALL_EFFECT_5_SIGNAL_CIRCUIT_OPEN,
		HALL_EFFECT_6_SIGNAL_SHORT_TO_GND,
		HALL_EFFECT_6_SIGNAL_SHORT_TO_BATT,
		HALL_EFFECT_6_SIGNAL_FAULT,
		HALL_EFFECT_6_SIGNAL_CIRCUIT_OPEN,
		HALL_EFFECT_7_SIGNAL_SHORT_TO_GND,
		HALL_EFFECT_7_SIGNAL_SHORT_TO_BATT,
		HALL_EFFECT_7_SIGNAL_FAULT,
		HALL_EFFECT_7_SIGNAL_CIRCUIT_OPEN,
		HALL_EFFECT_8_SIGNAL_SHORT_TO_GND,
		HALL_EFFECT_8_SIGNAL_SHORT_TO_BATT,
		HALL_EFFECT_8_SIGNAL_FAULT,
		HALL_EFFECT_8_SIGNAL_CIRCUIT_OPEN,
		MOTOR_OUTPUT_SHORT_TO_BATT,
		MOTOR_OUTPUT_SHORT_TO_GND,
		MOTOR_OUTPUT_PLUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_PLUS_SHORT_TO_GND,
		MOTOR_OUTPUT_MINUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_MINUS_SHORT_TO_GND,
		MOTOR_OUTPUT_CIRCUIT_FAILURE,
		MOTOR_OUTPUT_OPEN,
		MOTOR_OUTPUT_SHORT,
		MOTOR_OUTPUT_OVER_CURRENT,
		MOTOR_OUTPUT_UNDER_CURRENT,
		MOTOR_OUTPUT_1_SHORT_TO_BATT,
		MOTOR_OUTPUT_1_SHORT_TO_GND,
		MOTOR_OUTPUT_1_PLUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_1_PLUS_SHORT_TO_GND,
		MOTOR_OUTPUT_1_MINUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_1_MINUS_SHORT_TO_GND,
		MOTOR_OUTPUT_1_CIRCUIT_FAILURE,
		MOTOR_OUTPUT_1_OPEN,
		MOTOR_OUTPUT_1_SHORT,
		MOTOR_OUTPUT_1_OVER_CURRENT,
		MOTOR_OUTPUT_1_UNDER_CURRENT,
		MOTOR_OUTPUT_2_SHORT_TO_BATT,
		MOTOR_OUTPUT_2_SHORT_TO_GND,
		MOTOR_OUTPUT_2_PLUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_2_PLUS_SHORT_TO_GND,
		MOTOR_OUTPUT_2_MINUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_2_MINUS_SHORT_TO_GND,
		MOTOR_OUTPUT_2_CIRCUIT_FAILURE,
		MOTOR_OUTPUT_2_OPEN,
		MOTOR_OUTPUT_2_SHORT,
		MOTOR_OUTPUT_2_OVER_CURRENT,
		MOTOR_OUTPUT_2_UNDER_CURRENT,
		MOTOR_OUTPUT_3_SHORT_TO_BATT,
		MOTOR_OUTPUT_3_SHORT_TO_GND,
		MOTOR_OUTPUT_3_PLUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_3_PLUS_SHORT_TO_GND,
		MOTOR_OUTPUT_3_MINUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_3_MINUS_SHORT_TO_GND,
		MOTOR_OUTPUT_3_CIRCUIT_FAILURE,
		MOTOR_OUTPUT_3_OPEN,
		MOTOR_OUTPUT_3_SHORT,
		MOTOR_OUTPUT_3_OVER_CURRENT,
		MOTOR_OUTPUT_3_UNDER_CURRENT,
		MOTOR_OUTPUT_4_SHORT_TO_BATT,
		MOTOR_OUTPUT_4_SHORT_TO_GND,
		MOTOR_OUTPUT_4_PLUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_4_PLUS_SHORT_TO_GND,
		MOTOR_OUTPUT_4_MINUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_4_MINUS_SHORT_TO_GND,
		MOTOR_OUTPUT_4_CIRCUIT_FAILURE,
		MOTOR_OUTPUT_4_OPEN,
		MOTOR_OUTPUT_4_SHORT,
		MOTOR_OUTPUT_4_OVER_CURRENT,
		MOTOR_OUTPUT_4_UNDER_CURRENT,
		MOTOR_OUTPUT_5_SHORT_TO_BATT,
		MOTOR_OUTPUT_5_SHORT_TO_GND,
		MOTOR_OUTPUT_5_PLUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_5_PLUS_SHORT_TO_GND,
		MOTOR_OUTPUT_5_MINUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_5_MINUS_SHORT_TO_GND,
		MOTOR_OUTPUT_5_CIRCUIT_FAILURE,
		MOTOR_OUTPUT_5_OPEN,
		MOTOR_OUTPUT_5_SHORT,
		MOTOR_OUTPUT_5_OVER_CURRENT,
		MOTOR_OUTPUT_5_UNDER_CURRENT,
		MOTOR_OUTPUT_6_SHORT_TO_BATT,
		MOTOR_OUTPUT_6_SHORT_TO_GND,
		MOTOR_OUTPUT_6_PLUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_6_PLUS_SHORT_TO_GND,
		MOTOR_OUTPUT_6_MINUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_6_MINUS_SHORT_TO_GND,
		MOTOR_OUTPUT_6_CIRCUIT_FAILURE,
		MOTOR_OUTPUT_6_OPEN,
		MOTOR_OUTPUT_6_SHORT,
		MOTOR_OUTPUT_6_OVER_CURRENT,
		MOTOR_OUTPUT_6_UNDER_CURRENT,
		MOTOR_OUTPUT_7_SHORT_TO_BATT,
		MOTOR_OUTPUT_7_SHORT_TO_GND,
		MOTOR_OUTPUT_7_PLUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_7_PLUS_SHORT_TO_GND,
		MOTOR_OUTPUT_7_MINUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_7_MINUS_SHORT_TO_GND,
		MOTOR_OUTPUT_7_CIRCUIT_FAILURE,
		MOTOR_OUTPUT_7_OPEN,
		MOTOR_OUTPUT_7_SHORT,
		MOTOR_OUTPUT_7_OVER_CURRENT,
		MOTOR_OUTPUT_7_UNDER_CURRENT,
		MOTOR_OUTPUT_8_SHORT_TO_BATT,
		MOTOR_OUTPUT_8_SHORT_TO_GND,
		MOTOR_OUTPUT_8_PLUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_8_PLUS_SHORT_TO_GND,
		MOTOR_OUTPUT_8_MINUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_8_MINUS_SHORT_TO_GND,
		MOTOR_OUTPUT_8_CIRCUIT_FAILURE,
		MOTOR_OUTPUT_8_OPEN,
		MOTOR_OUTPUT_8_SHORT,
		MOTOR_OUTPUT_8_OVER_CURRENT,
		MOTOR_OUTPUT_8_UNDER_CURRENT,
		MOTOR_OUTPUT_9_SHORT_TO_BATT,
		MOTOR_OUTPUT_9_SHORT_TO_GND,
		MOTOR_OUTPUT_9_PLUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_9_PLUS_SHORT_TO_GND,
		MOTOR_OUTPUT_9_MINUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_9_MINUS_SHORT_TO_GND,
		MOTOR_OUTPUT_9_CIRCUIT_FAILURE,
		MOTOR_OUTPUT_9_OPEN,
		MOTOR_OUTPUT_9_SHORT,
		MOTOR_OUTPUT_9_OVER_CURRENT,
		MOTOR_OUTPUT_9_UNDER_CURRENT,
		MOTOR_OUTPUT_10_SHORT_TO_BATT,
		MOTOR_OUTPUT_10_SHORT_TO_GND,
		MOTOR_OUTPUT_10_PLUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_10_PLUS_SHORT_TO_GND,
		MOTOR_OUTPUT_10_MINUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_10_MINUS_SHORT_TO_GND,
		MOTOR_OUTPUT_10_CIRCUIT_FAILURE,
		MOTOR_OUTPUT_10_OPEN,
		MOTOR_OUTPUT_10_SHORT,
		MOTOR_OUTPUT_10_OVER_CURRENT,
		MOTOR_OUTPUT_10_UNDER_CURRENT,
		MOTOR_OUTPUT_11_SHORT_TO_BATT,
		MOTOR_OUTPUT_11_SHORT_TO_GND,
		MOTOR_OUTPUT_11_PLUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_11_PLUS_SHORT_TO_GND,
		MOTOR_OUTPUT_11_MINUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_11_MINUS_SHORT_TO_GND,
		MOTOR_OUTPUT_11_CIRCUIT_FAILURE,
		MOTOR_OUTPUT_11_OPEN,
		MOTOR_OUTPUT_11_SHORT,
		MOTOR_OUTPUT_11_OVER_CURRENT,
		MOTOR_OUTPUT_11_UNDER_CURRENT,
		MOTOR_OUTPUT_12_SHORT_TO_BATT,
		MOTOR_OUTPUT_12_SHORT_TO_GND,
		MOTOR_OUTPUT_12_PLUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_12_PLUS_SHORT_TO_GND,
		MOTOR_OUTPUT_12_MINUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_12_MINUS_SHORT_TO_GND,
		MOTOR_OUTPUT_12_CIRCUIT_FAILURE,
		MOTOR_OUTPUT_12_OPEN,
		MOTOR_OUTPUT_12_SHORT,
		MOTOR_OUTPUT_12_OVER_CURRENT,
		MOTOR_OUTPUT_12_UNDER_CURRENT,
		MOTOR_OUTPUT_13_SHORT_TO_BATT,
		MOTOR_OUTPUT_13_SHORT_TO_GND,
		MOTOR_OUTPUT_13_PLUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_13_PLUS_SHORT_TO_GND,
		MOTOR_OUTPUT_13_MINUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_13_MINUS_SHORT_TO_GND,
		MOTOR_OUTPUT_13_CIRCUIT_FAILURE,
		MOTOR_OUTPUT_13_OPEN,
		MOTOR_OUTPUT_13_SHORT,
		MOTOR_OUTPUT_13_OVER_CURRENT,
		MOTOR_OUTPUT_13_UNDER_CURRENT,
		MOTOR_OUTPUT_14_SHORT_TO_BATT,
		MOTOR_OUTPUT_14_SHORT_TO_GND,
		MOTOR_OUTPUT_14_PLUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_14_PLUS_SHORT_TO_GND,
		MOTOR_OUTPUT_14_MINUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_14_MINUS_SHORT_TO_GND,
		MOTOR_OUTPUT_14_CIRCUIT_FAILURE,
		MOTOR_OUTPUT_14_OPEN,
		MOTOR_OUTPUT_14_SHORT,
		MOTOR_OUTPUT_14_OVER_CURRENT,
		MOTOR_OUTPUT_14_UNDER_CURRENT,
		MOTOR_OUTPUT_15_SHORT_TO_BATT,
		MOTOR_OUTPUT_15_SHORT_TO_GND,
		MOTOR_OUTPUT_15_PLUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_15_PLUS_SHORT_TO_GND,
		MOTOR_OUTPUT_15_MINUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_15_MINUS_SHORT_TO_GND,
		MOTOR_OUTPUT_15_CIRCUIT_FAILURE,
		MOTOR_OUTPUT_15_OPEN,
		MOTOR_OUTPUT_15_SHORT,
		MOTOR_OUTPUT_15_OVER_CURRENT,
		MOTOR_OUTPUT_15_UNDER_CURRENT,
		MOTOR_OUTPUT_16_SHORT_TO_BATT,
		MOTOR_OUTPUT_16_SHORT_TO_GND,
		MOTOR_OUTPUT_16_PLUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_16_PLUS_SHORT_TO_GND,
		MOTOR_OUTPUT_16_MINUS_SHORT_TO_BATT,
		MOTOR_OUTPUT_16_MINUS_SHORT_TO_GND,
		MOTOR_OUTPUT_16_CIRCUIT_FAILURE,
		MOTOR_OUTPUT_16_OPEN,
		MOTOR_OUTPUT_16_SHORT,
		MOTOR_OUTPUT_16_OVER_CURRENT,
		MOTOR_OUTPUT_16_UNDER_CURRENT,
		MOTOR_SOFTSTOPS_NOT_CONFIGURED,
		BATTERY_DROPOUT,
		WATER_HEATER_IGNITION_FAILURE,
		AUTO_LEVEL_TIMEOUT,
		AUTO_LEVEL_FAIL,
		AUTO_RETRACT_TIMEOUT,
		AUTO_HITCH_TIMEOUT,
		GROUND_JACKS_TIMEOUT,
		EXCESS_ANGLE,
		USER_PANIC_STOP,
		PARK_BRAKE_NOT_ENGAGED,
		REQUESTED_FEATURE_DISABLED,
		PSI_SWITCH_TIMEOUT,
		PSI_SWITCH_SHORT_TO_GND,
		PSI_SWITCH_SHORT_TO_BATT,
		PSI_SWITCH_SWITCH_STUCK,
		PSI_SWITCH_SWITCH_FAULT,
		PSI_SWITCH_CIRCUIT_OPEN,
		TOUCH_PAD_POWER_SHORT_TO_BATT,
		TOUCH_PAD_POWER_SHORT_TO_GND,
		TOUCH_PAD_POWER_PLUS_SHORT_TO_BATT,
		TOUCH_PAD_POWER_PLUS_SHORT_TO_GND,
		TOUCH_PAD_POWER_MINUS_SHORT_TO_BATT,
		TOUCH_PAD_POWER_MINUS_SHORT_TO_GND,
		TOUCH_PAD_POWER_CIRCUIT_FAILURE,
		TOUCH_PAD_POWER_OPEN,
		TOUCH_PAD_POWER_SHORT,
		TOUCH_PAD_POWER_OVER_CURRENT,
		TOUCH_PAD_POWER_UNDER_CURRENT,
		IGNITION_SWITCH_NOT_ENGAGED,
		AUTO_START_VOLTAGE_LOW,
		RETURN_FUSE_OPEN,
		OUTPUT_1_SOFTWARE_FUSE_OPEN,
		OUTPUT_2_SOFTWARE_FUSE_OPEN,
		OUTPUT_3_SOFTWARE_FUSE_OPEN,
		OUTPUT_4_SOFTWARE_FUSE_OPEN,
		OUTPUT_5_SOFTWARE_FUSE_OPEN,
		OUTPUT_6_SOFTWARE_FUSE_OPEN,
		OUTPUT_7_SOFTWARE_FUSE_OPEN,
		OUTPUT_8_SOFTWARE_FUSE_OPEN,
		OUTPUT_9_SOFTWARE_FUSE_OPEN,
		OUTPUT_10_SOFTWARE_FUSE_OPEN,
		OUTPUT_11_SOFTWARE_FUSE_OPEN,
		OUTPUT_12_SOFTWARE_FUSE_OPEN,
		OUTPUT_13_SOFTWARE_FUSE_OPEN,
		OUTPUT_14_SOFTWARE_FUSE_OPEN,
		OUTPUT_15_SOFTWARE_FUSE_OPEN,
		OUTPUT_16_SOFTWARE_FUSE_OPEN,
		OUTPUT_17_SOFTWARE_FUSE_OPEN,
		OUTPUT_18_SOFTWARE_FUSE_OPEN,
		OUTPUT_19_SOFTWARE_FUSE_OPEN,
		OUTPUT_20_SOFTWARE_FUSE_OPEN,
		OUTPUT_21_SOFTWARE_FUSE_OPEN,
		OUTPUT_22_SOFTWARE_FUSE_OPEN,
		OUTPUT_23_SOFTWARE_FUSE_OPEN,
		OUTPUT_24_SOFTWARE_FUSE_OPEN,
		OUTPUT_25_SOFTWARE_FUSE_OPEN,
		OUTPUT_26_SOFTWARE_FUSE_OPEN,
		OUTPUT_27_SOFTWARE_FUSE_OPEN,
		OUTPUT_28_SOFTWARE_FUSE_OPEN,
		OUTPUT_29_SOFTWARE_FUSE_OPEN,
		OUTPUT_30_SOFTWARE_FUSE_OPEN,
		OUTPUT_31_SOFTWARE_FUSE_OPEN,
		OUTPUT_32_SOFTWARE_FUSE_OPEN,
		WIRELESS_SWITCH_BATTERY_LOW,
		WIRELESS_SWITCH_1_BATTERY_LOW,
		WIRELESS_SWITCH_2_BATTERY_LOW,
		WIRELESS_SWITCH_3_BATTERY_LOW,
		WIRELESS_SWITCH_4_BATTERY_LOW,
		WIRELESS_SWITCH_5_BATTERY_LOW,
		WIRELESS_SWITCH_6_BATTERY_LOW,
		WIRELESS_SWITCH_7_BATTERY_LOW,
		WIRELESS_SWITCH_8_BATTERY_LOW,
		WIRELESS_SWITCH_9_BATTERY_LOW,
		WIRELESS_SWITCH_10_BATTERY_LOW,
		WIRELESS_SWITCH_11_BATTERY_LOW,
		WIRELESS_SWITCH_12_BATTERY_LOW,
		WIRELESS_SWITCH_13_BATTERY_LOW,
		WIRELESS_SWITCH_14_BATTERY_LOW,
		WIRELESS_SWITCH_15_BATTERY_LOW,
		WIRELESS_SWITCH_16_BATTERY_LOW,
		WATER_INTRUSION_DETECTED,
		WATER_INTRUSION_DETECTED_IN_CONNECTOR,
		WATER_INTRUSION_DETECTED_IN_TOUCHPAD,
		OPERATING_VOLTAGE_DROPOUT,
		BATTERY_VOLTAGE_HIGH_FAST_DETECT,
		IGNITION_NOT_ACTIVE,
		BATTERY_TOO_LOW_TO_OPERATE,
		LIGHT_OUTPUT_SOFTWARE_FUSE_OPEN,
		LIGHT_OUTPUT_1_SOFTWARE_FUSE_OPEN,
		LIGHT_OUTPUT_2_SOFTWARE_FUSE_OPEN,
		LIGHT_OUTPUT_3_SOFTWARE_FUSE_OPEN,
		LIGHT_OUTPUT_4_SOFTWARE_FUSE_OPEN,
		LIGHT_OUTPUT_5_SOFTWARE_FUSE_OPEN,
		LIGHT_OUTPUT_6_SOFTWARE_FUSE_OPEN,
		LIGHT_OUTPUT_7_SOFTWARE_FUSE_OPEN,
		LIGHT_OUTPUT_8_SOFTWARE_FUSE_OPEN,
		LIGHT_OUTPUT_9_SOFTWARE_FUSE_OPEN,
		LIGHT_OUTPUT_10_SOFTWARE_FUSE_OPEN,
		LIGHT_OUTPUT_11_SOFTWARE_FUSE_OPEN,
		LIGHT_OUTPUT_12_SOFTWARE_FUSE_OPEN,
		LIGHT_OUTPUT_13_SOFTWARE_FUSE_OPEN,
		LIGHT_OUTPUT_14_SOFTWARE_FUSE_OPEN,
		LIGHT_OUTPUT_15_SOFTWARE_FUSE_OPEN,
		LIGHT_OUTPUT_16_SOFTWARE_FUSE_OPEN,
		POWER_INPUT_MISSING,
		POWER_INPUT_1_MISSING,
		POWER_INPUT_2_MISSING,
		POWER_INPUT_3_MISSING,
		POWER_INPUT_4_MISSING,
		POWER_INPUT_5_MISSING,
		POWER_INPUT_6_MISSING,
		POWER_INPUT_7_MISSING,
		POWER_INPUT_8_MISSING,
		POWER_INPUT_9_MISSING,
		POWER_INPUT_10_MISSING,
		POWER_INPUT_11_MISSING,
		POWER_INPUT_12_MISSING,
		POWER_INPUT_13_MISSING,
		POWER_INPUT_14_MISSING,
		POWER_INPUT_15_MISSING,
		POWER_INPUT_16_MISSING,
		IGNITION_ACTIVE,
		PARKBRAKE_NOT_ACTIVE,
		PARKBRAKE_ACTIVE,
		MOTOR_RETRACT_SOFTSTOP_NOT_CONFIGURED,
		MOTOR_EXTEND_SOFTSTOP_NOT_CONFIGURED,
		SWITCH_ACTIVE_ONLY_VIA_HARDWARE,
		LEVELER_OPERATING_VOLTAGE_DROPOUT,
		OUTPUT_1_OPERATING_VOLTAGE_DROPOUT,
		OUTPUT_2_OPERATING_VOLTAGE_DROPOUT,
		OUTPUT_3_OPERATING_VOLTAGE_DROPOUT,
		OUTPUT_4_OPERATING_VOLTAGE_DROPOUT,
		OUTPUT_5_OPERATING_VOLTAGE_DROPOUT,
		OUTPUT_6_OPERATING_VOLTAGE_DROPOUT,
		OUTPUT_7_OPERATING_VOLTAGE_DROPOUT,
		OUTPUT_8_OPERATING_VOLTAGE_DROPOUT,
		OUTPUT_9_OPERATING_VOLTAGE_DROPOUT,
		OUTPUT_10_OPERATING_VOLTAGE_DROPOUT,
		OUTPUT_11_OPERATING_VOLTAGE_DROPOUT,
		OUTPUT_12_OPERATING_VOLTAGE_DROPOUT,
		OUTPUT_13_OPERATING_VOLTAGE_DROPOUT,
		OUTPUT_14_OPERATING_VOLTAGE_DROPOUT,
		OUTPUT_15_OPERATING_VOLTAGE_DROPOUT,
		OUTPUT_16_OPERATING_VOLTAGE_DROPOUT,
		BLE_PAIRING_CORRUPTION,
		SURESHADE_GENERAL_FAULT,
		TBB_COMM_ERROR,
		USER_AUTO_OPERATION_IN_PROGRESS,
		WIND_EVENT_AUTO_OPERATION_IN_PROGRESS,
		AUTO_OPERATION_CANCELED,
		WIND_SENSOR_1_COMM_FAILURE,
		WIND_SENSOR_2_COMM_FAILURE,
		WIND_SENSOR_3_COMM_FAILURE,
		WIND_SENSOR_4_COMM_FAILURE,
		WIND_SENSOR_5_COMM_FAILURE,
		WIND_SENSOR_6_COMM_FAILURE,
		WIND_SENSOR_7_COMM_FAILURE,
		WIND_SENSOR_8_COMM_FAILURE,
		WIND_SENSOR_9_COMM_FAILURE,
		WIND_SENSOR_10_COMM_FAILURE,
		WIND_SENSOR_11_COMM_FAILURE,
		WIND_SENSOR_12_COMM_FAILURE,
		WIND_SENSOR_13_COMM_FAILURE,
		WIND_SENSOR_14_COMM_FAILURE,
		WIND_SENSOR_15_COMM_FAILURE,
		WIND_SENSOR_16_COMM_FAILURE,
		LOW_BATTERY_OPERATING_MODE,
		AXLE_1_LEFT_ACTUATOR_OPEN,
		AXLE_1_LEFT_ACTUATOR_SHORT_TO_BATT,
		AXLE_1_LEFT_ACTUATOR_SHORT_TO_GROUND,
		AXLE_1_LEFT_ACTUATOR_RELAY_FAILURE,
		AXLE_1_LEFT_WSS_OPEN,
		AXLE_1_LEFT_WSS_SHORT_TO_BATT,
		AXLE_1_LEFT_WSS_SHORT_TO_GROUND,
		AXLE_1_RIGHT_ACTUATOR_OPEN,
		AXLE_1_RIGHT_ACTUATOR_SHORT_TO_BATT,
		AXLE_1_RIGHT_ACTUATOR_SHORT_TO_GROUND,
		AXLE_1_RIGHT_ACTUATOR_RELAY_FAILURE,
		AXLE_1_RIGHT_WSS_OPEN,
		AXLE_1_RIGHT_WSS_SHORT_TO_BATT,
		AXLE_1_RIGHT_WSS_SHORT_TO_GROUND,
		AXLE_2_LEFT_ACTUATOR_OPEN,
		AXLE_2_LEFT_ACTUATOR_SHORT_TO_BATT,
		AXLE_2_LEFT_ACTUATOR_SHORT_TO_GROUND,
		AXLE_2_LEFT_ACTUATOR_RELAY_FAILURE,
		AXLE_2_LEFT_WSS_OPEN,
		AXLE_2_LEFT_WSS_SHORT_TO_BATT,
		AXLE_2_LEFT_WSS_SHORT_TO_GROUND,
		AXLE_2_RIGHT_ACTUATOR_OPEN,
		AXLE_2_RIGHT_ACTUATOR_SHORT_TO_BATT,
		AXLE_2_RIGHT_ACTUATOR_SHORT_TO_GROUND,
		AXLE_2_RIGHT_ACTUATOR_RELAY_FAILURE,
		AXLE_2_RIGHT_WSS_OPEN,
		AXLE_2_RIGHT_WSS_SHORT_TO_BATT,
		AXLE_2_RIGHT_WSS_SHORT_TO_GROUND,
		AXLE_3_LEFT_ACTUATOR_OPEN,
		AXLE_3_LEFT_ACTUATOR_SHORT_TO_BATT,
		AXLE_3_LEFT_ACTUATOR_SHORT_TO_GROUND,
		AXLE_3_LEFT_ACTUATOR_RELAY_FAILURE,
		AXLE_3_LEFT_WSS_OPEN,
		AXLE_3_LEFT_WSS_SHORT_TO_BATT,
		AXLE_3_LEFT_WSS_SHORT_TO_GROUND,
		AXLE_3_RIGHT_ACTUATOR_OPEN,
		AXLE_3_RIGHT_ACTUATOR_SHORT_TO_BATT,
		AXLE_3_RIGHT_ACTUATOR_SHORT_TO_GROUND,
		AXLE_3_RIGHT_ACTUATOR_RELAY_FAILURE,
		AXLE_3_RIGHT_WSS_OPEN,
		AXLE_3_RIGHT_WSS_SHORT_TO_BATT,
		AXLE_3_RIGHT_WSS_SHORT_TO_GROUND,
		BRAKE_LOAD_CIRCUIT_OPEN,
		BRAKE_LOAD_CIRCUIT_SHORT_TO_BATT,
		BRAKE_LOAD_CIRCUIT_SHORT_TO_GROUND,
		BRAKE_OUTPUT_OPEN,
		BRAKE_OUTPUT_SHORT_TO_BATT,
		BRAKE_OUTPUT_SHORT_TO_GROUND,
		TAIL_RUNLIGHTS_OUTPUT_OPEN,
		TAIL_RUNLIGHTS_OUTPUT_SHORT_TO_BATT,
		TAIL_RUNLIGHTS_OUTPUT_SHORT_TO_GROUND,
		LEFT_TURN_BRAKE_OUTPUT_OPEN,
		LEFT_TURN_BRAKE_OUTPUT_SHORT_TO_BATT,
		LEFT_TURN_BRAKE_OUTPUT_SHORT_TO_GROUND,
		RIGHT_TURN_BRAKE_OUTPUT_OPEN,
		RIGHT_TURN_BRAKE_OUTPUT_SHORT_TO_BATT,
		RIGHT_TURN_BRAKE_OUTPUT_SHORT_TO_GROUND,
		RV_C_EXTERNAL_NODE_SERIAL_NUMBER_ERROR,
		PRODUCT_WATCHDOG_TRIGGERED,
		RV_C_EXTERNAL_NODE_CALIBRATION_REQUIRED,
		RV_C_COMMUNICATION_ERROR,
		RV_C_EXTERNAL_NODE_COMMUNICATION_ERROR,
		RV_C_EXTERNAL_NODE_TEMP_SENSOR_FAILURE,
		RV_C_EXTERNAL_NODE_LOW_VOLTAGE,
		RV_C_EXTERNAL_NODE_HIGH_VOLTAGE,
		RV_C_EXTERNAL_NODE_YELLOW_LAMP,
		RV_C_EXTERNAL_NODE_RED_LAMP,
		BRAKE_CONTROLLER_LOSS_OF_COMM_BLE_MICRO,
		BRAKE_CONTROLLER_MODULE_FAILURE,
		WSS_A1L_MECHANICAL_FAILURE,
		WSS_A1R_MECHANICAL_FAILURE,
		WSS_A2L_MECHANICAL_FAILURE,
		WSS_A2R_MECHANICAL_FAILURE,
		WSS_A3L_MECHANICAL_FAILURE,
		WSS_A3R_MECHANICAL_FAILURE,
		BRAKE_CONTROLLER_MODULE_OVER_TEMPERATURE,
		USER_AUTO_OPERATION_COMPLETE,
		WIND_EVENT_AUTO_OPERATION_COMPLETE,
		WIND_SENSOR_1_ERROR,
		WIND_SENSOR_2_ERROR,
		WIND_SENSOR_3_ERROR,
		WIND_SENSOR_4_ERROR,
		WIND_SENSOR_5_ERROR,
		WIND_SENSOR_6_ERROR,
		WIND_SENSOR_7_ERROR,
		WIND_SENSOR_8_ERROR,
		WIND_SENSOR_9_ERROR,
		WIND_SENSOR_10_ERROR,
		WIND_SENSOR_11_ERROR,
		WIND_SENSOR_12_ERROR,
		WIND_SENSOR_13_ERROR,
		WIND_SENSOR_14_ERROR,
		WIND_SENSOR_15_ERROR,
		WIND_SENSOR_16_ERROR,
		USER_AUTO_OPERATION_ERROR,
		WIND_EVENT_AUTO_OPERATION_ERROR,
		WIND_PROTECTION_OFF,
		WIND_PROTECTION_LOW,
		WIND_PROTECTION_MEDIUM,
		WIND_PROTECTION_HIGH,
		AUTO_HITCH_UNAVAILABLE,
		EXCESS_ANGLE_MANUAL,
		FRONT_JACKS_OUT_OF_STROKE,
		LOCKOUT_ACTION_BLOCKED,
		TANK_LOW_THRESHOLD_EXCEEDED,
		TANK_HIGH_THRESHOLD_EXCEEDED,
		CONTROLLER_LOSS_OF_COMM_BLE_MICRO,
		BLE_MICRO_LOSS_OF_COMM_CONTROLLER,
		MOTOR_POSITIONS_OUT_OF_SYNC,
		MOTOR1_REVERSE_WIRED,
		MOTOR2_REVERSE_WIRED,
		CONTROLLER_EVENT_AUTO_OPERATION_IN_PROGRESS,
		CONTROLLER_EVENT_AUTO_OPERATION_COMPLETE,
		CONTROLLER_EVENT_AUTO_OPERATION_ERROR,
		AXLE_1_MOTOR_CONTROL_MODULE_MISSING,
		AXLE_2_MOTOR_CONTROL_MODULE_MISSING,
		AXLE_3_MOTOR_CONTROL_MODULE_MISSING,
		BRAKE_CONTROLLER_MISSING,
		MOTOR_A1L_FAILURE_DETECTED,
		MOTOR_A1R_FAILURE_DETECTED,
		MOTOR_A2L_FAILURE_DETECTED,
		MOTOR_A2R_FAILURE_DETECTED,
		MOTOR_A3L_FAILURE_DETECTED,
		MOTOR_A3R_FAILURE_DETECTED,
		AXLE_1_MOTOR_CONTROLLER_MODULE_OVER_TEMPERATURE,
		AXLE_2_MOTOR_CONTROLLER_MODULE_OVER_TEMPERATURE,
		AXLE_3_MOTOR_CONTROLLER_MODULE_OVER_TEMPERATURE,
		SWITCH_PWR_SHORT_TO_GROUND,
		MOTOR_LEARN_FAULT,
		MOTOR_OUTPUT_1_OPEN_B,
		MOTOR_OUTPUT_2_OPEN_B,
		MOTOR_OUTPUT_1_SHORT_TO_BATT_B,
		MOTOR_OUTPUT_2_SHORT_TO_BATT_B,
		FRAME_TWIST_RELIEF_ACTIVE,
		FRAME_TWIST_PREVENTED_MOVEMENT,
		MOTOR_1_BRAKE_FAILURE,
		MOTOR_2_BRAKE_FAILURE,
		HALL_EFFECT_1_SIGNAL_UNEXPECTED_PULSES,
		HALL_EFFECT_2_SIGNAL_UNEXPECTED_PULSES,
		MOTOR_OUTPUT_1_SOFTWARE_FUSE_OPEN,
		MOTOR_OUTPUT_2_SOFTWARE_FUSE_OPEN,
		MOTOR_OUTPUT_3_SOFTWARE_FUSE_OPEN,
		MOTOR_OUTPUT_4_SOFTWARE_FUSE_OPEN,
		MOTOR_OUTPUT_5_SOFTWARE_FUSE_OPEN,
		MOTOR_OUTPUT_6_SOFTWARE_FUSE_OPEN,
		MOTOR_OUTPUT_7_SOFTWARE_FUSE_OPEN,
		MOTOR_OUTPUT_8_SOFTWARE_FUSE_OPEN,
		MOTOR_OUTPUT_9_SOFTWARE_FUSE_OPEN,
		MOTOR_OUTPUT_10_SOFTWARE_FUSE_OPEN,
		MOTOR_OUTPUT_11_SOFTWARE_FUSE_OPEN,
		MOTOR_OUTPUT_12_SOFTWARE_FUSE_OPEN,
		MOTOR_OUTPUT_13_SOFTWARE_FUSE_OPEN,
		MOTOR_OUTPUT_14_SOFTWARE_FUSE_OPEN,
		MOTOR_OUTPUT_15_SOFTWARE_FUSE_OPEN,
		MOTOR_OUTPUT_16_SOFTWARE_FUSE_OPEN,
		MOTOR_A1L_OVERCURRENT,
		MOTOR_A1R_OVERCURRENT,
		MOTOR_A2L_OVERCURRENT,
		MOTOR_A2R_OVERCURRENT,
		MOTOR_A3L_OVERCURRENT,
		MOTOR_A3R_OVERCURRENT,
		POWER_INPUT_1_OVER_CURRENT,
		POWER_INPUT_2_OVER_CURRENT,
		POWER_INPUT_3_OVER_CURRENT,
		POWER_INPUT_4_OVER_CURRENT,
		POWER_INPUT_5_OVER_CURRENT,
		POWER_INPUT_6_OVER_CURRENT,
		POWER_INPUT_7_OVER_CURRENT,
		POWER_INPUT_8_OVER_CURRENT,
		LOAD_SHEDDING_ACTIVE,
		REAR_JACKS_OUT_OF_STROKE,
		LEFT_JACKS_OUT_OF_STROKE,
		RIGHT_JACKS_OUT_OF_STROKE,
		AIRBAGS_PANIC_STOP,
		EMERGENCY_RETRACT_PANIC_STOP,
		SWITCH_OVERRIDE_ACTIVE,
		MOTOR_POSITIONS_OUT_OF_SYNC_B,
		MOTOR_OUTPUT_1_SHORT_B,
		MOTOR_OUTPUT_2_SHORT_B,
		CURT_LEGACY_BRAKE_OUTPUT_OVERLOAD,
		CURT_UNDERDASH2_BRAKE_OUTPUT_OVERLOAD,
		CURT_BANK_OUTPUT_SHORT,
		CURT_LAMP_OUTPUT_OVERLOAD,
		CURT_LAMP_OUTPUT_SHORT,
		CURT_OPEN_GROUND,
		CURT_BATTERY_OUT_OF_RANGE,
		CURT_TRAILER_DISCONNECT,
		CURT_INTERNAL_ERROR,
		GENERATOR_AUTO_STARTING_LOW_VOLTAGE,
		GENERATOR_AUTO_STARTING_HVAC_SUPPORT,
		GENERATOR_STOPPING_QUIET_HOURS,
		GENERATOR_STOPPING_RUN_DURATION_EXCEEDED
	}
	public enum ICON : ushort
	{
		UNKNOWN,
		DIAGNOSTIC_TOOL,
		DIAGNOSTIC_TOOL_UNKNOWN,
		TABLET,
		TABLET_UNKNOWN,
		LATCHING_RELAY,
		LATCHING_RELAY_UNKNOWN,
		MOMENTARY_RELAY,
		MOMENTARY_RELAY_UNKNOWN,
		LATCHING_H_BRIDGE,
		LATCHING_H_BRIDGE_UNKNOWN,
		MOMENTARY_H_BRIDGE,
		MOMENTARY_H_BRIDGE_UNKNOWN,
		LEVELER,
		LEVELER_UNKNOWN,
		SWITCH,
		SWITCH_UNKNOWN,
		TOUCHSCREEN_SWITCH,
		TOUCHSCREEN_SWITCH_UNKNOWN,
		TANK_SENSOR,
		TANK_SENSOR_UNKNOWN,
		AWNING,
		VENT,
		VENT_COVER,
		BED_LIFT,
		BLACK_WATER_TANK,
		LOCK,
		ELECTRIC_WATER_HEATER,
		FRESH_WATER_TANK,
		FRONT_STABILIZER,
		FUEL_TANK,
		GAS_WATER_HEATER,
		GENERATOR,
		GREY_WATER_TANK,
		LANDING_GEAR,
		LIGHT,
		REAR_STABILIZER,
		SLIDE,
		TV_LIFT,
		WATER_PUMP,
		WATER_TANK_HEATER,
		JACKS,
		HOUR_METER,
		RGB_LIGHT,
		CLOCK,
		IR_REMOTE_CONTROL,
		HVAC_CONTROL,
		FIREPLACE,
		LP_TANK_VALVE,
		NETWORK_BRIDGE,
		IPDM,
		DIMMABLE_LIGHT,
		OCTP,
		ANDROID,
		IOS,
		FUEL_PUMP,
		STABILIZER,
		THERMOMETER,
		POWER_MONITOR,
		POWER_MANAGER,
		CLOUD,
		DOOR,
		FAN,
		BLUETOOTH,
		GENERIC,
		RAIN_SENSOR,
		CHASSIS,
		TPMS
	}
}
namespace IDS.Core.IDS_CAN.Descriptors
{
	public static class BuildInfo
	{
		public const int BuildYear = 2025;

		public const int BuildMonth = 10;

		public const int BuildDay = 21;

		public const int BuildHour = 14;

		public const int BuildMinute = 9;

		public const int BuildSecond = 11;

		public static readonly System.DateTime DateTime = new System.DateTime(2025, 10, 21, 14, 9, 11);
	}
}
