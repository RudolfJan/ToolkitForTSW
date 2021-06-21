using SQLiteDatabase.Library;
using System;
using System.Collections.Generic;
using System.Text;
using ToolkitForTSW.Models;

namespace ToolkitForTSW.DataAccess
  {
  public class EngineIniWorkSetDataAccess
    {
    public static List<EngineIniWorkSetModel> GetAllEngineIniWorkSets()
      {
      var sql = "SELECT * FROM EngineIniWorkSets";
      return DbAccess.LoadData<EngineIniWorkSetModel, dynamic>(sql, new { });
      }

    public static int InsertEngineIniWorkSet(EngineIniWorkSetModel workSet)
      {
      var sql = $"INSERT OR IGNORE INTO EngineIniWorkSets (WorkSetName, WorkSetDescription) " +
            $"VALUES(@WorkSetName, @WorkSetDescription);{DbAccess.LastRowInsertQuery}";
      return DbAccess.SaveData<dynamic>(sql, new { workSet.WorkSetName, workSet.WorkSetDescription});
      }

    public static int UpdateEngineIniWorkSet(EngineIniWorkSetModel workSet)
      {
      var sql = "UPDATE OR IGNORE EngineIniWorkSets SET WorkSetName=@WorkSetName, WorkSetDescription=@WorkSetDescription " +
                "WHERE Id = @Id;" +
                $"{DbAccess.LastRowInsertQuery}";
      return DbAccess.SaveData<dynamic>(sql, new {workSet.WorkSetName, workSet.WorkSetDescription, workSet.Id });
      }

    public static void DeleteEngineIniWorkSet(int id)
      {
      var sql = "DELETE FROM EngineIniWorkSets WHERE Id=@id";
      DbAccess.SaveData<dynamic>(sql, new { id });
      }

    }
  }
