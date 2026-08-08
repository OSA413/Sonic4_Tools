use std::io::Read;

use crate::error::{CommonBinaryError, PointerOutOfBoundsDetails};

// TODO: add check for incorrect symbols like in the reader
pub fn write(target: &mut [u8], pointer: usize, data: &str, what: String) -> Result<(), CommonBinaryError> {
    let data_len = data.len();
    let end = pointer + data_len;

    if end > target.len() {
        return Err(CommonBinaryError::PointerOutOfBounds(PointerOutOfBoundsDetails {
            pointer,
            source_len: target.len(),
            when: format!("Writing {}", what).to_string(),
        }));
    }

    data.as_bytes().read_exact(&mut target[pointer..end])?;

    Ok(())
}

#[cfg(test)]
mod tests {
    use super::*;

    static MAGIC: &str = "MAGIC";

    #[test]
    fn test_write_magic_0() {
        let mut target = [0; 6];
        write(&mut target, 0, MAGIC, "test_write_magic_0".to_string()).unwrap();
        assert_eq!(target, [0x4D, 0x41, 0x47, 0x49, 0x43, 0]);
    }

    #[test]
    fn test_write_magic_1() {
        let mut target = [0; 6];
        write(&mut target, 1, MAGIC, "test_write_magic_1".to_string()).unwrap();
        assert_eq!(target, [0, 0x4D, 0x41, 0x47, 0x49, 0x43]);
    }

    #[test]
    fn test_write_magic_2() {
        let mut target = [0; 6];
        let result = write(&mut target, 2, MAGIC, "test_write_magic_2".to_string()).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Writing test_write_magic_2 for 6 at 2"
        );
    }

    #[test]
    fn test_write_magic_3() {
        let mut target = [0; 6];
        let result = write(&mut target, 99, MAGIC, "test_write_magic_3".to_string()).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Writing test_write_magic_3 for 6 at 99"
        );
    }
}

