﻿using System;
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
			Console.Write("Author query:	{0}. ", authorNameToSearch);
			var booksList = MetadataDB.MetadataDB.selectBooksByAuthor(authorNameToSearch);
			Console.WriteLine("Found {0} entities.", booksList.Count);
			return booksList;
		}

		public List<BookEntity.BookEntity> selectBooksByTitle(string titleToSearch)
		{
			Console.Write("Title query:	{0}. ", titleToSearch);
			var books = MetadataDB.MetadataDB.selectBooksByTitle(titleToSearch);
			Console.WriteLine("Found {0} entities.", books.Count);
			return books;
		}

		public List<BookEntity.BookEntity> selectBooksByGenre(int genreIDToSearch)
		{
			Console.Write("Genre query:	{0} - {1}. ", GenresList.GenresList.getGenreName(genreIDToSearch), GenresList.GenresList.getGenreDescription(genreIDToSearch));
			var books = MetadataDB.MetadataDB.selectBooksByGenre(genreIDToSearch);
			Console.WriteLine("Found {0} entities.", books.Count);
			return books;
		}

		public List<BookEntity.BookEntity> selectBooksByGenres(Genres genres)
		{
			Console.Write("Genres query:	{0}. ", genres.getGenres());
			var books = MetadataDB.MetadataDB.selectBooksByGenres(genres);
			Console.WriteLine("Found {0} entities.", books.Count);
			return books;
		}

		public BookEntity.BookEntity selectBookByID(int bookID)
		{
			Console.Write("Book ID query:	");
			var book = MetadataDB.MetadataDB.selectBookByID(bookID);
			book.printInfoDebug();
			return book;
		}

		public List<BookEntity.BookEntity> selectBooksByTemplate(BookEntity.BookEntity template)
		{
//			Console.Write("Template query:	{0} - {1} - {2}. ", template.authors[0].fullName, template.title, template.genres);
			Console.Write("Template query:	");
			Console.Write(string.IsNullOrEmpty(template.authors[0].fullName) ? "[all]" : template.authors[0].fullName);
			Console.Write(" - ");
			Console.Write(string.IsNullOrEmpty(template.title) ? "[all]" : template.title);
			Console.Write(" - ");
			Console.Write(template.genres.genresIDs.Count == 0 ? "[all]" : template.genres.ToString());
			Console.Write(". ");

			var books = MetadataDB.MetadataDB.selectBooksByTemplate(template);
			Console.WriteLine("Found {0} entities.", books.Count);
			return books;
		}

		public Stream extractBook(BookEntity.BookEntity book)
		{
			Console.Write("Book query:	");
			book.printInfoDebug();
			return BookExtractor.BookExtractor.extract(book);
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
