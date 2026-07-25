use serde::{Deserialize, Serialize};

use crate::{gl_texture_mag_filter::GlTextureMagFilter, gl_texture_min_filter::GlTextureMinFilter};

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct TxbObject {
    pub name: String,
    pub min_filter: GlTextureMinFilter,
    pub mag_filter: GlTextureMagFilter,
}

impl TxbObject {
    pub fn new_empty() -> Self {
        Self {
            name: String::new(),
            // According to the OpenGL's specification
            min_filter: GlTextureMinFilter::GlNearestMipmapLinear,
            mag_filter: GlTextureMagFilter::GlLinear,
        }
    }

    pub fn length() -> usize {
        5 * 4
    }
}

impl std::fmt::Display for TxbObject {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        write!(f, "{}", serde_json::to_string(&self).unwrap_or("couldn't represent this TXB object as JSON".to_string()))
    }
}
