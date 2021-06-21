using ClosedXML.Excel;
using Logging.Library;
using Microsoft.VisualBasic.FileIO;
using SQLiteDatabase.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ToolkitForTSW.Models;

namespace ToolkitForTSW.DataAccess
  {
  public class EngineIniSettingDataAccess
    {
    public static List<EngineIniSettingsModel> GetAllEngineIniSettings()
      {
      var sql = "SELECT * FROM EngineIniSettings";
      return DbAccess.LoadData<EngineIniSettingsModel, dynamic>(sql, new { });
      }

    public static List<EngineIniSettingsModel> GetEngineIniSettingsInWorkSet(int worksetId)
      {
      var sql = "SELECT EngineIniSettings.Id, EngineIniSettings.SettingName, EngineIniSettings.SettingDescription, " +
        "EngineIniSettings.MinValue, EngineIniSettings.MaxValue, EngineIniSettings.DefaultValue, EngineIniSettings.ValueType " +
        "FROM EngineIniSettings, EngineIniWorkSetConnectors " +
        "WHERE EngineIniSettings.Id = EngineIniWorkSetConnectors.EngineIniSettingId " +
        "AND EngineIniWorkSetConnectors.EngineIniWorkSetId= @worksetId;";
      return DbAccess.LoadData<EngineIniSettingsModel, dynamic>(sql, new { worksetId });
      }

    public static string GetEngineIniSettingDescriptionById(int settingId)
      {
      var sql = "SELECT EngineIniSettings.SettingDescription " +
          "FROM EngineIniSettings " +
           "WHERE EngineIniSettings.Id = @settingId;";
      return DbAccess.LoadData<string, dynamic>(sql, new { settingId }).FirstOrDefault();
      }

   
    public static int InsertEngineIniSetting(EngineIniSettingsModel setting)
      {
      var sql = $"INSERT OR IGNORE INTO EngineIniSettings (SettingName, SettingDescription, MinValue, MaxValue, DefaultValue, ValueType) " +
            $"VALUES(@SettingName, @SettingDescription, @MinValue, @MaxValue, @DefaultValue, @ValueType);{DbAccess.LastRowInsertQuery}";
      return DbAccess.SaveData<dynamic>(sql, new { setting.SettingName, setting.SettingDescription, setting.MinValue, setting.MaxValue, setting.DefaultValue, setting.ValueType });
      }

    public static int UpdateEngineIniSetting(EngineIniSettingsModel setting)
      {
      var sql = "UPDATE OR IGNORE EngineIniSettings SET SettingName=@SettingName, SettingDescription=@SettingDescription, " +
                "MinValue=@MinValue, MaxValue=@MaxValue, DefaultValue=@DefaultValue, ValueType=@ValueType) " +
                "WHERE Id = @Id;" +
                $"{DbAccess.LastRowInsertQuery}";
      return DbAccess.SaveData<dynamic>(sql, new { setting.SettingName, setting.SettingDescription, setting.MinValue, setting.MaxValue, setting.DefaultValue, setting.ValueType, setting.Id });
      }

    private static int UpdateEngineIniDescription(string settingName, string settingDescription)
      {
      var sql = "UPDATE OR IGNORE EngineIniSettings SET SettingDescription=@settingDescription " +
                "WHERE SettingName=@SettingName;" +
                $"{DbAccess.LastRowInsertQuery}";
      return DbAccess.SaveData<dynamic>(sql, new { settingName, settingDescription});
      }

    public static void DeleteEngineIniSetting(int id)
      {
      var sql = "DELETE FROM EngineIniSettings WHERE Id=@id";
      DbAccess.SaveData<dynamic>(sql, new { id });
      }

    public static void ImportEngineIniSettingsFromCsv(string settingsCsvFilePath)
      {
      // https://stackoverflow.com/questions/5282999/reading-csv-file-and-storing-values-into-an-array

      try
        {
        using (TextFieldParser csvParser = new TextFieldParser(settingsCsvFilePath))
          {
          csvParser.CommentTokens = new string[] { "#" };
          csvParser.SetDelimiters(new string[] { "," });
          csvParser.HasFieldsEnclosedInQuotes = true;

          // Skip the row with the column names
          csvParser.ReadLine();

          while (!csvParser.EndOfData)
            {
            // Read current line fields, pointer moves to the next line.
            string[] fields = csvParser.ReadFields();
            int length = fields.GetLength(0);

            if (length > 0)
              {
              var setting = new EngineIniSettingsModel
                {
                SettingName = fields[0]
                };
              InsertEngineIniSetting(setting);
              }
            }
          }
        }
      catch (Exception ex)
        {
        Log.Trace($"Error reading Experimental Settings import {ex.Message}");
        }
      }

    public static void ImportDescriptionsFromExcel(string fileName)
      {
      if (!File.Exists(fileName))
        {
        Log.Trace("Annotation file does not exist", LogEventType.Error);
        return;
        }
      using (var wb = new XLWorkbook(fileName))
        {
        try
          {
          var ws = wb.Worksheet("Settings");
          var settingName = "";
          var settingDescription = "";
          var row = 1;
          var rangeUsed= ws.RangeUsed();
          int maxRows = rangeUsed.LastRowUsed().RowNumber();
          while (row < maxRows)
            {
            var range = ws.MergedRanges.FirstOrDefault(r => r.Contains(ws.Cell(row, 1)));
            if (range == null)
              {
              var address = ws.Cell(row, 1).Address;
              range = ws.Range(address, address);
              }
            settingName = ws.Cell(row, 1).Value.ToString();
            settingDescription=string.Empty;
            foreach (var cell in range.Cells())
              {
              settingDescription += cell.CellRight().Value.ToString() + "\r\n";
              }
            UpdateEngineIniDescription(settingName,settingDescription);
            row += range.Cells().Count();
            }
          }
        catch (Exception ex)
          {
          Log.Trace("Exception while reading annotation file", ex, LogEventType.Error);
          }
        }
      }
    }
  }
