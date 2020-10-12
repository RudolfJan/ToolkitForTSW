using System;
using System.Collections.Generic;
using System.Text;

namespace SavCrackerTest.Models
	{
	public class StringPropertyModel: SavPropertyModel
		{
		public string Value { get; set; }
		public int ContentLength { get; set; } // length of string, including Length value
		public int IndexValue { get; set; }
		public new string Report
			{
			get
				{
				return $"{base.Report}, {Value}";
				}
			}
		}
	}
