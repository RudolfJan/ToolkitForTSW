using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TreeBuilders.Library.Wpf;


namespace ToolkitForTSW
	{
	/// <summary>
	/// Interaction logic for FormRouteGuides.xaml
	/// </summary>
	public partial class FormRouteGuides
		{
		public FileTreeViewModel Tree { get; set; }

		public FormRouteGuides(string rootFolder)
			{
			InitializeComponent();
			FileTreeViewControl.FolderImage = "Images\\folder.png";
			FileTreeViewControl.FileImage = "Images\\file_extension_doc.png";
			FileTreeViewControl.SetImages();
			Tree = new FileTreeViewModel(rootFolder);
			FileTreeBuilder.RenameFilesToUnquoted(Tree.FileTree);
			FileTreeViewControl.Tree = Tree;
			FileTreeViewControl.DataContext = Tree;
			SetControlStates();
			}


    private void SetControlStates()
			{
			}


		private void OnOKButtonClicked(Object Sender, RoutedEventArgs E)
			{
			Close();
			}
	}
}