using Caliburn.Micro;
using Logging.Library;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToolkitForTSW.DataAccess;
using ToolkitForTSW.Models;
using ToolkitForTSW.Settings.EventModels;
using Utilities.Library.TextHelpers;

namespace ToolkitForTSW.Settings.ViewModels
  {

  public class SettingsExperimentalViewModel : Conductor<object>, IHandle<EngineIniClosedEvent>
    {
    private readonly IWindowManager _windowManager;
    private readonly IEventAggregator _events;
    private readonly ISettingsManagerLogic _settingsManagerLogic;

    private List<EngineIniWorkSetModel> _EngineIniWorkSetList;
    public List<EngineIniWorkSetModel> EngineIniWorkSetList
      {
      get { return _EngineIniWorkSetList; }
      set
        {
        _EngineIniWorkSetList = value;
        NotifyOfPropertyChange(nameof(EngineIniWorkSetList));
        }
      }

    private BindableCollection<EngineIniValueModel> _ValueList = new BindableCollection<EngineIniValueModel>();
    public BindableCollection<EngineIniValueModel> ValueList
      {
      get { return _ValueList; }
      set
        {
        _ValueList = value;
        NotifyOfPropertyChange(nameof(ValueList));
        }
      }
    private EngineIniValueModel _SelectedValue;
    public EngineIniValueModel SelectedValue
      {
      get { return _SelectedValue; }
      set
        {
        _SelectedValue = value;
        NotifyOfPropertyChange(nameof(SelectedValue));
        }
      }
    private EngineIniWorkSetModel _SelectedWorkSet;
    public EngineIniWorkSetModel SelectedWorkSet
      {
      get { return _SelectedWorkSet; }
      set
        {
        _SelectedWorkSet = value;
        NotifyOfPropertyChange(nameof(SelectedWorkSet));
        }
      }

    public string SaveSetName { get; set; }
    public SettingsExperimentalViewModel(IWindowManager windowManager, IEventAggregator events, ISettingsManagerLogic settingsManagerLogic)
      {
      DisplayName = "Experimental";
      _windowManager = windowManager;
      _events = events;
      _events.SubscribeOnUIThread(this);
      _settingsManagerLogic = settingsManagerLogic;
      }

    public void Init()
      {
      EngineIniWorkSetList = EngineIniWorkSetDataAccess.GetAllEngineIniWorkSets();
      LoadValueSetFromString(_settingsManagerLogic.ExperimentalSettingsString, false);
      }

    public void Update()
      {
      _settingsManagerLogic.ExperimentalSettingsString = SaveValueSetToString();
      }

    public void AddWorkSet()
      {
      var settingsTempList = EngineIniSettingDataAccess.GetEngineIniSettingsInWorkSet(SelectedWorkSet.Id);
      foreach (var setting in settingsTempList)
        {
        var value = new EngineIniValueModel
          {
          WorkSetId = SelectedWorkSet.Id,
          WorkSetName = SelectedWorkSet.WorkSetName,
          SettingId = setting.Id,
          SettingName = setting.SettingName
          };
        if (!ValueList.Where(x => x.SettingId == setting.Id).Any())
          {
          var description = EngineIniSettingDataAccess.GetEngineIniSettingDescriptionById(value.SettingId);
          if (!string.IsNullOrEmpty(description))
            {
            value.SettingDescription = description;
            }
          ValueList.Add(value);
          }
        }
      }

    public void EditWorkSets()
      {
      var engineinivm = IoC.Get<EngineIniViewModel>();
      _windowManager.ShowWindowAsync(engineinivm);
      }

    public void RemoveWorkSet()
      {
      int id = SelectedWorkSet.Id;
      var temp = ValueList.Where(x => x.WorkSetId != id).ToList();
      ValueList = new BindableCollection<EngineIniValueModel>(temp);
      }

    public string SaveValueSetToString()
      {
      var output = string.Empty;
      if (ValueList.Count == 0)
        {
        return string.Empty;
        }
      foreach (var value in ValueList)
        {
        output += $";{value.WorkSetId},{TextHelper.DoubleQuotes(value.WorkSetName)},{value.SettingId},{TextHelper.DoubleQuotes(value.SettingName)},{TextHelper.DoubleQuotes(value.SettingValue)}\r\n";
        output += $"{value.SettingName}={value.SettingValue}\r\n";
        }
      return output;
      }

    public void LoadValueSetFromSaveSet(string saveDirectory)
      {
      var savePath = $"{saveDirectory}ExperimentalSettings.csv";

      if (!File.Exists(savePath))
        {
        return;
        }
      StringReader reader = new StringReader(File.ReadAllText(savePath));
      LoadValueSet(reader, true);
      }

    public void LoadValueSetFromString(string settings, bool hasHeader)
      {
      StringReader reader = new StringReader(settings);
      LoadValueSet(reader, hasHeader);
      }


    // https://stackoverflow.com/questions/5282999/reading-csv-file-and-storing-values-into-an-array
    private void LoadValueSet(StringReader reader, bool hasHeader)
      {
      try
        {
        using (TextFieldParser csvParser = new TextFieldParser(reader))
          {
          csvParser.CommentTokens = new string[] { "#" };
          csvParser.SetDelimiters(new string[] { "," });
          csvParser.HasFieldsEnclosedInQuotes = true;

          // Skip the row with the column names
          if (hasHeader)
            {
            csvParser.ReadLine();
            }
          ValueList.Clear();
          while (!csvParser.EndOfData)
            {
            // Read current line fields, pointer moves to the next line.
            string[] fields = csvParser.ReadFields();
            int length = fields.GetLength(0);

            if (length == 5)
              {
              var value = new EngineIniValueModel
                {
                WorkSetId = int.Parse(fields[0]),
                WorkSetName = fields[1],
                SettingId = int.Parse(fields[2]),
                SettingName = fields[3],
                SettingValue = fields[4]
                };
              // Do some sanity checks
              var description = EngineIniSettingDataAccess.GetEngineIniSettingDescriptionById(value.SettingId);
              if (description != null)
                {
                // check if the value is still part of a workset
                var check = EngineIniWorkSetConnectorDataAccess.CheckEngineIniSettingsInWorkSet(value.SettingId, value.WorkSetId);
                if (check > 0)
                  {
                  value.SettingDescription = description;
                  ValueList.Add(value);
                  }
                }
              }
            }
          }
        }
      catch (Exception ex)
        {
        Log.Trace($"Error reading Experimental Settings import {ex.Message}");
        }
      }

    Task IHandle<EngineIniClosedEvent>.HandleAsync(EngineIniClosedEvent message, CancellationToken cancellationToken)
      {
      var hasChanged = message.HasChanged;
      if (hasChanged)
        {
        Init();
        }
      return Task.CompletedTask;
      }
    }
  }
