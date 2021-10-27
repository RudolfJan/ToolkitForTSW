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
    private BindableCollection<ModModel> _AvailableModList = new BindableCollection<ModModel>();
    public BindableCollection<ModModel> AvailableModList
      {
      get { return _AvailableModList; }
      set
        {
        _AvailableModList = value;
        NotifyOfPropertyChange(()=>AvailableModList);
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
        NotifyOfPropertyChange(() => CanAddToSet);
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
        NotifyOfPropertyChange(() => SelectedModSet);
        NotifyOfPropertyChange(() => CanAddToSet);
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
        ModStatusViewModel.ActivateMod(mod,selectedPlatform,true);
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

    private String _SetName=string.Empty;

    public String SetName
      {
      get { return _SetName; }
      set
        {
        _SetName = value;
        NotifyOfPropertyChange(() => SetName);
        NotifyOfPropertyChange(() => CanAddToSet);
        }
      }

    private String _Description=string.Empty;

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

    public ModSetViewModel(BindableCollection<ModModel> availableModList)
      {
      DisplayName="Sets";
      }
 
    public void Initialise(BindableCollection<ModModel> availableModList)
      {
      AvailableModList = availableModList;
      ModSetList = new BindableCollection<ModSetModel>(ModSetDataAccess.GetAllModSets());
      SelectedModSet =null;
      SelectedModInSet=null;
      ModsInSetList.Clear();
      }

    #endregion


    internal void CreateModSet()
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

    //RemoveFromSetButton.IsEnabled= ModManager.ModSet.SelectedModSet != null && ModManager.ModSet.SelectedModInSet!=null;
    //SaveSetButton.IsEnabled = !string.IsNullOrEmpty(ModManager.ModSet.SetName);
    //EditSetButton.IsEnabled = ModManager.ModSet.SelectedModSet != null;
    //DeleteSetButton.IsEnabled= ModManager.ModSet.SelectedModSet != null;

    public bool CanAddToSet
      {
      get
        {
        return (SelectedModSet != null || !string.IsNullOrEmpty(SetName)) && SelectedMod != null;
        }
      }


    internal void Clear()
      {
      SetName=string.Empty;
      Description=string.Empty;
      ModSetId = 0;
      SelectedModSet =null;
      ModsInSetList.Clear();
      SelectedModInSet = null;
      }

    internal void SaveSet()
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
        ModSetId=ModSetDataAccess.InsertModSet(newModSet);
        ModSetList.Add(newModSet);
        }
      else
        {
        ModSetDataAccess.UpdateModSet(newModSet);
        ModSetList.Remove(newModSet);
        ModSetList.Add(newModSet);
        }
      NotifyOfPropertyChange(()=>ModSetList);
      }

    internal void DeleteSet()
      {
      ModSetDataAccess.DeleteModSet(SelectedModSet.Id);
      ModSetList.Remove(SelectedModSet);
      ModsInSetList.Clear();
      SelectedModSet=null;
      NotifyOfPropertyChange(() => ModSetList);
      }

    internal void AddModToSet()
      {
      if(SelectedModSet == null)
        {
        SaveSet();
        SelectedModSet= ModSetDataAccess.GetModSetById(ModSetId);
        }
      ModModSetModel modModSet = new ModModSetModel
        {
        ModId = SelectedMod.Id,
        ModSetId = SelectedModSet.Id
        };
      var id=ModModSetDataAccess.InsertModModSet(modModSet);
      if (id > 0)
        {
        ModsInSetList.Add(SelectedMod);
        }
      NotifyOfPropertyChange(() => ModsInSetList);
      }

    internal void RemoveModFromSet()
      {
      ModsInSetList.Remove(SelectedModInSet);
      ModModSetDataAccess.DeleteModFromModSet(SelectedModInSet.Id, SelectedModSet.Id);
      SelectedModInSet=null;
      NotifyOfPropertyChange(() => ModsInSetList);
      }

    internal void EditSet()
      {
      SetName = SelectedModSet.ModSetName;
      Description = SelectedModSet.ModSetDescription;
      ModSetId = SelectedModSet.Id;
      }

    #region ModSets
    //TODO Stupid, but at the moment I do not know how to invoke the ModSet
    //methods directly using Caliburn.Micro conventions.

    //AddToSetButton.IsEnabled= (ModSet.SelectedModSet!=null || !string.IsNullOrEmpty(ModSet.SetName)) && ModSet.SelectedMod!=null;
    //RemoveFromSetButton.IsEnabled= ModManager.ModSet.SelectedModSet != null && ModManager.ModSet.SelectedModInSet!=null;
    //SaveSetButton.IsEnabled = !string.IsNullOrEmpty(ModManager.ModSet.SetName);
    //EditSetButton.IsEnabled = ModManager.ModSet.SelectedModSet != null;
    //DeleteSetButton.IsEnabled= ModManager.ModSet.SelectedModSet != null;

  

 
    #endregion
    }
  }
