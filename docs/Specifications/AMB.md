# AMB File Structure
### AMBs contain these components:
* A Header
* A Sub Header
* A File Index
* File data
* A name table (**Not** required and can be excluded for masking)

Templates to quickly view this data in 010 Editor can be found [here](https://github.com/RadiantDerg/DimpsSonicLib/tree/main/Templates "Dimps Templates")

# Header
The header of an AMB file is unanimous among almost all versions of the file, with the exception of [Windows Phone AMBs.](https://github.com/OSA413/Sonic4_Tools/blob/master/docs/Specifications/AMB_wp.md "Windows Phone AMB File Structure")

    Offset(h)  0  1  2  3  4  5  6  7   8  9  A  B  C  D  E  F  0123456789ABCDEF
    00000000  23 41 4D 42 20 00 00 00  00 00 04 00 00 00 00 00  #AMB ...........

### The information stored in the header is as follows:
  
Address | Description
------- | -----------
  0x00  | `char signature[4]` - The file magic, better known as signature
  0x04  | `uint fileVersion` - The version of the AMB file
  0x08  | `ushort unknown1` - This always seems to be `0`
  0x0A  | `ushort unknown2` - This alwaus seems to be `4`
  0x0C  | `byte endianess` - Endianness flag. `0` is little, `1` is big
  0x0D  | `byte unknown3` - This always seems to be `0`
  0x0E  | `byte unknown4` - This always seems to be `0`
  0x0F  | `byte compressionType` - This flag details the compression method
  
**¹** - Changing this value to a random one doesn't crash game.
### Here are the identifiers for the AMB versions:

fileVersion | Description
------- | -----------
  32  | **AMB v1** (These are the most common types of memory binders you will encounter)
  40  | **AMB v2**
  48  | **AMB v3**

<br></br>
# Sub Header
Contains pointers to sections inside the AMB.

<br></br>
## AMB v1

    Offset(h)  0  1  2  3  4  5  6  7   8  9  A  B  C  D  E  F  0123456789ABCDEF
    00000010  03 00 00 00 20 00 00 00  60 00 00 00 60 2E 10 00  .... ...`...`...

### The information stored in the sub header is as follows:

Address | Description
------- | -----------
  0x00  | `uint fileCount` - The number of files contained in the AMB
  0x04  | `uint listPointer` - The pointer to the File Index
  0x08  | `uint dataPointer` - The pointer to the file data
  0x0C  | `uint nameTable`¹ - The pointer to the list of names. This CAN be nulled.

**¹** - Changing this value to a random one doesn't crash game.

<br></br>
## AMB v2
//Todo
<br></br>
## AMB v3

    Offset(h)  0  1  2  3  4  5  6  7   8  9  A  B  C  D  E  F  0123456789ABCDEF
    00000010  15 00 00 00 00 00 00 00  30 00 00 00 00 00 00 00  ........0.......
    00000020  40 02 00 00 00 00 00 00  50 D7 0E 00 00 00 00 00  @.......P×......

### The information stored in the sub header is as follows:

Address | Description
------- | -----------
  0x00  | `uint64 fileCount` - The number of files contained in the AMB
  0x04  | `uint64 listPointer` - The pointer to the File Index
  0x08  | `uint64 dataPointer` - The pointer to the file data
  0x0C  | `uint64 nameTable`¹ - The pointer to the list of names. This CAN be nulled.

**¹** - Changing this value to a random one doesn't crash game.

<br></br><br></br>
# File Index

This is a complete list of the files contained within the AMB. Each file has it's own sequence of bytes detailing information about the file.

<br></br>
## AMB v1

    Offset(h)  0  1  2  3  4  5  6  7   8  9  A  B  C  D  E  F  0123456789ABCDEF
    00000010  60 00 00 00 30 8A 08 00  FF FF FF FF 00 00 00 00  `...0Š..ÿÿÿÿ....

### The information stored in the index entry is as follows:

Address | Description
------- | -----------
  0x00  | `uint filePointer` - The pointer to the file in the AMB
  0x04  | `uint fileSize` - The total length of the file in bytes
  0x08  | `uint unkEditorData`¹ - Unknown Data (Dimps Internal Tool)
  0x0C  | `short USR0`¹ - User 1 Data (Dimps Internal Tool)
  0x0E  | `short USR1`¹ - User 2 Data (Dimps Internal Tool)

**¹** - Changing this value to a random one doesn't crash game.

<br></br>
## AMB v2
//Todo
<br></br>
## AMB v3

    Offset(h)  0  1  2  3  4  5  6  7   8  9  A  B  C  D  E  F  0123456789ABCDEF
    00000030  40 02 00 00 00 00 00 00  00 00 00 00 EC 16 01 00  @...........ì...
    00000040  FF FF FF FF 00 00 00 00                           ÿÿÿÿ....

### The information stored in the sub header is as follows:

Address | Description
------- | -----------
  0x00  | `uint filePointer` - The pointer to the file in the AMB
  0x00  | `uint unk1` - Unknown
  0x00  | `uint unk2` - Unknown
  0x04  | `uint fileSize` - The total length of the file in bytes
  0x08  | `uint unkEditorData`¹ - Unknown Data (Dimps Internal Tool)
  0x0C  | `short USR0`¹ - User 1 Data (Dimps Internal Tool)
  0x0E  | `short USR1`¹ - User 2 Data (Dimps Internal Tool)

**¹** - Changing this value to a random one doesn't crash game.

<br></br>
<!-- Note: In some iOS version of Episode 1 the actual Data Pointer is at 0x1C. Starting from 0x14 there are zero (0x00) byte following after a meaningful value (LP 00 DP 00 NP 00..) //TODO

(They are probably uint64) -->
