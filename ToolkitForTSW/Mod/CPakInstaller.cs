using Logging.Library;
using Styles.Library.Helpers;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using ToolkitForTSW.DataAccess;
using ToolkitForTSW.Models;
using TreeBuilders.Library.Wpf;
using TreeBuilders.Library.Wpf.ViewModels;
using Utilities.Library.Zip;

namespace ToolkitForTSW.Mod
  {
  public class CPakInstaller : Notifier
    {
    private string _ArchiveFile;
    public string ArchiveFile
      {
      get { return _ArchiveFile; }
      set
        {
        _ArchiveFile = value;
        OnPropertyChanged("ArchiveFile");
        }
      }

    private string _InstallDirectory;
    public string InstallDirectory
      {
      get { return _InstallDirectory; }
      set
        {
        _InstallDirectory = value;
        OnPropertyChanged("InstallDirectory");
        }
      }

    private CFilePresenter _FileEntry;
    public CFilePresenter FileEntry
      {
      get { return _FileEntry; }
      set
        {
        _FileEntry = value;
        OnPropertyChanged("FileEntry");
        }
      }

    private FileTreeViewModel _FileTree;

    public FileTreeViewModel FileTree
      {
      get => _FileTree;
      set
        {
        _FileTree = value;
        OnPropertyChanged("FileTree");
        }
      }

    private ObservableCollection<CFilePresenter> _PakFileList;
    public ObservableCollection<CFilePresenter> PakFileList
      {
      get { return _PakFileList; }
      set
        {
        _PakFileList = value;
        OnPropertyChanged("PakFileList");
        }
      }

    private ObservableCollection<CFilePresenter> _DocumentsList;
    public ObservableCollection<CFilePresenter> DocumentsList
      {
      get { return _DocumentsList; }
      set
        {
        _DocumentsList = value;
        OnPropertyChanged("DocumentsList");
        }
      }

    private string _Result;

    public string Result
      {
      get { return _Result; }
      set
        {
        _Result = value;
        OnPropertyChanged("Result");
        }
      }

    private string _modName="";
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

    private string _modDescription="";
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

    private string _modSource="";
    public string ModSource
      {
      get { return _modSource; }
      set
        {
        _modSource = value;
        OnPropertyChanged("ModSource");
        }
      }

    private ModTypesEnum _modType=ModTypesEnum.Undefined;
    public ModTypesEnum ModType
      {
      get { return _modType; }
      set
        {
        _modType = value;
        OnPropertyChanged("ModType");
        }
      }

    private string _DLCName="";
    public string DLCName
      {
      get { return _DLCName; }
      set
        {
        _DLCName = value;
        OnPropertyChanged("DLCName");
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

    public CPakInstaller()
      {
      PakFileList = new ObservableCollection<CFilePresenter>();
      DocumentsList = new ObservableCollection<CFilePresenter>();
      FillPakDirList();
      }

    public void FillPakDirList()
      {
      var Dir = new DirectoryInfo(TSWOptions.ModsFolder);
      FileTree = new FileTreeViewModel(Dir.FullName, true);
      OnPropertyChanged("FileTree");
      }

    public void GetArchivedFiles(FileInfo archiveFile, ObservableCollection<CFilePresenter> DestinationFileList, string FileType = "")
      {
      var Extension = archiveFile.Extension.ToLower();
      if (DestinationFileList == null)
        {
        Result += Log.Trace("File presenter is null", LogEventType.Error);
        return;
        }

      switch (Extension)
        {
        case ".zip":
            {
            GetZipArchivedFiles(archiveFile, DestinationFileList, FileType);
            break;
            }
        case ".rar":
        case ".7z":
            {
            GetRwpArchivedFiles(archiveFile, DestinationFileList, FileType);
            break;
            }
        case ".exe":
            {
            //Empty list
            break;
            }
        case ".pak":
            {
            GetNotArchivedPakFile(archiveFile, DestinationFileList);
            break;
            }
        default:
            {
            Result += Log.Trace("Archive type " + Extension + " is not (yet) supported");
            break;
            }
        }
      }


    // Show .pak file entry if not in archive
    public void GetNotArchivedPakFile(FileInfo archiveFile,
      ObservableCollection<CFilePresenter> DestinationFileList)
      {
      var FilePresenter =
        new CFilePresenter(archiveFile.FullName, archiveFile.Name, archiveFile.LastWriteTime);
      DestinationFileList.Add(FilePresenter);
      DocumentsList.Clear();
      }

    // Show contents of a .zip file
    public void GetZipArchivedFiles(FileInfo archiveFile, ObservableCollection<CFilePresenter> DestinationFileList, string FileType = "")
      {
      try
        {
        if (DestinationFileList == null)
          {
          Result += Log.Trace("File presenter is null", LogEventType.Error);
          return;
          }
        using (var Archive = ZipFile.OpenRead(archiveFile.FullName))
          {
          foreach (var Entry in Archive.Entries)
            {
            if (FileType.Length == 0 ||
                string.CompareOrdinal(Path.GetExtension(Entry.Name), FileType) == 0
              ) // first part makes this work if no filter is specified
              {
              var FilePresenter =
                new CFilePresenter(Entry.FullName, Entry.Name, Entry.LastWriteTime);
              DestinationFileList.Add(FilePresenter);
              }
            }
          }
        }
      catch (Exception)
        {
        Result += Log.Trace("Failed to show file entries for archive " + archiveFile.FullName,
          LogEventType.Error);
        }
      }

    public void GetRwpArchivedFiles(FileInfo archiveFile, ObservableCollection<CFilePresenter> DestinationFileList, string FileType = "")
      {
      GetRwpArchivedFiles(archiveFile.FullName, DestinationFileList, FileType);
      }

    public void GetRwpArchivedFiles(string archiveFile, ObservableCollection<CFilePresenter> DestinationFileList, string FileType)
      {
      SevenZipLib.ListFilesInArchive(ArchiveFile, out var FileReport,true);
      //var Skip = 16;
      //if (string.Compare(Path.GetExtension(archiveFile), ".rar", StringComparison.Ordinal) == 0 ||
      //    string.Compare(Path.GetExtension(archiveFile), ".7z", StringComparison.Ordinal) == 0)
      //  {
      //  Skip = 19;
      //  }

      var Reader = new StringReader(FileReport);
//#pragma warning disable IDE0059 // Unnecessary assignment of a value
//      var MetaData = "";
//#pragma warning restore IDE0059 // Unnecessary assignment of a value
//      for (var I = 0; I < Skip; I++) // tricky!
//        {
//        // ReSharper disable once RedundantAssignment debugging, do not remove this
//        MetaData = Reader.ReadLine() + "\r\n";
//        }

      var Done = false;
      while (!Done)
        {
        var Temp = Reader.ReadLine();
        if (Temp == null || Temp.StartsWith("-----"))
          {
          Done = true;
          }
        else
          {
          if (Temp.Length > 0)
            {
            var FilePresenter = new CFilePresenter();
            FilePresenter.Parse7ZLine(Temp);
            if (FileType.Length == 0 ||
                string.CompareOrdinal(Path.GetExtension(FilePresenter.Name), FileType) == 0)
              {
              DestinationFileList.Add(FilePresenter);
              }
            }
          }
        }
      }

    public void AddDirectory(TreeNodeModel TreeItem, string DirName, bool AsChild)
      {
      try
        {
        string Path;
        if (AsChild)
          {
          Path = TreeItem.Root + "\\" + DirName;
          Directory.CreateDirectory(Path);
          }
        else
          {
          if (TreeItem?.Root == null)
            {
            Path = TSWOptions.ModsFolder + DirName;
            }
          else
            {
            Path = TreeItem.Root + "\\..\\" + DirName;
            }

          Directory.CreateDirectory(Path);
          FillPakDirList();
          }
        }
      catch (Exception E)
        {
        Result += Log.Trace("Cannot create directory because " + E.Message, LogEventType.Error);
        }
      }

    #region Installers

    public void InstallPakFile()
      {
      // Otherwise, check if there is an installable SelectedFIle
      if (FileEntry != null)
        {
        switch (FileEntry.Extension)
          {
          case ".pak":
              {
              if (Path.GetExtension(ArchiveFile) == ".pak")
                {
                File.Copy(FileEntry.FullName, $"{InstallDirectory}\\{FileEntry.Name}", true);
                // Result += CModManager.UpdateModTable(new FileInfo($"{InstallDirectory}\\{FileEntry.Name}"));
                return;
                }
              SevenZipLib.ExtractSingle(ArchiveFile, InstallDirectory, FileEntry.FullName);
              // Result += CModManager.UpdateModTable(new FileInfo($"{InstallDirectory}\\{FileEntry.Name}"));
              return;
              }
          case ".exe:":
              {
              Result += CApps.ExecuteFile(ArchiveFile);
              return;
              }
          default:
              {
              Result += Log.Trace("No suitable installer found for file " + ArchiveFile);
              break;
              }
          }
        }
      
      }

    public void InstallMod()
      {
      InstallDirectory= FileTree.SelectedTreeNode.Root.FullName;
      if (InstallDirectory == null)
        {
        Result +=
          Log.Trace("Invalid input for InstallPakfile", LogEventType.Error);
        return;
        }

      InstallPakFile();
      InsertModInDatabase();
      Result +=
        Log.Trace("Pak " + FileEntry?.Name + "Installed");
      }

    public void InsertModInDatabase()
      {
      var filePath = $"{InstallDirectory}\\{FileEntry.Name}";
      var mod = new ModModel()
        {
        ModName = ModName,
        ModDescription = ModDescription,
        ModSource = ModSource,
        ModType = ModType,
        DLCName = DLCName,
        FilePath = CModManager.StripModDir(filePath),
        FileName = Path.GetFileName(filePath),
        IsInstalled = IsInstalled
        };
      ModDataAccess.UpsertMod(mod);
      if (IsInstalled)
        {
        CModManager.ActivateMod(mod);
        }
      }

    public void ClearInstallData()
      {
      ModName = "";
      ModDescription = "";
      ModSource = "";
      ModType = ModTypesEnum.Undefined;
      DLCName = "";
      FileName = "";
      FilePath = "";
      IsInstalled = false;
      ArchiveFile = "";
      DocumentsList.Clear();
      PakFileList.Clear();
      }

    #endregion Installers
    }
  }
