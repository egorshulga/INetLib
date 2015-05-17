using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Discovery;
using BookEntity;
using WCFServiceLibrary;

namespace testWCFService
{
	class Program
	{
		static void Main(string[] args)
		{
			Stopwatch timer = new Stopwatch();

			timer.Start();
			var discoveryClient = new DiscoveryClient(new UdpDiscoveryEndpoint());
			var criteria = new FindCriteria(typeof (IService)) {MaxResults = 1};
			var foundServices = discoveryClient.Find(criteria);

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
				},
				MaxReceivedMessageSize = int.MaxValue
			};
			var address = foundServices.Endpoints.First(ep => ep.Address.Uri.Scheme == "net.tcp").Address.Uri.AbsoluteUri;
			var channelFactory = new ChannelFactory<IService>(binding, address);

			IService client = channelFactory.CreateChannel();
			timer.Stop();
			Console.WriteLine("Server search time:	{0}", timer.Elapsed);




			timer.Restart();
			var books = client.selectBooksByGenre(11);
			timer.Stop();
			Console.WriteLine("Genre query time:	{0}", timer.Elapsed);

			timer.Restart();
			books = client.selectBooksByGenres(new Genres{genresIDs = new List<int>{15, 22}});
			timer.Stop();
			Console.WriteLine("Genres query time:	{0}", timer.Elapsed);

			timer.Restart();
			books = client.selectBooksByAuthor("норман");
			timer.Stop();
			Console.WriteLine("Author query time:	{0}", timer.Elapsed);

			timer.Restart();
			books = client.selectBooksByTitle("potter");
			timer.Stop();
			Console.WriteLine("Title query time:	{0}", timer.Elapsed);

			timer.Restart();
			var book = client.selectBookByID(books[0].bookID);
			timer.Stop();
			Console.WriteLine("ID query time:		{0}", timer.Elapsed);

			timer.Restart();
			book = new BookEntity.BookEntity();
			book.authors = new Authors(new Author{fullName = "роулинг"});
			book.title = "поттер";
			book.genres = new Genres{genresIDs = new List<int>{-1}};
			books = client.selectBooksByTemplate(book);
			timer.Stop();
			Console.WriteLine("Template query time:	{0}", timer.Elapsed);

			timer.Restart();
			TextReader stream = new StreamReader(client.extractBook(books[0]));
//			Console.WriteLine(stream.ReadToEnd());
			timer.Stop();
			Console.WriteLine("Book query time:	{0}", timer.Elapsed);

//			Console.ReadLine();
		}
	}
}
