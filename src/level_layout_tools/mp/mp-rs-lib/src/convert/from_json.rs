use common_binary::error::CommonBinaryError;
use crate::mp_set::MpSet;

pub fn convert(str: &String) -> Result<MpSet, CommonBinaryError> {
    let result = serde_json::from_str::<MpSet>(str)?;
    Ok(result)
}