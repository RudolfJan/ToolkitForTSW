using SavCrackerTest.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SavCracker.Library
  {
  public class RouteLogic
    {
    public static string StripRouteString(string s)
      {
      var idx = s.IndexOf("_Route_Gameplay", StringComparison.Ordinal);
      if (idx > 0)
        {
        s = s.Substring(1, idx - 1);
        }
      else
        {
        s = s.Substring(1);
        idx = s.IndexOf("/", StringComparison.Ordinal);
        if (idx > 0)
          {
          s = s.Substring(0, idx);
          }
        }

      return s;
      }

    public static void HarmonizeRouteName(SavScenarioModel scenario, string inputName)
      {
      switch (inputName)
        {
        case "SanFranSanJose":
            {
            scenario.RouteName = "Peninsula Corridor";
            scenario.RouteAbbreviation = "SFJ";
            break;
            }
        case "SandPatchGrade":
            {
            scenario.RouteName = "Sandpatch Grade";
            scenario.RouteAbbreviation = "SPG";
            break;
            }
        case "Bakerloo":
            {
            scenario.RouteName = "Bakerloo Line";
            scenario.RouteAbbreviation = "BKL";
            break;
            }
        case "DSN":
            {
            scenario.RouteName = "Tees Valley Line";
            scenario.RouteAbbreviation = "TVL";
            break;
            }
        case "BrightonEastbourne":
            {
            scenario.RouteName = "East Coast Way";
            scenario.RouteAbbreviation = "ECW";
            break;
            }
        case "LIRR":
            {
            scenario.RouteName = "Long Island Railroad";
            scenario.RouteAbbreviation = "LIRR";
            break;
            }
        case "RuhrSiegNord":
            {
            scenario.RouteName = "Ruhr-Sieg Nord";
            scenario.RouteAbbreviation = "RSN";
            break;
            }
        case "KolnAachen":
            {
            scenario.RouteName = "Schnellstrecke Köln-Aachen";
            scenario.RouteAbbreviation = "SKA";
            break;
            }
        case "DuisburgBochum":
            {
            scenario.RouteName = "Hauptstrecke Rhein-Ruhr";
            scenario.RouteAbbreviation = "HRR";
            break;
            }
        case "Leipzig_S2_S-Bahn":
            {
            scenario.RouteName = "Rapid transit";
            scenario.RouteAbbreviation = "RT";
            break;
            }
        case "MainSpessartBahn":
            {
            scenario.RouteName = "MainSpessartBahn";
            scenario.RouteAbbreviation = "MSB";
            break;
            }

        default:
            {
            scenario.RouteName = inputName;
            scenario.RouteAbbreviation = "???";
            break;
            }
        }
      }
    }
  }
