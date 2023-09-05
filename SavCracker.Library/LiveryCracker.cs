// Ignore Spelling: sav

using SavCracker.Library.Models;
using System.Collections.Generic;
using System.Text;

namespace SavCracker.Library
  {
  public class LiveryCracker
    {
    public string LogString { get; set; } = string.Empty;
    public List<LiveryModel> LiveryList { get; set; }
    public static readonly byte[] Header = Encoding.ASCII.GetBytes("GVAS");
    public SavCracker savCracker { get; set; }
    public LiveryCracker()
      {
      var liveryFile = "D:\\UGCLiveries_0 two empties.sav";
      //var jsonFile = $"{Path.GetDirectoryName(liveryFile)}{Path.GetFileNameWithoutExtension(liveryFile)}.json";
      savCracker = new SavCracker(liveryFile);
      ReadHeader();
      ReadLiveryArray();
      //var json = JsonConvert.SerializeObject(savCracker, new JsonSerializerSettings { Formatting = Newtonsoft.Json.Formatting.Indented });
      //File.WriteAllText(jsonFile, json);
      }



    private void ReadHeader()
      {
      savCracker.Position = Header.Length; // skip header bytes
      var header = new HeaderModel
        {
        SaveGameVersion = savCracker.GetInt(),
        PackageVersion = savCracker.GetInt()
        };
      EngineVersion version;
      version.Major = savCracker.GetShort();
      version.Minor = savCracker.GetShort();
      version.Patch = savCracker.GetShort();
      version.Build = savCracker.GetInt();
      version.BuildId = savCracker.GetString();

      header.Version = version;
      header.CustomFormatVersion = savCracker.GetInt();
      var entriesCount = savCracker.GetInt();
      for (int i = 0; i < entriesCount; i++)
        {
        var entry = new CustomFormatDataEntry
          {
          Id = savCracker.GetGuid(),
          Value = savCracker.GetInt()
          };
        header.CustomFormatList.Add(entry);
        }
      header.SaveGameType = savCracker.GetString();
      }

    public void ReadLiveryArray()
      {
      var arrayName = savCracker.GetString();
#pragma warning disable IDE0059 // Unnecessary assignment of a value
      var arrayType = savCracker.GetString();

      var reskins = savCracker.ExtractArrayProperty(arrayName);
#pragma warning restore IDE0059 // Unnecessary assignment of a value
      }
    }
  }

