using SQLiteDatabase.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToolkitForTSW.Models;

namespace ToolkitForTSW.DataAccess
  {
  public class ScenarioDataAccess
    {
    public static List<ScenarioModel> GetAllScenarios()
      {
      var sql = "SELECT * FROM Scenarios";
      return DbAccess.LoadData<ScenarioModel, dynamic>(sql, new { });
      }

    public static ScenarioModel GetScenarioByGuid(Guid scenarioGuid)
      {
      var sql = "SELECT * FROM Scenarios WHERE ScenarioGuid=@guidString";
      var guidString= scenarioGuid.ToString();
      return DbAccess.LoadData<ScenarioModel, dynamic>(sql, new {guidString }).FirstOrDefault();
      }

    public static int InsertScenario(ScenarioModel scenario)
      {
      var sql = $"INSERT OR IGNORE INTO Scenarios (ScenarioName, ScenarioGuid, RouteId) " +
                $"VALUES(@ScenarioName, @ScenarioGuid, @RouteId);{DbAccess.LastRowInsertQuery}";
      return DbAccess.SaveData<dynamic>(sql, new { scenario.ScenarioName, scenario.ScenarioGuid, scenario.RouteId });
      }

    public static int UpdateScenario(ScenarioModel scenario)
      {
      var sql = "UPDATE OR IGNORE Scenarios SET ScenarioName=@ScenarioName, ScenarioGuid=@ScenarioGuid, RouteId=@RouteId " +
                $"WHERE Id= @Id; {DbAccess.LastRowInsertQuery}";
      return DbAccess.SaveData<dynamic>(sql, new { scenario.ScenarioName, scenario.ScenarioGuid, scenario.RouteId, scenario.Id });
      }

    public static void DeleteScenario(int id)
      {
      var sql = "DELETE FROM Scenarios WHERE Id=@id";
      DbAccess.SaveData<dynamic>(sql, new { id });
      }
    }
  }
