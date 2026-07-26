use common_binary::{
    binary_reader, binary_writer, endianness::Endianness, error::CommonBinaryError
};
use serde::{Deserialize, Serialize};
use crate::md_entry::MdEntry;

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct MdSet {
    pub x_tiles: u16,
    pub y_tiles: u16,
    pub entries: Vec<MdEntry>,
}

impl MdSet {
    pub fn new_from_file_name(file_path: &String) -> Result<Self, CommonBinaryError> {
        Self::new_from_src_ptr_name(&std::fs::read(file_path)?, Some(0))
    }

    pub fn new_from_src_ptr_name(
        source: &[u8],
        ptr: Option<usize>,
    ) -> Result<Self, CommonBinaryError> {
        let ptr = ptr.unwrap_or(0);

        let x_tiles = binary_reader::u16::read(source, ptr, &Endianness::Little)
        // TODO: move exceptions to Result Err() with Option<When>
            .expect("Error reading x_tiles from MdSet header");
        let y_tiles = binary_reader::u16::read(source, ptr + 2, &Endianness::Little)
            .expect("Error reading y_tiles from MdSet header");

        let entries_number = x_tiles as usize * y_tiles as usize;
        let mut entries = Vec::with_capacity(entries_number);

        let mut entry_pointer = ptr + 0x04;
        for _ in 0..entries_number {
            let unknown1 = binary_reader::u8::read(source, entry_pointer)?;
            entry_pointer += 0x01;
            entries.push(MdEntry { unknown1 });
        }

        Ok(MdSet {
            x_tiles,
            y_tiles,
            entries,
        })
    }

    pub fn length(&self) -> usize {
        0x04 + self.entries.len()
    }

    pub fn write(&self) -> Result<Vec<u8>, CommonBinaryError> {
        let length = self.length();
        let mut result = Vec::<u8>::with_capacity(length);

        for _ in 0..length {
            result.push(0);
        }

        binary_writer::u16::write(&mut result, 0x00, self.x_tiles, &Endianness::Little, "x_tiles".to_string())?;
        binary_writer::u16::write(&mut result, 0x02, self.y_tiles, &Endianness::Little, "y_tiles".to_string())?;

        let mut entry_pointer = 0x04;
        for entry in &self.entries {
            binary_writer::u8::write(&mut result, entry_pointer, entry.unknown1, "unknown1".to_string())?;
            entry_pointer += 0x01;
        }

        Ok(result)
    }
}