using Caliburn.Micro;
using Logging.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ToolkitForTSW.Settings.ViewModels
  {

  public class SettingsManagerViewModel : Conductor<IScreen>.Collection.OneActive
    {
    private readonly ISetting _setting;
    private readonly ISettingsManagerLogic _settingsManagerLogic;
    private readonly IWindowManager _windowManager;

    private bool _setIsLoaded = false;
    public bool SetIsLoaded
      {
      get { return _setIsLoaded; }
      set
        {
        _setIsLoaded = value;
        NotifyOfPropertyChange(nameof(CanUpdateLoadedSettings));
        NotifyOfPropertyChange(nameof(SetIsNotLoaded));
        NotifyOfPropertyChange(nameof(CanSaveInGame));
        }
      }

    public bool SetIsNotLoaded
      {
      get
        {
        return !SetIsLoaded;
        }
      }

    #region Properties
    /*
		Dictionary to contain all settings
		*/
    private BindableCollection<ISetting> _SettingsDictionary = new BindableCollection<ISetting>();

    public BindableCollection<ISetting> SettingsDictionary
      {
      get { return _SettingsDictionary; }
      set
        {
        _SettingsDictionary = value;
        NotifyOfPropertyChange(nameof(SettingsDictionary));
        }
      }

    private DirectoryInfo _selectedSaveSet;
    public DirectoryInfo SelectedSaveSet
      {
      get { return _selectedSaveSet; }
      set
        {
        _selectedSaveSet = value;
        NotifyOfPropertyChange(nameof(CanLoadSaveSet));
        NotifyOfPropertyChange(nameof(CanDeleteSaveSet));
        NotifyOfPropertyChange(nameof(CanOpenEngineIniFile));
        NotifyOfPropertyChange(nameof(CanOpenUserSettingsFile));
        }
      }

    //private FileInfo _CurrentUserSettingsFile;

    //public FileInfo CurrentUserSettingsFile
    //  {
    //  get { return _CurrentUserSettingsFile; }
    //  set
    //    {
    //    _CurrentUserSettingsFile = value;
    //    NotifyOfPropertyChange(nameof(CurrentUserSettingsFile));
    //    }
    //   }

    private FileInfo _CurrentEngineIniSettingsFile;

    public FileInfo CurrentEngineIniSettingsFile
      {
      get { return _CurrentEngineIniSettingsFile; }
      set
        {
        _CurrentEngineIniSettingsFile = value;
        NotifyOfPropertyChange(nameof(CurrentEngineIniSettingsFile));
        }
      }

    /*
    Contains text for [System.Core] part in Engine.ini
    */
    private String _SectionCore;
    public String SectionCore
      {
      get { return _SectionCore; }
      set
        {
        _SectionCore = value;
        NotifyOfPropertyChange(nameof(SectionCore));
        }
      }

    private String _SaveSetName;
    public String SaveSetName
      {
      get { return _SaveSetName; }
      set
        {
        _SaveSetName = value;
        NotifyOfPropertyChange(nameof(CanUpdateSaveSet));
        NotifyOfPropertyChange(nameof(SaveSetName));
        }
      }

    private BindableCollection<FileInfo> _SavedEngineIniSettingsFileList;

    public BindableCollection<FileInfo> SavedEngineIniSettingsFileList
      {
      get { return _SavedEngineIniSettingsFileList; }
      set
        {
        _SavedEngineIniSettingsFileList = value;
        NotifyOfPropertyChange(nameof(SavedEngineIniSettingsFileList));
        }
      }

    private SettingsHUDViewModel _SettingsHUD;

    public SettingsHUDViewModel SettingsHUD
      {
      get { return _SettingsHUD; }
      set
        {
        _SettingsHUD = value;
        NotifyOfPropertyChange(nameof(SettingsHUD));
        }
      }

    private SettingsGamePlayViewModel _SettingsGamePlay;

    public SettingsGamePlayViewModel SettingsGamePlay
      {
      get { return _SettingsGamePlay; }
      set
        {
        _SettingsGamePlay = value;
        NotifyOfPropertyChange(nameof(SettingsGamePlay));
        }
      }

    private SettingsSoundViewModel _SettingsSound;

    public SettingsSoundViewModel SettingsSound
      {
      get { return _SettingsSound; }
      set
        {
        _SettingsSound = value;
        NotifyOfPropertyChange(nameof(SettingsSound));
        }
      }

    private SettingsScreenViewModel _SettingsScreen;

    public SettingsScreenViewModel SettingsScreen
      {
      get { return _SettingsScreen; }
      set
        {
        _SettingsScreen = value;
        NotifyOfPropertyChange(nameof(SettingsScreen));
        }
      }

    private SettingsQualityViewModel _SettingsQuality;

    public SettingsQualityViewModel SettingsQuality
      {
      get { return _SettingsQuality; }
      set
        {
        _SettingsQuality = value;
        NotifyOfPropertyChange(nameof(SettingsQuality));
        }
      }

    private SettingsUserViewModel _SettingsUser;

    public SettingsUserViewModel SettingsUser
      {
      get { return _SettingsUser; }
      set
        {
        _SettingsUser = value;
        NotifyOfPropertyChange(nameof(SettingsUser));
        }
      }

    private SettingsAdvancedViewModel _SettingsAdvanced;

    public SettingsAdvancedViewModel SettingsAdvanced
      {
      get { return _SettingsAdvanced; }
      set
        {
        _SettingsAdvanced = value;

        NotifyOfPropertyChange(nameof(SettingsAdvanced));
        }
      }

    private SettingsExperimentalViewModel _SettingsExperimental;
    public SettingsExperimentalViewModel SettingsExperimental
      {
      get { return _SettingsExperimental; }
      set
        {
        _SettingsExperimental = value;
        NotifyOfPropertyChange(nameof(SettingsExperimental));
        }
      }

    private BindableCollection<DirectoryInfo> _SavedUserSettingsList;

    public BindableCollection<DirectoryInfo> SavedUserSettingsList
      {
      get { return _SavedUserSettingsList; }
      set
        {
        _SavedUserSettingsList = value;
        NotifyOfPropertyChange(nameof(SavedUserSettingsList));
        }
      }
    #endregion

    #region Constructors

    public SettingsManagerViewModel(ISetting setting, ISettingsManagerLogic settingsManagerLogic, IWindowManager windowManager)
      {
      _setting = setting;
      _settingsManagerLogic = settingsManagerLogic;
      SettingsScreen = new SettingsScreenViewModel(_setting); // Must be here to init video modes in time
      _windowManager = windowManager;
      SavedUserSettingsList = new BindableCollection<DirectoryInfo>();
      GetSavedSettings();
      }

    public void GetSavedSettings()
      {
      String Path = TSWOptions.GetOptionsSetPath();
      DirectoryInfo SavedSettingsDir = new DirectoryInfo(Path);
      DirectoryInfo[] SavedSets = SavedSettingsDir.GetDirectories("*", SearchOption.TopDirectoryOnly);
      SavedUserSettingsList.Clear();
      foreach (var X in SavedSets)
        {
        SavedUserSettingsList.Add(X);
        }
      }

    protected override void OnViewLoaded(object view)
      {
      base.OnViewLoaded(view);

      }

    public void TabsSetup()
      {
      if (!SetIsLoaded)
        {
        Items.Clear();
        SettingsAdvanced = IoC.Get<SettingsAdvancedViewModel>();
        SettingsScreen = IoC.Get<SettingsScreenViewModel>();
        Items.Add(SettingsScreen);
        SettingsQuality = IoC.Get<SettingsQualityViewModel>();
        SettingsQuality.SettingsAdvanced = SettingsAdvanced;
        Items.Add(SettingsQuality);
        SettingsSound = IoC.Get<SettingsSoundViewModel>();
        Items.Add(SettingsSound);
        SettingsHUD = IoC.Get<SettingsHUDViewModel>();
        Items.Add(SettingsHUD);
        SettingsGamePlay = IoC.Get<SettingsGamePlayViewModel>();
        Items.Add(SettingsGamePlay);
        SettingsUser = IoC.Get<SettingsUserViewModel>();
        Items.Add(SettingsUser);
        Items.Add(SettingsAdvanced);
        SettingsExperimental = IoC.Get<SettingsExperimentalViewModel>();
        SettingsExperimental.SaveSetName = SaveSetName;
        Items.Add(SettingsExperimental);
        SetIsLoaded = true;
        }
      }

    public void Init()
      {
      TabsSetup();
      SettingsHUD.Init();
      SettingsSound.Init();
      SettingsGamePlay.Init();
      SettingsScreen.Init();
      SettingsQuality.Init();
      SettingsUser.Init();
      SettingsAdvanced.Init();
      SettingsExperimental.Init();
      }

    public void Update()
      {
      SettingsHUD.Update();
      SettingsSound.Update();
      SettingsGamePlay.Update();
      SettingsScreen.Update();
      SettingsQuality.Update();
      SettingsAdvanced.Update();
      SettingsUser.Update();
      SettingsExperimental.Update();
      }

    public bool CanDeleteSaveSet
      {
      get
        {
        return SelectedSaveSet != null;
        }
      }

    public void DeleteSaveSet()
      {
      var dir = ((DirectoryInfo)SelectedSaveSet);
      try
        {
        dir.Delete(true);
        GetSavedSettings();
        }
      catch (Exception ex)
        {
        Log.Trace($"Cannot delete save set because {ex.Message}", ex, LogEventType.Error);
        }
      }

    public void LoadActiveGameSettings()
      {
      _settingsManagerLogic.LoadSettingsInDictionary(SettingsManagerLogic.GetInGameSettingsLocation(), SettingsManagerLogic.GetInGameEngineIniLocation());
      SettingsDictionary = new BindableCollection<ISetting>(_settingsManagerLogic.SettingsDictionary.OrderBy(x => x.Key));
      Init();
      }

    public bool CanLoadSaveSet
      {
      get
        {
        return SelectedSaveSet != null;
        }
      }

    public void LoadSaveSet()
      {
      var path = SelectedSaveSet;
      var gameUserSettings = SettingsManagerLogic.GetSavedSettingsLocation(SelectedSaveSet);
      var engineIniSettings = SettingsManagerLogic.GetEngineIniLocation(SelectedSaveSet);
      _settingsManagerLogic.LoadSettingsInDictionary(gameUserSettings, engineIniSettings);
      SettingsDictionary = new BindableCollection<ISetting>(_settingsManagerLogic.SettingsDictionary.OrderBy(x => x.Key));
      Init();
      SaveSetName = path.Name;
      }

    public bool CanSaveInGame
      {
      get
        {
        return SetIsLoaded;
        }
      }
    public void SaveInGame()
      {
      Update();
      var FileName = SettingsManagerLogic.GetInGameSettingsLocation();
      _settingsManagerLogic.WriteSettingsInDictionary(FileName);
      FileName = SettingsManagerLogic.GetInGameEngineIniLocation();
      _settingsManagerLogic.WriteSettingsInDictionary(FileName, true);
      }

    public bool CanUpdateSaveSet
      {
      get
        {
        return SaveSetName?.Length > 3 && SetIsLoaded;
        }
      }
    public void UpdateSaveSet()
      {
      // Get all updates from UI and set them back into the logic function.
      Update();
      //_settingsManagerLogic.SettingsDictionary = CopyBindableSettings(SettingsDictionary);
      _settingsManagerLogic.SaveSetName = SaveSetName;
      _settingsManagerLogic.WriteSettingsToSaveSet();
      }

    public bool CanUpdateLoadedSettings
      {
      get
        {
        return SetIsLoaded;
        }
      }

    public void UpdateLoadedSettings()
      {
      Update();
      SettingsDictionary = new BindableCollection<ISetting>(_settingsManagerLogic.SettingsDictionary.OrderBy(x => x.Key));
      }

    public static List<ISetting> CopyBindableSettings(BindableCollection<ISetting> settingsDictionary)
      {
      var output = new List<ISetting>();
      foreach (var s in settingsDictionary)
        {
        output.Add(s);
        }
      return output;
      }

    public bool CanOpenUserSettingsFile
      {
      get
        {
        return SelectedSaveSet != null;
        }
      }

    public void OpenUserSettingsFile()
      {
      var path = SettingsManagerLogic.GetSavedSettingsLocation(SelectedSaveSet);
      CApps.EditTextFile(path.FullName);
      }

    public bool CanOpenEngineIniFile
      {
      get
        {
        return SelectedSaveSet != null;
        }
      }

    public void OpenEngineIniFile()
      {
      var path = SettingsManagerLogic.GetEngineIniLocation(SelectedSaveSet);
      CApps.EditTextFile(path.FullName);
      }

    public Task CloseForm()
      {
      TryCloseAsync();
      return Task.CompletedTask;
      }
    #endregion


    }
  }
