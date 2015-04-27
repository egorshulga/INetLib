using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace InpxImport
{
	public static class InpxImport
	{
		const string inpFileExtension = ".inp";
		public static List<BookEntity.BookEntity> import(string inpxFilePath)
		{
			return getBooksListFromInpxFile(inpxFilePath);
		}

		private static List<BookEntity.BookEntity> getBooksListFromInpxFile(string inpxFilePath)
		{
			List<BookEntity.BookEntity> booksList = new List<BookEntity.BookEntity>();

			ZipArchive archive = ZipFile.OpenRead(inpxFilePath);
			foreach (var archiveEntry in archive.Entries.Where(isInpFile))
			{
				booksList.AddRange(getBookEntitiesFromInpArchiveEntry(archiveEntry));
			}
			return booksList;
		}

		private static List<BookEntity.BookEntity> getBookEntitiesFromInpArchiveEntry(ZipArchiveEntry archiveEntry)
		{
			StreamReader reader = new StreamReader(archiveEntry.Open());
			List<BookEntity.BookEntity> bookEntitiesFromFile = new List<BookEntity.BookEntity>();
			while (!reader.EndOfStream)
			{
				var bookEntity = createBookEntity(reader.ReadLine());
				bookEntitiesFromFile.Add(bookEntity);
			}
			return bookEntitiesFromFile;
		}

		private static BookEntity.BookEntity createBookEntity(string bookData)
		{
			return new BookEntity.BookEntity(bookData);
		}

		private static bool isInpFile(ZipArchiveEntry inpFile)
		{
			return Path.GetExtension(inpFile.Name) == inpFileExtension;
		}
	}
}
