#!/bin/bash
set -e
echo "Removing old distribution package..."
rm -rf "./dist"
mkdir -p "./dist/Sonic4Tools/licenses"

echo "Copying new distribution files..."

cp ./target/release/amb-rs ./dist/Sonic4Tools/amb-rs
cp ./target/release/txb2json ./dist/Sonic4Tools/txb2json
cp ./target/release/rg2json ./dist/Sonic4Tools/rg2json
cp ./target/release/dc2json ./dist/Sonic4Tools/dc2json
cp ./target/release/ev2json ./dist/Sonic4Tools/ev2json
cp ./target/release/md2json ./dist/Sonic4Tools/md2json
cp ./target/release/mp2json ./dist/Sonic4Tools/mp2json

cargo install -f copydeps cargo-bundle-licenses
cargo bundle-licenses --format json --output "./dist/Sonic4Tools/licenses/Rust-THIRDPARTY.json"

echo "Creating SHA256SUMS..."
cd dist
find * -type f -exec sha256sum {} \; >> "SHA256SUMS_linux"

7z a "./Sonic4Tools_linux.zip" ./* -mx=9