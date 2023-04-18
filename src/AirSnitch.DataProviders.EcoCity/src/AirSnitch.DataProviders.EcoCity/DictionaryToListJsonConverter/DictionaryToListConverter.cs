using AirSnitch.DataProviders.EcoCity.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace AirSnitch.DataProviders.EcoCity.DictionaryToListJsonConverter;

public class DictionaryToListConverter : JsonConverter<List<AirPollutionResultDto>>
{
	public override List<AirPollutionResultDto> ReadJson(JsonReader reader, Type objectType, List<AirPollutionResultDto> existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		JToken token = JToken.Load(reader);
		switch (token.Type)
		{
			case JTokenType.Object:
				JObject obj = (JObject)token;
				Dictionary<string, JObject> dict = obj.ToObject<Dictionary<string, JObject>>();
				var values = new List<AirPollutionResultDto>();
				foreach (KeyValuePair<string, JObject> pair in dict) {
					values.Add(pair.Value.ToObject<AirPollutionResultDto>());
				}
				return values;

			case JTokenType.Array:
				JArray array = (JArray)token;
				List<AirPollutionResultDto> list = array.ToObject<List<AirPollutionResultDto>>();
				return list;

			default:
				throw new JsonSerializationException("Unexpected token type: " + token.Type);
		}
	}

	public override void WriteJson(JsonWriter writer, List<AirPollutionResultDto> value, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
