using Logging.Library;
using SavCracker.Library;
using SavCrackerTest.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace SavCrackerTest
  {
  public class SavCracker
    {
    public SavScenarioModel Scenario { get; set; } = new SavScenarioModel();
    public int Position { get; set; }
    public string LogString { get; set; } = string.Empty;
    public byte[] Data { get; set; }
    public string ScenarioFileName { get; set; }
    public List<SavPropertyModel> SavDataList { get; set; } = new List<SavPropertyModel>();


    public SavCracker(string scenarioFileName)
      {
      ScenarioFileName = scenarioFileName;
      Data = ReadDataFromStream(ScenarioFileName);
      }

    private byte[] ReadDataFromStream(string scenarioFileName)
      {
      try
        {
        var s = new FileStream(
          scenarioFileName, FileMode.Open);
        byte[] data = new BinaryReader(s).ReadBytes(((int)s.Length));
        return data;
        }
      catch (Exception ex)
        {
        Console.WriteLine("Exception during reading data "); //TODO: use logging
        return null;
        }
      }

    public void ParseScenario()
      {
      GetRoute();
      try
        {
        SkipCode(9);
        while (Position < Data.GetLength(0))
          {
          var result = ProcessNextProperty(Position);
          SavDataList.Add(result);
          }
        }
      catch (Exception ex)
        {
        Log.Trace($"Fatal error parsing scenario file {ScenarioFileName}", ex, LogEventType.Error);
        return;
        }
      }


 

    private void GetRoute()
      {
      var position = Position;
      var element = GrabStringElement();
      Scenario.ElementList.Add(element);
      var s = GetStringFromElement(element);
      Scenario.RouteString = s;
      s = RouteLogic.StripRouteString(s);
      RouteLogic.HarmonizeRouteName(Scenario, s);
      LogString += $"{position:D4} {s}\n";
      }



    private SavPropertyModel ProcessNextProperty(int position)
      {
      var elementName = GrabStringElement();
      var s = GetStringFromElement(elementName);
      if (String.CompareOrdinal(s, "None") == 0)
        {
        var output = new SavPropertyModel();
        output.PropertyType = PropertyTypeEnum.Empty;
        output.PropertyName = "None";
        output.Position = position;
        return output;
        }

      if (String.CompareOrdinal(s, "NameProperty") == 0)
        {
        var output = new StringPropertyModel();
        output.Position = position;
        output.Length = elementName.Length + 4;
        output.PropertyName = string.Empty;
        output.PropertyType = PropertyTypeEnum.NameProperty;
        var element = GrabStringElement();
        output.Length += element.Length + 4;
        output.Value = GetStringFromElement(element);
        LogString += $"{output.Report}\n";
        return output;
        }
      else
        {
        var element = GrabStringElement();
        var propertyTypeString = GetStringFromElement(element);
        var output = CreatePropertyType(propertyTypeString, s);
        output.Position = position;
        output.Length = elementName.Length + element.Length + 8;
        output.PropertyName = s;
        LogString += $"{output.Report}\n";
        return output;
        }
      }

    public SavPropertyModel CreatePropertyType(string propertyTypeString, string propertyName)
      {
      switch (propertyTypeString)
        {
        case "NameProperty":
            {
            var output = new StringPropertyModel();
            output.PropertyType = PropertyTypeEnum.NameProperty;
            output.PropertyName = propertyName;
            // get payload length and index
            output.ContentLength = GetInt();
            output.IndexValue = GetInt();
            SkipCode(1); //skip terminator here

            // get value
            var element = GrabStringElement();
            output.Value = GetStringFromElement(element);
            output.Length += element.Length + 4;
            return output;
            }
        case "StrProperty":
            {
            var output = new StringPropertyModel();
            output.PropertyType = PropertyTypeEnum.StringProperty;
            output.PropertyName = propertyName;
            // get payload length and index
            output.ContentLength = GetInt();
            output.IndexValue = GetInt();
            SkipCode(1); //skip terminator here

            // get value
            var element = GrabStringElement();
            output.Value = GetStringFromElement(element);
            output.Length += element.Length + 4;
            return output;
            }
        case "BoolProperty":
            {
            var output = new BoolPropertyModel();
            output.PropertyType = PropertyTypeEnum.BoolProperty;
            output.PropertyName = propertyName;
            SkipCode(8); // 8 null values, no need for length and index?
            var element = GrabFlagElement();
            output.Length += 9;
            output.Value = BitConverter.ToBoolean(element.Data, 0);
            return output;
            }
        case "StructProperty":
            {
            var output = new StructPropertyModel();
            output.PropertyType = PropertyTypeEnum.StructProperty;
            output.PropertyName = propertyName;
            // get payload length and index
            output.ContentLength = GetInt();
            output.IndexValue = GetInt();
            var structureElement = GrabStringElement();
            var structureContentNameString = GetStringFromElement(structureElement);
            output.StructureContentName = structureContentNameString;
            output.StructureType = GetStructureType(structureContentNameString);

            switch (output.StructureType)
              {
              case StructureTypeEnum.Guid:
                  {
                  var g = new GuidPropertyModel();
                  g.PropertyName = "Guid";
                  g.Position = Position;
                  g.Length = 16; // check this!
                  g.PropertyType = PropertyTypeEnum.GuidProperty;
                  SkipCode(17); // This should contain a Value type but seems not to be used. Contains null values
                  g.GuidValue = GetGuid();
                  output.PayLoad.Add(g);
                  break;
                  }
              case StructureTypeEnum.TimeSpan:
                  {
                  SkipCode(17);
                  var timeSpanElement = GrabCodeElement(8);
                  //Array.Reverse(timeSpanElement.Data);
                  //DateTime getNow = DateTime.FromBinary(getLong);
                  var t = new TimespanPropertyModel();
                  t.PropertyName = "StartTime";
                  t.PropertyType = PropertyTypeEnum.TimeSpanProperty;
                  t.TimeValue = BitConverter.ToUInt64(timeSpanElement.Data, 0) / 10000000; //TODO: check again, assumes time in seconds
                  var minutes = t.TimeValue / 60;
                  var hours = minutes / 60;
                  minutes = minutes - hours * 60;
                  t.TimeString = $"{hours:D2}:{minutes:D2}";
                  output.PayLoad.Add(t);
                  break;
                  }
              case StructureTypeEnum.SoftObjectPath:
                  {
                  var p = new StringPropertyModel();
                  p.PropertyName = "SoftObjectPath";
                  p.Position = Position;
                  p.PropertyType = PropertyTypeEnum.SoftObjectPathProperty;
                  SkipCode(17);
                  var element = GrabStringElement();
                  p.Value = GetStringFromElement(element);
                  SkipCode(4); //four null bytes found after the string
                  p.Length += element.Length + 8;
                  output.PayLoad.Add(p);
                  break;
                  }
              case StructureTypeEnum.EmbeddedObject:
                  {
                  SkipCode(17);
                  int structPosition = Position; // check if this is correct
                  while (Position < structPosition + output.ContentLength)
                    {
                    var result = ProcessNextProperty(Position);
                    output.PayLoad.Add(result);

                    }
                  break;
                  }
              }
            return output;
            }
        case "ArrayProperty":
            {
            var output = new ArrayPropertyModel();
            output.PropertyType = PropertyTypeEnum.ArrayProperty;
            // get payload length and index
            output.ContentLength = GetInt();
            output.IndexValue = GetInt();
            var element = GrabStringElement();
            var elementTypeString = GetStringFromElement(element);
            output.ElementType = GetArrayElementType(elementTypeString);
            SkipCode(1);
            output.ElementCount = GetInt();
            // Tricky, if the ArrayProperty has elementTpe NameProperty a shortcut is used, where a list of strings is provided.
            if (output.ElementType == PropertyTypeEnum.NameProperty)
              {
              for (int i = 0; i < output.ElementCount; i++)
                {
                var n = new StringPropertyModel();
                n.PropertyType = PropertyTypeEnum.NameProperty;
                n.PropertyName = propertyName;
                // get payload length and index
                n.ContentLength = -1;
                n.IndexValue = -1;
                // get value
                var nameElement = GrabStringElement();
                n.Value = GetStringFromElement(nameElement);
                output.Length += element.Length + 4;
                output.PayLoad.Add(n);
                }
              }
            else
              {
              for (int i = 0; i < output.ElementCount; i++)
                {
                var result = ProcessNextProperty(Position);
                output.PayLoad.Add(result);
                }
              }
            return output;
            }
        case "SoftObjectProperty":
            {
            var output = new SoftObjectPropertyModel();
            output.PropertyType = PropertyTypeEnum.SoftObjectProperty;
            output.PropertyName = propertyName;
            // get payload length and index
            output.ContentLength = GetInt();
            output.IndexValue = GetInt();
            SkipCode(1); //skip terminator here

            // get value
            var element = GrabStringElement();
            output.Value = GetStringFromElement(element);
            output.Length += element.Length + 4;
            SkipCode(4); // TODO: is this correct? 
            return output;
            }
        }
      return null;
      }

    public PropertyTypeEnum GetArrayElementType(string propertyTypeString)
      {
      switch (propertyTypeString)
        {
        case "NameProperty":
            {
            return PropertyTypeEnum.NameProperty;
            }
        case "StrProperty":
            {
            return PropertyTypeEnum.StringProperty;
            }
        case "BoolProperty":
            {
            return PropertyTypeEnum.BoolProperty;
            }
        case "StructProperty":
            {
            return PropertyTypeEnum.StructProperty;
            }
        case "ArrayProperty":
            {
            return PropertyTypeEnum.ArrayProperty;
            }
        case "SoftObjectProperty":
            {
            return PropertyTypeEnum.SoftObjectProperty;
            }
        }
      return PropertyTypeEnum.Undefined;
      }


    private int GetInt()
      {
      var position = Position;
      var element = GrabCodeElement(4);
      if (!BitConverter.IsLittleEndian)
        {
        Array.Reverse(element.Data);
        }
      int output = BitConverter.ToInt32(element.Data, 0);
      return output;
      }

    private StructureTypeEnum GetStructureType(string structureTypeTypeString)
      {

      switch (structureTypeTypeString)
        {
        case "Guid":
            {
            return StructureTypeEnum.Guid;
            }
        case "Timespan":
            {
            return StructureTypeEnum.TimeSpan;

            }
        case "SoftObjectPath":
            {
            return StructureTypeEnum.SoftObjectPath;
            }
        case "ScenarioDesignService":
        case "ScenarioDesignFormation":
            {
            return StructureTypeEnum.EmbeddedObject;
            }
        default:
            {
            return StructureTypeEnum.Undefined;
            }
        }
      }

    private Guid GetGuid()
      {
      var position = Position;
      var element = GrabGuidElement();
      ReadOnlySpan<Byte> guidSpan = new ReadOnlySpan<Byte>(element.Data);
      var output = new Guid(guidSpan);
      return output;
      }

    private SavElementModel GrabGuidElement()
      {
      var element = new SavElementModel();
      byte[] subset = new byte[16];
      System.Array.ConstrainedCopy(Data, Position, subset, 0, 16);
      element.Length = 16;
      element.Position = Position;
      element.DataType = SavDataType.Guid;
      element.Data = subset;
      Position += 16;
      return element;
      }


    // https://stackoverflow.com/questions/18306104/c-sharp-byte-array-to-string-and-vice-versa
    private static string ConvertByteArrayToString(Byte[] ByteOutput)
      {
      string StringOutput = System.Text.Encoding.UTF8.GetString(ByteOutput);
      return StringOutput.Substring(0, StringOutput.Length - 1); //strip trailing zero byte
      }

    public string GetStringFromElement(SavElementModel element)
      {
      var s = string.Empty;
      if (element.Length > 0) // negative length means Unicode string
        {
        return ConvertByteArrayToString(element.Data);
        }
      s = System.Text.Encoding.Unicode.GetString(element.Data, 0, Math.Abs(element.Length * 2));
      return s.Substring(0, s.Length - 1);
      }
    public void SkipCode(int length)
      {
      var position = Position;
      var element = GrabCodeElement(length);
      Scenario.ElementList.Add(element);
      LogString += $"{position:D4} Code length {length}\n";
      }


    public SavElementModel GrabCodeElement(int length)
      {
      var element = new SavElementModel();
      byte[] subset = new byte[length];
      System.Array.ConstrainedCopy(Data, Position, subset, 0, length);
      element.Position = Position;
      element.Length = length;
      element.Data = subset;
      element.DataType = SavDataType.Code;
      Position += length;
      return element;
      }

    private SavElementModel GrabFlagElement()
      {
      var element = new SavElementModel();
      byte[] subset = new byte[1];
      System.Array.ConstrainedCopy(Data, Position, subset, 0, 1);
      element.Position = Position;
      element.Length = 1;
      element.Data = subset;
      element.DataType = SavDataType.Flag;
      Position += 2; // you need to skip a null terminating value here
      return element;
      }

    private SavElementModel GrabStringElement()
      {
      var element = new SavElementModel();
      int length = GetInt();
      int absLength = length;

      if (length < 0)
        {
        absLength = Math.Abs(length) * 2; //Unicode!
        }
      byte[] subset = new byte[absLength];
      System.Array.ConstrainedCopy(Data, Position, subset, 0, absLength - 1);
      element.Position = Position;
      element.Length = length;
      element.Data = subset;
      element.DataType = SavDataType.Text;
      Position += absLength;
      return element;
      }
    }
  }

