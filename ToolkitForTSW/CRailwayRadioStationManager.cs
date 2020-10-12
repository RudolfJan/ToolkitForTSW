using Styles.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ToolkitForTSW
  {
  public class CRailwayRadioStationManager : Notifier
    {
    private DataSet _RadioStationDataSet;

    public DataSet RadioStationDataSet
      {
      get { return _RadioStationDataSet; }
      set
        {
        _RadioStationDataSet = value;
        OnPropertyChanged("RadioStationDataSet");
        }
      }

    private String _Result = String.Empty;

    public String Result
      {
      get { return _Result; }
      set
        {
        _Result = value;
        OnPropertyChanged("Result");
        }
      }

    private String RadioStationsTableName = "RadioStations";

    public CRailwayRadioStationManager()
      {
      Initialize();
      }

    private void CreateDatabase()
      {
      try
        {
        var Db = new CDatabase();
        var RadioStationTableCreateStatement = @"CREATE TABLE ""RadioStations"" (

        ""Id""  INTEGER,
        ""Url"" TEXT,
        ""Route"" TEXT,
        ""Description"" TEXT,
        PRIMARY KEY(""Id"")
          ))";

        Db.CreateTable(RadioStationsTableName, RadioStationTableCreateStatement);
        }
      catch (Exception E)
        {
        Result += CLog.Trace("Error creating Radio stations database because " + E.Message);
        }
      }

    private void CreateDataSet()
      {
      var Db = new CDatabase();

      var ColumnSet =
        "Id, Url, Route, Description";
      RadioStationDataSet = Db.BuildDataSet(RadioStationsTableName, ColumnSet);
      }

    public void Initialize()
      {
      CreateDatabase();
      CreateDataSet();
      }

    // Note: FieldList must have format  Field="Value", Field2="Value2"
    public void UpdateRadioStationTableFields(Int64 RowId, String FieldList)
      {
      var Db = new CDatabase();

      try
        {
        using (var DbConnection = new SQLiteConnection(Db.ConnectionString))
          {
          DbConnection.Open();
          using (var DbCommand = DbConnection.CreateCommand())
            {
            try
              {
              //"UPDATE YOURTABLE SET FIELD1= @FIELD1, FIELD2= @FIELD2   WHERE ID = @ID"
              DbCommand.CommandText =
                "BEGIN TRANSACTION; UPDATE OR IGNORE RadioStations SET " + FieldList +
                " WHERE ID=" +
                RowId + "; COMMIT;";
              DbCommand.CommandType = CommandType.Text;
              DbCommand.ExecuteNonQuery();
              }
            catch (SQLiteException E)
              {
              using (var RollbackCommand = new SQLiteCommand("ROLLBACK;", DbConnection))
                {
                RollbackCommand.ExecuteNonQuery();
                }

              Result += CLog.Trace("Failed to update database " + FieldList + " " + E.Message,
                LogEventType.Error);
              }
            }
          }
        }
      catch (Exception E)
        {
        Result += CLog.Trace(
          "Failed to add radio station to database " + FieldList + " " + E.Message,
          LogEventType.Error);
        }
      }

    public void AddRadioStation(String MyUrl, String MyRoute, String MyDescription)
      {
      var Db = new CDatabase();
      try
        {
        using (var DbConnection = new SQLiteConnection(Db.ConnectionString))
          {
          DbConnection.Open();
          using (var DbCommand = DbConnection.CreateCommand())
            {
            try
              {
              DbCommand.CommandText =
                "BEGIN TRANSACTION; INSERT OR IGNORE INTO RadioStations (Url, Route, Description) VALUES (@Url, @Route, @Description); COMMIT;";
              DbCommand.Parameters.Add(new SQLiteParameter("@Url") {Value = MyUrl});
              DbCommand.Parameters.Add(new SQLiteParameter("@Route") {Value = MyRoute});
              DbCommand.Parameters.Add(new SQLiteParameter("@Description") {Value = MyDescription});
              DbCommand.CommandType = CommandType.Text;
              DbCommand.ExecuteNonQuery();
              Result += "RadioStation entry added for route " + MyRoute + "\r\n";
              }
            catch (SQLiteException E)
              {
              using (var RollbackCommand = new SQLiteCommand("ROLLBACK;", DbConnection))
                {
                RollbackCommand.ExecuteNonQuery();
                }

              Result += CLog.Trace("Failed to add radio station to database " + E.Message,
                LogEventType.Error);
              }
            }
          }
        }
      catch (Exception E)
        {
        Result += CLog.Trace("Failed to add radio station to database " + E.Message,
          LogEventType.Error);
        }
      }

    public void DeleteRadioStation(Int64 Id)
      {
      var Db = new CDatabase();
      try
        {
        using (var DbConnection = new SQLiteConnection(Db.ConnectionString))
          {
          DbConnection.Open();
          using (var DbCommand = DbConnection.CreateCommand())
            {
            try
              {
              DbCommand.CommandText =
                "BEGIN TRANSACTION; DELETE FROM RadioStations WHERE Id=@Id; COMMIT;";
              DbCommand.Parameters.Add(new SQLiteParameter("@Id") { Value = Id });
              DbCommand.CommandType = CommandType.Text;
              DbCommand.ExecuteNonQuery();
              Result += "RadioStation entry deleted " + "\r\n";
              }
            catch (SQLiteException E)
              {
              using (var RollbackCommand = new SQLiteCommand("ROLLBACK;", DbConnection))
                {
                RollbackCommand.ExecuteNonQuery();
                }

              Result += CLog.Trace("Failed to delete radio station entry" + E.Message,
                LogEventType.Error);
              }
            }
          }
        }
      catch (Exception E)
        {
        Result += CLog.Trace("Failed to add radio station to database " + E.Message,
          LogEventType.Error);
        }
      }


    }
  }
