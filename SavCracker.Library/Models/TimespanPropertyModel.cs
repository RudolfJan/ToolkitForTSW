using System;

namespace SavCrackerTest.Models
  {
  public class TimespanPropertyModel: SavPropertyModel
    {
    public ulong TimeValue { get; set; }
    public DateTime StartTime { get; set; }
    public string TimeString { get; set; }
    }
  }
