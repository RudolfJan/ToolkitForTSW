using Caliburn.Micro;
using System;
using System.Threading.Tasks;
using ToolkitForTSW.Views;
using TreeBuilders.Library.Wpf.ViewModels;

namespace ToolkitForTSW.ViewModels
  {
  public class RouteGuideViewModel: Conductor<object>
    {
#pragma warning disable IDE0052 // Remove unread private members
    private readonly IWindowManager _windowManager;
#pragma warning restore IDE0052 // Remove unread private members
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
#pragma warning disable IDE0017 // Simplify object initialization
      var viewmodel= new FileTreeViewModel(); // Use new here because IoC causes ordering issue
#pragma warning restore IDE0017 // Simplify object initialization
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
