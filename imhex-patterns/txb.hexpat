#pragma endian big
import "std/core";

struct TXBObject {
    u32 unknown1;
    u32 unknown2;
    u16 minFilter;
    u16 magFilter;
    u32 unknown3;
    u32 unknown4;
    // I didn't figure out how to get names from the end of the file, so I'll leave it for the reader
    //if (std::core::array_index() == 0)
    //    u32 name_pointer = (parent.object_pointer + parent.object_number * 0x14);
    //else
    //    u32 name_pointer = (parent.objects[std::core::array_index()*1]).name_pointer;    
    //char name[] @ name_pointer;
};

struct TXB {
    char magic[0x4];
    u32 version;
    u32 unknown1;
    u32 unknown2;
    u32 object_number;
    u32 object_pointer;
    TXBObject objects[object_number] @ object_pointer;
};

TXB txb @ 0x0;
