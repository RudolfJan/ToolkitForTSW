using Logging.Library;
using System.IO;

namespace ToolkitForTSW
  {
  public class IntroMovies
    {
    public static void DeleteAllIntroMovies(bool makeBackupFirst = true)
      {
      string programFolder;
      if (TSWOptions.CurrentPlatform == PlatformEnum.Steam)
        {
        programFolder = TSWOptions.SteamTrainSimWorldDirectory;
        }
      else
        {
        programFolder = TSWOptions.EGSTrainSimWorldDirectory;
        }
      string path = $"{programFolder}TS2Prototype\\Content\\Movies";
      string backupPath = $"{TSWOptions.BackupFolder}Movies\\";
      DirectoryInfo dir = new DirectoryInfo(path);
      var files = dir.GetFiles();
      foreach (var file in files)
        {
        if (makeBackupFirst)
          {
          File.Copy(file.FullName, $"{backupPath}{file.Name}", true);
          }
        Utilities.Library.FileHelpers.DeleteSingleFile(file.FullName);
        }
      Log.Trace("Deleted intro movies", LogEventType.InformUser);
      }
    }
  }
