; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "H6 Eetfestijn"
#define MyAppVersion "2.0.1"
#define MyAppPublisher "Nils Berghs"
#define MyAppExeName "EetfestijnUI-WPF.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{FBBAB7EB-BA91-4DC5-A7D1-65EF4FE5AC66}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\{#MyAppName}
DisableDirPage=yes
DisableProgramGroupPage=yes
; Uncomment the following line to run in non administrative install mode (install for current user only.)
;PrivilegesRequired=lowest
OutputBaseFilename=Eetfestijn-Setup
SetupIconFile=.\..\H6-icon.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "dutch"; MessagesFile: "compiler:Languages\Dutch.isl"

[Files]
Source: "..\bin\Release\net5.0-windows\publish\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion 
Source: "..\bin\Release\net5.0-windows\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"

