use serde::{Deserialize, Serialize};

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct RingCoordinates {
    /** X coordinate is from left to right */
    pub x: u8,
    /** Y coordinate is from up to down */
    pub y: u8,
}
