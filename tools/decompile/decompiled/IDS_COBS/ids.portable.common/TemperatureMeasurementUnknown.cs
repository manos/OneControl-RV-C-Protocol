using System.Runtime.InteropServices;

namespace IDS.Portable.Common;

[StructLayout((LayoutKind)0, Size = 1)]
public struct TemperatureMeasurementUnknown : ITemperatureMeasurement
{
	public bool IsTemperatureValid => false;

	public float TemperatureFahrenheit => 0f;

	public float TemperatureCelsius => 0f;
}
