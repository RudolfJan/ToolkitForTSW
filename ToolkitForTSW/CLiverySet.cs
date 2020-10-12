using Styles.Library.Helpers;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;


namespace ToolkitForTSW
  {
  public class CLiverySet : Notifier
    {
    /*
    XDocument containing livery specification
    */
    private XDocument _Doc;

    public XDocument Doc
      {
      get { return _Doc; }
      set
        {
        _Doc = value;
        OnPropertyChanged("Doc");
        }
      }

    private String _SetName;

    public String SetName
      {
      get { return _SetName; }
      set
        {
        _SetName = value;
        OnPropertyChanged("SetName");
        }
      }

    private String _Description;

    public String Description
      {
      get { return _Description; }
      set
        {
        _Description = value;
        OnPropertyChanged("Description");
        }
      }

    private ObservableCollection<String> _PakList;

    public ObservableCollection<String> PakList
      {
      get { return _PakList; }
      set
        {
        _PakList = value;
        OnPropertyChanged("PakList");
        }
      }

    private ObservableCollection<FileInfo> _LiverySetList;

    public ObservableCollection<FileInfo> LiverySetList
      {
      get { return _LiverySetList; }
      set
        {
        _LiverySetList = value;
        OnPropertyChanged("LiverySetList");
        }
      }

    private String _FilePath;

    public String FilePath
      {
      get { return _FilePath; }
      set
        {
        _FilePath = value;
        OnPropertyChanged("FilePath");
        }
      }

    private String _Result;

    public String Result
      {
      get { return _Result; }
      set
        {
        _Result = value;
        OnPropertyChanged("Result");
        }
      }

    public CLiverySet()
      {
      PakList = new ObservableCollection<String>();
      }

    private void SetFilePath()
      {
      FilePath = CTSWOptions.LiveriesFolder + SetName + ".xml";
      }

    public void FillLiverySetList()
      {
      var DirPath = CTSWOptions.LiveriesFolder;
      LiverySetList = new ObservableCollection<FileInfo>();
      var Dir = new DirectoryInfo(DirPath);
      var Files = Dir.GetFiles("*.xml");
      foreach (var F in Files)
        {
        LiverySetList.Add(F);
        }
      }

    public void OpenLiverySetFile(String MySetName)
      {
      if (!File.Exists(MySetName))
        {
        CLog.Trace("Livery set not found " + MySetName, LogEventType.Message);
        return;
        }

      SetName = Path.GetFileName(MySetName);
      SetName = SetName?.Substring(0, SetName.Length - 4);
      Doc = XDocument.Load(MySetName);
      Description = Doc.XPathSelectElement("PakSet/Description")?.Value;
      var Paks = Doc.XPathSelectElements("PakSet/PakList/PakName");
      PakList = new ObservableCollection<String>();
      foreach (var V in Paks)
        {
        PakList.Add(V.Value);
        }
      }

    public void AddLiveryPak(String PakName)
      {
      PakList.Add(PakName);
      }

    public void RemoveLiveryPak(String PakName)
      {
      if (!PakList.Remove(PakName))
        {
        Result += CLog.Trace("Cannot remove " + PakName + "from PakList");
        }
      }

    public void AddDescription(String Text)
      {
      }

    public void SaveLiverySetAsFile()
      {
      if (String.IsNullOrEmpty(SetName))
        {
        CLog.Trace("Livery set name is not defined ", LogEventType.Message);
        return;
        }

      SetFilePath();
      CApps.DeleteSingleFile(FilePath);
      Doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement("PakSet",
        new XElement("Description", Description), new XElement("PakList")
        ));
      var Paks = Doc.XPathSelectElement("PakSet/PakList");
      foreach (var V in PakList)
        {
        Paks.Add(new XElement("PakName", V));
        }

      CApps.Save(Doc, FilePath);
      }

    public void InstallSets(FileInfo LiverySet)
      {
      InstallSets(LiverySet.FullName);
      }

    public void InstallSets(String LiverySet)
      {
      try
        {
        OpenLiverySetFile(LiverySet);
        var Destination = CTSWOptions.TrainSimWorldDirectory + @"TS2Prototype\Content\DLC\";
        foreach (var Pak in PakList)
          {
          var Target = Destination + Path.GetFileName(Pak);
          var Source = CTSWOptions.LiveriesFolder + Pak;
          if (!File.Exists(Target))
            {
            File.Copy(Source, Target);
            }
          }
        }
      catch (Exception E)
        {
        Result += CLog.Trace("Cannot install livery set because " + E.Message, LogEventType.Error);
        }
      }
    }
  }
