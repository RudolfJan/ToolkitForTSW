using Caliburn.Micro;
using Logging.Library;
using Styles.Library.Helpers;
using System;
using System.IO;
using ToolkitForTSW.Mod;
using ToolkitForTSW.Settings;

namespace ToolkitForTSW.ViewModels
  {
  public class LaunchTSWViewModel : Screen
    {
    private Boolean _LaunchRadio;

    public Boolean LaunchRadio
      {
      get { return _LaunchRadio; }
      set
        {
        _LaunchRadio = value;
        NotifyOfPropertyChange(()=>LaunchRadio);
        }
      }

    private String _RadioUrl;

    public String RadioUrl
      {
      get { return _RadioUrl; }
      set
        {
        _RadioUrl = value;
        NotifyOfPropertyChange(() => RadioUrl);
        }
      }

    private CSettingsManager _SettingsManager;

    public CSettingsManager SettingsManager
      {
      get { return _SettingsManager; }
      set
        {
        _SettingsManager = value;
        NotifyOfPropertyChange(() => SettingsManager);
        }
      }

    private CModSet _modSet;

    public CModSet ModSet
      {
      get { return _modSet; }
      set
        {
        _modSet = value;
        NotifyOfPropertyChange(() => ModSet);
        }
      }

    private RadioStationsViewModel _RailwayRadioStationManager;

    public RadioStationsViewModel RailwayRadioStationManager
      {
      get { return _RailwayRadioStationManager; }
      set
        {
        _RailwayRadioStationManager = value;
        NotifyOfPropertyChange(() => RailwayRadioStationManager);
        }
      }

    private String _Result;

    public String Result
      {
      get { return _Result; }
      set
        {
        _Result = value;
        NotifyOfPropertyChange(() => Result);
        }
      }

    public LaunchTSWViewModel()
      {
      SettingsManager = new CSettingsManager();
      RailwayRadioStationManager = new RadioStationsViewModel();
      RailwayRadioStationManager.Initialize();

//		RadioUrl = "https://tunein.com/radio/Railroad-Radio-West-Slope-s89688/"; // Default
      RadioUrl = String.Empty;
      ModSet = new CModSet();
      }

    public void LaunchPrograms(DirectoryInfo SaveSet)
      {
      if (SaveSet != null)
        {
        try
          {
          var SourceFile = SaveSet.FullName + "\\GameUserSettings.ini";
          var DestinationFile = SettingsManager.GetInGameSettingsLocation().FullName;
          File.Copy(SourceFile, DestinationFile, true);
          SourceFile = SaveSet.FullName + "\\Engine.ini";
          DestinationFile = SettingsManager.GetInGameEngineIniLocation().FullName;
          File.Copy(SourceFile, DestinationFile, true);
          }
        catch (Exception E)
          {
          Result += Log.Trace("Failed to launch TSW problem loading ini file " + E.Message,
            LogEventType.Error);
          return;
          }
        }

      if (RadioUrl.Length > 0)
        {
        Result+=CApps.LaunchUrl(RadioUrl, true);
        }

      var TSWProgram = new FileInfo(TSWOptions.SteamProgramDirectory + "Steam.exe");
      Result += CApps.ExecuteFile(TSWProgram, "steam://rungameid/1282590");
      }
    }
  }
