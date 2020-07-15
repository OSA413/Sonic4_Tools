# TXB Specification

## Header

.    | 0-3  | 4-7  | 8-B  | C-F
---- | ---- | ---- | ---- | ----
0x00 | #TXB | 0x10 | 0x00 | 0x00
0x10 |  FN  | OP_0 |      |     

FN - File number

OP_0 - first object pointer

## Body

Address | Description
------- | ---------------------
  OP_0  | Beginning of Object#0
OP_0 + 4 (NP0)| Object#0 file name pointer
   ...  | Some other values
OP_0 + 5*4 | Beginning of Object#1
   ...  | And so on up to FN objects
   NP0  | Name of the file of the object#0
   NP1  | Name of the file of the object#1

## Object

Address | Description
------- | ---------------------
  0x00  | Unknown0
  0x04  | File name pointer (NPX)
  0x08  | Unknown1
  0x0C  | Unknown2
  0x10  | Unknown3

This is big-endiann

