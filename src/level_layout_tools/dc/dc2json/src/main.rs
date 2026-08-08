#![deny(clippy::unwrap_used)]
use std::{env, fs};
use common_binary::{cli, error::CommonBinaryError};
use dc_rs_lib::decoration_set::DecorationSet;

mod help;
mod version;

fn main() {
    let mut args = env::args().skip(1);

    match args.next() {
        Some(arg) => {
            match &arg[..] {
                "--help" | "-h" => help::print(),
                "--version" | "-v" => version::print(),
                _ => {
                    cli::handle_result(convert(&arg));
                }
            }
        }
        None => help::print(),
    }
}


fn convert(arg: &String) -> Result<(), CommonBinaryError> {
    if arg.ends_with(".dc") || arg.ends_with(".DC") {
        let dc = DecorationSet::new_from_file_name(arg)?;
        let result = dc_rs_lib::convert::to_json::convert(&dc)?;
        fs::write(format!("{}.json", arg), result)?;
        return Ok(());
    } else if arg.ends_with(".dc.json") || arg.ends_with(".DC.json") {
        let str = fs::read_to_string(arg)?;
        let result = dc_rs_lib::convert::from_json::convert(&str)?;
        fs::write(arg.chars().take(arg.len() - ".json".len()).collect::<String>(), result.write()?)?;
        return Ok(());
    }
    Err(CommonBinaryError::ProvidedSourceIsNotOfExpectedFormat("Provided path doesn't end with .DC or .DC.json".to_string()))
}