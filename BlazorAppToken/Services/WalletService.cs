using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class WalletService : IEntityService<Wallet>
{
    private readonly IWalletRepository _repo;

    public WalletService(IWalletRepository repo)
    {
        _repo = repo;
    }

    public Task<List<Wallet>> GetAllAsync() => _repo.GetAllAsync();

    public Task<Wallet?> GetByIdAsync(Guid id) => _repo.GetByIdAsync(id);

    public async Task AddOrEditAsync(Wallet model)
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