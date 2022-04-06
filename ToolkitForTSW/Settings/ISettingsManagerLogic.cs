using System.Collections.Generic;
using System.IO;
using ToolkitForTSW.Settings.Enums;

namespace ToolkitForTSW.Settings
  {
  public interface ISettingsManagerLogic
    {
    SectionEnum CurrentSection { get; set; }
    string Result { get; set; }
    string SaveSetName { get; set; }
    string SectionCore { get; set; }
    List<ISetting> SettingsDictionary { get; set; }
    string ExperimentalSettingsString { get; set; }
    void GetSetting(string Key, out string Value);
    void LoadSettingsInDictionary(FileInfo SettingsFile, FileInfo EngineIniFile = null);
    void UpdateSetting(string Key, string Value, SectionEnum Section);
    void WriteSettingsInDictionary(FileInfo SettingsFile, bool EngineIni = false);
    void WriteSettingsToSaveSet();
    }
  }