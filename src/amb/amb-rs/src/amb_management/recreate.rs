use std::path::{Path, PathBuf};
use amb_rs_lib::{amb::Amb, binary_object::BinaryObject};
use common_binary::error::CommonBinaryError;

use crate::amb_management::{self, common_handler::do_a_thing_over_an_amb_and_save};

pub fn recreate_amb(file: String, save_as_file_name: Option<String>) -> Result<(), CommonBinaryError> {
    do_a_thing_over_an_amb_and_save(
        &file,
        &|_| {},
        &save_as_file_name.unwrap_or(file.clone()),
    )
}

fn _recreate_amb_recursively(amb: &mut Amb) -> (&mut Amb, Vec<CommonBinaryError>) {
    let mut errors = Vec::new();
    let mut new_binary_objects = Vec::new();
    for object in &amb.objects {
        let probably_amb = Amb::new_from_binary_object(object);
        let binary_object = match probably_amb {
            Ok(mut amb) => {
                let (amb, inner_errors) = _recreate_amb_recursively(&mut amb);
                errors.extend(inner_errors);
                match amb.write() {
                    Ok(buffer) => BinaryObject::new_from_src_ptr_len(buffer.as_slice(), 0, buffer.len()),
                    Err(error) => {
                        errors.push(error);
                        BinaryObject::new_from_src_ptr_len(object.data.as_slice(), object.pointer, object.length())
                    }
                }
            },
            Err(_error) => {
                BinaryObject::new_from_src_ptr_len(object.data.as_slice(), object.pointer, object.length())
            }
        };

        new_binary_objects.push(binary_object);
    }

    amb.objects = new_binary_objects;

    (amb, errors)
}

pub fn recreate_amb_recursively(file: String, save_as_file_name: Option<String>) -> Result<(), CommonBinaryError> {
    do_a_thing_over_an_amb_and_save(
        &file,
        &|amb| {
            let (_, errors) = _recreate_amb_recursively(amb);
            if !errors.is_empty() {
                eprintln!("There were errors while recreating the AMB: {:?}", errors);
            }
        },
        &save_as_file_name.unwrap_or(file.clone()),
    )
}

pub fn recreate_amb_from_dir(dir: String) -> Result<(), CommonBinaryError> {
    let dir_path = Path::new(&dir);
    if !dir_path.is_dir() {
        return Err(CommonBinaryError::Io(std::io::Error::other(format!("Error: {dir:?} is not a directory"))));
    }

    let extracted_prefix = "_extracted";

    let amb_file_path = if dir.ends_with(extracted_prefix) {
        let possible_file = &dir;
        let possible_file = possible_file.chars().take(possible_file.len() - extracted_prefix.len()).collect::<String>();
        Path::new(&possible_file).to_path_buf()
    } else {
        let mut result = PathBuf::new();
        let possible_file = dir_path.join(".amb");
        if possible_file.is_file() {
            result = possible_file
        }

        let possible_file = dir_path.join(".AMB");
        if possible_file.is_file() {
            result = possible_file
        }

        result
    };

    amb_management::add::directory::add_dir_to_amb_from_dir_path(&amb_file_path, dir_path)
}