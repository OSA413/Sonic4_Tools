#![deny(clippy::unwrap_used)]
pub mod decoration_set;
pub mod decoration_tile;
pub mod decoration;
pub mod convert {
    pub mod from_json;
    pub mod to_json;
}

pub static VERSION: &str = env!("CARGO_PKG_VERSION");