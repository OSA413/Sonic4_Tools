#pragma endian little

struct Entry {
    u8 unknown1;
};

struct MD {
    u16 x_chunks;
    u16 y_chunks;
    
    Entry entries[x_chunks * y_chunks];
};

MD md @ 0x00;
