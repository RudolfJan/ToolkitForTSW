using System;
using System.Collections.Generic;
using System.Text;

namespace SavCracker.Library.Models
  {
  public class LiveryModel
    {
    public int Id { get; set; }
    public string LiveryName { get; set; }
    public string BaseDefinition { get; set; } // used to be livery string
    public string RailVehicleName { get; set; }
    public string LiveryIdString { get; set; } // Guid like format in the livery editor.
    // public List<UEProperty> PropertyList { get; set; } // properties found in this livery at Unreal level
    }
  }
