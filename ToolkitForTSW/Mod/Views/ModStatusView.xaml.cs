using System.Windows.Controls;

namespace ToolkitForTSW.Mod.Views
  {
   public partial class ModStatusView : UserControl
    {
    public ModStatusView()
      {
      InitializeComponent();
      PakfileTextBox.IsEnabled = false;
      IsInstalledSteamCheckBox.IsEnabled = false;
      IsInstalledEGSCheckBox.IsEnabled = false;
      }
    }
  }
