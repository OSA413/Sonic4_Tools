import random, sys

if len(sys.args) < 2:
    file_name = input("Enter file name>>> ")

    #removing quotes and space from ubuntu's drag&drop in terminal
    if file_name[0] == file_name[-2] == "\'" and file_name[-1] == " ":
        file_name = file_name[1:-2]
else:
    file_name = sys.args[1]

with open(file_name, "rb") as f:
    a = f.read()

random_byte_address = random.randint(len(a))
random_byte_value = byte(random.randint(0, 255))

a = a[0:random_byte_address] + bytes(random_byte_value) + a[random_byte_address + 1, len(a)]

with open(file_name, "wb") as f:
    f.write(a)

