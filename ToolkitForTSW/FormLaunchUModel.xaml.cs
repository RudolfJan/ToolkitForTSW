using System;
using System.Windows;
using System.Windows.Controls;

namespace TSWTools
	{
	/// <summary>
	/// Interaction logic for FormLaunchUModel.xaml
	/// </summary>
	public partial class FormLaunchUModel
		{
		public CUModelLauncher UModelLauncher { get; set; }

		public FormLaunchUModel()
			{
			InitializeComponent();
			UModelLauncher = new CUModelLauncher();
			DataContext = UModelLauncher;
			SetControlStates();
			}

		private void SetControlStates()
			{
			AddCommandButton.IsEnabled = UModelCommandsDataGrid.SelectedItem != null;
			AddFilesButton.IsEnabled = FileTreeView.SelectedItem != null;
			}

		private void OnOKButtonClicked(Object Sender, RoutedEventArgs E)
			{
			Close();
			}

		private void OnRunButtonClicked(Object Sender, RoutedEventArgs E)
      {
      var CommandLine = UModelLauncher.CommandLine + " " + UModelLauncher.PathSettings + " " + 
                       UModelLauncher.Package;

      UModelLauncher.Results += CApps.UnPackAsset(CommandLine);
			}

		private void OnUModelCommandsDataGridSelectionChanged(Object Sender,
			SelectionChangedEventArgs E)
			{
			SetControlStates();
			}

		private void OnAddCommandButtonClicked(Object Sender, RoutedEventArgs E)
			{
			var Command = ((CUModelCommand) UModelCommandsDataGrid.SelectedItem).Command;
			UModelLauncher.CommandLine += Command + " ";
			}

		private void OnFileTreeViewSelectedItemChanged(Object Sender, RoutedPropertyChangedEventArgs<Object> E)
		{
		SetControlStates();
		}

		private void OnAddFilesButtonClicked(Object Sender, RoutedEventArgs E)
			{
			var Path= ((CDirTreeItem)FileTreeView.SelectedItem).Path;
			UModelLauncher.BuildPath(Path);
			}
	}
	}
