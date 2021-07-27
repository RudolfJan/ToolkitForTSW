using SavCracker.Library;
using SavCracker.Library.Models;
using System.Windows;
using System.Windows.Controls;
using Utilities.Library;

namespace ToolkitForTSW.Views
  {
  /// <summary>
  /// Interaction logic for FormScenarioManager.xaml
  /// </summary>
  public partial class ScenarioManagerView
    {
   
    public ScenarioManagerView()
      {
      InitializeComponent();
  
      }

     private void OnExitButtonClicked(object sender, RoutedEventArgs e)
      {
      Close();
      }

     //private void PublishButton_Click(object sender, RoutedEventArgs e)
     // {
     // CPublishScenario publish= new CPublishScenario(ScenarioManager.SelectedSavScenario.ScenarioFile.FullName, ScenarioManager.SelectedSavScenario.SavScenario);
     // var Form= new FormPublishScenario(publish);
     // Form.ShowDialog();
     // }

    //private void EditButton_Click(object sender, RoutedEventArgs e)
    //  {
    //  CScenario cloneSource = ScenarioManager.SelectedSavScenario;
    //  var Form = new FormScenarioEdit(cloneSource);
    //  Form.ShowDialog();
    //  ScenarioManager.BuildScenarioList();
    //  ScenarioManager.SelectedSavScenario = null; // TODO refresh works but this feels clumsy ...
    //  SetControlStates();
    //  }

 
 
    }
  }
