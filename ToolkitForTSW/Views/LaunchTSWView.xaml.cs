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
    public LaunchTSWViewModel LaunchTSW { get; set; }

    public LaunchTSWView()
      {
      InitializeComponent();

      LaunchTSW = new LaunchTSWViewModel();
      DataContext = LaunchTSW;
      }
 
    private void OnSettingFilesDataGridSelectionChanged(Object Sender, SelectionChangedEventArgs E)
      {
      }

    private void LaunchTSWButton_Click(Object Sender, RoutedEventArgs E)
      {
      DirectoryInfo MySaveSet = null;
      if (SettingFilesDataGrid.SelectedItem != null)
        {
        MySaveSet =
          (DirectoryInfo) SettingFilesDataGrid.SelectedItem;
        }

      var Selection = (RadioStationModel) RadioStationsUrlDataGrid.SelectedItem;
      if (Selection != null)
        {
        LaunchTSW.RadioUrl = Selection.Url;
        }
      LaunchTSW.LaunchPrograms(MySaveSet);
      }

    private void OKButton_Click(Object Sender, RoutedEventArgs E)
      {
      Close();
      }

    private void OnSetModsButtonClicked(Object Sender, RoutedEventArgs E)
      {
      var ModManager = new CModManager();
      var Form = new FormModManager(ModManager);
      Form.ShowDialog();
      }

    private void OnActivateSelectedModSet(object sender, RoutedEventArgs e)
      {
      CModSet.ActivateModSet(LaunchTSW.ModSet.SelectedModSet);
      }
    }
  }
