using System;
using System.Collections.Generic;
using testWCFService.INetLibService;

namespace testWCFService
{
	class Program
	{
		static void Main(string[] args)
		{
			ServiceClient client = new ServiceClient("NetTcpBinding_IService", "net.tcp://172.19.12.228:9854/INetLib");

			List<INetLibService.BookEntity> books = client.selectBooksByAuthor("поттер");
			foreach (INetLibService.BookEntity book in books)
			{
				Console.WriteLine("{0}" , book.title);
			}
		}
	}
}
