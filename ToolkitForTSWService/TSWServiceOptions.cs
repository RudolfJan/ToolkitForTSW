using Microsoft.Win32;
using System.IO;

namespace ToolkitForTSWService
  {
  public enum PlatformEnum
    {
    NotSet = 0,
    Steam = 1,
    EpicGamesStore = 2
    }

  // Note, we do not manage options exclusively for this service, but they are reatrieved for ththe registry, where ToolkitForTSW has the capabilities to change them.
  internal class TSWServiceOptions
    {
    private static RegistryKey? AppKey;
    // private static read only Assembly currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();

    //TODO this is now hard coded in the code, not very brilliant
    public static string RegkeyString
      {
      get
        {
        return "software\\Holland Hiking\\ToolkitForTSW";
        }
      }
    public static string ToolkitForTSWFolder { get; set; } = string.Empty;
    public static string BackupFolder { get; set; } = string.Empty;
    public static string SaveGameArchiveFolder { get; set; } = string.Empty;
    public static bool NotFirstRun { get; set; }
    public static bool AutoBackup { get; set; }
    public static bool AutoBackupSaveGames { get; set; }
    public static bool AutoBackupSaveGamesOnly { get; set; }
    public static bool IsInitialized { get; set; }
    public static string LastBackupDate { get; set; } = String.Empty;

    public static PlatformEnum CurrentPlatform { get; set; } = PlatformEnum.NotSet;
    public static string Version { get; } = "0.1";

    public static string GameSaveLocation
      {
      get
        {
        return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
         + @"\My Games\TrainSimWorld2\";
        }
      }

    #region Constructor
    // static holder for instance, need to use lambda to construct since constructor private
    private static readonly Lazy<TSWServiceOptions> _instance
      = new Lazy<TSWServiceOptions>(() => new TSWServiceOptions());

    // private to prevent direct instantiation.

    private TSWServiceOptions()
      {

      }

    // accessor for instance

    public static TSWServiceOptions Instance
      {
      get
        {
        return _instance.Value;
        }
      }
    #endregion
    private static RegistryKey OpenRegistry()
      {
      return Registry.CurrentUser.CreateSubKey(RegkeyString, false);
      }

    public static void ReadFromRegistry()
      {
      AppKey ??= OpenRegistry();

      if (AppKey is null)
        {
        return;
        }

      ToolkitForTSWFolder = AppKey.GetValue("TSWToolsFolder", "") as string ?? string.Empty;
      BackupFolder = AppKey.GetValue("BackupFolder", "") as string ?? string.Empty;

      //var SavedCurrentPlatform = TSWServiceOptions.CurrentPlatform;

      var temp = AppKey.GetValue("CurrentPlatform") as int? ?? 0;
      TSWServiceOptions.CurrentPlatform = (PlatformEnum)temp;
      IsInitialized = (AppKey.GetValue("Initialized", 0) as int? == 1);
      AutoBackup = (AppKey.GetValue("AutoBackup", 0) as int? == 1);
      LastBackupDate = GetLastBackupDate();
      }

    public static string CreateDateString()
      {
      DateTime Now = DateTime.Now;
      return Now.ToString("yyyy-MM-dd");
      }

    public static string GetLastBackupDate()
      {
      if (File.Exists($"{ToolkitForTSWFolder}LastbackupDate.txt"))
        {
        return File.ReadAllText($"{ToolkitForTSWFolder}LastbackupDate.txt") ?? "";
        }
      return "";
      }

    public static string UpdateLastbackupDate()
      {
      LastBackupDate = CreateDateString();
      try
        {
        File.WriteAllText($"{ToolkitForTSWFolder}LastbackupDate.txt", LastBackupDate);
        return "Backup date saved";
        }
      catch
        {
        return "Error writing backup date";
        }
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
      if (TSWServiceOptions.CurrentPlatform == PlatformEnum.EpicGamesStore)
        {
        var location = TSWServiceOptions.GameSaveLocation;
        if (location.EndsWith("\\"))
          {
          location = location.Substring(0, location.Length - 1);
          }
        return $"{location}EGS\\";
        }
      return TSWServiceOptions.GameSaveLocation;
      }
    }
  }
