// Ignore Spelling: Sav

using SavCracker.Library.Models;
using System;

namespace SavCracker.Library
  {
  public class ConsistLogic
    {
    public static void ParseConsistString(SavServiceModel service)
      {
      var s = service.ConsistString;
      service.ConsistName = GetConsistNamePart(s);
      string[] x = service.ConsistName.Split('.');
      foreach (var s3 in x)
        {
        service.ConsistVehicles.Add(s3);
        }

      if (service.ConsistVehicles.Count > 0)
        {
        service.ConsistName = service.ConsistVehicles[0];
        }
      }

    private static string GetConsistNamePart(string input)
      {
      var output = TryLocateConsistNamePart(input, "ServiceModFormations/GP38Formations/");
      if (output.Length > 0)
        return output;
      output = TryLocateConsistNamePart(input, "ServiceModFormations/");
      if (output.Length > 0)
        return output;
      output = TryLocateConsistNamePart(input, "Formations/");

      if (output.Length > 0)
        return output;
      output = TryLocateConsistNamePart(input, "RailVehicleDefinition/");
      if (output.Length > 0)
        return output;
      output = TryLocateConsistNamePart(input, "Data/");
      if (output.Length > 0)
        return output;

      return input; // fallback
      }

    private static string TryLocateConsistNamePart(string input, string searchKey)
      {
      var idx = input.IndexOf(searchKey, StringComparison.Ordinal);
      if (idx > 1)
        {
        return input.Substring(idx + searchKey.Length);
        }
      return string.Empty;
      }
    }
  }
