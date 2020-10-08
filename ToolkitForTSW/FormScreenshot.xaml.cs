using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
using System.Windows.Shapes;

namespace TSWTools
{
	/// <summary>
	/// Interaction logic for FormScreenshot.xaml
	/// </summary>
	public partial class FormScreenshot : Window
	{
	private CScreenshot Screenshot { get; set; }

	public  FormScreenshot(CScreenshot MyScreenshot)
		{
		InitializeComponent();
		Contract.Assert(MyScreenshot!=null);
		Screenshot = MyScreenshot;
		DataContext = Screenshot;
		}

	private void OnDeleteButtonClicked(Object Sender, RoutedEventArgs E)
		{
		CApps.DeleteSingleFile(Screenshot.Path.FullName);
		Close();
		}

	private void OnSaveAsButtonClicked(Object Sender, RoutedEventArgs E)
		{
		CScreenshot.SaveScreenShotAs(Screenshot.Path.FullName, "C:\\");
		}

	private void OnOkButtonClicked(Object Sender, RoutedEventArgs E)
		{
		Close();
		}

	}




}
