using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace INetLib
{
    public static class MetadataDB
    {
	    private static List<BookEntity> metadataDB;
		
	    public static void initialize(string inpxFilePath)
	    {
		    metadataDB = InpxImport.import(inpxFilePath);
//		    sortByAuthorAscending();			
	    }
	    private static void sortByAuthorAscending()
	    {
		    metadataDB.Sort(
			    (BookEntity entity1, BookEntity entity2) =>
				    string.Compare(entity1.authors.getAuthors(), entity2.authors.getAuthors(), StringComparison.Ordinal));
	    }



	    public static List<BookEntity> selectBooksByAuthor(string authorNameToSearch)
		{
			return metadataDB.Where(book => book.authors.Any(author => (author.fullName).Contains(authorNameToSearch, StringComparison.OrdinalIgnoreCase))).ToList();
		}

	    public static List<BookEntity> selectBooksByTitle(string titleToSearch)
	    {
		    return metadataDB.Where(book => book.title.Contains(titleToSearch, StringComparison.CurrentCultureIgnoreCase)).ToList();
	    }

	    public static List<BookEntity> selectBooksByGenre(int genreIDToSearch)
	    {
		    return metadataDB.Where(book => book.genres.Any(genreID => genreID == genreIDToSearch)).ToList();
	    }


	    //Had to rewrite string.Contains method cause it does not work with case insensitive option
		[SuppressMessage("ReSharper", "InconsistentNaming")]
		private static bool Contains(this string source, string toCheck, StringComparison stringComparison)
		{
			if (string.IsNullOrEmpty(toCheck) || string.IsNullOrEmpty(source))
				return true;	//behaviour of those official string methods is the same
			return source.IndexOf(toCheck, stringComparison) >= 0;
		}

	}
}
