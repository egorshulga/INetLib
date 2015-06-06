using System;
using System.IO;
using INetLibServiceHost.Properties;

namespace INetLibServiceHost
{
	static class ServerInitialization
	{
		private static string genresListPath;
		private static string metadataPath;
		private static string booksFolderPath;
//		private const string serverConfigurationFilePath = "config.ini";

		public static void initialize()
		{
			getInitializationPaths();

			tryGenresInitialization();

			tryMetadataDBInitialization();

			tryBooksStorageInitialization();

			saveConfigurationPathsToConfigFile();

			Console.WriteLine("Initialization finished. Starting service...");
		}


		private static void getInitializationPaths()
		{
			if (!areSettingsNotSet())
			{
				Console.WriteLine("Settings found. Initializing...");
				tryGetConfigurationPathsFromFile();
			}
			else
			{
				getConfigurationPathsFromUserInput();
			}
			saveConfigurationPathsToConfigFile();
		}

		private static bool areSettingsNotSet()
		{
			return string.IsNullOrEmpty(Settings.Default.genresListPath)  || 
				   string.IsNullOrEmpty(Settings.Default.metadataPath)    ||
			       string.IsNullOrEmpty(Settings.Default.booksFolderPath);
		}

		private static void tryGetConfigurationPathsFromFile()
		{
			try
			{
				getConfigurationPathsFromFile();
			}
			catch (Exception e)
			{
//				Console.WriteLine(e.Message);
				getConfigurationPathsFromUserInput();
			}
		}

		private static void getConfigurationPathsFromFile()
		{
//			StreamReader configReader = new StreamReader(serverConfigurationFilePath);
//			genresListPath = configReader.ReadLine();
//			metadataPath = configReader.ReadLine();
//			booksFolderPath = configReader.ReadLine();
//			configReader.Close();

			genresListPath = Settings.Default.genresListPath;
			metadataPath = Settings.Default.metadataPath;
			booksFolderPath = Settings.Default.booksFolderPath;
		}

		private static void saveConfigurationPathsToConfigFile()
		{
//			StreamWriter configWriter = new StreamWriter(serverConfigurationFilePath);
//			configWriter.WriteLine(genresListPath);
//			configWriter.WriteLine(metadataPath);
//			configWriter.WriteLine(booksFolderPath);
//			configWriter.Close();

			Settings.Default.genresListPath = genresListPath;
			Settings.Default.metadataPath = metadataPath;
			Settings.Default.booksFolderPath = booksFolderPath;
			Settings.Default.Save();
		}

		private static void getConfigurationPathsFromUserInput()
		{
			getGenresListPathFromUserInput();
			getMetadataPathFromUserInput();
			getBooksPathFromUserInput();
		}

		private static void getGenresListPathFromUserInput()
		{
			Console.WriteLine("Enter genres file path (.glst): ");
			genresListPath = Console.ReadLine();
		}

		private static void getMetadataPathFromUserInput()
		{
			Console.WriteLine("Enter metadata file path (.inpx): ");
			metadataPath = Console.ReadLine();
		}

		private static void getBooksPathFromUserInput()
		{
			Console.WriteLine("Enter books archives folder path: ");
			booksFolderPath = Console.ReadLine();
		}



		private static void tryGenresInitialization()
		{
			bool successfulInitialization = false;
			while (!successfulInitialization)
			{
				try
				{
					genresInitialization();
					successfulInitialization = true;
				}
				catch (Exception e)
				{
					successfulInitialization = false;
					Console.WriteLine("Failed to fetch genres. ");
					getGenresListPathFromUserInput();
				}
			}
		}

		private static void genresInitialization()
		{
			GenresList.GenresList.initialize(genresListPath);
			Console.WriteLine("Genres fetched.");
		}

		private static void tryMetadataDBInitialization()
		{
			bool successfulInitialization = false;
			while (!successfulInitialization)
			{
				try
				{
					metadataDBInitialization();
					successfulInitialization = true;
				}
				catch (Exception e)
				{
					successfulInitialization = false;
					Console.WriteLine("Failed to fetch metadata. ");
//					Console.WriteLine(e.Message);
					getMetadataPathFromUserInput();
				}
			}
		}

		private static void metadataDBInitialization()
		{
			MetadataDB.MetadataList.initialize(metadataPath);
			Console.WriteLine("Metadata fetched.");
		}

		private static void tryBooksStorageInitialization()
		{
			bool successfulInitialization = false;
			while (!successfulInitialization)
			{
				try
				{
					if (string.IsNullOrEmpty(Settings.Default.booksFolderPath))
						getBooksPathFromUserInput();
					booksStorageInitialization();
					successfulInitialization = true;
				}
				catch (Exception e)
				{
					successfulInitialization = false;
					Console.WriteLine("Failed to fetch metadata. ");
					getMetadataPathFromUserInput();
				}
			}
		}

		private static void booksStorageInitialization()
		{
			BookExtractor.BookExtractor.initialize(booksFolderPath);
			Console.WriteLine("Books extractor initialized.");
		}
	}
}