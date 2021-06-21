using Logging.Library;
using Styles.Library.Helpers;
using System;
using System.ComponentModel;
using System.Globalization;

namespace ToolkitForTSW.Settings
  {
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

  // https://stackoverflow.com/questions/1799370/getting-attributes-of-enums-value

  //public static class EnumExtensions
  //  {
  //  // This extension method is broken out so you can use a similar pattern with 
  //  // other MetaData elements in the future. This is your base method for each.
  //  public static T GetAttribute<T>(this Enum Value) where T : Attribute
  //    {
  //    var Type = Value.GetType();
  //    var MemberInfo = Type.GetMember(Value.ToString());
  //    var Attributes = MemberInfo[0].GetCustomAttributes(typeof(T), false);
  //    return Attributes.Length > 0
  //      ? (T)Attributes[0]
  //      : null;
  //    }

  //  // This method creates a specific call to the above method, requesting the
  //  // Description MetaData attribute.
  //  public static String ToName(this Enum Value)
  //    {
  //    var Attribute = Value.GetAttribute<DescriptionAttribute>();
  //    return Attribute == null ? Value.ToString() : Attribute.Description;
  //    }
  //  }

  public class CSetting : Notifier
    {
    public String Key { get; set; } = String.Empty;
    public String Value { get; set; } = String.Empty;
    public SectionEnum Section { get; set; }

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

    public CSetting()
      {
      }

    public CSetting(String MyKey, String MyValue, SectionEnum MySection)
      {
      Key = MyKey;
      Value = MyValue;
      Section = MySection;
      }

    public CSetting(String InputLine, SectionEnum CurrentSection)
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

    public bool GetBooleanValue(string key, bool defaultValue)
      {
      SettingsManager.GetSetting(key, out var temp);
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
      SettingsManager.GetSetting(key, out var temp);
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
      SettingsManager.GetSetting(key, out var Temp);
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
      SettingsManager.GetSetting(key, out var Temp);
      if (Temp.Length > 0)
        {
        Double.TryParse(Temp, Style, Culture, out var Temp2);
        return (UInt32)Temp2;
        }
      return defaultValue;
      }

    public double GetDoubleValue(string key, double defaultValue)
      {
      var Culture = CultureInfo.CreateSpecificCulture("en-GB");

      SettingsManager.GetSetting(key, out var Temp);
      if (Temp.Length > 0)
        {
        return Math.Round(Convert.ToDouble(Temp, Culture), 2);
        }
      return defaultValue;
      }

    public void WriteBooleanValue(bool value, string key, SectionEnum section)
      {
      if (value)
        {
        SettingsManager.UpdateSetting(key, "True", section);
        return;
        }
      SettingsManager.UpdateSetting(key, "False", section);
      }

    public void WriteBooleanValueAsInt(bool value, string key, SectionEnum section)
      {
      if (value)
        {
        SettingsManager.UpdateSetting(key, "1", section);
        return;
        }
      SettingsManager.UpdateSetting(key, "0", section);
      }

    public override String ToString()
      {
      return Key + "=" + Value;
      }
    }
  }
