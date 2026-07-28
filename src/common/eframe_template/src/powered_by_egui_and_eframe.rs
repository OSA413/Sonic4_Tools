/*
    This template was taken from https://github.com/emilk/eframe_template.
    It's licensed under the MIT and Apache-2.0 licenses.
    This method is taken from https://github.com/emilk/eframe_template/blob/main/src/app.rs
    as is without changes (according to Apache-2.0 I have to state what changes were made)
*/

pub fn powered_by_egui_and_eframe(ui: &mut egui::Ui) {
    ui.horizontal(|ui| {
        ui.spacing_mut().item_spacing.x = 0.0;
        ui.label("Powered by ");
        ui.hyperlink_to("egui", "https://github.com/emilk/egui");
        ui.label(" and ");
        ui.hyperlink_to(
            "eframe",
            "https://github.com/emilk/egui/tree/master/crates/eframe",
        );
        ui.label(".");
    });
}