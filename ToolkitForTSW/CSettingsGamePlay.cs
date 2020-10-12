using Styles.Library.Helpers;
using System;

namespace ToolkitForTSW
{
	public class CSettingsGamePlay : CSetting
		{
		#region Properties



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
      QuickWalk = GetBooleanValue("QuickWalk", false);
      CabSway = GetBooleanValue("bCameraMotionSwayEnabled", true);
      ForceFeedback = GetBooleanValue("ForceFeedback", false);
      DisableJunctionDerail = GetBooleanValue("DisableJunctionDerail", false);
			GetUnits();
			GetGradeUnits();
			GetTemperatureUnits();
			}

		public void Update()
			{
			WriteBooleanValue(CabSway, "bCameraMotionSwayEnabled",SectionEnum.User);
			WriteBooleanValue(QuickWalk, "QuickWalk",SectionEnum.User);
			WriteBooleanValue(ForceFeedback, "ForceFeedback",SectionEnum.User);
			WriteBooleanValue(DisableJunctionDerail, "DisableJunctionDerail",SectionEnum.User);
      WriteGradeSettings();
      WriteMeasurement();
      WriteTemperature();
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

		private void GetUnits()
			{
      SettingsManager.GetSetting("MeasurementSetting", out var Temp);
      if (string.CompareOrdinal(Temp, "Automatic")==0)
        {
        Units = UnitsEnum.Automatic;
        return;
        }
			SettingsManager.GetSetting("Measurement", out Temp);
			Units = UnitsEnum.Metric;
			if (String.CompareOrdinal(Temp, "Imperial") == 0)
				{
				Units = UnitsEnum.Imperial;
				}
			}

		private void GetGradeUnits()
			{
      SettingsManager.GetSetting("GradeSetting", out var Temp);
      if (string.CompareOrdinal(Temp, "Automatic") == 0)
        {
        GradeUnits = GradeUnitsEnum.Automatic;
        return;
        }

			SettingsManager.GetSetting("GradeUnit", out Temp);
			GradeUnits = GradeUnitsEnum.Percentage;
			if (String.CompareOrdinal(Temp, "Ratio") == 0)
				{
				GradeUnits = GradeUnitsEnum.Ratio;
				}
			}

		private void GetTemperatureUnits()
			{
      SettingsManager.GetSetting("TemperatureSetting", out var Temp);
      if (string.CompareOrdinal(Temp, "Automatic")==0)
        {
        TemperatureUnits = TemperatureEnum.Automatic;
        return;
        }
			SettingsManager.GetSetting("TemperatureUnit", out Temp);
			TemperatureUnits = TemperatureEnum.Celsius;
			if (String.CompareOrdinal(Temp, "Fahrenheit") == 0)
				{
				TemperatureUnits = TemperatureEnum.Fahrenheit;
				}
			}
		}
	}
