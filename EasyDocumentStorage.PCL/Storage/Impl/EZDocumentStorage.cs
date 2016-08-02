using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using EasyDocumentStorage.Cache;
using EasyDocumentStorage.Serialization;
using EasyDocumentStorage.Crypto;

namespace EasyDocumentStorage
{
	/// <summary>
	/// EasyDocumentStorage default implementation.
	/// </summary>
	public class EZDocumentStorage : IEZDocumentStorage
	{

		#region MEMBERS

		static EZDocumentStorage _instance;

		FsBlobRepository _blobRepository;
		IEZDocumentCache _cache;
		IEZDocumentSerializer _serializer;

		readonly Dictionary<Type, TypeMapper> _documentTypeMappers = new Dictionary<Type, TypeMapper>();

		/// <summary>
		/// Gets or sets the cache.
		/// </summary>
		/// <value>The cache.</value>
		public IEZDocumentCache Cache
		{
			get { return _cache; }
			set { _cache = value; }
		}

		/// <summary>
		/// Gets or sets the serializer.
		/// </summary>
		/// <value>The serializer.</value>
		public IEZDocumentSerializer Serializer
		{
			get { return _serializer; }
			set { _serializer = value; }
		}

		/// <summary>
		/// Gets or sets the encryption service.
		/// </summary>
		/// <value>The encryption service.</value>
		public IEZDocumentEncryptionService EncryptionService
		{
			get;
			set;
		}

		#endregion

		#region ID MAPPING

		public bool Register<T>(Func<T, string> documentTypeIdMapper, IEZDocumentSerializer serializer = null)
		{

			var type = typeof(T);

			if (_documentTypeMappers.ContainsKey(type))
				throw new EZDocumentStorageException(string.Format("Document type '{0}' has already been registered.", type.Name));

			_documentTypeMappers.Add (type, new TypeMapper () { // register the mapper

				IdMapper = (documentInstance) => {

					var documentId = documentTypeIdMapper ((T)documentInstance);

					return documentId;

				},

				Serialzer = serializer
					
			});

			return true;

		}

		#endregion

		#region SYNCHRONOUS METHODS

		public bool Insert<T>(T document)
		{

			var documentId = GetDocumentId(document);

			var bucketId = GetBucketId<T>();

			var result = WriteDocument(bucketId, documentId, document, false);

			return result;

		}

		public bool Insert<T>(IEnumerable<T> documents)
		{

			var result = true;

			foreach (var document in documents)
			{
				result &= Insert(document);
			}

			return result;

		}

		public bool InsertOrUpdate<T>(T document)
		{

			var documentId = GetDocumentId(document);

			var bucketId = GetBucketId<T>();

			var result = WriteDocument(bucketId, documentId, document, true);

			return result;

		}

		public bool InsertOrUpdate<T>(IEnumerable<T> documents)
		{
			var result = true;

			foreach (var document in documents)
			{
				result &= InsertOrUpdate(document);
			}

			return result;

		}

		public bool Delete<T>(T document)
		{

			var docId = GetDocumentId(document);

			var bucketId = GetBucketId<T>();

			_blobRepository.DeleteBlob(bucketId, docId).RunSynchronously();

			RemoveCachedDocument<T>(bucketId, docId);

			return true;

		}

		public bool Delete<T>(IEnumerable<T> documents)
		{
			var result = true;

			foreach (var document in documents)
			{
				result &= Delete(document);
			}

			return result;
   		}

		public bool Exists<T>(string documentId)
		{

			if (string.IsNullOrEmpty(documentId))
				throw new EZDocumentStorageException("Document Ids can't be null or empty.");

			var bucketId = GetBucketId<T>();

			var blobs = _blobRepository.GetBlobs(bucketId).Result;

			return blobs.Contains(documentId);

		}

		public IEnumerable<T> Get<T>(Func<T, bool> clause = null)
		{

			var bucketId = GetBucketId<T>();

			var documents = new List<T>();

			var documentIds = _blobRepository.GetBlobs(bucketId).Result;

			foreach (var documentId in documentIds)
			{

				T document = ReadDocument<T>(bucketId, documentId);

				if (document != null && (clause == null || clause(document)))
					documents.Add(document);

			}

			return documents;

		}

		public T GetById<T>(string documentId)
		{
			
			var bucketId = GetBucketId<T>();

			return ReadDocument<T>(bucketId,documentId);

		}

