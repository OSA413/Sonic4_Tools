#pragma endian little

struct Entry {
    u16 unknown1;
};

struct MP {
    u16 x_tiles;
    u16 y_tiles;
    
    Entry entries[x_tiles * y_tiles];
};

MP mp @ 0x00;
