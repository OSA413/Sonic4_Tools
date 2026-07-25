#pragma endian little

struct Event {
    u16 unknown1;
    u16 unknown2;
    u16 unknown3;
    u16 unknown4;
    u16 unknown5;
    u16 unknown6;
};

struct EventList {
    u16 number;
    Event event[number];
};

struct EventPointer {
    u32 pointer;
    
    EventList list @ pointer;
};

struct EventSet {
    u16 x_chunks;
    u16 y_chunks;
    
    EventPointer events[x_chunks * y_chunks];
};

EventSet ev @ 0x00;
