using Styles.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using ToolkitForTSW;

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
      }

    public void Update()
      {
      WriteBooleanValue(AutoLoadJourneys,"bAutoLoadJourneys", SectionEnum.User);
      WriteBooleanValue(HideUIForScreenshots,"bHideUIInDTLScreenshots", SectionEnum.User); 
      WriteBooleanValue(BigSpeedoMeter,"bLargeSpeedoHUD", SectionEnum.User);
      WriteBooleanValueAsInt(Immersive,"ControllerLayout", SectionEnum.User);
      }
    }
  }
