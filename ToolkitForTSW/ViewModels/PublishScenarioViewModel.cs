using Caliburn.Micro;
using Logging.Library;
using SavCracker.Library.Models;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Utilities.Library;

namespace ToolkitForTSW.ViewModels
  {
  public class PublishScenarioViewModel : Screen

    {
    public SavScenarioModel SavScenario { get; set; }

    private string _author;
    public string Author
      {
      get { return _author; }
      set
        {
        _author = value;
        NotifyOfPropertyChange(() => Author);
        NotifyOfPropertyChange(() => CanSave);
        }
      }

    private string _description;
    private string _scenarioFilePath;

    public string Description
      {
      get { return _description; }
      set
        {
        _description = value;
        NotifyOfPropertyChange(() => Description);
        NotifyOfPropertyChange(() => CanSave);
        }
      }

    public string ScenarioFilePath
      {
      get
        {
        return _scenarioFilePath;
        }
      set
        {
        _scenarioFilePath = value;
        NotifyOfPropertyChange(() => ScenarioFilePath);
        }
      }

    private BindableCollection<FileInfo> _templateList;
    public BindableCollection<FileInfo> TemplateList
      {
      get
        {
        return _templateList;
        }
      set
        {
        _templateList = value;
        NotifyOfPropertyChange(() => TemplateList);
        }
      }

    private FileInfo _selectedTemplate;
    public FileInfo SelectedTemplate
      {
      get { return _selectedTemplate; }
      set
        {
        _selectedTemplate = value;
        NotifyOfPropertyChange(() => SelectedTemplate);
        NotifyOfPropertyChange(() => CanSave);
        }
      }

    private string _screenshot1;
    public string Screenshot1
      {
      get
        {
        return _screenshot1;
        }
      set
        {
        _screenshot1 = value;
        NotifyOfPropertyChange(() => Screenshot1);
        }
      }

    public void Init(string scenarioFilePath, SavScenarioModel savScenario)
      {
      SavScenario = savScenario;
      ScenarioFilePath = scenarioFilePath;
      TemplateList = new BindableCollection<FileInfo>(GetTemplates(TSWOptions.TemplateFolder));
      if (TemplateList.Count < 1)
        {
        Log.Trace("No template found, cannot create scenario documentation");

        }
      }


    public void CreateDocumentFile()
      {
      var fileBase = $"{TSWOptions.ScenarioFolder}{SavScenario.RouteAbbreviation}-{SavScenario.ScenarioName}";

      var template = File.ReadAllText(SelectedTemplate.FullName);
      var output = CreateDocumentString(template);
      File.WriteAllText($"{fileBase}.html", output);
      var targetFilePath = $"{TSWOptions.ScenarioFolder}{Path.GetFileName(ScenarioFilePath)}";
      File.Copy(ScenarioFilePath, targetFilePath, true);
      FileHelpers.DeleteSingleFile($"{fileBase}.zip");
      using (var archive = ZipFile.Open($"{fileBase}.zip", ZipArchiveMode.Create))
        {
        archive.CreateEntryFromFile(targetFilePath, Path.GetFileName(ScenarioFilePath));
        archive.CreateEntryFromFile($"{fileBase}.html", $"{SavScenario.RouteAbbreviation}-{SavScenario.ScenarioName}.html");
        if (Screenshot1 != null)
          {
          archive.CreateEntryFromFile(Screenshot1, Path.GetFileName(Screenshot1));
          }
        }
      }

    public static List<FileInfo> GetTemplates(string templateDir)
      {
      var templateList = new List<FileInfo>();
      var dir = new DirectoryInfo(templateDir);
      var files = dir.GetFiles("*.html", SearchOption.TopDirectoryOnly);
      foreach (var file in files)
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
        .Replace("{Screenshot1}", GetScreenshot(Screenshot1))
        .Replace("{Filename}", GetSavFileName(SavScenario))
        .Replace("{ScenarioName}", SavScenario.ScenarioName)
        .Replace("{RouteName}", SavScenario.RouteName)
        .Replace("{PlayerEngine}", GetPlayerEngine(SavScenario))
        .Replace("{ServiceList}", GetServiceList(SavScenario));

      return output;
      }

    private static string GetScreenshot(string screenshot)
      {
      if (string.IsNullOrEmpty(screenshot))
        {
        return string.Empty;
        }
      return $"<img src=\"{Path.GetFileName(screenshot)}\">";
      }

    private static string GetServiceList(SavScenarioModel savScenario)
      {
      var output = "<table>\n";

      foreach (var service in savScenario.SavServiceList)
        {
        output += $"\t<tr>\n\t\t<td>{PrintIsPlayerService(service.IsPlayerService)}</td>\n" +
                  $"\t\t<td>{service.StartTimeText}</td>\n" +
                  $"\t\t<td>{service.EngineName}</td>" +
                  $"\t\t<td>{service.ConsistName}</td>" +
                  $"\t\t<td>{service.ServiceName}</td>" +
                  "\t<tr>\n";
        }

      output += "</table\n";
      return output;
      }

    public static string PrintIsPlayerService(bool isPlayerService)
      {
      if (isPlayerService)

        {
        return "Player";
        }
      return string.Empty;
      }

    private static string GetPlayerEngine(SavScenarioModel savScenario)
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

    public void Cancel()
      {
      TryCloseAsync();
      }

    public bool CanSave
      {
      get
        {
        return !string.IsNullOrEmpty(Author) && !string.IsNullOrEmpty(Description) && SelectedTemplate != null;
        }
      }
    public void Save()
      {
      CreateDocumentFile();
      TryCloseAsync();
      }

    private static string GetSavFileName(SavScenarioModel savScenario)
      {
      return $"USD_{savScenario.ScenarioGuid.ToString().ToUpper()}.sav";
      }
    }
  }
