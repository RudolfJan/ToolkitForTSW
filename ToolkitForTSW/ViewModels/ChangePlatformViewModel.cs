using Caliburn.Micro;
using System.Diagnostics;
using System.Windows;
using ToolkitForTSW.DataAccess;
using ToolkitForTSW.Models;

namespace ToolkitForTSW.ViewModels
  {
  public class ChangePlatformViewModel : Screen
    {
    private BindableCollection<EditionModel> _editionsList;

    public BindableCollection<EditionModel> EditionsList
      {
      get
        {
        return _editionsList;
        }
      set
        {
        _editionsList = value;
        NotifyOfPropertyChange(nameof(EditionsList));
        }
      }

    private EditionModel _selectedEdition;

    public EditionModel SelectedEdition
      {
      get
        {
        return _selectedEdition;
        }
      set
        {
        _selectedEdition = value;
        NotifyOfPropertyChange(nameof(SelectedEdition));
        }
      }

    private string _EGSTrainSimWorldStarter = "";

    public string EGSTrainSimWorldStarter
      {
      get
        {
        return _EGSTrainSimWorldStarter;
        }
      set
        {
        _EGSTrainSimWorldStarter = value;
        NotifyOfPropertyChange(nameof(EGSTrainSimWorldStarter));
        }
      }

    private PlatformEnum _currentPlatform = PlatformEnum.Steam;
    public PlatformEnum CurrentPlatform
      {
      get
        {
        return _currentPlatform;
        }
      set
        {
        _currentPlatform = value;
        NotifyOfPropertyChange(nameof(CurrentPlatform));
        }
      }

    protected override void OnViewLoaded(object view)
      {
      base.OnViewLoaded(view);
      EditionsList = new BindableCollection<EditionModel>(EditionDataAccess.GetAllEditions());
      CurrentPlatform = TSWOptions.CurrentPlatform;
      EGSTrainSimWorldStarter = TSWOptions.EGSTrainSimWorldStarter;
      }

    public void ApplyChange()
      {
      EditionDataAccess.SetSelectedEdition(SelectedEdition.Id);
      TSWOptions.CurrentPlatform = CurrentPlatform;
      TSWOptions.EGSTrainSimWorldStarter = EGSTrainSimWorldStarter;
      TSWOptions.WriteToRegistry();
      // https://stackoverflow.com/questions/61843128/wpf-application-restart
      var currentExecutablePath = Process.GetCurrentProcess().MainModule.FileName;
      Process.Start(currentExecutablePath);
      Application.Current.Shutdown();
      }
    }
  }
