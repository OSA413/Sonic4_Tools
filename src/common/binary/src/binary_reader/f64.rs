use crate::error::{CommonBinaryError, PointerOutOfBoundsDetails};
use crate::endianness::Endianness;

pub fn read(source: &[u8], pointer: usize, endianness: &Endianness, what: &str) -> Result<f64, CommonBinaryError> {
    // This approach won't eat up the RAM and should be safe and fast
    // And is using Rust's built in conversion to type from binary
    if source.len() < pointer + size_of::<f64>() {
        return Err(CommonBinaryError::PointerOutOfBounds(PointerOutOfBoundsDetails {
            when: format!("reading {what} (f64)"),
            pointer,
            source_len: source.len(),
        }));
    }

    let bytes = [
        source[pointer],
        source[pointer + 1],
        source[pointer + 2],
        source[pointer + 3],
        source[pointer + 4],
        source[pointer + 5],
        source[pointer + 6],
        source[pointer + 7]
    ];

    match endianness {
        Endianness::Little => Ok(f64::from_le_bytes(bytes)),
        Endianness::Big => Ok(f64::from_be_bytes(bytes)),
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    static SIMPLE_SOURCE: [u8; 10] = [0x00, 0x43, 0xE5, 0x6A, 0x95, 0x31, 0x9D, 0x63, 0xE1, 0x00];

    #[test]
    fn test_read_le_0() {
        assert_eq!(read(&SIMPLE_SOURCE, 0, &Endianness::Little, "test_read_le_0").unwrap(), 7.051255980114156e171);
    }

    #[test]
    fn test_read_le_1() {
        assert_eq!(read(&SIMPLE_SOURCE, 1, &Endianness::Little, "test_read_le_1").unwrap(), -1.3787791467183283e161);
    }

    #[test]
    fn test_read_le_2() {
        assert_eq!(read(&SIMPLE_SOURCE, 2, &Endianness::Little, "test_read_le_2").unwrap(), 1.981034004750801e-304);
    }

    #[test]
    fn test_read_le_3() {
        let result = read(&SIMPLE_SOURCE, 3, &Endianness::Little, "test_read_le_3").unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when reading test_read_le_3 (f64) for 10 at 3"
        );
    }

    #[test]
    fn test_read_le_4() {
        let result = read(&SIMPLE_SOURCE, 99, &Endianness::Little, "test_read_le_4").unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when reading test_read_le_4 (f64) for 10 at 99"
        );
    }

    #[test]
    fn test_read_be_0() {
        assert_eq!(read(&SIMPLE_SOURCE, 0, &Endianness::Big, "test_read_be_0").unwrap(), 2.213521005164029e-307);
    }

    #[test]
    fn test_read_be_1() {
        assert_eq!(read(&SIMPLE_SOURCE, 1, &Endianness::Big, "test_read_be_1").unwrap(), 12345678901234567890_f64);
    }

    #[test]
    fn test_read_be_2() {
        assert_eq!(read(&SIMPLE_SOURCE, 2, &Endianness::Big, "test_read_be_2").unwrap(), -3.447052936383801e180);
    }

    #[test]
    fn test_read_be_3() {
        let result = read(&SIMPLE_SOURCE, 3, &Endianness::Big, "test_read_be_3").unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when reading test_read_be_3 (f64) for 10 at 3"
        );
    }
    
    #[test]
    fn test_read_be_4() {
        let result = read(&SIMPLE_SOURCE, 99, &Endianness::Big, "test_read_be_4").unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when reading test_read_be_4 (f64) for 10 at 99"
        );
    }
}