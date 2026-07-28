use common_binary::error::CommonBinaryError;
use crate::md_set::MdSet;

pub fn convert(str: &String) -> Result<MdSet, CommonBinaryError> {
    let result = serde_json::from_str::<MdSet>(str)?;
    Ok(result)
}