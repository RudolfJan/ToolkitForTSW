using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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

public partial class FormCloneScenario
    {
    public CCloneScenario CloneScenario { get; set; }
    public FormCloneScenario(CScenario _scenario)
      {
      InitializeComponent();
      CloneScenario= new CCloneScenario();
      CloneScenario.Scenario = _scenario;
      DataContext = CloneScenario;
      SetControlStates();
      }
    private void SetControlStates()
      {
      OKButton.IsEnabled = ClonedScenarioNameTextBox.TextBoxText!=null && ClonedScenarioNameTextBox.TextBoxText.Length > 3; // Cloned scenario MUST have a name
      }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
      {
      Close();
      }

    private void OKButton_OnClick(object sender, RoutedEventArgs e)
      {
      CloneScenario.ClonedScenarioName = ClonedScenarioNameTextBox.TextBoxText;
      CloneScenario.Clone();
      MessageBox.Show("Scenario cloned successfully", "Clone scenario", MessageBoxButton.OK, MessageBoxImage.Information);
      Close();
      }

    private void ClonedScenarioNameTextBox_OnTextChanged(object sender, RoutedEventArgs e)
      {
      SetControlStates();
      }
    }
  }
