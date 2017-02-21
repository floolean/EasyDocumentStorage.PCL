using System.Text;
using PCLCrypto;

namespace System
{
    internal static class StringExtension
    {

        /// <summary>
        /// Get the MD5 hash of a string as string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        internal static string ToMd5String(this string text)
        {
           	return Encoding.UTF8.GetBytes(text).ToMd5String().Replace("-", string.Empty).ToLower();
        }

        /// <summary>
        /// Get the MD5 hash of a string as byte array
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        internal static byte[] ToMd5Buffer(this string text)
        {
            return Encoding.UTF8.GetBytes(text).ToMd5Buffer();
        }

        /// <summary>
        /// Generate a random string with an optional length (default 20)
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        internal static string GetRandomString(int length = 20)
        {

            var buffer = WinRTCrypto.CryptographicBuffer.GenerateRandom(length);

            return buffer.ToUtf8String();

        }

        /// <summary>
        /// Generate a random base64 string with an optional length (default 20)
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        internal static string GetRandomBase64String(int length = 20)
        {

            var buffer = WinRTCrypto.CryptographicBuffer.GenerateRandom(length);

            return buffer.ToBase64String();

        }

		internal static byte[] ToBuffer(this string str, Encoding encoding = null)
		{

			if (encoding == null)
				encoding = Encoding.UTF8;

			var buffer = encoding.GetBytes(str);

			return buffer;

		}

    }
}
