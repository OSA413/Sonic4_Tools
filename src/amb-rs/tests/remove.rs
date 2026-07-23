#[cfg(test)]
mod add_tests {
    use std::{fs, time::Duration};

    use amb_rs_tests::{AmbPrint};
    use assert_cmd::Command;

    #[test]
    fn add_1() {
        let initial_file = "../amb-rs-tests/tests/reference_files/le/add_1.amb";
        let temp_dir = std::env::temp_dir().join("amb-rs-tests").join("remove");
        fs::create_dir_all(&temp_dir).unwrap();

        fs::copy(initial_file, temp_dir.join("add_1.amb")).unwrap();

        let mut cmd = Command::cargo_bin("amb-rs").unwrap();
        let assert = cmd
            .arg("remove")
            .arg("add_1.amb")
            .arg("1")
            .current_dir(&temp_dir)
            .timeout(Duration::from_millis(100))
            .assert();

        assert
            .success()
            .stdout("");

        let result_path = temp_dir.join("add_1.amb");
        amb_rs_tests::check_amb_eq(
            &result_path,
            &"../amb-rs-tests/tests/reference_files/le/add_empty.amb".to_string(),
            &AmbPrint {
                name: result_path.display().to_string(),
                endianness: "little".to_string(),
                objects: vec![],
                version: "PC".to_string(),
            }
        );
    }
}