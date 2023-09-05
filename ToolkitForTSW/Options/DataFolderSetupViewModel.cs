// Ignore Spelling: TSW

using Caliburn.Micro;
using Logging.Library;
using Microsoft.Win32;
using System.Threading.Tasks;

namespace ToolkitForTSW.Options
  {
  public class DataFolderSetupViewModel : Screen
    {
    private string _TSW2DataFolder = "";
    public string TSW2DataFolder
      {
      get
        {
        return _TSW2DataFolder;
        }
      set
        {
        _TSW2DataFolder = value;
        NotifyOfPropertyChange(nameof(TSW2DataFolder));
        }
      }


    private string _TSW3DataFolder = "";
    public string TSW3DataFolder
      {
      get
        {
        return _TSW3DataFolder;
        }
      set
        {
        _TSW3DataFolder = value;
        NotifyOfPropertyChange(nameof(TSW3DataFolder));
        NotifyOfPropertyChange(nameof(CanCloseForm));
        }
      }

    protected override void OnViewLoaded(object view)
      {
      base.OnViewLoaded(view);
      GetDataFolderLocations();
      }

    private void GetDataFolderLocations()
      {
      var subKeyString = "TSW3";
      var AppKey2 = TSWOptions.OpenCurrentUserRegistry();
      TSW2DataFolder = (string)AppKey2.GetValue("TSWToolsFolder", "");
      var subKey = AppKey2.CreateSubKey(subKeyString);
      if (subKey != null)
        {
        TSW3DataFolder = (string)subKey.GetValue("TSWToolsFolder", "");
        }
      }

    public bool StoreLocations()
      {
      var subKeyString = "TSW3";
      var AppKey2 = TSWOptions.OpenCurrentUserRegistry();
      var subKey = AppKey2.CreateSubKey(subKeyString);
      if (subKey != null)
        {
        subKey.SetValue("TSWToolsFolder", TSW3DataFolder, RegistryValueKind.String);
        return true;
        }
      else
        {
        Log.Trace("Failed to create TSW3 Sub-key in registry. Contact Toolkit author", LogEventType.Error);
        return false;
        }
      }

    public bool CanCloseForm
      {
      get
        {
        return TSW3DataFolder.Length > 2;
        }
      }

    public Task CloseForm()
      {
      return TryCloseAsync(StoreLocations());
      }
    }
  }
