using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace TSWTools
  {
  public class CTSWOptions
		{
		// reg key 	HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\railsimulator.com\railworks\install_path
		// Number of read only constants, that depend on the version
		public static Boolean
			AllowDevMode = false; // set to false to completely disable DevMode for users

		public static String TSWToolsManualVersion { get; } = "v0.6";

		private static Boolean _PropDeveloperMode = false;
		private static Boolean _PropTestMode = false;
		private static String _PropTextEditor = String.Empty;
		private static String _PropXmlEditor = String.Empty;
		private static String _PropSevenZip = String.Empty;
		private static String _PropFileCompare = String.Empty;
		private static String _PropRegkeyString = String.Empty;
		private static String _PropSteamRegKey = String.Empty;
		private static String _PropTrainSimWorldDirectory = String.Empty;
		private static String _PropSteamProgramDirectory = String.Empty;
		private static String _PropSteamUserId = String.Empty;
		private static String _PropUnpacker = String.Empty;

		private static String
			_PropAssetUnpacker = String.Empty; // Unpack tool for UAsset files, must be UModel 

		private static String _PropTSWToolsFolder = String.Empty;
		private static Boolean _PropIsInitialized = false;

		protected static RegistryKey AppKey = null;
		protected static RegistryKey SteamKey = null;

		// Properties will be initialized programmatically
		public static String ManualsFolder { get; set; }
		public static String LiveriesFolder { get; set; }
		public static String UnpackFolder { get; set; }
		public static String AssetUnpackFolder { get; set; }
		public static String TempFolder { get; set; }
		public static String BackupFolder { get; set; }

		public static Boolean NotFirstRun { get; set; }

		public static String Version { get; } = "Version 0.6 alpha";

    public static String GetGameSaveLocation()
      {
      var MyPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
      MyPath += @"\My Games\TrainSimWorld2\";
      return MyPath;
      }

    public static string GameSaveLocation
      {
      get
        {
        return GetGameSaveLocation();
        }
      }

		// static holder for instance, need to use lambda to construct since constructor private
		// ReSharper disable once InconsistentNaming
		private static readonly Lazy<CTSWOptions> _instance
			= new Lazy<CTSWOptions>(() => new CTSWOptions());

		// private to prevent direct instantiation.

		private CTSWOptions()
			{
			}

		// accessor for instance

		public static CTSWOptions Instance
			{
			get
				{
				ReadFromRegistry();
				return _instance.Value;
				}
			}

		#region Properties

		public static String SavedScreenshots
			{
			get
				{
				return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
				       + "\\My Games\\TrainsimWorld2\\Saved\\Screenshots\\WindowsNoEditor";
				}
			}

		public static String TSWSettingsDirectory
			{
			get
				{
				return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
				       + "\\My Games\\TrainsimWorld2\\Saved\\Config\\WindowsNoEditor";
				}
			}

		public static String SteamProgramDirectory
			{
			set { _PropSteamProgramDirectory = value; }

			get
				{
				ReadFromRegistry();
				if (TestMode) // append test folder to default directory if test mode has been selected
					{
					return _PropSteamProgramDirectory;
					}
				else
					{
					return _PropSteamProgramDirectory;
					}
				}
			}

		public static String SteamUserId
			{
			set { _PropSteamUserId = value; }
			get
				{
				ReadFromRegistry();
				return _PropSteamUserId;
				}
			}

		public static String SavedSteamScreenshots
			{
			get
				{
				return SteamProgramDirectory + "\\userdata\\" + SteamUserId +
							 "\\760\\remote\\1282590\\screenshots";
				}
			}

		public static String RegkeyString
			{
			get
				{
				_PropRegkeyString = "software\\" + AssemblyInfoProvider.AssemblyCompany + "\\" +
				                    AssemblyInfoProvider.AssemblyProduct;
				return _PropRegkeyString;
				}
			}

		public static String SteamKeyString
			{
			get
				{
				_PropSteamRegKey = "Software\\Valve\\Steam";
				return _PropSteamRegKey;
				}
			}

		public static String TrainSimWorldDirectory
			{
			set { _PropTrainSimWorldDirectory = value; }
			get
				{
				ReadFromRegistry();
				if (TestMode) // append test folder to default directory if test mode has been selected
					{
					return _PropTrainSimWorldDirectory;
					}
				else
					{
					return _PropTrainSimWorldDirectory;
					}
				}
			}

		public static String TSWToolsFolder
			{
			set { _PropTSWToolsFolder = value; }
			get
				{
				ReadFromRegistry();
				return _PropTSWToolsFolder;
				}
			}

		private static String _InstallDirectory;

		public static String InstallDirectory
			{
			set { _InstallDirectory = value; }
			get
				{
				ReadFromRegistry();
				return _InstallDirectory;
				}
			}

		public static String OptionsSetDir
			{
			get { return TSWToolsFolder + "OptionsSets\\"; }
			}

		public static Boolean IsInitialized
			{
			set { _PropIsInitialized = value; }
			get
				{
				ReadFromRegistry();
				return _PropIsInitialized;
				}
			}

		public static String XmlEditor
			{
			set { _PropXmlEditor = value; }
			get
				{
				ReadFromRegistry();
				return _PropXmlEditor;
				}
			}

		public static String TextEditor
			{
			set { _PropTextEditor = value; }
			get
				{
				ReadFromRegistry();
				return _PropTextEditor;
				}
			}

		public static String Unpacker
			{
			set { _PropUnpacker = value; }
			get
				{
				ReadFromRegistry();
				return _PropUnpacker;
				}
			}

		public static String UAssetUnpacker
			{
			set { _PropAssetUnpacker = value; }
			get
				{
				ReadFromRegistry();
				return _PropAssetUnpacker;
				}
			}

		public static String FileCompare
			{
			set { _PropFileCompare = value; }
			get
				{
				ReadFromRegistry();
				return _PropFileCompare;
				}
			}

		public static String SevenZip
			{
			set { _PropSevenZip = value; }
			get
				{
				ReadFromRegistry();
				return _PropSevenZip;
				}
			}

		public static Boolean DeveloperMode
			{
			set { _PropDeveloperMode = value; }
			get
				{
				if (AllowDevMode
					) // this allows to prevent reading the DevMode option from the registry, forcing devmode to false
					{
					ReadFromRegistry();
					return _PropDeveloperMode;
					}
				else
					{
					return false;
					}
				}
			}

		public static Boolean TestMode
			{
			set { _PropTestMode = value; }
			get
				{
				ReadFromRegistry();
				return _PropTestMode;
				}
			}

		public static Boolean PropDeveloperMode1
			{
			get { return _PropDeveloperMode; }

			set { _PropDeveloperMode = value; }
			}

		public static String Installer { get; set; } = String.Empty;

		public static Boolean UseAdvancedSettings { get; set; } = false;

		#endregion

		private static RegistryKey OpenRegistry()
			{
			return Registry.CurrentUser.CreateSubKey(RegkeyString, true);
			}

		// try to guess the correct user for screenshots by having a look at the file system.

		public static void GuessUserId(String SteamProgramPath)
			{
			String BasePath = SteamProgramPath + "\\userdata";
			if (Directory.Exists(BasePath))
				{
				DirectoryInfo Basedir = new DirectoryInfo(BasePath);
				DirectoryInfo[] Dirinfo = Basedir.GetDirectories();

				foreach (var Dir in Dirinfo)
					{
					if (String.CompareOrdinal(Dir.Name, _PropSteamUserId) == 0)
						return; // found, nothing to do
					}

				foreach (var Dir in Dirinfo)
					{
					_PropSteamUserId = Dir.Name;
					AppKey.SetValue("SteamUserId", _PropSteamUserId,
						RegistryValueKind.String); // set it in the registry right away.
					return; // pick the first one
					}
				}
			}

		public static void CreateDirectories()
			{
			ManualsFolder = TSWToolsFolder + "Manuals\\";
			var RouteGuidesFolder = ManualsFolder + "RouteGuides\\";
			LiveriesFolder = TSWToolsFolder + "Liveries\\";
			UnpackFolder = TSWToolsFolder + "Unpack\\";
			AssetUnpackFolder = TSWToolsFolder + "Unpack\\UnpackedAssets";
			TempFolder = TSWToolsFolder + "Temp\\";
			BackupFolder = TSWToolsFolder + "Backup\\";
			try
				{
				Directory.CreateDirectory(ManualsFolder);
				Directory.CreateDirectory(RouteGuidesFolder);
				Directory.CreateDirectory(TempFolder);
				Directory.CreateDirectory(LiveriesFolder);
				Directory.CreateDirectory(UnpackFolder);
				Directory.CreateDirectory(AssetUnpackFolder);
				Directory.CreateDirectory(OptionsSetDir);
				Directory.CreateDirectory(BackupFolder);
				}
			catch (Exception E)
				{
				CLog.Trace("Error creating directories because " + E.Message, LogEventType.Error);
				}
			}

		public static void MoveManuals()
			{
			var SourceDir = InstallDirectory;
			try
				{
				File.Copy(SourceDir + "Manuals\\ToolkitForTSW Manual " + CTSWOptions.TSWToolsManualVersion+".pdf",
					TSWToolsFolder + "ToolkitForTSW Manual " + CTSWOptions.TSWToolsManualVersion + ".pdf", true);
				File.Copy(SourceDir + "Manuals\\TSW Starters guide.pdf", TSWToolsFolder + "TSW Starters guide.pdf",
					true);
				File.Copy(SourceDir + "Manuals\\License information.pdf", TSWToolsFolder + "License information.pdf",
					true);
				}
			catch (Exception E)
				{
				CLog.Trace("Error installing manual files because " + E.Message);
				}
			}

		public static void ReadFromRegistry()
			{
			if (AppKey == null)
				{
				AppKey = OpenRegistry();
				SteamKey = Registry.CurrentUser.CreateSubKey(SteamKeyString, true);
				}

			_InstallDirectory = (String) AppKey.GetValue("InstallDirectory", "");
			_PropTrainSimWorldDirectory = (String) AppKey.GetValue("TrainSimWorldDirectory", "");
			String DefaultSteamProgramPath = (String) AppKey.GetValue("SteamPath", "");
			_PropSteamProgramDirectory =
				(String) AppKey.GetValue("SteamProgramDirectory", DefaultSteamProgramPath);
			_PropSteamUserId = (String) AppKey.GetValue("SteamUserId", "");
			_PropTSWToolsFolder = (String) AppKey.GetValue("TSWToolsFolder", "");

			_PropTextEditor = (String) AppKey.GetValue("TextEditor", "");
			_PropXmlEditor = (String) AppKey.GetValue("XMLEditor", "");
			_PropUnpacker = (String) AppKey.GetValue("Unpacker", "");
			_PropAssetUnpacker = (String) AppKey.GetValue("UAssetUnpacker", "");
			_PropFileCompare = (String) AppKey.GetValue("FileCompare", "");

			_PropSevenZip = (String) AppKey.GetValue("7Zip", "");

			//PropTestMode = ((int)AppKey.GetValue("TestMode", 0) == 1);
			_PropTestMode = false;
			_PropIsInitialized = ((Int32) AppKey.GetValue("Initialized", 0) == 1);

			if (!AllowDevMode)
				{
				_PropDeveloperMode = ((Int32) AppKey.GetValue("DeveloperMode", 0) == 1);
				}

			UseAdvancedSettings = (Boolean) AppKey.GetValue("UseAdvacendSettings", true);
			}

		public static void WriteToRegistry()
			{
			if (AppKey == null)
				{
				AppKey = OpenRegistry();
				}

			AppKey.SetValue("TrainSimWorldDirectory", _PropTrainSimWorldDirectory,
				RegistryValueKind.String);
			AppKey.SetValue("SteamProgramDirectory", _PropSteamProgramDirectory,
				RegistryValueKind.String);
			AppKey.SetValue("SteamUserId", _PropSteamUserId, RegistryValueKind.String);
			AppKey.SetValue("TSWToolsFolder", _PropTSWToolsFolder, RegistryValueKind.String);
			AppKey.SetValue("TextEditor", _PropTextEditor, RegistryValueKind.String);
			AppKey.SetValue("XMLEditor", _PropXmlEditor, RegistryValueKind.String);
			AppKey.SetValue("Unpacker", _PropUnpacker, RegistryValueKind.String);
			AppKey.SetValue("UAssetUnpacker", _PropAssetUnpacker, RegistryValueKind.String);
			AppKey.SetValue("7Zip", _PropSevenZip, RegistryValueKind.String);
			AppKey.SetValue("FileCompare", _PropFileCompare, RegistryValueKind.String);
			// AppKey.SetValue("TestMode", PropTestMode, RegistryValueKind.DWord);
			AppKey.SetValue("DeveloperMode", _PropDeveloperMode, RegistryValueKind.DWord);
			AppKey.SetValue("Initialized", _PropIsInitialized, RegistryValueKind.DWord);
			AppKey.SetValue("UseAdvancedSettings", UseAdvancedSettings, RegistryValueKind.DWord);
			}

		public static Boolean GetNotFirstRun()
			{
			var AppKey2 = OpenRegistry();
			NotFirstRun = ((Int32) AppKey2.GetValue("NotFirstRun", 0) == 1);
			return NotFirstRun;
			}

		public static void SetNotFirstRun()
			{
			var AppKey2 = OpenRegistry();
			AppKey2.SetValue("NotFirstRun", true, RegistryValueKind.DWord);
			}

		public static void TestNotFirstRun()
			{
			var AppKey2 = OpenRegistry();
			AppKey2.SetValue("NotFirstRun", false, RegistryValueKind.DWord);
			}

		public static void UpdateTSWToolsDirectory(String InitialInstallDirectory)
			{
			if (String.CompareOrdinal(InitialInstallDirectory, TSWToolsFolder) != 0)
				{
				// try moving files
				if (!Directory.Exists(TSWToolsFolder) && Directory.Exists(InitialInstallDirectory))
					{
					Directory.Move(InitialInstallDirectory, TSWToolsFolder);
					SetNotFirstRun();
					return;
					}

				if (Directory.Exists(TSWToolsFolder) && Directory.Exists(InitialInstallDirectory))
					{
					CApps.CopyDir(InitialInstallDirectory, TSWToolsFolder, true);
					SetNotFirstRun();
					return;
					}

				if (!Directory.Exists(TSWToolsFolder) && Directory.Exists(InitialInstallDirectory))
					{
					MessageBox.Show(@"Please complete the configuration before you proceed",
						@"Set configuration",
						MessageBoxButton.OK, MessageBoxImage.Asterisk);
					SetNotFirstRun();
					MessageBox.Show(@"Something went wrong, I cannot find the ToolkitForTSW folder structure",
						@"Something went wrong",
						MessageBoxButton.OK, MessageBoxImage.Error);
					return;
					}
				}
			else
				{
				SetNotFirstRun();
				}
			}
		}
	}
