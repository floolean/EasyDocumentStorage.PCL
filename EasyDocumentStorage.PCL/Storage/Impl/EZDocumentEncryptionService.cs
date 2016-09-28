using System;
using System.IO;
using System.Text;
using PCLCrypto;

namespace EasyDocumentStorage.Crypto
{

	/// <summary>
	/// Document encryption service.
	/// </summary>
	public class EZDocumentEncryptionService : IEZDocumentEncryptionService
	{

		const int kKeyLength = 16; //128 bit

		string _key;
		string _salt;
		ICryptographicKey _cryptographicKey;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:EasyDocumentStorage.Crypto.EZDocumentEncryptionService"/> class.
		/// </summary>
		/// <param name="password">Password.</param>
		/// <param name="salt">Salt.</param>
		public EZDocumentEncryptionService(string password, string salt)
		{
			_key = password;
			_salt = salt;
			RegenerateKey();
		}

		/// <summary>
		/// Gets or sets the key.
		/// </summary>
		/// <value>The key.</value>
		public string Key
		{
			get { return _key; }
			set
			{

				if (string.IsNullOrEmpty(value))
					throw new ArgumentNullException("value");

				_key = value;

				RegenerateKey();

			}
		}

		/// <summary>
		/// Gets or sets the salt.
		/// </summary>
		/// <value>The salt.</value>
		public string Salt
		{
			get { return _salt; }
			set
			{

				if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException("value");

				_salt = value;

				RegenerateKey();

			}
		}

		/// <summary>
		/// Gets or sets the iv.
		/// </summary>
		/// <value>The iv.</value>
		public byte[] IV { get; set; }

		/// <summary>
		/// Encrypt the specified stream.
		/// </summary>
		/// <param name="stream">Stream.</param>
		public Stream Encrypt(Stream stream)
		{

			var data = stream.ToBuffer();

			var encryptedStream = WinRTCrypto.CryptographicEngine.Encrypt(_cryptographicKey, data, IV).ToStream();

			return encryptedStream;

		}

		/// <summary>
		/// Decrypt the specified stream.
		/// </summary>
		/// <param name="stream">Stream.</param>
		public Stream Decrypt(Stream stream)
		{

			var data = stream.ToBuffer();

			var decryptedStream = WinRTCrypto.CryptographicEngine.Decrypt(_cryptographicKey, data, IV).ToStream();

			return decryptedStream;

		}

		private void RegenerateKey()
		{
			_cryptographicKey = GenerateCryptoKey(_key, _salt);
		}

		/// <summary>
		/// Generates the key.
		/// </summary>
		/// <returns>The key.</returns>
		/// <param name="password">Password.</param>
		/// <param name="salt">Salt.</param>
		public static byte[] GenerateKey(string password, string salt)
		{

			var key = GenerateCryptoKey(password, salt);

			return key.Export();

		}

		/// <summary>
		/// Generates the crypto key.
		/// </summary>
		/// <returns>The crypto key.</returns>
		/// <param name="password">Password.</param>
		/// <param name="salt">Salt.</param>
		public static ICryptographicKey GenerateCryptoKey(string password, string salt)
		{

			var keyMaterial = Encoding.UTF8.GetBytes(password);

			var saltMaterial = Encoding.UTF8.GetBytes(salt);

			int iterations = 1000;

			var key = NetFxCrypto.DeriveBytes.GetBytes(password, saltMaterial, iterations, kKeyLength);

			var provider = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);

			var cryptoKey = provider.CreateSymmetricKey(key);

			return cryptoKey;

		}

	}

}

