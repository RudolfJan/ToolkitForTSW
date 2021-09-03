using System;
using System.IO;
using System.Reflection;
using System.Windows;
using Logging.Library;
using Microsoft.Win32;
using ToolkitForTSW.Options;
using Utilities.Library;
using Utilities.Library.Wpf.Models;
using MessageBox = System.Windows.MessageBox;

namespace ToolkitForTSW
  {
  public enum PlatformEnum
    {
    NotSet = 0,
    Steam = 1,
    EpicGamesStore = 2
    }

  public class TSWOptions
    {
    private static readonly Assembly currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();
    private static readonly AboutModel AboutData = new AboutModel();

    // Number of read only constants, that depend on the version
    public static bool AllowDevMode = false; // set to false to completely disable DevMode for users

    private static bool _DeveloperMode = false;
    private static bool _TestMode = false;
    private static string _TextEditor = string.Empty;
    private static string _XmlEditor = string.Empty;
    private static string _SevenZip = string.Empty;
    private static string _FileCompare = string.Empty;
    private static string _RegkeyString = string.Empty;
    private static string _SteamRegKey = string.Empty;
    private static string _TrainSimWorldDirectory = string.Empty;
    private static string _SteamProgramDirectory = string.Empty;
    private static string _SteamUserId = string.Empty;
    private static string _Unpacker = string.Empty;
    private static string _trackIRProgram = string.Empty;

    private static string
      _UAssetUnpacker = string.Empty; // Unpack tool for UAsset files, must be UModel 

    private static string _ToolkitForTSWFolder = string.Empty;
    private static bool _IsInitialized = false;

    protected static RegistryKey AppKey = null;
    protected static RegistryKey SteamKey = null;

    // Properties will be initialized anagrammatically
    public static string ManualsFolder { get; set; }
    public static string ModsFolder { get; set; }
    public static string UnpackFolder { get; set; }
    public static string AssetUnpackFolder { get; set; }
    public static string TempFolder { get; set; }

    public static string TemplateFolder { get; set; }
    public static string ScenarioFolder { get; set; }
    public static string ThumbnailFolder { get; set; }
    public static bool NotFirstRun { get; set; }

    public static PlatformEnum CurrentPlatform { get; set; } = PlatformEnum.NotSet;
    public static string Version { get; } = "1.0";

    public static string GameSaveLocation
      {
      get
        {
        return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
         + @"\My Games\TrainSimWorld2\";
        }
      }

    // static holder for instance, need to use lambda to construct since constructor private
    private static readonly Lazy<TSWOptions> _instance
      = new Lazy<TSWOptions>(() => new TSWOptions());

    // private to prevent direct instantiation.

    private TSWOptions()
      {

      }

    // accessor for instance

    public static TSWOptions Instance
      {
      get
        {
         return _instance.Value;
        }
      }

    #region Properties

    public static string SavedScreenshots
      {
      get
        {
        return $"{GetSaveLocationPath()}Saved\\Screenshots\\WindowsNoEditor";
        }
      }

    public static string TSWSettingsDirectory
      {
      get
        {
        return $"{GetSaveLocationPath()}Saved\\Config\\WindowsNoEditor";
        }
      }

    public static string SteamProgramDirectory
      {
      set { _SteamProgramDirectory = value; }

      get
        {

        if (TestMode) // append test folder to default directory if test mode has been selected
          {
          return _SteamProgramDirectory;
          }
        else
          {
          return _SteamProgramDirectory;
          }
        }
      }

    public static string SteamUserId
      {
      set
        {
        _SteamUserId = value;
        }
      get
        {
        return _SteamUserId;
        }
      }

    public static string SavedSteamScreenshots
      {
      get
        {
        return SteamProgramDirectory + "\\userdata\\" + SteamUserId +
               "\\760\\remote\\1282590\\screenshots";
        }
      }

    public static string RegkeyString
      {
      get
        {
        AboutData.CurrentAssembly = currentAssembly;

        _RegkeyString = "software\\" + AboutData.Company + "\\" +
                            AboutData.Product;
        return _RegkeyString;
        }
      }

    public static string SteamKeyString
      {
      get
        {
        _SteamRegKey = "Software\\Valve\\Steam";
        return _SteamRegKey;
        }
      }

    private static string _steamTrainSimWorldDirectory;

    public static string SteamTrainSimWorldDirectory
      {
      get { return _steamTrainSimWorldDirectory; }
      set { _steamTrainSimWorldDirectory = value; }
      }

    private static string _egsTrainSimWorldDirectory;

    public static string EGSTrainSimWorldDirectory
      {
      get { return _egsTrainSimWorldDirectory; }
      set { _egsTrainSimWorldDirectory = value; }
      }


    private static string _egsTrainSimWorldStarter;

    public static string EGSTrainSimWorldStarter
      {
      get { return _egsTrainSimWorldStarter; }
      set { _egsTrainSimWorldStarter = value; }
      }

    public static string TrainSimWorldDirectory
      {
      set { _TrainSimWorldDirectory = value; }
      get
        {
        if (TestMode) // append test folder to default directory if test mode has been selected
          {
          return _TrainSimWorldDirectory;
          }
        else
          {
          return _TrainSimWorldDirectory;
          }
        }
      }

    public static string ToolkitForTSWFolder
      {
      set { _ToolkitForTSWFolder = value; }
      get
        {
        return _ToolkitForTSWFolder;
        }
      }

    private static string _backupFolder;

    public static string BackupFolder
      {
      get
        {
        if (string.IsNullOrEmpty(_backupFolder))
          {
          _backupFolder = ToolkitForTSWFolder + "Backup\\";
          }
        return _backupFolder;
        }
      set
        {
        _backupFolder = value;
        }
      }

    private static string _InstallDirectory;

    public static string InstallDirectory
      {
      set { _InstallDirectory = value; }
      get
        {
        return _InstallDirectory;
        }
      }

    public static string OptionsSetDir
      {
      get { return ToolkitForTSWFolder + "OptionsSets\\"; }
      }

    public static bool IsInitialized
      {
      set { _IsInitialized = value; }
      get
        {
        return _IsInitialized;
        }
      }

    public static string XmlEditor
      {
      set { _XmlEditor = value; }
      get
        {
        return _XmlEditor;
        }
      }

    public static string TextEditor
      {
      set { _TextEditor = value; }
      get
        {
        return _TextEditor;
        }
      }

    public static string Unpacker
      {
      set { _Unpacker = value; }
      get
        {
        return _Unpacker;
        }
      }

    public static string TrackIRProgram
      {
      set { _trackIRProgram = value; }
      get
        {
        return _trackIRProgram;
        }
      }

    public static string UAssetUnpacker
      {
      set { _UAssetUnpacker = value; }
      get
        {
        return _UAssetUnpacker;
        }
      }

    public static string FileCompare
      {
      set { _FileCompare = value; }
      get
        {
        return _FileCompare;
        }
      }

    public static string SevenZip
      {
      set { _SevenZip = value; }
      get
        {
        return _SevenZip;
        }
      }

    public static bool DeveloperMode
      {
      set { _DeveloperMode = value; }
      get
        {
        if (AllowDevMode
          ) // this allows to prevent reading the DevMode option from the registry, forcing devmode to false
          {
          return _DeveloperMode;
          }
        else
          {
          return false;
          }
        }
      }

    public static bool UseAdvancedSettings { get; set; }
    public static bool LimitSoundVolumes { get; set; }
    public static bool AutoBackup { get; set; }

    public static bool TestMode
      {
      set { _TestMode = value; }
      get
        {
        return _TestMode;
        }
      }

    public static bool DeveloperMode1
      {
      get { return _DeveloperMode; }

      set { _DeveloperMode = value; }
      }

    public static string Installer { get; set; } = string.Empty;


    #endregion

    private static RegistryKey OpenRegistry()
      {
      return Registry.CurrentUser.CreateSubKey(RegkeyString, true);
      }

    // try to guess the correct user for screenshots by having a look at the file system.

    public static void GuessUserId()
      {
      string BasePath = SteamProgramDirectory + "\\userdata";
      if (Directory.Exists(BasePath))
        {
        DirectoryInfo Basedir = new DirectoryInfo(BasePath);
        DirectoryInfo[] Dirinfo = Basedir.GetDirectories();

        foreach (var Dir in Dirinfo)
          {
          if (string.CompareOrdinal(Dir.Name, SteamUserId) == 0)
            return; // found, nothing to do
          }

        foreach (var Dir in Dirinfo)
          {
          SteamUserId = Dir.Name;
          AppKey.SetValue("SteamUserId", SteamUserId,
            RegistryValueKind.String); // set it in the registry right away.
          return; // pick the first one
          }
        }
      }

    // To be used to show the platform to the users
    public static string GetPlatformDisplayString(PlatformEnum platform)
      {
      return platform switch
        {
          PlatformEnum.NotSet => "Platform not set",
          PlatformEnum.Steam => "Steam",
          PlatformEnum.EpicGamesStore => "Epic Game Store",
          _ => "Platform unknown",
          };
      }

    // Get Platform name for use as folder name
    public static string GetPlatformFolderName(PlatformEnum platform)
      {
      return platform switch
        {
          PlatformEnum.NotSet => "",
          PlatformEnum.Steam => "Steam",
          PlatformEnum.EpicGamesStore => "EGS",
          _ => "",
          };
      }

    public static string GetSaveLocationPath()
      {
      if(TSWOptions.CurrentPlatform==PlatformEnum.EpicGamesStore)
        {
        var location= TSWOptions.GameSaveLocation;
        if(location.EndsWith("\\"))
          {
          location=location.Substring(0, location.Length - 1);
          }
        return $"{location}EGS\\";
        }
      return TSWOptions.GameSaveLocation;
      }

    public static string GetOptionsSetPath()
      {
      return TSWOptions.OptionsSetDir + TSWOptions.GetPlatformFolderName(TSWOptions.CurrentPlatform) + "\\";
      }


    public static void CreateDirectories()
      {
      ManualsFolder = ToolkitForTSWFolder + "Manuals\\";
      var RouteGuidesFolder = ManualsFolder + "RouteGuides\\";
      ModsFolder = ToolkitForTSWFolder + "Mods\\";
      UnpackFolder = ToolkitForTSWFolder + "Unpack\\";
      AssetUnpackFolder = ToolkitForTSWFolder + "Unpack\\UnpackedAssets";
      TempFolder = ToolkitForTSWFolder + "Temp\\";
      TemplateFolder = ToolkitForTSWFolder + "Templates\\";
      ScenarioFolder = ToolkitForTSWFolder + "Scenarios\\";
      ThumbnailFolder = ToolkitForTSWFolder + "Thumbnails\\";
      // Unfortunately this does not work and gives security issues
      //try
      //  {
      //  MoveData(BackupFolder, $"{BackupFolder}Steam\\");
      //  MoveData(OptionsSetDir, $"{OptionsSetDir}Steam\\");
      //  }
      //catch(Exception ex)
      //  {
      //  Log.Trace("Error moving directories for EGS update because " + ex.Message, LogEventType.Error);
      //  }
      try
        {
        Directory.CreateDirectory(ManualsFolder);
        Directory.CreateDirectory(RouteGuidesFolder);
        Directory.CreateDirectory(TempFolder);
        Directory.CreateDirectory(ModsFolder);
        Directory.CreateDirectory(UnpackFolder);
        Directory.CreateDirectory(AssetUnpackFolder);
        Directory.CreateDirectory(OptionsSetDir);
        
        Directory.CreateDirectory(TempFolder);
        Directory.CreateDirectory($"{OptionsSetDir}Steam\\");
        Directory.CreateDirectory($"{OptionsSetDir}EGS\\");
        Directory.CreateDirectory(BackupFolder);
        Directory.CreateDirectory($"{BackupFolder}Steam\\"); // Just in case, order is important!
        Directory.CreateDirectory($"{BackupFolder}EGS\\");
        Directory.CreateDirectory(TemplateFolder);
        Directory.CreateDirectory(ScenarioFolder);
        Directory.CreateDirectory(ThumbnailFolder);
        }
      catch (Exception ex)
        {
        Log.Trace("Error creating directories because " + ex.Message, LogEventType.Error);
        }
      }

    public static void MoveData(string source, string destination)
      {
      // Move Data to proper directory
  
      if (Directory.Exists(source)&&!Directory.Exists(destination))
        {
        Directory.Move(source, destination);
        }
      }

    public static void CopyManuals()
      {
      var SourceDir = InstallDirectory;
      try
        {
        File.Copy(SourceDir + "Manuals\\ToolkitForTSW Manual.pdf",
          ManualsFolder + "ToolkitForTSW Manual.pdf", true);
        File.Copy(SourceDir + "Manuals\\TSW2 Starters guide.pdf", ManualsFolder + "TSW2 Starters guide.pdf",
          true);
        File.Copy(SourceDir + "Manuals\\License information.pdf", ManualsFolder + "License information.pdf",
          true);
        }
      catch (Exception E)
        {
        Log.Trace("Error installing manual files because " + E.Message);
        }
      }

    public static void ReadFromRegistry()
      {
      if (AppKey == null)
        {
        AppKey = OpenRegistry();
        SteamKey = Registry.CurrentUser.CreateSubKey(SteamKeyString, true);
        }

      InstallDirectory = (string)AppKey.GetValue("InstallDirectory", "");
      
      string DefaultSteamProgramPath = (string)AppKey.GetValue("SteamPath", "");
      SteamProgramDirectory =
        (string)AppKey.GetValue("SteamProgramDirectory", DefaultSteamProgramPath);
      SteamUserId = (string)AppKey.GetValue("SteamUserId", "");
      ToolkitForTSWFolder = (string)AppKey.GetValue("TSWToolsFolder", "");
      BackupFolder = (string)AppKey.GetValue("BackupFolder", "");
      TextEditor = (string)AppKey.GetValue("TextEditor", "");
      XmlEditor = (string)AppKey.GetValue("XMLEditor", "");
      Unpacker = (string)AppKey.GetValue("Unpacker", "");
      UAssetUnpacker = (string)AppKey.GetValue("UAssetUnpacker", "");
      TrackIRProgram = (string)AppKey.GetValue("TrackIRProgram", "");
      FileCompare = (string)AppKey.GetValue("FileCompare", "");
      SevenZip = (string)AppKey.GetValue("7Zip", "");
      var SavedCurrentPlatform= TSWOptions.CurrentPlatform;

      CurrentPlatform =(PlatformEnum)AppKey.GetValue("CurrentPlatform", PlatformEnum.NotSet);
      if(SavedCurrentPlatform!=CurrentPlatform)
        {
        PlatformChangedEventHandler.SetPlatformChangedEvent(new PlatformChangedEventArgs(SavedCurrentPlatform, CurrentPlatform));
        }

      TrainSimWorldDirectory = (string)AppKey.GetValue("TrainSimWorldDirectory", ""); //Deprecated for the registry.
      SteamTrainSimWorldDirectory = (string)AppKey.GetValue("SteamTrainSimWorldDirectory", "");
      EGSTrainSimWorldDirectory = (string)AppKey.GetValue("EGSTrainSimWorldDirectory", "");
      EGSTrainSimWorldStarter = (string)AppKey.GetValue("EGSTrainSimWorldStarter", "");
      if (SteamTrainSimWorldDirectory.Length==0 && TrainSimWorldDirectory.Length>0)
        {
        SteamTrainSimWorldDirectory=TrainSimWorldDirectory;
        }


      //TestMode = ((int)AppKey.GetValue("TestMode", 0) == 1);
      TestMode = false;
      IsInitialized = ((int)AppKey.GetValue("Initialized", 0) == 1);

      if (!AllowDevMode)
        {
        _DeveloperMode = ((int)AppKey.GetValue("DeveloperMode", 0) == 1);
        }

      UseAdvancedSettings = (int)AppKey.GetValue("UseAdvancedSettings", 1) == 1;
      LimitSoundVolumes = (int)AppKey.GetValue("LimitSoundVolumes", 1) == 1;
      AutoBackup = (int)AppKey.GetValue("AutoBackup", 0) == 1;


    }

    public static void WriteToRegistry()
      {
      if (AppKey == null)
        {
        AppKey = OpenRegistry();
        }

      if (!CheckOptionsModel.SteamIdOk)
        {
        GuessUserId();
        }
 
      AppKey.SetValue("SteamProgramDirectory", SteamProgramDirectory,
        RegistryValueKind.String);
      AppKey.SetValue("SteamUserId", SteamUserId, RegistryValueKind.String);
      AppKey.SetValue("TSWToolsFolder", ToolkitForTSWFolder, RegistryValueKind.String);
      AppKey.SetValue("BackupFolder", BackupFolder, RegistryValueKind.String);
      AppKey.SetValue("TextEditor", TextEditor, RegistryValueKind.String);
      AppKey.SetValue("XMLEditor", XmlEditor, RegistryValueKind.String);
      AppKey.SetValue("Unpacker", Unpacker, RegistryValueKind.String);
      AppKey.SetValue("UAssetUnpacker", UAssetUnpacker, RegistryValueKind.String);
      AppKey.SetValue("TrackIRProgram", TrackIRProgram, RegistryValueKind.String);
      AppKey.SetValue("7Zip", SevenZip, RegistryValueKind.String);
      AppKey.SetValue("FileCompare", FileCompare, RegistryValueKind.String);
      // AppKey.SetValue("TestMode", TestMode, RegistryValueKind.DWord);
      AppKey.SetValue("DeveloperMode", DeveloperMode, RegistryValueKind.DWord);
      AppKey.SetValue("Initialized", IsInitialized, RegistryValueKind.DWord);
      AppKey.SetValue("UseAdvancedSettings", UseAdvancedSettings, RegistryValueKind.DWord);
      AppKey.SetValue("LimitSoundVolumes", LimitSoundVolumes, RegistryValueKind.DWord);
      AppKey.SetValue("AutoBackup", AutoBackup, RegistryValueKind.DWord);
      AppKey.SetValue("CurrentPlatform",CurrentPlatform,RegistryValueKind.DWord);
      AppKey.SetValue("SteamTrainSimWorldDirectory", SteamTrainSimWorldDirectory,
   RegistryValueKind.String);
      AppKey.SetValue("EGSTrainSimWorldDirectory", EGSTrainSimWorldDirectory,
   RegistryValueKind.String);
      AppKey.SetValue("EGSTrainSimWorldStarter", EGSTrainSimWorldStarter,
   RegistryValueKind.String);
      }

    public static bool GetNotFirstRun()
      {
      var AppKey2 = OpenRegistry();
      if (AppKey2 != null)
        {
        NotFirstRun = (int)AppKey2.GetValue("NotFirstRun", 0) == 1;
        }

      return NotFirstRun;
      }

    public static void SetNotFirstRun()
      {
      var AppKey2 = OpenRegistry();
      AppKey2.SetValue("NotFirstRun", true, RegistryValueKind.DWord);
      }

    public static void TestNotFirstRun()
      {
      var AppKey2 = OpenRegistry();
      AppKey2.SetValue("NotFirstRun", false, RegistryValueKind.DWord);
      }

    public static void UpdateTSWToolsDirectory(string InitialInstallDirectory)
      {
      if (string.CompareOrdinal(InitialInstallDirectory, ToolkitForTSWFolder) != 0)
        {
        // try moving files
        if (!Directory.Exists(ToolkitForTSWFolder) && Directory.Exists(InitialInstallDirectory))
          {
          Directory.Move(InitialInstallDirectory, ToolkitForTSWFolder);
          SetNotFirstRun();
          return;
          }

        if (Directory.Exists(ToolkitForTSWFolder) && Directory.Exists(InitialInstallDirectory))
          {
          FileHelpers.CopyDir(InitialInstallDirectory, ToolkitForTSWFolder, true);
          SetNotFirstRun();
          return;
          }

        if (!Directory.Exists(ToolkitForTSWFolder) && Directory.Exists(InitialInstallDirectory))
          {
          MessageBox.Show(@"Please complete the configuration before you proceed",
            @"Set configuration",
            MessageBoxButton.OK, MessageBoxImage.Asterisk);
          SetNotFirstRun();
          MessageBox.Show(@"Something went wrong, I cannot find the ToolkitForTSW folder structure",
            @"Something went wrong",
            MessageBoxButton.OK, MessageBoxImage.Error);
          return;
          }
        }
      else
        {
        SetNotFirstRun();
        }
      }
    }
  }
