using Caliburn.Micro;
using System.Threading.Tasks;

namespace ToolkitForTSW.ViewModels
  {
  // This is a workaround because I cannot use a messagebox. You can only use a messagebox if at least one window is open in WPF.
  public class MessageBoxViewModel : Screen
    {
    private string _message = "";
    public string Message
      {
      get
        {
        return _message;
        }
      set
        {
        _message = value;
        NotifyOfPropertyChange(nameof(Message));
        }
      }

    private string _title = "";
    public string Title
      {
      get
        {
        return _title;
        }
      set
        {
        _title = value;
        NotifyOfPropertyChange(nameof(Title));
        }
      }
    public Task CloseForm()
      {
      return TryCloseAsync();
      }
    }
  }
