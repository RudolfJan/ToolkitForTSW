using SavCracker.Library;
using SavCrackerTest.Models;
using Styles.Library.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using Utilities.Library.TextHelpers;

namespace ToolkitForTSW
  {

  public class CScenarioEdit : Notifier
    {
    private CScenario _scenario;
    public CScenario Scenario
      {
      get
        {
        return _scenario;
        }
      set
        {
        _scenario = value;
        OnPropertyChanged("Scenario");
        }
      }

    private string _ClonedScenarioName;
    public string ClonedScenarioName
      {
      get { return _ClonedScenarioName; }
      set
        {
        _ClonedScenarioName = value;
        OnPropertyChanged("ClonedScenarioName");
        }
      }

    private Guid _ClonedScenarioGuid = Guid.NewGuid();
    public Guid ClonedScenarioGuid
      {
      get { return _ClonedScenarioGuid; }
      set
        {
        _ClonedScenarioGuid = value;
        OnPropertyChanged("ClonedScenarioGuid");
        }
      }

    private string _ScenarioStartTime;
    public string ScenarioStartTime
      {
      get { return _ScenarioStartTime; }
      set
        {
        _ScenarioStartTime = value;
        OnPropertyChanged("ScenarioStartTime");
        }
      }

    private string _ServiceName;
    public string ServiceName
      {
      get { return _ServiceName; }
      set
        {
        _ServiceName = value;
        OnPropertyChanged("ServiceName");
        }
      }

    private string _ServiceStartTime;
    public string ServiceStartTime
      {
      get { return _ServiceStartTime; }
      set
        {
        _ServiceStartTime = value;
        OnPropertyChanged("ServiceStartTime");
        }
      }

    private string _StartLocation;
    public string StartLocation
      {
      get { return _StartLocation; }
      set
        {
        _StartLocation = value;
        OnPropertyChanged("StartLocation");
        }
      }

    private string _EndLocation;
    public string EndLocation
      {
      get { return _EndLocation; }
      set
        {
        _EndLocation = value;
        OnPropertyChanged("EndLocation");
        }
      }

    private string _EngineString;
    public string EngineString
      {
      get { return _EngineString; }
      set
        {
        _EngineString = value;
        OnPropertyChanged("EngineString");
        }
      }

    private string _ConsistString;
    public string ConsistString
      {
      get { return _ConsistString; }
      set
        {
        _ConsistString = value;
        OnPropertyChanged("ConsistString");
        }
      }

    private string _LiveryIdentifier;
    public string LiveryIdentifier
      {
      get { return _LiveryIdentifier; }
      set
        {
        _LiveryIdentifier = value;
        OnPropertyChanged("LiveryIdentifier");
        }
      }

    private bool _OffTheRailsMode;
    public bool OffTheRailsMode
      {
      get { return _OffTheRailsMode; }
      set
        {
        _OffTheRailsMode = value;
        OnPropertyChanged("OffTheRailsMode");
        }
      }

    private List<SavServiceModel> _ServicesList;
    public List<SavServiceModel> ServicesList
      {
      get { return _ServicesList; }
      set
        {
        _ServicesList = value;
        OnPropertyChanged("ServicesList");
        }
      }

    private SavServiceModel _SelectedService;
    public SavServiceModel SelectedService
      {
      get { return _SelectedService; }
      set
        {
        _SelectedService = value;
        OnPropertyChanged("SelectedService");
        }
      }

    private bool _IsPlayerService;
    public bool IsPlayerService
      {
      get { return _IsPlayerService; }
      set
        {
        _IsPlayerService = value;
        OnPropertyChanged("IsPlayerService");
        }
      }

    private bool _IsPassengerService;
    public bool IsPassengerService
      {
      get { return _IsPassengerService; }
      set
        {
        _IsPassengerService = value;
        OnPropertyChanged("IsPassengerService");
        }
      }

    private List<string> _StopPointsList;
    public List<string> StopPointsList
      {
      get { return _StopPointsList; }
      set
        {
        _StopPointsList = value;
        OnPropertyChanged("StopPointsList");
        }
      }

    private string _StopLocation;
    public string StopLocation
      {
      get { return _StopLocation; }
      set
        {
        _StopLocation = value;
        OnPropertyChanged("StopLocation");
        }
      }

    private string _SelectedStopLocation;
    public string SelectedStopLocation
      {
      get { return _SelectedStopLocation; }
      set
        {
        _SelectedStopLocation = value;
        OnPropertyChanged("SelectedStopLocation");
        }
      }


    #region Builder

    // https://stackoverflow.com/questions/5958495/append-data-to-byte-array/5958537

    public void Build()
      {
      string fileName = GetClonedScenarioFileName(false);
      using (MemoryStream ms = new MemoryStream())
        {
        // You could also just use StreamWriter to do "writer.Write(bytes)"
        var bytes = BuildRouteString(Scenario.SavScenario.RouteString);
        ms.Write(bytes, 0, bytes.Length);

        bytes = BuildPreAmbleFiller();
        ms.Write(bytes, 0, bytes.Length);
        bytes = BuildStringProperty("UserScenarioName", ClonedScenarioName);
        ms.Write(bytes, 0, bytes.Length);
        var guid=  BuildGuidProperty(ClonedScenarioGuid);
        bytes = BuildStruct("UserScenarioGuid", "Guid",guid);
        ms.Write(bytes, 0, bytes.Length);
        bytes = BuildBoolean("bRulesDisabledMode", Scenario.SavScenario.RulesDisabledMode);
        ms.Write(bytes, 0, bytes.Length);
        bytes = BuildBoolean("bGlobalElectrificationMode", Scenario.SavScenario.GlobalElectrificationMode);
        ms.Write(bytes, 0, bytes.Length);
        var targetAssetBytes = GetStringByteString(Scenario.SavScenario.TargetAsset); 
        bytes = BuildSoftObjectStruct("TargetAsset", "SoftObjectPath", targetAssetBytes, 4);
        ms.Write(bytes, 0, bytes.Length);
        bytes = BuildServices();
        ms.Write(bytes, 0, bytes.Length);
        WriteClonedScenarioToFile(fileName, ms.ToArray());
        }
      }

    private byte[] BuildRouteString(string routeString)
      {
      return GetStringByteString(routeString);
      }


    private byte[] BuildPreAmbleFiller()
      {
      byte[] output = new byte[9];
      for(int j=4; j<8; j++)
        {
        output[j] = 0xCD;
        }
      return output;
      }

    private byte[] BuildStringProperty(string typeName, string payLoad)
      {
      var typeNameBytes = GetStringByteString(typeName);
      var propertyBytes = GetStringByteString("StrProperty");
      var payLoadBytes = GetStringByteString(payLoad);
      var payLoadLengthBytes = GetBytesPayloadLength(payLoadBytes); // includes string length in count
      var output = typeNameBytes.Concat(propertyBytes).Concat(payLoadLengthBytes).Concat(payLoadBytes).ToArray(); // Using Linq here :-)
      return output;
      }

    private byte[] BuildNameProperty(string typeName, string payLoad)
      {
      var typeNameBytes = new byte[0];
      if (typeName.Length > 0)
        {
        typeNameBytes = GetStringByteString(typeName);
        }
      var propertyBytes = GetStringByteString("NameProperty");
      var payLoadBytes = GetStringByteString(payLoad);
      var payLoadLengthBytes = GetBytesPayloadLength(payLoadBytes);
      var output = typeNameBytes.Concat(propertyBytes).Concat(payLoadLengthBytes).Concat(payLoadBytes).ToArray(); // Using Linq here :-)
      return output;
      }

    private byte[] BuildScenarioName(string scenarioName)
      {
      var typeNameBytes = GetStringByteString("UserScenarioName");
      var propertyBytes = GetStringByteString("StrProperty");
      var filler = new byte[1];

      var strIndex = new byte[4];
      var payLoadBytes = GetStringByteString(scenarioName);
      var payLoadLengthBytes = ConvertIntToBytes(payLoadBytes.Length); // TODO is this always correct? It should be but ....
      var output = typeNameBytes
        .Concat(propertyBytes)
        .Concat(payLoadLengthBytes)
        .Concat(strIndex)
        .Concat(filler)
        .Concat(payLoadBytes)
        .ToArray(); // Using Linq here :-)
      return output;
      }

  

  private byte[] BuildSoftObjectProperty(string typeName, string payLoad)
      {
      var typeNameBytes = GetStringByteString(typeName);
      var filler2 = new byte[4];
      var propertyBytes = GetStringByteString("SoftObjectProperty");
      var payLoadBytes = GetStringByteString(payLoad);
      var payLoadLengthBytes = GetBytesPayloadLength(payLoadBytes,1,4); // includes string length
      var output = typeNameBytes
        .Concat(propertyBytes)
        .Concat(payLoadLengthBytes)
        .Concat(payLoadBytes)
        .Concat(filler2) // No idea if this is correct, not included after string property, but it comes after a SoftObjectProperty
        .ToArray(); // Using Linq here :-)
      return output;
      }



    private byte[] GetStringByteString(string input)
      {
      int length = input.Length + 1; // Must include terminating zero
      byte[] finalZero;
      byte[] stringAsBytes;
      if (IsUnicode(input))
        {
        length= -length;
        finalZero = new byte[2];
        stringAsBytes = Encoding.Unicode.GetBytes(input);
        }
      else
        {
        finalZero = new byte[1];
        stringAsBytes = Encoding.ASCII.GetBytes(input);
        }
      var stringLengthBytes = ConvertIntToBytes(length);
      
      var output = stringLengthBytes.Concat(stringAsBytes).Concat(finalZero).ToArray();
      return output;
      }

    private byte[] GetStringPayloadLength(string payLoad, int fillers=1)
      {
      var length = payLoad.Length + 1;
      return GetLengthBytesWithIndex(length, fillers);
      }
    
    private byte[] GetBytesPayloadLength(byte[] payLoad, int fillers=1, int additionalLength = 0)
      {
      var length = payLoad.GetLength(0);
      return GetLengthBytesWithIndex(length, fillers, additionalLength);
      }



    private byte[] GetLengthBytesWithIndex(int length, int fillers = 1, int additionalLength=0)
      {
      var lb = ConvertIntToBytes(length+additionalLength);
      var indexBytes = new byte[4 + fillers]; // includes additional filler byte
      return lb.Concat(indexBytes).ToArray();
      }

    private byte[] BuildStruct(string typeName, string payloadType, byte[] payload, int fillers=0)
      {
      var propertyNameBytes = GetStringByteString(typeName);
      var propertyTypeBytes = GetStringByteString("StructProperty");
      var payloadlengthBytes = GetBytesPayloadLength(payload, fillers);
      var payloadTypeBytes = GetStringByteString(payloadType);
      var filler = new byte[17];
      var output = propertyNameBytes.Concat(propertyTypeBytes)
                                          .Concat(payloadlengthBytes)
                                          .Concat(payloadTypeBytes)
                                          .Concat(filler)
                                          .Concat(payload).ToArray();
      return output;
      }

    private byte[] BuildSoftObjectStruct(string typeName, string payloadType, byte[] payload, int fillers = 0)
      {
      var propertyNameBytes = GetStringByteString(typeName);
      var propertyTypeBytes = GetStringByteString("StructProperty");
      var length = payload.GetLength(0)+4;
      var payloadLengthBytes = ConvertIntToBytes(length);
      var payloadTypeBytes = GetStringByteString(payloadType);
      var filler1 = new byte[17];
      var filler2 = new byte[4];
      var output = propertyNameBytes.Concat(propertyTypeBytes)
        .Concat(payloadLengthBytes)
        .Concat(filler2) // placeholder for unused index/repeat count?
        .Concat(payloadTypeBytes)
        .Concat(filler1)
        .Concat(payload)
        .Concat(filler2)
        .ToArray();
      return output;
      }

    private byte[] BuildBoolean(string propertyName, bool value)
      {
      var propertyNameBytes = GetStringByteString(propertyName);
      var typeNameBytes= GetStringByteString("BoolProperty");
      var filler = new byte[8]; // length and index, not used
      var boolValueBytes = new byte[2];
      boolValueBytes[0] = Convert.ToByte(value);
      var output = propertyNameBytes.Concat(typeNameBytes).Concat(filler).Concat(boolValueBytes)
        .ToArray();
      return output;

      }

    private byte[] BuildGuidProperty(Guid myGuid)
      {
      byte[] guidBytes = myGuid.ToByteArray();
      var output = guidBytes;
      return output;
      }
    #endregion

    #region BuildServices

    private byte[] BuildServices()
      {
      byte[] output=null;
      var arrayName = GetStringByteString("Services");
      var payload = BuildServicesPayload();
      var arrayProperty = GetServicesArrayProperty(payload, Scenario.SavScenario.SavServiceList.Count);
      output= arrayName
        .Concat(arrayProperty)
        .Concat(BuildNoneString()) // TODO is this at the correct level?
        .ToArray();

      return output;
      }

    private byte[] GetServicesArrayProperty(byte[] payload, int elementCount)
      {
      var arrayPropertyType = GetStringByteString("ArrayProperty");
      var payloadLength = payload.GetLength(0)+4; // TODO check adjusted length with 4, not sure if this is correct ??

      // DEBUG
      //byte[] l1Bytes = {0x7B, 0x04, 0x00, 0x00};
      //int l1= BitConverter.ToInt32(l1Bytes, 0);

      //byte[] l2Bytes = { 0x7F, 0x04, 0x00, 0x00 };
      //int l2 = BitConverter.ToInt32(l2Bytes, 0);
      // DEBUG ends here

      var payloadLengthBytes = ConvertIntToBytes(payloadLength); 
      byte[] indexFiller1 = new byte[4];
      byte[] filler = new byte[1];
      byte[] elementCountBytes = ConvertIntToBytes(elementCount);
      var arrayObjectType= GetStringByteString("StructProperty");


      return arrayPropertyType
        .Concat(payloadLengthBytes)
        .Concat(indexFiller1)
        .Concat(arrayObjectType)
        .Concat(filler)
        .Concat(elementCountBytes)
        .Concat(payload)
        .ToArray();
      }

    private byte[] BuildServicesPayload()
      {
      var payloadContent = new byte[0];
      foreach (var service in Scenario.SavScenario.SavServiceList)
        {
        var serviceContent = BuildServicesPayLoadElement(service); // TODO one or al?
        payloadContent = payloadContent.Concat(serviceContent).ToArray();
        }

      var output = BuildServicesStruct("Services", "ScenarioDesignService", payloadContent);
      return output;
      }

    private byte[] BuildServicesStruct(string typeName, string payloadType, byte[] payload)
      {
      var propertyNameBytes = GetStringByteString(typeName);
      var propertyTypeBytes = GetStringByteString("StructProperty");
      var payloadLengthBytes = GetBytesPayloadLength(payload,0);
      var structIndex = new byte[4];
      var payloadTypeBytes = GetStringByteString(payloadType);
      var filler = new byte[17];
      var output = propertyNameBytes.Concat(propertyTypeBytes)
        .Concat(payloadLengthBytes)
        .Concat(payloadTypeBytes)
        .Concat(filler)
        .Concat(payload).ToArray();
      return output;
      }


    private byte[] BuildServicesPayLoadElement(SavServiceModel service)
      {
      var serviceName = BuildStringProperty("UserServiceName", service.ServiceName);
      var formationStruct = BuildFormation(service);

      var guid= BuildGuidProperty(service.ServiceGuid);
      var serviceGuid= BuildStruct("ServiceGuid", "Guid", guid);
      var pathName = BuildNameProperty("PathName", service.ServicePath);
      var startPoint = BuildNameProperty("StartPoint", service.StartPoint);
      var endPoint = BuildNameProperty("EndPoint", service.EndPoint);
      var stopPoints = BuildStopPoints(service.StopLocationList);
      var playerService = BuildBoolean("bPlayerService", service.IsPlayerService);
      var passengerService = BuildBoolean("bPassengerService", service.IsPassengerService);
      var startTime = BuildStartTime(service.StartTime);
      var none = BuildNoneString();
      var output = serviceName
        .Concat(formationStruct)
        .Concat(pathName)
        .Concat(startPoint)
        .Concat(endPoint)
        .Concat(stopPoints)
        .Concat(serviceGuid)
        .Concat(playerService)
        .Concat(passengerService)
        .Concat(startTime)
        .Concat(none)
        .ToArray();
      return output;
      }


    private byte[] BuildFormation(SavServiceModel service)
      {
      var formation = BuildFormationPayload(service);

      var output = BuildStruct("formation", "ScenarioDesignFormation", formation);
      return output;
      }


    private byte[] BuildFormationPayload(SavServiceModel service)
      {
      var driveableVehicle =
        BuildSoftObjectProperty("DrivableVehicleOverride", service.EngineString);
      var selectedReskins = BuildSelectedReskinsArray(service);
      var trainEntry = BuildSoftObjectProperty("TrainEntry", service.ConsistString);
      var none = BuildNoneString();

      var output= driveableVehicle
          .Concat(selectedReskins)
          .Concat(trainEntry)
        .Concat(none)
        .ToArray();
      return output;
      }


    private byte[] BuildSelectedReskinsArray(SavServiceModel service)
      {
      // TODO implement this fully ... now just a placeholder to make it work ....
      var selectedReskinsBytes = GetStringByteString("SelectedReskins");
      var arrayProperty = GetStringByteString("ArrayProperty");
      var namePropertyBytes = GetStringByteString("NameProperty");
      
      var filler2 = new byte[1];
      var liveryCount = 0;
      var arrayIndex = new byte[4];
      var liveryIdentifierBytes = new byte[0];
      if (service.LiveryIdentifier.Length > 0)
        {
        liveryCount = 1;
        liveryIdentifierBytes = GetStringByteString(service.LiveryIdentifier);
        }
      byte[] namePropertyIndex = ConvertIntToBytes(liveryCount);

      int payloadLength = namePropertyIndex.Length+liveryIdentifierBytes.Length;
      var payloadLengthBytes = ConvertIntToBytes(payloadLength);
      var output = selectedReskinsBytes
        .Concat(arrayProperty)
        .Concat(payloadLengthBytes)
        .Concat(arrayIndex)
        .Concat(namePropertyBytes)
        .Concat(filler2)
        .Concat(namePropertyIndex)
        .Concat(liveryIdentifierBytes)
        .ToArray();
      return output;
      }

    private byte[] BuildStopPoints(List<string> stopLocationList)
      {
      var stopPointsArrayName = GetStringByteString("EnabledStopPoints");
      var arrayPropertyType = GetStringByteString("ArrayProperty");
      var arrayIndex =new  byte[4];
      var arrayElementCount = ConvertIntToBytes(stopLocationList.Count);
      var nameProperty = GetStringByteString("NameProperty");
      var filler = new byte[1];
      var stopLocations = new byte[0];
      foreach (var stop in stopLocationList)
        {
        var result = GetStringByteString(stop);
        stopLocations = stopLocations.Concat(result).ToArray();
        }

      var arrayPayloadLength = ConvertIntToBytes(stopLocations.Length+4);
      var output = stopPointsArrayName
        .Concat(arrayPropertyType)
        .Concat(arrayPayloadLength)
        .Concat(arrayIndex)
        .Concat(nameProperty)
        .Concat(filler)
        .Concat(arrayElementCount)
        .Concat(stopLocations)
        .ToArray();
      return output;
      }

    private byte[] BuildStartTime(ulong startTime)
      {
      var timeSpan = BuildTimeSpan(startTime);

      var output = BuildStruct("StartTime", "Timespan", timeSpan);
      return output;
      }

    private byte[] BuildNoneString()
      {
      return GetStringByteString("None");
      }


    private byte[] BuildNoneStruct()
      {
      var none = BuildNoneString();
      var output = BuildStruct("", "None", none);
      return output;
      }
    private byte[] BuildTimeSpan(ulong timeValue)
      {
      var filler = new byte[0];
      var output = filler
        .Concat(BitConverter.GetBytes(timeValue* 10000000))
        .ToArray();
      return output;
      }

    #endregion

    #region ScenarioEditHandlers

    public void ScenarioEdit()
      {
      ClonedScenarioName = Scenario.SavScenario.ScenarioName;
      ClonedScenarioGuid = Scenario.SavScenario.ScenarioGuid;
      ScenarioStartTime = GetPlayerServiceStartTimeText(Scenario.SavScenario.SavServiceList);
      OffTheRailsMode = Scenario.SavScenario.RulesDisabledMode;
      ServicesList = Scenario.SavScenario.SavServiceList; // TODO: Warning, this is a reference copy. Is this what we intend? 
      }

    public static string GetPlayerServiceStartTimeText(List<SavServiceModel> savServiceList)
      {
      foreach (var service in savServiceList)
        {
        if (service.IsPlayerService)
          {
          return service.StartTimeText;
          }
        }
      return string.Empty;
      }


    public static ulong GetPlayerServiceStartTime(List<SavServiceModel> savServiceList)
      {
      foreach (var service in savServiceList)
        {
        if (service.IsPlayerService)
          {
          return service.StartTime;
          }
        }
      return 0;
      }

    public static void RecalculateStartTimes(long offset,List<SavServiceModel> savServiceList)
      {
      foreach (var service in savServiceList)
        {
        long tmp = (long) service.StartTime;
        service.StartTime=(ulong) (tmp+offset);
        service.StartTimeText = TimeConverters.SecondsToString(service.StartTime, false);
        }
      }

    public void ServiceEdit()
      {
      ServiceName = SelectedService.ServiceName;
      ServiceStartTime = SelectedService.StartTimeText;
      StartLocation = SelectedService.StartPoint;
      EndLocation = SelectedService.EndPoint;
      IsPlayerService = SelectedService.IsPlayerService;
      IsPassengerService = SelectedService.IsPassengerService;
      EngineString = SelectedService.EngineString;
      ConsistString = SelectedService.ConsistString;
      LiveryIdentifier = SelectedService.LiveryIdentifier;
      StopPointsList = new List<string>();
      foreach(var stopPoint in SelectedService.StopLocationList)
        {
        StopPointsList.Add(stopPoint);
        }
      }

    public void ServiceClone()
      {
      // TODO
      }

    public void ServiceDelete()
      {
      // TODO
      }

    public void MoveUpStopPoint()
      {
      //if (selectedPerson == null)
      //  {
      //  return;
      //  }
      //int ix = Persons.IndexOf(selectedPerson);
      //if (ix > 0)
      //  {
      //  Person _thisPerson = selectedPerson;
      //  Person _previousPerson = Persons[ix - 1];
      //  Persons[ix] = _previousPerson;
      //  Persons[ix - 1] = _thisPerson;
      //}
      }


    public void MoveDownStopPoint()
      {
      //if (selectedPerson == null)
      //  {
      //  return;
      //  }
      //int ix = Persons.IndexOf(selectedPerson);
      //if (ix < Persons.Count - 1)
      //  {
      //  Person _thisPerson = selectedPerson;
      //  Person _nextPerson = Persons[ix + 1];
      //  Persons[ix] = _nextPerson;
      //  Persons[ix + 1] = _thisPerson;
      //  }
      }

    public void UpdateScenarioStartTime()
      {
      if (TimeConverters.IsValidTimeString(ScenarioStartTime))
        {
        ulong newStartTime = TimeConverters.TimeStringToSeconds(ScenarioStartTime);
        ulong oldStartTime= GetPlayerServiceStartTime(Scenario.SavScenario.SavServiceList);
        if (newStartTime != oldStartTime)
          {
          long offset = (long) (newStartTime - oldStartTime);
          RecalculateStartTimes(offset, Scenario.SavScenario.SavServiceList);
          }
        }
      
      }

    public void SaveCopy()
      {
      UpdateScenarioStartTime();
      }

    public void SaveOverwrite()
      {
      }

    #endregion






    #region Clone
    public void Clone()
      {
      string fileName = GetClonedScenarioFileName();
      byte[] guidBytes = ClonedScenarioGuid.ToByteArray();
      byte[] clonedScenarioNameBytes = GetScenarioNameByteString(ClonedScenarioName);
      var sourceDataLength = Scenario.Cracker.Data.GetLength(0);
      // correct output length for the length of the scenario
      var sourceScenarioNameLength = Scenario.Cracker.SavDataList[1].Position -
                               Scenario.Cracker.SavDataList[0].Position - 37;
      var destinationScenarioNameLength = clonedScenarioNameBytes.GetLength(0);
      var destinationDataLength = sourceDataLength - sourceScenarioNameLength + destinationScenarioNameLength;
      var output = new byte[destinationDataLength];

      var destinationScenarioNamePosition = Scenario.Cracker.SavDataList[0].Position + 37; // tricky there is an additional null byte after the index
      int destinationOffset = destinationScenarioNameLength - sourceScenarioNameLength;
      int startIndexInData = Scenario.Cracker.SavDataList[1].Position;
      int destinationRemainderStartPositionScenario = Scenario.Cracker.SavDataList[1].Position + destinationOffset;
      int bytesToCopy = destinationDataLength - destinationRemainderStartPositionScenario;

      // copy part till ScenarioName
      Buffer.BlockCopy(Scenario.Cracker.Data, 0, output, 0, destinationScenarioNamePosition);
      // insert new scenario name
      Buffer.BlockCopy(clonedScenarioNameBytes, 0, output, destinationScenarioNamePosition,
        destinationScenarioNameLength);
      // copy rest of data
      Buffer.BlockCopy(Scenario.Cracker.Data,
        startIndexInData, output, startIndexInData + destinationOffset, bytesToCopy);

      // Update ScenarioGuid
      int destinationGuidLocation = Scenario.Cracker.SavDataList[1].Position + 74 + destinationOffset; // 74 is the offset till the actual Guid value
      Buffer.BlockCopy(guidBytes,
        0, output, destinationGuidLocation, 16);
      WriteClonedScenarioToFile(fileName, output);
      }

    private static void WriteClonedScenarioToFile(string fileName, byte[] clonedScenarioBytes)
      {
      //var destinationDataLength = clonedScenarioBytes.GetLength(0);
      try
        {
        File.WriteAllBytes(fileName, clonedScenarioBytes);
        }
      catch (Exception ex)
        {
        CLog.Trace("Exception during reading data for cloned scenario"); // TODO replace this by Logging.Library
        return;
        }
      }

    // public static void BlockCopy (Array src, int srcOffset, Array dst, int dstOffset, int count);
    private byte[] GetScenarioNameByteString(string clonedScenarioName)
      {
      int length = ClonedScenarioName.Length + 1; // Must include terminating zero and filler byte
      int payloadLength = length + 4;
      int byteSequenceLength = payloadLength + 8+1; // add space for payloadLength and index and filler byte
      var output = new byte[byteSequenceLength];
      var payLoadLengthPart = ConvertIntToBytes(payloadLength);
      Buffer.BlockCopy(payLoadLengthPart, 0, output, 0, 4);
      var stringLengthBytes = ConvertIntToBytes(length);
      Buffer.BlockCopy(stringLengthBytes, 0, output, 9, 4);
      var stringAsBytes = Encoding.Default.GetBytes(ClonedScenarioName);
      Buffer.BlockCopy(stringAsBytes, 0, output, 13, length - 1);
      return output;
      }

    public static bool IsUnicode(string input)
      {
      return (Encoding.UTF8.GetByteCount(input) != input.Length); 
      }

    private byte[] ConvertIntToBytes(int inp)
      {
      byte[] intBytes = BitConverter.GetBytes(inp);
      if (!BitConverter.IsLittleEndian)
        Array.Reverse(intBytes);
      byte[] result = intBytes;
      return result;
      }
    #endregion

    public string GetClonedScenarioFileName(bool isTest=false)
      {
      if (isTest)
        {
        return $"{CTSWOptions.GameSaveLocation}Saved\\SaveGames\\TestScenario.sav";
        }
      return $"{CTSWOptions.GameSaveLocation}Saved\\SaveGames\\USD_{ClonedScenarioGuid.ToString().ToUpper()}.sav";
      }
    }
  }
