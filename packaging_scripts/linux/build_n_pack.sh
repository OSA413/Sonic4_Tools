#!/bin/bash
cd "$(dirname "$0")"

bash ./build.sh
rm -r ./../../dist

for f in ./pack/*.sh; do
    echo
    echo "$f"
    echo "-----------------------------------------------------"
    bash "$f"
    echo "====================================================="
done
