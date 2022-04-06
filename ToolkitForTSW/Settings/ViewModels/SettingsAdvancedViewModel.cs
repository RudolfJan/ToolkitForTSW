using Caliburn.Micro;
using System;
using ToolkitForTSW.Settings.Enums;
using ToolkitForTSW.Settings.EventModels;

namespace ToolkitForTSW.Settings.ViewModels
  {
  public class SettingsAdvancedViewModel : Screen

    {
    private readonly ISetting _setting;
    private readonly IEventAggregator _events;
    #region Properties
    private UInt32 _ViewDistanceScale = 1;

    public UInt32 ViewDistanceScale
      {
      get { return _ViewDistanceScale; }
      set
        {
        _ViewDistanceScale = value;
        NotifyOfPropertyChange(nameof(ViewDistanceScale));
        }
      }

    private Boolean _EyeAdaptationQuality = false;

    public Boolean EyeAdaptationQuality
      {
      get { return _EyeAdaptationQuality; }
      set
        {
        _EyeAdaptationQuality = value;
        NotifyOfPropertyChange(nameof(EyeAdaptationQuality));
        }
      }

    private Boolean _MaterialQualityLevel = true;

    public Boolean MaterialQualityLevel
      {
      get { return _MaterialQualityLevel; }
      set
        {
        _MaterialQualityLevel = value;
        NotifyOfPropertyChange(nameof(MaterialQualityLevel));
        }
      }

    private UInt32 _MotionBlurQuality = 1;

    public UInt32 MotionBlurQuality
      {
      get { return _MotionBlurQuality; }
      set
        {
        _MotionBlurQuality = value;
        NotifyOfPropertyChange(nameof(MotionBlurQuality));
        }
      }

    private UInt32 _LODDistanceScale = 3;

    public UInt32 LODDistanceScale
      {
      get { return _LODDistanceScale; }
      set
        {
        _LODDistanceScale = value;
        NotifyOfPropertyChange(nameof(LODDistanceScale));
        }
      }

    private UInt32 _ScreenPercentage = 100;

    public UInt32 ScreenPercentage
      {
      get { return _ScreenPercentage; }
      set
        {
        _ScreenPercentage = value;
        NotifyOfPropertyChange(nameof(ScreenPercentage));
        }
      }

    /*
Makes mid tones darker or lighter. Supported values 0.2 to 0.8, default 0.5 higher value is lighter
*/
    private Double _GammaCorrection;
    public Double GammaCorrection
      {
      get { return _GammaCorrection; }
      set
        {
        _GammaCorrection = value;
        NotifyOfPropertyChange(nameof(GammaCorrection));
        }
      }

    /*
     if true used advanced settings, otherwise do not write them to output
     */
    private Boolean _UseAdvanced = TSWOptions.UseAdvancedSettings;
    public Boolean UseAdvanced
      {
      get { return _UseAdvanced; }
      set
        {
        _UseAdvanced = value;
        TSWOptions.UseAdvancedSettings = _UseAdvanced;
        // Tell others this is changed
        TSWOptions.WriteToRegistry();
        UseAdvancedEvent useAdvancedEvent = new UseAdvancedEvent
          {
          UseAdvanced = _UseAdvanced
          };
        _events.PublishOnUIThreadAsync(useAdvancedEvent);
        NotifyOfPropertyChange(nameof(UseAdvanced));
        }
      }
    #endregion

    public SettingsAdvancedViewModel(ISetting setting, IEventAggregator events)
      {
      _setting = setting;
      DisplayName = "Advanced";
      _events = events;
      }

    public void Init()
      {
      EyeAdaptationQuality = _setting.GetBooleanValue("r.EyeAdaptationQuality", false);
      MaterialQualityLevel = _setting.GetBooleanValueFromInt("r.MaterialQualityLevel", false);
      ScreenPercentage = _setting.GetUIntValue("ScreenPercentage", 100);
      GammaCorrection = _setting.GetDoubleValue("r.Color.mid", 0.5);
      ViewDistanceScale = _setting.GetUIntValue("sg.ViewDistanceQuality", 1);
      MotionBlurQuality = _setting.GetUIntValue("r.MotionBlurQuality", 0);
      LODDistanceScale = _setting.GetUIntValue("foliage.LODDistanceScale", 3);
      }

    public void Update()
      {
      if (UseAdvanced)
        {
        _setting.WriteBooleanValue(EyeAdaptationQuality, "r.EyeAdaptationQuality", SectionEnum.System);
        _setting.WriteBooleanValueAsInt(MaterialQualityLevel, "r.MaterialQualityLevel", SectionEnum.System);
        _setting.WriteStringValue(ScreenPercentage.ToString(), "ScreenPercentage",
          SectionEnum.User);
        _setting.WriteDoubleValue(GammaCorrection, "r.Color.mid",
                SectionEnum.System);
        _setting.WriteStringValue(ViewDistanceScale.ToString(), "r.ViewDistanceScale",
          SectionEnum.System);
        // Keep this in sync with ViewDistanceQuality, we do not read the value
        _setting.WriteStringValue(ViewDistanceScale.ToString(), "sg.ViewDistanceQuality",
          SectionEnum.Scalability);
        _setting.WriteStringValue(MotionBlurQuality.ToString(), "r.MotionBlurQuality",
          SectionEnum.System);
        _setting.WriteStringValue(LODDistanceScale.ToString(), "foliage.LODDistanceScale",
          SectionEnum.System);
        }
      }

    public void SetRecommendedAdvancedSettings()
      {
      ScreenPercentage = 100;
      ViewDistanceScale = 5;
      MotionBlurQuality = 0;
      LODDistanceScale = 3;
      EyeAdaptationQuality = false;
      MaterialQualityLevel = false;
      GammaCorrection = 0.5;
      }
    }
  }
