pub fn print() {
    println!(
        "ev2json: {}\nev-rs-lib: {}",
        env!("CARGO_PKG_VERSION"),
        ev_rs_lib::VERSION,
    );
}