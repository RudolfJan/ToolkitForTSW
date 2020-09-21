using System;
using System.Data;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace TSWTools
  {
  /// <summary>
  /// Interaction logic for FormLiveryManager.xaml
  /// </summary>
  public partial class FormLiveryManager
    {
    public CLiveryManager LiveryManager { get; set; }
    private Boolean _HasChanged;

    public FormLiveryManager(CLiveryManager MyLiveryManager)
      {
      InitializeComponent();
      Contract.Assert(MyLiveryManager != null);
      LiveryManager = MyLiveryManager;
      DataContext = LiveryManager;

      // var WinHeight = SystemParameters.MaximizedPrimaryScreenHeight * 0.85;
      //var AllowedHeightFactor = 0.3;

      AvailablePaksDataGrid.ItemsSource = LiveryManager.LiveryDataSet.Tables[0].DefaultView;
      AvailablePaksDataGridSets.ItemsSource = LiveryManager.LiveryDataSet.Tables[0].DefaultView;
      AvailablePaksDataGridInstaller.ItemsSource =
        LiveryManager.LiveryDataSet.Tables[0].DefaultView;
      InstalledPaksDataGrid.Height = AvailablePaksDataGrid.Height;
      PakfileTextBox.IsEnabled = false;
      IsInstalledCheckBox.IsEnabled = false;
      SetControlStates();
      }

    private void SetControlStates()
      {
      UpdateLiveryButton.IsEnabled = _HasChanged && AvailablePaksDataGrid.SelectedItem != null;
      LiveryNameTextBox.IsEnabled = AvailablePaksDataGrid.SelectedItem != null;
      SourceTextBox.IsEnabled = AvailablePaksDataGrid.SelectedItem != null;
      ReplaceNameTextBox.IsEnabled = AvailablePaksDataGrid.SelectedItem != null;
      LiveryComboBox.IsEnabled = AvailablePaksDataGrid.SelectedItem != null;
      DescriptionTextBox.IsEnabled = AvailablePaksDataGrid.SelectedItem != null;
      InstallPakButton.IsEnabled = AvailablePaksDataGridInstaller.SelectedItem != null;
      UnInstallPakButton.IsEnabled = AvailablePaksDataGridInstaller.SelectedItem != null;
      RemoveFromSetPakButton.IsEnabled = LiveryPakSetsDataGrid.SelectedItem != null;
      AddToSetPakButton.IsEnabled = AvailablePaksDataGridSets.SelectedItem != null;
      DeleteLiveryButton.IsEnabled = false;
      RefreshLiveryButton.IsEnabled = false;
      }
  
    private void OnOKButtonClicked(Object Sender, RoutedEventArgs E)
      {
      Close();
      }

    // Update the selected record in the available liveries
    private void OnUpdateLiveryButtonClicked(Object Sender, RoutedEventArgs E)
      {
      var Selection = (DataRowView) AvailablePaksDataGrid.SelectedItem;

      // Update database
      var FieldList = "Name=\"" + LiveryNameTextBox.TextBoxText + "\"";
      FieldList += ", Description=\"" + CApps.DoubleQuotes(DescriptionTextBox.TextBoxText) + "\"";
      FieldList += ", Source=\"" + SourceTextBox.TextBoxText + "\"";
      FieldList += ", LiveryType=\"" + LiveryComboBox.Text + "\"";
      FieldList += ", ReplaceName=\"" + ReplaceNameTextBox.TextBoxText + "\"";
      FieldList += ", DLCName=\"" + DLCNameTextBox.TextBoxText + "\"";
      LiveryManager.UpdateLiveryTableFields((Int64) Selection.Row[0], FieldList);
      _HasChanged = false;
      SetControlStates();
      }

    private void OnAvailablePaksDataGridSelectionChanged(Object Sender, SelectionChangedEventArgs E)
      {
      SetControlStates();
      }

    private void OnLiveryNameTextBoxTextChanged(Object Sender, RoutedEventArgs E)
      {
      _HasChanged = true;
      SetControlStates();
      }

    private void OnSourceTextBoxTextChanged(Object Sender, RoutedEventArgs E)
      {
      _HasChanged = true;
      SetControlStates();
      }

    private void OnReplaceNameTextBoxTextChanged(Object Sender, RoutedEventArgs E)
      {
      _HasChanged = true;
      SetControlStates();
      }

    private void OnLiveryComboBoxSelectionChanged(Object Sender, SelectionChangedEventArgs E)
      {
      _HasChanged = true;
      SetControlStates();
      }

    private void OnDescriptionTextBoxTextChanged(Object Sender, RoutedEventArgs E)
      {
      _HasChanged = true;
      SetControlStates();
      }

    private void OnInstallPakButtonClicked(Object Sender, RoutedEventArgs E)
      {
      LiveryManager.InstallPak((String) ((DataRowView) AvailablePaksDataGridInstaller.SelectedItem).Row[2]);
      var FieldList = "IsInstalled=1";
      LiveryManager.UpdateLiveryTableFields(
        (Int64) ((DataRowView) AvailablePaksDataGridInstaller.SelectedItem).Row[0], FieldList);
      ((DataRowView)AvailablePaksDataGridInstaller.SelectedItem).Row[8] = 1;
      SetControlStates();
      }

    private void OnUnInstallPakButtonClicked(Object Sender, RoutedEventArgs E)
      {
      LiveryManager.UnInstallPak(
        (String) ((DataRowView) AvailablePaksDataGridInstaller.SelectedItem).Row[9]);
      var FieldList = "IsInstalled=0";
      LiveryManager.UpdateLiveryTableFields(
        (Int64)((DataRowView)AvailablePaksDataGridInstaller.SelectedItem).Row[0], FieldList);
      ((DataRowView)AvailablePaksDataGridInstaller.SelectedItem).Row[8] = 0; // Update the dataset explicitly
      SetControlStates();
      }

    private void OnDLCNameTextBoxTextChanged(Object Sender, RoutedEventArgs E)
      {
      _HasChanged = true;
      SetControlStates();
      }

    private void OnCreateSetButtonClicked(Object Sender, RoutedEventArgs E)
      {
      // LiveryManager.LiverySet.SetName = LiveryNameTextBox.TextBoxText;
      LiveryManager.LiverySet.SaveLiverySetAsFile();
      }

    private void OnAddToSetPakButtonClicked(Object Sender, RoutedEventArgs E)
      {
      if (AvailablePaksDataGridSets.SelectedItem != null)
        {
        var PakName =
          (String) ((DataRowView) AvailablePaksDataGridSets.SelectedItem).Row.ItemArray[2];
        LiveryManager.LiverySet.AddLiveryPak(PakName);
        }
      }

    private void OnLoadSetButtonClicked(Object sender, RoutedEventArgs e)
      {
      var Dial = new OpenFileDialog
        {
        Filter = "Livery set files (*.xml)|*.xml|All files (*.*)|*.*",
        InitialDirectory = CTSWOptions.LiveriesFolder,
        ShowReadOnly = true,
        Title = "Open livery set file",
        CheckFileExists = true
        };
      if (Dial.ShowDialog() == true)
        {
        LiveryManager.LiverySet.OpenLiverySetFile(Dial.FileName);
        }
      }

    private void OnRemoveFromSetPakButtonClicked(Object sender, RoutedEventArgs e)
      {
      if (LiveryPakSetsDataGrid.SelectedItem == null) return;
      var PakName = (String) LiveryPakSetsDataGrid.SelectedItem;
      LiveryManager.LiverySet.RemoveLiveryPak(PakName);
      SetControlStates();
      }

    private void OnLiveryPakSetsDataGridSelectionChanged(Object sender, SelectionChangedEventArgs e)
      {
      SetControlStates();
      }

    private void OnAvailablePaksDataGridSetsSelectionChanged(Object sender,
      SelectionChangedEventArgs e)
      {
      SetControlStates();
      }

    private void OnDeleteSetButtonClicked(Object sender, RoutedEventArgs e)
      {
      var Dial = new OpenFileDialog
        {
        Filter = "Livery set files (*.xml)|*.xml|All files (*.*)|*.*",
        InitialDirectory = CTSWOptions.LiveriesFolder,
        ShowReadOnly = true,
        Title = "Delete livery set file",
        CheckFileExists = true
        };
      if (Dial.ShowDialog() == true)
        {
        LiveryManager.Result += CApps.DeleteSingleFile(Dial.FileName);
        }
      }
    }
  }
