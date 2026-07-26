#pragma endian little

struct Decoration {
    u16 unknown1;
    u16 unknown2;
};

struct DecorationList {
    u16 number;
    Decoration decoration[number];
};

struct DecorationPointer {
    u32 pointer;
    
    DecorationList list @ pointer;
};

struct DecorationSet {
    u16 x_tiles;
    u16 y_tiles;
    
    DecorationPointer deco[x_tiles * y_tiles];
};

DecorationSet dc @ 0x00;
