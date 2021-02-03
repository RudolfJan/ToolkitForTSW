using Logging.Library;
using SQLiteDatabase.Library;
using Styles.Library.Helpers;
using System;
using System.Windows;
using ToolkitForTSW.DataAccess;

namespace ToolkitForTSW
	{
	public class CMain : Notifier
		{
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

    public CMain()
			{
			while (!CTSWOptions.GetNotFirstRun())
				{
				MessageBox.Show(@"Please complete the configuration before you proceed", @"Set configuration",
					MessageBoxButton.OK,MessageBoxImage.Asterisk);

				CTSWOptions.ReadFromRegistry();

				var InitialInstallDirectory = CTSWOptions.TSWToolsFolder;
				var Form = new FormOptions();

				Form.ShowDialog();
				if (Form.DialogResult == true)
					{
					CTSWOptions.WriteToRegistry();
					CTSWOptions.UpdateTSWToolsDirectory(InitialInstallDirectory);
					}
				}
			if (CTSWOptions.TSWToolsFolder.Length > 1)
				{
				CTSWOptions.CreateDirectories();
				CTSWOptions.MoveManuals();
        InitDatabase();
        }
			}

    public void InitDatabase()
      {
      var factory = new DatabaseFactory();
			var databasePath=$"{CTSWOptions.TSWToolsFolder}TSWTools.db";
			var connectionString = $"Data Source = {databasePath}; Version = 3;";
			DbManager.InitDatabase(connectionString, databasePath, factory);
      var version = DbManager.GetCurrentVersion();
      if (version.VersionNr < 3) // old database version ois not compatible
        {
        DbManager.DeleteDatabase(); 
        DbManager.InitDatabase(connectionString, databasePath, factory);
				}
			DbManager.UpdateDatabaseVersionNumber(3, "Refactoring DbAccess");
      version = DbManager.GetCurrentVersion();
      RouteDataAccess.InitRouteForSavCracker("SQL\\RouteDataImport.csv");

			Log.Trace($"Created database {databasePath} Version={version.VersionNr} {version.Description}");
			}



		public void OpenManual()
			{
			Result += CApps.OpenGenericFile(CTSWOptions.ManualsFolder + "ToolkitForTSW Manual "+ CTSWOptions.TSWToolsManualVersion+".pdf");
			}

		public void OpenStartersGuide()
			{
			Result += CApps.OpenGenericFile(CTSWOptions.ManualsFolder + "TSW2 Starters guide.pdf");
			}
		}
	}