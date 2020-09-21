using Styles.Library.Helpers;
using System;



namespace TSWTools
{
	public class CSettingsHUD : Notifier
		{
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

		private Boolean _ObjectiveMarker;

		public Boolean ObjectiveMarker
			{
			get { return _ObjectiveMarker; }
			set
				{
				_ObjectiveMarker = value;
				OnPropertyChanged("ObjectiveMarker");
				}
			}

		private HudStyleEnum _NextSignalMarker;

		public HudStyleEnum NextSignalMarker
			{
			get { return _NextSignalMarker; }
			set
				{
				_NextSignalMarker = value;
				OnPropertyChanged("NextSignalMarker");
				}
			}

		private HudStyleEnum _NextSpeedLimitMarker;

		public HudStyleEnum NextSpeedLimitMarker
			{
			get { return _NextSpeedLimitMarker; }
			set
				{
				_NextSpeedLimitMarker = value;
				OnPropertyChanged("NextSpeedLimitMarker");
				}
			}

		private Boolean _NextSignalAspect;

		public Boolean NextSignalAspect
			{
			get { return _NextSignalAspect; }
			set
				{
				_NextSignalAspect = value;
				OnPropertyChanged("NextSignalAspect");
				}
			}

		private Boolean _ScenarioMarker;

		public Boolean ScenarioMarker
			{
			get { return _ScenarioMarker; }
			set
				{
				_ScenarioMarker = value;
				OnPropertyChanged("ScenarioMarker");
				}
			}

		private Boolean _Score;

		public Boolean Score
			{
			get { return _Score; }
			set
				{
				_Score = value;
				OnPropertyChanged("Score");
				}
			}

		private Boolean _Compass;

		public Boolean Compass
			{
			get { return _Compass; }
			set
				{
				_Compass = value;
				OnPropertyChanged("Compass");
				}
			}

		private Boolean _StopMarker = true;
		public Boolean StopMarker
			{
			get { return _StopMarker; }
			set
				{
				_StopMarker = value;
				OnPropertyChanged("StopMarker");
				}
			}


		public CSettingsHUD(CSettingsManager MySettingsManager)
			{
			SettingsManager = MySettingsManager;
			}

		public void Init()
			{
			GetObjectiveMarker();
			GetNextSignalMarker();
			GetNextSignalAspect();
			GetNextSpeedLimitMarker();
			GetCompass();
			GetScenarioMarker();
			GetScore();
			GetStopMarker();
			}

		public void Update()
			{
			WriteObjectiveMarker();
			WriteNextSignalMarker();
			WriteNextSignalAspect();
			WriteNextSpeedLimitMarker();
			WriteCompass();
			WriteStopMarker();
			WriteScenarioMarker();
			WriteScore();
			}

		private Boolean StringToBoolean(String Value)
			{
			return String.Equals(Value, "true", StringComparison.OrdinalIgnoreCase);
			}

		private void GetObjectiveMarker()
			{
			SettingsManager.GetSetting("ShowObjectiveMarker", out var ShowObjectiveMarker);
			SettingsManager.GetSetting("ShowObjectiveStopAtMarker", out var ShowObjectiveStopAtMarker);
			var ObjectiveMarkerBool = StringToBoolean(ShowObjectiveMarker);
			var ObjectiveMarkerStopAt = StringToBoolean(ShowObjectiveStopAtMarker);
			ObjectiveMarker = true;
			}

		public void WriteObjectiveMarker()
			{
	    if (ObjectiveMarker)
        {
        SettingsManager.UpdateSetting("ShowObjectiveMarker", "True", SectionEnum.User);
        }
      else
        {
        SettingsManager.UpdateSetting("ShowObjectiveMarker", "False", SectionEnum.User);
        }
      }

		private void GetNextSignalMarker()
			{
			SettingsManager.GetSetting("ShowNextSignalMarker", out var ShowNextSignalMarker);
			SettingsManager.GetSetting("ShowNextSignalScreen", out var ShowNextSignalScreen);
			var NextSignalMarkerBool = StringToBoolean(ShowNextSignalMarker);
			var ShowNextSignalScreenBool = StringToBoolean(ShowNextSignalScreen);
			NextSignalMarker = HudStyleEnum.None;
			if (NextSignalMarkerBool && ShowNextSignalScreenBool)
				{
				NextSignalMarker = HudStyleEnum.Both;
				return;
				}

			if (!NextSignalMarkerBool && ShowNextSignalScreenBool)
				{
				NextSignalMarker = HudStyleEnum.HUD;
				return;
				}

			NextSignalMarker = HudStyleEnum.Marker;
			}

