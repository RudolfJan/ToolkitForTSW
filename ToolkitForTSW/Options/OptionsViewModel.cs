using Microsoft.Win32;
using Styles.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using ToolkitForTSW.DataAccess;
using ToolkitForTSW.Models;
using ToolkitForTSW.Options;
using Utilities.Library;
using Utilities.Library.TextHelpers;

namespace ToolkitForTSW
  {
  // local copy of all options, to allow explicit saving them
  public class OptionsViewModel : Notifier
    {
    #region Properties


    public CheckOptionsLogic Check
      {
      get;
      set;
      } = CheckOptionsLogic.Instance;

    private string _steamTrainSimWorldDirectory;
    public string SteamTrainSimWorldDirectory
      {
      get { return _steamTrainSimWorldDirectory; }
      set
        {
        _steamTrainSimWorldDirectory = value;
        TSWOptions.SteamTrainSimWorldDirectory = value;
        Check.SetSteamTSW2ProgramOK();
        OnPropertyChanged("SteamTrainSimWorlDirectory");
        }
      }

    private string _steamTrainSimWorldProgram;

    public string SteamTrainSimWorldProgram
      {
      get { return _steamTrainSimWorldProgram; }
      set
        {
        _steamTrainSimWorldProgram = value;
        Check.SetSteamTSW2ProgramOK();
        OnPropertyChanged("SteamTrainSimWorldProgram");
        }
      }

    private string _egsTrainSimWorldDirectory;

    public string EGSTrainSimWorldDirectory
      {
      get { return _egsTrainSimWorldDirectory; }
      set
        {
        _egsTrainSimWorldDirectory = value;
        TSWOptions.EGSTrainSimWorldDirectory = value;
        Check.SetEGSTSW2ProgramOK();
        }
      }

    private string _egsTrainSimWorldProgram;

    public string EGSTrainSimWorldProgram
      {
      get { return _egsTrainSimWorldProgram; }
      set
        {
        _egsTrainSimWorldProgram = value;
        Check.SetEGSTSW2ProgramOK();
        }
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
        TSWOptions.SteamProgramDirectory = value;
        Check.SetSteamFolderOK();
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
        TSWOptions.SteamUserId = value;
        Check.SetSteamIdOK();
        OnPropertyChanged("SteamUserId");
        }
      }

    #region ClearSettings
    public void ClearTrackIr()
      {
      TrackIRProgram = "";
      TSWOptions.TrackIRProgram = TrackIRProgram;
      Check.SetTrackIROK();

      }

    public void Clear7Zip()
      {
      SevenZip = "";
      TSWOptions.SevenZip = SevenZip;
      Check.SetSevenZipOK();
      }

    public void ClearUmodel()
      {
      UAssetUnpacker = "";
      TSWOptions.UAssetUnpacker=UAssetUnpacker;
      Check.SetUmodelOK();
      }

    public void ClearUnreal()
      {
      Unpacker="";
      TSWOptions.Unpacker=Unpacker;
      Check.SetUnrealOK();
      }


    public void CleartextEditor()
      {
      TextEditor="";
      TSWOptions.TextEditor=TextEditor;
      Check.SetTextEditorOK();
      }

    public void ClearEGS()
      {
      EGSTrainSimWorldProgram="";
      EGSTrainSimWorldDirectory="";
      TSWOptions.EGSTrainSimWorldDirectory = EGSTrainSimWorldDirectory;
      Check.SetEGSTSW2ProgramOK();
      }

    public void ClearSteam()
      {
      SteamTrainSimWorldProgram="";
      SteamTrainSimWorldDirectory="";
      TSWOptions.SteamTrainSimWorldDirectory = SteamTrainSimWorldDirectory;
      Check.SetSteamTSW2ProgramOK();
      }


    public void ClearSteamProgramFolder()
      {
      SteamProgramDirectory="";
      TSWOptions.SteamProgramDirectory=SteamProgramDirectory;
      Check.SetSteamFolderOK();
      }

    public void ClearToolkitFolder()
      {
      ToolkitForTSWFolder="";
      TSWOptions.ToolkitForTSWFolder = ToolkitForTSWFolder;
      Check.SetToolkitFolderOK();
      }


    public void ClearBackupFolder()
      {
      BackupFolder="";
      TSWOptions.BackupFolder=BackupFolder;
      Check.SetBackupFolderOK();
      }

    #endregion
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
        TSWOptions.ToolkitForTSWFolder = value;
        Check.SetToolkitFolderOK();
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
        TSWOptions.BackupFolder = value;
        Check.SetBackupFolderOK();
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
        TSWOptions.TextEditor = value;
        Check.SetTextEditorOK();
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
        TSWOptions.Unpacker = value;
        Check.SetUnrealOK();
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
        TSWOptions.UAssetUnpacker = value;
        Check.SetUmodelOK();
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
        TSWOptions.TrackIRProgram = value;
        Check.SetTrackIROK();
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
        TSWOptions.SevenZip = value;
        Check.SetSevenZipOK();
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

    private PlatformEnum _currentPlatform = PlatformEnum.NotSet;
    public PlatformEnum CurrentPlatform
      {
      get
        {
        return _currentPlatform;
        }
      set
        {
        _currentPlatform = value;
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

    private List<string> SteamGameDirs { get; set; } = new List<string>();
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
      ToolkitForTSWFolder = TSWOptions.ToolkitForTSWFolder;
      BackupFolder = TSWOptions.BackupFolder;
      XMLEditor = TSWOptions.XmlEditor;
      TextEditor = TSWOptions.TextEditor;
      SevenZip = TSWOptions.SevenZip;
      Unpacker = TSWOptions.Unpacker;
      TrackIRProgram = TSWOptions.TrackIRProgram;
      UAssetUnpacker = TSWOptions.UAssetUnpacker;
      UseAdvancedSettings = TSWOptions.UseAdvancedSettings;
      LimitSoundVolumes = TSWOptions.LimitSoundVolumes;
      AutoBackup = TSWOptions.AutoBackup;
      EGSTrainSimWorldStarter = TSWOptions.EGSTrainSimWorldStarter;
      EGSTrainSimWorldDirectory = TSWOptions.EGSTrainSimWorldDirectory;
      SteamTrainSimWorldDirectory = TSWOptions.SteamTrainSimWorldDirectory;
      SteamTrainSimWorldProgram = SteamTrainSimWorldDirectory + "TS2Prototype.exe";
      EGSTrainSimWorldProgram = EGSTrainSimWorldDirectory + "TS2Prototype.exe";
      CurrentPlatform = TSWOptions.CurrentPlatform;
      RouteList = new ObservableCollection<RouteModel>(RouteDataAccess.GetAllRoutes());
      // ClearFileOptions(); //DEBUG
      }

    // This method is only used for testing purposes.
    public void ClearFileOptions()
      {
      SteamProgramDirectory = "";
      SteamUserId = "";
      TextEditor = "";
      SevenZip = "";
      Unpacker = "";
      UAssetUnpacker = "";
      SteamTrainSimWorldDirectory = "";
      SteamTrainSimWorldProgram = "";
      CheckOptionsLogic.Instance.SetAllOptionChecks();
      }

    public void FindDefaults()
      {
      var textEditor = LocateProgram.FindFile("\\Notepad++\\Notepad++.exe");
      if (string.IsNullOrEmpty(XMLEditor))
        {
        XMLEditor = textEditor;
        }

      if (string.IsNullOrEmpty(TextEditor))
        {
        TextEditor = textEditor;
        }

      if (string.IsNullOrEmpty(UAssetUnpacker))
        {
        UAssetUnpacker = LocateProgram.FindFile(@"\UModel\umodel.exe");
        }
      if (string.IsNullOrEmpty(SevenZip))
        {
        SevenZip = LocateProgram.FindFile(@"\7-Zip\7z.exe");
        }

      if (string.IsNullOrEmpty(Unpacker))
        {
        Unpacker = LocateProgram.FindFile(@"\Epic\UE_4.26\Engine\Binaries\Win64\UnrealPak.exe");
        }
      if (string.IsNullOrEmpty(SteamProgramDirectory))
        {
        SearchSteam();
        LocateTSW2Program();
        GuessUserId();
        }
      if(string.IsNullOrEmpty(SteamUserId))
        {
        GuessUserId();
        }
      if (string.IsNullOrEmpty(TrackIRProgram))
        {
        TrackIRProgram = LocateProgram.FindFile(@"\NaturalPoint\TrackIR5\TrackIR5.exe");
        }
      if (string.IsNullOrEmpty(TrackIRProgram))
        {
        TrackIRProgram = LocateProgram.FindFile(@"\opentrack\opentrack.exe");
        }
      if (string.IsNullOrEmpty(TrackIRProgram))
        {
        if (File.Exists(@"{SteamProgramDirectory}\viewtracker.exe"))
          {
          TrackIRProgram = @"{SteamProgramDirectory}\viewtracker.exe";
          }
        }
      }

    private void LocateTSW2Program()
      {
      foreach (var basePath in SteamGameDirs)
        {
        var path = $"{basePath}Train Sim World 2\\WindowsNoEditor\\TS2ProtoType.exe";
        if (File.Exists(path))
          {
          SteamTrainSimWorldDirectory = $"{basePath}Train Sim World 2\\WindowsNoEditor\\";
          SteamTrainSimWorldProgram = path;
          return;
          }
        }
      }

    // https://stackoverflow.com/questions/54767662/finding-game-launcher-executables-in-directory-c-sharp
    public void SearchSteam()
      {
      SteamGameDirs.Clear();

      var steam32 = "SOFTWARE\\VALVE\\";
      var steam64 = "SOFTWARE\\Wow6432Node\\Valve\\";
      string steam32path;
      string steam64path;
      string config32path;
      string config64path;
      var key32 = Registry.LocalMachine.OpenSubKey(steam32);
      var key64 = Registry.LocalMachine.OpenSubKey(steam64);
      if (string.IsNullOrEmpty(key64.ToString()))
        {
        foreach (var k32subKey in key32.GetSubKeyNames())
          {
          using (var subKey = key32.OpenSubKey(k32subKey))
            {
            steam32path = subKey.GetValue("InstallPath").ToString();
            SteamProgramDirectory = TextHelper.AddBackslash(steam32path);
            config32path = steam32path + "/steamapps/libraryfolders.vdf";
            var driveRegex = @"[A-Z]:\\";
            if (File.Exists(config32path))
              {
              var configLines = File.ReadAllLines(config32path);
              foreach (var item in configLines)
                {
                Console.WriteLine("32:  " + item);
                var match = Regex.Match(item, driveRegex);
                if (item != string.Empty && match.Success)
                  {
                  var matched = match.ToString();
                  var item2 = item.Substring(item.IndexOf(matched));
                  item2 = item2.Replace("\\\\", "\\");
                  item2 = item2.Replace("\"", "\\steamapps\\common\\");
                  SteamGameDirs.Add(item2);
                  }
                }
              if (!string.IsNullOrEmpty(SteamProgramDirectory))
                {
                return;
                }
              }
            }
          }
        }
      key64 = Registry.LocalMachine.OpenSubKey(steam64);
      foreach (var k64subKey in key64.GetSubKeyNames())
        {
        using (var subKey = key64.OpenSubKey(k64subKey))
          {
          steam64path = subKey.GetValue("InstallPath")?.ToString();
          SteamProgramDirectory = TextHelper.AddBackslash(steam64path);
          config64path = steam64path + "/steamapps/libraryfolders.vdf";
          var driveRegex = @"[A-Z]:\\";
          if (File.Exists(config64path))
            {
            var configLines = File.ReadAllLines(config64path);
            foreach (var item in configLines)
              {
              Console.WriteLine("64:  " + item);
              var match = Regex.Match(item, driveRegex);
              if (item != string.Empty && match.Success)
                {
                var matched = match.ToString();
                var item2 = item.Substring(item.IndexOf(matched));
                item2 = item2.Replace("\\\\", "\\");
                item2 = item2.Replace("\"", "\\steamapps\\common\\");
                SteamGameDirs.Add(item2);
                }
              }
            if (!string.IsNullOrEmpty(SteamProgramDirectory))
              {
              return;
              }
            }
          }
        }
      }

    public void SaveOptions()
      {
      TSWOptions.SteamProgramDirectory = TextHelper.AddBackslash(SteamProgramDirectory);
      TSWOptions.SteamUserId = SteamUserId;
      TSWOptions.ToolkitForTSWFolder = TextHelper.AddBackslash(ToolkitForTSWFolder);
      TSWOptions.BackupFolder = TextHelper.AddBackslash(BackupFolder);
      TSWOptions.XmlEditor = XMLEditor;
      TSWOptions.TextEditor = TextEditor;
      TSWOptions.SevenZip = SevenZip;
      TSWOptions.Unpacker = Unpacker;
      TSWOptions.TrackIRProgram = TrackIRProgram;
      TSWOptions.UAssetUnpacker = UAssetUnpacker;
      TSWOptions.UseAdvancedSettings = UseAdvancedSettings;
      TSWOptions.LimitSoundVolumes = LimitSoundVolumes;
      TSWOptions.AutoBackup = AutoBackup;
      TSWOptions.EGSTrainSimWorldStarter = EGSTrainSimWorldStarter;
      SteamTrainSimWorldDirectory = Path.GetDirectoryName(SteamTrainSimWorldProgram);
      EGSTrainSimWorldDirectory = Path.GetDirectoryName(EGSTrainSimWorldProgram);
      TSWOptions.EGSTrainSimWorldDirectory = TextHelper.AddBackslash(EGSTrainSimWorldDirectory);
      TSWOptions.SteamTrainSimWorldDirectory = TextHelper.AddBackslash(SteamTrainSimWorldDirectory);

      var oldPlatform = TSWOptions.CurrentPlatform;
      TSWOptions.CurrentPlatform = CurrentPlatform;
      if (CurrentPlatform == PlatformEnum.Steam)
        {
        TSWOptions.TrainSimWorldDirectory = SteamTrainSimWorldDirectory;
        }
      if (CurrentPlatform == PlatformEnum.EpicGamesStore)
        {
        TSWOptions.TrainSimWorldDirectory = EGSTrainSimWorldDirectory;
        }

      TSWOptions.WriteToRegistry();
      CheckOptionsLogic.Instance.SetAllOptionChecks();
      TSWOptions.CreateDirectories();
      TSWOptions.CopyManuals();
      if (CurrentPlatform != oldPlatform)
        {
        PlatformChangedEventHandler.SetPlatformChangedEvent(new PlatformChangedEventArgs(oldPlatform, CurrentPlatform));
        }
      }

    // try to guess the correct user for screenshots by having a look at the file system.

    public void GuessUserId()
      {
      var BasePath = SteamProgramDirectory + "userdata";
      if (Directory.Exists(BasePath))
        {
        var Basedir = new DirectoryInfo(BasePath);
        var Dirinfo = Basedir.GetDirectories();

        foreach (var Dir in Dirinfo)
          {
          if (string.CompareOrdinal(Dir.Name, SteamUserId) == 0)
            {
            return; // found, nothing to do
            }
          }

        foreach (var Dir in Dirinfo)
          {
          SteamUserId = Dir.Name;
          return; // pick the first one
          }
        }
      }

    public void CancelOptions()
      {
      LoadOptions(); // Revert all values, this looks a bit clumsy but we need to keep the
                     // OptionsviewModel and TSWOptions in sync to make the OptionsChecker work.
                     // Would love a better solution ....
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
      RouteId = 0;
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
      SelectedRoute = null;
      RouteList = new ObservableCollection<RouteModel>(RouteDataAccess.GetAllRoutes());
      }

    #endregion
    }
  }