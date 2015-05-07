using System;
using System.Collections.Generic;

namespace MetadataDB
{
    public static class MetadataDB
    {
	    private static List<BookEntity.BookEntity> metadataDB;
		
	    public static void initialize(string inpxFilePath)
	    {
		    metadataDB = InpxImport.InpxImport.import(inpxFilePath);
//		    sortByAuthorAscending();			
	    }
	    private static void sortByAuthorAscending()
	    {
		    metadataDB.Sort(
			    (entity1, entity2) =>
				    string.Compare(entity1.authors.getAuthors(), entity2.authors.getAuthors(), StringComparison.Ordinal));
	    }


		public static List<BookEntity.BookEntity> selectBooksByAuthor(string authorNameToSearch)
		{
			return metadataDB.selectBooksByAuthor(authorNameToSearch);
		}
		public static List<BookEntity.BookEntity> selectBooksByTitle(string titleToSearch)
		{
			return metadataDB.selectBooksByTitle(titleToSearch);
		}
		public static List<BookEntity.BookEntity> selectBooksByGenre(int genreIDToSearch)
		{
			return metadataDB.selectBooksByGenre(genreIDToSearch);
		}
		public static BookEntity.BookEntity selectBookByID(int bookID)
		{
			return metadataDB.selectBookByID(bookID);
		}


    }
}
