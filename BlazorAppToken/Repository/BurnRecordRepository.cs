using Microsoft.EntityFrameworkCore;

public class BurnRecordRepository : BaseRepository<BurnRecord>, IBurnRecordRepository
{
    public BurnRecordRepository(ApplicationDbContext context) : base(context) { }
}