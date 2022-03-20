using Logging.Library;
using System;
using System.Collections.Generic;
using System.IO;
using ToolkitForTSW.Settings.Enums;
using Utilities.Library;

namespace ToolkitForTSW.Settings
  {
  public class SettingsManagerLogic : ISettingsManagerLogic
    {
    #region Properties
    private String _SaveSetName;
    public String SaveSetName
      {
      get { return _SaveSetName; }
      set
        {
        _SaveSetName = value;
        }
      }

    private SectionEnum _CurrentSection;

    public SectionEnum CurrentSection
      {
      get { return _CurrentSection; }
      set
        {
        _CurrentSection = value;
        }
      }

    /*
Dictionary to contain all settings
*/
    private List<ISetting> _SettingsDictionary;

    public List<ISetting> SettingsDictionary
      {
      get { return _SettingsDictionary; }
      set
        {
        _SettingsDictionary = value;
        }
      }

    /*
    Contains text for [System.Core] part in Engine.ini
    */
    private String _SectionCore;
    public String SectionCore
      {
      get { return _SectionCore; }
      set
        {
        _SectionCore = value;
        }
      }

    private String _Result;
    public String Result
      {
      get { return _Result; }
      set
        {
        _Result = value;
        }
      }

    public string ExperimentalSettingsString { get; set; }
    #endregion

    #region Initialization
    public SettingsManagerLogic()
      {
      SettingsDictionary = new List<ISetting>();
      }
    #endregion
    #region Filehandling


    public static FileInfo GetInGameSettingsLocation()
      {
      var MyPath = TSWOptions.GetSaveLocationPath();
      MyPath += @"Saved\Config\WindowsNoEditor\GameUserSettings.ini";
      return new FileInfo(MyPath);
      }

    public static FileInfo GetInGameEngineIniLocation()
      {
      var MyPath = TSWOptions.GetSaveLocationPath();
      MyPath += @"Saved\Config\WindowsNoEditor\Engine.ini";
      return new FileInfo(MyPath);
      }

    public static FileInfo GetSavedSettingsLocation(DirectoryInfo saveSet)
      {
      var MyPath = saveSet.FullName;
      MyPath += @"\GameUserSettings.ini";
      return new FileInfo(MyPath);
      }

    public static FileInfo GetEngineIniLocation(DirectoryInfo saveSet)
      {
      var MyPath = saveSet.FullName;
      MyPath += @"\Engine.ini";
      return new FileInfo(MyPath);
      }

    public void LoadSettingsInDictionary(FileInfo SettingsFile, FileInfo EngineIniFile = null)
      {
      if (!File.Exists(SettingsFile.FullName))
        {
        Result += Log.Trace("Settings file " + SettingsFile.FullName + " not found",
          LogEventType.Message);
        return;
        }

      SettingsDictionary.Clear();
      SectionCore = ""; // Make sure this section is cleared to avoid double data entries!
      ProcessSettingsFile(SettingsFile);

      if (EngineIniFile == null)
        {
        Result += Log.Trace("Settings file Engine.ini" + " not specified");
        return;
        }
      if (!File.Exists(EngineIniFile.FullName))
        {
        Result += Log.Trace("Engine.ini file " + EngineIniFile.FullName + " not found");
        return;
        }
      ProcessSettingsFile(EngineIniFile);
      }

    private void ProcessSettingsFile(FileInfo SettingsFile)
      {
      bool inExperimental = false;
      ExperimentalSettingsString = string.Empty;
      try
        {
        using (var Reader = new StreamReader(SettingsFile.FullName))
          {
          String Str;
          CurrentSection = SectionEnum.None;
          while ((Str = Reader.ReadLine()) != null)
            {
            if (inExperimental)
              {
              inExperimental = false; // skip one line in this case
              }
            else
              {
              if (Str.StartsWith("["))
                {
                var Setting = new Setting(Str, CurrentSection);
                CurrentSection = Setting.Section;
                if (CurrentSection != SectionEnum.Core)
                  {
                  }
                }
              else
                {
                if (CurrentSection == SectionEnum.Core)
                  {
                  SectionCore += Str + "\r\n";
                  }
                else
                  {
                  if (Str.Length > 1)
                    {
                    if (Str.StartsWith(";"))
                      {
                      inExperimental = true;
                      ExperimentalSettingsString += $"{Str.Substring(1)}\r\n";
                      }
                    else
                      {
                      var Setting = new Setting(Str, CurrentSection);
                      SettingsDictionary.Add(Setting);
                      }
                    }
                  }
                }
              }
            }
          }
        }
      catch (Exception E)
        {
        Result += Log.Trace(
          "Cannot read Settings file " + SettingsFile.FullName + " because " + E.Message,
          LogEventType.Error);
        return;
        }
      }

    public void WriteSettingsToSaveSet()
      {
      if (SaveSetName.Length < 3)
        {
        Result += Log.Trace("SaveSet name is too short", LogEventType.Error);
        return; // failed, should never happen
        }

      var Path = TSWOptions.GetOptionsSetPath() + SaveSetName + "\\"; // Warning order is important here
      if (!Directory.Exists(Path))
        {
        var Dir = Directory.CreateDirectory(Path);
        }
      var SettingsFile = new FileInfo(Path + "GameUserSettings.ini");
      WriteSettingsInDictionary(SettingsFile);
      var EngineIniFile = new FileInfo(Path + "Engine.ini");
      //SettingsExperimental.SaveValueSetToSaveSet(Path);
      WriteSettingsInDictionary(EngineIniFile, true);
      }

    public void WriteSettingsInDictionary(FileInfo SettingsFile, Boolean EngineIni = false)
      {
      if (SettingsFile == null)
        {
        return;
        }

      try
        {
        using (var Writer = new StreamWriter(SettingsFile.FullName))
          {
          if (EngineIni)
            {
            WriteSection(Writer, SectionEnum.Core);
            if (TSWOptions.UseAdvancedSettings)
              {
              WriteSection(Writer, SectionEnum.System);
              }
            Writer.Write(ExperimentalSettingsString); // this string will be filled by SettingsExperimentalViewModel
            }
          else
            {
            WriteSection(Writer, SectionEnum.User);
            WriteSection(Writer, SectionEnum.Engine);
            WriteSection(Writer, SectionEnum.Scalability);
            }
          }
        }
      catch (Exception E)
        {
        Result += Log.Trace(
          "Cannot write Settings file " + SettingsFile.FullName + " because " + E.Message,
          LogEventType.Error);
        return;
        }
      }

    private void WriteSection(StreamWriter Writer, SectionEnum Section)
      {
      if (Section == SectionEnum.Core)
        {
        Writer.WriteLine(Section.ToName());
        Writer.WriteLine(SectionCore);
        return;
        }
      Writer.WriteLine(Section.ToName());
      foreach (var Entry in SettingsDictionary)
        {
        if (Entry.Value.Length > 0 && Section == Entry.Section)
          {
          Writer.WriteLine(Entry.ToString());
          }
        }
      }

    private Int32 GetItemIndex(String Key)
      {
      var Idx = 0;
      foreach (var Setting in SettingsDictionary)
        {
        if (String.CompareOrdinal(Key, Setting.Key) == 0)
          {
          return Idx;
          }

        Idx++;
        }
      return -1;
      }

    public void UpdateSetting(String Key, String Value, SectionEnum Section)
      {
      var Idx = GetItemIndex(Key);
      if (Idx == -1)
        {
        var Setting = new Setting(Key, Value, Section);
        SettingsDictionary.Add(Setting);
        Result += Log.Trace("Settings key not found " + Key);
        return;
        }
      SettingsDictionary[Idx].Value = Value;
      }

    public void GetSetting(String Key, out String Value)
      {
      var Idx = GetItemIndex(Key);
      if (Idx == -1)
        {
        Result += Log.Trace("Settings key not found " + Key);
        Value = String.Empty;
        return;
        }

      Value = SettingsDictionary[Idx].Value;
      }

    #endregion
    }
  }
