using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace INPImport
{
	class Authors : IEnumerable<Author>
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
			return ((IEnumerable<Author>) authors).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
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
