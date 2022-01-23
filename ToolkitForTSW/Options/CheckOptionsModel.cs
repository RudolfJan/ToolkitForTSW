using System.IO;
using Caliburn.Micro;

namespace ToolkitForTSW.Options
  {
  public class CheckOptionsModel: PropertyChangedBase
    {
    private bool _textEditorOK;
    public bool TextEditorOK
      {
      get
        {
        return _textEditorOK;
        }
      set
        {
        _textEditorOK= value;
        NotifyOfPropertyChange(()=>TextEditorOK);
        }
      }

    private bool _sevenZipOK;
    public bool SevenZipOK
      {
      get
        {
        return _sevenZipOK;
        }
      set
        {
        _sevenZipOK= value;
        NotifyOfPropertyChange(()=>SevenZipOK);
        }
      }

    private bool _unrealOK;
    public bool UnrealOK
      {
      get
        {
        return _unrealOK;
        }
      set
        {
        _unrealOK= value;
        NotifyOfPropertyChange(()=> UnrealOK);
        }
      }

    private bool _trackIROK;

    public bool TrackIROK
      {
      get
        {
        return _trackIROK;
        }
      set
        {
        _trackIROK= value;
        NotifyOfPropertyChange(()=>TrackIROK);
        }
      }
    private bool _backupFolderOK;

    public bool BackupFolderOK
      {
      get
        {
        return _backupFolderOK;
        
        }
      set
        {
        _backupFolderOK= value;
        NotifyOfPropertyChange(()=>BackupFolderOK);
        }
      }
    private bool _umodelOK;
    public bool UmodelOK
      {
      get
        {
        return _umodelOK;
        }
      set
        {
        _umodelOK= value;
        NotifyOfPropertyChange(()=> UmodelOK);
        }
      }

    private bool _steamFolderOK;
    public bool SteamFolderOK 
      { 
      get
        {
        return _steamFolderOK;
        }
      set
        {
        _steamFolderOK= value;
        NotifyOfPropertyChange(()=> SteamFolderOK);
        }
      }

    private bool _steamTSW2ProgramOK;
    public bool SteamTSW2ProgramOK { 
      get 
        {
        return _steamTSW2ProgramOK;
        }
      set
        {
        _steamTSW2ProgramOK= value;
        NotifyOfPropertyChange(()=>SteamTSW2ProgramOK);
        }
      }

    private bool _EGSTSW2ProgramOK;
    public bool EGSTSW2ProgramOK
      {
      get
        {
        return _EGSTSW2ProgramOK;
        
        }
      set
        {
        _EGSTSW2ProgramOK=value;
        NotifyOfPropertyChange(()=> EGSTSW2ProgramOK);
        }
      }

    private bool _toolkitFolderOK;
    public bool ToolkitFolderOK 
      { 
      get
        {
        return _toolkitFolderOK;
 
        }
      set
        {
        _toolkitFolderOK= value;
        NotifyOfPropertyChange(()=>ToolkitFolderOK);
        }
      }

    private bool _steamIdOK;
    public bool SteamIdOk 
      { 
      get
        {
        return _steamIdOK;
        
        }
      set
        {
        _steamIdOK= value;
        NotifyOfPropertyChange(()=>SteamIdOk);
        }
      }
    }
  }
