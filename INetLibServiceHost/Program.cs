using System;
using System.IO;
using System.Net.Mime;
using System.ServiceModel;
using WCFServiceLibrary;

namespace INetLibServiceHost
{
	static class Program
	{
		static void Main(string[] args)
		{
			ServerInitialization.initialize();

			tryStartService();
		}

		private static void tryStartService()
		{
			try
			{
				startService();
			}
			catch (Exception e)
			{
				Console.WriteLine("Failed to start the service.");
				Console.WriteLine("Check server address configuration.");
				Console.WriteLine("Terminating.");
			}
		}

		private static void startService()
		{
			using (ServiceHost host = new ServiceHost(typeof (Service)))
			{
				host.Open();
				Console.WriteLine("INetLib host service started at " + DateTime.Now);
				Console.WriteLine("Press any key to stop the host service.");
				Console.ReadKey();
				host.Close();
			}
		}
	}
}
