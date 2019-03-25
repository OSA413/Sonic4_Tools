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

with open(r"D:\Steam\steamapps\common\Sonic the Hedgehog 4 EP 1\mods\tes\G_ZONE1\MAP\ZONE11_MAP.AMB\OUT\EL\ZONE11.EV", "rb") as f:
    a = f.read()

c = []

for i in range(len(a)//4):
    b = byte2int(a[i*4:i*4+4],True)
    if not b in c:
        c.append(b)
        plt.scatter(b, 0)
    else:
        print(b)

plt.show()
