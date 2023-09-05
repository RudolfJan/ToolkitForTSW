using Caliburn.Micro;
using Logging.Library;
using System;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Windows;
using Utilities.Library;

namespace ToolkitForTSW.ViewModels
  {
  public class BackupViewModel : Screen
    {

    #region Properties
    /*
  List with all backup sets
  */
    public string SetName { get; set; } = string.Empty;

    private BindableCollection<DirectoryInfo> _BackupSetsList;
    public BindableCollection<DirectoryInfo> BackupSetsList
      {
      get { return _BackupSetsList; }
      set
        {
        _BackupSetsList = value;
        NotifyOfPropertyChange(nameof(BackupSetsList));
        }
      }

    private DirectoryInfo _selectedBackupSet;
    public DirectoryInfo SelectedBackupSet
      {
      get
        {
        return _selectedBackupSet;
        }
      set
        {
        _selectedBackupSet = value;
        NotifyOfPropertyChange(nameof(CanDeleteBackup));
        NotifyOfPropertyChange(nameof(CanRestoreBackup));
        NotifyOfPropertyChange(nameof(CanViewMetaData));
        NotifyOfPropertyChange(nameof(CanViewBackupFolder));
        }
      }

    private bool _SaveConfig;
    public bool SaveConfig
      {
      get { return _SaveConfig; }
      set
        {
        _SaveConfig = value;
        NotifyOfPropertyChange(nameof(SaveConfig));
        }
      }

    private bool _SaveSaveGames;
    public bool SaveSaveGames
      {
      get { return _SaveSaveGames; }
      set
        {
        _SaveSaveGames = value;
        NotifyOfPropertyChange(() => SaveSaveGames);
        }
      }

    private bool _SaveLoadingScreens;
    public bool SaveLoadingScreens
      {
      get { return _SaveLoadingScreens; }
      set
        {
        _SaveLoadingScreens = value;
        NotifyOfPropertyChange(() => SaveLoadingScreens);
        }
      }

    private bool _SaveLogs;
    public bool SaveLogs
      {
      get { return _SaveLogs; }
      set
        {
        _SaveLogs = value;
        NotifyOfPropertyChange(() => SaveLogs);
        }
      }

    private bool _SaveScreenShots;
    public bool SaveScreenShots
      {
      get { return _SaveScreenShots; }
      set
        {
        _SaveScreenShots = value;
        NotifyOfPropertyChange(() => SaveScreenShots);
        }
      }

    private bool _saveCreatorsClub;
    public bool SaveCreatorsClub
      {
      get { return _saveCreatorsClub; }
      set
        {
        _saveCreatorsClub = value;
        NotifyOfPropertyChange(() => SaveCreatorsClub);
        }
      }


    private bool _saveScenarios;
    public bool SaveScenarios
      {
      get { return _saveScenarios; }
      set
        {
        _saveScenarios = value;
        NotifyOfPropertyChange(() => SaveScenarios);
        }
      }

    private bool _SaveCrashes;
    public bool SaveCrashes
      {
      get { return _SaveCrashes; }
      set
        {
        _SaveCrashes = value;
        NotifyOfPropertyChange(() => SaveCrashes);
        }
      }

    private bool _SaveDatabase;
    public bool SaveDatabase
      {
      get { return _SaveDatabase; }
      set
        {
        _SaveDatabase = value;
        NotifyOfPropertyChange(() => SaveDatabase);
        }
      }

    private bool _SaveManuals;
    public bool SaveManuals
      {
      get { return _SaveManuals; }
      set
        {
        _SaveManuals = value;
        NotifyOfPropertyChange(() => SaveManuals);
        }
      }

    private bool _SaveSettings;
    public bool SaveSettings
      {
      get { return _SaveSettings; }
      set
        {
        _SaveSettings = value;
        NotifyOfPropertyChange(() => SaveSettings);
        }
      }

    private bool _SaveMods;
    public bool SaveMods
      {
      get { return _SaveMods; }
      set
        {
        _SaveMods = value;
        NotifyOfPropertyChange(() => SaveMods);
        }
      }


    private string _BackupPath;
    public string BackupPath
      {
      get { return _BackupPath; }
      set
        {
        _BackupPath = value;
        NotifyOfPropertyChange(() => BackupPath);
        }
      }

    private string _Result;
    public string Result
      {
      get { return _Result; }
      set
        {
        _Result = value;
        NotifyOfPropertyChange(() => Result);
        }
      }
    #endregion

    public BackupViewModel()
      {
      BackupSetsList = new BindableCollection<DirectoryInfo>();
      BackupPath = $"{TSWOptions.BackupFolder}{TSWOptions.GetPlatformFolderName(TSWOptions.CurrentPlatform)}\\";
      FillBackupList(BackupPath);
      SetSaveDefault();

      }

    private void FillBackupList(string backupPath)
      {
      BackupSetsList.Clear();
      if (!Directory.Exists(backupPath))
        {
        Log.Trace($"Backup folder {backupPath} does not exist. Did you forget to save your options", LogEventType.Error);
        }
      DirectoryInfo DirInfo = new DirectoryInfo(backupPath);
      var Dirs = DirInfo.GetDirectories("*", SearchOption.TopDirectoryOnly);
      foreach (var X in Dirs)
        {
        BackupSetsList.Add(X);
        }
      NotifyOfPropertyChange(() => BackupSetsList);
      NotifyOfPropertyChange(() => SelectedBackupSet);
      }

    private void SetSaveDefault()
      {
      SaveConfig = true;
      SaveSaveGames = true;
      SaveScenarios = true;
      SaveDatabase = true;
      SaveManuals = true;
      SaveSettings = true;
      SaveMods = true;
      SaveCreatorsClub = false;
      SetName = "Manual";
      }

    public void SetSaveAll()
      {
      SaveConfig = true;
      SaveSaveGames = true;
      SaveScreenShots = true;
      SaveLoadingScreens = true;
      SaveLogs = true;
      SaveCrashes = true;
      SaveScenarios = true;
      SaveDatabase = true;
      SaveManuals = true;
      SaveSettings = true;
      SaveMods = true;
      SaveCreatorsClub = true;
      SetName = "Manual";
      }

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
      SetName = "Daily";
      }

    public void SetSaveNone()
      {
      SaveScenarios = false;
      SaveConfig = false;
      SaveSaveGames = false;
      SaveScreenShots = false;
      SaveLoadingScreens = false;
      SaveLogs = false;
      SaveCrashes = false;
      SaveDatabase = false;
      SaveManuals = false;
      SaveSettings = false;
      SaveMods = false;
      SaveCreatorsClub = false;
      SetName = "Manual";
      }

    private DateTime? _backupDateTime;
    public string CreateBackupSetName()
      {
      _backupDateTime = DateTime.Now;
      return _backupDateTime?.ToString("yyyy-MM-dd$HHmm");
      }

    public void MakeDailyBackup()
      {
      FillBackupList(BackupPath);
      var TargetBase = BackupPath + CreateBackupSetName() + "\\";
      var datePart = TargetBase.Substring(0, TargetBase.Length - 6);
      var shouldDo = BackupSetsList.Where(x => x.FullName.Substring(0, x.FullName.Length - 5) == datePart).FirstOrDefault();


      if (shouldDo == null)
        {
        SetSaveDaily();
        MakeBackup();
        SetSaveDefault();
        }
      }

    public void MakeBackup()
      {
      var SourceBase = TSWOptions.GetSaveLocationPath();
      var ToolkitBase = TSWOptions.ToolkitForTSWFolder;
      var TargetBase = BackupPath + CreateBackupSetName() + "\\";
      BackUpPart(SourceBase, TargetBase, "Saved\\Config\\", SaveConfig);
      BackUpPart(SourceBase, TargetBase, "Saved\\Screenshots\\", SaveScreenShots);
      BackUpPart(SourceBase, TargetBase, "Saved\\LoadingScreens\\", SaveLoadingScreens);
      BackUpPart(SourceBase, TargetBase, "Saved\\Logs\\", SaveLogs);
      BackUpPart(SourceBase, TargetBase, "Saved\\Crashes\\", SaveCrashes);
      BackUpPart(SourceBase, TargetBase, "Saved\\PersistentDownloadDir\\UGC\\0\\", SaveCreatorsClub);
      BackUpToolkitPart(TSWOptions.ManualsFolder, TargetBase + "ToolKit\\Manuals\\", SaveManuals);
      BackUpToolkitPart(TSWOptions.GetSaveLocationPath(), TargetBase + "ToolKit\\OptionsSets\\", SaveSettings);
      BackUpToolkitPart(TSWOptions.ModsFolder, TargetBase + "ToolKit\\Mods\\", SaveMods);
      BackUpSingleFile(ToolkitBase, TargetBase + "ToolKit\\", "TSWTools.db", SaveDatabase);
      BackupSetsList.Add(new DirectoryInfo(TargetBase));
      SaveSaveGamesFiles(SourceBase, TargetBase, SaveSaveGames);
      SaveScenarioFiles(SourceBase, TargetBase, SaveScenarios);
      WriteBackupMetaData(TargetBase, CreateMetaData(SetName));

      Result += "Backup succeeded for set " + TargetBase;
      }

    private static void SaveScenarioFiles(string sourceBase, string targetBase, bool saveScenarios)
      {
      if (saveScenarios)
        {
        var sourceDir = new DirectoryInfo($"{sourceBase}Saved\\SaveGames\\");
        var targetDir = $"{targetBase}Saved\\SaveGames\\";
        var files = sourceDir.GetFiles("USD_*.sav", SearchOption.TopDirectoryOnly);
        Directory.CreateDirectory(targetDir);
        foreach (var file in files)
          {
          File.Copy(file.FullName, $"{targetDir}{file.Name}", true);
          }
        }
      }

    private static void SaveSaveGamesFiles(string sourceBase, string targetBase, bool saveSaveGames)
      {
      if (saveSaveGames)
        {
        var sourceDir = new DirectoryInfo($"{sourceBase}Saved\\SaveGames\\");
        var targetDir = $"{targetBase}Saved\\SaveGames\\";
        Directory.CreateDirectory(targetDir);
        var files = sourceDir.GetFiles("*.sav", SearchOption.TopDirectoryOnly);
        foreach (var file in files)
          {
          if (!file.Name.StartsWith("USD_"))
            {
            File.Copy(file.FullName, $"{targetDir}{file.Name}", true);
            }
          }
        }
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
    private static void BackUpToolkitPart(string SourceFolder, string TargetBase, bool Included)
      {
      if (Included)
        {
        FileHelpers.CopyDir(SourceFolder, TargetBase, true);
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

    public bool CanRestoreBackup
      {
      get
        {
        return SelectedBackupSet != null;
        }
      }

    public bool CanDeleteBackup
      {
      get
        {
        return SelectedBackupSet != null;
        }
      }

    public void RestoreBackup()
      {
      var source = SelectedBackupSet.FullName;
      var target = TSWOptions.GetSaveLocationPath() + "Saved\\";
      FileHelpers.CopyDir($"{source}\\Saved\\", target, true);
      RestoreToolkitFiles($"{source}\\Toolkit\\");
      }

    public static void RestoreToolkitFiles(string source)
      {
      var target = TSWOptions.ToolkitForTSWFolder;
      // var target="D:\\Test\\";
      if (Directory.Exists(source))
        {
        FileHelpers.CopyDir(source, target, true);
        }
      else
        {
        Log.Trace("ToolkitForTSW data not restored because the data is not in backup set", LogEventType.InformUser);
        }
      }

    public void DeleteBackup()
      {
      // TODO may cause game crash, needs better fault protection
      var source = SelectedBackupSet.FullName;
      try
        {
        Directory.Delete(source, true);
        }
      catch (Exception ex)
        {
        Log.Trace($"Cannot delete backup set because {ex.Message}", ex, LogEventType.InformUser);
        }
      FillBackupList(BackupPath);
      }

    #region backgroundservice     
    public static bool GetBackupServiceStatus()
      {
      //check service
      var services = ServiceController.GetServices();
      foreach (var s in services)
        {
        if (s.ServiceName == "ToolkitForTSWService")
          {
          return true;
          }
        }
      return false;
      }
    public static void StartBackupService()
      {
      CApps.ExecuteFile("Scripts\\StartBackupService.bat", runAsAdmin: true);
      }

    public static void StopBackupService()
      {
      CApps.ExecuteFile("Scripts\\StopBackupService.bat");
      }

    public static void SetBackupServiceState()
      {
      if (TSWOptions.AutoBackup && !GetBackupServiceStatus())
        {
        StartBackupService();
        }
      if (!TSWOptions.AutoBackup && GetBackupServiceStatus())
        {
        StopBackupService();
        }
      }

    #endregion

    #region metadata
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

    private static void WriteBackupMetaData(string targetBase, string metaData)
      {
      var path = $"{targetBase}Metadata.txt";
      File.WriteAllText(path, metaData);
      }

    public bool CanViewMetaData
      {
      get
        {
        return SelectedBackupSet != null;
        }
      }

    public void ViewMetaData()
      {
      string metaData = string.Empty;
      var path = $"{SelectedBackupSet.FullName}\\Metadata.txt";
      if (File.Exists(path))
        {
        metaData = File.ReadAllText(path);
        }
      if (string.IsNullOrEmpty(metaData))
        {
        metaData = "No metadata available";
        }
      MessageBox.Show(metaData, "Metadata", MessageBoxButton.OK, MessageBoxImage.Information);
      }


    public bool CanViewBackupFolder
      {
      get
        {
        return SelectedBackupSet != null;
        }
      }
    public void ViewBackupFolder()
      {
      var path = SelectedBackupSet.FullName;
      if (Directory.Exists(path))
        {
        ProcessHelper.OpenFolder(path);
        }
      }

    #endregion
    }
  }
