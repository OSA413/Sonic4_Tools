#!/bin/bash
set -e
cd "$(dirname "$0")"

rm -r "dependencies_source"
mkdir "dependencies_source"
cd "dependencies_source"

#AMBPatcher from Mod Loader
mkdir "Sonic4_ModLoader"
cd "Sonic4_ModLoader"
curl -L https://github.com/OSA413/Sonic4_ModLoader/releases/download/v0.1.6.2p1/AMBPatcher.exe > AMBPatcher.exe
curl https://raw.githubusercontent.com/OSA413/Sonic4_ModLoader/refs/tags/v0.1.6.2p1/LICENSE > LICENSE
cp ./AMBPatcher.exe ./../../dependencies/AMBPatcher/AMBPatcher.exe
cp ./LICENSE ./../../dependencies/AMBPatcher/LICENSE

#SonicAudioTools
mkdir "SonicAudioTools"
cd "SonicAudioTools"
url=$(curl -LIs -w %{url_effective} -o /dev/null https://github.com/blueskythlikesclouds/SonicAudioTools/releases/latest)
version=$(basename $url)
curl -L https://github.com/blueskythlikesclouds/SonicAudioTools/releases/download/$version/SonicAudioTools.7z > SonicAudioTools.7z
curl https://raw.githubusercontent.com/blueskythlikesclouds/SonicAudioTools/master/LICENSE.md > LICENSE.md
7z e SonicAudioTools.7z CsbEditor.exe SonicAudioLib.dll
ls
cp ./CsbEditor.exe ./../../dependencies/SonicAudioTools/CsbEditor.exe
cp ./SonicAudioLib.dll ./../../dependencies/SonicAudioTools/SonicAudioLib.dll
cp ./LICENSE.md ./../../dependencies/SonicAudioTools/LICENSE

cd ..