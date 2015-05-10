using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace BookEntity
{
	[SuppressMessage("ReSharper", "ParameterHidesMember")]
	[DataContract]
	public class BookEntity
	{
		//Для совместимости сюда включены все доступные поля.
		[DataMember]
		public Authors authors { get; set; }			//Список авторов
		[DataMember]
		public Genres genres { get; set; }				//Список жанров
		[DataMember]
		public string title { get; set; }				//Название
		[DataMember]
		public string seriesTitle { get; set; }			//Название серии
		[DataMember]
		public int numberInSeries { get; set; }			//Номер книги в серии
		[DataMember]
		public string fileName { get; set; }			//Имя файла книги; для _Lib.rus.ec имена файлов книг и их ID совпадают
		[DataMember]
		public int fileSize { get; set; }				//Размер файла с книгой; может не совпадать с реальным размером файла
		[DataMember]
		public int bookID { get; set; }					//Уникальный ID книги
		[DataMember]
		public bool isDeleted { get; set; }				//Признак того, что файл удален (???)
		[DataMember]
		public string extension { get; set; }			//Расширение файла книги
		[DataMember]
		public string dateAdded { get; set; }			//Дата добавления файла книги
		[DataMember]
		public string language { get; set; }			//Язык книги
		[DataMember]
		public string bookRate { get; set; }			//Внешний рейтинг книги (???)
		[DataMember]
		public string keywords { get; set; }			//Теги

		[DataMember]
		public string archiveName { get; set; }			//Имя архива с файлом книги 
														//Поле инициализируется в момент чтения метаданных
														//(Имя архива совпадает с именем файла с метаданными в inpx файле)


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
			catch
			{ }		
		}

		private void splitAndParseBookInfo(string bookRawInfo)
		{
			const char bookInfoDelimiter = (char)0x04;
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
			if (numberInSeriesString.Length == 0)
				numberInSeries = 0;
			else
				numberInSeries = Convert.ToInt32(numberInSeriesString);
		}
		private void setFileName(string fileName)
		{
			this.fileName = fileName;
		}
		private void setFileSize(string fileSizeString)
		{
			if (fileSizeString.Length == 0)
				fileSize = 0;
			else
				fileSize = Convert.ToInt32(fileSizeString);
		}
		private void setBookID(string bookIDString)
		{
			if (bookIDString.Length == 0)
				bookID = 0;
			else
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
			Console.WriteLine("{0}	{1}	{2}", authors.getAuthors(), title, bookID);
//			Console.WriteLine("{0}({1}) in {2}", fileName, bookID, archiveName);
		}

	}
}
