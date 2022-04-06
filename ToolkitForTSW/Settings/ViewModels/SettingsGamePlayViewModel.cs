using Caliburn.Micro;
using System;
using ToolkitForTSW.Settings.Enums;

namespace ToolkitForTSW.Settings.ViewModels
  {
  public class SettingsGamePlayViewModel : Screen
    {
    private readonly ISetting _setting;
    #region Properties

    private Boolean _QuickWalk;

    public Boolean QuickWalk
      {
      get { return _QuickWalk; }
      set
        {
        _QuickWalk = value;
        NotifyOfPropertyChange(nameof(QuickWalk));
        }
      }

    private Boolean _DisableJunctionDerail;

    public Boolean DisableJunctionDerail
      {
      get { return _DisableJunctionDerail; }
      set
        {
        _DisableJunctionDerail = value;
        NotifyOfPropertyChange(nameof(DisableJunctionDerail));
        }
      }

    private Boolean _CabSway;
    public Boolean CabSway
      {
      get { return _CabSway; }
      set
        {
        _CabSway = value;
        NotifyOfPropertyChange(nameof(CabSway));
        }
      }

    private GradeUnitsEnum _GradeUnits;

    public GradeUnitsEnum GradeUnits
      {
      get { return _GradeUnits; }
      set
        {
        _GradeUnits = value;
        NotifyOfPropertyChange(nameof(GradeUnits));
        }
      }

    private UnitsEnum _Units;

    public UnitsEnum Units
      {
      get { return _Units; }
      set
        {
        _Units = value;
        NotifyOfPropertyChange(nameof(Units));
        }
      }

    private TemperatureEnum _TemperatureUnits;

    public TemperatureEnum TemperatureUnits
      {
      get { return _TemperatureUnits; }
      set
        {
        _TemperatureUnits = value;
        NotifyOfPropertyChange(nameof(TemperatureUnits));
        }
      }

    private Boolean _ForceFeedback;

    public Boolean ForceFeedback
      {
      get { return _ForceFeedback; }
      set
        {
        _ForceFeedback = value;
        NotifyOfPropertyChange(nameof(ForceFeedback));
        }
      }

    private int _cabSwayLevelPercentage;
    public int CabSwayLevelPercentage
      {
      get
        {
        return _cabSwayLevelPercentage;
        }
      set
        {
        _cabSwayLevelPercentage = value;
        NotifyOfPropertyChange(nameof(CabSwayLevelPercentage));
        }
      }
    public double CabSwayLevel { get; set; }
    #endregion

    public SettingsGamePlayViewModel(ISetting setting)
      {
      _setting = setting;
      DisplayName = "Game play";
      }

    public void Init()
      {
      QuickWalk = _setting.GetBooleanValue("QuickWalk", false);
      CabSway = _setting.GetBooleanValue("bCameraMotionSwayEnabled", true);
      CabSwayLevel = _setting.GetDoubleValue("CameraMotionSwayLevel", 1);
      CabSwayLevelPercentage = (int)(CabSwayLevel * 100);
      ForceFeedback = _setting.GetBooleanValue("ForceFeedback", false);
      DisableJunctionDerail = _setting.GetBooleanValue("DisableJunctionDerail", false);
      GetUnits();
      GetGradeUnits();
      GetTemperatureUnits();
      }

    public void Update()
      {
      _setting.WriteBooleanValue(CabSway, "bCameraMotionSwayEnabled", SectionEnum.User);
      _setting.WriteBooleanValue(QuickWalk, "QuickWalk", SectionEnum.User);
      _setting.WriteBooleanValue(ForceFeedback, "ForceFeedback", SectionEnum.User);
      CabSwayLevel = ((double)CabSwayLevelPercentage / 100.0);
      _setting.WriteDoubleValue(CabSwayLevel, "CameraMotionSwayLevel", SectionEnum.User);
      _setting.WriteBooleanValue(DisableJunctionDerail, "DisableJunctionDerail", SectionEnum.User);
      WriteGradeSettings();
      WriteMeasurement();
      WriteTemperature();
      }

    private void WriteTemperature()
      {
      switch (TemperatureUnits)
        {
        case TemperatureEnum.Automatic:
            {
            _setting.WriteStringValue("Automatic", "TemperatureSetting", SectionEnum.User);
            break;
            }
        case TemperatureEnum.Fahrenheit:
            {
            _setting.WriteStringValue("Manual", "TemperatureSetting", SectionEnum.User);
            _setting.WriteStringValue("TemperatureUnit", TemperatureUnits.ToString(), SectionEnum.User);
            break;
            }
        case TemperatureEnum.Celsius:
            {
            _setting.WriteStringValue("Manual", "TemperatureSetting", SectionEnum.User);
            _setting.WriteStringValue(TemperatureUnits.ToString(), "TemperatureUnit", SectionEnum.User);
            break;
            }
        }
      }

    private void WriteMeasurement()
      {
      switch (Units)
        {
        case UnitsEnum.Automatic:
            {
            _setting.WriteStringValue("Automatic", "MeasurementSetting", SectionEnum.User);
            break;
            }
        case UnitsEnum.Imperial:
            {
            _setting.WriteStringValue("Manual", "MeasurementSetting", SectionEnum.User);
            _setting.WriteStringValue(Units.ToString(), "Measurement", SectionEnum.User);
            break;
            }
        case UnitsEnum.Metric:
            {
            _setting.WriteStringValue("Manual", "MeasurementSetting", SectionEnum.User);
            _setting.WriteStringValue(Units.ToString(), "Measurement", SectionEnum.User);
            break;
            }
        }
      }

    private void WriteGradeSettings()
      {
      switch (GradeUnits)
        {
        case GradeUnitsEnum.Automatic:
            {
            _setting.WriteStringValue("Automatic", "GradeSetting", SectionEnum.User);
            break;
            }
        case GradeUnitsEnum.Percentage:
            {
            _setting.WriteStringValue("Manual", "GradeSetting", SectionEnum.User);
            _setting.WriteStringValue(GradeUnits.ToString(), "GradeUnit", SectionEnum.User);
            break;
            }
        case GradeUnitsEnum.Ratio:
            {
            _setting.WriteStringValue("Manual", "GradeSetting", SectionEnum.User);
            _setting.WriteStringValue(GradeUnits.ToString(), "GradeUnit", SectionEnum.User);
            break;
            }
        }
      }

    private void GetUnits()
      {
      var temp = _setting.GetStringValue("MeasurementSetting", "Automatic");
      if (string.CompareOrdinal(temp, "Automatic") == 0)
        {
        Units = UnitsEnum.Automatic;
        return;
        }
      Units = UnitsEnum.Metric;
      if (String.CompareOrdinal(temp, "Imperial") == 0)
        {
        Units = UnitsEnum.Imperial;
        }
      }

    private void GetGradeUnits()
      {
      var temp = _setting.GetStringValue("GradeSetting", "Automatic");

      if (string.CompareOrdinal(temp, "Automatic") == 0)
        {
        GradeUnits = GradeUnitsEnum.Automatic;
        return;
        }
      temp = _setting.GetStringValue("GradeUnits", "");
      GradeUnits = GradeUnitsEnum.Percentage;
      if (String.CompareOrdinal(temp, "Ratio") == 0)
        {
        GradeUnits = GradeUnitsEnum.Ratio;
        }
      }

    private void GetTemperatureUnits()
      {
      var temp = _setting.GetStringValue("TemperatureSetting", "Automatic");
      if (string.CompareOrdinal(temp, "Automatic") == 0)
        {
        TemperatureUnits = TemperatureEnum.Automatic;
        return;
        }
      temp = _setting.GetStringValue("TemperatureUnit", "Automatic");
      TemperatureUnits = TemperatureEnum.Celsius;
      if (String.CompareOrdinal(temp, "Fahrenheit") == 0)
        {
        TemperatureUnits = TemperatureEnum.Fahrenheit;
        }
      }
    }
  }
