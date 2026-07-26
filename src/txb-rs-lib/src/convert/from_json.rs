use common_binary::error::CommonBinaryError;
use crate::txb::Txb;

pub fn convert(str: &String) -> Result<Txb, CommonBinaryError> {
    let result = serde_json::from_str::<Txb>(str)?;
    Ok(result)
}
