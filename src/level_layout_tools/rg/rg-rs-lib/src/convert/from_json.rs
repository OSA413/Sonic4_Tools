use common_binary::error::CommonBinaryError;
use crate::ring_set::RingSet;

pub fn convert(str: &String) -> Result<RingSet, CommonBinaryError> {
    let result = serde_json::from_str::<RingSet>(str)?;
    Ok(result)
}