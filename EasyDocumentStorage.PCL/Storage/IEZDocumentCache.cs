using System;
namespace EasyDocumentStorage.Cache
{

	/// <summary>
	/// Cache interface
	/// </summary>
	public interface IEZDocumentCache
	{

		/// <summary>
		/// Gets or sets the max amount of objects to be cached.
		/// </summary>
		/// <value>The max objects.</value>
		int MaxObjects { get; set; }

		/// <summary>
		/// Tries to get an cached object.
		/// </summary>
		/// <returns>bool if the object was in the cache.</returns>
		/// <param name="documentId">Document identifier.</param>
		/// <param name="document">Document.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		bool TryGet<T>(string documentId, out T document);

		/// <summary>
		/// Caches the specified document.
		/// </summary>
		/// <param name="documentId">Document identifier.</param>
		/// <param name="document">Document.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		void Cache<T>(string documentId, T document);

		/// <summary>
		/// Removes the specified document with Id documentId from cache.
		/// </summary>
		/// <param name="documentId">Document identifier.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		void Remove<T>(string documentId);

		/// <summary>
		/// Clears the cache
		/// </summary>
		void Clear();

	}

}

