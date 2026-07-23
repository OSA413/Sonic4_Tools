
pub fn print() {
    println!(
        "amb-rs: {}\namb-rs-lib: {}",
        env!("CARGO_PKG_VERSION"),
        amb_rs_lib::VERSION,
    );
}