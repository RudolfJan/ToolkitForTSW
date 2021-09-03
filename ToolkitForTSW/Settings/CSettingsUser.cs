using Styles.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using ToolkitForTSW;
using ToolkitForTSW.Settings;

namespace ToolkitForTSW.Settings
  {
  public class CSettingsUser: CSetting
    {
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
        OnPropertyChanged("AutoLoadJourneys");
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
        OnPropertyChanged("Immersive");
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
        OnPropertyChanged("HideUIForScreenshots");
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
        OnPropertyChanged("BigSpeedoMeter");
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
        OnPropertyChanged("AutoHideCrosshair");
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
        OnPropertyChanged("CrosshairVisibility");
        }
      }


    public CSettingsUser(CSettingsManager _settingsManager)
      {
      SettingsManager = _settingsManager;
      }

    public void Init()
      {
      AutoLoadJourneys = GetBooleanValue("bAutoLoadJourneys",true);
      HideUIForScreenshots = GetBooleanValue("bHideUIInDTLScreenshots",true);
      BigSpeedoMeter = GetBooleanValue("bLargeSpeedoHUD",false);
      Immersive = GetBooleanValueFromInt("ControllerLayout",true);
      AutoHideCrosshair = GetBooleanValueFromInt("AutoHideCrosshair", true);
      GetCrosshairVisbility();
      }

    public void Update()
      {
      WriteBooleanValue(AutoLoadJourneys,"bAutoLoadJourneys", SectionEnum.User);
      WriteBooleanValue(HideUIForScreenshots,"bHideUIInDTLScreenshots", SectionEnum.User); 
      WriteBooleanValue(BigSpeedoMeter,"bLargeSpeedoHUD", SectionEnum.User);
      WriteBooleanValueAsInt(Immersive,"ControllerLayout", SectionEnum.User);
      WriteBooleanValueAsInt(AutoHideCrosshair, "AutoHideCrosshair", SectionEnum.User);
      WriteCrosshairVisbility();
      }

    private void GetCrosshairVisbility()
      {
      SettingsManager.GetSetting("CursorType", out var Temp);
      if (string.CompareOrdinal(Temp, "Off") == 0)
        {
        CrosshairVisibility = CrosshairVisibilityEnum.Off;
        return;
        }
      SettingsManager.GetSetting("CursorType", out Temp);
      CrosshairVisibility = CrosshairVisibilityEnum.Off;
      if (String.CompareOrdinal(Temp, "Medium") == 0)
        {
        CrosshairVisibility = CrosshairVisibilityEnum.Fifty;
        return;
        }
      if (String.CompareOrdinal(Temp, "Full") == 0)
        {
        CrosshairVisibility = CrosshairVisibilityEnum.Hundred;
        return;
        }
      if (String.CompareOrdinal(Temp, "Large") == 0)
        {
        CrosshairVisibility = CrosshairVisibilityEnum.Large;
        return;
        }
      }

private void WriteCrosshairVisbility()
      {
      switch (CrosshairVisibility)
        {
        case CrosshairVisibilityEnum.Off:
            {
            SettingsManager.UpdateSetting("CursorType", "Off", SectionEnum.User);
            break;
            }
        case CrosshairVisibilityEnum.Fifty:
            {
            SettingsManager.UpdateSetting("CursorType", "Medium", SectionEnum.User);
            break;
            }
        case CrosshairVisibilityEnum.Hundred:
            {
            SettingsManager.UpdateSetting("CursorType", "Full", SectionEnum.User);
            break;
            }
        case CrosshairVisibilityEnum.Large:
            {
            SettingsManager.UpdateSetting("CursorType", "Large", SectionEnum.User);
            break;
            }
        }
      }
    }
  }
