using Styles.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using TSWTools;

namespace ToolkitForTSW
  {
  public class CSettingsUser: Notifier
    {
    private CSettingsManager _SettingsManager;
    public CSettingsManager SettingsManager
      {
      get { return _SettingsManager; }
      set
        {
        _SettingsManager = value;
        OnPropertyChanged("SettingsManager");
        }
      }

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
      AutoLoadJourneys = GetBooleanValue("bAutoLoadJourneys");
      HideUIForScreenshots = GetBooleanValue("bHideUIInDTLScreenshots");
      BigSpeedoMeter = GetBooleanValue("bLargeSpeedoHUD");
      Immersive = GetBooleanValueFromInt("ControllerLayout");
      }

    public void Update()
      {
      WriteAutoLoadJourneys();
      WriteHideUIInDTLScreenshots();
      WriteImmersive();
      WriteBigSpeedoMeter();
      }
    private Boolean GetBooleanValueFromInt(String Key)
      {
      SettingsManager.GetSetting(Key, out var Temp);
      if (Temp.Length == 0 || (String.CompareOrdinal(Temp, "0") == 0))
        {
        return false;
        }
      return true;
      }

    private Boolean GetBooleanValue(String Key)
      {
      SettingsManager.GetSetting(Key, out var Temp);
      if (Temp.Length == 0 || (String.CompareOrdinal(Temp, "False") == 0))
        {
        return false;
        }
      return true;
      }
    private void WriteAutoLoadJourneys()
      {
      if (AutoLoadJourneys)
        {
        SettingsManager.UpdateSetting("bAutoLoadJourneys", "True", SectionEnum.User);
        }
      else
        {
        SettingsManager.UpdateSetting("bAutoLoadJourneys", "False", SectionEnum.User);
        }
      }

    private void WriteHideUIInDTLScreenshots()
      {
      if (HideUIForScreenshots)
        {
        SettingsManager.UpdateSetting("bHideUIInDTLScreenshots", "True", SectionEnum.User);
        }
      else
        {
        SettingsManager.UpdateSetting("bHideUIInDTLScreenshots", "False", SectionEnum.User);
        }
      }

    private void WriteBigSpeedoMeter()
      {
      if (BigSpeedoMeter)
        {
        SettingsManager.UpdateSetting("bLargeSpeedoHUD", "True", SectionEnum.User);
        }
      else
        {
        SettingsManager.UpdateSetting("bLargeSpeedoHUD", "False", SectionEnum.User);
        }
      }
    private void WriteImmersive()
      {
      if (Immersive)
        {
        SettingsManager.UpdateSetting("ControllerLayout", "1", SectionEnum.User);
        }
      else
        {
        SettingsManager.UpdateSetting("ControllerLayout", "0", SectionEnum.User);
        }
      }
    }
  }
