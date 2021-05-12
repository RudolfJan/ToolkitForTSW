using System;
using System.Globalization;



namespace ToolkitForTSW.Settings
  {
  public class CSettingsSound : CSetting
    {

    #region Properties

    private Double _MasterSoundVolume;

    public Double MasterSoundVolume
      {
      get { return _MasterSoundVolume; }
      set
        {
        _MasterSoundVolume = value;
        OnPropertyChanged("MasterSoundVolume");
        }
      }

    private Double _AmbienceSoundVolume;

    public Double AmbienceSoundVolume
      {
      get { return _AmbienceSoundVolume; }
      set
        {
        _AmbienceSoundVolume = value;
        OnPropertyChanged("AmbienceSoundVolume");
        }
      }

    private Double _DialogSoundVolume;

    public Double DialogSoundVolume
      {
      get { return _DialogSoundVolume; }
      set
        {
        _DialogSoundVolume = value;
        OnPropertyChanged("DialogSoundVolume");
        }
      }

    private Double _ExternalAlertVolume;

    public Double ExternalAlertVolume
      {
      get { return _ExternalAlertVolume; }
      set
        {
        _ExternalAlertVolume = value;
        OnPropertyChanged("ExternalAlertVolume");
        }
      }

    private Double _SFXSoundVolume;

    public Double SFXSoundVolume
      {
      get { return _SFXSoundVolume; }
      set
        {
        _SFXSoundVolume = value;
        OnPropertyChanged("SFXSoundVolume");
        }
      }

    private Double _MenuSFXSoundVolume;

    public Double MenuSFXSoundVolume
      {
      get { return _MenuSFXSoundVolume; }
      set
        {
        _MenuSFXSoundVolume = value;
        OnPropertyChanged("MenuSFXSoundVolume");
        }
      }


    private Boolean _Subtitles;

    public Boolean Subtitles
      {
      get { return _Subtitles; }
      set
        {
        _Subtitles = value;
        OnPropertyChanged("Subtitles");
        }
      }

    private Boolean _WindowFocus;

    public Boolean WindowFocus
      {
      get { return _WindowFocus; }
      set
        {
        _WindowFocus = value;
        OnPropertyChanged("WindowFocus");
        }
      }

    private Boolean _LimitVolume= CTSWOptions.LimitSoundVolumes;

    public Boolean LimitVolume
      {
      get { return _LimitVolume; }
      set
        {
        _LimitVolume = value;
        OnPropertyChanged("LimitVolume");
        }
      }

    #endregion

    public CSettingsSound(CSettingsManager _settingsManager)
      {
      SettingsManager = _settingsManager;
      }
    public void Init()
      {
      GetAllSounds();
      WindowFocus = GetBooleanValue("WindowFocus", true);
      Subtitles = GetBooleanValue("Subtitles", true);
      }

    public void Update()
      {
      WriteAllSounds();
      WriteBooleanValue(WindowFocus, "WindowFocus", SectionEnum.User);
      WriteBooleanValue(Subtitles, "Subtitles", SectionEnum.User);
      }

    private Double GetSoundVolume(String Key)
      {
      var Style = NumberStyles.AllowDecimalPoint | NumberStyles.Number;
      var Culture = CultureInfo.CreateSpecificCulture("en-GB");
      SettingsManager.GetSetting(Key, out var Temp);
      Double.TryParse(Temp, Style, Culture, out var Temp2);
      return Temp2;
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
        LimitVolumes();
        }
      CultureInfo Gb = CultureInfo.CreateSpecificCulture("en-GB");
      SettingsManager.UpdateSetting("MasterSoundVolume", MasterSoundVolume.ToString("0.0000", Gb), SectionEnum.User);
      SettingsManager.UpdateSetting("AmbienceSoundVolume", AmbienceSoundVolume.ToString("0.0000", Gb), SectionEnum.User);
      SettingsManager.UpdateSetting("DialogueSoundVolume", DialogSoundVolume.ToString("0.0000", Gb), SectionEnum.User);
      SettingsManager.UpdateSetting("ExternalAlertVolume", ExternalAlertVolume.ToString("0.0000", Gb), SectionEnum.User);
      SettingsManager.UpdateSetting("SFXSoundVolume", SFXSoundVolume.ToString("0.0000", Gb), SectionEnum.User);
      SettingsManager.UpdateSetting("MenuSFXVolume", MenuSFXSoundVolume.ToString("0.0000", Gb), SectionEnum.User);
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

    public void LimitVolumes()
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
