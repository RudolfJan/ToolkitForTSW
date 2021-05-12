using System;
using System.Collections.Generic;
using System.Text;

namespace SavCracker.Library.Models
	{
	public class SavScenarioModel
		{
		public List<SavElementModel> ElementList { get; set; } = new List<SavElementModel>();
		public List<SavServiceModel> SavServiceList { get; set; }= new List<SavServiceModel>();
		public string RouteString { get; set; }
    public string RouteName { get; set; }
    public string RouteAbbreviation { get; set; }
		public string ScenarioName { get; set; }
		public Guid ScenarioGuid { get; set; }
		public bool RulesDisabledMode { get; set; }
		public bool GlobalElectrificationMode { get; set; }
    public string TargetAsset { get; set; }
    }
	}
