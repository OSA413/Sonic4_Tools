#[cfg(test)]
mod version_tests {
    use assert_cmd::Command;

    #[test]
    fn with_v_flag() {
        let mut cmd = Command::cargo_bin("amb-rs").unwrap();
        let assert = cmd
            .arg("-v")
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
            .assert();

        assert
            .success()
            .stdout("amb-rs version: 0.8.0\n");
    }
}