#[cfg(test)]
mod version_tests {
    use std::time::Duration;

    use assert_cmd::Command;
    use predicates::prelude::*;

    #[test]
    fn with_v_flag() {
        let mut cmd = Command::cargo_bin("amb-rs").unwrap();
        let assert = cmd
            .arg("-v")
            .timeout(Duration::from_millis(100))
            .assert();

        assert
            .success()
            .stdout(predicate::function(
                |x: &[u8]| x.starts_with(b"amb-rs: 0.")
            ));
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
            .stdout(predicate::function(
                |x: &[u8]| x.starts_with(b"amb-rs: 0.")
            ));
    }
}