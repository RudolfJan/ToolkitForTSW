using System.IO;

namespace ToolkitForTSW.Options
  {
  public class CheckOptionsModel
    {
    public static bool TextEditorOK
      {
      get
        {
        return !string.IsNullOrEmpty(TSWOptions.TextEditor) 
          && File.Exists(TSWOptions.TextEditor);
        }
      }

    public static bool SevenZipOK
      {
      get
        {
        return !string.IsNullOrEmpty(TSWOptions.SevenZip) 
          && File.Exists(TSWOptions.SevenZip) 
          && TSWOptions.SevenZip.EndsWith("7z.exe");
        }
      }

    public static bool UnrealOK
      {
      get
        {
        return !string.IsNullOrEmpty(TSWOptions.Unpacker) 
          && File.Exists(TSWOptions.Unpacker);
        }
      }

    public static bool TrackIROK
      {
      get
        {
        // may be either non-existent, but must be valid if it exists
        return string.IsNullOrEmpty(TSWOptions.TrackIRProgram)
          || File.Exists(TSWOptions.TrackIRProgram);
        }
      }

    public static bool BackupFolderOK
      {
      get
        {
        return !string.IsNullOrEmpty(TSWOptions.BackupFolder) 
          && Directory.Exists(TSWOptions.BackupFolder);
        }
      }
    public static bool UmodelOK
      {
      get
        {
        return !string.IsNullOrEmpty(TSWOptions.UAssetUnpacker) 
          && File.Exists(TSWOptions.UAssetUnpacker) 
          && TSWOptions.UAssetUnpacker.EndsWith("exe");
        }
      }

    public static bool SteamFolderOK 
      { 
      get
        {

        return !string.IsNullOrEmpty(TSWOptions.SteamProgramDirectory) 
          && Directory.Exists(TSWOptions.SteamProgramDirectory)
          && File.Exists(TSWOptions.SteamProgramDirectory+"steam.exe");
        }
      }

    public static bool SteamTSW2ProgramOK { 
      get 
        {
        return !string.IsNullOrEmpty(TSWOptions.SteamTrainSimWorldDirectory) 
          && File.Exists(TSWOptions.SteamTrainSimWorldDirectory+"TS2prototype.exe"); 
        }
      }

    public static bool EGSTSW2ProgramOK
      {
      get
        {
        return !string.IsNullOrEmpty(TSWOptions.EGSTrainSimWorldDirectory)
          && File.Exists(TSWOptions.EGSTrainSimWorldDirectory + "TS2prototype.exe");
        }
      }

    public static  bool ToolkitFolderOK 
      { 
      get
        {
        return !string.IsNullOrEmpty(TSWOptions.ToolkitForTSWFolder) 
          && Directory.Exists(TSWOptions.ToolkitForTSWFolder);
        }
      }

    public static bool SteamIdOk 
      { 
      get
        {
        return !string.IsNullOrEmpty(TSWOptions.SteamUserId) 
          &&!string.IsNullOrEmpty(TSWOptions.SavedSteamScreenshots) 
          && Directory.Exists(TSWOptions.SavedSteamScreenshots);
        }
      }
    }
  }
