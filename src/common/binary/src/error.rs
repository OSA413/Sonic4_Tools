pub struct PointerOutOfBoundsDetails {
    pub pointer: usize,
    pub source_len: usize,
    pub when: String,
}

pub struct StringBadCharacterDetails {
    pub pointer: usize,
    pub target_string: String,
    pub bad_character: u8,
    pub when: String,
}

pub struct StringTooLongDetails {
    pub pointer: usize,
    pub target_string: String,
    pub when: String,
}

pub struct IoDetails {
    pub cause: std::io::Error,
    pub description: &'static str,
}

impl std::fmt::Debug for IoDetails {
    fn fmt(&self, f: &mut std::fmt::Formatter) -> std::fmt::Result {
        writeln!(f, "{}: {}", self.description, self.cause)
    }
}

pub enum CommonBinaryError {
    SeeConsole(),
    Io(std::io::Error),
    IoDetracked(IoDetails),
    SerdeJson(serde_json::error::Error),
    PointerOutOfBounds(PointerOutOfBoundsDetails),
    ProvidedSourceIsNotOfExpectedFormat(String),
    StringTooLong(StringTooLongDetails),
    StringBadCharacter(StringBadCharacterDetails),
}

impl From<IoDetails> for CommonBinaryError {
    fn from(e: IoDetails) -> Self {
        Self::IoDetracked(e)
    }
}

impl From<std::io::Error> for CommonBinaryError {
    fn from(e: std::io::Error) -> Self {
        Self::Io(e)
    }
}

impl From<serde_json::error::Error> for CommonBinaryError {
    fn from(e: serde_json::error::Error) -> Self {
        Self::SerdeJson(e)
    }
}

impl std::fmt::Debug for CommonBinaryError {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        match self {
            CommonBinaryError::SeeConsole() => write!(f, "There were some errors, please see console to check what's wrong."),
            CommonBinaryError::Io(e) => write!(f, "IO error: {e}"),
            CommonBinaryError::IoDetracked(e) => write!(f, "{}: {}", e.description, e.cause),
            CommonBinaryError::SerdeJson(e) => write!(f, "SerdeJson error: {e}"),
            CommonBinaryError::PointerOutOfBounds(e) => write!(f, "PointerOutOfBounds when {} for {} at {}", e.when, e.source_len, e.pointer),
            CommonBinaryError::ProvidedSourceIsNotOfExpectedFormat(e) => write!(f, "{e}"),
            CommonBinaryError::StringTooLong(e) => write!(f, "StringTooLong when {} at {} with value {}", e.when, e.pointer, e.target_string),
            CommonBinaryError::StringBadCharacter(e) => write!(f, "Detected non-ASCII character {:#04X} when {} at {} with value {}", e.bad_character, e.when, e.pointer, e.target_string),
        }
    }
}