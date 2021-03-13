# AMB File Structure

.    | 0-3  | 4-7  | 8-B  | C-F
---- | ---- | ---- | ---- | ----
0x00 | #AMB | [VE]¹ |  ?¹   |  ?¹  
0x10 | [FN] | [LP] | [DP] | [NP]
[LP] | [FP] | [FL] |  0x00 or 0xFF   | 0x00

## Header

[VE]¹ (AMB Version) - This 32-bit integer identifies the endianness of the file.

* 0x20 is PC
* todo: add others

[FN] (File Number) - Number of files in the archive (may differ from the actual number of files).

[LP] (List Pointer) - Pointer to where enumeration of the file pointers and lengths starts.

[DP] (Data Pointer) - Pointer to where enumeration of the file data starts.

[NP] (Name Pointer) - Pointer to where enumeration of the file names starts.

[FP] (File Pointer) - Pointer to where file data starts.

[FL] (File Length) - Pointer to the length of the file.

¹ - Changing this value to a random one doesn't crash game.

Note: In some iOS version of Episode 1 the actual Data Pointer is at 0x1C. Starting from 0x14 there are zero (0x00) byte following after a meaningful value (LP 00 DP 00 NP 00..) //TODO

(They are probably uint64)

Extra note: WinPhone AMB files differ from regular AMB (see https://github.com/OSA413/Sonic4_Tools/blob/master/docs/Specifications/AMB_wp.md)

## Endianness

Console versions are big endian. PC and Mobile are little endian.
