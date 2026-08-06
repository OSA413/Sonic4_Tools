use crate::error::{CommonBinaryError, PointerOutOfBoundsDetails};

pub fn write(target: &mut [u8], pointer: usize, data: u8, what: String) -> Result<(), CommonBinaryError> {
    if target.len() <= pointer {
        return Err(CommonBinaryError::PointerOutOfBounds(PointerOutOfBoundsDetails { 
            pointer,
            source_len: target.len(),
            when: format!("Writing {}", what).to_string(),
        }))
    }

    target[pointer] = data;
    Ok(())
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_write_le_0() {
        let mut target = [0; 3];
        write(&mut target, 0, 0x12, "test_write_le_0".to_string()).unwrap();
        assert_eq!(target, [0x12, 0, 0]);
    }

    #[test]
    fn test_write_le_1() {
        let mut target = [0; 3];
        write(&mut target, 1, 0x12, "test_write_le_1".to_string()).unwrap();
        assert_eq!(target, [0, 0x12, 0]);
    }

    #[test]
    fn test_write_le_2() {
        let mut target = [0; 3];
        write(&mut target, 2, 0x12, "test_write_le_2".to_string()).unwrap();
        assert_eq!(target, [0, 0, 0x12]);
    }

    #[test]
    fn test_write_le_3() {
        let mut target = [0; 3];
        let result = write(&mut target, 3, 0x12, "test_write_le_3".to_string()).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Writing test_write_le_3 for 3 at 3"
        );
    }

    #[test]
    fn test_write_le_4() {
        let mut target = [0; 3];
        let result = write(&mut target, 99, 0x12, "test_write_le_4".to_string()).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Writing test_write_le_4 for 3 at 99"
        );
    }

    #[test]
    fn test_write_be_0() {
        let mut target = [0; 3];
        write(&mut target, 0, 0x12, "test_write_be_0".to_string()).unwrap();
        assert_eq!(target, [0x12, 0, 0]);
    }

    #[test]
    fn test_write_be_1() {
        let mut target = [0; 3];
        write(&mut target, 1, 0x12, "test_write_be_1".to_string()).unwrap();
        assert_eq!(target, [0, 0x12, 0]);
    }

    #[test]
    fn test_write_be_2() {
        let mut target = [0; 3];
        write(&mut target, 2, 0x12, "test_write_be_2".to_string()).unwrap();
        assert_eq!(target, [0, 0, 0x12]);
    }

    #[test]
    fn test_write_be_3() {
        let mut target = [0; 3];
        let result = write(&mut target, 3, 0x12, "test_write_be_3".to_string()).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Writing test_write_be_3 for 3 at 3"
        );
    }

    #[test]
    fn test_write_be_4() {
        let mut target = [0; 3];
        let result = write(&mut target, 99, 0x12, "test_write_be_4".to_string()).unwrap_err();
        assert_eq!(
            format!("{result:?}"),
            "PointerOutOfBounds when Writing test_write_be_4 for 3 at 99"
        );
    }
}
