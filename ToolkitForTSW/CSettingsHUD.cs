using Styles.Library.Helpers;
using System;



namespace ToolkitForTSW
{
	public class CSettingsHUD : CSetting
		{

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

    private Boolean _StopAtMarker;

    public Boolean StopAtMarker
			{
      get { return _StopAtMarker; }
      set
        {
				_StopAtMarker = value;
        OnPropertyChanged("StopAtMarker");
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
      ObjectiveMarker = GetBooleanValue("ShowObjectiveMarker", true);
      StopAtMarker = GetBooleanValue("ShowObjectiveStopAtMarker", true);
			GetNextSignalMarker();
      NextSignalAspect = GetBooleanValue("ShowNextSignalAspectMarker", true);
			GetNextSpeedLimitMarker();
      Compass = GetBooleanValue("ShowCompass", true);
			Score = GetBooleanValue("ShowScore", true);
      StopMarker = GetBooleanValue("ShowObjectiveStopAtMarker", true);
      ScenarioMarker = GetBooleanValue("ShowScenarioMarker", true);
      }

		public void Update()
			{
			WriteBooleanValue(ObjectiveMarker, "ShowObjectiveMarker",SectionEnum.User);
			WriteBooleanValue(StopAtMarker, "ShowObjectiveStopAtMarker",SectionEnum.User);
			WriteBooleanValue(NextSignalAspect, "ShowNextSignalAspectMarker",SectionEnum.User);
      WriteNextSignalMarker();
			WriteNextSpeedLimitMarker();
			WriteBooleanValue(Compass, "ShowCompass",SectionEnum.User);
			WriteBooleanValue(StopMarker, "ShowObjectiveStopAtMarker",SectionEnum.User);
			WriteBooleanValue(ScenarioMarker, "ShowScenarioMarker",SectionEnum.User);
			WriteBooleanValue(Score, "ShowScore",SectionEnum.User);
			}

		private void GetNextSignalMarker()
			{
      var NextSignalMarkerBool = GetBooleanValue("ShowNextSignalMarker", true);
      var ShowNextSignalScreenBool = GetBooleanValue("ShowNextSignalScreen", true);
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

	private void GetNextSpeedLimitMarker()
			{
      var ShowSpeedLimitMarkerBool = GetBooleanValue("ShowSpeedLimitMarker",true);
      var ShowSpeedLimitScreenBool = GetBooleanValue("ShowSpeedLimitScreen", true);
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
	}
}
