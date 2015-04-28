using System;

namespace BookEntity
{
	enum Name
	{
		LastName,
		FirstName,
		MiddleName
	};

	public class Author
	{
		public string surname { get; private set; }		//фамилия
		public string name { get; private set; }		//имя
		public string middleName { get; private set; }	//отчество
		public string fullName { get; private set; } 

		public Author(string fullName)
		{
			tryParseFullName(fullName);
		}

		private const char nameDelimiter = ',';
		private void tryParseFullName(string fullName)
		{
			string[] splittedFullName = fullName.Split(nameDelimiter);
			try
			{
				this.fullName = fullName;
				setNameSurnameAndMiddlename(splittedFullName);
			}
			catch (Exception e)
			{ }		//Something bad happened while trying to set fields of the full name
		}

		private void setNameSurnameAndMiddlename(string[] splittedFullName)
		{
			surname = splittedFullName[(int) Name.LastName];
			name = splittedFullName[(int) Name.FirstName];
			middleName = splittedFullName[(int) Name.MiddleName];
		}
	}
}
