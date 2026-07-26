#!/bin/bash
set -e
echo "Removing old distribution package..."
rm -rf "./dist"
mkdir -p "./dist/Sonic4Tools"

echo "Copying new distribution files..."

cp ./target/release/amb-rs ./dist/Sonic4Tools/amb-rs
cp ./target/release/txb2json ./dist/Sonic4Tools/txb2json
cp ./target/release/rg2json ./dist/Sonic4Tools/rg2json

echo "Creating SHA256SUMS..."
cd dist
find * -type f -exec sha256sum {} \; >> "SHA256SUMS_linux"

7z a "./Sonic4Tools_linux.zip" ./* -mx=9