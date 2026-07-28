use serde::{Deserialize, Serialize};

use crate::event::Event;

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct EventTile {
    pub events: Vec<Event>,
}