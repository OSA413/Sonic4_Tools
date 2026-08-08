use serde::{Deserialize, Serialize};

// https://www.khronos.org/registry/OpenGL-Refpages/es2.0/xhtml/glTexParameter.xml
#[derive(Debug, Clone, Copy, PartialEq, Eq)]
#[repr(u16)]
pub enum GlTextureMagFilter {
    GlNearest = 0,
    GlLinear = 1,
    Unknown(u16),
}

// Unfortunately I couldn't find a way to have the u16 enum and a "non-unit" Unknown variant to support anything that I couldn't think of
// without writing a lot of code (I mean a custom Deserialize and Serialize implementation)
// Despite the fact that I have the discriminant values in the enum
impl From<GlTextureMagFilter> for u16 {
    fn from(value: GlTextureMagFilter) -> Self {
        match value {
            GlTextureMagFilter::GlNearest => 0,
            GlTextureMagFilter::GlLinear => 1,
            GlTextureMagFilter::Unknown(value) => value,
        }
    }
}

impl From<u16> for GlTextureMagFilter {
    fn from(value: u16) -> Self {
        match value {
            0 => GlTextureMagFilter::GlNearest,
            1 => GlTextureMagFilter::GlLinear,
            _ => GlTextureMagFilter::Unknown(value),
        }
    }
}

impl Serialize for GlTextureMagFilter {
    fn serialize<S>(&self, serializer: S) -> Result<S::Ok, S::Error>
    where
        S: serde::Serializer,
    {
        serializer.serialize_u16(u16::from(*self))
    }
}

impl<'de> Deserialize<'de> for GlTextureMagFilter {
    fn deserialize<D>(deserializer: D) -> Result<Self, D::Error>
    where
        D: serde::Deserializer<'de>,
    {
        let value = u16::deserialize(deserializer)?;
        Ok(GlTextureMagFilter::from(value))
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn from_json() {
        assert_eq!(GlTextureMagFilter::GlNearest, serde_json::from_str("0").unwrap());
        assert_eq!(GlTextureMagFilter::GlLinear, serde_json::from_str("1").unwrap());
        assert_eq!(GlTextureMagFilter::Unknown(99), serde_json::from_str("99").unwrap());
    }

    #[test]
    fn to_json() {
        assert_eq!(serde_json::to_string(&GlTextureMagFilter::GlNearest).unwrap(), "0");
        assert_eq!(serde_json::to_string(&GlTextureMagFilter::GlLinear).unwrap(), "1");
        assert_eq!(serde_json::to_string(&GlTextureMagFilter::Unknown(99)).unwrap(), "99");
    }
}