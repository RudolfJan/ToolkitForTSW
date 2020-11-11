using System.Collections.Generic;

namespace SavCrackerTest.Models
	{
	public class ArrayPropertyModel : SavPropertyModel
		{
		public int ContentLength { get; set; }
		public int IndexValue { get; set; }
		public PropertyTypeEnum ElementType { get; set; }
		public int ElementCount { get; set; }
		public List<SavPropertyModel> PayLoad { get; set; }= new List<SavPropertyModel>();

		//public string Report
		//	{
		//	get
		//		{
		//		return $"{base.Report}, ElementType {ElementType}, Content length {ContentLength}, Element Count {ElementCount} ";
		//		}
		//	}
		}
	}
