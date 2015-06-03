using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BookEntity;
using GenresList;
using WCFServiceLibrary;

namespace INetLibClient
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		public List<GenresListEntity> genres { get; set; }

		public MainWindow()
		{
			InitializeComponent();

			fetchServerAddressAndDownloadFolderFromConfigFile();
		}

		private void getAvailableGenresFromServerAndInitializeGenresBox()
		{
			GenresList.GenresList.initialize(client.getAvailableGenres());

			genreBox.ItemsSource = GenresList.GenresList.genres;
		}


		private void cancelButton_Click(object sender, RoutedEventArgs e)
		{
			writePreferencesToConfigFile();
			Close();
		}

		private void booksListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			setSelectedBookMetadataLabels();
		}

		private void setSelectedBookMetadataLabels()
		{
			BookEntity.BookEntity selectedBook = booksListBox.SelectedItem as BookEntity.BookEntity;
			if (selectedBook == null) return;

			titleLabel.Content = selectedBook.title;
			authorsLabel.Content = selectedBook.authors.getAuthors();
			genresLabel.Content = selectedBook.genres.getGenres();
			seriesLabel.Content = selectedBook.seriesTitle + '[' + selectedBook.numberInSeries + ']';
			languageLabel.Content = selectedBook.language;
			dateLabel.Content = selectedBook.dateAdded;
			sizeLabel.Content = selectedBook.fileSize.ToString();
		}


		private void closeMenuItem_Click(object sender, RoutedEventArgs e)
		{
			cancelButton_Click(sender, e);
		}

		private void genreBox_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Delete)
				genreBox.SelectedIndex = -1;
		}

		private void searchButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				BookEntity.BookEntity template = new BookEntity.BookEntity();
				template.authors = new Authors(new Author{fullName = authorBox.Text});
				template.title = titleBox.Text;
				template.genres = new Genres(genreBox.SelectedIndex);

				booksListBox.ItemsSource = client.selectBooksByTemplate(template);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void downloadButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (booksListBox.SelectedItem != null)
				{
					Thread retrievingThread = new Thread(retrieveBookFileFromServer);
					retrievingThread.Start((BookEntity.BookEntity)booksListBox.SelectedItem);
				}
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message);
			}
		}

		private void retrieveBookFileFromServer(object bookObject)
		{
			BookEntity.BookEntity book = (BookEntity.BookEntity)bookObject;
			string saveFilePath = Path.Combine(downloadFolder, Path.ChangeExtension(getFileName(book), book.extension));

			using (var fileStream = File.Open(saveFilePath, FileMode.Create))
			{
				try
				{
					var stream = client.extractBook(book);
					stream.CopyTo(fileStream);
				}
				catch (Exception e)
				{
					MessageBox.Show(e.Message);
				}
			}

			FormatConvertor.convert(saveFilePath, formatToUse);
		}

		private string getFileName(BookEntity.BookEntity book)
		{
			return cleanFileName(book.authors + " - " + book.title);
		}
		private static string cleanFileName(string fileName)
		{
//			return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
			List<char> invalidChars = new List<char>(Path.GetInvalidFileNameChars());
			invalidChars.Add('.');

			string invalidCharsRemoved = new string(fileName.Where(x => !invalidChars.Contains(x)).ToArray());

			return invalidCharsRemoved;
		}

		private const string configFilePath = "config.ini";
		
		public static string serverFullURI;
		private IService client;
		public static string downloadFolder;
		public static FormatConvertor.Format formatToUse;

		private void fetchServerAddressAndDownloadFolderFromConfigFile()
		{
			try
			{
				StreamReader reader = new StreamReader(configFilePath);
				serverFullURI = reader.ReadLine();
				downloadFolder = reader.ReadLine();
				readChosenPreferredFormat(reader);
				reader.Close();
//				File.Delete(configFilePath);

				checkPreferences();
			}
			catch
			{
				showPreferencesWindow();
			}
		}

		private void checkPreferences()
		{
			if (!arePreferencesCorrect())
			{
				showPreferencesWindow();
			}
		}

		private bool arePreferencesCorrect()
		{
			try
			{
				establishConnectionToServer();
				getAvailableGenresFromServerAndInitializeGenresBox();
			}
			catch
			{
				MessageBox.Show("Unable connect to server. Check server address.");
				return false;
			}

			return Directory.Exists(downloadFolder);
		}

		private void establishConnectionToServer()
		{
			var binding = new NetTcpBinding
			{
				Security =
				{
					Mode = SecurityMode.None,
					Transport = { ClientCredentialType = TcpClientCredentialType.None },
					Message = { ClientCredentialType = MessageCredentialType.None }
				},
				MaxReceivedMessageSize = int.MaxValue
			};
			var channelFactory = new ChannelFactory<IService>(binding, serverFullURI);

			client = channelFactory.CreateChannel();
		}

		static public bool isPreferencesWindowOpenedAtStartUp = true;
		private void showPreferencesWindow()
		{
			showDialogPreferencesWindow();

			checkPreferences();
		}

		private void showDialogPreferencesWindow()
		{
			PreferencesWindow preferencesWindow = new PreferencesWindow {fb2Button = {IsChecked = true}};
			preferencesWindow.ShowDialog();
		}
		private void showDialogPreferencesWindowInitializingBoxes()
		{
			PreferencesWindow preferencesWindow = new PreferencesWindow();
			setPreferencesWindowBoxes(preferencesWindow);
			setPreferredBookFormat(preferencesWindow);
			preferencesWindow.ShowDialog();
			checkPreferences();
		}

		private void setPreferredBookFormat(PreferencesWindow preferencesWindow)
		{
			switch (formatToUse)
			{
				case FormatConvertor.Format.fb2:
					preferencesWindow.fb2Button.IsChecked = true;
					break;
				case FormatConvertor.Format.epub:
					preferencesWindow.epubButton.IsChecked = true;
					break;
				case FormatConvertor.Format.mobi:
					preferencesWindow.mobiButton.IsChecked = true;
					break;
				case FormatConvertor.Format.azw3:
					preferencesWindow.azw3Button.IsChecked = true;
					break;
			}
		}

		private void setPreferencesWindowBoxes(PreferencesWindow preferencesWindow)
		{
			if (!string.IsNullOrEmpty(serverFullURI))
				preferencesWindow.setServerHostUri(serverFullURI);
			if (!string.IsNullOrEmpty(downloadFolder))
				preferencesWindow.setDownloadFolderBox(downloadFolder);
		}



		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			showDialogPreferencesWindowInitializingBoxes();
		}


		private static void writePreferencesToConfigFile()
		{
			try
			{
				StreamWriter writer = new StreamWriter(configFilePath);
				writer.WriteLine(serverFullURI);
				writer.WriteLine(downloadFolder);
				writeChosenPreferredFormat(writer);
				writer.Close();
			}
			catch
			{
//				MessageBox.Show("Unable to write preferences to config file");
			}
		}

		private static void writeChosenPreferredFormat(StreamWriter writer)
		{
			switch (formatToUse)
			{
				case FormatConvertor.Format.fb2:
					writer.WriteLine("fb2");
					break;
				case FormatConvertor.Format.epub:
					writer.WriteLine("epub");
					break;
				case FormatConvertor.Format.mobi:
					writer.WriteLine("mobi");
					break;
				case FormatConvertor.Format.azw3:
					writer.WriteLine("azw3");
					break;
			}
		}

		private static void readChosenPreferredFormat(StreamReader reader)
		{
			string formatString = reader.ReadLine();
			switch (formatString)
			{
				case "fb2":
					formatToUse = FormatConvertor.Format.fb2;
					break;
				case "epub":
					formatToUse = FormatConvertor.Format.epub;
					break;
				case "mobi":
					formatToUse = FormatConvertor.Format.mobi;
					break;
				case "azw3":
					formatToUse = FormatConvertor.Format.azw3;
					break;
			}
		}
	}
}
