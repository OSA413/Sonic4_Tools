use common_binary::error::CommonBinaryError;
use crate::md_set::MdSet;

pub fn convert(str: &str) -> Result<MdSet, CommonBinaryError> {
    let result = serde_json::from_str::<MdSet>(str)?;
    Ok(result)
}