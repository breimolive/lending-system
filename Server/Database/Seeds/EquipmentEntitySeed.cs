using Server.Database.Entities;
using Server.Models;

namespace Server.Database.Seeds;

public class EquipmentEntitySeed
{
    public static List<EquipmentEntity> Seeds(CategoryEntity category)
    {
        return
        [
            new EquipmentEntity("Laptop Dell XPS 13",
                "Ultrabook with 13.3-inch display, Intel Core i7, 16GB RAM, 512GB SSD.", "58374192068415",
                category),
            new EquipmentEntity("Projector Epson PowerLite 1781W",
                "Portable projector with 3LCD technology, WXGA resolution, and 3200 lumens brightness.",
                "91730548267194", category),
            new EquipmentEntity("Camera Canon EOS 5D Mark IV",
                "Full-frame DSLR camera with 30.4MP sensor, 4K video recording, and advanced autofocus system.",
                "26481975302846", category),
            new EquipmentEntity("Microphone Shure SM58",
                "Dynamic vocal microphone with cardioid polar pattern, ideal for live performances and recording.",
                "58374192068416", category),
            new EquipmentEntity("Headphones Sony WH-1000XM4",
                "Wireless noise-canceling headphones with up to 30 hours of battery life and superior sound quality.",
                "91730548267195", category),
            new EquipmentEntity("Tablet Apple iPad Pro 11",
                "Powerful tablet with 11-inch Liquid Retina display, A12Z Bionic chip, and support for Apple Pencil.",
                "26481975302847", category),
            new EquipmentEntity("Monitor LG UltraFine 27MD5KL-B",
                "27-inch 5K monitor with P3 wide color gamut, Thunderbolt 3 connectivity, and stunning image quality.",
                "58374192068417", category),
            new EquipmentEntity("Printer HP LaserJet Pro M404n",
                "Monochrome laser printer with fast printing speeds, high-quality output, and reliable performance.",
                "91730548267196", category),
            new EquipmentEntity("Scanner Epson Perfection V600",
                "High-resolution flatbed scanner with advanced features for photo and document scanning.",
                "26481975302848", category),
            new EquipmentEntity("External Hard Drive Seagate Backup",
                "Portable external hard drive with 2TB capacity, USB 3.0 connectivity, and compact design for easy storage and backup.",
                "58374192068418", category)
        ];
    }
}