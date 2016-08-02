# EasyDocumentStorage.PCL

.NET cross-platform easy to use document storage engine. Documents are kept on disc as files.
The default implementation uses Json.Net as default serializer, a binary serializer is also provided via SharpSerializer.PCL.
It is supposed to be used with small to medium datasets, as it is not a high performance solution.

### Install with nuget

```
PM> Install-Package EasyDocumentStorage.PCL
```

### Platforms

 * Net45
 * WP8.1
 * Windows 8
 * WP8 Silverlight
 * Xamarin Android
 * Xamarin iOS
 * Xamarin iOS Classic
 * Xamarin.Mac.Unified


## Let' see some code!

### Type registration

Create a simple document class first:

```csharp
class MyDocument {
	public int Id { get; set; }
    public string Name { get; set; }
    public string Author { get; set; }
}
```

Somewhere in your code you need to register the previously created MyDocument class, this is the only configuration
needed as EasyDocumentStorage treats ids internally as string. This way you can use whatever you want as id, as 
long as you provide a converter that returns a string.

```csharp
void main(){
	EZDocumentStorage.Default.Register<MyDocument>(document => document.Id.ToString());
}
```

### Usage

```csharp
bool InsertNewDocument( int id, string name, string author ){
	return EZDocumentStorage.Default.Insert( new MyDocument() {
    	Id = id,
        Name = name,
        Author = author
    });
}

bool UpdateDocument( MyDocument document ){
	return EZDocumentStorage.Default.InsertOrUpdate(document);
}

IEnumerable<MyDocument> GetAllDocuments(){
	return EZDocumentStorage.Default.Get<MyDocument>();
}

IEnumerable<MyDocument> GetAllDocumentsFromAuthor( string author ){
	return EZDocumentStorage.Default.Get<MyDocument>( (doc) => doc.Author == author );
}

bool DeleteAllDocuments(){
    var docs = GetAllDocuments();
    return EZDocumentStorage.Default.Delete(docs);
}
```

### Async

For all methods there is an async extension:

```csharp
public static Task<bool> InsertAsync<T>(this IEZDocumentStorage eds, T document)

public static Task<bool> InsertAsync<T>(this IEZDocumentStorage eds, IEnumerable<T> documents)

public static Task<bool> InsertOrUpdateAsync<T>(this IEZDocumentStorage eds, T document)

public static Task<bool> InsertOrUpdateAsync<T>(this IEZDocumentStorage eds, IEnumerable<T> documents)

public static Task<bool> DeleteAsync<T>(this IEZDocumentStorage eds, T document)

public static Task<bool> DeleteAsync<T>(this IEZDocumentStorage eds, IEnumerable<T> documents)

public static Task<bool> ExistsAsync<T>(this IEZDocumentStorage eds, string documentId)

public static Task<T> GetByIdAsync<T>(this IEZDocumentStorage eds, string documentId)

public static Task<IEnumerable<T>> GetAsync<T>(this IEZDocumentStorage eds, Func<T, bool> clause = null)
```

### Binary Serialization

```csharp
void main(){
	EZDocumentStorage.Default.Serializer = new BinaryDocumentSerializer();
}
```

### Cache
Optionally you can use the provided cache to speed up the system a little bit:

```csharp
EZDocumentStorage.Default.Cache = new EZDocumentCache();
```

### Cryptography
There is also a cryptography layer if you need it, just add:

```csharp
EZDocumentStorage.Default.EncryptionService = new EZDocumentEncryptionService("mykey", "mysalt");
```

Documents will automatically get encrypted with AES 128bit.