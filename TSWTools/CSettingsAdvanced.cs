using Styles.Library.Helpers;
using System;
using System.Globalization;


namespace TSWTools
	{
	public class CSettingsAdvanced : Notifier

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

    public CSettingsAdvanced(CSettingsManager MySettingsManager)
			{
			SettingsManager = MySettingsManager;
			}

		public void Init()
			{
			GetEyeAdaptation();
			GetMaterialQuality();
			var Culture = CultureInfo.CreateSpecificCulture("en-GB");
			SettingsManager.GetSetting("ScreenPercentage", out var Temp);
			if (Temp.Length > 0)
				{
				ScreenPercentage = (UInt32) Math.Round(Convert.ToDouble(Temp, Culture));
				}
			else
				{
				ScreenPercentage = 100;
				}

      SettingsManager.GetSetting("r.Color.mid", out Temp);
      if (Temp.Length > 0)
        {
        GammaCorrection = Math.Round(Convert.ToDouble(Temp, Culture),2);
        }
      else
        {
        GammaCorrection = 0.5;
        }


      SettingsManager.GetSetting("sg.ViewDistanceQuality",
				out Temp); // Read from the settable string
			if (Temp.Length > 0)
				{
				ViewDistanceScale = Convert.ToUInt32(Temp);
				}
			else
				{
				ViewDistanceScale = 1;
				}

			SettingsManager.GetSetting("r.MotionBlurQuality", out Temp);
			if (Temp.Length > 0)
				{
				MotionBlurQuality = Convert.ToUInt32(Temp, Culture);
				}
			else
				{
				MotionBlurQuality = 0;
				}

			SettingsManager.GetSetting("foliage.LODDistanceScale", out Temp);
			if (Temp.Length > 0)
				{
				LODDistanceScale = Convert.ToUInt32(Temp, Culture);
				}
			else
				{
				LODDistanceScale = 3;
				}
			}

		public void Update()
			{
      CultureInfo Gb = CultureInfo.CreateSpecificCulture("en-GB");
      WriteEyeAdaptation();
			WriteMaterialQuality();
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

		private Boolean IntStringToBoolean(String Value)
			{
			return String.Equals(Value, "1", StringComparison.OrdinalIgnoreCase);
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

		private void GetEyeAdaptation()
			{
			SettingsManager.GetSetting("r.EyeAdaptationQuality", out var EyeAdaptationValue);
			EyeAdaptationQuality = IntStringToBoolean(EyeAdaptationValue);
			}

		private void WriteEyeAdaptation()
			{
			if (EyeAdaptationQuality)
				{
				SettingsManager.UpdateSetting("r.EyeAdaptationQuality", "1", SectionEnum.System);
				}
			else
				{
				SettingsManager.UpdateSetting("r.EyeAdaptationQuality", "0", SectionEnum.System);
				}
			}

		private void GetMaterialQuality()
			{
			SettingsManager.GetSetting("r.MaterialQualityLevel", out var MaterialQualityValue);
			MaterialQualityLevel = IntStringToBoolean(MaterialQualityValue);
			}

		private void WriteMaterialQuality()
			{
			if (MaterialQualityLevel)
				{
				SettingsManager.UpdateSetting("r.MaterialQualityLevel", "1", SectionEnum.System);
				}
			else
				{
				SettingsManager.UpdateSetting("r.MaterialQualityLevel", "0", SectionEnum.System);
				}
			}
		}
	}
