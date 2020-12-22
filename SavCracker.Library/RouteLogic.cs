using Logging.Library;
using SavCracker.Library.Models;
using SavCrackerTest.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SavCracker.Library
  {
  public class RouteLogic
    {
    public static string StripRouteString(string s)
      {
      var idx = s.IndexOf("_Route_Gameplay", StringComparison.Ordinal);
      if (idx > 0)
        {
        s = s.Substring(1, idx - 1);
        }
      else
        {
        s = s.Substring(1);
        idx = s.IndexOf("/", StringComparison.Ordinal);
        if (idx > 0)
          {
          s = s.Substring(0, idx);
          }
        }

      return s;
      }

    public static void HarmonizeRouteName(SavScenarioModel scenario, string inputName, List<SavCrackerRouteModel> routeList)
      {
      if (routeList == null)
        {
        Log.Trace("RoueList is null, initialization missing?", LogEventType.Error);
        scenario.RouteName = "inputName";
        scenario.RouteAbbreviation = "XXX";
        return;
        }
      SavCrackerRouteModel route = routeList.FirstOrDefault(x => x.ScenarioPlannerRouteName == inputName);
      if (route == null)
        {
        scenario.RouteName = "inputName";
        scenario.RouteAbbreviation = "XXX";
        }
      else
        {
        scenario.RouteName = route.RouteName;
        scenario.RouteAbbreviation = route.RouteAbbrev;
        }
      }
    }
  }
