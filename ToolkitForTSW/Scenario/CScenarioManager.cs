using SavCracker.Library;
using SavCracker.Library.Models;
using Styles.Library.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ToolkitForTSW.DataAccess;

namespace ToolkitForTSW
  {
  public class CScenarioManager: Notifier
    {
    private List<CScenario> _scenarioList= new List<CScenario>();
   
    public List<CScenario> ScenarioList
      {
      get { return _scenarioList; }
      set
        {
        _scenarioList = value;
        OnPropertyChanged("ScenarioList");
        }
      }

    private CScenario _selectedSavScenario;
    public CScenario SelectedSavScenario {
      get
        {
        return _selectedSavScenario;
        }
      set
        {
        _selectedSavScenario = value;
        OnPropertyChanged("SelectedSavScenario");
        }
       }

    private SavServiceModel _selectedSavService;
    public SavServiceModel SelectedSavService
      {
      get
        {
        return _selectedSavService;
        }
      set
        {
        _selectedSavService = value;
        OnPropertyChanged("SelectedSavService");
        }
      }

    private List<string> _scenarioIssueList = new List<string>();


    public List<string> ScenarioIssueList
      {
      get
        {
        return _scenarioIssueList;
        }
      set
        {
        _scenarioIssueList = value;
        OnPropertyChanged("ScenarioIssueList");
        }
      }

    public CScenarioManager()
      {
      SavCracker.Library.SavCracker.RouteList = RouteDataAccess.GetSavCrackerRouteList();
      BuildScenarioList();
      }

    public void ScenarioDelete(CScenario toBeDeleted)
      {
      CApps.DeleteSingleFile(toBeDeleted.ScenarioFile.FullName);
      ScenarioDataAccess.DeleteScenario(toBeDeleted.SavScenario.ScenarioGuid); // remove from database if it is there 
      BuildScenarioList();
      SelectedSavScenario = null; // TODO refresh works but this feels clumsy ...
      }

    public void BuildScenarioList()
      {
      var Path=  $"{CTSWOptions.GameSaveLocation}Saved\\SaveGames\\";
      DirectoryInfo DirInfo = new DirectoryInfo(Path);
      ScenarioList.Clear();
      var files = DirInfo.GetFiles("USD_*.sav", SearchOption.TopDirectoryOnly);
      foreach (var file in files)
        {
        var Scenario = new CScenario
          {
          ScenarioFile = file,
          Cracker = new SavCracker.Library.SavCracker(file.FullName)
          };
        Scenario.Cracker.ParseScenario();
        Scenario.SavScenario = Scenario.Cracker.Scenario;
        SavScenarioLogic.BuildSavScenario(Scenario.SavScenario,Scenario.Cracker);
        Scenario.IsToolkitCreated= ScenarioDataAccess.GetScenarioByGuid(Scenario.SavScenario.ScenarioGuid)!=null;
        ScenarioList.Add(Scenario);
        }
      ScenarioList = ScenarioList.OrderBy(x => x.SavScenario.RouteAbbreviation).ToList();
      }
     }
    }

