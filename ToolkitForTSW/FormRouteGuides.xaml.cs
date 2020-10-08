using System;
using System.Windows;

namespace TSWTools
	{
	/// <summary>
	/// Interaction logic for FormRouteGuides.xaml
	/// </summary>
	public partial class FormRouteGuides
		{
		public CRouteGuideManager RouteGuideManager { get; set; }
    public CMain MainData { get; set; }

    public FormRouteGuides(CMain MyMainData)
			{
			InitializeComponent();
      MainData = MyMainData;
			RouteGuideManager = new CRouteGuideManager();
			RouteGuideManager.FillRouteGuidesList();
			DataContext = RouteGuideManager;
			SetControlStates();
			}

		private void SetControlStates()
			{
			OpenGuideButton.IsEnabled = FileTreeView.SelectedItem != null;
			}

		private void OnFileTreeViewSelectedItemChanged(Object Sender, RoutedPropertyChangedEventArgs<Object> E)
			{
			SetControlStates();
			}

		private void OnOpenGuideButtonClicked(Object Sender, RoutedEventArgs E)
      {
      var SelectedFile = CApps.DoubleQuotes(((CDirTreeItem) FileTreeView.SelectedItem).Path);
			MainData.Result+= CApps.OpenGenericFile(SelectedFile);
      }

		private void OnOKButtonClicked(Object Sender, RoutedEventArgs E)
			{
			Close();
			}
	}
}