PATH_TO_EP1 = #path to extracted content from Episode 1
PATH_TO_EP2 = #path to extracted content from Episode 2
FILE_EXTENSION = #file extension that will be checked
PATH_TO_EDITOR = #path to one of the editors
#possible flags:
#--strange-only  -  shows pointers of values of input file that may differ from specification
#--sanity-only   -  shows if converter can properly recreate input file
FLAG = "--sanity-only"

print(FILE_EXTENSION + " with " + FLAG)

import glob, subprocess

a = []

for i in [PATH_TO_EP1, PATH_TO_EP2]:
    a += glob.glob(i + "/**/*."+FILE_EXTENSION, recursive=True)
    a += glob.glob(i + "/**/*."+FILE_EXTENSION.lower(), recursive=True)

print("Total: " + str(len(a)))
ok = 0
length_errors = 0

for i in a:
    #replace mono with wine or remove it.
    b = str(subprocess.run(["mono", PATH_TO_EDITOR, "--check", FLAG, i], capture_output=True, text=True).stdout)
    #print(b, end="")
    if b.startswith("OK"):
        ok += 1
    elif b.startswith("0xFFFFFFFF "):
        #print(i)
        length_errors += 1
    else:
        #print(i)
        #print(b)
        pass
        
print("Has length problems: "+ str(length_errors) + " (" + str(round(100*length_errors/len(a), 2)) + "%)")
print("Properly editable: " + str(ok) + " (" + str(round(100*ok/len(a), 2)) + "%)")
