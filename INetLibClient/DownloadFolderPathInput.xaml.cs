using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace INetLibClient
{
	/// <summary>
	/// Interaction logic for DownloadFolderPathInput.xaml
	/// </summary>
	public partial class DownloadFolderPathInput : Window
	{
		public DownloadFolderPathInput()
		{
			InitializeComponent();
		}

		public static string downloadFolder;

		private void downloadFolderButton_Click(object sender, RoutedEventArgs e)
		{
			FolderBrowserDialog saveFileDialog = new FolderBrowserDialog();
			if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				downloadFolderBox.Text = saveFileDialog.SelectedPath;
			}
		}

		private bool isDownloadFolderPathCorrect = false;
		private void OKButton_Click(object sender, RoutedEventArgs e)
		{
			downloadFolder = downloadFolderBox.Text;
			try
			{
				Directory.CreateDirectory(downloadFolder);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				return;
			}
			isDownloadFolderPathCorrect = true;
			Close();
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			if (!isDownloadFolderPathCorrect)
				Environment.Exit(0);
		}



	}
}
