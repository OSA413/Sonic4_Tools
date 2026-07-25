// TODO: find out all the values on large and small files

#pragma endian little

struct Group1Object {
    u32 unknown1;
    u32 index;
    u32 childObject1Pointer;
    u32 childObject2Pointer;
    u32 parentObjectPointer;
    u32 unknown2;
    u32 unknown3;
};

struct GrandDad {
    
};

struct Group2ChildObjectGrandChild {
    u32 unknown1;
    u32 unknown2;
    float unknown3;
    float unknown4;
    float unknown5;
    float unknown6;
    u32 unknown7;
    u32 unknown8;
    u32 unknown9;
    u32 pointer1;
    u32 pointer2;
    u32 unknown10;
    u32 unknown11;
    u32 unknown12;
    u32 unknown13;
    u32 unknown14;
    u32 unknown15;
    u32 unknown16;
    
    //GrandDad child1 @ pointer1;
};

struct Group2ChildObjectChild1 {
    u32 unknown1;
    u32 unknown2;
    u32 unknown3;
    u32 unknown4;
    u32 unknown5;
    u32 unknown6;
    u32 unknown7;
    u32 pointer1;
    u32 pointer2;
    u32 unknown8;
    u32 unknown9;
    u32 unknown10;
    
    Group2ChildObjectGrandChild child1 @ pointer1;
};

struct Group2ChildObject {
    u32 numberOfChildrenObjects;
    u32 unknown1;
    u32 unknown2;
    u32 unknown3;
    float bounding_box_left_from_center;
    float bounding_box_top_from_center;
    float bounding_box_right_from_center;
    float bounding_box_bottom_from_center;
    u32 pointer1;
    u32 pointer2;
    u32 pointer3;
    u32 pointer4;
    u32 pointer5;
    u32 pointer6;
    u32 pointer7;
    u32 pointer8;
    
    Group2ChildObjectChild1 child1 @ pointer1;
};

struct Group2ChildObject2 {
    u32 numberOfChildrenObjects;
    u32 unknown1;
    u32 unknown2;
    u32 unknown3;
    float bounding_box_left_from_center;
    float bounding_box_top_from_center;
    float bounding_box_right_from_center;
    float bounding_box_bottom_from_center;
    u32 uknown4;
};

struct Group2Object {
    u32 pointer1;
    
    Group2ChildObject Group2ChildObject @ pointer1;
    //Group2ChildObject2 Group2ChildObject2 @ pointer2;
};

struct AMAName {
    u32 pointer;
    char name[] @ pointer;
};

struct AMA {
    char magic[0x4];
    u32 version;
    u32 Group1ObjectCount;
    u32 Group2ObjectCount;
    u32 Group1ObjectPointer;
    u32 Group2ObjectPointer;
    // Do you like pointers to pointers?
    u32 Group1ObjectNamePointersPointer;
    u32 Group2ObjectNamePointersPointer;
    Group1Object g1objects[Group1ObjectCount] @ Group1ObjectPointer;
    Group2Object g2objects[Group2ObjectCount] @ Group2ObjectPointer;
    AMAName g1ObjectNames[Group1ObjectCount] @ Group1ObjectNamePointersPointer;
    AMAName g2ObjectNames[Group2ObjectCount] @ Group2ObjectNamePointersPointer;
};

AMA ama @ 0x0;
