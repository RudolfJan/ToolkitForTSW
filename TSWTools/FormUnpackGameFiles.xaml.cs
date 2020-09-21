using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace TSWTools
  {
  /// <summary>
  /// Interaction logic for FormUnpackGameFiles.xaml
  /// </summary>
  public partial class FormUnpackGameFiles
    {
    public CUnpacker Unpacker { get; set; }

    public FormUnpackGameFiles()
      {
      InitializeComponent();
      var WinHeight = SystemParameters.MaximizedPrimaryScreenHeight * 0.85;
      var AllowedHeightFactor = 0.7;
      if (PakListView.Height / WinHeight > AllowedHeightFactor)
        {
        PakListView.Height = PakListView.MaxHeight * PakListView.MaxHeight / WinHeight;
        }

      Unpacker = new CUnpacker();
      Unpacker.LoadPakList("DLC");
      DataContext = Unpacker;
      SetControlStates();
      }

    private void SetControlStates()
      {
      UnpackSelectedDLCButton.IsEnabled = PakListView.SelectedItems.Count > 0;
      }

    private void OnOkButtonClicked(Object Sender, RoutedEventArgs E)
      {
      Close();
      }

    private void OnUnpackCoreButtonClicked(Object Sender, RoutedEventArgs E)
      {
      var PakName = CTSWOptions.TrainSimWorldDirectory +
                    "TS2Prototype\\Content\\Paks\\TS2Prototype-WindowsNoEditor.pak";
      Unpacker.UnpackFile(PakName);
      }

    private void OnUnpackAllDlcButtonClicked(Object Sender, RoutedEventArgs E)
      {
      Unpacker.UnpackAll();
      }

    private void OnUnpackSelectedDlcButtonClicked(Object Sender, RoutedEventArgs E)
      {
      var SelectedFiles = new ObservableCollection<FileInfo>();
      foreach (var F in PakListView.SelectedItems)
        {
        SelectedFiles.Add((FileInfo) F);
        }

      Unpacker.UnpackSelected(SelectedFiles);
      }

    private void OnPakListViewSelectionChanged(Object Sender, SelectionChangedEventArgs E)
      {
      SetControlStates();
      }
    }
  }
