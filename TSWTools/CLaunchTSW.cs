using Styles.Library.Helpers;
using System;
using System.IO;


namespace TSWTools
  {
  public class CLaunchTSW : Notifier
    {
    private Boolean _LaunchRadio;

    public Boolean LaunchRadio
      {
      get { return _LaunchRadio; }
      set
        {
        _LaunchRadio = value;
        OnPropertyChanged("LaunchRadio");
        }
      }

    private String _RadioUrl;

    public String RadioUrl
      {
      get { return _RadioUrl; }
      set
        {
        _RadioUrl = value;
        OnPropertyChanged("RadioUrl");
        }
      }

    private CSettingsManager _SettingsManager;

    public CSettingsManager SettingsManager
      {
      get { return _SettingsManager; }
      set
        {
        _SettingsManager = value;
        OnPropertyChanged("Settings");
        }
      }

    private CLiverySet _LiverySet;

    public CLiverySet LiverySet
      {
      get { return _LiverySet; }
      set
        {
        _LiverySet = value;
        OnPropertyChanged("LiverySet");
        }
      }

    private CRailwayRadioStationManager _RailwayRadioStationManager;

    public CRailwayRadioStationManager RailwayRadioStationManager
      {
      get { return _RailwayRadioStationManager; }
      set
        {
        _RailwayRadioStationManager = value;
        OnPropertyChanged("RailwayRadioStationManager");
        }
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

    public CLaunchTSW()
      {
      SettingsManager = new CSettingsManager();
      RailwayRadioStationManager = new CRailwayRadioStationManager();
      RailwayRadioStationManager.Initialize();

//		RadioUrl = "https://tunein.com/radio/Railroad-Radio-West-Slope-s89688/"; // Default
      RadioUrl = String.Empty;
      LiverySet = new CLiverySet();
      LiverySet.FillLiverySetList();
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
          Result += CLog.Trace("Failed to launch TSW problem loading ini file " + E.Message,
            LogEventType.Error);
          return;
          }
        }

      if (RadioUrl.Length > 0)
        {
        Result+=CApps.LaunchUrl(RadioUrl, true);
        }

      var TSWProgram = new FileInfo(CTSWOptions.SteamProgramDirectory + "Steam.exe");
      Result += CApps.ExecuteFile(TSWProgram, "steam://rungameid/530070");
      }
    }
  }
