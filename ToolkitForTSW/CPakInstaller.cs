using Logging.Library;
using Styles.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ToolkitForTSW
  {
  public class CPakInstaller : Notifier
    {
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
        Result +=Log.Trace("File presenter is null", LogEventType.Error);
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
            Result +=Log.Trace("Archive type " + Extension + " is not (yet) supported");
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
      // ReSharper disable once NotAccessedVariable debugging, do not remove this
      var MetaData = String.Empty;
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

    public void InstallPakFile(String ArchiveFile, String InstallDir,
      CFilePresenter FileEntry, Boolean ToGame)
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
                File.Copy(FileEntry.FullName, $"{InstallDir}\\{FileEntry.Name}", true);
                Result += CModManager.UpdateModTable(new FileInfo($"{InstallDir}\\{FileEntry.Name}"));
                return;
                }
              CApps.SevenZipExtractSingle(ArchiveFile, InstallDir,
                FileEntry.FullName);
              Result += CModManager.UpdateModTable(new FileInfo($"{InstallDir}\\{FileEntry.Name}"));
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

    #endregion Installers
    }
  }
