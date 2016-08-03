using NUnit.Framework;
using System;
namespace EasyDocumentStorage.PCL.Tests
{
	[TestFixture()]
	public class EasyDocumentStorageTests
	{

		[SetUp]
		public void Setup()
		{
			
			EZDocumentStorage.Default.Register<DocumentA>(td => td.Id.ToString());

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

		}


	}
}

