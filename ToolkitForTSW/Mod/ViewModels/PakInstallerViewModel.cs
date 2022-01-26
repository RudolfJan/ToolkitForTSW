using Caliburn.Micro;
using Logging.Library;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ToolkitForTSW.DataAccess;
using ToolkitForTSW.Mod.Models;
using ToolkitForTSW.Mod.Views;
using ToolkitForTSW.Models;
using TreeBuilders.Library.Wpf;
using TreeBuilders.Library.Wpf.ViewModels;
using Utilities.Library.Zip;
using File = System.IO.File;

namespace ToolkitForTSW.Mod.ViewModels
  {
  public class PakInstallerViewModel : Conductor<object>
    {
    private PakInstallerView _pakInstallerView = null; // This is a bit dirty but I really need it here to be able to update the contents for Mod Filetree
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
          var pakList = PakFileList.Where(x => x.Name.EndsWith("pak"));
          if (pakList.Count() == 1)
            {
            FileEntry = pakList.First();
            }
          }
        NotifyOfPropertyChange(nameof(ArchiveFile));
        }
      }

    private string _InstallDirectory;
    public string InstallDirectory
      {
      get { return _InstallDirectory; }
      set
        {
        _InstallDirectory = value;
        NotifyOfPropertyChange(nameof(InstallDirectory));
        NotifyOfPropertyChange(nameof(CanInstallMod));
        }
      }

    private CFilePresenter _FileEntry;
    public CFilePresenter FileEntry
      {
      get { return _FileEntry; }
      set
        {
        _FileEntry = value;
        NotifyOfPropertyChange(nameof(FileEntry));
        NotifyOfPropertyChange(nameof(CanInstallMod));
        }
      }

    private FileTreeViewModel _FileTree;

    public FileTreeViewModel FileTree
      {
      get => _FileTree;
      set
        {
        _FileTree = value;
        InstallDirectory = FileTree?.SelectedTreeNode?.Root?.FullName;
        NotifyOfPropertyChange(nameof(FileTree));
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
        NotifyOfPropertyChange(nameof(PakFileList));
        }
      }

    private BindableCollection<CFilePresenter> _DocumentsList;
    public BindableCollection<CFilePresenter> DocumentsList
      {
      get { return _DocumentsList; }
      set
        {
        _DocumentsList = value;
        NotifyOfPropertyChange(nameof(DocumentsList));
        }
      }

    private string _modName = "";
    public string ModName
      {
      get { return _modName; }
      set
        {
        _modName = value;
        NotifyOfPropertyChange(nameof(ModName));
        }
      }

    private string _filePath;
    public string FilePath
      {
      get { return _filePath; }
      set
        {
        _filePath = value;
        NotifyOfPropertyChange(nameof(FilePath));
        }
      }


    private string _fileName;
    public string FileName
      {
      get { return _fileName; }
      set
        {
        _fileName = value;
        NotifyOfPropertyChange(nameof(FileName));
        }
      }

    private string _modDescription = "";
    public string ModDescription
      {
      get { return _modDescription; }
      set
        {
        _modDescription = value;
        NotifyOfPropertyChange(nameof(ModDescription));
        }
      }

    private string _modImage;
    public string ModImage
      {
      get { return _modImage; }
      set
        {
        _modImage = value;
        NotifyOfPropertyChange(nameof(ModImage));
        }
      }

    private string _modSource = "";
    public string ModSource
      {
      get { return _modSource; }
      set
        {
        _modSource = value;
        NotifyOfPropertyChange(nameof(ModSource));
        }
      }

    private ModTypesEnum _modType = ModTypesEnum.Undefined;
    public ModTypesEnum ModType
      {
      get { return _modType; }
      set
        {
        _modType = value;
        NotifyOfPropertyChange(nameof(ModType));
        }
      }

    private string _DLCName = "";
    public string DLCName
      {
      get { return _DLCName; }
      set
        {
        _DLCName = value;
        NotifyOfPropertyChange(nameof(DLCName));
        }
      }

    private string _modVersion = "";
    public string ModVersion
      {
      get { return _modVersion; }
      set
        {
        _modVersion = value;
        NotifyOfPropertyChange(nameof(ModVersion));
        }
      }

    private bool _IsInstalledSteam;
    public bool IsInstalledSteam
      {
      get { return _IsInstalledSteam; }
      set
        {
        _IsInstalledSteam = value;
        NotifyOfPropertyChange(nameof(IsInstalledSteam));
        }
      }

    private bool _IsInstalledEGS;
    public bool IsInstalledEGS
      {
      get { return _IsInstalledEGS; }
      set
        {
        _IsInstalledEGS = value;
        NotifyOfPropertyChange(nameof(IsInstalledEGS));
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
        NotifyOfPropertyChange(() => NewDirectory);
        NotifyOfPropertyChange(() => CanAddDirectory);
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

      _pakInstallerView = view as PakInstallerView;
      FillPakDirList();
      if (string.IsNullOrEmpty(RootFolder))
        {
        throw new ArgumentException($"No root folder specified for File Tree viewer");
        }
      NotifyOfPropertyChange(nameof(FileTree));
      }

    public void BuildFileTree()
      {
      FileTree = _pakInstallerView.ViewModel;
      FileTree.RootFolder = RootFolder;
      _pakInstallerView.FileTreeViewControl.FolderImage = FolderImage;
      _pakInstallerView.FileTreeViewControl.FileImage = FileImage;
      _pakInstallerView.FileTreeViewControl.SetImages();
      }

    public void FillPakDirList()
      {
      if (string.IsNullOrEmpty(RootFolder))
        {
        throw new ArgumentException($"No root folder specified for File Tree viewer");
        }
      BuildFileTree();
      FileTree.FileTree = new TreeNodeModel();
      FileTree.Initialize(RootFolder, false);
      NotifyOfPropertyChange(nameof(FileTree.FileTree));
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
        Log.Trace("File presenter is null", LogEventType.Error);
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
            Log.Trace("Archive type " + Extension + " is not (yet) supported");
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
    public static void GetZipArchivedFiles(FileInfo archiveFile, BindableCollection<CFilePresenter> DestinationFileList, string FileType = "")
      {
      try
        {
        if (DestinationFileList == null)
          {
          Log.Trace("File presenter is null, report this to the developers", LogEventType.Error);
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
        Log.Trace("Failed to show file entries for archive " + archiveFile.FullName,
          LogEventType.Error);
        }
      }

    public static void GetRwpArchivedFiles(FileInfo archiveFile, BindableCollection<CFilePresenter> DestinationFileList, string FileType = "")
      {
      GetRwpArchivedFiles(archiveFile.FullName, DestinationFileList, FileType);
      }

    public static void GetRwpArchivedFiles(string archiveFile, BindableCollection<CFilePresenter> DestinationFileList, string FileType)
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
          }
        else
          {
          if (TreeItem?.Root == null)
            {
            Path = TSWOptions.ModsFolder + DirName;
            }
          else
            {
            Path = TreeItem.Root + "\\" + DirName;
            }
          }
        Directory.CreateDirectory(Path);
        NewDirectory = "";
        FillPakDirList();
        }
      catch (Exception E)
        {
        Log.Trace("Cannot create directory because " + E.Message, LogEventType.Error);
        }
      }

    #region Installers

    public void InstallPakFile()
      {
      if (FileEntry != null)
        {
        switch (FileEntry.Extension)
          {
          case ".pak":
              {
              if (Path.GetExtension(ArchiveFile) == ".pak")
                {
                File.Copy(FileEntry.FullName, $"{InstallDirectory}\\{FileEntry.Name}", true);
                FillPakDirList();
                return;
                }
              SevenZipLib.ExtractSingle(ArchiveFile, InstallDirectory, FileEntry.FullName);
              FillPakDirList();
              return;
              }
          case ".exe:":
              {
              CApps.ExecuteFile(ArchiveFile);
              return;
              }
          default:
              {
              Log.Trace("No suitable installer found for file " + ArchiveFile);
              break;
              }
          }
        }
      }

    public bool CanInstallMod
      {
      get
        {
        return FileEntry?.FullName.Length > 1;
        }
      }

    public void InstallMod()
      {
      InstallDirectory = FileTree?.SelectedTreeNode?.Root?.FullName;
      if (InstallDirectory == null)
        {
        MessageBox.Show("You need to select a folder where the mod will installed at the right side of this window",
        "Select a folder",
        MessageBoxButton.OK,
        MessageBoxImage.Error);
        Log.Trace("Please select a folder in the right pane of this screen", LogEventType.Message);
        return;
        }

      InstallPakFile();
      InsertModInDatabase();
      Log.Trace("Pak " + FileEntry?.Name + " Installed", LogEventType.InformUser);
      FillPakDirList();
      ClearInstallData();
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
        ModActivator.ActivateMod(mod, PlatformEnum.Steam, false);
        }
      if (IsInstalledEGS)
        {
        ModActivator.ActivateMod(mod, PlatformEnum.EpicGamesStore, false);
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
      IsInstalledEGS = false;
      ModVersion = "";
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
        return NewDirectory?.Length >= 1 && FileTree.FileTree.Root != null;
        }
      }

    public void AddDirectory()
      {
      AddDirectory(FileTree.FileTree, NewDirectory, false);

      }

    public bool CanAddDirectoryChild
      {
      get
        {
        return NewDirectory?.Length >= 1;
        }
      }
    public void AddDirectoryChild()
      {
      if (FileTree?.SelectedTreeNode?.Root == null)
        {
        MessageBox.Show("You need to select the parent folder in folder tree above",
        "Select parent folder",
        MessageBoxButton.OK,
        MessageBoxImage.Error);
        return;
        }
      AddDirectory(FileTree.SelectedTreeNode, NewDirectory, true);
      }

    #endregion
    }
  }
