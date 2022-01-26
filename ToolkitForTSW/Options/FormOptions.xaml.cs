using Filter.Library.WPF.ViewModels;
using Screenshots.Library.WPF.ViewModels;
using System;
using System.Windows;

namespace ToolkitForTSW
  {

  public partial class FormOptions : Window
    {
    OptionsViewModel OptionsView { get; set; }
    private bool _canClose = false;

    public FormOptions()
      {
      InitializeComponent();
      OptionsView = new OptionsViewModel();
      DataContext = OptionsView;
      TagsView.TagAndCategoryData = new TagAndCategoryViewModel();
      TagsView.DataContext = TagsView.TagAndCategoryData;
      CollectionsView.ScreenshotCollectionManager = new ScreenshotCollectionViewModel();
      CollectionsView.DataContext = CollectionsView.ScreenshotCollectionManager;
      SetControlStates();
      }

    private void SetControlStates()
      {
      RouteEditButton.IsEnabled = OptionsView.SelectedRoute != null;
      RouteDeleteButton.IsEnabled = OptionsView.SelectedRoute != null;
      RouteSaveButton.IsEnabled = OptionsView.RouteAbbrev != null && OptionsView.RouteAbbrev.Length >= 2;
      OKButton.IsEnabled = _canClose == true;
      }

    private void OnOKButtonClicked(Object sender, RoutedEventArgs e)
      {
      DialogResult=true;
      Close();
      }

    private void OnCancelButtonClicked(Object sender, RoutedEventArgs e)
      {
      OptionsView.LoadOptions();
      SetControlStates();
      DialogResult=false;
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

    private void FindDefaultsButton_Click(object sender, RoutedEventArgs e)
      {
      OptionsView.FindDefaults();
      }

    private void ClearTrackIr_Click(object sender, RoutedEventArgs e)
      {
      OptionsView.ClearTrackIr();
      }

    private void Clear7Zip_Click(object sender, RoutedEventArgs e)
      {
      OptionsView.Clear7Zip();
      }

    private void ClearUmodel_Click(object sender, RoutedEventArgs e)
      {
      OptionsView.ClearUmodel();
      }

    private void ClearUnreal_Click(object sender, RoutedEventArgs e)
      {
      OptionsView.ClearUnreal();
      }

    private void ClearTextEditor_Click(object sender, RoutedEventArgs e)
      {
      OptionsView.CleartextEditor();
      }

    private void RevertButton_Click(object sender, RoutedEventArgs e)
      {

      }

    private void ClearEGS_Click(object sender, RoutedEventArgs e)
      {
      OptionsView.ClearEGS();
      }

    private void ClearSteamProgramFolder_Click(object sender, RoutedEventArgs e)
      {
      OptionsView.ClearSteamProgramFolder();
      }

    private void ClearTSW2Program_Click(object sender, RoutedEventArgs e)
      {
      OptionsView.ClearSteam();
      }

    private void ClearToolkitFolder_Click(object sender, RoutedEventArgs e)
      {
      OptionsView.ClearToolkitFolder();
      }

    private void ClearBackupFolder_Click(object sender, RoutedEventArgs e)
      {
      OptionsView.ClearBackupFolder();
      }
    }
  }