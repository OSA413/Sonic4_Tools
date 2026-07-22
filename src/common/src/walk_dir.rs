use std::{ffi::OsStr, path::{Path, PathBuf}};

pub fn walk_dir(dir: &Path, extension: Option<&OsStr>) -> Vec<PathBuf> {
    let mut files: Vec<PathBuf> = vec![];
    match dir.read_dir() {
        Ok(entries) => {
            for entry in entries {
                match entry {
                    Ok(entry) => {
                        let path = entry.path();
                        if path.is_dir() {
                            files.extend(walk_dir(&path, extension));
                        } else {
                            match extension {
                                Some(desired_extension) => {
                                    if let Some(file_extension) = path.extension() {
                                        if file_extension.to_ascii_lowercase() == desired_extension {
                                            files.push(path);
                                        }
                                    } else if desired_extension.is_empty() {
                                        files.push(path);
                                    }
                                },
                                None => files.push(path),
                            }
                        }
                    },
                    Err(e) => eprintln!("Error: {e}"),
                }
            }
        },
        Err(e) => eprintln!("Error: {e}"),
    }
    files
}

pub fn walk_dir_for_dirs(dir: &Path) -> Vec<PathBuf> {
    let mut dirs: Vec<PathBuf> = vec![];
    match dir.read_dir() {
        Ok(entries) => {
            for entry in entries {
                match entry {
                    Ok(entry) => {
                        let path = entry.path();
                        if path.is_dir() {
                            dirs.extend(walk_dir_for_dirs(&path));
                            dirs.push(path);
                        }
                    },
                    Err(e) => eprintln!("Error: {e}"),
                }
            }
        },
        Err(e) => eprintln!("Error: {e}"),
    }
    dirs
}

#[cfg(test)]
mod tests {
    use std::fs;
    use super::*;

    #[test]
    fn test_copy_dir() {
        let temp_dir = std::env::temp_dir();

        let dir = temp_dir.join("common_test/from");

        let _ = fs::remove_dir_all(&dir);
        fs::create_dir_all(&dir).unwrap();

        assert_eq!(walk_dir(&dir, None), Vec::<PathBuf>::new());
        assert_eq!(walk_dir_for_dirs(&dir), Vec::<PathBuf>::new());

        fs::write(&dir.join("test.amb"), "test").unwrap();
        fs::create_dir(&dir.join("test.amb_extracted")).unwrap();
        fs::write(&dir.join("test.amb_extracted/1.txt"), "1").unwrap();
        fs::write(&dir.join("test.amb_extracted/2"), "22").unwrap();
        
        assert_eq!(walk_dir(&dir, None), [
            dir.join("test.amb"),
            dir.join("test.amb_extracted/1.txt"),
            dir.join("test.amb_extracted/2"),
        ]);
        assert_eq!(walk_dir(&dir, Some(OsStr::new("amb"))), [
            dir.join("test.amb"),
        ]);
        assert_eq!(walk_dir(&dir, Some(OsStr::new("txt"))), [
            dir.join("test.amb_extracted/1.txt"),
        ]);
        assert_eq!(walk_dir(&dir, Some(OsStr::new(""))), [
            dir.join("test.amb_extracted/2"),
        ]);
        assert_eq!(walk_dir_for_dirs(&dir), [
            dir.join("test.amb_extracted"),
        ]);
    }
}