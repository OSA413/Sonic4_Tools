use std::io::Read;

use crate::{common::ALLOWED_CHARACTER_RANGES, error::{CommonBinaryError, PointerOutOfBoundsDetails, StringBadCharacterDetails}};

pub fn write(target: &mut [u8], pointer: usize, data: &str, what: &str) -> Result<(), CommonBinaryError> {
    let data_len = data.len();
    let end = pointer + data_len;

    if end > target.len() {
        return Err(CommonBinaryError::PointerOutOfBounds(PointerOutOfBoundsDetails {
            pointer,
            source_len: target.len(),
            when: format!("Writing {} (string32)", what).to_string(),
        }));
    }

    for character in data.as_bytes() {
        if !(ALLOWED_CHARACTER_RANGES).iter().any(|range| range.contains(&character)) {
            return Err(CommonBinaryError::StringBadCharacter(StringBadCharacterDetails {
                pointer,
                target_string: data.to_string(),
                bad_character: character.clone(),
                when: "Writing a string".to_string(),
            }));
        }
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
        write(&mut target, 0, MAGIC, "test_write_magic_0").unwrap();
        assert_eq!(target, [0x4D, 0x41, 0x47, 0x49, 0x43, 0]);
    }

    #[test]
    fn test_write_magic_1() {
        let mut target = [0; 6];
        write(&mut target, 1, MAGIC, "test_write_magic_1").unwrap();
        assert_eq!(target, [0, 0x4D, 0x41, 0x47, 0x49, 0x43]);
    }

    #[test]
    fn test_write_magic_2() {
        let mut target = [0; 6];
        let result = write(&mut target, 2, MAGIC, "test_write_magic_2").unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Writing test_write_magic_2 for 6 at 2"
        );
    }

    #[test]
    fn test_write_magic_3() {
        let mut target = [0; 6];
        let result = write(&mut target, 99, MAGIC, "test_write_magic_3").unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Writing test_write_magic_3 for 6 at 99"
        );
    }
}

