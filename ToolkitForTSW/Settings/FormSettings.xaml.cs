using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ToolkitForTSW.Settings
  {
 
  public partial class FormSettings
    {
    public CSettingsManager SettingsManager { get; set; }

    public FormSettings()
      {
      InitializeComponent();
      SettingsManager = new CSettingsManager();
      DataContext = SettingsManager;
      SortDataGrid(VideoModesDataGrid, 0, ListSortDirection.Descending);
      SortDataGrid(VideoModesDataGrid, 2, ListSortDirection.Descending, true);
      SetControlStates();
      }

    private void SetControlStates()
      {
      //SaveSettings.IsEnabled = SettingsManager.CurrentUserSettingsFile != null;
      SetScreenResButton.IsEnabled = VideoModesDataGrid.SelectedItem != null;
      UpdateSaveSetButton.IsEnabled = SaveSetNameTextBox.Text.Length > 2;
      LoadSaveSetSettingsButton.IsEnabled = SettingFilesDataGrid.SelectedItem != null;
      SetRecommendedSoundButton.IsEnabled =
        SettingsManager != null && SettingsManager.SettingsSound != null;
      SetRecommendedAdvancedButton.IsEnabled =
        SettingsManager != null && SettingsManager.SettingsAdvanced != null;
      // View distance also is an advanced setting with a larger range.
      ViewDistanceComboBox.IsEnabled = UseAdvancedSettingsCheckBox.IsChecked == false;
      DeleteSaveSetButton.IsEnabled = SettingFilesDataGrid.SelectedItem != null;
      AddWorkSet.IsEnabled= SettingsManager?.SettingsExperimental?.SelectedWorkSet!=null;
      RemoveWorkSet.IsEnabled = SettingsManager?.SettingsExperimental?.SelectedWorkSet != null;
      }

    private void OnOKButtonClicked(Object Sender, RoutedEventArgs E)
      {
      Close();
      }

    private void OnLoadActiveGameSettingsClicked(Object Sender, RoutedEventArgs E)
      {
      SettingsManager.LoadSettingsInDictionary(CSettingsManager.GetInGameSettingsLocation(),
        CSettingsManager.GetInGameEngineIniLocation());
      SettingsManager.Init();
      SettingFilesDataGrid.Items.SortDescriptions.Add(new SortDescription(NameColumn.SortMemberPath,
        ListSortDirection.Ascending));
      SettingsDictionaryDataGrid.Items.SortDescriptions.Add(
        new SortDescription(KeyColumn.SortMemberPath, ListSortDirection.Ascending));
      SettingsDictionaryDataGrid.Items.SortDescriptions.Add(
        new SortDescription(SectionColumn.SortMemberPath, ListSortDirection.Ascending));
      SetControlStates();
      }

    public static void SortDataGrid(DataGrid MyDataGrid, Int32 ColumnIndex = 0,
      ListSortDirection MySortDirection = ListSortDirection.Ascending, Boolean Add = false)
      {
      var Column = MyDataGrid.Columns[ColumnIndex];

      // Clear current sort descriptions
      if (!Add)
        {
        MyDataGrid.Items.SortDescriptions.Clear();
        }

      // Add the new sort description
      MyDataGrid.Items.SortDescriptions.Add(new SortDescription(Column.SortMemberPath,
        MySortDirection));

      // Apply sort
      foreach (var Col in MyDataGrid.Columns)
        {
        Col.SortDirection = null;
        }

      Column.SortDirection = MySortDirection;

      // Refresh items to display sort
      MyDataGrid.Items.Refresh();
      }

    private void OnSaveSettingsClicked(Object Sender, RoutedEventArgs E)
      {
      var FileName = CSettingsManager.GetInGameSettingsLocation();
      SettingsManager.Update();
      SettingsManager.WriteSettingsInDictionary(FileName);
      FileName = CSettingsManager.GetInGameEngineIniLocation();
      SettingsManager.WriteSettingsInDictionary(FileName, true);
      SetControlStates();
      }

    private void OnUpdateButtonClicked(Object Sender, RoutedEventArgs E)
      {
      SettingsManager.Update();
      }

    private void OnSetRecommendedSoundButtonClicked(Object Sender, RoutedEventArgs E)
      {
      SettingsManager.SettingsSound.SetRecommendedValues();
      }

    private void OnSetScreenResButtonClicked(Object Sender, RoutedEventArgs E)
      {
      SettingsManager.SettingsScreen.ResolutionSizeX =
        ((VideoMode)VideoModesDataGrid.SelectedItem).Width;
      SettingsManager.SettingsScreen.ResolutionSizeY =
        ((VideoMode)VideoModesDataGrid.SelectedItem).Height;
      }

    private void OnVideoModesDataGridSelectionChanged(Object Sender, SelectionChangedEventArgs E)
      {
      SetControlStates();
      }

    private void OnLoadSaveSetSettingsButtonClicked(Object Sender, RoutedEventArgs E)
      {
      SettingsManager.LoadSaveSet();
      ValuesDatagrid.Items.Refresh();
      SetControlStates();
      }

    private void OnUpdateSaveSetButtonClicked(Object Sender, RoutedEventArgs E)
      {
      SettingsManager.WriteSettingsToSaveSet();
      }

    private void OnSaveSetNameTextBoxTextChanged(Object Sender, TextChangedEventArgs E)
      {
      SetControlStates();
      }

    private void OnSettingFilesDataGridSelectionChanged(Object Sender, SelectionChangedEventArgs E)
      {
      if (SettingFilesDataGrid.SelectedItem != null)
        {
        SettingsManager.SaveSetName = ((DirectoryInfo)SettingFilesDataGrid.SelectedItem).Name;
        }
      SetControlStates();
      }

    private void OnSetRecommendedAdvancedButtonClicked(Object Sender, RoutedEventArgs E)
      {
      SettingsManager.SettingsAdvanced.SetRecommendedValues();
      }

    private void UseAdvancedSettingsCheckBoxChanged(Object Sender, RoutedEventArgs E)
      {
      if (UseAdvancedSettingsCheckBox.IsChecked == true)
        {
        SettingsManager.UseAdvanced = true;
        }
      else
        {
        SettingsManager.UseAdvanced = false;
        }
      SetControlStates();
      TSWOptions.UseAdvancedSettings = SettingsManager.UseAdvanced;
      TSWOptions.WriteToRegistry();
      }

    private void OnMasterVolumeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
      {
      if (SettingsManager.SettingsSound == null)
        {
        return;
        }
      if (SettingsManager.SettingsSound.LimitVolume && MasterVolumeSlider.Value > 1.0)
        {
        MasterVolumeSlider.Value = 1.0;
        }
      }

    private void OmAmbientVolumeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
      {
      if (SettingsManager.SettingsSound == null)
        {
        return;
        }
      if (SettingsManager.SettingsSound.LimitVolume && AmbientVolumeSlider.Value > 1.0)
        {
        AmbientVolumeSlider.Value = 1.0;
        }
      }

    private void OnDialogSoundVolumeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
      {
      if (SettingsManager.SettingsSound == null)
        {
        return;
        }
      if (SettingsManager.SettingsSound.LimitVolume && DialogSoundVolumeSlider.Value > 1.0)
        {
        DialogSoundVolumeSlider.Value = 1.0;
        }
      }

    private void OnExternalAlertVolumeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
      {
      if (SettingsManager.SettingsSound == null)
        {
        return;
        }
      if (SettingsManager.SettingsSound.LimitVolume && ExternalAlertVolumeSlider.Value > 1.0)
        {
        ExternalAlertVolumeSlider.Value = 1.0;
        }
      }

    private void OnSFXSoundVolumeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
      {
      if (SettingsManager.SettingsSound == null)
        {
        return;
        }
      if (SettingsManager.SettingsSound.LimitVolume && SFXSoundVolumeSlider.Value > 1.0)
        {
        SFXSoundVolumeSlider.Value = 1.0;
        }
      }

    private void OnMenuSFXVolumeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
      {
      if (SettingsManager.SettingsSound == null)
        {
        return;
        }
      if (SettingsManager.SettingsSound.LimitVolume && MenuSFXVolumeSlider.Value > 1.0)
        {
        MenuSFXVolumeSlider.Value = 1.0;
        }
      }

    private void OnLimitVolumeChecked(object sender, RoutedEventArgs e)
      {
      if (SettingsManager.SettingsSound == null)
        {
        return;
        }
      SettingsManager.SettingsSound.LimitVolumes();
      }

    private void OnDeleteSaveSetBurttonClicked(object sender, RoutedEventArgs e)
      {
      var dir = ((DirectoryInfo)SettingFilesDataGrid.SelectedItem);
      dir.Delete(true);
      SettingsManager.GetSavedSettings();
      }

    private void EditWorkSets_Click(object sender, RoutedEventArgs e)
      {
      var form= new EngineIniView();
      form.ShowDialog();
      }

    private void WorkSetsDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
      SetControlStates();
      }

    private void RemoveWorkSet_Click(object sender, RoutedEventArgs e)
      {
      SettingsManager.SettingsExperimental.RemoveWorkSet();
      ValuesDatagrid.Items.Refresh();
      }

    private void AddWorkSet_Click(object sender, RoutedEventArgs e)
      {
      SettingsManager.SettingsExperimental.AddWorkSet();
      ValuesDatagrid.Items.Refresh();
      }

    private void ValuesDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {

      }
    }
  }
