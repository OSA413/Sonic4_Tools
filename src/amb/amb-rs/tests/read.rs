#[cfg(test)]
mod create_tests {
    use std::{time::Duration};

    use amb_rs_tests::AmbPrint;
    use assert_cmd::Command;

    #[test]
    fn create_empty() {
        let reference_file = "../amb-rs-tests/tests/reference_files/le/add_empty.amb";

        let mut cmd = Command::cargo_bin("amb-rs").unwrap();
        let assert = cmd
            .arg("read")
            .arg(&reference_file)
            .timeout(Duration::from_millis(100))
            .assert();

        assert
            .success()
            .stdout(format!("{}\n", serde_json::to_string(&AmbPrint {
                name: reference_file.to_string(),
                endianness: "little".to_string(),
                objects: Vec::new(),
                version: "v1".to_string(),
            }).unwrap()));
    }
}