using Microsoft.EntityFrameworkCore;

public class SmartContractRepository : BaseRepository<SmartContract>, ISmartContractRepository
{
    public SmartContractRepository(ApplicationDbContext context) : base(context) { }
}