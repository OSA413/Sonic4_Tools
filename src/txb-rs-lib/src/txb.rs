use std::io::Read;
use common_binary::{
    binary_reader, binary_writer, endianness::Endianness, error::CommonBinaryError
};
use serde::{Deserialize, Serialize};
use crate::{gl_texture_mag_filter::GlTextureMagFilter, gl_texture_min_filter::GlTextureMinFilter, txb_object::TxbObject};

#[derive(Serialize, Deserialize)]
pub struct Txb {
    pub textures: Vec<TxbObject>,
}

struct TxbPointersPrediction {
    data: usize,
    name: usize
}

impl Txb {
    pub fn is_source_txb(source: &[u8], ptr: Option<usize>) -> bool {
        let ptr = ptr.unwrap_or(0);
        source.len() - ptr >= 0x18
            && source[ptr] == b'#'
            && source[ptr + 1] == b'T'
            && source[ptr + 2] == b'X'
            && source[ptr + 3] == b'B'
    }

    pub fn new_empty() -> Self {
        Self { textures: Vec::new() }
    }

    pub fn length(&self) -> usize {
        let pointers_predition = self.predict_pointers();
        pointers_predition.name + self.textures.iter().map(|x| x.name.len() + 1).sum::<usize>()
    }

    fn predict_pointers(&self) -> TxbPointersPrediction {
        let data = 0x18;
        let name = data + self.textures.len() * 5 * 4;
        TxbPointersPrediction {
            data,
            name
        }
    }

    pub fn new_from_file_name(file_path: &String) -> Result<Self, CommonBinaryError> {
        Self::new_from_src_ptr_name(&std::fs::read(file_path)?, Some(0))
    }

    pub fn new_from_src_ptr_name(
        source: &[u8],
        ptr: Option<usize>,
    ) -> Result<Self, CommonBinaryError> {
        if !Txb::is_source_txb(source, ptr) {
            return Err(CommonBinaryError::ProvidedSourceIsNotOfExpectedFormat(
                format!("Provided source is not a TXB file, ptr: {ptr:?}")
            ));
        }

        let ptr = ptr.unwrap_or(0);

        let file_number = binary_reader::u32::read(source, ptr + 0x10, &Endianness::Big)
            .expect("Error reading file number from TXB header");
        let object_pointer = binary_reader::u32::read(source, ptr + 0x14, &Endianness::Big)
            .expect("Error reading object pointer from TXB header") as usize;

        let mut objects = Vec::<TxbObject>::new();

        for i in 0..file_number as usize {
            let ptr = object_pointer + i * 5 * 4;

            // Maybe convert .expect to a Result Err()?
            let name_pointer = binary_reader::u32::read(source, ptr + 0x04, &Endianness::Big)
                .expect(&format!("Error reading name pointer of {}th object", i + 1).into_boxed_str()) as usize;
            let (name, _) = binary_reader::string32::read(source, name_pointer)
                .expect(&format!("Error reading the name of {}th object", i + 1).into_boxed_str());
            let min_filter = binary_reader::u16::read(source, ptr + 0x08, &Endianness::Big)
                .expect(&format!("Error reading min_filter of {name}").into_boxed_str());
            let mag_filter = binary_reader::u16::read(source, ptr + 0x0A, &Endianness::Big)
                .expect(&format!("Error reading mag_filter of {name}").into_boxed_str());

            objects.push(TxbObject {
                name,
                min_filter: GlTextureMinFilter::from(min_filter),
                mag_filter: GlTextureMagFilter::from(mag_filter),
            });
        }

        Ok(Txb { textures: objects })
    }

    pub fn write(&self) -> Result<Vec<u8>, CommonBinaryError> {
        let txb_length = self.length();
        let mut result = Vec::<u8>::with_capacity(txb_length);
        let mut pointers = self.predict_pointers();

        let mut i = 0;
        while i < txb_length {
            result.push(0);
            i += 1;
        }

        "#TXB".as_bytes().read_exact(&mut result[0x0..0x4]).unwrap();
        // Version, suppose it's 0x10 as for now
        binary_writer::u32::write(&mut result, 0x04, 0x10, &Endianness::Big, "version".to_string())?;
        // unknown1, TODO: add
        // unknown2, TODO: add
        binary_writer::u32::write(&mut result, 0x10, self.textures.len() as u32, &Endianness::Big, "object length".to_string())?;
        binary_writer::u32::write(&mut result, 0x14, pointers.data as u32, &Endianness::Big, "list pointer".to_string())?;

        for texture in self.textures.iter() {
            // unknown1
            binary_writer::u32::write(&mut result, pointers.data + 4, pointers.name as u32, &Endianness::Big, "name pointer".to_string())?;
            binary_writer::u16::write(&mut result, pointers.data + 8, texture.min_filter.into(), &Endianness::Big, "min_filter".to_string())?;
            binary_writer::u16::write(&mut result, pointers.data + 10, texture.mag_filter.into(), &Endianness::Big, "mag_filter".to_string())?;

            // TODO: move to the binary_writer
            texture.name.as_bytes().read_exact(&mut result[pointers.name..(pointers.name + texture.name.len())]).unwrap();

            pointers.data += 5 * 4;
            pointers.name += texture.name.len() + 1;
        }

        Ok(result)
    }
}

impl std::fmt::Display for Txb {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        write!(f, "{}", serde_json::to_string(&self).unwrap_or("couldn't represent this TXB as JSON".to_string()))
    }
}