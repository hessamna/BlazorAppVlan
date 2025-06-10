using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class SystemSettingService : IEntityService<SystemSetting>
{
    private readonly ISystemSettingRepository _repo;

    public SystemSettingService(ISystemSettingRepository repo)
    {
        _repo = repo;
    }

    public Task<List<SystemSetting>> GetAllAsync() => _repo.GetAllAsync();

    public Task<SystemSetting?> GetByIdAsync(Guid id) => _repo.GetByIdAsync(id);

    public async Task AddOrEditAsync(SystemSetting model)
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