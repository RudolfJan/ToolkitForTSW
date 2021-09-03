using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using ToolkitForTSW.Mod;
using ToolkitForTSW.Models;
using ToolkitForTSW.ViewModels;

namespace ToolkitForTSW.Views
  {
  public partial class LaunchTSWView
    {

    public LaunchTSWView()
      {
      InitializeComponent();
      }
 
    private void OnSetModsButtonClicked(Object Sender, RoutedEventArgs E)
      {
      var ModManager = new CModManager();
      var Form = new FormModManager(ModManager);
      Form.ShowDialog();
      }

    private void OnActivateSelectedModSet(object sender, RoutedEventArgs e)
      {
      var viewModel=DataContext as LaunchTSWViewModel;
      if (viewModel.ModSet.SelectedModSet != null)
        { 
        CModSet.ActivateModSet(viewModel.ModSet.SelectedModSet,TSWOptions.CurrentPlatform);
        }
      }
    }
  }
