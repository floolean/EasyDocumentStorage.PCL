using System;
using System.IO;
using Polenter.Serialization;

namespace EasyDocumentStorage.Serialization
{
	public class BinaryDocumentSerializer : IEasyDocumentSerializer
	{

		SharpSerializer _serializer = new SharpSerializer(true);

		public T Deserialize<T>(Stream stream)
		{
			return (T)_serializer.Deserialize(stream);
		}

		public void Serialize(Stream stream, object instance)
		{
			_serializer.Serialize(instance, stream);
		}
	}
}

