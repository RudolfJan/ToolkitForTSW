using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using ToolkitForTSW.Views;
using TreeBuilders.Library.Wpf.ViewModels;

namespace ToolkitForTSW.ViewModels
  {
  public class RouteGuideViewModel: Conductor<object>
    {
    private IWindowManager _windowManager;
    public string RootFolder { get;set; }= $"{TSWOptions.ManualsFolder}RouteGuides\\";
    public string FolderImage { get;} = "..\\Images\\folder.png";
    public string FileImage { get; } = "..\\Images\\file_extension_doc.png";
    FileTreeViewModel FileTree { get; set; }
    public RouteGuideViewModel(IWindowManager windowManager)
      {
      _windowManager= windowManager;
      }

    protected override void OnViewLoaded(object view)
      {
      base.OnViewLoaded(view);
      if(string.IsNullOrEmpty(RootFolder))
        {
        throw new ArgumentException($"No root folder specified for File Tree viewer");
        }
      var viewmodel= new FileTreeViewModel(); // Use new here because IoC causes ordering issue
      viewmodel.RootFolder=RootFolder;
      var view2= view as RouteGuideView;
      FileTree = view2.ViewModel;
      FileTree.RootFolder = RootFolder;
      view2.FileTreeViewControl.FolderImage = FolderImage;
      view2.FileTreeViewControl.FileImage = FileImage;
      view2.FileTreeViewControl.SetImages();
      FileTree.Initialize(RootFolder, false);
      NotifyOfPropertyChange(() => FileTree);
      }


    public Task CloseForm()
      {
      return TryCloseAsync();
      }
    }
  }
