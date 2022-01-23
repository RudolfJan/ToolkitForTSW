using Caliburn.Micro;
using System;
using System.Collections.Generic;
using ToolkitForTSW.DataAccess;
using ToolkitForTSW.Models;

namespace ToolkitForTSW.Mod.ViewModels
  {
  public class ModSetViewModel : Screen, IModTabManager
    {
    private int ModSetId = 0;

    #region properties
    public List<ModModel> AvailableModList { get; set; } = new List<ModModel>();
    private BindableCollection<ModModel> _SelectedAvailableModList = new BindableCollection<ModModel>();
    public BindableCollection<ModModel> SelectedAvailableModList
      {
      get { return _SelectedAvailableModList; }
      set
        {
        _SelectedAvailableModList = value;
        NotifyOfPropertyChange(() => SelectedAvailableModList);
        }
      }

    private ModModel _SelectedMod;
    public ModModel SelectedMod
      {
      get { return _SelectedMod; }
      set
        {
        _SelectedMod = value;
        NotifyOfPropertyChange(() => SelectedMod);
        NotifyOfPropertyChange(() => CanAddModToSet);
        NotifyOfPropertyChange(nameof(CanRemoveModFromSet));
        }
      }

    private ModSetModel _SelectedModSet;
    public ModSetModel SelectedModSet
      {
      get { return _SelectedModSet; }
      set
        {
        _SelectedModSet = value;
        if (SelectedModSet != null)
          {
          ModsInSetList = new BindableCollection<ModModel>(ModModSetDataAccess.GetAllModsPerModSet(SelectedModSet.Id));
          }
        ClearSetEntry();
        NotifyOfPropertyChange(() => SelectedModSet);
        NotifyOfPropertyChange(() => CanAddModToSet);
        NotifyOfPropertyChange(nameof(CanRemoveModFromSet));
        NotifyOfPropertyChange(nameof(CanDeleteSet));
        NotifyOfPropertyChange(nameof(CanEditSet));
        NotifyOfPropertyChange(nameof(CanSaveSet));
        }
      }

    private BindableCollection<ModModel> _ModsInSetList = new BindableCollection<ModModel>();
    public BindableCollection<ModModel> ModsInSetList
      {
      get { return _ModsInSetList; }
      set
        {
        _ModsInSetList = value;
        NotifyOfPropertyChange(() => ModsInSetList);
        }
      }

    internal static void ActivateModSet(ModSetModel modSet, PlatformEnum selectedPlatform)
      {
      var modsInSet = ModModSetDataAccess.GetAllModsPerModSet(modSet.Id);
      foreach (var mod in modsInSet)
        {
        ModActivator.ActivateMod(mod, selectedPlatform, true);
        }
      }

    private ModModel _SelectedModInSet;
    public ModModel SelectedModInSet
      {
      get { return _SelectedModInSet; }
      set
        {
        _SelectedModInSet = value;
        NotifyOfPropertyChange(() => SelectedModInSet);
        }
      }

    private BindableCollection<ModSetModel> _ModSetList;
    public BindableCollection<ModSetModel> ModSetList
      {
      get { return _ModSetList; }
      set
        {
        _ModSetList = value;
        NotifyOfPropertyChange(() => ModSetList);
        }
      }

    private String _SetName = string.Empty;

    public String SetName
      {
      get { return _SetName; }
      set
        {
        _SetName = value;
        NotifyOfPropertyChange(() => SetName);
        NotifyOfPropertyChange(() => CanAddModToSet);
        NotifyOfPropertyChange(nameof(CanRemoveModFromSet));
        NotifyOfPropertyChange(nameof(CanSaveSet));
        }
      }

    private String _Description = string.Empty;

    public String Description
      {
      get { return _Description; }
      set
        {
        _Description = value;
        NotifyOfPropertyChange(() => Description);
        }
      }
    #endregion

    #region Initialization

    public ModSetViewModel()
      {
      DisplayName = "Sets";
      }

    public void Initialise(List<ModModel> availableModList)
      {
      AvailableModList = availableModList;
      SelectedAvailableModList = ModManagerViewModel.GetSelectedAvailableMods(availableModList);
      ModSetList = new BindableCollection<ModSetModel>(ModSetDataAccess.GetAllModSets());
      SelectedModSet = null;
      SelectedModInSet = null;
      ModsInSetList.Clear();
      }
    #endregion

    public void CreateModSet()
      {
      var modSetModel = new ModSetModel
        {
        ModSetName = SetName,
        ModSetDescription = Description,
        RouteId = 0
        };
      modSetModel.Id = ModSetDataAccess.InsertModSet(modSetModel);
      ModSetList.Add(modSetModel);
      }

    public bool CanAddModToSet
      {
      get
        {
        return (SelectedModSet != null || !string.IsNullOrEmpty(SetName)) && SelectedMod != null;
        }
      }

    public void AddModToSet()
      {
      if (SelectedModSet == null)
        {
        SaveSet();
        SelectedModSet = ModSetDataAccess.GetModSetById(ModSetId);
        }
      ModModSetModel modModSet = new ModModSetModel
        {
        ModId = SelectedMod.Id,
        ModSetId = SelectedModSet.Id
        };
      var id = ModModSetDataAccess.InsertModModSet(modModSet);
      if (id > 0)
        {
        ModsInSetList.Add(SelectedMod);
        }
      NotifyOfPropertyChange(() => ModsInSetList);
      }

    public bool CanRemoveModFromSet
      {
      get
        {
        return SelectedModSet != null && SelectedModInSet != null;
        }
      }

    public void RemoveModFromSet()
      {
      ModsInSetList.Remove(SelectedModInSet);
      ModModSetDataAccess.DeleteModFromModSet(SelectedModInSet.Id, SelectedModSet.Id);
      SelectedModInSet = null;
      NotifyOfPropertyChange(() => ModsInSetList);
      NotifyOfPropertyChange(nameof(CanAddModToSet));
      NotifyOfPropertyChange(nameof(CanRemoveModFromSet));
      }

    public void ClearSet()
      {
      ClearSetEntry();
      ModSetId = 0;
      SelectedModSet = null;
      ModsInSetList.Clear();
      SelectedModInSet = null;
      }

    public void ClearSetEntry()
      {
      SetName = string.Empty;
      Description = string.Empty;
      }


    public bool CanSaveSet
      {
      get
        {
        return !string.IsNullOrEmpty(SetName);
        }
      }

    public void SaveSet()
      {
      ModSetModel newModSet;
      if (ModSetId == 0)
        {
        newModSet = new ModSetModel();
        }
      else
        {
        newModSet = SelectedModSet;
        }
      newModSet.ModSetName = SetName;
      newModSet.ModSetDescription = Description;
      if (ModSetId == 0)
        {
        ModSetId = ModSetDataAccess.InsertModSet(newModSet);
        ModSetList.Add(newModSet);
        }
      else
        {
        ModSetDataAccess.UpdateModSet(newModSet);
        ModSetList.Remove(SelectedModSet);
        ModSetList.Add(newModSet);
        }
      NotifyOfPropertyChange(() => ModSetList);
      ClearSet();
      }

    public bool CanDeleteSet
      {
      get
        {
        return SelectedModSet != null;
        }
      }

    public void DeleteSet()
      {
      ModSetDataAccess.DeleteModSet(SelectedModSet.Id);
      ModSetList.Remove(SelectedModSet);
      ModsInSetList.Clear();
      SelectedModSet = null;
      NotifyOfPropertyChange(() => ModSetList);
      }


    public bool CanEditSet
      {
      get
        {
        return SelectedModSet != null;
        }
      }

    public void EditSet()
      {
      SetName = SelectedModSet.ModSetName;
      Description = SelectedModSet.ModSetDescription;
      ModSetId = SelectedModSet.Id;
      }
    }
  }
