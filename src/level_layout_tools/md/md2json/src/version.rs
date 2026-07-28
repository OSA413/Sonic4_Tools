pub fn print() {
    println!(
        "md2json: {}\nmd-rs-lib: {}",
        env!("CARGO_PKG_VERSION"),
        md_rs_lib::VERSION,
    );
}