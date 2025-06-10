using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CompanyService : IEntityService<Company>
{
    private readonly ICompanyRepository _repo;

    public CompanyService(ICompanyRepository repo)
    {
        _repo = repo;
    }

    public Task<List<Company>> GetAllAsync() => _repo.GetAllAsync();

    public Task<Company?> GetByIdAsync(Guid id) => _repo.GetByIdAsync(id);

    public async Task AddOrEditAsync(Company model)
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