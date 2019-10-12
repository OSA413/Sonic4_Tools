import sys

def byte2int(byte):
    return int.from_bytes(byte, byteorder="little")

file_name = ""
if (len(sys.argv) == 1):
    file_name = input(">>> ")
else:
    file_name = sys.argv[1]

print("Opening the file...")
with open(file_name, "rb") as f:
    a = f.read()

split_ind = -1

print("Searching AMB files...")
for i in range(len(a) - 4):
    if a[i] == 0x23 \
    and a[i+1] == 0x41 \
    and a[i+2] == 0x4d \
    and a[i+3] == 0x42:
        split_ind = i
        end_index = i + byte2int(a[split_ind + 0x1c : split_ind + 0x20]) + byte2int(a[split_ind + 0x10 : split_ind + 0x14]) * 0x20

    if split_ind >= 0 and end_index < len(a)/4:
        with open(".".join(file_name.split(".")[:-1]) + "_" + str(split_ind) + ".AMB", "wb") as f:
            f.write(a[split_ind:end_index])
        split_ind = -1

print("Done!")
input()
