pub fn print() {
    println!(
        "mp2json: {}\nmp-rs-lib: {}",
        env!("CARGO_PKG_VERSION"),
        mp_rs_lib::VERSION,
    );
}