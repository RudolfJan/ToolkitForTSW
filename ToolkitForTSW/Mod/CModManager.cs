using Logging.Library;
using Styles.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows.Controls;
using ToolkitForTSW.DataAccess;
using ToolkitForTSW.Models;

namespace ToolkitForTSW.Mod
  {
  public enum ModTypesEnum

		{
		[Description("Undefined")] Undefined,
		[Description("Engine")] Engine,
		[Description("Wagon")] Wagon,
		[Description("Consist")] Consist,
		[Description("Scenery")] Scenery,
		[Description("Route")] Route,
		[Description("Scenario")] Scenario,
		[Description("Service timetable")] Service,
		[Description("Weather")] Weather,
    [Description("Game")] Game,
		[Description("Other")] Other
		};

	public class CModManager : Notifier
		{
    private List<ModModel> _AvailableModList= new List<ModModel>();
    public List<ModModel> AvailableModList
      {
      get { return _AvailableModList; }
      set
        {
        _AvailableModList = value;
        OnPropertyChanged("AvailableModList");
        }
      }
 

		/*
		List with all (DLC) Pak files. 
		*/
		private ObservableCollection<FileInfo> _PakFilesList;
		public ObservableCollection<FileInfo> PakFilesList
			{
			get { return _PakFilesList; }
			set
				{
				_PakFilesList = value;
				OnPropertyChanged("PakFilesList");
				}
			}

	  private String _Result = String.Empty;
		public String Result
			{
			get { return _Result; }
			set
				{
				_Result = value;
				OnPropertyChanged("Result");
				}
			}

    private CModSet _modSet;
    public CModSet ModSet
      {
      get { return _modSet; }
      set
        {
        _modSet = value;
        OnPropertyChanged("ModSet");
        }
      }

 
    private string _modName;
    public string ModName
      {
      get { return _modName; }
      set
        {
        _modName = value;
        OnPropertyChanged("ModName");
        }
      }

    private string _filePath;
    public string FilePath
      {
      get { return _filePath; }
      set
        {
        _filePath = value;
        OnPropertyChanged("FilePath");
        }
      }
   
    private string _fileName;
    public string FileName
      {
      get { return _fileName; }
      set
        {
        _fileName = value;
        OnPropertyChanged("FileName");
        }
      }

    private string _modDescription;
    public string ModDescription
      {
      get { return _modDescription; }
      set
        {
        _modDescription = value;
        OnPropertyChanged("ModDescription");
        }
      }

    private string _modImage;
    public string ModImage
      {
      get { return _modImage; }
      set
        {
        _modImage = value;
        OnPropertyChanged("ModImage");
        }
      }

    private string _modSource;
    public string ModSource
      {
      get { return _modSource; }
      set
        {
        _modSource = value;
        OnPropertyChanged("ModSource");
        }
      }

    private ModTypesEnum _modType;
    public ModTypesEnum ModType
      {
      get { return _modType; }
      set
        {
        _modType = value;
        OnPropertyChanged("ModType");
        }
      }

    private string _DLCName;
    public string DLCName
      {
      get { return _DLCName; }
      set
        {
        _DLCName = value;
        OnPropertyChanged("DLCName");
        }
      }

    private ModModel _SelectedMod;

    public ModModel SelectedMod
      {
      get { return _SelectedMod; }
      set
        {
        _SelectedMod = value;
        OnPropertyChanged("SelectedMod");
        }
      }

    private bool _IsInstalled;
    public bool IsInstalled
      {
      get { return _IsInstalled; }
      set
        {
        _IsInstalled = value;
        OnPropertyChanged("IsInstalled");
        }
      }

    private bool _InEditMode;
    public bool InEditMode
      {
      get { return _InEditMode; }
      set
        {
        _InEditMode = value;
        OnPropertyChanged("InEditMode");
        }
      }


    public CModManager()
			{
   
			Initialise();
			}

		private void Initialise()
			{
      PakFilesList = new ObservableCollection<FileInfo>();
      AvailableModList = ModDataAccess.GetAllMods();
      ModSet = new CModSet(AvailableModList);
      GetPakFiles();
      GetInstalledPakFiles();
			}

    public void DeactivateAllInstalledPaks()
      {
			foreach (var X in AvailableModList)
        {
        if (X.IsInstalled)
          {
          var filePath = X.FileName;
          if (!String.IsNullOrEmpty(filePath))
            {
            FilePath = CTSWOptions.TrainSimWorldDirectory + @"TS2Prototype\Content\DLC\" + filePath;
            CApps.DeleteSingleFile(filePath);
            X.IsInstalled=false;
            }
          }
        }
      }

    private void GetPakFiles()
			{
			var BaseDir = new DirectoryInfo(CTSWOptions.ModsFolder);
			FileInfo[] Files = BaseDir.GetFiles("*.pak", SearchOption.AllDirectories);
			PakFilesList.Clear();
      AvailableModList.Clear();
      foreach (var F in Files)
        {
        PakFilesList.Add(F);
        var mod= ModDataAccess.UpsertMod(StripModDir(F.FullName));
        AvailableModList.Add(mod);
        }
      }

    private void GetInstalledPakFiles()
			{
			var BaseDir =
				new DirectoryInfo(CTSWOptions.TrainSimWorldDirectory + "TS2Prototype\\Content\\DLC");
			FileInfo[] Files = BaseDir.GetFiles("*.pak", SearchOption.AllDirectories);
			foreach (var F in Files)
        {
        SetInstallationStatus(F);
        }
			}

    private void SetInstallationStatus(FileInfo f)
      {
      var fileName = f.Name;
      foreach (var mod in AvailableModList)
        {
        if (string.CompareOrdinal(fileName, mod.FileName) == 0)
          {
          mod.IsInstalled=true;
          }
        }
      }

    public static String StripModDir(String Input)
			{
			if (Input.StartsWith(CTSWOptions.ModsFolder))
				{
				return Input.Substring(CTSWOptions.ModsFolder.Length);
				}
			return String.Empty;
			}
 
    public void ActivatePak()
      {
      var PakPath = SelectedMod.FilePath;
			var source = CTSWOptions.ModsFolder + PakPath;
			var fileName = Path.GetFileName(source);
			var destination = CTSWOptions.TrainSimWorldDirectory + "TS2Prototype\\Content\\DLC\\" +
			                  fileName;
			try
				{
				File.Copy(source, destination, false);
				var F = new FileInfo(destination);
        if (InEditMode)
          {
          IsInstalled=true;
          }
        SelectedMod.IsInstalled=true;
				}
			catch (Exception E)
				{
				Result += Log.Trace("Failed to install mod pak because " + E.Message,
					LogEventType.Error);
				}
			}

		public void DeactivatePak()
			{
			var filePath = new FileInfo(CTSWOptions.TrainSimWorldDirectory +
			                            "TS2Prototype\\Content\\DLC\\" + SelectedMod.FileName);
			Result += CApps.DeleteSingleFile(filePath.FullName);
	    SelectedMod.IsInstalled=false;
      if (InEditMode)
        {
        IsInstalled = false;
        }
      }

    public static string ActivateMod(ModModel mod)
      {
      var PakPath = mod.FilePath;
      var source = CTSWOptions.ModsFolder + PakPath;
      var fileName = Path.GetFileName(source);
      var destination = CTSWOptions.TrainSimWorldDirectory + "TS2Prototype\\Content\\DLC\\" +
                        fileName;
      try
        {
        File.Copy(source, destination, true);
        return string.Empty;
        }
      catch (Exception E)
        {
        return Log.Trace("Failed to install mod pak because " + E.Message,
          LogEventType.Error);
        }
      }

    public void EditModProperties()
      {
      ModName = SelectedMod.ModName;
      ModDescription = SelectedMod.ModDescription;
      ModSource = SelectedMod.ModSource;
      ModType = SelectedMod.ModType;
      DLCName = SelectedMod.DLCName;
      FileName = SelectedMod.FileName;
      FilePath=SelectedMod.FilePath;
      IsInstalled = SelectedMod.IsInstalled;
      InEditMode=true;
      }

    public void SaveModProperties()
      {
      SelectedMod.ModName= ModName;
      SelectedMod.ModDescription=ModDescription;
      SelectedMod.ModSource= ModSource;
      SelectedMod.DLCName= DLCName;
      SelectedMod.ModType=ModType;
      ModDataAccess.UpdateMod(SelectedMod);
      // Note FileName and FilePath should never be updated!
      }

    public static string UpdateModTable(FileInfo F, Boolean IsInstalled=false)
      {
      var mod = ModDataAccess.UpsertMod(StripModDir(F.FullName));
      return "";
      }

    public static string UpdateModTable(ModModel mod)
      {
      ModDataAccess.UpsertMod(mod);
      return "";
      }

    internal void DeleteMod()
      {
      if (SelectedMod.IsInstalled)
        {
        DeactivatePak();
        }
      var PakPath = SelectedMod.FilePath;
      var source = CTSWOptions.ModsFolder + PakPath;
      CApps.DeleteSingleFile(source);
      ModDataAccess.DeleteMod(SelectedMod.Id);
      if (InEditMode)
        {
        ClearEdit();
        }
      }

    public void ClearEdit()
      {
      SelectedMod = null;
      ModName = string.Empty;
      ModDescription = string.Empty;
      ModSource = string.Empty;
      ModType = ModTypesEnum.Undefined;
      DLCName = string.Empty;
      FileName = string.Empty;
      FilePath = string.Empty;
      IsInstalled = false;
      InEditMode = false;
      }
		}
	}
