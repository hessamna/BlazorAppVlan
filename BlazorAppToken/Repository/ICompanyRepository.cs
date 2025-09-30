using BalzorAppVlan.Repository.BaseRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

// ======================= Company =======================
public interface ICompanyRepository : IBaseRepository<Company> 
{
}
public interface ISwitchRepository : IBaseRepository<Switch> { }
public interface IVlanRepository : IBaseRepository<Vlan> { }
public interface IDeviceInterfaceRepository : IBaseRepository<DeviceInterface> { }
public interface INeighborRepository : IBaseRepository<Neighbor> { }


