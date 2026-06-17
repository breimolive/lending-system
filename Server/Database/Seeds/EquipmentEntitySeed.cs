using Server.Database.Entities;
using Server.Models;

namespace Server.Database.Seeds;

public class EquipmentEntitySeed
{
    public static List<EquipmentEntity> Seeds(CategoryEntity category)
    {
        return
        [
            new EquipmentEntity("Laptop Dell XPS 13", "Ultrabook with 13.3-inch display, Intel Core i7, 16GB RAM, 512GB SSD.", "58374192068415", EquipmentStatus.Available, category),
            new EquipmentEntity("Projector Epson PowerLite 1781W", "Portable projector with 3LCD technology, WXGA resolution, and 3200 lumens brightness.", "91730548267194", EquipmentStatus.Available, category),
            new EquipmentEntity("Camera Canon EOS 5D Mark IV", "Full-frame DSLR camera with 30.4MP sensor, 4K video recording, and advanced autofocus system.", "26481975302846", EquipmentStatus.Available, category)
        ];
    }
}