using System;
using System.Collections.Generic;

namespace SavCracker.Library.Models
  {
	public class SavServiceModel
		{
    public SavServiceModel()
      {

      }

    // Make a deep copy of the SavServiceModel
    public SavServiceModel(SavServiceModel original)
      {
      ServiceName = original.ServiceName;
      ServiceGuid = original.ServiceGuid;
      EngineString = original.EngineString;
      EngineName = original.EngineName;
      EngineVehicles = new List<string>();
      foreach (var vehicle in original.EngineVehicles)
        {
        EngineVehicles.Add(vehicle);
        }
      ConsistString = original.ConsistString;
      ConsistName = original.ConsistName;
      ConsistVehicles = new List<string>();
      foreach (var vehicle in original.ConsistVehicles)
        {
        ConsistVehicles.Add(vehicle);
        }

      ServicePath = original.ServicePath;
      StartPoint= original.StartPoint;
      EndPoint = original.EndPoint;
      IsPlayerService = original.IsPlayerService;
      IsPassengerService = original.IsPassengerService;
      StartTime= original.StartTime;
      StartTimeText = original.StartTimeText;
      LiveryIdentifier = original.LiveryIdentifier;
      LiveryName = original.LiveryName;
      StopLocationList = new List<string>();
      foreach (var stopLocation in original.StopLocationList)
        {
        StopLocationList.Add(stopLocation);
        }
      }
		public string ServiceName { get; set; }
		public Guid ServiceGuid { get; set; }
    public string EngineString { get; set; }

    public string EngineName { get; set; }
    public List<string> EngineVehicles { get; set; }= new List<string>();
    public string ConsistString { get; set; }
    public string ConsistName { get; set; }
    public List<string> ConsistVehicles { get; set; } = new List<string>();
		public string ServicePath { get; set; }
		public string StartPoint { get; set; }
		public string EndPoint { get; set; }
		public bool IsPlayerService { get; set; }
		public bool IsPassengerService { get; set; }
    public ulong StartTime { get; set; }
		public string StartTimeText { get; set; }
    public string LiveryIdentifier { get; set; }
    public string LiveryName { get; set; } // Not yet used, in order to use we need to open the livery editor .sav file
    // public List<string> LiveryList { get; set; } =new List<string>(); // TODO Obsolete? 
		public List<string> StopLocationList { get; set; }=new List<string>();
		}
	}
