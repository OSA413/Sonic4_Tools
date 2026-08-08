use common_binary::{
    binary_reader, binary_writer, endianness::Endianness, error::CommonBinaryError
};
use serde::{Deserialize, Serialize};
use crate::{event::Event, event_tile::EventTile};

#[derive(Debug, Clone, Serialize, Deserialize)]
pub struct EventSet {
    pub x_tiles: u16,
    pub y_tiles: u16,
    pub tiles: Vec<EventTile>,
}

pub struct EventSetPointerPrediction {
    pointers: usize,
    tiles: usize,
    length: usize,
}

impl EventSet {
    pub fn new_from_file_name(file_path: &String) -> Result<Self, CommonBinaryError> {
        Self::new_from_src_ptr_name(&std::fs::read(file_path)?, Some(0))
    }

    pub fn new_from_src_ptr_name(
        source: &[u8],
        ptr: Option<usize>,
    ) -> Result<Self, CommonBinaryError> {
        let ptr = ptr.unwrap_or(0);

        let x_tiles = binary_reader::u16::read(source, ptr, &Endianness::Little)
        // TODO: move exceptions to Result Err() with Option<When>
            .expect("Error reading x_tiles from EventSet header");
        let y_tiles = binary_reader::u16::read(source, ptr + 2, &Endianness::Little)
            .expect("Error reading y_tiles from EventSet header");

        let total_tiles = x_tiles as usize * y_tiles as usize;
        let mut tiles = Vec::with_capacity(total_tiles);

        let mut pointers_pointer = ptr + 0x04;
        for i in 0..total_tiles {
            let number_pointer = binary_reader::u32::read(source, pointers_pointer, &Endianness::Little)
                .expect(&format!("Error reading {}th event pointer", i).into_boxed_str()) as usize;

            let number = binary_reader::u16::read(source, number_pointer, &Endianness::Little)
                .expect("Error reading events number") as usize;

            let mut events = Vec::with_capacity(number);
            let mut event_pointer = number_pointer + 0x02;

            for _ in 0..number {
                let unknown1 = binary_reader::u16::read(source, event_pointer, &Endianness::Little)
                    .expect("Error reading event unknown1");
                let unknown2 = binary_reader::u16::read(source, event_pointer + 0x02, &Endianness::Little)
                    .expect("Error reading event unknown2");
                let unknown3 = binary_reader::u16::read(source, event_pointer + 0x04, &Endianness::Little)
                    .expect("Error reading event unknown3");
                let unknown4 = binary_reader::u16::read(source, event_pointer + 0x06, &Endianness::Little)
                    .expect("Error reading event unknown4");
                let unknown5 = binary_reader::u16::read(source, event_pointer + 0x08, &Endianness::Little)
                    .expect("Error reading event unknown5");
                let unknown6 = binary_reader::u16::read(source, event_pointer + 0x0A, &Endianness::Little)
                    .expect("Error reading event unknown6");

                events.push(Event { unknown1, unknown2, unknown3, unknown4, unknown5, unknown6 });
                event_pointer += 0x0C;
            }
            pointers_pointer += 0x04;

            tiles.push(EventTile { events });
        }

        Ok(EventSet {
            x_tiles,
            y_tiles,
            tiles,
        })
    }

    pub fn predict_pointers(&self) -> EventSetPointerPrediction {
        let header_length = 0x04;
        let pointers_length = self.tiles.len() * 0x04;
        let tiles_length = self.tiles.iter()
            .map(|tile| 0x02 + tile.events.len() * 0x02 * 6)
            .sum::<usize>();

        EventSetPointerPrediction {
            pointers: header_length,
            tiles: header_length + pointers_length,
            length: header_length + pointers_length + tiles_length,
        }
    }

    pub fn write(&self) -> Result<Vec<u8>, CommonBinaryError> {
        let mut pointers = self.predict_pointers();
        let length = pointers.length;
        let mut result = vec![0; length];

        binary_writer::u16::write(&mut result, 0x00, self.x_tiles, &Endianness::Little, "x_tiles".to_string())?;
        binary_writer::u16::write(&mut result, 0x02, self.y_tiles, &Endianness::Little, "y_tiles".to_string())?;

        for tile in &self.tiles {
            binary_writer::u32::write(&mut result, pointers.pointers, pointers.tiles as u32, &Endianness::Little, "tile pointer".to_string())?;
            pointers.pointers += 0x04;

            binary_writer::u16::write(&mut result, pointers.tiles, tile.events.len() as u16, &Endianness::Little, "number of events".to_string())?;
            pointers.tiles += 0x02;
            for event in &tile.events {
                binary_writer::u16::write(&mut result, pointers.tiles, event.unknown1, &Endianness::Little, "unknown1".to_string())?;
                binary_writer::u16::write(&mut result, pointers.tiles + 0x02, event.unknown2, &Endianness::Little, "unknown2".to_string())?;
                binary_writer::u16::write(&mut result, pointers.tiles + 0x04, event.unknown3, &Endianness::Little, "unknown3".to_string())?;
                binary_writer::u16::write(&mut result, pointers.tiles + 0x06, event.unknown4, &Endianness::Little, "unknown4".to_string())?;
                binary_writer::u16::write(&mut result, pointers.tiles + 0x08, event.unknown5, &Endianness::Little, "unknown5".to_string())?;
                binary_writer::u16::write(&mut result, pointers.tiles + 0x0A, event.unknown6, &Endianness::Little, "unknown6".to_string())?;
                pointers.tiles += 0x0C;
            }
        }

        Ok(result)
    }
}
