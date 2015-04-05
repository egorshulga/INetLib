using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace INPImport
{
	static class GenresList
	{
		private static readonly List<GenresListEntity> genres = new List<GenresListEntity>();

		public static void initialize(string genresFilePath)
		{
			tryReadGenresFromFile(genresFilePath);
		}

		private static void tryReadGenresFromFile(string genresFilePath)
		{
			try
			{
				readGenresFromFile(genresFilePath);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
		}

		private static void readGenresFromFile(string genresFilePath)
		{
			StreamReader genresFile = new StreamReader(genresFilePath);
			readGenresByLine(genresFile);
			genresFile.Close();
		}

		private static void readGenresByLine(TextReader genresFile)
		{
			string line = genresFile.ReadLine();
			while (line != null)
			{
				addGenresIgnoreComments(line);
				line = genresFile.ReadLine();
			}
		}

		private const int firstCharacterPosition = 0;
		private static void addGenresIgnoreComments(string line)
		{
			if (line[firstCharacterPosition] == '#') return;
			if (line.Length == 0) return;
			GenresListEntity genre = new GenresListEntity(line);
			genres.Add(genre);
		}



		static public string getGenreName(int id)
		{
			return genres[id].name;
		}

		static public string getGenreDescription(int id)
		{
			return genres[id].description;
		}

		static public int getGenreID(string genreString)
		{
			return genres.FindIndex(entity => entity.name == genreString);
		}


		public static void printGenresListDebug()
		{
			foreach (var genre in genres)
			{
				Console.WriteLine("{0}.{1}	{2}:	{3}", genre.genreNumber, genre.subgenreNumber, genre.name, genre.description);
			}
		}
	}
}
