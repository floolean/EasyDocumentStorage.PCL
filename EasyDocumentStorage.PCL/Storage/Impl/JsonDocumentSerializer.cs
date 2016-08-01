using System;
using System.IO;
using Newtonsoft.Json;

namespace EasyDocumentStorage.Serialization
{

	public class JsonDocumentSerializer : IEasyDocumentSerializer
	{

		public JsonDocumentSerializer()
		{
			Settings = new JsonSerializerSettings();
		}

		public JsonSerializerSettings Settings { get; set; }

		public JsonDocumentSerializer(JsonSerializerSettings settings)
		{
			Settings = settings;
		}

		public T Deserialize<T>(Stream stream)
		{
			return JsonConvert.DeserializeObject<T>(stream.ToUtf8String(), Settings);
		}

		public void Serialize(Stream stream, object instance)
		{

			var json = JsonConvert.SerializeObject(instance, Settings);

			var buffer = json.ToBuffer();

			stream.Write(buffer, 0, buffer.Length);

		}

	}
}

