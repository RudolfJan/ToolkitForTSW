using SQLiteDatabase.Library;
using System.Collections.Generic;
using ToolkitForTSW.Models;

namespace ToolkitForTSW.DataAccess
  {
  public class RadioStationDataAccess
    {
    public static List<RadioStationModel> GetAllRadioStations()
      {
      var sql = "SELECT * FROM RadioStations";
      return DbAccess.LoadData<RadioStationModel, dynamic>(sql, new { });
      }

    public static int InsertRadioStation(RadioStationModel radioStation)
      {
      var sql = $"INSERT OR IGNORE INTO RadioStations (Url, RouteName, Description) VALUES(@Url, @RouteName, @Description);{DbAccess.LastRowInsertQuery}";
      return DbAccess.SaveData<dynamic>(sql, new {radioStation.Url, radioStation.RouteName, radioStation.Description });
      }

    public static int UpdateRadioStation(RadioStationModel radioStation)
      {
      // $"UPDATE OR IGNORE Persons SET FirstName=@FirstName, LastName=@LastName WHERE Id= @Id;
      var sql = $"UPDATE OR IGNORE RadioStations SET Url=@Url, RouteName=@RouteName, Description=@Description WHERE Id= @Id; {DbAccess.LastRowInsertQuery}";
      return DbAccess.SaveData<dynamic>(sql, new { radioStation.Url, radioStation.RouteName, radioStation.Description, radioStation.Id});
      }

    public static void DeleteRadioStation(int id)
      {
      var sql = "DELETE FROM RadioStations WHERE Id=@id";
      DbAccess.SaveData<dynamic>(sql, new { id });
      }
    }
  }

