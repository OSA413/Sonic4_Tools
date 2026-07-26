use common_binary::error::CommonBinaryError;
use crate::event_set::EventSet;

pub fn convert(ev: &EventSet) -> Result<String, CommonBinaryError> {
    let result = serde_json::to_string_pretty(ev)?;
    Ok(result)
}