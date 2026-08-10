use std::{ffi::OsStr, fs, path::Path};
use amb_rs_lib::amb::Amb;
use common_utils::walk_dir;
use common_binary::error::{CommonBinaryError, IoDetails};

fn continue_extraction(amb: Amb, base_dir: &Path) -> Result<(), Vec<CommonBinaryError>> {
    let mut errors = Vec::new();
    
    for binary_object in amb.objects {
        // We do this because some of the files inside Episode 1 have no names and are AMB
        let probably_amb = Amb::new_from_binary_object(&binary_object);
        match probably_amb {
            Ok(inner_amb) => {
                println!("Extracting {base_dir:?}");
                match continue_extraction(inner_amb, &base_dir.join(&binary_object.name)) {
                    Ok(_) => (),
                    Err(e) => errors.extend(e)
                }
            }
            Err(_) => {
                let file_path = base_dir.join(&binary_object.name);
                let created_dirs = match file_path.parent() {
                    Some(parent) => fs::create_dir_all(parent),
                    // Here we are at the root of the drive/fs
                    None => Ok(()),
                };

                // This probably can be minimized using unwrap_or or something
                match created_dirs {
                    Ok(_) => match fs::write(file_path, &binary_object.data) {
                        Ok(_) => (),
                        Err(e) => errors.push(CommonBinaryError::IoDetracked(IoDetails { cause: e, description: "Failed to write file" })),
                    },
                    Err(e) => errors.push(CommonBinaryError::IoDetracked(IoDetails { cause: e, description: "Failed to create directory" })),
                }
            },
        }
    }

    if errors.is_empty() {
        Ok(())
    } else {
        Err(errors)
    }
}

pub fn extract_amb(file_or_dir: String, destination: Option<String>) -> Result<(), CommonBinaryError> {
    let path = Path::new(&file_or_dir);
    let probably_amb_files = if path.is_file() {
        vec![path.to_path_buf()]
    } else {
        walk_dir::walk_dir(path, Some(OsStr::new("amb")))
    };

    let mut errors = Vec::new();
    for entry in probably_amb_files {
        let path = entry.as_path().to_string_lossy().to_string();
        println!("Extracting {path}");
        let amb = Amb::new_from_file_name(&path)?;
        let base_dir = match destination {
            Some(ref destination) => destination.clone(),
            None => format!("{path}_extracted")
        };
        match continue_extraction(amb, Path::new(&base_dir)) {
            Ok(()) => {},
            Err(e) => errors.extend(e),
        }
    }
    
    println!("Done!");
    if errors.is_empty() {
        Ok(())
    } else {
        eprintln!("Errors: {:?}", errors);
        Err(CommonBinaryError::SeeConsole())
    }
}

#[cfg(test)]
mod tests {
    use std::fs;
    use common_utils::walk_dir;
    use super::*;

    macro_rules! extract_tests {
        ($($name:ident: $value:expr,)*) => {
            $(
                #[test]
                fn $name() {
                    let (source_ref, expected_objects, expected_directories): (&str, Vec<(&str, &str)>, usize) = $value;

                    let file_path = format!("../amb-rs-tests/tests/reference_files/le/{source_ref}");
                    let temp_dir = std::env::temp_dir().join("amb-rs-tests").join(format!("extract_test_all_{source_ref}"));
                    let temp_dir_str = temp_dir.join("extraction_result").display().to_string();

                    // BEFORE, it may fail because the dir may not exist
                    let _ = fs::remove_dir_all(&temp_dir);

                    // TEST
                    extract_amb(file_path, Some(temp_dir_str.clone())).unwrap();

                    for (name, data_path) in &expected_objects {
                        assert_eq!(
                            fs::read(format!("../amb-rs-tests/{data_path}")).unwrap(),
                            fs::read(format!("{temp_dir_str}/{name}")).unwrap()
                        );
                    }

                    let dir_files = walk_dir::walk_dir(&temp_dir, None);
                    let dir_dirs = walk_dir::walk_dir_for_dirs(&temp_dir);

                    assert_eq!(dir_files.len(), expected_objects.len());
                    assert_eq!(dir_dirs.len(), expected_directories);

                    // AFTER, must not fail, or else something is wrong
                    if !expected_objects.is_empty() {
                        fs::remove_dir_all(&temp_dir).unwrap();
                    }
                }
            )*
        }
    }

    extract_tests! {
        extract_from_empty: (
            "add_empty.amb",
            vec![],
            0
        ),
        extract_from_1: (
            "add_1.amb",
            vec![("1", "test_files/files/1")],
            1
        ),
        extract_from_2: (
            "add_2.amb",
            vec![("2", "test_files/files/2")],
            1
        ),
        extract_from_3: (
            "add_3.amb",
            vec![("3", "test_files/files/3")],
            1
        ),
        extract_from_1_2: (
            "add_1_2.amb",
            vec![
                ("1", "test_files/files/1"),
                ("2", "test_files/files/2"),
            ],
            1
        ),
        extract_from_1_3: (
            "add_1_3.amb",
            vec![
                ("1", "test_files/files/1"),
                ("3", "test_files/files/3"),
            ],
            1
        ),
        extract_from_2_3: (
            "add_2_3.amb",
            vec![
                ("2", "test_files/files/2"),
                ("3", "test_files/files/3"),
            ],
            1
        ),
        extract_from_1_2_3: (
            "add_1_2_3.amb",
            vec![
                ("1", "test_files/files/1"),
                ("2", "test_files/files/2"),
                ("3", "test_files/files/3"),
            ],
            1
        ),

        // Shuffled content doesn't affect the extraction result
        extract_from_3_2_1: (
            "add_3_2_1.amb",
            vec![
                ("1", "test_files/files/1"),
                ("2", "test_files/files/2"),
                ("3", "test_files/files/3"),
            ],
            1
        ),
    }
}