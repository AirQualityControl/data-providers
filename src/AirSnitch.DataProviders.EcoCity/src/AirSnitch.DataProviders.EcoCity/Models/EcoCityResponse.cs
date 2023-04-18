using AirSnitch.SDK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AirSnitch.DataProviders.EcoCity.Models
{
	public class EcoCityResponse
	{
		private readonly List<AirPollutionResultDto> _rawResult;

		public EcoCityResponse(List<AirPollutionResultDto> rawResult) {
			_rawResult = rawResult;
		}

		/// <summary>
		/// Property that returns Air pollution measurement date and time as DateTime.
		/// If parsing of data was success datetime will return otherwise DateTime.MinValue will be returned
		/// </summary>
		/// <returns></returns>
		public DateTime MeasurementDateTime
		{
			get
			{
				var timeAsString = _rawResult.FirstOrDefault(d => d.Weather != null)?.Weather.Time;
				return DateTime.TryParse(timeAsString, out DateTime result) ? result : DateTime.MinValue;
			}
		}

		/// <summary>
		/// Returns calculated by provider AIQ index 
		/// </summary>
		public int Index
		{
			get
			{
				AirPollutionResultDto dto = _rawResult.SingleOrDefault(p => p.Name == "PM2.5");
				if (dto == null)
				{
					Console.WriteLine("Unable to fetch result with parameter name PM2.5");
					return 0;
				}
				return dto.Index ?? 0;
			}
		}

		public double? PM1 => GetValue("PM1.0");

		public double? PM2_5 => GetValue("PM2.5");

		public double? Temperature => GetValue("Temperature");

		public double? Pressure => GetValue("Pressure");

		public double? Humidity => GetValue("Humidity");

		public double? PM10 => GetValue("PM10");

		private double? GetValue(string propName)
		{
			AirPollutionResultDto dto = _rawResult.SingleOrDefault(p => p.Name == propName);
			if (dto == null) {
				Console.WriteLine($"Unable to fetch result with parameter name {propName}");
				return default;
			}
			if (double.TryParse(dto.Value, out double value)) {
				return value;
			} else {
				return default;
			}
		}

	}
}