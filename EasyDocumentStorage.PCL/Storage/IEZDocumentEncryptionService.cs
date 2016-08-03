using System;
using System.IO;

namespace EasyDocumentStorage.Crypto
{
	/// <summary>
	/// Interface for a simple encryption service
	/// </summary>
	public interface IEZDocumentEncryptionService
	{
		
		/// <summary>
		/// Encrypt the specified stream.
		/// </summary>
		/// <param name="stream">Stream.</param>
		Stream Encrypt(Stream stream);

		/// <summary>
		/// Decrypt the specified stream.
		/// </summary>
		/// <param name="stream">Stream.</param>
		Stream Decrypt(Stream stream);

	}
}

