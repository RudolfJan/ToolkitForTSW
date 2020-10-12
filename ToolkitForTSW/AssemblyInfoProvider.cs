using System;
using System.Reflection;

namespace ToolkitForTSW
{
    public static class AssemblyInfoProvider
		{
		#region Assembly Attribute Accessors
		private static T GetAssemblyAttribute<T>() where T : Attribute
			{
			Object[] Attributes = Assembly.GetExecutingAssembly()
				.GetCustomAttributes(typeof(T), true);
			if (Attributes.Length == 0) return null;
			return (T)Attributes[0];
			}

		public static String AssemblyCompany
			{
			get
				{
				var Attr = GetAssemblyAttribute<AssemblyCompanyAttribute>();
				if (Attr != null)
					return Attr.Company;
				return String.Empty;
				}
			}

		public static String AssemblyCopyright
			{
			get
				{
				var Attr = GetAssemblyAttribute<AssemblyCopyrightAttribute>();
				if (Attr != null)
					return Attr.Copyright;
				return String.Empty;
				}
			}

		public static String AssemblyProduct
			{
			get
				{
				var Attr = GetAssemblyAttribute<AssemblyProductAttribute>();
				if (Attr != null)
					return Attr.Product;
				return String.Empty;
				}
			}

		public static String AssemblyDescription
			{
			get
				{
				var Attr = GetAssemblyAttribute<AssemblyDescriptionAttribute>();
				if (Attr != null)
					return Attr.Description;
				return String.Empty;
				}
			}
		#endregion
		}
	}
