using Filter.Library.Filters.DataAccess;
using Logging.Library;
using SavCracker.Library;
using Screenshots.Library.Logic;
using Screenshots.Library.WPF.ViewModels;
using SQLiteDatabase.Library;
using Styles.Library.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using ToolkitForTSW.DataAccess;
using Utilities.Library.Filters.DataAccess;
using DatabaseFactory = ToolkitForTSW.DataAccess.DatabaseFactory;

namespace ToolkitForTSW
	{
	public class CMain : Notifier
		{
		private string _Result = string.Empty;

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
      var InitialInstallDirectory = CTSWOptions.TSWToolsFolder;
      InitDatabase(); // must be done before trying to get options to avoid exception
			while (!CTSWOptions.GetNotFirstRun())
				{
				MessageBox.Show(@"Please complete the configuration before you proceed", @"Set configuration",
					MessageBoxButton.OK,MessageBoxImage.Asterisk);
				CTSWOptions.ReadFromRegistry();
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
        InitDatabase(); // TODO Need to do this again, in case the data folder is changed, maybe make it inmutable?
				CTSWOptions.CreateDirectories();
				CTSWOptions.CopyManuals();
        MoveMods();
        InitScreenshotManagerSettings();
        }

      // LiveryCracker cracker = new LiveryCracker(); // DEBUG
      }

    public static void MoveMods()
      {
			// Move Mods to proper directory
      var destination = CTSWOptions.ModsFolder;
      var source = destination.Replace("Mods", "Liveries");
      if (Directory.Exists(source))
        {
        CApps.CopyDir(source, destination, true);
        MessageBox.Show($"You can now safely remove the Liveries folder {source}");
        }
      }

		public void InitDatabase()
      {
      var factory = new DatabaseFactory();
			var databasePath=$"{CTSWOptions.TSWToolsFolder}TSWTools.db";
			var connectionString = $"Data Source = {databasePath}; Version = 3;";
			DbManager.InitDatabase(connectionString, databasePath, factory);
			EngineIniSettingDataAccess.ImportEngineIniSettingsFromCsv("SQL\\EngineIniSettingsList.csv");
      InitScreenshotManagerDatabase();
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

    private void InitScreenshotManagerDatabase()
      {
      var screenshotFactory = new Screenshots.Library.DataAccess.DatabaseFactory();
      screenshotFactory.CreateStructures();
      CategoryDataAccess.ImportCategoriesFromCsv("SQL\\ImportCategories.csv");
      TagDataAccess.ImportTagsFromCsv("SQL\\ImportTags.csv");
      }

    private void InitScreenshotManagerSettings()
      {
      ImageManager.ThumbnailBasePath= CTSWOptions.ThumbnailFolder;

			// TODO make this settable in the options
      ScreenshotManagerViewModel.ScreenshotsPerPage = 20;
      ScreenshotManagerViewModel.ScreenshotsPerColumn = 5;
      ScreenshotManagerViewModel.ScreenshotsPerRow = 4;
      ThumbnailLogic.ThumbnailWidth = 120;

			// https://briancaos.wordpress.com/2018/11/13/c-async-fire-and-forget/
      ImageManager.DeleteOrphanImagesFromDatabase();
      Task.Factory.StartNew(ImageManager.LoadNewImagesForAllCollectionsAsync);
			}

		public void OpenManual()
			{
			Result += CApps.OpenGenericFile(CTSWOptions.ManualsFolder + "ToolkitForTSW Manual.pdf");
			}

		public void OpenStartersGuide()
			{
			Result += CApps.OpenGenericFile(CTSWOptions.ManualsFolder + "TSW2 Starters guide.pdf");
			}
		}
	}