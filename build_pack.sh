#!/bin/bash
#please, keep all sh scripts in Unix new line (LF)
cd "$(dirname "$0")"

echo "Compiling..."
msbuild ./Sonic4_Tools/Sonic4_Tools.sln /p:Configuration=Release -m

EXIT_CODE="$?"
if [ "$EXIT_CODE" != "0" ]; then
    exit $EXIT_CODE
fi

echo "Removing old distribution package..."
rm -r "./dist"
mkdir -p "./dist/Sonic4_Tools/Common/Licenses"

echo "Copying new distribution files..."

#License
cp "LICENSE" "./dist/Sonic4_Tools/LICENSE"

#Main files
for file in $(ls ./packaging_scripts); do
    [ $file == "_common.sh" ] && continue
    
    bash ./packaging_scripts/$file
done

cp -r "./python3" "./dist/Sonic4_Tools/python3/"

#Dependencies
for dir in $(ls ./dependencies); do
    [ $dir == "readme.md" ] && continue

    #Files
    for file in $(cat ./dependencies/$dir/files); do
        cp "./dependencies/$dir/$file" "./dist/Sonic4_Tools/Common/$file"
    done
    
    #License
    for file in $(ls ./dependencies/$dir); do
        if [ $file == "LICENSE" ] || [ $file == "License.txt" ]; then
            cp "./dependencies/$dir/$file" "./dist/Sonic4_Tools/Common/Licenses/LICENSE-"$dir
            break
        fi
    done
done

echo "Creating SHA256SUMS..."
cd dist
find * -type f -exec sha256sum {} \; >> "SHA256SUMS"
cd ..

echo "Archiving..."
7z a "./dist/Sonic4_Tools.7z" ./dist/* -mx=9
