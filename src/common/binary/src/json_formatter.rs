pub fn add_str(field: &'static str, value: &str) -> String {
    add_value(field, &format!("\"{value}\""))
}

pub fn add_value(field: &'static str, value: &str) -> String {
    format!("\"{field}\":{value}")
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_add_str() {
        assert_eq!(add_str("field", "value"), "\"field\":\"value\"");
    }

    #[test]
    fn test_add_value() {
        assert_eq!(add_value("field", "value"), "\"field\":value");
    }
}