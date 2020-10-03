using System;
using System.Collections.Generic;
using System.Text;
using SavCrackerTest.Models;

namespace SavCracker.Library
  {
  public class SavScenarioLogic
    {

    public static void BuildSavScenario(SavScenarioModel savScenario, SavCrackerTest.SavCracker cracker)
      {
      foreach (var property in cracker.SavDataList)
        {
        switch (property.PropertyType)
          {
            case PropertyTypeEnum.StringProperty:
              {
              if (String.CompareOrdinal(property.PropertyName, "UserScenarioName") == 0)
                {
                savScenario.ScenarioName = PropertiesLogic.GetStrPropertyValue(property);
                }

              break;
              }
            case PropertyTypeEnum.StructProperty:
              {
              if (String.CompareOrdinal(property.PropertyName, "UserScenarioGuid") == 0)
                {
                savScenario.ScenarioGuid = PropertiesLogic.GetGuidPropertyValue(property);
                }

              break;
              }
            case PropertyTypeEnum.BoolProperty:
              {

              if (String.CompareOrdinal(property.PropertyName, "bRulesDisabledMode") == 0)
                {
                savScenario.RulesDisabledMode = PropertiesLogic.GetBoolPropertyValue(property);
                break;
                }

              if (String.CompareOrdinal(property.PropertyName, "bGlobalElectrificationMode") == 0)
                {
                savScenario.GlobalElectrificationMode =
                  PropertiesLogic.GetBoolPropertyValue(property);
                }

              break;
              }
            case PropertyTypeEnum.ArrayProperty:
              {
              var arrayProperty = property as ArrayPropertyModel;
              if (String.CompareOrdinal(property.PropertyName, "Services") == 0)
                {
                SavServiceLogic.BuildServices(savScenario, arrayProperty);
                }
              break;
              }
          }
        }

      }
    }
  }
