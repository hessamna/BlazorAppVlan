using BalzorAppVlan.Helper;
using Microsoft.Extensions.DependencyInjection;

namespace BalzorAppVlan.Services
{
    // ======================= CompanyService =======================
    public class CompanyService : ICompanyService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public CompanyService(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

        public async Task<List<CompanyViewModel>> GetAllAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ICompanyRepository>();
            var entities = await repo.GetAllAsync(false);
            return entities.Select(MapToViewModel).ToList();
        }

        public async Task<CompanyViewModel?> GetByIdAsync(Guid id)
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ICompanyRepository>();
            var entity = await repo.GetByIdAsync(id, false);
            return entity is null ? null : MapToViewModel(entity);
        }

        public async Task<ServiceResult> AddOrEditAsync(CompanyViewModel vm)
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ICompanyRepository>();

            var exists = await repo.ExistsAsync(c =>
                c.Name.ToLower() == vm.Name.ToLower() && c.Id != vm.Id);
            if (exists)
                return ServiceResult.Fail($"Company with name '{vm.Name}' already exists.");

            if (vm.Id == Guid.Empty)
            {
                var entity = MapToEntity(vm);
                entity.Id = Guid.NewGuid();
                await repo.AddAsync(entity);
            }
            else
            {
                var entity = MapToEntity(vm);
                repo.Update(entity);
            }

            await repo.SaveChangesAsync();
            return ServiceResult.Ok(vm.Id == Guid.Empty ? "Company created successfully." : "Company updated successfully.");
        }

        public async Task<ServiceResult> DeleteAsync(Guid id)
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ICompanyRepository>();
            var entity = await repo.GetByIdAsync(id, false);
            if (entity == null) return ServiceResult.Fail("Company not found.");
            repo.Delete(entity);
            await repo.SaveChangesAsync();
            return ServiceResult.Ok("Company deleted successfully.");
        }

        private static CompanyViewModel MapToViewModel(Company e) => new()
        {
            Id = e.Id,
            Name = e.Name,
            IsVerified = e.IsVerified
        };
        private static Company MapToEntity(CompanyViewModel vm) => new()
        {
            Id = vm.Id,
            Name = vm.Name,
            IsVerified = vm.IsVerified
        };
    }

    // ======================= SwitchService =======================
    public class SwitchService : ISwitchService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public SwitchService(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

        public async Task<List<SwitchViewModel>> GetAllAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ISwitchRepository>();
            var entities = await repo.GetAllAsync(false);
            return entities.Select(MapToViewModel).ToList();
        }

        public async Task<SwitchViewModel?> GetByIdAsync(Guid id)
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ISwitchRepository>();
            var entity = await repo.GetByIdAsync(id, false);
            return entity is null ? null : MapToViewModel(entity);
        }

        public async Task<ServiceResult> AddOrEditAsync(SwitchViewModel vm)
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ISwitchRepository>();
            var vlanRepo = scope.ServiceProvider.GetRequiredService<IVlanRepository>();

            var exists = await repo.ExistsAsync(s =>
                s.Name.ToLower() == vm.Name.ToLower() && s.Id != vm.Id);
            if (exists)
                return ServiceResult.Fail($"Switch with name '{vm.Name}' already exists.");

            if (vm.Id == Guid.Empty)
            {
                var entity = MapToEntity(vm);
                entity.Id = Guid.NewGuid();
                await repo.AddAsync(entity);

                // Default VLANs
                await vlanRepo.AddAsync(new Vlan
                {
                    Id = Guid.NewGuid(),
                    VlanCode = "1",
                    Name = "Default VLAN",
                    SwitchId = entity.Id
                });
                await vlanRepo.AddAsync(new Vlan
                {
                    Id = Guid.NewGuid(),
                    VlanCode = "Trunk",
                    Name = "Trunk VLAN",
                    SwitchId = entity.Id
                });
            }
            else
            {
                var entity = MapToEntity(vm);
                repo.Update(entity);
            }

            await repo.SaveChangesAsync();
            return ServiceResult.Ok(vm.Id == Guid.Empty ? "Switch created successfully with default VLANs." : "Switch updated successfully.");
        }

        public async Task<ServiceResult> DeleteAsync(Guid id)
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ISwitchRepository>();
            var vlanRepo = scope.ServiceProvider.GetRequiredService<IVlanRepository>();
            var deviceRepo = scope.ServiceProvider.GetRequiredService<IDeviceInterfaceRepository>();
            var neighborRepo = scope.ServiceProvider.GetRequiredService<INeighborRepository>();

            var entity = await repo.GetByIdAsync(id, false);
            if (entity == null) return ServiceResult.Fail("Switch not found.");

            var neighbors = await neighborRepo.FindAsync(n => n.SwitchId == id);
            neighborRepo.DeleteRange(neighbors);

            var devices = await deviceRepo.FindAsync(d => d.SwitchId == id);
            deviceRepo.DeleteRange(devices);

            var vlans = await vlanRepo.FindAsync(v => v.SwitchId == id);
            vlanRepo.DeleteRange(vlans);

            repo.Delete(entity);

            await neighborRepo.SaveChangesAsync();
            await deviceRepo.SaveChangesAsync();
            await vlanRepo.SaveChangesAsync();
            await repo.SaveChangesAsync();

            return ServiceResult.Ok("Switch and all related entities deleted successfully.");
        }

        private static SwitchViewModel MapToViewModel(Switch e) => new()
        {
            Id = e.Id,
            Name = e.Name,
            IpInterface = e.IpInterface,
            Model = e.Model,
            CompanyId = e.CompanyId
        };
        private static Switch MapToEntity(SwitchViewModel vm) => new()
        {
            Id = vm.Id,
            Name = vm.Name,
            IpInterface = vm.IpInterface,
            Model = vm.Model,
            CompanyId = vm.CompanyId
        };
    }

    // ======================= VlanService =======================
    public class VlanService : IVlanService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public VlanService(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

        public async Task<List<VlanViewModel>> GetAllAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IVlanRepository>();
            var entities = await repo.GetAllAsync(false);
            return entities.Select(MapToViewModel).ToList();
        }

        public async Task<VlanViewModel?> GetByIdAsync(Guid id)
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IVlanRepository>();
            var entity = await repo.GetByIdAsync(id, false);
            return entity is null ? null : MapToViewModel(entity);
        }

        public async Task<ServiceResult> AddOrEditAsync(VlanViewModel vm)
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IVlanRepository>();

            var exists = await repo.ExistsAsync(v =>
                v.SwitchId == vm.SwitchId &&
                v.VlanCode.ToLower() == vm.VlanCode.ToLower() &&
                v.Id != vm.Id);
            if (exists)
                return ServiceResult.Fail($"VLAN with code '{vm.VlanCode}' already exists in this switch.");

            if (vm.Id == Guid.Empty)
            {
                var entity = MapToEntity(vm);
                entity.Id = Guid.NewGuid();
                await repo.AddAsync(entity);
            }
            else
            {
                var entity = MapToEntity(vm);
                repo.Update(entity);
            }

            await repo.SaveChangesAsync();
            return ServiceResult.Ok(vm.Id == Guid.Empty ? "VLAN created successfully." : "VLAN updated successfully.");
        }

        public async Task<ServiceResult> DeleteAsync(Guid id)
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IVlanRepository>();
            var entity = await repo.GetByIdAsync(id, false);
            if (entity == null) return ServiceResult.Fail("VLAN not found.");
            repo.Delete(entity);
            await repo.SaveChangesAsync();
            return ServiceResult.Ok("VLAN deleted successfully.");
        }

        private static VlanViewModel MapToViewModel(Vlan e) => new()
        {
            Id = e.Id,
            VlanCode = e.VlanCode,
            Name = e.Name,
            IpInterface = e.IpInterface,
            SwitchId = e.SwitchId
        };
        private static Vlan MapToEntity(VlanViewModel vm) => new()
        {
            Id = vm.Id,
            VlanCode = vm.VlanCode,
            Name = vm.Name,
            IpInterface = vm.IpInterface,
            SwitchId = vm.SwitchId
        };
    }

    // ======================= DeviceInterfaceService =======================
    public class DeviceInterfaceService : IDeviceInterfaceService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public DeviceInterfaceService(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

        public async Task<List<DeviceInterfaceViewModel>> GetAllAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IDeviceInterfaceRepository>();
            var entities = await repo.GetAllAsync(false);
            return entities.Select(MapToViewModel).ToList();
        }

        public async Task<DeviceInterfaceViewModel?> GetByIdAsync(Guid id)
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IDeviceInterfaceRepository>();
            var entity = await repo.GetByIdAsync(id, false);
            return entity is null ? null : MapToViewModel(entity);
        }

        public async Task<ServiceResult> AddOrEditAsync(DeviceInterfaceViewModel vm)
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IDeviceInterfaceRepository>();

            if (vm.Id == Guid.Empty)
            {
                var entity = MapToEntity(vm);
                entity.Id = Guid.NewGuid();
                await repo.AddAsync(entity);
            }
            else
            {
                var entity = MapToEntity(vm);
                repo.Update(entity);
            }

            await repo.SaveChangesAsync();
            return ServiceResult.Ok(vm.Id == Guid.Empty ? "Device interface created successfully." : "Device interface updated successfully.");
        }

        public async Task<ServiceResult> DeleteAsync(Guid id)
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IDeviceInterfaceRepository>();
            var entity = await repo.GetByIdAsync(id, false);
            if (entity == null) return ServiceResult.Fail("Device interface not found.");
            repo.Delete(entity);
            await repo.SaveChangesAsync();
            return ServiceResult.Ok("Device interface deleted successfully.");
        }

        public async Task<ServiceResult> UpdateRangeAsync(List<DeviceInterfaceViewModel> vms)
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IDeviceInterfaceRepository>();
            if (vms == null || vms.Count == 0)
                return ServiceResult.Fail("No interfaces provided.");
            var entities = vms.Select(MapToEntity).ToList();
            repo.UpdateRange(entities);
            await repo.SaveChangesAsync();
            return ServiceResult.Ok("All device interfaces updated successfully.");
        }

        public async Task<ServiceResult> AddRangeAsync(Guid switchId, int count, string portPrefix = "Gi1/0/")
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IDeviceInterfaceRepository>();
            var vlanRepo = scope.ServiceProvider.GetRequiredService<IVlanRepository>();

            if (count <= 0)
                return ServiceResult.Fail("Count must be greater than zero.");

            var defaultVlan = await vlanRepo.FirstOrDefaultAsync(v =>
                v.SwitchId == switchId && v.VlanCode == "1");
            if (defaultVlan == null)
                return ServiceResult.Fail("Default VLAN (1) not found for this switch.");

            for (int i = 1; i <= count; i++)
            {
                var entity = new DeviceInterface
                {
                    Id = Guid.NewGuid(),
                    Port = $"{portPrefix}{i}",
                    Description = "",
                    IsConnected = false,
                    SwitchId = switchId,
                    VlanId = defaultVlan.Id
                };
                await repo.AddAsync(entity);
            }

            await repo.SaveChangesAsync();
            return ServiceResult.Ok($"{count} interfaces created successfully with default VLAN 1.");
        }

        private static DeviceInterfaceViewModel MapToViewModel(DeviceInterface e) => new()
        {
            Id = e.Id,
            Port = e.Port,
            Description = e.Description,
            IsConnected = e.IsConnected,
            SwitchId = e.SwitchId,
            VlanId = e.VlanId
        };
        private static DeviceInterface MapToEntity(DeviceInterfaceViewModel vm) => new()
        {
            Id = vm.Id,
            Port = vm.Port,
            Description = vm.Description,
            IsConnected = vm.IsConnected,
            SwitchId = vm.SwitchId,
            VlanId = vm.VlanId
        };
    }

    // ======================= NeighborService =======================
    public class NeighborService : INeighborService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public NeighborService(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

        public async Task<List<NeighborViewModel>> GetAllAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<INeighborRepository>();
            var entities = await repo.GetAllAsync(false);
            return entities.Select(MapToViewModel).ToList();
        }

        public async Task<NeighborViewModel?> GetByIdAsync(Guid id)
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<INeighborRepository>();
            var entity = await repo.GetByIdAsync(id, false);
            return entity is null ? null : MapToViewModel(entity);
        }

        public async Task<ServiceResult> AddOrEditAsync(NeighborViewModel vm)
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<INeighborRepository>();

            if (vm.Id == Guid.Empty)
            {
                var entity = MapToEntity(vm);
                entity.Id = Guid.NewGuid();
                await repo.AddAsync(entity);
            }
            else
            {
                var entity = MapToEntity(vm);
                repo.Update(entity);
            }

            await repo.SaveChangesAsync();
            return ServiceResult.Ok(vm.Id == Guid.Empty ? "Neighbor created successfully." : "Neighbor updated successfully.");
        }

        public async Task<ServiceResult> DeleteAsync(Guid id)
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<INeighborRepository>();
            var entity = await repo.GetByIdAsync(id, false);
            if (entity == null) return ServiceResult.Fail("Neighbor not found.");
            repo.Delete(entity);
            await repo.SaveChangesAsync();
            return ServiceResult.Ok("Neighbor deleted successfully.");
        }

        private static NeighborViewModel MapToViewModel(Neighbor e) => new()
        {
            Id = e.Id,
            DeviceId = e.DeviceId,
            LocalInterface = e.LocalInterface,
            NeighborSWName = e.NeighborSWName,
            NeighborSWNamePortId = e.NeighborSWNamePortId,
            SwitchId = e.SwitchId,
            VlanId = e.VlanId
        };
        private static Neighbor MapToEntity(NeighborViewModel vm) => new()
        {
            Id = vm.Id,
            DeviceId = vm.DeviceId,
            LocalInterface = vm.LocalInterface,
            NeighborSWName = vm.NeighborSWName,
            NeighborSWNamePortId = vm.NeighborSWNamePortId,
            SwitchId = vm.SwitchId,
            VlanId = vm.VlanId
        };
    }
}
