use std::{env, fs, path::Path};
use winresource::WindowsResource;

fn main() {
    if env::var_os("CARGO_CFG_WINDOWS").is_some() {
        let icon_name = "txb2json";
        let icons_dir = Path::new("../../../icons");
        let input_icon = &icons_dir.join(format!("{}.svg", icon_name)).into_boxed_path();
        let output_icon = &icons_dir.join(format!("{}.ico", icon_name)).into_boxed_path();

        let container = fs::read_to_string(icons_dir.join("base\\svg_container.svg")).unwrap();
        let base = fs::read_to_string(icons_dir.join("base\\txb.svg")).unwrap();
        let container = container.replace(
            "<content />",
            &base.replace("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>", "")
                .replacen("svg", "svg width=\"256\" height=\"256\"", 1)
                .replacen("width=\"32\"", "", 1)
                .replacen("height=\"32\"", "", 1)
            .into_boxed_str()
        );
        let overlay = fs::read_to_string(icons_dir.join("base\\json.svg")).unwrap();
        let container = container.replace(
            "<overlay />",
            &overlay
                .replace("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?>", "")
                .replacen("svg", "svg viewBox=\"0 0 32 32\" x=\"128\" y=\"128\"", 1)
                .replacen("width=\"32\"", "width=\"128\"", 1)
                .replacen("height=\"32\"", "height=\"128\"", 1)
                .into_boxed_str()
        );
        fs::write(input_icon, container).unwrap();

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