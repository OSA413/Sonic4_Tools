pub fn make_safe(raw_name: &str) -> String {
    //removing ".\" in the names (Windows can't create "." folders)
    //sometimes they can have several ".\" in the names
    //Turns out there's a double dot directory in file names
    //And double backslash in file names
    let mut safe_index = 0;
    let chars = raw_name.chars();
    for ch in chars {
        if ch == '.' || ch == '\\' || ch == '/' {
            safe_index += 1;
        } else {
            break;
        }
    }

    raw_name.chars().skip(safe_index).collect()
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_make_safe_0() {
        assert_eq!(make_safe(".\\test.amb"), "test.amb");
    }

    #[test]
    fn test_make_safe_1() {
        assert_eq!(make_safe("..\\test.amb"), "test.amb");
    }

    #[test]
    fn test_make_safe_2() {
        assert_eq!(make_safe("..\\.\\test.amb"), "test.amb");
    }
    
    #[test]
    fn test_make_safe_3() {
        assert_eq!(make_safe("..\\..\\test.amb"), "test.amb");
    }

    #[test]
    fn test_make_safe_4() {
        assert_eq!(make_safe(".\\..\\test.amb"), "test.amb");
    }
}