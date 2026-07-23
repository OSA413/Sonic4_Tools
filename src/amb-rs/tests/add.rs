#[cfg(test)]
mod add_tests {
    use std::{fs, time::Duration};

    use amb_rs_tests::{AmbPrint, BinaryObjectPrint};
    use assert_cmd::Command;

    #[test]
    fn add_1() {
        let initial_file = "../amb-rs-tests/tests/reference_files/le/add_empty.amb";
        let file_to_add = "../amb-rs-tests/test_files/files/1";
        let temp_dir = std::env::temp_dir().join("amb-rs-tests").join("add");
        fs::create_dir_all(&temp_dir).unwrap();

        fs::copy(initial_file, temp_dir.join("empty.amb")).unwrap();
        fs::copy(file_to_add, temp_dir.join("1")).unwrap();

        let mut cmd = Command::cargo_bin("amb-rs").unwrap();
        let assert = cmd
            .arg("add")
            .arg("empty.amb")
            .arg("1")
            .current_dir(&temp_dir)
            .timeout(Duration::from_millis(100))
            .assert();

        assert
            .success()
            .stdout("");

        let result_path = temp_dir.join("empty.amb");
        amb_rs_tests::check_amb_eq(
            &result_path,
            &"../amb-rs-tests/tests/reference_files/le/add_1.amb".to_string(),
            &AmbPrint {
                name: result_path.display().to_string(),
                endianness: "little".to_string(),
                objects: vec![
                    BinaryObjectPrint {
                        name: "1".to_string(),
                        real_name: "1".to_string(),
                        flag1: 0,
                        flag2: 0,
                        pointer: 48,
                        length: 73,
                    },
                ],
                version: "PC".to_string(),
            }
        );
    }
}