#pragma endian little

struct Entry {
    u8 unknown1;
};

struct MD {
    u16 x_tiles;
    u16 y_tiles;
    
    Entry entries[x_tiles * y_tiles];
};

MD md @ 0x00;
