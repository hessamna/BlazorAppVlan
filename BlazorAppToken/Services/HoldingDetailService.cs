using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class HoldingDetailService : IEntityService<HoldingDetail>
{
    private readonly IHoldingDetailRepository _repo;

    public HoldingDetailService(IHoldingDetailRepository repo)
    {
        _repo = repo;
    }

    public Task<List<HoldingDetail>> GetAllAsync() => _repo.GetAllAsync();

    public Task<HoldingDetail?> GetByIdAsync(Guid id) => _repo.GetByIdAsync(id);

    public async Task AddOrEditAsync(HoldingDetail model)
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