using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Discovery;
using WCFServiceLibrary;

namespace testWCFService
{
	class Program
	{
		static void Main(string[] args)
		{
			var client = new DiscoveryClient(new UdpDiscoveryEndpoint());
			var criteria = new FindCriteria(typeof (IService)) {MaxResults = 1};
			var foundServices = client.Find(criteria);

			foreach (var service in foundServices.Endpoints)
			{
				Console.WriteLine(service.Address);
			}

			var binding = new NetTcpBinding
			{
				Security =
				{
					Mode = SecurityMode.None,
					Transport = {ClientCredentialType = TcpClientCredentialType.None},
					Message = {ClientCredentialType = MessageCredentialType.None}
				}
			};
//			var endPoint = new EndpointAddress("net.tcp://172.19.12.228:14141/INetLib");
			var address = foundServices.Endpoints.First(ep => ep.Address.Uri.Scheme == "net.tcp").Address;
			var channelFactory = new ChannelFactory<IService>(binding, address);

			IService factory = channelFactory.CreateChannel();

			List<BookEntity.BookEntity> books = factory.selectBooksByTitle("поттер");
			books = factory.selectBooksByAuthor("линдгрен");
			var book = factory.selectBookByID(books[0].bookID);
			books = factory.selectBooksByGenre(14);
			TextReader stream = new StreamReader(factory.extractBook(books[0]));
//			Console.WriteLine(stream.ReadToEnd());
		}
	}
}
