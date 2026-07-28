use common_binary::error::CommonBinaryError;
use crate::decoration_set::DecorationSet;

pub fn convert(dc: &DecorationSet) -> Result<String, CommonBinaryError> {
    let result = serde_json::to_string_pretty(dc)?;
    Ok(result)
}