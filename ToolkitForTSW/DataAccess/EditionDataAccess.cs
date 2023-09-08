using CsvHelper;
using SQLiteDatabase.Library;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ToolkitForTSW.Models;

namespace ToolkitForTSW.DataAccess
  {
  public class EditionDataAccess
    {
    public static List<EditionModel> GetAllEditions()
      {
      var sql = "SELECT * FROM Editions";
      return DbAccess.LoadData<EditionModel, dynamic>(sql, new { });
      }

    public static int InsertEdition(EditionModel edition)
      {
      var sql = $@"INSERT OR IGNORE INTO Editions (EditionOrder, EditionName, EditionLongName, SteamGameId, 
                      SteamprogramPath, EGSProgramPath, Selected, Description) " +
                      $@"VALUES(@EditionOrder, @EditionName, @EditionLongName, @SteamGameId, @SteamProgramPath, 
                      @EGSProgramPath, @Selected, @Description);{DbAccess.LastRowInsertQuery}";
      return DbAccess.SaveData<dynamic>(sql, new
        {
        edition.EditionOrder,
        edition.EditionName,
        edition.EditionLongName
      ,
        edition.SteamGameId,
        edition.SteamProgramPath,
        edition.EGSProgramPath,
        edition.Selected,
        edition.Description
        });
      }

    public static int UpdateEdition(EditionModel edition)
      {
      // $"UPDATE OR IGNORE Persons SET FirstName=@FirstName, LastName=@LastName WHERE Id= @Id;
      var sql = $@"UPDATE OR IGNORE Editions SET EditionOrder=@EditionOrder, EditionName=@EditionName, 
                      EditionLongName=@EditionLongName, SteamGameId=@SteamGameId, SteamProgramPath=@SteamProgramPath, 
                      EGSProgramPath=@EGSProgramPath, Selected=@Selected, Description=@Description WHERE Id= @Id; {DbAccess.LastRowInsertQuery}";
      return DbAccess.SaveData<dynamic>(sql, new { edition.EditionOrder, edition.EditionName, edition.SteamGameId, edition.SteamProgramPath, edition.EGSProgramPath, edition.Selected, edition.Description });
      }

    public static void DeleteEdition(int id)
      {
      var sql = "DELETE FROM Editions WHERE Id=@id";
      DbAccess.SaveData<dynamic>(sql, new { id });
      }

    public static int UpdateSelected(string editionName)
      {
      var sql = "UPDATE Editions SET Selected = 1 WHERE EditionName = @editionName; ";
      return DbAccess.SaveData<dynamic>(sql, new { editionName });
      }

    public static void LoadEditionsFromCSV()
      {
      var editionsFile = "SQL\\Editions.csv";
      List<EditionModel> editions = new();
      using var reader = new StreamReader(editionsFile);
      using var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
      var records = csv.GetRecords<EditionModel>();

      foreach (var edition in records)
        {
        InsertEdition(edition);
        }
      }

    internal static EditionModel GetActiveEdition()
      {
      var sql = "SELECT * FROM Editions WHERE Selected=1";
      return DbAccess.LoadData<EditionModel, dynamic>(sql, new { }).FirstOrDefault();
      }

    internal static int GetActiveEditionId()
      {
      var sql = "SELECT Id FROM Editions WHERE Selected=1";
      return DbAccess.LoadData<int, dynamic>(sql, new { }).FirstOrDefault();
      }


    internal static int SetSelectedEdition(int editionId)
      {
      var sql = "BEGIN TRANSACTION; UPDATE Editions SET Selected=0 WHERE Selected=1; UPDATE Editions SET Selected=1 WHERE Id=@editionId; COMMIT";
      return DbAccess.SaveData<dynamic>(sql, new { editionId });
      }
    }
  }

