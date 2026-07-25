
pub fn print() {
    println!(
        "txb2json: {}\ntxb2json-lib: {}\ntxb-rs-lib: {}",
        env!("CARGO_PKG_VERSION"),
        txb2json_lib::VERSION,
        txb_rs_lib::VERSION,
    );
}