import sys, os

def byte2int32(array, ptr):
    return int.from_bytes(array[ptr:ptr+4], byteorder="little")

amaFile = sys.argv[1]
output_dir = sys.argv[2]

with open(amaFile, "rb") as f:
    a = f.read()

G1C = byte2int32(a, 0x8)
G2C = byte2int32(a, 0xC)
G2P0 = byte2int32(a, 0x14)

name_pointer = byte2int32(a, 0x18)
if name_pointer == 0:
    name_pointer = byte2int32(a, 0x1C)

object_ptrs = []

for i in range(G2C):
    object_ptrs.append(byte2int32(a, G2P0 + i * 4))
object_ptrs.append(name_pointer)

for i in range(len(object_ptrs) - 1):
    obj_len = object_ptrs[i + 1] - object_ptrs[i]
    file_name = os.path.join(output_dir, str(obj_len), os.path.basename(amaFile) + "_" + str(hex(object_ptrs[i])))
    os.makedirs(os.path.dirname(file_name), exist_ok=True)
    with open(file_name, "wb") as f:
        f.write(a[object_ptrs[i]:object_ptrs[i + 1]])
