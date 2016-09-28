using System;
using System.Collections.Generic;
using System.Diagnostics;
using EasyDocumentStorage.Cache;
using EasyDocumentStorage.Crypto;
using EasyDocumentStorage.Serialization;

namespace EasyDocumentStorage.PCL.PerformaceTests
{
	class MainClass
	{

		const int kDocumentCount = 1000;

		public static void Main(string[] args)
		{

			Console.BufferWidth = 160;

			Console.WriteLine("********************************************");
			Console.WriteLine("****EasyDocumentStorage Performance Tests***");
			Console.WriteLine("");

			var cache = new EZDocumentCache();
			var binarySerializer = new BinaryDocumentSerializer();
			var encryptionService = new EZDocumentEncryptionService("password", "saltsalt");
			EZDocumentStorage.Default.Register<MyDocument>(document => document.Id.ToString());


			RunTests("Json.Net", kDocumentCount, (eds) => { });
			RunTests("Json.Net + Cache", kDocumentCount, (eds) => { eds.Cache = cache; });
			RunTests("Json.Net + Crypto", kDocumentCount, (eds) => { eds.EncryptionService = encryptionService; });
			RunTests("Json.Net + Crypto + Cache", kDocumentCount, (eds) => { eds.Cache = cache; eds.EncryptionService = encryptionService; });

			RunTests("SharpSerializer.PCL", kDocumentCount, (eds) => { eds.Serializer = binarySerializer; });
			RunTests("SharpSerializer.PCL + Cache", kDocumentCount, (eds) => { eds.Serializer = binarySerializer; eds.Cache = cache; });
			RunTests("SharpSerializer.PCL + Crypto", kDocumentCount, (eds) => { eds.Serializer = binarySerializer; eds.EncryptionService = encryptionService; });
			RunTests("SharpSerializer.PCL + Crypto + Cache", kDocumentCount, (eds) => { eds.Serializer = binarySerializer; eds.Cache = cache; eds.EncryptionService = encryptionService; });

			Console.WriteLine();
			Console.WriteLine("Done. Press any key.");
			Console.ReadKey();

		}

		static void RunTests(string description, int documentCount, Action<EZDocumentStorage> setup)
		{

			EZDocumentStorage.Default.Cache = null;
			EZDocumentStorage.Default.EncryptionService = null;
			EZDocumentStorage.Default.Serializer = new JsonDocumentSerializer();

			var desc = string.Format("{0} =>\t", description);

			setup(EZDocumentStorage.Default);

			var documents = CreateDocuments(documentCount);

			Run(string.Format("{0}INS {1} documents", desc, documentCount), () => InsertDocuments(documents));
			Run(string.Format("{0}UPD {1} documents", desc, documentCount), () => UpdateDocuments(documents));
			Run(string.Format("{0}GET {1} documents", desc, documentCount), () => RetrieveDocuments());

			Console.WriteLine("Cleaning up..");
			DeleteDocuments(documents);

		}

		static List<MyDocument> CreateDocuments(int count)
		{

			var documents = new List<MyDocument>();

			for (int i = 0; i < count; i++)
			{
				documents.Add(new MyDocument()
				{
					Id = i,
					Author = "Author",
					Name = "Document " + i
				});
			}

			return documents;

		}

		static bool UpdateDocuments(IEnumerable<MyDocument> documents)
		{
			return EZDocumentStorage.Default.InsertOrUpdateAll(documents);
		}

		static IEnumerable<MyDocument> RetrieveDocuments()
		{
			return EZDocumentStorage.Default.Get<MyDocument>();
		}

		static void InsertDocuments(List<MyDocument> documents)
		{
			EZDocumentStorage.Default.InsertAll(documents as IEnumerable<MyDocument>);
		}

		static void DeleteDocuments(List<MyDocument> documents)
		{
			EZDocumentStorage.Default.DeleteAll(documents as IEnumerable<MyDocument>);
		}

		static void Run(string description, Action action)
		{

			Console.Write("Executing {0}..", description);

			var watch = Stopwatch.StartNew();

			action();

			watch.Stop();

			var elapsedMs = watch.ElapsedMilliseconds;

			Console.WriteLine(" {0} ms (avg. {1} ms)", elapsedMs, elapsedMs / kDocumentCount);

		}

	}
}
