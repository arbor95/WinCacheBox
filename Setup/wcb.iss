; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MinorRevisionBuild "1.332.0"
#define MyAppName "WinCacheBox"
#define MyAppVersion "1"
#define Company "Team CacheBox"
#define MyAppURL "http://team-cachebox.de"
#define CurrentYear GetDateTimeString('yyyy', '', '');

[CustomMessages]
german.language=de
english.language=en
german.DesktopIcon=Eine Verkn�pfung f�r WinCacheBox auf dem Desktop anlegen
english.DesktopIcon=Create a desktop icon for WinCacheBox

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{8972a890-4d63-401a-b3e6-f260dde4d8d6}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#Company}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#Company}\{#MyAppName}
DefaultGroupName={#Company}{code:getSeparator}{#MyAppName}
AllowNoIcons=yes
OutputDir=.\
OutputBaseFilename=WCB-{#MyAppVersion}.{#MinorRevisionBuild}
SetupIconFile=..\Icon1.ico
Compression=lzma
SolidCompression=yes
PrivilegesRequired=admin
DisableDirPage=auto
DisableProgramGroupPage=auto
UsePreviousSetupType=yes
UninstallDisplayIcon={app}\{#MyAppName}.exe
;CreateUninstallRegKey=no only if only a update
UpdateUninstallLogAppName=no
AppCopyright=Copyright (�) {#CurrentYear} {#Company}
VersionInfoVersion={#MyAppVersion}.{#MinorRevisionBuild}
;ArchitecturesInstallIn64BitMode=x64

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "german"; MessagesFile: "compiler:Languages\German.isl"

[Registry]
;Root: HKCU; Subkey: "Software\Ging-Buh\{#MyAppName}\"; ValueType: string; ValueName: "Language"; ValueData: "{cm:language}"

[Tasks]
Name: "DesktopIcon"; Description: "{cm:DesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}";

[InstallDelete]
Type: filesandordirs; Name: "{group}"

[Files]
Source: "..\bin\WinCachebox.exe"; DestDir: "{app}"; Flags: ignoreversion;
Source: "..\bin\Newtonsoft.Json.xml"; DestDir: "{app}"; Flags: ignoreversion;
Source: "..\bin\Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion;
Source: "..\bin\Microsoft.Rest.ClientRuntime.xml"; DestDir: "{app}"; Flags: ignoreversion;
Source: "..\bin\Microsoft.Rest.ClientRuntime.dll"; DestDir: "{app}"; Flags: ignoreversion;
Source: "..\bin\sqlceca35.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\sqlcecompact35.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\sqlceer35DE.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\sqlceer35EN.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\sqlceme35.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\sqlceoledb35.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\sqlceqp35.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\sqlcese35.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\System.Data.SQLite.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\System.Data.SqlServerCe.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\WeifenLuo.WinFormsUI.Docking.dll"; DestDir: "{app}"; Flags: ignoreversion
;
Source: "..\bin\WinCachebox.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\WinCachebox.pdb"; DestDir: "{app}"; Flags: ignoreversion
; Data Dir
Source: "..\bin\data\*"; DestDir: "{app}\data"; Flags: ignoreversion recursesubdirs createallsubdirs

[Registry]

[Icons]
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppName}.exe"; Tasks: DesktopIcon
Name: "{group}\Uninstall {#MyAppName}"; Filename: "{uninstallexe}"

[Run]

[UninstallRun]

[Code]
function getSeparator(Param: String): String;
var
  Version: TWindowsVersion;
begin
  GetWindowsVersionEx(Version);
  if (Version.Major > 9) then
    Result := ' '
  else
    Result := '\';
end;