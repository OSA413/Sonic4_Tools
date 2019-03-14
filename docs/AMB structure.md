# AMB File Structure

. | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | A | B | C | D | E | F
-- | - | - | - | - | - | - | - | - | - | - | - | - | - | - | - | -
0x00 | 0x23 | 0x41 | 0x4D | 0x42 | [ED] | [ED] | [ED] | [ED] | 0x00 | 0x00 | 0x04 | 0x00 | 0x00 | 0x00 | 0x00 | 0x00
0x10 | [FN] | [FN] | [FN] | [FN] | [LP] | [LP] | [LP] | [LP] | [DP] | [DP] | [DP] | [DP] | [NP] | [NP] | [NP] | [NP]
[LP] | [FP] | [FP] | [FP] | [FP] | [FL] | [FL] | [FL] | [FL] | 0xFF | 0xFF | 0xFF | 0xFF | 0x00 | 0x00 | 0x00 | 0x00

If file structure after 0x10 is different, try to open the file with a different HEX editor.

## Header

[ED] (Endianness) - This 32-bit integer identifies the endianness of the file.

[FN] (File Number) - Number of files in the archive (may differ from the actual number of files).

[LP] (List Pointer) - Pointer to where enumeration of the files pointers and lengths starts.

[DP] (Data Pointer) - Pointer to where enumeration of the files data starts.

[NP] (Name Pointer) - Pointer to where enumeration of the files names starts.

[FP] (File Pointer) - Pointer to where file data starts.

[FL] (File Length) - Pointer to the length of the file.

## Endianness

Console versions are big endian. PC and Mobile are little endian.
