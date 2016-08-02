using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    internal static class ByteArrayExtensions
    {

        /// <summary>
        /// Get Md5 hash from byte buffer as byte array
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        internal static byte[] ToMd5Buffer(this byte[] array)
        {

            var md5 = PCLCrypto.WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(PCLCrypto.HashAlgorithm.Md5);

            return md5.HashData(array);

        }

        /// <summary>
        /// Get Md5 hash from byte buffer as string 
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        internal static string ToMd5String(this byte[] array)
        {

            var result = array.ToMd5Buffer();

            return BitConverter.ToString(result);

        }

        /// <summary>
        /// Get byte buffer as base64 byte array
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        internal static byte[] ToBase64Buffer(this byte[] array)
        {

            var buffer = Encoding.UTF8.GetBytes(array.ToBase64String());

            return buffer;

        }

        /// <summary>
        /// Get byte buffer as base64 string
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        internal static string ToBase64String(this byte[] array)
        {

            var base64String = Convert.ToBase64String(array);

            return base64String;

        }

        /// <summary>
        /// Convert byte array to UTF8 string
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        internal static string ToUtf8String(this byte[] array)
        {

            var result = Encoding.UTF8.GetString(array, 0, array.Length);

            return result;

        }

		/// <summary>
		/// Convert byte array to a stream (MemoryStream)
		/// </summary>
		/// <returns>The stream.</returns>
		/// <param name="array">Array.</param>
		internal static Stream ToStream(this byte[] array)
		{
			return new MemoryStream(array);
		}

    }
}
