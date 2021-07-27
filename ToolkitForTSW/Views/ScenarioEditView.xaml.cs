using SavCracker.Library.Models;
using System.Windows;
using System.Windows.Controls;
using ToolkitForTSW.ViewModels;

namespace ToolkitForTSW.Views
  {

  public partial class ScenarioEditView
    {

    public ScenarioEditView()
      {
      InitializeComponent();
      // Make this checkbox read only
      IsToolkitCreatedCheckBox.IsHitTestVisible = false;
      IsToolkitCreatedCheckBox.Focusable = false;
      }
  
     // https://stackoverflow.com/questions/58881309/is-there-a-way-to-prevent-the-change-of-the-selected-row-in-a-wpf-datagrid
    private void OnPreviewServicesDataGridMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
      {
      // If the stopLocationList is changed, you should not be able to select another service, before this is resolved. 
      //if(ScenarioCopy.IsStopLocationListChanged)
      //  { 
      //  e.Handled = true;
      //  }
      }
    }
  }
