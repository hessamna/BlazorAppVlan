using Microsoft.EntityFrameworkCore;

public class SystemSettingRepository : BaseRepository<SystemSetting>, ISystemSettingRepository
{
    public SystemSettingRepository(ApplicationDbContext context) : base(context) { }
}