using Filter.Library.Filters.DataAccess;
using Logging.Library;
using Screenshots.Library.Logic;
using Screenshots.Library.WPF.ViewModels;
using SQLiteDatabase.Library;
using Styles.Library.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using ToolkitForTSW.DataAccess;
using ToolkitForTSW.Options;
using ToolkitForTSW.ViewModels;
using Utilities.Library;
using Utilities.Library.Filters.DataAccess;
using Utilities.Library.Zip;
using DatabaseFactory = ToolkitForTSW.DataAccess.DatabaseFactory;

namespace ToolkitForTSW
  {
  public class CMain : Notifier
    {
    public CMain()
      {

      // https://docs.microsoft.com/en-us/dotnet/desktop/wpf/app-development/how-to-add-a-splash-screen-to-a-wpf-application?view=netframeworkdesktop-4.8

      TSWOptions.ReadFromRegistry();
      var InitialInstallDirectory = TSWOptions.ToolkitForTSWFolder; //Is always set by the installer
      InitDatabase();
      FirstRun(InitialInstallDirectory);
      FolderAndFileSetup();
      SevenZipSetup();
      CheckOptions();
      // LiveryCracker cracker = new LiveryCracker(); // DEBUG
      }

    private static void CheckOptions()
      {
      CheckOptionsLogic.Instance.SetAllOptionChecks();
      var optionsChecker = new CheckOptionsReporter();
      optionsChecker.BuildOptionsCheckReport();
      if (!optionsChecker.OptionsCheckStatus)
        {
        MessageBox.Show($"Options not all set correctly\r\n{optionsChecker.OptionsCheckReport}",
            @"There are issues with your settings",
            MessageBoxButton.OK,
            MessageBoxImage.Warning);
        }
      }

    private static void SevenZipSetup()
      {
      try
        {
        SevenZipLib.InitZip(TSWOptions.SevenZip);
        }
      catch (Exception ex)
        {
        Log.Trace("Failed to initialize SevenZip functions, probably 7Zip program is not installed", ex);
        }
      }

    private static void FolderAndFileSetup()
      {
      if (TSWOptions.ToolkitForTSWFolder.Length > 1)
        {
        // Do not initialize Database again, should not need to do that.
        TSWOptions.CreateDirectories();
        TSWOptions.CopyManuals();
        MoveMods();
        MoveTemplates();//TODO register application version, so we run this code only once
        EngineIniSettingDataAccess.ImportEngineIniSettingsFromCsv("SQL\\EngineIniSettingsList.csv");
        EngineIniSettingDataAccess.ImportDescriptionsFromExcel("SQL\\AnnotatedSettingsList.xlsx");
        InitScreenshotManagerSettings();
        MakeBackup();
        }
      }

    private static void MakeBackup()
      {
      // Make a backup when required
      if (TSWOptions.AutoBackup)
        {
        var backup = new BackupViewModel();
        backup.MakeDailyBackup();
        }
      }

    private static void FirstRun(string InitialInstallDirectory)
      {
      while (!TSWOptions.GetNotFirstRun())
        {
        MessageBox.Show(@"Please complete the configuration before you proceed", @"Set configuration",
          MessageBoxButton.OK, MessageBoxImage.Asterisk);
        TSWOptions.ReadFromRegistry();
        var Form = new FormOptions();
        if (Form.ShowDialog() == true)
        {
          TSWOptions.SetNotFirstRun();
          TSWOptions.WriteToRegistry();
          TSWOptions.UpdateTSWToolsDirectory(InitialInstallDirectory);
          }
        }
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

    public static void MoveTemplates()
      {
      var destination = TSWOptions.TemplateFolder;
      var source = ".\\Templates\\";
      if (Directory.Exists(source))
        {
        if (Directory.Exists(destination))
          {
          FileHelpers.CopyDir(source, destination, true);
          }
        }
      }

    public static void InitDatabase()
      {
      var factory = new DatabaseFactory();
      var databasePath = $"{TSWOptions.ToolkitForTSWFolder}TSWTools.db";
      var connectionString = $"Data Source = {databasePath}; Version = 3;";
      DbManager.CurrentDatabaseVersion = 5;
      DbManager.DatabaseVersionDescription = "Added experimental settings";
      DbManager.InitDatabase(connectionString, databasePath, factory);
      // TODO check process logic very carefully to make sure you set the version correct and execute the proper update procedure.
      var version = DbManager.GetCurrentVersion();
      if (version.VersionNr < 3) // old database version is not compatible
        {
        DbManager.DeleteDatabase();
        DbManager.InitDatabase(connectionString, databasePath, factory);
        version = DbManager.GetCurrentVersion();
        }
      else
        {
        DbManager.UpdateDatabaseVersionNumber(DbManager.CurrentDatabaseVersion, DbManager.DatabaseVersionDescription);

        }
      InitScreenshotManagerDatabase();
      RouteDataAccess.InitRouteForSavCracker("SQL\\RouteDataImport.csv");
      Log.Trace($"Created database {databasePath} Version={version.VersionNr} {version.Description}");
      }

    private static void InitScreenshotManagerDatabase()
      {
      var screenshotFactory = new Screenshots.Library.DataAccess.DatabaseFactory();
      screenshotFactory.CreateStructures();
      CategoryDataAccess.ImportCategoriesFromCsv("SQL\\ImportCategories.csv");
      TagDataAccess.ImportTagsFromCsv("SQL\\ImportTags.csv");
      }

    private static void InitScreenshotManagerSettings()
      {
      ImageManager.ThumbnailBasePath = TSWOptions.ThumbnailFolder;

      // TODO make this settable in the options
      ScreenshotManagerViewModel.ScreenshotsPerPage = 20;
      ScreenshotManagerViewModel.ScreenshotsPerColumn = 5;
      ScreenshotManagerViewModel.ScreenshotsPerRow = 4;
      ThumbnailLogic.ThumbnailWidth = 120;

      // https://briancaos.wordpress.com/2018/11/13/c-async-fire-and-forget/
      ImageManager.DeleteOrphanImagesFromDatabase();
      Task.Factory.StartNew(ImageManager.LoadNewImagesForAllCollectionsAsync);
      }
    }
  }