using Styles.Library.Helpers;
using System;
using System.Collections.ObjectModel;
using ToolkitForTSW.DataAccess;
using ToolkitForTSW.Models;

namespace ToolkitForTSW
  {

  public class CRailwayRadioStationManager : Notifier
    {
    private ObservableCollection<RadioStationModel> _RadioStationList;
    public ObservableCollection<RadioStationModel> RadioStationList
      {
      get { return _RadioStationList; }
      set
        {
        _RadioStationList = value;
        OnPropertyChanged("RadioStationList");
        }
      }

    private String _Result = String.Empty;

    public String Result
      {
      get { return _Result; }
      set
        {
        _Result = value;
        OnPropertyChanged("Result");
        }
      }

    public CRailwayRadioStationManager()
      {
      Initialize();
      }

    public void Initialize()
      {
      RadioStationList =
        new ObservableCollection<RadioStationModel>(RadioStationDataAccess.GetAllRadioStations());
      }

    // Note: FieldList must have format  Field="Value", Field2="Value2"
    public void UpdateRadioStation(RadioStationModel radioStation)
      {
      RadioStationDataAccess.UpdateRadioStation(radioStation);
      }

    public void AddRadioStation(String MyUrl, String MyRoute, String MyDescription)
      {
      var newRadioStation = new RadioStationModel
        {
        Url = MyUrl,
        RouteName = MyRoute,
        Description = MyDescription
        };
      RadioStationDataAccess.InsertRadioStation(newRadioStation);
      }

    public void DeleteRadioStation(int Id)
      {
      RadioStationDataAccess.DeleteRadioStation(Id);
      }
    }
  }
