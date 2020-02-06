# AMA file structure

## Header

.    | 0-3  | 4-7  | 8-B  | C-F
---- | ---- | ---- | ---- | ----
0x00 | #AMA | 0x00 | G1C  | G2C
0x10 |   ?  |   ?  | G1NP | G2NP

G1C - Number of object in group 1
G2C - Number of object in group 2
G1NP - Pointer to list of pointers of object names in group 1.
G2NP - Pointer to list of pointers of object names in group 2.

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
