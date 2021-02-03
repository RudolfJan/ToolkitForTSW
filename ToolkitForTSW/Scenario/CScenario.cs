using SavCrackerTest.Models;
using Styles.Library.Helpers;
using System.IO;

namespace ToolkitForTSW
  {
  public class CScenario : Notifier
    {
    public FileInfo ScenarioFile { get; set; }
    public SavCrackerTest.SavCracker Cracker { get; set; }
    private SavScenarioModel _savScenario;

    public SavScenarioModel SavScenario
      {
      get
        {
        return _savScenario;
        }
      set
        {
        _savScenario = value;
        OnPropertyChanged("SavScenario");
        }
      }

    private bool _IsToolkitCreated;
    public bool IsToolkitCreated
      {
      get { return _IsToolkitCreated; }
      set
        {
        _IsToolkitCreated = value;
        OnPropertyChanged("IsToolkitCreated");
        }
      }

    }
  }

