﻿using SavCracker.Library.Models;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ToolkitForTSW
  {
  /// <summary>
  /// Interaction logic for FormCloneScenario.xaml
  /// </summary>
  ///
  ///
  // https://stackoverflow.com/questions/58717392/validation-rule-for-checking-if-text-has-only-ascii
  public class NameRule : ValidationRule
  {
  public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
    return value is string str && str.All(ch => ch < 128)
      ? ValidationResult.ValidResult
      : new ValidationResult(false, "The name contains illegal characters");
    }
  }

  public partial class FormScenarioEdit
    {
    public CScenarioEdit ScenarioCopy { get; set; }

    public FormScenarioEdit(CScenario _scenario)
      {
      InitializeComponent();
      ScenarioCopy = new CScenarioEdit
        {
        Scenario = _scenario
        };
      ScenarioCopy.ScenarioEdit();
      DataContext = ScenarioCopy;
      SetControlStates();
      }

    private void SetControlStates()
      {
      SaveCopyButton.IsEnabled = TargetScenarioNameTextBox.TextBoxText != null &&
                           TargetScenarioNameTextBox.TextBoxText.Length > 3; // Cloned scenario MUST have a name
      SaveOverWriteButton.IsEnabled= ScenarioCopy.IsToolkitCreated; // overwrite only allowed if scenario is generated by toolkit, otherwise you MUST make a copy first.
      EditService.IsEnabled= ScenarioCopy.SelectedService!=null;
      DeleteService.IsEnabled = ScenarioCopy.SelectedService!=null;
      CloneService.IsEnabled= ScenarioCopy.SelectedService!=null;
      MoveDownButton.IsEnabled= ScenarioCopy.SelectedStopLocation!=null;
      MoveUpButton.IsEnabled = ScenarioCopy.SelectedStopLocation != null;
      EditStopLocation.IsEnabled= ScenarioCopy.SelectedStopLocation != null;
      DeleteStopLocation.IsEnabled= ScenarioCopy.SelectedStopLocation != null;

      // Make this checkbox read only
      IsToolkitCreatedCheckBox.IsHitTestVisible=false;
      IsToolkitCreatedCheckBox.Focusable=false;
      }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
      {
      Close();
      }

    private void SaveOverWriteClicked(object sender, RoutedEventArgs e)
      {
      ScenarioCopy.SaveOverwrite();
      //CloneScenario.ClonedScenarioName = ClonedScenarioNameTextBox.TextBoxText;
      //CloneScenario.Clone();
      System.Windows.MessageBox.Show("Scenario overwritten, edited and rebuilt successfully", "Updated scenario", MessageBoxButton.OK,
        MessageBoxImage.Information);
      Close();
      }

    private void SaveCopyClicked(object sender, RoutedEventArgs e)
      {
      ScenarioCopy.SaveCopy();
      System.Windows.MessageBox.Show("Scenario copied, edited and rebuilt successfully", "Save changes as copy", MessageBoxButton.OK,
        MessageBoxImage.Information);
      Close();
      }

    private void TargetScenarioNameTextBox_OnTextChanged(object sender, RoutedEventArgs e)
      {
      SetControlStates();
      }

    private void OnServiceEditClicked(object sender, RoutedEventArgs e)
      {
      ScenarioCopy.ServiceEdit();
      }

    private void OnServiceDeleteClicked(object sender, RoutedEventArgs e)
      {
      ScenarioCopy.ServiceDelete();
      ServicesDataGrid.Items.Refresh();
      SetControlStates();
      }

    private void OnServiceCloneClicked(object sender, RoutedEventArgs e)
      {
      ScenarioCopy.ServiceClone();
      ServicesDataGrid.Items.Refresh();
      SetControlStates();
      }

    private void OnSelectedServiceChanged(object sender, SelectionChangedEventArgs e)
      {
      ScenarioCopy.SelectedService = (SavServiceModel) ServicesDataGrid.SelectedItem;
      SetControlStates();
      }

    private void OnSelectedStopLocationChanged(object sender, SelectionChangedEventArgs e)
      {
      ScenarioCopy.SelectedStopLocation = (string) StopLocationsDataGrid.SelectedItem;
      ScenarioCopy.StopLocation = ScenarioCopy.SelectedStopLocation;
      SetControlStates();
      }

    private void OnServiceSaveClicked(object sender, RoutedEventArgs e)
      {
      ScenarioCopy.ServiceSave();
      ServicesDataGrid.Items.Refresh();
      SetControlStates();
      }

    private void OnServiceClearClicked(object sender, RoutedEventArgs e)
      {
      ScenarioCopy.ServiceClear();
      SetControlStates();
      }

    private void MoveUpButtonClicked(object sender, RoutedEventArgs e)
      {
      ScenarioCopy.MoveUpStopPoint();
      StopLocationsDataGrid.Items.Refresh();
      SetControlStates();
      }

    private void MoveDownButtonClicked(object sender, RoutedEventArgs e)
      {
      ScenarioCopy.MoveDownStopPoint();
      StopLocationsDataGrid.Items.Refresh();
      SetControlStates();
      }

    private void OnEditStopLocationClicked(object sender, RoutedEventArgs e)
      { 
      ScenarioCopy.EditStopLocation();
      SetControlStates();
      }

    private void OnAddStopLocationClicked(object sender, RoutedEventArgs e)
      {
      ScenarioCopy.AddStopLocation();
      SetControlStates();
      }

    private void OnDeleteStopLocationClicked(object sender, RoutedEventArgs e)
      {
      ScenarioCopy.DeleteStopLocation();
      StopLocationsDataGrid.Items.Refresh();
      SetControlStates();
      }

    private void OnSaveStopLocationClicked(object sender, RoutedEventArgs e)
      {
      ScenarioCopy.SaveStopLocation();
      StopLocationsDataGrid.Items.Refresh();
      SetControlStates();
      }

    private void OnCancelStopLocationList(object sender, RoutedEventArgs e)
      {
      ScenarioCopy.RefreshStopLocationList();
      SetControlStates();
      }

    private void OnSaveStopLocationList(object sender, RoutedEventArgs e)
      {
      ScenarioCopy.SaveStopLocationList();
      SetControlStates();
      }

    // https://stackoverflow.com/questions/58881309/is-there-a-way-to-prevent-the-change-of-the-selected-row-in-a-wpf-datagrid
    private void OnPreviewServicesDataGridMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
      {
      // If the stopLocationList is changed, you should not be able to select another service, before this is resolved. 
      if(ScenarioCopy.IsStopLocationListChanged)
        { 
        e.Handled = true;
        }
      }
    }
  }
