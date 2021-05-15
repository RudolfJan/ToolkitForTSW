using Styles.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToolkitForTSW.DataAccess;
using ToolkitForTSW.Models;

namespace ToolkitForTSW.Settings
  {
  public class EngineIniViewModel: Notifier
    {
		private int _workSetId;

		private List<EngineIniSettingsModel> _EngineIniSettingsList;
		public List<EngineIniSettingsModel> EngineIniSettingsList
			{
			get { return _EngineIniSettingsList; }
			set
				{
				_EngineIniSettingsList = value;
				OnPropertyChanged("EngineIniSettingsList");
				}
			}

		private List<EngineIniWorkSetModel> _EngineIniWorkSetList;
		public List<EngineIniWorkSetModel> EngineIniWorkSetList
			{
			get { return _EngineIniWorkSetList; }
			set
				{
				_EngineIniWorkSetList = value;
				OnPropertyChanged("EngineIniWorkSetList");
				}
			}

		private List<EngineIniSettingsModel> _FilteredSettingsList;
		public List<EngineIniSettingsModel> FilteredSettingsList
			{
			get { return _FilteredSettingsList; }
			set
				{
				_FilteredSettingsList = value;
				OnPropertyChanged("FilteredSettingsList");
				}
			}

		private EngineIniSettingsModel _SelectedEngineIniSettings;
		public EngineIniSettingsModel SelectedEngineIniSettings
			{
			get { return _SelectedEngineIniSettings; }
			set
				{
				_SelectedEngineIniSettings = value;
				OnPropertyChanged("SelectedEngineIniSettings");
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
					SettingsInWorkSetList = EngineIniSettingDataAccess.GetEngineIniSettingsInWorkSet(SelectedEngineIniWorkSet.Id);
					}
				OnPropertyChanged("SelectedEngineIniWorkSet");
				}
			}

		private string _WorkSetName;
		public string WorkSetName
			{
			get { return _WorkSetName; }
			set
				{
				_WorkSetName = value;
				OnPropertyChanged("WorkSetName");
				}
			}

		private string _WorkSetDescription;
		public string WorkSetDescription
			{
			get { return _WorkSetDescription; }
			set
				{
				_WorkSetDescription = value;
				OnPropertyChanged("WorkSetDescription");
				}
			}

		private string _Filter=string.Empty;
		public string Filter
			{
			get { return _Filter; }
			set
				{
				_Filter = value;
				OnPropertyChanged("Filter");
				}
			}


		private List<EngineIniSettingsModel> _SettingsInWorkSetList;

    public List<EngineIniSettingsModel> SettingsInWorkSetList
			{
			get { return _SettingsInWorkSetList; }
			set
				{
				_SettingsInWorkSetList = value;

				OnPropertyChanged("SettingsInWorkSetList");
				}
			}

	
    private EngineIniSettingsModel _SelectedSettingInWorkSet;
		public EngineIniSettingsModel SelectedSettingInWorkSet
			{
			get { return _SelectedSettingInWorkSet; }
			set
				{
				_SelectedSettingInWorkSet = value;

					OnPropertyChanged("SelectedSettingInWorkSet");
				}
			}

		public EngineIniViewModel()
      {

      }


		public void Init()
      {
			EngineIniSettingsList = EngineIniSettingDataAccess.GetAllEngineIniSettings();
			EngineIniWorkSetList = EngineIniWorkSetDataAccess.GetAllEngineIniWorkSets();
			FilterChanged();
			}

		internal void DeleteWorkSet()
			{
			throw new NotImplementedException();
			}

		internal void EditWorkSet()
			{
			_workSetId = SelectedEngineIniWorkSet.Id;
			WorkSetName = SelectedEngineIniWorkSet.WorkSetName;
			WorkSetDescription = SelectedEngineIniWorkSet.WorkSetDescription;

			}

		internal void SaveWorkSet()
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
			EngineIniWorkSetList = EngineIniWorkSetDataAccess.GetAllEngineIniWorkSets();
			ClearWorkSet();
			}
		internal void RemoveSetting()
			{
			EngineIniWorkSetConnectorDataAccess.DeleteEngineIniWorkSetConnectorByParticipants(SelectedEngineIniWorkSet.Id, SelectedSettingInWorkSet.Id);
			SettingsInWorkSetList = EngineIniSettingDataAccess.GetEngineIniSettingsInWorkSet(SelectedEngineIniWorkSet.Id);
			}

		internal void AddSetting()
			{
      var connector = new EngineIniWorkSetConnectorModel
        {
        EngineIniSettingId = SelectedEngineIniSettings.Id,
        EngineIniWorkSetId = SelectedEngineIniWorkSet.Id
        };
      EngineIniWorkSetConnectorDataAccess.InsertEngineIniWorkSetConnector(connector);
			SettingsInWorkSetList = EngineIniSettingDataAccess.GetEngineIniSettingsInWorkSet(SelectedEngineIniWorkSet.Id);
			}

		internal void ClearWorkSet()
			{
			_workSetId = 0;
			WorkSetName = string.Empty;
			WorkSetDescription = string.Empty;
			SelectedEngineIniWorkSet = null;
			}


		public void FilterChanged()
      {
			if(Filter.Length>0)
        {
				FilteredSettingsList= EngineIniSettingsList.Where(x=>x.SettingName.ToLower().Contains(Filter)).ToList();
				}
			else
        {
				FilteredSettingsList= EngineIniSettingsList;
        }
      }
		}
	}
