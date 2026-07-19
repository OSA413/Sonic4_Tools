#!/bin/bash
set -e
echo "Removing old distribution package..."
rm -rf "./dist"
mkdir -p "./dist/Sonic4Tools"

echo "Copying new distribution files..."

cp ./target/release/amb-rs.exe ./dist/Sonic4Tools/amb-rs.exe

echo "Creating SHA256SUMS..."
cd dist
find * -type f -exec sha256sum {} \; >> "SHA256SUMS_win"

7z a "./Sonic4ModLoader_win.zip" ./* -mx=9