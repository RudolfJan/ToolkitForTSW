using SQLiteDatabase.Library;
using System;
using System.Collections.Generic;
using System.Text;
using ToolkitForTSW.Models;

namespace ToolkitForTSW.DataAccess
  {
  public class LiveryLiveryLiverySetDataAccess
    {
    //public int Id { get; set; }
    //public int LiveryId { get; set; }
    //public int LiverySetId { get; set; }
    public static List<LiveryLiverySetModel> GetAllLiveryLiverySets()
      {
      var sql = "SELECT * FROM LiveryLiverySets";
      return DbAccess.LoadData<LiveryLiverySetModel, dynamic>(sql, new { });
      }

    public static List<LiveryLiverySetModel> GetAllLiveryLiverySetsPerLivery(int liveryId)
      {
      var sql = "SELECT * FROM LiveryLiverySets WHERE LiveryId=@liveryId";
      return DbAccess.LoadData<LiveryLiverySetModel, dynamic>(sql, new { liveryId });
      }

    public static List<LiveryLiverySetModel> GetAllLiveryLiverySetsPerLiverySet(int liverySetId)
      {
      var sql = "SELECT * FROM LiveryLiverySets WHERE LiverySetId=@liverySetId";
      return DbAccess.LoadData<LiveryLiverySetModel, dynamic>(sql, new { liverySetId });
      }
    public static int InsertLiveryLiverySet(LiveryLiverySetModel liveryLiverySet)
      {
      var sql = $"INSERT OR IGNORE INTO LiveryLiverySets (LiveryId, LiverySetId) " +
                $"VALUES(@LiveryId, @LiverySetId);{DbAccess.LastRowInsertQuery}";
      return DbAccess.SaveData<dynamic>(sql, new { liveryLiverySet.LiveryId, liveryLiverySet.LiverySetId});
      }

    public static int UpdateLiveryLiverySet(LiveryLiverySetModel liveryLiverySet)
      {
      // $"UPDATE OR IGNORE Persons SET FirstName=@FirstName, LastName=@LastName WHERE Id= @Id;
      var sql = "UPDATE OR IGNORE Liveries SET LiveryId=@LiveryId, LiverySetId=@LiverySetId " +
                $"WHERE Id= @Id;{DbAccess.LastRowInsertQuery}";
      return DbAccess.SaveData<dynamic>(sql, new { liveryLiverySet.LiveryId, liveryLiverySet.LiverySetId, liveryLiverySet.Id });
      }

    public static void DeleteLiveryLiverySet(int id)
      {
      var sql = "DELETE FROM LiveryLiverySets WHERE Id=@id";
      DbAccess.SaveData<dynamic>(sql, new { id });
      }
    }
  }
