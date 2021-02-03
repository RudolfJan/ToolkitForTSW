using SQLiteDatabase.Library;
using System;
using System.Collections.Generic;
using System.Text;
using ToolkitForTSW.Models;

namespace ToolkitForTSW.DataAccess
  {
  public class ModModSetDataAccess
    {
    //public int Id { get; set; }
    //public int LiveryId { get; set; }
    //public int LiverySetId { get; set; }
    public static List<ModModSetModel> GetAllModModSets()
      {
      var sql = "SELECT * FROM ModModSet";
      return DbAccess.LoadData<ModModSetModel, dynamic>(sql, new { });
      }

    public static List<ModModSetModel> GetAllModModSetsPerMod(int modId)
      {
      var sql = "SELECT * FROM ModModSet WHERE ModId=@modId";
      return DbAccess.LoadData<ModModSetModel, dynamic>(sql, new { modId });
      }

    public static List<ModModSetModel> GetAllModModSetsPerModSet(int modSetId)
      {
      var sql = "SELECT * FROM ModModSet WHERE ModSetId=@modSetId";
      return DbAccess.LoadData<ModModSetModel, dynamic>(sql, new { modSetId });
      }
    public static int InsertModModSet(ModModSetModel modModSet)
      {
      var sql = $"INSERT OR IGNORE INTO ModModSet (ModId, ModSetId) " +
                $"VALUES(@ModId, @ModSetId);{DbAccess.LastRowInsertQuery}";
      return DbAccess.SaveData<dynamic>(sql, new { modModSet.ModId, modModSet.ModSetId});
      }

    public static List<ModModel> GetAllModsPerModSet(int modSetId)
      {
      var sql = "SELECT Mods.Id, Mods.ModName, Mods.FileName, Mods.FilePath, Mods.ModDescription, Mods.ModImage, Mods.ModSource, Mods.ModType, Mods.DLCName FROM Mods, ModModSet " +
                "WHERE Mods.Id=ModModSet.ModId AND ModModSet.ModSetId= @modSetId;";
      return DbAccess.LoadData<ModModel, dynamic>(sql, new { modSetId});
      }

    public static int UpdateModModSet(ModModSetModel modModSet)
      {
      // $"UPDATE OR IGNORE Persons SET FirstName=@FirstName, LastName=@LastName WHERE Id= @Id;
      var sql = "UPDATE OR IGNORE ModModSet SET ModId=@ModId, ModSetId=@ModSetId " +
                $"WHERE Id= @Id;{DbAccess.LastRowInsertQuery}";
      return DbAccess.SaveData<dynamic>(sql, new { modModSet.ModId, modModSet.ModSetId, modModSet.Id });
      }

    public static void DeleteModModSet(int id)
      {
      var sql = "DELETE FROM ModModSet WHERE Id=@id";
      DbAccess.SaveData<dynamic>(sql, new { id });
      }

    internal static void DeleteModFromModSet(int modId, int modSetId)
      {
      var sql = "DELETE FROM ModModSet WHERE ModId=@modId AND ModSetId=@modSetId";
      DbAccess.SaveData<dynamic>(sql, new { modId, modSetId });
      }
    }
  }
