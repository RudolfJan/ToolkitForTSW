using SavCracker.Library;
using SavCracker.Library.Models;
using System.Windows;
using System.Windows.Controls;

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
      HexButton.IsEnabled = ScenarioDataGrid.SelectedItem != null;
      PublishButton.IsEnabled = ScenarioDataGrid.SelectedItem != null;
      EditButton.IsEnabled= ScenarioDataGrid.SelectedItem != null;
      }

    private void OnExitButtonClicked(object sender, RoutedEventArgs e)
      {
      Close();
      }

    private void OnSelectedScenarioChanged(object sender, SelectionChangedEventArgs e)
      {
      ScenarioManager.SelectedSavScenario= (CScenario) ScenarioDataGrid.SelectedItem;
      if (ScenarioManager.SelectedSavScenario != null)
        {
        ScenarioManager.ScenarioIssueList =
          ScenarioProblemTracker.FindScenarioProblems(ScenarioManager.SelectedSavScenario
            .SavScenario);
        }

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

    private void EditButton_Click(object sender, RoutedEventArgs e)
      {
      CScenario cloneSource = ScenarioManager.SelectedSavScenario;
      var Form = new FormScenarioEdit(cloneSource);
      Form.ShowDialog();
      ScenarioManager.BuildScenarioList();
      ScenarioManager.SelectedSavScenario = null; // TODO refresh works but this feels clumsy ...
      SetControlStates();
      }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
      {
      CScenario toBeDeleted = ScenarioManager.SelectedSavScenario;
      ScenarioManager.ScenarioDelete(toBeDeleted);
      SetControlStates();
      }

 

    private void OnHexButtonClicked(object sender, RoutedEventArgs e)
      {
      CApps.OpenGenericFile(ScenarioManager.SelectedSavScenario.ScenarioFile.FullName);
      }
    }
  }
