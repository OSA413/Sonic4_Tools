use common_binary::error::CommonBinaryError;
use crate::md_set::MdSet;

pub fn convert(md: &MdSet) -> Result<String, CommonBinaryError> {
    let result = serde_json::to_string_pretty(md)?;
    Ok(result)
}