using BalzorAppVlan.Repository.BaseRepository;
using Microsoft.EntityFrameworkCore;

public class AuditLogRepository : BaseRepository<AuditLog>, IAuditLogRepository
{
    public AuditLogRepository(ApplicationDbContext context) : base(context) { }
}