		public void WriteNextSignalMarker()
			{
			if (NextSignalMarker == HudStyleEnum.Marker || NextSignalMarker == HudStyleEnum.Both)
				{
				SettingsManager.UpdateSetting("ShowNextSignalMarker", "True", SectionEnum.User);
				}
			else
				{
				SettingsManager.UpdateSetting("ShowNextSignalMarker", "False", SectionEnum.User);
				}

			if (NextSignalMarker == HudStyleEnum.HUD || NextSignalMarker == HudStyleEnum.Both)
				{
				SettingsManager.UpdateSetting("ShowNextSignalScreen", "True", SectionEnum.User);
				}
			else
				{
				SettingsManager.UpdateSetting("ShowNextSignalScreen", "False", SectionEnum.User);
				}
			}

		private void GetNextSignalAspect()
			{
			SettingsManager.GetSetting("ShowNextSignalAspectMarker", out var ShowNextSignalAspectMarker);
			NextSignalAspect = StringToBoolean(ShowNextSignalAspectMarker);
			}

		private void WriteNextSignalAspect()
			{
			if (NextSignalAspect)
				{
				SettingsManager.UpdateSetting("ShowNextSignalAspectMarker", "True", SectionEnum.User);
				}
			else
				{
				SettingsManager.UpdateSetting("ShowNextSignalAspectMarker", "False", SectionEnum.User);
				}
			}

		private void GetNextSpeedLimitMarker()
			{
			SettingsManager.GetSetting("ShowSpeedLimitMarker", out var ShowSpeedLimitMarker);
			SettingsManager.GetSetting("ShowSpeedLimitScreen", out var ShowSpeedLimitScreen);
			var ShowSpeedLimitMarkerBool = StringToBoolean(ShowSpeedLimitMarker);
			var ShowSpeedLimitScreenBool = StringToBoolean(ShowSpeedLimitScreen);
			NextSpeedLimitMarker = HudStyleEnum.None;
			if (ShowSpeedLimitMarkerBool && ShowSpeedLimitScreenBool)
				{
				NextSpeedLimitMarker = HudStyleEnum.Both;
				return;
				}

			if (!ShowSpeedLimitMarkerBool && ShowSpeedLimitScreenBool)
				{
				NextSpeedLimitMarker = HudStyleEnum.HUD;
				return;
				}

			NextSpeedLimitMarker = HudStyleEnum.Marker;
			}

		public void WriteNextSpeedLimitMarker()
			{
			if (NextSpeedLimitMarker == HudStyleEnum.Marker || NextSpeedLimitMarker == HudStyleEnum.Both)
				{
				SettingsManager.UpdateSetting("ShowSpeedLimitMarker", "True", SectionEnum.User);
				}
			else
				{
				SettingsManager.UpdateSetting("ShowSpeedLimitMarker", "False", SectionEnum.User);
				}

			if (NextSpeedLimitMarker == HudStyleEnum.HUD || NextSpeedLimitMarker == HudStyleEnum.Both)
				{
				SettingsManager.UpdateSetting("ShowSpeedLimitScreen", "True", SectionEnum.User);
				}
			else
				{
				SettingsManager.UpdateSetting("ShowSpeedLimitScreen", "False", SectionEnum.User);
				}
			}

		private void GetScenarioMarker()
			{
			SettingsManager.GetSetting("ShowScenarioMarker", out var ShowScenarioMarker);
			ScenarioMarker = StringToBoolean(ShowScenarioMarker);
			}

		private void WriteScenarioMarker()
			{
			if (ScenarioMarker)
				{
				SettingsManager.UpdateSetting("ShowScenarioMarker", "True", SectionEnum.User);
				}
			else
				{
				SettingsManager.UpdateSetting("ShowScenarioMarker", "False", SectionEnum.User);
				}
			}

		private void GetScore()
			{
			SettingsManager.GetSetting("ShowScore", out var ShowScore);
			Score = StringToBoolean(ShowScore);
			}

		private void WriteScore()
			{
			if (Score)
				{
				SettingsManager.UpdateSetting("ShowScore", "True", SectionEnum.User);
				}
			else
				{
				SettingsManager.UpdateSetting("ShowScore", "False", SectionEnum.User);
				}
			}

		private void GetCompass()
			{
			SettingsManager.GetSetting("ShowCompass", out var ShowCompass);
			Compass = StringToBoolean(ShowCompass);
			}

		private void WriteCompass()
			{
			if (Compass)
				{
				SettingsManager.UpdateSetting("ShowCompass", "True", SectionEnum.User);
				}
			else
				{
				SettingsManager.UpdateSetting("ShowCompass", "False", SectionEnum.User);
				}
			}

		private void GetStopMarker()
			{
			SettingsManager.GetSetting("ShowObjectiveStopAtMarker", out var ShowStopMarker);
			StopMarker = StringToBoolean(ShowStopMarker);
			}

		private void WriteStopMarker()
			{
			if (StopMarker)
				{
				SettingsManager.UpdateSetting("ShowObjectiveStopAtMarker", "True", SectionEnum.User);
				}
			else
				{
				SettingsManager.UpdateSetting("ShowObjectiveStopAtMarker", "False", SectionEnum.User);
				}
			}


	}
}
