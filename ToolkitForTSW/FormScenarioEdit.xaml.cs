using SavCrackerTest.Models;
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
    public CScenarioEdit CloneScenario { get; set; }

    public FormScenarioEdit(CScenario _scenario)
      {
      InitializeComponent();
      CloneScenario = new CScenarioEdit
        {
        Scenario = _scenario
        };
      CloneScenario.ScenarioEdit();
      DataContext = CloneScenario;
      SetControlStates();
      }

    private void SetControlStates()
      {
      OKButton.IsEnabled = ClonedScenarioNameTextBox.TextBoxText != null &&
                           ClonedScenarioNameTextBox.TextBoxText.Length >
                           3; // Cloned scenario MUST have a name
      EditService.IsEnabled= CloneScenario.SelectedService!=null;
      DeleteService.IsEnabled = CloneScenario.SelectedService!=null;
      CloneService.IsEnabled= CloneScenario.SelectedService!=null;
      }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
      {
      Close();
      }

    private void OKButton_OnClick(object sender, RoutedEventArgs e)
      {
      CloneScenario.SaveOverwrite();
      //CloneScenario.ClonedScenarioName = ClonedScenarioNameTextBox.TextBoxText;
      //CloneScenario.Clone();
      //System.Windows.MessageBox.Show("Scenario rebuilt successfully", "Clone scenario", MessageBoxButton.OK,
      //  MessageBoxImage.Information);
      Close();
      }

    private void SaveButton_OnClick(object sender, RoutedEventArgs e)
      {
      CloneScenario.SaveCopy();
      //System.Windows.MessageBox.Show("Scenario saved successfully", "Save scenario", MessageBoxButton.OK,
      //  MessageBoxImage.Information);
      Close();
      }

    private void ClonedScenarioNameTextBox_OnTextChanged(object sender, RoutedEventArgs e)
      {
      SetControlStates();
      }

    private void OnServiceEditClicked(object sender, RoutedEventArgs e)
      {
      CloneScenario.ServiceEdit();
      }

    private void OnServiceDeleteClicked(object sender, RoutedEventArgs e)
      {
      CloneScenario.ServiceDelete();
      }

    private void OnServiceCloneClicked(object sender, RoutedEventArgs e)
      {
      CloneScenario.ServiceClone();
      SetControlStates();
      }

    private void OnSelectedServiceChanged(object sender, SelectionChangedEventArgs e)
      {
      CloneScenario.SelectedService = (SavServiceModel) ServicesDataGrid.SelectedItem;
      SetControlStates();
      }

    private void OnSelectedStopLocationChanged(object sender, SelectionChangedEventArgs e)
      {
      CloneScenario.SelectedStopLocation = (string) StopLocationsDataGrid.SelectedItem;
      CloneScenario.StopLocation = CloneScenario.SelectedStopLocation;
      SetControlStates();
      }

    private void OnServiceSaveClicked(object sender, RoutedEventArgs e)
      {
      CloneScenario.ServiceSave();
      SetControlStates();
      }

    private void OnServiceClearClicked(object sender, RoutedEventArgs e)
      {
      CloneScenario.ServiceClear();
      SetControlStates();
      }
    }
  }
