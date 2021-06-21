using System;
using System.Collections.Generic;
using System.Text;

namespace SavCracker.Library.Models
  {
	public class SoftObjectPropertyModel: SavPropertyModel
		{
		public string Value { get; set; }
    public int ContentLength { get; set; } // length of string, including Length value
    public int IndexValue { get; set; }
    //public new string Report
    //  {
    //  get
    //    {
    //    return $"{base.Report}, {Value}";
    //    }
    //  }
		}
	}
