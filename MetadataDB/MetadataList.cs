using System;
using System.Collections.Generic;
using BookEntity;

namespace MetadataDB
{
    public static class MetadataList
    {
	    private static List<BookEntity.BookEntity> metadataList;
		
	    public static void initialize(string inpxFilePath)
	    {
		    metadataList = InpxImport.InpxImport.import(inpxFilePath);
//		    sortByAuthorAscending();			
	    }
	    private static void sortByAuthorAscending()
	    {
		    metadataList.Sort(
			    (entity1, entity2) =>
				    string.Compare(entity1.authors.getAuthors(), entity2.authors.getAuthors(), StringComparison.Ordinal));
	    }


		public static List<BookEntity.BookEntity> selectBooksByAuthor(string authorNameToSearch)
		{
			return metadataList.selectBooksByAuthor(authorNameToSearch);
		}
		public static List<BookEntity.BookEntity> selectBooksByTitle(string titleToSearch)
		{
			return metadataList.selectBooksByTitle(titleToSearch);
		}
		public static List<BookEntity.BookEntity> selectBooksByGenre(int genreIDToSearch)
		{
			return metadataList.selectBooksByGenre(genreIDToSearch);
		}
	    public static List<BookEntity.BookEntity> selectBooksByGenres(Genres genres)
	    {
		    return metadataList.selectBooksByGenres(genres);
	    }
		public static BookEntity.BookEntity selectBookByID(int bookID)
		{
			return metadataList.selectBookByID(bookID);
		}
	    public static List<BookEntity.BookEntity> selectBooksByTemplate(BookEntity.BookEntity template)
	    {
		    return metadataList.selectBooksByTemplate(template);
	    }
		
    }
}
