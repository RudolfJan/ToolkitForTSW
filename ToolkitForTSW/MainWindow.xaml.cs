using Logging.Library;
using System;
using System.Diagnostics;
using System.Windows;
using ToolkitForTSW;
using ToolkitForTSW.Backups;
using ToolkitForTSW.Mod;
using ToolkitForTSW.Settings;
using Utilities.Library;

namespace ToolkitForTSW
	{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
		{
		public CMain MainData { get; set; }
		public Log LogForm;
		public static LogEventHandler LogEventHandler { get; set; }

		public MainWindow()
			{
			InitializeComponent();
			// trace setup
			Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
			Trace.AutoFlush = true;
			Trace.Indent();
			LogEventHandler = new LogEventHandler();
			LogForm = new Log();
			MainData = new CMain();
			DataContext = MainData;
			}

		#region Utilities

		private void OnOptionsButtonClicked(Object Sender, RoutedEventArgs E)
			{
			var Form = new FormOptions();
			
			Form.ShowDialog();
			}

		private void OnLogViewButtonClicked(Object Sender, RoutedEventArgs E)
			{
			var Form = new FormLogViewer(LogForm);
			Form.Show();
			}

		private void OnBackupButtonClicked(Object Sender, RoutedEventArgs E)
			{
			var Form = new FormBackup();
			Form.Show();
			}

		private void OnKeyBindingButtonClicked(Object Sender, RoutedEventArgs E)
			{
			var InputMapperList = new CInputMapperList();
			var Form = new FormInputMapperManager(InputMapperList);
			Form.Show();
			}

		#endregion

		#region Unpack

    private void OnScenarioManager(object sender, RoutedEventArgs e)
      {
			var Form= new FormScenarioManager();
      Form.Show();
      }

		private void OnUnpackerButtonClicked(Object Sender, RoutedEventArgs E)
			{
			var Form = new FormUnpackGameFiles();
			Form.Show();
			}

		private void OnViewUnpackedPaksButtonClicked(Object Sender, RoutedEventArgs E)
			{
			ProcessHelper.OpenFolder(CTSWOptions.UnpackFolder);
			}

		private void OnUModelLauncherButtonClicked(Object Sender, RoutedEventArgs E)
			{
			var Form = new FormLaunchUModel();
			Form.Show();
			}
		#endregion

		#region Tools

		private void OnLaunchTSWButtonClicked(Object Sender, RoutedEventArgs E)
			{
			var Form = new FormLaunchTSW();
			Form.Show();
			}

		private void OnScreenshotManagerButtonClicked(Object Sender, RoutedEventArgs E)
			{
			var Form = new FormScreenshotManager();
			Form.Show();
			}

		private void OnModManagerButtonClicked(Object Sender, RoutedEventArgs E)
			{
			var ModManager = new CModManager();
			var Form = new FormModManager(ModManager);
			Form.Show();
			}

		private void OnEditSettingsButtonClicked(Object Sender, RoutedEventArgs E)
			{
			var Form = new FormSettings();
			Form.Show();
			}

    private void OnRadioStationsButtonClicked(Object Sender, RoutedEventArgs E)
      {
      var Form= new FormRadioStationManager();
      Form.Show();
      }

    private void OnPakInstallerButtonClicked(Object Sender, RoutedEventArgs E)
      {
      var Form=new FormPakInstaller();
      Form.Show();
      }

    #endregion

    #region Help

    private void OnAboutButtonClicked(Object Sender, RoutedEventArgs E)
			{
			var Form = new FormAbout();
			Form.Show();
			}

		private void OnManualButtonClicked(Object Sender, RoutedEventArgs E)
			{
			MainData.OpenManual();
			}

		private void OnStartersGuideButtonClicked(Object Sender, RoutedEventArgs E)
			{
			MainData.OpenStartersGuide();
			}

		private void OnRouteGuidesButtonClicked(Object Sender, RoutedEventArgs E)
			{
			var Form = new FormRouteGuides(CTSWOptions.ManualsFolder+"RouteGuides\\");
			Form.Show();
			}

		#endregion

		#region Closing

		private void OnOkButtonClicked(Object Sender, RoutedEventArgs E)
			{
			Close();
			}


    #endregion

 
    }
	}
