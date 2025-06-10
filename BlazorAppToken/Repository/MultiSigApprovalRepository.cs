using Microsoft.EntityFrameworkCore;

public class MultiSigApprovalRepository : BaseRepository<MultiSigApproval>, IMultiSigApprovalRepository
{
    public MultiSigApprovalRepository(ApplicationDbContext context) : base(context) { }
}