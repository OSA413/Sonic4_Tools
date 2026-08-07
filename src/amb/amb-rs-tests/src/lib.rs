
use std::{fs, path::PathBuf};
use serde::{Deserialize, Serialize};

use amb_rs_lib::amb::Amb;


#[derive(Serialize, Deserialize)]
pub struct BinaryObjectPrint {
    pub name: String,
    pub real_name: String,
    pub unknown: u32,
    #[serde(rename = "USR0")]
    pub usr0: u16,
    #[serde(rename = "USR1")]
    pub usr1: u16,
    pub pointer: u32,
    pub length: u32,
}

#[derive(Serialize, Deserialize)]
pub struct AmbPrint {
    pub name: String,
    pub version: String,
    pub endianness: String,
    pub objects: Vec<BinaryObjectPrint>
}

pub fn check_amb_eq(left_path: &PathBuf, reference_file: &String, amb_print: &AmbPrint) {
    let resulting_amb_path = left_path.display().to_string();
    let resulting_amb = Amb::new_from_file_name(&resulting_amb_path).unwrap();
    
    assert_eq!(
        format!("{resulting_amb}"),
        serde_json::to_string(amb_print).unwrap()
    );
    
    assert_eq!(
        fs::read(&resulting_amb_path).unwrap(),
        fs::read(reference_file).unwrap()
    )
}
