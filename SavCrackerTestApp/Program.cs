using SQLiteDatabase.Library;
using System;
using System.IO;
using ToolkitForTSW;
using ToolkitForTSW.DataAccess;
using ToolkitForTSW.Scenario;
using Utilities.Library.TextHelpers;

namespace SavCrackerTestApp
  {
  class Program
    {
    static void Main(string[] args)
      {
      var factory = new DatabaseFactory();
      var databasePath = $"{CTSWOptions.TSWToolsFolder}TSWTools.db";
      var connectionString = $"Data Source = {databasePath}; Version = 3;";
      DbManager.InitDatabase(connectionString, databasePath, factory);
      CScenarioManager ScenarioManager = new CScenarioManager();
      foreach (var scenario in ScenarioManager.ScenarioList)
        {
        CScenarioEdit clone = new CScenarioEdit
          {
          Scenario = scenario,
          ClonedScenarioGuid = scenario.SavScenario.ScenarioGuid,
          ClonedScenarioName = scenario.SavScenario.ScenarioName
          };
        var testFile= SavScenarioBuilder.GetClonedScenarioFileName(clone.ClonedScenarioGuid.ToString(),true);
        SavScenarioBuilder.Build(scenario);
        CompareBinaryFiles(clone.Scenario.ScenarioFile.FullName, testFile,scenario);
        }
      }

    public static bool CompareBinaryFiles(string filename1, string filename2, CScenario scenario)
      {
      byte[] file1 = File.ReadAllBytes(filename1);
      byte[] file2 = File.ReadAllBytes(filename2);

      if (file1.Length != file2.Length)
        {
        return false;
        }

      for (int i = 0; i < file1.Length; i++)
        {
        if (file1[i] != file2[i])
          {
          var length = 100;
          // Display some data
          byte[] display1 = new byte[length];
          byte[] display2 = new byte[length];
          System.Array.ConstrainedCopy(file1, Math.Max(i - length / 2, 0), display1, 0, length);
          System.Array.ConstrainedCopy(file2, Math.Max(i - length / 2, 0), display2, 0, length);
          var s1 = TextHelper.HexViewer(display1);
          var s2 = TextHelper.HexViewer(display2);

          Console.WriteLine($"Issue found in {filename1}\r\n{scenario.SavScenario.RouteAbbreviation} {scenario.SavScenario.ScenarioName}\r\n{s1}\r\n{s2}\r\n\r\n");
          return false;
          }
        }
      return true;
      }
    }
  }
