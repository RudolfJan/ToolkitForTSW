using System;
using System.IO;

namespace ToolkitForTSW.Options
  {
  public class CheckOptionsLogic
    {
    #region static
    // Make this class a singleton
    // static holder for instance, need to use lambda to construct since constructor private
    private static readonly Lazy<CheckOptionsLogic> _instance
      = new Lazy<CheckOptionsLogic>(() => new CheckOptionsLogic());

    // private to prevent direct instantiation.

    private CheckOptionsLogic()
      {

      }

    // accessor for instance

    public static CheckOptionsLogic Instance
      {
      get
        {
        return _instance.Value;
        }
      }
    #endregion

    public CheckOptionsModel Check { get; set; } = new CheckOptionsModel();

    public void SetAllOptionChecks()
      {
      SetTextEditorOK();
      SetSevenZipOK();
      SetUnrealOK();
      SetTrackIROK();
      SetBackupFolderOK();
      SetUmodelOK();
      SetSteamFolderOK();
      SetSteamTSW2ProgramOK();
      SetEGSTSW2ProgramOK();
      SetToolkitFolderOK();
      SetSteamIdOK();
      }

    public void SetTextEditorOK()
      {
      Check.TextEditorOK = !string.IsNullOrEmpty(TSWOptions.TextEditor)
          && File.Exists(TSWOptions.TextEditor)
          && TSWOptions.TextEditor.EndsWith(".exe");
      ;
      }

    public void SetSevenZipOK()
      {
      Check.SevenZipOK = !string.IsNullOrEmpty(TSWOptions.SevenZip)
          && File.Exists(TSWOptions.SevenZip)
          && TSWOptions.SevenZip.EndsWith("7z.exe");
      }

    public void SetUnrealOK()
      {
      Check.UnrealOK = !string.IsNullOrEmpty(TSWOptions.Unpacker)
          && File.Exists(TSWOptions.Unpacker)
          && TSWOptions.Unpacker.EndsWith("UnrealPak.exe");
      ;
      }

    public void SetTrackIROK()
      {
      // may be either non-existent, but must be valid if it exists
      Check.TrackIROK = string.IsNullOrEmpty(TSWOptions.TrackIRProgram)
        || File.Exists(TSWOptions.TrackIRProgram);
      }

    public void SetBackupFolderOK()
      {
      Check.BackupFolderOK = !string.IsNullOrEmpty(TSWOptions.BackupFolder)
          && Directory.Exists(TSWOptions.BackupFolder);
      }

    public void SetUmodelOK()
      {
      Check.UmodelOK = !string.IsNullOrEmpty(TSWOptions.UAssetUnpacker)
          && File.Exists(TSWOptions.UAssetUnpacker)
          && TSWOptions.UAssetUnpacker.EndsWith("umodel.exe");
      }

    public void SetSteamFolderOK()
      {
      Check.SteamFolderOK = !string.IsNullOrEmpty(TSWOptions.SteamProgramDirectory)
          && Directory.Exists(TSWOptions.SteamProgramDirectory)
          && File.Exists(TSWOptions.SteamProgramDirectory + "steam.exe");
      }

    public void SetSteamTSW2ProgramOK()
      {
      Check.SteamTSW2ProgramOK = !string.IsNullOrEmpty(TSWOptions.SteamTrainSimWorldDirectory)
          && File.Exists(TSWOptions.SteamTrainSimWorldDirectory + "TS2prototype.exe");
      }

    public void SetEGSTSW2ProgramOK()
      {
      Check.EGSTSW2ProgramOK = string.IsNullOrEmpty(TSWOptions.EGSTrainSimWorldDirectory)
          || (!string.IsNullOrEmpty(TSWOptions.EGSTrainSimWorldDirectory) && File.Exists(TSWOptions.EGSTrainSimWorldDirectory + "TS2prototype.exe"));
      }

    public void SetToolkitFolderOK()
      {
      Check.ToolkitFolderOK = !string.IsNullOrEmpty(TSWOptions.ToolkitForTSWFolder)
          && Directory.Exists(TSWOptions.ToolkitForTSWFolder);
      }

    public void SetSteamIdOK()
      {
      Check.SteamIdOk = !string.IsNullOrEmpty(TSWOptions.SteamUserId)
          && !string.IsNullOrEmpty(TSWOptions.SavedSteamScreenshots)
          && Directory.Exists(TSWOptions.SavedSteamScreenshots);
      }
    }
  }
