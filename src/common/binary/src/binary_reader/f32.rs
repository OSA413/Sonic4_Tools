use crate::error::{CommonBinaryError, PointerOutOfBoundsDetails};
use crate::endianness::Endianness;

pub fn read(source: &[u8], pointer: usize, endianness: &Endianness, what: &str) -> Result<f32, CommonBinaryError> {
    // This approach won't eat up the RAM and should be safe and fast
    // And is using Rust's built in conversion to type from binary
    if source.len() < pointer + size_of::<f32>() {
        return Err(CommonBinaryError::PointerOutOfBounds(PointerOutOfBoundsDetails {
            when: format!("reading {what} (f32)"),
            pointer,
            source_len: source.len(),
        }));
    }

    let bytes = [
        source[pointer],
        source[pointer + 1],
        source[pointer + 2],
        source[pointer + 3]
    ];

    match endianness {
        Endianness::Little => Ok(f32::from_le_bytes(bytes)),
        Endianness::Big => Ok(f32::from_be_bytes(bytes)),
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    static SIMPLE_SOURCE: [u8; 6] = [0x0, 0x4B, 0x3C, 0x61, 0x4E, 0x00];

    #[test]
    fn test_read_le_0() {
        assert_eq!(read(&SIMPLE_SOURCE, 0, &Endianness::Little, "test_read_le_0").unwrap(), 2.1708701e+20);
    }

    #[test]
    fn test_read_le_1() {
        assert_eq!(read(&SIMPLE_SOURCE, 1, &Endianness::Little, "test_read_le_1").unwrap(), 944706240_f32);
    }

    #[test]
    fn test_read_le_2() {
        assert_eq!(read(&SIMPLE_SOURCE, 2, &Endianness::Little, "test_read_le_2").unwrap(), 7.19805e-39);
    }

    #[test]
    fn test_read_le_3() {
        let result = read(&SIMPLE_SOURCE, 3, &Endianness::Little, "test_read_le_3").unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when reading test_read_le_3 (f32) for 6 at 3"
        );
    }

    #[test]
    fn test_read_le_4() {
        let result = read(&SIMPLE_SOURCE, 99, &Endianness::Little, "test_read_le_4").unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when reading test_read_le_4 (f32) for 6 at 99"
        );
    }

    #[test]
    fn test_read_be_0() {
        assert_eq!(read(&SIMPLE_SOURCE, 0, &Endianness::Big, "test_read_be_0").unwrap(), 6.909322e-39);
    }

    #[test]
    fn test_read_be_1() {
        assert_eq!(read(&SIMPLE_SOURCE, 1, &Endianness::Big, "test_read_be_1").unwrap(), 12345678_f32);
    }

    #[test]
    fn test_read_be_2() {
        assert_eq!(read(&SIMPLE_SOURCE, 2, &Endianness::Big, "test_read_be_2").unwrap(), 0.013751507);
    }

    #[test]
    fn test_read_be_3() {
        let result = read(&SIMPLE_SOURCE, 3, &Endianness::Big, "test_read_be_3").unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when reading test_read_be_3 (f32) for 6 at 3"
        );
    }
    
    #[test]
    fn test_read_be_4() {
        let result = read(&SIMPLE_SOURCE, 99, &Endianness::Big, "test_read_be_4").unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when reading test_read_be_4 (f32) for 6 at 99"
        );
    }
}