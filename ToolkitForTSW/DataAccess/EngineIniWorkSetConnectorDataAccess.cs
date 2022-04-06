using SQLiteDatabase.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToolkitForTSW.Models;

namespace ToolkitForTSW.DataAccess
  {
  public class EngineIniWorkSetConnectorDataAccess
    {
    public static List<EngineIniWorkSetConnectorModel> GetAllEngineIniSettings()
      {
      var sql = "SELECT * FROM EngineIniWorkSetConnectors";
      return DbAccess.LoadData<EngineIniWorkSetConnectorModel, dynamic>(sql, new { });
      }

    public static int CheckEngineIniSettingsInWorkSet(int settingId, int worksetId)
      {
      var sql = "SELECT Count() " +
        "FROM EngineIniWorkSetConnectors " +
        "WHERE EngineIniWorkSetConnectors.EngineIniSettingId = @settingId " +
        "AND EngineIniWorkSetConnectors.EngineIniWorkSetId= @worksetId;";
      return DbAccess.LoadData<int, dynamic>(sql, new { settingId, worksetId }).First();
      }

    public static int InsertEngineIniWorkSetConnector(EngineIniWorkSetConnectorModel workSetConnector)
      {
      var sql = $"INSERT OR IGNORE INTO EngineIniWorkSetConnectors (EngineIniSettingId, EngineIniWorkSetId) " +
            $"VALUES(@EngineIniSettingId, @EngineIniWorkSetId);{DbAccess.LastRowInsertQuery}";
      return DbAccess.SaveData<dynamic>(sql, new { workSetConnector.EngineIniSettingId, workSetConnector.EngineIniWorkSetId });
      }

    public static int UpdateEngineIniWorkSetConnector(EngineIniWorkSetConnectorModel workSetConnector)
      {
      var sql = "UPDATE OR IGNORE EngineIniWorkSetConnectors SET EngineIniSettingId=@EngineIniSettingId, EngineIniWorkSetId=@EngineIniWorkSetId, " +
                "WHERE Id = @Id;" +
                $"{DbAccess.LastRowInsertQuery}";
      return DbAccess.SaveData<dynamic>(sql, new { workSetConnector.EngineIniSettingId, workSetConnector.EngineIniWorkSetId, workSetConnector.Id });
      }

    public static void DeleteEngineIniWorkSetConnector(int id)
      {
      var sql = "DELETE FROM EngineIniWorkSetConnectors WHERE Id=@id";
      DbAccess.SaveData<dynamic>(sql, new { id });
      }


    public static void DeleteEngineIniWorkSetConnectorByParticipants(int workSetId, int settingId)
      {
      var sql = "DELETE FROM EngineIniWorkSetConnectors WHERE EngineIniWorkSetId=@workSetId AND EngineIniSettingId= @settingId";
      DbAccess.SaveData<dynamic>(sql, new { workSetId, settingId });
      }

    public static void DeleteEngineIniWorkSetConnectorByWorkSet(int workSetId)
      {
      var sql = "DELETE FROM EngineIniWorkSetConnectors WHERE EngineIniWorkSetId=@workSetId";
      DbAccess.SaveData<dynamic>(sql, new { workSetId });
      }
    }
  }
