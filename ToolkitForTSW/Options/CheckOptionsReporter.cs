using System;
using System.Collections.Generic;
using System.Text;

namespace ToolkitForTSW.Options
  {
  public class CheckOptionsReporter
    {
    public string OptionsCheckReport { get; set; }= string.Empty;
    public bool OptionsCheckStatus { get; set; } =false;
    public void BuildOptionsCheckReport()
      {
      OptionsCheckStatus=true;
      OptionsCheckReport=string.Empty;

      if(!CheckOptionsModel.BackupFolderOK)
        {
        OptionsCheckStatus = false;
        OptionsCheckReport += "";
        OptionsCheckReport += "Backup folder is not set properly\r\n";
        }
      if(!CheckOptionsModel.SevenZipOK)
        {
        OptionsCheckStatus = false;
        OptionsCheckReport += "SevenZip program is not set correctly\r\n";
        }

      if (!CheckOptionsModel.SteamFolderOK)
        {
        OptionsCheckStatus = false;
        OptionsCheckReport += "Steam folder is not set correctly\r\n";
        }

      if (!CheckOptionsModel.SteamIdOk)
        {
        OptionsCheckStatus = false;
        OptionsCheckReport += "SteamId is not set correctly\r\n";
        }

      if (!CheckOptionsModel.TextEditorOK)
        {
        OptionsCheckStatus = false;
        OptionsCheckReport += "Text Editor is not set correctly\r\n";
        }

      if (!CheckOptionsModel.ToolkitFolderOK)
        {
        OptionsCheckStatus = false;
        OptionsCheckReport += "ToolkitForTSW data folder not set correctly\r\n";
        }

      if (!CheckOptionsModel.SteamTSW2ProgramOK || !CheckOptionsModel.EGSTSW2ProgramOK)
        {
        OptionsCheckStatus = false;
        OptionsCheckReport += "TSW Program location is not set correctly\r\n";
        }

      if (!CheckOptionsModel.UmodelOK)
        {
        OptionsCheckStatus = false;
        OptionsCheckReport += "UAsset unpacker program not set correctly\r\n";
        }

      if (!CheckOptionsModel.UnrealOK)
        {
        OptionsCheckStatus = false;
        OptionsCheckReport += "Unreal unpacker not set correctly\r\n";
        }

      if (!CheckOptionsModel.TrackIROK)
        {
        OptionsCheckStatus = false;
        OptionsCheckReport += "TrackIR program not set correctly\r\n";
        }
      }
    }
  }