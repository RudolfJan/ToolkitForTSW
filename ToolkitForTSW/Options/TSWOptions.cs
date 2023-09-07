using Logging.Library;
using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using ToolkitForTSW.Options;
using Utilities.Library;
using Utilities.Library.TextHelpers;
using Utilities.Library.Wpf.Models;
using MessageBox = System.Windows.MessageBox;

namespace ToolkitForTSW
  {
  public class TSWOptions
    {
    private static readonly Assembly currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();
    private static readonly AboutModel AboutData = new AboutModel();

    // Number of read only constants, that depend on the version
    public static bool AllowDevMode { get; set; } = false; // set to false to completely disable DevMode for users

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

    private static RegistryKey AppKey = null;

    // Properties will be initialized anagrammatically
    public static string ManualsFolder { get; set; }
    public static string ModsFolder { get; set; }
    public static string UnpackFolder { get; set; }
    public static string AssetUnpackFolder { get; set; }
    public static string TempFolder { get; set; }

    public static string TemplateFolder { get; set; }
    public static string ScenarioFolder { get; set; }
    public static string ThumbnailFolder { get; set; }
    public static string SaveGameArchiveFolder { get; set; }
    public static bool NotFirstRun { get; set; }
    private static readonly string subKeyString = "TSW3";

    public static PlatformEnum CurrentPlatform { get; set; } = PlatformEnum.NotSet;

    public static string Version { get; } = "3.0";

    public static string GameSaveLocation
      {
      get
        {
        return TextHelper.AddBackslash(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Games\\TrainSimWorld3");
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
           "\\760\\remote\\1944790\\screenshots";
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

    private static string _steamTrainSimWorldDirectory = string.Empty;

    public static string SteamTrainSimWorldDirectory
      {
      get { return _steamTrainSimWorldDirectory; }
      set { _steamTrainSimWorldDirectory = value; }
      }

    private static string _egsTrainSimWorldDirectory = string.Empty;

    public static string EGSTrainSimWorldDirectory
      {
      get { return _egsTrainSimWorldDirectory; }
      set { _egsTrainSimWorldDirectory = value; }
      }


    private static string _egsTrainSimWorldStarter = string.Empty;

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

    private static string _backupFolder = "";

    public static string BackupFolder
      {
      get
        {
        return _backupFolder;
        }
      set
        {
        _backupFolder = value;
        }
      }

    private static string _InstallDirectory = string.Empty;

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
    public static object GameEdition { get; set; } = string.Empty;
    public static int GameEditionId { get; internal set; }


    #endregion

    internal static RegistryKey OpenCurrentUserRegistry()
      {
      return Registry.CurrentUser.CreateSubKey(RegkeyString, true);
      }


    // To be used to show the platform to the users
    public static string GetPlatformDisplayString(PlatformEnum platform)
      {
      return platform switch
        {
          PlatformEnum.NotSet => $"{GameEdition} Platform not set",
          PlatformEnum.Steam => $"{GameEdition} Steam",
          PlatformEnum.EpicGamesStore => $"{GameEdition} Epic Game Store",
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
      if (TSWOptions.CurrentPlatform == PlatformEnum.EpicGamesStore)
        {
        var location = TSWOptions.GameSaveLocation;
        if (location.EndsWith("\\"))
          {
          location = location.Substring(0, location.Length - 1);
          }
        return $"{location}EGS\\";
        }
      return TSWOptions.GameSaveLocation;
      }

    public static string GetOptionsSetPath()
      {
      return TSWOptions.OptionsSetDir + TSWOptions.GetPlatformFolderName(TSWOptions.CurrentPlatform) + "\\";
      }


    public static void CreateDirectories(string baseFolder)
      {
      ManualsFolder = baseFolder + "Manuals\\";
      var RouteGuidesFolder = ManualsFolder + "RouteGuides\\";
      SaveGameArchiveFolder = baseFolder + "SaveGame\\";
      ModsFolder = baseFolder + "Mods\\";
      UnpackFolder = baseFolder + "Unpack\\";
      AssetUnpackFolder = baseFolder + "Unpack\\UnpackedAssets";
      TempFolder = baseFolder + "Temp\\";
      TemplateFolder = baseFolder + "Templates\\";
      ScenarioFolder = baseFolder + "Scenarios\\";
      ThumbnailFolder = baseFolder + "Thumbnails\\";
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
        Directory.CreateDirectory(SaveGameArchiveFolder);
        Directory.CreateDirectory(TempFolder);
        Directory.CreateDirectory(ModsFolder);
        Directory.CreateDirectory(UnpackFolder);
        Directory.CreateDirectory(AssetUnpackFolder);
        Directory.CreateDirectory(OptionsSetDir);
        Directory.CreateDirectory(TempFolder);
        Directory.CreateDirectory($"{OptionsSetDir}Steam\\");
        Directory.CreateDirectory($"{OptionsSetDir}EGS\\");
        // By default, the Backup folder is not set.
        if (!string.IsNullOrEmpty(BackupFolder))
          {
          Directory.CreateDirectory(BackupFolder);
          Directory.CreateDirectory($"{BackupFolder}Steam\\"); // Just in case, order is important!
          Directory.CreateDirectory($"{BackupFolder}EGS\\");
          Directory.CreateDirectory($"{BackupFolder}Movies\\");
          }
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

      if (Directory.Exists(source) && !Directory.Exists(destination))
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
        File.Copy(SourceDir + "Manuals\\TSW3 Starters guide.pdf", ManualsFolder + "TSW3 Starters guide.pdf",
  true);
        File.Copy(SourceDir + "Manuals\\TSW3 Advanced guide.pdf", ManualsFolder + "TSW3 Advanced guide.pdf",
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
      AppKey ??= OpenCurrentUserRegistry();
      // For TSW3 we use a separate subKey where relevant, so you still can use the old version for TSW2 next to the TSW3 version of ToolkitForTSW
      var subKey = AppKey.CreateSubKey(subKeyString);

      IsInitialized = ((int)subKey.GetValue("Initialized", 0) == 1);
      InstallDirectory = (string)subKey.GetValue("InstallDirectory", "");
      ToolkitForTSWFolder = (string)subKey.GetValue("TSWToolsFolder", "");
      BackupFolder = (string)subKey.GetValue("BackupFolder", "");
      UseAdvancedSettings = (int)subKey.GetValue("UseAdvancedSettings", 1) == 1;
      SteamTrainSimWorldDirectory = (string)subKey.GetValue("SteamTrainSimWorldDirectory", "");
      EGSTrainSimWorldDirectory = (string)subKey.GetValue("EGSTrainSimWorldDirectory", "");
      EGSTrainSimWorldStarter = (string)subKey.GetValue("EGSTrainSimWorldStarter", "");
      if (SteamTrainSimWorldDirectory.Length == 0 && TrainSimWorldDirectory.Length > 0)
        {
        SteamTrainSimWorldDirectory = TrainSimWorldDirectory;
        }

      string DefaultSteamProgramPath = (string)AppKey.GetValue("SteamPath", "");
      SteamProgramDirectory =
        (string)AppKey.GetValue("SteamProgramDirectory", DefaultSteamProgramPath);
      SteamUserId = (string)AppKey.GetValue("SteamUserId", "");

      TextEditor = (string)AppKey.GetValue("TextEditor", "");
      XmlEditor = (string)AppKey.GetValue("XMLEditor", "");
      Unpacker = (string)AppKey.GetValue("Unpacker", "");
      UAssetUnpacker = (string)AppKey.GetValue("UAssetUnpacker", "");
      TrackIRProgram = (string)AppKey.GetValue("TrackIRProgram", "");
      FileCompare = (string)AppKey.GetValue("FileCompare", "");
      SevenZip = (string)AppKey.GetValue("7Zip", "");

      var SavedCurrentPlatform = TSWOptions.CurrentPlatform;

      CurrentPlatform = (PlatformEnum)AppKey.GetValue("CurrentPlatform", PlatformEnum.NotSet);
      if (SavedCurrentPlatform != CurrentPlatform)
        {
        PlatformChangedEventHandler.SetPlatformChangedEvent(new PlatformChangedEventArgs(SavedCurrentPlatform, CurrentPlatform));
        }

      //TestMode = ((int)AppKey.GetValue("TestMode", 0) == 1);
      TestMode = false;

      if (!AllowDevMode)
        {
        _DeveloperMode = ((int)AppKey.GetValue("DeveloperMode", 0) == 1);
        }

      LimitSoundVolumes = (int)AppKey.GetValue("LimitSoundVolumes", 1) == 1;
      AutoBackup = (int)AppKey.GetValue("AutoBackup", 0) == 1;

      CheckOptionsLogic.Instance.SetAllOptionChecks();
      }

    public static string GetTSW2ToolkitPath()
      {
      var appKey = OpenCurrentUserRegistry();
      return (string)appKey.GetValue("TSWToolsFolder", "");
      }

    public static void WriteToRegistry()
      {
      var appKey = OpenCurrentUserRegistry();
      AppKey = appKey; // is this correct?
      // For TSW3 we use a separate subKey where relevant, so you still can use the old version for TSW2 next to the TSW3 version of ToolkitForTSW

      var subKey = appKey.CreateSubKey(subKeyString, true);
      WriteToRegistry(appKey, subKey);
      }


    public static void WriteToRegistry(RegistryKey appKey, RegistryKey subKey)
      {
      subKey.SetValue("Initialized", IsInitialized, RegistryValueKind.DWord);
      subKey.SetValue("UseAdvancedSettings", UseAdvancedSettings, RegistryValueKind.DWord);
      subKey.SetValue("TSWToolsFolder", ToolkitForTSWFolder, RegistryValueKind.String);
      subKey.SetValue("BackupFolder", BackupFolder, RegistryValueKind.String);
      subKey.SetValue("SteamTrainSimWorldDirectory", SteamTrainSimWorldDirectory,
RegistryValueKind.String);
      subKey.SetValue("EGSTrainSimWorldDirectory", EGSTrainSimWorldDirectory,
   RegistryValueKind.String);
      subKey.SetValue("EGSTrainSimWorldStarter", EGSTrainSimWorldStarter,
   RegistryValueKind.String);

      appKey.SetValue("SteamProgramDirectory", SteamProgramDirectory,
        RegistryValueKind.String);
      appKey.SetValue("SteamUserId", SteamUserId, RegistryValueKind.String);

      appKey.SetValue("TextEditor", TextEditor, RegistryValueKind.String);
      appKey.SetValue("XMLEditor", XmlEditor, RegistryValueKind.String);
      appKey.SetValue("Unpacker", Unpacker, RegistryValueKind.String);
      appKey.SetValue("UAssetUnpacker", UAssetUnpacker, RegistryValueKind.String);
      appKey.SetValue("TrackIRProgram", TrackIRProgram, RegistryValueKind.String);
      appKey.SetValue("7Zip", SevenZip, RegistryValueKind.String);
      appKey.SetValue("FileCompare", FileCompare, RegistryValueKind.String);
      // AppKey.SetValue("TestMode", TestMode, RegistryValueKind.DWord);
      appKey.SetValue("DeveloperMode", DeveloperMode, RegistryValueKind.DWord);

      appKey.SetValue("LimitSoundVolumes", LimitSoundVolumes, RegistryValueKind.DWord);
      appKey.SetValue("AutoBackup", AutoBackup, RegistryValueKind.DWord);
      appKey.SetValue("CurrentPlatform", CurrentPlatform, RegistryValueKind.DWord);

      }

    public static bool GetNotFirstRun()
      {
      var AppKey2 = OpenCurrentUserRegistry();
      var subKey = AppKey2.CreateSubKey(subKeyString);
      if (subKey != null)
        {
        NotFirstRun = (int)subKey.GetValue("NotFirstRun", 0) == 1;
        }

      return NotFirstRun;
      }

    public static void SetNotFirstRun()
      {
      var AppKey2 = OpenCurrentUserRegistry();
      var subKey = AppKey2.CreateSubKey(subKeyString, true);
      subKey.SetValue("NotFirstRun", true, RegistryValueKind.DWord);
      }

    public static void TestNotFirstRun()
      {
      var AppKey2 = OpenCurrentUserRegistry();
      var subKey = AppKey2.CreateSubKey(subKeyString);
      subKey.SetValue("NotFirstRun", false, RegistryValueKind.DWord);
      }

    public static void UpdateTSWToolsDirectory(string InitialInstallDirectory)
      {
      if (!string.IsNullOrEmpty(ToolkitForTSWFolder) && string.CompareOrdinal(InitialInstallDirectory, ToolkitForTSWFolder) != 0)
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
