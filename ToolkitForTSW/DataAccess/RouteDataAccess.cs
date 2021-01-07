using Logging.Library;
using Microsoft.VisualBasic.FileIO;
using SavCracker.Library.Models;
using SQLiteDatabase.Library;
using System;
using System.Collections.Generic;
using ToolkitForTSW.Models;

namespace ToolkitForTSW.DataAccess
  {
  public class RouteDataAccess
    {
    public static List<RouteModel> GetAllRoutes()
      {
      var sql = "SELECT * FROM Routes  ORDER BY RouteAbbrev";
      return DbAccess.LoadData<RouteModel, dynamic>(sql, new { });
      }

    // Prepares dataset for SavCracker project
    public static List<SavCrackerRouteModel>
      GetSavCrackerRouteList()
      {
      var sql = "SELECT RouteName, RouteAbbrev, ScenarioPlannerRouteName, ScenarioPlannerRouteString FROM Routes ORDER BY RouteAbbrev";
      return DbAccess.LoadData<SavCrackerRouteModel, dynamic>(sql, new { });
      }

    public static void InitRouteForSavCracker(string savCrackerCsv)
      {
      // https://stackoverflow.com/questions/5282999/reading-csv-file-and-storing-values-into-an-array


      try
        {
        using (TextFieldParser csvParser = new TextFieldParser(savCrackerCsv))
          {
          csvParser.CommentTokens = new string[] {"#"};
          csvParser.SetDelimiters(new string[] {","});
          csvParser.HasFieldsEnclosedInQuotes = true;

          // Skip the row with the column names
          csvParser.ReadLine();

          while (!csvParser.EndOfData)
            {
            // Read current line fields, pointer moves to the next line.
            string[] fields = csvParser.ReadFields();
            int length = fields.GetLength(0);
            RouteModel route = new RouteModel();
            if (length > 0)
              {
              route.ScenarioPlannerRouteName = fields[0];
              }

            if (length > 1)
              {
              route.RouteName = fields[1];
              }

            if (length > 2)
              {
              route.RouteAbbrev = fields[2];
              }

            if (length > 3)
              {
              route.ScenarioPlannerRouteString = fields[3];
              }

            route.RouteDescription = string.Empty;
            route.RouteImagePath = string.Empty;
            InsertRoute(route);
            }
          }
        }
      catch (Exception ex)
        {
        Log.Trace( $"Error reading route import {ex.Message}");
        }
      }

    public static int InsertRoute(RouteModel route)
      {
      var sql = $"INSERT OR IGNORE INTO Routes (RouteName, RouteAbbrev, RouteDescription, RouteImagePath, ScenarioPlannerRouteName, ScenarioPlannerRouteString, DlcId) " +
                $"VALUES(@RouteName, @RouteAbbrev, @RouteDescription, @RouteImagePath, @ScenarioPlannerRouteName, @ScenarioPlannerRouteString, @DlcId);{DbAccess.LastRowInsertQuery}";
      return DbAccess.SaveData<dynamic>(sql, new { route.RouteName, route.RouteAbbrev, route.RouteDescription, route.RouteImagePath, route.ScenarioPlannerRouteName, route.ScenarioPlannerRouteString, route.DlcId });
      }

    public static int UpdateRoute(RouteModel route)
      {
      var sql = "UPDATE OR IGNORE Routes SET RouteName=@RouteName, RouteAbbrev=@RouteAbbrev, RouteDescription=@RouteDescription, RouteImagePath=@RouteImagePath, " +
                "ScenarioPlannerRouteName=@ScenarioPlannerRouteName, ScenarioPlannerRouteString=@ScenarioPlannerRouteString, DlcId=@DlcId " +
                $"WHERE Id= @Id; {DbAccess.LastRowInsertQuery}";
      return DbAccess.SaveData<dynamic>(sql, new { route.RouteName, route.RouteAbbrev, route.RouteDescription, route.RouteImagePath, route.ScenarioPlannerRouteName, route.ScenarioPlannerRouteString, route.DlcId, route.Id});
      }

    public static void DeleteRoute(int id)
      {
      var sql = "DELETE FROM Routes WHERE Id=@id";
      DbAccess.SaveData<dynamic>(sql, new { id });
      }
    }
  }
