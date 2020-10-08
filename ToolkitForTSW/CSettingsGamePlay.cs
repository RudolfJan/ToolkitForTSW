using Styles.Library.Helpers;
using System;
using System.Windows.Input;

namespace TSWTools
{
	public class CSettingsGamePlay : Notifier
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

		private Boolean _QuickWalk;

		public Boolean QuickWalk
			{
			get { return _QuickWalk; }
			set
				{
				_QuickWalk = value;
				OnPropertyChanged("QuickWalk");
				}
			}

		private Boolean _DisableJunctionDerail;

		public Boolean DisableJunctionDerail
			{
			get { return _DisableJunctionDerail; }
			set
				{
				_DisableJunctionDerail = value;
				OnPropertyChanged("DisableJunctionDerail");
				}
			}

		private Boolean _CabSway;
		public Boolean CabSway
			{
			get { return _CabSway; }
			set { _CabSway= value;
				OnPropertyChanged("CabSway");}
			}

		private GradeUnitsEnum _GradeUnits;

		public GradeUnitsEnum GradeUnits
			{
			get { return _GradeUnits; }
			set
				{
				_GradeUnits = value;
				OnPropertyChanged("GradeUnits");
				}
			}

		private UnitsEnum _Units;

		public UnitsEnum Units
			{
			get { return _Units; }
			set
				{
				_Units = value;
				OnPropertyChanged("Units");
				}
			}

		private TemperatureEnum _TemperatureUnits;

		public TemperatureEnum TemperatureUnits
			{
			get { return _TemperatureUnits; }
			set
				{
				_TemperatureUnits = value;
				OnPropertyChanged("TemperatureUnits");
				}
			}

		private Boolean _ForceFeedback;

		public Boolean ForceFeedback
			{
			get { return _ForceFeedback; }
			set
				{
				_ForceFeedback = value;
				OnPropertyChanged("ForceFeedback");
				}
			}

		#endregion

		public CSettingsGamePlay(CSettingsManager MySettingsManager)
			{
			SettingsManager = MySettingsManager;
			}

		public void Init()
			{
			GetQuickWalk();
			GetCabSway();
			GetForceFeedback();
			GetJunctionDerail();
			GetUnits();
			GetGradeUnits();
			GetTemperatureUnits();
			}

		public void Update()
			{
			WriteCabSway();
			WriteQuickWalk();
			WriteForceFeedback();
			WriteJunctionDerail();
      WriteGradeSettings();
      WriteMeasurement();
      WriteTemperature();
			SettingsManager.UpdateSetting("TemperatureUnit", TemperatureUnits.ToString(), SectionEnum.User);
			}

    private void WriteTemperature()
      {
			switch (TemperatureUnits)
        {
          case TemperatureEnum.Automatic:
            {
            SettingsManager.UpdateSetting("TemperatureSetting", "Automatic", SectionEnum.User);
            break;
            }
          case TemperatureEnum.Fahrenheit:
            {
            SettingsManager.UpdateSetting("TemperatureSetting", "Manual", SectionEnum.User);
            SettingsManager.UpdateSetting("TemperatureUnit", TemperatureUnits.ToString(), SectionEnum.User);
            break;
            }
          case TemperatureEnum.Celsius:
            {
            SettingsManager.UpdateSetting("TemperatureSetting", "Manual", SectionEnum.User);
            SettingsManager.UpdateSetting("TemperatureUnit", TemperatureUnits.ToString(), SectionEnum.User);
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
            SettingsManager.UpdateSetting("MeasurementSetting", "Automatic", SectionEnum.User);
            break;
            }
          case UnitsEnum.Imperial:
            {
            SettingsManager.UpdateSetting("MeasurementSetting", "Manual", SectionEnum.User);
            SettingsManager.UpdateSetting("Measurement", Units.ToString(), SectionEnum.User);
            break;
            }
          case UnitsEnum.Metric:
            {
            SettingsManager.UpdateSetting("MeasurementSetting", "Manual", SectionEnum.User);
            SettingsManager.UpdateSetting("Measurement", Units.ToString(), SectionEnum.User);
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
            SettingsManager.UpdateSetting("GradeSetting", "Automatic", SectionEnum.User);
						break;
            }
          case GradeUnitsEnum.Percentage:
            {
            SettingsManager.UpdateSetting("GradeSetting", "Manual", SectionEnum.User);
						SettingsManager.UpdateSetting("GradeUnit", GradeUnits.ToString(), SectionEnum.User);
						break;
            }
          case GradeUnitsEnum.Ratio:
            {
            SettingsManager.UpdateSetting("GradeSetting", "Manual", SectionEnum.User);
						SettingsManager.UpdateSetting("GradeUnit", GradeUnits.ToString(), SectionEnum.User);
						break;
            }
        }
			}



