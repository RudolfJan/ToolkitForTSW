using Caliburn.Micro;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using ToolkitForTSW.Settings.Enums;
using ToolkitForTSW.Settings.EventModels;

namespace ToolkitForTSW.Settings.ViewModels
  {
  public class SettingsQualityViewModel : Screen, IHandle<UseAdvancedEvent>
    {
    private readonly ISetting _setting;
    private readonly IEventAggregator _events;

    #region Properties


    public SettingsAdvancedViewModel SettingsAdvanced { get; set; }

    private QualityEnum _OverAllQualityLevel;
    public QualityEnum OverAllQualityLevel
      {
      get { return _OverAllQualityLevel; }
      set
        {
        _OverAllQualityLevel = value;
        NotifyOfPropertyChange(nameof(OverAllQualityLevel));
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
        NotifyOfPropertyChange(nameof(QualityLevel));
        }
      }

    private QualityEnum _AudioQualityLevel;
    public QualityEnum AudioQualityLevel
      {
      get { return _AudioQualityLevel; }
      set
        {
        _AudioQualityLevel = value;
        NotifyOfPropertyChange(nameof(AudioQualityLevel));
        }
      }

    private QualityPlusOffEnum _ShadowQuality;
    public QualityPlusOffEnum ShadowQuality
      {
      get { return _ShadowQuality; }
      set
        {
        _ShadowQuality = value;
        NotifyOfPropertyChange(nameof(ShadowQuality));
        }
      }

    private QualityEnum _TextureQuality;
    public QualityEnum TextureQuality
      {
      get { return _TextureQuality; }
      set
        {
        _TextureQuality = value;
        NotifyOfPropertyChange(nameof(TextureQuality));
        }
      }

    private QualityEnum _ViewDistanceQuality;
    public QualityEnum ViewDistanceQuality
      {
      get { return _ViewDistanceQuality; }
      set
        {
        _ViewDistanceQuality = value;
        NotifyOfPropertyChange(nameof(ViewDistanceQuality));
        }
      }

    private QualityPlusOffEnum _EffectsQuality;
    public QualityPlusOffEnum EffectsQuality
      {
      get { return _EffectsQuality; }
      set
        {
        _EffectsQuality = value;
        NotifyOfPropertyChange(nameof(EffectsQuality));
        }
      }

    private QualityEnum _FoliageQuality;
    public QualityEnum FoliageQuality
      {
      get { return _FoliageQuality; }
      set
        {
        _FoliageQuality = value;
        NotifyOfPropertyChange(nameof(FoliageQuality));
        }
      }

    private QualityEnum _PostProcessQuality;
    public QualityEnum PostProcessQuality
      {
      get { return _PostProcessQuality; }
      set
        {
        _PostProcessQuality = value;
        NotifyOfPropertyChange(nameof(PostProcessQuality));
        }
      }

    private AntiAliasingEnum _AntiAliasingMethod;
    public AntiAliasingEnum AntiAliasingMethod
      {
      get { return _AntiAliasingMethod; }
      set
        {
        _AntiAliasingMethod = value;
        NotifyOfPropertyChange(nameof(AntiAliasingMethod));
        }
      }

    private Int32 _MaxFrameRate;
    public Int32 MaxFrameRate
      {
      get { return _MaxFrameRate; }
      set
        {
        _MaxFrameRate = value;
        NotifyOfPropertyChange(nameof(MaxFrameRate));
        }
      }

    private Boolean _ScreenShotQuality;
    public Boolean ScreenShotQuality
      {
      get { return _ScreenShotQuality; }
      set
        {
        _ScreenShotQuality = value;
        NotifyOfPropertyChange(nameof(ScreenShotQuality));
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
        // Do not write registry here, we do that only in SettingsAdvanced, where you can change this option
        _UseAdvanced = value;
        NotifyOfPropertyChange(nameof(UseAdvanced));
        }
      }

    #endregion

    public SettingsQualityViewModel(ISetting setting, IEventAggregator events)
      {
      _setting = setting;
      DisplayName = "Quality";
      _events = events;
      _events.SubscribeOnPublishedThread(this);
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
      MaxFrameRate = _setting.GetIntValue("MaxFPS", 0);
      AntiAliasingMethod = GetAntiAliasingMethod("AAMethod");
      ViewDistanceQuality = GetQuality("ViewDistanceQuality");
      }

    public void Update()
      {
      CultureInfo Gb = CultureInfo.CreateSpecificCulture("en-GB");
      _setting.WriteStringValue(((Int32)OverAllQualityLevel).ToString(), "OverallQualityLevel", SectionEnum.User);

      _setting.WriteStringValue(((Int32)TextureQuality).ToString(), "TextureQuality", SectionEnum.User);
      _setting.WriteStringValue(((Int32)ShadowQuality).ToString(), "ShadowQuality", SectionEnum.User);
      _setting.WriteStringValue(((Int32)FoliageQuality).ToString(), "FoliageQuality", SectionEnum.User);
      _setting.WriteStringValue(((Int32)EffectsQuality).ToString(), "EffectsQuality", SectionEnum.User);
      _setting.WriteStringValue(((Int32)PostProcessQuality).ToString(), "PostProcessQuality", SectionEnum.User);
      _setting.WriteStringValue(((Int32)AudioQualityLevel).ToString(), "AudioQualityLevel", SectionEnum.User);
      _setting.WriteStringValue(MaxFrameRate.ToString("0.0000", Gb), "MaxFPS", SectionEnum.User);
      _setting.WriteStringValue(((Int32)AntiAliasingMethod).ToString(), "AAMethod", SectionEnum.User);
      if (UseAdvanced)
        {
        if (SettingsAdvanced.ViewDistanceScale > 3)
          {
          ViewDistanceQuality = QualityEnum.Ultra;
          }
        else
          {
          ViewDistanceQuality = (QualityEnum)SettingsAdvanced.ViewDistanceScale;
          }
        }
      _setting.WriteStringValue(((Int32)ViewDistanceQuality).ToString(), "ViewDistanceQuality", SectionEnum.User);
      }

    Task IHandle<UseAdvancedEvent>.HandleAsync(UseAdvancedEvent message, CancellationToken cancellationToken)
      {
      UseAdvanced = message.UseAdvanced;
      return Task.CompletedTask;
      }
    private QualityEnum GetQuality(String Key)
      {
      return (QualityEnum)_setting.GetIntValue(Key, (int)QualityEnum.Medium);
      }

    private QualityPlusOffEnum GetQualityPlusOff(String Key)
      {
      return (QualityPlusOffEnum)_setting.GetIntValue(Key, (int)QualityPlusOffEnum.Medium);
      }

    private AntiAliasingEnum GetAntiAliasingMethod(String Key)
      {
      return (AntiAliasingEnum)_setting.GetIntValue(Key, (int)AntiAliasingEnum.Off);
      }
    }
  }
