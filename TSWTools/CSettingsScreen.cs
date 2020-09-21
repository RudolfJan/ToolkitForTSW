using Styles.Library.Helpers;
using System;



namespace TSWTools
{
	public class CSettingsScreen: Notifier
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
	    SettingsManager.GetSetting("ResolutionSizeX", out var Temp);
	    if (Temp.Length > 0)
		    {
		    ResolutionSizeX = Convert.ToInt32(Temp);
		    }
	    else
		    {
		    ResolutionSizeX = 0;
		    }

	    SettingsManager.GetSetting("ResolutionSizeY", out Temp);
	    if (Temp.Length > 0)
		    {
		    ResolutionSizeY = Convert.ToInt32(Temp);
		    }
	    else
		    {
		    ResolutionSizeY = 0;
		    }
			VSync = GetBooleanValue("bUseVSync");
	    ScreenshotQuality = GetBooleanValueFromInt("ScreenShotQuality");
		}

    public void Update()
	    {
			SettingsManager.UpdateSetting("ResolutionSizeX", ResolutionSizeX.ToString(), SectionEnum.User);
	    SettingsManager.UpdateSetting("ResolutionSizeY", ResolutionSizeY.ToString(), SectionEnum.User);
			SettingsManager.UpdateSetting("FullscreenMode", ((Int32)ScreenMode).ToString(), SectionEnum.User);
			WriteVSync();
	    WriteScreenshotQuality();
	    }

    private Boolean GetBooleanValueFromInt(String Key)
	    {
	    SettingsManager.GetSetting(Key, out var Temp);
	    if (Temp.Length == 0 || (String.CompareOrdinal(Temp, "1") == 0))
		    {
		    return true;
		    }
	    return false;
	    }
    private Boolean GetBooleanValue(String Key)
      {
      SettingsManager.GetSetting(Key, out var Temp);
      if (Temp.Length == 0 || (String.CompareOrdinal(Temp, "True") == 0))
        {
        return true;
        }
      return false;
      }

    private void GetScreenMode()
	    {
	    SettingsManager.GetSetting("FullscreenMode", out var Temp);
	    if (Temp.Length == 0)
		    {
		    ScreenMode = ScreenModeEnum.FullScreen;
		    return;
		    }
			ScreenMode= (ScreenModeEnum)Convert.ToInt32(Temp);
			}

    private void WriteVSync()
	    {
	    if (VSync)
		    {
		    SettingsManager.UpdateSetting("bUseVSync", "True", SectionEnum.User);
		    }
	    else
		    {
		    SettingsManager.UpdateSetting("bUseVSync", "False", SectionEnum.User);
		    }
	    }

    private void WriteScreenshotQuality()
	    {
	    if (ScreenshotQuality)
		    {
		    SettingsManager.UpdateSetting("ScreenShotQuality", "1", SectionEnum.User);
		    }
	    else
		    {
		    SettingsManager.UpdateSetting("ScreenShotQuality", "0", SectionEnum.User);
		    }
	    }

	}
}
