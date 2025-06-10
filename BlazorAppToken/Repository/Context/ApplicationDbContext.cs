using BlazorApptToken.Datas;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public DbSet<Token> Tokens { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Holding> Holdings { get; set; }
    public DbSet<HoldingDetail> HoldingDetails { get; set; }
    public DbSet<TokenTransfer> TokenTransfers { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<TokenAllocation> TokenAllocations { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<MultiSigApproval> MultiSigApprovals { get; set; }
    public DbSet<SmartContract> SmartContracts { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<SupportTicket> SupportTickets { get; set; }
    public DbSet<SystemSetting> SystemSettings { get; set; }
    public DbSet<BurnRecord> BurnRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // TokenTransfer: FromWallet & ToWallet
        modelBuilder.Entity<TokenTransfer>()
            .HasOne(t => t.FromWallet)
            .WithMany(w => w.SentTransfers)
            .HasForeignKey(t => t.FromWalletId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TokenTransfer>()
            .HasOne(t => t.ToWallet)
            .WithMany(w => w.ReceivedTransfers)
            .HasForeignKey(t => t.ToWalletId)
            .OnDelete(DeleteBehavior.Restrict);

        // TokenAllocation: Token & Wallet
        modelBuilder.Entity<TokenAllocation>()
            .HasOne(a => a.Token)
            .WithMany(t => t.Allocations)
            .HasForeignKey(a => a.TokenId);

        modelBuilder.Entity<TokenAllocation>()
            .HasOne(a => a.Wallet)
            .WithMany(w => w.Allocations)
            .HasForeignKey(a => a.WalletId);

        // Wallet: Company
        modelBuilder.Entity<Wallet>()
            .HasOne(w => w.Company)
            .WithMany(c => c.Wallets)
            .HasForeignKey(w => w.CompanyId);

        // User: Company
        modelBuilder.Entity<User>()
            .HasOne(u => u.Company)
            .WithMany(c => c.Users)
            .HasForeignKey(u => u.CompanyId);

        // MultiSigApproval: TokenTransfer & User
        modelBuilder.Entity<MultiSigApproval>()
            .HasOne(a => a.Transfer)
            .WithMany(t => t.Approvals)
            .HasForeignKey(a => a.TransferId);

        modelBuilder.Entity<MultiSigApproval>()
            .HasOne(a => a.User)
            .WithMany(u => u.Approvals)
            .HasForeignKey(a => a.UserId);

        // BurnRecord: Token
        modelBuilder.Entity<BurnRecord>()
            .HasOne(b => b.Token)
            .WithMany()
            .HasForeignKey(b => b.TokenId);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity &&
                        (e.State == EntityState.Added || e.State == EntityState.Modified));

        var user = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
        var ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
        var machine = _httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString();

        foreach (var entry in entries)
        {
            if (entry.Entity is BaseEntity entity)
            {
                var now = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDate = now;
                    entity.CreatedBy = user;
                    entity.CreatorIp = ip;
                    entity.CreatorMachine = machine;
                }

                entity.ModifiedDate = now;
                entity.ModifiedBy = user;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}