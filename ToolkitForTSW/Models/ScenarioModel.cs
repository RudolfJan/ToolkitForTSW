using System;
using System.Collections.Generic;
using System.Text;

namespace ToolkitForTSW.Models
  {
  public class ScenarioModel
    {
    public int Id { get; set; }
    public string ScenarioName { get; set; }
    public string ScenarioGuid { get; set; }
    public int RouteId { get; set; }
    }
  }
