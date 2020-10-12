using Styles.Library.Helpers;
using System;
using System.Collections.ObjectModel;
using System.IO;


namespace ToolkitForTSW
{
	public class CRouteGuideManager : Notifier
		{
		private CTreeItemProvider _FileTree;
		public CTreeItemProvider FileTree
			{
			get => _FileTree;
			set
				{
				_FileTree = value;
				OnPropertyChanged("FileTree");
				}
			}

		private ObservableCollection<CDirTreeItem> _TreeItems;
		public ObservableCollection<CDirTreeItem> TreeItems
			{
			get => _TreeItems;
			set
				{
				_TreeItems = value;
				OnPropertyChanged("TreeItems");
				}
			}

		private String _Results;

		public String Results
			{
			get { return _Results; }
			set
				{
				_Results = value;
				OnPropertyChanged("Results");
				}
			}



		public CRouteGuideManager()
			{
			FillRouteGuidesList();
			}

		public void FillRouteGuidesList()
			{
			var Dir = new DirectoryInfo(CTSWOptions.ManualsFolder + "RouteGuides\\");
			FileTree = new CTreeItemProvider();
			TreeItems = FileTree.GetItems(Dir.FullName);
			}
		}
	}