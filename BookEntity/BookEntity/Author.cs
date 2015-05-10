using System;
using System.Runtime.Serialization;

namespace BookEntity
{

	[DataContract]
	public class Author
	{
		[DataContract]
		enum Name
		{
			[EnumMember]
			LastName,
			[EnumMember]
			FirstName,
			[EnumMember]
			MiddleName
		};

		[DataMember]
		public string surname { get; private set; }		//фамилия
		[DataMember]
		public string name { get; private set; }		//имя
		[DataMember]
		public string middleName { get; private set; }	//отчество
		[DataMember]
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
