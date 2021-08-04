using System.Threading.Tasks;
using Caliburn.Micro;
using Logging.Library;
using Logging.Library.Wpf.ViewModels;
using ToolkitForTSW.Options;
using Utilities.Library;
using Utilities.Library.Wpf.ViewModels;

namespace ToolkitForTSW.ViewModels
  {
  public class ShellViewModel : Conductor<object>
    {
    private readonly IEventAggregator _events;
    private readonly IWindowManager _windowManager;
		
		private string _currentPlatformText= $"Current platform: {TSWOptions.GetPlatformDisplayString(TSWOptions.CurrentPlatform)}";
		public string CurrentPlatformText
      {
      get { return _currentPlatformText;
				}
			set
        {
				_currentPlatformText=value;
				NotifyOfPropertyChange(() => CurrentPlatformText);
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
			CurrentPlatformText= $"Current platform: {TSWOptions.GetPlatformDisplayString(TSWOptions.CurrentPlatform)}";
      }

    protected override void OnViewLoaded(object view)
			{
			base.OnViewLoaded(view);
			// await EditRoutes();
			}

		public async Task LogViewer()
      {
			var viewmodel= IoC.Get<LoggingViewModel>();
			await _windowManager.ShowWindowAsync(viewmodel);
      }

		public async Task ShowKeyBindings()
			{
			var keyBindingVM = IoC.Get<KeyBindingViewModel>();
			await _windowManager.ShowWindowAsync(keyBindingVM);
			}

		public async Task LaunchTSW()
			{
			var launchVM = IoC.Get<LaunchTSWViewModel>();
			await _windowManager.ShowWindowAsync(launchVM);
			}

		public async Task BackupTool()
			{
			var backupToolVM = IoC.Get<BackupViewModel>();
			await _windowManager.ShowWindowAsync(backupToolVM);
			}
		public async Task RadioStations()
      {
			var viewmodel= IoC.Get<RadioStationsViewModel>();
			await _windowManager.ShowWindowAsync(viewmodel);
      }

		public async Task ScenarioManager()
			{
			var scenarioManagerVM = IoC.Get<ScenarioManagerViewModel>();
			await _windowManager.ShowWindowAsync(scenarioManagerVM);
			}

		public async Task About()
      {
			var viewmodel = IoC.Get<AboutViewModel>();
			var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();
			// TODO obtain these values from Settings
			viewmodel.Initialize(currentAssembly,TSWOptions.Version, "../../Images/AboutPicture.png","https://www.hollandhiking.nl/trainsimulator");
			await _windowManager.ShowDialogAsync(viewmodel);
			}

		public void GetManual()
      {
			ProcessHelper.OpenGenericFile(TSWOptions.ManualsFolder + "ToolkitForTSW Manual.pdf");
			}

		public void GetStartersGuide()
      {
			ProcessHelper.OpenGenericFile(TSWOptions.ManualsFolder + "TSW2 Starters guide.pdf");
			}

		//public async Task GetRouteGuides()
  //    {
		//	var path= TSWOptions.ManualsFolder + "RouteGuides\\";
		//	var viewmodel = IoC.Get<RouteGuideViewModel>();
		//	viewmodel.RootFolder = path;
		//	await _windowManager.ShowWindowAsync(viewmodel);
		//	}

		public async Task Close()
      {
			await TryCloseAsync();
      }


		private void OnLogEvent(object Sender, LogEventArgs args)
			{
			if (args.EntryClass.EventType == LogEventType.Error || args.EntryClass.EventType == LogEventType.Event)
				{
				LogCollectionManager.LogEvents.Add(args.EntryClass);
				var message = args.EntryClass.LogEntry;
				var viewmodel = IoC.Get<NotificationViewModel>();
				viewmodel.Message = message;
				_windowManager.ShowWindowAsync(viewmodel);
				}
			}
		}
	}
