using Caliburn.Micro;
using Logging.Library;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using ToolkitForTSW.DataAccess;
using ToolkitForTSW.Mod.Models;
using ToolkitForTSW.Models;
using Utilities.Library;

namespace ToolkitForTSW.Mod.ViewModels
  {
  public class ModStatusViewModel: Screen, IModTabManager

    {
    private BindableCollection<ModModel> _AvailableModList = new BindableCollection<ModModel>();
    public BindableCollection<ModModel> AvailableModList
      {
      get { return _AvailableModList; }
      set
        {
        _AvailableModList = value;
        NotifyOfPropertyChange("AvailableModList");
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
        NotifyOfPropertyChange("PakFilesList");
        }
      }

    private ModSetViewModel _modSet;
    public ModSetViewModel ModSet
      {
      get { return _modSet; }
      set
        {
        _modSet = value;
        NotifyOfPropertyChange("ModSet");
        }
      }


    private string _modName;
    public string ModName
      {
      get { return _modName; }
      set
        {
        _modName = value;
        NotifyOfPropertyChange(() => ModName);
        NotifyOfPropertyChange(() => CanSaveModProperties);
        }
      }

    private string _modVersion;
    public string ModVersion
      {
      get { return _modVersion; }
      set
        {
        _modVersion = value;
        NotifyOfPropertyChange("ModVersion");
        }
      }

    private string _filePath;
    public string FilePath
      {
      get { return _filePath; }
      set
        {
        _filePath = value;
        NotifyOfPropertyChange("FilePath");
        }
      }

    private string _fileName;
    public string FileName
      {
      get { return _fileName; }
      set
        {
        _fileName = value;
        NotifyOfPropertyChange("FileName");
        }
      }

    private string _modDescription;
    public string ModDescription
      {
      get { return _modDescription; }
      set
        {
        _modDescription = value;
        NotifyOfPropertyChange("ModDescription");
        }
      }

    private string _modImage;
    public string ModImage
      {
      get { return _modImage; }
      set
        {
        _modImage = value;
        NotifyOfPropertyChange("ModImage");
        }
      }

    private string _modSource;
    public string ModSource
      {
      get { return _modSource; }
      set
        {
        _modSource = value;
        NotifyOfPropertyChange("ModSource");
        }
      }

    private ModTypesEnum _modType;
    public ModTypesEnum ModType
      {
      get { return _modType; }
      set
        {
        _modType = value;
        NotifyOfPropertyChange("ModType");
        }
      }

    private string _DLCName;
    public string DLCName
      {
      get { return _DLCName; }
      set
        {
        _DLCName = value;
        NotifyOfPropertyChange(() => DLCName);
        }
      }

    private ModModel _SelectedMod;

    public ModModel SelectedMod
      {
      get { return _SelectedMod; }
      set
        {
        _SelectedMod = value;
        NotifyOfPropertyChange (() => SelectedMod);
        NotifyOfPropertyChange(() => CanEditModProperties);
        NotifyOfPropertyChange(() => CanDeleteMod);
        NotifyOfPropertyChange(() => CanSaveModProperties);
        NotifyOfPropertyChange(() => CanDeactivateMod);
        NotifyOfPropertyChange(() => CanActivateSteamMod);
        NotifyOfPropertyChange(() => CanActivateEGSMod);
        NotifyOfPropertyChange(() => IsInstalledSteam);
        NotifyOfPropertyChange(() => IsInstalledEGS);
        }
      }

    private bool _IsInstalledSteam;
    public bool IsInstalledSteam
      {
      get { return _IsInstalledSteam; }
      set
        {
        _IsInstalledSteam = value;
        NotifyOfPropertyChange(() => IsInstalledSteam);
        }
      }

    private bool _IsInstalledEGS;
    public bool IsInstalledEGS
      {
      get { return _IsInstalledEGS; }
      set
        {
        _IsInstalledEGS = value;
        NotifyOfPropertyChange("IsInstalledEGS");
        }
      }

    private bool _InEditMode;
    public bool InEditMode
      {
      get { return _InEditMode; }
      set
        {
        _InEditMode = value;
        NotifyOfPropertyChange("InEditMode");
        }
      }

    public ModStatusViewModel()
      {
      DisplayName="Mods";
      }
    
    public void Initialise(BindableCollection<ModModel> availableModList)
      {
      AvailableModList = availableModList;
      if (AvailableModList==null)
        {
        throw new Exception("AvailableModList should be filled");
        }
      PakFilesList = new ObservableCollection<FileInfo>();
      
      GetPakFiles();
      GetInstalledPakFiles();
      }

    public void DeactivateAllMods()
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
      if (TSWOptions.SteamProgramDirectory.Length > 0)
        {
        GetInstalledPakFiles(PlatformEnum.Steam);
        }
      if (TSWOptions.EGSTrainSimWorldDirectory.Length > 0)
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

    public static String StripModDir(String Input)
      {
      if (Input.StartsWith(TSWOptions.ModsFolder))
        {
        return Input.Substring(TSWOptions.ModsFolder.Length);
        }
      return String.Empty;
      }

    public bool CanActivateSteamMod 
      { 
      get
        {
        return SelectedMod!=null && TSWOptions.SteamProgramDirectory.Length>10; // This is a simplified check. It will not always work but there is a catchall that checks for existence of the Folder.
        }

      }

    public void ActivateSteamMod()
      {
      ActivateMod(SelectedMod,PlatformEnum.Steam,false);
      }

    public bool CanActivateEGSMod 
      { 
      get
        {
        return SelectedMod!=null && TSWOptions.EGSTrainSimWorldDirectory.Length>5;
        }
      }

    public void ActivateEGSMod()
      {
      ActivateMod(SelectedMod,PlatformEnum.EpicGamesStore,false);
      }


 

 

    public bool CanDeactivateMod 
      { get
        {
        return SelectedMod!=null;
        }
      }

    public void DeactivateMod()
      {
      if (TSWOptions.SteamProgramDirectory.Length > 0)
        {
        DeactivateMod(PlatformEnum.Steam);
        }
      if (TSWOptions.EGSTrainSimWorldDirectory.Length > 0)
        {
        DeactivateMod(PlatformEnum.EpicGamesStore);
        }
      }

    public void DeactivateMod(PlatformEnum selectedPlatform)
      {
      var filePath = $"{GetBaseDir(selectedPlatform).FullName}\\{SelectedMod.FileName}";
      FileHelpers.DeleteSingleFile(filePath);
      SetInstallationStatus(selectedPlatform, false);
      }

    public static void ActivateMod(ModModel mod, PlatformEnum selectedPlatform, bool overWrite)
      {
      var PakPath = mod.FilePath;
      var source = TSWOptions.ModsFolder + PakPath;
      var fileName = Path.GetFileName(source);
      var destination = $"{GetBaseDir(selectedPlatform).FullName}\\{fileName}";

      try
        {
        File.Copy(source, destination, overWrite);
        
        if(selectedPlatform == PlatformEnum.Steam)
          {
          mod.IsInstalledSteam = true;
          }
        else
          {
          mod.IsInstalledEGS = true;
          }
        }
      catch (Exception E)
        {
        Log.Trace("Failed to install mod pak because " + E.Message,
          LogEventType.Error);
        }
      }

 
    public bool CanEditModProperties { 
      get
        {
        return SelectedMod!=null;
        } 
      }

    public void EditModProperties()
      {
      ModName = SelectedMod.ModName;
      ModDescription = SelectedMod.ModDescription;
      ModSource = SelectedMod.ModSource;
      ModType = SelectedMod.ModType;
      ModVersion = SelectedMod.ModVersion;
      DLCName = SelectedMod.DLCName;
      FileName = SelectedMod.FileName;
      FilePath = SelectedMod.FilePath;
      IsInstalledSteam = SelectedMod.IsInstalledSteam;
      IsInstalledEGS = SelectedMod.IsInstalledEGS;
      InEditMode = true;
      }

    public bool CanSaveModProperties { 
      get
        {
        return SelectedMod!=null && ModName?.Length>2;
        }
      }
    public void SaveModProperties()
      {
      var newMod = new ModModel
        {
        ModName = ModName,
        ModDescription = ModDescription,
        ModSource = ModSource,
        DLCName = DLCName,
        ModType = ModType,
        ModVersion = ModVersion,
        Id = SelectedMod.Id,
        FilePath = SelectedMod.FilePath,
        FileName = SelectedMod.FileName
        };
      ModDataAccess.UpdateMod(newMod);
      AvailableModList.Remove(SelectedMod);
      AvailableModList.Add(newMod);
      SelectedMod=null;
      NotifyOfPropertyChange(() => SelectedMod);
      NotifyOfPropertyChange(() => AvailableModList); //TODO check
      // Note FileName and FilePath should never be updated!
      }

    public static string UpdateModTable(ModModel mod)
      {
      ModDataAccess.UpsertMod(mod);
      return "";
      }

    public bool CanDeleteMod 
      { 
      get
        {
        return SelectedMod!=null;
        }
      }

    public void DeleteMod()
      {
      if (SelectedMod.IsInstalledSteam)
        {
        DeactivateMod(PlatformEnum.Steam);
        }
      if (SelectedMod.IsInstalledEGS)
        {
        DeactivateMod(PlatformEnum.EpicGamesStore);
        }
      var PakPath = SelectedMod.FilePath;
      var source = TSWOptions.ModsFolder + PakPath;
      FileHelpers.DeleteSingleFile(source);
      ModDataAccess.DeleteMod(SelectedMod.Id);
      AvailableModList.Remove(SelectedMod);
      if (InEditMode)
        {
        ClearEdit();
        }
      SelectedMod = null;
      NotifyOfPropertyChange(() => SelectedMod);
      NotifyOfPropertyChange(() => AvailableModList); //TODO check
      }

    public void ClearEdit()
      {
      SelectedMod = null;
      ModName = string.Empty;
      ModDescription = string.Empty;
      ModSource = string.Empty;
      ModType = ModTypesEnum.Undefined;
      ModVersion = string.Empty;
      DLCName = string.Empty;
      FileName = string.Empty;
      FilePath = string.Empty;
      IsInstalledSteam = false;
      IsInstalledEGS = false;
      InEditMode = false;
      }




    }
  }
