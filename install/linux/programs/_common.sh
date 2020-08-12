#!/bin/bash
OUTPUT=~/.local/bin/"$(basename -s .exe "$1")"
PROGRAM_PATH=$(readlink -f "$1")
echo "mono $PROGRAM_PATH \"\$@\"" > $OUTPUT
chmod +x $OUTPUT
