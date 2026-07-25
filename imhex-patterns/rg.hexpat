#pragma endian little

struct Ring {
    u16 unknown1;
};

struct RingList {
    u16 number;
    Ring  ring[number];
};

struct RingPointer {
    u32 pointer;
    
    RingList list @ pointer;
};

struct RingSet {
    u16 x_chunks;
    u16 y_chunks;
    
    RingPointer rings[x_chunks * y_chunks];
};

RingSet rg @ 0x00;