    private Boolean StringToBoolean(String Value)
			{
			return String.Equals(Value, "true", StringComparison.OrdinalIgnoreCase);
			}

		private void GetCabSway()
			{
			SettingsManager.GetSetting("bCameraMotionSwayEnabled", out var ShowCabSway);
			CabSway = StringToBoolean(ShowCabSway);
			}

		private void GetQuickWalk()
			{
			SettingsManager.GetSetting("QuickWalk", out var ShowQuickWalk);
			QuickWalk = StringToBoolean(ShowQuickWalk);
			}

		private void WriteQuickWalk()
			{
			if (QuickWalk)
				{
				SettingsManager.UpdateSetting("QuickWalk", "True", SectionEnum.User);
				}
			else
				{
				SettingsManager.UpdateSetting("QuickWalk", "False", SectionEnum.User);
				}
			}

		private void WriteCabSway()
			{
			if (CabSway)
				{
				SettingsManager.UpdateSetting("bCameraMotionSwayEnabled", "True", SectionEnum.User);
				}
			else
				{
				SettingsManager.UpdateSetting("bCameraMotionSwayEnabled", "False", SectionEnum.User);
				}
			}

		private void GetJunctionDerail()
			{
			SettingsManager.GetSetting("DisableJunctionDerail", out var Temp);
			DisableJunctionDerail = StringToBoolean(Temp);
			}

		private void WriteJunctionDerail()
			{
			if (DisableJunctionDerail)
				{
				SettingsManager.UpdateSetting("DisableJunctionDerail", "True", SectionEnum.User);
				}
			else
				{
				SettingsManager.UpdateSetting("DisableJunctionDerail", "False", SectionEnum.User);
				}
			}

		private void GetForceFeedback()
			{
			SettingsManager.GetSetting("ForceFeedback", out var Temp);
			ForceFeedback = StringToBoolean(Temp);
			}

		private void WriteForceFeedback()
			{
			if (ForceFeedback)
				{
				SettingsManager.UpdateSetting("ForceFeedback", "True", SectionEnum.User);
				}
			else
				{
				SettingsManager.UpdateSetting("ForceFeedback", "False", SectionEnum.User);
				}
			}

		private void GetUnits()
			{
			SettingsManager.GetSetting("Measurement", out var Temp);
			Units = UnitsEnum.Metric;
			if (String.CompareOrdinal(Temp, "Imperial") == 0)
				{
				Units = UnitsEnum.Imperial;
				}
			}

		private void GetGradeUnits()
			{
			SettingsManager.GetSetting("GradeUnit", out var Temp);
			GradeUnits = GradeUnitsEnum.Percentage;
			if (String.CompareOrdinal(Temp, "Ratio") == 0)
				{
				GradeUnits = GradeUnitsEnum.Ratio;
				}
			}

		private void GetTemperatureUnits()
			{
			SettingsManager.GetSetting("TemperatureUnit", out var Temp);
			TemperatureUnits = TemperatureEnum.Celsius;
			if (String.CompareOrdinal(Temp, "Fahrenheit") == 0)
				{
				TemperatureUnits = TemperatureEnum.Fahrenheit;
				}
			}
		}
	}
