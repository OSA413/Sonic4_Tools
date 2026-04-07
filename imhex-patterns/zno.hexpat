/*
Warning: this is a WIP

Found a problem or want to improve the script?
Just open an issue or send your code via Pull Request.
It's GNU GPL, baby!

Credits:
 * RadiantDerg for DimpsSonicLib (GNU GPL-3.0);
 * Argx2121 for Sega_NN_tools (GNU GPL-3.0/MIT);
 * Hedgedocs: https://hedgedocs.com/index.php/NN_Chunk_Format (CC BY-SA);
 * Cat/Anon98, for providing their initial research of ZNO

This particular hexpat is licensed under GNU GPL-3.0 license: 

    Copyright (C) 2026 OSA413

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/


#define big endian;

import std.mem;
import std.io;

struct NZSectionHeader
{
    u32 signature;
    u32 SectionSize;
};

struct NZIF_Section
{
    NZSectionHeader SectionHeader;
    u32 NumberOfSectionsInDataSection;
    u32 OffsetToStartOfDataSection;
    u32 SizeOfDataSection;
    u32 OffsetToNOF0Section;
    u32 SizeOfNOF0Lists;
    u32 version;
};

struct FaceLensList
{
    u16 entries[parent.parent.parent.ModelInfoStruct.FaceCount];
};

struct FaceList
{
    u16 entries[parent.parent.parent.ModelInfoStruct.FaceCount];
};

struct FaceInfo
{
    u32 ReservedMaybe;
    u32 FaceShortCount;
    u32 FaceLensCount;
    u32 FaceLensOffset;
    u32 FaceOffset;
    FaceLensList faceLensList[FaceLensCount] @ (parent.parent.parent.nzif.OffsetToStartOfDataSection + FaceLensOffset);
    FaceList faceList[FaceLensCount] @ (parent.parent.parent.nzif.OffsetToStartOfDataSection + FaceOffset);
};

struct MeshInfo
{
    u32 MeshType;
    u32 MeshCount;
    u32 MeshOffset;
    u32 unk1;
    u32 unk2;
};

struct StringList {
    char value[];
};

struct NZTLStruct
{
    u32 NumberOfTextures;
    u32 OffsetIndexArray;
    StringList TexList[NumberOfTextures];
    //char *TexList[NumberOfTextures];
};

struct NZTLStructIndexEntry
{
    u32 fType; //Used if EAX & 0x200 != 0; Read at instruction 0x6C6FBC
    u32 TexNamePtr;
    //Value used in function 0x66E0E0
    u16 InterpolationMinFilter; //Used Always. Possible values: 0 <= unk2 <= 2
    //Value checked in function 0x66E0E0
    u16 interpolationMagFilter; //Used Always. Possible values: 0 <= unk3 <= A
    u32 GlobalIndex; //Used if EAX & 0x400 != 0
    u32 Bank; //Used if EAX & 0x800 != 0
};

struct NZTL_Section
{
    NZSectionHeader SectionHeader;
    u32 OffsetTexList;
    u32 Reserved;
    NZTLStructIndexEntry entries[];
    //struct NZTLStructIndexEntry entries[NumberOfTextures];
    NZTLStruct mainList;
};

struct Texture
{
    u32 TextureFlags;
    //This index is an index into the NZTL Texture list.
    u32 TextureIndex;
    u32 unk;
    float TextureAlpha;
    u32 unk2;
    u32 unk3;
    float TextureScale[2];
    u32 UnusedMaybe[8];
};

struct Colour
{
    float Diffuse[4] = {0.75f, 0.75f, 0.75f, 1.0f};
    float Ambient[4] = {0.75f, 0.75f, 0.75f, 1.0f};
    float Specular[4] = {0.9f, 0.9f, 0.9f, 1.0f};
    float Emission[4] = {0.0f, 0.0f, 0.0f, 1.0f};
    //These aren't embedded in the binary.
    float Shininess = 0.5;
    float SpecularValue = 0.5;
};

struct TexInfo
{
    u32 Reserved;
    u32 Reserved2;
    u32 ColourOffset;
    u32 Reserved3;
    u32 Reserved4;
    u32 TextureCount;
    u32 TextureOffset;
};

struct TexOffset
{
    u32 ReservedMaybe;
    u32 Offset;
    TexInfo info @ (parent.parent.nzif.OffsetToStartOfDataSection + Offset);
    Texture textures[info.TextureCount] @ (parent.parent.nzif.OffsetToStartOfDataSection + info.TextureOffset);
    Colour colours[info.TextureCount] @ (parent.parent.nzif.OffsetToStartOfDataSection + info.ColourOffset);
};

struct VertexInfo {
    u64 VertexFlags;
    u32 BlockSize;
    u32 VertexCount;
    u32 VertexMeshOffset;
    u32 BoneCount;
    u32 BoneOffsetComplex;
};

struct VertexMesh
{
    //This is an array of floats but;
    //If the bit flag for colour short is set, it is an array of colours.
    //If the bit flag for weight indices or colour bytes is set, it is an array of bytes.
    //char VertexBuffer[VertexCount * BlockSize];
    //Each of these fields are independent from one another and depends on the VertexFlags value.
    //It is not possible to read this structure straight from the VertexBuffer.
    u64 VertexFlags;
    float Position[3];
    float Weights[3];
    u8 Bones[4];
    float Normal[3];
    float UV[2];
    //This is UV2 in Godot.
    float WX[2];
    //It should be noted that colours are stored in the BGRA format.
    u16 ColourS[4];
    //u8 ColourB[4];
};


struct VertexOffset
{
    u32 InfoFlags; //If this flag is equal to 4, the VertexInfo struct does not exist.
    u32 Offset;
    VertexInfo info @ (parent.parent.nzif.OffsetToStartOfDataSection + Offset);
    u32 boneComplex[info.BoneCount] @ (parent.parent.nzif.OffsetToStartOfDataSection + info.BoneOffsetComplex);
    VertexMesh vertexMesh[info.VertexCount] @ (parent.parent.nzif.OffsetToStartOfDataSection + info.VertexMeshOffset);
};

struct FaceOffset
{
    u32 ReservedMaybe;
    u32 Offset;
    FaceInfo data[parent.ModelInfoStruct.FaceCount] @ (parent.parent.nzif.OffsetToStartOfDataSection + Offset);
};

struct SubMeshInfo
{
    float boundsPosition[3];
    float boundsScale;
    u32 BoneVisibility;
    u32 Bone; // 0xFFFF'FFFF means NULL.
    u32 Material;
    u32 Vertex;
    u32 Face;
    //According to Sega NN Tools, Shader System -> User -> Shader File being the final index.
    u32 Shader;
};

struct Matrix
{
    float l0[4];
    float l1[4];
    float l2[4];
    float l3[4];
};

struct Bone
{
    u32 BoneFlags;
    u16 BoneGroup;
    u16 BoneParent;
    u16 BoneChild;
    u16 BoneSibling;
    float BonePos[3];
    //Each value of this vector is encoded using the Binary Angular Measurement format with n = 16.
    //The XYZ order of this Euler Vector is determined by the BoneFlags.
    s32 BoneRot[3];
    float BoneScale[3];
    //Matrices are linearly stored such that each column follows the other in the following order:
    //x0, x1, x2, x3, y0, y1, y2, y3, z0, z1, z2, z3, w0, w1, w2, w3
    //It seems every matrix always represents an homogeneous transform.
    Matrix BoneMatrix;
    float BoneCenter[3];
    float BoneRadius; 
    u32 BoneUser;
    float BoneLength[3];
};

struct SubMeshInfoList
{
    SubMeshInfo entries[];
};

struct ModelInfo
{
    float BoundsPosition[3]; //x, y, z
    float BoundsScale;
    u32 MaterialCount;
    u32 MaterialOffset;
    u32 VertexCount;
    u32 VertexOffset;
    u32 FaceCount;
    u32 FaceOffset;
    u32 BoneCount;
    u32 ReservedMaybe;
    u32 BoneOffset; //This offset should be added to the start of the DataSection.
    u32 BoneUsedCount;
    u32 MeshSets;
    u32 MeshOffset; //At Offset  + StartOfDataSection, one can read an array of MeshInfo if it exists.
};

struct NZOB_Section
{
    NZSectionHeader SectionHeader;
    u32 OffsetToModelInfo; //This offset is added to the start of the DataSection
    ModelInfo ModelInfoStruct @ (parent.nzif.OffsetToStartOfDataSection + OffsetToModelInfo);

    Bone bones[ModelInfoStruct.BoneCount] @ (parent.nzif.OffsetToStartOfDataSection + ModelInfoStruct.BoneOffset);
    TexOffset textures[ModelInfoStruct.MaterialCount] @ (parent.nzif.OffsetToStartOfDataSection + ModelInfoStruct.MaterialOffset);
    FaceOffset faces[ModelInfoStruct.FaceCount] @ (parent.nzif.OffsetToStartOfDataSection + ModelInfoStruct.FaceOffset);
    VertexOffset vertices[ModelInfoStruct.VertexCount] @ (parent.nzif.OffsetToStartOfDataSection + ModelInfoStruct.VertexOffset);
};

struct NOF0_Section
{
    NZSectionHeader SectionHeader;
    //Offsets are found via the operation [PTR + (Value[CurrentNumber] >> 2) * sizeof(uint32_t)] in function 0x6C5420.
    //PTR is added to the value stored in this offset, pointing to a part of the NZOB data.
    //It should be noted that these offsets are part of the DataSection.
    u32 NumberOfPointersToReplaceInData;
    u32 Reserved;// I think.
    u32 Pointers[]; //Always padded to 16 byte boundaries.
};

struct NFN0_Section
{
    NZSectionHeader SectionHeader;
    u32 Padding[2];
    char Name[]; //ZNO Name Padded to 16 byte boundary.
};

struct NEND_Section
{
    NZSectionHeader SectionHeader;
    u32 Padding[2];
};

struct ZNO {
    NZIF_Section nzif @ 0x0;
    
    if (std::mem::find_string(0, "NZTL") >= 0)
        NZTL_Section nztl @ std::mem::find_string(0, "NZTL");
    
    //if (std::mem::find_string(0, "NZNN") >= 0)
    //    NZNN_Section nznn @ std::mem::find_string(0, "NZNN");
    
    if (std::mem::find_string(0, "NZOB") >= 0)
        NZOB_Section nzob @ std::mem::find_string(0, "NZOB");
    
    //if (std::mem::find_string(0, "NZMO") >= 0)
    //    NZMO_Section nzmo @ std::mem::find_string(0, "NZMO");
    
    //if (std::mem::find_string(0, "NZTN") >= 0)
    //    NZTN_Section nztn @ std::mem::find_string(0, "NZTN");
    
    NOF0_Section nof0 @ nzif.OffsetToNOF0Section;
    
    // Is optional
    if (std::mem::find_string(0, "NFN0") >= 0)
        NFN0_Section nfn0 @ std::mem::find_string(0, "NFN0");
    
    NEND_Section nend @ std::mem::find_string(0, "NEND");
};

ZNO zno @ 0x0;