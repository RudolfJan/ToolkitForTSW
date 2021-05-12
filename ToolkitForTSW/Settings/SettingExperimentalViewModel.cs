using Logging.Library;
using Microsoft.VisualBasic.FileIO;
using Styles.Library.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ToolkitForTSW.DataAccess;
using ToolkitForTSW.Models;
using Utilities.Library.TextHelpers;

namespace ToolkitForTSW.Settings
  {
  public class SettingExperimentalViewModel: Notifier
    {
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

		private List<EngineIniValueModel> _ValueList = new List<EngineIniValueModel>();
		public List<EngineIniValueModel> ValueList
			{
			get { return _ValueList; }
			set
				{
				_ValueList = value;
				OnPropertyChanged("ValueList");
				}
			}
		private EngineIniValueModel _SelectedValue;
		public EngineIniValueModel SelectedValue
			{
			get { return _SelectedValue; }
			set
				{
				_SelectedValue = value;
				OnPropertyChanged("SelectedValue");
				}
			}
		private EngineIniWorkSetModel _SelectedWorkSet;
		public EngineIniWorkSetModel SelectedWorkSet
			{
			get { return _SelectedWorkSet; }
			set
				{
				_SelectedWorkSet = value;
				OnPropertyChanged("SelectedWorkSet");
				}
			}

		public SettingExperimentalViewModel()
      {
      }

		public void Init()
      {
			EngineIniWorkSetList= EngineIniWorkSetDataAccess.GetAllEngineIniWorkSets();
      }

    internal void AddWorkSet()
      {
      var settingsTempList= EngineIniSettingDataAccess.GetEngineIniSettingsInWorkSet(SelectedWorkSet.Id);
			foreach(var setting in settingsTempList)
        {
        var value = new EngineIniValueModel
          {
          WorkSetId = SelectedWorkSet.Id,
          WorkSetName = SelectedWorkSet.WorkSetName,
          SettingId = setting.Id,
          SettingName = setting.SettingName
          };
        if ( ValueList.Where(x => x.SettingId == setting.Id).Count() ==0)
          {
					ValueList.Add(value);
          }
				}
      }

    internal void RemoveWorkSet()
      {
			int id= SelectedWorkSet.Id;
			ValueList = ValueList.Where(x=> x.WorkSetId != id).ToList();
      }

		public void SaveValueSetToSaveSet(string saveDirectory)
      {
			if(ValueList.Count==0)
        {
				return;
        }
			string output="WorkSetId,WorkSetName,SettingId,SettingName,SettingValue\r\n";
			foreach(var value in ValueList)
        {
				output += $"{value.WorkSetId},{TextHelper.DoubleQuotes(value.WorkSetName)},{value.SettingId},{TextHelper.DoubleQuotes(value.SettingName)},{TextHelper.DoubleQuotes(value.SettingValue)}\r\n";
        }
			var savePath=$"{saveDirectory}ExperimentalSettings.csv";
			File.WriteAllText(savePath,output);
      }

		public string SaveValueSetToGame()
			{
			var output=string.Empty;
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
			LoadValueSet(reader,true);
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
					if(hasHeader)
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
              ValueList.Add(value);
							}
						}
					}
				}
			catch (Exception ex)
				{
				Log.Trace($"Error reading Experimental Settings import {ex.Message}");
				}
			}
    }
	}
