using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using BookEntity;
using GenresList;

namespace WCFServiceLibrary
{
	public class Service : IService
	{
		public List<BookEntity.BookEntity> selectBooksByAuthor(string authorNameToSearch)
		{
			var booksList = MetadataDB.MetadataList.selectBooksByAuthor(authorNameToSearch);
			Console.WriteLine("Author query:	{0}. Found {1} entities.", authorNameToSearch, booksList.Count);
			return booksList;
		}

		public List<BookEntity.BookEntity> selectBooksByTitle(string titleToSearch)
		{
			var books = MetadataDB.MetadataList.selectBooksByTitle(titleToSearch);
			Console.WriteLine("Title query:	{0}. Found {1} entities.", titleToSearch, books.Count);
			return books;
		}

		public List<BookEntity.BookEntity> selectBooksByGenre(int genreIDToSearch)
		{
			var books = MetadataDB.MetadataList.selectBooksByGenre(genreIDToSearch);
			Console.WriteLine("Genre query:	{0} - {1}. Found {2} entities.", GenresList.GenresList.getGenreName(genreIDToSearch), GenresList.GenresList.getGenreDescription(genreIDToSearch), books.Count);
			return books;
		}

		public List<BookEntity.BookEntity> selectBooksByGenres(Genres genres)
		{
			var books = MetadataDB.MetadataList.selectBooksByGenres(genres);
			Console.WriteLine("Genres query:	{0}. Found {1} entities.", genres.getGenres(), books.Count);
			return books;
		}

		public BookEntity.BookEntity selectBookByID(int bookID)
		{
			return MetadataDB.MetadataList.selectBookByID(bookID);
		}

		public List<BookEntity.BookEntity> selectBooksByTemplate(BookEntity.BookEntity template)
		{
			var books = MetadataDB.MetadataList.selectBooksByTemplate(template);
			Console.WriteLine("Template query:	{0} - {1} - {2}. Found {3} entities. ",
				string.IsNullOrEmpty(template.authors[0].fullName) ? "[all]" : template.authors[0].fullName,
				string.IsNullOrEmpty(template.title) ? "[all]" : template.title,
				template.genres.genresIDs.Count == 0 ? "[all]" : template.genres.ToString(), 
				books.Count);
			return books;
		}

		public Stream extractBook(BookEntity.BookEntity book)
		{
			Console.Write("Book query:	{0}", book);
			return BookExtractor.BookExtractor.extract(book);
		}

		public Stream extractBookByID(int bookID)
		{
			var book = selectBookByID(bookID);
			return extractBook(book);
		}

		public List<GenresListEntity> getAvailableGenres()
		{
			Console.WriteLine("Request for genres from {0}", getConnectedClientAddress());
			return GenresList.GenresList.getAvailableGenres();
		}

		private string getConnectedClientAddress()
		{
			MessageProperties prop = OperationContext.Current.IncomingMessageProperties;
			RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
			return "[" + endpoint.Address + "]:" + endpoint.Port;
		}
	}
}
