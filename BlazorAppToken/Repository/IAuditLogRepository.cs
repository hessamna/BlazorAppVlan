using BalzorAppVlan.Repository.BaseRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IAuditLogRepository : IBaseRepository<AuditLog>
{
    // Add custom methods for AuditLog if needed
}