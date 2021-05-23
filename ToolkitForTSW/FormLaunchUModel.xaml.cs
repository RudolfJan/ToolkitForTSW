using System;
using System.Windows;
using System.Windows.Controls;

namespace ToolkitForTSW
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
			FileTreeViewControl.FolderImage = "Images\\folder.png";
			FileTreeViewControl.FileImage = "Images\\file_extension_doc.png";
			FileTreeViewControl.SetImages();
			
			FileTreeViewControl.Tree = UModelLauncher.FileTree;
			FileTreeViewControl.DataContext = UModelLauncher.FileTree;
			SetControlStates();
			}

		private void SetControlStates()
			{
			AddCommandButton.IsEnabled = UModelCommandsDataGrid.SelectedItem != null;
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

		private void OnAddFilesButtonClicked(Object Sender, RoutedEventArgs E)
			{
			if(FileTreeViewControl.Tree.SelectedFileNode!=null)
				{
				var Path= FileTreeViewControl.Tree.SelectedFileNode.FileEntry.FullName;
				UModelLauncher.BuildPath(Path);
				}
			}
	}
	}
