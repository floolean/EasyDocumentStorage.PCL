using System;
using System.Collections.Generic;

namespace EasyDocumentStorage.Cache
{

	/// <summary>
	/// EZDocumentCache.
	/// </summary>
	public class EZDocumentCache : IEZDocumentCache
	{

		int _maxObjects;
		Dictionary<string, object> _cacheDictionary = new Dictionary<string, object>();

		/// <summary>
		/// Initializes a new instance of the <see cref="T:EasyDocumentStorage.Cache.EZDocumentCache"/> class.
		/// </summary>
		public EZDocumentCache()
		{
			_maxObjects = 1000;
		}

		/// <summary>
		/// Gets or sets the max objects.
		/// </summary>
		/// <value>The max objects.</value>
		public int MaxObjects
		{
			get { return _maxObjects; }

			set
			{
				_maxObjects = Math.Max(value, 1);
			}
		}

		/// <summary>
		/// Cache the specified documentId and document.
		/// </summary>
		/// <param name="documentId">Document identifier.</param>
		/// <param name="document">Document.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void Cache<T>(string documentId, T document)
		{

			if (_cacheDictionary.ContainsKey(documentId))
				_cacheDictionary.Remove(documentId);

			while (_cacheDictionary.Count >= _maxObjects)
			{

				var iterator = _cacheDictionary.GetEnumerator();

				if (iterator.MoveNext())
					_cacheDictionary.Remove(iterator.Current.Key);

			}

			_cacheDictionary.Add(documentId, document);

		}

		/// <summary>
		/// Tries the get.
		/// </summary>
		/// <returns>The get.</returns>
		/// <param name="documentId">Document identifier.</param>
		/// <param name="document">Document.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public bool TryGet<T>(string documentId, out T document)
		{

			document = default(T);

			object obj = null;

			var result = _cacheDictionary.TryGetValue(documentId, out obj);

			if (result)
				document = (T)obj;

			return result;

		}

		/// <summary>
		/// Clear this instance.
		/// </summary>
		public void Clear()
		{
			_cacheDictionary.Clear();
		}

		/// <summary>
		/// Remove the specified documentId.
		/// </summary>
		/// <param name="documentId">Document identifier.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void Remove<T>(string documentId)
		{
			if (_cacheDictionary.ContainsKey(documentId))
				_cacheDictionary.Remove(documentId);
		}
	}
}

