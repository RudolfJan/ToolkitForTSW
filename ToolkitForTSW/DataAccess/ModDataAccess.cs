using SQLiteDatabase.Library;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ToolkitForTSW.Models;

namespace ToolkitForTSW.DataAccess
  {
  public class ModDataAccess
    {
    public static List<ModModel> GetAllMods()
      {
      var sql = "SELECT * FROM Mods";
      return DbAccess.LoadData<ModModel, dynamic>(sql, new { });
      }

    public static ModModel GetModByFilePath(string path)
      {
      var sql = "SELECT * FROM Mods WHERE FilePath=@path";
      return DbAccess.LoadData<ModModel, dynamic>(sql, new {path }).FirstOrDefault();
      }

    public static int InsertMod(ModModel mod)
      {
      var sql = $"INSERT OR IGNORE INTO Mods (ModName, FileName, FilePath, ModDescription, ModImage, ModSource, ModType, DLCName) " +
                $"VALUES(@ModName, @FileName, @FilePath, @ModDescription, @ModImage, @ModSource, @ModType, @DLCName);{DbAccess.LastRowInsertQuery}";
      return DbAccess.SaveData<dynamic>(sql, new { mod.ModName, mod.FileName,mod.FilePath, mod.ModDescription, mod.ModImage, mod.ModSource, mod.ModType, mod.DLCName});
      }

    public static int UpdateMod(ModModel mod)
      {
      // $"UPDATE OR IGNORE Persons SET FirstName=@FirstName, LastName=@LastName WHERE Id= @Id;
      var sql = "UPDATE OR IGNORE Mods SET ModName=@ModName, FileName=@FileName, Filepath=@FilePath, ModDescription=@ModDescription, ModImage=@ModImage, ModSource=@ModSource, ModType=@ModType, DLCName= @DLCName " +
                $"WHERE Id= @Id; {DbAccess.LastRowInsertQuery}";
      return DbAccess.SaveData<dynamic>(sql, new { mod.ModName, mod.FileName, mod.FilePath, mod.ModDescription, mod.ModImage, mod.ModSource, mod.ModType, mod.DLCName, mod.Id });
      }

    public static ModModel UpsertMod(string filePath)
      {
      var mod = ModDataAccess.GetModByFilePath(filePath);
      if (mod == null)
        {
        mod = new ModModel
          {
          FilePath = filePath,
          FileName = Path.GetFileName(filePath)
          };
        ModDataAccess.InsertMod(mod);
        return ModDataAccess.GetModByFilePath(filePath);
        }
      return mod;
      }

    public static ModModel UpsertMod(ModModel mod)
      {
      var mod2 = ModDataAccess.GetModByFilePath(mod.FilePath);
      if (mod2 == null)
        {
        ModDataAccess.InsertMod(mod);
        return mod;
        };
      UpdateMod(mod);
      return mod;
      }


    public static void DeleteMod(int id)
      {
      var sql = "DELETE FROM Mods WHERE Id=@id";
      DbAccess.SaveData<dynamic>(sql, new { id });
      }
    }
  }
