using Server.Database.Entities;

namespace Server.Database.Seeds;

public class CategoryEntitySeed
{
    public static List<CategoryEntity> Seeds()
    {
        return
        [
            new CategoryEntity("Default"),
            new CategoryEntity("Tech")
        ];
    }
}