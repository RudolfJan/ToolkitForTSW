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

namespace ToolkitForTSW.Settings
  {

  public partial class EngineIniView
    {
    public EngineIniViewModel SettingsView {get;set; }
    public EngineIniView()
      {
      InitializeComponent();
      SettingsView= new EngineIniViewModel();
      SettingsView.Init();
      DataContext=SettingsView;
      }

    private void SetControlStates()
      {
      EditWorkSet.IsEnabled= SettingsView.SelectedEngineIniWorkSet!=null;
      DeleteWorkSet.IsEnabled= SettingsView.SelectedEngineIniWorkSet != null;
      AddSetting.IsEnabled= SettingsView.SelectedEngineIniSettings!=null;
      RemoveSetting.IsEnabled=SettingsView.SelectedSettingInWorkSet!=null;
      }

    private void OKButton_Click(object sender, RoutedEventArgs e)
      {
      Close();
      }

    private void EditWorkSet_Click(object sender, RoutedEventArgs e)
      {
      SettingsView.EditWorkSet();
      SetControlStates();
      }

    private void DeleteWorkSet_Click(object sender, RoutedEventArgs e)
      {
      SettingsView.DeleteWorkSet();
      SetControlStates();
      }

    private void SaveWorkSet_Click(object sender, RoutedEventArgs e)
      {
      SettingsView.SaveWorkSet();
      SetControlStates();
      }

    private void ClearWorkSet_Click(object sender, RoutedEventArgs e)
      {
      SettingsView.ClearWorkSet();
      SetControlStates();
      }

    private void AddSetting_Click(object sender, RoutedEventArgs e)
      {
      SettingsView.AddSetting();
      SetControlStates();
      }

    private void RemoveSetting_Click(object sender, RoutedEventArgs e)
      {
      SettingsView.RemoveSetting();
      SetControlStates();
      }

    private void WorkSetsDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
      SetControlStates();
      }

    private void EngineIniSettingsDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
      SetControlStates();
      }

    private void SelectedWorkSetDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
      SetControlStates();
      }

    private void Filter_TextChanged(object sender, RoutedEventArgs e)
      {
      SettingsView.FilterChanged();
      }
    }
  }
