using System;
using System.Windows;
using System.Windows.Controls;
using ToolkitForTSW.Models;

namespace ToolkitForTSW
  {
  /// <summary>
  /// Interaction logic for Form_RadioStationManager.xaml
  /// </summary>
  public partial class FormRadioStationManager
    {
    public CRailwayRadioStationManager RailWayRadioStationManager { get; set; }

    public FormRadioStationManager()
      {
      InitializeComponent();
      RailWayRadioStationManager=new CRailwayRadioStationManager();
      DataContext = RailWayRadioStationManager;
      SetControlStates();
      }

   
    private void SetControlStates()
      {
      DeleteUrlButton.IsEnabled= RadioStationsUrlDataGrid.SelectedItem != null;
      EditUrlButton.IsEnabled= RadioStationsUrlDataGrid.SelectedItem != null;
      TestUrlButton.IsEnabled= RadioStationsUrlDataGrid.SelectedItem != null;
      }

    private void OnRadioStationsUrlDataGridSelectionChanged(Object Sender, SelectionChangedEventArgs E)
      {
      SetControlStates();
      }

    private void OnEditUrlButtonClicked(Object Sender, RoutedEventArgs E)
      {
      RailWayRadioStationManager.EditRadioStation();
      RadioStationsUrlDataGrid.Items.Refresh();
      SetControlStates();
      }

    private void OnSaveUrlButtonClicked(Object Sender, RoutedEventArgs E)
      {
      RailWayRadioStationManager.SaveRadioStation();
      RadioStationsUrlDataGrid.Items.Refresh();
      }

    private void OnDeleteUrlButtonClicked(Object Sender, RoutedEventArgs E)
      {
      RailWayRadioStationManager.DeleteRadioStation();
      RadioStationsUrlDataGrid.Items.Refresh();
      SetControlStates();
      }

    private void OnTestUrlButtonClicked(Object Sender, RoutedEventArgs E)
      {
      RailWayRadioStationManager.TestUrl();
      }

    private void OnOKButtonClicked(Object Sender, RoutedEventArgs E)
      {
      Close();
      }

    private void OnClearButtonClicked(object sender, RoutedEventArgs e)
      {
      RailWayRadioStationManager.ClearRadioStation();
      }
    }
  }
