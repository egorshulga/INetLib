using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookEntity
{
	public class Authors : IEnumerable<Author>
	{
		private readonly List<Author> authors = new List<Author>();

		public Authors(string fullNames)
		{
			parseAuthors(fullNames);
		}

		private const char authorDelimiter = ':';
		private void parseAuthors(string fullName)
		{
			List<string> splittedAuthorsFullNames = fullName.Split(authorDelimiter).ToList();
			deleteLastAuthor(splittedAuthorsFullNames);		//Последний автор всегда пустой. Таков формат файла.

			foreach (string authorName in splittedAuthorsFullNames)
			{
				Author author = new Author(authorName);
				authors.Add(author);
			}
		}

		private static void deleteLastAuthor(IList splittedAuthorsFullNames)
		{
			splittedAuthorsFullNames.RemoveAt( splittedAuthorsFullNames.Count - 1 );
		}

		public IEnumerator<Author> GetEnumerator()
		{
			return authors.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}


		public string getAuthors()
		{
			return getAllAuthorsString();
		}

		private string getAllAuthorsString()
		{
			StringBuilder authorsStringBuilder = new StringBuilder();
			StringBuilder authorFullName;
			for (int i = 0; i < authors.Count - 1; i++)
			{
				authorFullName = getAuthorFullNameStringBuilder(authors[i]);
				authorsStringBuilder.Append(authorFullName);
				authorsStringBuilder.Append(", ");
			}
			authorFullName = getAuthorFullNameStringBuilder(authors[authors.Count - 1]);
			authorsStringBuilder.Append(authorFullName);
			return authorsStringBuilder.ToString();			
		}

		private StringBuilder getAuthorFullNameStringBuilder(Author author)
		{
			StringBuilder authorsStringBuilder = new StringBuilder();
			authorsStringBuilder.Append(author.surname);
			authorsStringBuilder.Append(' ');
			authorsStringBuilder.Append(author.name);
			authorsStringBuilder.Append(' ');
			authorsStringBuilder.Append(author.middleName);
			return authorsStringBuilder;
		}


		public void printAuthorsToConsoleDebug()
		{
			foreach (var author in authors)
			{
				Console.WriteLine("{0} {1} {2}", author.surname, author.name, author.middleName);
			}
		}
	}
}
