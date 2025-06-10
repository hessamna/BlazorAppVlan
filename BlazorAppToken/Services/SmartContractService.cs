using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class SmartContractService : IEntityService<SmartContract>
{
    private readonly ISmartContractRepository _repo;

    public SmartContractService(ISmartContractRepository repo)
    {
        _repo = repo;
    }

    public Task<List<SmartContract>> GetAllAsync() => _repo.GetAllAsync();

    public Task<SmartContract?> GetByIdAsync(Guid id) => _repo.GetByIdAsync(id);

    public async Task AddOrEditAsync(SmartContract model)
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