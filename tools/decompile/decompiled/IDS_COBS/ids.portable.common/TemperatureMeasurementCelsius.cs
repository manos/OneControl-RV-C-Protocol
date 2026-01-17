using System.Runtime.CompilerServices;

namespace IDS.Portable.Common;

public struct TemperatureMeasurementCelsius : ITemperatureMeasurement
{
	public bool IsTemperatureValid => true;

	public float TemperatureFahrenheit => Temperature.ConvertCelsiusToFahrenheit(TemperatureCelsius);

	[field: CompilerGenerated]
	public float TemperatureCelsius
	{
		[CompilerGenerated]
		get;
	}

	public TemperatureMeasurementCelsius(float celsius)
	{
		TemperatureCelsius = celsius;
	}
}
