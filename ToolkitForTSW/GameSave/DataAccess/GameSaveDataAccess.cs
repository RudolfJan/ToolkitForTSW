using SQLiteDatabase.Library;
using System.Collections.Generic;
using ToolkitForTSW.GameSave.Models;

namespace ToolkitForTSW.GameSave.DataAccess
  {
  public class GameSaveDataAccess
    {
    public static List<GameSaveModel> GetAllGameSaves()
      {
      var sql = "SELECT * FROM GameSaves";
      return DbAccess.LoadData<GameSaveModel, dynamic>(sql, new { });
      }

    public static int InsertGameSave(GameSaveModel gameSave)
      {
      var sql = $"INSERT OR IGNORE INTO GameSaves (Activity, SaveName, Description, RouteAbbreviation) " +
          $"VALUES(@Activity, @SaveName, @Description, @RouteAbbreviation);{DbAccess.LastRowInsertQuery}";
      return DbAccess.SaveData<dynamic>(sql, new { gameSave.Activity, gameSave.SaveName, gameSave.Description, gameSave.RouteAbbreviation });
      }

    public static int UpdateGameSave(GameSaveModel gameSave)
      {
      // $"UPDATE OR IGNORE Persons SET FirstName=@FirstName, LastName=@LastName WHERE Id= @Id;
      var sql = "UPDATE OR IGNORE GameSaves SET Activity=@Activity, SaveName=@SaveName, Description=@Description, RouteAbbreviation=@RouteAbbreviation " +
                $"WHERE Id= @Id; {DbAccess.LastRowInsertQuery}";
      return DbAccess.SaveData<dynamic>(sqlStatement: sql, new { gameSave.Activity, gameSave.SaveName, gameSave.Description, gameSave.RouteAbbreviation, gameSave.Id });
      }

    public static void DeleteGameSave(int id)
      {
      var sql = "DELETE FROM GameSaves WHERE Id=@id";
      DbAccess.SaveData<dynamic>(sql, new { id });
      }
    }
  }
