﻿using System;
using System.Collections.Generic;
using System.IO;

namespace GenresList
{
	static public class GenresList
	{
		private static readonly List<GenresListEntity> genres = new List<GenresListEntity>();
		private static bool isGenresListInitialized = false;

		public static void initialize(string genresFilePath)
		{
			tryReadGenresFromFile(genresFilePath);
		}

		private static void tryReadGenresFromFile(string genresFilePath)
		{
			try
			{
				readGenresFromFile(genresFilePath);
				isGenresListInitialized = true;
			}
			catch (Exception e)
			{ }
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
			var genreName = isGenresListInitialized ? genres[id].name : null;
			return genreName;
		}

		static public string getGenreDescription(int id)
		{
			var genreDescription = isGenresListInitialized ? genres[id].description : null;
			return genreDescription;
		}

		public const int notFoundID = -1;
		static public int getGenreID(string genreString)
		{
			var findIndex = isGenresListInitialized ? genres.FindIndex(entity => entity.name == genreString) : -1;
			return findIndex;
		}


		public static void printGenresListDebug()
		{
			if (!isGenresListInitialized) return;
			foreach (var genre in genres)
			{
				Console.WriteLine("{0}.{1}	{2}:	{3}", genre.genreNumber, genre.subgenreNumber, genre.name, genre.description);
			}
		}
	}
}
