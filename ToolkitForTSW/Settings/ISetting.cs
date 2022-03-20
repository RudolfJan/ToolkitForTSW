using ToolkitForTSW.Settings.Enums;

namespace ToolkitForTSW.Settings
  {
  public interface ISetting
    {
    string Key { get; set; }
    SectionEnum Section { get; set; }
    string Value { get; set; }
    string GetStringValue(string key, string defaultValue);
    bool GetBooleanValue(string key, bool defaultValue);
    bool GetBooleanValueFromInt(string key, bool defaultValue);
    double GetDoubleValue(string key, double defaultValue);
    int GetIntValue(string key, int defaultValue);
    uint GetUIntValue(string key, uint defaultValue);
    string ToString();
    void WriteStringValue(string value, string key, SectionEnum section);
    void WriteBooleanValue(bool value, string key, SectionEnum section);
    void WriteIntValue(int value, string key, SectionEnum section);
    void WriteBooleanValueAsInt(bool value, string key, SectionEnum section);
    void WriteDoubleValue(double value, string key, SectionEnum section);
    }
  }