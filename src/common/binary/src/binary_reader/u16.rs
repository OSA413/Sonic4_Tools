use crate::error::{CommonBinaryError, PointerOutOfBoundsDetails};
use crate::endianness::Endianness;

pub fn read(source: &[u8], pointer: usize, endianness: &Endianness) -> Result<u16, CommonBinaryError> {
    // This approach won't eat up the RAM and should be safe and fast
    // And is using Rust's built in conversion to type from binary
    if source.len() < pointer + size_of::<u16>() {
        return Err(CommonBinaryError::PointerOutOfBounds(PointerOutOfBoundsDetails {
            when: "Reading an u16".to_string(),
            pointer,
            source_len: source.len(),
        }));
    }

    let bytes = [
        source[pointer],
        source[pointer + 1]
    ];

    match endianness {
        Endianness::Little => Ok(u16::from_le_bytes(bytes)),
        Endianness::Big => Ok(u16::from_be_bytes(bytes)),
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    static SIMPLE_SOURCE: [u8; 4] = [0x12, 0x34, 0x56, 0x78];

    #[test]
    fn test_read_le_0() {
        assert_eq!(read(&SIMPLE_SOURCE, 0, &Endianness::Little).unwrap(), 0x3412);
    }

    #[test]
    fn test_read_le_1() {
        assert_eq!(read(&SIMPLE_SOURCE, 1, &Endianness::Little).unwrap(), 0x5634);
    }

    #[test]
    fn test_read_le_2() {
        assert_eq!(read(&SIMPLE_SOURCE, 2, &Endianness::Little).unwrap(), 0x7856);
    }

    #[test]
    fn test_read_le_3() {
        let result = read(&SIMPLE_SOURCE, 3, &Endianness::Little).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading an u16 for 4 at 3"
        );
    }

    #[test]
    fn test_read_le_4() {
        let result = read(&SIMPLE_SOURCE, 99, &Endianness::Little).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading an u16 for 4 at 99"
        );
    }

    #[test]
    fn test_read_be_0() {
        assert_eq!(read(&SIMPLE_SOURCE, 0, &Endianness::Big).unwrap(), 0x1234);
    }

    #[test]
    fn test_read_be_1() {
        assert_eq!(read(&SIMPLE_SOURCE, 1, &Endianness::Big).unwrap(), 0x3456);
    }

    #[test]
    fn test_read_be_2() {
        assert_eq!(read(&SIMPLE_SOURCE, 2, &Endianness::Big).unwrap(), 0x5678);
    }

    #[test]
    fn test_read_be_3() {
        let result = read(&SIMPLE_SOURCE, 3, &Endianness::Big).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading an u16 for 4 at 3"
        );
    }
    
    #[test]
    fn test_read_be_4() {
        let result = read(&SIMPLE_SOURCE, 99, &Endianness::Big).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Reading an u16 for 4 at 99"
        );
    }
}