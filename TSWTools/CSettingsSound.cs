using Styles.Library.Helpers;
using System;
using System.Globalization;



namespace TSWTools
{
public class CSettingsSound : Notifier
	{

	#region Properties

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

	private Double _MusicSoundVolume;

	public Double MusicSoundVolume
		{
		get { return _MusicSoundVolume; }
		set
			{
			_MusicSoundVolume = value;
			OnPropertyChanged("MusicSoundVolume");
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

	private Double _MenuSoundVolume;

	public Double MenuSoundVolume
		{
		get { return _MenuSoundVolume; }
		set
			{
			_MenuSoundVolume = value;
			OnPropertyChanged("MenuSoundVolume");
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

	private Boolean _LimitVolume;

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


	public CSettingsSound(CSettingsManager MySettingsManager)
		{
		SettingsManager = MySettingsManager;
		}

	public void Init()
		{
		GetAllSounds();
		GetWindowFocus();
		GetSubtitles();
		LimitVolume = false;
		}

	public void Update()
		{
		WriteAllSounds();
		WriteSubtitles();
		WriteWindowFocus();
		}

	private Double GetSoundVolume(String Key)
		{
		var Style = NumberStyles.AllowDecimalPoint | NumberStyles.Number;
		var Culture = CultureInfo.CreateSpecificCulture("en-GB");
		SettingsManager.GetSetting(Key,   out var Temp);
		Double.TryParse(Temp, Style, Culture, out var Temp2);
		return Temp2;
		}

	private Boolean StringToBoolean(String Value)
		{
		return String.Equals(Value, "true", StringComparison.OrdinalIgnoreCase);
		}

		private void GetAllSounds()
		{


		MasterSoundVolume = GetSoundVolume("MasterSoundVolume");
		AmbienceSoundVolume = GetSoundVolume("AmbienceSoundVolume");
		DialogSoundVolume = GetSoundVolume("DialogueSoundVolume");
		MusicSoundVolume = GetSoundVolume("MusicSoundVolume");
		ExternalAlertVolume = GetSoundVolume("ExternalAlertVolume");
		SFXSoundVolume = GetSoundVolume("SFXSoundVolume");
		MenuSFXSoundVolume = GetSoundVolume("MenuSFXVolume");
		MenuSoundVolume = GetSoundVolume("MenuSoundVolume");
		}

	private void WriteAllSounds()
		{
		if (LimitVolume)
			{
			LimitVolumes();
			}
		CultureInfo Gb = CultureInfo.CreateSpecificCulture("en-GB");
		SettingsManager.UpdateSetting("MasterSoundVolume", MasterSoundVolume.ToString("0.0000",Gb), SectionEnum.User);
		SettingsManager.UpdateSetting("AmbienceSoundVolume",AmbienceSoundVolume.ToString("0.0000", Gb), SectionEnum.User);
		SettingsManager.UpdateSetting("DialogueSoundVolume",DialogSoundVolume.ToString("0.0000", Gb), SectionEnum.User);
		SettingsManager.UpdateSetting("MusicSoundVolume",MusicSoundVolume.ToString("0.0000", Gb), SectionEnum.User);
		SettingsManager.UpdateSetting("ExternalAlertVolume", ExternalAlertVolume.ToString("0.0000", Gb), SectionEnum.User);
		SettingsManager.UpdateSetting("SFXSoundVolume", SFXSoundVolume.ToString("0.0000", Gb), SectionEnum.User);
		SettingsManager.UpdateSetting("MenuSFXVolume", MenuSFXSoundVolume.ToString("0.0000", Gb), SectionEnum.User);
		SettingsManager.UpdateSetting("MenuSoundVolume", MenuSoundVolume.ToString("0.0000", Gb), SectionEnum.User);
		}


		private void GetWindowFocus()
		{
		SettingsManager.GetSetting("WindowFocus", out var Temp);
		WindowFocus = StringToBoolean(Temp);
		}

	private void WriteWindowFocus()
		{
		if (WindowFocus)
			{
			SettingsManager.UpdateSetting("WindowFocus", "True", SectionEnum.User);
			}
		else
			{
			SettingsManager.UpdateSetting("WindowFocus", "False", SectionEnum.User);
			}
		}



		private void GetSubtitles()
		{
		SettingsManager.GetSetting("Subtitles", out var Temp);
		Subtitles = StringToBoolean(Temp);
		}

	private void WriteSubtitles()
		{
		if (Subtitles)
			{
			SettingsManager.UpdateSetting("Subtitles", "True", SectionEnum.User);
			}
		else
			{
			SettingsManager.UpdateSetting("Subtitles", "False", SectionEnum.User);
			}
		}

	public void SetRecommendedValues()
		{
		MasterSoundVolume = 3.5;
		AmbienceSoundVolume = 2.5;
		ExternalAlertVolume = 1.5;
		DialogSoundVolume = 1.5;
		SFXSoundVolume = 3.5;
		MusicSoundVolume = 1.5;
		MenuSFXSoundVolume = 1.0;
		MenuSoundVolume = 2.5;
		}

	private void LimitVolumes()
		{
		MasterSoundVolume = Math.Min(MasterSoundVolume,1.0);
		AmbienceSoundVolume = Math.Min(AmbienceSoundVolume,1.0);
		ExternalAlertVolume = Math.Min(ExternalAlertVolume,1.0);
		DialogSoundVolume = Math.Min(DialogSoundVolume,1.0);
		SFXSoundVolume = Math.Min(SFXSoundVolume,1.0);
		MusicSoundVolume = Math.Min(MusicSoundVolume,1.0);
		MenuSFXSoundVolume =Math.Min(MenuSFXSoundVolume,1.0);
		MenuSoundVolume = Math.Min(MenuSoundVolume,1.0);
		}

	

	}
}
