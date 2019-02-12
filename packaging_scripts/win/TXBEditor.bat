::Your 7-Zip folder goes here

SET sevenZ="C:\Program Files\7-Zip"

::Current working directory
set cwd=%~dp0

cd /d %cwd%
cd ..
cd ..

::Creating empty directory
rmdir /s /q "dist/TXBEditor"
mkdir "dist/TXBEditor"

::Copying required files

SET dist="dist\TXBEditor\"

::OldModConversionTool
::License
COPY "LICENSE"									%dist%"LICENSE-Sonic4_Tools"
::EXEs
COPY "Sonic4_Tools\TXBEditor\bin\Release\TXBEditor.exe"	%dist%"TXBEditor.exe"

CD dist

::Archiving
%SevenZ%\7z a "TXBEditor.7z" "TXBEditor\*" -mx=9

exit