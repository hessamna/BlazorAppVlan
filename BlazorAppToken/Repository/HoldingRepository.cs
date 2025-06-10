using Microsoft.EntityFrameworkCore;

public class HoldingRepository : BaseRepository<Holding>, IHoldingRepository
{
    public HoldingRepository(ApplicationDbContext context) : base(context) { }
}