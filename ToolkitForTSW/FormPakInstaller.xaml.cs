using Logging.Library;
using Syroot.Windows.IO;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using ToolkitForTSW.Mod;
using Utilities.Library;
using Utilities.Library.Zip;

namespace ToolkitForTSW
  {
  /// <summary>
  /// Interaction logic for FormPakInstaller.xaml
  /// </summary>
  public partial class FormPakInstaller
    {
    private CPakInstaller PakInstaller { get; set; }

    public FormPakInstaller()
      {
      InitializeComponent();
      PakInstaller = new CPakInstaller();

      DataContext = PakInstaller;
      PakInstaller.Result += "PakInstaller initialized\r\n";
      FileTreeViewControl.FolderImage = "Images\\folder.png";
      FileTreeViewControl.FileImage = "Images\\file_extension_doc.png";
      FileTreeViewControl.SetImages();
      FileTreeViewControl.Tree = PakInstaller.FileTree;
      FileTreeViewControl.DataContext = PakInstaller.FileTree;

      // https://gitlab.com/Syroot/KnownFolders/blob/master/src/Syroot.KnownFolders.Scratchpad/Program.cs

      var MyKnownFolder = new KnownFolder(KnownFolderType.Downloads);
      ArchiveFileTextBox.InitialDirectory = MyKnownFolder.Path;
      SetControlStates();
      }

    private void SetControlStates()
      {
      InstallToSetsButton.IsEnabled = PakFileListDataGrid.SelectedItem != null &&
                                      PakInstaller.FileTree?.SelectedFileNode != null;
      AddDirButton.IsEnabled =
        NewDirTextBox.TextBoxText != null && NewDirTextBox.TextBoxText.Length > 2;
      AddChildButton.IsEnabled = NewDirTextBox.TextBoxText != null &&
                                 NewDirTextBox.TextBoxText.Length > 2 &&
                                 PakInstaller.FileTree?.SelectedFileNode != null;
      }

    private void OnOKButtonClicked(Object Sender, RoutedEventArgs E)
      {
      Close();
      }

    private void OnFileTreeViewSelectedItemChanged(Object Sender,
      RoutedPropertyChangedEventArgs<Object> E)
      {
      SetControlStates();
      }

    private void OnArchiveFileTextBoxFileNameChanged(Object Sender, RoutedEventArgs E)
      {
      PakInstaller.PakFileList.Clear();
      PakInstaller.DocumentsList.Clear();
      if (!string.IsNullOrEmpty(ArchiveFileTextBox.FileName))
        {
        PakInstaller.GetArchivedFiles(new FileInfo(ArchiveFileTextBox.FileName),
          PakInstaller.PakFileList, ".pak");
        PakInstaller.GetArchivedFiles(new FileInfo(ArchiveFileTextBox.FileName),
          PakInstaller.DocumentsList, ".txt");
        PakInstaller.GetArchivedFiles(new FileInfo(ArchiveFileTextBox.FileName),
          PakInstaller.DocumentsList, ".pdf");
        PakInstaller.GetArchivedFiles(new FileInfo(ArchiveFileTextBox.FileName),
          PakInstaller.DocumentsList, ".docx");
        }
      }

    private void OnPakFileListDataGridSelectionChanged(Object Sender, SelectionChangedEventArgs E)
      {
      SetControlStates();
      }

    private void OnInstallToSetsButtonClicked(Object Sender, RoutedEventArgs E)
      {
      PakInstaller.InstallMod();
      }

    private void OnAddDirButtonClicked(Object Sender, RoutedEventArgs E)
      {
      PakInstaller.AddDirectory(PakInstaller.FileTree.SelectedTreeNode, NewDirTextBox.TextBoxText, false);
      FileTreeViewControl.Tree = PakInstaller.FileTree;
      FileTreeViewControl.DataContext = PakInstaller.FileTree;
      }

    private void OnAddChildButtonClicked(Object Sender, RoutedEventArgs E)
      {
      PakInstaller.AddDirectory(PakInstaller.FileTree.SelectedTreeNode, NewDirTextBox.TextBoxText, true);
      FileTreeViewControl.Tree = PakInstaller.FileTree;
      FileTreeViewControl.DataContext = PakInstaller.FileTree;
      }

    private void OnNewDirTextBoxTextChanged(Object Sender, RoutedEventArgs E)
      {
      SetControlStates();
      }

    private void OnDocumentationDataGridSelectionChanged(Object sender, SelectionChangedEventArgs e)
      {
      SetControlStates();
      }

    private void DocumentationDataGrid_MouseUp(Object sender,
      System.Windows.Input.MouseButtonEventArgs e)
      {
      if (DocumentationDataGrid.SelectedItem != null)
        {
        try
          {
          var FilePresenter = ((CFilePresenter) DocumentationDataGrid.SelectedItem);
          SevenZipLib.ExtractSingle(ArchiveFileTextBox.FileName, CTSWOptions.TempFolder,
            FilePresenter.FullName);
          ProcessHelper.OpenGenericFile(CTSWOptions.TempFolder + FilePresenter.FullName);
          }
        catch (Exception)
          {
          // ReSharper disable once RedundantJumpStatement
          return;
          }
        }
      }

    private void OnClearButtonClicked(object Sender, RoutedEventArgs E)
      {
      PakInstaller.ClearInstallData();
      }
    }
  }
