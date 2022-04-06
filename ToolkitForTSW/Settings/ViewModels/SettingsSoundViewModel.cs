using Caliburn.Micro;
using System;
using System.Globalization;
using ToolkitForTSW.Settings.Enums;

namespace ToolkitForTSW.Settings.ViewModels
  {
  public class SettingsSoundViewModel : Screen
    {
    private readonly ISetting _setting;
    #region Properties

    private Double _MasterSoundVolume;

    public Double MasterSoundVolume
      {
      get { return _MasterSoundVolume; }
      set
        {
        _MasterSoundVolume = value;
        //SetLimitVolumes();
        NotifyOfPropertyChange(nameof(MasterSoundVolume));
        }
      }

    private Double _AmbienceSoundVolume;

    public Double AmbienceSoundVolume
      {
      get { return _AmbienceSoundVolume; }
      set
        {
        _AmbienceSoundVolume = value;
        //SetLimitVolumes();
        NotifyOfPropertyChange(nameof(AmbienceSoundVolume));
        }
      }

    private Double _DialogSoundVolume;

    public Double DialogSoundVolume
      {
      get { return _DialogSoundVolume; }
      set
        {
        _DialogSoundVolume = value;
        //SetLimitVolumes();
        NotifyOfPropertyChange(nameof(DialogSoundVolume));
        }
      }

    private Double _ExternalAlertVolume;

    public Double ExternalAlertVolume
      {
      get { return _ExternalAlertVolume; }
      set
        {
        _ExternalAlertVolume = value;
        //SetLimitVolumes();
        NotifyOfPropertyChange(nameof(ExternalAlertVolume));
        }
      }

    private Double _SFXSoundVolume;

    public Double SFXSoundVolume
      {
      get { return _SFXSoundVolume; }
      set
        {
        _SFXSoundVolume = value;
        //SetLimitVolumes();
        NotifyOfPropertyChange(nameof(SFXSoundVolume));
        }
      }

    private Double _MenuSFXSoundVolume;

    public Double MenuSFXSoundVolume
      {
      get { return _MenuSFXSoundVolume; }
      set
        {
        _MenuSFXSoundVolume = value;
        //SetLimitVolumes();
        NotifyOfPropertyChange(nameof(MenuSFXSoundVolume));
        }
      }


    private Boolean _Subtitles;

    public Boolean Subtitles
      {
      get { return _Subtitles; }
      set
        {
        _Subtitles = value;
        NotifyOfPropertyChange(nameof(Subtitles));
        }
      }

    private Boolean _WindowFocus;

    public Boolean WindowFocus
      {
      get { return _WindowFocus; }
      set
        {
        _WindowFocus = value;
        NotifyOfPropertyChange(nameof(WindowFocus));
        }
      }

    private Boolean _LimitVolume = TSWOptions.LimitSoundVolumes;

    public Boolean LimitVolume
      {
      get { return _LimitVolume; }
      set
        {
        _LimitVolume = value;
        SetLimitVolumes();
        NotifyOfPropertyChange(nameof(MasterSoundVolume));
        NotifyOfPropertyChange(nameof(AmbienceSoundVolume));
        NotifyOfPropertyChange(nameof(DialogSoundVolume));
        NotifyOfPropertyChange(nameof(ExternalAlertVolume));
        NotifyOfPropertyChange(nameof(SFXSoundVolume));
        NotifyOfPropertyChange(nameof(MenuSFXSoundVolume));
        }
      }

    #endregion

    public SettingsSoundViewModel(ISetting setting)
      {
      _setting = setting;
      DisplayName = "Sound";
      }
    public void Init()
      {
      GetAllSounds();
      WindowFocus = _setting.GetBooleanValue("WindowFocus", true);
      Subtitles = _setting.GetBooleanValue("Subtitles", true);
      }

    public void Update()
      {
      WriteAllSounds();
      _setting.WriteBooleanValue(WindowFocus, "WindowFocus", SectionEnum.User);
      _setting.WriteBooleanValue(Subtitles, "Subtitles", SectionEnum.User);
      }

    private Double GetSoundVolume(String Key)
      {
      return _setting.GetDoubleValue(Key, 0.0);
      }

    private void GetAllSounds()
      {
      MasterSoundVolume = GetSoundVolume("MasterSoundVolume");
      AmbienceSoundVolume = GetSoundVolume("AmbienceSoundVolume");
      DialogSoundVolume = GetSoundVolume("DialogueSoundVolume");
      ExternalAlertVolume = GetSoundVolume("ExternalAlertVolume");
      SFXSoundVolume = GetSoundVolume("SFXSoundVolume");
      MenuSFXSoundVolume = GetSoundVolume("MenuSFXVolume");
      }

    private void WriteAllSounds()
      {
      if (LimitVolume)
        {
        SetLimitVolumes();
        }
      CultureInfo Gb = CultureInfo.CreateSpecificCulture("en-GB");
      _setting.WriteStringValue(MasterSoundVolume.ToString("0.0000", Gb), "MasterSoundVolume", SectionEnum.User);
      _setting.WriteStringValue(AmbienceSoundVolume.ToString("0.0000", Gb), "AmbienceSoundVolume", SectionEnum.User);
      _setting.WriteStringValue(DialogSoundVolume.ToString("0.0000", Gb), "DialogueSoundVolume", SectionEnum.User);
      _setting.WriteStringValue(ExternalAlertVolume.ToString("0.0000", Gb), "ExternalAlertVolume", SectionEnum.User);
      _setting.WriteStringValue(SFXSoundVolume.ToString("0.0000", Gb), "SFXSoundVolume", SectionEnum.User);
      _setting.WriteStringValue(MenuSFXSoundVolume.ToString("0.0000", Gb), "MenuSFXVolume", SectionEnum.User);
      }

    public void SetRecommendedValues()
      {
      if (LimitVolume)
        {
        SetRecommendedLimitValues();
        }
      else
        {
        MasterSoundVolume = 3.5;
        AmbienceSoundVolume = 2.5;
        ExternalAlertVolume = 1.5;
        DialogSoundVolume = 1.5;
        SFXSoundVolume = 3.5;
        MenuSFXSoundVolume = 1.0;
        }
      }

    public void SetRecommendedLimitValues()
      {
      MasterSoundVolume = 1.0;
      AmbienceSoundVolume = 1.0;
      ExternalAlertVolume = 10;
      DialogSoundVolume = 1.0;
      SFXSoundVolume = 1.0;
      MenuSFXSoundVolume = 0.5;
      }

    public void SetLimitVolumes()
      {
      if (LimitVolume)
        {
        MasterSoundVolume = Math.Min(MasterSoundVolume, 1.0);
        AmbienceSoundVolume = Math.Min(AmbienceSoundVolume, 1.0);
        ExternalAlertVolume = Math.Min(ExternalAlertVolume, 1.0);
        DialogSoundVolume = Math.Min(DialogSoundVolume, 1.0);
        SFXSoundVolume = Math.Min(SFXSoundVolume, 1.0);
        MenuSFXSoundVolume = Math.Min(MenuSFXSoundVolume, 1.0);
        }
      }
    }
  }
