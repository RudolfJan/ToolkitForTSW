using SavCracker.Library;
using SavCrackerTest.Models;
using Styles.Library.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using ToolkitForTSW.DataAccess;
using ToolkitForTSW.Models;
using ToolkitForTSW.Scenario;
using Utilities.Library.TextHelpers;

namespace ToolkitForTSW
  {

  public class CScenarioEdit : Notifier
    {
    private CScenario _scenario;
    public CScenario Scenario
      {
      get
        {
        return _scenario;
        }
      set
        {
        _scenario = value;
        OnPropertyChanged("Scenario");
        }
      }

    private string _ClonedScenarioName;
    public string ScenarioName
      {
      get { return _ClonedScenarioName; }
      set
        {
        _ClonedScenarioName = value;
        OnPropertyChanged("ClonedScenarioName");
        }
      }

    private Guid _ClonedScenarioGuid = Guid.NewGuid();
    public Guid ScenarioGuid
      {
      get { return _ClonedScenarioGuid; }
      set
        {
        _ClonedScenarioGuid = value;
        OnPropertyChanged("ClonedScenarioGuid");
        }
      }

    private string _ScenarioStartTime;
    public string ScenarioStartTime
      {
      get { return _ScenarioStartTime; }
      set
        {
        _ScenarioStartTime = value;
        OnPropertyChanged("ScenarioStartTime");
        }
      }

    private string _ServiceName;
    public string ServiceName
      {
      get { return _ServiceName; }
      set
        {
        _ServiceName = value;
        OnPropertyChanged("ServiceName");
        }
      }

    private string _ServiceStartTime;
    public string ServiceStartTime
      {
      get { return _ServiceStartTime; }
      set
        {
        _ServiceStartTime = value;
        OnPropertyChanged("ServiceStartTime");
        }
      }

    private string _StartLocation;
    public string StartLocation
      {
      get { return _StartLocation; }
      set
        {
        _StartLocation = value;
        OnPropertyChanged("StartLocation");
        }
      }

    private string _EndLocation;
    public string EndLocation
      {
      get { return _EndLocation; }
      set
        {
        _EndLocation = value;
        OnPropertyChanged("EndLocation");
        }
      }

    private string _EngineString;
    public string EngineString
      {
      get { return _EngineString; }
      set
        {
        _EngineString = value;
        OnPropertyChanged("EngineString");
        }
      }

    private string _ConsistString;
    public string ConsistString
      {
      get { return _ConsistString; }
      set
        {
        _ConsistString = value;
        OnPropertyChanged("ConsistString");
        }
      }

    private string _LiveryIdentifier;
    public string LiveryIdentifier
      {
      get { return _LiveryIdentifier; }
      set
        {
        _LiveryIdentifier = value;
        OnPropertyChanged("LiveryIdentifier");
        }
      }

    private bool _OffTheRailsMode;
    public bool OffTheRailsMode
      {
      get { return _OffTheRailsMode; }
      set
        {
        _OffTheRailsMode = value;
        OnPropertyChanged("OffTheRailsMode");
        }
      }

    private List<SavServiceModel> _ServicesList;
    public List<SavServiceModel> ServicesList
      {
      get { return _ServicesList; }
      set
        {
        _ServicesList = value;
        OnPropertyChanged("ServicesList");
        }
      }

    private SavServiceModel _SelectedService;
    public SavServiceModel SelectedService
      {
      get { return _SelectedService; }
      set
        {
        _SelectedService = value;
        OnPropertyChanged("SelectedService");
        }
      }

    private bool _IsPlayerService;
    public bool IsPlayerService
      {
      get { return _IsPlayerService; }
      set
        {
        _IsPlayerService = value;
        OnPropertyChanged("IsPlayerService");
        }
      }

    private bool _IsPassengerService;
    public bool IsPassengerService
      {
      get { return _IsPassengerService; }
      set
        {
        _IsPassengerService = value;
        OnPropertyChanged("IsPassengerService");
        }
      }

    private List<string> _StopPointsList;
    public List<string> StopPointsList
      {
      get { return _StopPointsList; }
      set
        {
        _StopPointsList = value;
        OnPropertyChanged("StopPointsList");
        }
      }

    private string _StopLocation;
    public string StopLocation
      {
      get { return _StopLocation; }
      set
        {
        _StopLocation = value;
        OnPropertyChanged("StopLocation");
        }
      }

    private string _SelectedStopLocation;
    public string SelectedStopLocation
      {
      get { return _SelectedStopLocation; }
      set
        {
        _SelectedStopLocation = value;
        OnPropertyChanged("SelectedStopLocation");
        }
      }

    public bool IsToolkitGenerated { get; set; }


    #region ScenarioEditHandlers

    public void ScenarioEdit()
      {
      // ScenarioName = Scenario.SavScenario.ScenarioName;
      ScenarioGuid = Guid.NewGuid();
      ScenarioStartTime = GetPlayerServiceStartTimeText(Scenario.SavScenario.SavServiceList);
      OffTheRailsMode = Scenario.SavScenario.RulesDisabledMode;
      IsToolkitGenerated= (ScenarioDataAccess.GetScenarioByGuid(Scenario.SavScenario.ScenarioGuid)!=null);
      ServicesList = new List<SavServiceModel>();
      foreach (var service in Scenario.SavScenario.SavServiceList)
        {
        ServicesList.Add(service);
        }
      }

    public static string GetPlayerServiceStartTimeText(List<SavServiceModel> savServiceList)
      {
      foreach (var service in savServiceList)
        {
        if (service.IsPlayerService)
          {
          return service.StartTimeText;
          }
        }
      return string.Empty;
      }


    public static ulong GetPlayerServiceStartTime(List<SavServiceModel> savServiceList)
      {
      foreach (var service in savServiceList)
        {
        if (service.IsPlayerService)
          {
          return service.StartTime;
          }
        }
      return 0;
      }

    public static void RecalculateStartTimes(long offset,List<SavServiceModel> savServiceList)
      {
      foreach (var service in savServiceList)
        {
        long tmp = (long) service.StartTime;
        service.StartTime=(ulong) (tmp+offset);
        service.StartTimeText = TimeConverters.SecondsToString(service.StartTime, false);
        }
      }

    public void ServiceEdit()
      {
      ServiceName = SelectedService.ServiceName;
      ServiceStartTime = SelectedService.StartTimeText;
      StartLocation = SelectedService.StartPoint;
      EndLocation = SelectedService.EndPoint;
      IsPlayerService = SelectedService.IsPlayerService;
      IsPassengerService = SelectedService.IsPassengerService;
      EngineString = SelectedService.EngineString;
      ConsistString = SelectedService.ConsistString;
      LiveryIdentifier = SelectedService.LiveryIdentifier;
      StopPointsList = new List<string>();
      foreach(var stopPoint in SelectedService.StopLocationList)
        {
        StopPointsList.Add(stopPoint);
        }
      }

    public void ServiceClone()
      {
      var newService = new SavServiceModel(SelectedService);
      newService.ServiceName= $"Copy - { newService.ServiceName}";
      newService.ServiceGuid = Guid.NewGuid();
      ServicesList.Add(newService);
      }

    public void ServiceDelete()
      {
      ServicesList.Remove(SelectedService);
      }

    public void ServiceSave()
      {
      SavServiceModel newService;
      if (SelectedService != null)
        {
        newService= SelectedService;
        }
      else
        {
        newService = new SavServiceModel
          {
          ServiceGuid = Guid.NewGuid()
          };
        ServicesList.Add(newService);
        }

      newService.ServiceName = ServiceName;
      newService.StartTime = TimeConverters.TimeStringToSeconds(ServiceStartTime);
      newService.StartTimeText = ServiceStartTime;
      if (IsPlayerService)
        {
        foreach (var service in ServicesList)
          {
          service.IsPlayerService=false;
          }
        }
      newService.IsPlayerService = IsPlayerService;
      newService.IsPassengerService= IsPassengerService;
      newService.StartPoint = StartLocation;
      newService.EndPoint = EndLocation;
      newService.StopLocationList = StopPointsList;
      newService.ConsistString= ConsistString;
      newService.EngineString= EngineString;
      newService.LiveryIdentifier = LiveryIdentifier;
      ServiceClear();
      }

    public void ServiceClear()
      {
      IsPlayerService=false;
      IsPassengerService=false;
      ServiceName= string.Empty;
      ServiceStartTime = "00:00";
      StartLocation=string.Empty;
      StopLocation=string.Empty;
      StopPointsList=null;
      ConsistString=string.Empty;
      EngineString=string.Empty;
      LiveryIdentifier=string.Empty;
      SelectedService=null;
      }


    public void MoveUpStopPoint()
      {
      if (SelectedStopLocation == null)
        {
        return;
        }
      var ix = StopPointsList.IndexOf(SelectedStopLocation);
      if (ix > 0)
        {
        string currentLocation = SelectedStopLocation;
        string previousLocation = StopPointsList[ix - 1];
        StopPointsList[ix] = previousLocation;
        StopPointsList[ix - 1] = currentLocation;
        }
      }

    public void MoveDownStopPoint()
      {
      if (SelectedStopLocation == null)
        {
        return;
        }
      var ix = StopPointsList.IndexOf(SelectedStopLocation);
      if (ix < StopPointsList.Count - 1)
        {
        string currentLocation = SelectedStopLocation;
        string nextLocation = StopPointsList[ix + 1];
        StopPointsList[ix] = nextLocation;
        StopPointsList[ix + 1] = currentLocation;
        }
      }

    public void UpdateScenarioStartTime()
      {
      if (TimeConverters.IsValidTimeString(ScenarioStartTime))
        {
        var newStartTime = TimeConverters.TimeStringToSeconds(ScenarioStartTime);
        var oldStartTime= GetPlayerServiceStartTime(Scenario.SavScenario.SavServiceList);
        if (newStartTime != oldStartTime)
          {
          var offset = (long) (newStartTime - oldStartTime);
          RecalculateStartTimes(offset, Scenario.SavScenario.SavServiceList);
          }
        }
      }

    public void EditStopLocation()
      {
      StopLocation = SelectedStopLocation;
      }

    public void DeleteStopLocation()
      {
      StopPointsList.Remove(SelectedStopLocation);
      SelectedStopLocation=null;
      StopLocation = "";
      }

    public void SaveStopLocation()
      {
      if (SelectedStopLocation == null)
        {
        StopPointsList.Add(StopLocation);
        StopLocation = "";
        }
      else
        {
        SelectedStopLocation = StopLocation;
        }
      }

    public void SaveCopy()
      {
      UpdateScenarioStartTime();
      var newScenario = new CScenario();
      var newSavScenario = new SavScenarioModel();
      newScenario.SavScenario = newSavScenario;
      newSavScenario.ScenarioName = ScenarioName;
      newSavScenario.ScenarioGuid = ScenarioGuid;
      newSavScenario.GlobalElectrificationMode = Scenario.SavScenario.GlobalElectrificationMode;
      newSavScenario.RouteAbbreviation = Scenario.SavScenario.RouteAbbreviation;
      newSavScenario.RouteName = Scenario.SavScenario.RouteName;
      newSavScenario.RouteString = Scenario.SavScenario.RouteString;
      newSavScenario.RulesDisabledMode = OffTheRailsMode;
      newSavScenario.SavServiceList = ServicesList;
      newSavScenario.TargetAsset = Scenario.SavScenario.TargetAsset;
      newScenario.ScenarioFile = new FileInfo(SavScenarioBuilder.GetClonedScenarioFileName(newSavScenario.ScenarioGuid.ToString(),false));
      Scenario=newScenario;
      SavScenarioBuilder.Build(newScenario);
      if (!IsToolkitGenerated)
        {
        ScenarioModel scenarioDb = new ScenarioModel
          {
          ScenarioGuid = newSavScenario.ScenarioGuid.ToString(),
          ScenarioName = newSavScenario.ScenarioName,
          RouteId = RouteDataAccess.GetRouteIdByAbbreviation(newSavScenario.RouteAbbreviation)
          };
        ScenarioDataAccess.InsertScenario(scenarioDb);
        }
      }

    public void SaveOverwrite()
      {
      // TO implement this
      }

    #endregion


  
    }
  }
