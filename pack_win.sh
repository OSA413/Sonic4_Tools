#!/bin/bash
set -e
echo "Removing old distribution package..."
rm -rf "./dist"
mkdir -p "./dist/Sonic4Tools/licenses"

echo "Copying new distribution files..."

cp ./target/release/amb-rs.exe ./dist/Sonic4Tools/amb-rs.exe
cp ./target/release/txb2json.exe ./dist/Sonic4Tools/txb2json.exe
cp ./target/release/rg2json.exe ./dist/Sonic4Tools/rg2json.exe
cp ./target/release/dc2json.exe ./dist/Sonic4Tools/dc2json.exe
cp ./target/release/ev2json.exe ./dist/Sonic4Tools/ev2json.exe
cp ./target/release/md2json.exe ./dist/Sonic4Tools/md2json.exe
cp ./target/release/mp2json.exe ./dist/Sonic4Tools/mp2json.exe

cargo install -f copydeps cargo-bundle-licenses
cargo bundle-licenses --format json --output "./dist/Sonic4Tools/licenses/Rust-THIRDPARTY.json"

echo "Creating SHA256SUMS..."
cd dist
find * -type f -exec sha256sum {} \; >> "SHA256SUMS_win"

7z a "./Sonic4Tools_win.zip" ./* -mx=9