using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Jint;

namespace AirSnitch.DataProviders.EcoCity.Services;

public static class EcoCityRequestUtilities
{

	public static async Task<string> GetApiKeyAsync()
	{
		HttpClient client = new HttpClient();
		var jsFileContent = await client.GetStringAsync("https://eco-city.org.ua/js/map.js");
		var linesArray = jsFileContent.Split("\n");
		var keyDeclarationLine = linesArray.FirstOrDefault(line => line.Contains("key"));
		if (keyDeclarationLine != null)
		{
			var jsValue = new Engine()
				.Execute(keyDeclarationLine)
				.GetValue("key");
			return jsValue.ToString();
		}
		return string.Empty;
	}
}