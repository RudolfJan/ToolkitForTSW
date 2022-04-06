using Caliburn.Micro;
using System;
using System.Linq;
using System.Threading.Tasks;
using ToolkitForTSW.DataAccess;
using ToolkitForTSW.Models;
using ToolkitForTSW.Settings.EventModels;

namespace ToolkitForTSW.Settings.ViewModels
  {
  public class EngineIniViewModel : Screen
    {
    private int _workSetId;
    private readonly IEventAggregator _events;
    public bool HasChanged { get; set; } = false;

    private BindableCollection<EngineIniSettingsModel> _EngineIniSettingsList;
    public BindableCollection<EngineIniSettingsModel> EngineIniSettingsList
      {
      get { return _EngineIniSettingsList; }
      set
        {
        _EngineIniSettingsList = value;
        NotifyOfPropertyChange(nameof(EngineIniSettingsList));
        }
      }

    private BindableCollection<EngineIniWorkSetModel> _EngineIniWorkSetList;
    public BindableCollection<EngineIniWorkSetModel> EngineIniWorkSetList
      {
      get { return _EngineIniWorkSetList; }
      set
        {
        _EngineIniWorkSetList = value;
        NotifyOfPropertyChange(nameof(EngineIniWorkSetList));
        NotifyOfPropertyChange(nameof(CanEditWorkSet));
        NotifyOfPropertyChange(nameof(CanDeleteWorkSet));
        NotifyOfPropertyChange(nameof(CanAddSetting));
        NotifyOfPropertyChange(nameof(CanRemoveSetting));
        }
      }

    private BindableCollection<EngineIniSettingsModel> _FilteredSettingsList;
    public BindableCollection<EngineIniSettingsModel> FilteredSettingsList
      {
      get { return _FilteredSettingsList; }
      set
        {
        _FilteredSettingsList = value;
        NotifyOfPropertyChange(nameof(FilteredSettingsList));
        }
      }

    private EngineIniSettingsModel _SelectedEngineIniSettings;
    public EngineIniSettingsModel SelectedEngineIniSettings
      {
      get { return _SelectedEngineIniSettings; }
      set
        {
        _SelectedEngineIniSettings = value;
        NotifyOfPropertyChange(nameof(SelectedEngineIniSettings));
        NotifyOfPropertyChange(nameof(CanEditWorkSet));
        NotifyOfPropertyChange(nameof(CanDeleteWorkSet));
        NotifyOfPropertyChange(nameof(CanAddSetting));
        NotifyOfPropertyChange(nameof(CanRemoveSetting));
        }
      }

    private EngineIniWorkSetModel _SelectedEngineIniWorkSet;
    public EngineIniWorkSetModel SelectedEngineIniWorkSet
      {
      get { return _SelectedEngineIniWorkSet; }
      set
        {
        _SelectedEngineIniWorkSet = value;
        if (SelectedEngineIniWorkSet == null)
          {
          SettingsInWorkSetList = null;
          }
        else
          {
          SettingsInWorkSetList = new BindableCollection<EngineIniSettingsModel>(EngineIniSettingDataAccess.GetEngineIniSettingsInWorkSet(SelectedEngineIniWorkSet.Id));
          }
        NotifyOfPropertyChange(nameof(SelectedEngineIniWorkSet));
        NotifyOfPropertyChange(nameof(CanEditWorkSet));
        NotifyOfPropertyChange(nameof(CanDeleteWorkSet));
        NotifyOfPropertyChange(nameof(CanAddSetting));
        NotifyOfPropertyChange(nameof(CanRemoveSetting));
        }
      }

    private string _WorkSetName;
    public string WorkSetName
      {
      get { return _WorkSetName; }
      set
        {
        _WorkSetName = value;
        NotifyOfPropertyChange(nameof(WorkSetName));
        NotifyOfPropertyChange(nameof(CanSaveWorkSet));
        }
      }

    private string _WorkSetDescription;
    public string WorkSetDescription
      {
      get { return _WorkSetDescription; }
      set
        {
        _WorkSetDescription = value;
        NotifyOfPropertyChange(nameof(WorkSetDescription));
        NotifyOfPropertyChange(nameof(CanSaveWorkSet));
        }
      }

    private string _Filter = string.Empty;
    public string Filter
      {
      get { return _Filter; }
      set
        {
        _Filter = value;
        FilterChanged();
        // NotifyOfPropertyChange(nameof(Filter));
        }
      }


    private BindableCollection<EngineIniSettingsModel> _SettingsInWorkSetList;

    public BindableCollection<EngineIniSettingsModel> SettingsInWorkSetList
      {
      get { return _SettingsInWorkSetList; }
      set
        {
        _SettingsInWorkSetList = value;
        NotifyOfPropertyChange(nameof(SettingsInWorkSetList));
        NotifyOfPropertyChange(nameof(CanRemoveSetting));
        }
      }


    private EngineIniSettingsModel _SelectedSettingInWorkSet;
    public EngineIniSettingsModel SelectedSettingInWorkSet
      {
      get { return _SelectedSettingInWorkSet; }
      set
        {
        _SelectedSettingInWorkSet = value;

        NotifyOfPropertyChange(nameof(SelectedSettingInWorkSet));
        }
      }

    public EngineIniViewModel(IEventAggregator events)
      {
      _events = events;
      }

    protected override void OnViewLoaded(object view)
      {
      base.OnViewLoaded(view);
      Init();
      }

    public void Init()
      {
      EngineIniSettingsList = new BindableCollection<EngineIniSettingsModel>(EngineIniSettingDataAccess.GetAllEngineIniSettings().OrderBy(x => x.SettingName));
      EngineIniWorkSetList = new BindableCollection<EngineIniWorkSetModel>(EngineIniWorkSetDataAccess.GetAllEngineIniWorkSets().OrderBy(x => WorkSetName));
      FilterChanged();
      }

    public bool CanDeleteWorkSet
      {
      get
        {
        return SelectedEngineIniWorkSet != null;
        }
      }
    public void DeleteWorkSet()
      {
      throw new NotImplementedException();
      }

    public bool CanEditWorkSet
      {
      get
        {
        return SelectedEngineIniWorkSet != null;
        }
      }

    public void EditWorkSet()
      {
      _workSetId = SelectedEngineIniWorkSet.Id;
      WorkSetName = SelectedEngineIniWorkSet.WorkSetName;
      WorkSetDescription = SelectedEngineIniWorkSet.WorkSetDescription;
      }

    public bool CanSaveWorkSet
      {
      get
        {
        return !string.IsNullOrEmpty(WorkSetName);
        }
      }

    public void SaveWorkSet()
      {
      if (_workSetId <= 0)
        {
        var newWorkSet = new EngineIniWorkSetModel
          {
          WorkSetName = WorkSetName,
          WorkSetDescription = WorkSetDescription
          };
        EngineIniWorkSetDataAccess.InsertEngineIniWorkSet(newWorkSet);
        EngineIniWorkSetList.Add(newWorkSet);
        }
      else
        {
        SelectedEngineIniWorkSet.WorkSetName = WorkSetName;
        SelectedEngineIniWorkSet.WorkSetDescription = WorkSetDescription;
        EngineIniWorkSetDataAccess.UpdateEngineIniWorkSet(SelectedEngineIniWorkSet);
        }
      EngineIniWorkSetList = new BindableCollection<EngineIniWorkSetModel>(EngineIniWorkSetDataAccess.GetAllEngineIniWorkSets());
      ClearWorkSet();
      HasChanged = true;
      }

    public bool CanRemoveSetting
      {
      get
        {
        return SelectedEngineIniWorkSet != null && SelectedSettingInWorkSet != null;
        }
      }

    public void RemoveSetting()
      {
      EngineIniWorkSetConnectorDataAccess.DeleteEngineIniWorkSetConnectorByParticipants(SelectedEngineIniWorkSet.Id, SelectedSettingInWorkSet.Id);
      SettingsInWorkSetList = new BindableCollection<EngineIniSettingsModel>(EngineIniSettingDataAccess.GetEngineIniSettingsInWorkSet(SelectedEngineIniWorkSet.Id));
      }

    public bool CanAddSetting
      {
      get
        {
        return SelectedEngineIniWorkSet != null && SelectedEngineIniSettings != null;
        }
      }
    public void AddSetting()
      {
      var connector = new EngineIniWorkSetConnectorModel
        {
        EngineIniSettingId = SelectedEngineIniSettings.Id,
        EngineIniWorkSetId = SelectedEngineIniWorkSet.Id
        };
      EngineIniWorkSetConnectorDataAccess.InsertEngineIniWorkSetConnector(connector);
      SettingsInWorkSetList = new BindableCollection<EngineIniSettingsModel>(EngineIniSettingDataAccess.GetEngineIniSettingsInWorkSet(SelectedEngineIniWorkSet.Id));
      }

    public void ClearWorkSet()
      {
      _workSetId = 0;
      WorkSetName = string.Empty;
      WorkSetDescription = string.Empty;
      SelectedEngineIniWorkSet = null;
      }

    public void FilterChanged()
      {
      if (Filter.Length > 0)
        {
        FilteredSettingsList = new BindableCollection<EngineIniSettingsModel>(EngineIniSettingsList
                      .Where(x => x.SettingName.ToLower().Contains(Filter))
                      .ToList());
        }
      else
        {
        FilteredSettingsList = EngineIniSettingsList;
        }
      }

    public Task CloseForm()
      {
      var engineIniClosedEvent = new EngineIniClosedEvent
        {
        HasChanged = HasChanged // TODO add additional logic for this. It tells Settingsexperimental to update the UI.
        };
      _events.PublishOnUIThreadAsync(engineIniClosedEvent);
      TryCloseAsync();
      return Task.CompletedTask;
      }
    }
  }
