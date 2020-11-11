using System;
using System.Collections.Generic;
using System.Text;

namespace SavCrackerTest.Models
	{
	public enum StructureTypeEnum
		{
		Undefined,
		Guid,
		TimeSpan,
		SoftObjectPath,
		EmbeddedObject
		}
	public class StructPropertyModel: SavPropertyModel
		{
		public int ContentLength { get; set; }
		public int IndexValue { get; set; }
		public string StructureContentName { get; set; } = string.Empty;
		public StructureTypeEnum StructureType { get; set; }
		public List<SavPropertyModel> PayLoad { get; set; } = new List<SavPropertyModel>();

		//public new string Report
		//	{
		//	get
		//		{
		//		return $"{base.Report}, Structure Type {StructureType}, Content length {ContentLength}";
		//		}
		//	}
		}
	}
