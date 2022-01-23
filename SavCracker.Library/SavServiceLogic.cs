using SavCracker.Library.Models;

namespace SavCracker.Library
  {
  public class SavServiceLogic
    {
    public static void BuildServices(SavScenarioModel savScenario, ArrayPropertyModel property)
      {
      // A bit tricky. It looks like the payload length of the array consists of the whole payload and the struct uses an empty property to separate the services.
      SavServiceModel service = new SavServiceModel();
      foreach (var arrayElement in property.PayLoad)
        {
        switch (arrayElement.PropertyName)
          {
          case "Services":
              {
              if (!(arrayElement is StructPropertyModel arrayStructElement))
                {
                break;
                }
              foreach (var payloadProperty in arrayStructElement.PayLoad)
                {
                switch (payloadProperty.PropertyName)
                  {
                  case "UserServiceName":
                      {
                      service = new SavServiceModel
                        {
                        ServiceName = PropertiesLogic.GetStrPropertyValue(payloadProperty)
                        };
                      break;
                      }
                  case "formation":
                  case "Formation":
                      {
                      if (!(payloadProperty is StructPropertyModel))
                        {
                        //exception
                        }
                      var formationProperty = (StructPropertyModel)payloadProperty;
                      foreach (var elementProperty in formationProperty.PayLoad)
                        {
                        switch (elementProperty.PropertyName)
                          {
                          case "DrivableVehicleOverride":
                              {
                              service.EngineString = PropertiesLogic.GetSoftObjectPropertyValue(elementProperty);
                              EngineLogic.ParseEngineString(service);
                              break;
                              }
                          case "TrainEntry":
                              {
                              service.ConsistString = PropertiesLogic.GetSoftObjectPropertyValue(elementProperty);
                              ConsistLogic.ParseConsistString(service);
                              break;
                              }
                          case "SelectedReskins":
                            {
                            service.LiveryIdentifier = PropertiesLogic.GetReskin(elementProperty);
                              break;
                              }
                          case "None":
                              {
                              break;
                              }
                          }
                        }

                      break;
                      }
                  case "PathName":
                      {
                      service.ServicePath = PropertiesLogic.GetStrPropertyValue(payloadProperty);
                      break;
                      }
                  case "StartPoint":
                      {
                      service.StartPoint = PropertiesLogic.GetStrPropertyValue(payloadProperty);
                      break;
                      }
                  case "EndPoint":
                      {
                      service.EndPoint = PropertiesLogic.GetStrPropertyValue(payloadProperty);
                      break;
                      }
                  case "EnabledStopPoints":
                      {
                      service.StopLocationList = PropertiesLogic.GetStopLocations(payloadProperty);
                      break;
                      }
                  case "ServiceGuid":
                      {
                      service.ServiceGuid = PropertiesLogic.GetGuidPropertyValue(payloadProperty);
                      break;
                      }
                  case "bPlayerService":
                      {
                      service.IsPlayerService = PropertiesLogic.GetBoolPropertyValue(payloadProperty);
                      break;
                      }
                  case "bPassengerService":
                      {
                      service.IsPassengerService = PropertiesLogic.GetBoolPropertyValue(payloadProperty);
                      break;
                      }
                  case "StartTime":
                      {
                      service.StartTime = PropertiesLogic.GetTimeSpanIntValue(payloadProperty);
                      service.StartTimeText = PropertiesLogic.GetTimeSpanStringValue(payloadProperty);
                      savScenario.SavServiceList.Add(service);
                      break;
                      }
                  case "None":
                      {
                      break;
                      }
                  }
                }

              break;
              }
          case "None":
              {
              break;
              }
          }
        }
      }
    }
  }
