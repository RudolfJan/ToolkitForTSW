using Filter.Library.Filters.DataAccess;
using Logging.Library;
using Screenshots.Library.Logic;
using Screenshots.Library.WPF.ViewModels;
using SQLiteDatabase.Library;
using System;
using System.IO;
using System.Threading.Tasks;
using ToolkitForTSW.DataAccess;
using ToolkitForTSW.Options;
using ToolkitForTSW.ViewModels;
using Utilities.Library;
using Utilities.Library.Filters.DataAccess;
using Utilities.Library.Zip;
using DatabaseFactory = ToolkitForTSW.DataAccess.DatabaseFactory;

namespace ToolkitForTSW
  {
  public class InitializeDataLogic

    {
    internal static string InitialInstallDirectory = "";
    public InitializeDataLogic()
      {
      // https://docs.microsoft.com/en-us/dotnet/desktop/wpf/app-development/how-to-add-a-splash-screen-to-a-wpf-application?view=netframeworkdesktop-4.8

      TSWOptions.ReadFromRegistry();
      InitialInstallDirectory = TSWOptions.ToolkitForTSWFolder; //Is always set by the installer
      // DEBUG
      if (string.IsNullOrEmpty(InitialInstallDirectory))
        {
        InitialInstallDirectory = "D:\\ToolkitForTSWTest\\";// TODO change this
        }

      TSWOptions.CreateDirectories(InitialInstallDirectory);
      }

    public static CheckOptionsReporter CheckOptions()
      {
      CheckOptionsLogic.Instance.SetAllOptionChecks();
      var optionsChecker = new CheckOptionsReporter();
      optionsChecker.BuildOptionsCheckReport();
      return optionsChecker;
      }

    internal static void SevenZipSetup()
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

    internal static void FolderAndFileSetup()
      {
      if (TSWOptions.ToolkitForTSWFolder.Length > 1)
        {
        // Do not initialize Database again, should not need to do that.
        TSWOptions.CopyManuals();
        MoveMods();
        MoveTemplates();//TODO register application version, so we run this code only once
        EngineIniSettingDataAccess.ImportEngineIniSettingsFromCsv("SQL\\EngineIniSettingsList.csv");
        EngineIniSettingDataAccess.ImportDescriptionsFromExcel("SQL\\AnnotatedSettingsList.xlsx");
        InitScreenshotManagerSettings();
        MakeBackup();
        }
      }

    private static void CopyToolkitFolder(string oldToolkitPath, string targetPath, string folderName)
      {
      var source = $"{oldToolkitPath}{folderName}";
      var target = $"{targetPath}{folderName}";
      FileHelpers.CopyDir(source, target, true);
      }
    // This function makes sure all TSW2 stuff is properly transferred, if you migrate to TSW3
    // It only should run once
    internal static void CopyTSW2StuffAndCleanDatabase()
      {
      if (!TSWOptions.GetNotFirstRun())
        {
        var toolkitPath = TSWOptions.GetTSW2ToolkitPath();
        if (!string.IsNullOrEmpty(toolkitPath))
          {
          // TODO need to do this BEFORE attempting to create the database
          var dbSource = $"{toolkitPath}TSWTools.db";
          var dbTarget = $"{InitialInstallDirectory}TSWTools.db";
          if (File.Exists(dbSource) && !File.Exists(dbTarget))
            {
            File.Copy(dbSource, dbTarget);
            // remove data from modsdatabase
            InitDatabase();
            //ModDataAccess.DeleteAllMods();
            //ModSetDataAccess.DeleteAllModSets();
            //ModModSetDataAccess.DeleteAllModModSets();
            }
          else
            {
            InitDatabase();
            }
          CopyToolkitFolder(toolkitPath, InitialInstallDirectory, "Manuals");
          CopyToolkitFolder(toolkitPath, InitialInstallDirectory, "Thumbnails");
          CopyToolkitFolder(toolkitPath, InitialInstallDirectory, "Templates");
          CopyToolkitFolder(toolkitPath, InitialInstallDirectory, "Scenarios");
          }
        }
      else
        {
        InitDatabase();
        }
      }
    public static void InitializeEdition()
      {
      EditionDataAccess.LoadEditionsFromCSV();
      TSWOptions.GameEdition = EditionDataAccess.GetActiveEdition();
      TSWOptions.GameEditionId = EditionDataAccess.GetActiveEditionId();
      }

    private static void MakeBackup()
      {
      // Make a backup when required
      if (TSWOptions.AutoBackup && !string.IsNullOrEmpty(TSWOptions.BackupFolder))
        {
        var backup = new BackupViewModel();
        backup.MakeDailyBackup();
        }
      }

    public static void MoveMods()
      {
      // Move Mods to proper directory
      var destination = TSWOptions.ModsFolder;
      if (Directory.Exists(destination))
        {
        var source = destination.Replace("Mods", "Liveries");
        if (Directory.Exists(source))
          {
          FileHelpers.CopyDir(source, destination, true);
          System.Windows.MessageBox.Show($"You can now safely remove the Liveries folder {source}");
          }
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

    public static string DatabasePath
      {
      get
        {
        return $"{TSWOptions.ToolkitForTSWFolder}TSWTools.db";
        }
      }
    public static void InitDatabase()
      {
      var factory = new DatabaseFactory();
      var databasePath = DatabasePath;
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