::Your 7-Zip folder goes here

SET sevenZ="C:\Program Files\7-Zip"

::Current working directory
set cwd=%~dp0

cd /d %cwd%
cd ..
cd ..

::Creating empty directory
rmdir /s /q "dist/OldModConversionTool"
mkdir "dist/OldModConversionTool"

::Copying required files

SET dist="dist\OldModConversionTool\"

::OldModConversionTool
::License
COPY "LICENSE"									%dist%"LICENSE-OldModConversionTool"
::EXEs
COPY "Sonic4_Tools\OldModConversionTool\bin\Release\OldModConversionTool.exe"	%dist%"OldModConversionTool.exe"

::AMBPatcher
::License
COPY "dependencies\AMBPatcher\LICENSE"			%dist%"LICENSE-AMBPatcher"
::EXEs
COPY "dependencies\AMBPatcher\AMBPatcher.exe"		%dist%"AMBPatcher.exe"

::SonicAudioTools
::License
COPY "dependencies\SonicAudioTools\LICENSE"		%dist%"LICENSE-SonicAudioTools"
::EXEs
COPY "dependencies\SonicAudioTools\SonicAudioLib.dll"	%dist%"SonicAudioLib.dll"
COPY "dependencies\SonicAudioTools\CsbEditor.exe"	%dist%"CsbEditor.exe"

CD dist

::Archiving
%SevenZ%\7z a "OldModConversionTool.7z" "OldModConversionTool\*" -mx=9

exit