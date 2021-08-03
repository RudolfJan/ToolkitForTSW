using TreeBuilders.Library.Wpf.ViewModels;

namespace ToolkitForTSW.Views
  {

  public partial class RouteGuideView
		{
		FileTreeViewModel ViewModel { get;set; }
		public RouteGuideView()
			{
			InitializeComponent();
			var rootFolder = $"{TSWOptions.ManualsFolder}RouteGuides\\";
			ViewModel = new FileTreeViewModel( rootFolder);
			DataContext = ViewModel;
			FileTreeViewControl.FolderImage = "Images\\folder.png";
			FileTreeViewControl.FileImage = "Images\\file_extension_doc.png";
			FileTreeViewControl.SetImages();

			FileTreeViewControl.Tree = ViewModel;
			FileTreeViewControl.DataContext = ViewModel;
			//SetControlStates();
			}

    private void Close_Click(object sender, System.Windows.RoutedEventArgs e)
      {
			Close();
      }
    }
}