using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class BurnRecordService : IEntityService<BurnRecord>
{
    private readonly IBurnRecordRepository _repo;

    public BurnRecordService(IBurnRecordRepository repo)
    {
        _repo = repo;
    }

    public Task<List<BurnRecord>> GetAllAsync() => _repo.GetAllAsync();

    public Task<BurnRecord?> GetByIdAsync(Guid id) => _repo.GetByIdAsync(id);

    public async Task AddOrEditAsync(BurnRecord model)
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