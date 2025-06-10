using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MultiSigApprovalService : IEntityService<MultiSigApproval>
{
    private readonly IMultiSigApprovalRepository _repo;

    public MultiSigApprovalService(IMultiSigApprovalRepository repo)
    {
        _repo = repo;
    }

    public Task<List<MultiSigApproval>> GetAllAsync() => _repo.GetAllAsync();

    public Task<MultiSigApproval?> GetByIdAsync(Guid id) => _repo.GetByIdAsync(id);

    public async Task AddOrEditAsync(MultiSigApproval model)
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