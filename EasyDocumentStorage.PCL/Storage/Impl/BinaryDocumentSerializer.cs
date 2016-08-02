using System;
using System.IO;
using Polenter.Serialization;

namespace EasyDocumentStorage.Serialization
{

	/// <summary>
	/// Binary document serializer.
	/// </summary>
	public class BinaryDocumentSerializer : IEZDocumentSerializer
	{

		SharpSerializer _serializer = new SharpSerializer(true);

		/// <summary>
		/// Deserialize the specified stream.
		/// </summary>
		/// <param name="stream">Stream.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T Deserialize<T>(Stream stream)
		{
			return (T)_serializer.Deserialize(stream);
		}

		/// <summary>
		/// Serialize the specified stream and instance.
		/// </summary>
		/// <param name="stream">Stream.</param>
		/// <param name="instance">Instance.</param>
		public void Serialize(Stream stream, object instance)
		{
			_serializer.Serialize(instance, stream);
		}
	}
}

