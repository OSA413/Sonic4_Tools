# AMB File Structure

.    | 0-3  | 4-7  | 8-B  | C-F
---- | ---- | ---- | ---- | ----
0x00 | #AMB | [ED] |  ?   | 0x00
0x10 | [FN] | [LP] | [DP] | [NP]
[LP] | [FP] | [FL] |  ?   | 0x00

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
