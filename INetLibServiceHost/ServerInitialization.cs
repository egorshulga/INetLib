using System;
using System.IO;

namespace INetLibServiceHost
{
	static class ServerInitialization
	{
		private static string genresListPath;
		private static string metadataPath;
		private static string booksFolderPath;
		private const string serverConfigurationFilePath = "server.config";

		public static void initialize()
		{
			getInitializationPaths();

			tryGenresInitialization();

			tryMetadataDBInitialization();

			tryBooksStorageInitialization();

			saveConfigurationPathsToConfigFile();

			Console.WriteLine("Initializations finished. Starting service...");
		}


		private static void getInitializationPaths()
		{
			if (File.Exists(serverConfigurationFilePath))
			{
				Console.WriteLine("Config file found. Initializing...");
				tryGetConfigurationPathsFromFile();
			}
			else
			{
				getConfigurationPathsFromUserInput();
			}
			saveConfigurationPathsToConfigFile();
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
			StreamReader configReader = new StreamReader(serverConfigurationFilePath);
			genresListPath = configReader.ReadLine();
			metadataPath = configReader.ReadLine();
			booksFolderPath = configReader.ReadLine();
			configReader.Close();
		}

		private static void saveConfigurationPathsToConfigFile()
		{
			StreamWriter configWriter = new StreamWriter(serverConfigurationFilePath);
			configWriter.WriteLine(genresListPath);
			configWriter.WriteLine(metadataPath);
			configWriter.WriteLine(booksFolderPath);
			configWriter.Close();
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
					Console.WriteLine("Failed to fetch genre. ");
//					Console.WriteLine(e.Message);
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
			MetadataDB.MetadataDB.initialize(metadataPath);
			Console.WriteLine("Metadata fetched.");
		}

		private static void tryBooksStorageInitialization()
		{
			bool successfulInitialization = false;
			while (!successfulInitialization)
			{
				try
				{
					booksStorageInitialization();
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

		private static void booksStorageInitialization()
		{
			BookExtractor.BookExtractor.initialize(booksFolderPath);
			Console.WriteLine("Books extractor initialized.");
		}
	}
}