using System;
using System.Windows;
using System.Windows.Data;
using Microsoft.Win32;
using System.IO;

namespace ToolkitForTSW
	{
	/// <summary>
	/// Interaction logic for FormLogViewer.xaml
	/// </summary>
	public partial class FormLogViewer
		{
		CLog Log { get; set; }
		CLogFilter Filter { get; set; }

		public FormLogViewer(CLog MyLog)
			{
			InitializeComponent();
			var WinHeight = SystemParameters.MaximizedPrimaryScreenHeight * 0.85;
			var AllowedHeightFactor = 0.65;
			if (LogView.Height / WinHeight > AllowedHeightFactor)
				{
				LogView.Height = LogView.MaxHeight * LogView.MaxHeight / WinHeight;
				}

			Log = MyLog;
			DataContext = Log;
			Filter = new CLogFilter(DebugCheckBox.IsChecked != null && (Boolean) DebugCheckBox.IsChecked,
				ErrorCheckBox.IsChecked != null && (Boolean) ErrorCheckBox.IsChecked,
				MessageCheckBox.IsChecked != null && (Boolean) MessageCheckBox.IsChecked,
				EventCheckBox.IsChecked != null && (Boolean) EventCheckBox.IsChecked);

			if (CollectionViewSource.GetDefaultView(LogView.ItemsSource) is ListCollectionView View)
				{
				View.Filter = Filter.EventTypeFilter;
				View.Refresh();
				}
			}

		private void OnLogViewDataContextChanged(Object Sender, DependencyPropertyChangedEventArgs E)
			{
			LogView.Items.MoveCurrentToLast();
			LogView.ScrollIntoView(LogView.Items.CurrentItem); // scroll to last item
			}

		private void OnClearLogButtonClicked(Object Sender, RoutedEventArgs E)
			{
			Log.LogManager.Clear();
			}

		private void OnSaveLogButtonClicked(Object Sender, RoutedEventArgs E)
			{
			var FileDialog = new SaveFileDialog
				{
				InitialDirectory = CTSWOptions.TempFolder,
				RestoreDirectory = true,
				Title = "Save log file",
				Filter = "Text file (*.txt)|*.txt|All files (*.*)|*.*"
				};

			if (FileDialog.ShowDialog() == true)
				{
				var AllText = String.Empty;
				foreach (var X in Log.LogManager)
					{
					AllText += X + "\r\n";
					}

				File.WriteAllText(FileDialog.FileName, AllText);
				}
			}

		private void OnFilterChanged(Object Sender, RoutedEventArgs E)
			{
			if (DebugCheckBox != null && ErrorCheckBox != null && MessageCheckBox != null &&
			    EventCheckBox != null)
				{
				if (Filter == null)
					{
					Filter = new CLogFilter(
						DebugCheckBox.IsChecked != null && (Boolean) DebugCheckBox.IsChecked,
						ErrorCheckBox.IsChecked != null && (Boolean) ErrorCheckBox.IsChecked,
						MessageCheckBox.IsChecked != null && (Boolean) MessageCheckBox.IsChecked,
						EventCheckBox.IsChecked != null && (Boolean) EventCheckBox.IsChecked);
					}
				else
					{
					Filter.UpdateFilterSettings(
						DebugCheckBox.IsChecked != null && (Boolean) DebugCheckBox.IsChecked,
						ErrorCheckBox.IsChecked != null && (Boolean) ErrorCheckBox.IsChecked,
						MessageCheckBox.IsChecked != null && (Boolean) MessageCheckBox.IsChecked,
						EventCheckBox.IsChecked != null && (Boolean) EventCheckBox.IsChecked);
					}

				if (CollectionViewSource.GetDefaultView(LogView.ItemsSource) is ListCollectionView View)
					{
					View.Filter = Filter.EventTypeFilter;
					View.Refresh();
					}
				}
			}

		private void OnOKButtonClicked(Object Sender, RoutedEventArgs E)
			{
			Close();
			}
		}
	}
