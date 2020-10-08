using System;
using System.Windows;

namespace TSWTools
{
    /// <summary>
    /// Interaction logic for FormOptions.xaml
    /// </summary>
    public partial class FormOptions : Window
		{
		CTSWOptionsView OptionsView { get; set; }

		public FormOptions()
			{
			InitializeComponent();
			OptionsView = new CTSWOptionsView();
			DataContext = OptionsView;
			}

		private void OnOKButtonClicked(Object sender, RoutedEventArgs e)
			{
			DialogResult = true;
			OptionsView.SaveOptions();
			Close();
			}

		private void OnCancelButtonClicked(Object sender, RoutedEventArgs e)
			{
			DialogResult = false;
			Close();
			}

        private void OnSteamProgramFolderFileInputChanged(Object sender, RoutedEventArgs e)
        {
				
        }

    private void FileInputBox_Loaded(object sender, RoutedEventArgs e)
      {

      }
    }
	}