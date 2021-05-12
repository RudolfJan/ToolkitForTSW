using System;
using System.Collections.Generic;
using System.Text;

namespace ToolkitForTSW.Models
  {
  public class EngineIniSettingsModel
    {
    public int Id { get; set; }
    public string SettingName { get; set; }
    public string SettingDescription { get; set; } =string.Empty;
    public string MinValue { get; set; } = string.Empty;
    public string MaxValue { get; set; } = string.Empty;
    public string DefaultValue { get; set; } = string.Empty;
    public string ValueType { get; set; } = string.Empty;
    }
  }
