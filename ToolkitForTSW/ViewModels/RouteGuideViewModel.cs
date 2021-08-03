using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using TreeBuilders.Library.Wpf.ViewModels;

namespace ToolkitForTSW.ViewModels
  {
  public class RouteGuideViewModel: Conductor<object>
    {
    private IWindowManager _windowManager;
    public string RootFolder { get;set; }
    public string FolderImage { get;} = "..\\Images\\folder.png";
    public string FileImage { get; } = "..\\Images\\file_extension_doc.png";
    public RouteGuideViewModel(IWindowManager windowManager)
      {
      _windowManager= windowManager;
      }

    protected override async void OnViewLoaded(object view)
      {
      base.OnViewLoaded(view);
      //FileTreeViewControl.SetImages();
      if(string.IsNullOrEmpty(RootFolder))
        {
        throw new ArgumentException($"No root folder specified for File Tree viewer");
        }
      var viewmodel= IoC.Get<FileTreeViewModel>();
      viewmodel.RootFolder=RootFolder;
      await ActivateItemAsync(viewmodel);
      //FileTreeBuilder.RenameFilesToUnquoted(Tree.FileTree);
      //FileTreeViewControl.Tree = Tree;
      }

    public async Task Close()
      {
      await TryCloseAsync();
      }
    }
  }
