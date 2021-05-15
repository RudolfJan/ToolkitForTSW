using Logging.Library;
using Utilities.Library;
using SavCracker.Library.Models;
using Styles.Library.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using ToolkitForTSW;

namespace ToolkitForTSW
  {
  public class CPublishScenario: Notifier
    {
    public SavScenarioModel SavScenario{ get; set; }

    private string _author;
    public string Author
      {
      get { return _author; }
      set
        {
        _author = value;
        OnPropertyChanged("Author");
        }
      }

    private string _description;
    public string Description
      {
      get { return _description; }
      set
        {
        _description = value;
        OnPropertyChanged("Description");
        }
      }

    public string ScenarioFilePath { get; set; }

  public CPublishScenario(string scenarioFilePath, SavScenarioModel savScenario)
      {
      SavScenario = savScenario;
      ScenarioFilePath = scenarioFilePath;
      }


    public void CreateDocumentFile()
      {
      var fileBase = $"{CTSWOptions.ScenarioFolder}{SavScenario.RouteAbbreviation}-{SavScenario.ScenarioName}";
      var templateList = GetTemplates(CTSWOptions.TemplateFolder);
      if (templateList.Count < 1)
        {
        Log.Trace("No template found, cannot create scenario documentation");
        return;
        }
      var template = File.ReadAllText(templateList[0].FullName);
      var output = CreateDocumentString(template);
      File.WriteAllText($"{fileBase}.html",output);
      var targetFilePath= $"{CTSWOptions.ScenarioFolder}{Path.GetFileName(ScenarioFilePath)}";
      File.Copy(ScenarioFilePath,targetFilePath,true);
      FileHelpers.DeleteSingleFile($"{fileBase}.zip");
      using (ZipArchive archive = ZipFile.Open($"{fileBase}.zip", ZipArchiveMode.Create))
        {
        archive.CreateEntryFromFile(targetFilePath, Path.GetFileName(ScenarioFilePath));
        archive.CreateEntryFromFile($"{fileBase}.html", $"{SavScenario.RouteAbbreviation}-{SavScenario.ScenarioName}.html");
        }
      }

    public static List<FileInfo> GetTemplates(string templateDir)
      {
      var templateList = new List<FileInfo>();
      DirectoryInfo dir= new DirectoryInfo(templateDir);
      FileInfo[] files = dir.GetFiles("*.html", SearchOption.TopDirectoryOnly);
      foreach ( var file in files)
        {
        templateList.Add(file);
        }

      templateList = templateList.OrderBy(x => x.Name).ToList();
      return templateList;
      }

    public string CreateDocumentString(string template)
      {
      var output = template.Replace("{Author}", Author)
        .Replace("{Description}", Description)
        .Replace("{Filename}",GetSavFileName(SavScenario))
        .Replace("{ScenarioName}", SavScenario.ScenarioName)
        .Replace("{RouteName}", SavScenario.RouteName)
        .Replace("{PlayerEngine}", GetPlayerEngine(SavScenario))
        .Replace("{ServiceList}", GetServiceList(SavScenario));

      return output;
      }

    private string GetServiceList(SavScenarioModel savScenario)
      {
      var output = string.Empty;
      output = "<table>\n";

      foreach(var service in savScenario.SavServiceList)
        {
        output += $"\t<tr>\n\t\t<td>{service.IsPlayerService}</td>\n" +
                  $"\t\t<td>{service.StartTimeText}</td>\n"+
                  $"\t\t<td>{service.EngineName}</td>"+
                  $"\t\t<td>{service.ConsistName}</td>"+
                  $"\t\t<td>{service.ServiceName}</td>"+
                  "\t<tr>\n";
        }

      output += "</table\n";
      return output;
      }

    private string GetPlayerEngine(SavScenarioModel savScenario)
      {
      foreach (var service in savScenario.SavServiceList)
        {
        if (service.IsPlayerService)
          {
          return service.EngineName;
          }
        }

      return "";
      }

    private string GetSavFileName(SavScenarioModel savScenario)
      {
      return $"USD_{SavScenario.ScenarioGuid.ToString().ToUpper()}.sav";
      }
    }
  }
