using System;
using System.IO;
using System.Text;
using PCLCrypto;

namespace EasyDocumentStorage.Crypto
{
	public class EasyDocumentEncryptionService : IEasyDocumentEncryptionService
	{

		const int kKeyLength = 16; //128 bit

		string _key;
		string _salt;
		ICryptographicKey _cryptographicKey;

		public EasyDocumentEncryptionService(string password, string salt)
		{
			_key = password;
			_salt = salt;
			RegenerateKey();
		}

		public string Key
		{
			get { return _key; }
			set
			{

				if (string.IsNullOrEmpty(value))
					throw new ArgumentNullException(nameof(value));

				_key = value;

				RegenerateKey();

			}
		}

		public string Salt
		{
			get { return _salt; }
			set
			{

				if (string.IsNullOrEmpty(value))
					throw new ArgumentNullException(nameof(value));

				_salt = value;

				RegenerateKey();

			}
		}

		public byte[] IV { get; set; }

		public Stream Encrypt(Stream stream)
		{

			var data = stream.ToBuffer();

			var encryptedStream = WinRTCrypto.CryptographicEngine.Encrypt(_cryptographicKey, data, IV).ToStream();

			return encryptedStream;

		}

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

		public static byte[] GenerateKey(string password, string salt)
		{

			var key = GenerateCryptoKey(password, salt);

			return key.Export();

		}

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

