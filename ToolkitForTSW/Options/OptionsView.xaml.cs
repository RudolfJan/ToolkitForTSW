using Filter.Library.WPF.ViewModels;
using Screenshots.Library.WPF.ViewModels;

namespace ToolkitForTSW
  {
  public partial class OptionsView
    {
    //OptionsViewModel OptionsSet
    //  { get; set; }

    public OptionsView()
      {
      InitializeComponent();
      TagsView.TagAndCategoryData = new TagAndCategoryViewModel();
      TagsView.DataContext = TagsView.TagAndCategoryData;
      CollectionsView.ScreenshotCollectionManager = new ScreenshotCollectionViewModel();
      CollectionsView.DataContext = CollectionsView.ScreenshotCollectionManager;
      }
    }
  }