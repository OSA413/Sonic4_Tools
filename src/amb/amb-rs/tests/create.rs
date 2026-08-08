#[cfg(test)]
mod create_tests {
    use std::{fs, time::Duration};

    use amb_rs_tests::AmbPrint;
    use assert_cmd::Command;

    #[test]
    fn create_empty() {
        let temp_dir = std::env::temp_dir().join("amb-rs-tests").join("create");
        fs::create_dir_all(&temp_dir).unwrap();

        let mut cmd = Command::cargo_bin("amb-rs").unwrap();
        let assert = cmd
            .arg("create")
            .arg("test.amb")
            .current_dir(&temp_dir)
            .timeout(Duration::from_millis(100))
            .assert();

        assert
            .success()
            .stdout("");

        amb_rs_tests::check_amb_eq(
            &temp_dir.join("test.amb"),
            &"../amb-rs-tests/tests/reference_files/le/add_empty.amb".to_string(),
            &AmbPrint {
                name: temp_dir.join("test.amb").display().to_string(),
                endianness: "little".to_string(),
                objects: Vec::new(),
                version: "v1".to_string(),
            }
        );
    }
}