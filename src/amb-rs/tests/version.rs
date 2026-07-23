#[cfg(test)]
mod version_tests {
    use std::time::Duration;

    use assert_cmd::Command;

    #[test]
    fn with_v_flag() {
        let mut cmd = Command::cargo_bin("amb-rs").unwrap();
        let assert = cmd
            .arg("-v")
            .timeout(Duration::from_millis(100))
            .assert();

        assert
            .success()
            .stdout("amb-rs version: 0.8.0\n");
    }

    #[test]
    fn with_version_flag() {
        let mut cmd = Command::cargo_bin("amb-rs").unwrap();
        let assert = cmd
            .arg("--version")
            .timeout(Duration::from_millis(100))
            .assert();

        assert
            .success()
            .stdout("amb-rs version: 0.8.0\n");
    }
}