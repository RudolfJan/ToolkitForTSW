using Styles.Library.Helpers;
using System;
using System.IO;
using System.Collections.ObjectModel;


namespace ToolkitForTSW
	{
	public class CDirTreeItem : Notifier
		{
		private String _Name = String.Empty;
		public String Name
			{
			get => _Name;
			set
				{
				_Name = value;
				OnPropertyChanged("Name");
				}
			}
		private String _Path = String.Empty;
		public String Path
			{
			get => _Path;
			set
				{
				_Path = value;
				OnPropertyChanged("Path");
				}
			}

		private Boolean _IsSelected;
		public Boolean IsSelected
			{
			get => _IsSelected;
			set
				{
				_IsSelected = value;
				OnPropertyChanged("IsSelected");
				}
			}

		public override String ToString()
			{
			return Name;
			}
		}

	public class CFileItem : CDirTreeItem
		{
		private FileInfo _FileDetails;
		public FileInfo FileDetails
			{
			get => _FileDetails;
			set
				{
				_FileDetails = value;
				OnPropertyChanged("FileDetails");
				}
			}
		}

	public class CDirectoryItem : CDirTreeItem
		{
		private ObservableCollection<CDirTreeItem> _DirectoryItems;
		public ObservableCollection<CDirTreeItem> DirectoryItems
			{
			get => _DirectoryItems;
			set
				{
				_DirectoryItems = value;
				OnPropertyChanged("DirectoryItems");
				}
			}

		private DirectoryInfo _DirectoryDetails;
		public DirectoryInfo DirectoryDetails
			{
			get => _DirectoryDetails;
			set
				{
				_DirectoryDetails = value;
				OnPropertyChanged("DirectoryDetails");
				}
			}

		public CDirectoryItem()
			{
			DirectoryItems = new ObservableCollection<CDirTreeItem>();
			}
		}

	public class CTreeItemProvider
		{
		public ObservableCollection<CDirTreeItem> GetItems(String Path, Boolean Always = false)
			{
			var Items = new ObservableCollection<CDirTreeItem>();

			var DirInfo = new DirectoryInfo(Path);
      foreach (var Directory in DirInfo.GetDirectories())
        {
        var DirItem = new CDirectoryItem
          {
          Name = Directory.Name,
          Path = Directory.FullName,
          DirectoryItems = GetItems(Directory.FullName, Always)
          };
        Items.Add(DirItem);
        }

			foreach (var File in DirInfo.GetFiles())
				{
				var Item = new CFileItem
					{
					Name = File.Name,
					Path = File.FullName,
					};
				Items.Add(Item);
				}
			return Items;
			}

		// Will only return directories 
		public ObservableCollection<CDirTreeItem> GetDirItems(String Path)
			{
			var Items = new ObservableCollection<CDirTreeItem>();

			var DirInfo = new DirectoryInfo(Path);
      foreach (var Directory in DirInfo.GetDirectories())
				{
				var DirItem = new CDirectoryItem
          {
          Name = Directory.Name,
          Path = Directory.FullName
          };
				DirItem.DirectoryItems = GetDirItems(Directory.FullName);
				Items.Add(DirItem);
				}
			return Items;
			}
		}
	}

