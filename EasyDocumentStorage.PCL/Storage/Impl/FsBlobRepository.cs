using System;
using PCLStorage;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace EasyDocumentStorage
{
	public class FsBlobRepository : IBlobRepository
	{

		string _baseFolderName;
		IFolder _baseFolder;
		readonly List<IBlobRepositoryListener> _listeners = new List<IBlobRepositoryListener>();

		public string BaseFolderName 
		{
			get { return _baseFolderName; }
			set {
				_baseFolder = null;
				_baseFolderName = value;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
				EnsureBaseDirectoryExists();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			}
		}

		public FsBlobRepository()
		{
			BaseFolderName = "buckets";
		}
		 
		public FsBlobRepository (string baseFolderName)
		{
			BaseFolderName = baseFolderName;
		}

		public async Task<IEnumerable<string>> GetBuckets( )
		{

			await EnsureBaseDirectoryExists ();

			var folders = await _baseFolder.GetFoldersAsync ();

			return folders.Select ( f => f.Name ).ToArray();

		}
			
		public async Task<IEnumerable<string>> GetBlobs( string bucketId )
		{

			await EnsureBaseDirectoryExists ();

			var bucketFolder = await _baseFolder.CreateFolderAsync (bucketId, CreationCollisionOption.OpenIfExists);

			if (bucketFolder != null) {

				var files = await bucketFolder.GetFilesAsync ();

				return files.Select (f => f.Name).ToArray();

			}

			return null;

		}

		public async Task<BlobInfo?> GetBlobInfo(string bucketId, string blobId)
		{

			await EnsureBaseDirectoryExists();

			var bucketFolder = await _baseFolder.GetFolderAsync(bucketId);

			if (bucketFolder != null)
			{

				var files = await bucketFolder.GetFilesAsync();

				var file = files.FirstOrDefault(f => f.Name == blobId);

				if (file != null)
				{

					return new BlobInfo()
					{
						Id = file.Name,
						CreationTime = default(DateTime),
						LastWriteTime = default(DateTime),
						LastAccessTime = default(DateTime),
						Length = 0
					};

				}

			}

			return null;

		}

		public async Task<IEnumerable<BlobInfo>> GetBlobInfos( string bucketId )
		{

			await EnsureBaseDirectoryExists ();

			var bucketFolder = await _baseFolder.GetFolderAsync (bucketId);

			if (bucketFolder != null) {

				var files = await bucketFolder.GetFilesAsync ();

				return files.Select(f => new BlobInfo()
				{
					Id = f.Name,
					CreationTime = default(DateTime),
					LastWriteTime = default(DateTime),
					LastAccessTime = default(DateTime),
					Length = 0
				});

			}

			return new List<BlobInfo>();

		}

		public async Task<bool> StoreBlob(string bucketId, string blobId, Stream stream, bool overwrite = false)
		{

			await EnsureBaseDirectoryExists();

			var bucket = await _baseFolder.CreateFolderAsync(bucketId, CreationCollisionOption.OpenIfExists);

			var file = await bucket.CreateFileAsync(blobId, overwrite ? CreationCollisionOption.ReplaceExisting : CreationCollisionOption.FailIfExists );

			using (var ostream = await file.OpenAsync(FileAccess.ReadAndWrite))
				stream.CopyTo(ostream);

			IterateListeners(l => l.BlobStored(bucketId, blobId));

			return true;

		}

		public async Task<Stream> GetBlobStream( string bucketId, string blobId, bool readwrite = false )
		{

			await EnsureBaseDirectoryExists ();

			var bucketFolder = await _baseFolder.GetFolderAsync (bucketId);

			if (bucketFolder != null) {

				var files = await bucketFolder.GetFilesAsync ();

				var file = files.FirstOrDefault (f => f.Name == blobId);

				if (file != null) 
				{
					
					var fileStream = await file.OpenAsync ( readwrite ? FileAccess.ReadAndWrite : FileAccess.Read);

					return fileStream;

				}

			}

			return null;

		}

		public async Task MoveBlob( string bucketId, string blobId, string newBucketId, string newBlobId, bool overwrite = false )
		{

			await EnsureBaseDirectoryExists ();

			var bucketFolder = await _baseFolder.GetFolderAsync (bucketId);

			if (bucketFolder != null) {

				var result = await bucketFolder.CheckExistsAsync (blobId);

				if (result == ExistenceCheckResult.FileExists) 
				{

					using (var blobStream = await GetBlobStream (bucketId, blobId)) {

						StoreBlob (newBucketId, newBlobId, blobStream, overwrite).Wait();

					}

					await DeleteBlob (bucketId, blobId);

					IterateListeners(l => l.BlobMoved(bucketId, blobId, newBucketId, newBlobId));

				}
			}

		}

		public async Task RenameBlob( string bucketId, string blobId, string newBlobId, bool overwrite = false )
		{

			await EnsureBaseDirectoryExists ();

			var bucketFolder = await _baseFolder.GetFolderAsync (bucketId);

			if (bucketFolder != null) {

				var result = await bucketFolder.CheckExistsAsync (blobId);

				if (result == ExistenceCheckResult.FileExists){
					
					var fileToRename = await bucketFolder.GetFileAsync (blobId);
						
					await fileToRename.RenameAsync (newBlobId, overwrite ? NameCollisionOption.ReplaceExisting : NameCollisionOption.FailIfExists);

					IterateListeners(l => l.BlobRenamed(bucketId, blobId, newBlobId));

				}

			}

		}

		public async Task DeleteBlob( string bucketId, string blobId )
		{

			await EnsureBaseDirectoryExists ();

			var bucketFolder = await _baseFolder.GetFolderAsync (bucketId);

			if (bucketFolder != null) {

				var result = await bucketFolder.CheckExistsAsync (blobId);

				if (result == ExistenceCheckResult.FileExists)
				{

					var file = await bucketFolder.GetFileAsync(blobId);

					await file.DeleteAsync();

					IterateListeners(l => l.BlobDeleted(bucketId, blobId));

				}

			}

		}

		public void AddListener(IBlobRepositoryListener listener)
		{
			_listeners.Add(listener);
		}

		public void RemoveListener(IBlobRepositoryListener listener)
		{
			_listeners.Remove(listener);
		}

		private async Task EnsureBaseDirectoryExists()
		{

			if (_baseFolder != null && _baseFolder.Name == BaseFolderName)
				return;

			await EnsureDirectoryExists (BaseFolderName);

			_baseFolder = await FileSystem.Current.LocalStorage.GetFolderAsync (_baseFolderName);

		}

		private async Task EnsureDirectoryExists(string directoryName)
		{
			var root = FileSystem.Current.LocalStorage;

			if (await root.CheckExistsAsync (directoryName) == ExistenceCheckResult.NotFound)
				await root.CreateFolderAsync (directoryName, CreationCollisionOption.OpenIfExists);

		}

		private void IterateListeners(Action<IBlobRepositoryListener> callback)
		{
			foreach (var listener in _listeners)
			{
				callback(listener);
			}
		}

	}

}

