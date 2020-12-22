using SQLiteDatabase.Library;
using System.Collections.Generic;
using ToolkitForTSW.Models;

namespace ToolkitForTSW.DataAccess
  {
  public class LiveryDataAccess
    {
    public static List<LiveryModel> GetAllLiveries()
      {
      var sql = "SELECT * FROM Liveries";
      return DbAccess.LoadData<LiveryModel, dynamic>(sql, new { });
      }

    public static List<LiveryModel> GetAllLiveriesPerRoute(int routeId)
      {
      var sql = "SELECT * FROM Liveries WHERE RouteId=@routeId";
      return DbAccess.LoadData<LiveryModel, dynamic>(sql, new {routeId });
      }

    public static int InsertLivery(LiveryModel livery)
      {
      var sql = $"INSERT OR IGNORE INTO Liveries (LiveryName, FileName, LiveryDescription, LiveryImage, LiverySource, LiveryType, RouteId) " +
                $"VALUES(@LiveryName, @FileName, @LiveryDescription, @LiveryImage, @LiverySource, @LiveryType, @RouteId);{DbAccess.LastRowInsertQuery}";
      return DbAccess.SaveData<dynamic>(sql, new { livery.LiveryName, livery.FileName, livery.LiveryDescription, livery.LiveryImage, livery.LiverySource, livery.LiveryType, livery.RouteId });
      }

    public static int UpdateLivery(LiveryModel livery)
      {
      // $"UPDATE OR IGNORE Persons SET FirstName=@FirstName, LastName=@LastName WHERE Id= @Id;
      var sql = "UPDATE OR IGNORE Liveries SET LiveryName=@LiveryName, FileName=@FileName, LiveryDescription=@LiveryDescription, LiveryImage=@LiveryImage, LiverySource=@LiverySource, LiveryType=@LiveryType, RouteId=@RouteId, " +
                $"WHERE Id= @Id; {DbAccess.LastRowInsertQuery}";
      return DbAccess.SaveData<dynamic>(sql, new { livery.LiveryName, livery.FileName, livery.LiveryDescription, livery.LiveryImage, livery.LiverySource, livery.LiveryType, livery.RouteId, livery.Id });
      }

    public static void DeleteLivery(int id)
      {
      var sql = "DELETE FROM Liveries WHERE Id=@id";
      DbAccess.SaveData<dynamic>(sql, new { id });
      }
    }
  }
