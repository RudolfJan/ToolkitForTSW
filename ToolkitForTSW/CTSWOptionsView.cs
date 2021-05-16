using Styles.Library.Helpers;
using System;
using System.Collections.ObjectModel;
using System.IO;
using ToolkitForTSW.DataAccess;
using ToolkitForTSW.Models;

namespace ToolkitForTSW
  {
  // local copy of all options, to allow explicit saving them
  public class CTSWOptionsView : Notifier
    {
    #region Properties

    /*
Installation folder for Steam program
*/
    private String _SteamProgramDirectory = String.Empty;

    public String SteamProgramDirectory
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
    private String _SteamUserId = String.Empty;

    public String SteamUserId
      {
      get { return _SteamUserId; }
      set
        {
        _SteamUserId = value;
        OnPropertyChanged("SteamUserId");
        }
      }

    /*
    Installation directory for TSW
    */
    private String _TrainSimWorldDirectory = String.Empty;

    public String TrainSimWorldDirectory
      {
      get { return _TrainSimWorldDirectory; }
      set
        {
        _TrainSimWorldDirectory = value;
        OnPropertyChanged("TrainSimWorldDirectory");
        }
      }

    /*
		Installation directory for TSW
		*/
    private String _TrainSimWorldProgram = String.Empty;

    public String TrainSimWorldProgram
      {
      get { return _TrainSimWorldDirectory; }
      set
        {
        _TrainSimWorldProgram = value;
        OnPropertyChanged("TrainSimWorldProgram");
        }
      }



    /*
		Data folder for TSWTools
		*/
    private String _TSWToolsFolder = String.Empty;

    public String TSWToolsFolder
      {
      get { return _TSWToolsFolder; }
      set
        {
        _TSWToolsFolder = value;
        OnPropertyChanged("TSWToolsFolder");
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
    private String _XMLEditor = String.Empty;

    public String XMLEditor
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
    private String _TextEditor = String.Empty;

    public String TextEditor
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
    private String _Unpacker = String.Empty;

    public String Unpacker
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
    private String _UAssetUnpacker = String.Empty;

    public String UAssetUnpacker
      {
      get { return _UAssetUnpacker; }
      set
        {
        _UAssetUnpacker = value;
        OnPropertyChanged("UAssetUnpacker");
        }
      }

    /*
    Path to 7Zip program
    */
    private String _SevenZip = String.Empty;

    public String SevenZip
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

    public CTSWOptionsView()
      {
      LoadOptions();
      }

    #endregion

    public void LoadOptions()
      {
      CTSWOptions.ReadFromRegistry();
      SteamProgramDirectory = CTSWOptions.SteamProgramDirectory;
      SteamUserId = CTSWOptions.SteamUserId;
      TrainSimWorldDirectory = CTSWOptions.TrainSimWorldDirectory;
      TrainSimWorldProgram = TrainSimWorldDirectory + "TS2Prototype.exe";
      TSWToolsFolder = CTSWOptions.TSWToolsFolder;
      BackupFolder= CTSWOptions.BackupFolder;
      XMLEditor = CTSWOptions.XmlEditor;
      TextEditor = CTSWOptions.TextEditor;
      SevenZip = CTSWOptions.SevenZip;
      Unpacker = CTSWOptions.Unpacker;
      UAssetUnpacker = CTSWOptions.UAssetUnpacker;
      UseAdvancedSettings = CTSWOptions.UseAdvancedSettings;
      LimitSoundVolumes = CTSWOptions.LimitSoundVolumes;
      AutoBackup= CTSWOptions.AutoBackup;
      RouteList = new ObservableCollection<RouteModel>(RouteDataAccess.GetAllRoutes());
      }

    public void SaveOptions()
      {
      CTSWOptions.SteamProgramDirectory = FixEndSlash(SteamProgramDirectory);
      CTSWOptions.SteamUserId = SteamUserId;
      TrainSimWorldDirectory = Path.GetDirectoryName(TrainSimWorldProgram);
      CTSWOptions.TrainSimWorldDirectory = FixEndSlash(TrainSimWorldDirectory);
      CTSWOptions.TSWToolsFolder = FixEndSlash(TSWToolsFolder);
      CTSWOptions.BackupFolder= FixEndSlash(BackupFolder);
      CTSWOptions.XmlEditor = XMLEditor;
      CTSWOptions.TextEditor = TextEditor;
      CTSWOptions.SevenZip = SevenZip;
      CTSWOptions.Unpacker = Unpacker;
      CTSWOptions.UAssetUnpacker = UAssetUnpacker;
      CTSWOptions.UseAdvancedSettings = UseAdvancedSettings;
      CTSWOptions.LimitSoundVolumes = LimitSoundVolumes;
      CTSWOptions.AutoBackup= AutoBackup;
      CTSWOptions.WriteToRegistry();
      CTSWOptions.CreateDirectories();
      CTSWOptions.CopyManuals();
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



    private static String FixEndSlash(String Input)
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