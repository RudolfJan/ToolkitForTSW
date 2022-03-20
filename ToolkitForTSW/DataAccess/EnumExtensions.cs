using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Styles.Library.Helpers
  {
  public enum SampleEnum
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

  //  Usage example:       var Temp = SectionEnum.User.ToName();


  public static class EnumExtensions
    {
    // This extension method is broken out so you can use a similar pattern with 
    // other MetaData elements in the future. This is your base method for each.
    public static T GetAttribute<T>(this Enum Value) where T : Attribute
      {
      var Type = Value.GetType();
      var MemberInfo = Type.GetMember(Value.ToString());
      var Attributes = MemberInfo[0].GetCustomAttributes(typeof(T), false);
      return Attributes.Length > 0
        ? (T)Attributes[0]
        : null;
      }

    // This method creates a specific call to the above method, requesting the
    // Description MetaData attribute.
    public static string ToName(this Enum Value)
      {
      var Attribute = Value.GetAttribute<DescriptionAttribute>();
      return Attribute == null ? Value.ToString() : Attribute.Description;
      }
    }
  }
