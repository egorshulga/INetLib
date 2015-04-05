namespace INPImport
{
	enum NAMEPART
	{
		SURNAME,
		NAME,
		MIDDLENAME
	};

	class Author
	{
		public string surname { get; private set; }		//фамилия
		public string name { get; private set; }		//имя
		public string middleName { get; private set; }	//отчество

		public Author(string fullName)
		{
			parseFullName(fullName);
		}

		private const char nameDelimiter = ',';
		private void parseFullName(string fullName)
		{
			string[] splittedFullName = fullName.Split(nameDelimiter);

			surname = splittedFullName[(int)NAMEPART.SURNAME];
			name = splittedFullName[(int)NAMEPART.NAME];
			middleName = splittedFullName[(int)NAMEPART.MIDDLENAME];
		}
	}
}
