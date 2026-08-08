use std::io;

pub struct IoError {
    pub cause: io::Error,
    pub description: &'static str,
}

impl std::fmt::Debug for IoError {
    fn fmt(&self, f: &mut std::fmt::Formatter) -> std::fmt::Result {
        writeln!(f, "{}: {}", self.description, self.cause)
    }
}