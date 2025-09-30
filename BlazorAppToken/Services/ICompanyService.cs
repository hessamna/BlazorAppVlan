using BalzorAppVlan.Helper;

namespace BalzorAppVlan.Services
{
    public interface ICompanyService
    {
        Task<List<CompanyViewModel>> GetAllAsync();
        Task<CompanyViewModel?> GetByIdAsync(Guid id);
        Task<ServiceResult> AddOrEditAsync(CompanyViewModel vm);
        Task<ServiceResult> DeleteAsync(Guid id);
    }

    public interface ISwitchService
    {
        Task<List<SwitchViewModel>> GetAllAsync();
        Task<SwitchViewModel?> GetByIdAsync(Guid id);
        Task<ServiceResult> AddOrEditAsync(SwitchViewModel vm);
        Task<ServiceResult> DeleteAsync(Guid id);
    }

    public interface IVlanService
    {
        Task<List<VlanViewModel>> GetAllAsync();
        Task<VlanViewModel?> GetByIdAsync(Guid id);
        Task<ServiceResult> AddOrEditAsync(VlanViewModel vm);
        Task<ServiceResult> DeleteAsync(Guid id);
    }

    public interface IDeviceInterfaceService
    {
        Task<List<DeviceInterfaceViewModel>> GetAllAsync();
        Task<DeviceInterfaceViewModel?> GetByIdAsync(Guid id);
        Task<ServiceResult> AddOrEditAsync(DeviceInterfaceViewModel vm);
        Task<ServiceResult> DeleteAsync(Guid id);
        Task<ServiceResult> UpdateRangeAsync(List<DeviceInterfaceViewModel> vms);
        Task<ServiceResult> AddRangeAsync(Guid switchId, int count, string portPrefix = "Gi1/0/");
    }

    public interface INeighborService
    {
        Task<List<NeighborViewModel>> GetAllAsync();
        Task<NeighborViewModel?> GetByIdAsync(Guid id);
        Task<ServiceResult> AddOrEditAsync(NeighborViewModel vm);
        Task<ServiceResult> DeleteAsync(Guid id);
    }
}
