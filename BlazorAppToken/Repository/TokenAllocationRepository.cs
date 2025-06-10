using Microsoft.EntityFrameworkCore;

public class TokenAllocationRepository : BaseRepository<TokenAllocation>, ITokenAllocationRepository
{
    public TokenAllocationRepository(ApplicationDbContext context) : base(context) { }
}