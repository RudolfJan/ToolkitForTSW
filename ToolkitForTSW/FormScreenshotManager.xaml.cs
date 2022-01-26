using Screenshots.Library.WPF.ViewModels;
using System.Windows;

//	https://github.com/mirzaevolution/ThumbnailSharp

namespace ToolkitForTSW
  {
  /// <summary>
  /// Interaction logic for FormScreenshotManager.xaml
  /// </summary>
  public partial class FormScreenshotManager
    {

    public FormScreenshotManager()
      {
      InitializeComponent();
      ScreenshotManagerView.ScreenshotManager = new ScreenshotManagerViewModel();
      DataContext = ScreenshotManagerView.ScreenshotManager;
      }

    private void OnOkButtonClicked(object Sender, RoutedEventArgs E)
      {
      Close();
      }
    }
  }
