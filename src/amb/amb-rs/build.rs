use std::{env, path::Path};
use winresource::WindowsResource;

fn main() {
    if env::var_os("CARGO_CFG_WINDOWS").is_some() {
        let icon_name = "archive";
        let icons_dir = Path::new("../../../icons");
        let input_icon = &icons_dir.join("base").join(format!("{}.svg", icon_name)).into_boxed_path();
        let output_icon = &icons_dir.join(format!("{}.ico", icon_name)).into_boxed_path();

        svg_to_ico::svg_to_ico(
            input_icon,
            96.0,
            output_icon,
            &[32, 64, 128, 256]
        ).expect("failed to convert svg to ico");

        WindowsResource::new()
            .set_icon(&output_icon.to_string_lossy())
            .set_manifest_file("../../windows.manifest")
            .compile()
            .unwrap();
    }
}