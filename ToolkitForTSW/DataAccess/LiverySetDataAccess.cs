using SQLiteDatabase.Library;
using System.Collections.Generic;
using ToolkitForTSW.Models;

namespace ToolkitForTSW.DataAccess
  {

  public class LiverySetDataAccess
    {
    public static List<LiverySetModel> GetAllLiverySets()
      {
      var sql = "SELECT * FROM LiverySets";
      return DbAccess.LoadData<LiverySetModel, dynamic>(sql, new { });
      }

    public static List<LiverySetModel> GetAllLiverySetsPerRoute(int routeId)
      {
      var sql = "SELECT * FROM LiverySets WHERE RouteId=@routeId";
      return DbAccess.LoadData<LiverySetModel, dynamic>(sql, new { routeId });
      }

    public static int InsertLiverySet(LiverySetModel liverySet)
      {
      var sql = $"INSERT OR IGNORE INTO LiverySets (LiverySetName, LiverySetDescription, RouteId) " +
                $"VALUES(@LiverySetName, @LiverySetDescription, @RouteId);{DbAccess.LastRowInsertQuery}";
      return DbAccess.SaveData<dynamic>(sql, new { liverySet.LiverySetName, liverySet.LiverySetDescription, liverySet.RouteId });
      }

    public static int UpdateLiverySet(LiverySetModel liverySet)
      {
      // $"UPDATE OR IGNORE Persons SET FirstName=@FirstName, LastName=@LastName WHERE Id= @Id;
      var sql = "UPDATE OR IGNORE Liveries SET LiverySetName=@LiverySetName, LiverySetDescription=@LiverySetDescription, RouteId=@RouteId, " +
                $"WHERE Id= @Id; {DbAccess.LastRowInsertQuery}";
      return DbAccess.SaveData<dynamic>(sql, new { liverySet.LiverySetName, liverySet.LiverySetDescription, liverySet.RouteId, liverySet.Id });
      }

    public static void DeleteLiverySet(int id)
      {
      var sql = "DELETE FROM LiverySets WHERE Id=@id";
      DbAccess.SaveData<dynamic>(sql, new { id });
      }
    }
  }
