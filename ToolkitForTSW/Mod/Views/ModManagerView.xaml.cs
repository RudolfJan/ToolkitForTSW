using System.Windows;

namespace ToolkitForTSW.Mod.Views

  {

  public partial class ModManagerView
    {

    public ModManagerView()
      {
      InitializeComponent();
 
      SetControlStates();
      }

    private void SetControlStates()
      {
      //SaveModButton.IsEnabled= ModManager.SelectedMod!=null && ModManager.InEditMode==true;
      //ActivateSteamModButton.IsEnabled= ModManager.SelectedMod != null && ModManager.SelectedMod.IsInstalledSteam==false && TSWOptions.SteamTrainSimWorldDirectory.Length>0;
      //ActivateEGSModButton.IsEnabled = ModManager.SelectedMod != null && ModManager.SelectedMod.IsInstalledEGS == false && TSWOptions.EGSTrainSimWorldDirectory.Length>0;
      //DeActivateModButton.IsEnabled = ModManager.SelectedMod != null && (ModManager.SelectedMod.IsInstalledSteam == true || ModManager.SelectedMod.IsInstalledEGS == true);
      //AddToSetButton.IsEnabled= (ModManager.ModSet.SelectedModSet!=null || !string.IsNullOrEmpty(ModManager.ModSet.SetName)) && ModManager.ModSet.SelectedMod!=null;
      //RemoveFromSetButton.IsEnabled= ModManager.ModSet.SelectedModSet != null && ModManager.ModSet.SelectedModInSet!=null;
      //SaveSetButton.IsEnabled = !string.IsNullOrEmpty(ModManager.ModSet.SetName);
      //EditSetButton.IsEnabled = ModManager.ModSet.SelectedModSet != null;
      //DeleteSetButton.IsEnabled= ModManager.ModSet.SelectedModSet != null;
      }

    private void ModStatusView_Loaded(object sender, RoutedEventArgs e)
      {

      }
    }
  }
