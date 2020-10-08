using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TSWTools
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
      RadioStationsUrlDataGrid.ItemsSource = RailWayRadioStationManager.RadioStationDataSet.Tables[0].DefaultView;
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
      RailWayRadioStationManager.Initialize(); //TODO make this more effective, this is clumsy code
      SetItemsSource();
      SetControlStates();
      }

    private void OnUpdateUrlButtonClicked(Object Sender, RoutedEventArgs E)
      {
      var Selection = (DataRowView)RadioStationsUrlDataGrid.SelectedItem;

      // Update database
      var FieldList = "Url=\"" + UrlTextBox.TextBoxText + "\"";
      FieldList += ", Description=\"" + CApps.DoubleQuotes(DescriptionTextBox.TextBoxText) + "\"";
      FieldList += ", Route=\"" + RouteTextBox.TextBoxText + "\"";
      RailWayRadioStationManager.UpdateRadioStationTableFields((Int64)Selection.Row[0], FieldList);
      }

    private void OnDeleteUrlButtonClicked(Object Sender, RoutedEventArgs E)
      {
      var Selection = (DataRowView)RadioStationsUrlDataGrid.SelectedItem;
      RailWayRadioStationManager.DeleteRadioStation((Int64)Selection.Row[0]);
      RailWayRadioStationManager.Initialize();
      SetItemsSource();
      SetControlStates();
      }

    private void OnTestUrlButtonClicked(Object Sender, RoutedEventArgs E)
      {
      var Selection = (DataRowView)RadioStationsUrlDataGrid.SelectedItem;
      CApps.LaunchUrl((String)Selection.Row[1], true);
      }

    private void OnOKButtonClicked(Object Sender, RoutedEventArgs E)
      {
      Close();
      }
    }
  }
