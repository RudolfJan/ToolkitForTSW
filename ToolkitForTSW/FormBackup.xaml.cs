using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ToolkitForTSW
	{
	/// <summary>
	/// Interaction logic for FormBackup.xaml
	/// </summary>
	public partial class FormBackup
		{
		private CBackup Backup { get; set; }

		public FormBackup()
			{
			InitializeComponent();
			Backup = new CBackup();
			DataContext = Backup;
			SetControlStates();
			}

		private void SetControlStates()
			{
			DeleteBackupButton.IsEnabled = BackupSetsDataGrid.SelectedItem != null;
			RestoreBackupButton.IsEnabled = BackupSetsDataGrid.SelectedItem != null;
			}

		private void OnMakeBackupButtonClicked(Object Sender, RoutedEventArgs E)
			{
			Backup.MakeBackup();
			SetControlStates();
			}

		private void OnBackupSetsDataGridSelectionChanged(Object Sender, SelectionChangedEventArgs E)
			{
			SetControlStates();
			}

		private void OnRestoreBackupButtonClicked(Object Sender, RoutedEventArgs E)
			{
			var Source = ((DirectoryInfo) BackupSetsDataGrid.SelectedItem).FullName;
			Backup.RestoreBackup(Source);
			SetControlStates();
			}

		private void OnDeleteBackupButtonClicked(Object Sender, RoutedEventArgs E)
			{
			var Source = ((DirectoryInfo) BackupSetsDataGrid.SelectedItem).FullName;
			Backup.DeleteBackup(Source);
			SetControlStates();
			}

		private void OnSelectAllButtonClicked(Object Sender, RoutedEventArgs E)
			{
			Backup.SetSaveAll();
			}

		private void OnSelectNoneButtonClicked(Object Sender, RoutedEventArgs E)
			{
			Backup.SetSaveNone();
			}

		private void OnOKButtonClicked(Object Sender, RoutedEventArgs E)
			{
			Close();
			}
		}
	}
