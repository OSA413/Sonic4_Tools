use serde::{Deserialize, Serialize};

use crate::decoration::Decoration;

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct DecorationTile {
    pub decorations: Vec<Decoration>,
}
