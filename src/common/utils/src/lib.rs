use sha2::{Digest, Sha256};

pub mod walk_dir;
pub mod copy_dir;

pub fn get_sha256(data: impl AsRef<[u8]>) -> String {
    Sha256::digest(data).iter().map(|x| format!("{x:02x}")).collect()
}