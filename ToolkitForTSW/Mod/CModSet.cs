using Styles.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;
using ToolkitForTSW.DataAccess;
using ToolkitForTSW.Models;

namespace ToolkitForTSW.Mod
  {
 public class CModSet : Notifier
    {
    private int ModSetId = 0;

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

   private ModSetModel _SelectedModSet;
    public ModSetModel SelectedModSet
      {
      get { return _SelectedModSet; }
      set
        {
        _SelectedModSet = value;
        if (SelectedModSet != null)
          {
          ModsInSetList = ModModSetDataAccess.GetAllModsPerModSet(SelectedModSet.Id);
          }
        OnPropertyChanged("SelectedModSet");
        }
      }

    private List<ModModel> _ModsInSetList = new List<ModModel>();
    public List<ModModel> ModsInSetList
      {
      get { return _ModsInSetList; }
      set
        {
        _ModsInSetList = value;
        OnPropertyChanged("ModsInSetList");
        }
      }

    internal static void ActivateModSet(ModSetModel modSet)
      {
      var modsInSet = ModModSetDataAccess.GetAllModsPerModSet(modSet.Id);
      foreach (var mod in modsInSet)
        {
        CModManager.ActivateMod(mod);
        }
      }

    private ModModel _SelectedModInSet;
    public ModModel SelectedModInSet
      {
      get { return _SelectedModInSet; }
      set
        {
        _SelectedModInSet = value;
        OnPropertyChanged("SelectedModInSet");
        }
      }

    private List<ModSetModel> _ModSetList;
    public List<ModSetModel> ModSetList
      {
      get { return _ModSetList; }
      set
        {
        _ModSetList = value;
        OnPropertyChanged("ModSetList");
        }
      }

    private String _SetName;

    public String SetName
      {
      get { return _SetName; }
      set
        {
        _SetName = value;
        OnPropertyChanged("SetName");
        }
      }

    private String _Description;

    public String Description
      {
      get { return _Description; }
      set
        {
        _Description = value;
        OnPropertyChanged("Description");
        }
      }

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

    public CModSet()
      {
      ModSetList = ModSetDataAccess.GetAllModSets();
      }
    public CModSet(List<ModModel> availableModList)
      {
      AvailableModList = availableModList;
      ModSetList = ModSetDataAccess.GetAllModSets();
      }

    public void Initialise()
      {
      AvailableModList = ModDataAccess.GetAllMods();
      ModSetList = ModSetDataAccess.GetAllModSets();
      SelectedModSet=null;
      SelectedModInSet=null;
      ModsInSetList.Clear();
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
        ModSetDataAccess.InsertModSet(newModSet);
        ModSetList.Add(newModSet);
        }
      else
        {
        ModSetDataAccess.UpdateModSet(newModSet);
        }
      }

    internal void DeleteSet()
      {
      ModSetDataAccess.DeleteModSet(SelectedModSet.Id);
      ModSetList.Remove(SelectedModSet);
      SelectedModSet=null;
      
      }

    internal void AddModToSet()
      {
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
      }

    internal void RemoveModFromSet()
      {
      ModsInSetList.Remove(SelectedModInSet);
      ModModSetDataAccess.DeleteModFromModSet(SelectedModInSet.Id, SelectedModSet.Id);
      SelectedModInSet=null;
      }

    internal void EditSet()
      {
      SetName = SelectedModSet.ModSetName;
      Description = SelectedModSet.ModSetDescription;
      ModSetId= SelectedModSet.Id;
      }
    }

  }
