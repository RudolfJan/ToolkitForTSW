using System;
using System.Globalization;


namespace ToolkitForTSW.Settings
  {
  public class CSettingsAdvanced : CSetting

    {
    #region Properties
    private UInt32 _ViewDistanceScale = 1;

    public UInt32 ViewDistanceScale
      {
      get { return _ViewDistanceScale; }
      set
        {
        _ViewDistanceScale = value;
        OnPropertyChanged("ViewDistanceScale");
        }
      }

    private Boolean _EyeAdaptationQuality = false;

    public Boolean EyeAdaptationQuality
      {
      get { return _EyeAdaptationQuality; }
      set
        {
        _EyeAdaptationQuality = value;
        OnPropertyChanged("EyeAdaptationQuality");
        }
      }

    private Boolean _MaterialQualityLevel = true;

    public Boolean MaterialQualityLevel
      {
      get { return _MaterialQualityLevel; }
      set
        {
        _MaterialQualityLevel = value;
        OnPropertyChanged("MaterialQualityLevel");
        }
      }

    private UInt32 _MotionBlurQuality = 1;

    public UInt32 MotionBlurQuality
      {
      get { return _MotionBlurQuality; }
      set
        {
        _MotionBlurQuality = value;
        OnPropertyChanged("MotionBlurQuality");
        }
      }

    private UInt32 _LODDistanceScale = 3;

    public UInt32 LODDistanceScale
      {
      get { return _LODDistanceScale; }
      set
        {
        _LODDistanceScale = value;
        OnPropertyChanged("LODDistanceScale");
        }
      }

    private UInt32 _ScreenPercentage = 100;

    public UInt32 ScreenPercentage
      {
      get { return _ScreenPercentage; }
      set
        {
        _ScreenPercentage = value;
        OnPropertyChanged("ScreenPercentage");
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
        OnPropertyChanged("GammaCorrection");
        }
      }

    #endregion

    public CSettingsAdvanced(CSettingsManager _settingsManager)
      {
      SettingsManager = _settingsManager;
      }

    public void Init()
      {
      EyeAdaptationQuality = GetBooleanValue("r.EyeAdaptationQuality",false);
      MaterialQualityLevel = GetBooleanValueFromInt("r.MaterialQualityLevel", false);
      ScreenPercentage = GetUIntValue("ScreenPercentage", 100);
      GammaCorrection = GetDoubleValue("r.Color.mid", 0.5);
      ViewDistanceScale = GetUIntValue("sg.ViewDistanceQuality", 1);
      MotionBlurQuality = GetUIntValue("r.MotionBlurQuality", 0);
      LODDistanceScale = GetUIntValue("foliage.LODDistanceScale", 3);
      }

    public void Update()
      {
      CultureInfo Gb = CultureInfo.CreateSpecificCulture("en-GB");
      WriteBooleanValue(EyeAdaptationQuality, "r.EyeAdaptationQuality", SectionEnum.System);
      WriteBooleanValueAsInt(MaterialQualityLevel, "r.MaterialQualityLevel", SectionEnum.System);

      SettingsManager.UpdateSetting("ScreenPercentage", ScreenPercentage.ToString(),
        SectionEnum.User);
      SettingsManager.UpdateSetting("r.Color.mid", GammaCorrection.ToString("0.00", Gb),
        SectionEnum.System);
      SettingsManager.UpdateSetting("sg.ResolutionQuality", ScreenPercentage.ToString(),
        SectionEnum.Scalability);
      SettingsManager.UpdateSetting("r.ViewDistanceScale", ViewDistanceScale.ToString(),
        SectionEnum.System);
      // Keep this in sync with ViewDistanceQuality, we do not read the value
      SettingsManager.UpdateSetting("sg.ViewDistanceQuality", ViewDistanceScale.ToString(),
        SectionEnum.Scalability);
      SettingsManager.UpdateSetting("r.MotionBlurQuality", MotionBlurQuality.ToString(),
        SectionEnum.System);
      SettingsManager.UpdateSetting("foliage.LODDistanceScale", LODDistanceScale.ToString(),
        SectionEnum.System);
      }

  public void SetRecommendedValues()
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
