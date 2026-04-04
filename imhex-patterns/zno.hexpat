/*
Found a problem or want to improve the script?
Just open an issue or send your code via Pull Request.
It's GNU GPL, baby!

Credits:
 * RadiantDerg for DimpsSonicLib (GNU GPL-3.0);
 * Argx2121 for Sega_NN_tools (GNU GPL-3.0/MIT);
 * Hedgedocs: https://hedgedocs.com/index.php/NN_Chunk_Format (CC BY-SA);
 * Cat/Anon98, for providing their initial research of ZNO

This particular hexpat is licensed under GNU GPL-3.0 license: 

    Copyright (C) 2026 OSA413

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/


#define big endian;
 
struct NZSectionHeader
{
    u32 signature;
    u32 SectionSize;
};

struct NZIF_Section
{
    NZSectionHeader SectionHeader;
    u32 NumberOfSectionsInDataSection;
    u32 OffsetToStartOfDataSection;
    u32 SizeOfDataSection;
    u32 OffsetToNOF0Section;
    u32 SizeOfNOF0Lists;
    u32 version;
};

NZIF_Section nzif @ 0x0;