#[cfg(test)]
mod create_tests {
    use std::{fs, time::Duration};

    use amb_rs_lib::amb::Amb;
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

        // TODO move to common_tests
        let resulting_amb_path = temp_dir.join("test.amb").display().to_string();
        let resulting_amb = Amb::new_from_file_name(&resulting_amb_path).unwrap();

        assert_eq!(
            format!("{resulting_amb}"),
            serde_json::to_string(&AmbPrint {
                name: resulting_amb_path.clone(),
                endianness: "little".to_string(),
                objects: Vec::new(),
                version: "PC".to_string(),
            }).unwrap()
        );

        let reference_file = "../amb-rs-tests/tests/reference_files/le/add_empty.amb";

        assert_eq!(
            fs::read(&resulting_amb_path).unwrap(),
            fs::read(reference_file).unwrap()
        )
    }
}