using System;
using System.Collections.Generic;

namespace testImportInpxAndGenresList
{
	class Program
	{
		static void Main(string[] args)
		{
			GenresList.GenresList.initialize(@"D:\books\_Lib.rus.ec - Официальная\genres_fb2.glst");

			List<BookEntity.BookEntity> db = InpxImport.InpxImport.import(@"D:\books\_Lib.rus.ec - Официальная\librusec_local_fb2.inpx");

			foreach (var book in db)
				book.printInfoDebug();
		}
	}
}
