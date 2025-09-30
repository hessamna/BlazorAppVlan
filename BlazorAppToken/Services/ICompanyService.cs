using BalzorAppVlan.Helper;

namespace BalzorAppVlan.Services
{
    public interface ICompanyService
    {
        Task<List<CompanyViewModel>> GetAllAsync();
        Task<CompanyViewModel?> GetByIdAsync(int id);
        Task<ServiceResult> AddOrEditAsync(CompanyViewModel vm);
        Task<ServiceResult> DeleteAsync(int id);
        Task<List<CompanyViewModel>> GetAllHierarchyAsync();
        Task<CompanyViewModel?> GetByIdHierarchyAsync(int id);
    }

    public interface ISwitchService
    {
        Task<List<SwitchViewModel>> GetAllAsync();
        Task<SwitchViewModel?> GetByIdAsync(int id);
        Task<ServiceResult> AddOrEditAsync(SwitchViewModel vm);
        Task<ServiceResult> DeleteAsync(int id);
    }

    public interface IVlanService
    {
        Task<List<VlanViewModel>> GetAllAsync();
        Task<VlanViewModel?> GetByIdAsync(int id);
        Task<ServiceResult> AddOrEditAsync(VlanViewModel vm);
        Task<ServiceResult> DeleteAsync(int id);
    }

    public interface IDeviceInterfaceService
    {
        Task<List<DeviceInterfaceViewModel>> GetAllAsync();
        Task<DeviceInterfaceViewModel?> GetByIdAsync(int id);
        Task<ServiceResult> AddOrEditAsync(DeviceInterfaceViewModel vm);
        Task<ServiceResult> DeleteAsync(int id);
        Task<ServiceResult> UpdateRangeAsync(List<DeviceInterfaceViewModel> vms);
        Task<ServiceResult> AddRangeAsync(int switchId, int count, string portPrefix = "Gi1/0/");
    }

    public interface INeighborService
    {
        Task<List<NeighborViewModel>> GetAllAsync();
        Task<NeighborViewModel?> GetByIdAsync(int id);
        Task<ServiceResult> AddOrEditAsync(NeighborViewModel vm);
        Task<ServiceResult> DeleteAsync(int id);
    }
}
