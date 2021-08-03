using Logging.Library;
using System;
using System.Diagnostics;
using System.Windows;
using ToolkitForTSW;
using ToolkitForTSW.Mod;
using ToolkitForTSW.Settings;
using TreeBuilders.Library.Wpf;
using TreeBuilders.Library.Wpf.ViewModels;
using TreeBuilders.Library.Wpf.Views;
using Utilities.Library;
using Utilities.Library.Wpf;
using Utilities.Library.Wpf.ViewModels;
using Utilities.Library.Wpf.Views;

namespace ToolkitForTSW.Views
	{
	
	public partial class ShellView: Window
		{
		public CMain MainData { get; set; }
		public Log LogForm;
		public static LogEventHandler LogEventHandler { get; set; }

		public ShellView()
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

		#endregion

		#region Unpack

    private void OnUnpackerButtonClicked(Object Sender, RoutedEventArgs E)
			{
			var Form = new FormUnpackGameFiles();
			Form.Show();
			}

		private void OnViewUnpackedPaksButtonClicked(Object Sender, RoutedEventArgs E)
			{
			ProcessHelper.OpenFolder(TSWOptions.UnpackFolder);
			}

		private void OnUModelLauncherButtonClicked(Object Sender, RoutedEventArgs E)
			{
			var Form = new FormLaunchUModel();
			Form.Show();
			}
		#endregion

		#region Tools

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
      var Form= new RadioStationsView();
      Form.Show();
      }

    private void OnPakInstallerButtonClicked(Object Sender, RoutedEventArgs E)
      {
      var Form=new FormPakInstaller();
      Form.Show();
      }

    #endregion

    #region Help


  

    private void GetRouteGuides_Click(object sender, RoutedEventArgs e)
      {
			var form= new RouteGuideView();
		  form.Show();
      }
		#endregion
		}
	}
