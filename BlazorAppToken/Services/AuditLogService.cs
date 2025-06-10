using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AuditLogService : IEntityService<AuditLog>
{
    private readonly IAuditLogRepository _repo;

    public AuditLogService(IAuditLogRepository repo)
    {
        _repo = repo;
    }

    public Task<List<AuditLog>> GetAllAsync() => _repo.GetAllAsync();

    public Task<AuditLog?> GetByIdAsync(Guid id) => _repo.GetByIdAsync(id);

    public async Task AddOrEditAsync(AuditLog model)
    {
        if (model.Id == Guid.Empty)
        {
            model.Id = Guid.NewGuid();
            await _repo.AddAsync(model);
        }
        else
        {
            await _repo.UpdateAsync(model);
        }
    }
}