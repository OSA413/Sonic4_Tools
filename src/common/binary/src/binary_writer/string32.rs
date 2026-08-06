use std::io::Read;

use crate::error::{CommonBinaryError};

// TODO: unpanic and cover with tests
pub fn write(target: &mut [u8], pointer: usize, data: &str, what: String) -> Result<(), CommonBinaryError> {
    let result = data.as_bytes().read_exact(&mut target[pointer..pointer + data.len()])?;
    Ok(result)
}