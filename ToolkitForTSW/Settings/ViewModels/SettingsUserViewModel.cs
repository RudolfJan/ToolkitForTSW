using Caliburn.Micro;
using System;
using ToolkitForTSW.Settings.Enums;

namespace ToolkitForTSW.Settings.ViewModels
  {
  public class SettingsUserViewModel : Screen
    {
    private readonly ISetting _setting;
    #region Properties
    private bool _autoLoadJourneys;

    public bool AutoLoadJourneys
      {
      get
        {
        return _autoLoadJourneys;
        }
      set
        {
        _autoLoadJourneys = value;
        NotifyOfPropertyChange(nameof(AutoLoadJourneys));
        }
      }


    private bool _inWorldAdvertising;

    public bool InWorldAdvertising
      {
      get { return _inWorldAdvertising; }
      set
        {
        _inWorldAdvertising = value;
        NotifyOfPropertyChange(nameof(InWorldAdvertising));
        }
      }

    private bool _darkMode;

    public bool DarkMode
      {
      get { return _darkMode; }
      set
        {
        _darkMode = value;
        NotifyOfPropertyChange(nameof(DarkMode));
        }
      }


    private bool _immersive;

    public bool Immersive
      {
      get
        {
        return _immersive;
        }
      set
        {
        _immersive = value;
        NotifyOfPropertyChange(nameof(Immersive));
        }
      }

    private bool _hideUIForScreenshots;
    public bool HideUIForScreenshots
      {
      get
        {
        return _hideUIForScreenshots;
        }
      set
        {
        _hideUIForScreenshots = value;
        NotifyOfPropertyChange(nameof(HideUIForScreenshots));
        }
      }



    private bool _autoHideCrosshair;
    public bool AutoHideCrosshair
      {
      get
        {
        return _autoHideCrosshair;
        }
      set
        {
        _autoHideCrosshair = value;
        NotifyOfPropertyChange(nameof(AutoHideCrosshair));
        }
      }

    private CrosshairVisibilityEnum _crosshairVisibility;
    public CrosshairVisibilityEnum CrosshairVisibility
      {
      get
        {
        return _crosshairVisibility;
        }
      set
        {
        _crosshairVisibility = value;
        NotifyOfPropertyChange(nameof(CrosshairVisibility));
        }
      }

    private bool _lightningEffects;
    public bool LightningEffects
      {
      get { return _lightningEffects; }
      set
        {
        _lightningEffects = value;
        NotifyOfPropertyChange(nameof(LightningEffects));
        }
      }

    private bool _arcingSparkEffects;
    public bool ArcingSparkEffects
      {
      get
        {
        return _arcingSparkEffects;
        }
      set
        {
        _arcingSparkEffects = value;
        NotifyOfPropertyChange(nameof(ArcingSparkEffects));
        }
      }

    private bool _gameSaveEnabled;
    public bool GameSaveEnabled
      {
      get { return _gameSaveEnabled; }
      set
        {
        _gameSaveEnabled = value;
        NotifyOfPropertyChange(nameof(GameSaveEnabled));
        }
      }

    private bool _tutorialsInQuickPlay;
    public bool TutorialsInQuickPlay
      {
      get
        {
        return (_tutorialsInQuickPlay);
        }
      set
        {
        _tutorialsInQuickPlay = value;
        NotifyOfPropertyChange(nameof(TutorialsInQuickPlay));
        }
      }


    #endregion

    public SettingsUserViewModel(ISetting setting)
      {
      _setting = setting;
      DisplayName = "User";
      }

    public void Init()
      {
      AutoLoadJourneys = _setting.GetBooleanValue("bAutoLoadJourneys", true);
      HideUIForScreenshots = _setting.GetBooleanValue("bHideUIInDTLScreenshots", true);

      Immersive = _setting.GetBooleanValueFromInt("ControllerLayout", true);
      AutoHideCrosshair = _setting.GetBooleanValueFromInt("AutoHideCrosshair", true);
      DarkMode = _setting.GetBooleanValue("bLiveryEditorDarkMode", true);
      InWorldAdvertising = _setting.GetBooleanValue("bEnableInWorldAdvertising", false);
      LightningEffects = _setting.GetBooleanValue("bLightningEffects", false);
      ArcingSparkEffects = _setting.GetBooleanValue("bArcingSparkEffects", false);
      GameSaveEnabled = _setting.GetBooleanValue("bSaveGameEnabled", true);
      TutorialsInQuickPlay = _setting.GetBooleanValue("bQuickPlayIncludesTutorials", false);
      GetCrosshairVisbility();

      }

    public void Update()
      {
      _setting.WriteBooleanValue(AutoLoadJourneys, "bAutoLoadJourneys", SectionEnum.User);
      _setting.WriteBooleanValue(HideUIForScreenshots, "bHideUIInDTLScreenshots", SectionEnum.User);

      _setting.WriteBooleanValueAsInt(Immersive, "ControllerLayout", SectionEnum.User);
      _setting.WriteBooleanValueAsInt(AutoHideCrosshair, "AutoHideCrosshair", SectionEnum.User);
      _setting.WriteStringValue(CrosshairVisibility.ToString(), "CursorType", SectionEnum.User);
      _setting.WriteBooleanValue(DarkMode, "bLiveryEditorDarkMode", SectionEnum.User);
      _setting.WriteBooleanValue(InWorldAdvertising, "bEnableInWorldAdvertising", SectionEnum.User);
      _setting.WriteBooleanValue(LightningEffects, "bLightningEffects", SectionEnum.User);
      _setting.WriteBooleanValue(ArcingSparkEffects, "bArcingSparkEffects", SectionEnum.User);
      _setting.WriteBooleanValue(GameSaveEnabled, "bSaveGameEnabled", SectionEnum.User);
      _setting.WriteBooleanValue(TutorialsInQuickPlay, "bQuickPlayIncludesTutorials", SectionEnum.User);

      }

    private void GetCrosshairVisbility()
      {
      var temp = _setting.GetStringValue("CursorType", "Off");
      if (string.CompareOrdinal(temp, "Off") == 0)
        {
        CrosshairVisibility = CrosshairVisibilityEnum.Off;
        return;
        }
      CrosshairVisibility = CrosshairVisibilityEnum.Off;
      if (String.CompareOrdinal(temp, "Medium") == 0)
        {
        CrosshairVisibility = CrosshairVisibilityEnum.Medium;
        return;
        }
      if (String.CompareOrdinal(temp, "Full") == 0)
        {
        CrosshairVisibility = CrosshairVisibilityEnum.Full;
        return;
        }
      if (String.CompareOrdinal(temp, "Large") == 0)
        {
        CrosshairVisibility = CrosshairVisibilityEnum.Large;
        return;
        }
      }
    }
  }
