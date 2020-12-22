using System;
using System.Collections.Generic;
using System.Text;

namespace ToolkitForTSW.Models
  {
  public class RouteModel
    {
    public int Id { get; set; }
    public string RouteName { get; set; }
    public string RouteAbbrev { get; set; }
    public string RouteDescription { get; set; }
    public string RouteImagePath { get; set; }
    public string ScenarioPlannerRouteName { get; set; }
    public string ScenarioPlannerRouteString { get; set; }
    public int DlcId { get; set; }
    }
  }
