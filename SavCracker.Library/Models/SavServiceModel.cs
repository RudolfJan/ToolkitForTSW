using System;
using System.Collections.Generic;

namespace SavCrackerTest.Models
	{
	public class SavServiceModel
		{
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
    public List<string> LiveryList { get; set; } =new List<string>();
		public List<string> StopLocationList { get; set; }=new List<string>();
		}
	}
