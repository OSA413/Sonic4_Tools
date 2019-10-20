#!/bin/bash
#please, keep all sh scripts in Unix new line (LF)
cd "$(dirname "$0")"

rm -r "dependencies_source"
mkdir "dependencies_source"
cd "dependencies_source"

#AMBPatcher from Mod Loader
git clone --depth=1 https://github.com/OSA413/Sonic4_ModLoader
cd "Sonic4_ModLoader"
bash build_pack_test.sh

cp ./dist/Sonic4ModLoader/AMBPatcher.exe ./../../dependencies/AMBPatcher/AMBPatcher.exe
cp ./LICENSE ./../../dependencies/AMBPatcher/LICENSE

cd ..

#SonicAudioTools
git clone --depth=1 https://github.com/blueskythlikesclouds/SonicAudioTools
cd "SonicAudioTools"
nuget restore SonicAudioTools.sln
msbuild SonicAudioTools.sln /p:Configuration=Release

cp ./Release/CsbEditor.exe ./../../dependencies/SonicAudioTools/CsbEditor.exe
cp ./Release/SonicAudioLib.dll ./../../dependencies/SonicAudioTools/SonicAudioLib.dll
cp ./LICENSE.md ./../../dependencies/SonicAudioTools/LICENSE