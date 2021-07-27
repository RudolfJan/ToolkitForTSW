using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Caliburn.Micro;
using ToolkitForTSW.DataAccess;
using ToolkitForTSW.Models;

namespace ToolkitForTSW.ViewModels
  {

  public class RadioStationsViewModel : Screen
    {
    private List<RadioStationModel> _RadioStationList;
    public List<RadioStationModel> RadioStationList
      {
      get { return _RadioStationList; }
      set
        {
        _RadioStationList = value;
        NotifyOfPropertyChange(() => RadioStationList);
        }
      }

    private RadioStationModel _SelectedRadioStation;
    public RadioStationModel SelectedRadioStation
      {
      get { return _SelectedRadioStation; }
      set
        {
        _SelectedRadioStation = value;
        NotifyOfPropertyChange(() => SelectedRadioStation);
        NotifyOfPropertyChange(() => CanDeleteRadioStation);
        NotifyOfPropertyChange(() => CanEditRadioStation);
        NotifyOfPropertyChange(() => CanTestUrl);
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
        NotifyOfPropertyChange(() => Url);
        NotifyOfPropertyChange(() => CanSaveRadioStation);
        NotifyOfPropertyChange(() => CanTestUrl);
        }
      }

    private string _RouteName;
    public string RouteName
      {
      get { return _RouteName; }
      set
        {
        _RouteName = value;
        NotifyOfPropertyChange(() => RouteName);
        NotifyOfPropertyChange(() => CanSaveRadioStation);
        }
      }

    private string _Description;
    public string Description
      {
      get { return _Description; }
      set
        {
        _Description = value;
        NotifyOfPropertyChange(() => Description);
        }
      }


    private String _Result = String.Empty;

    public String Result
      {
      get { return _Result; }
      set
        {
        _Result = value;
        NotifyOfPropertyChange(() => Result);
        }
      }

    public RadioStationsViewModel()
      {
      Initialize();
      }

    public void Initialize()
      {
      RadioStationList = RadioStationDataAccess.GetAllRadioStations();
      }

    public bool CanSaveRadioStation
      {
      get
        {
        return Url?.Length > 1 && RouteName?.Length > 2;
        }
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
        SelectedRadioStation.Url = Url;
        SelectedRadioStation.RouteName = RouteName;
        SelectedRadioStation.Description = Description;
        RadioStationDataAccess.UpdateRadioStation(SelectedRadioStation);
        }
      RadioStationList = RadioStationDataAccess.GetAllRadioStations();
      NotifyOfPropertyChange(() => RadioStationList);
      ClearRadioStation();
      }

    public bool CanEditRadioStation
      {
      get
        {
        return SelectedRadioStation != null;
        }
      }

    public void EditRadioStation()
      {
      Url = SelectedRadioStation.Url;
      RouteName = SelectedRadioStation.RouteName;
      Description = SelectedRadioStation.Description;
      _radioStationId = SelectedRadioStation.Id;
      }

    public void ClearRadioStation()
      {
      Url = string.Empty;
      RouteName = string.Empty;
      Description = string.Empty;
      _radioStationId = 0;
      SelectedRadioStation = null;
      }
    public bool CanDeleteRadioStation
      {
      get
        {
        return SelectedRadioStation != null;
        }
      }

    public void DeleteRadioStation()
      {
      RadioStationDataAccess.DeleteRadioStation(SelectedRadioStation.Id);
      RadioStationList.Remove(SelectedRadioStation);
      RadioStationList = RadioStationDataAccess.GetAllRadioStations();
      NotifyOfPropertyChange(() => RadioStationList);
      }

    public bool CanTestUrl
      {
      get
        {
        return SelectedRadioStation != null && SelectedRadioStation.Url.Length > 4;
        }
      }
    public void TestUrl()
      {
      CApps.LaunchUrl(SelectedRadioStation.Url, true);
      }

    public async Task Close()
      {
      await TryCloseAsync();
      }
    }
  }
