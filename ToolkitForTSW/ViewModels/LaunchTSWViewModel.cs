﻿using Caliburn.Micro;
using Logging.Library;
using Styles.Library.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;
using ToolkitForTSW.DataAccess;
using ToolkitForTSW.Mod;
using ToolkitForTSW.Mod.ViewModels;
using ToolkitForTSW.Models;
using ToolkitForTSW.Settings;

namespace ToolkitForTSW.ViewModels
  {

  public class LaunchTSWViewModel : Screen
    {
    private IWindowManager _windowManager;
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

    private bool _isTrackIRActive=TSWOptions.TrackIRProgram.Length>0;

    public bool IsTrackIRActive
      {
      get {
        return _isTrackIRActive;
        }
      set 
        {
        _isTrackIRActive=value;
        NotifyOfPropertyChange(()=>IsTrackIRActive);
        }
      }


    private DirectoryInfo _selectedOptionsSet;
    public DirectoryInfo SelectedOptionsSet
      {
      get
        {
        return _selectedOptionsSet;
        }
      set
        {
        _selectedOptionsSet=value;
        NotifyOfPropertyChange(()=>SelectedOptionsSet);
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

    private BindableCollection<ModModel> _AvailableModList = new BindableCollection<ModModel>();
    public BindableCollection<ModModel> AvailableModList
      {
      get { return _AvailableModList; }
      set
        {
        _AvailableModList = value;
        NotifyOfPropertyChange(() => AvailableModList);
        }
      }

    private ModSetViewModel _modSet;

    public ModSetViewModel ModSet
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

    public LaunchTSWViewModel(IWindowManager  windowManager)
      {
      _windowManager = windowManager;
      SettingsManager = new CSettingsManager();
      RailwayRadioStationManager = new RadioStationsViewModel();
      RailwayRadioStationManager.Initialize();

//		RadioUrl = "https://tunein.com/radio/Railroad-Radio-West-Slope-s89688/"; // Default
      RadioUrl = String.Empty;
      AvailableModList = new BindableCollection<ModModel>(ModDataAccess.GetAllMods());
      ModSet = new ModSetViewModel(AvailableModList); //TODO this is maybe not very good code.
      }

    public void LaunchPrograms()
      {
      if (IsTrackIRActive)
        {
        try
          {
          Result += CApps.ExecuteFileMinimized(TSWOptions.TrackIRProgram);
          }
        catch(Exception ex)
          {
          Result += Log.Trace("Failed to launch TrackIR because " + ex.Message,  LogEventType.Error);
          }
        }

      if (SelectedOptionsSet != null)
        {
        try
          {
 
          var SourceFile = SelectedOptionsSet.FullName + "\\GameUserSettings.ini";
          var DestinationFile = SettingsManager.GetInGameSettingsLocation().FullName;
          File.Copy(SourceFile, DestinationFile, true);
          SourceFile = SelectedOptionsSet.FullName + "\\Engine.ini";
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

      if(TSWOptions.CurrentPlatform==PlatformEnum.Steam)
        {
        var TSWProgram = new FileInfo(TSWOptions.SteamProgramDirectory + "Steam.exe");
        Result += CApps.ExecuteFile(TSWProgram, "steam://rungameid/1282590");
        }
      if(TSWOptions.CurrentPlatform==PlatformEnum.EpicGamesStore)
        {
        Result += CApps.LaunchUrl(TSWOptions.EGSTrainSimWorldStarter,false);
        }
      }

    public Task EditMods()
      {
      var viewModel= IoC.Get<ModManagerViewModel>();
      return _windowManager.ShowDialogAsync(viewModel);
      }

    public Task CloseForm()
      {
      return TryCloseAsync();
      }
    }
  }
