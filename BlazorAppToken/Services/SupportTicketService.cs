using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class SupportTicketService : IEntityService<SupportTicket>
{
    private readonly ISupportTicketRepository _repo;

    public SupportTicketService(ISupportTicketRepository repo)
    {
        _repo = repo;
    }

    public Task<List<SupportTicket>> GetAllAsync() => _repo.GetAllAsync();

    public Task<SupportTicket?> GetByIdAsync(Guid id) => _repo.GetByIdAsync(id);

    public async Task AddOrEditAsync(SupportTicket model)
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