using System;
using System.IO;

namespace EasyDocumentStorage.Crypto
{
	/// <summary>
	/// Interface for a simple encryption service
	/// </summary>
	public interface IEasyDocumentEncryptionService
	{
		string Key { get; set; }
		byte[] IV { get; set; }
		Stream Encrypt(Stream stream);
		Stream Decrypt(Stream stream);
	}
}

