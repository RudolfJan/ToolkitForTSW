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
      SetItemsSource();
      SetControlStates();
      }

    private void SetItemsSource()
      {
      //RadioStationsUrlDataGrid.ItemsSource = RailWayRadioStationManager.RadioStationDataSet.Tables[0].DefaultView;
      RadioStationsUrlDataGrid.Items.Refresh();
      }

    private void SetControlStates()
      {
      DeleteUrlButton.IsEnabled= RadioStationsUrlDataGrid.SelectedItem != null;
      UpdateUrlButton.IsEnabled= RadioStationsUrlDataGrid.SelectedItem != null;
      TestUrlButton.IsEnabled= RadioStationsUrlDataGrid.SelectedItem != null;
      }

    private void OnRadioStationsUrlDataGridSelectionChanged(Object Sender, SelectionChangedEventArgs E)
      {
      SetControlStates();
      }

    private void OnAddUrlButtonClicked(Object Sender, RoutedEventArgs E)
      {
      RailWayRadioStationManager.AddRadioStation(UrlTextBox.TextBoxText, RouteTextBox.TextBoxText, DescriptionTextBox.TextBoxText);
      RadioStationsUrlDataGrid.SelectedItem = null;
      SetControlStates();
      }

    private void OnUpdateUrlButtonClicked(Object Sender, RoutedEventArgs E)
      {
      var Selection = (RadioStationModel)RadioStationsUrlDataGrid.SelectedItem;

      // Update database
      Selection.Url = UrlTextBox.TextBoxText;
      RailWayRadioStationManager.UpdateRadioStation(Selection);
      }

    private void OnDeleteUrlButtonClicked(Object Sender, RoutedEventArgs E)
      {
      var Selection = (RadioStationModel)RadioStationsUrlDataGrid.SelectedItem;
      RailWayRadioStationManager.DeleteRadioStation(Selection.Id);
      SetControlStates();
      }

    private void OnTestUrlButtonClicked(Object Sender, RoutedEventArgs E)
      {
      var Selection = (RadioStationModel)RadioStationsUrlDataGrid.SelectedItem;
      CApps.LaunchUrl(Selection.Url, true);
      }

    private void OnOKButtonClicked(Object Sender, RoutedEventArgs E)
      {
      Close();
      }
    }
  }
