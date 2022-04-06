using Caliburn.Micro;
using SavCracker.Library;
using SavCracker.Library.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ToolkitForTSW.DataAccess;
using Utilities.Library;

namespace ToolkitForTSW.ViewModels
  {
  public class ScenarioManagerViewModel : Screen
    {
    private readonly IEventAggregator _events;
    private readonly IWindowManager _windowManager;

    private BindableCollection<CScenario> _scenarioList = new BindableCollection<CScenario>();

    public BindableCollection<CScenario> ScenarioList
      {
      get { return _scenarioList; }
      set
        {
        _scenarioList = value;
        NotifyOfPropertyChange(() => ScenarioList);
        }
      }

    private CScenario _selectedSavScenario;
    public CScenario SelectedSavScenario
      {
      get
        {
        return _selectedSavScenario;
        }
      set
        {
        _selectedSavScenario = value;
        if (SelectedSavScenario != null)
          {
          ScenarioIssueList = new BindableCollection<string>(
          ScenarioProblemTracker.FindScenarioProblems(SelectedSavScenario
            .SavScenario));
          SavServiceList = new BindableCollection<SavServiceModel>(_selectedSavScenario.SavScenario.SavServiceList);
          NotifyOfPropertyChange(() => ScenarioIssueList);
          }
        NotifyOfPropertyChange(() => CanPublishScenario);
        NotifyOfPropertyChange(() => CanEditScenario);
        NotifyOfPropertyChange(() => CanOpenHex);
        }
      }


    private BindableCollection<SavServiceModel> _savServiceList;

    public BindableCollection<SavServiceModel> SavServiceList
      {
      get
        {
        return _savServiceList;
        }
      set
        {
        _savServiceList = value;
        NotifyOfPropertyChange(nameof(SavServiceList));
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
        NotifyOfPropertyChange(() => SelectedSavService);
        NotifyOfPropertyChange(() => CanDeleteScenario);
        NotifyOfPropertyChange(() => CanEditScenario);
        NotifyOfPropertyChange(() => CanOpenHex);
        }
      }

    private BindableCollection<string> _scenarioIssueList = new BindableCollection<string>();


    public BindableCollection<string> ScenarioIssueList
      {
      get
        {
        return _scenarioIssueList;
        }
      set
        {
        _scenarioIssueList = value;
        NotifyOfPropertyChange(() => ScenarioIssueList);
        }
      }

    public ScenarioManagerViewModel(IEventAggregator events, IWindowManager windowManager)
      {
      _events = events;
      _windowManager = windowManager;

      SavCracker.Library.SavCracker.RouteList = RouteDataAccess.GetSavCrackerRouteList();
      BuildScenarioList();
      }

    public bool CanDeleteScenario
      {
      get
        {
        return SelectedSavScenario != null;
        }
      }

    public void ScenarioDelete()
      {
      var toBeDeleted = SelectedSavScenario;
      FileHelpers.DeleteSingleFile(toBeDeleted.ScenarioFile.FullName);
      ScenarioDataAccess.DeleteScenario(toBeDeleted.SavScenario.ScenarioGuid); // remove from database if it is there 
      BuildScenarioList();
      SelectedSavScenario = null; // TODO refresh works but this feels clumsy ...
      NotifyOfPropertyChange(() => SelectedSavScenario);
      NotifyOfPropertyChange(() => ScenarioList);
      }

    public bool CanEditScenario
      {
      get
        {
        return SelectedSavScenario != null;
        }
      }

    public async Task EditScenario()
      {
      var scenarioEditorVM = IoC.Get<ScenarioEditViewModel>();
      scenarioEditorVM.Scenario = SelectedSavScenario;
      await _windowManager.ShowDialogAsync(scenarioEditorVM);
      BuildScenarioList();
      SelectedSavScenario = null; // TODO refresh works but this feels clumsy ...
      }

    public bool CanOpenHex
      {
      get
        {
        return SelectedSavScenario != null;
        }
      }

    public void OpenHex()
      {
      ProcessHelper.OpenGenericFile(SelectedSavScenario.ScenarioFile.FullName);
      }

    public bool CanPublishScenario
      {
      get
        {
        return SelectedSavScenario != null;
        }
      }

    public void PublishScenario()
      {
      var viewmodel = IoC.Get<PublishScenarioViewModel>();
      viewmodel.Init(SelectedSavScenario.ScenarioFile.FullName, SelectedSavScenario.SavScenario);
      _windowManager.ShowDialogAsync(viewmodel);
      }

    public void BuildScenarioList()
      {
      var Path = $"{TSWOptions.GetSaveLocationPath()}Saved\\SaveGames\\";
      DirectoryInfo DirInfo = new DirectoryInfo(Path);
      ScenarioList.Clear();
      var files = DirInfo.GetFiles("USD_*.sav", SearchOption.TopDirectoryOnly);
      foreach (var file in files)
        {
        CScenario Scenario = new CScenario
          {
          ScenarioFile = file,
          Cracker = new SavCracker.Library.SavCracker(file.FullName)
          };
        Scenario.Cracker.ParseScenario();
        Scenario.SavScenario = Scenario.Cracker.Scenario;
        SavScenarioLogic.BuildSavScenario(Scenario.SavScenario, Scenario.Cracker);
        Scenario.IsToolkitCreated = ScenarioDataAccess.GetScenarioByGuid(Scenario.SavScenario.ScenarioGuid) != null;
        ScenarioList.Add(Scenario);
        }
      ScenarioList = new BindableCollection<CScenario>(ScenarioList.OrderBy(x => x.SavScenario.RouteAbbreviation).ToList());
      }


    public Task CloseForm()
      {
      return TryCloseAsync();
      }
    }
  }

