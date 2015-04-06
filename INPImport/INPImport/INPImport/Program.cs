using System.Diagnostics.CodeAnalysis;

namespace INPImport
{
	static class Program
	{
		[SuppressMessage("ReSharper", "InconsistentNaming")]
		static void Main(string[] args)
		{
			GenresList.initialize("D:\\for-study\\coursework\\genres_fb2.glst");

			Genres genres = new Genres("sf_fantasy:love_sf:popadanec:");
			genres.printGenresDebug();
		}
	}
}
