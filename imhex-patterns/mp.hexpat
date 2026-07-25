#pragma endian little

struct Entry {
    u16 unknown1;
};

struct MP {
    u16 x_chunks;
    u16 y_chunks;
    
    Entry entries[x_chunks * y_chunks];
};

MP mp @ 0x00;
