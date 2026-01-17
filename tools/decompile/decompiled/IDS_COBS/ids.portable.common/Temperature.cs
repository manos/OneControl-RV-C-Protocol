using System;
using System.Runtime.CompilerServices;
using IDS.Portable.Common.Extensions;

namespace IDS.Portable.Common;

public struct Temperature
{
	public enum DisplayType
	{
		Condensed,
		Default
	}

	[field: CompilerGenerated]
	public decimal Fahrenheit
	{
		[CompilerGenerated]
		get;
		[CompilerGenerated]
		private set;
	}

	public decimal Celsius => ConvertFahrenheitToCelsius(Fahrenheit);

	public decimal Kelvin => ConvertCelsiusToKelvin(Celsius);

	public static decimal ConvertCelsiusToFahrenheit(decimal celsius)
	{
		return celsius * 9m / 5m + 32m;
	}

	public static decimal ConvertCelsiusDeltaToFahrenheitDelta(decimal celsiusDelta)
	{
		return celsiusDelta * 9m / 5m;
	}

	public static decimal ConvertFahrenheitToCelsius(decimal fahrenheit)
	{
		return (fahrenheit - 32m) * 5m / 9m;
	}

	public static decimal ConvertFahrenheitDeltaToCelsiusDelta(decimal fahrenheitDelta)
	{
		return fahrenheitDelta * 5m / 9m;
	}

	public static decimal ConvertCelsiusToKelvin(decimal celsius)
	{
		return celsius + 273.15m;
	}

	public static decimal ConvertCelsiusDeltaToKelvinDelta(decimal celsiusDelta)
	{
		return celsiusDelta;
	}

	public static double ConvertCelsiusToFahrenheit(double celsius)
	{
		return (double)ConvertCelsiusToFahrenheit((decimal)celsius);
	}

	public static double ConvertCelsiusDeltaToFahrenheitDelta(double celsiusDelta)
	{
		return (double)ConvertCelsiusDeltaToFahrenheitDelta((decimal)celsiusDelta);
	}

	public static double ConvertFahrenheitToCelsius(double fahrenheit)
	{
		return (double)ConvertFahrenheitToCelsius((decimal)fahrenheit);
	}

	public static double ConvertCelsiusToKelvin(double celsius)
	{
		return (double)ConvertCelsiusToKelvin((decimal)celsius);
	}

	public static float ConvertCelsiusToFahrenheit(float celsius)
	{
		return (float)ConvertCelsiusToFahrenheit((decimal)celsius);
	}

	public static float ConvertFahrenheitToCelsius(float fahrenheit)
	{
		return (float)ConvertFahrenheitToCelsius((decimal)fahrenheit);
	}

	public static float ConvertCelsiusToKelvin(float celsius)
	{
		return (float)ConvertCelsiusToKelvin((decimal)celsius);
	}

	public static decimal ConvertKelvinToFahrenheit(double kelvin)
	{
		return Convert.ToDecimal((kelvin - 273.1499938964844) * 9.0 / 5.0 + 32.0);
	}

	public Temperature(float fahrenheit)
	{
		Fahrenheit = (decimal)fahrenheit;
	}

	public Temperature(decimal temperature, TemperatureScale scale)
	{
		this = default(Temperature);
		SetTemperature(temperature, scale);
	}

	public void SetFahrenheit(decimal fahrenheit)
	{
		SetTemperature(fahrenheit, TemperatureScale.Fahrenheit);
	}

	public void SetCelsius(decimal celsius)
	{
		SetTemperature(celsius, TemperatureScale.Celsius);
	}

