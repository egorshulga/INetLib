using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace INetLibClient
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			ServerSearch serverSearchWindow = new ServerSearch();
			serverSearchWindow.ShowDialog();

			DownloadFolderPathInput downloadFolderPathInput = new DownloadFolderPathInput();
			downloadFolderPathInput.ShowDialog();
		}

		private void cancelButton_Click(object sender, RoutedEventArgs e)
		{
			Environment.Exit(0);
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
	}
}
