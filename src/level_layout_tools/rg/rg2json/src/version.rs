pub fn print() {
    println!(
        "rg2json: {}\nrg-rs-lib: {}",
        env!("CARGO_PKG_VERSION"),
        rg_rs_lib::VERSION,
    );
}