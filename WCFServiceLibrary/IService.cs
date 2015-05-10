using System.Collections.Generic;
using System.IO;
using System.ServiceModel;

namespace WCFServiceLibrary
{
	[ServiceContract]
	public interface IService
	{
		[OperationContract]
		List<BookEntity.BookEntity> selectBooksByAuthor(string authorNameToSearch);

		[OperationContract]
		List<BookEntity.BookEntity> selectBooksByTitle(string titleToSearch);

		[OperationContract]
		List<BookEntity.BookEntity> selectBooksByGenre(int genreIDToSearch);

		[OperationContract]
		BookEntity.BookEntity selectBookByID(int bookID);

		[OperationContract]
		Stream extractBook(BookEntity.BookEntity book);
	}



}
