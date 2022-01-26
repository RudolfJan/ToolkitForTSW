; Script generated by the Inno Script Studio Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "ToolkitForTSW"
#define MyAppVersion "1.1"
#define MyAppPublisher "Holland Hiking"
#define MyAppURL "http://www.hollandhiking.nl/trainsimulator"
#define MyAppExeName "ToolkitForTSW.exe"
#define DataDirName= "{code:GetDataDir}"
#define DefaultDirName="{code:GetInstallDir}"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{8CF54723-2F7F-43AA-807F-15C3CA1919A3}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
LicenseFile=Inputfiles\NoCopy\License.txt
InfoBeforeFile=Inputfiles\NoCopy\Readme.txt
OutputDir=Output
OutputBaseFilename=ToolkitForTSWSetup
Compression=lzma
SolidCompression=yes
WizardImageFile=Inputfiles\NoCopy\Setup.bmp
WizardImageBackColor=clInfoBk
WizardImageStretch=False
AppCopyright=2017/2021 Rudolf Heijink
DisableWelcomePage=no    
DisableDirPage=no

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 0,6.1

[Files]
Source: "Inputfiles\Binaries\*.exe"; DestDir: "{#DefaultDirName}"; Flags: ignoreversion
Source: "Inputfiles\Binaries\*.dll"; DestDir: "{#DefaultDirName}"; Flags: ignoreversion
Source: "Inputfiles\Binaries\*.json"; DestDir: "{#DefaultDirName}"; Flags: ignoreversion
Source: "Inputfiles\Binaries\runtimes\*.*"; DestDir: "{#DefaultDirName}\runtimes"; Flags: ignoreversion recursesubdirs
Source: "Inputfiles\Binaries\Images\*.*"; DestDir: "{#DefaultDirName}\Images"; Flags: ignoreversion
Source: "Inputfiles\Binaries\SQL\*.*"; DestDir: "{#DefaultDirName}\SQL"; Flags: ignoreversion
Source: "Inputfiles\Manuals\*.pdf"; DestDir: "{#DefaultDirName}\Manuals"; Flags: ignoreversion
Source: "Inputfiles\NoCopy\CSXicon.bmp"; DestDir: "{#DefaultDirName}"; Flags: ignoreversion

; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{#DefaultDirName}\{#MyAppExeName}"
Name: "{group}\{cm:ProgramOnTheWeb,{#MyAppName}}"; Filename: "{#MyAppURL}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{#DefaultDirName}\{#MyAppExeName}"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppName}"; Filename: "{#DefaultDirName}\{#MyAppExeName}"; Tasks: quicklaunchicon

[Code]
var
   DataDirPage: TInputDirWizardPage;
   DataDir: String;
   InstallDir : string;

function GetInstallDir(def: string): string;

begin
  Result := 'C:\Program Files (x86)\ToolkitForTSW';
  if RegQueryStringValue(HKEY_CURRENT_USER, 'Software\Holland Hiking\ToolkitForTSW', 'InstallDirectory', InstallDir) then
  begin
    // Successfully read the value.
    Result := InstallDir;
  end;
end; 

function InitDataDirValue(): String;

begin
DataDir :='';
if RegQueryStringValue(HKEY_CURRENT_USER, 'Software\Holland Hiking\ToolkitForTSW', 'TSWToolsFolder', DataDir) then
  begin
    // Successfully read the value.
    Log('Datadir found in registry' );
    Result := DataDir;
  end
  else
  begin
  Log('Datadir NOT found in registry' );
  Result := '';
   end;
  end;

procedure InitializeWizard;
begin
  { Create the pages }
  DataDirPage := CreateInputDirPage(wpSelectDir,
    'Select ToolkitForTSW Data Directory', 'Select where ToolkitForTSW must store data files.'+#13+'NOTE: the installer will NOT move files from an existing directory to the directory you selected',
    'Select the folder in which Setup should install ToolkitForTSW data files, then click Next.',
    False, 'ToolkitForTSWData');
  DataDirPage.Add('');

  { Set default values, using settings that were stored last time if possible }
   DataDirPage.Values[0] := InitDataDirValue();
end;

function ShouldSkipPage(PageID: Integer): Boolean;
begin
  { Skip pages that shouldn't be shown }
    Result := False;
end;

function NextButtonClick(CurPageID: Integer): Boolean;
var
  I: Integer;
begin
  { Validate certain pages before allowing the user to proceed }
  if CurPageID = DataDirPage.ID then begin
     if DataDirPage.Values[0] = '' then
        DataDirPage.Values[0] := 'C:\ToolkitForTSW';
      Result := True;
    end;
     Result := True;
end;

function UpdateReadyMemo(Space, NewLine, MemoUserInfoInfo, MemoDirInfo, MemoTypeInfo,
  MemoComponentsInfo, MemoGroupInfo, MemoTasksInfo: String): String;
var
  S: String;
begin
  { Fill the 'Ready Memo' with the normal settings and the custom settings }
  S := 'Summary:' + NewLine;

  S := S + Space + ' ToolkitForTSW installation folder: '+ ExpandConstant('{app}') + NewLine;
  S := S + Space + ' ToolkitForTSW data files folder  : '+ DataDirPage.Values[0] + NewLine;

  Result := S;
end;

function GetDataDir(Param: String): String;
begin
  { Return the selected DataDir }
  Result := DataDirPage.Values[0];
end;



[Run]
Filename: "{#DefaultDirName}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Registry]
Root: "HKCU"; Subkey: "software\Holland Hiking\ToolkitForTSW"; ValueType: string; ValueName: "InstallDirectory"; ValueData: "{app}\"; Flags: createvalueifdoesntexist
Root: "HKCU"; Subkey: "software\Holland Hiking\ToolkitForTSW"; ValueType: string; ValueName: "TSWToolsFolder"; ValueData: "{#DataDirName}\"; Flags: createvalueifdoesntexist