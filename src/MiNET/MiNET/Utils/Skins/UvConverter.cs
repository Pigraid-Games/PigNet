using System;
using System.Collections.Generic;
using MiNET.Utils.Skins;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class UvConverter : JsonConverter
{
	public override bool CanConvert(Type objectType)
	{
		return objectType == typeof(Dictionary<string, FaceUv>);
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		var uvDictionary = new Dictionary<string, FaceUv>();

		if (reader.TokenType == JsonToken.StartObject)
		{
			var obj = JObject.Load(reader);
			foreach (JProperty property in obj.Properties())
			{
				FaceUv faceUv = property.Value.ToObject<FaceUv>();
				uvDictionary[property.Name] = faceUv;
			}
		}
		else if (reader.TokenType == JsonToken.StartArray)
		{
			float[] uvArray = JArray.Load(reader).ToObject<float[]>();
			uvDictionary["default"] = new FaceUv
			{
				Uv = uvArray,
				UvSize = new float[] { 16, 16 }
			};
		}

		return uvDictionary;
	}

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		var uv = (Dictionary<string, FaceUv>) value;

		if (uv.ContainsKey("default") && uv.Count == 1)
			serializer.Serialize(writer, uv["default"].Uv);
		else
			serializer.Serialize(writer, uv);
	}
}