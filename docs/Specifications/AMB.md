# AMB File Structure
### First, a little bit of background information is neecessary:
AMB files are also known as 'Memory Binders'. 

They are completely **hard-coded** file archives, meaning that the game will *only* reference the files inside of these binders by their ID (The ID itself is the order of which the said file appears inside). 

This means that while you can freely edit whatever file they contain, the order of those files or the file formats themselves must **NOT** be changed. This will confuse the game and cause it to crash or hang.

**For example:** You cannot replace a `.ZNO` file with a `.ZNM` file. 

There are also several bytes within the binder that go unused by the actual game. These bytes are actually meant for an internal tool Dimps used to describe these AMB files. It would spit out a header containing information about the binder and it's contents, which would then be compiled into the final game's code.

Console versions are big endian. PC and Mobile are little endian.

## Header
The header of an AMB file is unanimous among almost all versions of of the file, with the exception of [Windows Phone AMBs.](https://github.com/OSA413/Sonic4_Tools/blob/master/docs/Specifications/AMB_wp.md "AMB (WP) File Structure")

    Offset     0  1  2  3  4  5  6  7   8  9  A  B  C  D  E  F  123456789ABCDEF
    00000000  23 41 4D 42 20 00 00 00  00 00 04 00 00 00 00 00  #AMB ..........

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

## Sub Header

//TODO

### (AMB v1)
### (AMB v2)
### (AMB v3)

## File Index

//TODO

### (AMB v1)
### (AMB v2)
### (AMB v3)

<!-- Note: In some iOS version of Episode 1 the actual Data Pointer is at 0x1C. Starting from 0x14 there are zero (0x00) byte following after a meaningful value (LP 00 DP 00 NP 00..) //TODO

(They are probably uint64) -->