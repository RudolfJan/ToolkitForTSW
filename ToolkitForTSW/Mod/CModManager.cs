using Logging.Library;
using Styles.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using ToolkitForTSW.DataAccess;
using ToolkitForTSW.Models;
using Utilities.Library;

namespace ToolkitForTSW.Mod
  {
  public enum ModTypesEnum

    {
    [Description("Undefined")] Undefined,
    [Description("Engine")] Engine,
    [Description("Wagon")] Wagon,
    [Description("Consist")] Consist,
    [Description("Scenery")] Scenery,
    [Description("Route")] Route,
    [Description("Scenario")] Scenario,
    [Description("Service timetable")] Service,
    [Description("Weather")] Weather,
    [Description("Game")] Game,
    [Description("Other")] Other
    };

  public class CModManager : Notifier
    {
    private List<ModModel> _AvailableModList = new List<ModModel>();
    public List<ModModel> AvailableModList
      {
      get { return _AvailableModList; }
      set
        {
        _AvailableModList = value;
        OnPropertyChanged("AvailableModList");
        }
      }


    /*
		List with all (DLC) Pak files. 
		*/
    private ObservableCollection<FileInfo> _PakFilesList;
    public ObservableCollection<FileInfo> PakFilesList
      {
      get { return _PakFilesList; }
      set
        {
        _PakFilesList = value;
        OnPropertyChanged("PakFilesList");
        }
      }

    private String _Result = String.Empty;
    public String Result
      {
      get { return _Result; }
      set
        {
        _Result = value;
        OnPropertyChanged("Result");
        }
      }

    private CModSet _modSet;
    public CModSet ModSet
      {
      get { return _modSet; }
      set
        {
        _modSet = value;
        OnPropertyChanged("ModSet");
        }
      }


    private string _modName;
    public string ModName
      {
      get { return _modName; }
      set
        {
        _modName = value;
        OnPropertyChanged("ModName");
        }
      }

    private string _modVersion;
    public string ModVersion
      {
      get { return _modVersion; }
      set
        {
        _modVersion = value;
        OnPropertyChanged("ModVersion");
        }
      }

    private string _filePath;
    public string FilePath
      {
      get { return _filePath; }
      set
        {
        _filePath = value;
        OnPropertyChanged("FilePath");
        }
      }

    private string _fileName;
    public string FileName
      {
      get { return _fileName; }
      set
        {
        _fileName = value;
        OnPropertyChanged("FileName");
        }
      }

    private string _modDescription;
    public string ModDescription
      {
      get { return _modDescription; }
      set
        {
        _modDescription = value;
        OnPropertyChanged("ModDescription");
        }
      }

    private string _modImage;
    public string ModImage
      {
      get { return _modImage; }
      set
        {
        _modImage = value;
        OnPropertyChanged("ModImage");
        }
      }

    private string _modSource;
    public string ModSource
      {
      get { return _modSource; }
      set
        {
        _modSource = value;
        OnPropertyChanged("ModSource");
        }
      }

    private ModTypesEnum _modType;
    public ModTypesEnum ModType
      {
      get { return _modType; }
      set
        {
        _modType = value;
        OnPropertyChanged("ModType");
        }
      }

    private string _DLCName;
    public string DLCName
      {
      get { return _DLCName; }
      set
        {
        _DLCName = value;
        OnPropertyChanged("DLCName");
        }
      }

    private ModModel _SelectedMod;

    public ModModel SelectedMod
      {
      get { return _SelectedMod; }
      set
        {
        _SelectedMod = value;
        OnPropertyChanged("SelectedMod");
        }
      }

    private bool _IsInstalledSteam;
    public bool IsInstalledSteam
      {
      get { return _IsInstalledSteam; }
      set
        {
        _IsInstalledSteam = value;
        OnPropertyChanged("IsInstalledSteam");
        }
      }

    private bool _IsInstalledEGS;
    public bool IsInstalledEGS
      {
      get { return _IsInstalledEGS; }
      set
        {
        _IsInstalledEGS = value;
        OnPropertyChanged("IsInstalledEGS");
        }
      }

    private bool _InEditMode;
    public bool InEditMode
      {
      get { return _InEditMode; }
      set
        {
        _InEditMode = value;
        OnPropertyChanged("InEditMode");
        }
      }


    public CModManager()
      {

      Initialise();
      }

    private void Initialise()
      {
      PakFilesList = new ObservableCollection<FileInfo>();
      AvailableModList = ModDataAccess.GetAllMods();
      ModSet = new CModSet(AvailableModList);
      GetPakFiles();
      GetInstalledPakFiles();
      }

    public void DeactivateAllInstalledPaks()
      {
      foreach (var X in AvailableModList)
        {
        if (X.IsInstalledSteam)
          {
          var filePath = X.FileName;
          if (!String.IsNullOrEmpty(filePath))
            {
            FilePath = TSWOptions.SteamTrainSimWorldDirectory + @"TS2Prototype\Content\DLC\" + filePath;
            FileHelpers.DeleteSingleFile(filePath);
            X.IsInstalledSteam = false;
            }
          }
        if (X.IsInstalledEGS)
          {
          var filePath = X.FileName;
          if (!String.IsNullOrEmpty(filePath))
            {
            FilePath = TSWOptions.EGSTrainSimWorldDirectory + @"TS2Prototype\Content\DLC\" + filePath;
            FileHelpers.DeleteSingleFile(filePath);
            X.IsInstalledEGS = false;
            }
          }
        }
      }

    private void GetPakFiles()
      {
      var BaseDir = new DirectoryInfo(TSWOptions.ModsFolder);
      FileInfo[] Files = BaseDir.GetFiles("*.pak", SearchOption.AllDirectories);
      PakFilesList.Clear();
      AvailableModList.Clear();
      foreach (var F in Files)
        {
        PakFilesList.Add(F);
        var mod = ModDataAccess.UpsertMod(StripModDir(F.FullName));
        AvailableModList.Add(mod);
        }
      }

    private void GetInstalledPakFiles()
      {
      if(TSWOptions.SteamProgramDirectory.Length>0)
        {
        GetInstalledPakFiles(PlatformEnum.Steam);
        }
      if(TSWOptions.EGSTrainSimWorldDirectory.Length>0)
        { 
        GetInstalledPakFiles(PlatformEnum.EpicGamesStore);
        }
      }

    private void GetInstalledPakFiles(PlatformEnum selectedPlatform)
      {
      var BaseDir = GetBaseDir(selectedPlatform);
      FileInfo[] Files = BaseDir.GetFiles("*.pak", SearchOption.TopDirectoryOnly);
      foreach (var F in Files)
        {
        SetInstallationStatus(F, selectedPlatform);
        }
      }

    private static DirectoryInfo GetBaseDir(PlatformEnum selectedPlatform)
      {
      DirectoryInfo BaseDir;
      switch (selectedPlatform)
        {
        case PlatformEnum.Steam:
            {
            BaseDir = new DirectoryInfo(TSWOptions.SteamTrainSimWorldDirectory + "TS2Prototype\\Content\\DLC");
            break;
            }
        case PlatformEnum.EpicGamesStore:
            {
            BaseDir = new DirectoryInfo(TSWOptions.EGSTrainSimWorldDirectory + "TS2Prototype\\Content\\DLC");
      
            break;
            }
        default:
            {
            throw new NotImplementedException("Platform in ModManager not yet supported");
            }
        }
      // If no DLC are installed, this directory is missing. In this case, create it so we can install mods.

      // This also may happen for the Steam version, especially for new players
      if (!Directory.Exists(BaseDir.FullName))
        {
        Directory.CreateDirectory(BaseDir.FullName);
        }
      return BaseDir;
      }

    private void SetInstallationStatus(FileInfo f, PlatformEnum selectedPlatform)
      {
      var fileName = f.Name;
      foreach (var mod in AvailableModList)
        {
        if (string.CompareOrdinal(fileName, mod.FileName) == 0)
          {
          switch (selectedPlatform)
            {
            case PlatformEnum.Steam:
              mod.IsInstalledSteam = true;
              return;
            case PlatformEnum.EpicGamesStore:
              mod.IsInstalledEGS = true;
              return;
            default:
              throw new NotImplementedException("Platform in ModManager not yet supported");
            }
          }
        }
      }

    public static String StripModDir(String Input)
      {
      if (Input.StartsWith(TSWOptions.ModsFolder))
        {
        return Input.Substring(TSWOptions.ModsFolder.Length);
        }
      return String.Empty;
      }

    public void ActivatePak()
      {
      if (TSWOptions.SteamProgramDirectory.Length > 0)
        {
        ActivatePak(PlatformEnum.Steam);
        }
      if (TSWOptions.EGSTrainSimWorldDirectory.Length > 0)
        {
        ActivatePak(PlatformEnum.EpicGamesStore);
        }
      }

    public void ActivatePak(PlatformEnum selectedPlatform)
      {
      var PakPath = SelectedMod.FilePath;
      var source = TSWOptions.ModsFolder + PakPath;
      var fileName = Path.GetFileName(source);
      var destination = $"{GetBaseDir(selectedPlatform).FullName}\\{fileName}";
      try
        {
        File.Copy(source, destination, false);
        var F = new FileInfo(destination);
        SetInstallationStatus(selectedPlatform,true);
        }
      catch (Exception E)
        {
        Result += Log.Trace("Failed to install mod pak because " + E.Message,
          LogEventType.Error);
        }
      }

    private void SetInstallationStatus(PlatformEnum selectedPlatform, bool installationStatus)
      {
      switch (selectedPlatform)
        {
        case PlatformEnum.Steam:
            {
            if (InEditMode)
              {
              IsInstalledSteam = installationStatus;
              }
            SelectedMod.IsInstalledSteam = installationStatus;
            break;
            }
        case PlatformEnum.EpicGamesStore:
            {
            if (InEditMode)
              {
              IsInstalledEGS = installationStatus;
              }
            SelectedMod.IsInstalledEGS = installationStatus;
            break;
            }
        }
      }

    public void DeactivatePak()
      {
      if (TSWOptions.SteamProgramDirectory.Length > 0)
        {
        DeactivatePak(PlatformEnum.Steam);
        }
      if (TSWOptions.EGSTrainSimWorldDirectory.Length > 0)
        {
        DeactivatePak(PlatformEnum.EpicGamesStore);
        }
      }

    public void DeactivatePak(PlatformEnum selectedPlatform)
      {
      var filePath = $"{GetBaseDir(selectedPlatform).FullName}\\{SelectedMod.FileName}";
      Result += FileHelpers.DeleteSingleFile(filePath);
      SetInstallationStatus(selectedPlatform,false);
      }

    public static string ActivateMod(ModModel mod, PlatformEnum selectedPlatform)
      {
      var PakPath = mod.FilePath;
      var source = TSWOptions.ModsFolder + PakPath;
      var fileName = Path.GetFileName(source);
      var destination = GetBaseDir(selectedPlatform).FullName + fileName;
      try
        {
        File.Copy(source, destination, true);
        return string.Empty;
        }
      catch (Exception E)
        {
        return Log.Trace("Failed to install mod pak because " + E.Message,
          LogEventType.Error);
        }
      }

    public void EditModProperties()
      {
      ModName = SelectedMod.ModName;
      ModDescription = SelectedMod.ModDescription;
      ModSource = SelectedMod.ModSource;
      ModType = SelectedMod.ModType;
      ModVersion=SelectedMod.ModVersion;
      DLCName = SelectedMod.DLCName;
      FileName = SelectedMod.FileName;
      FilePath = SelectedMod.FilePath;
      IsInstalledSteam = SelectedMod.IsInstalledSteam;
      IsInstalledEGS = SelectedMod.IsInstalledEGS;
      InEditMode = true;
      }

    public void SaveModProperties()
      {
      SelectedMod.ModName = ModName;
      SelectedMod.ModDescription = ModDescription;
      SelectedMod.ModSource = ModSource;
      SelectedMod.DLCName = DLCName;
      SelectedMod.ModType = ModType;
      SelectedMod.ModVersion=ModVersion;
      ModDataAccess.UpdateMod(SelectedMod);
      // Note FileName and FilePath should never be updated!
      }

    public static string UpdateModTable(ModModel mod)
      {
      ModDataAccess.UpsertMod(mod);
      return "";
      }

    internal void DeleteMod()
      {
      if (SelectedMod.IsInstalledSteam)
        {
        DeactivatePak(PlatformEnum.Steam);
        }
      if (SelectedMod.IsInstalledEGS)
        {
        DeactivatePak(PlatformEnum.EpicGamesStore);
        }
      var PakPath = SelectedMod.FilePath;
      var source = TSWOptions.ModsFolder + PakPath;
      FileHelpers.DeleteSingleFile(source);
      ModDataAccess.DeleteMod(SelectedMod.Id);
      if (InEditMode)
        {
        ClearEdit();
        }
      }

    public void ClearEdit()
      {
      SelectedMod = null;
      ModName = string.Empty;
      ModDescription = string.Empty;
      ModSource = string.Empty;
      ModType = ModTypesEnum.Undefined;
      ModVersion= string.Empty;
      DLCName = string.Empty;
      FileName = string.Empty;
      FilePath = string.Empty;
      IsInstalledSteam = false;
      IsInstalledEGS=false;
      InEditMode = false;
      }
    }
  }
