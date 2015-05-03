using System;
using System.IO;
using System.Text;
using INetLib;

namespace testExtractor
{
	class Program
	{
		static void Main(string[] args)
		{
			GenresList.initialize(@"D:\books\_Lib.rus.ec - Официальная\genres_fb2.glst");
			MetadataDB.initialize(@"D:\books\_Lib.rus.ec - Официальная\librusec_local_fb2.inpx");
			BookExtractor.initialize(@"D:\books\_Lib.rus.ec - Официальная\lib.rus.ec");

			var books = MetadataDB.selectBooksByTitle("potter");

			StreamReader bookStream = new StreamReader(BookExtractor.extract(books[0]));

//			while (!bookStream.EndOfStream)
//			{
			Console.OutputEncoding = Encoding.ASCII;
			Console.Write("{0}", bookStream.ReadToEnd());
//			}

		}
	}
}
