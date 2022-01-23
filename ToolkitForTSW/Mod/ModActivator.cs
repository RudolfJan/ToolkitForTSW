using Logging.Library;
using System;
using System.IO;
using ToolkitForTSW.Mod.ViewModels;
using ToolkitForTSW.Models;
using Utilities.Library;

namespace ToolkitForTSW.Mod
  {
  internal class ModActivator
    {
    public static void ActivateMod(ModModel mod, PlatformEnum selectedPlatform, bool overWrite)
      {
      var PakPath = mod.FilePath;
      var source = TSWOptions.ModsFolder + PakPath;
      var fileName = Path.GetFileName(source);
      var destination = $"{ModManagerViewModel.GetDLCBaseDir(selectedPlatform).FullName}\\{fileName}";

      try
        {
        if (!(File.Exists(destination) || overWrite))
          {
          File.Copy(source, destination, overWrite);
          }
        }
      catch (Exception E)
        {
        Log.Trace("Failed to install mod pak because " + E.Message,
          LogEventType.Error);
        }
      }


    public static void DeactivateMod(ModModel selectedMod, PlatformEnum selectedPlatform)
      {
      var filePath = $"{ModManagerViewModel.GetDLCBaseDir(selectedPlatform).FullName}\\{selectedMod.FileName}";
      FileHelpers.DeleteSingleFile(filePath);
      }
    }
  }
