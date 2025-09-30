using BalzorAppVlan.Datas;
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

    // جداول اصلی
    public DbSet<Company> Companies { get; set; }
    public DbSet<Switch> Switches { get; set; }
    public DbSet<Vlan> Vlans { get; set; }
    public DbSet<DeviceInterface> DeviceInterfaces { get; set; }
    public DbSet<Neighbor> Neighbors { get; set; }


    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<SystemSetting> SystemSettings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Company>()
       .HasMany(c => c.SubCompanies)
       .WithOne(c => c.ParentCompany)
       .HasForeignKey(c => c.ParentCompanyId)
       .OnDelete(DeleteBehavior.Restrict); // جلوگیری از حذف زنجیره‌ای

        // Company → Switch (1..n)
        modelBuilder.Entity<Company>()
            .HasMany(c => c.Switches)
            .WithOne(s => s.Company)
            .HasForeignKey(s => s.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);



        // Switch → Vlan (1..n)
        modelBuilder.Entity<Switch>()
            .HasMany(s => s.Vlans)
            .WithOne(v => v.Switch)
            .HasForeignKey(v => v.SwitchId)
            .OnDelete(DeleteBehavior.Cascade);

        // Switch → DeviceInterface (1..n) ❌ تغییر به Restrict
        modelBuilder.Entity<Switch>()
            .HasMany(s => s.DeviceInterfaces)
            .WithOne(d => d.Switch)
            .HasForeignKey(d => d.SwitchId)
            .OnDelete(DeleteBehavior.Restrict);

        // Switch → Neighbor (1..n) ❌ تغییر به Restrict
        modelBuilder.Entity<Switch>()
            .HasMany(s => s.Neighbors)
            .WithOne(n => n.Switch)
            .HasForeignKey(n => n.SwitchId)
            .OnDelete(DeleteBehavior.Restrict);

        // Vlan → DeviceInterface (1..n)
        modelBuilder.Entity<Vlan>()
            .HasMany(v => v.DeviceInterfaces)
            .WithOne(d => d.Vlan)
            .HasForeignKey(d => d.VlanId)
            .OnDelete(DeleteBehavior.Cascade);

        // Vlan → Neighbor (1..n)
        modelBuilder.Entity<Vlan>()
            .HasMany(v => v.Neighbors)
            .WithOne(n => n.Vlan)
            .HasForeignKey(n => n.VlanId)
            .OnDelete(DeleteBehavior.Cascade);
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
                    entity.CreatedBy = string.IsNullOrWhiteSpace(user) ? "System" : user;
                    entity.CreatorIp = ip;
                    entity.CreatorMachine = machine?.Length > 500 ? machine.Substring(0, 500) : machine;
                }
                else if (entry.State == EntityState.Modified)
                {
                    // جلوگیری از تغییر Created*
                    entry.Property(nameof(BaseEntity.CreatedDate)).IsModified = false;
                    entry.Property(nameof(BaseEntity.CreatedBy)).IsModified = false;
                    entry.Property(nameof(BaseEntity.CreatorIp)).IsModified = false;
                    entry.Property(nameof(BaseEntity.CreatorMachine)).IsModified = false;
                }

                entity.ModifiedDate = now;
                entity.ModifiedBy = string.IsNullOrWhiteSpace(user) ? "System" : user;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
