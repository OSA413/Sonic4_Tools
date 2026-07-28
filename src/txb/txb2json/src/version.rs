
pub fn print() {
    println!(
        "txb2json: {}\ntxb-rs-lib: {}",
        env!("CARGO_PKG_VERSION"),
        txb_rs_lib::VERSION,
    );
}