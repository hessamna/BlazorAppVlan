using Microsoft.EntityFrameworkCore;

public class HoldingDetailRepository : BaseRepository<HoldingDetail>, IHoldingDetailRepository
{
    public HoldingDetailRepository(ApplicationDbContext context) : base(context) { }
}