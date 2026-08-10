use crate::{endianness::Endianness, error::{CommonBinaryError, PointerOutOfBoundsDetails}};

pub fn write(target: &mut [u8], pointer: usize, data: u16, endianness: &Endianness, what: &str) -> Result<(), CommonBinaryError> {
    if target.len() < pointer + size_of::<u16>() {
        return Err(CommonBinaryError::PointerOutOfBounds(PointerOutOfBoundsDetails { 
            pointer,
            source_len: target.len(),
            when: format!("writing {} (u16)", what),
        }))
    }

    let bytes = match endianness {
        Endianness::Little => data.to_le_bytes(),
        Endianness::Big => data.to_be_bytes(),
    };

    target[pointer..pointer+size_of::<u16>()].copy_from_slice(&bytes);
    Ok(())
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_write_le_0() {
        let mut target = [0; 4];
        write(&mut target, 0, 0x1234, &Endianness::Little, "test_write_le_0").unwrap();
        assert_eq!(target, [0x34, 0x12, 0, 0]);
    }

    #[test]
    fn test_write_le_1() {
        let mut target = [0; 4];
        write(&mut target, 1, 0x1234, &Endianness::Little, "test_write_le_1").unwrap();
        assert_eq!(target, [0, 0x34, 0x12, 0]);
    }

    #[test]
    fn test_write_le_2() {
        let mut target = [0; 4];
        write(&mut target, 2, 0x1234, &Endianness::Little, "test_write_le_2").unwrap();
        assert_eq!(target, [0, 0, 0x34, 0x12]);
    }

    #[test]
    fn test_write_le_3() {
        let mut target = [0; 4];
        let result = write(&mut target, 3, 0x1234, &Endianness::Little, "test_write_le_3").unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when writing test_write_le_3 (u16) for 4 at 3"
        );
    }

    #[test]
    fn test_write_le_4() {
        let mut target = [0; 4];
        let result = write(&mut target, 99, 0x1234, &Endianness::Little, "test_write_le_4").unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when writing test_write_le_4 (u16) for 4 at 99"
        );
    }

    #[test]
    fn test_write_be_0() {
        let mut target = [0; 4];
        write(&mut target, 0, 0x1234, &Endianness::Big, "test_write_be_0").unwrap();
        assert_eq!(target, [0x12, 0x34, 0, 0]);
    }

    #[test]
    fn test_write_be_1() {
        let mut target = [0; 4];
        write(&mut target, 1, 0x1234, &Endianness::Big, "test_write_be_1").unwrap();
        assert_eq!(target, [0, 0x12, 0x34, 0]);
    }

    #[test]
    fn test_write_be_2() {
        let mut target = [0; 4];
        write(&mut target, 2, 0x1234, &Endianness::Big, "test_write_be_2").unwrap();
        assert_eq!(target, [0, 0, 0x12, 0x34]);
    }

    #[test]
    fn test_write_be_3() {
        let mut target = [0; 4];
        let result = write(&mut target, 3, 0x1234, &Endianness::Big, "test_write_be_3").unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when writing test_write_be_3 (u16) for 4 at 3"
        );
    }

    #[test]
    fn test_write_be_4() {
        let mut target = [0; 4];
        let result = write(&mut target, 99, 0x1234, &Endianness::Big, "test_write_be_4").unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when writing test_write_be_4 (u16) for 4 at 99"
        );
    }
}
