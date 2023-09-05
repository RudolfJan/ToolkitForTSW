using SQLiteDatabase.Library;
using System.Collections.Generic;
using System.Linq;
using ToolkitForTSW.Models;

namespace ToolkitForTSW.DataAccess
  {

  public class ModSetDataAccess
    {
    public static List<ModSetModel> GetAllModSets()
      {
      var sql = "SELECT * FROM ModSets";
      return DbAccess.LoadData<ModSetModel, dynamic>(sql, new { });
      }

    public static ModSetModel GetModSetById(int id)
      {
      var sql = "SELECT * FROM ModSets WHERE Id=@id";
      return DbAccess.LoadData<ModSetModel, dynamic>(sql, new { id }).FirstOrDefault();
      }

    public static List<ModSetModel> GetAllLiverySetsPerRoute(int routeId)
      {
      var sql = "SELECT * FROM ModSets WHERE RouteId=@routeId";
      return DbAccess.LoadData<ModSetModel, dynamic>(sql, new { routeId });
      }

    public static int InsertModSet(ModSetModel modSet)
      {
      if (modSet.RouteId == 0)
        {
        var sql = $"INSERT OR IGNORE INTO ModSets (ModSetName, ModSetDescription, RouteId) " +
          $"VALUES(@ModSetName, @ModSetDescription, NULL);{DbAccess.LastRowInsertQuery}";

        return DbAccess.SaveData<dynamic>(sql, new { modSet.ModSetName, modSet.ModSetDescription });
        }
      else
        {
        var sql = $"INSERT OR IGNORE INTO ModSets (ModSetName, ModSetDescription, RouteId) " +
                  $"VALUES(@ModSetName, @ModSetDescription, @RouteId);{DbAccess.LastRowInsertQuery}";

        return DbAccess.SaveData<dynamic>(sql, new { modSet.ModSetName, modSet.ModSetDescription, modSet.RouteId });
        }
      }

    public static int UpdateModSet(ModSetModel modSet)
      {
      // $"UPDATE OR IGNORE Persons SET FirstName=@FirstName, LastName=@LastName WHERE Id= @Id;
      var sql = "UPDATE OR IGNORE ModSets SET ModSetName=@ModSetName, ModSetDescription=@ModSetDescription, RouteId=@RouteId " +
                $"WHERE Id= @Id; {DbAccess.LastRowInsertQuery}";
      return DbAccess.SaveData<dynamic>(sql, new { modSet.ModSetName, modSet.ModSetDescription, modSet.RouteId, modSet.Id });
      }

    public static void DeleteModSet(int id)
      {
      var sql = "DELETE FROM ModSets WHERE Id=@id";
      DbAccess.SaveData<dynamic>(sql, new { id });
      }

    public static void DeleteAllModSets()
      {
      var sql = "DELETE FROM ModSets";
      DbAccess.SaveData<dynamic>(sql, new { });
      }
    }
  }
