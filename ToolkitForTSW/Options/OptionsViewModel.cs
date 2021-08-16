using Styles.Library.Helpers;
using System;
using System.Collections.ObjectModel;
using System.IO;
using ToolkitForTSW.DataAccess;
using ToolkitForTSW.Models;
using ToolkitForTSW.Options;
using ToolkitForTSW.Views;

namespace ToolkitForTSW
  {
  // local copy of all options, to allow explicit saving them
  public class OptionsViewModel : Notifier
    {
    #region Properties

    private CheckOptionsModel _CheckOptions = new CheckOptionsModel();
    public CheckOptionsModel CheckOptions
      {
      get { return _CheckOptions; }
      set
        {
        _CheckOptions = value;
        OnPropertyChanged("CheckOptions");
        }
      }

    private string _steamTrainSimWorldDirectory;

    public string SteamTrainSimWorldDirectory
      {
      get { return _steamTrainSimWorldDirectory; }
      set { _steamTrainSimWorldDirectory = value; 
        OnPropertyChanged("CheckOptiions.SteamTSW2ProgramOK");
        OnPropertyChanged("SteamtrainSimWorlDirectory");
        }
      }

    private string _steamTrainSimWorldProgram;

    public string SteamTrainSimWorldProgram
      {
      get { return _steamTrainSimWorldProgram; }
      set
        {
        _steamTrainSimWorldProgram = value;
        OnPropertyChanged("CheckOptiions.SteamTSW2ProgramOK");
        OnPropertyChanged("SteamTrainSimWorlProgram");
        }
      }

    private string _egsTrainSimWorldDirectory;

    public string EGSTrainSimWorldDirectory
      {
      get { return _egsTrainSimWorldDirectory; }
      set { _egsTrainSimWorldDirectory = value; }
      }

    private string _egsTrainSimWorldProgram;

    public string EGSTrainSimWorldProgram
      {
      get { return _egsTrainSimWorldProgram; }
      set { _egsTrainSimWorldProgram = value; }
      }
    private string _egsTrainSimWorldStarter;

    public string EGSTrainSimWorldStarter
      {
      get { return _egsTrainSimWorldStarter; }
      set { _egsTrainSimWorldStarter = value; }
      }


    /*
Installation folder for Steam program
*/
    private string _SteamProgramDirectory = string.Empty;

    public string SteamProgramDirectory
      {
      get { return _SteamProgramDirectory; }
      set
        {
        _SteamProgramDirectory = value;
        OnPropertyChanged("SteamProgramDirectory");
        }
      }

    /*
    UserId for steam, needed to retrieve screenshots
    */
    private string _SteamUserId = string.Empty;

    public string SteamUserId
      {
      get { return _SteamUserId; }
      set
      {
        _SteamUserId = value;
        OnPropertyChanged("SteamUserId");
        }
      }

    /*
    Installation directory for TSW deprecated
    */
    private string _TrainSimWorldDirectory = string.Empty;

    public string TrainSimWorldDirectory
      {
      get { return _TrainSimWorldDirectory; }
      set
        {
        _TrainSimWorldDirectory = value;
        OnPropertyChanged("TrainSimWorldDirectory");
        }
      }

    /*
		Installation directory for TSW deprecated
		*/
    private string _TrainSimWorldProgram = string.Empty;

    public string TrainSimWorldProgram
      {
      get { return _TrainSimWorldProgram; }
      set
        {
        _TrainSimWorldProgram = value;
        OnPropertyChanged("TrainSimWorldProgram");
        }
      }

    /*
		Data folder for TSWTools
		*/
    private string _toolkitForTSWFolder = string.Empty;

    public string ToolkitForTSWFolder
      {
      get { return _toolkitForTSWFolder; }
      set
        {
        _toolkitForTSWFolder = value;
        OnPropertyChanged("ToolkitForTSWFolder");
        }
      }

    private string _BackupFolder;
    public string BackupFolder
      {
      get { return _BackupFolder; }
      set
        {
        _BackupFolder = value;
        OnPropertyChanged("BackupFolder");
        }
      }

    /*
    Path th preferred editor for XML files
    */
    private string _XMLEditor = string.Empty;

    public string XMLEditor
      {
      get { return _XMLEditor; }
      set
        {
        _XMLEditor = value;
        OnPropertyChanged("XMLEditor");
        }
      }

    /*
    Path to preferred TextEditor
    */
    private string _TextEditor = string.Empty;

    public string TextEditor
      {
      get { return _TextEditor; }
      set
        {
        _TextEditor = value;
        OnPropertyChanged("TextEditor");
        }
      }

    /*
    Path to preferred unpacker for .pak files
    */
    private string _Unpacker = string.Empty;

    public string Unpacker
      {
      get { return _Unpacker; }
      set
        {
        _Unpacker = value;
        OnPropertyChanged("Unpacker");
        }
      }

    /*
    Path to preferred UAsset Unpacker
    */
    private string _UAssetUnpacker = string.Empty;

    public string UAssetUnpacker
      {
      get { return _UAssetUnpacker; }
      set
        {
        _UAssetUnpacker = value;
        OnPropertyChanged("UAssetUnpacker");
        }
      }

    private string _trackIRProgram = string.Empty;

    public string TrackIRProgram
      {
      get { return _trackIRProgram; }
      set
        {
        _trackIRProgram = value;
        OnPropertyChanged("TrackIRProgram");
        }
      }

    /*
    Path to 7Zip program
    */
    private string _SevenZip = string.Empty;

    public string SevenZip
      {
      get { return _SevenZip; }
      set
        {
        _SevenZip = value;
        OnPropertyChanged("SevenZip");
        }
      }

    private bool _useAdvancedSettings;

    public bool UseAdvancedSettings
      {
      get
        {
        return _useAdvancedSettings;
        }
      set
        {
        _useAdvancedSettings = value;
        OnPropertyChanged("UseAdvancedSettings");
        }
      }

    private bool _limitSoundVolumes;

    public bool LimitSoundVolumes
      {
      get
        {
        return _limitSoundVolumes;
        }
      set
        {
        _limitSoundVolumes = value;
        OnPropertyChanged("LimitSoundVolumes");
        }
      }

    /*
    Make a backup when starting ToolkitForTSW
    */
    private bool _AutoBackup;
    public bool AutoBackup
      {
      get { return _AutoBackup; }
      set
        {
        _AutoBackup = value;
        OnPropertyChanged("AutoBackup");
        }
      }

    private PlatformEnum _currentPlatform= PlatformEnum.NotSet;
    public PlatformEnum CurrentPlatform
      {
      get
        {
        return _currentPlatform;
        }
      set
        {
        _currentPlatform= value;
        OnPropertyChanged("CurrentPlatform");
        }
      }


    private int RouteId { get; set; } = 0;


    private ObservableCollection<RouteModel> _RouteList;
    public ObservableCollection<RouteModel> RouteList
      {
      get { return _RouteList; }
      set
        {
        _RouteList = value;
        OnPropertyChanged("RouteList");
        }
      }

    private RouteModel _SelectedRoute;
    public RouteModel SelectedRoute
      {
      get { return _SelectedRoute; }
      set
        {
        _SelectedRoute = value;
        OnPropertyChanged("SelectedRoute");
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

    private string _RouteAbbrev;
    public string RouteAbbrev
      {
      get { return _RouteAbbrev; }
      set
        {
        _RouteAbbrev = value;
        OnPropertyChanged("RouteAbbrev");
        }
      }

    private string _RouteDescription;
    public string RouteDescription
      {
      get { return _RouteDescription; }
      set
        {
        _RouteDescription = value;
        OnPropertyChanged("RouteDescription");
        }
      }

    private string _ScenarioPlannerRouteName;
    public string ScenarioPlannerRouteName
      {
      get { return _ScenarioPlannerRouteName; }
      set
        {
        _ScenarioPlannerRouteName = value;
        OnPropertyChanged("ScenarioPlannerRouteName");
        }
      }
    private string _ScenarioPlannerRouteString;
    public string ScenarioPlannerRouteString
      {
      get { return _ScenarioPlannerRouteString; }
      set
        {
        _ScenarioPlannerRouteString = value;
        OnPropertyChanged("ScenarioPlannerRouteString");
        }
      }

    private string _RouteImagePath;
    public string RouteImagePath
      {
      get { return _RouteImagePath; }
      set
        {
        _RouteImagePath = value;
        OnPropertyChanged("RouteImagePath");
        }
      }

    #endregion

    #region Constructor

    public OptionsViewModel()
      {
      LoadOptions();
      }

    #endregion

    public void LoadOptions()
      {
      TSWOptions.ReadFromRegistry();
      SteamProgramDirectory = TSWOptions.SteamProgramDirectory;
      SteamUserId = TSWOptions.SteamUserId;
      TrainSimWorldProgram = TrainSimWorldDirectory + "TS2Prototype.exe";
      ToolkitForTSWFolder = TSWOptions.ToolkitForTSWFolder;
      BackupFolder= TSWOptions.BackupFolder;
      XMLEditor = TSWOptions.XmlEditor;
      TextEditor = TSWOptions.TextEditor;
      SevenZip = TSWOptions.SevenZip;
      Unpacker = TSWOptions.Unpacker;
      TrackIRProgram=TSWOptions.TrackIRProgram;
      UAssetUnpacker = TSWOptions.UAssetUnpacker;
      UseAdvancedSettings = TSWOptions.UseAdvancedSettings;
      LimitSoundVolumes = TSWOptions.LimitSoundVolumes;
      AutoBackup= TSWOptions.AutoBackup;
      EGSTrainSimWorldStarter=TSWOptions.EGSTrainSimWorldStarter;
      EGSTrainSimWorldDirectory=TSWOptions.EGSTrainSimWorldDirectory;
      SteamTrainSimWorldDirectory=TSWOptions.SteamTrainSimWorldDirectory;
      SteamTrainSimWorldProgram = SteamTrainSimWorldDirectory + "TS2Prototype.exe";
      EGSTrainSimWorldProgram = EGSTrainSimWorldDirectory + "TS2Prototype.exe";
      CurrentPlatform =TSWOptions.CurrentPlatform;
      RouteList = new ObservableCollection<RouteModel>(RouteDataAccess.GetAllRoutes());
      }

    public void SaveOptions()
      {
      TSWOptions.SteamProgramDirectory = FixEndSlash(SteamProgramDirectory);
      TSWOptions.SteamUserId = SteamUserId;
      TrainSimWorldDirectory = Path.GetDirectoryName(TrainSimWorldProgram);
      TSWOptions.ToolkitForTSWFolder = FixEndSlash(ToolkitForTSWFolder);
      TSWOptions.BackupFolder= FixEndSlash(BackupFolder);
      TSWOptions.XmlEditor = XMLEditor;
      TSWOptions.TextEditor = TextEditor;
      TSWOptions.SevenZip = SevenZip;
      TSWOptions.Unpacker = Unpacker;
      TSWOptions.TrackIRProgram = TrackIRProgram; 
      TSWOptions.UAssetUnpacker = UAssetUnpacker;
      TSWOptions.UseAdvancedSettings = UseAdvancedSettings;
      TSWOptions.LimitSoundVolumes = LimitSoundVolumes;
      TSWOptions.AutoBackup= AutoBackup;
      TSWOptions.EGSTrainSimWorldStarter=EGSTrainSimWorldStarter;
      SteamTrainSimWorldDirectory = Path.GetDirectoryName(SteamTrainSimWorldProgram);
      EGSTrainSimWorldDirectory = Path.GetDirectoryName(EGSTrainSimWorldProgram);
      TSWOptions.EGSTrainSimWorldDirectory=FixEndSlash(EGSTrainSimWorldDirectory);
      TSWOptions.SteamTrainSimWorldDirectory=FixEndSlash(SteamTrainSimWorldDirectory);

      var oldPlatform= TSWOptions.CurrentPlatform;
      TSWOptions.CurrentPlatform = CurrentPlatform;
      if(CurrentPlatform== PlatformEnum.Steam)
        {
        TSWOptions.TrainSimWorldDirectory = SteamTrainSimWorldDirectory;
        }
      if (CurrentPlatform == PlatformEnum.EpicGamesStore)
        {
        TSWOptions.TrainSimWorldDirectory = EGSTrainSimWorldDirectory;
        }

      TSWOptions.WriteToRegistry();
      TSWOptions.CreateDirectories();
      TSWOptions.CopyManuals();
      if (CurrentPlatform != oldPlatform)
        {
        PlatformChangedEventHandler.SetPlatformChangedEvent(new PlatformChangedEventArgs(oldPlatform, CurrentPlatform));
        }
      }

    #region RouteEditor

    public void LoadRouteList()
      {
      RouteDataAccess.InitRouteForSavCracker("SQL\\RouteDataImport.csv");
      RouteList = new ObservableCollection<RouteModel>(RouteDataAccess.GetAllRoutes());
      }

    public void EditRoute()
      {
      RouteId = SelectedRoute.Id;
      RouteName = SelectedRoute.RouteName;
      RouteAbbrev = SelectedRoute.RouteAbbrev;
      RouteDescription = SelectedRoute.RouteDescription;
      RouteImagePath = SelectedRoute.RouteImagePath;
      ScenarioPlannerRouteName = SelectedRoute.ScenarioPlannerRouteName;
      ScenarioPlannerRouteString = SelectedRoute.ScenarioPlannerRouteString;
      }

    public void SaveRoute()
      {
      RouteModel newRoute;

      if (RouteId == 0) // New route
        {
        newRoute = new RouteModel();
        }
      else
        {
        newRoute = SelectedRoute;
        }
      newRoute.RouteName = RouteName;
      newRoute.RouteAbbrev = RouteAbbrev;
      newRoute.RouteDescription = RouteDescription;
      newRoute.RouteImagePath = RouteImagePath;
      newRoute.ScenarioPlannerRouteName = ScenarioPlannerRouteName;
      newRoute.ScenarioPlannerRouteString = ScenarioPlannerRouteString;
      if (RouteId == 0)
        {
        RouteId = RouteDataAccess.InsertRoute(newRoute);
        }
      else
        {
        RouteId = RouteDataAccess.UpdateRoute(newRoute);
        }
      RouteList = new ObservableCollection<RouteModel>(RouteDataAccess.GetAllRoutes());
      }

    public void ClearRoute()
      {
      RouteId=0;
      RouteName = string.Empty;
      RouteAbbrev = string.Empty;
      RouteDescription = string.Empty;
      RouteImagePath = string.Empty;
      ScenarioPlannerRouteName = string.Empty;
      ScenarioPlannerRouteString = string.Empty;
      }

    public void DeleteRoute()
      {
      ClearRoute();
      RouteDataAccess.DeleteRoute(SelectedRoute.Id);
      SelectedRoute=null;
      RouteList = new ObservableCollection<RouteModel>(RouteDataAccess.GetAllRoutes());
      }

    #endregion


    private static string FixEndSlash(string Input)
      {
      if (!Input.EndsWith("\\"))
        {
        return Input + "\\";
        }
      else
        {
        return Input;
        }
      }
    }
  }