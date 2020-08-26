import sys, os

def byte2int32(array, ptr):
    return int.from_bytes(array[ptr:ptr+4], byteorder="little")
    
def get_first_greater_than(array, num):
    for i in array:
        if i > num:
            return i
    return 0

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

object_ptrs = {"G1": [], "G2": [], "G2-0": [], "G2-1": [], "G2-2": [], "G2-3": [], "G2-4": [], "G2-5":[]}
all_ptrs = [name_pointer]

for i in range(G1C):
    object_ptrs["G1"].append(byte2int32(a, G1P0 + i * 4))

for i in range(G2C):
    object_ptrs["G2"].append(byte2int32(a, G2P0 + i * 4))
    
for i in range(len(object_ptrs["G2"])):
    G20_ptr = byte2int32(a, object_ptrs["G2"][i] + 0x20)
    if G20_ptr != 0:
        object_ptrs["G2-0"].append(G20_ptr)
    G21_ptr = byte2int32(a, object_ptrs["G2"][i] + 0x24)
    if G21_ptr != 0:
        object_ptrs["G2-1"].append(G21_ptr)
    G22_ptr = byte2int32(a, object_ptrs["G2"][i] + 0x28)
    if G22_ptr != 0:
        object_ptrs["G2-2"].append(G22_ptr)
    G23_ptr = byte2int32(a, object_ptrs["G2"][i] + 0x2C)
    if G23_ptr != 0:
        object_ptrs["G2-3"].append(G23_ptr)
    G24_ptr = byte2int32(a, object_ptrs["G2"][i] + 0x30)
    if G24_ptr != 0:
        object_ptrs["G2-4"].append(G24_ptr)
    G25_ptr = byte2int32(a, object_ptrs["G2"][i] + 0x34)
    if G25_ptr != 0:
        object_ptrs["G2-5"].append(G25_ptr)
        
for k in object_ptrs.keys():
    for i in range(len(object_ptrs[k])):
        all_ptrs.append(object_ptrs[k][i])
all_ptrs.sort()

for k in object_ptrs.keys():
    for i in range(len(object_ptrs[k])):
        obj_len = 0
        if k == "G1":
            obj_len = 0x20
        if k == "G2":
            obj_len = 0x40
            
        if obj_len == 0:
            obj_len = get_first_greater_than(all_ptrs, object_ptrs[k][i]) - object_ptrs[k][i]

        file_name = os.path.join(output_dir, k, str(obj_len), os.path.basename(amaFile) + "_" + str(hex(object_ptrs[k][i])))
        os.makedirs(os.path.dirname(file_name), exist_ok=True)
        with open(file_name, "wb") as f:
            f.write(a[object_ptrs[k][i]:object_ptrs[k][i] + obj_len])
