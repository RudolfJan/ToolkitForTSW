using Styles.Library.Helpers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.IO;


namespace ToolkitForTSW
  {
  public enum LiveryTypesEnum

		{
		[Description("Undefined")] Undefined,
		[Description("Engine")] Engine,
		[Description("Wagon")] Wagon,
		[Description("Consist")] Consist,
		[Description("Scenery")] Scenery,
		[Description("Route")] Route,
		[Description("Scenario")] Scenario,
		[Description("Service timetable")] Service,
		[Description("Weather")] Weather,
    [Description("Game")] Game,
		[Description("Other")] Other
		};

	public class CLiveryManager : Notifier
		{
		private DataSet _LiveryDataSet;
		public DataSet LiveryDataSet
			{
			get { return _LiveryDataSet; }
			set
				{
				_LiveryDataSet = value;
				OnPropertyChanged("LiveryDataSet");
				}
			}

    private DataSet _InstalledDataSet;
    public DataSet InstalledDataSet
      {
      get { return _InstalledDataSet; }
      set
        {
        _InstalledDataSet = value;
        OnPropertyChanged("InstalledDataSet");
        }
      }

    /*
		List with all (DLC) Pak files. 
		*/
    private ObservableCollection<FileInfo> _PakFilesList;
		public ObservableCollection<FileInfo> PakFilesList
			{
			get { return _PakFilesList; }
			set
				{
				_PakFilesList = value;
				OnPropertyChanged("PakFilesList");
				}
			}

		/*
		List with all (DLC) Pak files. 
		*/
		private ObservableCollection<FileInfo> _InstalledPakFilesList;
		public ObservableCollection<FileInfo> InstalledPakFilesList
			{
			get { return _InstalledPakFilesList; }
			set
				{
				_InstalledPakFilesList = value;
				OnPropertyChanged("InstalledPakFilesList");
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

    private CLiverySet _LiverySet;
    public CLiverySet LiverySet
      {
      get { return _LiverySet; }
      set
        {
        _LiverySet = value;
        OnPropertyChanged("LiverySet");
        }
      }

    private const String LiveryTableName = "Livery";

		public CLiveryManager()
			{
			PakFilesList = new ObservableCollection<FileInfo>();
			InstalledPakFilesList = new ObservableCollection<FileInfo>();
      LiverySet = new CLiverySet();
			Initialise();
			}

		private void Initialise()
			{
			GetPakFiles();
			CreateDataSet();
			GetInstalledPakFiles();
			UpdateInstalledStates();
			}

		private void CreateDataSet()
			{
			var Db = new CDatabase();

			var ColumnSet =
				"Id, Name, FilePath, Description, Image, Source, LiveryType, ReplaceName, IsInstalled, FileName, DLCName";
			LiveryDataSet = Db.BuildDataSet(LiveryTableName, ColumnSet);
			}

    private void CreateInstalledDataSet()
      {
      var Db = new CDatabase();

      var ColumnSet =
        "Id, Name, FilePath, Description, Image, Source, LiveryType, ReplaceName, IsInstalled, FileName, DLCName";
      var Filter = "IsInstalled=1";
      InstalledDataSet = Db.BuildDataSet(LiveryTableName, ColumnSet,false, Filter);
      }

    public void RemoveAllInstalledPaks()
      {
      CreateInstalledDataSet();
      foreach(var X in InstalledDataSet.Tables[0].Rows)
        {
        var FilePath = (String)((DataRow)X).ItemArray[9];
        if (!String.IsNullOrEmpty(FilePath))
          {
          FilePath = CTSWOptions.TrainSimWorldDirectory + @"TS2Prototype\Content\DLC\" + FilePath;
          CApps.DeleteSingleFile(FilePath);
          }
        }
      }


    private void GetPakFiles()
			{
			var BaseDir = new DirectoryInfo(CTSWOptions.LiveriesFolder);
			FileInfo[] Files = BaseDir.GetFiles("*.pak", SearchOption.AllDirectories);
			PakFilesList.Clear();
			foreach (var F in Files)
				{
				PakFilesList.Add(F);
				UpdateLiveryTable(F);
				}
			}

		private void GetInstalledPakFiles()
			{
			var BaseDir =
				new DirectoryInfo(CTSWOptions.TrainSimWorldDirectory + "TS2Prototype\\Content\\DLC");
			FileInfo[] Files = BaseDir.GetFiles("*.pak", SearchOption.AllDirectories);
			InstalledPakFilesList.Clear();
			foreach (var F in Files)
				{
				InstalledPakFilesList.Add(F);
				}
			}

		private static String StripLiveriesDir(String Input)
			{
			if (Input.StartsWith(CTSWOptions.LiveriesFolder))
				{
				return Input.Substring(CTSWOptions.LiveriesFolder.Length);
				}

			return String.Empty;
			}

		public void InstallPak(String PakPath)
			{
			var Source = CTSWOptions.LiveriesFolder + PakPath;
			var FileName = Path.GetFileName(Source);
			var Destination = CTSWOptions.TrainSimWorldDirectory + "TS2Prototype\\Content\\DLC\\" +
			                  FileName;
			try
				{
				File.Copy(Source, Destination, false);
				var F = new FileInfo(Destination);
				InstalledPakFilesList.Add(F);
				}
			catch (Exception E)
				{
				Result += CLog.Trace("Failed to install livery pak because " + E.Message,
					LogEventType.Error);
				}
			}

		public void UnInstallPak(String FileName)
			{
			var FilePath = new FileInfo(CTSWOptions.TrainSimWorldDirectory +
			                            "TS2Prototype\\Content\\DLC\\" + FileName);
			Result += CApps.DeleteSingleFile(FilePath.FullName);
			GetInstalledPakFiles(); // Bit crude refresh, simple remove does not work properly because FileInfo object is not recognized properly.
			UpdateInstalledStatesDatabase(FileName, false);
			}

		// Note: FieldList must have format  Field="Value", Field2="Value2"
		public void UpdateLiveryTableFields(Int64 RowId, String FieldList)
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
								"BEGIN TRANSACTION; UPDATE OR IGNORE Livery SET " + FieldList + " WHERE ID=" +
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
				Result += CLog.Trace("Failed to add pak file to database " + FieldList + " " + E.Message,
					LogEventType.Error);
				}
			}

    public static String UpdateLiveryTable(FileInfo F, Boolean IsInstalled=false)
      {
      return UpdateLiveryTable(F.FullName, IsInstalled);
      }

    public static String UpdateLiveryTable(String F, Boolean IsInstalled = false)
			{
			var Db = new CDatabase();
			var Record = StripLiveriesDir(F);
      var FileName = Path.GetFileName(F);
      var IsInstalledInt = Convert.ToInt32(IsInstalled);
			if (Record.Length == 0)
				{
				return "";
				}

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
								"BEGIN TRANSACTION; INSERT OR IGNORE INTO Livery (FilePath, FileName, IsInstalled) VALUES (@FilePath, @FileName, @IsInstalled); COMMIT;";
							DbCommand.Parameters.Add(new SQLiteParameter("@FilePath") {Value = Record});
							DbCommand.Parameters.Add(new SQLiteParameter("@FileName") {Value = FileName});
              DbCommand.Parameters.Add(new SQLiteParameter("@IsInstalled") { Value = IsInstalledInt });
              DbCommand.CommandType = CommandType.Text;
							DbCommand.ExecuteNonQuery();
							return "Livery entry updated " + Record + "\r\n";
							}
						catch (SQLiteException E)
							{
							using (var RollbackCommand = new SQLiteCommand("ROLLBACK;", DbConnection))
								{
								RollbackCommand.ExecuteNonQuery();
								}

							return CLog.Trace("Failed to add pak file to database " + F + " " + E.Message,
								LogEventType.Error);
							}
						}
					}
				}
			catch (Exception E)
				{
				return CLog.Trace("Failed to add pak file to database " + F + " " + E.Message,
					LogEventType.Error);
				}
			}

		public void UpdateInstalledStates()
			{
			foreach (var Livery in PakFilesList)
				{
				var Found = false;
				foreach (var InstalledPak in InstalledPakFilesList)
					{
					if (String.CompareOrdinal(Livery.Name, InstalledPak.Name) == 0)
						{
						Found = true;
						break;
						}
					}

				UpdateInstalledStatesDatabase(Livery.Name, Found);
				}
			}

		private void UpdateInstalledStatesDatabase(String FileName, Boolean IsInstalled)
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
								"BEGIN TRANSACTION; UPDATE OR IGNORE Livery SET IsInstalled=\"" +
								Convert.ToInt32(IsInstalled) +
								"\" WHERE FileName=\"" + FileName + "\"; COMMIT;";
							DbCommand.CommandType = CommandType.Text;
							DbCommand.ExecuteNonQuery();
							}
						catch (SQLiteException E)
							{
							using (var RollbackCommand = new SQLiteCommand("ROLLBACK;", DbConnection))
								{
								RollbackCommand.ExecuteNonQuery();
								}

							Result += CLog.Trace(
								"Failed to update installed states to database " + FileName + " " + E.Message,
								LogEventType.Error);
							}
						}
					}
				}
			catch (Exception E)
				{
				Result += CLog.Trace(
					"Failed to update installed states to database " + FileName + " " + E.Message,
					LogEventType.Error);
				}
			}
		}
	}
