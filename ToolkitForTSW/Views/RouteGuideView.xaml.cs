using TreeBuilders.Library.Wpf.ViewModels;

namespace ToolkitForTSW.Views
  {

  public partial class RouteGuideView
    {
    public FileTreeViewModel ViewModel { get;set; }
    public RouteGuideView()
      {
      InitializeComponent();
      if(ViewModel==null)
        {
        ViewModel = new FileTreeViewModel();
       }
      
      DataContext = ViewModel;
       FileTreeViewControl.Tree = ViewModel;
      FileTreeViewControl.DataContext = ViewModel;
      }
    }
}