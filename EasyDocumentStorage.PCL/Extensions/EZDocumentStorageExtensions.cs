using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyDocumentStorage
{

	/// <summary>
	/// Easy document storage async extensions.
	/// </summary>
	public static class EZDocumentStorageExtensions
	{

		/// <summary>
		/// Async version of Insert single
		/// </summary>
		/// <returns>Task of type bool</returns>
		/// <param name="document">Document.</param>
		/// <typeparam name="T">The document type parameter.</typeparam>
		public static Task<bool> InsertAsync<T>(this IEZDocumentStorage eds, T document)
		{
			return Task.Run(() => eds.Insert(document));
		}

		/// <summary>
		/// Async version of Insert multiple
		/// </summary>
		/// <returns>Task of type bool</returns>
		/// <param name="documents">Documents.</param>
		/// <typeparam name="T">The document type parameter.</typeparam>
		public static Task<bool> InsertAsync<T>(this IEZDocumentStorage eds, IEnumerable<T> documents)
		{
			return Task.Run(() => eds.Insert(documents));
		}

		/// <summary>
		/// Async version of Insert or update single
		/// </summary>
		/// <returns>Task of type bool</returns>
		/// <param name="document">Documents.</param>
		/// <typeparam name="T">The document type parameter.</typeparam>
		public static Task<bool> InsertOrUpdateAsync<T>(this IEZDocumentStorage eds, T document)
		{
			return Task.Run(() => eds.InsertOrUpdate(document));
		}

		/// <summary>
		/// Async version of Insert or update multiple
		/// </summary>
		/// <returns>Task of type bool</returns>
		/// <param name="documents">Documents.</param>
		/// <typeparam name="T">The document type parameter.</typeparam>
		public static Task<bool> InsertOrUpdateAsync<T>(this IEZDocumentStorage eds, IEnumerable<T> documents)
		{
			return Task.Run(() => eds.InsertOrUpdate(documents));
		}

		/// <summary>
		/// Async version of Delete single
		/// </summary>
		/// <returns>Task of type bool</returns>
		/// <param name="document">Document.</param>
		/// <typeparam name="T">The document type parameter.</typeparam>
		public static Task<bool> DeleteAsync<T>(this IEZDocumentStorage eds, T document)
		{
			return Task.Run(() => eds.Delete(document));
		}

		/// <summary>
		/// Async version of Delete multiple
		/// </summary>
		/// <returns>Task of type bool</returns>
		/// <param name="documents">Documents.</param>
		/// <typeparam name="T">The document type parameter.</typeparam>
		public static Task<bool> DeleteAsync<T>(this IEZDocumentStorage eds, IEnumerable<T> documents)
		{
			return Task.Run(() => eds.Delete(documents));
		}

		/// <summary>
		/// Async version of Exists
		/// </summary>
		/// <returns>Task of type bool</returns>
		/// <param name="documentId">Document identifier.</param>
		/// <typeparam name="T">The document type parameter.</typeparam>
		public static Task<bool> ExistsAsync<T>(this IEZDocumentStorage eds, string documentId)
		{
			return Task.Run(() => eds.Exists<T>(documentId));
		}

		/// <summary>
		/// Async version of GetById
		/// </summary>
		/// <returns>Task of type T</returns>
		/// <param name="documentId">Document identifier.</param>
		/// <typeparam name="T">The document type parameter.</typeparam>
		public static Task<T> GetByIdAsync<T>(this IEZDocumentStorage eds, string documentId)
		{
			return Task.Run(() => eds.GetById<T>(documentId));
		}

		/// <summary>
		/// Async version of Get
		/// </summary>
		/// <returns>Task of type T</returns>
		/// <param name="clause">Clause.</param>
		/// <typeparam name="T">The document type parameter.</typeparam>
		public static Task<IEnumerable<T>> GetAsync<T>(this IEZDocumentStorage eds, Func<T, bool> clause = null)
		{
			return Task.Run(() => eds.Get<T>(clause));
		}
	}
}

