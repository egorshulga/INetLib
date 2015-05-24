using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using GenresList;

namespace BookEntity
{
	[DataContract]
	public class Genres : IEnumerable<int>
	{
		//Genres IDs are used instead of their names.
		//Names can be easily obtained via GenresList class (it should be initialized before)
		[DataMember]
		public List<int> genresIDs		// = new List<int>()
		{ get; set; }

		public Genres()
		{
			genresIDs = new List<int>();
		}

		public Genres(int genreID) : this()
		{
			genresIDs.Add(genreID);
		}

		public Genres(string genresString) : this()
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
				int genreID = GenresList.GenresList.getGenreID(genreString);
				if (genreID != GenresList.GenresList.notFoundID)
					genresIDs.Add(genreID);
			}
		}

		public string getGenres()
		{
			StringBuilder genresStringBuilder = new StringBuilder();
			if (genresIDs.Count != 0)
			{
				for (int i = 0; i < genresIDs.Count - 1; i++)
				{
					genresStringBuilder.Append(GenresList.GenresList.getGenreDescription(genresIDs[i]));
					genresStringBuilder.Append(", ");
				}
				genresStringBuilder.Append(GenresList.GenresList.getGenreDescription(genresIDs[genresIDs.Count - 1]));
			}
			return genresStringBuilder.ToString();
		}

		public string getGenresFromGenresList(List<GenresListEntity> genresList)
		{
			StringBuilder genresStringBuilder = new StringBuilder();
			if (genresIDs.Count != 0)
			{
				for (int i = 0; i < genresIDs.Count - 1; i++)
				{
					genresStringBuilder.Append(genresList[genresIDs[i]].description);   //.getGenreDescription(genresIDs[i]));
					genresStringBuilder.Append(", ");
				}
				genresStringBuilder.Append(GenresList.GenresList.getGenreDescription(genresIDs[genresIDs.Count - 1]));
			}
			return genresStringBuilder.ToString();
		}


		public void printGenresDebug()
		{
			foreach (var id in genresIDs)
			{
				Console.WriteLine("{0}	{1}	{2}", id, GenresList.GenresList.getGenreName(id), GenresList.GenresList.getGenreDescription(id));
			}
		}

		public IEnumerator<int> GetEnumerator()
		{
			return genresIDs.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}


		public override string ToString()
		{
			return getGenres();
		}
	}
}
