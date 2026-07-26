use common_binary::error::CommonBinaryError;
use crate::ring_set::RingSet;

pub fn convert(rg: &RingSet) -> Result<String, CommonBinaryError> {
    let result = serde_json::to_string_pretty(rg)?;
    Ok(result)
}