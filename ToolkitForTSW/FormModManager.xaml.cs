using System;
using System.Data;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using ToolkitForTSW.DataAccess;

namespace ToolkitForTSW
  {
 
  public partial class FormModManager
    {
    public CModManager ModManager { get; set; }

    public FormModManager(CModManager modManager)
      {
      InitializeComponent();
      ModManager = modManager;
      DataContext = ModManager;

      // var WinHeight = SystemParameters.MaximizedPrimaryScreenHeight * 0.85;
      //var AllowedHeightFactor = 0.3;

      PakfileTextBox.IsEnabled = false;
      IsInstalledCheckBox.IsEnabled = false;
      SetControlStates();
      }

    private void SetControlStates()
      {
      SaveModButton.IsEnabled= ModManager.SelectedMod!=null && ModManager.InEditMode==true;
      ActivateModButton.IsEnabled= ModManager.SelectedMod != null && ModManager.SelectedMod.IsInstalled==false;
      DeActivateModButton.IsEnabled = ModManager.SelectedMod != null && ModManager.SelectedMod.IsInstalled == true;
      AddToSetButton.IsEnabled= ModManager.ModSet.SelectedModSet!=null && ModManager.ModSet.SelectedMod!=null;
      RemoveFromSetButton.IsEnabled= ModManager.ModSet.SelectedModSet != null && ModManager.ModSet.SelectedModInSet!=null;
      SaveSetButton.IsEnabled = !string.IsNullOrEmpty(ModManager.ModSet.SetName);
      EditSetButton.IsEnabled = ModManager.ModSet.SelectedModSet != null;
      DeleteSetButton.IsEnabled= ModManager.ModSet.SelectedModSet != null;
      }
  
    private void OnOKButtonClicked(object Sender, RoutedEventArgs E)
      {
      Close();
      }

    // Update the selected record in the available liveries
    private void OnSaveModButtonClicked(object Sender, RoutedEventArgs E)
      {
      ModManager.SaveModProperties();
      AvailablePaksDataGrid.Items.Refresh();
      SetControlStates();
      }

    private void OnAvailablePaksDataGridSelectionChanged(object Sender, SelectionChangedEventArgs E)
      {
      SetControlStates();
      }

    private void OnModSetDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
      {
      SetControlStates();
      }

    private void OnAvailablePaksDataGridSetsSelectionChanged(object sender,
      SelectionChangedEventArgs e)
      {
      SetControlStates();
      }

    private void OnEditModProperties(object sender, RoutedEventArgs e)
      {
      ModManager.EditModProperties();
      SetControlStates();
      }

    private void OnDeleteMod(object sender, RoutedEventArgs e)
      {
      ModManager.DeleteMod();
      AvailablePaksDataGrid.Items.Refresh();
      SetControlStates();
      }

    private void OnActivatePak(object sender, RoutedEventArgs e)
      {
      ModManager.ActivatePak();
      AvailablePaksDataGrid.Items.Refresh();
      SetControlStates();
      }

    private void OnDeactivatePak(object sender, RoutedEventArgs e)
      {
      ModManager.DeactivatePak();
      AvailablePaksDataGrid.Items.Refresh();
      SetControlStates();
      }

    private void OnDeactivateAllMods(object sender, RoutedEventArgs e)
      {
      ModManager.DeactivateAllInstalledPaks();
      AvailablePaksDataGrid.Items.Refresh();
      SetControlStates();
      }

    private void OnEditSetClicked(object sender, RoutedEventArgs e)
      {
      ModManager.ModSet.EditSet();
      ModSetsDataGrid.Items.Refresh();
      SetControlStates();
      }

    private void OnSaveSetClicked(object sender, RoutedEventArgs e)
      {
      ModManager.ModSet.SaveSet();
      ModSetsDataGrid.Items.Refresh();
      ModManager.ModSet.Clear();
      SetControlStates();
      }

    private void OnDeleteSetClicked(object sender, RoutedEventArgs e)
      {
      ModManager.ModSet.DeleteSet();
      ModSetsDataGrid.Items.Refresh();
      SetControlStates();
      }

    private void OnClearSetClicked(object sender, RoutedEventArgs e)
      {
      ModManager.ModSet.Clear();
      }

    private void OnAddToSetButtonClicked(object sender, RoutedEventArgs e)
      {
      ModManager.ModSet.AddModToSet();
      ModsInSetDataGrid.Items.Refresh();
      SetControlStates();
      }

    private void OnRemoveFromSetClicked(object sender, RoutedEventArgs e)
      {
      ModManager.ModSet.RemoveModFromSet();
      ModsInSetDataGrid.Items.Refresh();
      SetControlStates();
      }

    private void OnModSetsDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
      {
      ModsInSetDataGrid.Items.Refresh();
      SetControlStates();
      }

    // https://stackoverflow.com/questions/772841/is-there-selected-tab-changed-event-in-the-standard-wpf-tab-control
    private void OnTabControlSelectionChanged(object sender, SelectionChangedEventArgs e)
      {
      if (e.Source is TabControl)
        {
        //do work when tab is changed
        if (SetsTab.IsSelected)
          {
          ModManager.ModSet.Initialise();
          AvailablePaksDataGridSets.Items.Refresh();
          ModSetsDataGrid.Items.Refresh();
          ModsInSetDataGrid.Items.Refresh();
          }
        }
      }

    private void OnModSetNameChanged(object Sender, TextChangedEventArgs E)
      {
      SetControlStates();
      }

    private void OnModsInSetDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
      {
      SetControlStates();
      }
    }
  }
