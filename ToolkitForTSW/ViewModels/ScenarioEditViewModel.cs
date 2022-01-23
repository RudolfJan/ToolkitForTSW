using Caliburn.Micro;
using SavCracker.Library;
using SavCracker.Library.Models;
using Styles.Library.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToolkitForTSW.DataAccess;
using ToolkitForTSW.DialogServices;
using ToolkitForTSW.Models;
using ToolkitForTSW.Scenario;
using Utilities.Library.TextHelpers;

namespace ToolkitForTSW.ViewModels
  {
 
  public class ScenarioEditViewModel : Screen
    {
    private readonly IDialogService _dialogService;

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
       NotifyOfPropertyChange(()=>Scenario);
        }
      }

    private string _ScenarioName;
    public string ScenarioName
      {
      get { return _ScenarioName; }
      set
        {
        _ScenarioName = value;
        NotifyOfPropertyChange(() =>Scenario);
        NotifyOfPropertyChange(() => CanSaveCopy);
        }
      }

    private Guid _ScenarioGuid = Guid.NewGuid();
    public Guid ScenarioGuid
      {
      get { return _ScenarioGuid; }
      set
        {
        _ScenarioGuid = value;
        NotifyOfPropertyChange(() =>ScenarioGuid);
        }
      }

    private string _ScenarioStartTime;
    public string ScenarioStartTime
      {
      get { return _ScenarioStartTime; }
      set
        {
        _ScenarioStartTime = value;
        NotifyOfPropertyChange(() => ScenarioStartTime);
        }
      }

    private string _ServiceName;
    public string ServiceName
      {
      get { return _ServiceName; }
      set
        {
        _ServiceName = value;
        NotifyOfPropertyChange(() =>ServiceName);
        }
      }

    private string _ServiceStartTime;
    public string ServiceStartTime
      {
      get { return _ServiceStartTime; }
      set
        {
        _ServiceStartTime = value;
        NotifyOfPropertyChange(() =>ServiceStartTime);
        }
      }

    private string _StartLocation;
    public string StartLocation
      {
      get { return _StartLocation; }
      set
        {
        _StartLocation = value;
        NotifyOfPropertyChange(() =>StartLocation);
        }
      }

    private string _EndLocation;
    public string EndLocation
      {
      get { return _EndLocation; }
      set
        {
        _EndLocation = value;
        NotifyOfPropertyChange(() =>EndLocation);
        }
      }

    private string _EngineString;
    public string EngineString
      {
      get { return _EngineString; }
      set
        {
        _EngineString = value;
        NotifyOfPropertyChange(() =>EngineString);
        }
      }

    private string _ConsistString;
    public string ConsistString
      {
      get { return _ConsistString; }
      set
        {
        _ConsistString = value;
        NotifyOfPropertyChange(() =>ConsistString);
        }
      }

    private string _LiveryIdentifier;
    public string LiveryIdentifier
      {
      get { return _LiveryIdentifier; }
      set
        {
        _LiveryIdentifier = value;
        NotifyOfPropertyChange(() =>LiveryIdentifier);
        }
      }

    private bool _OffTheRailsMode;
    public bool OffTheRailsMode
      {
      get { return _OffTheRailsMode; }
      set
        {
        _OffTheRailsMode = value;
        NotifyOfPropertyChange(() =>OffTheRailsMode);
        }
      }

    private BindableCollection<SavServiceModel> _ServicesList;
    public BindableCollection<SavServiceModel> ServicesList
      {
      get { return _ServicesList; }
      set
        {
        _ServicesList = value;
        NotifyOfPropertyChange(() =>ServicesList);
        }
      }

    private SavServiceModel _SelectedService;
    public SavServiceModel SelectedService
      {
      get { return _SelectedService; }
      set
        {
        _SelectedService = value;
        NotifyOfPropertyChange(() =>SelectedService);
        NotifyOfPropertyChange(()=> CanServiceEdit);
        NotifyOfPropertyChange(() => CanServiceDelete);
        NotifyOfPropertyChange(() => CanServiceClone);
        }
      }

    private bool _IsPlayerService;
    public bool IsPlayerService
      {
      get { return _IsPlayerService; }
      set
        {
        _IsPlayerService = value;
        NotifyOfPropertyChange(() =>IsPlayerService);
        }
      }

    private bool _IsPassengerService;
    public bool IsPassengerService
      {
      get { return _IsPassengerService; }
      set
        {
        _IsPassengerService = value;
        NotifyOfPropertyChange(() =>IsPassengerService);
        }
      }

    private BindableCollection<string> _StopPointsList;
    public BindableCollection<string> StopPointsList
      {
      get { return _StopPointsList; }
      set
        {
        _StopPointsList = value;
        NotifyOfPropertyChange(() =>StopPointsList);
        }
      }

    private bool _IsStopLocationListChanged=false;
    public bool IsStopLocationListChanged
      {
      get { return _IsStopLocationListChanged; }
      set
        {
        _IsStopLocationListChanged = value;
        NotifyOfPropertyChange(() =>IsStopLocationListChanged);
        }
      }


    private string _StopLocation;
    public string StopLocation
      {
      get { return _StopLocation; }
      set
        {
        _StopLocation = value;
        NotifyOfPropertyChange(() =>StopLocation);
        }
      }

    private string _SelectedStopLocation;
    public string SelectedStopLocation
      {
      get { return _SelectedStopLocation; }
      set
        {
        _SelectedStopLocation = value;
        NotifyOfPropertyChange(() =>SelectedStopLocation); 
        NotifyOfPropertyChange(() => CanEditStopLocation);
        NotifyOfPropertyChange(() => CanDeleteStopLocation);
        NotifyOfPropertyChange(() => CanMoveUpStopPoint);
        NotifyOfPropertyChange(() => CanMoveDownStopPoint);
        }
      }

    public bool IsToolkitCreated { get; set; }

 

    #region ScenarioEditHandlers

    public ScenarioEditViewModel(IDialogService dialogService)
      {

      _dialogService= dialogService;
      }

    public void Initialize()
      {
      // ScenarioName = Scenario.SavScenario.ScenarioName;
      ScenarioGuid = Guid.NewGuid();
      ScenarioStartTime = GetPlayerServiceStartTimeText(Scenario.SavScenario.SavServiceList);
      OffTheRailsMode = Scenario.SavScenario.RulesDisabledMode;
      IsToolkitCreated= (ScenarioDataAccess.GetScenarioByGuid(Scenario.SavScenario.ScenarioGuid)!=null);
      ServicesList = new BindableCollection<SavServiceModel>();
      foreach (var service in Scenario.SavScenario.SavServiceList)
        {
        ServicesList.Add(service);
        }
      }

    protected override void OnViewLoaded(object view)
      {
      base.OnViewLoaded(view);
      Initialize();
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

    public bool CanServiceEdit
      {
      get
        {
        return SelectedService!=null;
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
      StopPointsList = new BindableCollection<string>();
      foreach(var stopPoint in SelectedService.StopLocationList)
        {
        StopPointsList.Add(stopPoint);
        }
      NotifyOfPropertyChange(() => SelectedService.StopLocationList);
      NotifyOfPropertyChange(() => ServicesList);
      }

    public bool CanServiceClone
      {
      get
        {
        return SelectedService != null;
        }
      }
    public void ServiceClone()
      {
      var newService = new SavServiceModel(SelectedService);
      newService.ServiceName= $"Copy - { newService.ServiceName}";
      newService.ServiceGuid = Guid.NewGuid();
      ServicesList.Add(newService);
      NotifyOfPropertyChange(() => ServicesList);
      }

    public bool CanServiceDelete
      {
      get
        {
        return SelectedService != null;
        }
      }
    public void ServiceDelete()
      {
      ServicesList.Remove(SelectedService);
      NotifyOfPropertyChange(()=>ServicesList);
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
      newService.StopLocationList = new List<string>();
      foreach (var stopLocation in StopPointsList)
        {
        newService.StopLocationList.Add(stopLocation);
        }
      newService.ConsistString= ConsistString;
      newService.EngineString= EngineString;
      newService.LiveryIdentifier = LiveryIdentifier;
      ScenarioStartTime = GetPlayerServiceStartTimeText(Scenario.SavScenario.SavServiceList);
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

    #endregion

    #region StopPoints

    public bool CanMoveUpStopPoint
      {
      get
        {
        return SelectedStopLocation!=null;
        }
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
        IsStopLocationListChanged = true;
        }
      NotifyOfPropertyChange(()=> StopPointsList);
      }

    public bool CanMoveDownStopPoint
      {
      get
        {
        return SelectedStopLocation != null;
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
        IsStopLocationListChanged = true;
        }

      NotifyOfPropertyChange(() => StopPointsList);
      }

    public bool CanEditStopLocation
      {
      get
        {
        return SelectedStopLocation != null;
        }
      }


    public void EditStopLocation()
      {
      StopLocation = SelectedStopLocation;
      }

    public void AddStopLocation()
      {
      StopLocation = "";
      SelectedStopLocation=null;
      }

    public bool CanDeleteStopLocation
      {
      get
        {
        return SelectedStopLocation != null;
        }
      }
    public void DeleteStopLocation()
      {
      StopPointsList.Remove(SelectedStopLocation);
      SelectedStopLocation=null;
      StopLocation = "";
      IsStopLocationListChanged = true;
      NotifyOfPropertyChange(() => StopPointsList);
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
      IsStopLocationListChanged = true;
      NotifyOfPropertyChange(() => StopPointsList);
      }

    internal void SaveStopLocationList()
      {
      SelectedService.StopLocationList = new List<string>();
      foreach (var stopPoint in StopPointsList)
        {
        SelectedService.StopLocationList.Add(stopPoint);
        }
      IsStopLocationListChanged = false;
      NotifyOfPropertyChange(() => StopPointsList);
      }

    internal void RefreshStopLocationList()
      {
      StopPointsList.Clear();
      foreach (var stopPoint in SelectedService.StopLocationList)
        {
        StopPointsList.Add(stopPoint);
        }
      IsStopLocationListChanged = false;
      NotifyOfPropertyChange(() => StopPointsList);
      }

    #endregion

    #region SaveScenario

    public bool CanSaveCopy
      {
      get
        {
        return ScenarioName!=null && ScenarioName.Length>3;
        }
      }

    public Task SaveCopy()
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
      newSavScenario.SavServiceList = ServicesList.ToList();
      newSavScenario.TargetAsset = Scenario.SavScenario.TargetAsset;
      newScenario.ScenarioFile = new FileInfo(SavScenarioBuilder.GetClonedScenarioFileName(newSavScenario.ScenarioGuid.ToString(),false));
      Scenario=newScenario;
      SavScenarioBuilder.Build(newScenario);
      if (!IsToolkitCreated)
        {
        ScenarioModel scenarioDb = new ScenarioModel
          {
          ScenarioGuid = newSavScenario.ScenarioGuid.ToString(),
          ScenarioName = newSavScenario.ScenarioName,
          RouteId = RouteDataAccess.GetRouteIdByAbbreviation(newSavScenario.RouteAbbreviation)
          };
        ScenarioDataAccess.InsertScenario(scenarioDb);
        Scenario.IsToolkitCreated=true;
        }
      _dialogService.Show("Scenario copied, edited and rebuilt successfully", "Save changes as copy", DialogButton.OK, DialogImage.Information);
      return TryCloseAsync();
      }

    public bool CanSaveOverWrite
      {
      get
        {
        return Scenario?.SavScenario != null && IsToolkitCreated;
        }
      }

    public Task SaveOverWrite()
      {
      UpdateScenarioStartTime();
      var savScenario = Scenario.SavScenario;
      if(!string.IsNullOrEmpty(ScenarioName))
        {
        savScenario.ScenarioName = ScenarioName;
        }
      savScenario.ScenarioGuid = ScenarioGuid;
      // savScenario.GlobalElectrificationMode = Scenario.SavScenario.GlobalElectrificationMode;
      savScenario.RouteAbbreviation = Scenario.SavScenario.RouteAbbreviation;
      savScenario.RouteName = Scenario.SavScenario.RouteName;
      savScenario.RouteString = Scenario.SavScenario.RouteString;
      savScenario.RulesDisabledMode = OffTheRailsMode;
      savScenario.SavServiceList = ServicesList.ToList();
      savScenario.TargetAsset = Scenario.SavScenario.TargetAsset;
      SavScenarioBuilder.Build(Scenario);
      _dialogService.Show("Scenario overwritten, edited and rebuilt successfully", "Updated scenario", DialogButton.OK, DialogImage.Information);
      return TryCloseAsync();
      }

    public Task Cancel()
      {
      return TryCloseAsync();
      }

    public Task Close()
      {
      return TryCloseAsync();
      }
    #endregion


  
    }
  }
