using System;
using EasyDocumentStorage.Cache;
using EasyDocumentStorage.Crypto;
using EasyDocumentStorage.Serialization;
using NUnit.Framework;

namespace EasyDocumentStorage.PCL.Tests
{

	public class EZDocumentStorageTestsJson : EZDocumentStorageBaseTests
	{

        protected static JsonDocumentSerializer _jsonSerialier = new JsonDocumentSerializer();

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _eds.Serializer = _jsonSerialier;
        }
        
	}
}

