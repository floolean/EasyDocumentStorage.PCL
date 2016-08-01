using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace EasyDocumentStorage
{
	/// <summary>
	/// Interface for a simple asynchronous repository for binary data
	/// </summary>
	public interface IBlobRepository
	{

		/// <summary>
		/// Gets all buckets.
		/// </summary>
		/// <returns>Task with collection of bucket Ids</returns>
		Task<IEnumerable<string>> GetBuckets();

		/// <summary>
		/// Get all blobs of a bucket
		/// </summary>
		/// <returns>Task with collection of blob Ids.</returns>
		/// <param name="bucketId">Bucket identifier.</param>
		Task<IEnumerable<string>> GetBlobs(string bucketId);

		/// <summary>
		/// Gets all blob infos
		/// </summary>
		/// <returns>Task with collection of BlobInfo.</returns>
		/// <param name="bucketId">Bucket identifier.</param>
		Task<IEnumerable<BlobInfo>> GetBlobInfos(string bucketId);

		/// <summary>
		/// Gets a specific blob info
		/// </summary>
		/// <returns>Task BlobInfo.</returns>
		/// <param name="bucketId">Bucket identifier.</param>
		/// <param name="blobId">Blob identifier.</param>
		Task<BlobInfo?> GetBlobInfo(string bucketId, string blobId);

		/// <summary>
		/// Stores the blob.
		/// </summary>
		/// <returns>Task bool.</returns>
		/// <param name="bucketId">Bucket identifier.</param>
		/// <param name="blobId">BLOB identifier.</param>
		/// <param name="stream">Stream.</param>
		/// <param name="overwrite">Overwrites an existing blob. If not set and the specified blob exists an exception is thrown</param>
		Task<bool> StoreBlob(string bucketId, string blobId, Stream stream, bool overwrite = false);

		/// <summary>
		/// Gets the blob stream.
		/// </summary>
		/// <returns>The blob stream.</returns>
		/// <param name="bucketId">Bucket identifier.</param>
		/// <param name="blobId">Blob identifier.</param>
		/// <param name="readwrite">If readwrite is set a shared stream will be returned. If not set and the stream is opened twice an exception is thrown.</param>
		Task<Stream> GetBlobStream(string bucketId, string blobId, bool readwrite = false);

		/// <summary>
		/// Moves the blob.
		/// </summary>
		/// <returns>Task.</returns>
		/// <param name="bucketId">Bucket identifier.</param>
		/// <param name="blobId">BLOB identifier.</param>
		/// <param name="newBucketId">New bucket identifier.</param>
		/// <param name="newBlobId">New blob identifier.</param>
		/// <param name="overwrite">Overwrite.</param>
		Task MoveBlob(string bucketId, string blobId, string newBucketId, string newBlobId, bool overwrite = false);

		/// <summary>
		/// Renames the blob.
		/// </summary>
		/// <returns>Task.</returns>
		/// <param name="bucketId">Bucket identifier.</param>
		/// <param name="blobId">Blob identifier.</param>
		/// <param name="newBlobId">New Blob identifier.</param>
		/// <param name="overwrite">Overwrite.</param>
		Task RenameBlob(string bucketId, string blobId, string newBlobId, bool overwrite = false);

		/// <summary>
		/// Deletes the blob.
		/// </summary>
		/// <returns>Task.</returns>
		/// <param name="bucketId">Bucket identifier.</param>
		/// <param name="blobId">BLOB identifier.</param>
		Task DeleteBlob(string bucketId, string blobId);

		/// <summary>
		/// Adds the listener.
		/// </summary>
		/// <returns>The listener.</returns>
		/// <param name="listener">Listener.</param>
		void AddListener(IBlobRepositoryListener listener);

		/// <summary>
		/// Removes the listener.
		/// </summary>
		/// <returns>The listener.</returns>
		/// <param name="listener">Listener.</param>
		void RemoveListener(IBlobRepositoryListener listener);

	}

	/// <summary>
	/// Blob repository listener.
	/// </summary>
	public interface IBlobRepositoryListener
	{

		/// <summary>
		/// Called when a blob was stored.
		/// </summary>
		/// <returns>The stored.</returns>
		/// <param name="bucketId">Bucket identifier.</param>
		/// <param name="blobId">Blob identifier.</param>
		void BlobStored(string bucketId, string blobId);

		/// <summary>
		/// Called when a blob was deleted.
		/// </summary>
		/// <returns>The deleted.</returns>
		/// <param name="bucketId">Bucket identifier.</param>
		/// <param name="blobId">Blob identifier.</param>
		void BlobDeleted(string bucketId, string blobId);

		/// <summary>
		/// Called when a blob was renamed.
		/// </summary>
		/// <returns>The renamed.</returns>
		/// <param name="bucketId">Bucket identifier.</param>
		/// <param name="blobId">Blob identifier.</param>
		/// <param name="newBlobId">New blob identifier.</param>
		void BlobRenamed(string bucketId, string blobId, string newBlobId);

		/// <summary>
		/// Called when a blob was moved
		/// </summary>
		/// <returns>The moved.</returns>
		/// <param name="bucketId">Bucket identifier.</param>
		/// <param name="blobId">Blob identifier.</param>
		/// <param name="newBucketId">New bucket identifier.</param>
		/// <param name="newBlobId">New blob identifier.</param>
		void BlobMoved(string bucketId, string blobId, string newBucketId, string newBlobId);

	}

	/// <summary>
	/// Blob info.
	/// </summary>
	public struct BlobInfo
	{

		/// <summary>
		/// The Id of the blob
		/// </summary>
		/// <value>The identifier.</value>
		public string Id { get; internal set; }

		/// <summary>
		/// Creation time.
		/// </summary>
		/// <value>The creation time.</value>
		public DateTime CreationTime { get; internal set; }

		/// <summary>
		/// Last access time.
		/// </summary>
		/// <value>The last access time.</value>
		public DateTime LastAccessTime { get; internal set; }

		/// <summary>
		/// Last write time.
		/// </summary>
		/// <value>The last write time.</value>
		public DateTime LastWriteTime { get; internal set; }

		/// <summary>
		/// Length of the data
		/// </summary>
		/// <value>The length.</value>
		public long Length { get; internal set; }

	}

}

