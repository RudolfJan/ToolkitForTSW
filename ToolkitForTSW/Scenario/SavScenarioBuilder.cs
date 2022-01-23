using Logging.Library;
using SavCracker.Library.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ToolkitForTSW.Scenario
  {
  public class SavScenarioBuilder
    {
    #region Builder

    public static string GetClonedScenarioFileName(string scenarioGuid, bool isTest = false)
      {
      if (isTest)
        {
        return $"{TSWOptions.GetSaveLocationPath()}Saved\\SaveGames\\TestScenario.sav";
        }
      return $"{TSWOptions.GetSaveLocationPath()}Saved\\SaveGames\\USD_{scenarioGuid.ToString().ToUpper()}.sav";
      }

    // https://stackoverflow.com/questions/5958495/append-data-to-byte-array/5958537

    public static void Build(CScenario scenario)
      {
      string fileName = scenario.ScenarioFile.FullName;
      using (MemoryStream ms = new MemoryStream())
        {
        // You could also just use StreamWriter to do "writer.Write(bytes)"
        var bytes = BuildRouteString(scenario.SavScenario.RouteString);
        ms.Write(bytes, 0, bytes.Length);

        bytes = BuildPreAmbleFiller();
        ms.Write(bytes, 0, bytes.Length);
        bytes = BuildStringProperty("UserScenarioName", scenario.SavScenario.ScenarioName);
        ms.Write(bytes, 0, bytes.Length);
        var guid = BuildGuidProperty(scenario.SavScenario.ScenarioGuid);
        bytes = BuildStruct("UserScenarioGuid", "Guid", guid);
        ms.Write(bytes, 0, bytes.Length);
        bytes = BuildBoolean("bRulesDisabledMode", scenario.SavScenario.RulesDisabledMode);
        ms.Write(bytes, 0, bytes.Length);
        bytes = BuildBoolean("bGlobalElectrificationMode", scenario.SavScenario.GlobalElectrificationMode);
        ms.Write(bytes, 0, bytes.Length);
        var targetAssetBytes = GetStringByteString(scenario.SavScenario.TargetAsset);
        bytes = BuildSoftObjectStruct("TargetAsset", "SoftObjectPath", targetAssetBytes);
        ms.Write(bytes, 0, bytes.Length);
        bytes = BuildServices(scenario.SavScenario);
        ms.Write(bytes, 0, bytes.Length);
        WriteScenarioToFile(fileName, ms.ToArray());
        }
      }

    private static void WriteScenarioToFile(string fileName, byte[] clonedScenarioBytes)
      {
      //var destinationDataLength = clonedScenarioBytes.GetLength(0);
      try
        {
        File.WriteAllBytes(fileName, clonedScenarioBytes);
        }
      catch (Exception ex)
        {
        Log.Trace("Exception during reading data for cloned scenario " + ex.Message); // TODO replace this by Logging.Library
        return;
        }
      }
    private static byte[] BuildRouteString(string routeString)
      {
      return GetStringByteString(routeString);
      }


    private static byte[] BuildPreAmbleFiller()
      {
      byte[] output = new byte[9];
      for (int j = 4; j < 8; j++)
        {
        output[j] = 0xCD;
        }
      return output;
      }

    private static byte[] BuildStringProperty(string typeName, string payLoad)
      {
      var typeNameBytes = GetStringByteString(typeName);
      var propertyBytes = GetStringByteString("StrProperty");
      var payLoadBytes = GetStringByteString(payLoad);
      var payLoadLengthBytes = GetBytesPayloadLength(payLoadBytes); // includes string length in count
      var output = typeNameBytes.Concat(propertyBytes).Concat(payLoadLengthBytes).Concat(payLoadBytes).ToArray(); // Using Linq here :-)
      return output;
      }

    private static byte[] BuildNameProperty(string typeName, string payLoad)
      {
      var typeNameBytes = Array.Empty<byte>();
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

    private static byte[] BuildSoftObjectProperty(string typeName, string payLoad)
      {
      var typeNameBytes = GetStringByteString(typeName);
      var filler2 = new byte[4];
      var propertyBytes = GetStringByteString("SoftObjectProperty");
      var payLoadBytes = GetStringByteString(payLoad);
      var payLoadLengthBytes = GetBytesPayloadLength(payLoadBytes, 1, 4); // includes string length
      var output = typeNameBytes
        .Concat(propertyBytes)
        .Concat(payLoadLengthBytes)
        .Concat(payLoadBytes)
        .Concat(filler2) // No idea if this is correct, not included after string property, but it comes after a SoftObjectProperty
        .ToArray(); // Using Linq here :-)
      return output;
      }

    private static byte[] GetStringByteString(string input)
      {
      int length = input.Length + 1; // Must include terminating zero
      byte[] finalZero;
      byte[] stringAsBytes;
      if (IsUnicode(input))
        {
        length = -length;
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

    private static byte[] GetBytesPayloadLength(byte[] payLoad, int fillers = 1, int additionalLength = 0)
      {
      var length = payLoad.GetLength(0);
      return GetLengthBytesWithIndex(length, fillers, additionalLength);
      }

    private static byte[] GetLengthBytesWithIndex(int length, int fillers = 1, int additionalLength = 0)
      {
      var lb = ConvertIntToBytes(length + additionalLength);
      var indexBytes = new byte[4 + fillers]; // includes additional filler byte
      return lb.Concat(indexBytes).ToArray();
      }

    private static byte[] BuildStruct(string typeName, string payloadType, byte[] payload, int fillers = 0)
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

    private static byte[] BuildSoftObjectStruct(string typeName, string payloadType, byte[] payload)
      {
      var propertyNameBytes = GetStringByteString(typeName);
      var propertyTypeBytes = GetStringByteString("StructProperty");
      var length = payload.GetLength(0) + 4;
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

    private static byte[] BuildBoolean(string propertyName, bool value)
      {
      var propertyNameBytes = GetStringByteString(propertyName);
      var typeNameBytes = GetStringByteString("BoolProperty");
      var filler = new byte[8]; // length and index, not used
      var boolValueBytes = new byte[2];
      boolValueBytes[0] = Convert.ToByte(value);
      var output = propertyNameBytes.Concat(typeNameBytes).Concat(filler).Concat(boolValueBytes)
        .ToArray();
      return output;

      }
    private static byte[] BuildGuidProperty(Guid myGuid)
      {
      byte[] guidBytes = myGuid.ToByteArray();
      var output = guidBytes;
      return output;
      }
    #endregion

    #region BuildServices

    private static byte[] BuildServices(SavScenarioModel savScenario)
      {
      var arrayName = GetStringByteString("Services");
      var payload = BuildServicesPayload(savScenario);
      var arrayProperty = GetServicesArrayProperty(payload, savScenario.SavServiceList.Count);
      byte[] output = arrayName
          .Concat(arrayProperty)
          .Concat(BuildNoneString()) // TODO is this at the correct level?
          .ToArray();

      return output;
      }

    private static byte[] GetServicesArrayProperty(byte[] payload, int elementCount)
      {
      var arrayPropertyType = GetStringByteString("ArrayProperty");
      var payloadLength = payload.GetLength(0) + 4; // TODO check adjusted length with 4, not sure if this is correct ??

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
      var arrayObjectType = GetStringByteString("StructProperty");

      return arrayPropertyType
        .Concat(payloadLengthBytes)
        .Concat(indexFiller1)
        .Concat(arrayObjectType)
        .Concat(filler)
        .Concat(elementCountBytes)
        .Concat(payload)
        .ToArray();
      }

    private static byte[] BuildServicesPayload(SavScenarioModel savScenario)
      {
      var payloadContent = Array.Empty<byte>();
      foreach (var service in savScenario.SavServiceList)
        {
        var serviceContent = BuildServicesPayLoadElement(service); // TODO one or al?
        payloadContent = payloadContent.Concat(serviceContent).ToArray();
        }

      var output = BuildServicesStruct("Services", "ScenarioDesignService", payloadContent);
      return output;
      }

    private static byte[] BuildServicesStruct(string typeName, string payloadType, byte[] payload)
      {
      var propertyNameBytes = GetStringByteString(typeName);
      var propertyTypeBytes = GetStringByteString("StructProperty");
      var payloadLengthBytes = GetBytesPayloadLength(payload, 0);
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

    private static byte[] BuildServicesPayLoadElement(SavServiceModel service)
      {
      var serviceName = BuildStringProperty("UserServiceName", service.ServiceName);
      var formationStruct = BuildFormation(service);

      var guid = BuildGuidProperty(service.ServiceGuid);
      var serviceGuid = BuildStruct("ServiceGuid", "Guid", guid);
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

    private static byte[] BuildFormation(SavServiceModel service)
      {
      var formation = BuildFormationPayload(service);

      var output = BuildStruct("formation", "ScenarioDesignFormation", formation);
      return output;
      }

    private static byte[] BuildFormationPayload(SavServiceModel service)
      {
      var driveableVehicle =
        BuildSoftObjectProperty("DrivableVehicleOverride", service.EngineString);
      var selectedReskins = BuildSelectedReskinsArray(service);
      var trainEntry = BuildSoftObjectProperty("TrainEntry", service.ConsistString);
      var none = BuildNoneString();

      var output = driveableVehicle
          .Concat(selectedReskins)
          .Concat(trainEntry)
        .Concat(none)
        .ToArray();
      return output;
      }

    private static byte[] BuildSelectedReskinsArray(SavServiceModel service)
      {
      // TODO implement this fully ... now just a placeholder to make it work ....
      var selectedReskinsBytes = GetStringByteString("SelectedReskins");
      var arrayProperty = GetStringByteString("ArrayProperty");
      var namePropertyBytes = GetStringByteString("NameProperty");

      var filler2 = new byte[1];
      var liveryCount = 0;
      var arrayIndex = new byte[4];
      var liveryIdentifierBytes = Array.Empty<byte>();
      if (service.LiveryIdentifier.Length > 0)
        {
        liveryCount = 1;
        liveryIdentifierBytes = GetStringByteString(service.LiveryIdentifier);
        }
      byte[] namePropertyIndex = ConvertIntToBytes(liveryCount);

      int payloadLength = namePropertyIndex.Length + liveryIdentifierBytes.Length;
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

    private static byte[] BuildStopPoints(List<string> stopLocationList)
      {
      var stopPointsArrayName = GetStringByteString("EnabledStopPoints");
      var arrayPropertyType = GetStringByteString("ArrayProperty");
      var arrayIndex = new byte[4];
      var arrayElementCount = ConvertIntToBytes(stopLocationList.Count);
      var nameProperty = GetStringByteString("NameProperty");
      var filler = new byte[1];
      var stopLocations = Array.Empty<byte>();
      foreach (var stop in stopLocationList)
        {
        var result = GetStringByteString(stop);
        stopLocations = stopLocations.Concat(result).ToArray();
        }

      var arrayPayloadLength = ConvertIntToBytes(stopLocations.Length + 4);
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

    private static byte[] BuildStartTime(ulong startTime)
      {
      var timeSpan = BuildTimeSpan(startTime);

      var output = BuildStruct("StartTime", "Timespan", timeSpan);
      return output;
      }

    private static byte[] BuildNoneString()
      {
      return GetStringByteString("None");
      }

    private static byte[] BuildTimeSpan(ulong timeValue)
      {
      var filler = Array.Empty<byte>();
      var output = filler
        .Concat(BitConverter.GetBytes(timeValue * 10000000))
        .ToArray();
      return output;
      }

    #endregion


    public static bool IsUnicode(string input)
      {
      return (Encoding.UTF8.GetByteCount(input) != input.Length);
      }

    public static byte[] ConvertIntToBytes(int inp)
      {
      byte[] intBytes = BitConverter.GetBytes(inp);
      if (!BitConverter.IsLittleEndian)
        Array.Reverse(intBytes);
      byte[] result = intBytes;
      return result;
      }
    }
  }
