use crate::error::CommonBinaryError;

pub fn exit_with_error(error: String) {
    eprintln!("{error}");
    std::process::exit(1);
}

pub fn handle_result(result: Result<(), CommonBinaryError>) {
    match result {
        Ok(_) => (),
        Err(e) => exit_with_error(format!("{e:?}")),
    }
}
