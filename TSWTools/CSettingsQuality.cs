using Styles.Library.Helpers;
using System;
using System.Globalization;



namespace TSWTools
{
	public class CSettingsQuality: Notifier
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

	private QualityEnum _ShadowQuality;
	public QualityEnum ShadowQuality
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

	private QualityEnum _EffectsQuality;
	public QualityEnum EffectsQuality
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

		public CSettingsQuality(CSettingsManager MySettingsManager)
		{
		SettingsManager = MySettingsManager;
		}

	public void Init()
		{
		OverAllQualityLevel = GetQuality("OverallQualityLevel");
		TextureQuality = GetQuality("TextureQuality");
		ShadowQuality	= GetQuality("ShadowQuality");
		EffectsQuality	= GetQuality("EffectsQuality");
		PostProcessQuality	= GetQuality("PostProcessQuality");
		AudioQualityLevel	= GetQuality("AudioQualityLevel");
		ScreenShotQuality = GetBooleanValue("ScreenShotQuality");
		MaxFrameRate = GetIntValue("MaxFPS");
		AntiAliasingMethod = GetAntiAliasingMethod("AAMethod");

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
		WriteScreenShotQuality();
		SettingsManager.UpdateSetting("MaxFPS", MaxFrameRate.ToString("0.0000", Gb), SectionEnum.User);
		SettingsManager.UpdateSetting("AAMethod", ((Int32)AntiAliasingMethod).ToString(), SectionEnum.User);

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

	private Boolean GetBooleanValue(String Key)
		{
		SettingsManager.GetSetting(Key, out var Temp);
		if (Temp.Length == 0 || (String.CompareOrdinal(Temp,"0")==0))
			{
			return false;
			}
		return true;
		}

	private Int32 GetIntValue(String Key)
		{
		var Style = NumberStyles.AllowDecimalPoint | NumberStyles.Number;
		var Culture = CultureInfo.CreateSpecificCulture("en-GB");
		SettingsManager.GetSetting(Key, out var Temp);
		Double.TryParse(Temp, Style, Culture, out var Temp2);
		return (Int32)Temp2;
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


		private void WriteScreenShotQuality()
		{
		if (ScreenShotQuality)
			{
			SettingsManager.UpdateSetting("ScreenShotQuality", "True", SectionEnum.User);
			}
		else
			{
			SettingsManager.UpdateSetting("ScreenShotQuality", "False", SectionEnum.User);
			}
		}



	}
}
