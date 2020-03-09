# AMB (WP) File Structure

Address | Description
------- | -----------
  ----  | Header
  0x00  | (uint32) Number of files inside the container
  0x04  | (uint32 ?) Probably endianness sign
  0x08  | (uint32) ??? (usually 0x00)
  ----  | File Names
  0x0C  | (uint8) Length of the first file name
  0x0D  | (string) First file name (ends with 0xFF)
  after | (uint8) Length of the next file name
  after + 1 | (string) Next file name (ends with 0xFF)
   ...  | So on (up to file number)
  ----  | File pointers (right after the last file name)
  0x00  | (uint32) File0 pointer
  0x04  | (uint32) File1 pointer
   ...  | So on (up to file name number)
  ----  | File lengths (right after the file pointers)
  0x00  | (uint32) File0 length
  0x04  | (uint32) File1 length
   ...  | So on (up to file name number)
  ----  | File data (right after the file lengths)
   ...  | According to the file pointers and lengths


