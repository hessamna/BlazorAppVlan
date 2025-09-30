using BalzorAppVlan.Repository.BaseRepository;

public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
{
    public CompanyRepository(ApplicationDbContext context) : base(context) { }
}

public class SwitchRepository : BaseRepository<Switch>, ISwitchRepository
{
    public SwitchRepository(ApplicationDbContext context) : base(context) { }
}

public class VlanRepository : BaseRepository<Vlan>, IVlanRepository
{
    public VlanRepository(ApplicationDbContext context) : base(context) { }
}

public class DeviceInterfaceRepository : BaseRepository<DeviceInterface>, IDeviceInterfaceRepository
{
    public DeviceInterfaceRepository(ApplicationDbContext context) : base(context) { }
}

public class NeighborRepository : BaseRepository<Neighbor>, INeighborRepository
{
    public NeighborRepository(ApplicationDbContext context) : base(context) { }
}
