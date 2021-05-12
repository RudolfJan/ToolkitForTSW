using System;
using System.Collections.Generic;
using System.Text;

namespace SavCracker.Library.Models
  {
  public struct EngineVersion
    {
    public short Major;
    public short Minor;
    public short Patch;
    public int Build;
    public string BuildId;
    }
  public class HeaderModel
    {
    public EngineVersion Version { get;set; }
    public int SaveGameVersion {get;set;}
    public string SaveGameType { get; set; }
    public int PackageVersion { get; set; }
    public int CustomFormatVersion { get; set; }
    public List<CustomFormatDataEntry> CustomFormatList{get;set;} = new List<CustomFormatDataEntry>();
    }
  }
 