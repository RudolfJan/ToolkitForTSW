using SavCracker.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SavCracker.Library.Models
  {
  public class DateTimePropertyModel: SavPropertyModel
    {
      public ulong TimeValue { get; set; }
      public DateTime DateTimeData { get; set; }
      public string TimeString { get; set; }
      }
    }
