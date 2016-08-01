using System;
using System.Collections.Generic;

namespace EasyDocumentStorage.Cache
{
	public class EasyDocumentCache : IEasyDocumentCache
	{

		int _maxObjects;
		Dictionary<string, object> _cacheDictionary = new Dictionary<string, object>();

		public EasyDocumentCache()
		{
			_maxObjects = 1000;
		}

		public int MaxObjects
		{
			get { return _maxObjects; }

			set
			{
				_maxObjects = Math.Max(value, 1);
			}
		}

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

		public bool TryGet<T>(string documentId, out T document)
		{

			document = default(T);

			object obj = null;

			var result = _cacheDictionary.TryGetValue(documentId, out obj);

			if (result)
				document = (T)obj;

			return result;

		}

		public void Clear()
		{
			_cacheDictionary.Clear();
		}

		public void Remove<T>(string documentId)
		{
			if (_cacheDictionary.ContainsKey(documentId))
				_cacheDictionary.Remove(documentId);
		}
	}
}

