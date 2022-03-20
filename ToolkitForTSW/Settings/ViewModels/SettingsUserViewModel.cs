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

    private bool _bigSpeedoMeter;
    public bool BigSpeedoMeter
      {
      get
        {
        return _bigSpeedoMeter;
        }
      set
        {
        _bigSpeedoMeter = value;
        NotifyOfPropertyChange(nameof(BigSpeedoMeter));
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
      BigSpeedoMeter = _setting.GetBooleanValue("bLargeSpeedoHUD", false);
      Immersive = _setting.GetBooleanValueFromInt("ControllerLayout", true);
      AutoHideCrosshair = _setting.GetBooleanValueFromInt("AutoHideCrosshair", true);
      GetCrosshairVisbility();
      }

    public void Update()
      {
      _setting.WriteBooleanValue(AutoLoadJourneys, "bAutoLoadJourneys", SectionEnum.User);
      _setting.WriteBooleanValue(HideUIForScreenshots, "bHideUIInDTLScreenshots", SectionEnum.User);
      _setting.WriteBooleanValue(BigSpeedoMeter, "bLargeSpeedoHUD", SectionEnum.User);
      _setting.WriteBooleanValueAsInt(Immersive, "ControllerLayout", SectionEnum.User);
      _setting.WriteBooleanValueAsInt(AutoHideCrosshair, "AutoHideCrosshair", SectionEnum.User);
      _setting.WriteStringValue(CrosshairVisibility.ToString(), "CursorType", SectionEnum.User);
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
