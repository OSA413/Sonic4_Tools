use std::{fs, path::{Path, PathBuf}};

use crate::error::IoError;

pub fn copy_dir(from: &PathBuf, to: &PathBuf) -> Result<(), Vec<IoError>> {
    match fs::create_dir_all(to) {
        Ok(()) => {
            let mut errors = Vec::new();
            match fs::read_dir(from) {
                Ok(entries) => {
                    for entry in entries {
                        match entry {
                            Ok(entry) => {
                                let file_type = entry.file_type();
                                match file_type {
                                    Ok(file_type) => {
                                        if file_type.is_dir() {
                                            match copy_dir(&entry.path(), &Path::new(to).join(entry.file_name())) {
                                                Ok(_) => {},
                                                Err(e) => errors.extend(e),
                                            }
                                        } else {
                                            match fs::copy(&entry.path(), &Path::new(to).join(entry.file_name())) {
                                                Ok(_) => {},
                                                Err(e) => errors.push(IoError{cause: e, description: "Couldn't copy file"})
                                            }
                                        }
                                    }
                                    Err(e) => errors.push(IoError{cause: e, description: "Couldn't retrieve file type"})
                                }
                            },
                            Err(e) => errors.push(IoError{cause: e, description: "Couldn't get directory entry"}),
                        }
                    }
                }
                Err(e) => errors.push(IoError{cause: e, description: "Error reading directory"}),
            }

            if errors.is_empty() {
                return Ok(());
            } else {
                return Err(errors);
            }
        }
        Err(e) => {
            let mut errors = Vec::new();
            errors.push(IoError{cause: e, description: "Error creating all directories"});
            Err(errors)
        }
    }
}

#[cfg(test)]
mod tests {
    use crate::walk_dir;

    use super::*;

    #[test]
    fn test_copy_dir() {
        let temp_dir = std::env::temp_dir();

        let from = temp_dir.join("common_test/copy_dir_from");
        let to = temp_dir.join("common_test/copy_dir_to");

        let _ = fs::remove_dir_all(&from);
        let _ = fs::remove_dir_all(&to);
        fs::create_dir_all(&from).unwrap();

        fs::write(&from.join("test.amb"), "test").unwrap();
        fs::create_dir(&from.join("test.amb_extracted")).unwrap();
        fs::write(&from.join("test.amb_extracted/1.txt"), "1").unwrap();
        fs::write(&from.join("test.amb_extracted/2"), "22").unwrap();

        let walk_dir_before = walk_dir::walk_dir(&to, None);
        let walk_dir_before_dir = walk_dir::walk_dir_for_dirs(&to);
        assert_eq!(walk_dir_before, Vec::<PathBuf>::new());
        assert_eq!(walk_dir_before_dir, Vec::<PathBuf>::new());

        copy_dir(&from, &to).unwrap();
        
        let walk_dir_after = walk_dir::walk_dir(&to, None);
        let walk_dir_after_dir = walk_dir::walk_dir_for_dirs(&to);
        assert_eq!(walk_dir_after, [
            to.join("test.amb"),
            to.join("test.amb_extracted/1.txt"),
            to.join("test.amb_extracted/2"),
        ]);
        assert_eq!(walk_dir_after_dir, [
            to.join("test.amb_extracted"),
        ]);
    }
}