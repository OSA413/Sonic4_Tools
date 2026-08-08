#![deny(clippy::unwrap_used)]
pub mod ring_set;
pub mod ring_tile;
pub mod ring;
pub mod convert {
    pub mod from_json;
    pub mod to_json;
}

pub static VERSION: &str = env!("CARGO_PKG_VERSION");