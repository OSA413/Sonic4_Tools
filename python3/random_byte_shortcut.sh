#1 is file
#2 is path to ambpatcher

cp "$1"".bkp" "$1" && python3 random_byte_randomizer.py "$1"
cd "$(dirname "$2")"
wine "$2" &> /dev/null
