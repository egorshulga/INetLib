using System;
using System.Collections.Generic;
using System.IO;

namespace WCFServiceLibrary
{
	public class Service : IService
	{
		public List<BookEntity.BookEntity> selectBooksByAuthor(string authorNameToSearch)
		{
			Console.WriteLine("Author query:	{0}", authorNameToSearch);
			return MetadataDB.MetadataDB.selectBooksByAuthor(authorNameToSearch);
		}

		public List<BookEntity.BookEntity> selectBooksByTitle(string titleToSearch)
		{
			return MetadataDB.MetadataDB.selectBooksByTitle(titleToSearch);
		}

		public List<BookEntity.BookEntity> selectBooksByGenre(int genreIDToSearch)
		{
			return MetadataDB.MetadataDB.selectBooksByGenre(genreIDToSearch);
		}

		public BookEntity.BookEntity selectBookByID(int bookID)
		{
			return MetadataDB.MetadataDB.selectBookByID(bookID);
		}

		public Stream extractBook(BookEntity.BookEntity book)
		{
			return BookExtractor.BookExtractor.extract(book);
		}
	}
}
