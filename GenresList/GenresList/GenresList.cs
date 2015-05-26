using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace GenresList
{
	[DataContract]
	static public class GenresList
	{
		[DataMember] 
		public static List<GenresListEntity> genres = new List<GenresListEntity>();

		public static void initialize(List<GenresListEntity> availableGenres)
		{
			genres = availableGenres;
		}

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
//				Console.WriteLine("Failed to fetch genres list.");
				throw;
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
				addGenresIgnoringComments(line);
				line = genresFile.ReadLine();
			}
		}

		private const int firstCharacterPosition = 0;
		private static void addGenresIgnoringComments(string line)
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
			try
			{
				return id < 0 ? "[all]" : genres[id].description;
			}
			catch
			{
				return "";
			}
		}

		public const int notFoundID = -1;
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


		public static List<GenresListEntity> getAvailableGenres()
		{
			return genres;
		}
	}
}
