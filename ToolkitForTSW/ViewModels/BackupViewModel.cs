using Caliburn.Micro;
using Logging.Library;
using System;
using System.IO;
using System.Linq;
using Utilities.Library;

namespace ToolkitForTSW.ViewModels
  {
  public class BackupViewModel : Screen
    {

    #region Properties
    /*
  List with all backup sets
  */
    private BindableCollection<DirectoryInfo> _BackupSetsList;
    public BindableCollection<DirectoryInfo> BackupSetsList
      {
      get { return _BackupSetsList; }
      set
        {
        _BackupSetsList = value;
        NotifyOfPropertyChange(() => BackupSetsList);
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
        NotifyOfPropertyChange(() => CanDeleteBackup);
        NotifyOfPropertyChange(() => CanRestoreBackup);
        }
      }

    private bool _SaveConfig;
    public bool SaveConfig
      {
      get { return _SaveConfig; }
      set
        {
        _SaveConfig = value;
        NotifyOfPropertyChange(() => SaveConfig);
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
      }

    public static string CreateBackupSetName()
      {
      DateTime Now = DateTime.Now;
      return Now.ToString("yyyy-MM-dd$HHmm");
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
      BackUpToolkitPart(TSWOptions.ManualsFolder, TargetBase + "ToolKit\\Manuals\\", SaveManuals);
      BackUpToolkitPart(TSWOptions.GetSaveLocationPath(), TargetBase + "ToolKit\\OptionsSets\\", SaveSettings);
      BackUpToolkitPart(TSWOptions.ModsFolder, TargetBase + "ToolKit\\Mods\\", SaveMods);
      BackUpSingleFile(ToolkitBase, TargetBase + "ToolKit\\", "TSWTools.db", SaveDatabase);
      BackupSetsList.Add(new DirectoryInfo(TargetBase));
      SaveSaveGamesFiles(SourceBase, TargetBase, SaveSaveGames);
      SaveScenarioFiles(SourceBase, TargetBase, SaveScenarios);
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
      FileHelpers.CopyDir(source, target, true);
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
    }
  }
