using System;
using System.Collections;
using System.Collections.Generic;

namespace INPImport
{
	class Genres
	{
		//вместо названий жанров используются их ID
		private readonly List<int> genresIDs = new List<int>();

		public Genres(string genresString)
		{
			parseGenresString(genresString);
		}

		private void parseGenresString(string genresString)
		{
			List<string> genresStringSplitted = splitGenresString(genresString);
			deleteLastEmptyGenre(genresStringSplitted);

			addGenresByNames(genresStringSplitted);
		}

		private static List<string> splitGenresString(string genresString)
		{
			const char genresDelimiter = ':';
			return new List<string>(genresString.Split(genresDelimiter));
		}

		private static void deleteLastEmptyGenre(IList genresStringSplitted)
		{
			int lastElement = genresStringSplitted.Count - 1;
			genresStringSplitted.RemoveAt(lastElement);
		}

		private void addGenresByNames(IEnumerable<string> genresStringSplitted)
		{
			foreach (var genreString in genresStringSplitted)
			{
				genresIDs.Add(GenresList.getGenreID(genreString));
			}
		}


		public void printGenresDebug()
		{
			foreach (var id in genresIDs)
			{
				Console.WriteLine("{0}	{1}	{2}", id, GenresList.getGenreName(id), GenresList.getGenreDescription(id));
			}
		}
	}
}
