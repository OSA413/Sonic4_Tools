pub mod event_set;
pub mod event_tile;
pub mod event;
pub mod convert {
    pub mod from_json;
    pub mod to_json;
}

pub static VERSION: &str = env!("CARGO_PKG_VERSION");