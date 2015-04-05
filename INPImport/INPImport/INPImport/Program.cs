namespace INPImport
{
	static class Program
	{
		static void Main(string[] args)
		{
			GenresList.initialize("D:\\for-study\\coursework\\genres_fb2.glst");

			Genres genres = new Genres("adv_animal:nonf_biography:religion_esoterics:");
			genres.printGenresDebug();
		}
	}
}
