using Caliburn.Micro;
using System;
using ToolkitForTSW.Settings.Enums;

namespace ToolkitForTSW.Settings.ViewModels
  {
  public class SettingsScreenViewModel : Screen
    {
    private readonly ISetting _setting;
    #region Properties

    private BindableCollection<DisplaySettings> _VideoModesList;
    public BindableCollection<DisplaySettings> VideoModesList
      {
      get { return _VideoModesList; }
      set
        {
        _VideoModesList = value;
        NotifyOfPropertyChange(nameof(VideoModesList));
        }
      }


    private DisplaySettings _selectedScreenResolution;
    public DisplaySettings SelectedScreenResolution
      {
      get
        {
        return _selectedScreenResolution;
        }
      set
        {
        _selectedScreenResolution = value;
        NotifyOfPropertyChange(nameof(SelectedScreenResolution));
        }
      }
    private ScreenModeEnum _ScreenMode;
    public ScreenModeEnum ScreenMode
      {
      get { return _ScreenMode; }
      set
        {
        _ScreenMode = value;
        NotifyOfPropertyChange(nameof(ScreenMode));
        }
      }

    private Int32 _ResolutionSizeX;
    public Int32 ResolutionSizeX
      {
      get { return _ResolutionSizeX; }
      set
        {
        _ResolutionSizeX = value;
        NotifyOfPropertyChange(nameof(ResolutionSizeX));
        }
      }

    private Int32 _ResolutionSizeY;
    public Int32 ResolutionSizeY
      {
      get { return _ResolutionSizeY; }
      set
        {
        _ResolutionSizeY = value;
        NotifyOfPropertyChange(nameof(ResolutionSizeY));
        }
      }

    private Boolean _VSync;
    public Boolean VSync
      {
      get { return _VSync; }
      set
        {
        _VSync = value;
        NotifyOfPropertyChange(nameof(VSync));
        }
      }

    private Boolean _ScreenshotQuality;
    public Boolean ScreenshotQuality
      {
      get { return _ScreenshotQuality; }
      set
        {
        _ScreenshotQuality = value;
        NotifyOfPropertyChange(nameof(ScreenshotQuality));
        }
      }
    #endregion

    public SettingsScreenViewModel(ISetting setting)
      {
      _setting = setting;
      DisplayName = "Screen";
      }

    public void Init()
      {
      GetScreenMode();
      VideoModesList = new BindableCollection<DisplaySettings>(DisplayManager.GetDisplayModes());
      ResolutionSizeX = _setting.GetIntValue("ResolutionSizeX", 800);
      ResolutionSizeY = _setting.GetIntValue("ResolutionSizeY", 640);
      VSync = _setting.GetBooleanValue("bUseVSync", false);
      ScreenshotQuality = _setting.GetBooleanValueFromInt("ScreenShotQuality", false);
      }

    public void Update()
      {
      _setting.WriteIntValue(ResolutionSizeX, "ResolutionSizeX", SectionEnum.User);
      _setting.WriteIntValue(ResolutionSizeY, "ResolutionSizeY", SectionEnum.User);
      _setting.WriteIntValue((int)ScreenMode, "PreferredFullscreenMode", SectionEnum.User);
      _setting.WriteBooleanValue(VSync, "bUseVSync", SectionEnum.User);
      _setting.WriteBooleanValueAsInt(ScreenshotQuality, "ScreenShotQuality", SectionEnum.User);
      }

    private void GetScreenMode()
      {
      ScreenMode = (ScreenModeEnum)_setting.GetIntValue("PreferredFullscreenMode", 0);
      }

    public void SetScreenRes()
      {
      ResolutionSizeX = SelectedScreenResolution.Width;
      ResolutionSizeY = SelectedScreenResolution.Height;
      }
    }
  }
