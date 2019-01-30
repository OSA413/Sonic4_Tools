#!/bin/bash

NAME="OldModConversionTool"
DIST="./dist/"$NAME"/"

#Doing several dirnames to get out of the deep directory
BIN=$0
for ((i=0; i < 4; i++))
do
    BIN=$(dirname $BIN)"/"
done
DEPS=$BIN"dependencies/"
BIN=$BIN"Sonic4_Tools/"$NAME"/bin/Release/"

#Changing CWD
cd $(dirname $0)
cd ../../..

#Creating empty directory
rm -f -r "$DIST"
mkdir -p "$DIST"

echo $DIST
echo $BIN

#Copying files
cp "./LICENSE"      $DIST"LICENSE-Sonic4_Tools"
cp $BIN$NAME".exe"  $DIST$NAME".exe"

cp $DEPS"AMBPatcher/AMBPatcher.exe"             $DIST"AMBPatcher.exe"
cp $DEPS"AMBPatcher/LICENSE"                    $DIST"LICENSE-Sonic4ModLoader"

cp $DEPS"SonicAudioTools/CsbEditor.exe"         $DIST"CsbEditor.exe"
cp $DEPS"SonicAudioTools/SonicAudioLib.dll"     $DIST"SonicAudioLib.dll"
cp $DEPS"SonicAudioTools/LICENSE"               $DIST"LICENSE-SonicAudioTools"