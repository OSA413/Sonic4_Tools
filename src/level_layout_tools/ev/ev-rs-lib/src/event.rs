use serde::{Deserialize, Serialize};

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct Event {
    pub unknown1: u16,
    pub unknown2: u16,
    pub unknown3: u16,
    pub unknown4: u16,
    pub unknown5: u16,
    pub unknown6: u16,
}