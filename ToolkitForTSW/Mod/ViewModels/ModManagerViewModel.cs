using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ToolkitForTSW.DataAccess;
using ToolkitForTSW.Models;

namespace ToolkitForTSW.Mod.ViewModels
  {

  public class ModManagerViewModel : Conductor<IScreen>.Collection.OneActive
    {
    // Handles change of tabs. We only need to know if the Sets tab is selected in the UI.
    private bool _IsSetsTabSelected = false;
    public bool IsSetsTabSelected
      {
      get
        {
        return _IsSetsTabSelected;
        }
      set
        {
        _IsSetsTabSelected = value;
        NotifyOfPropertyChange(() => IsSetsTabSelected);
        }
      }

    public List<ModModel> AvailableModList { get; set; } = new List<ModModel>();

    public ModManagerViewModel()
      {

      }

    protected override void OnViewLoaded(object view)
      {
      base.OnViewLoaded(view);
      AvailableModList = ModDataAccess.GetAllMods();
      var sets = IoC.Get<ModSetViewModel>();
      sets.Initialise(AvailableModList);
      var mods = IoC.Get<ModStatusViewModel>();
      mods.Initialise(AvailableModList);
      Items.Add(mods);
      Items.Add(sets);
      }

    public static BindableCollection<ModModel> GetSelectedAvailableMods(List<ModModel> availableModList)
      {
      return new BindableCollection<ModModel>(availableModList.OrderBy(y => y.ModName).OrderBy(x => x.DLCName));
      }

    public static DirectoryInfo GetDLCBaseDir(PlatformEnum selectedPlatform)
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

    public Task CloseForm()
      {
      return TryCloseAsync();
      }
    }
  }
