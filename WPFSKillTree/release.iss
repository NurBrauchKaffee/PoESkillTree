; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define AppExeName AssemblyName + ".exe"
#define AppDataFolderName ProductName
#define DistDir ProjectDir + "\dist"
#define BuildOutputDir DistDir + "\PoESkillTree"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={#AppId}
AppMutex={#AppId}
AppName={#ProductName}
AppVersion={#ProductVersion}
AppVerName={#ProductName} {#ProductVersion}
AppPublisher={#AssemblyCompany}
AppPublisherURL={#ProductURL}
AppSupportURL={#ProductURL}
AppUpdatesURL={#ProductURL}
AppCopyright={#AssemblyCopyright}
DefaultDirName={pf}\{#ProductName}
DefaultGroupName={#ProductName}
UninstallDisplayName={#ProductName}
UninstallDisplayIcon={app}\{#AppExeName},0
;InfoBeforeFile="Release-Notes.txt"
LicenseFile={#BuildOutputDir}\LICENSE.txt
OutputDir={#DistDir}
OutputBaseFilename={#PackageName}-{#ProductVersion}
VersionInfoVersion={#FileVersion}
SetupIconFile={#ProjectDir}\logo.ico
Compression=lzma
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "czech"; MessagesFile: "compiler:Languages\Czech.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 0,6.1

[Dirs]
Name: "{userappdata}\{#AppId}"

[Files]
; Program Files
Source: "{#BuildOutputDir}\*.exe"; DestDir: "{app}"
Source: "{#BuildOutputDir}\*.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BuildOutputDir}\*.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BuildOutputDir}\*.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BuildOutputDir}\LICENSE.txt"; DestDir: "{app}"; Flags: ignoreversion
; Application Data
Source: "{#BuildOutputDir}\Data\*"; DestDir: "{userappdata}\{#AppDataFolderName}\Data"; Flags: ignoreversion recursesubdirs
Source: "{#BuildOutputDir}\Locale\*"; DestDir: "{userappdata}\{#AppDataFolderName}\Locale"; Flags: ignoreversion recursesubdirs
Source: "{#BuildOutputDir}\Items.xml"; DestDir: "{userappdata}\{#AppDataFolderName}"; Flags: ignoreversion

[Icons]
Name: "{group}\{#ProductName}"; Filename: "{app}\{#AppExeName}"
Name: "{group}\{cm:UninstallProgram,{#ProductName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#ProductName}"; Filename: "{app}\{#AppExeName}"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#ProductName}"; Filename: "{app}\{#AppExeName}"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\{#AppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(ProductName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
