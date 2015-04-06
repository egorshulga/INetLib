﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;

namespace INPImport
{
	[SuppressMessage("ReSharper", "ParameterHidesMember")]
	class BookEntity
	{
		//Для совместимости сюда включены все доступные поля.
		public Authors authors { get; set; }			//Список авторов
		public Genres genres { get; set; }				//Список жанров
		public string title { get; set; }				//Название
		private string seriesTitle { get; set; }		//Название серии
		private int numberInSeries {get;set;}			//Номер книги в серии
		public string fileName { get; set; }			//Имя файла с книгой; для _Lib.rus.ec имена файлов книг и их ID совпадают
		private int fileSize { get; set; }				//Размер файла с книгой; может не совпадать с реальным размером файла
		public int bookID { get; set; }					//Уникальный ID книги
		private bool isDeleted { get; set; }			//Признак того, что файл удален (???)
		public string extension { get; set; }			//Расширение файла книги
		public string dateAdded { get; set; }			//Дата добавления файла книги
		public string language { get; set; }			//Язык книги
		private string bookRate { get; set; }			//Внешний рейтинг книги (???)
		public string keywords { get; set; }			//Теги



		public BookEntity(string bookRawInfo)
		{
			parseBookRawInfo(bookRawInfo);
		}


		private void parseBookRawInfo(string bookRawInfo)
		{
			try
			{
				splitAndParseBookInfo(bookRawInfo);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
		}

		private void splitAndParseBookInfo(string bookRawInfo)
		{
			const char bookInfoDelimiter = (char) 0x04;
			string[] splittedBookInfo = bookRawInfo.Split(bookInfoDelimiter);
			setBookInfo(splittedBookInfo);
		}


		private enum BookInfo
		{
			Authors,
			Genres,
			Title,
			SeriesTitle,
			NumberInSeries,
			FileName,
			FileSize,
			BookID,
			IsDeleted,
			Extension,
			DateAdded,
			Language,
			BookRate,
			Keywords
		}

		private void setBookInfo(IReadOnlyList<string> bookInfo)
		{
			setAuthors(bookInfo[(int)BookInfo.Authors]);
			setGenres(bookInfo[(int)BookInfo.Genres]);
			setTitle(bookInfo[(int)BookInfo.Title]);
			setSeriesTitle(bookInfo[(int)BookInfo.SeriesTitle]);
			setNumberInSeries(bookInfo[(int)BookInfo.NumberInSeries]);
			setFileName(bookInfo[(int)BookInfo.FileName]);
			setFileSize(bookInfo[(int)BookInfo.FileSize]);
			setBookID(bookInfo[(int)BookInfo.BookID]);
			setDeletionFlag(bookInfo[(int)BookInfo.IsDeleted]);
			setExtension(bookInfo[(int)BookInfo.Extension]);
			setDateAdded(bookInfo[(int)BookInfo.DateAdded]);
			setLanguage(bookInfo[(int)BookInfo.Language]);
			setBookRate(bookInfo[(int)BookInfo.BookRate]);
			setKeywords(bookInfo[(int)BookInfo.Keywords]);
		}

		private void setAuthors(string authorsString)
		{
			authors = new Authors(authorsString);
		}
		private void setGenres(string genresString)
		{
			genres = new Genres(genresString);
		}
		private void setTitle(string title)
		{
			this.title = title;
		}
		private void setSeriesTitle(string seriesTitle)
		{
			this.seriesTitle = seriesTitle;
		}
		private void setNumberInSeries(string numberInSeriesString)
		{
			numberInSeries = Convert.ToInt32(numberInSeriesString);
		}
		private void setFileName(string fileName)
		{
			this.fileName = fileName;
		}
		private void setFileSize(string fileSizeString)
		{
			fileSize = Convert.ToInt32(fileSizeString);
		}
		private void setBookID(string bookIDString)
		{
			bookID = Convert.ToInt32(bookIDString);
		}
		private void setDeletionFlag(string deletedFlagString)
		{
			isDeleted = (deletedFlagString == "1");
		}

		private void setExtension(string extension)
		{
			this.extension = extension;
		}
		private void setDateAdded(string dateAdded)
		{
			this.dateAdded = dateAdded;
		}
		private void setLanguage(string language)
		{
			this.language = language;
		}
		private void setBookRate(string bookRate)
		{
			this.bookRate = bookRate;
		}
		private void setKeywords(string keywords)
		{
			this.keywords = keywords;
		}


		public void printInfoDebug()
		{
			Console.WriteLine("{0}	{1}", authors.getAuthors(), title);
		}
		
	}
}