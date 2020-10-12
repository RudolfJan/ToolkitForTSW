using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ToolkitForTSW
  {
  /// <summary>
  /// Interaction logic for FormPublishScenario.xaml
  /// </summary>
  public partial class FormPublishScenario
    {
    public CPublishScenario Publish { get; set; }
    public FormPublishScenario(CPublishScenario publish)
      {
      InitializeComponent();
      Publish = publish;
      DataContext = Publish;
      }

    private void OKButton_Click(object sender, RoutedEventArgs e)
      {
      Publish.Author = AuthorTextBox.TextBoxText;
      Publish.Description = DescriptionTextBox.TextBoxText;
      Publish.CreateDocumentFile();
      Close();
      }

    private void CancelKButton_Click(object sender, RoutedEventArgs e)
      {
      Close();
      }
    }
  }
