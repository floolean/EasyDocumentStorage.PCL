using System;
using System.IO;
using Newtonsoft.Json;

namespace EasyDocumentStorage.Serialization
{

	/// <summary>
	/// Json document serializer.
	/// </summary>
	public class JsonDocumentSerializer : IEZDocumentSerializer
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="T:EasyDocumentStorage.Serialization.JsonDocumentSerializer"/> class.
		/// </summary>
		public JsonDocumentSerializer()
		{
			Settings = new JsonSerializerSettings();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:EasyDocumentStorage.Serialization.JsonDocumentSerializer"/> class.
		/// </summary>
		/// <param name="settings">Settings.</param>
		public JsonDocumentSerializer(JsonSerializerSettings settings)
		{
			Settings = settings;
		}

		/// <summary>
		/// Gets or sets Newtonsoft.Json settings.
		/// </summary>
		/// <value>The settings.</value>
		public JsonSerializerSettings Settings { get; set; }

		/// <summary>
		/// Deserialize the specified stream.
		/// </summary>
		/// <param name="stream">Stream.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T Deserialize<T>(Stream stream)
		{
			return JsonConvert.DeserializeObject<T>(stream.ToUtf8String(), Settings);
		}

		/// <summary>
		/// Serialize the specified stream and instance.
		/// </summary>
		/// <param name="stream">Stream.</param>
		/// <param name="instance">Instance.</param>
		public void Serialize(Stream stream, object instance)
		{

			var json = JsonConvert.SerializeObject(instance, Settings);

			var buffer = json.ToBuffer();

			stream.Write(buffer, 0, buffer.Length);

		}

	}
}

