import sys, os

def byte2int32(array, ptr):
    return int.from_bytes(array[ptr:ptr+4], byteorder="little")

dir = sys.argv[1]

SHOW_VALUES = False
if len(sys.argv) > 2 and sys.argv[2] == "--show-values":
    SHOW_VALUES = True

files = [dir + "/" + x for x in os.listdir(dir) if not x.endswith("summary.txt")]

with open(files[0], "rb") as f:
    ama = f.read()

dif = [0] * int((len(ama)/4))

for i in range(len(dif)):
    dif[i] = set()

for fil in files:
    with open(fil, "rb") as f:
        ama = f.read()
        
    for i in range(len(dif)):
        dif[i].add(byte2int32(ama, i * 4))

summary = ""
summary += "="*15 + "\n"
summary += "    Summary    \n"
summary += "="*15 + "\n"
for i in range(len(dif)):
    summary += hex(i * 4) + " " * (len(hex(i * 4)) - 8) + ": "
    if len(dif[i]) == 1:
        summary += hex(byte2int32(ama, i*4))
    else:
        if SHOW_VALUES:
            summary += str([hex(x) for x in dif[i]])
        else:
            summary += str(len(dif[i])) + " different values"
    summary += "\n"
    
with open(dir + "/summary.txt", "w") as f:
    f.write(summary)
