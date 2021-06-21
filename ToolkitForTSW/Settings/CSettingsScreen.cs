using Styles.Library.Helpers;
using System;



namespace ToolkitForTSW.Settings
	{
	public class CSettingsScreen : CSetting
		{
		#region Properties

    private CVideoModes _VideoModes;
    public CVideoModes VideoModes
	    {
	    get { return _VideoModes; }
	    set
		    {
		    _VideoModes = value;
		    OnPropertyChanged("VideoModes");
		    }
	    }



		private ScreenModeEnum _ScreenMode;
		public ScreenModeEnum ScreenMode
		{
			get { return _ScreenMode; }
			set
			{
				_ScreenMode = value;
				OnPropertyChanged("ScreenMode");
			}
		}

		private Int32 _ResolutionSizeX;
		public Int32 ResolutionSizeX
		{
			get { return _ResolutionSizeX; }
			set
			{
				_ResolutionSizeX = value;
				OnPropertyChanged("ResolutionSizeX");
			}
		}

		private Int32 _ResolutionSizeY;
		public Int32 ResolutionSizeY
		{
			get { return _ResolutionSizeY; }
			set
			{
				_ResolutionSizeY = value;
				OnPropertyChanged("ResolutionSizeY");
			}
		}

    private Boolean _VSync;
    public Boolean VSync
	    {
	    get { return _VSync; }
	    set
		    {
		    _VSync = value;
		    OnPropertyChanged("VSync");
		    }
	    }

    private Boolean _ScreenshotQuality;
    public Boolean ScreenshotQuality
	    {
	    get { return _ScreenshotQuality; }
	    set
		    {
		    _ScreenshotQuality = value;
		    OnPropertyChanged("ScreenshotQuality");
		    }
	    }

		#endregion


		public CSettingsScreen(CSettingsManager MySettingsManager)
	    {
	    SettingsManager = MySettingsManager;
			VideoModes=new CVideoModes();
	    }

    public void Init()
	    {
	    GetScreenMode();
      ResolutionSizeX = GetIntValue("ResolutionSizeX", 800);
      ResolutionSizeY = GetIntValue("ResolutionSizeY", 640);
			VSync = GetBooleanValue("bUseVSync",false);
	    ScreenshotQuality = GetBooleanValueFromInt("ScreenShotQuality",false);
		}

    public void Update()
	    {
			SettingsManager.UpdateSetting("ResolutionSizeX", ResolutionSizeX.ToString(), SectionEnum.User);
	    SettingsManager.UpdateSetting("ResolutionSizeY", ResolutionSizeY.ToString(), SectionEnum.User);
			SettingsManager.UpdateSetting("PreferredFullscreenMode", ((Int32)ScreenMode).ToString(), SectionEnum.User);
			WriteBooleanValue(VSync,"bUseVSync",SectionEnum.User);
	    WriteBooleanValueAsInt(ScreenshotQuality, "ScreenShotQuality",SectionEnum.User);
	    }
  
    private void GetScreenMode()
	    {
	    SettingsManager.GetSetting("PreferredFullscreenMode", out var Temp);
	    if (Temp.Length == 0)
		    {
		    ScreenMode = ScreenModeEnum.FullScreen;
		    return;
		    }
			ScreenMode= (ScreenModeEnum)Convert.ToInt32(Temp);
			}
	}
}
