#!/bin/bash
set -e
echo "Removing old distribution package..."
rm -rf "./dist"

echo "Copying new distribution files..."

cp ./target/release/amb-rs ./dist/amb-rs

echo "Creating SHA256SUMS..."
cd dist
find * -type f -exec sha256sum {} \; >> "SHA256SUMS_linux"

7z a "./Sonic4ModLoader_linux.zip" ./* -mx=9