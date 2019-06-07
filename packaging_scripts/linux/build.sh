#!/bin/bash
cd "$(dirname "$0")"

msbuild ../../Sonic4_Tools/Sonic4_Tools.sln /p:Configuration=Release
