using Logging.Library;
using Styles.Library.Helpers;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using ToolkitForTSW.DataAccess;
using ToolkitForTSW.Models;

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

    private CTreeItemProvider _FileTree;

    public CTreeItemProvider FileTree
      {
      get => _FileTree;
      set
        {
        _FileTree = value;
        OnPropertyChanged("FileTree");
        }
      }

    private ObservableCollection<CDirTreeItem> _TreeItems;

    public ObservableCollection<CDirTreeItem> TreeItems
      {
      get => _TreeItems;
      set
        {
        _TreeItems = value;
        OnPropertyChanged("TreeItems");
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

    private String _Result;

    public String Result
      {
      get { return _Result; }
      set
        {
        _Result = value;
        OnPropertyChanged("Result");
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
      var Dir = new DirectoryInfo(CTSWOptions.ModsFolder);
      FileTree = new CTreeItemProvider();
      TreeItems = FileTree.GetDirItems(Dir.FullName);
      }

    public void GetArchivedFiles(FileInfo ArchiveFile, ObservableCollection<CFilePresenter> DestinationFileList, String FileType = "")
      {
      var Extension = ArchiveFile.Extension.ToLower();
      if (DestinationFileList == null)
        {
        Result += Log.Trace("File presenter is null", LogEventType.Error);
        return;
        }

      switch (Extension)
        {
        case ".zip":
            {
            GetZipArchivedFiles(ArchiveFile, DestinationFileList, FileType);
            break;
            }
        case ".rar":
        case ".7z":
            {
            GetRwpArchivedFiles(ArchiveFile, DestinationFileList, FileType);
            break;
            }
        case ".exe":
            {
            //Empty list
            break;
            }
        case ".pak":
            {
            GetNotArchivedPakFile(ArchiveFile, DestinationFileList);
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
    public void GetNotArchivedPakFile(FileInfo ArchiveFile,
      ObservableCollection<CFilePresenter> DestinationFileList)
      {
      CFilePresenter FilePresenter =
        new CFilePresenter(ArchiveFile.FullName, ArchiveFile.Name, ArchiveFile.LastWriteTime);
      DestinationFileList.Add(FilePresenter);
      DocumentsList.Clear();
      }

    // Show contents of a .zip file
    public void GetZipArchivedFiles(FileInfo ArchiveFile, ObservableCollection<CFilePresenter> DestinationFileList, String FileType = "")
      {
      try
        {
        if (DestinationFileList == null)
          {
          Result += Log.Trace("File presenter is null", LogEventType.Error);
          return;
          }
        using (ZipArchive Archive = ZipFile.OpenRead(ArchiveFile.FullName))
          {
          foreach (ZipArchiveEntry Entry in Archive.Entries)
            {
            if (FileType.Length == 0 ||
                String.CompareOrdinal(Path.GetExtension(Entry.Name), FileType) == 0
              ) // first part makes this work if no filter is specified
              {
              CFilePresenter FilePresenter =
                new CFilePresenter(Entry.FullName, Entry.Name, Entry.LastWriteTime);
              DestinationFileList.Add(FilePresenter);
              }
            }
          }
        }
      catch (Exception)
        {
        Result += Log.Trace("Failed to show file entries for archive " + ArchiveFile.FullName,
          LogEventType.Error);
        }
      }

    public void GetRwpArchivedFiles(FileInfo ArchiveFile, ObservableCollection<CFilePresenter> DestinationFileList, String FileType = "")
      {
      GetRwpArchivedFiles(ArchiveFile.FullName, DestinationFileList, FileType);
      }

    public void GetRwpArchivedFiles(String ArchiveFile, ObservableCollection<CFilePresenter> DestinationFileList, String FileType)
      {
      CApps.ListZipFiles(ArchiveFile, out var FileReport);
      var Skip = 16;
      if (String.Compare(Path.GetExtension(ArchiveFile), ".rar", StringComparison.Ordinal) == 0 ||
          String.Compare(Path.GetExtension(ArchiveFile), ".7z", StringComparison.Ordinal) == 0)
        {
        Skip = 19;
        }

      var Reader = new StringReader(FileReport);
#pragma warning disable IDE0059 // Unnecessary assignment of a value
      var MetaData = "";
#pragma warning restore IDE0059 // Unnecessary assignment of a value
      for (Int32 I = 0; I < Skip; I++) // tricky!
        {
        // ReSharper disable once RedundantAssignment debugging, do not remove this
        MetaData = Reader.ReadLine() + "\r\n";
        }

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
                String.CompareOrdinal(Path.GetExtension(FilePresenter.Name), FileType) == 0)
              {
              DestinationFileList.Add(FilePresenter);
              }
            }
          }
        }
      }

    public void AddDirectory(CDirTreeItem TreeItem, String DirName, Boolean AsChild)
      {
      String Path;
      try
        {
        if (AsChild)
          {
          Path = TreeItem.Path + "\\" + DirName;
          Directory.CreateDirectory(Path);
          }
        else
          {
          if (TreeItem?.Path == null)
            {
            Path = CTSWOptions.ModsFolder + DirName;
            }
          else
            {
            Path = TreeItem.Path + "\\..\\" + DirName;
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
                Result += CModManager.UpdateModTable(new FileInfo($"{InstallDirectory}\\{FileEntry.Name}"));
                return;
                }
              CApps.SevenZipExtractSingle(ArchiveFile, InstallDirectory,
                FileEntry.FullName);
              Result += CModManager.UpdateModTable(new FileInfo($"{InstallDirectory}\\{FileEntry.Name}"));

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
        FilePath= filePath,
        FileName = Path.GetFileName(filePath),
        IsInstalled = IsInstalled
        };
      ModDataAccess.UpsertMod(mod);
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
      IsInstalled=false;
      ArchiveFile = "";
      DocumentsList.Clear();
      PakFileList.Clear();
      }

    #endregion Installers
    }
  }
