using Filter.Library.WPF.ViewModels;
using Filter.Library.WPF.Views;
using Screenshots.Library.WPF.ViewModels;
using System;
using System.Windows;
using System.Windows.Data;

namespace ToolkitForTSW
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
      TagsView.TagAndCategoryData= new TagAndCategoryViewModel();
      TagsView.DataContext = TagsView.TagAndCategoryData;
      CollectionsView.ScreenshotCollectionManager= new ScreenshotCollectionViewModel();
      CollectionsView.DataContext = CollectionsView.ScreenshotCollectionManager;
      SetControlStates();
      }

    private void SetControlStates()
      {
      RouteEditButton.IsEnabled= OptionsView.SelectedRoute!=null;
      RouteDeleteButton.IsEnabled = OptionsView.SelectedRoute != null;
      RouteSaveButton.IsEnabled = OptionsView.RouteAbbrev!=null && OptionsView.RouteAbbrev.Length >= 2;
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

    private void OnEditRoute(object sender, RoutedEventArgs e)
      {
      OptionsView.EditRoute();
      SetControlStates();
      }

    private void OnDeleteRoute(object sender, RoutedEventArgs e)
      {
      OptionsView.DeleteRoute();
      SetControlStates();
      }

    private void OnSaveRoute(object sender, RoutedEventArgs e)
      {
      OptionsView.SaveRoute();
      SetControlStates();
      }

    private void OnClearRoute(object sender, RoutedEventArgs e)
      {
      OptionsView.ClearRoute();
      SetControlStates();
      }

    private void RouteListSelectedItemChanged(object sender, RoutedEventArgs e)
      {
      SetControlStates();
      }

    private void OnLoadRouteList(object sender, RoutedEventArgs e)
      {
      OptionsView.LoadRouteList();
      }

    private void OnAbbrevTextChanged(object sender, RoutedEventArgs e)
      {
      SetControlStates();
      }
    }
	}