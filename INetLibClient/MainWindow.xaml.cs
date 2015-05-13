using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BookEntity;

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

			GenresList.GenresList.initialize(@"D:\books\_Lib.rus.ec - Официальная\genres_fb2.glst");

			List<BookEntity.BookEntity> books =
				InpxImport.InpxImport.import(@"D:\books\_Lib.rus.ec - Официальная\librusec_local_fb2.inpx");
//				new List<BookEntity.BookEntity>
//			{
//				new BookEntity.BookEntity(
//					"Абдуллаева,Сахиба,:sf:Панаванне жанчын100239711001fb22007-06-20be")
//			};

			booksListBox.ItemsSource = books;
		}

		private void cancelButton_Click(object sender, RoutedEventArgs e)
		{
			Environment.Exit(0);
		}
	}
}