	public void SetTemperature(decimal temperature, TemperatureScale scale)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		switch (scale)
		{
		case TemperatureScale.Fahrenheit:
			Fahrenheit = temperature;
			break;
		case TemperatureScale.Celsius:
			Fahrenheit = ConvertCelsiusToFahrenheit(temperature);
			break;
		case TemperatureScale.Kelvin:
			Fahrenheit = ConvertKelvinToFahrenheit(Convert.ToDouble(temperature));
			break;
		default:
			throw new ArgumentOutOfRangeException("scale");
		}
	}

	public decimal ConvertToScale(TemperatureScale scale)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		return scale switch
		{
			TemperatureScale.Fahrenheit => Fahrenheit, 
			TemperatureScale.Celsius => Celsius, 
			TemperatureScale.Kelvin => Kelvin, 
			_ => throw new ArgumentOutOfRangeException("scale"), 
		};
	}

	public static decimal ConvertToScaleFromFahrenheit(decimal fahrenheit, TemperatureScale scale)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		return scale switch
		{
			TemperatureScale.Fahrenheit => fahrenheit, 
			TemperatureScale.Celsius => ConvertFahrenheitToCelsius(fahrenheit), 
			TemperatureScale.Kelvin => ConvertCelsiusToKelvin(ConvertFahrenheitToCelsius(fahrenheit)), 
			_ => throw new ArgumentOutOfRangeException("scale"), 
		};
	}

	public static decimal ConvertDeltaToScaleFromFahrenheitDelta(decimal fahrenheitDelta, TemperatureScale scale)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		return scale switch
		{
			TemperatureScale.Fahrenheit => fahrenheitDelta, 
			TemperatureScale.Celsius => ConvertFahrenheitDeltaToCelsiusDelta(fahrenheitDelta), 
			TemperatureScale.Kelvin => ConvertCelsiusDeltaToKelvinDelta(ConvertFahrenheitDeltaToCelsiusDelta(fahrenheitDelta)), 
			_ => throw new ArgumentOutOfRangeException("scale"), 
		};
	}

	public static decimal ConvertToScale(decimal temperature, TemperatureScale fromScale, TemperatureScale toScale, double? roundThreshold = null)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		if (fromScale == toScale)
		{
			return temperature;
		}
		decimal num = fromScale switch
		{
			TemperatureScale.Fahrenheit => temperature, 
			TemperatureScale.Celsius => ConvertCelsiusToFahrenheit(temperature), 
			_ => throw new ArgumentOutOfRangeException("fromScale"), 
		};
		if (!roundThreshold.HasValue)
		{
			return ConvertToScaleFromFahrenheit(num, toScale);
		}
		return ConvertToScaleFromFahrenheit(decimal.op_Implicit(((double)num).RoundUsingThreshold(roundThreshold.Value)), toScale);
	}

	public static decimal ConvertDeltaToScale(decimal temperatureDelta, TemperatureScale fromScale, TemperatureScale toScale, double? roundThreshold = null)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		if (fromScale == toScale)
		{
			return temperatureDelta;
		}
		decimal num = fromScale switch
		{
			TemperatureScale.Fahrenheit => temperatureDelta, 
			TemperatureScale.Celsius => ConvertCelsiusDeltaToFahrenheitDelta(temperatureDelta), 
			_ => throw new ArgumentOutOfRangeException("fromScale"), 
		};
		if (!roundThreshold.HasValue)
		{
			return ConvertDeltaToScaleFromFahrenheitDelta(num, toScale);
		}
		return ConvertDeltaToScaleFromFahrenheitDelta(decimal.op_Implicit(((double)num).RoundUsingThreshold(roundThreshold.Value)), toScale);
	}

	public static string StringFormat(double temperature, TemperatureScale scale, DisplayType displayType = DisplayType.Default)
	{
		return StringFormat((decimal)temperature, scale, displayType);
	}

	public static string StringFormat(float temperature, TemperatureScale scale, DisplayType displayType = DisplayType.Default)
	{
		return StringFormat((decimal)temperature, scale, displayType);
	}

	public static string StringFormat(byte temperature, TemperatureScale scale, DisplayType displayType = DisplayType.Default)
	{
		return StringFormat(decimal.op_Implicit(temperature), scale, displayType);
	}

	public static string StringFormat(decimal temperature, TemperatureScale scale, DisplayType displayType = DisplayType.Default)
	{
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		if ((uint)scale <= 2u)
		{
			if (displayType != DisplayType.Default)
			{
				return $"{decimal.ToInt32(temperature)}{scale.Description()}";
			}
			return temperature.ToString("N1") + " " + scale.Description();
		}
		throw new FormatException($"The specified scale of '{scale}' is not supported.");
	}

	public string ToString()
	{
		return ToString("F");
	}

	public string ToString(string format)
	{
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		if (string.IsNullOrEmpty(format))
		{
			format = "C";
		}
		format = format.Trim().ToUpperInvariant();
		if (!(format == "F"))
		{
			if (!(format == "K"))
			{
				if (format == "G" || format == "C")
				{
					return StringFormat(Celsius, TemperatureScale.Celsius);
				}
				throw new FormatException("The '" + format + "' format string is not supported.");
			}
			return StringFormat(Kelvin, TemperatureScale.Kelvin);
		}
		return StringFormat(Fahrenheit, TemperatureScale.Fahrenheit);
	}
}
