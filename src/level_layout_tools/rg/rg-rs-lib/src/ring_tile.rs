use serde::{Deserialize, Serialize};

use crate::ring::RingCoordinates;

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct RingTile {
    pub rings: Vec<RingCoordinates>,
}