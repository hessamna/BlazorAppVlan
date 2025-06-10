using Microsoft.EntityFrameworkCore;

public class TokenTransferRepository : BaseRepository<TokenTransfer>, ITokenTransferRepository
{
    public TokenTransferRepository(ApplicationDbContext context) : base(context) { }
}