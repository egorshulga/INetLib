using System.IO;
using System.IO.Compression;

namespace BookExtractor
{
    public static class BookExtractor
    {
		//Retrieves book from the archive it belongs to the stream 
		//(so it can be transmitted to another computer or extracted to the disk)

	    private static string booksStoragePath;		//Folder path with books archives

	    public static void initialize(string booksStoragePathToInitialize)
	    {
		    booksStoragePath = booksStoragePathToInitialize;
	    }

	    public static Stream extract(BookEntity.BookEntity book)
	    {
		    string archivePath = getArchivePath(book.archiveName);

		    ZipArchive archive = ZipFile.OpenRead(archivePath);

		    var bookEntry = archive.GetEntry(book.fileName.appendExtension(book.extension));

		    return bookEntry.Open();
	    }


	    private static string getArchivePath(string archiveName)
	    {
		    return Path.Combine(booksStoragePath, archiveName);
	    }

		private static string appendExtension(this string fileName, string extension)
		{
			return Path.ChangeExtension(fileName, extension);
		}
	}
}
