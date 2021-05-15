using Logging.Library;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using Utilities.Library;
using Utilities.Library.Zip;
using MessageBox = System.Windows.MessageBox;

namespace ToolkitForTSW
  {
  public class CTSWOptions
		{
		// Number of read only constants, that depend on the version
		public static bool AllowDevMode = false; // set to false to completely disable DevMode for users

	  private static bool _DeveloperMode = false;
		private static bool _TestMode = false;
		private static string _TextEditor = string.Empty;
		private static string _XmlEditor = string.Empty;
		private static string _SevenZip = string.Empty;
		private static string _FileCompare = string.Empty;
		private static string _RegkeyString = string.Empty;
		private static string _SteamRegKey = string.Empty;
		private static string _TrainSimWorldDirectory = string.Empty;
		private static string _SteamProgramDirectory = string.Empty;
		private static string _SteamUserId = string.Empty;
		private static string _Unpacker = string.Empty;

		private static string
      _AssetUnpacker = string.Empty; // Unpack tool for UAsset files, must be UModel 

		private static string _TSWToolsFolder = string.Empty;
		private static bool _IsInitialized = false;

		protected static RegistryKey AppKey = null;
		protected static RegistryKey SteamKey = null;

		// Properties will be initialized programmatically
		public static string ManualsFolder { get; set; }
		public static string ModsFolder { get; set; }
		public static string UnpackFolder { get; set; }
		public static string AssetUnpackFolder { get; set; }
		public static string TempFolder { get; set; }
		public static string BackupFolder { get; set; }
    public static string TemplateFolder { get; set; }
    public static string ScenarioFolder { get; set; }
    public static string ThumbnailFolder { get; set; }
		public static bool NotFirstRun { get; set; }

		public static string Version { get; } = "Version 0.8 beta";

    public static string GetGameSaveLocation()
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

		public static string SavedScreenshots
			{
			get
				{
				return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
				       + "\\My Games\\TrainsimWorld2\\Saved\\Screenshots\\WindowsNoEditor";
				}
			}

		public static string TSWSettingsDirectory
			{
			get
				{
				return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
				       + "\\My Games\\TrainsimWorld2\\Saved\\Config\\WindowsNoEditor";
				}
			}

		public static string SteamProgramDirectory
			{
			set { _SteamProgramDirectory = value; }

			get
				{
				ReadFromRegistry();
				if (TestMode) // append test folder to default directory if test mode has been selected
					{
					return _SteamProgramDirectory;
					}
				else
					{
					return _SteamProgramDirectory;
					}
				}
			}

		public static string SteamUserId
			{
			set { _SteamUserId = value; }
			get
				{
				ReadFromRegistry();
				return _SteamUserId;
				}
			}

		public static string SavedSteamScreenshots
			{
			get
				{
				return SteamProgramDirectory + "\\userdata\\" + SteamUserId +
							 "\\760\\remote\\1282590\\screenshots";
				}
			}

		public static string RegkeyString
			{
			get
				{
				_RegkeyString = "software\\" + AssemblyInfoProvider.AssemblyCompany + "\\" +
				                    AssemblyInfoProvider.AssemblyProduct;
				return _RegkeyString;
				}
			}

		public static string SteamKeyString
			{
			get
				{
				_SteamRegKey = "Software\\Valve\\Steam";
				return _SteamRegKey;
				}
			}

		public static string TrainSimWorldDirectory
			{
			set { _TrainSimWorldDirectory = value; }
			get
				{
				ReadFromRegistry();
				if (TestMode) // append test folder to default directory if test mode has been selected
					{
					return _TrainSimWorldDirectory;
					}
				else
					{
					return _TrainSimWorldDirectory;
					}
				}
			}

		public static string TSWToolsFolder
			{
			set { _TSWToolsFolder = value; }
			get
				{
				ReadFromRegistry();
				return _TSWToolsFolder;
				}
			}

		private static string _InstallDirectory;

		public static string InstallDirectory
			{
			set { _InstallDirectory = value; }
			get
				{
				ReadFromRegistry();
				return _InstallDirectory;
				}
			}

		public static string OptionsSetDir
			{
			get { return TSWToolsFolder + "OptionsSets\\"; }
			}

		public static bool IsInitialized
			{
			set { _IsInitialized = value; }
			get
				{
				ReadFromRegistry();
				return _IsInitialized;
				}
			}

		public static string XmlEditor
			{
			set { _XmlEditor = value; }
			get
				{
				ReadFromRegistry();
				return _XmlEditor;
				}
			}

		public static string TextEditor
			{
			set { _TextEditor = value; }
			get
				{
				ReadFromRegistry();
				return _TextEditor;
				}
			}

		public static string Unpacker
			{
			set { _Unpacker = value; }
			get
				{
				ReadFromRegistry();
				return _Unpacker;
				}
			}

		public static string UAssetUnpacker
			{
			set { _AssetUnpacker = value; }
			get
				{
				ReadFromRegistry();
				return _AssetUnpacker;
				}
			}

		public static string FileCompare
			{
			set { _FileCompare = value; }
			get
				{
				ReadFromRegistry();
				return _FileCompare;
				}
			}

		public static string SevenZip
			{
			set { _SevenZip = value; }
			get
				{
				ReadFromRegistry();
				return _SevenZip;
				}
			}

		public static bool DeveloperMode
			{
			set { _DeveloperMode = value; }
			get
				{
				if (AllowDevMode
					) // this allows to prevent reading the DevMode option from the registry, forcing devmode to false
					{
					ReadFromRegistry();
					return _DeveloperMode;
					}
				else
					{
					return false;
					}
				}
			}

    public static bool UseAdvancedSettings { get; set; }
    public static bool LimitSoundVolumes { get; set; }

    public static bool TestMode
			{
			set { _TestMode = value; }
			get
				{
				ReadFromRegistry();
				return _TestMode;
				}
			}

		public static bool DeveloperMode1
			{
			get { return _DeveloperMode; }

			set { _DeveloperMode = value; }
			}

		public static string Installer { get; set; } = string.Empty;


		#endregion

		private static RegistryKey OpenRegistry()
			{
			return Registry.CurrentUser.CreateSubKey(RegkeyString, true);
			}

		// try to guess the correct user for screenshots by having a look at the file system.

		public static void GuessUserId(string SteamProgramPath)
			{
      string BasePath = SteamProgramPath + "\\userdata";
			if (Directory.Exists(BasePath))
				{
				DirectoryInfo Basedir = new DirectoryInfo(BasePath);
				DirectoryInfo[] Dirinfo = Basedir.GetDirectories();

				foreach (var Dir in Dirinfo)
					{
					if (string.CompareOrdinal(Dir.Name, _SteamUserId) == 0)
						return; // found, nothing to do
					}

				foreach (var Dir in Dirinfo)
					{
					_SteamUserId = Dir.Name;
					AppKey.SetValue("SteamUserId", _SteamUserId,
						RegistryValueKind.String); // set it in the registry right away.
					return; // pick the first one
					}
				}
			}

		public static void CreateDirectories()
			{
			ManualsFolder = TSWToolsFolder + "Manuals\\";
			var RouteGuidesFolder = ManualsFolder + "RouteGuides\\";
			ModsFolder = TSWToolsFolder + "Mods\\";
			UnpackFolder = TSWToolsFolder + "Unpack\\";
			AssetUnpackFolder = TSWToolsFolder + "Unpack\\UnpackedAssets";
			TempFolder = TSWToolsFolder + "Temp\\";
			BackupFolder = TSWToolsFolder + "Backup\\";
			TemplateFolder= TSWToolsFolder + "Templates\\";
			ScenarioFolder= TSWToolsFolder + "Scenarios\\";
      ThumbnailFolder = TSWToolsFolder + "Thumbnails\\";
			try
				{
				Directory.CreateDirectory(ManualsFolder);
				Directory.CreateDirectory(RouteGuidesFolder);
				Directory.CreateDirectory(TempFolder);
				Directory.CreateDirectory(ModsFolder);
				Directory.CreateDirectory(UnpackFolder);
				Directory.CreateDirectory(AssetUnpackFolder);
				Directory.CreateDirectory(OptionsSetDir);
				Directory.CreateDirectory(BackupFolder);
        Directory.CreateDirectory(TemplateFolder);
        Directory.CreateDirectory(ScenarioFolder);
        Directory.CreateDirectory(ThumbnailFolder);
				}
			catch (Exception E)
				{
				Log.Trace("Error creating directories because " + E.Message, LogEventType.Error);
				}
			}

		public static void CopyManuals()
			{
			var SourceDir = InstallDirectory;
			try
				{
				File.Copy(SourceDir + "Manuals\\ToolkitForTSW Manual.pdf",
          ManualsFolder + "ToolkitForTSW Manual.pdf", true);
				File.Copy(SourceDir + "Manuals\\TSW2 Starters guide.pdf", ManualsFolder + "TSW2 Starters guide.pdf",
					true);
				File.Copy(SourceDir + "Manuals\\License information.pdf", ManualsFolder + "License information.pdf",
					true);
				}
			catch (Exception E)
				{
				Log.Trace("Error installing manual files because " + E.Message);
				}
			}

		public static void ReadFromRegistry()
			{
			if (AppKey == null)
				{
				AppKey = OpenRegistry();
				SteamKey = Registry.CurrentUser.CreateSubKey(SteamKeyString, true);
				}

			_InstallDirectory = (string) AppKey.GetValue("InstallDirectory", "");
			_TrainSimWorldDirectory = (string) AppKey.GetValue("TrainSimWorldDirectory", "");
      string DefaultSteamProgramPath = (string) AppKey.GetValue("SteamPath", "");
			_SteamProgramDirectory =
				(string) AppKey.GetValue("SteamProgramDirectory", DefaultSteamProgramPath);
			_SteamUserId = (string) AppKey.GetValue("SteamUserId", "");
			_TSWToolsFolder = (string) AppKey.GetValue("TSWToolsFolder", "");

			_TextEditor = (string) AppKey.GetValue("TextEditor", "");
			_XmlEditor = (string) AppKey.GetValue("XMLEditor", "");
			_Unpacker = (string) AppKey.GetValue("Unpacker", "");
			_AssetUnpacker = (string) AppKey.GetValue("UAssetUnpacker", "");
			_FileCompare = (string) AppKey.GetValue("FileCompare", "");

			_SevenZip = (string) AppKey.GetValue("7Zip", "");
			SevenZipLib.InitZip(_SevenZip);
			//TestMode = ((int)AppKey.GetValue("TestMode", 0) == 1);
			_TestMode = false;
			_IsInitialized = ((int) AppKey.GetValue("Initialized", 0) == 1);

			if (!AllowDevMode)
				{
				_DeveloperMode = ((int) AppKey.GetValue("DeveloperMode", 0) == 1);
				}

			UseAdvancedSettings = (int)AppKey.GetValue("UseAdvancedSettings", 1)==1;
      LimitSoundVolumes = (int)AppKey.GetValue("LimitSoundVolumes", 1)==1;

			}

		public static void WriteToRegistry()
			{
			if (AppKey == null)
				{
				AppKey = OpenRegistry();
				}

			AppKey.SetValue("TrainSimWorldDirectory", _TrainSimWorldDirectory,
				RegistryValueKind.String);
			AppKey.SetValue("SteamProgramDirectory", _SteamProgramDirectory,
				RegistryValueKind.String);
			AppKey.SetValue("SteamUserId", _SteamUserId, RegistryValueKind.String);
			AppKey.SetValue("TSWToolsFolder", _TSWToolsFolder, RegistryValueKind.String);
			AppKey.SetValue("TextEditor", _TextEditor, RegistryValueKind.String);
			AppKey.SetValue("XMLEditor", _XmlEditor, RegistryValueKind.String);
			AppKey.SetValue("Unpacker", _Unpacker, RegistryValueKind.String);
			AppKey.SetValue("UAssetUnpacker", _AssetUnpacker, RegistryValueKind.String);
			AppKey.SetValue("7Zip", _SevenZip, RegistryValueKind.String);
			AppKey.SetValue("FileCompare", _FileCompare, RegistryValueKind.String);
			// AppKey.SetValue("TestMode", TestMode, RegistryValueKind.DWord);
			AppKey.SetValue("DeveloperMode", _DeveloperMode, RegistryValueKind.DWord);
			AppKey.SetValue("Initialized", _IsInitialized, RegistryValueKind.DWord);
			AppKey.SetValue("UseAdvancedSettings", UseAdvancedSettings, RegistryValueKind.DWord);
      AppKey.SetValue("LimitSoundVolumes", LimitSoundVolumes, RegistryValueKind.DWord);
			}

		public static bool GetNotFirstRun()
			{
			var AppKey2 = OpenRegistry();
      if (AppKey2 != null)
        {
        NotFirstRun = (int) AppKey2.GetValue("NotFirstRun", 0) == 1;
        }

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

		public static void UpdateTSWToolsDirectory(string InitialInstallDirectory)
			{
			if (string.CompareOrdinal(InitialInstallDirectory, TSWToolsFolder) != 0)
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
					FileHelpers.CopyDir(InitialInstallDirectory, TSWToolsFolder, true);
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
