using System;
using System.ComponentModel;

namespace TSWTools
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
		[Description("[Other]")] Other
		}

	// https://stackoverflow.com/questions/1799370/getting-attributes-of-enums-value

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
				? (T) Attributes[0]
				: null;
			}

// This method creates a specific call to the above method, requesting the
// Description MetaData attribute.
		public static String ToName(this Enum Value)
			{
			var Attribute = Value.GetAttribute<DescriptionAttribute>();
			return Attribute == null ? Value.ToString() : Attribute.Description;
			}
		}

	public class CSetting
		{
		public String Key { get; set; } = String.Empty;
		public String Value { get; set; } = String.Empty;
		public SectionEnum Section { get; set; }

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

				Section = SectionEnum.Other;
				CLog.Trace("Section is unknown "+ InputLine +" Please use [SystemSettings] in Engine.ini");
				}

			if (InputLine.Length > 1)
				{
				String[] Str2 = InputLine.Split('=');
				Key = Str2[0];
				Value = Str2[1].Trim();
				Section = CurrentSection;
				}
			}

		public override String ToString()
			{
			return Key + "=" + Value;
			}
		}
	}
