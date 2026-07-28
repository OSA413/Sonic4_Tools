use std::{fs, path::{Path, PathBuf}};

// TODO: fix panics
pub fn copy_dir(from: &PathBuf, to: &PathBuf) {
    fs::create_dir_all(to).unwrap();
    match fs::read_dir(from) {
        Ok(entries) => {
            for entry in entries {
                match entry {
                    Ok(entry) => {
                        let file_type = entry.file_type().unwrap();
                        if file_type.is_dir() {
                            copy_dir(&entry.path(), &Path::new(to).join(entry.file_name()));
                        } else {
                            fs::copy(&entry.path(), &Path::new(to).join(entry.file_name())).unwrap();
                        }
                    },
                    Err(e) => eprintln!("Error: {e}"),
                }
            }
        }
        Err(e) => eprintln!("Error: {e}"),
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

        copy_dir(&from, &to);
        
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