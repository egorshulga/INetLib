using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INetLibClient
{
	public static class FormatConvertor
	{
		public enum Format
		{
			fb2, epub, mobi, azw3
		}

		private static string convertorPath = "convertor\\fb2conv.exe";

		public static void convert(string fileToConvert, Format outputFormat)
		{
			if (shouldOutputFileNotBeConverted(outputFormat)) return;

//			convertorPath = Directory.GetCurrentDirectory() + convertorPath;

			string arguments = getConversionArguments(outputFormat);

			fileToConvert = '"' + fileToConvert + '"';

			Process.Start(convertorPath, arguments + fileToConvert);
		}

		private static bool shouldOutputFileNotBeConverted(Format outputFormat)
		{
			return outputFormat == Format.fb2;
		}

		private static string getConversionArguments(Format outputFormat)
		{
			switch (outputFormat)
			{
				case Format.epub:
					return " --delete-source-file --output-format epub ";
				case Format.mobi:
					return " --delete-source-file --output-format mobi ";
				case Format.azw3:
					return " --delete-source-file --output-format azw3 ";
				default:
					return " --delete-source-file ";
			}
		}
	}
}
