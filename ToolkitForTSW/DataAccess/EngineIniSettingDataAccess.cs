using Logging.Library;
using Microsoft.VisualBasic.FileIO;
using SQLiteDatabase.Library;
using System;
using System.Collections.Generic;
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
      return DbAccess.LoadData<EngineIniSettingsModel, dynamic>(sql, new { worksetId});
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
              var setting = new EngineIniSettingsModel();
              setting.SettingName = fields[0];
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



    }
  }
