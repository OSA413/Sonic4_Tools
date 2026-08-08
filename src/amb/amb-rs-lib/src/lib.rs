#![deny(clippy::unwrap_used)]
pub mod amb;
pub mod binary_object;

pub static VERSION: &str = env!("CARGO_PKG_VERSION");