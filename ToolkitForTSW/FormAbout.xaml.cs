using System;
using System.Windows;

namespace ToolkitForTSW
{
    /// <summary>
    /// Interaction logic for FormAbout.xaml
    /// </summary>
    public partial class FormAbout : Window
    {
	    public CAboutData AboutData { get; set; }
        public FormAbout()
        {
            InitializeComponent();
		    AboutData = new CAboutData();
		    DataContext = AboutData;
		    }

	    private void Hyperlink_RequestNavigate(Object Sender, System.Windows.Navigation.RequestNavigateEventArgs E)
		    {
		    System.Diagnostics.Process.Start(E.Uri.AbsoluteUri);
		    }

        private void OnOkButtonClicked(Object Sender, RoutedEventArgs E)
        {
						Close();
        }
    }
}
