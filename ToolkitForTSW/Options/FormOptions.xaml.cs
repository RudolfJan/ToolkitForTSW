using Filter.Library.WPF.ViewModels;
using Screenshots.Library.WPF.ViewModels;
using System;
using System.Windows;

namespace ToolkitForTSW
  {

  public partial class FormOptions : Window
    {
    OptionsViewModel OptionsView { get; set; }
    private bool _canClose=false;

    public FormOptions()
      {
      InitializeComponent();
      OptionsView = new OptionsViewModel();
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
      OKButton.IsEnabled= _canClose==true;
      }

    private void OnOKButtonClicked(Object sender, RoutedEventArgs e)
      {
      Close();
      }

    private void OnCancelButtonClicked(Object sender, RoutedEventArgs e)
      {
      DialogResult = false;
      Close();
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

    private void SaveButton_Click(object sender, RoutedEventArgs e)
      {
      _canClose = true;
      OptionsView.SaveOptions();
      SetControlStates();
      }
    }
  }