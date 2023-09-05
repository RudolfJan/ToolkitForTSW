using Serilog;
using System.IO;
using Utilities.Library;

namespace ToolkitForTSWService
  {
  internal class TSWBackup
    {
    // We do not use this all for the daily backup, but it is easier to define them right now
    private bool SaveConfig { get; set; } = true;
    private bool SaveSaveGames { get; set; } = true;
    private bool SaveScreenShots { get; set; } = false;
    private bool SaveLoadingScreens { get; set; } = false;
    private bool SaveLogs { get; set; } = false;
    private bool SaveCrashes { get; set; } = false;
    private bool SaveScenarios { get; set; } = true;
    private bool SaveDatabase { get; set; } = true;
    private bool SaveManuals { get; set; } = false;
    private bool SaveSettings { get; set; } = false;
    private bool SaveMods { get; set; } = false;
    private bool SaveCreatorsClub { get; set; } = false;

    private List<DirectoryInfo> BackupSetsList { get; set; } = new List<DirectoryInfo>();

    private string BackupPath = "";

    public void SetSaveDaily()
      {
      SaveConfig = true;
      SaveSaveGames = true;
      SaveScreenShots = false;
      SaveLoadingScreens = false;
      SaveLogs = false;
      SaveCrashes = false;
      SaveScenarios = true;
      SaveDatabase = true;
      SaveManuals = false;
      SaveSettings = false;
      SaveMods = false;
      SaveCreatorsClub = false;
      }

    private DateTime? _backupDateTime;
    public string? CreateBackupSetName()
      {
      _backupDateTime = DateTime.Now;
      return _backupDateTime?.ToString("yyyy-MM-dd$HHmm");
      }

    private void CreateBackupPath()
      {
      BackupPath = $"{TSWServiceOptions.BackupFolder}{TSWServiceOptions.GetPlatformFolderName(TSWServiceOptions.CurrentPlatform)}\\";
      }

    private void FillBackupList()
      {
      if (string.IsNullOrEmpty(BackupPath))
        {
        CreateBackupPath();
        }

      BackupSetsList.Clear();
      DirectoryInfo DirInfo = new DirectoryInfo(BackupPath);
      var Dirs = DirInfo.GetDirectories("*", SearchOption.TopDirectoryOnly);
      foreach (var X in Dirs)
        {
        BackupSetsList.Add(X);
        }
      }

    public void MakeBackup()
      {
      var SourceBase = TSWServiceOptions.GetSaveLocationPath();
      var ToolkitBase = TSWServiceOptions.ToolkitForTSWFolder;
      var TargetBase = BackupPath + CreateBackupSetName() + "\\";
      BackUpPart(SourceBase, TargetBase, "Saved\\Config\\", SaveConfig);
      BackUpPart(SourceBase, TargetBase, "Saved\\Screenshots\\", SaveScreenShots);
      BackUpPart(SourceBase, TargetBase, "Saved\\LoadingScreens\\", SaveLoadingScreens);
      BackUpPart(SourceBase, TargetBase, "Saved\\Logs\\", SaveLogs);
      BackUpPart(SourceBase, TargetBase, "Saved\\Crashes\\", SaveCrashes);
      BackUpPart(SourceBase, TargetBase, "Saved\\PersistentDownloadDir\\UGC\\0\\", SaveCreatorsClub);
      //BackUpToolkitPart(TSWOptions.ManualsFolder, TargetBase + "ToolKit\\Manuals\\", SaveManuals);
      //BackUpToolkitPart(TSWOptions.GetSaveLocationPath(), TargetBase + "ToolKit\\OptionsSets\\", SaveSettings);
      //BackUpToolkitPart(TSWOptions.ModsFolder, TargetBase + "ToolKit\\Mods\\", SaveMods);
      BackUpSingleFile(ToolkitBase, TargetBase + "ToolKit\\", "TSWTools.db", SaveDatabase);
      BackupSetsList.Add(new DirectoryInfo(TargetBase));
      SaveSaveGamesFiles(SourceBase, TargetBase, SaveSaveGames);
      SaveScenarioFiles(SourceBase, TargetBase, SaveScenarios);
      WriteBackupMetaData(TargetBase, CreateMetaData("Automatic daily"));
      }

    private static void SaveScenarioFiles(string sourceBase, string targetBase, bool saveScenarios)
      {
      if (saveScenarios)
        {
        var targetDir = $"{targetBase}Saved\\SaveGames\\";
        Directory.CreateDirectory(targetDir);
        FileInfo[] files = GetSavFileList(sourceBase);
        foreach (var file in files)
          {
          if (file.Name.StartsWith("USD_"))
            {
            File.Copy(file.FullName, $"{targetDir}{file.Name}", true);
            }
          }
        }
      }

    private static void SaveSaveGamesFiles(string sourceBase, string targetBase, bool saveSaveGames)
      {
      if (saveSaveGames)
        {
        var targetDir = $"{targetBase}Saved\\SaveGames\\";
        Directory.CreateDirectory(targetDir);
        FileInfo[] files = GetSavFileList(sourceBase);
        foreach (var file in files)
          {
          if (!file.Name.StartsWith("USD_"))
            {
            File.Copy(file.FullName, $"{targetDir}{file.Name}", true);
            }
          }
        }
      }

    private static FileInfo[] GetSavFileList(string sourceBase)
      {
      var sourceDir = new DirectoryInfo($"{sourceBase}Saved\\SaveGames\\");
      var files = sourceDir.GetFiles("*.sav", SearchOption.TopDirectoryOnly);
      return files;
      }

    private static bool IsBackupNeeded(string sourceBase)
      {
      FileInfo[] files = GetSavFileList(sourceBase);
      if (string.IsNullOrEmpty(TSWServiceOptions.LastBackupDate))
        {
        return true;
        }
      foreach (var file in files)
        {
        var dateToCheck = file.LastWriteTime.ToString("yyyy-MM-dd");
        if (string.CompareOrdinal(dateToCheck, TSWServiceOptions.LastBackupDate) > 0)
          {
          return true;
          }
        }
      return false;
      }

    private static void BackUpPart(string SourceBase, string TargetBase, string Folder, bool Included)
      {
      if (Included)
        {
        var Source = SourceBase + Folder;
        var Target = TargetBase + Folder;
        FileHelpers.CopyDir(Source, Target, true);
        }
      }

    private static void BackUpSingleFile(string SourceBase, string TargetBase, string fileName, bool Included)
      {
      if (Included)
        {
        var Source = SourceBase + fileName;
        var Target = TargetBase + fileName;
        Directory.CreateDirectory(TargetBase);
        File.Copy(Source, Target, true);
        }
      }

    private static void DeleteOldBackups(string backupPath)
      {
      DirectoryInfo backupRoot = new DirectoryInfo(backupPath);
      DirectoryInfo[] backupList = backupRoot.GetDirectories();
      string oldBackup = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
      foreach (var backupFolder in backupList)
        {
        var path = $"{backupFolder.FullName}\\Metadata.txt";
        if (File.Exists(path))
          {
          string[] metaData = File.ReadAllText(path).Split('\n');
          if (metaData.Length > 2)
            {
            var backupType = metaData[0].Substring(13);
            var backupDate = metaData[1].Substring(13, 10);
            if (backupType.StartsWith("Automatic daily") && string.CompareOrdinal(backupDate, oldBackup) < 0)
              {
              DeleteBackup(backupFolder.FullName);
              }
            }
          }
        }
      }

    public static void DeleteBackup(string path)
      {
      try
        {
        Directory.Delete(path, true);
        Log.Information("Backup deleted {path}", path);
        }
      catch (Exception ex)
        {
        Log.Error("Cannot delete backup {path} {message}", path, ex.Message);
        }
      }

    public void MakeDailyBackup()
      {
      CreateBackupPath();
      DeleteOldBackups(BackupPath);
      FillBackupList();
      var SourceBase = TSWServiceOptions.GetSaveLocationPath();
      if (TSWServiceOptions.AutoBackup && IsBackupNeeded(SourceBase))
        {
        SetSaveDaily();
        MakeBackup();
        Log.Information("Backup created");
        var message = TSWServiceOptions.UpdateLastbackupDate();
        Log.Information("{message}", message);
        }
      else
        {
        Log.Information("No backup created");
        }
      }

    #region metadata

    private static void WriteBackupMetaData(string targetBase, string metaData)
      {
      var path = $"{targetBase}Metadata.txt";
      File.WriteAllText(path, metaData);
      }

    private string CreateMetaData(string setName)
      {
      var output = string.Empty;
      output += $"Backup type: {setName}\r\n";
      output += $"Backup date: {_backupDateTime?.ToString("yyyy-MM-dd$HHmm")}\r\n";
      output += $"Config: {SaveConfig}\r\n";
      output += $"Savegames: {SaveSaveGames}\r\n";
      output += $"Screenshots: {SaveScreenShots}\r\n";
      output += $"Loadingscreens: {SaveLoadingScreens}\r\n";
      output += $"Logs: {SaveLogs}\r\n";
      output += $"Crashes: {SaveCrashes}\r\n";
      output += $"Scenarios: {SaveScenarios}\r\n";
      output += $"Database: {SaveDatabase}\r\n";
      output += $"Manuals: {SaveManuals}\r\n";
      output += $"Settings: {SaveSettings}";
      output += $"Mods: {SaveMods}\r\n";
      output += $"Creators Club: {SaveCreatorsClub}\r\n";
      return output;
      }
    #endregion
    }
  }
