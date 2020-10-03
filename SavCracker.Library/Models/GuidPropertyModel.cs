using System;
using System.Collections.Generic;
using System.Text;

namespace SavCrackerTest.Models
	{
	public class GuidPropertyModel: SavPropertyModel
		{
		public Guid GuidValue { get; set; }
		public new string Report
			{
			get
				{
				return $"{base.Report}, {GuidValue.ToString().ToUpper()}";
				}
			}
		}
	}
