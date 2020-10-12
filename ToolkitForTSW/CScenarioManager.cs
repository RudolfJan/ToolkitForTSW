using SavCracker.Library;
using SavCrackerTest;
using SavCrackerTest.Models;
using Styles.Library.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ToolkitForTSW;

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
      BuildScenarioList();
      }

    private void BuildScenarioList()
      {
      var Path=  $"{CTSWOptions.GameSaveLocation}Saved\\SaveGames\\";
      DirectoryInfo DirInfo = new DirectoryInfo(Path);
      var files = DirInfo.GetFiles("USD_*.sav", SearchOption.TopDirectoryOnly);
      foreach (var file in files)
        {
        var Scenario= new CScenario();
        Scenario.ScenarioFile = file;
        Scenario.Cracker= new SavCrackerTest.SavCracker(file.FullName);
        Scenario.Cracker.ParseScenario();
        Scenario.SavScenario = Scenario.Cracker.Scenario;
        SavScenarioLogic.BuildSavScenario(Scenario.SavScenario,Scenario.Cracker);
        ScenarioList.Add(Scenario);
        }
      ScenarioList = ScenarioList.OrderBy(x => x.SavScenario.RouteAbbreviation).ToList();
      }
     }
    }

