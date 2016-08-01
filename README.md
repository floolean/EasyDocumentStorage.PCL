# EasyDocumentStorage.PCL

.NET cross-platform easy to use document storage engine. Documents are kept on disc as files.
The default implementation uses Json.Net as default serializer, a binary serializer is also provided via SharpSerializer.PCL.
It is supposed to be used with small to medium datasets, as it is not a high performance solution.

### Install with nuget

```
PM> Install-Package EasyDocumentStorage.PCL
```

### Platforms

 * Xamarin Android
 * Xamarin iOS
 * Xamarin iOS Classic
 * Xamarin.Mac.Unified
 * Net45
 * WP8.1
 * Win8
 * WP8 Silverlight




## Let' see some code!

Create a simple document class first:

```csharp
class MyDocument {
	public int Id { get; set; }
    public string Name { get; set; }
    public string Author { get; set; }
}
```

Somewhere in your code register the previously created MyDocument class. This is the only thing 
needed as the EDS treats ids internally as string. This way you can use whatever you want as id, as 
long as you provide a converter that returns a string.

```csharp
void main(){
	EasyDocumentStorage.Default.Register<MyDocument>(document => document.Id.ToString());
}
```

Then you can use it like this:

```csharp
bool InsertNewDocument( int id, string name, string author ){
	return EasyDocumentStorage.Default.Insert( new MyDocument() {
    	Id = id,
        Name = name,
        Author = author
    });
}

void GetAllDocuments(){
	return EasyDocumentStorage.Default.Get<MyDocument>();
}
```


### Cache
Optionally you can use the provided cache to speed up the system a little bit:

```csharp
EasyDocumentStorage.Default.Cache = new EasyDocumentCache();
```

### Cryptography
There is also a cryptography layer if you need it, just add:

```csharp
EasyDocumentStorage.Default.EncryptionService = new EasyDocumentEncryptionService("mykey", "mysalt");
```

Documents will automatically get encrypted with AES 128bit.