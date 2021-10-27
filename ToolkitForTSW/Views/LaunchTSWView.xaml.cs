using System.Windows;
using ToolkitForTSW.Mod.ViewModels;
using ToolkitForTSW.ViewModels;

namespace ToolkitForTSW.Views
  {
  public partial class LaunchTSWView
    {
    public LaunchTSWView()
      {
      InitializeComponent();
      }
 
    private void OnActivateSelectedModSet(object sender, RoutedEventArgs e)
      {
      var viewModel=DataContext as LaunchTSWViewModel;
      if (viewModel.ModSet.SelectedModSet != null)
        { 
        ModSetViewModel.ActivateModSet(viewModel.ModSet.SelectedModSet,TSWOptions.CurrentPlatform);
        }
      }
    }
  }
