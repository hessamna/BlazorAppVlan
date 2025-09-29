using BalzorAppVlan.Helper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// ======================= CompanyService =======================
public class CompanyService : IEntityService<Company>
{
    private readonly ICompanyRepository _repo;

    public CompanyService(ICompanyRepository repo) => _repo = repo;

    public Task<List<Company>> GetAllAsync() => _repo.GetAllAsync();
    public Task<Company?> GetByIdAsync(Guid id) => _repo.GetByIdAsync(id);

    public async Task<ServiceResult> AddOrEditAsync(Company model)
    {
        // 🚨 چک کردن یکتا بودن نام شرکت
        var exists = await _repo.ExistsAsync(c =>
            c.Name.ToLower() == model.Name.ToLower() &&
            c.Id != model.Id);

        if (exists)
            return ServiceResult.Fail($"Company with name '{model.Name}' already exists.");

        if (model.Id == Guid.Empty)
        {
            model.Id = Guid.NewGuid();
            await _repo.AddAsync(model);
            return ServiceResult.Ok("Company created successfully.");
        }
        else
        {
            await _repo.UpdateAsync(model);
            return ServiceResult.Ok("Company updated successfully.");
        }
    }

    public async Task<ServiceResult> DeleteAsync(Guid id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return ServiceResult.Fail("Company not found.");

        await _repo.DeleteAsync(entity);
        return ServiceResult.Ok("Company deleted successfully.");
    }
}


// ======================= SwitchService =======================
// ======================= SwitchService =======================
public class SwitchService : IEntityService<Switch>
{
    private readonly ISwitchRepository _repo;
    private readonly IVlanRepository _vlanRepo;
    private readonly IDeviceInterfaceRepository _deviceRepo;
    private readonly INeighborRepository _neighborRepo;

    public SwitchService(
        ISwitchRepository repo,
        IVlanRepository vlanRepo,
        IDeviceInterfaceRepository deviceRepo,
        INeighborRepository neighborRepo)
    {
        _repo = repo;
        _vlanRepo = vlanRepo;
        _deviceRepo = deviceRepo;
        _neighborRepo = neighborRepo;
    }

    public Task<List<Switch>> GetAllAsync() => _repo.GetAllAsync();
    public Task<Switch?> GetByIdAsync(Guid id) => _repo.GetByIdAsync(id);

    public async Task<ServiceResult> AddOrEditAsync(Switch model)
    {
        var exists = await _repo.ExistsAsync(s =>
            s.Name.ToLower() == model.Name.ToLower() &&
            s.Id != model.Id);

        if (exists)
            return ServiceResult.Fail($"Switch with name '{model.Name}' already exists.");

        if (model.Id == Guid.Empty)
        {
            model.Id = Guid.NewGuid();
            await _repo.AddAsync(model);

            // VLAN پیش‌فرض
            await _vlanRepo.AddAsync(new Vlan
            {
                Id = Guid.NewGuid(),
                VlanCode = "1",
                Name = "Default VLAN",
                SwitchId = model.Id
            });

            await _vlanRepo.AddAsync(new Vlan
            {
                Id = Guid.NewGuid(),
                VlanCode = "Trunk",
                Name = "Trunk VLAN",
                SwitchId = model.Id
            });

            return ServiceResult.Ok("Switch created successfully with default VLANs.");
        }
        else
        {
            await _repo.UpdateAsync(model);
            return ServiceResult.Ok("Switch updated successfully.");
        }
    }

    public async Task<ServiceResult> DeleteAsync(Guid id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return ServiceResult.Fail("Switch not found.");

        // 1. حذف Neighbors
        var neighbors = await _neighborRepo.FindAsync(n => n.SwitchId == id);
        foreach (var neighbor in neighbors)
            await _neighborRepo.DeleteAsync(neighbor);

        // 2. حذف Device Interfaces
        var devices = await _deviceRepo.FindAsync(d => d.SwitchId == id);
        foreach (var device in devices)
            await _deviceRepo.DeleteAsync(device);

        // 3. حذف VLANs
        var vlans = await _vlanRepo.FindAsync(v => v.SwitchId == id);
        foreach (var vlan in vlans)
            await _vlanRepo.DeleteAsync(vlan);

        // 4. در نهایت حذف خود Switch
        await _repo.DeleteAsync(entity);

        return ServiceResult.Ok("Switch and all related entities deleted successfully.");
    }
}


// ======================= VlanService =======================
public class VlanService : IEntityService<Vlan>
{
    private readonly IVlanRepository _repo;

    public VlanService(IVlanRepository repo) => _repo = repo;

