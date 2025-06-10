using Microsoft.EntityFrameworkCore;

public class SupportTicketRepository : BaseRepository<SupportTicket>, ISupportTicketRepository
{
    public SupportTicketRepository(ApplicationDbContext context) : base(context) { }
}