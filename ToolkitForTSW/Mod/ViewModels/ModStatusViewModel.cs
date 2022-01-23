using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.IO;
using ToolkitForTSW.DataAccess;
using ToolkitForTSW.Mod.Models;
using ToolkitForTSW.Models;
using Utilities.Library;

namespace ToolkitForTSW.Mod.ViewModels
  {
  public class ModStatusViewModel : Screen, IModTabManager

    {
    public List<ModModel> AvailableModList { get; set; } = new List<ModModel>();
    private BindableCollection<ModModel> _SelectedAvailableModList = new BindableCollection<ModModel>();
    public BindableCollection<ModModel> SelectedAvailableModList
      {
      get { return _SelectedAvailableModList; }
      set
        {
        _SelectedAvailableModList = value;
        NotifyOfPropertyChange(nameof(SelectedAvailableModList));
        }
      }

    private ModSetViewModel _modSet;
    public ModSetViewModel ModSet
      {
      get { return _modSet; }
      set
        {
        _modSet = value;
        NotifyOfPropertyChange(nameof(ModSet));
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
        NotifyOfPropertyChange(nameof(ModVersion));
        }
      }

    private string _filePath;
    public string FilePath
      {
      get { return _filePath; }
      set
        {
        _filePath = value;
        NotifyOfPropertyChange(nameof(FilePath));
        }
      }

    private string _fileName;
    public string FileName
      {
      get { return _fileName; }
      set
        {
        _fileName = value;
        NotifyOfPropertyChange(nameof(FileName));
        }
      }

    private string _modDescription;
    public string ModDescription
      {
      get { return _modDescription; }
      set
        {
        _modDescription = value;
        NotifyOfPropertyChange(nameof(ModDescription));
        }
      }

    private string _modImage;
    public string ModImage
      {
      get { return _modImage; }
      set
        {
        _modImage = value;
        NotifyOfPropertyChange(nameof(ModImage));
        }
      }

    private string _modSource;
    public string ModSource
      {
      get { return _modSource; }
      set
        {
        _modSource = value;
        NotifyOfPropertyChange(nameof(ModSource));
        }
      }

    private ModTypesEnum _modType;
    public ModTypesEnum ModType
      {
      get { return _modType; }
      set
        {
        _modType = value;
        NotifyOfPropertyChange(nameof(ModType));
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
        ClearEditEntry();
        NotifyOfPropertyChange(() => SelectedMod);
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
        NotifyOfPropertyChange(nameof(IsInstalledEGS));
        }
      }

    private bool _InEditMode;
    public bool InEditMode
      {
      get { return _InEditMode; }
      set
        {
        _InEditMode = value;
        NotifyOfPropertyChange(nameof(InEditMode));
        }
      }

    public ModStatusViewModel()
      {
      DisplayName = "Mods";
      }

    public void Initialise(List<ModModel> availableModList)
      {
      AvailableModList = availableModList;
      if (AvailableModList == null)
        {
        throw new Exception("AvailableModList should be filled");
        }
      SelectedAvailableModList = ModManagerViewModel.GetSelectedAvailableMods(AvailableModList);
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
      SelectedAvailableModList = ModManagerViewModel.GetSelectedAvailableMods(AvailableModList); //refresh
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
      var BaseDir = ModManagerViewModel.GetDLCBaseDir(selectedPlatform);
      var Files = BaseDir.GetFiles("*.pak", SearchOption.TopDirectoryOnly);
      foreach (var F in Files)
        {
        SetInstallationStatus(F, selectedPlatform);
        }
      }

    private void SetInstallationStatus(FileInfo f, PlatformEnum selectedPlatform)
      {
      var fileName = f.Name;
      foreach (var mod in SelectedAvailableModList)
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
        return SelectedMod != null && TSWOptions.SteamProgramDirectory.Length > 10; // This is a simplified check. It will not always work but there is a catchall that checks for existence of the Folder.
        }
      }

    public void ActivateSteamMod()
      {
      ModActivator.ActivateMod(SelectedMod, PlatformEnum.Steam, false);
      SetInstallationStatus(PlatformEnum.Steam, true);
      SelectedAvailableModList = ModManagerViewModel.GetSelectedAvailableMods(AvailableModList);
      NotifyOfPropertyChange(() => IsInstalledSteam);
      NotifyOfPropertyChange(() => SelectedMod);
      }

    public bool CanActivateEGSMod
      {
      get
        {
        return SelectedMod != null && TSWOptions.EGSTrainSimWorldDirectory.Length > 5;
        }
      }

    public void ActivateEGSMod()
      {
      ModActivator.ActivateMod(SelectedMod, PlatformEnum.EpicGamesStore, false);
      SetInstallationStatus(PlatformEnum.EpicGamesStore, true);
      SelectedAvailableModList = ModManagerViewModel.GetSelectedAvailableMods(AvailableModList);
      NotifyOfPropertyChange(() => IsInstalledEGS);
      NotifyOfPropertyChange(() => SelectedMod);
      }

    public bool CanDeactivateMod
      {
      get
        {
        return SelectedMod != null;
        }
      }

    public void DeactivateMod()
      {
      if (TSWOptions.SteamProgramDirectory.Length > 0)
        {
        ModActivator.DeactivateMod(SelectedMod, PlatformEnum.Steam);
        SetInstallationStatus(PlatformEnum.Steam, false);
        NotifyOfPropertyChange(() => IsInstalledSteam);
        }
      if (TSWOptions.EGSTrainSimWorldDirectory.Length > 0)
        {
        ModActivator.DeactivateMod(SelectedMod, PlatformEnum.EpicGamesStore);
        SetInstallationStatus(PlatformEnum.EpicGamesStore, false);
        NotifyOfPropertyChange(() => IsInstalledEGS);
        }
      NotifyOfPropertyChange(() => SelectedMod);
      SelectedAvailableModList = ModManagerViewModel.GetSelectedAvailableMods(AvailableModList);
      }

    public bool CanEditModProperties
      {
      get
        {
        return SelectedMod != null;
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

    public bool CanSaveModProperties
      {
      get
        {
        return SelectedMod != null && ModName?.Length > 2;
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
      SelectedAvailableModList = ModManagerViewModel.GetSelectedAvailableMods(AvailableModList);
      SelectedMod = null;
      NotifyOfPropertyChange(() => SelectedMod);
      NotifyOfPropertyChange(() => SelectedAvailableModList);
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
        return SelectedMod != null;
        }
      }

    public void DeleteMod()
      {
      DeactivateMod();
      var PakPath = SelectedMod.FilePath;
      var source = TSWOptions.ModsFolder + PakPath;
      FileHelpers.DeleteSingleFile(source);
      ModDataAccess.DeleteMod(SelectedMod.Id);
      SelectedAvailableModList.Remove(SelectedMod);
      if (InEditMode)
        {
        ClearEdit();
        }
      SelectedMod = null;
      SelectedAvailableModList = ModManagerViewModel.GetSelectedAvailableMods(AvailableModList);
      NotifyOfPropertyChange(() => SelectedMod);
      NotifyOfPropertyChange(() => SelectedAvailableModList);
      }

    public void ClearEditEntry()
      {
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

    public void ClearEdit()
      {
      SelectedMod = null;
      ClearEditEntry();
      }
    }
  }
