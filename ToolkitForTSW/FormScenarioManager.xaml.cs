using SavCracker.Library;
using SavCrackerTest.Models;
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

namespace ToolkitForTSW
  {
  /// <summary>
  /// Interaction logic for FormScenarioManager.xaml
  /// </summary>
  public partial class FormScenarioManager
    {
    public CScenarioManager ScenarioManager { get; set; } = new CScenarioManager();
    
    public FormScenarioManager()
      {
      InitializeComponent();
      DataContext = ScenarioManager;
      SetControlStates();
      }

    private void SetControlStates()
      {
      PublishButton.IsEnabled = ScenarioDataGrid.SelectedItem != null;
      CloneButton.IsEnabled= ScenarioDataGrid.SelectedItem != null;
      }

    private void OnExitButtonClicked(object sender, RoutedEventArgs e)
      {
      Close();
      }

    private void OnSelectedScenarioChanged(object sender, SelectionChangedEventArgs e)
      {
      ScenarioManager.SelectedSavScenario= (CScenario) ScenarioDataGrid.SelectedItem;
      ScenarioManager.ScenarioIssueList = ScenarioProblemTracker.FindScenarioProblems(ScenarioManager.SelectedSavScenario.SavScenario);
      SetControlStates();
      }

    private void OnSelectedServiceChanged(object sender, SelectionChangedEventArgs e)
      {
       ScenarioManager.SelectedSavService = (SavServiceModel) ServicesDataGrid.SelectedItem;
      }

    private void PublishButton_Click(object sender, RoutedEventArgs e)
      {
      CPublishScenario publish= new CPublishScenario(ScenarioManager.SelectedSavScenario.ScenarioFile.FullName, ScenarioManager.SelectedSavScenario.SavScenario);
      var Form= new FormPublishScenario(publish);
      Form.ShowDialog();
      }

    private void CloneButton_Click(object sender, RoutedEventArgs e)
      {

      }
    }
  }
