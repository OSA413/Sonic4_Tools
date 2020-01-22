AMA file structure:

Header:
0x00 - string (#AMA)
0x0C - uint (Number of files)
0x1C - uint (Pointer to pointers to file names)
0x20 - unit (number of meaningful values before name pointers)

Content:

0x34 - x possition (float)
0x38 - y size (float)
0x3C - x size (float)
0x40 - y size (float)
0xAC - UV x position 0 (float)
0xB0 - UV y position 0 (float)
0xB4 - UV x position 1 (float)
0xB8 - UV y position 1 (float)

(name pointers)
(names)
