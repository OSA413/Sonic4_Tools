import glob

user_input = input("Enter format>>> ")

a = glob.glob("./**/*."+user_input, recursive = True)

min_size = -1
min_size_name = "Not found"

for i in a:
    with open(i, "rb") as f:
        b = len(f.read())
        
        if min_size == -1: min_size = b + 1
        
        if b < min_size:
            min_size = b
            min_size_name = i
            
print(min_size_name)
