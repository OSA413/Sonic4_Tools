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

struct DecorationContainer {
    u16 x_chunks;
    u16 y_chunks;
    
    DecorationPointer deco[x_chunks * y_chunks];
};

DecorationContainer dc @ 0x00;
