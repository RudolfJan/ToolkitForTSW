using Syroot.Windows.IO;
using System;
using System.Windows;
using System.Windows.Controls;
using TreeBuilders.Library.Wpf.ViewModels;
using Utilities.Library;
using Utilities.Library.Zip;

namespace ToolkitForTSW.Mod.Views
  {

  public partial class PakInstallerView
    {
    public FileTreeViewModel ViewModel { get; set; }
    public PakInstallerView()
      {
      InitializeComponent();
      if (ViewModel == null)
        {
        ViewModel = new FileTreeViewModel();
        if (!string.IsNullOrEmpty(ViewModel?.RootFolder))
          {
          ViewModel.Initialize(ViewModel.RootFolder, false);
          }
        }
      DataContext = ViewModel;
      FileTreeViewControl.Tree = ViewModel;
      FileTreeViewControl.DataContext = ViewModel;

      // https://gitlab.com/Syroot/KnownFolders/blob/master/src/Syroot.KnownFolders.Scratchpad/Program.cs

      KnownFolder MyKnownFolder = new KnownFolder(KnownFolderType.Downloads);
      ArchiveFileTextBox.InitialDirectory = MyKnownFolder.Path;
      SetControlStates();
      }

    private void SetControlStates()
      {
      IsInstalledSteamCheckBox.IsEnabled = TSWOptions.SteamTrainSimWorldDirectory.Length > 0;
      IsInstalledSteamCheckBox.IsEnabled = TSWOptions.EGSTrainSimWorldDirectory.Length > 0;
      }

    private void OnFileTreeViewSelectedItemChanged(Object Sender,
     RoutedPropertyChangedEventArgs<Object> E)
      {
      SetControlStates();
      }


    private void OnDocumentationDataGridSelectionChanged(Object sender, SelectionChangedEventArgs e)
      {
      SetControlStates();
      }

    private void DocumentationDataGrid_MouseUp(object sender,
      System.Windows.Input.MouseButtonEventArgs e)
      {
      if (DocumentationDataGrid.SelectedItem != null)
        {
        try
          {
          CFilePresenter FilePresenter = ((CFilePresenter)DocumentationDataGrid.SelectedItem);
          SevenZipLib.ExtractSingle(ArchiveFileTextBox.FileName, TSWOptions.TempFolder,
            FilePresenter.FullName);
          ProcessHelper.OpenGenericFile(TSWOptions.TempFolder + FilePresenter.FullName);
          }
        catch (Exception)
          {
          return;
          }
        }
      }

    private void FileTreeViewControl_NodeSelectionChanged(object sender, RoutedEventArgs e)
      {
      SetControlStates();

      }
    }
  }
