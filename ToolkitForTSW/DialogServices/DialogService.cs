using System.Windows;
using System.Windows.Forms;

namespace ToolkitForTSW.DialogServices
  {
  public enum DialogButton
    {
    //
    // Summary:
    //     The message box displays an OK button.
    OK = 0,
    //
    // Summary:
    //     The message box displays OK and Cancel buttons.
    OKCancel = 1,
    //
    // Summary:
    //     The message box displays Yes, No, and Cancel buttons.
    YesNoCancel = 3,
    //
    // Summary:
    //     The message box displays Yes and No buttons.
    YesNo = 4
    }

  public enum DialogImage
    {
    //
    // Summary:
    //     The message box contains no symbols.
    None = 0,
    //
    // Summary:
    //     The message box contains a symbol consisting of white X in a circle with a red
    //     background.
    Error = 16,
    //
    // Summary:
    //     The message box contains a symbol consisting of a white X in a circle with a
    //     red background.
    Hand = 16,
    //
    // Summary:
    //     The message box contains a symbol consisting of white X in a circle with a red
    //     background.
    Stop = 16,
    //
    // Summary:
    //     The message box contains a symbol consisting of a question mark in a circle.
    //     The question mark message icon is no longer recommended because it does not clearly
    //     represent a specific type of message and because the phrasing of a message as
    //     a question could apply to any message type. In addition, users can confuse the
    //     question mark symbol with a help information symbol. Therefore, do not use this
    //     question mark symbol in your message boxes. The system continues to support its
    //     inclusion only for backward compatibility.
    Question = 32,
    //
    // Summary:
    //     The message box contains a symbol consisting of an exclamation point in a triangle
    //     with a yellow background.
    Exclamation = 48,
    //
    // Summary:
    //     The message box contains a symbol consisting of an exclamation point in a triangle
    //     with a yellow background.
    Warning = 48,
    //
    // Summary:
    //     The message box contains a symbol consisting of a lowercase letter i in a circle.
    Asterisk = 64,
    //
    // Summary:
    //     The message box contains a symbol consisting of a lowercase letter i in a circle.
    Information = 64
    }

  // http://www.embedded101.com/Blogs/PaoloPatierno/entryid/218/messagebox-from-viewmodel-without-violating-the-pattern-mvvm
  public class DialogService : IDialogService
    {
    #region MessageBox

    public DialogResult Show(string messageText)
      {
      System.Windows.MessageBox.Show(messageText);
      return DialogResult.OK;
      }

    public DialogResult Show(string messageText, string caption, DialogButton button, DialogImage image)
      {
      var result=System.Windows.MessageBox.Show(messageText, caption, this.DialogToMessageBoxButton(button), this.DialogToMessageBoxImage(image));
      return this.MessageBoxToDialogResult(result);
      }

    private DialogResult MessageBoxToDialogResult(MessageBoxResult messageBoxResult)
      {
      return (DialogResult)messageBoxResult;
      }

    private MessageBoxButton DialogToMessageBoxButton(DialogButton dialogButton)
      {
      return (MessageBoxButton)dialogButton;
      }

    private MessageBoxImage DialogToMessageBoxImage(DialogImage dialogImage)
      {
      return (MessageBoxImage)dialogImage;
      }
    #endregion
    }
  }
