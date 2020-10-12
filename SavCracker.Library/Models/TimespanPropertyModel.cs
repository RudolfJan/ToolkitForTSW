using System;
using System.Collections.Generic;
using System.Text;

namespace SavCrackerTest.Models
  {
  public class TimespanPropertyModel: SavPropertyModel
    {
    public ulong TimeValue { get; set; }
    public DateTime StartTime { get; set; }
    public string TimeString { get; set; }
    public new string Report
      {
      get
        {
        return $"{base.Report}";
        }
      }
    }
  }
