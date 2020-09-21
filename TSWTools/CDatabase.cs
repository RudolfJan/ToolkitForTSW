using Styles.Library.Helpers;
using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace TSWTools
{
	public class CDatabase : Notifier
		{
		private String _ConnectionString = String.Empty;

		public String ConnectionString
			{
			get { return _ConnectionString; }
			set
				{
				_ConnectionString = value;
				OnPropertyChanged("ConnectionString");
				}
			}

		/*
		Location of the database
		*/
		private String _DatabasePath = String.Empty;

		public String DatabasePath
			{
			get { return _DatabasePath; }
			set
				{
				_DatabasePath = value;
				OnPropertyChanged("DatabasePath");
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

		public CDatabase()
			{
			DatabasePath = CTSWOptions.TSWToolsFolder + "TSWTools.db";
			ConnectionString = "Data Source=\"" + DatabasePath + "\";Version=3;";
			if (!File.Exists(DatabasePath))
				{
				SQLiteConnection.CreateFile(DatabasePath);
				}
			}

		// https://stackoverflow.com/questions/1601151/how-do-i-check-in-sqlite-whether-a-table-exists
		public Boolean TableExists(String TableName)
			{
			try
				{
				using (var DbConnection = new SQLiteConnection(ConnectionString))
					{
					DbConnection.Open();
					// Create tables
					using (var Command = new SQLiteCommand("SELECT 1 FROM " + TableName + ";", DbConnection))
						{
						Command.ExecuteNonQuery();
						}

					return true;
					}
				}
			catch (SQLiteException)
				{
				return false;
				}
			}

		public void CreateTable(String TableName, String CreateStatement)
			{
			try
				{
				if (!TableExists(TableName))
					{
					using (var DbConnection = new SQLiteConnection(ConnectionString))
						{
						DbConnection.Open();
						// Create tables
						using (var Command = new SQLiteCommand(CreateStatement, DbConnection))
							{
							Command.ExecuteNonQuery();
							}
						}
					}
				}
			catch (Exception E)
				{
				Result += CLog.Trace("Failed to create table " + TableName + "because " + E.Message, LogEventType.Error);
				throw;
				}
			}

		public DataSet BuildDataSet(String TableName, String Columns, Boolean Distinct=false, String Filter = "")
			{
			var DistinctString = String.Empty;
			if (Distinct)
				{
				DistinctString = "DISTINCT ";
				}

      if (Filter.Length > 0)
        {
        Filter = " WHERE " + Filter;
        }

      var Query = "SELECT " + DistinctString + Columns + " FROM "+ TableName + Filter + " ORDER BY " + Columns;

			var MyDataSet = new DataSet(TableName + "View");
			try
				{
				using (var DbConnection = new SQLiteConnection(ConnectionString))
					{
					DbConnection.Open();
					using (var DbCommand = new SQLiteCommand(Query, DbConnection))
						{
						DbCommand.ExecuteNonQuery();
						using (var Adapter = new SQLiteDataAdapter(DbCommand))
							{
							Adapter.Fill(MyDataSet);
							Adapter.Update(MyDataSet);
							}
						}
					}
				return MyDataSet;
				}
			catch (Exception E)
				{
				Result += CLog.Trace("Failed to create query " + Query + " because " + E.Message,
					LogEventType.Error);
				return null;
				}
			}
	}
}