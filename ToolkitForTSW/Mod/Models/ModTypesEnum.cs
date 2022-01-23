using System.ComponentModel;

namespace ToolkitForTSW.Mod.Models
  {
  public enum ModTypesEnum

    {
    [Description("Undefined")] Undefined,
    [Description("Engine")] Engine,
    [Description("Wagon")] Wagon,
    [Description("Consist")] Consist,
    [Description("Scenery")] Scenery,
    [Description("Route")] Route,
    [Description("Scenario")] Scenario,
    [Description("Service timetable")] Service,
    [Description("Weather")] Weather,
    [Description("Game")] Game,
    [Description("Other")] Other
    };
  }
