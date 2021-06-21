using System;

namespace SavCracker.Library.Models
  {
  public class TimespanPropertyModel: SavPropertyModel
    {
    public ulong TimeValue { get; set; }
    public DateTime StartTime { get; set; }
    public string TimeString { get; set; }
    }
  }
