use common_binary::error::CommonBinaryError;
use crate::event_set::EventSet;

pub fn convert(str: &String) -> Result<EventSet, CommonBinaryError> {
    let result = serde_json::from_str::<EventSet>(str)?;
    Ok(result)
}