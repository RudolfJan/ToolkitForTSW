using SavCrackerTest.Models;
using System;

namespace SavCracker.Library
  {
  public class EngineLogic
    {
    public static void ParseEngineString(SavServiceModel service)
      {
      var s = service.EngineString;
      var rvDetailString = ExtractEngineName(service, s);
      var s2 = rvDetailString.Replace("RVD_", "", StringComparison.Ordinal).Replace("_RVD", "");
      string[] x = s2.Split('.');
      foreach (var s3 in x)
        {
        service.EngineVehicles.Add(s3);
        }
      }

    private static string ExtractEngineName(SavServiceModel service, string s)
      {
      var idx = s.IndexOf("/Data/RailVehicleDefinition/", StringComparison.Ordinal);
      if (idx > 1)
        {
        service.EngineName = s.Substring(1, idx - 1);
        return s.Substring(idx + 28);
        }
      idx = s.IndexOf("/RailVehicleDefinition/", StringComparison.Ordinal);
      if (idx > 1)
        {
        service.EngineName = s.Substring(1, idx - 1);
        return s.Substring(idx + 23);
        }

      idx = s.IndexOf("/Data/", StringComparison.Ordinal);
        {
        if (idx > 1)
          {
          service.EngineName = s.Substring(1, idx - 1);
          return s.Substring(idx + 6);
          }

        service.EngineName = s;
        return string.Empty;
        }
      }
    }
  }
