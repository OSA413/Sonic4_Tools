#pragma endian big

struct TXBObject {
    u32 unknown1;
    u32 unknown2;
    u16 minFilter;
    u16 magFilter;
    u32 unknown3;
    u32 unknown4;
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
    TXBName object_names[object_number];
};

TXB txb @ 0x0;
