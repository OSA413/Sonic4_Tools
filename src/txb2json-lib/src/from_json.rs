use txb_rs_lib::txb::Txb;

pub fn convert(str: &String) -> Result<Txb, serde_json::Error> {
    serde_json::from_str::<Txb>(str)
}
