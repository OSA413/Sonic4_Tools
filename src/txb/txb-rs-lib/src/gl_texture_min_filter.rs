use serde::{Deserialize, Serialize};

// https://www.khronos.org/registry/OpenGL-Refpages/es2.0/xhtml/glTexParameter.xml
#[derive(Debug, Clone, Copy, PartialEq, Eq)]
#[repr(u16)]
pub enum GlTextureMinFilter {
    GlNearest = 0,
    GlLinear = 1,
    GlNearestMipmapNearest = 2,
    GlLinearMipmapNearest = 3,
    GlNearestMipmapLinear = 4,
    GlLinearMipmapLinear = 5,
    Unknown(u16),
}

// Unfortunately I couldn't find a way to have the u16 enum and a "non-unit" Unknown variant to support anything that I couldn't think of
// without writing a lot of code (I mean a custom Deserialize and Serialize implementation)
// Despite the fact that I have the discriminant values in the enum
impl From<GlTextureMinFilter> for u16 {
    fn from(value: GlTextureMinFilter) -> Self {
        match value {
            GlTextureMinFilter::GlNearest => 0,
            GlTextureMinFilter::GlLinear => 1,
            GlTextureMinFilter::GlNearestMipmapNearest => 2,
            GlTextureMinFilter::GlLinearMipmapNearest => 3,
            GlTextureMinFilter::GlNearestMipmapLinear => 4,
            GlTextureMinFilter::GlLinearMipmapLinear => 5,
            GlTextureMinFilter::Unknown(value) => value,
        }
    }
}

impl From<u16> for GlTextureMinFilter {
    fn from(value: u16) -> Self {
        match value {
            0 => GlTextureMinFilter::GlNearest,
            1 => GlTextureMinFilter::GlLinear,
            2 => GlTextureMinFilter::GlNearestMipmapNearest,
            3 => GlTextureMinFilter::GlLinearMipmapNearest,
            4 => GlTextureMinFilter::GlNearestMipmapLinear,
            5 => GlTextureMinFilter::GlLinearMipmapLinear,
            _ => GlTextureMinFilter::Unknown(value),
        }
    }
}

impl Serialize for GlTextureMinFilter {
    fn serialize<S>(&self, serializer: S) -> Result<S::Ok, S::Error>
    where
        S: serde::Serializer,
    {
        serializer.serialize_u16(u16::from(self.clone()))
    }
}

impl<'de> Deserialize<'de> for GlTextureMinFilter {
    fn deserialize<D>(deserializer: D) -> Result<Self, D::Error>
    where
        D: serde::Deserializer<'de>,
    {
        let value = u16::deserialize(deserializer)?;
        Ok(GlTextureMinFilter::from(value))
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn from_json() {
        assert_eq!(GlTextureMinFilter::GlNearest, serde_json::from_str("0").unwrap());
        assert_eq!(GlTextureMinFilter::GlLinear, serde_json::from_str("1").unwrap());
        assert_eq!(GlTextureMinFilter::Unknown(99), serde_json::from_str("99").unwrap());
    }

    #[test]
    fn to_json() {
        assert_eq!(serde_json::to_string(&GlTextureMinFilter::GlNearest).unwrap(), "0");
        assert_eq!(serde_json::to_string(&GlTextureMinFilter::GlLinear).unwrap(), "1");
        assert_eq!(serde_json::to_string(&GlTextureMinFilter::Unknown(99)).unwrap(), "99");
    }
}
