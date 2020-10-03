using SavCrackerTest.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SavCracker.Library
  {
 public  class ScenarioProblemTracker
    {
    public static List<string> FindScenarioProblems(SavScenarioModel savScenario)
      {
      var output = new List<string>();
      output.Add(HasPlayerService(savScenario.SavServiceList));
      output.AddRange(AIStartTimeCheck(savScenario.SavServiceList));
      output.AddRange(ServiceConfirmationCheck(savScenario.SavServiceList));
      return output;
      }

    private static List<string> ServiceConfirmationCheck(List<SavServiceModel> savServiceList)
      {
      var output = new List<string>();
      foreach (var service in savServiceList)
        {
        if (String.CompareOrdinal(service.ConsistName, "None") == 0)
          {
          var s = $"ERROR Service {service.ServiceName} is not confirmed properly in the editor";
          output.Add(s);
          }
        else
          {
          var s = $"OK    Service {service.ServiceName} has consist set properly";
          output.Add(s);
          }
        }
      return output;
      }

    private static ulong GetPlayerServiceStartTime(List<SavServiceModel> savServiceList)
      {
      foreach (var service in savServiceList)
        {
        if (service.IsPlayerService)
          {
          return service.StartTime;
          }
        }
      return 0;
      }
    private static List<string> AIStartTimeCheck(List<SavServiceModel> savServiceList)
      {
      var output = new List<string>();
      var playerStartTime = GetPlayerServiceStartTime(savServiceList);
      foreach (var service in savServiceList)
        {
        if (service.StartTime < playerStartTime)
          {
          var s = $"ERROR {service.ServiceName} AI service cannot start before player service";
          output.Add(s);
          }
        else
          {
          var s = $"OK    {service.ServiceName} AI service will start when scenario starts or later";
          output.Add(s);
          }
        }
      return output;
      }

    private static string HasPlayerService(List<SavServiceModel> savServiceList)
      {
      var output = string.Empty;
      foreach (var service in savServiceList)
        {
        if (service.IsPlayerService)
          {
          return "OK    Player service is present";
          }
        }

      output = "ERROR Scenario does not have a player service";
      return output;
      }
    }
  }
