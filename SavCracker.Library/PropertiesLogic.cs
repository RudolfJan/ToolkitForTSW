using Logging.Library;
using SavCracker.Library.Models;
using System;
using System.Collections.Generic;

namespace SavCracker.Library
  {
  public class PropertiesLogic
    {
    public static string GetSoftObjectPropertyValue(SavPropertyModel property)
      {
      if (property is not SoftObjectPropertyModel)
        {
        var ex = new InvalidCastException("property is not a SoftObjectPropertyModel");
        Log.Trace("property is not a SoftObjectPropertyModel, should never happen", ex, LogEventType.Error);
        throw ex;
        }
      var strProperty = (SoftObjectPropertyModel)property;
      return strProperty.Value;
      }

    public static ulong GetTimeSpanIntValue(SavPropertyModel property)
      {
      var timeSpanProperty = GetTimespanPropertyFromStruct(property);
      return timeSpanProperty.TimeValue;
      }

    public static string GetTimeSpanStringValue(SavPropertyModel property)
      {
      var timeSpanProperty = GetTimespanPropertyFromStruct(property);
      return timeSpanProperty.TimeString;
      }
    private static TimespanPropertyModel GetTimespanPropertyFromStruct(SavPropertyModel property)
      {
      if (property is not StructPropertyModel)
        {
        var ex = new InvalidCastException("property is not a StructPropertyModel");
        Log.Trace("property is not a StructProperty, should never happen", ex, LogEventType.Error);
        throw ex;
        }

      var structProperty = (StructPropertyModel)property;

      if (structProperty.PayLoad[0] is not TimespanPropertyModel)
        {
        var ex = new InvalidCastException("property is not a TimespanPropertyModel");
        Log.Trace("property is not a TimespanProperty, should never happen", ex, LogEventType.Error);
        throw ex;
        }
      var timeSpanProperty = (TimespanPropertyModel)structProperty.PayLoad[0];
      return timeSpanProperty;
      }

    private static GuidPropertyModel GetGuidPropertyFromStruct(SavPropertyModel property)
      {
      if (property is not StructPropertyModel)
        {
        var ex = new InvalidCastException("property is not a StructPropertyModel");
        Log.Trace("property is not a StructProperty, should never happen", ex, LogEventType.Error);
        throw ex;
        }
      var structProperty = (StructPropertyModel)property;
      if (structProperty.PayLoad[0] is not GuidPropertyModel)
        {
        var ex = new InvalidCastException("property is not a GuidPropertyModel");
        Log.Trace("property is not a GuidProperty, should never happen", ex, LogEventType.Error);
        throw ex;
        }
      var guidProperty = (GuidPropertyModel)structProperty.PayLoad[0];
      return guidProperty;
      }

    public static string GetReskin(SavPropertyModel property)
      {
      if (property is not ArrayPropertyModel)
        {
        var ex = new InvalidCastException("property is not an ArrayPropertyModel");
        Log.Trace("property is not an ArrayProperty, should never happen", ex, LogEventType.Error);
        throw ex;
        }
      var arrayProperty = (ArrayPropertyModel)property;
      if (arrayProperty.PayLoad.Count == 0)
        {
        return string.Empty;
        }
      if (arrayProperty.PayLoad[0] is not StringPropertyModel)
        {
        var ex = new InvalidCastException("property is not a StringPropertyPropertyModel");
        Log.Trace("property is not a StringProperty, should never happen", ex, LogEventType.Error);
        throw ex;
        }
      var nameProperty = (StringPropertyModel)arrayProperty.PayLoad[0];
      return nameProperty.Value;
      }

    public static Guid GetGuidPropertyValue(SavPropertyModel property)
      {
      var guidProperty = GetGuidPropertyFromStruct(property);
      return guidProperty.GuidValue;
      }

    private static StringPropertyModel GetSoftObjectPathFromStruct(SavPropertyModel property)
      {
      if (property is not StructPropertyModel)
        {
        var ex = new InvalidCastException("property is not a StructPropertyModel");
        Log.Trace("property is not a StructProperty, should never happen", ex, LogEventType.Error);
        throw ex;
        }
      var structProperty = (StructPropertyModel)property;
      if (structProperty.PayLoad[0] is not StringPropertyModel)
        {
        var ex = new InvalidCastException("property is not a SoftObjectPropertyModel");
        Log.Trace("property is not a SoftObjectProperty, should never happen", ex, LogEventType.Error);
        throw ex;
        }
      var softObjectPathProperty = (StringPropertyModel)structProperty.PayLoad[0];
      return softObjectPathProperty;
      }

    public static string GetSoftObjectPathValue(SavPropertyModel property)
      {
      var softObjectPathProperty = GetSoftObjectPathFromStruct(property);
      return softObjectPathProperty.Value;
      }

    public static string GetStrPropertyValue(SavPropertyModel property)
      {
      if (property is not StringPropertyModel)
        {
        var ex = new InvalidCastException("property is not a StringPropertyModel");
        Log.Trace("property is not a StringProperty, should never happen", ex, LogEventType.Error);
        throw ex;
        }
      var strProperty = (StringPropertyModel)property;
      return strProperty.Value;
      }


    public static bool GetBoolPropertyValue(SavPropertyModel property)
      {
      if (property is not BoolPropertyModel)
        {
        var ex = new InvalidCastException("property is not a BoolPropertyModel");
        Log.Trace("property is not a BoolProperty, should never happen", ex, LogEventType.Error);
        throw ex;
        }
      var boolProperty = (BoolPropertyModel)property;
      return boolProperty.Value;
      }

    public static List<string> GetStopLocations(SavPropertyModel property)
      {
      var stopLocations = new List<string>();
      if (property is not ArrayPropertyModel)
        {
        var ex = new InvalidCastException("property is not an ArrayPropertyModel");
        Log.Trace("property is not an ArrayPropertyModel, should never happen", ex, LogEventType.Error);
        throw ex;
        }

      var payloadProperty = (ArrayPropertyModel)property;
      foreach (var locationProperty in payloadProperty.PayLoad)
        {
        var location = GetStrPropertyValue(locationProperty);
        stopLocations.Add(location);
        }
      return stopLocations;
      }
    }
  }
