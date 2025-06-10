using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class TokenTransferService : IEntityService<TokenTransfer>
{
    private readonly ITokenTransferRepository _repo;

    public TokenTransferService(ITokenTransferRepository repo)
    {
        _repo = repo;
    }

    public Task<List<TokenTransfer>> GetAllAsync() => _repo.GetAllAsync();

    public Task<TokenTransfer?> GetByIdAsync(Guid id) => _repo.GetByIdAsync(id);

    public async Task AddOrEditAsync(TokenTransfer model)
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