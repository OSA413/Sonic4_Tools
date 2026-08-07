use std::path::Path;

use common_binary::json_formatter;

pub struct BinaryObject {
    pub name: String,
    pub real_name: String,

    pub unknown: u32,
    pub usr0: u16,
    pub usr1: u16,

    pub pointer: usize, //This is used just for the json print and debugging
    pub data: Vec<u8>,
}

impl BinaryObject {
    pub fn length(&self) -> usize {
        self.data.len()
    }

    pub fn length_nice(&self) -> usize {
        self.length() + (16 - self.length() % 16) % 16_usize
    }

    pub fn new_from_src_ptr_len(
        source: &[u8],
        pointer: usize,
        length: usize
    ) -> Self {
        BinaryObject {
            data: source.iter().skip(pointer).take(length).map(|x| x.to_owned()).collect(),
            unknown: 0,
            usr0: 0,
            usr1: 0,
            pointer,
            name: String::new(),
            real_name: String::new(),
        }
    }

    pub fn new_from_file_path(
        file_path: &Path
    ) -> Result<Self, std::io::Error> {
        let file_content = std::fs::read(file_path)?;
        Ok(BinaryObject {
            data: file_content,
            unknown: 0,
            usr0: 0,
            usr1: 0,
            pointer: 0,
            name: String::new(),
            real_name: String::new(),
        })
    }
}

impl std::fmt::Display for BinaryObject {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        write!(f, "{{{}}}", [
            json_formatter::add_str("name", &self.name.replace("\\", "\\\\")),
            json_formatter::add_str("real_name", &self.real_name.replace("\\", "\\\\")),
            json_formatter::add_value("unknown", &self.unknown.to_string()),
            json_formatter::add_value("USR0", &self.usr0.to_string()),
            json_formatter::add_value("USR1", &self.usr1.to_string()),
            json_formatter::add_value("pointer", &self.pointer.to_string()),
            json_formatter::add_value("length", &self.length().to_string()),
        ].join(","))
    }
}