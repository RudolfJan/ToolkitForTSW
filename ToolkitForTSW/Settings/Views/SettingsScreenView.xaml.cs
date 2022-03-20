using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace ToolkitForTSW.Settings.Views
  {
  /// <summary>
  /// Interaction logic for SettingsScreenView.xaml
  /// </summary>
  public partial class SettingsScreenView : UserControl
    {
    public SettingsScreenView()
      {
      InitializeComponent();
      SortDataGrid(VideoModesDataGrid, 0, ListSortDirection.Descending);
      SortDataGrid(VideoModesDataGrid, 2, ListSortDirection.Descending, true);
      }

    public static void SortDataGrid(DataGrid MyDataGrid, Int32 ColumnIndex = 0,
    ListSortDirection MySortDirection = ListSortDirection.Ascending, Boolean Add = false)
      {
      var Column = MyDataGrid.Columns[ColumnIndex];

      // Clear current sort descriptions
      if (!Add)
        {
        MyDataGrid.Items.SortDescriptions.Clear();
        }

      // Add the new sort description
      MyDataGrid.Items.SortDescriptions.Add(new SortDescription(Column.SortMemberPath,
        MySortDirection));

      // Apply sort
      foreach (var Col in MyDataGrid.Columns)
        {
        Col.SortDirection = null;
        }

      Column.SortDirection = MySortDirection;

      // Refresh items to display sort
      MyDataGrid.Items.Refresh();
      }
    }
  }
