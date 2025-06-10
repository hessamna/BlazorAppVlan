using Microsoft.EntityFrameworkCore;

public class TokenRepository : BaseRepository<Token>, ITokenRepository
{
    public TokenRepository(ApplicationDbContext context) : base(context) { }
}