using Caliburn.Micro;
using Logging.Library;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows;
using ToolkitForTSW.DataAccess;
using ToolkitForTSW.Mod.Models;
using ToolkitForTSW.Mod.Views;
using ToolkitForTSW.Models;
using TreeBuilders.Library.Wpf;
using TreeBuilders.Library.Wpf.ViewModels;
using TreeBuilders.Library.Wpf.Views;
using Utilities.Library.Zip;

namespace ToolkitForTSW.Mod.ViewModels
  {
  public class PakInstallerViewModel : Conductor<object>
    {
    private string _ArchiveFile;
    public string ArchiveFile
      {
      get { return _ArchiveFile; }
      set
        {
        _ArchiveFile = value;

        PakFileList.Clear();
        DocumentsList.Clear();
        if (!string.IsNullOrEmpty(ArchiveFile))
          {
          GetArchivedFiles(new FileInfo(ArchiveFile),
           PakFileList, ".pak");
          GetArchivedFiles(new FileInfo(ArchiveFile),
            DocumentsList, ".txt");
          GetArchivedFiles(new FileInfo(ArchiveFile),
            DocumentsList, ".pdf");
          GetArchivedFiles(new FileInfo(ArchiveFile),
            DocumentsList, ".docx");
          }
        NotifyOfPropertyChange("ArchiveFile");
        }
      }

    private string _InstallDirectory;
    public string InstallDirectory
      {
      get { return _InstallDirectory; }
      set
        {
        _InstallDirectory = value;
        NotifyOfPropertyChange("InstallDirectory");
        }
      }

    private CFilePresenter _FileEntry;
    public CFilePresenter FileEntry
      {
      get { return _FileEntry; }
      set
        {
        _FileEntry = value;
        NotifyOfPropertyChange("FileEntry");
        NotifyOfPropertyChange(()=> CanInstallMod);
        }
      }

    private FileTreeViewModel _FileTree;

    public FileTreeViewModel FileTree
      {
      get => _FileTree;
      set
        {
        _FileTree = value;
        NotifyOfPropertyChange("FileTree");
        NotifyOfPropertyChange(() => CanInstallMod);
        }
      }

    private BindableCollection<CFilePresenter> _PakFileList;
    public BindableCollection<CFilePresenter> PakFileList
      {
      get { return _PakFileList; }
      set
        {
        _PakFileList = value;
        NotifyOfPropertyChange("PakFileList");
        }
      }

    private BindableCollection<CFilePresenter> _DocumentsList;
    public BindableCollection<CFilePresenter> DocumentsList
      {
      get { return _DocumentsList; }
      set
        {
        _DocumentsList = value;
        NotifyOfPropertyChange("DocumentsList");
        }
      }

    private string _Result;

    public string Result
      {
      get { return _Result; }
      set
        {
        _Result = value;
        NotifyOfPropertyChange("Result");
        }
      }

    private string _modName = "";
    public string ModName
      {
      get { return _modName; }
      set
        {
        _modName = value;
        NotifyOfPropertyChange("ModName");
        }
      }

    private string _filePath;
    public string FilePath
      {
      get { return _filePath; }
      set
        {
        _filePath = value;
        NotifyOfPropertyChange("FilePath");
        }
      }


    private string _fileName;
    public string FileName
      {
      get { return _fileName; }
      set
        {
        _fileName = value;
        NotifyOfPropertyChange("FileName");
        }
      }

    private string _modDescription = "";
    public string ModDescription
      {
      get { return _modDescription; }
      set
        {
        _modDescription = value;
        NotifyOfPropertyChange("ModDescription");
        }
      }

    private string _modImage;
    public string ModImage
      {
      get { return _modImage; }
      set
        {
        _modImage = value;
        NotifyOfPropertyChange("ModImage");
        }
      }

    private string _modSource = "";
    public string ModSource
      {
      get { return _modSource; }
      set
        {
        _modSource = value;
        NotifyOfPropertyChange("ModSource");
        }
      }

    private ModTypesEnum _modType = ModTypesEnum.Undefined;
    public ModTypesEnum ModType
      {
      get { return _modType; }
      set
        {
        _modType = value;
        NotifyOfPropertyChange("ModType");
        }
      }

    private string _DLCName = "";
    public string DLCName
      {
      get { return _DLCName; }
      set
        {
        _DLCName = value;
        NotifyOfPropertyChange("DLCName");
        }
      }

    private string _modVersion = "";
    public string ModVersion
      {
      get { return _modVersion; }
      set
        {
        _modVersion = value;
        NotifyOfPropertyChange("ModVersion");
        }
      }

    private bool _IsInstalledSteam;
    public bool IsInstalledSteam
      {
      get { return _IsInstalledSteam; }
      set
        {
        _IsInstalledSteam = value;
        NotifyOfPropertyChange("IsInstalledSteam");
        }
      }

    private bool _IsInstalledEGS;
    public bool IsInstalledEGS
      {
      get { return _IsInstalledEGS; }
      set
        {
        _IsInstalledEGS = value;
        NotifyOfPropertyChange("IsInstalledEGS");
        }
      }

    public string RootFolder { get; set; } = $"{TSWOptions.ModsFolder}";
    public string FolderImage { get; } = "..\\Images\\folder.png";
    public string FileImage { get; } = "..\\Images\\file_extension_doc.png";

    private string _newDirectory;

    public string NewDirectory
      {
      get 
        { 
        return _newDirectory; 
        }
      set 
        { 
        _newDirectory = value;
        NotifyOfPropertyChange(()=>CanAddDirectory);
        NotifyOfPropertyChange(() => CanAddDirectoryChild);
        }
      }


    public PakInstallerViewModel()
      {
      }

    protected override void OnViewLoaded(object view)
      {
      base.OnViewLoaded(view);
      PakFileList = new BindableCollection<CFilePresenter>();
      DocumentsList = new BindableCollection<CFilePresenter>();
      
      var view2= view as PakInstallerView;
      FileTree = view2.ViewModel;
      FileTree.RootFolder=RootFolder;
      view2.FileTreeViewControl.FolderImage= FolderImage;
      view2.FileTreeViewControl.FileImage = FileImage;
      view2.FileTreeViewControl.SetImages();
      FillPakDirList();
      if (string.IsNullOrEmpty(RootFolder))
        {
        throw new ArgumentException($"No root folder specified for File Tree viewer");
        }
      NotifyOfPropertyChange("FileTree");
      }

    public void FillPakDirList()
      {
      if (string.IsNullOrEmpty(RootFolder))
        {
        throw new ArgumentException($"No root folder specified for File Tree viewer");
         }
      if(FileTree==null)
        {
        FileTree = new FileTreeViewModel();
        }
      
      FileTree.Initialize(RootFolder, false);
 
      NotifyOfPropertyChange("FileTree");
      }

      public Task CloseForm()
      {
      return TryCloseAsync();
      }

    public void GetArchivedFiles(FileInfo archiveFile, BindableCollection<CFilePresenter> DestinationFileList, string FileType = "")
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
      BindableCollection<CFilePresenter> DestinationFileList)
      {
      var FilePresenter =
        new CFilePresenter(archiveFile.FullName, archiveFile.Name, archiveFile.LastWriteTime);
      DestinationFileList.Add(FilePresenter);
      DocumentsList.Clear();
      }

    // Show contents of a .zip file
    public void GetZipArchivedFiles(FileInfo archiveFile, BindableCollection<CFilePresenter> DestinationFileList, string FileType = "")
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

    public void GetRwpArchivedFiles(FileInfo archiveFile, BindableCollection<CFilePresenter> DestinationFileList, string FileType = "")
      {
      GetRwpArchivedFiles(archiveFile.FullName, DestinationFileList, FileType);
      }

    public void GetRwpArchivedFiles(string archiveFile, BindableCollection<CFilePresenter> DestinationFileList, string FileType)
      {
      SevenZipLib.ListFilesInArchive(archiveFile, out var FileReport, true);
       var Reader = new StringReader(FileReport);
 
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

    private void AddDirectory(TreeNodeModel TreeItem, string DirName, bool AsChild)
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

    public bool CanInstallMod 
      { 
      get
        {
        return FileEntry?.FullName.Length>1 && FileTree?.SelectedTreeNode?.Root?.FullName?.Length>1;
        }
      }

    public void InstallMod()
      {
      InstallDirectory = FileTree.SelectedTreeNode.Root.FullName;
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
        FilePath = ModStatusViewModel.StripModDir(filePath),
        FileName = Path.GetFileName(filePath),
        IsInstalledSteam = IsInstalledSteam,
        IsInstalledEGS = IsInstalledEGS
        };
      ModDataAccess.UpsertMod(mod);
      if (IsInstalledSteam)
        {
        ModStatusViewModel.ActivateMod(mod, PlatformEnum.Steam,false);
        }
      if (IsInstalledEGS)
        {
        ModStatusViewModel.ActivateMod(mod, PlatformEnum.EpicGamesStore,false);
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
      IsInstalledSteam = false;
      IsInstalledEGS=false;
      ModVersion="";
      ArchiveFile = "";
      DocumentsList.Clear();
      PakFileList.Clear();
      }



    #endregion Installers

    #region AddDirectory
    public bool CanAddDirectory 
      { 
      get
        {
        return NewDirectory?.Length>=1 && FileTree?.SelectedTreeNode?.IsSelected==true;
        }
      }

    public void AddDirectory()
      {
      AddDirectory(FileTree.SelectedTreeNode, NewDirectory, false);
      FillPakDirList();
      }

    public bool CanAddDirectoryChild
      {
      get
        {
        return NewDirectory?.Length >= 1 && FileTree?.SelectedTreeNode?.IsSelected == true;
        }
      }

    public void AddDirectoryChild()
      {
      AddDirectory(FileTree.SelectedTreeNode, NewDirectory, true);
      FillPakDirList();
      }

    #endregion
    }
  }
