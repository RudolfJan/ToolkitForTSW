using System.Windows.Forms;

namespace ToolkitForTSW.DialogServices
  {
  public interface IDialogService
    {
    DialogResult Show(string messageText);
    DialogResult Show(string messageText, string caption, DialogButton button, DialogImage image);
    }
  }
