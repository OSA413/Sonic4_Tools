use common_binary::error::CommonBinaryError;
use crate::txb::Txb;

pub fn convert(txb: &Txb) -> Result<String, CommonBinaryError> {
    let result = serde_json::to_string_pretty(txb)?;
    Ok(result)
}
