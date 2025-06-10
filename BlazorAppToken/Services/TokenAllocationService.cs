using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class TokenAllocationService : IEntityService<TokenAllocation>
{
    private readonly ITokenAllocationRepository _repo;

    public TokenAllocationService(ITokenAllocationRepository repo)
    {
        _repo = repo;
    }

    public Task<List<TokenAllocation>> GetAllAsync() => _repo.GetAllAsync();

    public Task<TokenAllocation?> GetByIdAsync(Guid id) => _repo.GetByIdAsync(id);

    public async Task AddOrEditAsync(TokenAllocation model)
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