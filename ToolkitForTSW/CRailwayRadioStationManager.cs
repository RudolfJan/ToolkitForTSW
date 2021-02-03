using Styles.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using ToolkitForTSW.DataAccess;
using ToolkitForTSW.Models;

namespace ToolkitForTSW
  {

  public class CRailwayRadioStationManager : Notifier
    {
    private List<RadioStationModel> _RadioStationList;
    public List<RadioStationModel> RadioStationList
      {
      get { return _RadioStationList; }
      set
        {
        _RadioStationList = value;
        OnPropertyChanged("RadioStationList");
        }
      }

    private RadioStationModel _SelectedRadioStation;
    public RadioStationModel SelectedRadioStation
      {
      get { return _SelectedRadioStation; }
      set
        {
        _SelectedRadioStation = value;
        OnPropertyChanged("SelectedRadioStation");
        }
      }

    private int _radioStationId = 0;

    private string _Url;
    public string Url
      {
      get { return _Url; }
      set
        {
        _Url = value;
        OnPropertyChanged("Url");
        }
      }

    private string _RouteName;
    public string RouteName
      {
      get { return _RouteName; }
      set
        {
        _RouteName = value;
        OnPropertyChanged("RouteName");
        }
      }

    private string _Description;
    public string Description
      {
      get { return _Description; }
      set
        {
        _Description = value;
        OnPropertyChanged("Description");
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
      RadioStationList = RadioStationDataAccess.GetAllRadioStations();
      }

    // Note: FieldList must have format  Field="Value", Field2="Value2"
    public void SaveRadioStation()
      {

      if (_radioStationId == 0)
        {
        var newRadioStation = new RadioStationModel
          {
          Url = Url,
          RouteName = RouteName,
          Description = Description
          };
        newRadioStation.Id = RadioStationDataAccess.InsertRadioStation(newRadioStation);
        RadioStationList.Add(newRadioStation);
        }
      else
        {
        SelectedRadioStation.Url=Url;
        SelectedRadioStation.RouteName=RouteName;
        SelectedRadioStation.Description=Description;
        RadioStationDataAccess.UpdateRadioStation(SelectedRadioStation);
        }

      ClearRadioStation();
      }

    public void EditRadioStation()
      {
      Url=SelectedRadioStation.Url;
      RouteName=SelectedRadioStation.RouteName;
      Description = SelectedRadioStation.Description;
      _radioStationId= SelectedRadioStation.Id;
      }

    public void ClearRadioStation()
      {
      Url=string.Empty;
      RouteName = string.Empty;
      Description = string.Empty;
      _radioStationId = 0;
      SelectedRadioStation=null;
      }

    public void DeleteRadioStation()
      {
      RadioStationDataAccess.DeleteRadioStation(SelectedRadioStation.Id);
      RadioStationList.Remove(SelectedRadioStation);
      }

    internal void TestUrl()
      {
      CApps.LaunchUrl(SelectedRadioStation.Url, true);
      }
    }
  }
