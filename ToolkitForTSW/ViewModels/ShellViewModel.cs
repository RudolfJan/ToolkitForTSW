using Caliburn.Micro;
using Logging.Library;
using Logging.Library.Wpf.ViewModels;
using System.IO;
using System.Threading.Tasks;
using ToolkitForTSW.GameSave.ViewModels;
using ToolkitForTSW.Mod.ViewModels;
using ToolkitForTSW.Options;
using ToolkitForTSW.Settings.ViewModels;
using Utilities.Library;
using Utilities.Library.TextHelpers;
using Utilities.Library.Wpf.ViewModels;

namespace ToolkitForTSW.ViewModels
  {
  public class ShellViewModel : Conductor<object>
    {
    private readonly IEventAggregator _events;
    private readonly IWindowManager _windowManager;
    private string _currentPlatformText;
    public string CurrentPlatformText
      {
      get
        {
        return _currentPlatformText;
        }
      set
        {
        _currentPlatformText = value;
        NotifyOfPropertyChange(() => CurrentPlatformText);
        }
      }

    public CheckOptionsLogic Check
      {
      get;
      set;
      } = CheckOptionsLogic.Instance;
    public bool BackupServiceActive
      {
      get
        {
        return BackupViewModel.GetBackupServiceStatus();
        }
      set
        {
        //_backupServiceActive = BackupViewModel.GetBackupServiceStatus();
        NotifyOfPropertyChange(nameof(BackupServiceActive));
        }
      }

    public ShellViewModel(IEventAggregator events, IWindowManager windowManager)
      {
      _events = events;
      _windowManager = windowManager;
      _events.SubscribeOnPublishedThread(this);
      LogEventHandler.LogEvent += OnLogEvent;
      PlatformChangedEventHandler.PlatformChanged += PlatformChangedEventHandler_PlatformChanged;
      }

    private void PlatformChangedEventHandler_PlatformChanged(object Sender, PlatformChangedEventArgs e)
      {
      CurrentPlatformText = $"Current platform: {TSWOptions.GetPlatformDisplayString(TSWOptions.CurrentPlatform)}";
      }

    protected override async void OnViewLoaded(object view)
      {
      base.OnViewLoaded(view);
      await FirstRun(_windowManager);
      TSWOptions.ReadFromRegistry();
      TSWOptions.CreateDirectories(TSWOptions.ToolkitForTSWFolder); // TODO this method creates the folders, but also sets the option names for folders. These roles must be refactored to avoid double logic.
      InitializeDataLogic.InitDatabase(); //TODo, logic to make this not happen if the database is already initialized properly
      InitializeDataLogic.InitializeEdition(); // Set the TSW version you are using
      InitializeDataLogic.FolderAndFileSetup();
      InitializeDataLogic.SevenZipSetup();
      CurrentPlatformText = $"Current platform: {TSWOptions.GetPlatformDisplayString(TSWOptions.CurrentPlatform)}";
      var optionsCheckResult = InitializeDataLogic.CheckOptions();

      CheckOptionsMessager(optionsCheckResult, _windowManager);
      // LiveryCracker cracker = new LiveryCracker(); // DEBUG
      }

    private static async Task FirstRun(IWindowManager windowManager)
      {
      while (!TSWOptions.GetNotFirstRun())
        {
          {
          var dataFolderSetup = IoC.Get<DataFolderSetupViewModel>();

          var task = windowManager.ShowDialogAsync(dataFolderSetup);
          await task;
          if (task.Result == true)
            {
            TSWOptions.ToolkitForTSWFolder = TextHelper.AddBackslash(dataFolderSetup.TSW3DataFolder);
            TSWOptions.WriteToRegistry();
            TSWOptions.CreateDirectories(TSWOptions.ToolkitForTSWFolder);
            if (!File.Exists(InitializeDataLogic.DatabasePath))
              {
              InitializeDataLogic.InitDatabase();
              }
            if (!string.IsNullOrEmpty(dataFolderSetup.TSW2DataFolder))
              {
              InitializeDataLogic.CopyTSW2StuffAndCleanDatabase();
              }
            TSWOptions.SetNotFirstRun();
            }
          }
        }
      return;
      }

    internal static void CheckOptionsMessager(CheckOptionsReporter optionsChecker, IWindowManager windowManager)
      {
      if (!optionsChecker.OptionsCheckStatus)
        {
        var messageBox = IoC.Get<MessageBoxViewModel>();
        messageBox.Title = @"There are issues with your settings";
        messageBox.Message = $"Options not all set correctly\r\n{optionsChecker.OptionsCheckReport}";
        var task = windowManager.ShowWindowAsync(messageBox);
        task.Wait();
        return;
        }
      }

    public Task ChangePlatform(string platform)
      {
      var viewmodel = IoC.Get<ChangePlatformViewModel>();
      return _windowManager.ShowDialogAsync(viewmodel);
      }
    public Task Options()
      {
      var viewmodel = IoC.Get<OptionsViewModel>();
      return _windowManager.ShowDialogAsync(viewmodel);
      }

    public Task LogViewer()
      {
      var viewmodel = IoC.Get<LoggingViewModel>();
      return _windowManager.ShowWindowAsync(viewmodel);
      }

    public Task ShowKeyBindings()
      {
      var keyBindingVM = IoC.Get<KeyBindingViewModel>();
      return _windowManager.ShowWindowAsync(keyBindingVM);
      }

    public Task LaunchTSW()
      {
      var launchVM = IoC.Get<LaunchTSWViewModel>();
      return _windowManager.ShowWindowAsync(launchVM);
      }

    public Task EditSettings()
      {
      var settingsVM = IoC.Get<SettingsManagerViewModel>();
      return _windowManager.ShowWindowAsync(settingsVM);
      }

    public Task ManageMods()
      {
      var viewModel = IoC.Get<ModManagerViewModel>();
      return _windowManager.ShowWindowAsync(viewModel);
      }

    public Task BackupTool()
      {
      var backupToolVM = IoC.Get<BackupViewModel>();
      return _windowManager.ShowWindowAsync(backupToolVM);
      }

    public static void DeleteIntroMovies()
      {
      IntroMovies.DeleteAllIntroMovies();
      }

    public Task RadioStations()
      {
      var viewmodel = IoC.Get<RadioStationsViewModel>();
      return _windowManager.ShowWindowAsync(viewmodel);
      }

    public Task ScenarioManager()
      {
      var scenarioManagerVM = IoC.Get<ScenarioManagerViewModel>();
      return _windowManager.ShowWindowAsync(scenarioManagerVM);
      }

    public Task PakInstaller()
      {
      var viewmodel = IoC.Get<PakInstallerViewModel>();
      return _windowManager.ShowWindowAsync(viewmodel);
      }

    public Task ManageGameSave()
      {
      var viewmodel = IoC.Get<GameSaveViewModel>();
      return _windowManager.ShowWindowAsync(viewmodel);
      }

    public Task About()
      {
      var viewmodel = IoC.Get<AboutViewModel>();
      var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();
      // TODO obtain these values from Settings
      viewmodel.Initialize(currentAssembly, TSWOptions.Version, "../../Images/AboutPicture.png", "https://www.hollandhiking.nl/trainsimulator");
      return _windowManager.ShowDialogAsync(viewmodel);
      }

    public static void GetManual()
      {
      ProcessHelper.OpenGenericFile(TSWOptions.ManualsFolder + "ToolkitForTSW Manual.pdf");
      }

    public bool CanGetStartersGuide
      {
      get
        {
        return Check.Check.ToolkitFolderOK && File.Exists(TSWOptions.ManualsFolder + "TSW3 Starters Guide.pdf");
        }
      }
    public static void GetStartersGuide()
      {
      ProcessHelper.OpenGenericFile(TSWOptions.ManualsFolder + "TSW3 Starters Guide.pdf");
      }

    public bool CanGetAdvancedGuide
      {
      get
        {
        return Check.Check.ToolkitFolderOK && File.Exists(TSWOptions.ManualsFolder + "TSW3 Advanced User Guide.pdf");
        }
      }

    public static void GetAdvancedGuide()
      {
      ProcessHelper.OpenGenericFile(TSWOptions.ManualsFolder + "TSW3 Advanced User Guide.pdf");
      }

    public Task GetRouteGuides()
      {
      var viewmodel = IoC.Get<RouteGuideViewModel>();
      return _windowManager.ShowWindowAsync(viewmodel);
      }

    public Task CloseForm()
      {
      return TryCloseAsync();
      }


    private void OnLogEvent(object Sender, LogEventArgs args)
      {
      if (args.EntryClass.EventType == LogEventType.Error || args.EntryClass.EventType == LogEventType.InformUser)
        {
        LogCollectionManager.LogEvents.Add(args.EntryClass);
        var message = args.EntryClass.LogEntry;
        var viewmodel = IoC.Get<NotificationViewModel>();
        if (message.Length > 100)
          {
          viewmodel.Message = message.Substring(0, 100); //Prevents this simple window tho clutter the whole screen
          }
        else
          {
          viewmodel.Message = message;
          }
        _windowManager.ShowWindowAsync(viewmodel);
        }
      }
    }
  }
