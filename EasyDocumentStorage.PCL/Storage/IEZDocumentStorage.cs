using System;
using System.Collections.Generic;
using EasyDocumentStorage.Cache;
using EasyDocumentStorage.Crypto;
using EasyDocumentStorage.Serialization;

namespace EasyDocumentStorage
{

	/// <summary>
	/// Interface for EasyDocumentStorage
	/// </summary>
	public interface IEZDocumentStorage
	{

		/// <summary>
		/// Gets or sets the document cache.
		/// </summary>
		/// <returns>True if succesful</returns>
		/// <value>The cache.</value>
		IEZDocumentCache Cache { get; set; }

		/// <summary>
		/// Gets or sets the serializer.
		/// </summary>
		/// <returns>True if succesful</returns>
		/// <value>The serializer.</value>
		IEZDocumentSerializer Serializer { get; set; }

		/// <summary>
		/// Gets or sets the encryption service.
		/// </summary>
		/// <returns>True if succesful</returns>
		/// <value>The encryption service.</value>
		IEZDocumentEncryptionService EncryptionService { get; set; }

		/// <summary>
		/// Register a function that takes an instance of type T and return an id as string.
		/// This is needed because the system is designed to be type agnostic. No interface, base class or pattern is expected.
		/// You are responsible for obtaining a valid and unique id.
		/// 
		/// Optionally you can provide a custom implementation of <see cref="IEZDocumentSerializer"/> for the type to be registered.
		/// </summary>
		/// <returns>True if succesful</returns>
		/// <param name="documentTypeIdProvider">Document type identifier mapper.</param>
		/// <param name="serializer">Serializer.</param>
		/// <typeparam name="T">The document type parameter.</typeparam>
		bool Register<T>(Func<T, string> documentTypeIdProvider, IEZDocumentSerializer serializer = null);

		/// <summary>
		/// Insert the specified document.
		/// </summary>
		/// <returns>True if succesful</returns>
		/// <param name="document">Document.</param>
		/// <typeparam name="T">The document type parameter.</typeparam>
		bool Insert<T>(T document);

		/// <summary>
		/// Insert the specified documents.
		/// </summary>
		/// <returns>True if succesful</returns>
		/// <param name="documents">Documents.</param>
		/// <typeparam name="T">The document type parameter.</typeparam>
		bool InsertAll<T>(IEnumerable<T> documents);

		/// <summary>
		/// Inserts or updates the specified document.
		/// </summary>
		/// <returns>True if succesful</returns>
		/// <param name="document">Document.</param>
		/// <typeparam name="T">The document type parameter.</typeparam>
		bool InsertOrUpdate<T>(T document);

		/// <summary>
		/// Inserts or updates the specified documents.
		/// </summary>
		/// <returns>The or update.</returns>
		/// <param name="documents">Documents.</param>
		/// <typeparam name="T">The document type parameter.</typeparam>
		bool InsertOrUpdateAll<T>(IEnumerable<T> documents);

		/// <summary>
		/// Deletes the specified document.
		/// </summary>
		/// <returns>True if succesful</returns>
		/// <param name="document">Document.</param>
		/// <typeparam name="T">The document type parameter.</typeparam>
		bool Delete<T>(T document);

		/// <summary>
		/// Deletes the specified documents
		/// </summary>
		/// <returns>True if succesful</returns>
		/// <param name="documents">Documents.</param>
		/// <typeparam name="T">The document type parameter.</typeparam>
		bool DeleteAll<T>(IEnumerable<T> documents);

		/// <summary>
		/// Checks if the specified document exists
		/// </summary>
		/// <returns>True if the document exists</returns>
		/// <param name="documentId">Document identifier.</param>
		/// <typeparam name="T">The document type parameter.</typeparam>
		bool Exists<T>(string documentId);

		/// <summary>
		/// Gets a document of type T by identifier.
		/// </summary>
		/// <returns>The document.</returns>
		/// <param name="documentId">Document identifier.</param>
		/// <typeparam name="T">The document type parameter.</typeparam>
		T GetById<T>(string documentId);

		/// <summary>
		/// Gets all documents of type T with an optional filter expression or method
		/// </summary>
		/// <param name="clause">Filter expression or method.</param>
		/// <typeparam name="T">The document type parameter.</typeparam>
		IEnumerable<T> Get<T>(Func<T, bool> clause = null);

		/// <summary>
		/// Gets the document identifier.
		/// </summary>
		/// <returns>The document identifier.</returns>
		/// <param name="document">Document.</param>
		/// <typeparam name="T">The document type parameter.</typeparam>
		string GetDocumentId<T>(T document);

	}

}

