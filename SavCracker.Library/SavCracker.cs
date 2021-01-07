using Logging.Library;
using SavCracker.Library;
using SavCracker.Library.Models;
using SavCrackerTest.Models;
using System;
using System.Collections.Generic;
using System.IO;
using Utilities.Library.TextHelpers;

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
    public static List<SavCrackerRouteModel> RouteList;

    public SavCracker(string scenarioFileName)
      {
      ScenarioFileName = scenarioFileName;
      Data = ReadDataFromStream(ScenarioFileName);
      }

    private byte[] ReadDataFromStream(string scenarioFileName)
      {
      try
        {
        //var s = new FileStream(
        //  scenarioFileName, FileMode.Open);
        //byte[] data = new BinaryReader(s).ReadBytes(((int)s.Length));
        var data = File.ReadAllBytes(scenarioFileName);
        return data;
        }
      catch (Exception ex)
        {
        Log.Trace("Exception during reading data ",ex, LogEventType.Error);
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
      RouteLogic.HarmonizeRouteName(Scenario, s, RouteList);
      LogString += $"{position:D4} {s}\n";
      }

    private SavPropertyModel ProcessNextProperty(int position)
      {
      var elementName = GrabStringElement();
      var s = GetStringFromElement(elementName);
      if (String.CompareOrdinal(s, "None") == 0)
        {
        return GrabNoneElement(position);
        }

      if (String.CompareOrdinal(s, "NameProperty") == 0)
        {
        return CreateAnonymousNameProperty(position, elementName);
        }
      else
        {
        return GrabProperty(position, elementName, s);
        }
      }

    private SavPropertyModel GrabProperty(int position, SavElementModel elementName, string s)
      {
      var element = GrabStringElement();
      var propertyTypeString = GetStringFromElement(element);
      var output = CreatePropertyType(propertyTypeString, s);
      output.Position = position;
      output.PropertyName = s;
      return output;
      }

    private SavPropertyModel CreateAnonymousNameProperty(int position, SavElementModel elementName)
      {
      var output = new StringPropertyModel();
      output.Position = position;
      output.PropertyName = string.Empty;
      output.PropertyType = PropertyTypeEnum.NameProperty;
      var element = GrabStringElement();
      output.Value = GetStringFromElement(element);
      return output;
      }

    private static SavPropertyModel GrabNoneElement(int position)
      {
      var output = new SavPropertyModel();
      output.PropertyType = PropertyTypeEnum.Empty;
      output.PropertyName = "None";
      output.Position = position;
      return output;
      }

    public SavPropertyModel CreatePropertyType(string propertyTypeString, string propertyName)
      {
      switch (propertyTypeString)
        {
        case "NameProperty":
            {
            return CreateNameProperty(propertyName);
            }
        case "StrProperty":
            {
            return CreateStringProperty(propertyName);
            }
        case "BoolProperty":
            {
            return CreateBoolProperty(propertyName);
            }
        case "StructProperty":
            {
            var output = CreateStructProperty(propertyName);
            return output;
            }
        case "ArrayProperty":
            {
            return CreateArrayProperty(propertyName);
            }
        case "SoftObjectProperty":
            {
            return CreateSoftObjectProperty(propertyName);
            }
        }
      return null;
      }

    private SavPropertyModel CreateSoftObjectProperty(string propertyName)
      {
      var output = new SoftObjectPropertyModel
        {
        PropertyType = PropertyTypeEnum.SoftObjectProperty,
        PropertyName = propertyName,
        ContentLength = GetInt(),
        IndexValue = GetInt()
        };
      SkipCode(1); //skip terminator here

      // get value
      var element = GrabStringElement();
      output.Value = GetStringFromElement(element);
      SkipCode(4);
      return output;
      }

    private SavPropertyModel CreateArrayProperty(string propertyName)
      {
      var output = new ArrayPropertyModel
        {
        PropertyType = PropertyTypeEnum.ArrayProperty,
        ContentLength = GetInt(),
        IndexValue = GetInt()
        };
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
          var n = new StringPropertyModel
            {
            PropertyType = PropertyTypeEnum.NameProperty,
            PropertyName = propertyName,
            ContentLength = -1,
            IndexValue = -1
            };
          var nameElement = GrabStringElement();
          n.Value = GetStringFromElement(nameElement);
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

    private StructPropertyModel CreateStructProperty(string propertyName)
      {
      var output = new StructPropertyModel
        {
        PropertyType = PropertyTypeEnum.StructProperty,
        PropertyName = propertyName,
        ContentLength = GetInt(),
        IndexValue = GetInt()
        };
      var structureElement = GrabStringElement();
      var structureContentNameString = GetStringFromElement(structureElement);
      output.StructureContentName = structureContentNameString;
      output.StructureType = GetStructureType(structureContentNameString);
      SkipCode(17); // This should contain a Value type but seems not to be used. Contains null values

      switch (output.StructureType)
        {
        case StructureTypeEnum.Guid:
            {
            var g = CreateGuidProperty();
            output.PayLoad.Add(g);
            break;
            }
        case StructureTypeEnum.TimeSpan:
            {
            var t = CreateTimeSpan();
            output.PayLoad.Add(t);
            break;
            }
        case StructureTypeEnum.SoftObjectPath:
            {
            var p = CreateSoftObjectPath();
            output.PayLoad.Add(p);
            break;
            }
        case StructureTypeEnum.EmbeddedObject:
            {
            int structPosition = Position;
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

    private StringPropertyModel CreateSoftObjectPath()
      {
      var p = new StringPropertyModel
        {
        PropertyName = "SoftObjectPath",
        Position = Position,
        PropertyType = PropertyTypeEnum.SoftObjectPathProperty
        };
      var element = GrabStringElement();
      p.Value = GetStringFromElement(element);
      SkipCode(4); //four null bytes found after the string
      return p;
      }

    private TimespanPropertyModel CreateTimeSpan()
      {
      var timeSpanElement = GrabCodeElement(8);
      var t = new TimespanPropertyModel();
      t.PropertyName = "StartTime";
      t.PropertyType = PropertyTypeEnum.TimeSpanProperty;
      t.TimeValue = BitConverter.ToUInt64(timeSpanElement.Data, 0) / 10000000;
      t.TimeString = TimeConverters.SecondsToString(t.TimeValue, false);
      return t;
      }

    private GuidPropertyModel CreateGuidProperty()
      {
      var g = new GuidPropertyModel
        {
        PropertyName = "Guid",
        Position = Position,
        PropertyType = PropertyTypeEnum.GuidProperty,
        GuidValue = GetGuid()
        };
      return g;
      }

    private SavPropertyModel CreateBoolProperty(string propertyName)
      {
      var output = new BoolPropertyModel
        {
        PropertyType = PropertyTypeEnum.BoolProperty,
        PropertyName = propertyName
        };
      SkipCode(8); // 8 null values, no need for length and index?
      var element = GrabFlagElement();
      output.Value = BitConverter.ToBoolean(element.Data, 0);
      return output;
      }

    private SavPropertyModel CreateStringProperty(string propertyName)
      {
      var output = new StringPropertyModel
        {
        PropertyType = PropertyTypeEnum.StringProperty,
        PropertyName = propertyName,
        ContentLength = GetInt(),
        IndexValue = GetInt()
        };
      SkipCode(1); //skip terminator here
      output.Value = GetString();
      return output;
      }


    private SavPropertyModel CreateNameProperty(string propertyName)
      {
      var output = new StringPropertyModel
        {
        PropertyType = PropertyTypeEnum.NameProperty,
        PropertyName = propertyName,
        ContentLength = GetInt(),
        IndexValue = GetInt()
        };
      SkipCode(1); //skip terminator here
      output.Value = GetString();
      return output;
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

    private string GetString()
      {
      var element = GrabStringElement();
      return GetStringFromElement(element);
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

