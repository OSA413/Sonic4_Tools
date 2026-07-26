use common_binary::error::CommonBinaryError;
use crate::mp_set::MpSet;

pub fn convert(mp: &MpSet) -> Result<String, CommonBinaryError> {
    let result = serde_json::to_string_pretty(mp)?;
    Ok(result)
}