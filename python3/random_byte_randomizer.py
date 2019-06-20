import random, sys, os

if len(sys.argv) < 2:
    file_name = input("Enter file name>>> ")

    #removing quotes and space from ubuntu's drag&drop in terminal
    if file_name[0] == file_name[-2] == "\'" and file_name[-1] == " ":
        file_name = file_name[1:-2]
else:
    file_name = sys.argv[1]

#list of addresses that should not be changed
exception_address_list = []
if os.path.isfile(file_name + "_exceptions"):
    with open(file_name + "_exceptions") as f:
        exception_address_list = f.readlines()

with open(file_name, "rb") as f:
    a = f.read()

random_byte_address = random.randint(len(a))
random_byte_value = byte(random.randint(0, 255))

if len(exception_address_list) == len(a): print("too much exceptions"); exit()

while random_byte_address in exception_address_list:
    random_byte_address += 1
    if random_byte_address >= len(a): random_byte_address = 0

a = a[0:random_byte_address] + bytes(random_byte_value) + a[random_byte_address + 1, len(a)]

print(hex(random_byte_address))
print(random_byte_value)

with open(file_name, "wb") as f:
    f.write(a)

