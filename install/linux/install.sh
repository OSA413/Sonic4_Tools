#!/bin/bash
echo "This script will create links to the programs"
echo "that are located in the /dist directory."
echo "They will be accessable for the current user"
echo "via console by typing the program name"
echo "e.g. \"AMBPather\" or \"TXBEditor\" without extensions."

cd "$(dirname "$0")"/programs

for file in $(ls .); do
    [ $file == "_common.sh" ] && continue
    bash $file
done

echo ""
echo "Done! You should be able to use it right now."
