using Caliburn.Micro;
using Logging.Library;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using ToolkitForTSW.DataAccess;
using ToolkitForTSW.Mod.Models;
using ToolkitForTSW.Models;
using Utilities.Library;

namespace ToolkitForTSW.Mod.ViewModels
  {

  public class ModManagerViewModel : Conductor<IScreen>.Collection.OneActive
    {
    // Handles change of tabs. We only need to know if the Sets tab is selected in the UI.
    private bool _IsSetsTabSelected=false;
    public bool IsSetsTabSelected
      {
      get 
        {  
        return _IsSetsTabSelected;
        }
      set 
        {
        _IsSetsTabSelected = value;
        NotifyOfPropertyChange(()=> IsSetsTabSelected );
         }
      }

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


    public ModManagerViewModel()
      {

      }

    protected override void OnViewLoaded(object view)
      {
      base.OnViewLoaded(view);
      AvailableModList = new BindableCollection<ModModel>(ModDataAccess.GetAllMods());
      var sets= IoC.Get<ModSetViewModel>();
      sets.Initialise(AvailableModList);
      var mods= IoC.Get<ModStatusViewModel>();
      mods.Initialise(AvailableModList);
      Items.Add(mods);
      Items.Add(sets);
      }


      public Task CloseForm()
      {
      return TryCloseAsync();
      }
    }
  }