		public string GetDocumentId<T>(T document)
		{

			if (document == null)
				throw new NullReferenceException("document");

			var idMapper = GetIdMapper<T>();

			var id = idMapper(document);

			if (string.IsNullOrEmpty(id))
				throw new EZDocumentStorageException("A document id can not be null or empty.");

			return id;

		}

		#endregion

		#region INTERNAL

		private EZDocumentStorage()
		{
			_blobRepository = new FsBlobRepository("dse.storage");
		}

		private string GetBucketId<T>()
		{

			var type = typeof(T);

			return string.Format("{0}.{1}", type.Namespace, type.Name);

		}

		private TypeMapper GetMapper<T>()
		{

			TypeMapper mapper;

			if (_documentTypeMappers.TryGetValue(typeof(T), out mapper))
				return mapper;

			throw new EZDocumentStorageException(string.Format("No type mapper has been registered for '{0}'", typeof(T).Name));

		}

		private Func<object, string> GetIdMapper<T>()
		{

			TypeMapper mapper;

			if (_documentTypeMappers.TryGetValue(typeof(T), out mapper))
				return mapper.IdMapper;

			throw new EZDocumentStorageException(string.Format("No Id mapper has been registered for '{0}'", typeof(T).Name));

		}
		
		private IEZDocumentSerializer GetSerializer<T>()
		{

			var mapper = GetMapper<T> ();

			return mapper.Serialzer ?? _serializer;

		}

		private bool TryGetCachedDocument<T>(string bucketId, string documentId, out T document)
		{

			document = default(T);

			if (_cache == null)
				return false;

			return _cache.TryGet(string.Format("{0}.{1}", bucketId, documentId), out document);

		}

		private void CacheDocument<T>(string bucketId, string documentId, T document)
		{
			
			if (_cache == null)
				return;

			_cache.Cache(string.Format("{0}.{1}", bucketId, documentId), document);

		}

		private void RemoveCachedDocument<T>(string bucketId, string documentId)
		{

			if (_cache == null)
				return;

			_cache.Remove<T>(string.Format("{0}.{1}", bucketId, documentId));

		}

		private T ReadDocument<T>(string bucketId, string documentId)
		{

			if (!Exists<T>(documentId))
				throw new EZDocumentStorageException(string.Format("Document with Id '{0}' does not exist", documentId));

			T document = default(T);

			if (TryGetCachedDocument(bucketId, documentId, out document))
				return document;

			using (var blobStream = _blobRepository.GetBlobStream(bucketId, documentId).Result)
			{

				Stream stream = blobStream;

				if (EncryptionService != null)
					stream = EncryptionService.Decrypt(blobStream);

				document = GetSerializer<T>().Deserialize<T>(stream);

				CacheDocument(bucketId, documentId, document);

				return document;

			}

		}

		private bool WriteDocument<T>(string bucketId, string documentId, T document, bool overwrite = false)
		{

			using (var memoryStream = new MemoryStream())
			{

				Stream stream = memoryStream;

				GetSerializer<T>().Serialize(memoryStream, document);

				memoryStream.Position = 0;

				if (EncryptionService != null)
				{

					stream = EncryptionService.Encrypt(memoryStream);

					stream.Position = 0;

				}

				var result = _blobRepository.StoreBlob(bucketId, documentId, stream, overwrite).Result;

				if (result)
					CacheDocument(bucketId, documentId, document);

				return result;

			}

		}

		#endregion

		#region SINGLETON

		/// <summary>
		/// Gets the default instance of EasyDocumentStorage. Json is used as default serializer, no cache or encryption initialized.
		/// </summary>
		/// <value>The default.</value>
		public static EZDocumentStorage Default
		{
			get
			{
				if (_instance == null)
					_instance = new EZDocumentStorage()
					{
						Serializer = new JsonDocumentSerializer()
					};

				return _instance;
			}
		}

		#endregion

		#region NESTED TYPES 

		private struct TypeMapper
		{
			public Func<object, string> IdMapper { get; set; }
			public IEZDocumentSerializer Serialzer { get; set; }
		}

		public class EZDocumentStorageException : System.Exception
		{
			public EZDocumentStorageException() { }
			public EZDocumentStorageException(string message) : base(message) { }
		}
		
		#endregion

	}
}

