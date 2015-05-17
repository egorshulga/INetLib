using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Discovery;
using System.Windows;
using WCFServiceLibrary;

namespace INetLibClient
{
	public partial class ServerSearch : Window
	{
		public ServerSearch()
		{
			InitializeComponent();
		}

		private string fullServerAddress;
		private string serverHostDomainName			//is a middle part of URL of fullServerAddress
		{
			get { return serverPathBox.Text; }
			set { serverPathBox.Text = value; } 
		}

		public static IService client;

		private const string scheme = "net.tcp";
		private const string serviceRelativeURL = "/INetLib";

		private void autdiscoveryButton_Click(object sender, RoutedEventArgs e)
		{
			var discoveryClient = new DiscoveryClient(new UdpDiscoveryEndpoint());
			var criteria = new FindCriteria(typeof(IService)) { MaxResults = 1 };
			var foundServices = discoveryClient.Find(criteria);
			if (foundServices.Endpoints.Count == 0)
			{
				MessageBox.Show("There are no INetLib services in current network.\nTry entering server address manually.");
				return;
			}
			var serviceURI = foundServices.Endpoints.First(ep => ep.Address.Uri.Scheme == scheme).Address.Uri;

			fullServerAddress = serviceURI.AbsoluteUri;

			serverHostDomainName = serviceURI.Host + ":" + serviceURI.Port;
		}

		private bool isServerFound = false;
		private void okButton_Click(object sender, RoutedEventArgs e)
		{
			fullServerAddress = scheme + "://" + serverHostDomainName + serviceRelativeURL;

			try
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
				var channelFactory = new ChannelFactory<IService>(binding, fullServerAddress);

				client = channelFactory.CreateChannel();

				isServerFound = true;
				Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			if (!isServerFound)
				Environment.Exit(0);
		}
	}
}
