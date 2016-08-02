using System;
using System.Text;

namespace System.IO
{
	internal static class StreamExtensions
	{
		internal static byte[] ToBuffer(this Stream stream)
		{

			if (stream is MemoryStream)
				return ((MemoryStream)stream).ToArray();


			stream.Position = 0;

			var memoryStream = new MemoryStream();

			stream.CopyTo(memoryStream);

			memoryStream.Position = 0;

			return memoryStream.ToArray();

		}

		internal static string ToUtf8String(this Stream stream)
		{

			var array = stream.ToBuffer();

			var result = Encoding.UTF8.GetString(array, 0, array.Length);

			return result;

		}

	}
}

