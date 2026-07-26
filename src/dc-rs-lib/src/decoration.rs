use serde::{Deserialize, Serialize};

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct Decoration {
    pub unknown1: u16,
    pub unknown2: u16,
}
