# AMA file structure

## Header

.    | 0-3  | 4-7  | 8-B  | C-F
---- | ---- | ---- | ---- | ----
0x00 | #AMA | 0x00 | G1C  | G2C
0x10 | G1P0 | G2P0 | G1NP | G2NP

## Body

Address | Description
------- | ---------------------
  G1P0  | List of pointers (`G1P1_0`, `G1P1_1`, ..., up to G1C)
 G1P1_0 | Some 8 values
 G1P1_1 | Some other 8 values
   ...  | So on
  G2P0  | List of pointers (`G2P1_0`, `G2P1_1`, ..., up to G2C)
 G2P1_0 | Some 38 values
 G2P1_1 | Some other 38 values
   ...  | So on
  G1NP  | List of name pointers (`G1NP_0`, `G1NP_1`, ..., up to G1C)
 G1NP_0 | Name of object0 of group 1 (string)
 G1NP_1 | Name of object1 of group 1 (string)
   ...  | So on
  G2NP  | List of name pointers (`G2NP_0`, `G2NP_1`, ..., up to G2C)
 G2NP_0 | Name of object0 of group 2 (string)
 G2NP_1 | Name of object1 of group 2 (string)
   ...  | So on

End of file.


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
