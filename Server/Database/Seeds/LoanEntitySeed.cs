using Server.Database.Entities;
using Server.Models;

namespace Server.Database.Seeds;

public class LoanEntitySeed
{
    public static List<LoanEntity> Seeds(BorrowerEntity borrower, EquipmentEntity equipment, UserEntity user)
    {
        return
        [
            new LoanEntity(DateTime.UtcNow, DateTime.UtcNow.AddDays(7), LoanStatus.OnLoan, equipment, borrower, user),
            new LoanEntity(DateTime.UtcNow.AddDays(-7), DateTime.UtcNow, LoanStatus.Returned, equipment, borrower, user)
        ];
    }
}