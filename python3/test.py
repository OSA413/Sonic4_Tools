swap_end = False

import struct
import matplotlib.pyplot as plt

def byte2float(byte, swap_endianness=False):
    end = 1
    if swap_endianness: end = -1
    return struct.unpack('f', byte[::end])[0]

def byte2int(byte, swap_endianness=False):
    end = -1
    if swap_endianness: end = -1
    return struct.unpack('i', byte[::end])[0]

file_name = input("Enter file name>>> ")

#removing quotes and space from ubuntu's drag&drop in terminal
if file_name[0] == file_name[-2] == "\'" and file_name[-1] == " ":
    print("+")
    file_name = file_name[1:-2]

with open(file_name, "rb") as f:
    a = f.read()

c = []

for i in range(len(a)//4):
    b = byte2int(a[i*4:i*4+4], swap_end)
    if not b in c:
        c.append(b)
        plt.scatter(b, 0)
    else:
        print(b)

plt.show()
