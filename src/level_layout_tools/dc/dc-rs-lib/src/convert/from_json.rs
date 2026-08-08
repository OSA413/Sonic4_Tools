use common_binary::error::CommonBinaryError;
use crate::decoration_set::DecorationSet;

pub fn convert(str: &str) -> Result<DecorationSet, CommonBinaryError> {
    let result = serde_json::from_str::<DecorationSet>(str)?;
    Ok(result)
}