    public Task<List<Vlan>> GetAllAsync() => _repo.GetAllAsync();
    public Task<Vlan?> GetByIdAsync(Guid id) => _repo.GetByIdAsync(id);

    public async Task<ServiceResult> AddOrEditAsync(Vlan model)
    {
        var exists = await _repo.ExistsAsync(v =>
            v.SwitchId == model.SwitchId &&
            v.VlanCode.ToLower() == model.VlanCode.ToLower() &&
            v.Id != model.Id);

        if (exists)
            return ServiceResult.Fail($"VLAN with code '{model.VlanCode}' already exists in this switch.");

        if (model.Id == Guid.Empty)
        {
            model.Id = Guid.NewGuid();
            await _repo.AddAsync(model);
            return ServiceResult.Ok("VLAN created successfully.");
        }
        else
        {
            await _repo.UpdateAsync(model);
            return ServiceResult.Ok("VLAN updated successfully.");
        }
    }

    public async Task<ServiceResult> DeleteAsync(Guid id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return ServiceResult.Fail("VLAN not found.");

        await _repo.DeleteAsync(entity);
        return ServiceResult.Ok("VLAN deleted successfully.");
    }
}

// ======================= DeviceInterfaceService =======================
public class DeviceInterfaceService : IEntityService<DeviceInterface>
{
    private readonly IDeviceInterfaceRepository _repo;
    private readonly IVlanRepository _vlanRepo;
    public DeviceInterfaceService(IDeviceInterfaceRepository repo, IVlanRepository vlanRepo)
    {
        _repo = repo;
        _vlanRepo = vlanRepo;
    }

    public Task<List<DeviceInterface>> GetAllAsync() => _repo.GetAllAsync();
    public Task<DeviceInterface?> GetByIdAsync(Guid id) => _repo.GetByIdAsync(id);

    public async Task<ServiceResult> AddOrEditAsync(DeviceInterface model)
    {
        if (model.Id == Guid.Empty)
        {
            model.Id = Guid.NewGuid();
            await _repo.AddAsync(model);
            return ServiceResult.Ok("Device interface created successfully.");
        }
        else
        {
            await _repo.UpdateAsync(model);
            return ServiceResult.Ok("Device interface updated successfully.");
        }
    }

    public async Task<ServiceResult> DeleteAsync(Guid id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return ServiceResult.Fail("Device interface not found.");

        await _repo.DeleteAsync(entity);
        return ServiceResult.Ok("Device interface deleted successfully.");
    }
    public async Task<ServiceResult> AddRangeAsync(Guid switchId, int count, string portPrefix = "Gi1/0/")
    {
        if (count <= 0)
            return ServiceResult.Fail("Count must be greater than zero.");

        // گرفتن VLAN 1 پیش‌فرض از repo
        var defaultVlan = await _vlanRepo.FirstOrDefaultAsync(
            v => v.SwitchId == switchId && v.VlanCode == "1"
        );

        if (defaultVlan == null)
            return ServiceResult.Fail("Default VLAN (1) not found for this switch.");

        for (int i = 1; i <= count; i++)
        {
            var newInterface = new DeviceInterface
            {
                Id = Guid.NewGuid(),
                Port = $"{portPrefix}{i}",
                Description = $"Auto generated port {i}",
                IsConnected = false,
                SwitchId = switchId,
                VlanId = defaultVlan.Id
            };

            await _repo.AddAsync(newInterface);
        }

        return ServiceResult.Ok($"{count} interfaces created successfully with default VLAN 1.");
    }
}

// ======================= NeighborService =======================
public class NeighborService : IEntityService<Neighbor>
{
    private readonly INeighborRepository _repo;

    public NeighborService(INeighborRepository repo) => _repo = repo;

    public Task<List<Neighbor>> GetAllAsync() => _repo.GetAllAsync();
    public Task<Neighbor?> GetByIdAsync(Guid id) => _repo.GetByIdAsync(id);

    public async Task<ServiceResult> AddOrEditAsync(Neighbor model)
    {
        if (model.Id == Guid.Empty)
        {
            model.Id = Guid.NewGuid();
            await _repo.AddAsync(model);
            return ServiceResult.Ok("Neighbor created successfully.");
        }
        else
        {
            await _repo.UpdateAsync(model);
            return ServiceResult.Ok("Neighbor updated successfully.");
        }
    }

    public async Task<ServiceResult> DeleteAsync(Guid id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity == null) return ServiceResult.Fail("Neighbor not found.");

        await _repo.DeleteAsync(entity);
        return ServiceResult.Ok("Neighbor deleted successfully.");
    }
}
