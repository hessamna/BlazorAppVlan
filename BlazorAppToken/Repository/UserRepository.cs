using BalzorAppVlan.Repository.BaseRepository;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context) { }
}