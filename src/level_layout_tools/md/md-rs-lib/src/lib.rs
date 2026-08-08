#![deny(clippy::unwrap_used)]
pub mod md_set;
pub mod md_entry;
pub mod convert {
    pub mod from_json;
    pub mod to_json;
}

pub static VERSION: &str = env!("CARGO_PKG_VERSION");