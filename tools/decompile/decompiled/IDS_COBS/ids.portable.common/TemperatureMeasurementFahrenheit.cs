using System.Runtime.CompilerServices;

namespace IDS.Portable.Common;

public struct TemperatureMeasurementFahrenheit : ITemperatureMeasurement
{
	public bool IsTemperatureValid => true;

	[field: CompilerGenerated]
	public float TemperatureFahrenheit
	{
		[CompilerGenerated]
		get;
	}

	public float TemperatureCelsius => Temperature.ConvertFahrenheitToCelsius(TemperatureFahrenheit);

	public TemperatureMeasurementFahrenheit(float fahrenheit)
	{
		TemperatureFahrenheit = fahrenheit;
	}
}
