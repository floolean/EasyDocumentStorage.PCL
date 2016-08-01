using System;
using System.IO;

namespace EasyDocumentStorage.Serialization
{

	/// <summary>
	/// Interface for a simple serializer
	/// </summary>
	public interface IEasyDocumentSerializer
	{
		/// <summary>
		/// Serialize the specified object into a stream.
		/// </summary>
		/// <param name="stream">Stream.</param>
		/// <param name="instance">Instance.</param>
		void Serialize(Stream stream, object instance);

		/// <summary>
		/// Deserializes an object from the specified stream.
		/// </summary>
		/// <param name="stream">Stream.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		T Deserialize<T>(Stream stream);

	}
}

