using System;
using System.Collections.Generic;
using System.Text;

namespace SavCracker.Library.Models
	{
	public enum PropertyTypeEnum
		{
		Undefined,
		StringProperty,
		BoolProperty,
		NameProperty,
		StructProperty,
		ArrayProperty,
		SoftObjectProperty,
		GuidProperty,
		TimeSpanProperty,
		DateTimeProperty,
		SoftObjectPathProperty,
		Empty,
    TextProperty,
    LinearColourProperty,
    MapProperty
    }
	public class SavPropertyModel
		{
		public int Position { get; set; }
		//public int Length { get; set; }
		public string PropertyName { get; set; }
		public PropertyTypeEnum PropertyType { get; set; }

		//public string Report
		//	{
		//	get
		//		{
		//		return $"{Position:D4}, {Length:D4}, {PropertyType}, {PropertyName}";
		//		}
		//	}
		}
	}
