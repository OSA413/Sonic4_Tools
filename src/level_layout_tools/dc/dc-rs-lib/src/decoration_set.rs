use common_binary::{
    binary_reader, binary_writer, endianness::Endianness, error::CommonBinaryError
};
use serde::{Deserialize, Serialize};
use crate::{decoration::Decoration, decoration_tile::DecorationTile};

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct DecorationSet {
    pub x_tiles: u16,
    pub y_tiles: u16,
    pub tiles: Vec<DecorationTile>,
}

pub struct DecorationSetPointerPrediction {
    pointers: usize,
    tiles: usize,
    length: usize,
}

impl DecorationSet {
    pub fn new_from_file_name(file_path: &String) -> Result<Self, CommonBinaryError> {
        Self::new_from_src_ptr_name(&std::fs::read(file_path)?, Some(0))
    }

    pub fn new_from_src_ptr_name(
        source: &[u8],
        ptr: Option<usize>,
    ) -> Result<Self, CommonBinaryError> {
        let ptr = ptr.unwrap_or(0);

        let x_tiles = binary_reader::u16::read(source, ptr, &Endianness::Little, " x_tiles from DecorationSet header")?;
        let y_tiles = binary_reader::u16::read(source, ptr + 2, &Endianness::Little, "y_tiles from DecorationSet header")?;

        let total_tiles = x_tiles as usize * y_tiles as usize;
        let mut tiles = Vec::with_capacity(total_tiles);

        let mut pointers_pointer = ptr + 0x04;
        for i in 0..total_tiles {
            let number_pointer = binary_reader::u32::read(source, pointers_pointer, &Endianness::Little, format!("Error reading {}th decoration pointer", i).as_str())? as usize;

            let number = binary_reader::u16::read(source, number_pointer, &Endianness::Little, "decorations number")? as usize;

            let mut decorations = Vec::with_capacity(number);
            let mut decoration_pointer = number_pointer + 0x02;

            for _ in 0..number {
                let unknown1 = binary_reader::u16::read(source, decoration_pointer, &Endianness::Little, "decoration unknown1")?;
                let unknown2 = binary_reader::u16::read(source, decoration_pointer + 0x02, &Endianness::Little, "decoration unknown2")?;

                decorations.push(Decoration { unknown1, unknown2 });
                decoration_pointer += 0x04;
            }
            pointers_pointer += 0x04;

            tiles.push(DecorationTile { decorations });
        }

        Ok(DecorationSet {
            x_tiles,
            y_tiles,
            tiles,
        })
    }

    pub fn predict_pointers(&self) -> DecorationSetPointerPrediction {
        let header_length = 0x04;
        let pointers_length = self.tiles.len() * 4;
        let tiles_length = self.tiles.iter()
            .map(|tile| 0x02 + tile.decorations.len() * 0x02 * 2)
            .sum::<usize>();

        DecorationSetPointerPrediction {
            pointers: header_length,
            tiles: header_length + pointers_length,
            length: header_length + pointers_length + tiles_length,
        }
    }

    pub fn write(&self) -> Result<Vec<u8>, CommonBinaryError> {
        let mut pointers = self.predict_pointers();
        let length = pointers.length;
        let mut result = vec![0; length];

        binary_writer::u16::write(&mut result, 0x00, self.x_tiles, &Endianness::Little, "x_tiles")?;
        binary_writer::u16::write(&mut result, 0x02, self.y_tiles, &Endianness::Little, "y_tiles")?;

        for tile in &self.tiles {
            binary_writer::u32::write(&mut result, pointers.pointers, pointers.tiles as u32, &Endianness::Little, "tile pointer")?;
            pointers.pointers += 0x04;

            binary_writer::u16::write(&mut result, pointers.tiles, tile.decorations.len() as u16, &Endianness::Little, "number of decorations")?;
            pointers.tiles += 0x02;
            for decoration in &tile.decorations {
                binary_writer::u16::write(&mut result, pointers.tiles, decoration.unknown1, &Endianness::Little, "unknown1")?;
                binary_writer::u16::write(&mut result, pointers.tiles + 0x02, decoration.unknown2, &Endianness::Little, "unknown2")?;
                pointers.tiles += 0x04;
            }
        }

        Ok(result)
    }
}
