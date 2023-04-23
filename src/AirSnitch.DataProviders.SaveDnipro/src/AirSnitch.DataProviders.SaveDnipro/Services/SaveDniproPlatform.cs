using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace AirSnitch.DataProviders.SaveDnipro.Services;

public class SaveDniproPlatform
{
	public IEnumerable<SaveDniproSensorData> GetSensorsDataStream()
	{
		HttpClient client = new HttpClient();

		using (Stream s = client.GetStreamAsync("https://api.saveecobot.com/output.json").Result)
		using (StreamReader sr = new StreamReader(s))
		using (JsonReader reader = new JsonTextReader(sr))
		{
			JsonSerializer serializer = new JsonSerializer();
			var stationsStream = serializer.Deserialize<IEnumerable<SaveDniproSensorData>>(reader);
			return stationsStream;
		}
	}
}
