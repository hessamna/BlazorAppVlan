using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class TokenService : IEntityService<Token>
{
    private readonly ITokenRepository _repo;

    public TokenService(ITokenRepository repo)
    {
        _repo = repo;
    }

    public Task<List<Token>> GetAllAsync() => _repo.GetAllAsync();

    public Task<Token?> GetByIdAsync(Guid id) => _repo.GetByIdAsync(id);

    public async Task AddOrEditAsync(Token model)
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