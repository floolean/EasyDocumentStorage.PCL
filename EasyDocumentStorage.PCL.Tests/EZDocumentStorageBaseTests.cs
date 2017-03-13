using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyDocumentStorage.Cache;
using EasyDocumentStorage.Crypto;
using EasyDocumentStorage.Serialization;
using NUnit.Framework;

namespace EasyDocumentStorage.PCL.Tests
{
    public abstract class EZDocumentStorageBaseTests
    {

        protected EZDocumentStorage _eds = new EZDocumentStorage("tests");
        protected EZDocumentCache _cache = new EZDocumentCache();
        
        
        protected EZDocumentEncryptionService _encryptionService = new EZDocumentEncryptionService("password", "saltsalt");

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _eds.Register<DocumentA>(doc => doc.Id.ToString());
            _eds.Register<DocumentB>(doc => doc.Id.ToString());
            _eds.Register<ComplexDocument>(doc => doc.Id.ToString());
            Cleanup();
        }

        [Test()]
        public void InsertDocument()
        {

            var doc = new DocumentA()
            {
                Id = 1,
                Created = DateTime.Parse("1.1.2000")
            };

            var docId = _eds.GetDocumentId(doc);

            Assert.IsTrue(_eds.Insert(doc));

            Assert.IsTrue(_eds.Exists<DocumentA>(docId));

        }

        [Test()]
        public void UpdateDocument()
        {

            var doc = new DocumentA()
            {
                Id = 1,
                Created = DateTime.Parse("1.1.2000")
            };

            var docId = _eds.GetDocumentId(doc);

            Assert.IsTrue(_eds.InsertOrUpdate(doc));

            Assert.IsTrue(_eds.Exists<DocumentA>(docId));

        }

        [Test()]
        public void DeleteDocument()
        {

            var doc = new DocumentA()
            {
                Id = 1,
                Created = DateTime.Parse("1.1.2000")
            };

            var docId = _eds.GetDocumentId(doc);

            Assert.IsTrue(_eds.Insert(doc));

            Assert.IsTrue(_eds.Exists<DocumentA>(docId));

            Assert.IsTrue(_eds.Delete<DocumentA>(doc));

        }

        [Test()]
        public void RetrieveDocument()
        {



            var doc = new DocumentA()
            {
                Id = 1,
                Created = DateTime.Parse("1.1.2000")
            };

            var docId = _eds.GetDocumentId(doc);

            Assert.IsTrue(_eds.Insert(doc));

            Assert.IsTrue(_eds.Exists<DocumentA>(docId));

            Assert.IsNotNull(_eds.Get<DocumentA>(d => d.Id == 1));

        }


        [Test()]
        public void InsertFailsIfDocumentExists()
        {

            var doc = new DocumentA()
            {
                Id = 1,
                Created = DateTime.Parse("1.1.2000")
            };

            var docId = _eds.GetDocumentId(doc);

            Assert.IsTrue(_eds.Insert(doc));

            Assert.IsTrue(_eds.Exists<DocumentA>(docId));

            Assert.IsNotNull(_eds.Get<DocumentA>(d => d.Id == 1));

            Assert.Catch(() => _eds.Insert(doc));

        }


        [TearDown]
        public void Cleanup()
        {
            _eds.Clear<DocumentA>();
            _eds.Clear<DocumentB>();
            _eds.Clear<ComplexDocument>();
        }

    }
}
