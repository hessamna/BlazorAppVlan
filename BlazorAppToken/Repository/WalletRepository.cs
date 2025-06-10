public class WalletRepository : BaseRepository<Wallet>, IWalletRepository
{
    public WalletRepository(ApplicationDbContext context) : base(context) { }
}