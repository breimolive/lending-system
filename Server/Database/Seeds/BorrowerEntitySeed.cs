using Server.Database.Entities;

namespace Server.Database.Seeds;

public class BorrowerEntitySeed
{
    public static BorrowerEntity Seed()
    {
        return new BorrowerEntity("heine.breimo@gmail.com", "Heine", "Breimo", "98148490");
    }
}