using Styles.Library.Helpers;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace ToolkitForTSW
  {
  public class CBackup : Notifier
    {

    #region Properties
    /*
  List with all backup sets
  */
    private ObservableCollection<DirectoryInfo> _BackupSetsList;
    public ObservableCollection<DirectoryInfo> BackupSetsList
      {
      get { return _BackupSetsList; }
      set
        {
        _BackupSetsList = value;
        OnPropertyChanged("BackupSetsList");
        }
      }

    private Boolean _SaveConfig;
    public Boolean SaveConfig
      {
      get { return _SaveConfig; }
      set
        {
        _SaveConfig = value;
        OnPropertyChanged("SaveConfig");
        }
      }

    private Boolean _SaveSaveGames;
    public Boolean SaveSaveGames
      {
      get { return _SaveSaveGames; }
      set
        {
        _SaveSaveGames = value;
        OnPropertyChanged("SaveSaveGames");
        }
      }

    private Boolean _SaveLoadingScreens;
    public Boolean SaveLoadingScreens
      {
      get { return _SaveLoadingScreens; }
      set
        {
        _SaveLoadingScreens = value;
        OnPropertyChanged("SaveLoadingScreens");
        }
      }

    private Boolean _SaveLogs;
    public Boolean SaveLogs
      {
      get { return _SaveLogs; }
      set
        {
        _SaveLogs = value;
        OnPropertyChanged("SaveLogs");
        }
      }

    private Boolean _SaveScreenShots;
    public Boolean SaveScreenShots
      {
      get { return _SaveScreenShots; }
      set
        {
        _SaveScreenShots = value;
        OnPropertyChanged("SaveScreenShots");
        }
      }


    private Boolean _saveScenarios;
    public Boolean SaveScenarios
      {
      get { return _saveScenarios; }
      set
        {
        _saveScenarios = value;
        OnPropertyChanged("SaveScenarios");
        }
      }

    private Boolean _SaveCrashes;
    public Boolean SaveCrashes
      {
      get { return _SaveCrashes; }
      set
        {
        _SaveCrashes = value;
        OnPropertyChanged("SaveCrashes");
        }
      }

    private String _Result;
    public String Result
      {
      get { return _Result; }
      set
        {
        _Result = value;
        OnPropertyChanged("Result");
        }
      }
    #endregion

    public CBackup()
      {
      BackupSetsList = new ObservableCollection<DirectoryInfo>();
      FillBackupList();
      SaveConfig = true;
      SaveSaveGames = true;
      SaveScenarios = true;
      }

    private void FillBackupList()
      {
      BackupSetsList.Clear();
      var Path = CTSWOptions.BackupFolder;
      DirectoryInfo DirInfo = new DirectoryInfo(Path);
      var Dirs = DirInfo.GetDirectories("*", SearchOption.TopDirectoryOnly);
      foreach (var X in Dirs)
        {
        BackupSetsList.Add(X);
        }
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
      }


    public String CreateBackupSetName()
      {
      DateTime Now = DateTime.Now;
      return Now.ToString("yyyy-MM-dd$HHmm");
      }

    public void MakeBackup()
      {
      var SourceBase = CTSWOptions.GameSaveLocation;
      var TargetBase = CTSWOptions.BackupFolder + CreateBackupSetName() + "\\";
      BackUpPart(SourceBase, TargetBase, "Saved\\Config\\", SaveConfig);
      BackUpPart(SourceBase, TargetBase, "Saved\\Screenshots\\", SaveScreenShots);
      BackUpPart(SourceBase, TargetBase, "Saved\\LoadingScreens\\", SaveLoadingScreens);
      BackUpPart(SourceBase, TargetBase, "Saved\\Logs\\", SaveLogs);
      BackUpPart(SourceBase, TargetBase, "Saved\\Crashes\\", SaveCrashes);
      BackupSetsList.Add(new DirectoryInfo(TargetBase));
      SaveSaveGamesFiles(SourceBase, TargetBase, SaveSaveGames);
      SaveScenarioFiles(SourceBase, TargetBase, SaveScenarios);
      Result += "Backup succeeded for set " + TargetBase;
      }

    private void SaveScenarioFiles(string sourceBase, string targetBase, bool saveScenarios)
      {
      if (saveScenarios)
        {
        var sourceDir = new DirectoryInfo($"{sourceBase}Saved\\SaveGames\\");
        var targetDir = $"{targetBase}Saved\\SavedGames\\";
        var files = sourceDir.GetFiles("USD_*.sav", SearchOption.TopDirectoryOnly);
        Directory.CreateDirectory(targetDir);
        foreach (var file in files)
          {
          File.Copy(file.FullName, $"{targetDir}{file.Name}", true);
          }
        }
      }

    private void SaveSaveGamesFiles(string sourceBase, string targetBase, bool saveSaveGames)
      {
      if (saveSaveGames)
        {
        var sourceDir = new DirectoryInfo($"{sourceBase}Saved\\SaveGames\\");
        var targetDir = $"{targetBase}Saved\\SavedGames\\";
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

    private void BackUpPart(String SourceBase, String TargetBase, String Folder, Boolean Included)
      {
      if (Included)
        {
        var Source = SourceBase + Folder;
        var Target = TargetBase + Folder;
        CApps.CopyDir(Source, Target, true);
        }
      }


    public void RestoreBackup(String Source)
      {
      var Target = CTSWOptions.GameSaveLocation;
      CApps.CopyDir(Source, Target, true);
      }

    public void DeleteBackup(String Source)
      {
      Directory.Delete(Source, true);
      FillBackupList();
      }


    }
  }
