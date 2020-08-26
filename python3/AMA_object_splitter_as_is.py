import sys, os

def byte2int32(array, ptr):
    return int.from_bytes(array[ptr:ptr+4], byteorder="little")

amaFile = sys.argv[1]
output_dir = sys.argv[2]

with open(amaFile, "rb") as f:
    a = f.read()

G1C = byte2int32(a, 0x8)
print(G1C)
G2C = byte2int32(a, 0xC)
G1P0 = byte2int32(a, 0x10)
G2P0 = byte2int32(a, 0x14)

name_pointer = byte2int32(a, 0x18) #G1NP
if name_pointer == 0:
    name_pointer = byte2int32(a, 0x1C) #G2NP

object_ptrs = {"G1": [], "G2": []}

for i in range(G1C):
    object_ptrs["G1"].append(byte2int32(a, G1P0 + i * 4))
object_ptrs["G1"].append(G2P0)

for i in range(G2C):
    object_ptrs["G2"].append(byte2int32(a, G2P0 + i * 4))
object_ptrs["G2"].append(name_pointer)

for k in object_ptrs.keys():
    for i in range(len(object_ptrs[k]) - 1):
        obj_len = object_ptrs[k][i + 1] - object_ptrs[k][i]
        file_name = os.path.join(output_dir, k, str(obj_len), os.path.basename(amaFile) + "_" + str(hex(object_ptrs[k][i])))
        os.makedirs(os.path.dirname(file_name), exist_ok=True)
        with open(file_name, "wb") as f:
            f.write(a[object_ptrs[k][i]:object_ptrs[k][i + 1]])
