#pragma endian big
import "std/core";

struct BinaryObject {
    u32 offset;
    u32 len;
    u32 flag1;
    u32 flag2;
    u8 data[len] @ offset;
    char name[0x20] @ (parent.names_offset + std::core::array_index() * 0x20);
};

struct AMB {
    char magic[0x4];
    u32 version;
    u32 flag1;
    u32 flag2;
    u32 object_number;
    u32 list_offset;
    u32 data_offset;
    u32 names_offset;
    BinaryObject objects[object_number];
};

AMB amb @ 0x0;
