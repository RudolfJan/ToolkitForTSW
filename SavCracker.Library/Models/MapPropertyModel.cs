using System;
using System.Collections.Generic;
using System.Text;

namespace SavCracker.Library.Models
  {
  public class MapPropertyModel: SavPropertyModel
    {
    public string Value { get; set; }
    public int ContentLength { get; set; }
    public int IndexValue { get; set; }
    public string Key { get; set; }
    }
  }
