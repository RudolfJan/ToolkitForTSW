using Caliburn.Micro;
using System;
using ToolkitForTSW.Settings.Enums;

namespace ToolkitForTSW.Settings.ViewModels
  {
  public class SettingsHUDViewModel : Screen
    {
    private readonly ISetting _setting;
    private Boolean _ObjectiveMarker;

    public Boolean ObjectiveMarker
      {
      get { return _ObjectiveMarker; }
      set
        {
        _ObjectiveMarker = value;
        NotifyOfPropertyChange(nameof(ObjectiveMarker));
        }
      }

    private Boolean _StopAtMarker;

    public Boolean StopAtMarker
      {
      get { return _StopAtMarker; }
      set
        {
        _StopAtMarker = value;
        NotifyOfPropertyChange(nameof(StopAtMarker));
        }
      }

    private HudStyleEnum _NextSignalMarker;

    public HudStyleEnum NextSignalMarker
      {
      get { return _NextSignalMarker; }
      set
        {
        _NextSignalMarker = value;
        NotifyOfPropertyChange(nameof(NextSignalMarker));
        }
      }

    private HudStyleEnum _NextSpeedLimitMarker;

    public HudStyleEnum NextSpeedLimitMarker
      {
      get { return _NextSpeedLimitMarker; }
      set
        {
        _NextSpeedLimitMarker = value;
        NotifyOfPropertyChange(nameof(NextSpeedLimitMarker));
        }
      }

    private Boolean _NextSignalAspect;

    public Boolean NextSignalAspect
      {
      get { return _NextSignalAspect; }
      set
        {
        _NextSignalAspect = value;
        NotifyOfPropertyChange(nameof(NextSignalAspect));
        }
      }

    private Boolean _ScenarioMarker;

    public Boolean ScenarioMarker
      {
      get { return _ScenarioMarker; }
      set
        {
        _ScenarioMarker = value;
        NotifyOfPropertyChange(nameof(ScenarioMarker));
        }
      }

    private Boolean _Score;

    public Boolean Score
      {
      get { return _Score; }
      set
        {
        _Score = value;
        NotifyOfPropertyChange(nameof(Score));
        }
      }

    private Boolean _Compass;

    public Boolean Compass
      {
      get { return _Compass; }
      set
        {
        _Compass = value;
        NotifyOfPropertyChange(nameof(Compass));
        }
      }

    private Boolean _StopMarker = true;
    public Boolean StopMarker
      {
      get { return _StopMarker; }
      set
        {
        _StopMarker = value;
        NotifyOfPropertyChange(nameof(StopMarker));
        }
      }

    private Boolean _safetySystemHelper = true;
    public Boolean SafetySystemHelper
      {
      get { return _safetySystemHelper; }
      set
        {
        _safetySystemHelper = value;
        NotifyOfPropertyChange(nameof(SafetySystemHelper));
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

    public SettingsHUDViewModel(ISetting setting)
      {
      _setting = setting;
      DisplayName = "HUD";
      }

    public void Init()
      {
      ObjectiveMarker = _setting.GetBooleanValue("ShowObjectiveMarker", true);
      StopAtMarker = _setting.GetBooleanValue("ShowObjectiveStopAtMarker", true);
      GetNextSignalMarker();
      NextSignalAspect = _setting.GetBooleanValue("ShowNextSignalAspectMarker", true);
      GetNextSpeedLimitMarker();
      Compass = _setting.GetBooleanValue("ShowCompass", true);
      Score = _setting.GetBooleanValue("ShowScore", true);
      StopMarker = _setting.GetBooleanValue("ShowObjectiveStopAtMarker", true);
      ScenarioMarker = _setting.GetBooleanValue("ShowScenarioMarker", true);
      SafetySystemHelper = _setting.GetBooleanValue("bDisplaySafetySystemHelper", true);
      BigSpeedoMeter = _setting.GetBooleanValue("bLargeSpeedoHUD", false);
      }

    public void Update()
      {
      _setting.WriteBooleanValue(ObjectiveMarker, "ShowObjectiveMarker", SectionEnum.User);
      _setting.WriteBooleanValue(StopAtMarker, "ShowObjectiveStopAtMarker", SectionEnum.User);
      _setting.WriteBooleanValue(NextSignalAspect, "ShowNextSignalAspectMarker", SectionEnum.User);
      WriteNextSignalMarker();
      WriteNextSpeedLimitMarker();
      _setting.WriteBooleanValue(Compass, "ShowCompass", SectionEnum.User);
      _setting.WriteBooleanValue(StopMarker, "ShowObjectiveStopAtMarker", SectionEnum.User);
      _setting.WriteBooleanValue(ScenarioMarker, "ShowScenarioMarker", SectionEnum.User);
      _setting.WriteBooleanValue(Score, "ShowScore", SectionEnum.User);
      _setting.WriteBooleanValue(SafetySystemHelper, "bDisplaySafetySystemHelper", SectionEnum.User);
      _setting.WriteBooleanValue(BigSpeedoMeter, "bLargeSpeedoHUD", SectionEnum.User);
      }

    private void GetNextSignalMarker()
      {
      var NextSignalMarkerBool = _setting.GetBooleanValue("ShowNextSignalMarker", true);
      var ShowNextSignalScreenBool = _setting.GetBooleanValue("ShowNextSignalScreen", true);
      NextSignalMarker = HudStyleEnum.None;
      if (NextSignalMarkerBool && ShowNextSignalScreenBool)
        {
        NextSignalMarker = HudStyleEnum.Both;
        return;
        }

      if (!NextSignalMarkerBool && ShowNextSignalScreenBool)
        {
        NextSignalMarker = HudStyleEnum.HUD;
        return;
        }

      NextSignalMarker = HudStyleEnum.Marker;
      }

    public void WriteNextSignalMarker()
      {
      if (NextSignalMarker == HudStyleEnum.Marker || NextSignalMarker == HudStyleEnum.Both)
        {
        _setting.WriteBooleanValue(true, "ShowNextSignalMarker", SectionEnum.User);
        }
      else
        {
        _setting.WriteBooleanValue(false, "ShowNextSignalMarker", SectionEnum.User);
        }

      if (NextSignalMarker == HudStyleEnum.HUD || NextSignalMarker == HudStyleEnum.Both)
        {
        _setting.WriteBooleanValue(true, "ShowNextSignalScreen", SectionEnum.User);
        }
      else
        {
        _setting.WriteBooleanValue(false, "ShowNextSignalScreen", SectionEnum.User);
        }
      }

    private void GetNextSpeedLimitMarker()
      {
      var ShowSpeedLimitMarkerBool = _setting.GetBooleanValue("ShowSpeedLimitMarker", true);
      var ShowSpeedLimitScreenBool = _setting.GetBooleanValue("ShowSpeedLimitScreen", true);
      NextSpeedLimitMarker = HudStyleEnum.None;
      if (ShowSpeedLimitMarkerBool && ShowSpeedLimitScreenBool)
        {
        NextSpeedLimitMarker = HudStyleEnum.Both;
        return;
        }

      if (!ShowSpeedLimitMarkerBool && ShowSpeedLimitScreenBool)
        {
        NextSpeedLimitMarker = HudStyleEnum.HUD;
        return;
        }

      NextSpeedLimitMarker = HudStyleEnum.Marker;
      }

    public void WriteNextSpeedLimitMarker()
      {
      if (NextSpeedLimitMarker == HudStyleEnum.Marker || NextSpeedLimitMarker == HudStyleEnum.Both)
        {
        _setting.WriteBooleanValue(true, "ShowSpeedLimitMarker", SectionEnum.User);
        }
      else
        {
        _setting.WriteBooleanValue(false, "ShowSpeedLimitMarker", SectionEnum.User);
        }

      if (NextSpeedLimitMarker == HudStyleEnum.HUD || NextSpeedLimitMarker == HudStyleEnum.Both)
        {
        _setting.WriteBooleanValue(true, "ShowSpeedLimitScreen", SectionEnum.User);
        }
      else
        {
        _setting.WriteBooleanValue(false, "ShowSpeedLimitScreen", SectionEnum.User);
        }
      }
    }
  }
