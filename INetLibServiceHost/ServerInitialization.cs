using System;
using INetLibServiceHost.Properties;
using MetadataDB;

namespace INetLibServiceHost
{
	static class ServerInitialization
	{
		private static string genresFilePath;
		private static string metadataPath;
		private static string booksStoragePath;

		public static void initialize()
		{
			Console.WriteLine("INetLibServer should be initialized before starting.");
			
			checkSettingsAndInitializeGenresList();

			checkSettingsAndInitializeMetadata();

			checkSettingsAndInitializeBooksStorage();
			
			Console.WriteLine("Initialization finished. Starting service...");
		}




		private static void checkSettingsAndInitializeGenresList()
		{
			setGenresListPath();

			tryGenresInitialization();

			saveGenresListPathToSettings();
		}
		private static void setGenresListPath()
		{
			if (isGenresFilePathSetInSettings())
			{
				setGenresListPathFromSettings();
			}
			else
			{
				setGenresListPathFromUserInput();
			}
		}
		private static bool isGenresFilePathSetInSettings()
		{
			return !string.IsNullOrEmpty(Settings.Default.genresListPath);
		}
		private static void setGenresListPathFromSettings()
		{
			Console.WriteLine("Genres file path found in settings. Initializing...");
			genresFilePath = Settings.Default.genresListPath;
		}
		private static void setGenresListPathFromUserInput()
		{
			Console.Write("Enter genres file path (.glst): ");
			genresFilePath = Console.ReadLine();
		}
		private static void tryGenresInitialization()
		{
			try
			{
				genresInitialization();
			}
			catch (Exception)
			{
				Console.WriteLine("Failed to fetch genres. ");

				setGenresListPathFromUserInput();

				tryGenresInitialization();
			}
		}
		private static void genresInitialization()
		{
			GenresList.GenresList.initialize(genresFilePath);
			Console.WriteLine("Genres fetched successfully.");
		}
		private static void saveGenresListPathToSettings()
		{
			Settings.Default.genresListPath = genresFilePath;
			Settings.Default.Save();
		}







		private static void checkSettingsAndInitializeMetadata()
		{
			setMetadataFilePath();

			tryMetadataInitialization();

			saveMetadataPathToSettings();
		}
		private static void setMetadataFilePath()
		{
			if (isMetadataPathSetInSettings())
			{
				setMetadataPathFromSettings();
			}
			else
			{
				setMetadataPathFromUserInput();
			}
		}
		private static bool isMetadataPathSetInSettings()
		{
			return !string.IsNullOrEmpty(Settings.Default.metadataPath);
		}
		private static void setMetadataPathFromSettings()
		{
			Console.WriteLine("Metadata file path found in settings. Initializing...");
			metadataPath = Settings.Default.metadataPath;
		}
		private static void setMetadataPathFromUserInput()
		{
			Console.Write("Enter metadata file path (.inpx): ");
			metadataPath = Console.ReadLine();
		}
		private static void tryMetadataInitialization()
		{
			try
			{
				metadataInitialization();
			}
			catch (Exception)
			{
				Console.WriteLine("Failed to fetch metadata. ");

				setMetadataPathFromUserInput();

				tryMetadataInitialization();
			}
		}
		private static void metadataInitialization()
		{
			MetadataList.initialize(metadataPath);
			Console.WriteLine("Metadata fetched successfully.");
		}

		private static void saveMetadataPathToSettings()
		{
			Settings.Default.metadataPath = metadataPath;
			Settings.Default.Save();
		}







		private static void checkSettingsAndInitializeBooksStorage()
		{
			setBooksStoragePath();

			initializeBooksStorage();

			saveBooksStoragePathToSettings();
		}
		private static void setBooksStoragePath()
		{
			if (isBooksStoragePathSetInSettings())
			{
				setBooksStoragePathFromSettings();
			}
			else
			{
				setBooksStoragePathFromUserInput();
			}
		}
		private static bool isBooksStoragePathSetInSettings()
		{
			return !string.IsNullOrEmpty(Settings.Default.booksFolderPath);
		}

		private static void setBooksStoragePathFromSettings()
		{
			Console.WriteLine("Books storage path found in settings. Initializing...");
			booksStoragePath = Settings.Default.booksFolderPath;
		}
		private static void setBooksStoragePathFromUserInput()
		{
			Console.Write("Enter books archives folder path: ");
			booksStoragePath = Console.ReadLine();
		}

		private static void initializeBooksStorage()
		{
			BookExtractor.BookExtractor.initialize(booksStoragePath);
			Console.WriteLine("Books extractor initialized.");
		}
		private static void saveBooksStoragePathToSettings()
		{
			Settings.Default.booksFolderPath = booksStoragePath;
			Settings.Default.Save();
		}
	}
}