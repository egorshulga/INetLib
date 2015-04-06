using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace INPImport
{
	static class Program
	{
		[SuppressMessage("ReSharper", "InconsistentNaming")]
		static void Main()
		{
			GenresList.initialize("D:\\for-study\\coursework\\genres_fb2.glst");

			StreamReader reader = new StreamReader("D:\\books\\_Lib.rus.ec - Официальная\\librusec_local_fb2 - Копия\\fb2-000024-030559.inp");
			string line;
			for (int i = 1; i < 35; i++)
				line = reader.ReadLine();
			line = reader.ReadLine();

			BookEntity book = new BookEntity(line);
			book.printInfoDebug();
		}
	}
}
