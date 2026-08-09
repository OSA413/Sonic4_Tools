use common_binary::{
    binary_reader, binary_writer, endianness::Endianness, error::CommonBinaryError
};
use serde::{Deserialize, Serialize};
use crate::{ring::RingCoordinates, ring_tile::RingTile};

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct RingSet {
    pub x_tiles: u16,
    pub y_tiles: u16,
    pub tiles: Vec<RingTile>,
}

pub struct RingSetPointerPrediction {
    pointers: usize,
    tiles: usize,
    length: usize,
}

impl RingSet {
    pub fn new_from_file_name(file_path: &String) -> Result<Self, CommonBinaryError> {
        Self::new_from_src_ptr_name(&std::fs::read(file_path)?, Some(0))
    }

    pub fn new_from_src_ptr_name(
        source: &[u8],
        ptr: Option<usize>,
    ) -> Result<Self, CommonBinaryError> {
        let ptr = ptr.unwrap_or(0);

        let x_tiles = binary_reader::u16::read(source, ptr, &Endianness::Little, "x_tiles from RingSet header")?;
        let y_tiles = binary_reader::u16::read(source, ptr + 2, &Endianness::Little, "y_tiles from RingSet header")?;

        let total_tiles = x_tiles * y_tiles;
        let mut tiles = Vec::with_capacity(total_tiles as usize);

        let mut pointers_pointer = ptr + 0x04;
        for i in 0..total_tiles {
            let number_pointer = binary_reader::u32::read(source, pointers_pointer, &Endianness::Little, &format!("Error reading {}th ring pointer", i + 1).into_boxed_str())? as usize;
            let number = binary_reader::u16::read(source, number_pointer, &Endianness::Little, "rings number")?;

            let mut rings = Vec::with_capacity(number as usize);
            let mut ring_pointer = number_pointer + 0x02;

            for _ in 0..number {
                let x = binary_reader::u8::read(source, ring_pointer, "ring x coordinate")?;
                let y = binary_reader::u8::read(source, ring_pointer + 1, "ring y coordinate")?;

                rings.push(RingCoordinates { x, y });
                ring_pointer += 0x02;
            }
            pointers_pointer += 0x04;

            tiles.push(RingTile { rings });
        }

        Ok(RingSet {
            x_tiles,
            y_tiles,
            tiles,
        })
    }

    pub fn predict_pointers(&self) -> RingSetPointerPrediction {
        let header_length = 0x04;
        let pointers_length = self.tiles.len() * 0x04;
        let tiles_length = self.tiles.iter()
            .map(|tile| 0x02 + tile.rings.len() * 0x02)
            .sum::<usize>();

        RingSetPointerPrediction {
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

            binary_writer::u16::write(&mut result, pointers.tiles, tile.rings.len() as u16, &Endianness::Little, "number of rings")?;
            pointers.tiles += 0x02;
            for ring in &tile.rings {
                binary_writer::u8::write(&mut result, pointers.tiles, ring.x, "x")?;
                binary_writer::u8::write(&mut result, pointers.tiles + 1, ring.y, "y")?;
                pointers.tiles += 0x02;
            }
        }

        Ok(result)
    }
}