using TreeBuilders.Library.Wpf.ViewModels;

namespace ToolkitForTSW.Views
  {

  public partial class RouteGuideView
    {
    public FileTreeViewModel ViewModel { get; set; }
    public RouteGuideView()
      {
      InitializeComponent();
      ViewModel ??= new FileTreeViewModel();

      DataContext = ViewModel;
      FileTreeViewControl.Tree = ViewModel;
      FileTreeViewControl.DataContext = ViewModel;
      }
    }
  }