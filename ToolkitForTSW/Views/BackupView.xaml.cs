using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ToolkitForTSW.Views
	{
	
	public partial class BackupView
		{

		public BackupView()
			{
			InitializeComponent();
			}

		private void OnOKButtonClicked(Object Sender, RoutedEventArgs E)
			{
			Close();
			}
		}
	}
