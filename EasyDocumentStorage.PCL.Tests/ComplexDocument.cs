using System;
using System.Collections.Generic;

namespace EasyDocumentStorage.PCL.Tests
{

	interface IContent
	{

	}

	class TextContent : IContent
	{
		public string Text { get; set; }
	}

	class NumericContent : IContent
	{
		public int Number { get; set; }
	}

	class ComplexDocument
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Author { get; set; }
		public List<IContent> Contents { get; set; } // a document can have many IContent derived items
	}

}

