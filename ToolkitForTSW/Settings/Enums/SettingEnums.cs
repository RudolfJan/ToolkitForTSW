using System.ComponentModel;

namespace ToolkitForTSW.Settings.Enums
  {
  #region enums

  public enum SectionEnum
    {
    [Description("")] None,
    [Description("[/Script/TS2Prototype.TS2GameUserSettings]")]
    User,
    [Description("[/Script/Engine.GameUserSettings]")]
    Engine,
    [Description("[ScalabilityGroups]")] Scalability,
    [Description("[Core.System]")] Core,
    [Description("[SystemSettings]")] System,
    [Description("[Audio]")] Audio,
    [Description("[Other]")] Other
    }


  public enum HudStyleEnum
    {
    [Description("None")] None,
    [Description("HUD")] HUD,
    [Description("Marker")] Marker,
    [Description("Both")] Both
    }

  public enum TemperatureEnum
    {
    [Description("Celsius")] Celsius,
    [Description("FahrenHeit")] Fahrenheit,
    [Description("Automatic")] Automatic
    }

  public enum UnitsEnum
    {
    [Description("Metric")] Metric,
    [Description("Imperial")] Imperial,
    [Description("Automatic")] Automatic
    }

  public enum GradeUnitsEnum
    {
    [Description("Percentage")] Percentage,
    [Description("Ratio")] Ratio,
    [Description("Automatic")] Automatic
    }

  public enum ScreenModeEnum
    {
    [Description("FullScreen")] FullScreen = 0,
    [Description("Windowed FullScreen")] WindowedFullScreen = 1,
    [Description("Windowed")] WindowedScreen = 2
    }

  public enum QualityEnum
    {
    [Description("Low")] Low = 0,
    [Description("Medium")] Medium = 1,
    [Description("High")] High = 2,
    [Description("Ultra")] Ultra = 3,
    }

  public enum QualityPlusOffEnum
    {
    [Description("Off")] Off = 0,
    [Description("Low")] Low = 1,
    [Description("Medium")] Medium = 2,
    [Description("High")] High = 3
    }

  public enum AntiAliasingEnum
    {
    [Description("Off")] Off = 0,
    [Description("FXAA")] FXAA = 1,
    [Description("TAA")] TAA = 2
    }

  public enum CrosshairVisibilityEnum
    {
    [Description("Off")] Off = 0,
    [Description("50%")] Medium = 1,
    [Description("100%")] Full = 2,
    [Description("Large")] Large = 3
    }
  #endregion
  }
