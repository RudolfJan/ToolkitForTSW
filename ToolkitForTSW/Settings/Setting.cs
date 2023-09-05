using Logging.Library;
using System;
using System.Globalization;
using ToolkitForTSW.Settings.Enums;
using Utilities.Library;

namespace ToolkitForTSW.Settings
  {

  public class Setting : ISetting
    {
    private readonly ISettingsManagerLogic _settingsManagerLogic;
    public String Key { get; set; } = String.Empty;
    public String Value { get; set; } = String.Empty;
    public SectionEnum Section { get; set; }
    public Setting(ISettingsManagerLogic settingsManagerLogic)
      {
      _settingsManagerLogic = settingsManagerLogic;
      }

    public Setting(String MyKey, String MyValue, SectionEnum MySection)
      {
      Key = MyKey;
      Value = MyValue;
      Section = MySection;
      }

    public Setting(String InputLine, SectionEnum CurrentSection)
      {
      if (InputLine.StartsWith("["))
        {
        Key = "";
        Value = "";
        InputLine = InputLine.Trim();
        var Temp = SectionEnum.User.ToName();
        if (String.CompareOrdinal(InputLine, Temp) == 0)
          {
          Section = SectionEnum.User;
          return;
          }

        Temp = SectionEnum.Engine.ToName();
        if (String.CompareOrdinal(InputLine, Temp) == 0)
          {
          Section = SectionEnum.Engine;
          return;
          }

        Temp = SectionEnum.Scalability.ToName();
        if (String.CompareOrdinal(InputLine, Temp) == 0)
          {
          Section = SectionEnum.Scalability;
          return;
          }

        Temp = SectionEnum.Core.ToName();
        if (String.CompareOrdinal(InputLine, Temp) == 0)
          {
          Section = SectionEnum.Core;
          return;
          }
        Temp = SectionEnum.System.ToName();
        if (String.CompareOrdinal(InputLine, Temp) == 0)
          {
          Section = SectionEnum.System;
          return;
          }

        Temp = SectionEnum.Audio.ToName();
        if (String.CompareOrdinal(InputLine, Temp) == 0)
          {
          Section = SectionEnum.System;
          return;
          }

        Section = SectionEnum.Other;
        Log.Trace("Section is unknown " + InputLine + " Please use [SystemSettings] in Engine.ini");
        }

      if (InputLine.Length > 1)
        {
        String[] Str2 = InputLine.Split('=');
        if (Str2.GetLength(0) >= 2)
          {
          Key = Str2[0];
          Value = Str2[1].Trim();
          Section = CurrentSection;
          }
        }
      }

    public string GetStringValue(string key, string defaultValue)
      {
      _settingsManagerLogic.GetSetting(key, out var temp);
      if (temp.Length == 0) // Not found in dictionary
        {
        return defaultValue;
        }
      return temp;
      }

    public bool GetBooleanValue(string key, bool defaultValue)
      {
      _settingsManagerLogic.GetSetting(key, out var temp);
      if (temp.Length == 0) // Not found in dictionary
        {
        return defaultValue;
        }

      if (string.CompareOrdinal(temp, "False") == 0)
        {
        return false;
        }
      return true;
      }

    public bool GetBooleanValueFromInt(string key, bool defaultValue)
      {
      _settingsManagerLogic.GetSetting(key, out var temp);
      if (temp.Length == 0)
        {
        return defaultValue;
        }

      if (string.CompareOrdinal(temp, "0") == 0)
        {
        return false;
        }
      return true;
      }

    public int GetIntValue(string key, int defaultValue)
      {
      var Style = NumberStyles.AllowDecimalPoint | NumberStyles.Number;
      var Culture = CultureInfo.CreateSpecificCulture("en-GB");
      _settingsManagerLogic.GetSetting(key, out var Temp);
      if (Temp.Length > 0)
        {
        Double.TryParse(Temp, Style, Culture, out var Temp2);
        return (Int32)Temp2;
        }
      return defaultValue;
      }

    public uint GetUIntValue(string key, uint defaultValue)
      {
      var Style = NumberStyles.AllowDecimalPoint | NumberStyles.Number;
      var Culture = CultureInfo.CreateSpecificCulture("en-GB");
      _settingsManagerLogic.GetSetting(key, out var Temp);
      if (Temp.Length > 0)
        {
        Double.TryParse(Temp, Style, Culture, out var Temp2);
        return (UInt32)Temp2;
        }
      return defaultValue;
      }

    public double GetDoubleValue(string key, double defaultValue)
      {
      //var style = NumberStyles.AllowDecimalPoint | NumberStyles.Number;
      var Culture = CultureInfo.CreateSpecificCulture("en-GB");

      _settingsManagerLogic.GetSetting(key, out var Temp);
      if (Temp.Length > 0)
        {
        return Math.Round(Convert.ToDouble(Temp, Culture), 2);
        }
      return defaultValue;
      }

    public void WriteStringValue(string value, string key, SectionEnum section)
      {
      _settingsManagerLogic.UpdateSetting(key, value, section);
      }
    public void WriteBooleanValue(bool value, string key, SectionEnum section)
      {
      if (value)
        {
        _settingsManagerLogic.UpdateSetting(key, "True", section);
        return;
        }
      _settingsManagerLogic.UpdateSetting(key, "False", section);
      }

    public void WriteDoubleValue(double value, string key, SectionEnum section)
      {
      var Culture = CultureInfo.CreateSpecificCulture("en-GB");
      // var tmp = value.ToString(Culture); //DEBUG
      _settingsManagerLogic.UpdateSetting(key, value.ToString(Culture), section);
      }


    public void WriteBooleanValueAsInt(bool value, string key, SectionEnum section)
      {
      if (value)
        {
        _settingsManagerLogic.UpdateSetting(key, "1", section);
        return;
        }
      _settingsManagerLogic.UpdateSetting(key, "0", section);
      }

    public void WriteIntValue(int value, string key, SectionEnum section)
      {
      _settingsManagerLogic.UpdateSetting(key, value.ToString(), section);
      }

    public override String ToString()
      {
      return Key + "=" + Value;
      }
    }
  }
