use crate::error::{CommonBinaryError, PointerOutOfBoundsDetails};

pub fn read(source: &[u8], pointer: usize, what: &str) -> Result<u8, CommonBinaryError> {
    if source.len() <= pointer {
        return Err(CommonBinaryError::PointerOutOfBounds(PointerOutOfBoundsDetails {
            when: format!("Reading {what} (u8)"),
            pointer,
            source_len: source.len(),
        }));
    }

    Ok(source[pointer])
}

#[cfg(test)]
mod tests {
    use super::*;

    static SIMPLE_SOURCE: [u8; 3] = [0x12, 0x34, 0x56];

    #[test]
    fn test_read_le_0() {
        assert_eq!(read(&SIMPLE_SOURCE, 0, "test_read_le_0").unwrap(), 0x12);
    }

    #[test]
    fn test_read_le_1() {
        assert_eq!(read(&SIMPLE_SOURCE, 1, "test_read_le_1").unwrap(), 0x34);
    }

    #[test]
    fn test_read_le_2() {
        assert_eq!(read(&SIMPLE_SOURCE, 2, "test_read_le_2").unwrap(), 0x56);
    }

    #[test]
    fn test_read_le_3() {
        let result = read(&SIMPLE_SOURCE, 3, "test_read_le_3").unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading an u8 for 3 at 3"
        );
    }

    #[test]
    fn test_read_le_4() {
        let result = read(&SIMPLE_SOURCE, 99, "test_read_le_4").unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading an u8 for 3 at 99"
        );
    }

    #[test]
    fn test_read_be_0() {
        assert_eq!(read(&SIMPLE_SOURCE, 0, "test_read_be_0").unwrap(), 0x12);
    }

    #[test]
    fn test_read_be_1() {
        assert_eq!(read(&SIMPLE_SOURCE, 1, "test_read_be_1").unwrap(), 0x34);
    }

    #[test]
    fn test_read_be_2() {
        assert_eq!(read(&SIMPLE_SOURCE, 2, "test_read_be_2").unwrap(), 0x56);
    }

    #[test]
    fn test_read_be_3() {
        let result = read(&SIMPLE_SOURCE, 3, "test_read_be_3").unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading an u8 for 3 at 3"
        );
    }
    
    #[test]
    fn test_read_be_4() {
        let result = read(&SIMPLE_SOURCE, 99, "test_read_be_4").unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading an u8 for 3 at 99"
        );
    }
}