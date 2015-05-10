using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;

namespace WCFServiceLibrary
{
	public class Service : IService
	{
		public List<BookEntity.BookEntity> selectBooksByAuthor(string authorNameToSearch)
		{
			Console.WriteLine("Author query received:	{0}", authorNameToSearch);
			return MetadataDB.MetadataDB.selectBooksByAuthor(authorNameToSearch);
		}

		public List<BookEntity.BookEntity> selectBooksByTitle(string titleToSearch)
		{
			Console.WriteLine("Title query received:	{0}", titleToSearch);
			return MetadataDB.MetadataDB.selectBooksByTitle(titleToSearch);
		}

		public List<BookEntity.BookEntity> selectBooksByGenre(int genreIDToSearch)
		{
			Console.WriteLine("Genre query received:	{0}	{1}", GenresList.GenresList.getGenreName(genreIDToSearch), GenresList.GenresList.getGenreDescription(genreIDToSearch));
			return MetadataDB.MetadataDB.selectBooksByGenre(genreIDToSearch);
		}

		public BookEntity.BookEntity selectBookByID(int bookID)
		{
			Console.Write("Book ID query received:	");
			var book = MetadataDB.MetadataDB.selectBookByID(bookID);
			book.printInfoDebug();
			return book;
		}

		public Stream extractBook(BookEntity.BookEntity book)
		{
			Console.Write("Book query received:	");
			book.printInfoDebug();
			return BookExtractor.BookExtractor.extract(book);
		}
	}
}
