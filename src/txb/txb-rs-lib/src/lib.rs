#![deny(clippy::unwrap_used)]
pub mod txb;
pub mod txb_object;
pub mod gl_texture_mag_filter;
pub mod gl_texture_min_filter;
pub mod convert {
    pub mod from_json;
    pub mod to_json;
}

pub static VERSION: &str = env!("CARGO_PKG_VERSION");