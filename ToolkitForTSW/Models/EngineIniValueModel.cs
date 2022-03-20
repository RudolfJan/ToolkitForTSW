namespace ToolkitForTSW.Models
  {
  public class EngineIniValueModel
    {
    public int WorkSetId { get; set; }
    public string WorkSetName { get; set; }
    public int SettingId { get; set; }
    public string SettingName { get; set; }
    public string SettingDescription { get; set; } = string.Empty;
    public string SettingValue { get; set; }
    }
  }
