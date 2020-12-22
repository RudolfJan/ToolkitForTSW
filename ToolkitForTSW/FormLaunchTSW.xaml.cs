using System;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ToolkitForTSW
  {
  /// <summary>
  /// Interaction logic for FormLaunchTSW.xaml
  /// </summary>
  public partial class FormLaunchTSW
    {
    public CLaunchTSW LaunchTSW { get; set; }

    public FormLaunchTSW()
      {
      InitializeComponent();

      LaunchTSW = new CLaunchTSW();
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

      var Selection = (DataRowView) RadioStationsUrlDataGrid.SelectedItem;
      if (Selection != null)
        {
        LaunchTSW.RadioUrl = (String) Selection.Row[1];
        }

      var LiverySetFile = (FileInfo) LiverySetsDataGrid.SelectedItem;
      if (LiverySetFile != null)
        {
        var LiveryManager = new CLiveryManager();
        LiveryManager.RemoveAllInstalledPaks();
        LaunchTSW.LiverySet.InstallSets(LiverySetFile);
        LiveryManager.UpdateInstalledStates();
        }

      LaunchTSW.LaunchPrograms(MySaveSet);
      }

    private void OKButton_Click(Object Sender, RoutedEventArgs E)
      {
      Close();
      }

    private void OnLiveryButtonClicked(Object Sender, RoutedEventArgs E)
      {
      var LiveryManager = new CLiveryManager();
      var Form = new FormLiveryManager(LiveryManager);
      Form.ShowDialog();
      }
    }
  }
