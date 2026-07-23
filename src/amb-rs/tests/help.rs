#[cfg(test)]
mod help_tests {
    use assert_cmd::Command;
    use predicates::prelude::*;

    #[test]
    fn without_arguments() {
        let mut cmd = Command::cargo_bin("amb-rs").unwrap();
        let assert = cmd.assert();

        assert
            .success()
            .stdout(predicate::function(
                |x: &[u8]| x.starts_with(b"        amb-rs")
            ));
    }

    #[test]
    fn with_h_flag() {
        let mut cmd = Command::cargo_bin("amb-rs").unwrap();
        let assert = cmd
            .arg("-h")
            .assert();

        assert
            .success()
            .stdout(predicate::function(
                |x: &[u8]| x.starts_with(b"        amb-rs")
            ));
    }

    #[test]
    fn with_help_flag() {
        let mut cmd = Command::cargo_bin("amb-rs").unwrap();
        let assert = cmd
            .arg("--help")
            .assert();

        assert
            .success()
            .stdout(predicate::function(
                |x: &[u8]| x.starts_with(b"        amb-rs")
            ));
    }
}