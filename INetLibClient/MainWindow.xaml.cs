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
	public partial class MainWindow : Window
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
				MessageBox.Show(ex.ToString());
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
				MessageBox.Show(exception.ToString());
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
		}

		private string getFileName(BookEntity.BookEntity book)
		{
			return cleanFileName(book.authors + " - " + book.title);
		}
		private static string cleanFileName(string fileName)
		{
			return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
		}

		private const string configFilePath = "config.ini";
		
		public static string serverFullURI;
		private IService client;
		public static string downloadFolder;

		private void fetchServerAddressAndDownloadFolderFromConfigFile()
		{
			try
			{
				StreamReader reader = new StreamReader(configFilePath);
				serverFullURI = reader.ReadLine();
				downloadFolder = reader.ReadLine();
				reader.Close();
				File.Delete(configFilePath);

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

				return Directory.Exists(downloadFolder);
			}
			catch
			{
				MessageBox.Show("Preferences are not correct");
				return false;
			}
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

		private void showPreferencesWindow()
		{
			showDialogPreferencesWindow();

			checkPreferences();
		}

		private void showDialogPreferencesWindow()
		{
			PreferencesWindow preferencesWindow = new PreferencesWindow();
			preferencesWindow.ShowDialog();
		}
		private void showDialogPreferencesWindowInitializingBoxes()
		{
			PreferencesWindow preferencesWindow = new PreferencesWindow();
			setPreferencesWindowBoxes(preferencesWindow);
			preferencesWindow.ShowDialog();
			checkPreferences();
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
				writer.Close();
			}
			catch
			{
//				MessageBox.Show("Unable to write preferences to config file");
			}
		}
	}
}
