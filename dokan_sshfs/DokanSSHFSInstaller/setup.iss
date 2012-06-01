; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "DokanSSHFS"
#define MyAppVersion "1.1"
#define MyAppExeName "DokanSSHFS.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{4DACE2AE-5182-44C1-A307-EE3BDDC33BD9}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
OutputBaseFilename=sshfs_service_setup
Compression=lzma
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 0,6.1

[Files]
Source: "..\DokanSSHFS\bin\x86\Release\DokanSSHFS.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\DokanSSHFS\bin\x86\Release\DiffieHellman.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\DokanSSHFS\bin\x86\Release\DokanNet.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\DokanSSHFS\bin\x86\Release\Org.Mentalis.Security.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\DokanSSHFS\bin\x86\Release\Tamir.SharpSSH.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\DokanSSHFS\notify3.ico"; DestDir: "{app}\"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Dirs]
Name: "{app}\"; Permissions: everyone-modify

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; IconFilename: "{app}\notify3.ico"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon; IconFilename: "{app}\notify3.ico"
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: quicklaunchicon; IconFilename: "{app}\notify3.ico"

[Run]
Filename: "{app}\{#MyAppExeName}"; Parameters: "--installservice"
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[UninstallRun]
Filename: "{app}\{#MyAppExeName}"; Parameters: "--uninstallservice"