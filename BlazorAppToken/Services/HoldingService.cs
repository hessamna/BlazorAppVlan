using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class HoldingService : IEntityService<Holding>
{
    private readonly IHoldingRepository _repo;

    public HoldingService(IHoldingRepository repo)
    {
        _repo = repo;
    }

    public Task<List<Holding>> GetAllAsync() => _repo.GetAllAsync();

    public Task<Holding?> GetByIdAsync(Guid id) => _repo.GetByIdAsync(id);

    public async Task AddOrEditAsync(Holding model)
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