﻿using System;
using EasyDocumentStorage.Cache;
using EasyDocumentStorage.Crypto;
using EasyDocumentStorage.Serialization;
using NUnit.Framework;

namespace EasyDocumentStorage.PCL.Tests
{

	public class EZDocumentStorageTestsJson
	{

		protected static EZDocumentStorage _eds = EZDocumentStorage.Default;
		protected static EZDocumentCache _cache = new EZDocumentCache();
		protected static JsonDocumentSerializer _jsonSerialier = new JsonDocumentSerializer();
		protected static BinaryDocumentSerializer _binarySerializer = new BinaryDocumentSerializer();
		protected static EZDocumentEncryptionService _encryptionService = new EZDocumentEncryptionService("password", "saltsalt");

		static bool _documentTypesRegistered;

		[TestFixtureSetUp]
		public void BaseSetup()
		{
			if (!_documentTypesRegistered)
			{
				EZDocumentStorage.Default.Register<DocumentA>(doc => doc.Id.ToString());
				EZDocumentStorage.Default.Register<DocumentB>(doc => doc.Id.ToString());
				EZDocumentStorage.Default.Register<ComplexDocument>(doc => doc.Id.ToString());
			}
			_documentTypesRegistered = true;
		}

		[Test()]
		public void InsertDocument()
		{

			var doc = new DocumentA()
			{
				Id = 1,
				Created = DateTime.Parse("1.1.2000")
			};

			Assert.IsTrue(EZDocumentStorage.Default.Insert(doc));

			Assert.IsTrue(EZDocumentStorage.Default.Exists<DocumentA>(1.ToString()));

		}

		[Test()]
		public void UpdateDocument()
		{

			var doc = new DocumentA()
			{
				Id = 1,
				Created = DateTime.Parse("1.1.2000")
			};

			Assert.IsTrue(EZDocumentStorage.Default.InsertOrUpdate(doc));

			Assert.IsTrue(EZDocumentStorage.Default.Exists<DocumentA>(1.ToString()));

		}

		[Test()]
		public void DeleteDocument()
		{



			var doc = new DocumentA()
			{
				Id = 1,
				Created = DateTime.Parse("1.1.2000")
			};

			Assert.IsTrue(_eds.Insert(doc));

			Assert.IsTrue(_eds.Exists<DocumentA>(1.ToString()));

			Assert.IsTrue(_eds.Delete<DocumentA>(doc));

		}

		[Test()]
		public void RetriveDocument()
		{



			var doc = new DocumentA()
			{
				Id = 1,
				Created = DateTime.Parse("1.1.2000")
			};

			Assert.IsTrue(_eds.Insert(doc));

			Assert.IsTrue(_eds.Exists<DocumentA>(1.ToString()));

			Assert.IsNotNull(_eds.Get<DocumentA>(d => d.Id == 1));

		}


		[TearDown]
		public void Cleanup()
		{

			EZDocumentStorage.Default.DeleteAll(EZDocumentStorage.Default.Get<DocumentA>());
			EZDocumentStorage.Default.DeleteAll(EZDocumentStorage.Default.Get<DocumentB>());
			EZDocumentStorage.Default.DeleteAll(EZDocumentStorage.Default.Get<ComplexDocument>());

		}

	}
}

