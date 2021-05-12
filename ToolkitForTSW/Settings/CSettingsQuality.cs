using System;
using System.Globalization;



namespace ToolkitForTSW.Settings
  {
  public class CSettingsQuality : CSetting
    {

    #region Properties

    private QualityEnum _OverAllQualityLevel;
    public QualityEnum OverAllQualityLevel
      {
      get { return _OverAllQualityLevel; }
      set
        {
        _OverAllQualityLevel = value;
        OnPropertyChanged("OverAllQualityLevel");
        }
      }

    /*
    Not sure if this is needed
    */
    private QualityEnum _QualityLevel;
    public QualityEnum QualityLevel
      {
      get { return _QualityLevel; }
      set
        {
        _QualityLevel = value;
        OnPropertyChanged("QualityLevel");
        }
      }

    private QualityEnum _AudioQualityLevel;
    public QualityEnum AudioQualityLevel
      {
      get { return _AudioQualityLevel; }
      set
        {
        _AudioQualityLevel = value;
        OnPropertyChanged("AudioQualityLevel");
        }
      }

    private QualityPlusOffEnum _ShadowQuality;
    public QualityPlusOffEnum ShadowQuality
      {
      get { return _ShadowQuality; }
      set
        {
        _ShadowQuality = value;
        OnPropertyChanged("ShadowQuality");
        }
      }

    private QualityEnum _TextureQuality;
    public QualityEnum TextureQuality
      {
      get { return _TextureQuality; }
      set
        {
        _TextureQuality = value;
        OnPropertyChanged("TextureQuality");
        }
      }

    private QualityEnum _ViewDistanceQuality;
    public QualityEnum ViewDistanceQuality
      {
      get { return _ViewDistanceQuality; }
      set
        {
        _ViewDistanceQuality = value;
        OnPropertyChanged("ViewDistanceQuality");
        }
      }

    private QualityPlusOffEnum _EffectsQuality;
    public QualityPlusOffEnum EffectsQuality
      {
      get { return _EffectsQuality; }
      set
        {
        _EffectsQuality = value;
        OnPropertyChanged("EffectsQuality");
        }
      }

    private QualityEnum _FoliageQuality;
    public QualityEnum FoliageQuality
      {
      get { return _FoliageQuality; }
      set
        {
        _FoliageQuality = value;
        OnPropertyChanged("FoliageQuality");
        }
      }



    private QualityEnum _PostProcessQuality;
    public QualityEnum PostProcessQuality
      {
      get { return _PostProcessQuality; }
      set
        {
        _PostProcessQuality = value;
        OnPropertyChanged("PostProcessQuality");
        }
      }

    private AntiAliasingEnum _AntiAliasingMethod;
    public AntiAliasingEnum AntiAliasingMethod
      {
      get { return _AntiAliasingMethod; }
      set
        {
        _AntiAliasingMethod = value;
        OnPropertyChanged("AntiAliasingMethod");
        }
      }

    private Int32 _MaxFrameRate;
    public Int32 MaxFrameRate
      {
      get { return _MaxFrameRate; }
      set
        {
        _MaxFrameRate = value;
        OnPropertyChanged("MaxFrameRate");
        }
      }

    private Boolean _ScreenShotQuality;
    public Boolean ScreenShotQuality
      {
      get { return _ScreenShotQuality; }
      set
        {
        _ScreenShotQuality = value;
        OnPropertyChanged("ScreenShotQuality");
        }
      }


    #endregion

    public CSettingsQuality(CSettingsManager _settingsManager)
      {
      SettingsManager = _settingsManager;
      }
    public void Init()
      {
      OverAllQualityLevel = GetQuality("OverallQualityLevel");
      TextureQuality = GetQuality("TextureQuality");
      FoliageQuality = GetQuality("FoliageQuality");
      ShadowQuality = GetQualityPlusOff("ShadowQuality");
      EffectsQuality = GetQualityPlusOff("EffectsQuality");
      PostProcessQuality = GetQuality("PostProcessQuality");
      AudioQualityLevel = GetQuality("AudioQualityLevel");
      MaxFrameRate = GetIntValue("MaxFPS", 0);
      AntiAliasingMethod = GetAntiAliasingMethod("AAMethod");
      ViewDistanceQuality = GetQuality("ViewDistanceQuality");
      }

    public void Update()
      {
      CultureInfo Gb = CultureInfo.CreateSpecificCulture("en-GB");
      SettingsManager.UpdateSetting("OverallQualityLevel", ((Int32)OverAllQualityLevel).ToString(), SectionEnum.User);
      SettingsManager.UpdateSetting("TextureQuality", ((Int32)TextureQuality).ToString(), SectionEnum.User);
      SettingsManager.UpdateSetting("ShadowQuality", ((Int32)ShadowQuality).ToString(), SectionEnum.User);
      SettingsManager.UpdateSetting("FoliageQuality", ((Int32)FoliageQuality).ToString(), SectionEnum.User);
      SettingsManager.UpdateSetting("EffectsQuality", ((Int32)EffectsQuality).ToString(), SectionEnum.User);
      SettingsManager.UpdateSetting("PostProcessQuality", ((Int32)PostProcessQuality).ToString(), SectionEnum.User);
      SettingsManager.UpdateSetting("AudioQualityLevel", ((Int32)AudioQualityLevel).ToString(), SectionEnum.User);
      SettingsManager.UpdateSetting("MaxFPS", MaxFrameRate.ToString("0.0000", Gb), SectionEnum.User);
      SettingsManager.UpdateSetting("AAMethod", ((Int32)AntiAliasingMethod).ToString(), SectionEnum.User);
      if (SettingsManager.UseAdvanced)
        {
        if (SettingsManager.SettingsAdvanced.ViewDistanceScale > 3)
          {
          ViewDistanceQuality = QualityEnum.Ultra;
          }
        else
          {
          ViewDistanceQuality = (QualityEnum) SettingsManager.SettingsAdvanced.ViewDistanceScale;
          }
        }
      SettingsManager.UpdateSetting("ViewDistanceQuality", ((Int32)ViewDistanceQuality).ToString(), SectionEnum.User);
      }

    private QualityEnum GetQuality(String Key)
      {
      SettingsManager.GetSetting(Key, out var Temp);
      if (Temp.Length == 0)
        {
        return QualityEnum.Medium;
        }
      return (QualityEnum)Convert.ToInt32(Temp);
      }

    private QualityPlusOffEnum GetQualityPlusOff(String Key)
      {
      SettingsManager.GetSetting(Key, out var Temp);
      if (Temp.Length == 0)
        {
        return QualityPlusOffEnum.Medium;
        }
      return (QualityPlusOffEnum)Convert.ToInt32(Temp);
      }



    private AntiAliasingEnum GetAntiAliasingMethod(String Key)
      {
      SettingsManager.GetSetting(Key, out var Temp);
      if (Temp.Length == 0)
        {
        return AntiAliasingEnum.Off;
        }
      return (AntiAliasingEnum)Convert.ToInt32(Temp);
      }
    }
  }
