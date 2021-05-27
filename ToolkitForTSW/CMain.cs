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
using ToolkitForTSW.Backups;
using ToolkitForTSW.DataAccess;
using ToolkitForTSW.Options;
using Utilities.Library;
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
			TSWOptions.ReadFromRegistry();
			var InitialInstallDirectory = TSWOptions.ToolkitForTSWFolder;
      InitDatabase(); // must be done before trying to get options to avoid exception
			while (!TSWOptions.GetNotFirstRun())
				{
				MessageBox.Show(@"Please complete the configuration before you proceed", @"Set configuration",
					MessageBoxButton.OK,MessageBoxImage.Asterisk);
				TSWOptions.ReadFromRegistry();
				var Form = new FormOptions();
				Form.ShowDialog();
				if (Form.DialogResult == true)
					{
					TSWOptions.WriteToRegistry();
					TSWOptions.UpdateTSWToolsDirectory(InitialInstallDirectory);
					}
				}
			if (TSWOptions.ToolkitForTSWFolder.Length > 1)
				{
        InitDatabase(); // TODO Need to do this again, in case the data folder is changed, maybe make it inmutable?
				TSWOptions.CreateDirectories();
				TSWOptions.CopyManuals();
        MoveMods();
				EngineIniSettingDataAccess.ImportEngineIniSettingsFromCsv("SQL\\EngineIniSettingsList.csv");
				EngineIniSettingDataAccess.ImportDescriptionsFromExcel("SQL\\AnnotatedSettingsList.xlsx");
				InitScreenshotManagerSettings();

				// Make a backup when required
				if(TSWOptions.AutoBackup)
					{
					var backup= new CBackup();
					backup.MakeDailyBackup();
					}

        }
			var optionsChecker= new CheckOptionsReporter();
			optionsChecker.BuildOptionsCheckReport();
			if(!optionsChecker.OptionsCheckStatus)
				{ 
			MessageBox.Show($"Options not all set correctly\r\n{optionsChecker.OptionsCheckReport}", 
					@"There are issues with your settings",
					MessageBoxButton.OK, 
					MessageBoxImage.Warning);
				}
			// LiveryCracker cracker = new LiveryCracker(); // DEBUG
			}

    public static void MoveMods()
      {
			// Move Mods to proper directory
      var destination = TSWOptions.ModsFolder;
      var source = destination.Replace("Mods", "Liveries");
      if (Directory.Exists(source))
        {
				FileHelpers.CopyDir(source, destination, true);
        MessageBox.Show($"You can now safely remove the Liveries folder {source}");
        }
      }

		public void InitDatabase()
      {
      var factory = new DatabaseFactory();
			var databasePath=$"{TSWOptions.ToolkitForTSWFolder}TSWTools.db";
			var connectionString = $"Data Source = {databasePath}; Version = 3;";
			DbManager.InitDatabase(connectionString, databasePath, factory);
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
      ImageManager.ThumbnailBasePath= TSWOptions.ThumbnailFolder;

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
			Result += ProcessHelper.OpenGenericFile(TSWOptions.ManualsFolder + "ToolkitForTSW Manual.pdf");
			}

		public void OpenStartersGuide()
			{
			Result += ProcessHelper.OpenGenericFile(TSWOptions.ManualsFolder + "TSW2 Starters guide.pdf");
			}
		}
	}