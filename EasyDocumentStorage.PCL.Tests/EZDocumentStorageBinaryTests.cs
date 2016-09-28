using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace EasyDocumentStorage.PCL.Tests
{
	[TestFixture()]
	public class EZDocumentStorageBinaryTests : EZDocumentStorageTestsJson
	{

		[OneTimeSetUp]
		public void Setup()
		{
			_eds.Serializer = _binarySerializer;
		}

		[Test()]
		public void InsertComplexDocument()
		{

			var doc = new ComplexDocument()
			{
				Id = 1,
				Name = "Name",
				Author = "Author",
				Contents = new List<IContent>()
				{
					new TextContent() { Text = "Hello" },
					new TextContent() { Text = "World" },
					new NumericContent() { Number = 42 },
				}
			};

			Assert.IsTrue(EZDocumentStorage.Default.Insert(doc));

			Assert.IsTrue(EZDocumentStorage.Default.Exists<ComplexDocument>(1.ToString()));

		}

		[Test()]
		public void RetrieveComplexDocument()
		{

			var doc = new ComplexDocument()
			{
				Id = 1,
				Name = "Name",
				Author = "Author",
				Contents = new List<IContent>()
				{
					new TextContent() { Text = "Hello" },
					new TextContent() { Text = "World" },
					new NumericContent() { Number = 42 },
				}
			};

			Assert.IsTrue(EZDocumentStorage.Default.Insert(doc));

			Assert.IsTrue(EZDocumentStorage.Default.Exists<ComplexDocument>(1.ToString()));

			var document = _eds.Get<ComplexDocument>().FirstOrDefault();

			Assert.IsNotNull(document);

			Assert.AreEqual(2, document.Contents.Count(c => c.GetType() == typeof(TextContent)));

			Assert.IsTrue(document.Contents.Any(c => c.GetType() == typeof(NumericContent)));

			var numericContent = (NumericContent)document.Contents.FirstOrDefault(c => c.GetType() == typeof(NumericContent));

			Assert.IsNotNull(numericContent);

			Assert.AreEqual(42, numericContent.Number);

		}

	}
}

