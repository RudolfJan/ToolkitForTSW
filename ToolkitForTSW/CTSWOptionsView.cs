using Styles.Library.Helpers;
using System;
using System.IO;



namespace ToolkitForTSW
{
	// local copy of all options, to allow explicit saving them
	public class CTSWOptionsView : Notifier
		{
		#region Properties

		/*
Installation folder for Steam program
*/
		private String _SteamProgramDirectory = String.Empty;

		public String SteamProgramDirectory
			{
			get { return _SteamProgramDirectory; }
			set
				{
				_SteamProgramDirectory = value;
				OnPropertyChanged("SteamProgramDirectory");
				}
			}

/*
UserId for steam, needed to retrieve screenshots
*/
		private String _SteamUserId = String.Empty;

		public String SteamUserId
			{
			get { return _SteamUserId; }
			set
				{
				_SteamUserId = value;
				OnPropertyChanged("SteamUserId");
				}
			}

/*
Installation directory for TSW
*/
		private String _TrainSimWorldDirectory = String.Empty;

		public String TrainSimWorldDirectory
			{
			get { return _TrainSimWorldDirectory; }
			set
				{
				_TrainSimWorldDirectory = value;
				OnPropertyChanged("TrainSimWorldDirectory");
				}
			}

		/*
		Installation directory for TSW
		*/
		private String _TrainSimWorldProgram = String.Empty;

		public String TrainSimWorldProgram
			{
			get { return _TrainSimWorldDirectory; }
			set
				{
				_TrainSimWorldProgram = value;
				OnPropertyChanged("TrainSimWorldProgram");
				}
			}



		/*
		Data folder for TSWTools
		*/
		private String _TSWToolsFolder = String.Empty;

		public String TSWToolsFolder
			{
			get { return _TSWToolsFolder; }
			set
				{
				_TSWToolsFolder = value;
				OnPropertyChanged("TSWToolsFolder");
				}
			}

/*
Path th prefeered editor for XML files
*/
		private String _XMLEditor = String.Empty;

		public String XMLEditor
			{
			get { return _XMLEditor; }
			set
				{
				_XMLEditor = value;
				OnPropertyChanged("XMLEditor");
				}
			}

/*
Path to preferred TextEditor
*/
		private String _TextEditor = String.Empty;

		public String TextEditor
			{
			get { return _TextEditor; }
			set
				{
				_TextEditor = value;
				OnPropertyChanged("TextEditor");
				}
			}

/*
Path to preferred unpacker for .pak files
*/
		private String _Unpacker = String.Empty;

		public String Unpacker
			{
			get { return _Unpacker; }
			set
				{
				_Unpacker = value;
				OnPropertyChanged("Unpacker");
				}
			}

/*
Path to preferred UAsset Unpacker
*/
		private String _UAssetUnpacker = String.Empty;

		public String UAssetUnpacker
			{
			get { return _UAssetUnpacker; }
			set
				{
				_UAssetUnpacker = value;
				OnPropertyChanged("UAssetUnpacker");
				}
			}

/*
Path to 7Zip program
*/
		private String _SevenZip = String.Empty;

		public String SevenZip
			{
			get { return _SevenZip; }
			set
				{
				_SevenZip = value;
				OnPropertyChanged("SevenZip");
				}
			}

    private bool _useAdvancedSettings;

    public bool UseAdvancedSettings
      {
      get
        {
        return _useAdvancedSettings;
        }
      set
        {
        _useAdvancedSettings = value;
				OnPropertyChanged("UseAdvancedSettings");
        }
      }

    private bool _limitSoundVolumes;

    public bool LimitSoundVolumes
      {
      get
        {
        return _limitSoundVolumes;
        }
      set
        {
        _limitSoundVolumes = value;
				OnPropertyChanged("LimitSoundVolumes");
        }
      }

		#endregion

		#region Constructor

		public CTSWOptionsView()
			{
			LoadOptions();
			}

		#endregion

		public void LoadOptions()
			{
			CTSWOptions.ReadFromRegistry();
			SteamProgramDirectory = CTSWOptions.SteamProgramDirectory;
			SteamUserId = CTSWOptions.SteamUserId;
			TrainSimWorldDirectory = CTSWOptions.TrainSimWorldDirectory;
			TrainSimWorldProgram = TrainSimWorldDirectory + "TS2Prototype.exe";
			TSWToolsFolder = CTSWOptions.TSWToolsFolder;
			XMLEditor = CTSWOptions.XmlEditor;
			TextEditor = CTSWOptions.TextEditor;
			SevenZip = CTSWOptions.SevenZip;
			Unpacker = CTSWOptions.Unpacker;
			UAssetUnpacker = CTSWOptions.UAssetUnpacker;
      UseAdvancedSettings = CTSWOptions.UseAdvancedSettings;
      LimitSoundVolumes = CTSWOptions.LimitSoundVolumes;
      }

		public void SaveOptions()
			{
			CTSWOptions.SteamProgramDirectory = FixEndSlash(SteamProgramDirectory);
			CTSWOptions.SteamUserId = SteamUserId;
			TrainSimWorldDirectory = Path.GetDirectoryName(TrainSimWorldProgram);
			CTSWOptions.TrainSimWorldDirectory = FixEndSlash(TrainSimWorldDirectory);
			CTSWOptions.TSWToolsFolder =FixEndSlash(TSWToolsFolder);
			CTSWOptions.XmlEditor = XMLEditor;
			CTSWOptions.TextEditor = TextEditor;
			CTSWOptions.SevenZip = SevenZip;
			CTSWOptions.Unpacker = Unpacker;
			CTSWOptions.UAssetUnpacker=UAssetUnpacker;
      CTSWOptions.UseAdvancedSettings = UseAdvancedSettings;
      CTSWOptions.LimitSoundVolumes = LimitSoundVolumes;
			CTSWOptions.WriteToRegistry();
			CTSWOptions.CreateDirectories();
			CTSWOptions.MoveManuals();
		}

		private static String FixEndSlash(String Input)
			{
			if (!Input.EndsWith("\\"))
				{
				return Input + "\\";
				}
			else
				{
				return Input;
				}
			}
		}
	}