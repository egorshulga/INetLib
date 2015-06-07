using System;
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

		    ZipArchive archive;
		    try
		    {
				archive = ZipFile.OpenRead(archivePath);
		    }
		    catch (Exception)
		    {
//				Console.WriteLine("Failed to extract book file.");
				Console.WriteLine("\nERROR: something went wrong while trying to extract a book {0}", book);
				Console.WriteLine("Please, stop the server resetting settings and restart.");
			    return Stream.Null;
		    }

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
