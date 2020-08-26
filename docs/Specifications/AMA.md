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
 G1P1_0 | Group 1 Object#0
 G1P1_1 | Group 1 Object#1
   ...  | So on
  G2P0  | List of pointers (`G2P1_0`, `G2P1_1`, ..., up to G2C)
 G2P1_0 | Group 2 Object#0
 G2P1_1 | Group 2 Object#1
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

## Group 1 Object

Address | Description
------- | ------------
  0x00  | Unknown
  0x04  | Objcect order number (int32)
  0x08  | Group 1 child object 0 pointer
  0x0C  | Group 1 child object 1 pointer
  0x10  | Parent object pointer
  0x14  | Group 2 child object pointer
  0x18  | 0x00
  0x1C  | 0x00

## Group 2 Object

WIP (object length may differ)

*If value is not specified, assume it is 0x00*

Address | Description
------- | ------------
  0x00  | Unknown0
  0x04  | Object order number (int32)
  0x10  | X position (float)
  0x14  | Y position (float)
  0x18  | X size (float)
  0x1C  | Y size (float)
  0x88  | UV left edge (float)
  0x8C  | UV upper edge (float)
  0x90  | UV right edge (float)
  0x94  | UV bottom edge (float)

G1C - Number of object in group 1
G2C - Number of object in group 2
G1NP - Pointer to list of pointers of object names in group 1.
G2NP - Pointer to list of pointers of object names in group 2.
