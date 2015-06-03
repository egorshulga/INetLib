using System;
using System.Linq;
using System.ServiceModel.Discovery;
using System.Windows;
using System.Windows.Forms;
using WCFServiceLibrary;
using MessageBox = System.Windows.MessageBox;

namespace INetLibClient
{
	/// <summary>
	/// Interaction logic for PreferencesWindow.xaml
	/// </summary>
	public partial class PreferencesWindow
	{
		public PreferencesWindow()
		{
			InitializeComponent();
		}

		
		private const string scheme = "net.tcp";
		private const string serviceRelativeURL = ":14141/INetLib";
		public void setServerHostUri(string serverFullURI)
		{
			serverFullUri = serverFullURI;
		}

		public void setDownloadFolderBox(string downloadFolder)
		{
			downloadFolderBox.Text = downloadFolder;
		}


		private string serverFullUri
		{
			get
			{
				if (string.IsNullOrEmpty(serverPathBox.Text))
					return null;
				return scheme + "://" + serverPathBox.Text + serviceRelativeURL;
			}
			set
			{
				Uri serverUri = new Uri(value);
				serverPathBox.Text = serverUri.Host;// + ":" + serverUri.Port;
			}
		}
		private void autoDiscoveryButton_Click(object sender, RoutedEventArgs e)
		{
			var discoveryClient = new DiscoveryClient(new UdpDiscoveryEndpoint());
			var criteria = new FindCriteria(typeof(IService)) { MaxResults = 1 };
			var foundServices = discoveryClient.Find(criteria);
			if (foundServices.Endpoints.Count == 0)
			{
				MessageBox.Show("There are no INetLib services found.\nTry entering server address manually.");
				return;
			}
			var serviceURI = foundServices.Endpoints.First(ep => ep.Address.Uri.Scheme == scheme).Address.Uri;

			serverFullUri = serviceURI.AbsoluteUri;
		}

		private void downloadFolderButton_Click(object sender, RoutedEventArgs e)
		{
			FolderBrowserDialog saveFileDialog = new FolderBrowserDialog();
			if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				downloadFolderBox.Text = saveFileDialog.SelectedPath;
			}
		}

		private bool isOKButtonPressed = false;
		private void okButton_Click(object sender, RoutedEventArgs e)
		{
			MainWindow.serverFullURI = serverFullUri;
			MainWindow.downloadFolder = downloadFolderBox.Text;
			MainWindow.formatToUse = format;
			isOKButtonPressed = true;
			Close();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!isOKButtonPressed)
				if (MainWindow.isPreferencesWindowOpenedAtStartUp)
					Environment.Exit(0);
			MainWindow.isPreferencesWindowOpenedAtStartUp = false;
		}

		private FormatConvertor.Format format = FormatConvertor.Format.fb2;
		private void fb2Button_Checked(object sender, RoutedEventArgs e)
		{
			format = FormatConvertor.Format.fb2;
		}

		private void epubButton_Checked(object sender, RoutedEventArgs e)
		{
			format = FormatConvertor.Format.epub;
		}

		private void mobiButton_Checked(object sender, RoutedEventArgs e)
		{
			format = FormatConvertor.Format.mobi;
		}

		private void azw3Button_Checked(object sender, RoutedEventArgs e)
		{
			format = FormatConvertor.Format.azw3;
		}
	}
}
