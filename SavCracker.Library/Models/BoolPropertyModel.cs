using System;
using System.Collections.Generic;
using System.Text;

namespace SavCrackerTest.Models
	{
	public class BoolPropertyModel: SavPropertyModel
		{
		public bool Value { get; set; }
		//public new string Report
		//	{
		//	get
		//		{
		//		return $"{base.Report}, {Value}";
		//		}
		//	}
		}
	}
