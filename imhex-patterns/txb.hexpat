#pragma endian big

struct TXBObject {
    u32 unknown1;
    u32 name_pointer;
    u16 minFilter;
    u16 magFilter;
    u32 unknown2;
    u32 unknown3;
    char name[] @ name_pointer;
};

struct TXBName {
    char name[];
};

struct TXB {
    char magic[0x4];
    u32 version;
    u32 unknown1;
    u32 unknown2;
    u32 object_number;
    u32 object_pointer;
    TXBObject objects[object_number];
};

TXB txb @ 0x0;
