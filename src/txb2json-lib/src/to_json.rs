use txb_rs_lib::txb::Txb;

pub fn convert(txb: &Txb) -> Result<String, serde_json::Error> {
    serde_json::to_string_pretty(txb)
}
