pub fn print() {
    println!(
        "dc2json: {}\ndc-rs-lib: {}",
        env!("CARGO_PKG_VERSION"),
        dc_rs_lib::VERSION,
    );
}