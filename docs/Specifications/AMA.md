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

Address | Description
------- | ------------
  0x00  | Unknown
  0x04  | Object order number (int32)
  0x08  | Nunber of inner (G2-X) objects
  0x08  | 0x00
  0x10  | X position (float)
  0x14  | Y position (float)
  0x18  | X size (float)
  0x1C  | Y size (float)
  0x20  | Pointer to G2-0 object
  0x24  | Pointer to G2-1 object
  0x28  | Pointer to G2-2 object
  0x2C  | Pointer to G2-3 object
  0x30  | Pointer to G2-4 object
  0x34  | Pointer to G2-5 object
  0x38  | 0x00
  0x3C  | 0x00
  
## Group 2 - 0 Object

Address | Description
------- | ------------
  0x00  | Unknown
  0x04  | Number of G2-0-0 objects
  0x08  | Number of G2-0-1 objects
  0x0C  | Pointer to G2-0-0 object
  0x10  | Pointer to G2-0-1 object
  0x14  | Number of G2-0-2 objects
  0x18  | Number of G2-0-3 objects
  0x1C  | Pointer to G2-0-2 object
  0x20  | Pointer to G2-0-3 object
  0x24  | 0x00
  0x28  | 0x00
  0x2C  | 0x00

## Group 2 - 0 - 0 Object

## Group 2 - 0 - 1 Object

## Group 2 - 0 - 2 Object

## Group 2 - 0 - 3 Object

G1C - Number of object in group 1
G2C - Number of object in group 2
G1NP - Pointer to list of pointers of object names in group 1.
G2NP - Pointer to list of pointers of object names in group 2.
