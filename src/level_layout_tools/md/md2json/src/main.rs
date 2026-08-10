use std::{env, fs};
use common_binary::{cli, error::CommonBinaryError};
use md_rs_lib::md_set::MdSet;

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
    if arg.ends_with(".md") || arg.ends_with(".MD") {
        let md = MdSet::new_from_file_name(arg)?;
        let result = md_rs_lib::convert::to_json::convert(&md)?;
        fs::write(format!("{}.json", arg), result)?;
        return Ok(());
    } else if arg.ends_with(".md.json") || arg.ends_with(".MD.json") {
        let str = fs::read_to_string(arg)?;
        let result = md_rs_lib::convert::from_json::convert(&str)?;
        fs::write(arg.chars().take(arg.len() - ".json".len()).collect::<String>(), result.write()?)?;
        return Ok(());
    }
    Err(CommonBinaryError::ProvidedSourceIsNotOfExpectedFormat("Provided path doesn't end with .MD or .MD.json".to_string()))
}