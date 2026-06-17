using Server.Database.Entities;

namespace Server.Database.Seeds;

public class UserEntitySeed
{
    public static List<UserEntity> Seeds()
    {
        return
        [
            new UserEntity("Heine Breimo", "heine.breimo@gmail.com", "123lol123", "XQxEV5vjtVczA3NZQlqHc"),
            new UserEntity("Test User", "test@gmail.com", "123lol123", "XQxEV5vjtVczA3NZQlqHc")
        ];
    }
}