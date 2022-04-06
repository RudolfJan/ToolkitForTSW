using Logging.Library;
using Styles.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ToolkitForTSW
  {
  public class CUnpacker : Notifier
    {
    /*
    List of .pak files
    */
    private ObservableCollection<FileInfo> _PakList = null;

    public ObservableCollection<FileInfo> PakList
      {
      get { return _PakList; }
      set
        {
        _PakList = value;
        OnPropertyChanged("PakList");
        }
      }


    /*
Unpacking in progress
*/
    private Boolean _Busy = false;

    public Boolean Busy
      {
      get { return _Busy; }
      set
        {
        _Busy = value;
        OnPropertyChanged("Busy");
        }
      }


    private String _Result = String.Empty;

    public String Result
      {
      get { return _Result; }
      set
        {
        _Result = value;
        OnPropertyChanged("Result");
        }
      }

    public CUnpacker()
      {
      PakList = new ObservableCollection<FileInfo>();
      }

    // No slash after DirName, can be DLC or PAK
    public void LoadPakList(String DirName)
      {
      var path = "";
      if (TSWOptions.CurrentPlatform == PlatformEnum.Steam)
        {
        path = TSWOptions.SteamTrainSimWorldDirectory;
        }
      if (TSWOptions.CurrentPlatform == PlatformEnum.EpicGamesStore)
        {
        path = TSWOptions.EGSTrainSimWorldDirectory;
        }
      var DirectoryName = path + "TS2Prototype\\Content\\" + DirName + "\\";
      try
        {
        var Dir = new DirectoryInfo(DirectoryName);
        var Files = Dir.GetFiles("*.pak");
        PakList.Clear();
        foreach (var FilePath in Files)
          {
          PakList.Add(FilePath);
          }
        }
      catch (Exception E)
        {
        Result = Log.Trace(
          "Failed to create list of .pak files for directory " + DirectoryName + "because " + E.Message,
          LogEventType.Error);
        }
      }

    public void UnpackAll()
      {
      Result += "Unpacking core game\r\n";
      Result += UnpackDir("Paks");
      Result += " Unpacking all DLC\r\n";
      Result += UnpackDir("DLC");
      }

    private String UnpackDir(String PakDir)
      {
      var Counter = 0;
      List<Task> TaskList = new List<Task>();
      try
        {
        String[] AllPaks = Directory.GetFiles(TSWOptions.TrainSimWorldDirectory + "\\TS2Prototype\\Content\\" + PakDir,
          "*.pak");
        Result += "Found " + AllPaks.Length.ToString() + " paks\r\n";
        foreach (String Pak in AllPaks)
          {
          String Destination = Pak.Substring(Pak.LastIndexOf("\\", StringComparison.Ordinal));
          Result += "Unpack started " + Path.GetFileName(Pak) + "\r\n";
          Result += Result;
          Destination = TSWOptions.UnpackFolder + Destination;
          Directory.CreateDirectory(Destination);
          Busy = true;
          var MyTask = Task.Run(() => CApps.UnPack(Pak, Destination));
          TaskList.Add(MyTask);
          }

        Task Terminated = Task.WhenAll(TaskList);
        while (!Terminated.IsCompleted)
          {
          Counter++;
          Result += "Working " + Counter + " " + Busy + "\r\n";
          System.Windows.Forms.Application.DoEvents(); // avoid blocking messages
          Thread.Sleep(1000);
          }
        }
      catch (Exception E)
        {
        Result += Log.Trace(@"Unpack all .pak files failed: " + E.Message, LogEventType.Error);
        }

      Busy = false;
      Result += "Unpacking done\r\n";
      return Result;
      }

    public String UnpackSelected(ObservableCollection<FileInfo> SelectedPaks)
      {
      var Counter = 0;
      List<Task> TaskList = new List<Task>();
      try
        {
        Result += "Found " + SelectedPaks.Count.ToString() + " paks\r\n";
        foreach (var Pak in SelectedPaks)
          {
          String Destination = Pak.FullName.Substring(Pak.FullName.LastIndexOf("\\", StringComparison.Ordinal));
          Result += "Unpack started " + Path.GetFileName(Pak.FullName) + "\r\n";
          Result += Result;
          Destination = TSWOptions.UnpackFolder + Destination;
          Directory.CreateDirectory(Destination);
          Busy = true;
          var MyTask = Task.Run(() => CApps.UnPack(Pak.FullName, Destination));
          TaskList.Add(MyTask);
          }

        Task Terminated = Task.WhenAll(TaskList);
        while (Terminated != null && !Terminated.IsCompleted)
          {
          Counter++;
          Result += "Working " + Counter + " " + Busy + "\r\n";
          System.Windows.Forms.Application.DoEvents(); // avoid blocking messages
          Thread.Sleep(1000);
          }
        }
      catch (Exception E)
        {
        Result += Log.Trace(@"Unpack all .pak files failed: " + E.Message, LogEventType.Error);
        }

      Busy = false;
      Result += "Unpacking done\r\n";
      return Result;
      }

    public void UnpackFile(String Pak)
      {
      var Counter = 0;
      string input = string.Empty;
      Task MyTask = null;
      try
        {
        String Destination = Pak.Substring(Pak.LastIndexOf("\\", StringComparison.Ordinal) + 1);
        Result += "Unpack started " + Path.GetFileName(Pak) + "\r\n";
        Destination = TSWOptions.UnpackFolder + Destination;
        Directory.CreateDirectory(Destination);

        if (TSWOptions.CurrentPlatform == PlatformEnum.Steam)
          {
          input = $"{TSWOptions.SteamTrainSimWorldDirectory}{Pak}";
          }
        else
          {
          input = $"{TSWOptions.EGSTrainSimWorldDirectory}{Pak}";
          }
        Busy = true;
        MyTask = Task.Run(() => CApps.UnPack(input, Destination));
        }
      catch (Exception E)
        {
        Result += Log.Trace(@"Unpack .pak file failed: " + input + " " + E.Message, LogEventType.Error);
        }

      while (MyTask != null && !MyTask.IsCompleted)
        {
        Counter++;
        Result += "Working " + Counter + " " + Busy + "\r\n";
        System.Windows.Forms.Application.DoEvents(); //TODO: refactor this  avoid blocking messages
        Thread.Sleep(1000);
        }
      Busy = false;
      Result += "Unpacked " + Pak + "\r\n";
      }
    }
  }